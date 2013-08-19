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
using ZJU.COPDB;
using System.Xml;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Data.Common;
using ZJU.COPLib;

public partial class Report : System.Web.UI.Page
{
    string TaskID;
    protected void Page_Load(object sender, EventArgs e)
    {
        TaskID = Request.QueryString["TaskID"];
        if (TaskID == null)
            TaskID = (string)Session["TaskID"];
        string ViewType = Request.QueryString["Type"];
        if (!this.IsPostBack)
        {

            if (ViewType == null)
            {
                ViewType = "Protein";
            }
            ShowProteinReportTree(TaskID, ViewType);
        }
    }

    string GetTheDescriptionofSC(string SymbolSC)
    {
        string[] tokens = SymbolSC.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        string Result = "";
        if (tokens[0] == "1")
            Result = string.Format("Rapid Searching and filter with similarity > {0}", tokens[1]);
        else
        {
            Result = "Customized Searching Mode with filter with ";
            for (int i = 1; i < tokens.Count(); i++)
            {
                string[] filterpair = tokens[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                Result += string.Format("( Similarity > {0} and Final Score > {1} )", filterpair[0], filterpair[1]);
            }

        }
        return Result;

    }

    string strReportFile = "";

    void ShowProteinReportTree(string TaskID, string ViewType)
    {
        //this.lbSaveReport.Text = string.Format("<a href='SaveReport.aspx?Result={0}' Target='_blank'>  Save this report</a>", TaskID);
        //string strSQL = "select task_seq,task_user,upload_time,task_status,upload_filename, search_model,report_filename from search_task where task_seq={0} ";
        //strSQL = string.Format(strSQL, TaskID);
        //DBInterface.ConnectDB();
        //System.Data.Common.DbDataReader result = DBInterface.QuerySQL(strSQL);
        //if (result != null)
        //{
        //    result.Read();
        strReportFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xml";
        string strReportFile2 = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xls";

        this.lbSave.Text = string.Format("Save this report<a href='SaveReport.aspx?Report={0}&ReportType=XML' Target='_blank'> COPa Client Format(*.cpr) </a> or <a href='SaveReport.aspx?Report={1}&ReportType=XLS' Target='_blank'> Excel Format(*.xls) </a>", strReportFile, strReportFile2);

        string FreqFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.Freq";
        if (File.Exists(FreqFile))
        {
            Image1.ImageUrl = string.Format("StatisticImage.aspx?file={0}", FreqFile);
            Panel1.Visible = true;
            //multiuser-fridendly streamreader 
            try
            {
                FileStream fs = File.Open(FreqFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                string line;
                if ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("H"))
                    {
                        string[] tokens = line.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (!this.IsPostBack)
                            tbThreshold.Text = tokens[5];
                    }
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                DBInterface.LogEvent(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
        }
        else
        { 
            Panel1.Visible = false;
        }

        if (!File.Exists(strReportFile))
        {
            Response.Redirect("ErrorPage.aspx?Err=2");
            return;
        }
        //result.Close();
        LoadSearchingCondition(TaskID);

        if (ViewType == "Protein")
        {
            lbGeneSymbol.Text = string.Format("<a href=Report.aspx?TaskId={0}&Type=Gene>Gene Symbol View</a>", TaskID);
            LoadProteinAccessionView(strReportFile);
        }
        else
        {
            lbProteinAccession.Text = string.Format("<a href=Report.aspx?TaskId={0}&Type=Protein>Protein Accession View</a>", TaskID);
            lbProteinAccession.BackColor = System.Drawing.Color.White;
            lbProteinAccession.BorderStyle = BorderStyle.Dashed;
            lbGeneSymbol.BackColor = System.Drawing.ColorTranslator.FromHtml("#99CCFF"); //System.Drawing.Color.Wheat;
            lbGeneSymbol.BorderStyle = BorderStyle.None;
            LoadGeneSymbolView(strReportFile);
        }


        //    if (xr.IsStartElement() && Proteins == xr.Name)
        //    {
        //        string ProteinText = string.Format("{0} {1}", xr[0], xr[2]);
        //        TreeNode Protein = new TreeNode(ProteinText, xr[1]);
        //        Protein.ImageUrl = "_image\\protein.gif";
        //        Protein.NavigateUrl = string.Format("javascript:mySpl.loadPage('RightContent', 'ProteinInfo.aspx?QType=Protein ID&QValue={0}')", xr[1]);
        //        this.TreeView1.Nodes[0].ChildNodes.Add(Protein);

        //    }
        //    if (xr.IsStartElement() && ScanPeptide == xr.Name)
        //    {
        //        string PeptideID = xr["Peptide"];
        //        string score = xr["MatchScore"];
        //        string SpectrumSeq = xr["Spectrum"];
        //        string Scan = xr["Scan"];
        //        string peptideSequence = xr["PeptideSequence"];

        //        string ScanText = string.Format("Scan:{0} Sequence:{1} Score:{2}", Scan, peptideSequence, score);
        //        TreeNode ScanNode = new TreeNode(ScanText, xr[3]);
        //        ScanNode.ImageUrl = "_image\\spectrum.gif";
        //        ScanNode.NavigateUrl = string.Format("javascript:mySpl.loadPage('RightContent', 'MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}')", SpectrumSeq, Scan, TaskID);

        //        this.TreeView1.Nodes[0].ChildNodes[this.TreeView1.Nodes[0].ChildNodes.Count - 1].ChildNodes.Add(ScanNode);
        //    }
        //}
        //this.TreeView1.Nodes[0].CollapseAll();
        //this.TreeView1.Nodes[0].Expand();
        // }
    }
    private void LoadGeneSymbolView(string strReportFile)
    {
        DataTable dtScans = new DataTable();
        dtScans.Columns.Add("Sequence");
        //dtScans.Columns.Add("ModifiedType");
        //dtScans.Columns.Add("FinalScore", typeof(float));
        //dtScans.Columns.Add("DetaM", typeof(float));
        dtScans.Columns.Add("mzFile");
        dtScans.Columns.Add("SpectraIndex");
        //dtScans.Columns.Add("Similary_Score", typeof(float));
        dtScans.Columns.Add("Spectrum");
        dtScans.Columns.Add("COPa_Peptide_ID");
        dtScans.Columns.Add("IPI");
        //dtScans.Columns.Add("Unique Peptide");
        dtScans.Columns.Add("Gene_Symbol");
        //dtScans.Columns.Add("Feature Peptide");
        //dtScans.Columns.Add("Charge");
        //dtScans.Columns.Add("Precursor MZ");
        //dtScans.Columns.Add("Theoretic Precursor MZ");

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        settings.IgnoreWhitespace = true;
        settings.IgnoreComments = true;
        Hashtable GeneList = new Hashtable();
        XmlReader xr = XmlReader.Create(strReportFile, settings);
        //object SearchInfo = xr.NameTable.Add("COPaReport");
        object BasicInfo = xr.NameTable.Add("COPaReport");
        object Proteins = xr.NameTable.Add("Proteins");
        object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
        int ProteinNumber = 0;
        try
        {
            while (xr.Read())
            {
                if (xr.IsStartElement() && BasicInfo == xr.Name)
                {
                    string files = xr["mzFile"].Replace("\r\n", "<br/>");
                    this.lbmzML.Text = files.Replace(" ", "<br/>");
                    this.lbSC.Text = xr["SearchFilter"];
                    this.lbSearchingModule.Text = xr["LibModule"];
                }
                if (xr.IsStartElement() && Proteins == xr.Name)
                {
                    ProteinNumber++;
                    string strIPI = xr["COPaID"];
                    string strGeneSymbol = xr["GeneSymbol"];
                    if (!GeneList.Contains(strGeneSymbol) && strGeneSymbol != "")
                        GeneList.Add(strGeneSymbol, 0);
                    while (xr.Read())//get the child tables
                    {
                        if (xr.IsStartElement() && ScanPeptide == xr.Name)
                        {
                            dtScans.Rows.Add(new object[] { xr["PeptideSequence"], xr["mzFile"], xr["Scan"], xr["Spectrum"], xr["Peptide"], strIPI, strGeneSymbol });

                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("ErrorPage.aspx?Err=2");
            return;
        }

        this.lbProteinCount.Text = string.Format("{0} distinct genes {1}", ProteinNumber, GeneList.Count);
        DataTable dtProteinScans = new DataTable();
        dtProteinScans.Columns.Add("Sequence");
        dtProteinScans.Columns.Add("mzFile");
        dtProteinScans.Columns.Add("SpectraIndex");
        dtProteinScans.Columns.Add("Spectrum");
        dtProteinScans.Columns.Add("COPa_Peptide_ID");
        dtProteinScans.Columns.Add("IPI");
        dtProteinScans.Columns.Add("Gene_Symbol");
        Dictionary<string, float> ProteinQVs = new Dictionary<string, float>();
        float totalValue = 0;
        float maxValue = 0;
        foreach (string GeneSymbol in GeneList.Keys)
        {
            DataRow[] ScanResults = dtScans.Select(string.Format("Gene_Symbol='{0}'", GeneSymbol));
            dtProteinScans.Rows.Clear();
            foreach (DataRow scan in ScanResults)
            {
                dtProteinScans.ImportRow(scan);
            }
            dtProteinScans = dtProteinScans.DefaultView.ToTable(true, new string[] { "Sequence", "mzFile", "SpectraIndex", "Spectrum", "COPa_Peptide_ID", "Gene_Symbol" });

            DataTable dtSequences = dtProteinScans.DefaultView.ToTable(true, new string[] { "COPa_Peptide_ID" });
            float geneRatioValue = (float)dtProteinScans.Rows.Count / (float)dtSequences.Rows.Count;
            ProteinQVs.Add(GeneSymbol, geneRatioValue);
            totalValue += geneRatioValue;
            if (geneRatioValue > maxValue)
                maxValue = geneRatioValue;
        }

        string ProIPIURL = "<a href='ProteinList.aspx?T=Gene+Symbol&V={0}' Target='_Blank'>{1}</a>";
        string ProgresText = "<div class=\"graph\"><strong class=\"bar\" style=\"width: {0}%;\">{1}</strong></div>";
        var sortedDict = (from entry in ProteinQVs orderby entry.Value descending select entry);
        int count = sortedDict.Count();
        //string ProteinLists = "";
        string ProteinList = "";
        string ProteinQuantitationLists = "";
        for (int i = 0; i < count; i++)
        {
            KeyValuePair<string, float> ipiv = sortedDict.ElementAt(i);
            TableRow tr = new TableRow();
            TableCell tcIPI = new TableCell();
            if (ipiv.Key.Length <= 20)
                tcIPI.Text = string.Format(ProIPIURL, ipiv.Key, ipiv.Key);
            else
                tcIPI.Text = string.Format(ProIPIURL, ipiv.Key, ipiv.Key.Substring(0, 20) + "...");
            if (ProteinList.Length < 2000)
                ProteinList += "|" + ipiv.Key;
            if (ProteinQuantitationLists.Length < 2000)
                ProteinQuantitationLists += string.Format("|{0};{1}", ipiv.Key, ipiv.Value / totalValue);
            tcIPI.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tcIPI);

            TableCell tcProgress = new TableCell();
            tcProgress.Text = string.Format(ProgresText, (int)((float)ipiv.Value * 100 / maxValue), (float)ipiv.Value / totalValue);
            tcIPI.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tcProgress);
            tbProteins.Rows.Add(tr);
        }

        lbShowInteract.Text = string.Format("<a href='IntActReport.aspx?PType=GeneName&PLists={0}' Target='_Blank' >Interactome</a>", ProteinList);
        lbGO.Text = string.Format("<a href='GO.aspx?PType=GeneName&Quant=True&PLists={0}' Target='_Black'>GO Annotation</a>", ProteinQuantitationLists);
    }

    private void LoadProteinAccessionView(string strReportFile)
    {

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        settings.IgnoreWhitespace = true;
        settings.IgnoreComments = true;

        XmlReader xr = XmlReader.Create(strReportFile, settings);
        object BasicInfo = xr.NameTable.Add("COPaReport");
        object Proteins = xr.NameTable.Add("Proteins");
        object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
        string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
        string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
        Dictionary<string, float> ProteinQVs = new Dictionary<string, float>();
        float maxValue = 0;
        float totalValue = 0;
        int ProteinNumber = 0;
        while (xr.Read())
        {
            if (xr.IsStartElement() && BasicInfo == xr.Name)
            {
                string files = xr["mzFile"].Replace("\r\n", "<br/>");
                this.lbmzML.Text = files.Replace(" ", "<br/>");

                this.lbSC.Text = xr["SearchFilter"];
                this.lbSearchingModule.Text = xr["LibModule"];
            }
            if (xr.IsStartElement() && Proteins == xr.Name)
            {
                string ProteinText = xr["IPI"];
                float QuantitateValue = float.Parse(xr["NormalizCount"]);
                ProteinQVs.Add(ProteinText, QuantitateValue);
                totalValue += QuantitateValue;
                if (QuantitateValue > maxValue)
                    maxValue = QuantitateValue;

                ProteinNumber++;

            }
        }
        xr.Close();
        this.lbProteinCount.Text = ProteinNumber.ToString();

        string ProIPIURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}' Target='_Blank'>{1}</a>";
        string ProgresText = "<div class=\"graph\"><strong class=\"bar\" style=\"width: {0}%;\">{1}</strong></div>";
        var sortedDict = (from entry in ProteinQVs orderby entry.Value descending select entry);
        int count = sortedDict.Count();
        string ProteinLists = "";
        string ProteinQuantitationLists = "";
        for (int i = 0; i < count; i++)
        {
            KeyValuePair<string, float> ipiv = sortedDict.ElementAt(i);
            TableRow tr = new TableRow();
            TableCell tcIPI = new TableCell();
            tcIPI.Text = string.Format(ProIPIURL, ipiv.Key, ipiv.Key);
            if (ProteinLists.Length < 2000)
                ProteinLists += "|" + ipiv.Key;
            if (ProteinQuantitationLists.Length < 2000)
                ProteinQuantitationLists += string.Format("|{0};{1}", ipiv.Key, ipiv.Value / totalValue);
            tcIPI.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tcIPI);
            TableCell tcProgress = new TableCell();
            tcProgress.Text = string.Format(ProgresText, (int)(ipiv.Value * 100 / maxValue), ipiv.Value / totalValue);
            tcIPI.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tcProgress);
            tbProteins.Rows.Add(tr);
        }

        lbShowInteract.Text = string.Format("<a href='IntActReport.aspx?PType=UNIPROT&PLists={0}' Target='_Blank'>Interactome</a>", ProteinLists);
        lbGO.Text = string.Format("<a href='GO.aspx?PType=UNIPROT&Quant=True&PLists={0}' Target='_Black'>GO Annotation</a>", ProteinQuantitationLists);
    }

    protected void btRestThreshold_Click(object sender, EventArgs e)
    {
        ResetReport();
    }
    private void ResetReport()
    {
        //string strOldFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt";
        //string strBackFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.back";
        //string strFreqFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.Freq";
        float threshold = 0.0f;
        try
        {
            threshold = float.Parse(this.tbThreshold.Text);
            if (threshold < 0 || threshold > 100)
            {
                lbMessage.Text = "Please input a number between 1 and 100.";
                return;
            }
        }
        catch (Exception ex)
        {
            lbMessage.Text = "Please input a number.";
            return;
        }
        COPaWS.COPaWS copaws = new COPaWS.COPaWS();
        if (copaws.ChangeThreshold(TaskID, this.lbmzML.Text.Replace("<br/>", "\r\n"), threshold))
            Response.Redirect(string.Format("Report.aspx?TaskID={0}", TaskID));
        // string strNewFreqFile = strFreqFile + ".back";
        // FileInfo fi = new FileInfo(strFreqFile);
        // fi.CopyTo(strNewFreqFile,true );
        // StreamWriter freqsw = new StreamWriter(strFreqFile, false);
        // StreamReader freqsr = new StreamReader(strNewFreqFile);
        // string line = "";
        // float newPossiblity = 0.0f;
        // try
        // {
        //     while ((line = freqsr.ReadLine()) != null)
        //     {
        //         if (line.StartsWith("H"))
        //         {
        //             string[] tokens = line.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        //             string miu1 = tokens[1];
        //             string sigma1 = tokens[2];
        //             string miu2 = tokens[3];
        //             string sigma2 = tokens[4];
        //             string strthreshold = threshold.ToString();
        //             string falseNumber = tokens[6];
        //             string trueNumber = tokens[7];
        //             freqsw.WriteLine(string.Format("H|{0}|{1}|{2}|{3}|{4}|{5}|{6}|", miu1, sigma1, miu2, sigma2, strthreshold, falseNumber, trueNumber));
        //             newPossiblity = getPossiblity(miu1, sigma1, miu2, sigma2, falseNumber, trueNumber, threshold);

        //         }
        //         else
        //             freqsw.WriteLine(line);
        //     }
        // }
        // catch (Exception ex)
        // {
        //     DBInterface.LogEvent(ex.ToString(),System.Diagnostics.EventLogEntryType.Error );
        // }
        // finally
        // {

        //     freqsw.Close();
        //     freqsr.Close();
        // }
        // //delte the old file
        // StreamWriter sw = new StreamWriter(strOldFile, false);
        // StreamReader sr = new StreamReader(strBackFile);
        // try
        // {
        //     while ((line = sr.ReadLine()) != null)
        //     {
        //         string[] tokens = line.Split(new string[] { "|" }, StringSplitOptions.None);
        //         if (tokens[0] == "Y")
        //         {
        //             string peptideID = tokens[2];
        //             if ((float.Parse(tokens[10])) * 100 >= threshold)
        //             {
        //                 sw.WriteLine(line);
        //             }

        //         }
        //     }
        // }
        // catch (Exception ex)
        // {
        //     DBInterface.LogEvent(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
        // }
        // finally
        // {
        //     sr.Close();
        //     sw.Close();
        // }
        //string sc = string.Format("Threshold set at {0}, peptide accuracy rate is {1}%.", threshold/100,newPossiblity ); 
        //int NeedDistinctPeptide = UpdateSearchConditionFile(TaskID, newPossiblity);
        //CreateReport(TaskID, this.lbmzML.Text.Replace("<br/>", "\r\n"), this.lbSearchingModule.Text, sc,NeedDistinctPeptide );



    }

    void UpdateSearchingCondition(string TaskID, float confidence, int distinctPeptides, int UniquePeptides)
    {
        string scLocation = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\searchcondition.xml";
        FileInfo fi = new FileInfo(scLocation);

        if (fi.Exists)
        {
            SearchingParameters mySP = new SearchingParameters(scLocation);
            mySP.ConfidenceLevel = confidence;
            mySP.DistinctPeptide = distinctPeptides;
            mySP.UniquePeptide = UniquePeptides;
            mySP.SaveSettings();
        }
        COPaWS.COPaWS copaws = new COPaWS.COPaWS();
        if (copaws.ChangeCreteria(TaskID, this.lbmzML.Text.Replace("<br/>", "\r\n")))
            Response.Redirect(string.Format("Report.aspx?TaskID={0}", TaskID));

    }

    void LoadSearchingCondition(string TaskID)
    {

        string scLocation = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\searchcondition.xml";
        FileInfo fi = new FileInfo(scLocation);

        if (fi.Exists)
        {
            SearchingParameters mySP = new SearchingParameters(scLocation);
            if (mySP.HighResolution)
                this.lbPeptideTolerance.Text = string.Format("{0}ppm consider {1} isotope peaks", mySP.PeptideToleranceHR, mySP.IsotopePeaks);
            else
                this.lbPeptideTolerance.Text = mySP.Peptidetolerance.ToString() + "Th (Da/e)";
            this.lbSlideSize.Text = mySP.SlideSize.ToString();
            if (mySP.UseNoiseLibrary)
                this.lbIsUsingNoiseDecoy.Text = "True";
            else
                this.lbIsUsingNoiseDecoy.Text = "False";
            this.lbPTMShift.Text = mySP.PTMShift.ToString();
            if (mySP.BonusDetaMz)
                this.lbBonusms.Text = "True";
            else
                this.lbBonusms.Text = "False";

            if (mySP.UseStatisticalMode)
            {
                this.lbThreshold.Text = string.Format("{0}%", mySP.ConfidenceLevel * 100);
            }
            else
            {

                this.lbThreshold.Text = mySP.ScoreCriteria;

            }
            this.lbPeptideNumber.Text = mySP.DistinctPeptide.ToString();
            this.lbUniquePeptide.Text = mySP.UniquePeptide.ToString();
        }


    }

    int UpdateSearchConditionFile(string TaskID, float newPossibility)
    {
        string scLocation = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\searchcondition.xml";
        FileInfo fi = new FileInfo(scLocation);

        if (fi.Exists)
        {
            SearchingParameters mySP = new SearchingParameters(scLocation);
            mySP.ConfidenceLevel = newPossibility / 100;
            mySP.SaveSettings(scLocation);
            return mySP.DistinctPeptide;
        }
        return 1;

    }


    float getPossiblity(string strmiu1, string strsigma1, string strmiu2, string strsigma2, string strfalsenumber, string strtruenumber, float threshold)
    {
        float miu1 = float.Parse(strmiu1);
        float sigma1 = float.Parse(strsigma1);
        float miu2 = float.Parse(strmiu2);
        float sigma2 = float.Parse(strsigma2);
        float falseNumber = float.Parse(strfalsenumber);
        float trueNumber = float.Parse(strtruenumber);

        float Freq = (float)(falseNumber * Math.Exp((-1 * (threshold - miu1) * (threshold - miu1)) / (2 * sigma1 * sigma1)) / (Math.Sqrt(2 * Math.PI) * sigma1));
        float Freq2 = (float)(trueNumber * Math.Exp((-1 * (threshold - miu2) * (threshold - miu2)) / (2 * sigma2 * sigma2)) / (Math.Sqrt(2 * Math.PI) * sigma2));
        float possible = Freq2 * 100 / (Freq + Freq2);
        return possible;
    }

    public bool CreateReport(string TaskID, string FileName, string searchModule, string sc, int distinctPeptides)
    {
        //string strReportName = FileName.Substring(0, FileName.LastIndexOf(".")) + ".txt";
        Dictionary<string, ProteinInfo> Proteins = new Dictionary<string, ProteinInfo>();
        string strReportFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt";
        if (File.Exists(strReportFile))
        {
            FileStream fs = File.Open(strReportFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                //the sample line:
                //Y 349 CoPep00035959 0.686646308289647 0.345257544453165 0.686646308289647 60989
                //N 369
                string[] tokens = line.Split(new string[] { "|" }, StringSplitOptions.None);
                if (tokens[0] == "Y")
                {
                    string peptideID = tokens[2];
                    string strIPI = tokens[12];

                    //string strSQL = "select ref_protein_id,t1.protein_cop_id ,protein_name,organism_source,sequence_length from protein_tbl t1, pp_relation_tbl t2 where t1.protein_cop_id = t2.protein_cop_id and t2.peptide_cop_id = '{0}'";
                    //strSQL = string.Format(strSQL, peptideID);
                    //DBInterface.ConnectDB();
                    //DbDataReader result = DBInterface.QuerySQL(strSQL);
                    //if (result != null)
                    //{                           
                    //    while (result.Read())
                    //    {
                    //        string strIPI = result.GetString (0);
                    if (Proteins.ContainsKey(strIPI))
                    {
                        ProteinInfo PI = Proteins[strIPI];
                        ScanPeptide spinfo = new ScanPeptide();
                        spinfo.ScanNO = tokens[1];
                        spinfo.PeptideID = peptideID;
                        spinfo.SimilarityScore = tokens[5];
                        spinfo.DetaM = double.Parse(tokens[6]);
                        spinfo.SpectrumSeq = tokens[7];
                        spinfo.PeptideSequence = tokens[8];
                        spinfo.ModifiedType = tokens[9];
                        spinfo.FinalScore = tokens[10];
                        spinfo.mzFile = tokens[11];
                        PI.AddScanPeptide(spinfo);
                        Proteins[strIPI] = PI;
                    }
                    else
                    {
                        string strCOPaID = strIPI;
                        string strProteinName = tokens[13];
                        string strOrganism;
                        if (this.lbSearchingModule.Text.ToLower().Contains("human"))
                            strOrganism = "Homo sapiens (Human)";
                        else
                            strOrganism = "Mus musculus (Mouse)";
                        int length = int.Parse(tokens[14]);
                        ProteinInfo PI = new ProteinInfo(strIPI, strCOPaID, strProteinName, strOrganism, length);
                        ScanPeptide spinfo = new ScanPeptide();
                        spinfo.ScanNO = tokens[1];
                        spinfo.PeptideID = peptideID;
                        spinfo.SimilarityScore = tokens[5];
                        spinfo.DetaM = double.Parse(tokens[6]);
                        spinfo.SpectrumSeq = tokens[7];
                        spinfo.PeptideSequence = tokens[8];
                        spinfo.ModifiedType = tokens[9];
                        spinfo.FinalScore = tokens[10];
                        spinfo.mzFile = tokens[11];
                        PI.AddScanPeptide(spinfo);
                        Proteins.Add(strIPI, PI);
                    }

                    //    }
                    //    result.Close();

                    //}
                    //DBInterface.CloseDB();

                }
            }
            sr.Close();
        }

        //write the protein view info to report 2

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = ("    ");
        //string strReportFile = Properties.Settings.Default.upload_path + searchInfo.task_id + "\\" + searchInfo.report_filename;
        string XSLFile = strReportFile + ".xls";
        strReportFile += ".xml";
        //XSL table colums : SCAN, PeptideSequence, ModifiedType,Protein Access Numbers, Protein Name, Species, Protein_COPa_ID, Peptide_COPa_ID,Spectra_COPa_ID, MatchScore,DetaM
        string ProteinLine = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}";
        try
        {
            using (StreamWriter sw = new StreamWriter(XSLFile))
            {
                sw.WriteLine(string.Format(ProteinLine, "mzFile", "Scan", "PeptideSequence", "ModifiedType", "Protein Access Numbers", "Protein Name", "Species", "Protein COPaID", "Peptide COPaID", "Spectrum COPaID", "Similarity Score", "DetaM/Z", "Final Score"));
                using (XmlWriter writer = XmlWriter.Create(strReportFile, settings))
                {
                    writer.WriteStartElement("COPaReport");
                    writer.WriteAttributeString("TaskID", TaskID);
                    writer.WriteAttributeString("mzFile", FileName);
                    string strLibModule = searchModule;


                    writer.WriteAttributeString("LibModule", strLibModule);
                    writer.WriteAttributeString("SearchFilter", sc);
                    writer.WriteAttributeString("IDProteins", Proteins.Count.ToString());
                    foreach (KeyValuePair<string, ProteinInfo> kvp in Proteins)
                    {
                        ArrayList splists = kvp.Value.ScanPeptides;
                        if (IsPassFilter(splists, distinctPeptides))
                        {
                            writer.WriteStartElement("Proteins");
                            writer.WriteAttributeString("IPI", kvp.Key);
                            writer.WriteAttributeString("COPaID", kvp.Value.COPaID);
                            writer.WriteAttributeString("ProteinName", kvp.Value.ProteinName);
                            writer.WriteAttributeString("Organism", kvp.Value.Organism);
                            //ArrayList splists = kvp.Value.ScanPeptides;
                            writer.WriteAttributeString("SpectraCount", splists.Count.ToString());
                            writer.WriteAttributeString("NormalizCount", ((float)(splists.Count) / kvp.Value.ProteinLength).ToString());
                            foreach (ScanPeptide sp in splists)
                            {
                                writer.WriteStartElement("Scan-Peptide");
                                writer.WriteAttributeString("mzFile", sp.mzFile);
                                writer.WriteAttributeString("Scan", sp.ScanNO);
                                writer.WriteAttributeString("PeptideSequence", sp.PeptideSequence);
                                if (sp.ModifiedType != "")
                                {
                                    writer.WriteAttributeString("ModifiedType", sp.ModifiedType);
                                }
                                writer.WriteAttributeString("Peptide", sp.PeptideID);
                                writer.WriteAttributeString("SimilarityScore", sp.SimilarityScore);
                                writer.WriteAttributeString("DetaMZ", sp.DetaM.ToString());
                                writer.WriteAttributeString("Spectrum", sp.SpectrumSeq);
                                writer.WriteAttributeString("FinalyScore", sp.FinalScore);
                                writer.WriteEndElement();
                                sw.WriteLine(ProteinLine, sp.mzFile, sp.ScanNO, sp.PeptideSequence, sp.ModifiedType, kvp.Key, kvp.Value.ProteinName, kvp.Value.Organism, kvp.Value.COPaID, sp.PeptideID, sp.SpectrumSeq, sp.SimilarityScore, sp.DetaM, sp.FinalScore);

                            }
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                    sw.Close();
                }
            }
        }
        catch (Exception ex)
        {
            DBInterface.LogEvent(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
        }

        return true;

    }

    private bool IsPassFilter(ArrayList PSMS, int uniquepeptide)
    {
        Hashtable PSM = new Hashtable();
        foreach (ScanPeptide sp in PSMS)
        {
            if (!PSM.Contains(sp.PeptideID))
                PSM.Add(sp.PeptideID, null);

        }
        if (PSM.Count >= uniquepeptide)
            return true;
        else
            return false;
    }

    class ProteinInfo
    {
        public string IPI;
        public string COPaID;
        public string ProteinName;
        public string Organism;
        public int ProteinLength;
        public ArrayList ScanPeptides;
        public ProteinInfo(string strIPI, string strCOPaID, string strProteinName, string strOrganism, int length)
        {
            IPI = strIPI;
            COPaID = strCOPaID;
            ProteinName = strProteinName;
            Organism = strOrganism;
            ProteinLength = length;
            ScanPeptides = new ArrayList();

        }

        public bool AddScanPeptide(ScanPeptide spinfo)
        {
            ScanPeptides.Add(spinfo);
            return true;
        }
    }

    struct ScanPeptide
    {
        public string ScanNO;
        public string mzFile;
        public string PeptideID;
        public string SpectrumSeq;
        public string FinalScore;
        public double DetaM;
        public string PeptideSequence;
        public string ModifiedType;
        public string SimilarityScore;
    }
    protected void lbShowIntact_Click(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        float confidence = 0.95f;
        try
        {
            confidence = float.Parse(this.lbThreshold.Text.Substring(0, this.lbThreshold.Text.Length - 1));
            if (confidence < 0 || confidence > 100)
            {
                lbMessage.Text = "Please input a number between 1 and 100 for the confidence statistic level.";
                return;
            }
            else
                confidence = confidence / 100;
        }
        catch (Exception ex)
        {
            lbMessage.Text = "Please input the right format number, such as 95%, for the confidence statistic level.";
            return;
        }
        int distinctPeptide = 1;
        try
        {
            distinctPeptide = int.Parse(this.lbPeptideNumber.Text);

        }
        catch (Exception ex)
        {
            lbMessage.Text = "Please input the right format number, such as 1, for the distinct peptides needed.";
            return;
        }
        int uniquePeptide = 0;
        try
        {
            uniquePeptide = int.Parse(this.lbUniquePeptide.Text);

        }
        catch (Exception ex)
        {
            lbMessage.Text = "Please input the right format number, such as 1, for the unique peptides needed.";
            return;
        }

        UpdateSearchingCondition(TaskID, confidence, distinctPeptide, uniquePeptide);
    }
}
