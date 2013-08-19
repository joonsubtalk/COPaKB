<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Proteins" Language="C#" MasterPageFile="~/SimpleCOP.master" AutoEventWireup="true" CodeFile="totalProteins.aspx.cs" Inherits="totalProteins" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
<table style="width:100%" class="Content" >
    <tr>
                                                <td class="style46" style="font-size: 12px" valign="top">
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td valign="top" >
                                                                <table style="width:100%;height:24px"  cellpadding ="0" cellspacing ="0">
                                                                    <tr bgcolor="#0066CC" style="color: #FFFFFF">
                                                                        <td class="Table_Title" colspan="3" bgcolor="#EDE7CF">
                                                                            &nbsp; Total Proteins</td>
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
                                                            <td class="style73" valign="top" style="height: 19px">
                                                                <br />
                                                                <br />
                                                                <br />
                                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                                <br />
                                                                <br />
                                                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                                                <br />
                                                                <br />
                                                                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                                                <br />
                                                                <br />
                                                                <br />
                                                                </td>
                                                           
                                                        </tr>
                                                    </table>
                                                    </td>
                                                
                        
                        </tr>
                        
                        </table>
    </form>
</asp:Content>

