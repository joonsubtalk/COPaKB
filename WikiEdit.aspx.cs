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
using FreeTextBoxControls;
using ZJU.COPLib;
using Faust.Andrew.LiteWiki.DataAccess;

public partial class WikiEdit : System.Web.UI.Page
{
    protected string m_PageName = "";
    static string prevPage = string.Empty;
    static string default_string = "There is no Wiki page on record for '{0}', please feel free to create one.";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            prevPage = Request.UrlReferrer.ToString();
            bool UseAuthentication = false;// Convert.ToBoolean(ConfigurationManager.AppSettings["UseAuthentication"]);

            m_PageName = Request.QueryString["PageName"];

            Page.Title = "COPaKB Wiki Edit: " + m_PageName;
            lbPageName.Text = m_PageName;

            if (UseAuthentication &&
                ((WikiOperations.IsPagePrivate(m_PageName) && !User.IsInRole("Super User"))
                    || (!User.Identity.IsAuthenticated && !WikiOperations.AllowAnonymousEdit(m_PageName))))
            {
                //They don't have rights to view this page
                Response.Redirect("~/Login.aspx");
            }

            LoadPage();
        }
        else
        {
            m_PageName = (string)ViewState["PageName"];

            Page.Title = "Edit: " + m_PageName;

            bool UseAuthentication = false;// Convert.ToBoolean(ConfigurationManager.AppSettings["UseAuthentication"]);

            if (UseAuthentication && WikiOperations.IsPagePrivate(m_PageName) && !User.IsInRole("Super User"))
            {
                //They don't have rights to view this page
                Response.Redirect("~/Login.aspx");
            }
        }
    }

    protected void LoadPage()
    {
        WikiPage wikiPage = WikiOperations.GetWikiPage(m_PageName);

        // begin add
        if (wikiPage == null)
        {
            wikiPage = instantiateWikiPage();
            ftbEdit.Text = string.Format(default_string, m_PageName);
        }
        else
            ftbEdit.Text = wikiPage.PageContent;
        // end add

        m_PageName = wikiPage.PageName;
        //chkPrivate.Checked = wikiPage.IsPrivate;
        //chkAnonEdit.Checked = wikiPage.AllowAnonEdit;

        ViewState.Add("PageName", m_PageName);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        /*string UserData = ftbEdit.Text;
        if (!UserData.Contains("free to create"))
        {*/
        nameEdit.Text = nameEdit.Text.Trim();
        if (nameEdit.Text == "")
        {
            prompt.Text = @"<div class=""alert alert-error""><strong>Note:</strong> Please enter a name.</div>";
        }
        else
        {
            WikiPage wikiPage = instantiateWikiPage();

            wikiPage.Created = null;

            if (WikiOperations.UpdatePage(wikiPage) != 1)
            {
                wikiPage.Created = DateTime.Now;

                if (WikiOperations.CreateWikiPage(wikiPage) != 1)
                    throw new Exception("Error Updating: " + m_PageName + " page");
            }
            Response.Redirect(prevPage);
        }
        /*}
        else
        {
            Response.Redirect(prevPage);
        }*/

        //if (m_PageName.StartsWith("IPI") || m_PageName.StartsWith("CoPro") )
        //{
        //    Response.Redirect("ProteinInfo.aspx?QType=Protein ID&QValue=" + m_PageName);
        //}
        //else if (m_PageName.StartsWith("CoPep"))
        //{
        //    Response.Redirect("PeptideInfo.aspx?QType=Peptide ID&QValue=" + m_PageName);
        //}
        //else
        //{ 
        //    Response.Redirect ("SpectrumInfo.aspx?QValue=" + m_PageName);
        //}
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //if (m_PageName.StartsWith("IPI") || m_PageName.StartsWith("CoPro"))
        //{
        //    Response.Redirect("ProteinInfo.aspx?QType=Protein ID&QValue=" + m_PageName);
        //}
        //else if (m_PageName.StartsWith("CoPep"))
        //{
        //    Response.Redirect("PeptideInfo.aspx?QType=Peptide ID&QValue=" + m_PageName);
        //}
        //else
        //{
        //    Response.Redirect("SpectrumInfo.aspx?QValue=" + m_PageName);
        //}
        Response.Redirect(prevPage);
    }

    private WikiPage instantiateWikiPage()
    {
        WikiPage wikiPage = new WikiPage();
        wikiPage.ModifiedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";
        if (nameEdit.Text != "")
            wikiPage.ModifiedBy = nameEdit.Text;
        else
            wikiPage.ModifiedBy = "Anonymous";
        wikiPage.PageContent = ftbEdit.Text;
        wikiPage.PageName = m_PageName;
        wikiPage.IsPrivate = false;//User.Identity.IsAuthenticated ? chkPrivate.Checked : false;
        wikiPage.AllowAnonEdit = true;//User.Identity.IsAuthenticated ? chkAnonEdit.Checked : true;
        wikiPage.LastModified = DateTime.Now;
        wikiPage.Created = DateTime.Now;
        return wikiPage;
    }
}
