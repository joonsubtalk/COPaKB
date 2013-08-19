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
using ZJU.COPDB;
using System.Xml;
using System.Web.Configuration;
using System.Collections.Generic;
using System.IO;

using System.Net;

public partial class ReportView : System.Web.UI.Page
{
     string TaskID;
     String moduleType;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsCallback)
        {
            TaskID = Request.QueryString["TaskID"];
            if (TaskID == null)
                TaskID = (string)Session["TaskID"];
            ShowProteinReportTree(TaskID);
            this.hlViewSummary.NavigateUrl  = string.Format("javascript:mySpl.loadPage('RightContent', 'Report.aspx?TaskID={0}')", TaskID );
            this.SwithTreeView.NavigateUrl = string.Format("ReportListView.aspx?TaskID={0}", TaskID);
            displayInteractome();
        }
    }

    private void displayInteractome()
    {
        string webtext = "";
        WebRequest request;
        WebResponse response;
        System.IO.StreamReader reader;
        string[] files = new string[7] {"drosophila_mitochondria", 
                                        "human_mitochondria", 
                                        "human_proteosome",
                                        "mouse_cytosol",
                                        "mouse_mitochondria",
                                        "mouse_nucleus",
                                        "mouse_proteosome"};

        for (int i = 0; i < 7; i++)
        {
            request = WebRequest.Create(string.Format("http://www.heartproteome.org/copa/js/copa_imex_interactome_{0}.js", files[i]));
            response = request.GetResponse();
            reader = new System.IO.StreamReader(response.GetResponseStream());
            webtext = reader.ReadToEnd();
            string protein = "P999999";
            if (System.Text.RegularExpressions.Regex.IsMatch(webtext, protein))
            {
                if (i == 2)
                    interactomeDisplay.Text += string.Format("<a href='interactome_{0}.aspx?iid={1}'>View {0}</a><br />", "human_proteasome", protein.ToUpper());
                else if (i == 6)
                    interactomeDisplay.Text += string.Format("<a href='interactome_{0}.aspx?iid={1}'>View {0}</a><br />", "mouse_proteasome", protein.ToUpper());
                else
                    interactomeDisplay.Text += string.Format("<a href='interactome_{0}.aspx?iid={1}'>View {0}</a><br />", files[i], protein.ToUpper());
            }
            else
                interactomeDisplay.Text += "no match found with " + moduleType;
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
            Result = "Normal Searching and filter with ";
            for (int i = 1; i < tokens.Count(); i++)
            {
                string[] filterpair = tokens[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                Result += string.Format("( Similarity > {0} and Final Score > {1} )", filterpair[0], filterpair[1]);
            }

        }
        return Result;

    }

      string strReportFile = "";
    void ShowProteinReportTree(string TaskID)
    {
        //this.lbSaveReport.Text = string.Format("<a href='SaveReport.aspx?Result={0}' Target='_blank'>  Save this report</a>", TaskID);
        //string strSQL = "select task_seq,task_user,upload_time,task_status,upload_filename, search_model,report_filename from search_task where task_seq={0} ";
        //strSQL = string.Format(strSQL, TaskID);
        //DBInterface.ConnectDB();
        //System.Data.Common.DbDataReader result = DBInterface.QuerySQL(strSQL);
        //if (result != null)
        //{
        //    if (result.Read())
        //    {
                strReportFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xml";// +result.GetString(6) + ".xml";
                FileInfo fiReportFile = new FileInfo(strReportFile);
                if (!fiReportFile.Exists)
                {
                    Response.Redirect("ErrorPage.aspx?Err=2");
                    return;
                }
                string strReportFile2 = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xls";// +result.GetString(6) + ".xls";

                this.lbSaveReport.Text = string.Format("Save this report:<br/><a href='SaveReport.aspx?Report={0}&ReportType=XML' Target='_blank'> COPa Client Format(*.cpr) </a> | <a href='SaveReport.aspx?Report={1}&ReportType=XLS' Target='_blank'> Excel Format(*.xls) </a>", strReportFile, strReportFile2);

                //result.Close();
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
                float maxValue = 0;
                float totalValue = 0;
                Dictionary<string, float> ProteinQVs = new Dictionary<string, float>();
                int ProteinNumber = 0;
                while (xr.Read())
                {
                    if (xr.IsStartElement() && BasicInfo == xr.Name)
                    {
                        this.lbmzML.Text = xr["mzFile"].Replace("\r\n", "<br/>"); ;
                        //this.lbProteinCount.Text = xr["IDProteins"];
                        this.lbSC.Text = xr["SearchFilter"];
                        //this.lbSearchingModule.Text = ;
                        moduleType = xr["LibModule"];
                        int index3 = moduleType.LastIndexOf('.');
                        if (index3 != -1 && index3 < moduleType.Length)
                            this.lbSearchingModule.Text = moduleType.Substring(index3 + 1);
                        else
                            this.lbSearchingModule.Text = moduleType;
                    }

                    if (xr.IsStartElement() && Proteins == xr.Name)
                    {
                        string ProteinText = string.Format("{0} {1}", xr[0], xr[2]);

                        TreeNode Protein = new TreeNode(ProteinText, xr[1]);
                        if (ProteinText.Length > 50)
                            Protein.Text = ProteinText.Substring(0, 50)+"...";
                        Protein.ToolTip = ProteinText;
                        Protein.ImageUrl = "_image\\protein.gif";
                        Protein.NavigateUrl = string.Format("javascript:mySpl.loadPage('RightContent', 'ProteinInfo.aspx?QType=Protein ID&QValue={0}&showInfo=T')", xr[1]);
                        this.TreeView1.Nodes[0].ChildNodes.Add(Protein);
                        string ProteinIPI = xr["IPI"];
                        float QuantitateValue = float.Parse(xr["NormalizCount"]);
                        ProteinQVs.Add(ProteinIPI, QuantitateValue);
                        totalValue += QuantitateValue;
                        if (QuantitateValue > maxValue)
                            maxValue = QuantitateValue;
                        ProteinNumber++;
                    }
                    if (xr.IsStartElement() && ScanPeptide == xr.Name)
                    {
                        string PeptideID = xr["Peptide"];
                        string score = xr["FinalyScore"];
                        string SpectrumSeq = xr["Spectrum"];
                        string Scan = xr["Scan"];
                        string mzFile = xr["mzFile"];
                        string peptideSequence = xr["PeptideSequence"];
                        if (score == null || score == "")
                            score = xr["SimilarityScore"];
                        string ScanText = string.Format("{0}[{1}]@{2}:(#{3})",peptideSequence,score.Substring (0,4),mzFile, Scan);
                        TreeNode ScanNode = new TreeNode(ScanText, xr[3]);
                        ScanNode.ToolTip = string.Format(" Sequence:{0}\r\n Score:{1}\r\n mzFile:{2}\r\n Scan:{3}", peptideSequence, score, mzFile, Scan);
                        ScanNode.ImageUrl = "_image\\spectrum.gif";
                        ScanNode.NavigateUrl = string.Format("javascript:mySpl.loadPage('RightContent', 'MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}&File={3}&showInfo=T')", SpectrumSeq, Scan, TaskID, mzFile);

                        this.TreeView1.Nodes[0].ChildNodes[this.TreeView1.Nodes[0].ChildNodes.Count - 1].ChildNodes.Add(ScanNode);
                    }
                }
                xr.Close();
                this.lbProteinCount.Text = ProteinNumber.ToString();
                string ProgresText = "<div class=\"graph\"><strong class=\"bar\" style=\"width: {0}%;\">{1}</strong></div>";
                string ProIPIURL = "<a href='ProteinInfo.aspx?QType=Protein%20ID&QValue={0}' Target='_Blank'>{1}</a>";

                var sortedDict = (from entry in ProteinQVs orderby entry.Value descending select entry);
                int count = sortedDict.Count();
                for (int i = 0; i < count; i++)
                {
                    KeyValuePair<string, float> ipiv = sortedDict.ElementAt(i);
                    TableRow tr = new TableRow();
                    TableCell tcIPI = new TableCell();
                    tcIPI.Text = string.Format (ProIPIURL,ipiv.Key,ipiv.Key );
                    tcIPI.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tcIPI);
                    TableCell tcProgress = new TableCell();
                    tcProgress.Text = string.Format(ProgresText, (int)(ipiv.Value * 100 / maxValue), getFifthPlace(ipiv.Value / totalValue));
                    tcIPI.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tcProgress);
                    tbProteins.Rows.Add(tr);
                }
                this.TreeView1.Nodes[0].CollapseAll();
                this.TreeView1.Nodes[0].Expand();
         //}
        //}
    }

    protected string getFifthPlace( float num ){
        return (String)num.ToString("N5");
    }
   
    //protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    //{
    //    if (this.TreeView1.SelectedNode.Parent == this.TreeView1.Nodes[0] && this.TreeView1.SelectedNode.ChildNodes.Count == 0)
    //    {
    //        string IPI = this.TreeView1.SelectedNode.Text;
    //        IPI = IPI.Substring(0, IPI.IndexOf(" "));
    //        XmlReaderSettings settings = new XmlReaderSettings();
    //        settings.ConformanceLevel = ConformanceLevel.Fragment;
    //        settings.IgnoreWhitespace = true;
    //        settings.IgnoreComments = true;

    //        XmlReader xr = XmlReader.Create(strReportFile, settings);
    //        object Proteins = xr.NameTable.Add("Proteins");
    //        object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
    //        string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
    //        string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
    //        while (xr.Read())
    //        {

    //            if (xr.IsStartElement() && Proteins == xr.Name)
    //            {
    //                if (xr[0] == IPI)
    //                {
    //                    while (xr.Read())
    //                    {
    //                        if (xr.IsStartElement() && ScanPeptide == xr.Name)
    //                        {
    //                            string ScanText = string.Format("Scan:{0} Peptide:{1} Score:{2}", xr[0], xr[1], xr[2]);
    //                            TreeNode ScanNode = new TreeNode(ScanText, xr[3]);
    //                            if (TaskID == null)
    //                                TaskID = (string)Session["TaskID"];
    //                            ScanNode.NavigateUrl = string.Format("javascript:mySpl.loadPage('RightContent', 'MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}')", xr[3], xr[0], TaskID);
    //                            this.TreeView1.SelectedNode.ChildNodes.Add(ScanNode);
    //                        }
    //                        if (Proteins == xr.Name)
    //                            break;
    //                    }
    //                    break;
    //                }

    //            }

    //        }
    //    }
    //}
}
