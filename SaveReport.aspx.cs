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

public partial class SaveReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strReportFile = Request.QueryString["Report"];
        string strReportType = Request.QueryString["ReportType"];
        if ((strReportType == null) || (strReportType == "XLS"))
        {
            string strSaveName = strReportFile.Substring(strReportFile.LastIndexOf("\\") + 1);
            strSaveName = strSaveName.Substring(0, strSaveName.IndexOf(".")) + ".xls";
            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", strReportFile));
            Response.TransmitFile(strReportFile);
            Response.End();
        }
        else
        {
            string strSaveName = strReportFile.Substring(strReportFile.LastIndexOf("\\") + 1);
            strSaveName = strSaveName.Substring(0, strSaveName.IndexOf(".") ) + ".cpr";
            Response.ContentType = "text/xml";
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", strSaveName));
            Response.TransmitFile(strReportFile);
            Response.End();
        }
           
    }
}
