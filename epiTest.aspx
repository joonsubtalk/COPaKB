<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- Result of Your Analysis" Language="C#" MasterPageFile="~/SimpleCOPaKB.master" AutoEventWireup="true" CodeFile="epiTest.aspx.cs" Inherits="epiTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="span12">
        <h2>Analysis Setting</h2>
        <asp:Table class="datagrid" ID="tbInfo" runat="server" BackColor="White" 
        CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
        HorizontalAlign="Center" Width="100%">
        </asp:Table>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <h2>Tasks Containing the Biomarker(s)</h2>
        <asp:Table class="datagrid" ID="tbtaskids" runat="server" BackColor="White" 
        CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
        HorizontalAlign="Center" Width="100%">

            <asp:TableRow ID="TableRow1" class="DataGridHeader" runat="server"  
            Height="20px" Font-Bold="True" Font-Size="12px">
                <asp:TableCell ID="TableCell1" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Task ID</asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Location</asp:TableCell>
                <asp:TableCell ID="TableCell3" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Time</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <h2>Proteins Commonly Found in the Tasks Noted Above </h2>
        <asp:Table class="datagrid table table-condensed table-hover table-striped" ID="tbSharedProtein" runat="server" BackColor="White" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%">
            <asp:TableRow ID="TableRow2" class="DataGridHeader" runat="server"  
            Height="20px" Font-Bold="True" Font-Size="12px">
            <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Protein COPaKB ID</asp:TableCell>
            <asp:TableCell ID="TableCell9" runat="server" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left">Protein Name</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</div>
</asp:Content>

