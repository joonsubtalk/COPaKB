using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using ZJU.COPLib;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using ZJU.COPDB;
using System.Data;

public partial class BiSpectrum : System.Web.UI.Page
{
    struct MZintensitiy
    {
        public float mz;
        public float intensity;
    }

    Color PreforColor = System.Drawing.Color.DarkGray ;
    Color NoiseModeColor = System.Drawing.Color.White;

    ArrayList SpectrumData, GuestSpectrumData;
    float MaxIntensitiy = 0.0F;
    float GuestMaxIntensitiy = 0.0F;
    float MinX = 20000.0F;
    float MaxX = 0.0F;
    float GuestMinX = 20000.0F;
    float GuestMaxX = 0.0F;
    int DisplayMinX = 0;
    int DisplayMaxX = 10000;
    int DisplayMaxY = 100;
    int HEIGHT = 510;
    int WIDTH = 720;
    int WHITEMARGIN = 14;
    int NETAREALEFTMARGIN = 50;
    int NETAREABOTTOMMARGIN = 32;
    int NETAREATOPMARGIN = 24;
    int XBIGSEG = 100;
    int YBIGSEG = 25;
    int XAxisScaleLength = 6;

    bool bZoomOut = false;
    bool bShowTheory = false;
    bool bShowNoise = false;
    bool bShowLabel = true;
    string PeptideSequence = "";
    string AdditionalSequence = "";
    string ChargeState = "";
    string SearchType = "DTA";
    float PrecursorMZ = 0.0F;
    float mzTolerance = 2.0F;
    string GuestSpectrums = "";
    string SessionID;
    string TaskID;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SpectrumData = new ArrayList();
        GuestSpectrumData = new ArrayList();
        SessionID = Request.QueryString["SessionID"];
        TaskID = Request.QueryString["TaskID"];
        if (Request.QueryString["file"] != null)
        {
            string file_path = Request.QueryString["file"];

            if (Request.QueryString["mode"] != null)
            {
                if (Request.QueryString["mode"] == "ZoomOut")
                {
                    //get the param how to show the spectrum
                    string displayMin = Request.QueryString["xmin"];
                    string displayMax = Request.QueryString["xmax"];
                    string displayYMax = Request.QueryString["ymax"];
                    try
                    {
                        if (displayMin == null)
                            DisplayMinX = -1;
                        else
                        {
                            DisplayMinX = int.Parse(displayMin);
                            if (DisplayMinX < 0)
                                DisplayMinX = -1;

                        }

                        if (displayMax == null)
                            DisplayMaxX = -1;
                        else
                        {
                            DisplayMaxX = int.Parse(displayMax);
                            if (DisplayMaxX < 0 || DisplayMaxX <= DisplayMinX)
                                DisplayMaxX = -1;
                        }
                    }
                    catch
                    {
                        DisplayMaxX = -1;
                        DisplayMinX = -1;
                    }
                    //use the params to decide the seg distance will show on the axial
                    if (DisplayMaxX != -1 && DisplayMinX != -1)
                    {
                        int Length = DisplayMaxX - DisplayMinX;

                        if (Length > 1000)
                            XBIGSEG = 100;
                        else if (Length > 500)
                            XBIGSEG = 50;
                        else if (Length > 200)
                            XBIGSEG = 25;
                        else if (Length > 100)
                            XBIGSEG = 15;
                        else if (Length > 50)
                            XBIGSEG = 10;
                        else if (Length > 20)
                            XBIGSEG = 5;
                        else if (Length > 10)
                            XBIGSEG = 2;
                        else 
                            XBIGSEG = 1;
                    }
                    try
                    {
                        if (displayYMax == null)
                            DisplayMaxY = 100;
                        else
                        {
                            DisplayMaxY = int.Parse(displayYMax);
                            if (DisplayMaxY < 75 && DisplayMaxY > 30)
                            {
                                YBIGSEG = 10;
                            }
                            else if (DisplayMaxY <= 30 && DisplayMaxY > 20)
                            {
                                YBIGSEG = 5;
                            }
                            else if (DisplayMaxY >= 0 && DisplayMaxY <= 20)
                            {
                                YBIGSEG = 2;
                            }
                            else if (DisplayMaxY <= 100 && DisplayMaxY >= 75)
                            {
                                YBIGSEG = 25;
                            }
                            else
                            {
                                DisplayMaxY = 100;
                                YBIGSEG = 25;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayMaxY = 100; ;
                    }
                    bZoomOut = true;
                }
                else
                {
                    bZoomOut = false;
                }
            }
            if (Request.QueryString["Printable"] != null)
            {
                if (Request.QueryString["Printable"] == "True")
                    WIDTH = 580;
                else
                    WIDTH = 800;
            }

            if (Request.QueryString["ShowTheory"] != null)
            {
                if (Request.QueryString["ShowTheory"] == "True")
                {
                    bShowTheory = true;

                }
                else
                    bShowTheory = false;

            }

            if (Request.QueryString["Noise"] != null)
            {
                if (Request.QueryString["Noise"] == "True")
                    bShowNoise = true;
                else
                    bShowNoise = false;
            }

            if (Request.QueryString["Label"] != null)
            {
                if (Request.QueryString["Label"] == "True")
                    bShowLabel = true;
                else
                    bShowLabel = false;
            }
            AdditionalSequence = Request.QueryString["SEQ"];
            ChargeState = Request.QueryString["CS"];
            SearchType = Request.QueryString["ST"];
            PrecursorMZ = float.Parse(Request.QueryString["pmz"]);
            mzTolerance = float.Parse(WebConfigurationManager.ConnectionStrings["mzTolerance"].ConnectionString);
            // get the library spectrum data
            if (true)//File.Exists(file_path))
            {
                //multiuser-fridendly streamreader 
                //FileInfo fiLibSpectra = new FileInfo(file_path);
                //if (!fiLibSpectra.Exists)
                //    return;

                //FileStream fs = File.Open(file_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //StreamReader sr = new StreamReader(fs);
                
                string lines = "";
                String strSQL = string.Format("select spectrum,ptm_sequence from spectrum_tbl where spectrum_seq={0}", file_path);
                DataSet result = DBInterface.QuerySQL2(strSQL);
                if (result != null)
                {

                    if (result.Tables[0].Rows.Count > 0)
                    {
                        lines = result.Tables[0].Rows[0].ItemArray[0].ToString();
                        PeptideSequence = result.Tables[0].Rows[0].ItemArray[1].ToString();
                    }
                }

                String[] dtaLines = lines.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (bZoomOut && DisplayMaxX != -1 && DisplayMinX != -1)
                {
                    MinX = DisplayMinX;
                    MaxX = DisplayMaxX;
                }
                foreach (String line in dtaLines)
                {
                    if (char.IsDigit(line, 0))
                    {
                        string[] tokens = line.Split (new string[]{" ","\t"},StringSplitOptions.RemoveEmptyEntries );
                        string mz = tokens[0];
                        string intensity = tokens [1];
                        MZintensitiy mzi = new MZintensitiy();
                        try
                        {
                            mzi.mz = float.Parse(mz);
                            if (bZoomOut && DisplayMaxX != -1 && DisplayMinX != -1)
                            {
                                if (mzi.mz < DisplayMaxX && mzi.mz > DisplayMinX)
                                {
                                    //if (mzi.mz < MinX)
                                    //    MinX = mzi.mz;
                                    //if (mzi.mz > MaxX)
                                    //    MaxX = mzi.mz;
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
                //sr.Close();

                if (SearchType == "DTA")
                {
                    ////get the guest spectrum data
                    //if (GuestSpectrums == "")
                    //    GuestSpectrums = (string)(Session[SessionID]);

                    //string[] lines = GuestSpectrums.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    //foreach (string pair in lines)
                    //{
                    //    if (char.IsDigit(pair, 0))
                    //    {
                    //        int sperator = pair.IndexOf(" ");
                    //        string mz = pair.Substring(0, sperator);
                    //        string intensity = pair.Substring(sperator);
                    //        MZintensitiy mzi = new MZintensitiy();
                    //        try
                    //        {
                    //            mzi.mz = float.Parse(mz);
                    //            if (bZoomOut && DisplayMaxX != -1 && DisplayMinX != -1)
                    //            {
                    //                // if mz in the zoom view
                    //                if (mzi.mz < DisplayMaxX && mzi.mz > DisplayMinX)
                    //                {
                    //                    if (mzi.mz < GuestMinX)
                    //                        GuestMinX = mzi.mz;
                    //                    if (mzi.mz > GuestMaxX)
                    //                        GuestMaxX = mzi.mz;
                    //                    mzi.intensity = float.Parse(intensity);
                    //                    if (mzi.intensity > GuestMaxIntensitiy)
                    //                        GuestMaxIntensitiy = mzi.intensity;
                    //                    GuestSpectrumData.Add(mzi);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (mzi.mz < GuestMinX)
                    //                    GuestMinX = mzi.mz;
                    //                if (mzi.mz > GuestMaxX)
                    //                    GuestMaxX = mzi.mz;
                    //                mzi.intensity = float.Parse(intensity);
                    //                if (mzi.intensity > GuestMaxIntensitiy)
                    //                    GuestMaxIntensitiy = mzi.intensity;
                    //                GuestSpectrumData.Add(mzi);
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        { }
                    //    }
                    //}
                }
                else
                {
                    string msFile = Request.QueryString["MSFile"];
                    string dtaFilePath = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + string.Format ("\\{0}_dta\\",msFile ) + SessionID + ".dta";

                    FileInfo fiDta = new FileInfo(dtaFilePath);
                    if (!fiDta.Exists)
                        return;

                    // get from the generated dta file
                    FileStream gfs = File.Open(dtaFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
                    StreamReader gsr = new StreamReader(gfs);
                    if (bZoomOut && DisplayMaxX != -1 && DisplayMinX != -1)
                    {
                        GuestMinX = DisplayMinX;
                        GuestMaxX = DisplayMaxX;
                    }
                    string line;
                    while ((line = gsr.ReadLine()) != null)
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
                                        //if (mzi.mz < GuestMinX)
                                        //    GuestMinX = mzi.mz;
                                        //if (mzi.mz > GuestMaxX)
                                        //    GuestMaxX = mzi.mz;
                                        mzi.intensity = float.Parse(intensity);
                                        if (mzi.intensity > GuestMaxIntensitiy)
                                            GuestMaxIntensitiy = mzi.intensity;
                                        GuestSpectrumData.Add(mzi);
                                    }
                                }
                                else
                                {
                                    if (mzi.mz < GuestMinX)
                                        GuestMinX = mzi.mz;
                                    if (mzi.mz > GuestMaxX)
                                        GuestMaxX = mzi.mz;
                                    mzi.intensity = float.Parse(intensity);
                                    if (mzi.intensity > GuestMaxIntensitiy)
                                        GuestMaxIntensitiy = mzi.intensity;
                                    GuestSpectrumData.Add(mzi);
                                }
                            }
                            catch
                            { }
                        }
                    }
                    gsr.Close();
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
        Response.ContentType = "image/gif";
    }
    int XLinePos;
    private void DrawAxis(Graphics g)
    {
        Font Numberfont = new Font("Arial", 9, FontStyle.Regular);
        Font WordFont = new Font("Arial", 9, FontStyle.Bold);
        Font smallwordFont = new Font("Arial", 9, FontStyle.Bold);
        Rectangle YtitleRect = new Rectangle(0, NETAREABOTTOMMARGIN, NETAREALEFTMARGIN - XAxisScaleLength, HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN);
        StringFormat format2 = new StringFormat(StringFormatFlags.DirectionVertical);
        format2.LineAlignment = StringAlignment.Near;
        format2.Alignment = StringAlignment.Center;
        
        g.DrawString("Relative Abundance", WordFont , new SolidBrush(PreforColor ), YtitleRect, format2);
        
        Rectangle XtitleRect = new Rectangle(WIDTH - NETAREALEFTMARGIN, NETAREATOPMARGIN/2, NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN );
        StringFormat format3 = new StringFormat(StringFormatFlags.NoClip);
        format3.LineAlignment = StringAlignment.Center;
        format3.Alignment = StringAlignment.Center;
        g.DrawString("m/z", smallwordFont, new SolidBrush(PreforColor), XtitleRect, format3);
        //draw the axis main line
        //Y axis
        g.DrawLine(new Pen(PreforColor), new Point(NETAREALEFTMARGIN, NETAREATOPMARGIN), new Point(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN));
        //X axis
        XLinePos = HEIGHT - NETAREABOTTOMMARGIN - (HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN) / 2;
        g.DrawLine(new Pen(PreforColor), new Point(NETAREALEFTMARGIN, XLinePos), new Point(WIDTH - WHITEMARGIN, XLinePos));

        int StartX = (int)(MinX / XBIGSEG) * XBIGSEG;
        int EndX = (int)(MaxX / XBIGSEG + 1) * XBIGSEG;
        float XUnit = (float)(WIDTH - WHITEMARGIN - NETAREALEFTMARGIN) / (EndX - StartX);
        float XStepValue = XUnit * XBIGSEG;

        int stepNum = (EndX - StartX) / XBIGSEG;
        //draw the top scale
            g.DrawLine(new Pen (PreforColor ), new Point(NETAREALEFTMARGIN, NETAREATOPMARGIN ), new Point(WIDTH - WHITEMARGIN, NETAREATOPMARGIN ));
        //draw the bottom scale
            g.DrawLine(new Pen(PreforColor), new Point(NETAREALEFTMARGIN, HEIGHT - NETAREABOTTOMMARGIN), new Point(WIDTH - WHITEMARGIN, HEIGHT - NETAREABOTTOMMARGIN));
       
        for (int i = 0; i <= stepNum; i++)
        {
            float x1 = NETAREALEFTMARGIN + i * XStepValue;
            g.DrawLine(new Pen(PreforColor), x1, NETAREATOPMARGIN, x1, NETAREATOPMARGIN + XAxisScaleLength);
            g.DrawLine(new Pen(PreforColor), x1, HEIGHT - NETAREABOTTOMMARGIN - XAxisScaleLength, x1, HEIGHT - NETAREABOTTOMMARGIN);
 
            Rectangle stringrect = new Rectangle((int)x1 - 32, NETAREATOPMARGIN  - 16, 64, 16);
            Rectangle stringrect2 = new Rectangle((int)x1 - 32, HEIGHT - NETAREABOTTOMMARGIN+2  , 64, 16);
            StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
            format1.LineAlignment = StringAlignment.Center;
            format1.Alignment = StringAlignment.Center;
            g.DrawString(string.Format("{0:F0}", StartX + i * XBIGSEG), Numberfont, new SolidBrush(PreforColor )  , stringrect, format1);
            g.DrawString(string.Format("{0:F0}", StartX + i * XBIGSEG), Numberfont, new SolidBrush(PreforColor), stringrect2, format1);
        }
        int TotalY = 100;
        if (bZoomOut)
        {
            TotalY = DisplayMaxY;
        }

        float YUnit = (XLinePos  - NETAREATOPMARGIN) / (float)TotalY;
        float YStepValue = YUnit * YBIGSEG;
        int yStepNum = TotalY / YBIGSEG;


        for (int i = 0; i <= yStepNum; i++)
        {
            float y1 = XLinePos  - i * YStepValue;
            g.DrawLine(new Pen(PreforColor), NETAREALEFTMARGIN - XAxisScaleLength, y1, NETAREALEFTMARGIN, y1);

            Rectangle stringrect = new Rectangle(WHITEMARGIN, (int)y1-5, NETAREALEFTMARGIN - XAxisScaleLength - WHITEMARGIN, 16);

            StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
            format1.LineAlignment = StringAlignment.Far;
            format1.Alignment = StringAlignment.Center;
            g.DrawString(string.Format("{0:F0}", i * YBIGSEG), Numberfont, new SolidBrush(PreforColor), stringrect, format1);
        }
        Font titleFont = new Font("Arial", 16, FontStyle.Bold);
        g.DrawString("Library", titleFont, new SolidBrush(PreforColor), new PointF(WIDTH - 86, NETAREATOPMARGIN));
        g.DrawString(PeptideSequence, titleFont, new SolidBrush(Color.LightGray), new PointF(NETAREALEFTMARGIN + 5, NETAREATOPMARGIN + 25));
        g.DrawString("Query", titleFont, new SolidBrush(PreforColor), new PointF(WIDTH - 80, HEIGHT - NETAREABOTTOMMARGIN - 30));
        g.DrawString(AdditionalSequence, titleFont, new SolidBrush(Color.LightGray), new PointF(NETAREALEFTMARGIN + 5, HEIGHT - NETAREABOTTOMMARGIN - 50));
        for (int i = 0; i <= yStepNum; i++)
        {
            float y1 = XLinePos + i * YStepValue;
            g.DrawLine(Pens.Gray, NETAREALEFTMARGIN - XAxisScaleLength, y1, NETAREALEFTMARGIN, y1);

            Rectangle stringrect = new Rectangle(WHITEMARGIN, (int)y1-5, NETAREALEFTMARGIN - XAxisScaleLength - WHITEMARGIN, 16);

            StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
            format1.LineAlignment = StringAlignment.Far;
            format1.Alignment = StringAlignment.Center;
            g.DrawString(string.Format("{0:F0}", i * YBIGSEG), Numberfont, new SolidBrush(PreforColor), stringrect, format1);
        }
        DrawDiffMap(g, XUnit, YUnit,StartX);
        DrawData(g, XUnit, YUnit, StartX);
        if (bShowTheory)
        {
            if (PeptideSequence == AdditionalSequence)
                DrawTheoryData(g, XUnit, StartX);
            else
            {
                DrawTheoryData2(g, XUnit, StartX);
                DrawUserTheoryData(g, XUnit, StartX);
            }
        }

    }
    private string ValidePepSeq(string Src_seq)
    {
        string newSeq = "";
        Regex reg = new Regex("[ACDEFGHIKLMNPQRSTVWY]");
        MatchCollection matchMade = reg.Matches(Src_seq);
        for (int i = 0; i < matchMade.Count; i++)
        {
            newSeq += matchMade[i].Value;
        }

        return newSeq;
    }

    bool HadModifed(int site, string mModifiedSequence, ref float ModifiedWeight)
    {
        Regex reg = new Regex("[ACDEFGHIKLMNPQRSTVWY]");
        int iCurrent = 0;
        int iSite = 1;
        if (site == 0)
        {
            if (mModifiedSequence.StartsWith("("))
            {
                string mod = mModifiedSequence.Substring(1, mModifiedSequence.IndexOf(")") - 1);
                ModifiedWeight = float.Parse(mod);
                return true;
            }
            return false;
        }
        while (true)
        {
            if (reg.IsMatch(mModifiedSequence.Substring(iCurrent, 1)))
            {

                if (iSite == site)
                {
                    if (reg.IsMatch(mModifiedSequence.Substring(iCurrent + 1, 1)))
                    {
                        return false;
                    }
                    else
                    {
                        string mod = mModifiedSequence.Substring(iCurrent + 2, mModifiedSequence.IndexOf(")", iCurrent) - iCurrent - 2);
                        ModifiedWeight = float.Parse(mod);
                        return true;
                    }
                }
                iSite++;
            }

            iCurrent++;
            if (iCurrent >= mModifiedSequence.Length - 1)
                break;

        }
        return false;

    }
    private void DrawUserTheoryData(Graphics g, float XUnit, int xstart)
    {
        PeptideMW PMW = new PeptideMW(this.AdditionalSequence);
        float[] Bs = PMW.GetPepFragmentBValues();
        float[] Ys = PMW.GetPepFragmentYValues();
        Pen dataPen = new Pen(Brushes.Blue, 1);
        dataPen.DashStyle = DashStyle.Dash;
        Pen dataPen2 = new Pen(Brushes.Red, 1);
        dataPen2.DashStyle = DashStyle.Dash;

        Font Numberfont = new Font("Times New Roman", 10, FontStyle.Regular);
        //int charge = int.Parse(ChargeState);
        StringFormat sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Center;
        int axisline = HEIGHT - NETAREABOTTOMMARGIN - (HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN) / 2;
        for (int i = 0; i < Bs.Length; i++)
        {
            if (Bs[i] > xstart)
            {
                float x = (Bs[i] - xstart) * XUnit + NETAREALEFTMARGIN;
                float previousAA = 0.0F;
                if (i == 0)
                    previousAA = NETAREALEFTMARGIN;
                else
                {
                    if (Bs[i - 1] > xstart)
                        previousAA = (Bs[i - 1] - xstart) * XUnit + NETAREALEFTMARGIN;
                    else
                        previousAA = NETAREALEFTMARGIN;
                }

                g.DrawLine(dataPen, x, (float)axisline, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
                string neatSequence = ValidePepSeq(this.AdditionalSequence);
                float modified = 0.0F;
                string AA = "";
                if (HadModifed(i+1, AdditionalSequence, ref modified))
                    AA = string.Format("{0}+{1:F2}", neatSequence.Substring(i, 1), modified);
                else
                    AA = neatSequence.Substring(i, 1);


                g.DrawString(AA, Numberfont, Brushes.Blue, new RectangleF(previousAA, HEIGHT - NETAREABOTTOMMARGIN - 25, x - previousAA, 25), sf);
            }

        }

        for (int i = 0; i < Ys.Length; i++)
        {
            if (Ys[i] > xstart)
            {
                float x = (Ys[i] - xstart) * XUnit + NETAREALEFTMARGIN;
                float previousAA = 0.0F;
                if (i == 0)
                    previousAA = NETAREALEFTMARGIN;
                else
                {
                    if (Ys[i - 1] > xstart)
                        previousAA = (Ys[i - 1] - xstart) * XUnit + NETAREALEFTMARGIN;
                    else
                        previousAA = NETAREALEFTMARGIN;
                }
                g.DrawLine(dataPen2, x, (float)axisline , x, (float)HEIGHT-NETAREABOTTOMMARGIN -25);
                string neatSequence = ValidePepSeq(AdditionalSequence);
                int iSite = neatSequence.Length -1- i;
                float modified = 0.0F;
                string AA = "";
                if (HadModifed(iSite+1, AdditionalSequence, ref modified))
                    AA = string.Format("{0}+{1:F2}", neatSequence.Substring(iSite, 1), modified);
                else
                    AA = neatSequence.Substring(iSite, 1);

                g.DrawString(AA, Numberfont, Brushes.Red, new RectangleF(previousAA, HEIGHT - NETAREABOTTOMMARGIN - 50, x - previousAA, 25), sf);
            }

            //if (Ys[i] / 2 > xstart)
            //{
            //    float x = (Ys[i] / 2 - xstart) * XUnit + NETAREALEFTMARGIN;

            //    g.DrawLine(dataPen, x, (float)NETAREATOPMARGIN, x, (float)HEIGHT - NETAREABOTTOMMARGIN);

            //    g.DrawString(string.Format("Y{0:D}:2+", i + 1), Numberfont, Brushes.Red, new PointF(x, NETAREATOPMARGIN + 12));
            //}
        }
    }
    private void DrawTheoryData2(Graphics g, float XUnit, int xstart)
    {
        PeptideMW PMW = new PeptideMW(this.PeptideSequence);
        float[] Bs = PMW.GetPepFragmentBValues();
        float[] Ys = PMW.GetPepFragmentYValues();
        Pen dataPen = new Pen(Brushes.Blue, 1);
        dataPen.DashStyle = DashStyle.Dash;
        Pen dataPen2 = new Pen(Brushes.Red, 1);
        dataPen2.DashStyle = DashStyle.Dash;

        Font Numberfont = new Font("Times New Roman", 10, FontStyle.Regular);
        //int charge = int.Parse(ChargeState);
        StringFormat sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Center;
        int axisline = HEIGHT - NETAREABOTTOMMARGIN - (HEIGHT - NETAREABOTTOMMARGIN - NETAREATOPMARGIN) / 2;
        for (int i = 0; i < Bs.Length; i++)
        {
            if (Bs[i] > xstart)
            {
                float x = (Bs[i] - xstart) * XUnit + NETAREALEFTMARGIN;
                float previousAA = 0.0F;
                if (i == 0)
                    previousAA = NETAREALEFTMARGIN;
                else
                {
                    if (Bs[i - 1] > xstart)
                        previousAA = (Bs[i - 1] - xstart) * XUnit + NETAREALEFTMARGIN;
                    else
                        previousAA = NETAREALEFTMARGIN;
                }

                g.DrawLine(dataPen, x, (float)NETAREATOPMARGIN, x, (float)axisline );
                string neatSequence = ValidePepSeq(this.AdditionalSequence);
                float modified = 0.0F;
                string AA = "";
                if (HadModifed(i + 1, this.PeptideSequence, ref modified))
                    AA = string.Format("{0}+{1:F2}", neatSequence.Substring(i, 1), modified);
                else
                    AA = neatSequence.Substring(i, 1);


                g.DrawString(AA, Numberfont, Brushes.Blue, new RectangleF(previousAA, NETAREATOPMARGIN, x - previousAA, 25), sf);
            }

        }

        for (int i = 0; i < Ys.Length; i++)
        {
            if (Ys[i] > xstart)
            {
                float x = (Ys[i] - xstart) * XUnit + NETAREALEFTMARGIN;
                float previousAA = 0.0F;
                if (i == 0)
                    previousAA = NETAREALEFTMARGIN;
                else
                {
                    if (Ys[i - 1] > xstart)
                        previousAA = (Ys[i - 1] - xstart) * XUnit + NETAREALEFTMARGIN;
                    else
                        previousAA = NETAREALEFTMARGIN;
                }
                g.DrawLine(dataPen2, x, (float)NETAREATOPMARGIN + 25, x, (float)axisline );
                string neatSequence = ValidePepSeq(AdditionalSequence);
                int iSite = neatSequence.Length - 1 - i;
                float modified = 0.0F;
                string AA = "";
                if (HadModifed(iSite + 1, this.PeptideSequence, ref modified))
                    AA = string.Format("{0}+{1:F2}", neatSequence.Substring(iSite, 1), modified);
                else
                    AA = neatSequence.Substring(iSite, 1);

                g.DrawString(AA, Numberfont, Brushes.Red, new RectangleF(previousAA, NETAREATOPMARGIN + 25, x - previousAA, 25), sf);
            }

            //if (Ys[i] / 2 > xstart)
            //{
            //    float x = (Ys[i] / 2 - xstart) * XUnit + NETAREALEFTMARGIN;

            //    g.DrawLine(dataPen, x, (float)NETAREATOPMARGIN, x, (float)HEIGHT - NETAREABOTTOMMARGIN);

            //    g.DrawString(string.Format("Y{0:D}:2+", i + 1), Numberfont, Brushes.Red, new PointF(x, NETAREATOPMARGIN + 12));
            //}
        }
    }

    private void DrawTheoryData(Graphics g, float XUnit, int xstart)
    {
        PeptideMW PMW = new PeptideMW(this.PeptideSequence);
        float[] Bs = PMW.GetPepFragmentBValues();
        float[] Ys = PMW.GetPepFragmentYValues();
        Pen dataPen = new Pen(Brushes.Blue, 1);
        dataPen.DashStyle = DashStyle.Dash;
        Pen dataPen2 = new Pen(Brushes.Red, 1);
        dataPen2.DashStyle = DashStyle.Dash;

        Font Numberfont = new Font("Times New Roman", 10, FontStyle.Regular);
        //int charge = int.Parse(ChargeState);
        StringFormat sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Center;
        for (int i = 0; i < Bs.Length; i++)
        {
            if (Bs[i] > xstart)
            {
                float x = (Bs[i] - xstart) * XUnit + NETAREALEFTMARGIN;
                float previousAA =0.0F; 
                if (i==0)
                    previousAA = NETAREALEFTMARGIN ;
                else
                {
                    if (Bs[i-1] > xstart )
                        previousAA = (Bs[i-1]-xstart )* XUnit + NETAREALEFTMARGIN;
                    else
                        previousAA = NETAREALEFTMARGIN ;
                }

                g.DrawLine(dataPen, x, (float)NETAREATOPMARGIN, x, (float)HEIGHT - NETAREABOTTOMMARGIN-25);
                string neatSequence = ValidePepSeq(this.AdditionalSequence);
                float modified = 0.0F;
                string AA = "";
                if (HadModifed(i+1, AdditionalSequence, ref modified))
                    AA = string.Format("{0}+{1:F2}", neatSequence.Substring(i, 1), modified);
                else
                    AA = neatSequence.Substring(i, 1);
              
                
                g.DrawString(AA, Numberfont, Brushes.Blue, new RectangleF (previousAA , NETAREATOPMARGIN ,x-previousAA ,25),sf);
            }

        }

        for (int i = 0; i < Ys.Length; i++)
        {
            if (Ys[i] > xstart)
            {
                float x = (Ys[i] - xstart) * XUnit + NETAREALEFTMARGIN;
                float previousAA = 0.0F;
                if (i == 0)
                    previousAA = NETAREALEFTMARGIN;
                else
                {
                    if (Ys[i - 1] > xstart)
                        previousAA = (Ys[i - 1] - xstart) * XUnit + NETAREALEFTMARGIN;
                    else
                        previousAA = NETAREALEFTMARGIN;
                }
                g.DrawLine(dataPen2, x, (float)NETAREATOPMARGIN+25, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
                string neatSequence = ValidePepSeq(AdditionalSequence);
                int iSite = neatSequence.Length-1  - i;
                float modified = 0.0F;
                string AA = "";
                if (HadModifed(iSite+1, AdditionalSequence, ref modified))
                    AA = string.Format("{0}+{1:F2}", neatSequence.Substring(iSite, 1), modified);
                else
                    AA = neatSequence.Substring(iSite, 1);
                
                g.DrawString(AA, Numberfont, Brushes.Red, new RectangleF(previousAA, HEIGHT - NETAREABOTTOMMARGIN-25, x - previousAA, 25), sf);
            }
            //if (Ys[i] / 2 > xstart)
            //{
            //    float x = (Ys[i] / 2 - xstart) * XUnit + NETAREALEFTMARGIN;

            //    g.DrawLine(dataPen, x, (float)NETAREATOPMARGIN, x, (float)HEIGHT - NETAREABOTTOMMARGIN);

            //    g.DrawString(string.Format("Y{0:D}:2+", i + 1), Numberfont, Brushes.Red, new PointF(x, NETAREATOPMARGIN + 12));
            //}
        }

    }

    const float OH = 17.00274F;
    const float H2O = 18.01057F;
    const float HPO3 = 79.966391F;
    const float H3PO4 = 97.976961F;
    const float Aion = 28.0F;
    private string GetAnnotation(ArrayList Spectra, int pos, float[] Bs, float[] Ys, bool NoiseMode, float PreMZ, int charge, bool bPhosphorylation)
    {
        if (!IsPeak(Spectra,pos)&&!NoiseMode)
            return "";
        float mz = ((MZintensitiy)Spectra[pos]).mz;
        return GetAnnotation(mz, Bs, Ys, PreMZ, charge, bPhosphorylation);
    }

    private bool IsPeak(ArrayList Spectra, int pos)
    {
        bool result = true ;
        float posmz = ((MZintensitiy)Spectra[pos]).mz;
        float posintensity = ((MZintensitiy)Spectra[pos]).intensity ;
        int i = pos-1;
        int j = pos+1;
        while (i > 0 && j < Spectra.Count)
        {           
            float ipos = ((MZintensitiy)Spectra[i]).mz;
            float jpos = ((MZintensitiy)Spectra[j]).mz;
            
            if (posmz - ipos > 0.9 && jpos - posmz > 0.9)
                break;
            float jintensity = ((MZintensitiy)Spectra[j]).intensity ;
            float iintensity = ((MZintensitiy)Spectra[i]).intensity;
            if (posmz - ipos <= 0.9 && iintensity > posintensity)
            {
                result = false;
                break;
            }
            if (jpos - posmz <= 0.9 && jintensity > posintensity)
            {
                result = false;
                break;
            } 
            i = i - 1;
            j = j + 1;
        }
        return result;
    }

     Boolean IsMatch(float i, Hashtable ions, ref string annotation)
    {
        float Prioritymindif = 100;
        string Priorityannotation = "";
        float othermindif = 100;
        string otherannotation = "";

        foreach (float v in ions.Keys)
        {
            string ionsname = (string)ions[v];
            if (ionsname.Contains ("NH") || ionsname.Contains ("H2O"))
            {
                if (Math.Abs(i - v) < othermindif)
                {
                    othermindif = Math.Abs(i - v);
                    otherannotation = ionsname;
                }
            }
            else
            {
                if (Math.Abs(i - v) < Prioritymindif)
                {
                    Prioritymindif = Math.Abs(i - v);
                    Priorityannotation = (string)ions[v];

                }
            }
        }

        if (Prioritymindif < mzTolerance)
        {
            annotation = Priorityannotation;
            return true;
        }
        if (othermindif < mzTolerance)
        {
            annotation = otherannotation;
            return true;
        }
        return false;
    }

     private string GetAnnotation(float mz, float[] B, float[] Y, float precursor, int charge, bool bPhosphorylation)
    {
        Hashtable Ions = new Hashtable();
        Ions.Add(precursor, string.Format("M+{0}", charge));
        Ions.Add(precursor - OH / charge, string.Format("(M-NH3)+{0}", charge));
        Ions.Add(precursor - H2O / charge, string.Format("(M-H2O)+{0}", charge));
        Ions.Add(precursor - 2 * OH / charge, string.Format("(M-2*NH3)+{0}", charge));
        Ions.Add(precursor - 2 * H2O / charge, string.Format("(M-2*H2O)+{0}", charge));
        Ions.Add(precursor - (OH + H2O) / charge, string.Format("(M-NH3-H2O)+{0}", charge));
        if (bPhosphorylation)
        {
            Ions.Add(precursor - HPO3 / charge, string.Format("(M-HPO3)+{0}", charge));
            Ions.Add(precursor - H3PO4 / charge, string.Format("(M-H3PO4)+{0}", charge));
        }
        for (int i = 0; i < B.Length; i++)
        {
            for (int z = 1; z <= charge; z++)
            {
                if (!Ions.Contains((B[i] + z - 1) / z))
                    Ions.Add((B[i] + z - 1) / z, string.Format("(b{0})+{1}", i+1, z));
                if (!Ions.Contains((B[i] - OH + z - 1) / z))
                    Ions.Add((B[i] - OH + z - 1) / z, string.Format("(b{0}-NH)+{1}", i+1, z));
                if (!Ions.Contains((B[i] - H2O + z - 1) / z))
                    Ions.Add((B[i] - H2O + z - 1) / z, string.Format("(b{0}-H2O)+{1}", i+1, z));
                //if (!Ions.Contains((B[i] - 2 * OH + z - 1) / z))
                //    Ions.Add((B[i] - 2 * OH + z - 1) / z, string.Format("B{0}:{1}:-2NH", i, z));
                //if (!Ions.Contains((B[i] - 2 * H2O + z - 1) / z))
                //    Ions.Add((B[i] - 2 * H2O + z - 1) / z, string.Format("B{0}:{1}:-2H2O", i, z));
                if (!Ions.Contains((B[i] - Aion + z - 1) / z))
                    Ions.Add((B[i] - Aion + z - 1) / z, string.Format("(a{0})+{1}", i+1, z));
            }

            //float B2 = (B[i] + 1) / 2;
            //Fragments.Add(B2);
            //Fragments.Add (B2-OH/2 );
            //Fragments.Add (B2-H2O/2);
            //float A = B[i] - Aion;
            //Fragments.Add(A);
            //float A2 = (A + 1) / 2;
            //Fragments.Add(A);

        }

        for (int i = 0; i < Y.Length; i++)
        {
            for (int z = 1; z <= charge; z++)
            {
                if (!Ions.Contains((Y[i] + z - 1) / z))
                    Ions.Add((Y[i] + z - 1) / z, string.Format("(y{0})+{1}", i+1, z));
                if (!Ions.Contains((Y[i] - OH + z - 1) / z))
                    Ions.Add((Y[i] - OH + z - 1) / z, string.Format("(y{0}-NH)+{1}", i+1, z));
                if (!Ions.Contains((Y[i] - H2O + z - 1) / z))
                    Ions.Add((Y[i] - H2O + z - 1) / z, string.Format("(y{0}-H2O)+{1}", i+1, z));
                //if (!Ions.Contains((Y[i] - 2 * OH + z - 1) / z))
                //Ions.Add((Y[i] - 2 * OH + z - 1) / z, string.Format("Y{0}:{1}", i, z));
                //if (!Ions.Contains((Y[i] - 2 * H2O + z - 1) / z))
                //Ions.Add((Y[i] - 2 * H2O + z - 1) / z, string.Format("Y{0}:{1}", i, z));
            }

            //float Y2 = (Y[i] + 1) / 2;
            //Fragments.Add(Y2);
            //Fragments.Add(Y2 - OH);
            //Fragments.Add(Y2 - H2O);
        }

        string annotation = "";
        IsMatch(mz, Ions, ref annotation);
        return annotation;
    }

    private string GetAnnotation(float mz, float[] Bs, float[] Ys)
    {
        string strAnnotation = "";
        string LossFlag1 = "";
        int b1 = -1;
        string bCharge = "";
        string yCharge = "";
        string aCharge = "";
        float minBDiff = 1000.0f;
        for (int i = 0; i < Bs.Length; i++)
        {
            float diff1 = Math.Abs(mz - Bs[i]);
            float diff2 = Math.Abs(mz - (Bs[i] + 1) / 2);

            float diff = Math.Min(diff1, diff2);
            if (diff < minBDiff)
            {
                if (diff1 > diff2)
                    bCharge = "++";
                else
                    bCharge = "+";
                minBDiff = diff;
                LossFlag1 = "";
                b1 = i;
            }

            float diff3 = Math.Abs(mz - Bs[i] + OH);
            float diff4 = Math.Abs(mz - Bs[i] + H2O);
            float diff5 = Math.Min(diff3, diff4);
            if (diff5 < minBDiff)
            {
                if (diff3 > diff4)
                {
                    LossFlag1 = "-H2O";
                }
                else
                    LossFlag1 = "-NH3";
                b1 = i;
                bCharge = "+";
                minBDiff = diff5;
            }
        }
        int a1 = -1;
        float minADiff = 1000.0f;
        string LossFlaga = "";
        //the a ions: b ions -28
        for (int i = 0; i < Bs.Length; i++)
        {
            float diff1 = Math.Abs(mz - Bs[i] + 28);
            float diff2 = Math.Abs(mz - (Bs[i] - 28 + 1) / 2);

            float diff = Math.Min(diff1, diff2);
            if (diff < minADiff)
            {
                if (diff1 > diff2)
                    aCharge = "++";
                else
                    aCharge = "+";
                minADiff = diff;
                LossFlaga = "";
                a1 = i;
            }

            float diff3 = Math.Abs(mz - Bs[i] + 28 + OH);
            float diff4 = Math.Abs(mz - Bs[i] + 28 + H2O);
            float diff5 = Math.Min(diff3, diff4);
            if (diff5 < minADiff)
            {
                if (diff3 > diff4)
                {
                    LossFlaga = "-H2O";
                }
                else
                    LossFlaga = "-NH3";
                a1 = i;
                aCharge = "+";
                minADiff = diff5;
            }
        }

        float minYDiff = 1000.0f;
        int y1 = -1;
        string LossFlag2 = "";
        for (int i = 0; i < Ys.Length; i++)
        {
            float diff1 = Math.Abs(mz - Ys[i]);
            float diff2 = Math.Abs(mz - (Ys[i] + 1) / 2);

            float diff = Math.Min(diff1, diff2);
            if (diff < minYDiff)
            {
                if (diff1 > diff2)
                    yCharge = "++";
                else
                    yCharge = "+";
                minYDiff = diff;
                LossFlag2 = "";
                y1 = i;
            }
            float diff3 = Math.Abs(mz - Ys[i] + OH);
            float diff4 = Math.Abs(mz - Ys[i] + H2O);
            float diff5 = Math.Min(diff3, diff4);
            if (diff5 < minYDiff)
            {
                if (diff3 > diff4)
                {
                    LossFlag2 = "-H2O";
                }
                else
                    LossFlag2 = "-NH3";
                y1 = i;
                yCharge = "+";
                minYDiff = diff5;
            }

        }

        float minPDiff = 1000.0f;
        float diffp1 = Math.Abs(mz - PrecursorMZ);

        float diffp2 = Math.Abs(mz - PrecursorMZ + H2O / int.Parse(ChargeState));
        float diffp3 = Math.Abs(mz - PrecursorMZ + OH / int.Parse(ChargeState));
        float diffp22 = Math.Abs(mz - PrecursorMZ + H2O * 2 / int.Parse(ChargeState));
        float diffp32 = Math.Abs(mz - PrecursorMZ + OH * 2 / int.Parse(ChargeState));
        string LossFlagP = "";
        if (diffp2 < diffp3 && diffp2 < diffp1 && diffp2 < diffp22 && diffp2 < diffp32)
        {
            minPDiff = diffp2;
            LossFlagP = "-H2O";
        }
        else if (diffp3 < diffp2 && diffp3 < diffp1 && diffp3 < diffp22 && diffp3 < diffp32)
        {
            minPDiff = diffp3;
            LossFlagP = "-NH3";
        }
        else if (diffp1 < diffp2 && diffp1 < diffp3 && diffp1 < diffp22 && diffp1 < diffp32)
        {
            minPDiff = diffp1;
            LossFlagP = "";
        }
        else if (diffp22 < diffp1 && diffp22 < diffp2 && diffp22 < diffp3 && diffp22 < diffp32)
        {
            minPDiff = diffp22;
            LossFlagP = "-2*H2O";
        }
        else
        {
            minPDiff = diffp32;
            LossFlagP = "-2*NH3";
        }


        if (minBDiff < minYDiff && minBDiff < minADiff)
        {
            if (minBDiff < mzTolerance)
            {
                strAnnotation = string.Format("(b{0:D}{1}){2}", b1 + 1, LossFlag1, bCharge);
            }
        }
        else if (minYDiff < minBDiff && minYDiff < minADiff)
        {
            if (minYDiff < mzTolerance)
            {
                strAnnotation = string.Format("(y{0:D}{1}){2}", y1 + 1, LossFlag2, yCharge);
            }
        }
        else
        {
            if (minADiff < mzTolerance)
            {
                strAnnotation = string.Format("(a{0:D}{1}){2}", a1 + 1, LossFlaga, aCharge);
            }
        }

        if (minBDiff < minYDiff && minBDiff < minADiff && minBDiff < minPDiff)
        {
            if (minBDiff < mzTolerance)
            {
                strAnnotation = string.Format("(b{0:D}{1}){2}", b1 + 1, LossFlag1, bCharge);
            }
        }
        else if (minYDiff < minBDiff && minYDiff < minADiff && minYDiff < minPDiff)
        {
            if (minYDiff < mzTolerance)
            {
                strAnnotation = string.Format("(y{0:D}{1}){2}", y1 + 1, LossFlag2, yCharge);
            }
        }
        else if (minADiff < minBDiff && minADiff < minYDiff && minADiff < minPDiff)
        {
            if (minADiff < mzTolerance)
            {
                strAnnotation = string.Format("(a{0:D}{1}){2}", a1 + 1, LossFlaga, aCharge);
            }
        }
        else
        {
            if (minPDiff < mzTolerance)
            {
                strAnnotation = string.Format("(M{0})+{1}", LossFlagP, ChargeState);
            }
        }

        return strAnnotation;
        

    }

    private void DrawData(Graphics g, float Xunit, float Yunit, int xstart)
    {

        PeptideMW PMW = new PeptideMW(PeptideSequence);
        float[] Bs = PMW.GetPepFragmentBValues();
        float[] Ys = PMW.GetPepFragmentYValues();
        int charge1 = int.Parse (ChargeState);//(int) (PMW.GetMW2() / PrecursorMZ) +1;
        bool bPhos1 = PMW.IsPhosphorylation();
        PeptideMW PMW2 = new PeptideMW(AdditionalSequence);
        float[] Bs2 = PMW2.GetPepFragmentBValues();
        float[] Ys2 = PMW2.GetPepFragmentYValues();
        float Precursor2 = PMW2.getPrecurMz2(charge1);
        bool bPhos2 = PMW2.IsPhosphorylation();
        

        Pen dataPen = new Pen(Brushes.Black, 1);
        Pen BLinePen = new Pen(Brushes.Blue, 1);
        Pen YLinePen = new Pen(Brushes.Red, 1);
        Pen ALinePen = new Pen(Brushes.Green, 1);
        Pen MLinePen = new Pen(Brushes.Gray , 1);
        int LabelTolerance = 20;
        if (bShowNoise)
        {
            BLinePen = new Pen(NoiseModeColor , 1);
            YLinePen = new Pen(NoiseModeColor, 1);
            ALinePen = new Pen(NoiseModeColor , 1);
            MLinePen = new Pen(NoiseModeColor , 1);
            LabelTolerance = 1;
        }
        //draw the library spectrum
        int Count = SpectrumData.Count;
        
        for (int i = 0; i < Count; i++)
        {

            float mz = ((MZintensitiy)SpectrumData[i]).mz;
            float intensity = ((MZintensitiy)SpectrumData[i]).intensity;

            float x = (mz - xstart) * Xunit + NETAREALEFTMARGIN;
            float y;

            if (bZoomOut)
            {
                if (intensity * 100 / MaxIntensitiy > DisplayMaxY)
                {
                    y = NETAREATOPMARGIN;
                }
                else
                {
                    y =  XLinePos - intensity * 100 * Yunit / MaxIntensitiy;
                }

            }
            else
                y = XLinePos  - intensity * 100 * Yunit / MaxIntensitiy;

           

            Font Numberfont = new Font("Arial", 9, FontStyle.Regular);
            if (y < XLinePos  - LabelTolerance )
            {
                string strAnn = GetAnnotation(SpectrumData,i, Bs, Ys,bShowNoise,PrecursorMZ,charge1,bPhos1 );
                if (strAnn.StartsWith("(b"))
                {

                    if (bShowLabel)
                    {
                        g.DrawLine(BLinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Blue, new PointF(x, y));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(BLinePen, x, y, x, (float)XLinePos);
                        else
                            g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else if (strAnn.StartsWith("(y"))
                {

                    if (bShowLabel)
                    {
                        g.DrawLine(YLinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(YLinePen, x, y, x, (float)XLinePos);
                        else
                            g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else if (strAnn.StartsWith("(a"))
                {

                    if (bShowLabel)
                    {
                        g.DrawLine(ALinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Green, new PointF(x, y));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(ALinePen, x, y, x, (float)XLinePos);
                        else
                            g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else if (strAnn.StartsWith("(M") && y < XLinePos - 100)
                {

                    if (bShowLabel)
                    {
                        g.DrawLine(MLinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Gray, new PointF(x, y));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(MLinePen, x, y, x, (float)XLinePos);
                        else
                            g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else
                {
                    g.DrawLine(dataPen, x, y, x, (float)XLinePos );
                }
                //g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y));
            }
            else
                g.DrawLine(dataPen, x, y, x, (float)XLinePos );         
        }

        //draw the guest spectrum
        Count = GuestSpectrumData.Count;
      
        for (int i = 0; i < Count; i++)
        {

            float mz = ((MZintensitiy)GuestSpectrumData[i]).mz;
            float intensity = ((MZintensitiy)GuestSpectrumData[i]).intensity;

            float x = (mz - xstart) * Xunit + NETAREALEFTMARGIN;
            float y;

            if (bZoomOut)
            {
                if (intensity * 100 / GuestMaxIntensitiy > DisplayMaxY)
                {
                    y = HEIGHT - NETAREABOTTOMMARGIN ;
                }
                else
                {
                    y = XLinePos + intensity * 100 * Yunit / GuestMaxIntensitiy;
                }

            }
            else
                y = XLinePos + intensity * 100 * Yunit / GuestMaxIntensitiy;

            

            Font Numberfont = new Font("Arial", 9, FontStyle.Regular);
            if (y > XLinePos + LabelTolerance)
            {
                string strAnn = GetAnnotation(GuestSpectrumData,i, Bs2, Ys2,bShowNoise,Precursor2,charge1 ,bPhos2);
                if (strAnn.StartsWith("(b"))
                {
                    if (bShowLabel)
                    {
                        g.DrawLine(BLinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Blue, new PointF(x, y - 10));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(BLinePen, x, y, x, (float)XLinePos);
                        else
                            g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else if (strAnn.StartsWith("(y"))
                {

                    if (bShowLabel)
                    {
                        g.DrawLine(YLinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y - 10));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(YLinePen, x, y, x, (float)XLinePos);
                        else
                            g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else if (strAnn.StartsWith("(a"))
                {

                    if (bShowLabel)
                    {
                        g.DrawLine(ALinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Green, new PointF(x, y - 10));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(ALinePen, x, y, x, (float)XLinePos);
                        else
                            g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else if (strAnn.StartsWith("(M") && y > XLinePos + 100)
                {
                    if (bShowLabel)
                    {
                        g.DrawLine(MLinePen, x, y, x, (float)XLinePos);
                        g.DrawString(strAnn, Numberfont, Brushes.Gray, new PointF(x, y - 10));
                    }
                    else
                    {
                        if (bShowNoise )
                            g.DrawLine(MLinePen, x, y, x, (float)XLinePos);
                        else
                        g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                    }
                }
                else
                {
                    g.DrawLine(dataPen, x, y, x, (float)XLinePos);
                }
                //g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y));
            }
            else
                g.DrawLine(dataPen, x, y, x, (float)XLinePos);
           
        }
    }
    void DrawDiffMap(Graphics g, float Xunit, float Yunit, int xstart)
    {
        g.DrawRectangle (new Pen(Brushes.LightGray ), new Rectangle(NETAREALEFTMARGIN-1, HEIGHT - NETAREABOTTOMMARGIN + 18, WIDTH - NETAREALEFTMARGIN - WHITEMARGIN, 10));
        int Xend = (int)((WIDTH - NETAREALEFTMARGIN - WHITEMARGIN)/Xunit );
       
        
        int Count = SpectrumData.Count;
        int icount = GuestSpectrumData.Count ;
        int iDiff = (MaxX >GuestMaxX ?(int)MaxX :(int)GuestMaxX)  +1;
        float[] diffs = new float[iDiff  ] ;
        for (int i = 0; i < Count; i++)
        {
            diffs[(int)(((MZintensitiy)SpectrumData[i]).mz)] += ((MZintensitiy)SpectrumData[i]).intensity  * 100 / MaxIntensitiy;
        }
         for (int i = 0; i < icount; i++)
        {
            diffs[(int)(((MZintensitiy)GuestSpectrumData[i]).mz)] -= ((MZintensitiy)GuestSpectrumData[i]).intensity * 100 / GuestMaxIntensitiy;
        }
         for (int i = xstart ; i < iDiff; i++)
         {
             float x = (i - xstart) * Xunit + NETAREALEFTMARGIN;
             
             float y = diffs[i];
             Color c = new Color();
             if (y > 0)
             {
                 int R = (int)(y * 255 / 100);
                 R = R > 255 ? 255 : R;
                 c = Color.FromArgb(255, 255 - R, 255 - R);
                 
             }
             else if (y < 0)
             {
                 int G =(int)( -1 * y * 255 / 100);
                 G = G > 255 ? 255 : G;
                 c = Color.FromArgb(255-G,255,255-G);
             }
             else
             {
                 c = Color.White   ;
             }
            SolidBrush  db = new SolidBrush(c);           
            g.FillRectangle(db, new RectangleF(x, HEIGHT - NETAREABOTTOMMARGIN + 19, Xunit, 9));
         }
    }
}
