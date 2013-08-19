using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NoResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        /*
         * string V = Request.QueryString["V"];
        string GSymbol = V.Substring(1, V.Length - 2);
        console.Text = GSymbol;
         * 
        string T = Request.QueryString["T"];
        string V = Request.QueryString["V"];
        if (!Page.IsPostBack)
        {
            DropDownList ddlQueryType = (DropDownList)(this.Master.FindControl("ddlQueryType"));
            ddlQueryType.Text = T;
            TextBox tbKeyWords = (TextBox)(this.Master.FindControl("tbKeyWords"));
            tbKeyWords.Text = V;
        }
         */
    }
}
