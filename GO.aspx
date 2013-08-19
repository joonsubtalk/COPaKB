<%@ Page Language="C#" MasterPageFile="~/SimpleCOP.master" AutoEventWireup="true" CodeFile="GO.aspx.cs" Inherits="GO" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- Gene Ontology (GO) Analyses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                            <asp:Button ID="btShowInteract" runat="server" Text="Show GO Annotation" 
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
                Cellular Component</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="center">
                <table style="width:100%;">
                    <tr>
                        <td width="60%">
                            <asp:Image ID="imgCellComponent" runat="server" />
                        </td>
                        <td width = "5%">
                            &nbsp;</td>
                        <td>
                            <asp:Table ID="tbCC" runat="server" HorizontalAlign="Left" >
                            </asp:Table>
                        </td>
                    </tr>
                    
                </table>
                                </td>
            <td>
                &nbsp;</td>
        </tr>
         <tr class="Table_Title">
            <td>
                </td>
            <td>
                Biological Process</td>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="center">
                 <table style="width:100%;">
                    <tr>
                        <td width="60%">
                            <asp:Image ID="imgBiologicalProcess" runat="server" />
                        </td>
                        <td width = "5%">
                            &nbsp;</td>
                        <td>
                            <asp:Table ID="tbBP" runat="server" HorizontalAlign="Left">
                            </asp:Table>
                        </td>
                    </tr>
                    
                </table></td>
            <td>
                &nbsp;</td>
        </tr>
        <tr class="Table_Title">
            <td>
                &nbsp;</td>
            <td>
                Molecular Function</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="center">
                 <table style="width:100%;">
                    <tr>
                        <td width="60%">
                            <asp:Image ID="imgMolecularFunction" runat="server" />
                        </td>
                        <td width = "5%">
                            &nbsp;</td>
                        <td>
                            <asp:Table ID="tbMF" runat="server" HorizontalAlign="Left">
                            </asp:Table>
                        </td>
                    </tr>
                    
                </table></td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </form>
</asp:Content>

