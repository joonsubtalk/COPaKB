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
using ZJU.COPLib;
using System.Collections;
using System.Text.RegularExpressions;
using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using System.Configuration;
using System.Data;

public partial class SpectrumInfo : System.Web.UI.Page
{
    string file_path;
    string normlize_file_path;
    string mSpectrum_Seq;
    string library_module;
    string dtaSpectra;
    List<SpectrumData> sd = new List<SpectrumData>();

    string ChargeState = "";
    string PrecursorMZ = "";

    //const
    const float OH = 17.00274F;
    const float H2O = 18.01057F;
    const float HPO3 = 79.966391F;
    const float H3PO4 = 97.976961F;
    const float Aion = 28.0F;
    float mzTolerance = 1F;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["QValue"] != null)
        {
            mSpectrum_Seq = Request.QueryString["QValue"];
            file_path = mSpectrum_Seq;
            MaxDiff = float.Parse(WebConfigurationManager.ConnectionStrings["mzTolerance"].ConnectionString);
            QuerySpectrum(mSpectrum_Seq);
            ShowWiki(mSpectrum_Seq);


            showChart(mSpectrum_Seq);
        }
        mzTolerance = float.Parse(WebConfigurationManager.ConnectionStrings["mzTolerance"].ConnectionString);
    }

    private void QuerySpectrum(string SpectrumSeq)
    {
        string strSQL = string.Format("select a.Instrumentation,a.charge_state,a.Xcorr,a.Delta_CN,a.Peptide_cop_ID,ptm_sequence,a.lib_mod,a.precur_mz,a.spectrum from spectrum_tbl a  where a.spectrum_SEQ ={0}", SpectrumSeq);
        //DBInterface.ConnectDB();
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            lbSysInfo.Text = "";
            if (result.Tables[0].Rows.Count > 0)
            {
                if (!result.Tables[0].Rows[0].IsNull(0))
                    lbInstrumentation.Text = result.Tables[0].Rows[0].ItemArray[0].ToString();
                if (!result.Tables[0].Rows[0].IsNull(1))
                    lbChargeState.Text = result.Tables[0].Rows[0].ItemArray[1].ToString();
                if (!result.Tables[0].Rows[0].IsNull(2))
                    lbXcorr.Text = result.Tables[0].Rows[0].ItemArray[2].ToString();
                if (!result.Tables[0].Rows[0].IsNull(3))
                    lbDeltaCN.Text = result.Tables[0].Rows[0].ItemArray[3].ToString();
                string peptideID = result.Tables[0].Rows[0].ItemArray[4].ToString();
                lbPeptideCOPID.Text = string.Format("<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>", peptideID, peptideID);
                //int File_index =int.Parse (((System.Data.OracleClient.OracleDataReader) result).GetOracleValue(5).ToString() ) ; 
                //string reletive_path = result.GetString (7);
                //string File_Index = result.Tables[0].Rows[0].ItemArray[5].ToString() ;
                library_module = result.Tables[0].Rows[0].ItemArray[6].ToString();
                //string relative_path = ConfigurationSettings.AppSettings[library_module ];// +"BestSpectra\\";
                string precursormz = string.Format("{0:F1}", result.Tables[0].Rows[0].ItemArray[7]);
                this.lbPrecursormz.Text = precursormz;
                //file_path = string.Format("{0}{1}", relative_path, File_Index);
                //file_path = string.Format("{0}{1:D5}.dta", WebConfigurationManager.ConnectionStrings["SPECTRUM"].ConnectionString+reletive_path , File_index );
                //normlize_file_path = string.Format("{0}{1:D5}.dta", WebConfigurationManager.ConnectionStrings["NORMALIZESPECTRUM"].ConnectionString+reletive_path , File_index);
                string Sequence = result.Tables[0].Rows[0].ItemArray[5].ToString();
                dtaSpectra = result.Tables[0].Rows[0].ItemArray[8].ToString();
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&SEQ={1}&CS={2}&pmz={3}", SpectrumSeq, EscapeURL(Sequence), lbChargeState.Text, precursormz);
                lbModifiedSequence.Text = Sequence;
                lbSysInfo.Text = "";

                //EscapeURL(lbModifiedSequence.Text);
                ChargeState = lbChargeState.Text;
                PrecursorMZ = lbPrecursormz.Text;

            }
            else
            {
                lbInstrumentation.Text = "";
                lbChargeState.Text = "";
                lbXcorr.Text = "";
                lbDeltaCN.Text = "";
                lbPeptideCOPID.Text = "";
                Image1.ImageUrl = "";
                lbSysInfo.Text = string.Format("No Spectrum with SEQ {0} was found.", SpectrumSeq);
            }
            //result.Close();
            //result.Dispose();
        }

        //DBInterface.CloseDB();


        ShowTheoryValues(lbModifiedSequence.Text);
        ComputeTotalMatch();
        ShowContributorInof();
        DrawData(lbModifiedSequence.Text);
        
        removeDuplicateTips(sd); // gets rid of the duplicates of the "tip" values such that the highest value intensity remains.
        
        int kk = 0;
        foreach (SpectrumData s in sd) // Loop through List with foreach
        {
            console.Text += "{x:" + s.getMZ() + ",\t y: " + s.getIntensity() + ",\t color: '" + s.getColor() + "',\t name: '" + s.getTip() + "'},\r\n<br />";
            kk++;
        }

    }

    private void showChart(String seq)
    {
        string strSQL = string.Format("select spectrum from spectrum_tbl where spectrum_seq={0}", seq);
        DataSet result = DBInterface.QuerySQL2(strSQL);
        string lines = "";
        if (result != null)
        {

            if (result.Tables[0].Rows.Count > 0)
            {
                lines = result.Tables[0].Rows[0].ItemArray[0].ToString();
            }
        }


    }
    private string setupData(string data, int type)
    {
        data = "[" + data;
        data = data.Replace("\n", "],\n[");
        data = data.Replace(" ", ", ");
        data = data.TrimEnd('[');
        data = data.Trim();
        data = data.TrimEnd(',');
        if (type != 0) //
            data = "['m/z','Relative Abundance']," + data;
        data = "[" + data + "]";
        return data;
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
    private void ShowTheoryValues(string PepSeq)
    {
        //PepSeq = ValidePepSeq(PepSeq);
        if (dtaSpectra != "")
        {
            string[] lines = dtaSpectra.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (char.IsDigit(line, 0))
                {
                    string[] tokens = line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    string mz = tokens[0];
                    string intensity = tokens[1];// line.Substring(sperator);
                    SpectrumData s = new SpectrumData(float.Parse(tokens[0]), float.Parse(tokens[1]));
                    sd.Add(s);
                    mzint mzi = new mzint();
                    mzi.mz = float.Parse(mz);
                    mzi.intensity = float.Parse(intensity);
                    mzi.bMatched = false;
                    MZS.Add(mzi);
                }
            }
            bFileReaded = true;
        }
        
        string modifiedSeq = PepSeq;
        PeptideMW PMW = new PeptideMW(PepSeq);
        float[] Bs = PMW.GetPepFragmentBValues();
        float[] Ys = PMW.GetPepFragmentYValues();

        PepSeq = ValidePepSeq(PepSeq);
        int Length = PepSeq.Length;
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
                console.Text += "";
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
    float MaxDiff = 0.5F;

    private bool IsMatch(float value, ArrayList values)
    {

        foreach (object v in values)
        {

            if (Math.Abs(((mzint)v).mz - value) < MaxDiff)
            {
                mzint newv = new mzint();
                newv.mz = ((mzint)v).mz;
                newv.intensity = ((mzint)v).intensity;
                newv.bMatched = true;
                values[values.IndexOf(v)] = newv;
                return true;

            }
        }
        return false;
    }

    protected void btDisplay_Click(object sender, EventArgs e)
    {
        string strFile = file_path;
        //if (cbShowNormlize.Checked)
        //    strFile = normlize_file_path;
        if (cbPrintable.Checked)
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&Printable=False&ShowTheory=True&SEQ={1}&CS={2}&pmz={3}", strFile, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
            else
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&Printable=False&SEQ={1}&CS={2}&pmz={3}", strFile, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
        }
        else
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&ShowTheory=True&SEQ={1}&CS={2}&pmz={3}", strFile, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
            else
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&SEQ={1}&CS={2}&pmz={3}", strFile, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
        }

    }
    protected void btZoomout_Click(object sender, EventArgs e)
    {
        string strFile = file_path;
        //if (cbShowNormlize.Checked)
        //    strFile = normlize_file_path;
        if (cbPrintable.Checked)
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&Printable=False&ShowTheory=True&SEQ={4}&CS={5}&pmz={6}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
            else
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&Printable=False&SEQ={4}&CS={5}&pmz={6}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
        }
        else
        {
            if (cbShowTheorySpectrum.Checked)
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&ShowTheory=True&SEQ={4}&CS={5}&pmz={6}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
            else
                Image1.ImageUrl = string.Format("Spectrum.aspx?file={0}&mode=ZoomOut&xmin={1}&xmax={2}&ymax={3}&SEQ={4}&CS={5}&pmz={6}", strFile, tbXmin.Text, tbXmax.Text, tbYmax.Text, EscapeURL(lbModifiedSequence.Text), lbChargeState.Text, lbPrecursormz.Text);
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

        /*if (wikiPage != null)
        {
            SetPageContent(wikiPage);
        }
        else*/
        if (wikiPage == null)
        {
            wikiPage = new WikiPage();
            wikiPage.PageName = SpectrumSEQ.Trim();
            wikiPage.PageContent = "There is no Wiki page on record for this spectrum, please feel free to create one.";
            wikiPage.ModifiedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";
            wikiPage.LastModified = DateTime.Now;
            wikiPage.Created = DateTime.Now;
            wikiPage.IsPrivate = User.Identity.IsAuthenticated ? true : false;
            wikiPage.AllowAnonEdit = User.Identity.IsAuthenticated ? false : true;

            /*if (WikiOperations.CreateWikiPage(wikiPage) == 1)
            {
                SetPageContent(wikiPage);
            }
            else
            {
                litContent.Text = "Error creating page";
            }*/
        }
        SetPageContent(wikiPage);

    }

    private void SetPageContent(WikiPage wikiPage)
    {
        WikiTextFormatter wf = new WikiTextFormatter();
        litContent.Text = wf.FormatPageForDisplay(wikiPage.PageContent);
        if (DateTime.Now != wikiPage.Created)
            lastMod.Text = wf.FormatPageForDisplay("<div class=\"rowfluid\"><div class=\"span12\">This wiki was last modified by " + wikiPage.ModifiedBy + " on " + wikiPage.LastModified + "</div></div>");
    }

    /*cmon baby!*/

    private string GetAnnotation(List<SpectrumData> Spectra, int pos, float[] Bs, float[] Ys, float PreMZ, int charge, bool bPhosphorylation)
    {
        if (!IsPeak(Spectra, pos))
            return "";
        float mz = ((SpectrumData)Spectra[pos]).mz;
        return GetAnnotation(mz, Bs, Ys, PreMZ, charge, bPhosphorylation);
    }

    // check thru list of spectrum to see if there's a matching tip, if so demote the lowest intensity value
    private void removeDuplicateTips(List<SpectrumData> Spectra)
    {
        for (int i = 0; i < Spectra.Count-1; i++)
        {
            // found a matching spectrum title
            if (Spectra[i].getTip() == Spectra[i + 1].getTip())
            {
                // start process of demoting the one with smaller intensity
                float max = Math.Max(Spectra[i].getIntensity(), Spectra[i].getIntensity());
                if (max == Spectra[i].getIntensity()) // 1st was highest
                {
                    Spectra[i + 1].setColor("#000000");
                    Spectra[i + 1].setTip("");
                }
                else if (max == Spectra[i+1].getIntensity()) //2nd was highest
                {
                    Spectra[i].setColor("#000000");
                    Spectra[i].setTip("");
                }

            }

        }
    }

    private bool IsPeak(List<SpectrumData> Spectra, int pos)
    {
        bool result = true;
        float posmz = ((SpectrumData)Spectra[pos]).mz;
        float posintensity = ((SpectrumData)Spectra[pos]).intensity;
        int i = pos - 1;
        int j = pos + 1;
        ///                    pos
        ///                    ij
        ///                   i   j
        ///                 i        j
        ///        0                                count
        while (i > 0 && j < Spectra.Count)
        {
            float ipos = ((SpectrumData)Spectra[i]).mz;
            float jpos = ((SpectrumData)Spectra[j]).mz;

            if (posmz - ipos > 0.9 && jpos - posmz > 0.9)
                break;
            float jintensity = ((SpectrumData)Spectra[j]).intensity;
            float iintensity = ((SpectrumData)Spectra[i]).intensity;
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
                    Ions.Add((B[i] + z - 1) / z, string.Format("(b{0})+{1}", i + 1, z));
                if (!Ions.Contains((B[i] - OH + z - 1) / z))
                    Ions.Add((B[i] - OH + z - 1) / z, string.Format("(b{0}-NH)+{1}", i + 1, z));
                if (!Ions.Contains((B[i] - H2O + z - 1) / z))
                    Ions.Add((B[i] - H2O + z - 1) / z, string.Format("(b{0}-H2O)+{1}", i + 1, z));
                if (!Ions.Contains((B[i] - Aion + z - 1) / z))
                    Ions.Add((B[i] - Aion + z - 1) / z, string.Format("(a{0})+{1}", i + 1, z));
            }
        }

        for (int i = 0; i < Y.Length; i++)
        {
            for (int z = 1; z <= charge; z++)
            {
                if (!Ions.Contains((Y[i] + z - 1) / z))
                    Ions.Add((Y[i] + z - 1) / z, string.Format("(y{0})+{1}", i + 1, z));
                if (!Ions.Contains((Y[i] - OH + z - 1) / z))
                    Ions.Add((Y[i] - OH + z - 1) / z, string.Format("(y{0}-NH)+{1}", i + 1, z));
                if (!Ions.Contains((Y[i] - H2O + z - 1) / z))
                    Ions.Add((Y[i] - H2O + z - 1) / z, string.Format("(y{0}-H2O)+{1}", i + 1, z));
            }
        }

        string annotation = "";
        IsMatch(mz, Ions, ref annotation);
        return annotation;
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
            if (ionsname.Contains("NH") || ionsname.Contains("H2O"))
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

    // REAL MAGIC HERE! y- B-
    private void DrawData(String pepseq)
    {

        PeptideMW PMW = new PeptideMW(pepseq);
        float[] Bs = PMW.GetPepFragmentBValues();
        float[] Ys = PMW.GetPepFragmentYValues();
        bool bPhos = PMW.IsPhosphorylation();
        int Count = sd.Count;
        int i = 0;
        for (i = 0; i < Count; i++)
        {

            float mz = ((SpectrumData)sd[i]).mz;
            float intensity = ((SpectrumData)sd[i]).intensity;

            /*
            Pen dataPen = new Pen(Brushes.Black, 1);
            Pen BLinePen = new Pen(Brushes.Blue, 2);
            Pen YLinePen = new Pen(Brushes.Red, 2);
            Pen ALinePen = new Pen(Brushes.Green, 2);
            Pen MLinePen = new Pen(Brushes.Gray, 2);
            Font Numberfont = new Font("Arial", 9, FontStyle.Regular);*/

            string strAnn = GetAnnotation(sd, i, Bs, Ys, float.Parse(PrecursorMZ), int.Parse(ChargeState), bPhos);
            
            if (strAnn.StartsWith("(b"))
            {
                sd[i].setColor("#0000ff");
                sd[i].setTip(labelChargeCount(stripParenth(strAnn)));
                //g.DrawLine(BLinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
                //g.DrawString(strAnn, Numberfont, Brushes.Blue, new PointF(x, y));
            }
            else if (strAnn.StartsWith("(y"))
            {
                sd[i].setColor("#ff0000");
                sd[i].setTip(labelChargeCount(stripParenth(strAnn)));
                //g.DrawLine(YLinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
                //g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y));
            }
            else if (strAnn.StartsWith("(a"))
            {
                sd[i].setColor("#00ff00");
                sd[i].setTip(labelChargeCount(stripParenth(strAnn)));
                //g.DrawLine(ALinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
                //g.DrawString(strAnn, Numberfont, Brushes.Green, new PointF(x, y));
            }
            else if (strAnn.StartsWith("(M"))
            {
                sd[i].setColor("#cccccc");
                sd[i].setTip(labelChargeCount(stripParenth(strAnn)));
                //g.DrawLine(MLinePen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
                //g.DrawString(strAnn, Numberfont, Brushes.Gray, new PointF(x, y));
            }
            else
            {
                sd[i].setColor("#000000");
                //g.DrawLine(dataPen, x, y, x, (float)HEIGHT - NETAREABOTTOMMARGIN);
            }
            //g.DrawString(strAnn, Numberfont, Brushes.Red, new PointF(x, y));

            /*
            if (intensity == MaxIntensitiy)
            {
                //peak value point
                GraphicsPath p = new GraphicsPath();
                p.AddLine(x, (float)HEIGHT - NETAREABOTTOMMARGIN, x + XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength);
                p.AddLine(x + XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength, x - XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN + XAxisScaleLength);
                p.CloseFigure();
                g.FillPath(Brushes.Red, p);

                g.DrawString(mz.ToString(), Numberfont, Brushes.Red, x + XAxisScaleLength, (float)HEIGHT - NETAREABOTTOMMARGIN);
            }*/

        }
    }

    private string stripParenth(string str)
    {
        return Regex.Replace(str, "[()]", "");
    }

    private string labelChargeCount(string label){
        int position = label.LastIndexOf('+');
        int chargeCnt = int.Parse(label.Substring(position + 1, 1));
        string s = label.Substring(0,position);
        int c = 0;
        while (c < chargeCnt)
        {
            s += "+";
            c++;
        }
        return s;
    }
}
