<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="Modules.aspx.cs" Inherits="Modules" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Organellar Modules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<!-- start breadcrumbs -->
<div class="row-fluid">
    <div class="span12">
        <ul class="breadcrumb">
          <li><a href="Default.aspx">Home</a> <span class="divider">/</span></li>
          <li class="active">Organeller Modules</li>
        </ul>
    </div>
</div> <!-- end breadcrumbs -->

<div class="row-fluid">
    <div class="span12">
        <h2>The Modular Structure of COPaKB</h2>
        <p>
            COPaKB is configured in a modular structure according to the organellar origin. Currently, there are ten modules in COPaKB, including human heart mitochondria, human heart proteasome, human heart total lysate, murine heart mitochondria, murine heart proteasome, murine heart cytosol, murine heart nuclei, murine heart total lysate, drosophila mitochondria, and C. elegans mitochondria. 
        </p>
        <p>
            The analytical performance in using the mass spectral library component of COPaKB depends on its proteome coverage. The construction and update of the mass spectral libraries are an ongoing process. Accordingly, modules with ample training datasets are provided; additional modules can be readily incorporated when new datasets become available. 
        </p>
        <p>
            In the section below, you can see an overview of each organelle module. To see more details about each module, please click on its icon. To find a summary of training datasets, please click on the species icons. You can also download the list of proteins in each module by directly clicking on the corresponding table below.
        </p>
    </div>
</div>
    <hr>
<div class="row-fluid">
    <div class="span3">
        <div class="imgpadding">
            <a href ="Mitochondria.aspx"><img alt="Mitochondria" src="_image/mitochondria.jpg" width="160" height="120" /><br /> <div class="text-center">Mitochondrion</div></a>
        </div>
    </div>
    <div class="span9">
        <table class="table table-condensed table-bordered table-hover table-striped">
            <caption><h2>Mitochondrion</h2></caption>
            <thead>
                <tr>
                    <th>Species</th>
                    <th>Download</th>
                    <th>Interactome</th>
                    <th>Proteins</th>
                    <th>Peptides</th>
                    <th>Spectra</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><div class="pleft"><a href="#human" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/human.jpg" /> Human</a></div></td>
                    <td><a href="files/Human_Heart_Mitochondrial_Proteins.xls">Protein List</a></td>
                    <td><a href="interactome_human_mitochondria.aspx">View</a></td>
                    <td>1,398</td>
                    <td>28,031</td>
                    <td>41,758</td>
                </tr>
                <tr>
                    <td><div class="pleft"><a href="#murine" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/rat.jpg" /> Mouse</a></div></td>
                    <td><a href="files/Murine_Heart_Mitochondrial_Proteins.xls">Protein List</a></td>
                    <td><a href="interactome_mouse_mitochondria.aspx">View</a></td>
                    <td>1,619</td>
                    <td>38,421</td>
                    <td>59,020</td>
                </tr>
                <tr>
                    <td><div class="pleft"><a href="#drosophila" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/fly.jpg" /> Drosophila</a></div></td>
                    <td><a href="files/Drosophila_Mitochondrial_Proteins.xls">Protein List</a></td>
                    <td><a href="interactome_drosophila_mitochondria.aspx">View</a></td>
                    <td>1,015</td>
                    <td>13,770</td>
                    <td>27,185</td>
                </tr>
                <tr>
                    <td><div class="pleft"><a href="#elegans" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/bacteria.jpg"  /> C. elegans</a></div></td>
                    <td><a href="http://onlinelibrary.wiley.com/store/10.1002/pmic.200900101/asset/supinfo/pmic_200900101_sm_SupplTbl1.xls?v=1&s=6649db36fe6563384ed1d2a6d142da6e9f201632">Protein List</a></td>
                    <td><a href="#">View</a></td>
                    <td>1,117</td>
                    <td>18,291</td>
                    <td>27,493</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
    <hr>
<div class="row-fluid">
    <div class="span3">
        <div class="imgpadding">
          <a href ="Proteasome.aspx"><img alt="Proteasome" src="_image/Proteasome.jpg" width="160" height="120" /><br /> <div class="text-center">Proteasome</div></a>
        </div>
    </div>
    <div class="span9">
        <table class="table table-condensed table-bordered table-hover table-striped">
            <caption><h2>Proteasome</h2></caption>
            <thead>
                <tr>
                    <th>Species</th>
                    <th>Download</th>
                    <th>Interactome</th>
                    <th>Proteins</th>
                    <th>Peptides</th>
                    <th>Spectra</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><div class="pleft"><a href="#phuman" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/human.jpg" /> Human</a></div></td>
                    <td><a href="files/Human Proteasome Proteins.xls">Protein List</a></td>
                    <td><a href="interactome_human_proteasome.aspx">View</a></td>
                    <td>283</td>
                    <td>3,482</td>
                    <td>5,668</td>
                </tr>
                <tr>
                    <td><div class="pleft"><a href="#pmurine" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/rat.jpg" /> Mouse</a></div></td>
                    <td><a href="files/Mouse Proteasome Proteins.xls">Protein List</a></td>
                    <td><a href="interactome_mouse_proteasome.aspx">View</a></td>
                    <td>151</td>
                    <td>6,409</td>
                    <td>9,442</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
    <hr>
<div class="row-fluid">
    <div class="span3">
        <div class="imgpadding">
            <a href ="Nuclei.aspx"><img alt="Proteasome" src="_image/nucleus.JPG"/><br /> <div class="text-center">Nucleus</div></a>
        </div>
    </div>
    <div class="span9">
        <table class="table table-condensed table-bordered table-hover table-striped">
            <caption><h2>Nucleus</h2></caption>
            <thead>
                <tr>
                    <th>Species</th>
                    <th>Download</th>
                    <th>Interactome</th>
                    <th>Proteins</th>
                    <th>Peptides</th>
                    <th>Spectra</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><div class="pleft"><a href="#nhuman" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/human.jpg" /> Human</a></div></td>
                    <td>...</td>
                    <td>...</td>
                    <td>...</td>
                    <td>...</td>
                    <td>...</td>
                </tr>
                <tr>
                    <td><div class="pleft"><a href="#nmurine" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/rat.jpg" /> Mouse</a></div></td>
                    <td><a href="http://www.mcponline.org/content/suppl/2010/08/31/M110.000703.DC1/mcp.M110.000703-2.xls">Protein List</a></td>
                    <td><a href="interactome_mouse_nucleus.aspx">View</a></td>
                    <td>1,048</td>
                    <td>6,918</td>
                    <td>9,115</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
    <hr>
<div class="row-fluid">
    <div class="span3">
        <div class="imgpadding">
            <a href ="Cytosol.aspx"><img alt="Cytosol" src="_image/Cytosol.jpg" width="160" height="120" /><br /> <div class="text-center">Cytosol</div></a>
        </div>
    </div>
    <div class="span9">
        <table class="table table-condensed table-bordered table-hover table-striped">
            <caption><h2>Cytosol</h2></caption>
            <thead>
                <tr>
                    <th>Species</th>
                    <th>Download</th>
                    <th>Interactome</th>
                    <th>Proteins</th>
                    <th>Peptides</th>
                    <th>Spectra</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><div class="pleft"><a href="#chuman" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/human.jpg" /> Human</a></div></td>
                    <td>...</td>
                    <td>...</td>
                    <td>...</td>
                    <td>...</td>
                    <td>...</td>
                </tr>
                <tr>
                    <td><div class="pleft"><a href="#cmurine" role="button" data-toggle="modal"><img alt="" class="xsmall" src="./assets/img/rat.jpg" /> Mouse</a></div></td>
                    <td><a href="files/Mouse_Heart_Cytosol_Proteins.xls">Protein List</a></td>
                    <td><a href="interactome_mouse_cytosol.aspx">View</a></td>
                    <td>2,587</td>
                    <td>13,983</td>
                    <td>19,141</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
    

<!-- Mitochondrial Modal -->
<div id="human" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="humanModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="humanModalLabel">Human Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br />
        The mass spectral data of all 868 LC-MS/MS experiments were conducted at NHLBI Proteomics Center in UCLA. LTQ-XL and LTQ-Orbitrap instruments were used in these analyses. All protein expression images were obtained from Royal Institute of Technology (KTH). 
    </p>
    <p>
        <strong>LTQ-XL protocol:</strong> The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8kV. Eighteen replicates were analyzed via this protocol.
    </p>
    <p>
        <strong>LTQ-Orbitrap protocol:</strong> The data-dependent scan was set in the same condition as described above. The resolution of MS1 scan was set at 60,000. Monoisotopic peak recognition function was enabled. Eleven replicates were analyzed via this protocol.
    </p>
    <p>
        <strong>Tissue Collection:</strong>
        Human heart samples from the free anterior left ventricular wall were collected from individuals (average age = 49±8 yrs, n=6, 5 males and 1 female) who were previously treated with a left ventricular assist device (LVAD). These individuals exhibit normal left ventricular end diastolic dimension (LVEDD), which was enabled after LVAD treatment. The protocol for mitochondria purification is provided in the following manuscript, "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, et. al., Proteomics.2008;8:1564-75).
    </p>
  </div>
  <div class="modal-footer">
      <!--<div class="pull-left lead"><a href="files/Human_Heart_Mitochondrial_Proteins.xls">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<div id="murine" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="murineModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="murineModalLabel">Murine Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br />
    The mass spectral data for 980 LC-MS/MS experiments were conducted at NHLBI Proteomics Center in UCLA; LTQ-XL and LTQ-Orbitrap instruments were used in these analyses. These data files come from the studies described in the following two publications: "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, <em>et. al</em>., <em>Proteomics. </em>2008;8:1564-75), and "Altered proteome biology of cardiac mitochondria under stress conditions" (Zhang J, <em>et. al.</em>, <em>J Proteome Res</em>. 2008 ;7:2204-14).
    </p>
    <p>
        The mass spectral data for 109 additional LC-MS/MS experiments were downloaded from Peptide Atlas (PAe000353). This dataset comes from the study described in the publication, "Global survey of organ and organelle protein expression in mouse: combined proteomic and transcriptomic profiling" (Kislinger T, <em>et. al.</em>, <em>Cell</em>. 2006;125:173-86). LCQ DECA XP was used in this study.
    </p>
    <p>
        <strong>LCQ DECA XP protocol:</strong> Samples were loaded onto biphasic silica capillary columns using a pressure vessel. Online 2D-LC was implemented before conducting analysis with the mass spectrometer. The ion-exchange 100-minute RPLC separation. The mass spectrometer was operated in a data-dependent mode with 1 full MS scan followed by 3 MS/MS scans. Nine replicates were analyzed via this protocol.
    </p>
    <p>
        <strong>LTQ-XL protocol:</strong> The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8 kV. Ten replicates were analyzed via this protocol.  
    </p>
    <p>
        <strong>LTQ-Orbitrap protocol:</strong> The data-dependent scan was set in the same condition as described in the LTQ-XL protocol above. The resolution of MS1 scan was set at 60,000. Monoisotopic peak recognition function was enabled. Fifteen replicates were analyzed via this protocol. </p>
    <p>
        <strong>Tissue Collection #1:</strong><br>Wild type adult male ICR mice (Harlan) were used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, <em>et. al</em>., <em>Proteomics</em>. 2008;8:1564-75).
    </p>
    <p>
        <strong>Tissue Collection #2:</strong><br>Wild type adult female ICR mice were used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Global survey of organ and organelle protein expression in mouse: combined proteomic and transcriptomic profiling" (Kislinger T, <em>et. al.</em>, <em>Cell</em>. 2006;125:173-86). LCQ DECA XP was used in this study.  
    </p>

  </div>
  <div class="modal-footer">
      <!--<div class="pull-left lead"><a href="files/Murine_Heart_Mitochondrial_Proteins.xls">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<div id="drosophila" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="drosophilaModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="drosophilaModalLabel">Drosophila Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <strong>Data Source:</strong><br> The mass spectral data of all 710 LC-MS/MS experiments were conducted at NHLBI Proteomics Center in UCLA. LTQ-XL and LTQ-Orbitrap instruments were used in these analyses.
    <p>
        <strong>LTQ-XL protocol:</strong> The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8 kV. Nine replicates were analyzed via this protocol.
    </p>
    <p>
        <strong>LTQ-Orbitrap protocol:</strong> The data-dependent scan was set in the same condition as described above. The MS1 scan was set at resolution of 60,000. The monoisotopic peak recognition function was enabled. Nine replicates were analyzed via this protocol. 
    </p>
    <p>
        <strong>Tissue Collection:</strong><br>Wild type Oregon-R-C strain (Bloomington Drosophila Stock Center stock #5) was used in this study. The protocol for mitochondria purification is provided in the following manuscript, "Systematic characterization of the murine mitochondrial proteome using functionally validated cardiac mitochondria" (Zhang J, <em>et. al</em>., <em>Proteomics</em>. 2008;8:1564-75).
    </p>
  </div>
  <div class="modal-footer">
      <!--<div class="pull-left lead"><a href="files/Drosophila_Mitochondrial_Proteins.xls">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<div id="elegans" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="elegansModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="elegansModalLabel">C. elegans Mitochondria Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        This mass spectral component of this module were built upon 165 LC-MS/MS experiments. Among them, 104 were downloaded from Peptide Atlas (PAe000434). These data associated with the publication "Comparative functional analysis of the Caenorhabditis elegans and Drosophila melanogaster proteomes" (Schrimpf SP, et. al., PLoS Biology. 2009;7:616-27). LTQ mass spectrometer was used in this study.
    </p>
    <p>
        The remaining 61 experiements associated with the publication "Proteomic analysis of mitochondria from Caenorhabditis elegans" (Li J, et. al., Proteomics 2009, 9, 4539–4553). LTQ was used in this study as well.
    </p>
    <p>
        LTQ protocol in the first publication: Tryptic peptides were fractionated via 1D-RPLC before mass spectrometer analyses. The instrument was operated in a data-dependent mode with 1 full MS scan followed by 5 MS/MS scans.
    </p>
    <p>
        LTQ protocol in the second publication: Tryptic peptides were loaded onto biphasic silica capillary columns using a pressure vessel. 2D-LC was conducted online before ESI-LC-MS/MS. Ion-exchange chromatography involved 12-step salt elution. The mass spectrometer was operated in a data-dependent mode with 1 full MS scan followed by 6 MS/MS scans.
    </p>
  </div>
  <div class="modal-footer">
      <!--<div class="pull-left lead"><a href="http://onlinelibrary.wiley.com/store/10.1002/pmic.200900101/asset/supinfo/pmic_200900101_sm_SupplTbl1.xls?v=1&s=6649db36fe6563384ed1d2a6d142da6e9f201632">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<!-- Proteasome Modal -->
<div id="phuman" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="phumanModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="phumanModalLabel">Human Proteasome Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br>
        The mass spectral data of all 160 LC-MS/MS experiments were collected at NHLBI Proteomics Center in UCLA. LTQ-XL and LTQ-Orbitrap instruments were used in these analyses. All protein expression images were obtained from Royal Institute of Technology (KTH).
    <p>
        LTQ-XL protocol: The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8 kV. Eleven replicates were analyzed via this protocol.
    </p>
    <p>
        LTQ-Orbitrap protocol: The data-dependent scan was set in the same condition as described above. The resolution of MS1 was set at 60,000. Monoisotopic peak recognition function was enabled. Ten replicates were analyzed via this protocol. 
    </p>
    <p>
        <strong>Tissue Collection:</strong> <br>Human heart samples from the free anterior left ventricular wall were collected from individuals (average age = 49±8 yrs, n=5, 4 males and 1 female) who were previously treated with a left ventricular assist device (LVAD). These individuals exhibit normal left ventricular end diastolic dimension (LVEDD), which was enabled after LVAD treatment.The protocol for proteasome purification is provided in the following manuscript, "Regulation of murine cardiac 20S proteasomes: role of associating partners." (Zong, <em>et. al</em>., <em>Circ.Res. </em>2008;8:1564-75).</p>
    </p>
  </div>
  <div class="modal-footer">
      <!--<div class="pull-left lead"><a href="files/Human Proteasome Proteins.xls">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<div id="pmurine" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="pmurineModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="pmurineModalLabel">Murine Proteasome Module of COPaKB</h3>
  </div>
  <div class="modal-body">
    <p>
        <strong>Data Source:</strong><br />
        The mass spectral data of all 270 LC-MS/MS experiments were collected at NHLBI Proteomics Center in UCLA. LCQ-DECA-XP, LTQ-XL and LTQ-Orbitrap instruments were used in these analyses.&nbsp;
    </p>
    <p>
        LCQ DECA XP protocol: Samples were fractionated with online 1D-RPLC before mass spectrometry analysis. The mass spectrometer was operated in a data-dependent mode with 1 full MS scan followed by 4 MS/MS scans. One replicate was analyzed via this protocol. 
    </p>
    <p>
        LTQ-XL protocol: The mass spectrometer was operated in data-dependent mode with 1 full MS scan followed by 5 MS/MS scans. The normalized collision energy of linear ion trap was set at 35% for ion fragmentation; the temperature of the ion transfer capillary was set at 190°C, and the spray voltage was 1.8 kV. Eleven replicates were analyzed via this protocol. 
    </p>
    <p>
        LTQ-Orbitrap protocol: The data-dependent scan was set in the same condition as described in the LTQ-XL protocol above. The resolution of MS1 scan was set at 60,000. Monoisotopic peak recognition function was enabled. Nine replicates were analyzed via this protocol. 
    </p>
    <p>
        <strong>Tissue Collection:</strong><br>Wild type adult male ICR mice (Harlan) were used in this study. The protocol for proteasome purification is provided in the following manuscript, "Regulation of murine cardiac 20S proteasomes: role of associating partners." (Zong, <em>et. al</em>., <em>Circ.Res. </em>2008;8:1564-75).
    </p>
  </div>
  <div class="modal-footer">
      <!--<div class="pull-left lead"><a href="files/Mouse Proteasome Proteins.xls">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<!-- Nuclear Modal -->
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
      <!--<div class="pull-left lead"><a href="http://www.mcponline.org/content/suppl/2010/08/31/M110.000703.DC1/mcp.M110.000703-2.xls">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<!-- Cytosolic Modal -->
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
      <!--<div class="pull-left lead"><a href="files/Mouse_Heart_Cytosol_Proteins.xls">Download</a> proteins list for this module (.xls)</div>-->
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

</asp:Content>