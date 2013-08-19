<%@ Page Language="C#" MasterPageFile="~/Wiki.master" AutoEventWireup="true" CodeFile="COPaWikiDefault.aspx.cs" Inherits="COPaWikiDefault" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- Wiki Pages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
   
    <table style="width:100%;" cellpadding=0 cellspacing=0 >
        <tr style="height: 32px">
            <td style="width: 10px" >
                &nbsp;</td>
            <td style="width: 711px" bgcolor="#D9EDF7" class="Table_Title">
                &nbsp;<asp:Label ID="lbPageName" runat="server" Text="lbPageName" Font-Size="Large"></asp:Label>
&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lbEdit" runat="server" Font-Underline="True" 
                    onclick="lbEdit_Click">Edit</asp:LinkButton>
&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lbViewHistory" runat="server" Font-Underline="True" 
                    onclick="lbViewHistory_Click">View 
                history</asp:LinkButton>
            </td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
            <td style="width: 10px">
                &nbsp;</td>
            <td style="width: 711px">
               <asp:Literal ID="litContent" runat="server" EnableViewState="False" 
                                                                    Mode="PassThrough"></asp:Literal>
             </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 10px">
                &nbsp;</td>
            <td style="width: 711px">
                <asp:Panel ID="pNewPages" runat="server">
                 <table style="width:100%;">
                 <tr>
                        <td class="Table_Title alert-info" style="height: 24px">
                            Recently updated Wiki pages</td>
                        
                    </tr>
                    <tr>
                        <td>
                                                                <asp:Table ID="tbNewPages" class="datagrid"
                                runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                                                                    <asp:TableRow class="DataGridHeader" ID="TableRow4" runat="server" 
                                                                        Height="20px" Font-Bold="True" Font-Size="12px">
                                                                        <asp:TableCell ID="TableCell12" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left" Width="50%">Page Name</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell14" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="30%">Latest Update</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell15" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="20%">Author</asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                                </td>
                        
                    </tr>
                    <tr>
                        <td bgcolor="#D9EDF7" class="Table_Title" style="height: 24px">
                            Recently updated Wiki pages on proteins</td>
                        
                    </tr>
                    <tr>
                        <td>
                                                                <asp:Table class="datagrid" ID="tbProteinPages" 
                                runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                                                                    <asp:TableRow class="DataGridHeader" ID="TableRow1" runat="server"  
                                                                        Height="20px" Font-Bold="True" Font-Size="12px">
                                                                        <asp:TableCell ID="TableCell1" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left" Width="50%">Protein ID</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell2" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="30%">Latest Update</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell3" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="20%">Author</asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                                </td>
                        
                    </tr>
                    <tr>
                        <td bgcolor="#D9EDF7" class="Table_Title" style="height: 24px">
                            Recently updated Wiki pages on peptides</td>
                        
                    </tr>
                    <tr>
                        <td>
                                                                <asp:Table class="datagrid" ID="tbPeptidePages" 
                                runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                                                                    <asp:TableRow class="DataGridHeader" ID="TableRow2" runat="server" 
                                                                        Height="20px" Font-Bold="True" Font-Size="12px">
                                                                        <asp:TableCell ID="TableCell4" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left" Width="50%">Peptide ID</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell5" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="30%">Latest Update</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell6" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="20%">Author</asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                                </td>
                        
                    </tr>
                    <tr>
                        <td bgcolor="#D9EDF7" class="Table_Title" style="height: 24px">
                            Recently updated Wiki pages on mass spectra</td>
                        
                    </tr>
                    <tr>
                        <td>
                                                                <asp:Table class="datagrid" ID="tbSpectrumPages" 
                                runat="server" BackColor="White" 
                                                                    CaptionAlign="Top" CellPadding="0" CellSpacing="0" 
                                                                    HorizontalAlign="Center" Width="100%">
                                                                    <asp:TableRow class="DataGridHeader" ID="TableRow3" runat="server"  
                                                                        Height="20px" Font-Bold="True" Font-Size="12px">
                                                                        <asp:TableCell ID="TableCell7" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Left" Width="50%">Spectrum ID</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell8" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="30%">Latest Update</asp:TableCell>
                                                                        <asp:TableCell ID="TableCell9" runat="server" BorderColor="White" BorderStyle="Solid" 
                                                                            BorderWidth="1px" HorizontalAlign="Right" Width="20%">Author</asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                                </td>
                        
                    </tr>
                </table>
                </asp:Panel>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 10px">
                &nbsp;</td>
            <td style="width: 711px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
   
</asp:Content>

