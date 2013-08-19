<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PeptideIdentified.aspx.cs" Inherits="PeptideIdentified" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Identified Peptides" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <meta charset="utf-8"/>
	<title>COPa Knowledgebase Query Report</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content="Joon-Sub Chung"/>
    <link rel="stylesheet" type="text/css" media="all" href="./assets/css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" media="all" href="./assets/css/bootstrap-responsive.min.css"/>

<style type="text/css">
.datagrid tr:hover td 
{ 
        background-color:#f2e8da; 
} </style>
<script>
var preEl ;
var orgBColor;
var orgTColor;
function HighLightTR(el, backColor,textColor){
if(typeof(preEl)!='undefined') {
preEl.bgColor=orgBColor;
try{ChangeTextColor(preEl,orgTColor);}catch(e){;}
}
orgBColor = el.bgColor;
orgTColor = el.style.color;
el.bgColor=backColor;
try{ChangeTextColor(el,textColor);}catch(e){;}
preEl = el;
}
function ChangeTextColor(a_obj,a_color){ ;
for (i=0;i<a_obj.cells.length;i++)
a_obj.cells(i).style.color=a_color;
}
</script>
</head>
<body>
    <div style ="cursor:pointer;padding-left:5px;padding-right:5px;padding-top:1px ">
        <asp:Table ID="tblPeptide" runat="server" class="table-hover table-striped datagrid" BackColor="#E0F3FF" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%">
            <asp:TableRow class="DataGridHeader" ID="TableRow1" runat="server" Height="20px" Font-Bold="True" Font-Size="12px">
                <asp:TableCell ID="TableCell1" runat="server" BorderStyle="Solid" BorderColor="White" BorderWidth="1px" Width="200px">Sequence</asp:TableCell>
                <asp:TableCell ID="TableCell3" runat="server" BorderStyle="Solid" BorderColor="White" BorderWidth="1px" HorizontalAlign ="Right">Score</asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" BorderStyle="Solid" BorderColor="White" BorderWidth="1px">∆m/z</asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" BorderStyle="Solid" BorderColor="White" BorderWidth="1px">File</asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderColor="White" BorderWidth="1px" HorizontalAlign="Right">Scan</asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderColor="White" BorderWidth="1px" HorizontalAlign="Right">Similarity</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</body>
</html>
