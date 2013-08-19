﻿<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Interactome Mouse Nucleus" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="interactome_mouse_nucleus.aspx.cs" Inherits="interactome_drosophila_mitochondria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<!-- start breadcrumbs -->
<div class="row-fluid">
    <div class="span12">
        <ul class="breadcrumb">
          <li><a href="Default.aspx">Home</a> <span class="divider">/</span></li>
          <li><a href="Modules.aspx">Organeller Modules</a> <span class="divider">/</span></li>
          <li class="active">Mouse Nucleus Interactome</li>
        </ul>
    </div>
</div> <!-- end breadcrumbs -->

<!-- start - INTERACTOME CONTENT -->

<div class="row-fluid">
    <div class="span12">
        <h2>Mouse Nucleus Interactome</h2>
        <p>
        This page presents an interactome view of the interacting proteins from this dataset.
        Interactions were retrieved from IMEx databases selecting interactions with experimental
        evidence curated by IMEx standards.
        </p>
    </div>
</div>


<!-- start - TABS -->
<div class="row-fluid">
    <ul class="nav nav-tabs" id="myTab">

      <li class="active">
          <a href="#reactome_tab">
              <i class="icon-list-alt"></i> Reactome pathways</a>
      </li>
      <li>
          <a href="#goprocesses_tab"><i class="icon-list-alt"></i> Gene Ontology processes</a>
      </li>
    </ul>
</div>

<div class="tab-content">
  <div class="tab-pane active" id="reactome_tab">
      <div class="row-fluid">
          <div class="vis_options">
          <a href="javascript:void(0)" class="gene_names_opt" onclick="copaInteractome.displayGeneNames();">Gene names</a> | <a href="javascript:void(0)" class="uniprot_accessions_opt" onclick="copaInteractome.displayUniprotAccessions();">UniProt accessions</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="copaInteractome.toggleSelectionListProteinDisplay();">Show/Hide protein list</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="copaInteractome.clearAllHighlights();">Clear all highlights</a>
      </div>
          <div id="reactomePathways"></div>
      </div>
  </div>
  <div class="tab-pane" id="goprocesses_tab">
        <div class="row-fluid">
        <div class="vis_options">
            <a href="javascript:void(0)" class="gene_names_opt" onclick="copaInteractome.displayGeneNames();">Gene names</a> | <a href="javascript:void(0)" class="uniprot_accessions_opt" onclick="copaInteractome.displayUniprotAccessions();">UniProt accessions</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="copaInteractome.toggleSelectionListProteinDisplay();">Show/Hide protein list</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="copaInteractome.clearAllHighlights();">Clear all highlights</a>
        </div>
            <div id="goProcesses"></div>
        </div>
  </div>
</div>
<!-- end - TABS -->
<br>
<h2>IMEx Interactions</h2>
<div id="proteinInformation"></div>
<div id="interactionsView"></div>

<!-- end - INTERACTOME CONTENT -->
<asp:literal ID="contentJS" runat="server" />
</asp:Content>