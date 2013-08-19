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
using ZJU.COPLib;
using System.Collections;
using ZJU.COPDB;

using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using gov.nih.nlm.ncbi.eutils;


public partial class epiSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        this.findTasks(this.tbQueryUserID.Text);
    }
    void findTasks(string loc)
    {
        string taskID = "";
        taskID = this.tbQueryUserID.Text;
        taskID = taskID.Replace("\"", "");
        taskID = taskID.Replace("\'", "");
        //this.tbQueryUserID.Text = taskID;
        if (this.tbQueryUserID.Text.Equals(""))
        {
            Response.Redirect(string.Format("epiTestNoMk.aspx?Tasks={0}", taskID));
        }
        else
        {
            Response.Redirect(string.Format("epiTest.aspx?Tasks={0}&Markers={1}", taskID, this.tbQueryTaskID.Text));
        }
    }
}


