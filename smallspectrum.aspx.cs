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
using ZJU.COPLib;
using ZJU.COPDB;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Data;

public partial class smallspectrum : System.Web.UI.Page
{
    struct MZintensitiy
    {
        public float mz;
        public float intensity;
    }

    ArrayList SpectrumData;
    float MaxIntensitiy = 0.0F;
    float MinX = 20000.0F;
    float MaxX = 0.0F;
    int DisplayMinX = 0;
    int DisplayMaxX = 10000;
    int DisplayMaxY = 100;
    int HEIGHT = 120;
    int WIDTH = 160;
    int WHITEMARGIN = 5;
    int NETAREALEFTMARGIN = 5;
    int NETAREABOTTOMMARGIN = 5;
    int NETAREATOPMARGIN = 5;
    int XBIGSEG = 250;
    int YBIGSEG = 25;
    int XAxisScaleLength = 6;

    bool bZoomOut = false;
    bool bShowTheory = false;
    //string PeptideSequence = "";
    //string ChargeState = "";

    //float mzTolerance = 1F;
    //float PrecursorMZ = 0.0F;

    protected void Page_Load(object sender, EventArgs e)
    {
        SpectrumData = new ArrayList();
        if (Request.QueryString["file"] != null)
        {
            string file_path = Request.QueryString["file"];

           
            //PeptideSequence = Request.QueryString["SEQ"];
            //ChargeState = Request.QueryString["CS"];
            //PrecursorMZ = float.Parse(Request.QueryString["pmz"]);

            //mzTolerance = float.Parse(WebConfigurationManager.ConnectionStrings["mzTolerance"].ConnectionString);
            if (true)//File.Exists(file_path))
            {
                //multiuser-fridendly streamreader 
                //FileStream fs = File.Open(file_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
                //StreamReader sr = new StreamReader(fs);
                string lines = "";
                string strSQL;
                if (file_path.Length < 10)
                {
                    strSQL = string.Format("select spectrum from spectrum_tbl where spectrum_seq={0}", file_path);
                }
                else
                {
                    strSQL = string.Format("select spectrum from spectrum_tbl where spectrum_seq={0}", "159268");
                }
                DataSet result = DBInterface.QuerySQL2(strSQL);
                if (result != null)
                {

                    if (result.Tables[0].Rows.Count > 0)
                    {
                        lines = result.Tables[0].Rows[0].ItemArray[0].ToString();
                    }
                }

                String[] dtaLines = lines.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (String line in dtaLines)
                {
                    if (char.IsDigit(line, 0))
                    {
                        int sperator = line.IndexOf(" ");
                        string mz = line.Substring(0, sperator);
                        string intensity = line.Substring(sperator);
                        MZintensitiy mzi = new MZintensitiy();
                        try
                        {
                            mzi.mz = float.Parse(mz);
                            if (bZoomOut && DisplayMaxX != -1 && DisplayMinX != -1)
                            {
                                if (mzi.mz < DisplayMaxX && mzi.mz > DisplayMinX)
                                {
                                    if (mzi.mz < MinX)
                                        MinX = mzi.mz;
                                    if (mzi.mz > MaxX)
                                        MaxX = mzi.mz;
                                    mzi.intensity = float.Parse(intensity);
                                    if (mzi.intensity > MaxIntensitiy)
                                        MaxIntensitiy = mzi.intensity;
                                    SpectrumData.Add(mzi);
                                }
                            }
                            else
                            {
                                if (mzi.mz < MinX)
                                    MinX = mzi.mz;
                                if (mzi.mz > MaxX)
                                    MaxX = mzi.mz;
                                mzi.intensity = float.Parse(intensity);
                                if (mzi.intensity > MaxIntensitiy)
                                    MaxIntensitiy = mzi.intensity;
                                SpectrumData.Add(mzi);
                            }
                        }
                        catch
                        { }
                    }
                }

                DrawSpectrum();

            }
        }
    }

    private void DrawSpectrum()
    {
        Bitmap image = new Bitmap(WIDTH, HEIGHT);

        Graphics g = Graphics.FromImage(image);
        g.FillRectangle(Brushes.White, 0, 0, WIDTH, HEIGHT);
        DrawAxis(g);
        image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
        g.Dispose();
        image.Dispose();
    }

    private void DrawAxis(Graphics g)
    {
        //Font Numberfont = new Font("Arial", 12, FontStyle.Regular);
        //Rectangle YtitleRect = new Rectangle(0, NETAREABOTTOMMARGIN, NETAREALEFTMARGIN - XAxisScaleLength, HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN);
        //StringFormat format2 = new StringFormat(StringFormatFlags.DirectionVertical);
        //format2.LineAlignment = StringAlignment.Near;
        //format2.Alignment = StringAlignment.Center;
        ////g.DrawString("Relative Abundance", Numberfont, Brushes.Black, YtitleRect, format2);

        //Rectangle XtitleRect = new Rectangle(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength, WIDTH - NETAREALEFTMARGIN - WHITEMARGIN, NETAREABOTTOMMARGIN - XAxisScaleLength);
        //StringFormat format3 = new StringFormat(StringFormatFlags.NoClip);
        //format3.LineAlignment = StringAlignment.Center;
        //format3.Alignment = StringAlignment.Center;
        ////g.DrawString("m/z", Numberfont, Brushes.Black, XtitleRect, format3);
        //draw the axis main line
        //Y axis
        //g.DrawLine(Pens.Gray , new Point(NETAREALEFTMARGIN, NETAREATOPMARGIN), new Point(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN));
        //X axis
        g.DrawLine(Pens.Gray , new Point(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN), new Point(WIDTH - WHITEMARGIN, HEIGHT - NETAREABOTTOMMARGIN));

        int StartX = (int)(MinX / XBIGSEG) * XBIGSEG;
        int EndX = (int)(MaxX / XBIGSEG + 1) * XBIGSEG;
        float XUnit = (float)(WIDTH - WHITEMARGIN - NETAREALEFTMARGIN) / (EndX - StartX);
        //float XStepValue = XUnit * XBIGSEG;

        //int stepNum = (EndX - StartX) / XBIGSEG;
        //for (int i = 0; i <= stepNum; i++)
        //{
        //    float x1 = NETAREALEFTMARGIN + i * XStepValue;
        //    g.DrawLine(Pens.Black, x1, HEIGHT - NETAREABOTTOMMARGIN, x1, HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength);

        //    //Rectangle stringrect = new Rectangle((int)x1 - 32, HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength, 64, 16);

        //    //StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
        //    //format1.LineAlignment = StringAlignment.Center;
        //    //format1.Alignment = StringAlignment.Center;
        //   // g.DrawString(string.Format("{0:F0}", StartX + i * XBIGSEG), Numberfont, Brushes.Black, stringrect, format1);
        //}
        int TotalY = 100;
        

        float YUnit = (HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN) / (float)TotalY;
        //float YStepValue = YUnit * YBIGSEG;
        //int yStepNum = TotalY / YBIGSEG;


        //for (int i = 0; i <= yStepNum; i++)
        //{
        //    float y1 = HEIGHT - NETAREABOTTOMMARGIN - i * YStepValue;
        //    g.DrawLine(Pens.Black, NETAREALEFTMARGIN - XAxisScaleLength, y1, NETAREALEFTMARGIN, y1);

        //    //Rectangle stringrect = new Rectangle(WHITEMARGIN, (int)y1, NETAREALEFTMARGIN - XAxisScaleLength - WHITEMARGIN, 16);

        //    //StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
        //    //format1.LineAlignment = StringAlignment.Far;
        //    //format1.Alignment = StringAlignment.Center;
        //    //g.DrawString(string.Format("{0:F0}", i * YBIGSEG), Numberfont, Brushes.Black, stringrect, format1);
        //}
        DrawData(g, XUnit, YUnit, StartX);
    

    }
    //private string ValidePepSeq(string Src_seq)
    //{
    //    string newSeq = "";
    //    Regex reg = new Regex("[ACDEFGHIKLMNPQRSTVWY]");
    //    MatchCollection matchMade = reg.Matches(Src_seq);
    //    for (int i = 0; i < matchMade.Count; i++)
    //    {
    //        newSeq += matchMade[i].Value;
    //    }

    //    return newSeq;
    //}

    //bool HadModifed(int site, string mModifiedSequence, ref float ModifiedWeight)
    //{
    //    Regex reg = new Regex("[ACDEFGHIKLMNPQRSTVWY]");
    //    int iCurrent = 0;
    //    int iSite = 1;
    //    if (site == 0)
    //    {
    //        if (mModifiedSequence.StartsWith("("))
    //        {
    //            string mod = mModifiedSequence.Substring(1, mModifiedSequence.IndexOf(")") - 1);
    //            ModifiedWeight = float.Parse(mod);
    //            return true;
    //        }
    //        return false;
    //    }
    //    while (true)
    //    {
    //        if (reg.IsMatch(mModifiedSequence.Substring(iCurrent, 1)))
    //        {

    //            if (iSite == site)
    //            {
    //                if (reg.IsMatch(mModifiedSequence.Substring(iCurrent + 1, 1)))
    //                {
    //                    return false;
    //                }
    //                else
    //                {
    //                    string mod = mModifiedSequence.Substring(iCurrent + 2, mModifiedSequence.IndexOf(")", iCurrent) - iCurrent - 2);
    //                    ModifiedWeight = float.Parse(mod);
    //                    return true;
    //                }
    //            }
    //            iSite++;
    //        }

    //        iCurrent++;
    //        if (iCurrent >= mModifiedSequence.Length - 1)
    //            break;

    //    }
    //    return false;

    //}

   
    private void DrawData(Graphics g, float Xunit, float Yunit, int xstart)
    {

        //PeptideMW PMW = new PeptideMW(PeptideSequence);
        //float[] Bs = PMW.GetPepFragmentBValues();
        //float[] Ys = PMW.GetPepFragmentYValues();

        int Count = SpectrumData.Count;
        int i = 0;
        for (i = 0; i < Count; i++)
        {

            float mz = ((MZintensitiy)SpectrumData[i]).mz;
            float intensity = ((MZintensitiy)SpectrumData[i]).intensity;

            float x = (mz - xstart) * Xunit + NETAREALEFTMARGIN;
            float y;

            //if (bZoomOut)
            //{
            //    if (intensity * 100 / MaxIntensitiy > DisplayMaxY)
            //    {
            //        y = NETAREATOPMARGIN;
            //    }
            //    else
            //    {
            //        y = HEIGHT - NETAREABOTTOMMARGIN - intensity * 100 * Yunit / MaxIntensitiy;
            //    }

            //}
            //else
                y = HEIGHT - NETAREABOTTOMMARGIN - intensity * 100 * Yunit / MaxIntensitiy;

            Pen dataPen = new Pen(Brushes.Black, 1);
            //Pen BLinePen = new Pen(Brushes.Blue, 2);
            //Pen YLinePen = new Pen(Brushes.Red, 2);
            //Pen ALinePen = new Pen(Brushes.Green, 2);
            //Pen MLinePen = new Pen(Brushes.Gray, 2);
            //Font Numberfont = new Font("Arial", 9, FontStyle.Regular);
            ////if (y < HEIGHT - NETAREABOTTOMMARGIN - 20)
            //{
            //    //string strAnn = GetAnnotation(SpectrumData, i, Bs, Ys);
            //    //if (strAnn.StartsWith("(b"))
            //    //{
            //    //    g.DrawLine(BLinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
            //    //    g.DrawString(strAnn, Numberfont, Brushes.Blue, new PointF(x, y));
            //    //}
            //    //else if (strAnn.StartsWith("(y"))
            //    //{
            //    //    g.DrawLine(YLinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
            //    //    g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y));
            //    //}
            //    //else if (strAnn.StartsWith("(a"))
            //    //{
            //    //    g.DrawLine(ALinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
            //    //    g.DrawString(strAnn, Numberfont, Brushes.Green, new PointF(x, y));
            //    //}
            //    //else if (strAnn.StartsWith("(M") && y < HEIGHT - NETAREABOTTOMMARGIN - 100)
            //    //{
            //    //    g.DrawLine(MLinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
            //    //    g.DrawString(strAnn, Numberfont, Brushes.Gray, new PointF(x, y));
            //    //}
            //    //else
            //    //{
            //        g.DrawLine(dataPen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
            //    //}
            //    //g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y));
            //}
            //else
                g.DrawLine(dataPen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);

            //if (intensity == MaxIntensitiy)
            //{
            //    //peak value point
            //    GraphicsPath p = new GraphicsPath();
            //    p.AddLine(x, (float)HEIGHT - NETAREABOTTOMMARGIN, x + XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength);
            //    p.AddLine(x + XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength, x - XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength);
            //    p.CloseFigure();
            //    g.FillPath(Brushes.Red, p);

            //    g.DrawString(mz.ToString(), Numberfont, Brushes.Red, x + XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN);
            //}

        }
    }
}
