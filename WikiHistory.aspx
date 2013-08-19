<%@ Page Language="C#" MasterPageFile="~/Wiki.master" AutoEventWireup="true" CodeFile="WikiHistory.aspx.cs" Inherits="WikiHistory" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- COPaKB Wiki Page Revision Hisotry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td bgcolor="#EDE7CF" class="Table_Title" style="height: 24px">
                Revision history on <asp:Label ID="lbPageName" runat="server"></asp:Label>
            </td> 
        </tr>
        <tr>
            <td>
                <asp:Table class="table table-hover table-striped" ID="tbVersionList" runat="server" Width="100%">
                    <asp:TableRow class="DataGridHeader" runat="server" >
                        <asp:TableCell runat="server" Width="20%"  >Version ID</asp:TableCell>
                        <asp:TableCell runat="server"  >Time</asp:TableCell>
                        <asp:TableCell runat="server" >Contributor</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#EDE7CF" class="Table_Title" style="height: 24px">View the content</td> 
        </tr>
        <tr>
            <td><asp:Literal ID="litContent" runat="server" EnableViewState="False" Mode="PassThrough"></asp:Literal></td>
        </tr>
    </table>
</asp:Content>

