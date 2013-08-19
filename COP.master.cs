using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZJU.COPDB;

public partial class COP : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ddlQueryType.Visible = false;
        tbKeyWords.Visible = false;
        ImageButton1.Visible = false;
        Button1.Visible = false;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (ddlQueryType.SelectedValue.StartsWith("Protein"))
        {

            string url = "ProteinInfo.aspx?";
            url += "QType=" + ddlQueryType.Text + "&";
            url += "QValue=" + tbKeyWords.Text;
            Response.Redirect(url);
        }
        else if (ddlQueryType.SelectedValue.StartsWith("Gene"))
        {
            string url = "ProteinList.aspx?";
            url += "T=" + ddlQueryType.Text + "&";
            url += "V=" + tbKeyWords.Text;
            Response.Redirect(url);
        }
        else if (ddlQueryType.SelectedValue.StartsWith("Peptide"))
        {
            string url = "PeptideInfo.aspx?";
            url += "QType=" + ddlQueryType.Text + "&";
            url += "QValue=" + tbKeyWords.Text;
            Response.Redirect(url);
        }
        else if (ddlQueryType.SelectedValue.StartsWith("Spectrum"))
        {
            string url = "SpectrumList.aspx?";
            url += "QType=" + ddlQueryType.Text + "&";
            url += "QValue=" + tbKeyWords.Text;
            Response.Redirect(url);
        }
    }

    protected void btSpectralSearch_Click(object sender, EventArgs e)
    {
       
            string url = "MSRUNSearch.aspx";
            Response.Redirect(url);
        
    }

    public string QueryType
    {
        get { return ddlQueryType.Text; }
        set
        { ddlQueryType.Text = value; }
    }

    public string QueryValue
    {
        get { return tbKeyWords.Text; }
        set
        { tbKeyWords.Text = value; }
    }
    protected void tbKeyWords_TextChanged(object sender, EventArgs e)
    {

    }
}
