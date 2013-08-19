<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="PeptideFamily.aspx.cs" Inherits="PeptideFamily" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Peptide Family" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <h2>Peptide Family:</h2>
    <asp:Table ID="tbPeptides" class="table-hover table-striped" runat="server" BackColor="White" 
    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
    HorizontalAlign="Center" Width="100%">
        <asp:TableRow class="DataGridHeader" ID="TableRow2" runat="server"  
        Height="20px" Font-Bold="True" Font-Size="12px">
            <asp:TableCell ID="TableCell18" runat="server" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" HorizontalAlign="Left">Peptide COPaKB ID</asp:TableCell>
            <asp:TableCell ID="TableCell20" runat="server" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" HorizontalAlign="Left">Sequence</asp:TableCell>
            <asp:TableCell ID="TableCell22" runat="server" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" HorizontalAlign="Right">MW</asp:TableCell>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="Right" BorderColor="White" 
            BorderStyle="Solid" BorderWidth="1px">Score</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</div>
</asp:Content>

