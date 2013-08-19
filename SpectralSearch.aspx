<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Spectral Analysis" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="SpectralSearch.aspx.cs" Inherits="SpectralSearch"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  Runat="Server">
    <asp:Panel ID="panel1" defaultbutton="btSearch" Runat="Server">

            <div class="row-fluid ">
                <h2>Query COPaKB with .DTA files</h2>
                <div class="alert alert-info">
                <button type="button" class="close button" data-toggle="collapse" data-target="#helpBar">&times;</button>
                <h4>Introduction:</h4>
                    <div id="helpBar" class="collapse in">
                        <p>
                        This webpage is dedicated to analyze individual MS2 spectrum in DTA format. Spectrum can be uploaded as a file from your computer or its content can be loaded as plain text into the textbox. You may test-run this utility by loading a sample file available in the &quot;Load a Sample Spectrum&quot; section. If you would like to analyze raw spectra files, please use <a href="COPaKBClient.aspx" target="_blank">COPaKB Client</a> (batch analysis) or visit <a href ="MSRUNSearch.aspx"> this webpage</a> (single file analysis).
                        </p>
                    </div>
                </div>
                <h2>Spectral Analysis</h2>
            </div>
            <div class="row-fluid ">
                <div class="span4">
                    COPaKB Module:
                </div>
                <div class="span8">
                    <asp:DropDownList ID="ddlModules" runat="server"  Width="220px">
                        <asp:ListItem>Murine Heart Mitochondria</asp:ListItem>
                        <asp:ListItem>Human Heart Mitochondria</asp:ListItem>
                    </asp:DropDownList>(<a href ='COPaWikiDefault.aspx?PageName=COPa Library Module'>Info on COPaKB Modules</a>)
                </div>
            </div>
            <div class="row-fluid ">
                <div class="span4">
                    Precursor m/z:
                </div>
                <div class="span8">
                    <asp:TextBox ID="tbPrecursorMZ" runat="server" Width="192px" Font-Bold="True" ValidationGroup="InputValid">0</asp:TextBox> Th 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbPrecursorMZ" ErrorMessage="(Required)" ValidationGroup="InputValid"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row-fluid ">
                <div class="span4">
                    Precursor m/z Tolerance:
                </div>
                <div class="span8">
                    <asp:TextBox ID="tbTolerance" runat="server" Width="192px" Font-Bold="True" ValidationGroup="InputValid">2.0</asp:TextBox> Th
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbTolerance" ErrorMessage="(Required)" ValidationGroup="InputValid"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row-fluid ">
                <div class="span4">
                    Fragment m/z Sliding Window:
                </div>
                <div class="span8">
                    <asp:DropDownList ID="ddlChargeState" runat="server"  Width="50px" Font-Bold="True">
                        <asp:ListItem>0</asp:ListItem>
                        <asp:ListItem Selected="True">1</asp:ListItem>                                
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                    </asp:DropDownList>
                    Th (<a href='COPaWikiDefault.aspx?PageName=Slide Dot Product' target =_blank >Info on Sliding Window</a>)
                </div>
            </div>
            <div class="row-fluid">
                <div class="span4">
                    <div class="row-fluid">
                        MS2 Spectrum (peaks list):
                    </div>
                    <div class="row-fluid">
                        Load a Sample Spectrum:
                        <asp:DropDownList ID="ddlSamples" runat="server" Width="120px">
                        </asp:DropDownList>
                        <asp:Button ID="btLoadSample" runat="server" type="button"  onclick="LoadSample_Click" Text="Load" Width="66px" />
                    </div>
                </div>
                <div class="span8">
                        <asp:TextBox ID="tbDTA" class="input-block-level" runat="server" Height="161px" TextMode="MultiLine" ValidationGroup="InputValid"></asp:TextBox>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span4"><span class="pull-right">Upload a DTA File: </span></div>
                <div class="span8">
                <asp:FileUpload ID="FileUpload1" runat="server" ToolTip="upload dta file from you local computer" />
                        <asp:Button ID="btUpload" runat="server" onclick="btUpload_Click" Text="Upload" type="button"  />
                        <asp:Button ID="btClearData" runat="server" Text="Clear data" type="button"  onclick="btClearData_Click" />
                </div>
            </div>     
            <div class="row-fluid">
                <div class="offset4 span8">
                      <br />
                     <asp:Button ID="btSearch" runat="server" Text="Search" Width="108px" type="button" class="btn btn-primary" onclick="btSearch_Click" ValidationGroup="InputValid" />
                     <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>         
            <hr>   
            <div class="row-fluid">
                <div class="span12">
                    Display Results with Final Scores &gt; <asp:TextBox ID="tbCutoff" runat="server" Width="61px">0.4</asp:TextBox> (0~1) (<a href ='COPaWikiDefault.aspx?PageName=COPa Final Score'>Info on Final Score</a>)
                </div>
            </div>                     
            <div class="row-fluid">
                <div class="span12"><h2>Search Report</h2></div>
                <div class="span12">
                    <asp:Table ID="tbPeptides" runat="server" BackColor="White"  CssClass="table table-condensed table-hover table-striped" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%" style="margin-left: 0px">
                        <asp:TableRow class="DataGridHeader" ID="TableRow1" runat="server"  
                            Height="20px" Font-Bold="True" Font-Size="12px">
                            <asp:TableCell ID="TableCell1" runat="server" BorderColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" HorizontalAlign="Center"> ID</asp:TableCell>
                            <asp:TableCell ID="TableCell2" runat="server" BorderColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" HorizontalAlign="Center">Peptide Sequence</asp:TableCell>
                            <asp:TableCell ID="TableCell3" runat="server" BorderColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" HorizontalAlign="Center">Modification</asp:TableCell>
                            <asp:TableCell ID="TableCell4" runat="server" BorderColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" HorizontalAlign="Center">Score</asp:TableCell>
                            <asp:TableCell ID="TableCell5" runat="server" BorderColor="White" BorderStyle="Solid" 
                                BorderWidth="1px" HorizontalAlign="Center">Spectra</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
    </asp:Panel>
</asp:Content>

