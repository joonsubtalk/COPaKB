<%@ Page Language="C#" MasterPageFile="~/COPaKB.master" AutoEventWireup="true" CodeFile="COPaKBClient.aspx.cs" Inherits="COPaKBClient" Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— COPaKB Client" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row-fluid ">
    <div class="span12">
        <h2>COPaKB Client</h2>
        <div class="alert alert-info">
        <button type="button" class="close button" data-toggle="collapse" data-target="#helpBar">&times;</button>
        <h4>Introduction:</h4>
            <div id="helpBar" class="collapse in">
                <p>COPaKB Client integrates a set of bioinformatics tools to help you manage and review proteomic analyses on your computer (PC, Mac or Linux; 32-bit or 64-bit). This program is tailored to process a large batch of raw data files. At the end of the analyses, this program provides an integrated report of the proteomic and genomic signatures, imaging datasets and biomedical attributes related to heart biology.</p>
                <p>COPaKB Client has been created to overcome possible loss of data when there is a failure in network connectivity. This is achieved by dividing large spectral files into small data packets, which are submitted independently to the server. Step-by-step illustration on the application of COPaKB Client is available in <a href="http://www.heartproteome.org/copa/COPaWikiDefault.aspx?PageName=COPaKB Client">COPaKB Wiki</a>.</p>
            </div>
        </div>
    </div>
</div>
<div class="row-fluid">
    <h2>Release Information</h2>
    <p>The current version of COPaKB Client is 2.0. Please select the appropriate version of COPaKB Client that matches the operating system (Windows, OS X or Linux) of your computer. Java Standard Edition Runtime Environment version 7 (32-bit or 64-bit) is also required to run this program. This is freely available from Oracle Corp. If you experience issues with Mac, please reboot it in Windows mode.</p>
</div>
<div class="row-fluid">
    <h2>Download Clients</h2>
    <ul class="thumbnails">
        <li class="span4">
            <div class="thumbnail">
                <img alt="Windows" class="oslogo" src="_image/Windows-Flag.png"/>
                <div class="caption">
                    <h3 class="text-center">Windows</h3>
                    <div class="row-fluid">
                        <div class="span2"><img alt="Java" class="minijava" src="_image/logoJava.png"/></div>
                        <div class="span10"><p>COPaKB Client Download</p></div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <p><a href="http://149.142.254.181/COPa2Client/COPaKB-win-x86.jar" class="btn btn-primary input-block-level">Windows (32-bit)</a></p>
                            <p><a href="http://149.142.254.181/COPa2Client/COPaKB-win-x86_64.jar" class="btn btn-info input-block-level">Windows (64-bit)</a></p>
                        </div>
                    </div>
                    
                </div>
            </div>
        </li>
        <li class="span4">
            <div class="thumbnail">
                <img alt="Mac" class="oslogo" src="_image/mac-os-x-logo.png"/>
                <div class="caption">
                    <h3 class="text-center">Mac</h3>
                    <div class="row-fluid">
                        <div class="span2"><img alt="Java" class="minijava" src="_image/logoJava.png"/></div>
                        <div class="span10"><p>COPaKB Client Download</p></div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <p><a href="#mac32" role="button" data-toggle="modal" class="btn btn-primary input-block-level"><!--<a href="http://149.142.254.181/COPa2Client/COPaKB-mac-x86.jar" class="btn btn-primary">-->Mac (32-bit)</a></p>
                            <p><a href="#mac64" role="button" data-toggle="modal" class="btn btn-info input-block-level"><!--<a href="http://149.142.254.181/COPa2Client/COPaKB-mac-x86_64.jar" class="btn btn-info">-->Mac (64-bit)</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </li>
        <li class="span4">
            <div class="thumbnail">
                <img alt="Linux" class="oslogo" src="_image/linux_icon.png"/>
                <div class="caption">
                    <h3 class="text-center">Linux</h3>
                    <div class="row-fluid">
                        <div class="span2"><img alt="Java" class="minijava" src="_image/logoJava.png"/></div>
                        <div class="span10"><p>COPaKB Client Download</p></div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <p><a href="http://149.142.254.181/COPa2Client/COPaKB-lin-x86.jar" class="btn btn-primary input-block-level">Linux (32-bit)</a></p>
                            <p><a href="http://149.142.254.181/COPa2Client/COPaKB-lin-x86_64.jar" class="btn btn-info input-block-level">Linux (64-bit)</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </li>
    </ul>
</div>

<div id="mac32" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="mac32ModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="mac32ModalLabel">Downloading Mac (32-bit)</h3>
  </div>
  <div class="modal-body">
    <div class="alert alert-error">
        Currently, our Mac version of COPaKB Client implementation depends on the SWT_AWT bridge, but it has been reported to be broken by the recent Mac OSX update in the JDK. More information of the bug can be found <a href="http://git.eclipse.org/c/platform/eclipse.platform.swt.git/commit/bundles/org.eclipse.swt/Eclipse%20SWT%20AWT/cocoa/org/eclipse/swt/awt/SWT_AWT.java?h=integration&id=269e9bd88659168cd99ab994fb73a4e91595fd06">here</a>. We hope that a bug fix will be reported soon.
        <!--https://bugs.eclipse.org/bugs/show_bug.cgi?id=374199#c27-->
    </div>
  </div>
  <div class="modal-footer">
      <div class="pull-left lead"><a href="http://149.142.254.181/COPa2Client/COPaKB-mac-x86.jar">Download</a> Mac (32-bit)</div>
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

<div id="mac64" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="mac64ModalLabel" aria-hidden="true">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
    <h3 id="mac64ModalLabel">Downloading Mac (64-bit)</h3>
  </div>
  <div class="modal-body">
    <div class="alert alert-error">
        Currently, our Mac version of COPaKB Client implementation depends on the SWT_AWT bridge, but it has been reported to be broken by the recent Mac OSX update in the JDK. More information of the bug can be found <a href="http://git.eclipse.org/c/platform/eclipse.platform.swt.git/commit/bundles/org.eclipse.swt/Eclipse%20SWT%20AWT/cocoa/org/eclipse/swt/awt/SWT_AWT.java?h=integration&id=269e9bd88659168cd99ab994fb73a4e91595fd06">here</a>. We hope that a bug fix will be reported soon.
        <!--https://bugs.eclipse.org/bugs/show_bug.cgi?id=374199#c27-->
    </div>
  </div>
  <div class="modal-footer">
      <div class="pull-left lead"><a href="http://149.142.254.181/COPa2Client/COPaKB-mac-x86_64.jar">Download</a> Mac (64-bit)</div>
      <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Close</button>
  </div>
</div>

</asp:Content>


