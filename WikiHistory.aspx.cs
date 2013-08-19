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
using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using ZJU.COPLib;


public partial class WikiHistory : System.Web.UI.Page
{
    string m_PageName = "";
    string m_VersionID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        GetWikiPageName();
        lbPageName.Text = string.Format ("<a href='COPaWikiDefault.aspx?PageName={0}'>{0}</a>", m_PageName);
        Page.Title = "COPaKB Wiki History:" + m_PageName;
        ListHistoryVersion();
        LoadVersionContent();
    }

    private void GetWikiPageName()
    {
        m_PageName = "";

        foreach (string Key in Request.QueryString.Keys)
        {
            if (Key.ToUpper() == "PAGENAME")
            {
                m_PageName = Request.QueryString[Key];
            }
            if (Key.ToUpper() == "VID")
            {
                m_VersionID = Request.QueryString[Key];
            }
        }
    }

    private void ListHistoryVersion()
    {
        string VersionIDURL = "<a href='WikiHistory.aspx?PageName={0}&VID={1}'>{1}</a>";
        string strSQL = string.Format("select pagehistoryid,modifydate,modifiedby from wiki_pagehistory where pagename='{0}' order by modifydate desc", m_PageName);;
        //DBInterface.ConnectDB();
        IDataReader result = DBInterface.QuerySQL(strSQL);
        int i = 0;
        if (result != null)
        {

            while (result.Read())
            {
                TableRow trCaption = new TableRow();

                TableCell tcVersion = new TableCell();
                tcVersion.Text = string.Format(VersionIDURL, m_PageName, result.GetValue(0).ToString ());
                tcVersion.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcVersion);

                TableCell tcVersionTime = new TableCell();
                tcVersionTime.Text = result.GetDateTime(1).ToString("MM/dd/yy H:mm:ss zzz");
                tcVersionTime.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcVersionTime);

                
                TableCell tcEditor = new TableCell();
                tcEditor.Text = result.GetString(2);
                tcEditor.HorizontalAlign = HorizontalAlign.Left ;
                trCaption.Cells.Add(tcEditor);

                if (i == 0 && m_VersionID == "")
                {
                    m_VersionID = result.GetValue(0).ToString();
                    trCaption.BackColor = System.Drawing.Color.LightBlue;
                }
                else if ( m_VersionID == result.GetValue(0).ToString ())
                {
                    trCaption.BackColor = System.Drawing.Color.LightBlue;
                }
                tbVersionList.Rows.Add(trCaption);
                i += 1;
            }
            result.Close();


        }
        DBInterface.CloseDB();

    }

    private void LoadVersionContent()
    {
        string strSQL = string.Format("select pagecontent from wiki_pagehistory where pagehistoryid='{0}' ", m_VersionID); ;
        //DBInterface.ConnectDB();
        IDataReader result = DBInterface.QuerySQL(strSQL);
     
        if (result != null)
        {
            while (result.Read())
            {
                WikiTextFormatter wf = new WikiTextFormatter();
                litContent.Text = wf.FormatPageForDisplay(result.GetString(0) );
            }
            result.Close();

        }
        DBInterface.CloseDB();

    }


}
