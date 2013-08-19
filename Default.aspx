<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- Home of Heart Proteome" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>
 
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  Runat="Server">
        <div class="row-fluid">
            <div class="feature-part">
            <h3>Get started with COPaKB</h3>
                <div class="row-fluid">
                    <div class="span6">
                        <p class="lead">
                            <i class="icon-search icon-white"></i> <a href="KeywordSearch.aspx?type=1">Protein Identifier Query</a><br />
                            <i class="icon-search icon-white"></i> <a href="KeywordSearch.aspx?type=2">Amino Acid Sequence Query</a><br />
                            <i class="icon-search icon-white"></i> <a href="MSRUNSearch.aspx">MS Data File (mzML)</a><br />
                        </p>
                    </div>
                    <div class="span6">
                        <p class="lead">
                            <i class="icon-search icon-white"></i> <a href="SpectralSearch.aspx">MS Data File (DTA)</a><br />
                            <i class="icon-search icon-white"></i> <a href="COPaKBClient.aspx">MS Data File (COPaKB Client)</a><br />
                            <i class="icon-search icon-white"></i> <a href="epiSearch.aspx">Analysis of Multiple Datasets</a><br />
                        </p>
                    </div>
                </div>
                    <!--<li><a href="#">Clinical Analysis <span class="label label-important">Coming Soon</span></a></li>-->
            </div>
        </div>
        <div class="row-fluid">
            <div class="span6 offset6">
                <h2>About COPaKB</h2>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span6">
                <div id="myCarousel" class="carousel slide">
                    <ol class="carousel-indicators middle">
                        <li data-target="#myCarousel" data-slide-to="0" class=""></li>
                        <!--<li data-target="#myCarousel" data-slide-to="1"></li>-->
                        <li data-target="#myCarousel" data-slide-to="2" class=""></li>
                        <li data-target="#myCarousel" data-slide-to="3" class="active"></li>
                        <li data-target="#myCarousel" data-slide-to="4" class=""></li>
                    </ol>
                    <!-- Carousel items -->
                    <div class="carousel-inner">
                    <div class="item active"><img src="./assets/img/heart.jpg" alt=""></div>
                    <div class="item"><img src="./assets/img/chart.jpg" alt=""></div>
                    <div class="item"><img src="./assets/img/cell.jpg" alt=""></div>
                    <div class="item"><img src="./assets/img/spectra.jpg" alt=""></div>
                    </div>
                    <!-- Carousel nav -->
                    <a class="carousel-control left" href="#myCarousel" data-slide="prev">‹</a>
                    <a class="carousel-control right" href="#myCarousel" data-slide="next">›</a>
                </div>
            </div>
            <div class="span6">
            <!--<img src="./assets/img/smallmito.png" class="img-rounded">-->
            <p>
                The COPa Knowledgebase (COPaKB) Project is developed under NHLBI Proteomics Centers Program. COPaKB has been created as a unique resource to facilitate understanding of novel biological insights from proteomic datasets: (1) COPaKB is a curated relational database of protein molecular and biomedical phenotype properties, interfaced to a website for public data retrieval. (2) COPaKB allows any investigators to process raw proteomic datasets without the need of accessing high-end instrumentation, and returns a consistently annotated report of protein properties. (3) COPaKB offers a wide range of informatics tools for investigators to analyze different studies in parallel and to conduct meta-analyses. 
            </p>
            </div>
         </div>
        <div class="row-fluid">
            <div class="span6">
                <div class="copa-img">
                    <p style="font-size: 14px; font-weight: bold; text-align:center">COPaKB Worldwide Usage Report<br /> <span style="font-size: 12px; font-weight: normal">(as of August 16<sup>th</sup>, 2013)</span></p>
                    <img src="assets/img/map.jpg" />
                    <p style="font-size: 12px; text-align:left">The color scale represents the number of visits to COPaKB from individual countries. Darker color (e.g., that of US) corresponds to more visits (e.g., 8,927 visits). </p>
                </div>
            </div>
            <div class="span6">
                <div class="row-fluid">
                    <p style="font-size: 20px; font-weight: bold;">Statistics and Updates of COPaKB</p>
                </div>
                <div class="row-fluid">
                    As of August 2013, Google Analytics reports the following data on COPaKB (Please see the Figure on the left): <br /> 
                    <ul>
                        <li><span style="text-decoration: underline"><em>120,414</em></span> pageviews;</li>
                        <li><span style="text-decoration: underline"><em>10.27</em></span> pageviews per visit;</li>
                        <li><span style="text-decoration: underline"><em>4,591</em></span> unique visitors;</li>
                        <li><span style="text-decoration: underline"><em>95</em></span> countries are represented.<br /></li>
                    </ul>
                    
                </div>
                <br />
                <div class="row-fluid">
                    In August 2013, COPaKB implemented the following updates: <br /> 
                    <ul>
                        <li>COPaKB now offers ten modules in total;</li>
                        <li>iHOP function is now part of the COPaKB reports;</li>
                        <li>COPaKB is updated with Human Protein Atlas version 11;</li>
                        <li>COPaKB Client V2.0 is now available in Java;</li>
                        <li>iCOPa V2.1 is available to download at iTunes store.</li>
                    </ul>
                </div>
            </div>
            <!--<div class="span4">
            <!--<img src="./assets/img/heart.jpg" class="img-rounded">-->
            <!--<h2>The Goal</h2>
            <p>
                A comprehensive delineation of cardiac proteome dynamics is a herculean task. We believe this pursuit will blossom from collaborative efforts and the input from many individual investigators. <br>
                COPaKB promotes this endeavor at multiple levels: 
            </p>
            <ul class="unstyled">
                <li>(i) <a href="DataDeposition.aspx">Results of previous studies are integrated to guide future analyses</a></li>
                <li>(ii) <a href="epiSearch.aspx">Results from multiple ongoing analyses can be combined in real time to test new hypothesis</a></li>
                <li>(iii) <a href="COPaWikiDefault.aspx">COPaKB Wiki component </a> invites and supports investigators with diverse expertise to participate in this knowledge-building process.</li>
            </ul>
            </div>-->
        </div>
    </asp:Content>

