<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Peptide Information" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="PeptideInfo.aspx.cs" Inherits="PeptideInfo2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="span12">
        <h2>Peptide Info</h2>
    </div>
</div>
<div class="row-fluid">
    <div class="alert alert-info">
    <button type="button" class="close button" data-toggle="collapse" data-target="#helpNote">&times;</button>
    <h4>Properties of your search peptide are outlined below:</h4>
    <div id="helpNote" class="collapse in">
        <ol class="">
            <li>Amino acid sequence and molecular weight of your search peptide are presented in the section of "Peptide Information" below.</li>
            <li>Three tabs below organize the following information:
                <ul>
                    <li>“Observed Spectra” tab presents the mass spectra associated with your search peptide. These mass spectra also show post-translational modifications and different charge states of your search peptide.</li>
                    <li>"Candidate Protein(s)" tab lists candidate proteins that contain the sequence of your search peptide.</li>
                    <li>“Wiki” tab has been prepared to provide a forum for discussion on the properties of your search peptide.</li>
                </ul>
            </li>
            
         </ol>
    </div>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <h4>Peptide Information</h4>
        <div class="row-fluid">
            <div class="span3"><strong>Peptide COPaKB ID: </strong></div>
            <div class="span9"><asp:Label ID="lbPeptide_ID" runat="server"></asp:Label> <asp:Label ID="lbOtherPID" runat="server"></asp:Label></div>
        </div>
        <div class="row-fluid">
            <div class="span3"><strong>Peptide Sequence: </strong></div>
            <div class="span9"><asp:Label ID="lbPepetide_Sequence" runat="server"></asp:Label></div>
        </div>
        <div class="row-fluid">
            <div class="span3"><strong>Molecular Weight: </strong></div>
            <div class="span9"><asp:Label ID="lbMolecular_Weight" runat="server"></asp:Label></div>
        </div>
    </div>
</div>
<div class="row-fluid">
    <ul class="nav nav-tabs" id="myTab">
      <li class="active"><a href="#observed"><i class="icon-eye-open"></i> Observed Spectra</a></li>
      <li><a href="#candidate"><i class="icon-list-alt"></i> Candidate Protein(s)</a></li>
      <li><a href="#wiki"><i class="icon-pencil"></i> Wiki</a></li>
    </ul>
</div>
<div class="tab-content">
    <div class="tab-pane active" id="observed">
        <div class="row-fluid">
            <h4>Observed Spectra</h4>
            <asp:Table ID="tbModified" runat="server" BackColor="White" 
                CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                HorizontalAlign="Center" Width="100%" GridLines="Horizontal" BorderColor="Silver">
            </asp:Table>
        </div>
    </div>
    <div class="tab-pane" id="candidate">
        <div class="row-fluid">
            <h4>Candidate Protein(s)</h4>
            <asp:Table class="datagrid" ID="tbRefProtein" runat="server" BackColor="White" 
            CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
            HorizontalAlign="Center" Width="100%">
                <asp:TableRow ID="TableRow2" class="DataGridHeader" runat="server"  
                    Height="20px" Font-Bold="True" Font-Size="12px">
                    <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" 
                        BorderWidth="1px" HorizontalAlign="Left">Protein COPaKB ID</asp:TableCell>
                    <asp:TableCell ID="TableCell9" runat="server" BorderColor="White" BorderStyle="Solid" 
                        BorderWidth="1px" HorizontalAlign="Left">Protein Name</asp:TableCell>
                    <asp:TableCell ID="TableCell10" runat="server" BorderColor="White" BorderStyle="Solid" 
                        BorderWidth="1px" HorizontalAlign="Left">Species</asp:TableCell>
                    <asp:TableCell ID="TableCell11" runat="server" BorderColor="White" BorderStyle="Solid" 
                        BorderWidth="1px" HorizontalAlign="Center"><span title ="Gene Chromosome">Chrom</span></asp:TableCell>
                    <asp:TableCell ID="TableCell8" runat="server" BorderColor="White" BorderStyle="Solid" 
                        BorderWidth="1px" HorizontalAlign="Right"><span title ="Peptide Sequence Location in Protein Sequence">Location</span></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="tab-pane" id="wiki">
        <div class="row-fluid">
            <h4>Wiki <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Edit</asp:LinkButton></h4>
            <small><asp:Literal ID="lastMod" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal></small>
            <asp:Literal ID="litContent" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal>
        </div>
    </div>
</div>
</asp:Content>