<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- Integration of Multiple Analyses" Language="C#" MasterPageFile="~/SimpleCOPaKB.master" AutoEventWireup="true" CodeFile="epiSearch.aspx.cs" Inherits="epiSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<form id="form2" runat="server" defaultbutton="">
<div class="row-fluid">
    <div class="alert alert-info">
    <button type="button" class="close button" data-toggle="collapse" data-target="#helpBar">&times;</button>
        <h2>Analysis of Multiple Datasets:</h2>
        <div id="helpBar" class="collapse in">
        <p>You can integrate the results of multiple analyses by providing the task IDs of existing analyses 
by any investigator team and/or by different groups in the "Task IDs" textbox.</p>
        <p><strong>(i)</strong> combining proteins commonly identified in these analyses <br />
            <strong>(ii)</strong> pinpointing analyses that carry a common biomarker. The list of analyses can be specified by their &quot;<strong>Task IDs</strong>&quot;. <br /></p>
    </div>
</div>
<div class="row-fluid">
    <h2>Specifications:</h2>
</div>
    <asp:Panel ID="panel2" defaultbutton="btViewReport" Runat="Server" Width="810px">
        <div class="row-fluid">
            <div class="span3"><strong>Task IDs</strong></div>
            <div class="span9"><asp:TextBox ID="tbQueryUserID" runat="server" Width="229px" ValidationGroup="ReportGroup"></asp:TextBox></div>
        </div>
        <div class="row-fluid">
            <div class="span3"><strong>Biomarker (UniProt)</strong></div>
            <div class="span9"><asp:TextBox ID="tbQueryTaskID" runat="server" Width="231px" ValidationGroup="ReportGroup"></asp:TextBox> (Optional)</div>
        </div>
        <div class="row-fluid">
            <div class="offset3 span9"><asp:Button ID="btViewReport" runat="server" onclick="Button3_Click" Text="Search" Width="98px" CausesValidation ="true" ValidationGroup="ReportGroup" /></div>
        </div>
    </asp:Panel>      
</form>
</asp:Content>

