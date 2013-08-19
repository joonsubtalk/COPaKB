using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Drawing;

using System.Collections;
using ZJU.COPLib;

public partial class ProSeqImage : System.Web.UI.Page
{
   
    private const int WIDTH = 600;
    private const int NETWIDTH = 580;
    private const int MARGIN = 5;
    private const int AXISY = 20;
    private  int BIGSEG = 100;
    private  int SMALLSEG = 10;
    private const int PEPROWHEIGHT = 12;
    private int Height = 80;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["PID"] == null)
        {
            // don't display anything
        }
        else
        {
            string ProteinID = Request.QueryString["PID"];
            int Totals = 500;
            string strTotals = Request.QueryString ["L"];
            string PeptideNumb = Request.QueryString["NO"];
            int iPeptideNumb = 0;
            try
            {
                if (strTotals != "")
                    Totals = int.Parse(strTotals);
                if (PeptideNumb != "")
                    iPeptideNumb = int.Parse(PeptideNumb);

            }
            catch
            { }
            

            if (Totals>= 2000 && Totals< 5000)
            {
                BIGSEG = 500;
                SMALLSEG = 50;
            }
            else if (Totals > 5000 && Totals <20000)
            {
                BIGSEG = 1000;
                SMALLSEG = 100;
            }
            else if (Totals >= 20000)
            {
                BIGSEG = 5000;
                SMALLSEG = 500;
            }
            else
            {
                BIGSEG = 100;
                SMALLSEG = 10;
            }

           
            //try
            //{
            //    if (PeptideNumb != null)
            //        Height += int.Parse(PeptideNumb) * PEPROWHEIGHT ;
            //}
            //catch (Exception ex)
            //{ 
            
            //}
            Font font = new Font("courier",10,FontStyle.Regular );

            PeptidesImageInfo PIF = new PeptidesImageInfo(ProteinID, Totals, iPeptideNumb);
            int totalHeight = 0;
            ArrayList RectArray = PIF.ComputeTheValues(ref totalHeight);
            Height = totalHeight;
            Bitmap image = new Bitmap(WIDTH , Height );

            Graphics g = Graphics.FromImage(image);
            g.FillRectangle(Brushes.White, 0, 0, WIDTH, Height);
            float unit;
            DrawAxis(g,Totals, BIGSEG, SMALLSEG,out unit);
            DrawPeptide(g,RectArray);
            image.Save(Response.OutputStream , System.Drawing.Imaging.ImageFormat.Gif );
            g.Dispose();
            image.Dispose();
        }
    }

    private void DrawAxis(Graphics g, int TotalLength, int BigSeg, int SmallSeg,out float unit)
    {
        Font Numberfont = new Font("Arial",8,FontStyle.Regular );
        g.DrawString("Sequence Position", Numberfont, Brushes.Black, MARGIN, MARGIN);
        //draw the axis main line
        g.DrawLine(Pens.Black, new Point(MARGIN, AXISY), new Point(MARGIN + NETWIDTH, AXISY));
        g.DrawLine(Pens.Black, new Point(MARGIN, Height - AXISY), new Point(MARGIN + NETWIDTH, Height - AXISY));
        int smallSegLength = 3;
        int bigSegLength = 5;
        int EndLength = ((TotalLength / SmallSeg) + 1) * SmallSeg;

        int smallStepNumber = EndLength /SmallSeg ;
      
        int bigStepNumber = EndLength / BigSeg ;

        float smallstep = NETWIDTH /(float)smallStepNumber ;
        unit = smallstep /SmallSeg ;
        for(int i=0;i<=smallStepNumber ;i++)
        {
            float x1 = MARGIN + i*smallstep ;
            g.DrawLine (Pens.Black ,x1,AXISY -smallSegLength ,x1,AXISY +smallSegLength );
            Pen dashLine = new Pen (Brushes.LightGray  );
            dashLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash ;
            g.DrawLine(dashLine, x1, AXISY + smallSegLength, x1, Height-AXISY );

        }

        float bigstep = smallstep *(BigSeg/SmallSeg);
        for (int i = 0; i <= bigStepNumber; i++)
        {
            float x1 = MARGIN + i * bigstep;
            g.DrawLine(Pens.Black, x1, AXISY - bigSegLength, x1, AXISY + bigSegLength );

            Pen dashLine = new Pen(Brushes.Gray );
            dashLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawLine(dashLine, x1, AXISY + bigSegLength, x1, Height-AXISY );

            
            Rectangle stringrect = new Rectangle((int)x1 - 32, AXISY + bigSegLength, 64, 16);

           StringFormat format1 = new StringFormat (StringFormatFlags.NoClip );
            format1.LineAlignment = StringAlignment.Center ;
            format1.Alignment = StringAlignment.Center ;
            g.DrawString(string.Format("{0:F0}", i * BIGSEG), Numberfont, Brushes.Black,stringrect ,format1);
        }
        

    }
    private void DrawPeptide(Graphics g, ArrayList RectArray)
    {
        Font Numberfont = new Font("Arial",8,FontStyle.Regular );
        g.DrawString("Observed Peptides", Numberfont, Brushes.Black, new Point(MARGIN, AXISY + 15));
        //string strSQL = string.Format("select a.pp_relationship_seq,a.location,b.peptide_cop_id,b.sequence_length from pp_relation_tbl a, peptide_tbl b where b.peptide_cop_id= a.peptide_cop_id and a.location <> -1 and a.protein_cop_id='{0}'  order by a.location", ProteinID);
        //DBInterface.ConnectDB();
        //System.Data.Common.DbDataReader result = DBInterface.QuerySQL(strSQL);
        //int i = 0;
        //if (result != null)
        //{
        int j = 0;
        for (j = 0; j < RectArray.Count; j++)
        {
            PeptidesImageInfo.PepRectInfo rect = (PeptidesImageInfo.PepRectInfo)RectArray[j];

            //int location = result.GetInt32(1);
            //string pepid = result.GetString (2);
            //int length = result.GetInt32 (3);
            
            RectangleF  pepRect = new RectangleF (rect.left,rect.top ,rect.right - rect.left  ,rect.bottom - rect.top  );
            g.DrawRectangle (new Pen (Brushes.Brown ,2),pepRect.X,pepRect.Y,pepRect.Width ,pepRect.Height  );
            //if (rect.bUnique)
            //    g.FillRectangle(Brushes.Green, pepRect);
            //else
                g.FillRectangle(Brushes.Yellow, pepRect);
           
            //g.DrawString(pepid, Numberfont, Brushes.Red, pepRect.Right+1 ,pepRect.Top-4 );
            //i += 1;
        }

       // }
    }

    
}
