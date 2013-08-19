using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class COPaKB : System.Web.UI.MasterPage
{
    string interactomeID = "Q9VEN1";

    protected void Page_Load(object sender, EventArgs e)
    {
        Button1.Visible = false;
        string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
        printActiveMenu(pageName);
        setJS(pageName);

        String notice = "Scheduled maintenance coming 7/24/13";
        this.serverNotice.Text = String.Format(@"<div class='alert alert-danger'>{0}</div>", notice);
        this.serverNotice.Visible = false;
        
        //interactomeID = strValue;
        //interactomeID = ContentPlaceHolder1.Page.Request.QueryString["iid"];
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("#");
    }

    protected void setJS(string currentPageName)
    {
        if (currentPageName.Length > 11)
        {
            string subpart = currentPageName.Substring(0, 11);
            if (subpart == "interactome")
            {
                this.addCSS.Text = @"	  <link href='css/icheck/minimal/minimal.css' rel='stylesheet' type='text/css'>
          <link href='css/Biojs.InteractionsCircleView.css' rel='stylesheet' type='text/css'>
          <link href='css/Biojs.SelectionList.css' rel='stylesheet' type='text/css'>
          <link href='css/copa_interactome.css' rel='stylesheet' type='text/css'>";
                this.scriptProteinInfo.Text =
                @"<script>$('#myTab a[href=""#reactome_tab""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
                jQuery('#myTab a[href=""#reactome_tab""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
                $('#myTab a[href=""#goprocesses_tab""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
               jQuery('#myTab a[href=""goprocesses_tab""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name

     var copaInteractome = {};
     window.onload = function() {
         copaInteractome.initCopaInteractome();
     };</script>
            <!-- BioJS dependencies -->
      <script language='JavaScript' type='text/javascript' src='js/biojs/dependencies/jquery.icheck.min.v0.9.1.js'></script>
      <script language='JavaScript' type='text/javascript' src='js/biojs/dependencies/d3.v2.5.js'></script>
      <script language='JavaScript' type='text/javascript' src='js/biojs/dependencies/d3.layout.js'></script>
      <script language='JavaScript' type='text/javascript' src='js/biojs/dependencies/packages.js'></script>
      <!-- Biojs -->
	  <script type='text/javascript' src='js/biojs/Biojs.js'></script>
      <script type='text/javascript' src='js/biojs/Biojs.InteractionsCircleView.js'></script>
	  <script type='text/javascript' src='js/biojs/Biojs.SelectionList.js'></script>
      <script type='text/javascript' src='js/copaint.js'></script>
<!-- end - INTERACTOME JS 
      <script type='text/javascript' src='js/copa_interactome.js'></script>-->";

            if (currentPageName == "interactome_human_mitochondria"){
                this.dataJS.Text = @"<!-- DATA -->
      <script type='text/javascript' src='js/copa_imex_interactome_human_mitochondria.js'></script>";
            }
            if (currentPageName == "interactome_mouse_mitochondria")
            {
                this.dataJS.Text = @"<!-- DATA -->
      <script type='text/javascript' src='js/copa_imex_interactome_mouse_mitochondria.js'></script>";
            }
            if (currentPageName == "interactome_drosophila_mitochondria")
            {
                this.dataJS.Text = @"<!-- DATA -->
                <script type='text/javascript'>
                function loadJS(src, callback) {
                    var s = document.createElement('script');
                    s.src = src;
                    s.async = true;
                    s.onreadystatechange = s.onload = function() {
                        var state = s.readyState;
                        if (!callback.done && (!state || /loaded|complete/.test(state))) {
                            callback.done = true;
                            callback();
                        }
                    };
                    document.getElementsByTagName('head')[0].appendChild(s);
                }
                loadJS('js/copa_imex_interactome_drosophila_mitochondria.js', function() { 
                    // put your code here to run after script is loaded
                    startInteract();
                });
                </script>";
            }
            if (currentPageName == "interactome_celeang_mitochondria")
            {
                this.dataJS.Text = @"<!-- DATA -->"; // NEED IMPLEMENT
            }
            if (currentPageName == "interactome_human_proteasome")
            {
                this.dataJS.Text = @"<!-- DATA -->
      <script type='text/javascript' src='js/copa_imex_interactome_human_proteosome.js'></script>";
            }
            if (currentPageName == "interactome_mouse_proteasome")
            {
                this.dataJS.Text = @"<!-- DATA -->
      <script type='text/javascript' src='js/copa_imex_interactome_mouse_proteosome.js'></script>";
            }
            if (currentPageName == "interactome_mouse_nucleus")
            {
                this.dataJS.Text = @"<!-- DATA -->
      <script type='text/javascript' src='js/copa_imex_interactome_mouse_nucleus.js'></script>";
            }
            if (currentPageName == "interactome_mouse_cytosol")
            {
                this.dataJS.Text = @"<!-- DATA -->
                <script type='text/javascript'>
                function loadJS(src, callback) {
                    var s = document.createElement('script');
                    s.src = src;
                    s.async = true;
                    s.onreadystatechange = s.onload = function() {
                        var state = s.readyState;
                        if (!callback.done && (!state || /loaded|complete/.test(state))) {
                            callback.done = true;
                            callback();
                        }
                    };
                    document.getElementsByTagName('head')[0].appendChild(s);
                }
                loadJS('js/biojs/dependencies/jquery.icheck.min.v0.9.1.js', function() { 
                    loadJS('js/biojs/dependencies/d3.v2.5.js', function() { 
                    loadJS('js/biojs/dependencies/d3.layout.js', function() { 
                    loadJS('js/biojs/dependencies/packages.js', function() { 
                    loadJS('js/biojs/Biojs.js', function() { 
                    loadJS('js/biojs/Biojs.InteractionsCircleView.js', function() { 
                    loadJS('js/biojs/Biojs.SelectionList.js', function() { 
                    loadJS('js/copa_imex_interactome_mouse_cytosol.js', function() { 
                    // put your code here to run after script is loaded
                    
                });
                });
                });
                });
                });
                });
                });
                });</script>";
            }

           this.lastJS.Text = string.Format(@"");

            }
        }
        if (currentPageName == "PeptideInfo")
        {
            this.scriptProteinInfo.Text =
            @"<script>$('#myTab a[href=""#observed""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#candidate""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#wiki""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
</script>";
        }
        if (currentPageName == "MatchSpectrum")
        {
            this.scriptProteinInfo.Text =
            @"<script>$('#myTab a[href=""#correlation""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#info""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#datasource""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#wiki""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
</script>";
        }
        if (currentPageName == "SpectrumInfo")
        {
            /*
            this.initJS.Text = @"<!--Load the AJAX API-->
    <script type=""text/javascript"" src=""https://www.google.com/jsapi""></script>
    <script type=""text/javascript"">
   
      // Load the Visualization API and the piechart package.
      google.load('visualization', '1.0', {'packages':['corechart']});
     
      // Set a callback to run when the Google Visualization API is loaded.
      google.setOnLoadCallback(drawChart);

      // Callback that creates and populates a data table, 
      // instantiates the pie chart, passes in the data and
      // draws it.
      function drawChart() {

      // Create the data table.
      var data = new google.visualization.DataTable();
      data.addColumn('number', 'm/z');
      data.addColumn('number', 'relative abundance');
      data.addRows([
        ['22', 3],
        ['32', 1],
        ['23', 1], 
        ['53', 1],
        ['23', 2]
      ]);

      // Set chart options
      var options = {'title':'Spectrum'};

      // Instantiate and draw our chart, passing in some options.
      var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
      chart.draw(data, options);
    }
    </script>
";
*/
            this.scriptProteinInfo.Text =
            @"<script>$('#myTab a[href=""#features""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#info""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#datasource""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#wiki""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
</script>";
        }
        if (currentPageName == "ProteinInfo")
        {
            this.scriptProteinInfo.Text =
            @"<script>$('#myTab a[href=""#info""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#motif""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#peptide""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#relevance""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#ref""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
    $('#myTab a[href=""#interact""]').click(function (e) {e.preventDefault();$(this).tab('show');}) // Select tab by name
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
            || currentPageName == "ReleaseHistory" || currentPageName == "COPaTools" || currentPageName == "ContactUs")
            this.isActiveResources.Text = @"<li class=""active dropdown"">";
        else if (currentPageName.Length > 11)
        {
            string subpart = currentPageName.Substring(0, 11);
            if (subpart == "interactome")
                this.isActiveResources.Text = @"<li class=""active dropdown"">";
            else
                this.isActiveResources.Text = @"<li class=""dropdown"">";
        }
        else
            this.isActiveResources.Text = @"<li class=""dropdown"">";


        if (currentPageName == "MSRUNSearch" || currentPageName == "SpectralSearch" || currentPageName == "KeywordSearch"
            || currentPageName == "COPaWikiDefault" ||  currentPageName == "COPaKBClient"
            || currentPageName == "ClinicalAnalysis")
            this.isActiveAnalysis.Text = @"<li class=""active dropdown"">";
        else
            this.isActiveAnalysis.Text = @"<li class=""dropdown"">";
        /*
        if (currentPageName == "COPaWikiDefault")
            this.isActiveHelp.Text = @"<li class=""active""><a href=""COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk"">Help Desk</a></li>";
        else
            this.isActiveHelp.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk"">Help Desk</a></li>";
        */
        if (currentPageName == "COPaWikiDefault")
            this.isActiveTutorial.Text = @"<li class=""active""><a href=""COPaWikiDefault.aspx?PageName=FUNCTIONS%20AND%20UTILTIES%20OF%20COPaKB"">Tutorials</a></li>";
        else
            this.isActiveTutorial.Text = @"<li><a href=""COPaWikiDefault.aspx?PageName=FUNCTIONS%20AND%20UTILTIES%20OF%20COPaKB"">Tutorials</a></li>";

        if (currentPageName == "COPaWikiDefault")
            this.isActiveWiki.Text = @"<li class=""active""><a href=""COPaWikiDefault.aspx"">COPaKB Wiki</a></li>";
        else
            this.isActiveWiki.Text = @"<li><a href=""COPaWikiDefault.aspx"">COPaKB Wiki</a></li>";
    }
}
