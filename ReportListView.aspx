<%@ Register TagPrefix="obspl" Namespace="OboutInc.Splitter2" Assembly="obout_Splitter2_Net" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportListView.aspx.cs" Inherits="ReportListView" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Report List View" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <meta charset="utf-8"/>
	<title>COPa Knowledgebase Query Report</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content="Joon-Sub Chung"/>
    <link rel="stylesheet" type="text/css" media="all" href="./assets/css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" media="all" href="./assets/css/bootstrap-responsive.min.css"/>
    <style type="text/css">

        body {
            padding-bottom: 20px;
            font-size: 12px;
        }

        .footer {
            color: #888;
        }

        .footer > hr {
            margin: 5px 0;
            border: 0;
            border-top: 1px solid #eeeeee;
            border-bottom: 1px solid #ffffff;
        }

        /* Custom container */
        .container {
            margin: 0 auto;
            max-width: 1000px;
        }

        .container > hr {
            margin: 0 0 60px 0;
            border: none;
        }

        /* Customize the navbar links to be fill the entire space of the .navbar */
        /* subhead
    -------------------------------------------------- */
        .subnav {
            margin: 25px 0 0 0;
            padding-bottom: 0;
            height: 36px;
            -webkit-border-top-left-radius: 0px;
            -webkit-border-top-right-radius: 0px;
            -moz-border-radius-topleft: 0px;
            -moz-border-radius-topright: 0px;
            border-top-left-radius: 0px;
            border-top-right-radius: 0px;
            font-weight: bold;
        }

        .subnav .nav > li > a {
            margin: 0;
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0;
        }

        .nav-pills > .active > a,
        .nav-pills > .active > a:hover,
        .nav-pills > .active > a:focus {
            color: #ffffff;
            background-color: #008cce;
        }

        .subnav .nav > .active > a,
        .subnav .nav > .active > a:hover {
            padding-left: 13px;
            border-right-color: #aaa;
            border-left: 0;
            background-color: none;
        }

        .subnav .nav > .active > a .caret,
        .subnav .nav > .active > a:hover .caret {
            border-top-color: #fff;
        }

        .nav-tabs .open .dropdown-toggle, 
        .nav-pills .open .dropdown-toggle, 
        .nav > li.dropdown.open.active > a:hover, 
        .nav > li.dropdown.open.active > a:focus {
            color: #fff;
            background-color: #16a9ea;
            border-color: #16a9ea;
        }

        .subnav .nav > li:first-child > a,
        .subnav .nav > li:first-child > a:hover {
            border-left: 0;
            padding-left: 12px;
            -webkit-border-radius: 0px 0 0 0px;
            -moz-border-radius: 0px 0 0 0px;
            border-radius: 0px 0 0 0px;
        }

        .subnav .nav > li:last-child > a {
            border-right: 0;
        }

        @media (max-width: 767px) {

            .subnav {
                position: static;
                top: auto;
                z-index: auto;
                width: auto;
                height: auto;
                background: #fff; /* whole background property since we use a background-image for gradient */
                -webkit-box-shadow: none;
                -moz-box-shadow: none;
                box-shadow: none;
            }

            .subnav .nav > li {
                float: none;
            }

            .subnav .nav > li > a {
                border: 0;
            }

            .subnav .nav > li + li > a {
                border-top: 1px solid #e5e5e5;
            }

            .subnav .nav > li:first-child > a,
            .subnav .nav > li:first-child > a:hover {
                -webkit-border-radius: 0px 0px 0 0;
                -moz-border-radius: 0px 0px 0 0;
                border-radius: 0px 0px 0 0;
            }
        }

        @media (min-width: 980px) {

            .subnav-fixed {
                position: fixed;
                top: 40px;
                left: 0;
                right: 0;
                z-index: 1020; /* 10 less than .navbar-fixed to prevent any overlap */
                border-color: #d5d5d5;
                border-width: 0 0 1px; /* drop the border on the fixed edges */
                -webkit-border-radius: 0px;
                -moz-border-radius: 0px;
                border-radius: 0px;
                -webkit-box-shadow: inset 0 1px 0 #fff, 0 1px 5px rgba(0,0,0,.1);
                -moz-box-shadow: inset 0 1px 0 #fff, 0 1px 5px rgba(0,0,0,.1);
                box-shadow: inset 0 1px 0 #fff, 0 1px 5px rgba(0,0,0,.1);
                filter: progid:DXImageTransform.Microsoft.gradient(enabled=false); /* IE6-9 */
            }

            .subnav-fixed .nav {
                width: 938px;
                margin: 0 auto;
                padding: 0 1px;
            }

            .subnav .nav > li:first-child > a,
            .subnav .nav > li:first-child > a:hover {
                -webkit-border-radius: 0;
                -moz-border-radius: 0;
                border-radius: 0px;
            }
        }


        @media (min-width: 1210px) {

            .subnav-fixed .nav {
                width: 1168px; /* 2px less to account for left/right borders being removed when in fixed mode */
            }
        }

        .bar-left {
            border-left: 1px solid #aaa;
            padding-left: 10px;
        }

        .nav {
            margin-bottom: 0px;
            margin-left: 0;
            list-style: none;
        }

        h2 {
            font-size: 20px;
        }

        .carousel .container {
            position: relative;
            z-index: 9;
        }

        .carousel-control {
            height: 80px;
            margin-top: 0;
            font-size: 120px;
            text-shadow: 0 1px 1px rgba(0,0,0,.4);
            background-color: transparent;
            border: 0;
            z-index: 10;
        }

        .carousel {
            margin-bottom: 18px;
            margin-top: 20px;
        }

        .carousel .item {
            height: 165px;
        }

        .carousel img {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 165px;
        }

        .wrapper {
            margin-left: 50px;
        }

        .carousel-caption {
            position: static;
            max-width: 100%;
            padding: 20px;
            margin-top: 25px;
            width: 550px;
        }

        .carousel-caption h1,
        .carousel-caption .lead {
            margin: 0;
            line-height: 1.25;
            color: #fff;
            text-shadow: 0 1px 1px rgba(0,0,0,.3);
        }

        .carousel-caption .btn {
            margin-top: 10px;
        }

        .carousel-indicators.middle {
            left: 0;
            right: 0;
            top: auto;
            bottom: 10px;
            text-align: center;
        }

        .carousel-indicators.middle li {
            float: none;
            display: inline-block;
            border: 1px solid #ddd;
        }

        .top-bar {
            background-color: #0085ca;
            color: #fff;
            height: 17px;
            padding: 5px;
            margin-bottom: 10px;
        }
        .top-bar-report {
            background-color: #0085ca;
            color: #fff;
            height: 15px;
            padding: 5px;
        }
        .dl-horizontal dt {
            float: left;
            width: 120px;
            overflow: hidden;
            clear: left;
            text-align: right;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .dl-horizontal dd {
            margin-left: 140px;
        }

        /** images **/
        .imgbox{
        display: block;
        width: 100%;
        text-align: center;
        margin-bottom: 40px;
        }
        .imgbox1 {
        display: block; 
        width: 100%;
        text-align: center;
        margin-bottom: 40px;
        }
        .imgbox img {
        -webkit-box-shadow: 1px 4px 9px -1px rgba(0,0,0,0.65);
        -moz-box-shadow: 1px 4px 9px -1px rgba(0,0,0,0.65);
        box-shadow: 1px 4px 9px -1px rgba(0,0,0,0.65);
        }

        .imgbox1 img {
        -webkit-box-shadow: 1px 4px 9px -1px rgba(0,0,0,0.65);
        -moz-box-shadow: 1px 4px 9px -1px rgba(0,0,0,0.65);
        box-shadow: 1px 4px 9px -1px rgba(0,0,0,0.65);
        }
        .magnify, .magnify1 {
        position: relative;
        }
        /* Let's create the magnifying glass */
        .large, .large1 {
        display: none;
        width: 250px;
        height: 250px;
        position: absolute;
        -webkit-border-radius: 100%;
        -moz-border-radius: 100%;
        border-radius: 100%;
        /* box shadows to achieve the glass effect */
        -webkit-box-shadow: 0 0 0 7px rgba(255, 255, 255, 0.85), 0 0 7px 7px rgba(0, 0, 0, 0.25), inset 0 0 40px 2px rgba(0, 0, 0, 0.25);
        -moz-box-shadow: 0 0 0 7px rgba(255, 255, 255, 0.85), 0 0 7px 7px rgba(0, 0, 0, 0.25), inset 0 0 40px 2px rgba(0, 0, 0, 0.25);
        box-shadow: 0 0 0 7px rgba(255, 255, 255, 0.85), 0 0 7px 7px rgba(0, 0, 0, 0.25), inset 0 0 40px 2px rgba(0, 0, 0, 0.25)
        }
        /* To solve overlap bug at the edges during magnification */
        .thumb, .thumb1 {
            display: block;
        }
        .tab-content {
         overflow: hidden;
        }
        .GeneExpressionSummary_subtitle {
            font-weight:bold;
        }
        .GeneExpressionSummary_feature {
            padding-left: 10px;
            font-style:italic
        }
        .thumbsize {
            height: 80px;
            width: 80px;
        }
        .bigthumbsize {
            width: 160px;
        }
        input[type="radio"], input[type="checkbox"] {
            margin: 0px 5px 3px 0;
            line-height: normal;
        }
        /* For copaclient */
        .minijava {
            height: 64px;
        }
        .oslogo {
            height: 100px;
        }
        /* match input's margin-bottom */
        .btn {
            margin-bottom: 10px;
        }
        .graph { 
	        position: relative;/* IE is dumb */
	        width: 600px; 
	        border: 1px solid #73CCFF;
	        padding: 2px; 
        }
        .graph .bar { 
	        display: block;
	        position: relative;
	        background:#B4F117;
	        text-align: left; 
	        color: #333; 
	        height: 1.5em; 
	        line-height: 1.5em;            
        }
        .lbump {
            margin-left: 20px;
        }
        .lblue {
            background-color:#D7F7FF;
        }
		</style>
<script>
    var preEl;
    var orgBColor;
    var orgTColor;
    function HighLightTR(el, backColor, textColor) {
        if (typeof (preEl) != 'undefined') {
            preEl.bgColor = orgBColor;
            try { ChangeTextColor(preEl, orgTColor); } catch (e) {; }
        }
        orgBColor = el.bgColor;
        orgTColor = el.style.color;
        el.bgColor = backColor;
        try { ChangeTextColor(el, textColor); } catch (e) {; }
        preEl = el;
    }
    function ChangeTextColor(a_obj, a_color) {;
        for (i = 0; i < a_obj.cells.length; i++)
            a_obj.cells(i).style.color = a_color;
    }
</script>
</head>
<body>

<div class="top-bar-report">
    <div class="container">
    </div>
</div>

<obspl:Splitter CollapsePanel="left" runat="server" StyleFolder="styles/default2" LiveResize="true" CookieDays="0" id="vSp1">
    <LeftPanel WidthMin="224" WidthMax="600" >
        	<Header>
                <div style="height:120px;">
                    <div class="lblue">
			            <div style="padding: 10px 10px 50px 10px; height: 120px;" >							
                            <asp:Label ID="lbSaveReport" runat="server" >Save the report file to your local:</asp:Label><br/>
                            <asp:HyperLink ID="hlViewSummary" runat="server" >Show the Statistical Distribution View</asp:HyperLink>&nbsp; <asp:HyperLink ID="SwithTreeView" runat="server" >Switch to Tree View</asp:HyperLink>
			            </div>
                    </div>
                </div>
		    </Header>
        <Content>

            <obspl:HorizontalSplitter CollapsePanel="top" runat="server" StyleFolder="styles/default2" LiveResize="true" CookieDays="0" id="hSpl">
                <TopPanel HeightDefault="500" HeightMin ="268" HeightMax ="769">
                    <Content>
                        <div style ="cursor:pointer;padding-left:5px;padding-right:5px;padding-top:1px ">
                            <asp:Table ID="tblProtein" runat="server"  BackColor="White" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%" class="datagrid">
                            </asp:Table>
                        </div>
                    </Content>
                </TopPanel>
                <BottomPanel>
                </BottomPanel>
            </obspl:HorizontalSplitter>
        </Content>
    </LeftPanel>
        <RightPanel WidthDefault="850">
            <content>
                <div class="container">
                    <div class="lbump">
                        <div class="masthead">
                            <div class="row-fluid">
                                <div class="span8">
                                    <a href="Default.aspx"><img src="./assets/img/copamed.jpg" alt="" /></a>
                                </div>
                            </div>
                            <div class="subnav">
                                <ul class="nav nav-pills uppercase">
                                    <li><a href="Default.aspx">Home</a></li>
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Resources <b class="caret"></b></a>
                                        <ul class="dropdown-menu">
                                            <li><a href="Modules.aspx">Organellar Modules</a></li>
                                            <li><a href="DataDeposition.aspx">Data Deposition</a></li>
                                            <li><a href="Participator.aspx">Participating Institutions</a></li>
                                            <li><a href="ReleaseHistory.aspx">Release History</a></li>
                                            <li><a href="ContactUs.aspx">Contact Us</a></li>
                                        </ul>
                                    </li>
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Analysis <b class="caret"></b></a>
                                        <ul class="dropdown-menu">
                                            <li><a href="MSRUNSearch.aspx">Query COPaKB (mzML File)</a></li>
                                            <li><a href="SpectralSearch.aspx">Query COPaKB (DTA file)</a></li>
                                            <li><a href="KeywordSearch.aspx">Keyword Search</a></li>
                                            <li><a href="COPaWikiDefault.aspx">COPaKB Wiki Pages</a></li>
                                            <li><a href="COPaTools.aspx">Tools for Proteome Biology</a></li>
                                            <li><a href="COPaKBClient.aspx">COPaKB Client</a></li>
                                            <li><a href="#">Clinical Analysis <span class="label label-important">Coming Soon</span></a></li>
                                        </ul>
                                    </li>
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Links <b class="caret"></b></a>
                                        <ul class="dropdown-menu">
                                            <li><a href="http://www.signalingmodules.org/">The UCLA Team</a></li>
                                            <li><a href="http://fields.scripps.edu/">The TSRI Team</a></li>
                                            <li><a href="http://www.ebi.ac.uk">The EMBL-EBI Team</a></li>
                                            <li><a href="http://www.biotech.kth.se/proteomics/">The KTH Team</a></li>
                                            <li><a href="http://www.cbeis.zju.edu.cn/bme1/index.htm">The ZJU Team</a></li>
                                            <li><a href="http://www.nhlbi-proteomics.org/">NHLBI Proteomics Program</a></li>
                                            <li><a href="http://www.nhlbi-ucla.org/">NHLBI Proteomics @ UCLA</a></li>
                                        </ul>
                                    </li>
                                    <li><a href="COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk">Help Desk</a></li>
                                    <li><a href="COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk">Tutorials</a></li>
                                    <li><a href="COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk">COPaKB Wiki</a></li>
                                </ul>
                            </div><!-- /.subnavbar -->
                        </div><!-- /.masthead -->
            
                        <div class="row-fluid">
                            <h2>COPa Knowledgebase Query Report</h2>
                            <h2>Basic Information</h2>
                        </div>
                        <div class="row-fluid">
                            <div class="span2">
                                mzML File:
                            </div>
                            <div class="span10">
                                <asp:Label ID="lbmzML" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2">
                                Searching Module: 
                            </div>
                            <div class="span10">
                                <asp:Label ID="lbSearchingModule" runat="server" >[hlIPIProteinID]</asp:Label>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2">
                                Searching Condition: 
                            </div>
                            <div class="span10">
                                <asp:Label ID="lbSC" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span2">
                                Total Protein ID:  
                            </div>
                            <div class="span10">
                                <asp:Label ID="lbProteinCount" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <h2>Protein Information</h2>
                        </div>
                        <div class="row-fluid">
                            <div class="span12">
                                <asp:Table ID="tbProteins" class="table-hover" runat="server" BackColor="White">
                                    <asp:TableRow ID="TableRow1" runat="server" BackColor="#99CCFF">
                                        <asp:TableCell ID="TableCell1" runat="server" BorderColor="White" BorderStyle="Solid" 
                                        BorderWidth="1px" HorizontalAlign="Left">Protein ID</asp:TableCell>
                                        <asp:TableCell ID="TableCell2" runat="server" BorderColor="White" BorderStyle="Solid" 
                                        BorderWidth="1px"  HorizontalAlign="Left" >Normalized Spectra Abundance Factor (NSAF)</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                    </div> <!--./lbump-->
                </div><!--./container-->
                <hr>
                <div class="container">
                    <div class="footer">
                        <div class="row-fluid">
                            <div class="span6">
                                &copy; COPaKB 2013 | Cardiovascular Research Laboratory<br />
                                Suite 1-609 MRL Building 675 Charles E. Young Dr. South Los Angeles, CA 90095-1760
                            </div>
                            <div class="text-right offset2 span4">
                                <a href="#">Help Desk/Tutorials</a> | <a href="#">COPaKB Wiki</a> | <a href="#">Private Policy</a>
                            </div>
                        </div>
                        <hr class="foot">
                        <p>
                            COPa Knowledgebase is supported by a Proteomics Center Award from NHLBI/NIH<br />
                            268201000035C Proteome Biology of Cardiovascular Disease
                        </p>
                    </div>
                </div> <!-- /container -->
            </content>
        </RightPanel>
    </obspl:Splitter>
    <script>
        document.getElementById('vSp1_LeftP_Header').style.height = '120px';
         </script>
</body>
</html>
