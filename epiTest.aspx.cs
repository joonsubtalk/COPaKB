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
using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using ZJU.COPLib;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Web.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using gov.nih.nlm.ncbi.eutils;

public partial class epiTest : System.Web.UI.Page
{

    string taskIDs;
    string bioMarkers;
    ArrayList markedIDs;
    ArrayList markedNames;
    bool thereIsReport;

    protected void Page_Load(object sender, EventArgs e)
    {
        thereIsReport = false;
        markedIDs = new ArrayList();
        markedNames = new ArrayList();
        string QueryType = Request.QueryString["Tasks"];
        taskIDs = QueryType;
        string QueryValue = Request.QueryString["Markers"];
        bioMarkers = QueryValue;
        TableRow taskInfo = new TableRow();
        TableRow markerInfo = new TableRow();
        TableCell taskIDList = new TableCell();
        TableCell markerList = new TableCell();
        taskIDList.Text = "";
        markerList.Text = "Biomarker(s):  ";
        if (bioMarkers == null || bioMarkers.Equals(""))
        {
            bioMarkers = String.Format("everything");
        }
        if (taskIDs != null)
        {
            string[] IDs = taskIDs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] MKs = bioMarkers.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            taskIDList.Text = string.Format("{0} task(s) were analyzed: ", IDs.Length.ToString());

            foreach (string id in IDs)
            {
                Regex.Replace(id, @"s", "");
                string taskIDURL = "<a href='ReportView.aspx?TaskID={0}'>{1}</a>";
                taskIDList.Text = string.Format("{0}   {1}", taskIDList.Text, string.Format(taskIDURL, id, id));
            }

            for (int i = 0; i < MKs.Length; i++)
            {
                if (MKs[0].Equals("everything"))
                    break;
                if (MKs[i].Length < 6)
                {
                    Response.Redirect("IOSError.aspx?Err=3");
                }
                MKs[i] = MKs[i].Trim();
                MKs[i] = MKs[i].ToUpper();
            }

            for (int i = 0; i < IDs.Length; i++)
            {
                IDs[i] = IDs[i].Trim();

            }


            foreach (string mk in MKs)
            {
                if (MKs[0].Equals("everything") == true) {
                    markerList.Text = string.Format("Biomarker(s): Not specified");
                    break;
                }

                Regex.Replace(mk, @"s", "");
                string markerURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>";
                markerList.Text = string.Format("{0} {1}", markerList.Text, string.Format(markerURL, mk, mk));
            }

            taskInfo.Cells.Add(taskIDList);
            tbInfo.Rows.Add(taskInfo);
            markerInfo.Cells.Add(markerList);
            tbInfo.Rows.Add(markerInfo);
            foreach (string id in IDs)
            {
                this.FindMarkers(id, MKs);
            }
            if (!thereIsReport)
            {
                Response.Redirect("ErrorPage.aspx?Err=2");
            }
            if (markedIDs.Count > 0)
            {
                //this.TableCell1.Text = string.Format("{0} task(s) Contains Biomarker(s)", markedIDs.Count.ToString());
                this.TableCell1.Text = "Task";
                this.FindSharedProteins(markedIDs, markedNames);
            }
            else
            {
                this.TableCell1.Text = "No task contained the biomarker(s) has been found";
            }
        }
        
    }
    void FindMarkers(string TaskID, string [] bioMarkers)
    {   
        bool containsMarker = true;
        ArrayList proteinIDs = new ArrayList();
        ArrayList proteinNames = new ArrayList();
        string strReportFile = "";
        strReportFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xml";// +result.GetString(6) + ".xml";
        FileInfo fiReportFile = new FileInfo(strReportFile);
        if (!fiReportFile.Exists)
        {
            
            return;
        }
        thereIsReport = true;
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        settings.IgnoreWhitespace = true;
        settings.IgnoreComments = true;

        XmlReader xr = XmlReader.Create(strReportFile, settings);
        object BasicInfo = xr.NameTable.Add("COPaReport");
        object Proteins = xr.NameTable.Add("Proteins");
        object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
        Dictionary<string, float> ProteinQVs = new Dictionary<string, float>();
        int ProteinNumber = 0;
        while (xr.Read())
        {

            if (xr.IsStartElement() && Proteins == xr.Name)
            {
                string ProteinText = string.Format("{0} {1}", xr[0], xr[2]);

                string ProteinUrl = string.Format("ProteinInfo.aspx?QType=Protein ID&QValue={0}", xr[1]);
                string ProteinIPI = xr["COPaID"];
                string ProteinName = xr[2];
                proteinIDs.Add(ProteinIPI);
                proteinNames.Add(ProteinName);
                ProteinNumber++;

            }
            
        }
        xr.Close();
        foreach (string mk in bioMarkers){
            if (mk.Equals("everything")){
                containsMarker = true;
                break;
            }
            for (int i = 0; i < proteinIDs.Count; i++) {
                if (mk.Equals(proteinIDs[i])){
                    break;
                }
                if (i == proteinIDs.Count-1){
                    containsMarker = false;
                    break;
                }
            }
            if (!containsMarker){
                break;
            }
        }
        if (containsMarker)
        {
            string strSQL = string.Format("select distinct upload_time, CITY, COUNTRY from search_task where task_seq='{0}'", TaskID);

            DataSet result = DBInterface.QuerySQL2(strSQL);

            markedIDs.Add(proteinIDs);
            markedNames.Add(proteinNames);
            TableRow theTask = new TableRow();
            TableCell theTaskID = new TableCell();
            TableCell theLocation = new TableCell();
            TableCell theTime = new TableCell();
            string taskIDURL = "<a href='ReportView.aspx?TaskID={0}'>{1}</a>";
            theTaskID.Text = string.Format(taskIDURL, TaskID, TaskID);
            if (result != null)
            {
                theTime.Text = result.Tables[0].Rows[0].ItemArray[0].ToString();
                theLocation.Text = string.Format("{0}, {1}",result.Tables[0].Rows[0].ItemArray[1].ToString(),result.Tables[0].Rows[0].ItemArray[2].ToString());
            }
            theTaskID.HorizontalAlign = HorizontalAlign.Left;
            theTask.Cells.Add(theTaskID);
            theTask.Cells.Add(theLocation);
            theTask.Cells.Add(theTime);
            tbtaskids.Rows.Add(theTask);
        }
    }
    void FindSharedProteins(ArrayList IDs, ArrayList Names)
    {
        ArrayList refArray = (ArrayList) IDs[0];
        ArrayList refNames = (ArrayList) Names[0];

        if (IDs.Count > 1)
        {
            for (int n = 0; n < refArray.Count; n++)
            {
                bool proteinShared = true;
                for (int i = 1; i < IDs.Count; i++)
                {
                    ArrayList tarArray = (ArrayList)IDs[i];
                    for (int m = 0; m < tarArray.Count; m++)
                    {
                        if (refArray[n].Equals(tarArray[m]))
                        {
                            break;
                        }
                        if (m == tarArray.Count - 1)
                        {
                            proteinShared = false;
                        }
                    }

              
                }
                if (proteinShared)
                {
                    TableRow theProtein = new TableRow();
                    TableCell theProteinID = new TableCell();
                    string proteinIDURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>";
                    theProteinID.Text = string.Format(proteinIDURL, (string)refArray[n], (string)refArray[n]);
                    theProtein.Cells.Add(theProteinID);
                    TableCell theProteinName = new TableCell();
                    theProteinName.Text = (string)refNames[n];
                    theProtein.Cells.Add(theProteinName);
                    tbSharedProtein.Rows.Add(theProtein);
                }
            }
        }
        else if (IDs.Count == 1)
        {
            for (int n = 0; n < refArray.Count; n++)
            {
                TableRow theProtein = new TableRow();
                TableCell theProteinID = new TableCell();
                string proteinIDURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>";
                theProteinID.Text = string.Format(proteinIDURL, (string)refArray[n], (string)refArray[n]);
                theProtein.Cells.Add(theProteinID);
                TableCell theProteinName = new TableCell();
                theProteinName.Text = (string)refNames[n];
                theProtein.Cells.Add(theProteinName);
                tbSharedProtein.Rows.Add(theProtein);
            }
        }
    }
}
