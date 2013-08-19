<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Mitochondrial Proteome" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="Mitochondria.aspx.cs" Inherits="Mitochondria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="span12">
        <h2>Current Status of Mitochondria Modules in COPaKB</h2>
        <p>
            Mitochondria are crucial for energy metabolism of the heart in both health and disease states. They play an important role in many cellular functions such as bioenergetics, calcium signaling, reactive oxygen species (ROS) generation and apoptosis. Mitochondrial dysfunctions have been found as the underlying cause of many forms of cardiomyopathy. Therefore, studying the cardiac mitochondrial proteome is a promising way to gain new insights into the protein functions, interactions, modifications and localizations at systems level.
        </p>
        <p>
            COPaKB serves as an open resource to facilitate this translation of experimental observations on mitochondrial proteome into better understanding of the cardiovascular biology. Currently, COPaKB has compiled mitochondrial proteome from four model systems, including the human heart, murine heart, drosophila and C. elegans.
        </p>
        <p>
            Please see the table below for a summary of the training datasets.
        </p>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <table class="table table-bordered table-hover table-striped">
            <caption><h2>Current Status of Mitochondria Modules in COPaKB</h2></caption>
            <thead>
                <tr>
                    <th>Mitochondria Modules in COPaKB</th>
                    <th>Biological Replicates</th>
                    <th>Technical Replicates</th>
                    <th>LC/MS/MS Experiments</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><a href="#human" role="button" data-toggle="modal">Human Cardiac Mitochondria Module</a></td>
                    <td>6</td>
                    <td>29</td>
                    <td>868</td>
                </tr>
                <tr>
                    <td><a href="#murine" role="button" data-toggle="modal">Murine Cardiac Mitochondria Module</a></td>
                    <td>24</td>
                    <td>34</td>
                    <td>1,089</td>
                </tr>
                <tr>
                    <td><a href="#drosophila" role="button" data-toggle="modal">Drosophila Mitochondria Module</a></td>
                    <td>6</td>
                    <td>18</td>
                    <td>710</td>
                </tr>
                <tr>
                    <td><a href="#elegans" role="button" data-toggle="modal">C. elegans Mitochondria Module</a></td>
                    <td>7</td>
                    <td>9</td>
                    <td>165</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>


<!-- Modal -->
<div id="human" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="humanModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="humanModalLabel">Human Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br />
        The mass spectral data of all 868 LC-MS/MS experiments were collected at NHLBI Proteomics Center in UCLA. LTQ-XL and LTQ-Orbitrap instruments were used in these analyses. All protein expression images were obtained from Royal Institute of Technology (KTH). 
    </p>
    <p>
        LTQ-XL protocol: The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8kV. Eighteen replicates were analyzed via this protocol.
    </p>
    <p>
        LTQ-Orbitrap protocol: The data-dependent scan was set in the same condition as described above. The resolution of MS1 scan was set at 60,000. Monoisotopic peak recognition function was enabled. Eleven replicates were analyzed via this protocol.
    </p>
    <p>
        <strong>Tissue Collection:</strong>
        Human heart samples from the free anterior left ventricular wall were collected from individuals (average age = 49±8 yrs, n=6, 5 males and 1 female) who were previously treated with a left ventricular assist device (LVAD). These individuals exhibit normal left ventricular end diastolic dimension (LVEDD), which was enabled after LVAD treatment. The protocol for mitochondria purification is provided in the following manuscript, "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, et. al., Proteomics.2008;8:1564-75).
    </p>
  </div>
  <div class="modal-footer">
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<!-- Modal -->
<div id="murine" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="murineModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="murineModalLabel">Murine Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br />
    The mass spectral data for 980 LC-MS/MS experiments were collected at NHLBI Proteomics Center in UCLA; LTQ-XL and LTQ-Orbitrap instruments were used in these analyses. These data files come from the studies described in the following two publications: "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, <em>et. al</em>., <em>Proteomics. </em>2008;8:1564-75), and "Altered proteome biology of cardiac mitochondria under stress conditions" (Zhang J, <em>et. al.</em>, <em>J Proteome Res</em>. 2008 ;7:2204-14).
    </p>
    <p>
        The mass spectral data for 109 additional LC-MS/MS experiments were downloaded from Peptide Atlas (PAe000353). This dataset comes from the study described in the publication, "Global survey of organ and organelle protein expression in mouse: combined proteomic and transcriptomic profiling" (Kislinger T, <em>et. al.</em>, <em>Cell</em>. 2006;125:173-86). LCQ DECA XP was used in this study.
    </p>
    <p>
        LCQ DECA XP protocol: Samples were loaded onto biphasic silica capillary columns using a pressure vessel. Online 2D-LC was implemented before conducting analysis with the mass spectrometer. The ion-exchange 100-minute RPLC separation. The mass spectrometer was operated in a data-dependent mode with 1 full MS scan followed by 3 MS/MS scans. Nine replicates were analyzed via this protocol.
    </p>
    <p>
        LTQ-XL protocol: The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8 kV. Ten replicates were analyzed via this protocol.  
    </p>
    <p>
        LTQ-Orbitrap protocol: The data-dependent scan was set in the same condition as described in the LTQ-XL protocol above. The resolution of MS1 scan was set at 60,000. Monoisotopic peak recognition function was enabled. Fifteen replicates were analyzed via this protocol. </p>
    <p>
        <strong>Tissue Collection #1:</strong><br>Wild type adult male ICR mice (Harlan) were used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, <em>et. al</em>., <em>Proteomics</em>. 2008;8:1564-75).
    </p>
    <p>
        <strong>Tissue Collection #2:</strong><br>Wild type adult female ICR mice were used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Global survey of organ and organelle protein expression in mouse: combined proteomic and transcriptomic profiling" (Kislinger T, <em>et. al.</em>, <em>Cell</em>. 2006;125:173-86). LCQ DECA XP was used in this study.  
    </p>

  </div>
  <div class="modal-footer">
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<!-- Modal -->
<div id="drosophila" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="drosophilaModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="drosophilaModalLabel">Drosophila Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br> The mass spectral data of all 710 LC-MS/MS experiments were collected at NHLBI Proteomics Center in UCLA. LTQ-XL and LTQ-Orbitrap instruments were used in these analyses.<br />
        LTQ-XL protocol: The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8 kV. Nine replicates were analyzed via this protocol.
    </p>
    <p>
        LTQ-Orbitrap protocol: The data-dependent scan was set in the same condition as described above. The MS1 scan was set at resolution of 60,000. The monoisotopic peak recognition function was enabled. Nine replicates were analyzed via this protocol. 
    </p>
    <p>
        <strong>Tissue Collection:</strong><br>Wild type Oregon-R-C strain (Bloomington Drosophila Stock Center stock #5) was used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, <em>et. al</em>., <em>Proteomics</em>. 2008;8:1564-75).
    </p>
  </div>
  <div class="modal-footer">
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<!-- Modal -->
<div id="elegans" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="elegansModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="elegansModalLabel">C. elegans Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br>The mass spectral data of this module were obtained from 165 LC-MS/MS experiments. The publication associated with 104 of these experiments is "Comparative functional analysis of the <i>Caenorhabditis elegans</i> and <i>Drosophila melanogaster</i> proteomes" (Schrimpf SP, <em>et. al</em>., <em>PLoS Biology</em>. 2009;7:616-27). LTQ mass spectrometer was used in this study. These data were downloaded from Peptide Atlas (PAe000434). 
    </p>
    <p>
        The remaining 61 of the 165 experiments were associated with the publication "Proteomic analysis of mitochondria from <em>Caenorhabditis elegans</em>" (Li J, <em>et. al</em>., <em>Proteomics. </em>2009;9:4539–4553). LTQ was used in this study as well.
    </p> 
    <p>
        LTQ protocol in the first publication: Tryptic peptides were fractionated via 1D-RPLC before mass spectrometer analyses. The instrument was operated in a data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. Four replicates were analyzed via this protocol.
    </p>
    <p>
        LTQ protocol in the second publication: Tryptic peptides were loaded onto biphasic silica capillary columns using a pressure vessel. 2D-LC was conducted online before ESI-LC-MS/MS. Ion-exchange chromatography involved 12-step salt elution. The mass spectrometer was operated in a data-dependent mode with 1 full MS scan followed by 6 MS/MS scans. Five replicates were analyzed via this protocol.
    </p>
    <p>
        <strong>Tissue Collection #1:<br></strong><em>C. elegans</em> wild-type strain N2 (Bristol) was used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Comparative functional analysis of the <i>Caenorhabditis elegans</i> and <i>Drosophila melanogaster</i> proteomes" (Schrimpf SP, <em>et. al</em>., <em>PLoS Biology</em>. 2009;7:616-27).<br><br /><strong>Tissue Collection #2:<br></strong>Wild-type <em>C. elegans</em> strain N2 obtained from the Caenorhabditis Genetics Center was used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Proteomic analysis of mitochondria from <em>Caenorhabditis elegans</em>" (Li J, <em>et. al</em>., <em>Proteomics. </em>2009;9:4539–4553).
    </p>
    </div>
  <div class="modal-footer">
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>
</asp:Content>



