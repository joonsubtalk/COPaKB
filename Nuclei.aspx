<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="Proteasome.aspx.cs" Inherits="Proteasome" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Heart Nulcear Proteome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="span12">
        <h2>Current Status of the Nuclei Module in COPaKB</h2>
        <p>
            Most mammalian cardiomyocytes are binucleated as a result of fusion between myoblasts. Adult cardiomyocytes do not replicate, but their gene transcription is highly regulated. The latter process involves dynamic interaction among a number of nuclear proteins, including transcription factors, histones, and nuclear pore proteins. 
        </p>                                                                             
        <p>
            Extraction of highly purified nuclear proteome is a challenge, particularly in heart samples, which are enriched in mitochondria and contractile proteins. In COPaKB, we provide nuclei module for one model system, i.e., the murine heart.
        </p> 
        <p>
            Please see the table below for a summary of the training datasets.
        </p>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <table class="table table-bordered table-hover table-striped">
            <caption><h2>Current Status of the Nuclei Modules in COPaKB</h2></caption>
            <thead>
                <tr>
                    <th>Nuclei Modules in COPaKB</th>
                    <th>Biological Replicates</th>
                    <th>Technical Replicates</th>
                    <th>LC/MS/MS Experiments</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><a href="#nmurine" role="button" data-toggle="modal">Murine Cardiac Nuclei Module</a></td>
                    <td>15</td>
                    <td>30</td>
                    <td>137</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>

<!-- Modal -->
<div id="nmurine" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="nmurineModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="nmurineModalLabel">Murine Nuclei Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br /> The mass spectral data of 137 LC-MS/MS experiments came from the study described in the following publication: "Specialized compartments of cardiac nuclei exhibit distinct proteomic anatomy" (Franklin S, <em>et. al</em>., <em>Mol Cell Proteomics</em>. 2011;10:M110.000703).&nbsp; Four different protein extraction strategies were applied in parallel to gain a comprehensive coverage of the cardiac nuclear proteome. LTQ Orbitrap was used for protein sequencing.
    </p>
    <p>
        LTQ Orbitrap was operated in the data-dependent mode with 1 full MS scan followed by 6 MS/MS scans. The resolution of MS1 scan was set at 30,000.
    </p>
      <p>
        <strong>Tissue Collection:</strong><br /> Adult male balb/c mice aged 8–12 weeks (Charles River Laboratories) were used for this study. The protocol for nuclei enrichment is provided in the following manuscript, "Specialized compartments of cardiac nuclei exhibit distinct proteomic anatomy" (Franklin S, <em>et. al</em>., <em>Mol Cell Proteomics</em>. 2011;10:M110.000703).     
    </p>
  </div>
  <div class="modal-footer">
    <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>
</asp:Content>

