<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="SearchReport.aspx.cs" Inherits="SearchReport" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Searching Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
    if (!$(".alert-success")[0]) {
        setTimeout("location.reload(true);", 3000);
    }
    else {
        clearInterval(refreshIntervalId);
    }
</script>
<div class="row-fluid">
    <h2>Analysis Report</h2> 
    <p><asp:Literal id="con1" runat="server" /></p>
    <p><asp:Literal id="con2" runat="server" /></p>
    <p><asp:Literal id="con3" runat="server" /></p>
</div>
<div class="row-fluid">
    <div class="span6">c
        <div class="row-fluid">
            <div class="span12">
                <h4>Summary of the Analysis:</h4>
                <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
            </div> 
        </div>
        <div class="row-fluid">
            <div class="span3">
                <strong>Task ID:</strong>
            </div> 
            <div class="span9">
                <asp:Literal id="taskidinfo" runat="server" />
            </div> 
        </div>
        <div class="row-fluid">
            <div class="span3">
                <strong>User:</strong>
            </div> 
            <div class="span9">
                <asp:Literal id="user" runat="server" />
            </div> 
        </div>
        <div class="row-fluid">
            <div class="span3">
                <strong>Time:</strong>
            </div> 
            <div class="span9">
                <asp:Literal id="time" runat="server" />
            </div> 
        </div>
        <br />
        <div class="row-fluid">
            <div class="span3">
                <strong>mzML File:</strong>
            </div> 
            <div class="span9">
                <asp:Literal id="mzmlFile" runat="server" />
            </div> 
        </div>
        <div class="row-fluid">
            <div class="span3">
                <strong>Search Status:</strong>
            </div> 
            <div class="span9">
                <asp:Literal id="searchStat" runat="server" />
            </div> 
        </div>
    </div>
    <div class="span6">
        <div class="row-fluid">
            <div class="span12">
                <h4>Settings of the Analysis:</h4>
            </div>
        </div>   
        <div class="row-fluid">
            <div class="span6">
                <strong>COPaKB Module:</strong>
            </div> 
            <div class="span6">
                <asp:Literal id="copakbModule" runat="server" />
            </div> 
        </div>
        <div class="row-fluid">
            <div class="span8">
                <strong>Peptide m/z Tolerance:</strong>
            </div>
            <div class="span4">
                <asp:Label ID="lbPeptideTolerance" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span8">
                <strong>Size of Sliding Window:</strong>
            </div>
            <div class="span4">
                <asp:Label ID="lbSlideSize" runat="server"></asp:Label>&nbsp;Th (Da/e)
            </div>
        </div>
        <div class="row-fluid">
            <div class="span8">
                <strong>Noise Decoy Control:</strong>
            </div>
            <div class="span4">
                <asp:Label ID="lbIsUsingNoiseDecoy" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span8">
                <strong>Bonus for Precursor m/z Accuracy:</strong>
            </div>
            <div class="span4">
                <asp:Label ID="lbBonusms" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span8">
                <strong>Threshold for Identification:</strong>
            </div>
            <div class="span4">
                <asp:Label ID="lbThreshold" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <strong><asp:Label ID="lbPeptideNumber" runat="server"></asp:Label></strong>&nbsp;distinct peptides are required for protein Identification
            </div>
        </div>   
    </div>
</div>
                    
                   
<div class="row-fluid">
    <div class="span12">
        <h4>Results of the Analysis:</h4>
    </div>
</div>   
<div class="row-fluid">
    <div class="span12">
        <asp:Table  class="datagrid table-hover table-striped" ID="tbReport" runat="server" BackColor="White" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%">
            <asp:TableRow class="DataGridHeader" ID="TableRow1" runat="server" 
            Font-Bold="True" Font-Size="12px" Height="20px">
                <asp:TableCell ID="TableCell4" runat="server" BorderColor="White" 
                BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Scan #</asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" BorderColor="White" 
                BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Peptide Sequence</asp:TableCell>
                <asp:TableCell ID="TableCell3" runat="server" BorderColor="White" BorderStyle="Solid" 
                BorderWidth="1px" HorizontalAlign="Left">Similary Score</asp:TableCell>
                <asp:TableCell ID="TableCell6" runat="server" BorderColor="White" BorderStyle="Solid" 
                BorderWidth="1px" HorizontalAlign="Right">ΔM</asp:TableCell>
                <asp:TableCell ID="TableCell10" runat="server" BorderColor="White" BorderStyle="Solid" 
                BorderWidth="1px" HorizontalAlign="Center">Final Score</asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" BorderColor="White" BorderStyle="Solid" 
                BorderWidth="1px">Spectrum</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</div>   
</asp:Content>

