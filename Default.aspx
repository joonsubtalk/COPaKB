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
                <h2>COPaKB Database</h2>
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
                The COPa Knowledgebase (COPaKB) Project is developed under NHLBI Proteomics Centers Program. COPaKB offers a unique informatics platform to facilitate understanding of novel biological insights from proteomics datasets: (1) COPaKB is a curated relational database of protein molecular and biomedical phenotype properties, interfaced to a website for public data retrieval. (2) COPaKB allows any investigators to process raw proteomics data, and returns a consistently annotated report of protein properties. (3) COPaKB offers a wide range of informatics tools for investigators to analyze different studies in parallel and to conduct meta-analyses. Collectively, COPaKB affords investigators with broad access to datasets of proteomics technologies, thus elevating their utilities in the studies of cardiovascular biology and medicine.
            </p>
            </div>
         </div>
        <div class="row-fluid">
            <div class="span6">
                <div class="copa-img">
                    <img src="assets/img/map.jpg" />
                </div>
            </div>
            <div class="span6">
                <div class="row-fluid">
                    <p style="font-size: 20px; font-weight: bold;">Statistics and Updates of COPaKB</p>
                       (as of August 16th, 2013)</p>
                </div>
                <div class="row-fluid">
                    120,414 Pageviews <br />
                    10.27 Pages/visit <br />
                    4,591 Unique visitors <br />
                    95 Countries <br />
                </div>
                <br />
                <div class="row-fluid">
                    -Six new modules are added to COPaKB, which now presents ten modules in total.<br />
                    -COPaKB now supports iHOP analysis. <br />
                    -COPaKB is synchronized with Human Protein Atlas version 11. <br />
                    -COPaKB Client V2.0 is available to download. It is coded in JAVA.<br />
                    -iCOPa V2.1 is available to download at iTunes store.<br />
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

