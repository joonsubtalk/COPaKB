<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" EnableSessionState="False" AutoEventWireup="true" CodeFile="MSRUNSearch.aspx.cs" Inherits="MSRUNSearch" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Analysis of Raw Spectral File" %>
<%@ Register assembly="Brettle.Web.NeatUpload" namespace="Brettle.Web.NeatUpload" tagprefix="Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<script type="text/javascript">

    function cbevent() {
        if (document.getElementById('<%= cbUseDefault.ClientID %>').checked == true)
            document.getElementById('<%= ParameterPanel.ClientID %>').style.display = "none";
        else
            document.getElementById('<%= ParameterPanel.ClientID %>').style.display = "block";
    }
    function ShowProgress() {
        document.getElementById("progresspanel").style.display = "block";
    }
    function UsingURL() {
        if (document.getElementById('<%= cbUseURL.ClientID %>').checked == true) {
        document.getElementById('<%= URLPanel.ClientID %>').style.display = "block";
        document.getElementById('<%= InputFile1.ClientID %>').style.display = "none";
        var elem = document.getElementById('<%= InputFile1.ClientID %>');
        elem.parentNode.innerHTML = elem.parentNode.innerHTML;
    }
    else {
        document.getElementById('<%= URLPanel.ClientID %>').style.display = "none";
        document.getElementById('<%= InputFile1.ClientID %>').style.display = "block";
    }

}
function LoadSample() {
    document.getElementById('<%= tbURL.ClientID %>').value = "http://www.heartproteome.org/sample/human_proteasome_sample.mzML";
    }
    function HighResolution() {
        if (document.getElementById('<%= cbHighResolution.ClientID %>').checked == true) {
        document.getElementById('<%= ddl_PrecursorTolerance.ClientID %>').disabled = true;
        document.getElementById('<%= ddlHRPrecursorTolerance.ClientID %>').disabled = false;
        document.getElementById('<%= ddlIsotopePeaks.ClientID %>').disabled = false;
    }
    else {
        document.getElementById('<%= ddl_PrecursorTolerance.ClientID %>').disabled = false;
        document.getElementById('<%= ddlHRPrecursorTolerance.ClientID %>').disabled = true;
        document.getElementById('<%= ddlIsotopePeaks.ClientID %>').disabled = true;
    }
}
 </script>

<div class="row-fluid ">
    <h2>Query COPaKB with .mzML File</h2>
    <div class="alert alert-info">
    <asp:Literal ID="informationBtn" runat="server"></asp:Literal>
    <h4>Introduction:</h4>
    <asp:Literal ID="informationShow" runat="server"></asp:Literal>
            <ul>
                <li>File in either .mzML or .raw (Thermo Scientific Inc.) format is supported. Other formats can be converted to .mzML format using <a href="http://proteowizard.sourceforge.net/">ProteoWizard Converter</a>. You can locate your local .mzML file in the "Local Data File" section. Alternatively, a file in the web (http or ftp) can be used. Its URL address can be specified in the textbox below. </li>
                <li>After the spectral file is uploaded, a task ID will be created and displayed toward the bottom of this page. It carries a hyperlink to review the progress of the ongoing analysis.</li>
                <li>When the analysis is completed, an email will be sent to the address you have provided. The task ID in the email or in this webpage will lead you to review the final report.</li>
                <li>The report is readily retrievable by providing the correct combination of its task ID and associated email address. </li>
            </ul>
            <p>If you would like to process multiple files in one batch, please use the <a href="COPaKBClient.aspx" target="_blank"> COPaKB Client</a> program (powered by Java). If you want to analyze a single ms2 spectrum, please follow <a href="SpectralSearch.aspx"> this link</a>.</p>
        </div>
    </div>
</div>
<div class="row-fluid">
    <asp:Panel ID="panel1" Runat="Server">
        <div class="span12">
        <h2>Spectral Analysis</h2>
            <div class="row-fluid">
                <div class="span3">
                   Email Address:
                </div>
                <div class="span9">
                    <asp:TextBox ID="tbUserID" runat="server" Width="250px" 
                        ValidationGroup="UploadGroup"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="tbUserID" ErrorMessage="(Required)" 
                        ValidationGroup="UploadGroup"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ControlToValidate="tbUserID" ErrorMessage="(Invalid email)" 
                        SetFocusOnError="True" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ValidationGroup="UploadGroup"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span3">
                   COPaKB Module:
                </div>
                <div class="span9">
                    <asp:DropDownList ID="ddlModels" runat="server"  Width="250px">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span3">
                   Local Data File:
                </div>
                <div class="span9">
                    <div class="row-fluid">
                        <div id="keepForJScheckScript" class="forLegacyCode notSureWhy pleaseFixIfYouCan">
                        <Upload:InputFile ID="InputFile1" runat="server" ValidationGroup="UploadGroup"  /> 
                        <asp:RegularExpressionValidator id="RegularExpressionValidator1" 
                            ControlToValidate="inputFile1"
                            ValidationExpression="^.+\.((raw)|(mzML)|(RAW)|(mzml))$"
                            Display="Static"
                            ErrorMessage="Not a Valid .mzML or .Raw File"
                            EnableClientScript="True" 
                            runat="server" ValidationGroup="UploadGroup"/>
                        </div>
                        <asp:CheckBox ID="cbUseURL" runat="server" Text="Or a File in the Web" />
                        <div id="URLPanel" style="display:none" runat ="server">
                            <asp:TextBox ID="tbURL" runat="server" Width="100%" ></asp:TextBox>
                                <a href ="javascript:LoadSample();">Load Sample Resource</a><asp:RegularExpressionValidator 
                                ID="RegularExpressionValidator4" runat="server" ControlToValidate="tbURL" 
                                ErrorMessage="This file is not in a valid mzML format" 
                                ValidationExpression="^(http|https|ftp)\://((.*)(:(.*))?@)?[a-zA-Z0-9\-\.]?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;amp;%\$#\=~])+.(mzML|mzml|MZML)$" 
                                ValidationGroup="UploadGroup"></asp:RegularExpressionValidator>
                        </div>
                        <br />
                        <asp:CheckBox ID="cbUseDefault" runat="server"  Checked ="true"  
                            Text="Use Default Parameters" oncheckedchanged="cbUseDefault_CheckedChanged"/>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="offset3 span9">
                    <!-- Begin Parameter toggle section -->
                    <div id="ParameterPanel" style="display:none ; background-color: #FFFFCC; padding: 20px; margin-bottom: 10px;" runat="server">
                        <div class="row-fluid">
                            <div class="span8">Precursor m/z tolerance:</div>
                            <div class="span4">
                                <div class="row-fluid">
                                    <div class="span8">
                                        <asp:DropDownList ID="ddl_PrecursorTolerance" runat="server" 
                                            width ="90%">
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="span4">
                                        th
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <asp:CheckBox ID="cbHighResolution" runat="server" Text="High Resolution" />
                                </div>
                                <div class="row-fluid">
                                    <div class="span8">
                                        <asp:DropDownList ID="ddlHRPrecursorTolerance" runat="server" Width="90%" 
                                            Enabled="False">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>200</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="span4">
                                        ppm
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <asp:DropDownList ID="ddlIsotopePeaks" runat="server" Width ="80%" 
                                        Enabled="False">
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="span6">
                                        Isotope Peaks
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span8">Peptide confidence threshold:</div>
                            <div class="span4">
                                <asp:DropDownList ID="ddl_Threshold" runat="server"  width ="90%">
                                    <asp:ListItem>95%</asp:ListItem>
                                    <asp:ListItem>98%</asp:ListItem>
                                    <asp:ListItem>99%</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span8">Precursor m/z sliding window:</div>
                            <div class="span4">
                                <div class="row-fluid">
                                    <div class="span9">
                                        <asp:DropDownList ID="ddl_SlideSize" runat="server"  width ="90%">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="span3">  
                                        Th
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span8">PTM induced m/z shift</div>
                            <div class="span4">
                                <div class="row-fluid">
                                    <div class="span9">
                                        <asp:DropDownList ID="ddl_PTMshift" runat="server"  width ="90%">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>-79.9</asp:ListItem>
                                            <asp:ListItem>-40</asp:ListItem>
                                            <asp:ListItem>-42.0</asp:ListItem>
                                            <asp:ListItem>-21</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="span3">  
                                        Th
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span8">Number of distinct peptides for protein ID:</div>
                            <div class="span4">
                                <asp:DropDownList ID="ddl_DistinctPepties" runat="server"  width ="90%">
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>                                       
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span8">Number of unique peptides for protein ID:</div>
                            <div class="span4">
                                <asp:DropDownList ID="ddl_UniquePeptides" runat="server"  width ="90%">
                                    <asp:ListItem>0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>                                       
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <asp:CheckBox ID="cbBonusMS" runat="server" Text="Bonus point for accurate measurements of precursor m/z" /><br />
                            <asp:CheckBox ID="cbUseNDP" runat="server" Checked ="true" Text ="Using noise decoy algorithm to control FDR" /> 
                        </div>
                    </div><!-- End Parameter toggle section -->
                </div>
            </div>
            <div class="row-fluid">
                <div class="offset3 span9">
                    <asp:Button ID="btUpload" runat="server" Text="Upload &amp; Search" 
                        onclick="btUpload_Click" type="button" class="btn btn-primary"  CausesValidation ="true" 
                        ValidationGroup="UploadGroup" />
                                
                        
                    Upload Progress:
                    <div id="progresspanel" style="display:block">
                        <Upload:ProgressBar ID="ProgressBar1" runat="server" Inline="True" width="100%" height="32"/>
                    </div>
                    <p>
                    <asp:Label ID="lbMessage" runat="server" ForeColor="Blue"></asp:Label>
                    </p>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
<div class="row-fluid ">
    <asp:Panel ID="panel2" defaultbutton="btViewReport" Runat="Server">
        <div class="span12">
            <div class="row-fluid">
                 <h2>Search Report</h2>
                 <asp:Label ID="lbReportError" runat="server" ForeColor="Red"></asp:Label>
            </div>
            <div class="row-fluid">
                <div class="span3">Email Address:</div>
                <div class="span9">
                    <asp:TextBox ID="tbQueryUserID" runat="server" Width="229px" 
                        ValidationGroup="ReportGroup"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="tbQueryUserID" ErrorMessage="(Required)" 
                        ValidationGroup="ReportGroup"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                        ControlToValidate="tbQueryUserID" ErrorMessage="(Invalid email)" 
                        SetFocusOnError="True" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ValidationGroup="ReportGroup"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span3">Task ID:</div>
                <div class="span9">
                    <asp:TextBox ID="tbQueryTaskID" runat="server" Width="230px" 
                    ValidationGroup="ReportGroup"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbQueryTaskID" ErrorMessage="(Required)" 
                    ValidationGroup="ReportGroup"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row-fluid">
                <div class="offset3 span9">
                    <asp:Button ID="btViewReport" runat="server" type="button" class="btn btn-primary" onclick="Button3_Click" 
                        Text="View Report" CausesValidation ="true" 
                        ValidationGroup="ReportGroup" />
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
</asp:Content>

