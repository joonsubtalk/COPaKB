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
using System.Text;
using ZJU.COPDB;
using System.Web.Configuration;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Net;

public partial class testme : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       lbMessage.Text = "";
    }
}
