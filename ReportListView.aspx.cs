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
using System.IO;
using System.Xml;
using System.Web.Configuration;
using System.Collections.Generic ;

public partial class ReportListView : System.Web.UI.Page
{
    string TaskID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsCallback)
        {
            TaskID = Request.QueryString["TaskID"];
            string SortBy = Request.QueryString["SortBy"];
            if (SortBy == null)
            {
                SortBy = "Total_Score";
            }

            string SortType = Request.QueryString["SortType"];
            if (SortType == null)
            {
                SortType = "DESC";
            }
            ShowProteinList(TaskID,SortBy,SortType );
            
            this.hlViewSummary.NavigateUrl = string.Format("javascript:vSp1.loadPage('RightContent', 'Report.aspx?TaskID={0}')", TaskID);
            this.SwithTreeView.NavigateUrl = string.Format("ReportView.aspx?TaskID={0}", TaskID);
        }
    }

    private void ShowProteinList(string TaskID,string SortBy, string SortType)
    {
        DataTable dtProteins = new DataTable();       
        dtProteins.Columns.Add("Protein_ID");
        dtProteins.Columns.Add("Protein_Name");
        dtProteins.Columns.Add("Species");
        dtProteins.Columns.Add("Spectra_Count",typeof (int));
        dtProteins.Columns.Add("Total_Score", typeof(float));
   
        string  strReportFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xml";// +result.GetString(6) + ".xml";
        FileInfo fiReportFile = new FileInfo(strReportFile);
        if (!fiReportFile.Exists)
        {
            Response.Redirect("ErrorPage.aspx?Err=2");
            return; }
        string strReportFile2 = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xls";// +result.GetString(6) + ".xls";
        this.lbSaveReport.Text = string.Format("Save this report:<br/><a href='SaveReport.aspx?Report={0}&ReportType=XML' Target='_blank'> COPa Client Format(*.cpr) </a> | <a href='SaveReport.aspx?Report={1}&ReportType=XLS' Target='_blank'> Excel Format(*.xls) </a>", strReportFile, strReportFile2);


        XmlReaderSettings settings = new XmlReaderSettings();
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        settings.IgnoreWhitespace = true;
        settings.IgnoreComments = true;

        XmlReader xr = XmlReader.Create(strReportFile, settings);
        object BasicInfo = xr.NameTable.Add("COPaReport");
        object Proteins = xr.NameTable.Add("Proteins");
        object ScanPeptide = xr.NameTable.Add("Scan-Peptide");
        
        //string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
        //string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
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
                String moduleType = xr["LibModule"];
                int index3 = moduleType.LastIndexOf('.');
                if (index3 != -1 && index3 < moduleType.Length)
                    this.lbSearchingModule.Text = moduleType.Substring(index3 + 1);
                else
                    this.lbSearchingModule.Text = moduleType;
            }

            if (xr.IsStartElement() && Proteins == xr.Name)
            {
                string strIPI = xr["COPaID"];
                string strName = xr["ProteinName"];
                string strOrganism = xr["Organism"];
                string strCount = xr["SpectraCount"];
                float fTotalScore = 0.0f;
                float QuantitateValue = float.Parse(xr["NormalizCount"]);
                ProteinQVs.Add(strIPI, QuantitateValue);
                totalValue += QuantitateValue;
                if (QuantitateValue > maxValue)
                    maxValue = QuantitateValue;
              
                while (xr.Read())//get the child tables
                {
                    if (xr.IsStartElement() && ScanPeptide == xr.Name)
                    {
                       // dtScans.Rows.Add(new object[] { xr["PeptideSequence"], xr["ModifiedType"], xr["FinalyScore"], xr["DetaMZ"], xr["mzFile"], xr["Scan"], xr["SimilarityScore"], xr["Spectrum"], xr["Peptide"], strIPI, });
                        fTotalScore += float.Parse(xr["FinalyScore"]);
                    }
                    else
                    {                        
                        dtProteins.Rows.Add(new object[] { strIPI,strName,strOrganism ,int.Parse (strCount) ,fTotalScore });
                        break;
                    }
                } 
                
                ProteinNumber++;
            }
        }
        xr.Close();
        DataRow [] SortProteins = dtProteins.Select("",string.Format ("{0} {1}",SortBy ,SortType ));
        CreateTableHeader(SortBy,SortType );
        for (int i = 0; i < SortProteins.Length ; i++)
        {
            string strIPI = SortProteins[i].ItemArray[0].ToString ();// "Protein_ID"];
            string strName = SortProteins[i].ItemArray[1].ToString ();//"Protein_Name"];
            string strOrganism = SortProteins[i].ItemArray[2].ToString();//["Species"];
            string strCount = SortProteins[i].ItemArray[3].ToString(); //"Spectra_Count"];
            string fTotalScore = SortProteins[i].ItemArray[4].ToString();//"Total_Score"]; 
            TableRow trProtein = new TableRow();
            TableCell tcIPI = new TableCell();
            tcIPI.Text = string.Format("<a href=\"javascript:vSp1.loadPage('RightContent', 'ProteinInfo.aspx?QType=Protein ID&QValue={0}')\">{1}</a>", strIPI, strIPI);
            trProtein.Cells.Add(tcIPI);
            TableCell tcProteinName = new TableCell();
            if (strName.Length > 40)
                tcProteinName.Text = string.Format("<span Title='{0}'>{1}...</span>", strName, strName.Substring(0, 40));
            else
                tcProteinName.Text = string.Format("<span Title='{0}'>{1}</span>", strName, strName);

            trProtein.Cells.Add(tcProteinName);
            TableCell tcSpectraCount = new TableCell();
            tcSpectraCount.Text = strCount;
            tcSpectraCount.HorizontalAlign = HorizontalAlign.Center;
            trProtein.Cells.Add(tcSpectraCount);
            TableCell tcTotalScore = new TableCell();
            tcTotalScore.Text = fTotalScore.ToString().Substring(0, 4);
            tcTotalScore.HorizontalAlign = HorizontalAlign.Right;
            trProtein.Cells.Add(tcTotalScore);
            trProtein.Attributes.Add("onclick", string.Format("HighLightTR(this,'#f2e8da','#000000');hSpl.loadPage('BottomContent','PeptideIdentified.aspx?TaskID={0}&PID={1}');", TaskID, strIPI));
            tblProtein.Rows.Add(trProtein);
        }
        this.lbProteinCount.Text = ProteinNumber.ToString();
        string ProgresText = "<div class=\"graph\"><strong class=\"bar\" style=\"width: {0}%;\">{1}</strong></div>";
        string ProIPIURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}' Target='_Blank'>{1}</a>";

        var sortedDict = (from entry in ProteinQVs orderby entry.Value descending select entry);
        int count = sortedDict.Count();
        for (int i = 0; i < count; i++)
        {
            KeyValuePair<string, float> ipiv = sortedDict.ElementAt(i);
            TableRow tr = new TableRow();
            TableCell tcIPI = new TableCell();
            tcIPI.Text = string.Format(ProIPIURL, ipiv.Key, ipiv.Key);
            tcIPI.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tcIPI);
            TableCell tcProgress = new TableCell();
            tcProgress.Text = string.Format(ProgresText, (int)(ipiv.Value * 100 / maxValue), getFifthPlace(ipiv.Value / totalValue));
            tcIPI.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tcProgress);
            tbProteins.Rows.Add(tr);
        }
    }

    protected string getFifthPlace(float num)
    {
        return (String)num.ToString("N5");
    }

    private void CreateTableHeader(string SortBy, String SortType)
    {
        TableRow trCaption = new TableRow();

        TableCell tcPID = new TableCell();
        
        string SortIcon = "";
        string SortURL = "{0}<a href='ReportListView.aspx?TaskID=" + TaskID  + "&SortBy={1}&SortType={2}'>{3}</a>";
        string newSortType = "";
        if (SortBy == "Protein_ID")
        {
            if (SortType == "DESC")
            {
                SortIcon = "<img src='_image/down.png' width='10' height='10'/>";
                newSortType = "ASC";
            }
            else
            {
                SortIcon = "<img src='_image/up.png' width='10' height='10'/>";
                newSortType = "DESC";
            }
        }
        else
        {
            SortIcon = "";
            newSortType = "ASC";
        }

        tcPID.Text = string.Format (SortURL, SortIcon ,"Protein_ID",newSortType,"Protein ID" );
        tcPID.HorizontalAlign = HorizontalAlign.Left;
        tcPID.BorderStyle = BorderStyle.Solid;
        tcPID.BorderColor = System.Drawing.Color.White;
        tcPID.BorderWidth = 1;
        trCaption.Cells.Add(tcPID);

        TableCell tcPName = new TableCell();
          
        if (SortBy == "Protein_Name")
        {
            if (SortType == "DESC")
            {
                SortIcon = "<img src='_image/down.png' width='10' height='10'/>";
                newSortType = "ASC";
            }
            else
            {
                SortIcon = "<img src='_image/up.png' width='10' height='10'/>";
                newSortType = "DESC";
            }
        }
        else
        {
            SortIcon = "";
            newSortType = "ASC";
        }

        tcPName.Text = string.Format(SortURL, SortIcon, "Protein_Name", newSortType,"Protein Name");
        tcPName.HorizontalAlign = HorizontalAlign.Left;
        tcPName.BorderStyle = BorderStyle.Solid;
        tcPName.BorderColor = System.Drawing.Color.White;
        tcPName.BorderWidth = 1;
        trCaption.Cells.Add(tcPName);

        TableCell tcSCount = new TableCell();

        if (SortBy == "Spectra_Count")
        {
            if (SortType == "DESC")
            {
                SortIcon = "<img src='_image/down.png' width='10' height='10'/>";
                newSortType = "ASC";
            }
            else
            {
                SortIcon = "<img src='_image/up.png' width='10' height='10'/>";
                newSortType = "DESC";
            }
        }
        else
        {
            SortIcon = "";
            newSortType = "ASC";
        }

        tcSCount.Text = string.Format(SortURL, SortIcon, "Spectra_Count", newSortType,"S Count");
        tcSCount.HorizontalAlign = HorizontalAlign.Right;
        tcSCount.BorderStyle = BorderStyle.Solid;
        tcSCount.BorderColor = System.Drawing.Color.White;
        tcSCount.BorderWidth = 1;
        trCaption.Cells.Add(tcSCount);

        TableCell tcTotalScore = new TableCell();

        if (SortBy == "Total_Score")
        {
            if (SortType == "DESC")
            {
                SortIcon = "<img src='_image/down.png'width='10' height='10'/>";
                newSortType = "ASC";
            }
            else
            {
                SortIcon = "<img src='_image/up.png'width='10' height='10'/>";
                newSortType = "DESC";
            }
        }
        else
        {
            SortIcon = "";
            newSortType = "ASC";
        }

        tcTotalScore.Text = string.Format(SortURL, SortIcon, "Total_Score", newSortType,"Total Score");
        tcTotalScore.HorizontalAlign = HorizontalAlign.Right;
        tcTotalScore.BorderStyle = BorderStyle.Solid;
        tcTotalScore.BorderColor = System.Drawing.Color.White;
        tcTotalScore.BorderWidth = 1;
        trCaption.Cells.Add(tcTotalScore);
        trCaption.Height = new Unit("24");
        trCaption.CssClass = "DataGridHeader";
        //trCaption.BackColor = System.Drawing.Color.FromArgb(222, 237, 251);
        //trCaption.ForeColor = System.Drawing.Color.FromArgb(50, 105, 155);//"#DEEDFB" ForeColor="#32699B" 
        this.tblProtein.Rows.Add(trCaption);
    }
}
