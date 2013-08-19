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
using System.IO;
using System.Web.Configuration;
using ZJU.COPLib;
// Mail Fn :: allows access to SMTPClient, MailMessage and NetworkCredential
using System.Net;
using System.Net.Mail;

using System.Windows.Forms;

//timer

using System;
using System.Timers;

public partial class SearchReport : System.Web.UI.Page
{

     protected void Page_Load(object sender, EventArgs e)
    {
        string QueryType = Request.QueryString["QType"];

        string QueryValue = Request.QueryString["QValue"];

        if ((!Page.IsPostBack) && QueryValue != null && QueryType != null)
        {
            Query(QueryType, QueryValue);
            LoadSearchingCondition(QueryValue);

        }

    }

    private void sendEmail(String userEmail, string taskID)
    {
        String email = "jarbitoapple@gmail.com";
        String pass = "giantmousecardgift";
        NetworkCredential cred = new NetworkCredential(email, pass); // Login for email account to send email from
        MailMessage msg = new MailMessage();                         // Create new email
        msg.To.Add(userEmail);                                       // To email address
        msg.From = new MailAddress(email);                           // This is the from email
        msg.Subject = "Your COPaKB Analysis Report Finished";
        msg.Body = "Hello, your COPaKB task ID: <strong>" + taskID + "</strong> has finished. <br />" +
                   "You may view the report <a href=\"http://www.heartproteome.org/copa_dev/SearchReport.aspx?QType=" + userEmail + "&QValue=" + taskID + "\" >here</a> <br />" +
                   "<br /><br />- - - - - - - - - - - - - - - - - - - - <br />" +
                   "© COPaKB 2013 | Cardiovascular Research Laboratory<br />" +
                   "Info@HeartProteome.org<br />" +
                   "P: 1(310)267-5624<br />" +
                   "F: 1(310)267-5623";
        msg.IsBodyHtml = true;

        SmtpClient client = new SmtpClient("smtp.gmail.com", 25);    // smtp host for gmail and port 25 is usually default

        client.Credentials = cred; // Send our account login details to the client.
        client.EnableSsl = true;   // Reqiured by most email providers to send mail,
        client.Send(msg);          // Send our email.
    }

    void LoadSearchingCondition(string TaskID)
    {

        string scLocation = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\searchcondition.xml";
        this.con1.Text = scLocation;
        FileInfo fi = new FileInfo(scLocation);

        if (fi.Exists)
        {

            this.con3.Text = "enter";
            SearchingParameters mySP = new SearchingParameters(scLocation);
            if (mySP.HighResolution)
                this.lbPeptideTolerance.Text = string.Format("{0}ppm consider {1} isotope peaks", mySP.PeptideToleranceHR, mySP.IsotopePeaks);
            else
                this.lbPeptideTolerance.Text = mySP.Peptidetolerance.ToString() + " Th (Da/e)";
            this.lbSlideSize.Text = mySP.SlideSize.ToString();
            if (mySP.UseNoiseLibrary)
                this.lbIsUsingNoiseDecoy.Text = "True";
            else
                this.lbIsUsingNoiseDecoy.Text = "False";
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

        }
        else
        {
            this.con3.Text = "fail";
        }

    }
    /*
    private void sendMailTo(String userEmail)
    {
        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        message.To.Add("kpnamja87@gmail.com");
        message.Subject = "COPaKB WebSys Alert : Your Report is Finished!";
        message.From = new System.Net.Mail.MailAddress("jarbitoapple@gmail.com");
        message.Body = "This is the message body you silly man!" + userEmail;
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
        smtp.Send(message);
    }*/

    void Query(string strUserID, string strTaskID)
    {
        string strSQL = "";
        strSQL = string.Format("select task_seq,task_user,upload_time,task_status,upload_filename, search_model,report_filename from search_task where task_user='{0}' and task_seq={1}", strUserID, strTaskID);

        string ReportFileName = "";
        string mzFileName = "";
        string TaskID = "";
        int TaskStatus = 0;
        int SpecialTaskStatus = 0;
        string TaskIDURL = "<a href='SearchReport.aspx?QType=TaskID&QValue={0}' Target='_blank'>{1}</a>";
        //DBInterface.ConnectDB();
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result == null || result.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("MSRUNSearch.aspx?MSG=1");
            return;
        }
        if (result != null)
        {
            bool bHaveUserTasks = false;

            for (int i = 0; i < result.Tables[0].Rows.Count; i++)
            {
                TableRow trCaption = new TableRow();
                TableCell tcTaskID = new TableCell();
                TaskID = result.Tables[0].Rows[i].ItemArray[0].ToString();


                int searchStatInt = 0;
                taskidinfo.Text = result.Tables[0].Rows[i].ItemArray[0].ToString();
                user.Text = result.Tables[0].Rows[i].ItemArray[1].ToString();
                string timeOfServer = ((DateTime)result.Tables[0].Rows[i].ItemArray[2]).ToShortDateString() + " " + ((DateTime)result.Tables[0].Rows[i].ItemArray[2]).ToShortTimeString();
                time.Text = string.Format(@"<script type=""text/javascript"" src=""./assets/js/moment.min.js""></script>
                    <script type=""text/javascript"">

var date = moment('{0}' + ' -0700', 'MM/DD/YYYY HH:mm Z').toString();
    var n=date.indexOf('GMT');
    var formatDate = '';
    formatDate = date.substring(0,n);
    document.write(formatDate);

</script>", timeOfServer);

                String mzmlfilePath = result.Tables[0].Rows[i].ItemArray[4].ToString();
                int index3 = mzmlfilePath.LastIndexOf('/');
                if (index3 != -1 && index3 < mzmlfilePath.Length)
                    mzmlFile.Text = mzmlfilePath.Substring(index3 + 1);
                else
                    mzmlFile.Text = mzmlfilePath;
                searchStatInt = int.Parse(result.Tables[0].Rows[i].ItemArray[3].ToString());
                switch (searchStatInt)
                {
                    case -1:
                    case -2:
                        searchStat.Text = "<div class=\"alert alert-error\">Error in reading file</div>";
                        break;
                    case 0:
                        searchStat.Text = "<div class=\"span4\"><img src=\"assets/img/ajax-loader.gif\"/></div><div class=\"span8\"> <div class=\"alert alert-info\">File Uploading ...</div></div>";

                        break;
                    case 1:
                        searchStat.Text = "File Uploaded";
                        break;
                    case 2:
                        searchStat.Text = "<div class=\"span4\"><img src=\"assets/img/ajax-loader.gif\"/></div><div class=\"span8\"> <div class=\"alert alert-info\">Library Searching ...</div></div>";
                        lbMessage.Text = "The library search is ongoing, you can refresh this page to check the progress.";
                        break;
                    case 8:
                        searchStat.Text = "<div class=\"span4\"><img src=\"assets/img/ajax-loader.gif\"/></div><div class=\"span8\"> <div class=\"alert alert-block\">Preparing Result Files ...</div></div>";
                        lbMessage.Text = "The library search has finished. Creating Report. You can refresh this page to check the progress.";
                        break;
                    case 9:
                        searchStat.Text = string.Format("<div class=\"alert alert-success\">Completed<br/><a href=\"ReportView.aspx?TaskID={0}\" target=\"_Blank\">View Report</a></div>", TaskID);
                        //sendEmail(user.Text,taskidinfo.Text); // A test of the send Email Fn
                        break;
                }
                String module = result.Tables[0].Rows[i].ItemArray[5].ToString();
                int index4 = module.IndexOf('.');
                if (index4 != -1 && index4 < module.Length)
                    copakbModule.Text = module.Substring(index4 + 1);
                else
                    copakbModule.Text = module;

                tcTaskID.Text = TaskID;//string.Format(TaskIDURL, TaskID, TaskID);
                tcTaskID.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcTaskID);
                TableCell tcUser = new TableCell();
                tcUser.Text = result.Tables[0].Rows[i].ItemArray[1].ToString();
                tcUser.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcUser);
                TableCell tcTaskTime = new TableCell();
                tcTaskTime.Text = "PT " + ((DateTime)result.Tables[0].Rows[i].ItemArray[2]).ToShortDateString() + " " + ((DateTime)result.Tables[0].Rows[i].ItemArray[2]).ToShortTimeString();
                tcTaskTime.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcTaskTime);
                TableCell tcMzFile = new TableCell();
                mzFileName = result.Tables[0].Rows[i].ItemArray[4].ToString();
                tcMzFile.Text = mzFileName.Replace("|", "<br/>");
                tcMzFile.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcMzFile);
                TableCell tcStatus = new TableCell();
                TaskStatus = int.Parse(result.Tables[0].Rows[i].ItemArray[3].ToString());
                if (TaskID == strTaskID)
                    SpecialTaskStatus = TaskStatus;
                switch (TaskStatus)
                {
                    case -1:
                    case -2:
                        tcStatus.Text = "<div class=\"alert alert-error\">Error in reading file</div>";
                        break;
                    case 0:
                        tcStatus.Text = "<img src=\"assets/img/ajax-loader.gif\"/> <div class=\"alert alert-info\">File Uploading ...</div>";

                        break;
                    case 1:
                        tcStatus.Text = "File Uploaded";
                        break;
                    case 2:
                        tcStatus.Text = "<img src=\"assets/img/ajax-loader.gif\"/> <div class=\"alert alert-info\">Library Searching ...</div>";
                        lbMessage.Text = "The library search is ongoing, you can refresh this page to check the progress.";
                        break;
                    case 9:
                        tcStatus.Text = string.Format("<div class=\"alert alert-success\">Completed<br/><a href=\"ReportView.aspx?TaskID={0}\" target=\"_Blank\">View Report</a></div>", TaskID);

                        //Session["TaskID"] = TaskID;
                        //lbProteinView.Text = string.Format("<a href=\"ReportView.aspx?TaskID={0}\" target=\"_Blank\">View the Protein View Report", TaskID);
                        break;
                }

                tcStatus.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcStatus);
                TableCell tcSearchModel = new TableCell();
                tcSearchModel.Text = result.Tables[0].Rows[i].ItemArray[5].ToString();
                tcSearchModel.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcSearchModel);
                //this.tbTasks.Rows.Add(trCaption);
                ReportFileName = result.Tables[0].Rows[i].ItemArray[6].ToString();
                bHaveUserTasks = true;
            }
        }
        if (SpecialTaskStatus >= 2)
        {
            string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}&File={3}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
            string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
            //get the report file
            string ReportPath = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + strTaskID + "\\Result.txt";// +ReportFileName;
            if (File.Exists(ReportPath))
            {
                FileStream fs = File.Open(ReportPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //the sample line:
                    //Y 349 CoPep00035959 0.686646308289647 0.345257544453165 0.521414394928674 0.50019999999995 60989 ILQEGVDPK
                    //N 369
                    string[] tokens = line.Split(new string[] { "|" }, StringSplitOptions.None);
                    if (tokens[0] == "Y")
                    {
                        TableRow trCaption = new TableRow();
                        TableCell tcScan = new TableCell();
                        tcScan.Text = tokens[1];
                        tcScan.HorizontalAlign = HorizontalAlign.Left;
                        trCaption.Cells.Add(tcScan);

                        /* 
                        TableCell tcPepSeq = new TableCell();
                        tcPepSeq.Text = tokens[8];
                        tcPepSeq.HorizontalAlign = HorizontalAlign.Left;
                        trCaption.Cells.Add(tcPepSeq);
                          
                        string ModifiedType = "";
                        if (tokens.Length > 9)
                            ModifiedType = tokens[9];
                        TableCell tcModifiedType = new TableCell();
                        if (ModifiedType == "NULL")
                            ModifiedType = "";
                        if (ModifiedType.Length > 10)
                            tcModifiedType.Text = string.Format("<span title='{0}'>{1}...</span>", ModifiedType, ModifiedType.Substring(0, 10));
                        else
                            tcModifiedType.Text = ModifiedType;
                        tcModifiedType.HorizontalAlign = HorizontalAlign.Left;
                        trCaption.Cells.Add(tcModifiedType);
                        */

                        TableCell tcPeptide = new TableCell();
                        tcPeptide.Text = string.Format(PepIDURL, tokens[2], tokens[2]);
                        tcPeptide.HorizontalAlign = HorizontalAlign.Left;
                        trCaption.Cells.Add(tcPeptide);

                        //TableCell tcDP = new TableCell();
                        //tcDP.Text = tokens[3];
                        //tcDP.HorizontalAlign = HorizontalAlign.Right;
                        //trCaption.Cells.Add(tcDP);
                        //TableCell tcDB = new TableCell();
                        //tcDB.Text = tokens[4];
                        //tcDB.HorizontalAlign = HorizontalAlign.Right;
                        //trCaption.Cells.Add(tcDB);
                        TableCell tcSS = new TableCell();
                        string ss = "";
                        if (tokens.Length > 5)
                            ss = tokens[5];
                        if (ss.Length > 6)
                            tcSS.Text = ss.Substring(0, 6);
                        else
                            tcSS.Text = ss;
                        tcSS.HorizontalAlign = HorizontalAlign.Center;

                        trCaption.Cells.Add(tcSS);

                        TableCell tcDetaM = new TableCell();
                        string detaM = "";
                        if (tokens.Length > 6)
                            detaM = tokens[6];
                        if (detaM.Length > 6)
                            tcDetaM.Text = detaM.Substring(0, 6);
                        else
                            tcDetaM.Text = detaM;
                        tcDetaM.HorizontalAlign = HorizontalAlign.Center;
                        trCaption.Cells.Add(tcDetaM);
                        TableCell tcFS = new TableCell();
                        string FS = "";
                        if (tokens.Length > 10)
                            FS = tokens[10];
                        if (FS.Length > 6)
                            tcFS.Text = FS.Substring(0, 6);
                        else
                            tcFS.Text = FS;
                        tcFS.HorizontalAlign = HorizontalAlign.Center;
                        trCaption.Cells.Add(tcFS);

                        TableCell tcSpectrum = new TableCell();
                        tcSpectrum.Text = string.Format(SpectrumURL, tokens[7], tokens[1], TaskID, tokens[11]);
                        tcSpectrum.HorizontalAlign = HorizontalAlign.Center;
                        trCaption.Cells.Add(tcSpectrum);

                        this.tbReport.Rows.Add(trCaption);


                    }
                    else
                    {
                        //TableRow trCaption = new TableRow();
                        //TableCell tcScan = new TableCell();
                        //tcScan.Text = tokens[1];
                        //tcScan.HorizontalAlign = HorizontalAlign.Left;
                        //trCaption.Cells.Add(tcScan);

                        //TableCell tcPeptide = new TableCell();
                        //tcPeptide.Text = "";
                        //tcPeptide.HorizontalAlign = HorizontalAlign.Left;
                        //trCaption.Cells.Add(tcPeptide);
                        //TableCell tcDP = new TableCell();
                        //tcDP.Text = "";
                        //tcDP.HorizontalAlign = HorizontalAlign.Right;
                        //trCaption.Cells.Add(tcDP);
                        //TableCell tcDB = new TableCell();
                        //tcDB.Text = "";
                        //tcDB.HorizontalAlign = HorizontalAlign.Right;
                        //trCaption.Cells.Add(tcDB);
                        //TableCell tcFS = new TableCell();
                        //tcFS.Text = "";
                        //tcFS.HorizontalAlign = HorizontalAlign.Right;
                        //trCaption.Cells.Add(tcFS);
                        //TableCell tcSpectrum = new TableCell();
                        //tcSpectrum.Text = "";
                        //tcSpectrum.HorizontalAlign = HorizontalAlign.Right;
                        //trCaption.Cells.Add(tcSpectrum);
                        ////trCaption.BackColor = System.Drawing.Color.LightGray  ;
                        //this.tbReport.Rows.Add(trCaption);
                    }


                }

                sr.Close();
            }
        }
    }

    //bool GetModifedPeptide(string PTMseq, ref string peptideSeq, ref string ModifiedType)
    //{
    //    string strSQL = string.Format(" select modified_sequence, type_of_modification from ptm_tbl t, spectrum_tbl t2 where t2.spectrum_seq = {0}  and t.ptm_seq = t2.ptm_seq ", PTMseq);
    //    DBInterface.ConnectDB();
    //    System.Data.Common.DbDataReader result = DBInterface.QuerySQL(strSQL);

    //    if (result != null)
    //    {
    //        if (result.Read())
    //        {
    //            peptideSeq = result.GetString(0);
    //            if (result.IsDBNull(1))
    //                ModifiedType = "";
    //            else
    //                ModifiedType = result.GetString(1);
    //            return true;
    //        }
    //    }
    //    return false;
    //}


}
