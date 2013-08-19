<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Protein List" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="ProteinList.aspx.cs" Inherits="ProteinList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="alert alert-info">
    <button type="button" class="close button" data-toggle="collapse" data-target="#helpNote">&times;</button>
    <h4>Introduction</h4>
    <div id="helpNote" class="collapse in">
        <br />
        <p>Proteins listed below matched your query. The table includes UniProt ID, protein name, species origin and gene symbol of each protein. You can access detailed properties by clicking on its UniProt ID.</p>  
    </div>
    </div>
</div>
<div class="row-fluid">
    <h4>Proteins that Matched Your Query: <asp:Label ID="proteinKey" runat="server"></asp:Label></h4>  
    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    <asp:Table class="datagrid table table-hover table-striped" ID="tbRefProtein" runat="server" BackColor="White" 
    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
    HorizontalAlign="Center" Width="100%" >
        <asp:TableRow class="DataGridHeader" ID="TableRow2" runat="server"  
        Height="20px" 
        Font-Bold="True" Font-Size="12px" Width="100%">
            <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" HorizontalAlign="Left">Protein 
            ID</asp:TableCell>
            <asp:TableCell ID="TableCell9" runat="server" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" HorizontalAlign="Left">Protein Name</asp:TableCell>
            <asp:TableCell ID="TableCell10" runat="server" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" HorizontalAlign="Left">Species</asp:TableCell>
            <asp:TableCell ID="TableCell8" runat="server" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" HorizontalAlign="Left">Gene Symbol</asp:TableCell>
    </asp:TableRow>
    </asp:Table>
</div>
</asp:Content>

