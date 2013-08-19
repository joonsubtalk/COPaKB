<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="Proteasome.aspx.cs" Inherits="Proteasome" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Murine Cytosol Proteome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="span12">
        <h2>Current Status of the Cytosol Module in COPaKB</h2>
            <p>Cytoplasm hosts individual organelles as well as proteins and protein complexes. Many of these proteins are involved in cellular activities such as protein synthesis, degradation and trafficking. In the myocardium, contractile proteins are expressed in particularly high levels. </p>
            <p>Extraction of cytosolic proteins commonly involves centrifugation methods. Organelles are pelleted out while cytosolic proteins are collected in the supernatant.&nbsp;In COPaKB, we have compiled cytosol module for one model system, i.e., the murine heart.</p>
            <p>Please see the table below for a summary of the training datasets.</p>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <table class="table table-bordered table-hover table-striped">
            <caption><h2>Current Status of the Cytosol Module in COPaKB</h2></caption>
            <thead>
                <tr>
                    <th>Cytosol Modules in COPaKB</th>
                    <th>Biological Replicates</th>
                    <th>Technical Replicates</th>
                    <th>LC/MS/MS Experiments</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><a href="#cmurine" role="button" data-toggle="modal">Murine Cardiac Cytosol Module</a></td>
                    <td>9</td>
                    <td>9</td>
                    <td>125</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>

<!-- Modal -->
<div id="cmurine" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="cmurineModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="cmurineModalLabel">Murine Cytosolic Module of COPaKB</h3>
  </div>
  <div class="modal-body">
        <p>
            <strong>Data Source:</strong><br/>The mass spectral data of 41 LC-MS/MS experiments were collected at NHLBI Proteomics Center in UCLA. Among these, 21 experiments were analyzed with LTQ-Orbitrap with a mass resolution of 7,500; 20 experiments were analyzed with LTQ-Orbitrap with a mass resolution of 60,000. The remaining 84 LC-MS/MS experiments came from the study described in the publication, "Global survey of organ and organelle protein expression in mouse: combined proteomic and transcriptomic profiling" (Kislinger T, <em>et. al</em>., <em>Cell</em>. 2006;125:173-86). LCQ DECA XP was used in this study.
        </p> 
        <p>
            LCQ DECA XP protocol: Samples were loaded onto biphasic silica capillary columns using a pressure vessel. Online 2D-LC was implemented before conducting analysis with the mass spectrometer. The ion-exchange chromatography involved 12-step salt elution, each of which was followed by a 100-minute RPLC separation. The mass spectrometer was operated in a data-dependent mode with 1 full MS scan followed by 3 MS/MS scans. Seven replicates were analyzed via this protocol.
        </p>
        <p>
            LTQ-Orbitrap protocol: LTQ Orbitrap was operated in the data-dependent mode with 1 full MS followed by 5 MS/MS scans. The full MS scan was set at a resolution of either 60,000 or 7,500 for two distinct datasets. Two replicates were analyzed via this protocol.
        </p>
        <p>
            <strong>Tissue Collection #1:</strong><br/>Wild type adult male ICR mice (Harlan) were used in this study. The protocol for cytosol enrichment is provided in the following manuscript, "Ischemic preconditioning induces selective translocation of protein kinase C isoforms epsilon and eta in the heart of conscious rabbits without subcellular redistribution of total protein kinase C activity." (Zhang J, <em>et. al</em>., <em>Circ. Res.</em>. 1997;81:404-14).
        </p>
        <p>
            <strong>Tissue Collection #2:</strong><br/> Wild type adult female ICR mice were used in this study. The protocol for cytosol enrichment is provided in the following manuscript, "Global survey of organ and organelle protein expression in mouse: combined proteomic and transcriptomic profiling" (Kislinger T, <em>et. al</em>., <em>Cell</em>. 2006;125:173-86).
        </p>
  </div>
  <div class="modal-footer">
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>
</asp:Content>


