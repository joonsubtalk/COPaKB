<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Search Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">     

    function cbevent()
{
if  (document.getElementById('<%= cbShowDetail.ClientID %>').checked==true)
    document.getElementById("ParameterPanel").style.display ="block";
else
        document.getElementById("ParameterPanel").style.display ="none";
}
</script>
    <style type="text/css">
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
    </style>
  
<div class="row-fluid">
    <div class="span12">
        <h2>COPa Knowledgebase Search Report</h2>
        <asp:Label ID="lbSave" runat="server"></asp:Label>
    </div><!-- end span 12-->
</div><!-- end row -->
<div class="row-fluid">
    <div class="span12">
        <h4>Overview of the Analysis</h4>
    </div><!-- end span 12-->
</div><!-- end row -->
<div class="row-fluid">
    <div class="span4">
        <strong>mzML File:</strong>
    </div><!-- end span-->
    <div class="span8">
        <asp:Label ID="lbmzML" runat="server"></asp:Label>
    </div><!-- end span-->
</div><!-- end row -->
<div class="row-fluid">
    <div class="span4">
        <strong>COPaKB Module:</strong>
    </div><!-- end span-->
    <div class="span8">
        <asp:Label ID="lbSearchingModule" runat="server" >[hlIPIProteinID]</asp:Label>
    </div><!-- end span-->
</div><!-- end row -->
<div class="row-fluid">
    <div class="span4">
        <strong>Number of Protein ID:</strong>
    </div><!-- end span-->
    <div class="span8">
        <asp:Label ID="lbProteinCount" runat="server"></asp:Label>
    </div><!-- end span-->
</div><!-- end row -->
<div class="row-fluid">
    <div class="span4">
        <strong>Analytical Settings:</strong>
    </div><!-- end span-->
    <div class="span8">
        <asp:Label ID="lbSC" runat="server"></asp:Label><br /><asp:CheckBox ID="cbShowDetail" runat="server" Text="Show Details" onclick="return cbevent();" />
    </div><!-- end span-->
</div><!-- end row -->


<div id="ParameterPanel" style="display:none ; background-color: #FFFFCC;">
    <table width ="100%" >
<tr >
<td width="50%" >
&nbsp;
Peptide m/z Tolerance:</td><td>
<asp:Label ID="lbPeptideTolerance" runat="server"></asp:Label></td>
                                                           
</tr>
                                                        
<tr>
<td   >
&nbsp;
Allow Slide Size:</td><td>
<asp:Label ID="lbSlideSize" runat="server"></asp:Label>
Th (Da/e)</td>
                                                           
</tr>
<tr >
<td width="20%" >
&nbsp;
Use the Noise Decoy:</td><td>
<asp:Label ID="lbIsUsingNoiseDecoy" runat="server"></asp:Label></td>
                                                           
</tr>
<tr>
<td   >
&nbsp; Bonus accuracy of precursor m/z:</td><td>
<asp:Label ID="lbBonusms" runat="server"></asp:Label>
</td>
                                                           
</tr>
<tr>
<td   >
&nbsp; Detect PTM Shift:</td><td>
<asp:Label ID="lbPTMShift" runat="server"></asp:Label>
</td>
                                                           
</tr>
<tr >
<td width="20%" >
&nbsp;
Threshold:</td><td>
<asp:TextBox ID="lbThreshold" runat="server"></asp:TextBox></td>
                                                           
</tr>
                                                        
<tr>
<td  colspan="2" >
&nbsp;
<asp:TextBox ID="lbPeptideNumber" runat="server"></asp:TextBox>
&nbsp;distinct peptides needed for protein identification<br />
&nbsp;
<asp:TextBox ID="lbUniquePeptide" runat="server"></asp:TextBox>
&nbsp;unique peptides needed for protein identification</td>
                                                           
</tr>
<tr><td>
<asp:Button ID="Button1" runat="server" class="btn" onclick="Button1_Click" 
Text="Reset the Creteria" />
</td>
</tr>
</table>

</div>

<div class="row-fluid">
    <div class="span12">
        <h4>Information on Individual Proteins</h4>

        <asp:Image ID="Image1" runat="server" ImageUrl="~/_image/blank.GIF"/>
        <asp:Panel ID="Panel1" runat="server">
            <asp:Label ID="Label1" runat="server" Text="Threshold (X100):"></asp:Label>
            <asp:TextBox ID="tbThreshold" runat="server" Width="77px" 
            ValidationGroup="ThresholdGroup"></asp:TextBox>
            <asp:Button ID="btRestThreshold" class="btn" runat="server" onclick="btRestThreshold_Click" 
            Text="Apply New Threshold" ValidationGroup="ThresholdGroup" />
            <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="tbThreshold" ErrorMessage="(Required)" 
            ValidationGroup="ThresholdGroup"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="tbThreshold" ErrorMessage="(Invalid)" 
            ValidationExpression="[0-9]{0,2}(\.[0-9]*)?" ValidationGroup="ThresholdGroup"></asp:RegularExpressionValidator>
        </asp:Panel>
    </div>
</div>
<div class="row-fluid">
    <div class="span7">
        <asp:Label ID="lbProteinAccession" runat="server" Text="Protein-centric View" 
BackColor ="#99CCFF" style="padding: 10px 0" BorderStyle="None" 
BorderWidth="1px"></asp:Label>
        <asp:Label ID="lbGeneSymbol" runat="server" BorderStyle="Dashed" 
Text="Gene-centric View" style="padding: 10px" BackColor="#FFFFFF"
BorderWidth="1px"></asp:Label>
    </div>
    
    <div class="offset1 span4" style="padding: 10px 0;">
        <asp:Label ID="lbGO" runat="server"></asp:Label>
&nbsp; <asp:Label ID="lbShowInteract" runat="server" 
></asp:Label>
    </div>
    
</div>

<div class="row-fluid">
    <asp:Table ID="tbProteins" class="table-bordered table table-condensed table-stiped table-hover" runat="server" BackColor="White" Width="100%" 
    BorderWidth="0px" CellPadding="0" CellSpacing="0">
    <asp:TableRow ID="TableRow1" BackColor="#99CCFF"   runat="server" >
    <asp:TableCell ID="TableCell1" runat="server" BorderColor="Silver" BorderStyle="Solid" 
    BorderWidth="1px" Width="20%" HorizontalAlign="Left">Protein</asp:TableCell>
    <asp:TableCell ID="TableCell2" runat="server" BorderColor="Silver" BorderStyle="Solid" 
    BorderWidth="1px" Width="60%" HorizontalAlign ="Left" >Semi-Quantitative Measure (NSAF)</asp:TableCell>
    </asp:TableRow>
    </asp:Table>
</div>

</asp:Content>

