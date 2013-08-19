<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- Disease Relevance" Language="C#" MasterPageFile="~/SimpleCOP.master" AutoEventWireup="true" CodeFile="DiseaseInfo.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<!--JQuery LIb and CSS -->
			<script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
			
			<!-- CSS -->
		    <link rel="stylesheet" href="css/Biojs.UniProtDiseaseSummary.css" />
			<link rel="stylesheet" href="css/GeneExpressionSummary.css" />
			
			
			<!-- BIOJS LIB and CSS -->
			<script language="JavaScript" type="text/javascript" src="js/Biojs.js"></script>
			<script language="JavaScript" type="text/javascript" src="js/Biojs.UniProtDiseaseSummary.js"></script>
			<script language="JavaScript" type="text/javascript" src="js/Biojs.GeneExpressionSummary.js"></script>
			    
			<script language="JavaScript" type="text/javascript">
			    window.onload = function () {
			        var currentURL = window.location.search;
                    if (currentURL.indexOf("uniprot") != -1) {
                      var uniprotID = currentURL[currentURL.indexOf("uniprot") + 8] + currentURL[currentURL.indexOf("uniprot") + 9] + currentURL[currentURL.indexOf("uniprot") + 10] + currentURL[currentURL.indexOf("uniprot") + 11] + currentURL[currentURL.indexOf("uniprot") + 12] + currentURL[currentURL.indexOf("uniprot") + 13];
                    }
                    else {
                          unitprotID = '';
                    }
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
			</script>

            <!-- Start Body -->
		<div id="uniProtDiseaseSummary" style="width:826px;">
		</div>
		<br/>&nbsp;<div id="geneExpressionSummary" style="width:826px;">
		</div>
        Information is provided by OMIM and Gene Expression Atlas.
		<!-- End Body -->
</asp:Content>

