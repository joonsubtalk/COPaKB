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

using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using ZJU.COPLib;
using ZJU.COPDB;

public partial class COPaWikiDefault : System.Web.UI.Page
{
    string m_PageName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        GetWikiPageName();
        if (m_PageName == "")
        {
            lbPageName.Text = "COPaKB Wiki Home";
            m_PageName = "COPaKB Wiki Home";
            //lbEdit.Visible = false;
            //lbViewHistory.Visible = false;
            WikiPage wikiPage = GetWikiPage();
            if (wikiPage != null)
            {
                SetPageContent(wikiPage);
            }
            LoadNewPages();
        }
        else
        {
            Page.Title = "COPaKB Wiki:"+m_PageName;
            lbPageName.Text = m_PageName;
            pNewPages.Visible = false;

            bool UseAuthentication = Convert.ToBoolean(ConfigurationManager.AppSettings["UseAuthentication"]);

            if (UseAuthentication && WikiOperations.IsPagePrivate(m_PageName) && !User.IsInRole("Super User"))
            {
                //They don't have rights to view this page
                Response.Redirect("~/Login.aspx");
            }

            WikiPage wikiPage = GetWikiPage();
            if (wikiPage != null)
            {
                SetPageContent(wikiPage);
            }
            else
            {
                bool AllowAnonCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowAnonymousPageCreation"]);

                if (UseAuthentication && !AllowAnonCreate && !User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Login.aspx");
                }

                wikiPage = new WikiPage();
                wikiPage.PageName = m_PageName;
                wikiPage.PageContent = string.Format("There is no Wiki page on record for '{0}', please feel free to create one.", m_PageName);
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
                SetPageContent(wikiPage);
            }
        }
    }
    private void SetPageContent(WikiPage wikiPage)
    {
        WikiTextFormatter wf = new WikiTextFormatter();

        litContent.Text = wf.FormatPageForDisplay(wikiPage.PageContent);
    }
    private WikiPage GetWikiPage()
    {
        return WikiOperations.GetWikiPage(m_PageName);
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
        }
    }

    private void LoadNewPages()
    { 
        string PageNameURL = "<a href='COPaWikiDefault.aspx?PageName={0}'>{0}</a>";
        //string strSQL = "select * from (select a.pagename,a.lastmodified,a.modifiedby from wiki_pages a where ((a.pagename like '%%') order by lastmodified desc) where rownum<10";
        string strSQL = "select * from (select a.pagename, a.lastmodified, a.modifiedby from wiki_pages a where not regexp_like(a.pagename, '^([A-Z0-9]{6}(-[0-9]+)?|[0-9]+|[A-Z]+)$') order by a.lastmodified desc) where rownum < 10";
        if (DBInterface.DbType == "MySql")
            strSQL = "select a.pagename,a.lastmodified,a.modifiedby from wiki_pages a where  not ((a.pagename like 'CoPro%' or a.pagename like 'CoPep%'  ) and length(a.pagename)=13)   order by lastmodified desc limit 10";
        //DBInterface.ConnectDB();
        IDataReader result = DBInterface.QuerySQL(strSQL);
        int i = 0;
        if (result != null)
        {

            while (result.Read())
            {
                TableRow trCaption = new TableRow();

                TableCell tcPagename = new TableCell();
                tcPagename.Text = string.Format(PageNameURL, result.GetString(0));
                tcPagename.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcPagename);

                

                TableCell tcLastModifiedTime = new TableCell();
                tcLastModifiedTime.Text = result.GetDateTime(1).ToLongDateString();
                tcLastModifiedTime.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcLastModifiedTime);

                TableCell tcEditor = new TableCell();
                tcEditor.Text = result.GetString(2);
                tcEditor.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcEditor);


                tbNewPages.Rows.Add(trCaption);
                i += 1;
            }
            result.Close();


        }
        
        string ProteinIDURL = "<a href='ProteinInfo.aspx?QType=Protein+ID&QValue={0}'>{1}</a>";
        strSQL = "select * from (select a.pagename,b.protein_name,a.lastmodified,a.modifiedby from wiki_pages a, protein_tbl b where trim(b.protein_cop_id) = trim(a.pagename)  order by lastmodified desc) where rownum<10";
        if (DBInterface.DbType == "MySql")
            strSQL = "select a.pagename,b.protein_name,a.lastmodified,a.modifiedby from wiki_pages a, protein_tbl b where  a.pagename like 'IPI%' and trim(b.protein_cop_id) = trim(a.pagename)  order by lastmodified desc limit 10";
     
        //DBInterface.ConnectDB();
        result = DBInterface.QuerySQL(strSQL);
       
        if (result != null)
        {

            while (result.Read())
            {
                TableRow trCaption = new TableRow();

                TableCell tcPagename = new TableCell();
                tcPagename.Text = string.Format(ProteinIDURL, result.GetString(0), result.GetString(0));
                tcPagename.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcPagename);

                TableCell tcLastModifiedTime = new TableCell();
                tcLastModifiedTime.Text = result.GetDateTime(2).ToLongDateString();
                tcLastModifiedTime.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcLastModifiedTime);

                TableCell tcEditor = new TableCell();
                tcEditor.Text = result.GetString(3);
                tcEditor.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcEditor);


                tbProteinPages.Rows.Add(trCaption);
               
            }
            result.Close();


        }
        
        string PeptideIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
        strSQL = "select * from (select a.pagename,b.peptide_sequence,a.lastmodified,a.modifiedby from wiki_pages a, peptide_tbl b where  trim(b.peptide_sequence) = trim(a.pagename)  order by lastmodified desc) where rownum<10";
        if (DBInterface.DbType == "MySql")
            strSQL = "select a.pagename,b.peptide_sequence,a.lastmodified,a.modifiedby from wiki_pages a, peptide_tbl b where  a.pagename like 'CoPep%' and trim(b.peptide_cop_id) = trim(a.pagename) order by lastmodified desc limit 10";
     
        result = DBInterface.QuerySQL(strSQL);
        
        if (result != null)
        {

            while (result.Read())
            {
                TableRow trCaption = new TableRow();

                TableCell tcPagename = new TableCell();
                tcPagename.Text = string.Format(PeptideIDURL, result.GetString(0), result.GetString(0));
                tcPagename.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcPagename);

                TableCell tcLastModifiedTime = new TableCell();
                tcLastModifiedTime.Text = result.GetDateTime(2).ToLongDateString();
                tcLastModifiedTime.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcLastModifiedTime);

                TableCell tcEditor = new TableCell();
                tcEditor.Text = result.GetString(3);
                tcEditor.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcEditor);


                tbPeptidePages.Rows.Add(trCaption);
                
            }
            result.Close();
        }


        string SpectrumURL = "<a href='SpectrumInfo.aspx?QValue={0}' Target='_blank'>{0}</a>";
        strSQL = "select a.pagename,b.instrumentation,a.lastmodified,a.modifiedby from wiki_pages a, spectrum_tbl b where to_char(b.spectrum_seq) = a.pagename and rownum < 10 order by lastmodified desc";
        if (DBInterface.DbType == "MySql")
            strSQL = "select a.pagename,b.instrumentation,a.lastmodified,a.modifiedby from wiki_pages a, spectrum_tbl b where to_char(b.spectrum_seq) = a.pagename order by lastmodified desc limit 10";
     
        result = DBInterface.QuerySQL(strSQL);
       
        if (result != null)
        {

            while (result.Read())
            {
                TableRow trCaption = new TableRow();

                TableCell tcPagename = new TableCell();
                tcPagename.Text = string.Format(SpectrumURL, result.GetString(0));
                tcPagename.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcPagename);

                TableCell tcLastModifiedTime = new TableCell();
                tcLastModifiedTime.Text = result.GetDateTime(2).ToLongDateString();
                tcLastModifiedTime.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcLastModifiedTime);

                TableCell tcEditor = new TableCell();
                tcEditor.Text = result.GetString(3);
                tcEditor.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcEditor);


                tbSpectrumPages.Rows.Add(trCaption);
                
            }
         
            result.Close();
         
        }
         
        DBInterface.CloseDB();
         
       
    }
    protected void lbEdit_Click(object sender, EventArgs e)
    {
        Response.Redirect("WikiEdit.aspx?PageName=" + m_PageName);
    }
    protected void lbViewHistory_Click(object sender, EventArgs e)
    {
        Response.Redirect("WikiHistory.aspx?PageName=" + m_PageName);
    }
}
