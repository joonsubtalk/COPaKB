<%@ Page Language="C#" MasterPageFile="~/SimpleCOPaKB.master" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Your Query Returned No Result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">

    <div class="row-fluid">
        <div class="alert alert-info">
            <button type="button" class="close" data-dismiss="alert">&times;</button>
            <h4>Exception Occured while Accessing COPaKB</h4>
            <p>
                <br /><strong>Note:</strong> You were directed here because you requested to access a protected or a void 
                webpage.</p>
        </div>
    </div>
    </form>
</asp:Content>

