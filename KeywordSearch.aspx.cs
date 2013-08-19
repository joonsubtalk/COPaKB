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

public partial class KeywordSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            int ktype = int.Parse(Request.QueryString["type"]);

            if (ktype == 1)
                this.ddlQueryType3.Items.FindByValue("Protein ID").Selected = true;
            else if (ktype == 2)
                this.ddlQueryType3.Items.FindByValue("Peptide Sequence").Selected = true;
        }
        
        // not implemented yet
        // this.ddlQueryType3.Items.FindByValue("Spectrum Precursor").Selected = true;
    }

    protected void bt1_click(object sender, EventArgs e)
    {
        

        if (this.ddlQueryType3.SelectedValue.StartsWith("Protein"))
        {

            string url = "ProteinInfo.aspx?";
            url += "QType=" + this.ddlQueryType3.Text + "&";
            url += "QValue=" + this.tbKeyWords3.Text;
            Response.Redirect(url);
        }
        if (this.ddlQueryType3.SelectedValue.StartsWith("Peptide"))
        {

            string url = "PeptideInfo.aspx?";
            url += "QType=" + this.ddlQueryType3.Text + "&";
            url += "QValue=" + this.tbKeyWords3.Text;
            Response.Redirect(url);
        }
        if (this.ddlQueryType3.SelectedValue.StartsWith("Spectrum"))
        {

            string url = "SpectrumList.aspx?";
            url += "QType=" + this.ddlQueryType3.Text + "&";
            url += "QValue=" + this.tbKeyWords3.Text;
            Response.Redirect(url);
        }
    }

    protected void tbKeyWords_TextChanged3(object sender, EventArgs e)
    {

    }
}