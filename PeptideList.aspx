<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Peptide List" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="PeptideList.aspx.cs" Inherits="PeptideList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="alert alert-info">
    <button type="button" class="close button" data-toggle="collapse" data-target="#helpNote">&times;</button>
    <h4>Introduction</h4>
    <div id="helpNote" class="collapse in">
        <br />
        <p>Peptides that contain your search sequence are listed below. The table is categorized into peptide sequence, molecular weight and sequence length. Mass spectral data and candidate protein(s) for each peptide sequence can be viewed by clicking on the sequence. The table presents up to 50 candidate sequences. You can narrow down the list by providing a more specific query sequence.</p>  
    </div>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <h4>Peptides that Matched Your Query: <asp:Label ID="peptideKey" runat="server"></asp:Label></h4>
        <asp:Table class="datagrid table table-hover table-striped" ID="tbPeptides" runat="server" BackColor="White" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%">
            <asp:TableRow class="DataGridHeader" ID="TableRow1" runat="server" Height="20px" Font-Bold="True" Font-Size="12px">
                <asp:TableCell ID="TableCell1" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Peptide Sequence</asp:TableCell>
                <asp:TableCell ID="TableCell3" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Location of Query String</asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center">MW</asp:TableCell>
                <asp:TableCell ID="TableCell6" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center">Length</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</div>  
</asp:Content>

