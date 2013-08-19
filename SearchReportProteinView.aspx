<%@ Page Language="C#" MasterPageFile="~/SimpleCOP.master" AutoEventWireup="true" CodeFile="SearchReportProteinView.aspx.cs" Inherits="SearchReportProteinView" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Protein View" %>
<%@ Register TagPrefix="obspl" Namespace="OboutInc.Splitter2" Assembly="obout_Splitter2_Net"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:TreeView ID="TreeView1" runat="server" onselectednodechanged="TreeView1_SelectedNodeChanged" >
        <Nodes>
            <asp:TreeNode Text="Proteins" Value="Proteins"></asp:TreeNode>
        </Nodes>
    </asp:TreeView>
    <obspl:Splitter CollapsePanel="left" runat="server" StyleFolder="styles/default2" LiveResize="true" CookieDays="0" id="mySpl">
				<LeftPanel WidthMin="100" WidthMax="400">
					<Header Height="50">
						<div style="width:100%;height:100%;" >
							<br />
						</div>
					</Header>
					<content>
					<div style="margin:5px;">
						
    <ul>
							<li>
								<a href="javascript:mySpl.loadPage('RightContent', 'aspnet_ViewPDF.aspx?bookId=1')">ASP.NET Development</a>
							</li>
							<li>
								<a href="javascript:mySpl.loadPage('RightContent', 'aspnet_ViewPDF.aspx?bookId=5')">Developing Applications Using VB.NET and ASP.NET</a>
							</li>
							<li>
								<a href="javascript:mySpl.loadPage('RightContent', 'aspnet_ViewPDF.aspx?bookId=2')">Advanced ASP.NET 2.0 and ASP.NET AJAX</a>
							</li>
							<li>
								<a href="javascript:mySpl.loadPage('RightContent', 'aspnet_ViewPDF.aspx?bookId=3')">ASP.NET State Management</a>
							</li>
							<li>
								<a href="javascript:mySpl.loadPage('RightContent', 'aspnet_ViewPDF.aspx?bookId=4')">ASP.NET with Visual Basic .NET Training</a>
							</li>
						</ul>
					</div>
					</content>
				</LeftPanel>
			
				<RightPanel>
					<content>
					<div  style="width:400px;height:80%;padding-left:30px;padding-top:30px">
						<h2>PDF Library</h2>
						Choose a book from the left menu...
					</div>
					</content>
				</RightPanel>
			</obspl:Splitter>
    
    
</asp:Content>

