<%@ Page Language="C#" MasterPageFile="~/Wiki.master" AutoEventWireup="true" CodeFile="WikiEdit.aspx.cs" Inherits="WikiEdit" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) -- COPaKB Wiki Page Editor" ValidateRequest="false" %>
<%@ Register TagPrefix="FTB" Assembly="FreeTextBox" Namespace="FreeTextBoxControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

    <Script>
    var FTB_LinkPopUpHtml = "<html><body><head><title>Link Editor</title>"+
"<style type='text/css'>"+
"html, body { "+
	"background-color: #ECE9D8; "+
	"color: #000000; "+
	"font: 11px Tahoma,Verdana,sans-serif; "+
	"padding: 0px; "+
"} "+
"body { margin: 5px; } "+
"form { margin: 0px; padding: 0px;} "+
"table { "+
"  font: 11px Tahoma,Verdana,sans-serif; "+
"} "+
"form p { "+
"  margin-top: 5px; "+
"  margin-bottom: 5px; "+
"} "+
"h3 { margin: 0; margin-top: 4px;  margin-bottom: 5px; font-size: 12px; border-bottom: 2px solid #90A8F0; color: #90A8F0;} "+
".fl { width: 9em; float: left; padding: 2px 25px; text-align: right; } "+
".fr { width: 7em; float: left; padding: 2px 25px; text-align: right; } "+
"fieldset { padding: 0px 10px 5px 5px; } "+
"button { width: 75px; } "+
"select, input, button { font: 11px Tahoma,Verdana,sans-serif; } "+
".space { padding: 2px; } "+
".title { background: #ddf; color: #000; font-weight: bold; font-size: 120%; padding: 3px 10px; margin-bottom: 10px; "+
"border-bottom: 1px solid black; letter-spacing: 2px; "+
"} "+
".f_title { text-align:right; }"+
".footer { border-top:2px solid #90A8F0; padding-top: 3px; margin-top: 4px; text-align:right; }"+
"</style>"+
"<script type='text/javascript'>"+
"function insertLink() {"+
"	ftb = window.launchParameters['ftb'];"+
"	link = ftb.GetNearest('a');"+
"	href = document.getElementById('link_href');"+
"	if (href.value == '') {"+
"		alert('You must enter a link');"+
"		return false;"+
"	}"+
"	if (!link) {"+
"		var tempUrl = 'http://tempuri.org/tempuri.html';"+
"		ftb.ExecuteCommand('createlink',null,tempUrl);"+
"		var links = ftb.designEditor.document.getElementsByTagName('a');"+
"		for (var i=0;i<links.length;i++) {"+
"			if (links[i].href == tempUrl) {"+
"				link = links[i];"+
"				break;"+
"			}"+
"		}"+
"	}"+
"	updateLink(link);"+
"}"+
"function updateLink(link) {"+
"	if (link) {"+
"		href = document.getElementById('link_href');"+
"		title = document.getElementById('link_title');"+
"		target = document.getElementById('link_target');"+
"		cssClass = document.getElementById('link_cssClass');"+
"		targetVal = target.options[target.selectedIndex].value;"+
"		customtarget = document.getElementById('link_customtarget');"+
"		link.href = href.value;"+
"		link.setAttribute('temp_href', href.value) ;"+
"		if (title.value != '') "+
"			link.title = title.value;"+
"		if (cssClass.value != '') "+
"			link.className = cssClass.value;"+
"		if (targetVal == '_custom') {"+
"			if (customtarget.value != '') "+
"				link.target = customtarget.value; "+
"		} else { "+
"			if (targetVal != '') "+
"				link.target = targetVal;"+
"			else"+
"				link.removeAttribute('target');"+
"		} "+
"	}"+
"}"+
"function link_target_changed() {"+
"	list = document.getElementById('link_target');"+
"	customtarget = document.getElementById('link_customtarget');"+
"	if (list.options[list.options.selectedIndex].value == '_custom')"+
"		customtarget.style.display = '';"+
"	else"+
"		customtarget.style.display = 'none';"+
"}<"+"/script>"+
"</head>"+
"<body>"+
"<form action='' onsubmit='insertLink();window.close();'> "+
"<h3>Link Editor</h3> "+
"<fieldset><legend>Link Properties</legend><table>"+
"<tr><td class='f_title'>URL</td>"+
"<td><input type='text' id='link_href' style='width:250px;' /></td></tr>"+
"<tr><td class='f_title'>Title</td>"+
"<td><input type='text' id='link_title' style='width:250px;' /></td></tr>"+
"<tr><td class='f_title'>Target</td>"+
"<td><select id='link_target' style='width:150px;' onchange='link_target_changed();'>"+
"<option value=''>None</option>"+
"<option value='_blank'>New Window (_blank)</option>"+
"<option value='_top'>Top Frame (_top)</option>"+
"<option value='_parent'>Parent Frame (_parent)</option>"+
"<option value='_self'>Same Frame (_self)</option>"+
"<option value='_custom'>Custom Target</option>"+
"</select>&nbsp;"+
"<input type='text' id='link_customtarget' style='width:95px;display:none;' /> "+
"</td></tr>"+
"<tr style='display:none;'><td class='f_title'>Class</td>"+
"<td><input type='text' id='link_cssClass' style='width:250px;' /></td></tr>"+
"</table>"+
"</fieldset>"+
"<div class='footer'>"+
"<button type='button' name='insertLinkButton' id='insertLinkButton' onclick='insertLink();window.close();'>OK</button>"+
"<button type='button' name='cancel' id='cancelButton' onclick='window.close();'>Cancel</button>"+
"</div>"+
"<script type='text/javascript'>"+
"function load() {"+
"	ftb = window.launchParameters['ftb'];"+
"	link = ftb.GetNearest('a');"+
"	href = document.getElementById('link_href');"+
"	title = document.getElementById('link_title');"+
"	target = document.getElementById('link_target');"+
"	customtarget = document.getElementById('link_customtarget');"+
"	cssClass = document.getElementById('link_cssClass');"+
"	if (link) {"+
"		var url = link.getAttribute('temp_href');"+
"		href.value = (url != '') ? url : link.href;"+
"		title.value = link.title;"+
"		cssClass.value = link.className;"+
"		if (link.target == '' || link.target == '_blank' || link.target == '_top' || link.target == '_self' || link.target == '_parent')"+
"			window.opener.FTB_SetListValue(target,link.target,true);"+
"		else"+
"			window.opener.FTB_SetListValue(target,'_custom',false);"+
"		"+
"		if (target.options[target.options.selectedIndex].value == '_custom') {"+
"			customtarget.style.display='';"+
"			customtarget.value = link.target;"+
"		}"+
"	}"+
"}"+
"</"+"script>"+
"</"+"form> "+
"</body> "+
"</html>";
</Script>
<table style="width:100% " cellpadding=0 cellspacing =0>
<tr style="height: 32px"> <td style="width: 6px"></td><td bgcolor="#D9EDF7" 
        class="Table_Title" style="width: 603px"> &nbsp;<asp:Label ID="lbPageName" runat="server" Text="lbPageName" Font-Size="Large"></asp:Label></td></tr>
<tr style ="height:6px"><td></td></tr>
<tr><td style="width: 6px"></td><td style="width: 603px"><FTB:FreeTextBox DesignModeCss="" AutoParseStyles="true" HtmlModeCss="" 
    id="ftbEdit" runat="server" 
                ToolbarStyleConfiguration="Office2000" 
                toolbarlayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker|InsertDiv,EditStyle,InsertImageFromGallery,Preview,SelectAll|Bold,Italic,Underline,Strikethrough,Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink|Cut,Copy,Paste,Delete;Undo,Redo,Print|SymbolsMenu,StylesMenu,InsertHtmlMenu|InsertRule,InsertDate,InsertTime|InsertTable,EditTable;InsertTableRowAfter,InsertTableRowBefore,DeleteTableRow;InsertTableColumnAfter,InsertTableColumnBefore,DeleteTableColumn|InsertForm,InsertTextBox,InsertTextArea,InsertRadioButton,InsertCheckBox,InsertDropDownList,InsertButton" 
                ImageGalleryPath="images" Height="500px" Width="100%" />
                <div class="well">
            Name: <asp:TextBox id="nameEdit" name="nameEdit" runat="server" placeholder="Name" ValidationGroup="MasterInputValid"></asp:TextBox>        
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div><asp:Literal id="prompt" runat="server" /></td></tr>
</table>
    
</asp:Content>

