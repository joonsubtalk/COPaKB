<%@ Page Language="C#" MasterPageFile="~/COP.master" AutoEventWireup="true" CodeFile="SearchProtein.aspx.cs" Inherits="SearchProtein" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Protein Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td></td>
            <td  class="Table_Title" align="left" bgcolor="#EDE7CF" colspan="2" 
                                                                style="height: 24px">
                Advanced Protein Query </td>
           
        </tr>
        <tr>
            <td style="height: 106px; width: 11px">
            </td>
            <td style="width: 553px; height: 106px">
                <table style="width:100%; font-weight: bold;">
                    <tr>
                        <td class="style62" style="width: 157px">
                            Terms</td>
                             <td style="width: 56px">
                                 &nbsp;</td>
                        <td style="width: 246px">
                            Values</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style62" style="width: 157px">
                            <asp:DropDownList ID="ddField1" runat="server" Height="21px" Width="156px">
                                <asp:ListItem Value="REF_Protein_ID">Protein ID</asp:ListItem>
                                <asp:ListItem Value="Protein_Name">Protein Name</asp:ListItem>
                                <asp:ListItem Value="Protein_Name">Protein Name</asp:ListItem>
                                <asp:ListItem Value="Organism_Source">Organism Source</asp:ListItem>
                                <asp:ListItem Value="Protein_Sequence">Protein Sequence</asp:ListItem>
                                <asp:ListItem Value="Sequence_Length">Sequence Length</asp:ListItem>
                                <asp:ListItem>Chromosome</asp:ListItem>
                                <asp:ListItem>Molecular Weight</asp:ListItem>
                                <asp:ListItem Value="isoelectronic_point">IP</asp:ListItem>
                                <asp:ListItem>GRAVY</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                         <td style="width: 56px">
                             <asp:DropDownList ID="ddOperator1" runat="server">
                                 <asp:ListItem>=</asp:ListItem>
                                 <asp:ListItem>&lt;&gt;</asp:ListItem>
                                 <asp:ListItem>like</asp:ListItem>
                                 <asp:ListItem>&gt;</asp:ListItem>
                                 <asp:ListItem>&lt;</asp:ListItem>
                                 <asp:ListItem>&gt;=</asp:ListItem>
                                 <asp:ListItem>&lt;=</asp:ListItem>
                             </asp:DropDownList>
                        </td>
                        <td style="width: 246px">
                            <asp:TextBox ID="Term1" runat="server" Width="283px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList2" runat="server">
                                <asp:ListItem>AND</asp:ListItem>
                                <asp:ListItem>OR</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style62" style="width: 157px">
                            <asp:DropDownList ID="ddField2" runat="server" Height="21px" Width="156px">
                                <asp:ListItem Value="REF_Protein_ID">Protein ID</asp:ListItem>
                                <asp:ListItem Value="Protein_Name">Protein Name</asp:ListItem>
                                <asp:ListItem Value="Protein_Name">Protein Name</asp:ListItem>
                                <asp:ListItem Value="Organism_Source">Organism Source</asp:ListItem>
                                <asp:ListItem Value="Protein_Sequence">Protein Sequence</asp:ListItem>
                                <asp:ListItem Value="Sequence_Length">Sequence Length</asp:ListItem>
                                <asp:ListItem>Chromosome</asp:ListItem>
                                <asp:ListItem>Molecular Weight</asp:ListItem>
                                <asp:ListItem Value="isoelectronic_point">IP</asp:ListItem>
                                <asp:ListItem>GRAVY</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                         <td style="width: 56px">
                             <asp:DropDownList ID="ddOperator2" runat="server">
                                 <asp:ListItem>=</asp:ListItem>
                                 <asp:ListItem>&lt;&gt;</asp:ListItem>
                                 <asp:ListItem>like</asp:ListItem>
                             </asp:DropDownList>
                        </td>
                        <td style="width: 246px">
                            <asp:TextBox ID="Term2" runat="server" Width="283px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList6" runat="server">
                                <asp:ListItem>AND</asp:ListItem>
                                <asp:ListItem>OR</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style62" style="width: 157px">
                            <asp:DropDownList ID="ddField3" runat="server" Height="21px" Width="156px">
                                <asp:ListItem Value="REF_Protein_ID">Protein ID</asp:ListItem>
                                <asp:ListItem Value="Protein_Name">Protein Name</asp:ListItem>
                                <asp:ListItem Value="Protein_Name">Protein Name</asp:ListItem>
                                <asp:ListItem Value="Organism_Source">Organism Source</asp:ListItem>
                                <asp:ListItem Value="Protein_Sequence">Protein Sequence</asp:ListItem>
                                <asp:ListItem Value="Sequence_Length">Sequence Length</asp:ListItem>
                                <asp:ListItem>Chromosome</asp:ListItem>
                                <asp:ListItem>Molecular Weight</asp:ListItem>
                                <asp:ListItem Value="isoelectronic_point">IP</asp:ListItem>
                                <asp:ListItem>GRAVY</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                         <td style="width: 56px">
                             <asp:DropDownList ID="ddOperator3" runat="server">
                                 <asp:ListItem>=</asp:ListItem>
                                 <asp:ListItem>&lt;&gt;</asp:ListItem>
                                 <asp:ListItem>like</asp:ListItem>
                             </asp:DropDownList>
                        </td>
                        <td style="width: 246px">
                            <asp:TextBox ID="Term3" runat="server" Width="283px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList9" runat="server">
                                <asp:ListItem>AND</asp:ListItem>
                                <asp:ListItem>OR</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style62" style="width: 157px">
                            <asp:Button ID="Button3" runat="server" Text="Search" Width="118px" />
                        </td>
                             <td style="width: 56px">
                                 &nbsp;</td>
                        <td style="width: 246px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
            <td class="style6" style="height: 106px">
            </td>
        </tr>
        <tr>
            <td style="width: 11px">
                &nbsp;</td>
            <td style="width: 553px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
     <table style="width:100%;">
        <tr>
            <td valign="top" >
                <table style="width:100%;height:24px"  cellpadding ="0" cellspacing ="0">
                    <tr bgcolor="#EDE7CF" >
                        <td class="Table_Title">
                            Proteins 
                            that matced 
                            your query</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" class="Table_Content">
                <asp:Table ID="tbRefProtein" runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                    <asp:TableRow ID="TableRow2" runat="server" BackColor="#DEEDFB" ForeColor="#32699B" 
                                                                        Height="20px" Font-Bold="True" Font-Size="12px">
                        <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left">Protein COPaKB ID</asp:TableCell>
                        <asp:TableCell ID="TableCell9" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left">Protein Name</asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left">Species</asp:TableCell>
                        <asp:TableCell ID="TableCell11" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Center">Gene</asp:TableCell>
                        <asp:TableCell ID="TableCell8" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left">Protein IPI ID</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </td>
        </tr>
        <tr>
            <td class="style73" valign="top" style="color: #FF0000">
                page is in processing</td>
        </tr>
    </table>
</asp:Content>

