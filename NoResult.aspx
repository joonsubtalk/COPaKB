<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Your Query Returned No Result" Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="NoResult.aspx.cs" Inherits="NoResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div class="row-fluid">
    <div class="alert alert-info">
            <button type="button" class="close" data-dismiss="alert">&times;</button>
            <h4>Exception Occured while Accessing COPaKB</h4>
            <p>There was no result matching your query; please verify the input.</p>
        </div>
</div>

</asp:Content>

