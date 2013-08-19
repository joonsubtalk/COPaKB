using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Configuration;
using ZJU.COPDB;
using System.Data.SqlClient;
using System.Data;
using ZJU.COPLib;
using System.Collections;
using System.Text.RegularExpressions;
using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using System.Configuration;
public partial class MatchSpectrum : System.Web.UI.Page
{
    string file_path;
    string normlize_file_path;
    string SessionID;
    string SearchType;
    string TaskID;
    string MSFile;
    string mSpectrum_Seq;
    string library_module;
    string PTMSequence;
    string dtaconent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["QValue"] != null)
        {
            mSpectrum_Seq = Request.QueryString["QValue"];
            file_path = mSpectrum_Seq;
            SessionID = Request.QueryString["SessionID"];
            SearchType = Request.QueryString["ST"];
            TaskID = Request.QueryString ["TaskID"];
            MSFile = Request.QueryString ["File"];
            PTMSequence = Request.QueryString["SEQ"];
            if (PTMSequence != null && CustomizedSequence.Text == "")
                CustomizedSequence.Text = PTMSequence;
            MaxDiff = float.Parse(WebConfigurationManager.ConnectionStrings["mzTolerance"].ConnectionString);
            QuerySpectrum(mSpectrum_Seq);
            ShowWiki(mSpectrum_Seq);
        }
    }

    private void QuerySpectrum(string SpectrumSeq)
    {
        string strSQL = string.Format("select a.Instrumentation,a.charge_state,a.Xcorr,a.Delta_CN,a.Peptide_cop_ID,ptm_sequence,a.lib_mod,a.precur_mz from spectrum_tbl a  where a.spectrum_SEQ ={0}", SpectrumSeq);
        //DBInterface.ConnectDB();
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            //lbSysInfo.Text = "";
            if (result.Tables[0].Rows.Count >0 )
            {
                if (!result.Tables[0].Rows[0].IsNull(0))
                    lbInstrumentation.Text = result.Tables[0].Rows[0].ItemArray[0].ToString ();
                if (!result.Tables[0].Rows[0].IsNull(1))
                    lbChargeState.Text = result.Tables[0].Rows[0].ItemArray[1].ToString();
                if (!result.Tables[0].Rows[0].IsNull(2))
                    lbXcorr.Text = result.Tables[0].Rows[0].ItemArray[2].ToString();
                if (!result.Tables[0].Rows[0].IsNull(3))
                    lbDeltaCN.Text = result.Tables[0].Rows[0].ItemArray[3].ToString();
                string PeptideID = result.Tables[0].Rows[0].ItemArray[4].ToString();
                lbPeptideCOPID.Text = string.Format("<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>", PeptideID , PeptideID );
                //int File_index = int.Parse(((System.Data.OracleClient.OracleDataReader)result).GetOracleValue(5).ToString());
                //string relative_path = result.GetString(7) ;
                //string File_Index = result.Tables[0].Rows[0].ItemArray[5].ToString ();
                library_module =result.Tables[0].Rows[0].ItemArray[6].ToString ();
                //string relative_path =  ConfigurationSettings.AppSettings[library_module ];//result.GetString(7) + "BestSpectra\\";
                string precursormz = string.Format("{0:F1}", result.Tables[0].Rows[0].ItemArray[7]);
                this.lbPrecursormz.Text = precursormz;
                //file_path = string.Format("{0}{1}", relative_path, File_Index);
                //file_path = string.Format("{0}{1:D5}.dta", WebConfigurationManager.ConnectionStrings["SPECTRUM"].ConnectionString+relative_path, File_index);
                //normlize_file_path = string.Format("{0}{1:D5}.dta", WebConfigurationManager.ConnectionStrings["NORMALIZESPECTRUM"].ConnectionString+relative_path , File_index);
                 lbModifiedSequence.Text = result.Tables[0].Rows[0].ItemArray[5].ToString ();
                if (CustomizedSequence.Text == "")
                 CustomizedSequence.Text = lbModifiedSequence.Text;
                if (SearchType == "DTA")
                    Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&SEQ={1}&ST={2}&SessionID={3}&pmz={4}&CS={5}", file_path, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, precursormz, lbChargeState.Text);
                else
                    Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&SEQ={1}&ST={2}&SessionID={3}&TaskID={4}&pmz={5}&CS={6}&MSFile={7}", file_path, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, precursormz, lbChargeState.Text, MSFile);
               
                //lbSysInfo.Text = "";
                //##I should note this modified sequence
                //GetPeptideFamily(lbModifiedSequence.Text);
                if (DBInterface.DbType == "Oracle")
                    lbPeptideFamily.Text = string.Format("<a href='PeptideFamily.aspx?PepSeq={0}'>View the whole peptide family in COPa Library</a>", lbModifiedSequence.Text);
                else
                    lbPeptideFamily.Text = "";

            }
            else
            {
                lbInstrumentation.Text = "";
                lbChargeState.Text = "";
                lbXcorr.Text = "";
                lbDeltaCN.Text = "";
                lbPeptideCOPID.Text = "";
                Image1.ImageUrl = "";
                
            }
            //result.Close();
            //result.Dispose();
        }

        //DBInterface.CloseDB();

        ShowTheoryValues(this.CustomizedSequence.Text);
        ComputeTotalMatch();
        ShowContributorInof();
    }


    private void ShowContributorInof()
    {
        string strSQL = string.Format("select HTML_REF from experiment_tbl where lib_mod='{0}'", library_module);
        //DBInterface.ConnectDB();
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            if (result.Tables[0].Rows.Count > 0)
            {
                //string ContributorInfo = "<b>Contributors:</b>{0}</br>";
                //ContributorInfo =string.Format (ContributorInfo , result.Tables[0].Rows[0].ItemArray [0].ToString ());
                //string Contact_info = "<b>Contact Email:</b>{0}</br>";
                //Contact_info = string.Format(Contact_info, result.Tables[0].Rows[0].ItemArray[1].ToString());
                //string Publication = string.Format("<b>Publication:</b>{0}", result.Tables[0].Rows[0].ItemArray[2].ToString());
                //ltContributors.Text = string.Format("{0}{1}{2}", ContributorInfo, Contact_info, Publication);
                ltContributors.Text = result.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            //result.Close();
        }
        //DBInterface.CloseDB();
    }

    private void ComputeTotalMatch()
    {
        int totalInos = MZS.Count;
        int MatchedInos = 0;
        float TotalIntensity = 0.0F;
        float MatchedIntensity = 0.0F;
        foreach (object value in MZS)
        {
            if (((mzint)value).bMatched)
            {
                MatchedInos += 1;
                MatchedIntensity += ((mzint)value).intensity;
            }

            TotalIntensity += ((mzint)value).intensity;
        }

        float InoMatchRate = (float)MatchedInos * 100 / totalInos;
        float IntensityMatchRate = MatchedIntensity * 100 / TotalIntensity;
        this.lbIonsPercent.Text = string.Format("{0:F2}%", InoMatchRate);
        this.lbIntensityPercent.Text = string.Format("{0:F2}%", IntensityMatchRate);
    }
    const float OH = 17.00274F;
    const float H2O = 18.01057F;
    ArrayList MZS = new ArrayList();

    struct mzint
    {
        public float mz;
        public float intensity;
        public bool bMatched;
    }
    ArrayList Intensitys = new ArrayList();
    bool bFileReaded = false;
    System.Drawing.Color Y1Color = System.Drawing.Color.Red;
    System.Drawing.Color Y2Color = System.Drawing.Color.Brown;
    System.Drawing.Color Y3Color = System.Drawing.Color.Purple;
    System.Drawing.Color B1Color = System.Drawing.Color.Blue;
    System.Drawing.Color B2Color = System.Drawing.Color.Green;
    System.Drawing.Color B3Color = System.Drawing.Color.YellowGreen;

    string GuestSpectrums = "";

    private void ShowTheoryValues(string PepSeq)
    {
        //PepSeq = ValidePepSeq(PepSeq);

        if (!bFileReaded)
        {
            if (SearchType == "DTA")
            {
                if (GuestSpectrums == "")
                    GuestSpectrums = (string)(Session[SessionID]);
                dtaconent = GuestSpectrums;
                string[] lines = GuestSpectrums.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (char.IsDigit(line, 0))
                    {
                        int sperator = line.IndexOf(" ");
                        string mz = line.Substring(0, sperator);
                        string intensity = line.Substring(sperator);
                        mzint mzi = new mzint();
                        mzi.mz = float.Parse(mz);
                        mzi.intensity = float.Parse(intensity);
                        mzi.bMatched = false;
                        MZS.Add(mzi);
                    }
                }
            }
            else
            { 
                string TaskID = Request.QueryString["TaskID"];
                
                string dtaFilePath = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + string.Format ("\\{0}_dta\\",MSFile ) + SessionID + ".dta";
                FileInfo fiDta = new FileInfo(dtaFilePath);
                if (!fiDta.Exists)
                {
                    Response.Redirect("ErrorPage.aspx?Err=1");
                    return;
                }
                dtaconent = "";
                // get from the generated dta file
                FileStream gfs = File.Open(dtaFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
                StreamReader gsr = new StreamReader(gfs);

                string line;
                while ((line = gsr.ReadLine()) != null)
                {
                    if (char.IsDigit(line, 0))
                    {
                        int sperator = line.IndexOf(" ");
                        string mz = line.Substring(0, sperator);
                        string intensity = line.Substring(sperator);
                        mzint mzi = new mzint();
                        mzi.mz = float.Parse(mz);
                        mzi.intensity = float.Parse(intensity);
                        mzi.bMatched = false;
                        MZS.Add(mzi);
                    }
                    dtaconent += line +"\r\n";
                }
                gsr.Close();
            }
            bFileReaded = true;
        }
        string modifiedSeq = PepSeq;
        PeptideMW PMW = new PeptideMW(PepSeq);
        float[] Bs = PMW.GetPepFragmentBValues();
        float[] Ys = PMW.GetPepFragmentYValues();

        PepSeq = ValidePepSeq(PepSeq);
        int Length = PepSeq.Length;
        // started here! 123,456,,789
        for (int i = 0; i <= Length - 1; i++)
        {
            TableRow trValues = new TableRow();
            TableCell tcAA = new TableCell();
            TableCell tcYA = new TableCell();

            float ModifiedWeight = 0.0F;


            if (HadModifed(i + 1, modifiedSeq, ref ModifiedWeight))
                tcAA.Text = string.Format("{0}<sub>{1}</sub>+{2:F2}", PepSeq.Substring(i, 1), i + 1, ModifiedWeight);
            else
                tcAA.Text = string.Format("{0}<sub>{1}</sub>", PepSeq.Substring(i, 1), i + 1);

            tcAA.HorizontalAlign = HorizontalAlign.Left;
            trValues.Cells.Add(tcAA);

            /* B format */
            float B1;
            if (i > (Length - 2))
                B1 = Bs[0];
            else
                B1 = Bs[i];
            if (i < Length - 1)
            {
                TableCell tcB1 = new TableCell();
                tcB1.Text = string.Format("{0:F2}", B1);
                if (IsMatch(B1, MZS))
                    tcB1.ForeColor = B1Color;
                tcB1.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tcB1);
                TableCell tcB2 = new TableCell();
                tcB2.Text = string.Format("{0:F2}", (B1 - OH));
                if (IsMatch(B1 - OH, MZS))
                    tcB2.ForeColor = B2Color;
                tcB2.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tcB2);
                TableCell tcB3 = new TableCell();
                tcB3.Text = string.Format("{0:F2}", (B1 - H2O));
                if (IsMatch(B1 - H2O, MZS))
                    tcB3.ForeColor = B3Color;
                tcB3.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tcB3);

                float B2 = (B1 + 1) / 2;
                TableCell tc2B = new TableCell();
                tc2B.Text = string.Format("{0:F2}", B2);
                if (IsMatch(B2, MZS))
                    tc2B.ForeColor = B1Color;
                tc2B.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tc2B);
                trValues.Height = new Unit("20px");
            }
            else
            {
                TableCell tcB1 = new TableCell();
                tcB1.Text = "";
                trValues.Cells.Add(tcB1);
                TableCell tcB2 = new TableCell();
                tcB2.Text = "";
                trValues.Cells.Add(tcB2);
                TableCell tcB3 = new TableCell();
                tcB3.Text = "";
                trValues.Cells.Add(tcB3);

                TableCell tc2B = new TableCell();
                tc2B.Text = "";
                trValues.Cells.Add(tc2B);
                trValues.Height = new Unit("20px");
            }

            /* Y format */

            float Y1;
            if (i > (Length - 2) || i == 0)
                Y1 = Ys[0];
            else
                Y1 = Ys[Length - 1 - i];
            if (i != 0)
            {
                float Y2 = (Y1 + 1) / 2;
                TableCell tc2Y = new TableCell();
                tc2Y.Text = string.Format("{0:F2}", Y2);
                if (IsMatch(Y2, MZS))
                    tc2Y.ForeColor = Y1Color;
                tc2Y.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tc2Y);

                TableCell tcY3 = new TableCell();
                tcY3.Text = string.Format("{0:F2}", (Y1 - H2O));
                if (IsMatch(Y1 - H2O, MZS))
                    tcY3.ForeColor = Y3Color;
                tcY3.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tcY3);

                TableCell tcY2 = new TableCell();
                tcY2.Text = string.Format("{0:F2}", (Y1 - OH));
                if (IsMatch(Y1 - OH, MZS))
                    tcY2.ForeColor = Y2Color;
                tcY2.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tcY2);

                TableCell tcY1 = new TableCell();
                tcY1.Text = string.Format("{0:F2}", Y1);
                if (IsMatch(Y1, MZS))
                    tcY1.ForeColor = Y1Color;
                tcY1.HorizontalAlign = HorizontalAlign.Right;
                trValues.Cells.Add(tcY1);
            }
            else
            {
                TableCell tcY1 = new TableCell();
                tcY1.Text = "";
                trValues.Cells.Add(tcY1);
                TableCell tcY2 = new TableCell();
                tcY2.Text = "";
                trValues.Cells.Add(tcY2);
                TableCell tcY3 = new TableCell();
                tcY3.Text = "";
                trValues.Cells.Add(tcY3);
                TableCell tc2Y = new TableCell();
                tc2Y.Text = "";
                trValues.Cells.Add(tc2Y);
            }
            if (HadModifed(i + 1, modifiedSeq, ref ModifiedWeight))
                tcYA.Text = string.Format("<div class=\"offset9 span3\"><span class=\"pull-left\">{0}<sub>{1}</sub>+{2:F2}</span></div>", PepSeq.Substring(i, 1), Length - i, ModifiedWeight);
            else
                tcYA.Text = string.Format("<div class=\"offset9 span3\"><span class=\"pull-left\">{0}<sub>{1}</sub></span></div>", PepSeq.Substring(i, 1), Length - i);

            tcYA.HorizontalAlign = HorizontalAlign.Right;
            trValues.Cells.Add(tcYA);

            tbTheoryValues.Rows.Add(trValues);
        }
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
    float MaxDiff = 1.1F;

    private bool IsMatch(float value, ArrayList values)
    {
        bool result = false;
        for (int i=0;i<values.Count ;i++)
        {

            if (Math.Abs(((mzint)values[i]).mz - value) < MaxDiff)
            {
                mzint newv = new mzint();
                newv.mz = ((mzint)values[i]).mz;
                newv.intensity = ((mzint)values[i]).intensity;
                newv.bMatched = true;
                values[i] = newv;
                result = true;

            }
        }
        return result;
    }

    protected void btDisplay_Click(object sender, EventArgs e)
    {
        string strFile = file_path;
        //if (cbShowNormlize.Checked)
        //    strFile = normlize_file_path;
        string strLabel = "";
        if (cbShowLabel.Checked)
            strLabel = "&Label=True";
        else
            strLabel = "&Label=False";
        string strNoise = "";
        if (cbShowNoise.Checked)
            strNoise = "&Noise=True";
        else
            strNoise = "&Noise=False";

        if (cbPrintable.Checked)
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&Printable=True&ShowTheory=True&SEQ={1}&ST={2}&SessionID={3}&TaskID={4}&pmz={5}&CS={6}&MSFile={7}", strFile, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text,MSFile )+strLabel +strNoise ;
            else
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&Printable=True&SEQ={1}&ST={2}&SessionID={3}&TaskID={4}&pmz={5}&CS={6}&MSFile={7}", strFile, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text, MSFile) + strLabel + strNoise;
        }
        else
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&ShowTheory=True&SEQ={1}&ST={2}&SessionID={3}&TaskID={4}&pmz={5}&CS={6}&MSFile={7}", strFile, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text, MSFile) + strLabel + strNoise;
            else
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&SEQ={1}&ST={2}&SessionID={3}&TaskID={4}&pmz={5}&CS={6}&MSFile={7}", strFile, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text, MSFile) + strLabel + strNoise;
        }

    }
    protected void btZoomout_Click(object sender, EventArgs e)
    {
        string strFile = file_path;
        //if (cbShowNormlize.Checked)
        //    strFile = normlize_file_path;

        string strLabel = "";
        if (cbShowLabel.Checked)
            strLabel = "&Label=True";
        else
            strLabel = "&Label=False";
        string strNoise = "";
        if (cbShowNoise.Checked)
            strNoise = "&Noise=True";
        else
            strNoise = "&Noise=False";

        if (cbPrintable.Checked)
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&Printable=True&ShowTheory=True&SEQ={4}&ST={5}&SessionID={6}&TaskID={7}&pmz={8}&CS={9}&MSFile={10}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text, MSFile) + strLabel + strNoise;
            else
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&Printable=True&SEQ={4}&ST={5}&SessionID={6}&TaskID={7}&pmz={8}&CS={9}&MSFile={10}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text, MSFile) + strLabel + strNoise;
        }
        else
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&ShowTheory=True&SEQ={4}&ST={5}&SessionID={6}&TaskID={7}&pmz={8}&CS={9}&MSFile={10}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text, MSFile) + strLabel + strNoise;
            else
                Image1.ImageUrl = string.Format("BiSpectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&SEQ={4}&ST={5}&SessionID={6}&TaskID={7}&pmz={8}&CS={9}&MSFile={10}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(CustomizedSequence.Text), SearchType, SessionID, TaskID, lbPrecursormz.Text, lbChargeState.Text, MSFile) + strLabel + strNoise;
        }
    }

    private string EscapeURL(string Src)
    {
        string result = Src.Replace("#", "%23");
        return result;
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("WikiEdit.aspx?PageName=" + mSpectrum_Seq.Trim());
    }

    private void ShowWiki(string SpectrumSEQ)
    {
        WikiPage wikiPage = WikiOperations.GetWikiPage(SpectrumSEQ.Trim());

        if (wikiPage == null)
        //{
         //   SetPageContent(wikiPage);
       // }
       // else
        {
            wikiPage = new WikiPage();
            wikiPage.PageName = SpectrumSEQ.Trim();
            wikiPage.PageContent = "There no wiki page create for this spectrum, You are welcome to creat it.";
            wikiPage.ModifiedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";
            wikiPage.LastModified = DateTime.Now;
            wikiPage.Created = DateTime.Now;
            wikiPage.IsPrivate = User.Identity.IsAuthenticated ? true : false;
            wikiPage.AllowAnonEdit = User.Identity.IsAuthenticated ? false : true;

         //   if (WikiOperations.CreateWikiPage(wikiPage) == 1)
          //  {
            //    SetPageContent(wikiPage);
           // }
           // else
           // {
           //     litContent.Text = "Error creating page";
            //}
        }
        SetPageContent(wikiPage);

    }

    private void SetPageContent(WikiPage wikiPage)
    {
        WikiTextFormatter wf = new WikiTextFormatter();

        litContent.Text = wf.FormatPageForDisplay(wikiPage.PageContent);
    }
   
}
