<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Match Spectrum" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="MatchSpectrum.aspx.cs" Inherits="MatchSpectrum"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


<div class="row-fluid">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Mirror illustration between library and query spectra</h2>
            <asp:Image ID="Image1" runat="server" />
            <p class="well">
                Xmin: <asp:TextBox ID="tbXmin" runat="server" Width="70px"></asp:TextBox>
                Xmax: <asp:TextBox ID="tbXmax" runat="server" Width="70px"></asp:TextBox>
                Ymax: <asp:TextBox ID="tbYmax" runat="server" Width="70px"></asp:TextBox>
                <asp:Button ID="btZoomout" runat="server" class="btn btn-primary" onclick="btZoomout_Click" Text="Zoom" />
                <asp:Button ID="btDisplay" runat="server" class="btn btn-primary" onclick="btDisplay_Click" Text="Reset View" />
                <asp:CheckBox ID="cbShowNoise" runat="server" Text="Noise Peaks" />
                <br />
                <asp:CheckBox ID="cbPrintable" runat="server" Text="Printer Friendly" />
                <asp:CheckBox ID="cbShowTheorySpectrum" runat="server" Text="Theoretical Peaks" />
                <asp:CheckBox ID="cbShowLabel" runat="server" Checked="True" Text="m/z Values" />
                <asp:TextBox ID="CustomizedSequence" runat="server" Width="236px"></asp:TextBox>
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="row-fluid">
    <ul class="nav nav-tabs" id="myTab"> 
        <li class="active"><a href="#correlation"><i class="icon-ok-sign"></i> Correlation</a></li>
        <li><a href="#info"><i class="icon-info-sign"></i> Information</a></li>
        <li><a href="#datasource"><i class="icon-file"></i> Data Source</a></li>
        <li><a href="#wiki"><i class="icon-pencil"></i> Wiki</a></li>
    </ul>
</div>
<div class="tab-content">
    <div class="tab-pane active" id="correlation">
        <div class="row-fluid">
            <div class="span12">
                <h4>Correlations between Query Spectrum and Library Spectrum:</h4>
        
                <strong>Matched/Total: #ions:</strong><asp:Label ID="lbIonsPercent" runat="server"></asp:Label>
                &nbsp;&nbsp; <strong>intensity:</strong> <asp:Label ID="lbIntensityPercent" runat="server"></asp:Label>

                <asp:Table class="datagrid table-hover" ID="tbTheoryValues" runat="server" BackColor="White" CaptionAlign="Top" CellPadding="0" CellSpacing="0" Width="100%">
                    <asp:TableRow ID="TableRow1" runat="server" 
                        Height="20px">
                        <asp:TableCell ID="TableCell1" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" HorizontalAlign="Left">b series</asp:TableCell>
                        <asp:TableCell ID="TableCell5" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" ForeColor="Blue" HorizontalAlign="Right"><sup>+1</sup>b</asp:TableCell>
                        <asp:TableCell ID="TableCell6" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" ForeColor="Green" HorizontalAlign="Right"><sup>+1</sup>b-17</asp:TableCell>
                        <asp:TableCell ID="TableCell17" runat="server" ForeColor="YellowGreen" 
                            HorizontalAlign="Right"><sup>+1</sup>b-18</asp:TableCell>
                        <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" ForeColor="Blue" HorizontalAlign="Right"><sup>+2</sup>b</asp:TableCell>

                        <asp:TableCell BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" ForeColor="Red" HorizontalAlign="Right"><sup>+2</sup>y</asp:TableCell>
                        <asp:TableCell ID="TableCell4" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" ForeColor="Purple" HorizontalAlign="Right"><sup>+1</sup>y-18</asp:TableCell>
                        <asp:TableCell ID="TableCell3" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" ForeColor="Brown" HorizontalAlign="Right"><sup>+1</sup>y-17</asp:TableCell>
                        <asp:TableCell ID="TableCell2" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" ForeColor="Red" HorizontalAlign="Right"><sup>+1</sup>y</asp:TableCell>
                        
                        <asp:TableCell ID="TableCell8" runat="server" BorderColor="White" BorderStyle="Solid" 
                            BorderWidth="1px" HorizontalAlign="Right">y series</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div> 
        </div>
    </div>
    <div class="tab-pane" id="info">
        <div class="row-fluid">
            <h4>Information on the Libray Spectrum: </h4>
        </div>
        <div class="row-fluid">
            <div class="span6">
                <strong>Instrument:</strong> <asp:Label ID="lbInstrumentation" runat="server"></asp:Label><br />
                <strong>Xcorr:</strong> <asp:Label ID="lbXcorr" runat="server"></asp:Label><br />
                <strong>Peptide COPaKB ID:</strong> <asp:Label ID="lbPeptideCOPID" runat="server"></asp:Label>
            </div>
            <div class="span6">
                <strong>Precursor m/z:</strong> (<asp:Label ID="lbPrecursormz" runat="server"></asp:Label>)+<asp:Label ID="lbChargeState" runat="server"></asp:Label><br />
                <strong>DeltaCN:</strong> <asp:Label ID="lbDeltaCN" runat="server"></asp:Label><br />
                <strong>Peptide Sequence:</strong> <asp:Label ID="lbModifiedSequence" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row-fluid">
            <asp:Label ID="lbPeptideFamily" runat="server"></asp:Label>
        </div>
    </div>
    <div class="tab-pane" id="datasource">
        <div class="row-fluid">
            <h4> Data Sources </h4>
            <asp:Literal ID="ltContributors" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="tab-pane" id="wiki">
        <div class="row-fluid">
            <h4>Wiki&nbsp; <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Edit</asp:LinkButton></h4>
            <asp:Literal ID="litContent" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal>
        </div>
    </div>
</div>

                                                
    
    
</asp:Content>