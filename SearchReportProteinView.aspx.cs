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
using System.Xml;
using ZJU.COPDB;
using System.Web.Configuration;

public partial class SearchReportProteinView : System.Web.UI.Page
{
    string TaskID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsCallback)
        {
            TaskID = Request.QueryString["TaskID"];
            if (TaskID == null)
                TaskID = (string)Session["TaskID"];
            ShowProteinReportTree(TaskID);
        }
    }

    //void ShowProteinReport(string TaskID)
    //{
    //   string strSQL = "select task_seq,task_user,upload_time,task_status,upload_filename, search_model,report_filename from search_task where task_seq={0} ";
    //    strSQL = string.Format(strSQL,TaskID );
    //     DBInterface.ConnectDB();
    //    System.Data.Common.DbDataReader result = DBInterface.QuerySQL(strSQL);
    //    if (result != null)
    //    {
    //        result.Read();
    //        string strReportFile =  WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\" +result.GetString(6)+".xml";
    //        result.Close();
    //        XmlReaderSettings settings = new XmlReaderSettings();
    //        settings.ConformanceLevel = ConformanceLevel.Fragment;
    //        settings.IgnoreWhitespace = true;
    //        settings.IgnoreComments = true;

    //        XmlReader xr = XmlReader.Create(strReportFile, settings);
    //        object Proteins = xr.NameTable.Add("Proteins");
    //        object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
    //        string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
    //        string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
    //        while (xr.Read() )
    //        {
    //            if (xr.IsStartElement() && Proteins == xr.Name)
    //            {
    //                TableRow trProtein = new TableRow();
    //                TableCell ProteinID = new TableCell();
    //                ProteinID.Text = string.Format("<a href='http://srs.ebi.ac.uk/srsbin/cgi-bin/wgetz?-newId+[IPI-AllText:{0}*]+-lv+30+-view+SeqSimpleView+-page+qResult' Target='_Blank'>{1}</a>", xr[0],xr[0]);
    //                trProtein.Cells.Add(ProteinID);
    //                TableCell ProteinCOPaID = new TableCell();
    //                ProteinCOPaID.Text = string.Format ( "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>",xr[1],xr[1]);
    //                trProtein.Cells.Add(ProteinCOPaID);
    //                TableCell ProteinName = new TableCell();
    //                ProteinName.Text = xr[2];
    //                ProteinName.ColumnSpan = 2;
    //                trProtein.Cells.Add(ProteinName);
    //                TableCell Organism = new TableCell();
    //                Organism.Text = xr[3];
    //                trProtein.Cells.Add(Organism);                   
    //                this.tbReport.Rows.Add(trProtein);
    //                // add the sub table title
    //                TableRow trCaption = new TableRow();
    //                TableCell tcSpace = new TableCell();
    //                tcSpace.Text = "  ";
    //                trCaption.Cells.Add(tcSpace);

    //                TableCell tcScan = new TableCell();
    //                tcScan.Text = "Scan";
    //                tcScan.HorizontalAlign = HorizontalAlign.Left;
    //                tcScan.BorderColor = System.Drawing.Color.White;
    //                tcScan.BorderStyle = BorderStyle.Solid;
    //                tcScan.BorderWidth = 1;
    //                trCaption.Cells.Add(tcScan);

    //                TableCell tcPeptide = new TableCell();
    //                tcPeptide.Text = "Peptide COPa ID";
    //                tcPeptide.BorderColor = System.Drawing.Color.White;
    //                tcPeptide.BorderStyle = BorderStyle.Solid;
    //                tcPeptide.BorderWidth = 1;
    //                tcPeptide.HorizontalAlign = HorizontalAlign.Left;
    //                trCaption.Cells.Add(tcPeptide);

    //                TableCell tcFS = new TableCell();
    //                tcFS.Text = "Match Score";
    //                tcFS.HorizontalAlign = HorizontalAlign.Right;
    //                tcFS.BorderColor = System.Drawing.Color.White;
    //                tcFS.BorderStyle = BorderStyle.Solid;
    //                tcFS.BorderWidth = 1;
    //                trCaption.Cells.Add(tcFS);

    //                TableCell tcSpectrum = new TableCell();
    //                tcSpectrum.Text ="Mirror Spectrum view";
    //                tcSpectrum.HorizontalAlign = HorizontalAlign.Center;
    //                tcSpectrum.BorderColor = System.Drawing.Color.White;
    //                tcSpectrum.BorderStyle = BorderStyle.Solid;
    //                tcSpectrum.BorderWidth = 1;
    //                trCaption.Cells.Add(tcSpectrum);
    //                trCaption.BackColor = System.Drawing.Color.LightBlue;
    //                trCaption.Font.Bold = true;
    //                this.tbReport.Rows.Add(trCaption);
    //            }
    //            if (xr.IsStartElement() && ScanPeptide == xr.Name)
    //            {
    //                TableRow trCaption = new TableRow();

    //                TableCell tcSpace = new TableCell();
    //                tcSpace.Text = "  ";
    //                trCaption.Cells.Add(tcSpace);

    //                TableCell tcScan = new TableCell();
    //                tcScan.Text = xr[0];
    //                tcScan.HorizontalAlign = HorizontalAlign.Left;
    //                trCaption.Cells.Add(tcScan);

    //                TableCell tcPeptide = new TableCell();
    //                tcPeptide.Text = string.Format(PepIDURL, xr[1], xr[1]);
    //                tcPeptide.HorizontalAlign = HorizontalAlign.Left;
    //                trCaption.Cells.Add(tcPeptide);

    //                TableCell tcFS = new TableCell();
    //                tcFS.Text = xr[2];
    //                tcFS.HorizontalAlign = HorizontalAlign.Right;
    //                trCaption.Cells.Add(tcFS);

    //                TableCell tcSpectrum = new TableCell();
    //                tcSpectrum.Text = string.Format(SpectrumURL, xr[3], xr[0], TaskID);
    //                tcSpectrum.HorizontalAlign = HorizontalAlign.Center;
    //                trCaption.Cells.Add(tcSpectrum);
                   
    //                this.tbReport.Rows.Add(trCaption);
    //            }
    //        }
    //    }
        

    //}

    string strReportFile = "";
    void ShowProteinReportTree(string TaskID)
    {
        string strSQL = "select task_seq,task_user,upload_time,task_status,upload_filename, search_model,report_filename from search_task where task_seq={0} ";
        strSQL = string.Format(strSQL, TaskID);
        //DBInterface.ConnectDB();
        IDataReader result = DBInterface.QuerySQL(strSQL);
        if (result != null)
        {
            result.Read();
            strReportFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\" + result.GetString(6) + ".xml";
            result.Close();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            XmlReader xr = XmlReader.Create(strReportFile, settings);
            object Proteins = xr.NameTable.Add("Proteins");
            object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
            string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
            string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
            while (xr.Read())
            {

                if (xr.IsStartElement() && Proteins == xr.Name)
                {
                    string ProteinText = string.Format("{0} {1}", xr[0], xr[2]);
                    TreeNode Protein = new TreeNode(ProteinText, xr[1]);
                    this.TreeView1.Nodes[0].ChildNodes.Add(Protein);

                }
                //if (xr.IsStartElement() && ScanPeptide == xr.Name)
                //{
                //    string ScanText = string.Format("Scan:{0} Peptide:{1} Score:{2}", xr[0], xr[1], xr[2]);
                //    TreeNode ScanNode = new TreeNode(ScanText, xr[3]);
                //    this.TreeView1.Nodes[0].ChildNodes[this.TreeView1.Nodes[0].ChildNodes.Count - 1].ChildNodes.Add (ScanNode);
                //}
            }
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (this.TreeView1.SelectedNode.Parent == this.TreeView1.Nodes[0] && this.TreeView1.SelectedNode.ChildNodes.Count == 0)
        {
            string IPI = this.TreeView1.SelectedNode.Text;
            IPI = IPI.Substring(0, IPI.IndexOf(" "));
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            XmlReader xr = XmlReader.Create(strReportFile, settings);
            object Proteins = xr.NameTable.Add("Proteins");
            object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
            string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
            string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
            while (xr.Read())
            {

                if (xr.IsStartElement() && Proteins == xr.Name)
                {
                    if (xr[0] == IPI)
                    {
                        while (xr.Read())
                        {
                            if (xr.IsStartElement() && ScanPeptide == xr.Name)
                            {
                                string ScanText = string.Format("Scan:{0} Peptide:{1} Score:{2}", xr[0], xr[1], xr[2]);
                                TreeNode ScanNode = new TreeNode(ScanText, xr[3]);
                                if (TaskID == null)
                                    TaskID = (string)Session["TaskID"]; 
                                ScanNode.NavigateUrl = string.Format("javascript:mySpl.loadPage('RightContent', 'MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}')", xr[3], xr[0], TaskID);
                                this.TreeView1.SelectedNode.ChildNodes.Add(ScanNode);
                            }
                            if (Proteins == xr.Name)
                                break;
                        }
                        break;
                    }

                }

            }
        }
        //else
        //{
        //    //if (this.TreeView1.SelectedNode != this.TreeView1.Nodes[0])
        //    //{ 
        //    //    //string NodeText = this.TreeView1.SelectedNode.Text ;
        //    //    //string ScanID = NodeText.Substring(5, NodeText.IndexOf(" ")-5);
        //    //    //string spectrumseq = this.TreeView1.SelectedNode.Value;
                
        //    //    //HtmlContainerControl rightframe = (HtmlContainerControl)this.Parent.FindControl("right_frame");
        //    //    //if (TaskID == null)
        //    //    //    TaskID = (string)Session["TaskID"]; 

        //    //    //rightframe.Attributes["src"] = string.Format(" MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}", spectrumseq, ScanID, TaskID);
        //    //}
        //}
    }
}
