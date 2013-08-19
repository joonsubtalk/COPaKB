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
using System.Web.Configuration;
using System.IO;
using System.Net;
using ZJU.COPLib;

public partial class MSRUNSearch : System.Web.UI.Page
{
    string NoInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        string queryShow = Request.QueryString["showInfo"];
        NoInfo = queryShow;

        if (NoInfo == "T")
        { // collapse
            informationShow.Text = @"<div id='helpNote' class='collapse'>";
            informationBtn.Text = @"<button type='button' class='close button collapsed' data-toggle='collapse' data-target='#helpNote'>&times;</button>";
        }
        else
        {
            informationShow.Text = @"<div id='helpNote' class='collapse in'>";
            informationBtn.Text = @"<button type='button' class='close button' data-toggle='collapse' data-target='#helpNote'>&times;</button>";
        }

        cbUseDefault.Attributes.Add("onclick", "return cbevent();");
        cbUseURL.Attributes.Add("onclick", "return UsingURL();");
        cbHighResolution.Attributes.Add("onclick", "return HighResolution();");
        string strMsg = Request.QueryString["MSG"];
        if (strMsg != "" && strMsg != null)
            this.lbReportError.Text = "No report availabe for your query!";
        //task ID was not found in the task_seq table
        if (cbUseURL.Checked)
        {
            this.InputFile1.Style.Remove("display");
            this.InputFile1.Style.Add("display", "none");
            this.URLPanel.Style.Remove("display");
            this.URLPanel.Style.Add("display", "block");
            //call url panel and disable file browse panel
        }
        if (!cbUseDefault.Checked)
        {
            this.ParameterPanel.Style.Remove("display");
            this.ParameterPanel.Style.Add("display", "block");
            //call in detailed settings panel
        }

        //btUpload.Attributes.Add("onclick", "return ShowProgress();");
        if (!Page.IsPostBack)
        {

            this.ddlModels.Items.Clear();
            COPaWS.COPaWS ws = new COPaWS.COPaWS();
            NetworkCredential objCredential = new NetworkCredential(ConfigurationSettings.AppSettings["COPaWSUser"], ConfigurationSettings.AppSettings["COPaWSPassword"]);
            ws.Credentials = objCredential;
            string[] Mods = ws.GetLibraryModules();
            foreach (string mod in Mods)
            {
                this.ddlModels.Items.Add(mod);
            }
            this.ddlModels.Text = this.ddlModels.Items[0].Text;
            //initializing dropdown menu
        }
    }
    protected void btUpload_Click(object sender, EventArgs e)
    {
        //if (this.tbUserID.Text == "")
        //{
        //    this.lbEmailMessage1.Text = "Required field.";
        //    return;
        //}
        //else
        //    this.lbEmailMessage1.Text = "*";

        if (!this.RegularExpressionValidator2.IsValid || !this.RegularExpressionValidator1.IsValid)
        {
            return;
            //validation group for mzMl or raw file
        }

        if (!cbUseURL.Checked && !this.InputFile1.HasFile)
            return;
        if (cbUseURL.Checked && this.tbURL.Text == "")
        {
            this.lbMessage.Text = "You need to upload a local file or provide the URL of a file in the web.";
            return;
        }

        string FileName = "";
        string SingleFileName = "";

        if (cbUseURL.Checked)
        {
            FileName = this.tbURL.Text;
            SingleFileName = FileName.Substring(FileName.LastIndexOf("/") + 1);
        }
        else
        {
            FileName = this.InputFile1.FileName;
            SingleFileName = FileName;
        }



        string strSQL = "insert into search_task (task_user,upload_filename,search_model,report_filename) values ('{0}','{1}','{2}','{3}')";
        string strReportName = SingleFileName.Substring(0, SingleFileName.LastIndexOf(".")) + ".txt";
        strSQL = string.Format(strSQL, DBInterface.SQLValidString(this.tbUserID.Text), DBInterface.SQLValidString(FileName), this.ddlModels.Text, DBInterface.SQLValidString(strReportName));
        //DBInterface.ConnectDB();
        string taskID = "";
        if (DBInterface.UpdateSQL(strSQL) > 0)
        {
            strSQL = "select task_seq from search_task where task_user='{0}' and  upload_filename = '{1}' order by task_seq desc";
            strSQL = string.Format(strSQL, DBInterface.SQLValidString(this.tbUserID.Text), DBInterface.SQLValidString(FileName));
            IDataReader result = DBInterface.QuerySQL(strSQL);
            if (result != null)
            {
                result.Read();
                taskID = result.GetValue(0).ToString();
            }
            DBInterface.CloseDB();
            string UploadPath = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + taskID + "\\";

            DirectoryInfo newpath = new DirectoryInfo(UploadPath);
            if (!newpath.Exists)
            {
                newpath.Create();
            }
            if (!cbUseURL.Checked)
            {
                string TargeFile = UploadPath + FileName;
                this.InputFile1.MoveTo(TargeFile, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
            }
            //***save the searching conditions in files***
            SearchingParameters mySP = new SearchingParameters();
            if (cbUseDefault.Checked)
            {
                mySP.ConfidenceLevel = 0.95F;
                mySP.DistinctPeptide = 1;
                mySP.LibraryModule = this.ddlModels.Text;
                mySP.OnlyTop1 = true;
                mySP.Peptidetolerance = 2;
                mySP.PTMShift = 0;
                mySP.SlideSize = 1;
                mySP.ScoreCriteria = "|0;-1";
                mySP.UseNoiseLibrary = true;
                mySP.UseStatisticalMode = true;
                mySP.HighResolution = false;
                mySP.PeptideToleranceHR = 20;
                mySP.IsotopePeaks = 2;
                mySP.BonusDetaMz = false;
                mySP.CustomizedLibraryFDR = false;
                mySP.UniquePeptide = 0;
                mySP.UsePTMModule = false;
            }
            else
            {
                try
                {
                    mySP.ConfidenceLevel = float.Parse(this.ddl_Threshold.Text.Substring(0, this.ddl_Threshold.Text.Length - 1)) / 100;
                    if (mySP.ConfidenceLevel >= 1 || mySP.ConfidenceLevel <= 0)
                    {
                        lbMessage.Text = "The peptide confidence should be set in the range 0%-100%.";
                        return;
                    }
                }
                catch
                {
                    lbMessage.Text = "The peptide confidence setting is out of range.";
                    return;
                }
                mySP.DistinctPeptide = int.Parse(this.ddl_DistinctPepties.Text);// sc.DistinctPeptide;
                mySP.UniquePeptide = int.Parse(this.ddl_UniquePeptides.Text);
                mySP.LibraryModule = this.ddlModels.Text;// GetModuleName(sc.searchModule);
                mySP.OnlyTop1 = true;// sc.bOnlyTop1;
                mySP.Peptidetolerance = float.Parse(this.ddl_PrecursorTolerance.Text);//sc.fPrecursorWindow;
                mySP.PTMShift = float.Parse(this.ddl_PTMshift.Text);// sc.fPTMShift;
                string Criterias = "|0;-1";
                //foreach (SearchingThreadPair stp in sc.NormalSearchCondition)
                //{
                //    Criterias += string.Format("|{0};{1}", stp.MatchScore, stp.DetaDecoyScore);
                //}
                mySP.ScoreCriteria = Criterias;
                mySP.SlideSize = int.Parse(this.ddl_SlideSize.Text);// sc.iSlideSize;
                mySP.UseNoiseLibrary = this.cbUseNDP.Checked;// sc.bUseNoiseLibrary;
                mySP.UseStatisticalMode = true;// sc.bStatisticSearching;
                mySP.HighResolution = this.cbHighResolution.Checked;
                mySP.PeptideToleranceHR = float.Parse(this.ddlHRPrecursorTolerance.Text);
                mySP.IsotopePeaks = int.Parse(this.ddlIsotopePeaks.Text);
                mySP.BonusDetaMz = this.cbBonusMS.Checked;
                mySP.CustomizedLibraryFDR = false;
                mySP.UsePTMModule = false;
            }
            string sclocation = UploadPath + "searchcondition.xml";
            mySP.SaveSettings(sclocation);
            //*****************************************
            strSQL = "update search_task set task_status=1 where task_seq=" + taskID;
            if (DBInterface.UpdateSQL(strSQL) > 0)
            {
                if (cbUseURL.Checked)
                {
                    informationShow.Text = @"<div id='helpNote' class='collapse'>";
                    informationBtn.Text = @"<button type='button' class='close button collapsed' data-toggle='collapse' data-target='#helpNote'>&times;</button>";
                    lbMessage.Text = "You URL resource has been submitted. The task ID is <a href=\"SearchReport.aspx?QType=" + this.tbUserID.Text + "&QValue=" + taskID + "\" style=\"font-size: large; color: #FF0000\" >" + taskID + "</a>.";
                    //this.cbUseURL.Checked = false;
                }
                else
                {
                    lbMessage.Text = "You mzML file has been successfully uploaded. The  task ID is <a href=\"SearchReport.aspx?QType=" + this.tbUserID.Text + "&QValue=" + taskID + "\" style=\"font-size: large; color: #FF0000\" >" + taskID + "</a>.";
                }
            }
        }

    }
    protected void Button3_Click(object sender, EventArgs e)
    {

        string url = "SearchReport.aspx?";
        url += string.Format("QType={0}&", this.tbQueryUserID.Text);
        url += "QValue=" + this.tbQueryTaskID.Text;
        Response.Redirect(url);

    }
    protected void cbUseDefault_CheckedChanged(object sender, EventArgs e)
    {

    }
}
