<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Keyword Search" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="KeywordSearch.aspx.cs" Inherits="KeywordSearch" %>
 
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  Runat="Server">
        <asp:Panel ID="panel1" defaultbutton="bt1" Runat="Server">
            <div class="row-fluid">
              <div class="span12">
                <h2>Keyword Search</h2>
                <div class="alert alert-info">
                <button type="button" class="close button" data-toggle="collapse" data-target="#helpBar">&times;</button>
                <h4>Instructions:</h4>
                <div id="helpBar" class="collapse in">
                    <p>Keyword Search is a versatile tool to help you find the protein, peptide or mass spectrum of your interest. Here are the ways you can explore the properties of your query.</p>
                    <ul class="">
                        <li>If you would like to investigate a particular protein, you can select the "Protein ID" option in the dropdown menu and enter any of these identifiers in the textbox: UniProt ID, protein name, Ensemble ID and gene symbol.</li>
                        <li> If you would like to investigate a specific peptide, you can select the "Peptide Sequence" option in the dropdown menu and provide its sequence (in full or partial form) in the textbox.</li>
                        <li> If you would like to investigate a mass spectrum, you can select the "Spectrum Precursor" option in the dropdown menu and provide its precursor m/z value in the textbox.</li>
                    </ul>
                </div>
              </div>
                  <dl class="dl-horizontal">
                    <dt>Type</dt>
                    <dd>
                        <asp:DropDownList ID="ddlQueryType3" runat="server">
		                    <asp:ListItem>Protein ID</asp:ListItem>
		                    <asp:ListItem>Peptide Sequence</asp:ListItem>
		                    <asp:ListItem>Spectrum Precursor</asp:ListItem>
	                    </asp:DropDownList>
                    </dd>
                    <dt>Keyword</dt>
                     <dd>
                          <asp:TextBox id="tbKeyWords3" name="tbKeyWords3" runat="server" ontextchanged="tbKeyWords_TextChanged3"  placeholder="keyword" ValidationGroup="MasterInputValid"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbKeyWords3" ErrorMessage="*" ValidationGroup="MasterInputValid"></asp:RequiredFieldValidator><br />
                           <asp:Button id="bt1" type="button" class="btn" OnClick="bt1_click" Text="Search Keyword" data-loading-text="Loading..." runat="server" ValidationGroup="MasterInputValid"/>
                     </dd>
                  </dl>
                      
              </div>
            </div>
        </asp:Panel>
    </asp:Content>