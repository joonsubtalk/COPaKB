<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="Proteasome.aspx.cs" Inherits="Proteasome" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Heart Proteasome Proteome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="span12">
        <h2>Current Status of the Proteasome Modules in COPaKB</h2>
        <p>Cardiac proteasomes govern a major degradation pathway of the myocardial proteins, directing virtually all cardiac cellular processes. Therapeutic potential of targetting this protein complex thus motivates greater efforts to understand the molecular diversity of proteasome.</p>
        <p>Molecular heterogeneity of mammalian proteasomes originates from multiple mechanisms, including incorporation of inducible subunits, alternative splicing and association with proteasome-interacting proteins (PIPs). We intend to capture these regulatory factors of the proteasome complex by integrating results from multiple replicate analyses. In COPaKB, we provide proteasome modules from two model systems, i.e., human heart and murine heart. </p>
        <p>Please see the table below for a summary of the training datasets.</p>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <table class="table table-bordered table-hover table-striped">
            <caption><h2>Current Status of the Proteasome Modules in COPaKB</h2></caption>
            <thead>
                <tr>
                    <th>Proteasome Modules in COPaKB</th>
                    <th>Biological Replicates</th>
                    <th>Technical Replicates</th>
                    <th>LC/MS/MS Experiments</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><a href="#phuman" role="button" data-toggle="modal">Human Cardiac Proteasome Module</a></td>
                    <td>5</td>
                    <td>21</td>
                    <td>160</td>
                </tr>
                <tr>
                    <td><a href="#pmurine" role="button" data-toggle="modal">Murine Cardiac Proteasome Module</a></td>
                    <td>14</td>
                    <td>20</td>
                    <td>270</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>

<!-- Modal -->
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
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<!-- Modal -->
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
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

</asp:Content>

