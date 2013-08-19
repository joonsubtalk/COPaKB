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
using System.Drawing.Imaging  ;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Globalization;


public partial class PieChart : System.Web.UI.Page
{
    const int WIDTH = 420;
    const int HEIGHT = 300;
    ArrayList PieColors = new ArrayList ();
    ArrayList PartitionValues = new ArrayList ();
    private struct PartitionValue
    {
        public String Description;
        public float Value;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Partitions"] != null)
        {
            string Partitions = Request.QueryString["Partitions"];
            string[] EachPartition = Partitions.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string Partition in EachPartition)
            {
                PartitionValue pv = new PartitionValue();
                pv.Description = Partition.Substring (0,Partition.LastIndexOf (";"));
                pv.Value = float.Parse(Partition.Substring(Partition.LastIndexOf(";") + 1));
                PartitionValues.Add(pv);
            }



            string definecolors = ConfigurationSettings.AppSettings["PieColors"];
            string[] colors = definecolors.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string color in colors)
            {
                string[] rgbs = color.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int R = int.Parse(rgbs[0]);
                int G = int.Parse(rgbs[1]);
                int B = int.Parse(rgbs[2]);
                System.Drawing.Color newcolor = Color.FromArgb (R,G,B);
                
                PieColors.Add(newcolor);
            }
            DrawPieChart();
        }
    }
    float currentposition = 0;
    int currentIndex = 0;
    private void DrawPieChart()
    {
        Bitmap image = new Bitmap(WIDTH, HEIGHT);

        Graphics g = Graphics.FromImage(image);
        g.FillRectangle(Brushes.White, 0, 0, WIDTH, HEIGHT);

        foreach (PartitionValue pv in PartitionValues)
        {
            DrawPieSlide(pv, g);
            currentIndex++;            
        }
        currentIndex = 0;
        currentposition = 0;
        foreach (PartitionValue pv in PartitionValues)
        {
            DrawPie(pv, g);
            currentIndex++;
        }

        Response.ContentType = "image/gif";
        image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg );
        g.Dispose();
        image.Dispose();
    }

    private void DrawPieSlide(PartitionValue pv,Graphics g)
    {
        float startAngle = currentposition;
        float sweepAngle = (pv.Value * 360);
        currentposition = startAngle + sweepAngle;
        int x = 5;
        int y = 5;
        int width = 350;
        int height = 260;
        SolidBrush objBrush = new SolidBrush((Color)PieColors[currentIndex % PieColors.Count]);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        if (startAngle == 0)
        {
            int iDiff = 30;
            if (sweepAngle >= 180)
                iDiff = 0;
            for (int iLoop2 = 0; iLoop2 < 15; iLoop2++)
            {
                g.FillPie(new HatchBrush(HatchStyle.Percent50, objBrush.Color), x+iDiff , y + iLoop2+20, width, height, startAngle, sweepAngle);
               

            }
        }
        else if (startAngle <= 180 || currentposition >= 359)
        {
            for (int iLoop2 = 0; iLoop2 < 15; iLoop2++)
            {
                g.FillPie(new HatchBrush(HatchStyle.Percent50, objBrush.Color), x, y + iLoop2, width, height, startAngle, sweepAngle);
                //g.DrawPie(Pens.White, x, y + iLoop2, width, height, startAngle, sweepAngle);
                //g.FillPie(objBrush, x, y, width, height, startAngle, sweepAngle);
            }
        }
        else
        {
            //g.FillPie(objBrush, x, y, width, height, startAngle, sweepAngle);
            //g.DrawPie(Pens.White, x, y, width, height, startAngle, sweepAngle);
        }

    }

    private void DrawPie(PartitionValue pv, Graphics g)
    {
        float startAngle = currentposition;
        float sweepAngle = (pv.Value * 360);
        currentposition = startAngle + sweepAngle;
        int x = 5;
        int y = 5;
        int width = 350;
        int height = 260;
        //int labelwidth = 50;
        //int labelHeight = 50;
        //StringFormat sfLabel = new StringFormat();
        //sfLabel.Alignment = StringAlignment.Center;
        //sfLabel.LineAlignment = StringAlignment.Center;
        SolidBrush objBrush = new SolidBrush((Color)PieColors[currentIndex % PieColors.Count]);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        if (startAngle == 0)
        {
            int iDiff = 30;
            if (sweepAngle >= 180)
                iDiff = 0;
            g.FillPie(objBrush, x + iDiff , y + 20, width, height, startAngle, sweepAngle);
            //if (sweepAngle > 20)
            //{
            //    float centx = (float)(180 + 200 * (Math.Cos((startAngle + sweepAngle / 2) * 3.14159 / 180)));
            //    float centy = (float)(135+ 150*(Math.Sin ((startAngle + sweepAngle / 2) * 3.14159 / 180)));
            //    g.DrawRectangle(Pens.Red, centx + iDiff - labelwidth / 2, centy + 20 - labelHeight / 2, labelwidth, labelHeight);
            //    g.DrawString(pv.Value.ToString ("P",CultureInfo.InvariantCulture), new Font("Arial", 9), Brushes.Black, new RectangleF(centx+iDiff-labelwidth /2,centy+20-labelHeight/2,labelwidth,labelHeight),sfLabel );
                
            //}
        }
        
        else
        {
            g.FillPie(objBrush, x, y, width, height, startAngle, sweepAngle);
            //if (sweepAngle > 20)
            //{
            //    float centx = (float)(180 + 200 * (Math.Cos((startAngle + sweepAngle / 2) * 3.14159 / 180)));
            //    float centy = (float)(135 + 150 * (Math.Sin((startAngle + sweepAngle / 2) * 3.14159 / 180)));
            //    g.DrawRectangle(Pens.Red, centx - labelwidth / 2, centy - labelHeight / 2, labelwidth, labelHeight);
            //    g.DrawString(pv.Value.ToString("P", CultureInfo.InvariantCulture), new Font("Arial", 9), Brushes.Black, new RectangleF(centx-labelwidth/2 , centy-labelHeight/2,labelwidth,labelHeight),sfLabel );
            //}
            //g.DrawPie(Pens.White, x, y, width, height, startAngle, sweepAngle);
        }

    }
}
