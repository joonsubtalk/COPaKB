<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- Result of Your Analysis" Language="C#" MasterPageFile="~/SimpleCOP.master" AutoEventWireup="true" CodeFile="epiTestNoMk.aspx.cs" Inherits="epiTestNoMk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%" class="Content" >
    <tr>
                                                <td class="style46" style="font-size: 12px" valign="top">
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td valign="top" >
                                                                <table style="width:100%;height:24px"  cellpadding ="0" cellspacing ="0">
                                                                    <tr bgcolor="#0066CC" style="color: #FFFFFF">
                                                                        <td class="Table_Title" colspan="3" bgcolor="#EDE7CF">
                                                                            &nbsp; Search Information</td>
                                                                    </tr>
                                                                </table>
                                                                </td>
                                                           
                                                       </tr>
                                                        <tr>
                                                            <td valign="top" class="Table_Content">
                                                                <asp:Table class="datagrid" ID="tbInfo" runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                                                                   
                                                                </asp:Table>
                                                                </td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td class="style73" valign="top">
                                                                </td>
                                                           
                                                        </tr>
                                                    </table>
                                                    </td>
                                                
                        
                        </tr>
                        <tr>
                                                <td class="style46" style="font-size: 12px" valign="top">
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td valign="top" >
                                                                <table style="width:100%;height:24px"  cellpadding ="0" cellspacing ="0">
                                                                    <tr bgcolor="#0066CC" style="color: #FFFFFF">
                                                                        <td class="Table_Title" colspan="3" bgcolor="#EDE7CF">
                                                                            &nbsp; Analyses</td>
                                                                    </tr>
                                                                </table>
                                                                </td>
                                                           
                                                       </tr>
                                                        <tr>
                                                            <td valign="top" class="Table_Content">
                                                                <asp:Table class="datagrid" ID="tbtaskids" runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                                                                   
                                                                </asp:Table>
                                                                </td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td class="style73" valign="top">
                                                                </td>
                                                           
                                                        </tr>
                                                    </table>
                                                    </td>
                                                
                        
                        </tr>
                        <tr>
                            <td class="style46" style="font-size: 12px" valign="top">
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td valign="top" >
                                                                <table style="width:100%;height:24px"  cellpadding ="0" cellspacing ="0">
                                                                    <tr bgcolor="#EDE7CF" >
                                                                        <td class="Table_Title" colspan="3">
                                                                            &nbsp;&nbsp;Common Proteins Among These Analyses</td>
                                                                        
                                                                    </tr>
                                                                    
                                                                </table>
                                                                </td>
                                                           
                                                       </tr>
                                                        <tr>
                                                            <td valign="top" class="Table_Content">
                                                                <asp:Table class="datagrid" ID="tbSharedProtein" runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                                                                    <asp:TableRow ID="TableRow2" class="DataGridHeader" runat="server"  
                                                                        Height="20px" Font-Bold="True" Font-Size="12px">
                                                                        <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left">Protein COPaKB ID</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell9" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left">Protein Name</asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                                </td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td class="style73" valign="top">
                                                                </td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                           
                                                        </tr>
                                                    </table>
                                                    </td>
                            
                        </tr>
                        </table>
</asp:Content>

