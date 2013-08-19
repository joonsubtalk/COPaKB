<%@ Page Language="C#" MasterPageFile="~/SimpleCOP.master" AutoEventWireup="true" CodeFile="IntActReport.aspx.cs" Inherits="IntActReport" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Protein Interactions (InAct)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
<!--
.verticaltext {
writing-mode: tb-rl;
filter: flipv fliph;
}
-->
</style>


    <form id="form1" runat="server">
    <table style="width:100%;">
        <tr class="Table_Title">
            <td>
                &nbsp;</td>
            <td width="96%">
                Protein List:<asp:RadioButton ID="rbUniprot" runat="server" GroupName="protein" 
                    Text="UniProt ID" />
                <asp:RadioButton ID="rbIPI" runat="server" 
                    GroupName="protein" Text="IPI" ValidationGroup="protein" />
                <asp:RadioButton ID="rbGeneName" runat="server" Checked="True" 
                    GroupName="protein" Text="Gene Name" ValidationGroup="protein" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td style="width: 96%">
                            <asp:TextBox ID="tbProteinLists" runat="server" Height="80px" 
                                TextMode="MultiLine" Width="100%"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="btShowInteract" runat="server" Text="Show Interactions" 
                                onclick="btShowInteract_Click" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr class="Table_Title">
            <td>
                &nbsp;</td>
            <td>
                Interaction Matrix </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="left">
                <asp:Table class="datagrid" ID="tblInteract" runat="server" GridLines="Both" Font-Names="Arial">
                </asp:Table>
            </td>
            <td>
                &nbsp;</td>
        </tr>
         <tr class="Table_Title">
            <td>
                &nbsp;</td>
            <td>
                Pathways in Reactome</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="left">
                <asp:Table width="100%" class="datagrid" ID="tbPathways" runat="server" GridLines="Both" Font-Names="Arial" HorizontalAlign="Left">
                </asp:Table>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </form>
</asp:Content>

