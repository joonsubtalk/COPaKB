<%@ Page Language="C#" MasterPageFile="~/COP.master" AutoEventWireup="true" CodeFile="Contribution.aspx.cs" Inherits="Contribution" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB)-- Data Exchange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td class="style62" style="width: 17px; height: 55px;">
                </td>
            <td style="width: 562px; height: 55px; font-size: 12px;" 
                >
COPaKB welcomes 
                sharing of proteomic knowledge among cardiovascular investigators. <br />
                                            </td>
            <td style="height: 55px">
                </td>
        </tr>
        <tr>
            <td class="style62" style="width: 17px">
                &nbsp;</td>
            <td style="width: 562px">
                <table style="width:100%;">
                    <tr>
                        <td  colspan=3  bgcolor="#EDE7CF"  class="Table_Title">
                            Contact Information</td>
                       
                    </tr>
                    <tr>
                        <td class="style62" style="width: 169px; height: 25px;">
                            Institution <span  style="color: #FF0000">*</span></td>
                        <td class="style69">
                            <asp:TextBox ID="TextBox1" runat="server" Width="373px"></asp:TextBox>
                                                        </td>
                        
                    </tr>
                    <tr>
                        <td class="style62" style="width: 169px">
                            Name (Last, First) <span  style="color: #FF0000">*</span></td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Width="372px"></asp:TextBox>
                                                        </td>
                        
                    </tr>
                    <tr>
                        <td class="style62" style="width: 169px">
                            Email <span  style="color: #FF0000">*</span></td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" Width="372px"></asp:TextBox>
                                                        </td>
                        
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style62" style="width: 17px">
                &nbsp;</td>
            <td style="width: 562px">
                <table style="width:100%;" align="right">
                    <tr class="Table_Title">
                        <td colspan=3 bgcolor="#EDE7CF" >
                            Experimental Condition</td>
                       
                    </tr>
                    <tr>
                        <td class="style62" style="width: 170px">
                            Enzyme</td>
                        <td style="width: 378px">
                            <asp:DropDownList ID="DropDownList7" runat="server" Height="22px" Width="373px">
                                <asp:ListItem>Trypsin</asp:ListItem>
                                <asp:ListItem>Lys-C</asp:ListItem>
                                <asp:ListItem>Glu-C</asp:ListItem>
                                <asp:ListItem>Asp-N</asp:ListItem>
                                <asp:ListItem>Chymotrypsin</asp:ListItem>
                                <asp:ListItem>Pepsin</asp:ListItem>
                                <asp:ListItem>Proteinase K</asp:ListItem>
                            </asp:DropDownList>
                                                        </td>
                        
                    </tr>
                     <tr>
                        <td class="style62" style="width: 170px">
                            Instrument</td>
                        <td style="width: 378px">
                            <asp:DropDownList ID="DropDownList8" runat="server" Height="22px" Width="373px">
                                <asp:ListItem>LTQ or LCQ</asp:ListItem>
                                <asp:ListItem>LTQ Orbitrap/FT</asp:ListItem>
                                <asp:ListItem>ETD</asp:ListItem>
                                <asp:ListItem>Q-Exactive</asp:ListItem>
                                <asp:ListItem>Q-TOF</asp:ListItem>
                                <asp:ListItem>Triple Quad</asp:ListItem>
                                <asp:ListItem>TOF-TOF</asp:ListItem>
                                <asp:ListItem>Other</asp:ListItem>
                            </asp:DropDownList>
                                                        </td>
                        
                    </tr>
                    <tr>
                        <td class="style62" style="width: 170px">
                            Chemical or Metabolic Labeling</td>
                        <td style="width: 378px">
                            <asp:TextBox ID="TextBox6" runat="server" Width="366px"></asp:TextBox>
                                                        </td>
                        
                    </tr>
                                          <tr>
                        <td class="style62" style="width: 170px">
                            Additional Specifications on Sample Preparation and Instrument Operation</td>
                        <td style="width: 378px" colspan=3>
                            <asp:TextBox ID="TextBox3" runat="server" Rows="6" TextMode="MultiLine" 
                                Width="366px"></asp:TextBox>
                        </td>
                        
                    </tr>
                   
                </table>
            </td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
            <td class="style62" style="width: 17px">
                &nbsp;</td>
            <td style="width: 562px">
                <table style="width:100%;">
          
                    <tr>
                        <td class="style62" style="width: 170px">
                            Citation (PMID or DOI):</td>
                        <td style="width: 378px">
                            <asp:TextBox ID="TextBox4" runat="server" Width="370px"></asp:TextBox>
                        </td>
                       
                    </tr>
                     <tr>
                        <td class="style62" style="width: 170px">
                                                        Targeted COPaKB Module <span  style="color: #FF0000">*</span></td>
                        <td style="width: 373px">
                            <asp:TextBox ID="TextBox7" runat="server" Width="370px"></asp:TextBox>
                        </td>
                       
                    </tr>
                </table>
             </td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
            <td class="style62" style="width: 17px">
                &nbsp;</td>
            <td style="width: 562px">
                <table style="width:100%;">
                    <tr class="Table_Title">
                        <td colspan =3 bgcolor="#EDE7CF" style="height: 19px" >
                            Data Upload</td>
                        
                    </tr>
                    
                    <tr>
                        <td class="style62" style="width: 256px">
                            Your data file or its compressed form:<br />
                            (raw, ms2, mzML, zip or rar formats)</td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="280px" Height="19px" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr><tr>
                        <td colspan=3 style="text-align: justify">
                            If the size of your file is big (&gt; 1GB), please email us to coordinate the 
                            upload procedure. Alternatively, you can use ProteomeXchange to deposit your data and 
                            email us its &quot;dataset ID&quot; afterwards.</td>
                       
                    </tr>
                </table>
             </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
        <td ></td>
         <td >
                            <asp:Button ID="Button3" runat="server" Height="22px" Text="Submit" 
                                Width="74px" />
                        </td>
          <td ></td>
        </tr>
    </table>
</asp:Content>

