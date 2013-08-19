<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Protein Information" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="ProteinInfo.aspx.cs" Inherits="ProteinInfo2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="span12">
        <h2>Protein Info</h2>
    </div>
</div>
<div class="row-fluid">
    <div class="alert alert-info">
    <asp:Literal ID="informationBtn" runat="server"></asp:Literal>
    <h4>Properties of your search protein are outlined below:</h4>
    <asp:Literal ID="informationShow" runat="server"></asp:Literal>
        <br />
        <ol class="">
            <li>Common identifiers and sequence features about your search protein are provided in "Basic Information" and “Sequence Features” below.</li>
            <li>In the section of “Overview of Peptides Detected”, a map of peptide sequence coverage from mass spectrometry experiments is provided. You can also click on individual sequence bars to obtain associated mass spectra.</li>
            <li>At the bottom of this page, five tabs are provided to organize the following information: 
                <ul class="">
                    <li>“Relevance to Cardiac Diseases” tab presents disease phenotypes relevant to your search protein, including gene transcription data, genetic phenotypes and reference literature.</li>
                    <li>“Distinct Peptides” tab lists peptide sequences that were detected for your search protein. You can click on these sequences to obtain associated mass spectra.</li>
                    <li>“Expression Profiles” tab displays immunofluorescence and immunohistochemistry images obtained from Human Protein Atlas (HPA) for most human proteins. The immunofluorescence image shows your search protein in individual cells with green color; immunohistochemistry image shows your search protein in myocardial tissue with brown stain. The image rollover and zoom functions provide higher resolution images. To review additional images, please click on the Ensemble ID next to the HPA logo.</li>
                    <li>“Additional References” tab helps you explore additional databases on your search protein.</li>
                    <li>“Wiki” tab has been prepared to provide a forum to discuss the properties of your search protein.</li>
                </ul>
            </li>
        </ol>
    </div>
    </div>
</div>
<div class="row-fluid">
    <div class="span5">
        <h4>Basic Information</h4>
        <dl class="dl-horizontal">
            <dt>Protein COPaKB ID</dt><dd><asp:Label ID="lbProtein_ID" runat="server"></asp:Label></dd>
            <dt>Description</dt><dd><asp:Label ID="lbProtein_Name" runat="server"></asp:Label></dd>
            <dt>Gene Symbol</dt><dd><asp:Label ID="lbGeneSymbol" runat="server"></asp:Label></dd>
            <dt>UniProt ID</dt><dd><asp:Label ID="lbUniprot" runat="server"></asp:Label></dd>
            <dt>Species</dt><dd><asp:Label ID="lbOrganism_Source" runat="server"></asp:Label></dd>
            <dt>Observed Peptides</dt><dd><asp:Label ID="lbDistinct_Peptides" runat="server"></asp:Label></dd>
        </dl>
    </div>
    <div class="span7">
        <div class="row-fluid">
            <h4>Sequence Features:</h4>
            <dl class="dl-horizontal">
                <dt>Sequence Length</dt> <dd><asp:Label ID="lbSequence_Length" runat="server"></asp:Label></dd>
                <dt>Molecular Weight</dt> <dd><asp:Label ID="lbMolecuar_Weight" runat="server"></asp:Label></dd>
            </dl>
            <div class="sequence-type"><asp:Label ID="lbSequence" runat="server"></asp:Label></div>
        </div>
    </div>
</div>
<div class="row-fluid">
    <h4>Overview of Peptides Detected</h4>
    <code>
        <asp:ImageMap ID="imgPeptides" runat="server"></asp:ImageMap>
    </code>
</div>
<div class="row-fluid">
    
            <style type="text/css">
                #myTab.nav-tabs>li>a, #myTab.nav-pills>li>a {
                    padding-right: 3px;
                    padding-left: 3px;
                    margin-right: 1px;
                    line-height: 14px;
                  }
            </style>
    <ul class="nav nav-tabs" id="myTab">
        
      <li class="active"><a href="#relevance"><i class="icon-ok-sign"></i> Relevance to Cardiac Diseases</a></li>
      <li><a href="#peptide"><i class="icon-list-alt"></i> Distinct Peptides</a></li>
      <li><a href="#info"><i class="icon-info-sign"></i> Expression Profiles</a></li>
      <li><a href="#interact"><i class="icon-screenshot"></i> Interactome </a></li>
      <li><a href="#ref"><i class="icon-plus-sign"></i> Additional References</a></li>
      <li><a href="#wiki"><i class="icon-pencil"></i> Wiki</a></li>
    </ul>
</div>
 
<div class="tab-content">
  <div class="tab-pane" id="info">
      <div class="row-fluid">
        <h4>Expression Profiles</h4>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/_image/logo_text_small.gif"/> 
            <asp:Image ID="Image2" runat="server" ImageUrl="~/_image/logo_hex_anim_small.gif" />
            
            <p><asp:Label ID="lbHumanProtAtlas" runat="server"></asp:Label></p>
                      
            <div class="row-fluid">
                <div class="offset1 span4">
                    <div class="magnify">
                        <div class="large"></div>
                        <div class="imgbox">
                            <asp:Literal ID="linkHPAImgStart" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal>
                            <asp:Image ID="HPAImg"  runat="server" />
                            <asp:Literal ID="linkHPAImgClose" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal>

                        </div>
                    </div>
                    <!-- @end .magnify -->
                </div>
                <div class="span7">
                    <table class="table table-striped table-bordered">
                      <tbody>
                        <tr>
                          <td>Main Location</td>
                          <td><asp:Label ID="lb_main_subcelluar" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                          <td>Additional Location</td>
                          <td><asp:Label ID="lb_additional_subcellular" runat="server"></asp:Label></td>
                        </tr>
                      </tbody>
                    </table>
                </div>
            </div>
            <div class="row-fluid">
                <div class="offset1 span4">
                    <div class="magnify1">
                        <div class="large1"></div>
                        <asp:Literal ID="linkIHCImgStart" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal>
                            <div class="imgbox1"><asp:Image ID="IHCImg"  runat="server" /></div>
                        <asp:Literal ID="linkIHCImgClose" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal>
                    </div>
                </div>
                <div class="span7">
                    <table class="table table-striped table-bordered">
                      <tbody>
                        <tr>
                          <td>IHC Summary</td>
                          <td><asp:Label ID="lb_IHC_summary" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                          <td>IHC Heart Expression</td>
                          <td><asp:Label ID="lb_IHC_heart_expression" runat="server"></asp:Label></td>
                        </tr>
                      </tbody>
                    </table>
                </div>
            </div>

      </div>
  </div>
  <div class="tab-pane" id="peptide">
        <div class="row-fluid">
            <h4> Distinct Peptides: </h4>
            <asp:Label ID="lbPeptideNumber" runat="server"></asp:Label>

            <asp:Table class="table table-condensed table-hover table-striped datagrid" ID="tbPeptides" runat="server" BackColor="White" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%">
                <asp:TableRow ID="TableRow1" class="DataGridHeader" runat="server">
                    <asp:TableCell ID="TableCell1" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Peptide COPaKB ID</asp:TableCell>
                    <asp:TableCell ID="TableCell2" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center">Pre AA</asp:TableCell>
                    <asp:TableCell ID="TableCell4" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center">Next AA</asp:TableCell>
                    <asp:TableCell ID="TableCell5" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center">MW</asp:TableCell>
                    <asp:TableCell ID="TableCell6" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center">Location</asp:TableCell>
                    <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Enzyme</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
  </div>
    
  <div class="tab-pane" id="interact">
        <div class="row-fluid">
            <h4> Interactome </h4>
            <asp:Literal ID="interactomeDisplay" runat="server"></asp:Literal>
        </div>
  </div>
  <div class="tab-pane" id="ref">
        <div class="row-fluid">
            <h4>Additional References:</h4>
            <asp:Label ID="AdditionalReport" runat="server"></asp:Label>
            <asp:Label ID="lbRefKB" runat="server"></asp:Label>
        </div>
  </div>
  <div class="tab-pane active" id="relevance">
    <div class="row-fluid">
        <h4> Relevance to Cardiac Diseases</h4>

        <!-- Start Body -->
		<div id="uniProtDiseaseSummary" style="width:826px;">
		</div>
        <asp:Table ID="tbDisease" runat="server" GridLines="Horizontal">
            <asp:TableRow ID="TableRow2" class="DataGridHeader" runat="server">
                <asp:TableCell ID="TableCell3" runat="server" Width="100px">Perturbation</asp:TableCell>
                <asp:TableCell ID="TableCell8" runat="server" Width="100px">Description</asp:TableCell>
                <asp:TableCell ID="TableCell9" runat="server" Width="250px">Disease Type</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
		<br/>&nbsp;<div id="geneExpressionSummary" style="width:826px;">
		</div>
		<!-- End Body -->
    </div>
  </div>
  <div class="tab-pane" id="wiki">
    <div class="row-fluid">
        <h4>Wiki <asp:Label ID="lbMoreInformation" runat="server"></asp:Label></h4>
        <small><asp:Literal ID="lastMod" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal></small>
        <asp:Literal ID="litContent" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal>
    </div>
  </div>
</div>
<div class="row-fluid">
</div>
</asp:Content>

