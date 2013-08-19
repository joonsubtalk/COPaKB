using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SimpleCOPaKB : System.Web.UI.MasterPage
{
    string pn;
    bool sorry = true;
    protected void Page_Load(object sender, EventArgs e)
    {

        pn = Request.QueryString["PageName"];
        string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
        printActiveMenu(pageName);
        setJS(pageName);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("#");
    }

    protected void setJS(string currentPageName)
    {

        if (currentPageName == "ProteinInfo")
        {
            this.scriptProteinInfo.Text =
            @"<script>$('#myTab a[href=""#info""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#motif""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#peptide""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#relevance""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#wiki""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
</script>

<!-- BIOJS LIB and CSS -->
			<!-- BIOJS LIB and CSS -->
			<script language=""JavaScript"" type=""text/javascript"" src=""js/Biojs.js""></script>
			<script language=""JavaScript"" type=""text/javascript"" src=""js/Biojs.UniProtDiseaseSummaryIOS.js""></script>
			<script language=""JavaScript"" type=""text/javascript"" src=""js/Biojs.GeneExpressionSummaryIOS.js""></script>
			    
			  <script language=""JavaScript"" type=""text/javascript"">
			    window.onload = function () {
			        var currentURL = window.location.search;
			        if (currentURL.indexOf(""QValue="") != -1) {
			            var uniprotID = currentURL[currentURL.indexOf(""QValue="") + 7] + currentURL[currentURL.indexOf(""QValue="") + 8] + currentURL[currentURL.indexOf(""QValue="") + 9] + currentURL[currentURL.indexOf(""QValue="") + 10] + currentURL[currentURL.indexOf(""QValue="") + 11] + currentURL[currentURL.indexOf(""QValue="") + 12];
			        }
			        else {
			            uniprotID = '';
			        }
			        uniprotID=uniprotID.toUpperCase();
			        var uniprotAcc = uniprotID;
                    /* DISPLAY DISEASE TABEL */
			        var UDS = new Biojs.UniProtDiseaseSummary({
			            target: 'uniProtDiseaseSummary',
			            uniProtDasUrl: 'http://www.ebi.ac.uk/das-srv/uniprot/das/uniprot/features?segment=' + uniprotAcc + ';type=BS:01019',
			            keywordFiltereing: ['heart', 'cardi', 'aortic', 'aorta', 'vascular'],
			            width: '100%',
			            referencesColumnWidth: '150px',
			            imageWidth: '150px',
			            proxyUrl: 'http://www.heartproteome.org/dasty/proxy-asp/proxy.aspx',
			            tableHeader: true,
			            componentTitle: true
			        });
			        /* DISPLAY GENE EXPRESSION INFORMATION */
			        var GES = new Biojs.GeneExpressionSummary({
			            target: 'geneExpressionSummary',
			            proxyUrl: 'http://www.heartproteome.org/dasty/proxy-asp/proxy.aspx',
			            identifier: uniprotAcc,
			            legend: 'false'
			        });

			    };
			</script>";
        }
        if (currentPageName == "Default")
        {
            this.scriptProteinInfo.Text =
            @"<script>
    !function ($) {$(function () {$('#myCarousel').carousel()})}(window.jQuery)
</script>";
        }
        if (currentPageName == "Modules")
        {
            this.scriptProteinInfo.Text = @"<script type=""text/javascript"">
                    function DoNav(url) {
                        document.location.href = url;
                    }
            </script>";

        }
    }

    protected void printActiveMenu(string currentPageName)
    {
        if (currentPageName == "Default")
            this.isActiveHome.Text = @"<li class=""active""><a href=""Default.aspx"">Home</a></li>";
        else
            this.isActiveHome.Text = @"<li><a href=""Default.aspx"">Home</a></li>";

        if (currentPageName == "Modules" || currentPageName == "DataDeposition" || currentPageName == "Participator"
            || currentPageName == "ReleaseHistory" || currentPageName == "ContactUs" || currentPageName == "COPaTools")
            this.isActiveResources.Text = @"<li class=""active dropdown"">";
        else
            this.isActiveResources.Text = @"<li class=""dropdown"">";

        if (currentPageName == "MSRUNSearch" || currentPageName == "SpectralSearch" || currentPageName == "KeywordSearch"
            || currentPageName == "COPaKBClient" 
            || currentPageName == "ClinicalAnalysis")
            this.isActiveAnalysis.Text = @"<li class=""active dropdown"">";
        else
            this.isActiveAnalysis.Text = @"<li class=""dropdown"">";

        if (currentPageName == "COPaWikiDefault")
        {
            if (pn == "COPaKB Help Desk")
            {
                this.isActiveWiki.Text = @"<li><a href=""COPaWikiDefault.aspx"">COPaKB Wiki</a></li>";
                this.isActiveTutorial.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=FUNCTIONS%20AND%20UTILTIES%20OF%20COPaKB"">Tutorials</a></li>";
                this.isActiveHelp.Text = @"<li class=""active""><a href=""COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk"">Help Desk</a></li>";
            }
            else if (pn == "FUNCTIONS AND UTILTIES OF COPaKB")
            {
                this.isActiveTutorial.Text = @"<li class=""active""><a href=""COPaWikiDefault.aspx?PageName=FUNCTIONS%20AND%20UTILTIES%20OF%20COPaKB"">Tutorials</a></li>";
                this.isActiveWiki.Text = @"<li><a href=""COPaWikiDefault.aspx"">COPaKB Wiki</a></li>";
                this.isActiveHelp.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk"">Help Desk</a></li>";
            }
            else
            {
                this.isActiveWiki.Text = @"<li class=""active""><a href=""COPaWikiDefault.aspx"">COPaKB Wiki</a></li>";
                this.isActiveHelp.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk"">Help Desk</a></li>";
                this.isActiveTutorial.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=FUNCTIONS%20AND%20UTILTIES%20OF%20COPaKB"">Tutorials</a></li>";
            }
        }
        else
        {
            this.isActiveWiki.Text = @"<li><a href=""COPaWikiDefault.aspx"">COPaKB Wiki</a></li>";
            this.isActiveHelp.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk"">Help Desk</a></li>";
            this.isActiveTutorial.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=FUNCTIONS%20AND%20UTILTIES%20OF%20COPaKB"">Tutorials</a></li>";
        }
    }
}