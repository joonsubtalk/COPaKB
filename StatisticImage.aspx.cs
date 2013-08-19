using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.IO;

public partial class StatisticImage : System.Web.UI.Page
{

    float miu1 = 0.0f;
    float sigma1 = 0.0f;
    float miu2 = 0.0f;
    float sigma2 = 0.0f;
    float threshold = 0.0f;
    float falseNumber = 0.0f;
    float trueNumber = 0.0f;
    int[] FreqCount;
    int WIDTH = 600;
    int HEIGHT = 400;
    int WHITEMARGIN = 30;
    int NETAREALEFTMARGIN = 60;
    int NETAREABOTTOMMARGIN = 50;
    int NETAREATOPMARGIN = 10;
    int XAxisScaleLength = 6;
    int XBIGSEG = 250;
    int YBIGSEG = 25;
    int MaxFreq = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["file"] != null)
        {
            string file_path = Request.QueryString["file"];
            if (File.Exists(file_path))
            {
                //multiuser-fridendly streamreader 
                FileStream fs = File.Open(file_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                string line;
                FreqCount = new int[100];
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("H"))
                    {
                        string[] tokens = line.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        miu1 = float.Parse(tokens[1]);
                        sigma1 = float.Parse(tokens[2]);
                        miu2 = float.Parse(tokens[3]);
                        sigma2 = float.Parse(tokens[4]);
                        try
                        {
                            threshold = float.Parse(tokens[5]);
                        }
                        catch (Exception ex)
                        {
                            threshold = 0;
                        }
                        falseNumber = float.Parse(tokens[6]);
                        trueNumber = float.Parse(tokens[7]);
                    }
                    else
                    {
                        FreqCount[i] = int.Parse(line);
                        if (FreqCount[i] > MaxFreq)
                            MaxFreq = FreqCount[i];
                        i++;
                    }

                }
                YBIGSEG = 100;
                if (MaxFreq > 25000)
                {
                    YBIGSEG = 5000;
                }
                else if (MaxFreq > 10000)
                {
                    YBIGSEG = 2500;
                }
                else if (MaxFreq > 5000)
                {
                    YBIGSEG = 1000;
                }
                else if (MaxFreq > 2000)
                {
                    YBIGSEG = 500;
                }
                else if (MaxFreq > 1000)
                {
                    YBIGSEG = 200;
                }
                else if (MaxFreq > 500)
                {
                    YBIGSEG = 100;
                }
                else if (MaxFreq > 100)
                {
                    YBIGSEG = 25;
                }
                else if (MaxFreq > 50)
                {
                    YBIGSEG = 10;
                }
                else if (MaxFreq > 10)
                {
                    YBIGSEG = 5;
                }
                DrawSpectrum();
            }
            else
            { 
                
            }
          
        }
    }

    private void DrawSpectrum()
    {
        Bitmap image = new Bitmap(WIDTH, HEIGHT);

        Graphics g = Graphics.FromImage(image);
        g.FillRectangle(Brushes.White, 0, 0, WIDTH, HEIGHT);
        DrawAxis(g);
        Response.ContentType = "image/gif";
        image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif  );
        g.Dispose();
        image.Dispose();
    }

    private void DrawAxis(Graphics g)
    {
        Font wordFont = new Font("Arial", 12, FontStyle.Bold);

        Font Numberfont = new Font("Arial", 10, FontStyle.Regular);
        Rectangle YtitleRect = new Rectangle(0, NETAREATOPMARGIN, NETAREALEFTMARGIN , HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN);
        StringFormat format2 = new StringFormat(StringFormatFlags.DirectionVertical);
        format2.LineAlignment = StringAlignment.Near;
        format2.Alignment = StringAlignment.Center;
        g.DrawString("Number of Spectrum", wordFont , Brushes.Black, YtitleRect, format2);

        Rectangle XtitleRect = new Rectangle(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength, WIDTH - NETAREALEFTMARGIN - WHITEMARGIN, NETAREABOTTOMMARGIN - XAxisScaleLength);
        StringFormat format3 = new StringFormat(StringFormatFlags.NoClip);
        format3.LineAlignment = StringAlignment.Center;
        format3.Alignment = StringAlignment.Center;
        g.DrawString("Final Score", wordFont , Brushes.Black, XtitleRect, format3);
        //draw the axis main line
        //Y axis
        g.DrawLine(Pens.Black, new Point(NETAREALEFTMARGIN, NETAREATOPMARGIN), new Point(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN));
        //X axis
        g.DrawLine(Pens.Black, new Point(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN), new Point(WIDTH - WHITEMARGIN, HEIGHT - NETAREABOTTOMMARGIN));
                
        float XUnit = (float)(WIDTH - WHITEMARGIN - NETAREALEFTMARGIN) / 100;
        float XStepValue = XUnit * 10;

        int stepNum = 10;
        for (int i = 0; i <= stepNum; i++)
        {
            float x1 = NETAREALEFTMARGIN + i * XStepValue;
            g.DrawLine(Pens.Black, x1, HEIGHT - NETAREABOTTOMMARGIN, x1, HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength);

            Rectangle stringrect = new Rectangle((int)x1 - 32, HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength, 64, 16);

            StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
            format1.LineAlignment = StringAlignment.Center;
            format1.Alignment = StringAlignment.Center;
            g.DrawString(string.Format("{0:F1}", i * 0.1), Numberfont, Brushes.Black, stringrect, format1);
        }
        int TotalY = (MaxFreq /YBIGSEG +1)*YBIGSEG ;
        

        float YUnit = (HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN) / (float)TotalY  ;
        float YStepValue = YUnit * YBIGSEG;
        int yStepNum = TotalY / YBIGSEG;


        for (int i = 0; i <= yStepNum; i++)
        {
            float y1 = HEIGHT - NETAREABOTTOMMARGIN - i * YStepValue;
            g.DrawLine(Pens.Black, NETAREALEFTMARGIN - XAxisScaleLength, y1, NETAREALEFTMARGIN, y1);

            Rectangle stringrect = new Rectangle(0, (int)y1, NETAREALEFTMARGIN - XAxisScaleLength, 16);

            StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
            format1.LineAlignment = StringAlignment.Far;
            format1.Alignment = StringAlignment.Far ;
            g.DrawString(string.Format("{0}", i * YBIGSEG), Numberfont, Brushes.Black, stringrect, format1);
        }
        DrawData(g, XUnit, YUnit);
       
    }


    private void DrawData(Graphics g, float Xunit, float Yunit)
    {
        int i = 0;
        for (i = 0; i < 100; i++)
        {

            int Freq = FreqCount[i];
            float y = HEIGHT - NETAREABOTTOMMARGIN - Freq * Yunit;
            float x = i*Xunit + NETAREALEFTMARGIN;
            Pen dataPen = new Pen(Brushes.LightSkyBlue, 3);
            g.DrawLine(dataPen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
        }
        Pen ThresholdPen = new Pen(Brushes.Green , 2);
        if (threshold > 0)
        {
            
            //ThresholdPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            float thresholdx = threshold * Xunit + NETAREALEFTMARGIN;
            g.DrawLine(ThresholdPen, thresholdx, NETAREATOPMARGIN, thresholdx, (float)HEIGHT - NETAREABOTTOMMARGIN);
        }
        PointF[] FalsePoint = new PointF[100];
        PointF[] TruePoint = new PointF[100];
        for (i = 0; i < 100; i++)
        { 
            float x = i*Xunit + NETAREALEFTMARGIN;
            float Freq =  (float)(falseNumber  * Math.Exp((-1 * (i  - miu1) * (i - miu1)) / (2 * sigma1*sigma1)) / (Math.Sqrt(2 * Math.PI) * sigma1));
            float Freq2 = (float)(trueNumber * Math.Exp((-1 * (i - miu2) * (i - miu2)) / (2 * sigma2 * sigma2)) / (Math.Sqrt(2 * Math.PI) * sigma2));
            float y = HEIGHT - NETAREABOTTOMMARGIN - Freq * Yunit;
            float y2 = HEIGHT - NETAREABOTTOMMARGIN - Freq2 * Yunit;
            PointF fpf = new PointF(x, y);
            PointF tpf = new PointF(x, y2);
            FalsePoint[i] = fpf;
            TruePoint[i] = tpf;
        }

        Pen falsePen = new Pen (Brushes.Red,2);
        falsePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        Pen truePen = new Pen (Brushes.Blue ,2);
        truePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        Font legendFont = new Font("Arial",10,FontStyle.Regular );
        g.DrawLine(falsePen, new Point(WIDTH - 150, NETAREATOPMARGIN+8), new Point(WIDTH - 110, NETAREATOPMARGIN+8));
        g.DrawString("False Match", legendFont, Brushes.Red, new PointF(WIDTH - 100, NETAREATOPMARGIN));
        g.DrawLines(falsePen, FalsePoint);
        g.DrawLine(truePen, new Point(WIDTH - 150, NETAREATOPMARGIN+24), new Point(WIDTH - 110, NETAREATOPMARGIN + 24));
        g.DrawString("True Match", legendFont, Brushes.Blue , new PointF(WIDTH - 100, NETAREATOPMARGIN+16));
        g.DrawLines(truePen, TruePoint);
        g.DrawLine(ThresholdPen, new Point(WIDTH - 150, NETAREATOPMARGIN + 40), new Point(WIDTH - 110, NETAREATOPMARGIN+40));
        g.DrawString("Threshold", legendFont, Brushes.Green, new PointF(WIDTH - 100, NETAREATOPMARGIN + 32));
    }
}
