<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="SpectrumList.aspx.cs" Inherits="SpectrumList" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Spectral Query Result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid">
    <div class="alert alert-info">
    <button type="button" class="close button" data-toggle="collapse" data-target="#helpNote">&times;</button>
    <h4>Introduction</h4>
    <div id="helpNote" class="collapse in">
        <br />
        <p>Mass spectra that have a precursor m/z value in the range of your specification are listed in the table below. In this table, you can review the peptide sequence, the charge state, the theoretical precursor m/z value and the Xcorr (cross-correlation) score of each of these spectra. Detailed information of each mass spectrum can be found by clicking on the spectral image icon.</p>
    </div>
    </div>
</div>
<div class="row-fluid">
    <h4>Spectra with Precursor m/z in the Range of <asp:Label ID="lbPrecursorRange" runat="server"></asp:Label></h4>
    <asp:Table class="datagrid table table-striped table-hover" ID="tbSpectra" runat="server" BackColor="White" CaptionAlign="Top" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="100%"></asp:Table>
</div>    
</asp:Content>

