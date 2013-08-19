<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Release History" MasterPageFile="~/COPaKB.master" Language="C#" AutoEventWireup="true" CodeFile="ReleaseHistory.aspx.cs" Inherits="ReleaseHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="row-fluid">
    <div class="span12">
        <h2>Release Schedule</h2>
        <p>COPaKB has a standard protocol for updating and maintaining the database resources by a specific set of software that we have already developed. Our current update schedule is listed below.</p>
        <ul>
            <li>
                The new entries in the Wiki component of COPaKB are being reviewed and processed on a weekly basis. Two individuals at the UCLA site, one individual at TSRI site, and one individual at the EBI site have committed to this task on a regular schedule.
                <br>- Update log is available <a href="http://www.heartproteome.org/copa/COPaWikiDefault.aspx?PageName=COPaKB%20Wiki%20Review%20Log">here</a>.
            </li>
            <li>
                The mass spectral dataset of COPaKB is being reprocessed semi-annually in the months of May and November, which accommodates the addition of new and updated mass spectral datasets contributed by investigators via COPaKB portal. The update of protein sequence dataset is synchronized semi-annually with the corresponding new release of UniProt database.
                <br>- Last update was on May 31<sup>st</sup>, 2013.
            </li>
            <li>
                The update of immunohistochemistry and immunofluorescence images is  being synchronized annually with every release of Human Protein Atlas database. 
                <br>- Last update was on March 31<sup>st</sup>, 2013.
            </li>
            <li>
                Components of computational toolbox are being tested and released semi-annually.
                <br>- Last update was on May 31<sup>st</sup>, 2013.
            </li>
            <li>
                The server computer hosting COPaKB is being upgraded to adequately support the requirement of dataset storage as well as the increased demand of query requests.
                <br>- Last update was on January 31<sup>st</sup>, 2013.
            </li>
        </ul>
    </div>
</div>
<div class="row-fluid">
    <div class="span12">
        <h2>Release History</h2>
        <h4>May 31<sup>st</sup>, 2013: Official Implementation of COPa Knowledgebase Version 1.1</h4>
        <ul>
            <li>Six new modules were added to COPaKB, including human heart total lysate, mouse heart nuclei, mouse heart cytosol, mouse heart total lysate, drosophila mitochondria and <em>C. elegans</em> mitochondria.</li>
            <li>Mass spectrometry dataset now has 329,690 curated mass spectra. The training datasets were processed by the bioinformatics software developed by the TSRI team (e.g., ProLuCID and DTASelect).</li>
            <li>Protein expression imaging component of the COPaKB has been updated with immunohistochemistry images of the Human Protein Atlas version 11.0.</li>
            <li>Java-coded COPaKB Client (version 2) was released. It can now be used in Windows, Mac OS X and Linux operating systems.</li>
            <li>iCOPa (version 2.0) was released via iTunes. The six new modules in COPaKB (version 2) were included. It also provided a keyword search function.</li>
            <li>COPaKB website has been updated from .NET 3.5 framework to .NET 4.0 framework.</li>
            <li>The workstation hosting COPaKB web server was upgraded to accommodate increasing demands of data storage and query requests.</li>
        </ul>


        <h4>November 30<sup>th</sup>, 2012: Update on the Web Portals and the Computational Toolbox</h4>
        <ul>
            <li>The Wiki component of COPaKB was updated to enhance its Internet security.</li>
            <li>iCOPa (version 1.0) was released via iTunes. This was the first bioinformatics tool of proteome biology that functions on mobile devices.</li>
        </ul>
        <h4>May 31<sup>st</sup>, 2012: Establishement of COPaKB Distributed Annotation System (DAS) Services</h4>
        <ul>
            <li>DAS service program was set up on the servers of COPaKB and Human Protein Atlas, which exchanges data with the DAS server at EBI.</li>
            <li>Protein expression imaging component of COPaKB  was synchronized  with Human Protein Atlas version 9.0.</li>
            <li>Annotations on genetic phenotypes and transcriptional regulation were updated using DAS services.</li>
        </ul>
        <h4>November 30<sup>th</sup>, 2011: Integration of Data on Genetic Phenotypes and Transcriptional Regulation </h4>
        <ul>
            <li>Phenotypes associated with genetic mutations were curated using Online Mendelian Inheritance in Man (OMIM). Results were incorporated into COPaKB.</li>
            <li>Transcriptional regulation of proteins was curated using Gene Expression Atlas. Results were incorporated into COPaKB.</li>
        </ul>
        <h4>May 12<sup>th</sup>, 2011: Beta Implementation of COPaKB Version 1.0</h4>
        <ul>
            <li>Four modules were created, which include proteome knowledge on human heart mitochondria, human heart proteasome, mouse heart mitochondria and mouse heart proteasome.</li>
            <li>Mass spectrometry component had 98,170 curated spectra. The training datasets were processed by the bioinformatics software (SEQUEST and Scaffold). </li>
            <li>Protein expression imaging component was synchronized with Human Protein Atlas version 8.0.</li>
            <li>Biomedical attributes of human proteins involved in cardiovascular diseases were analyzed using literature search via PubMed. A total of 413 publications were cited for this analysis.</li>
            <li>COPaKB Client (version 1) was released for free download. It was developed in C# language and can be used in the Windows operating system.</li>
        </ul>
        
    </div>
</div>
</asp:Content>