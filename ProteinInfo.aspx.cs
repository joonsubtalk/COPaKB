using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using ZJU.COPLib;
using System.Collections;
using ZJU.COPDB;

using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using gov.nih.nlm.ncbi.eutils;
using System.Net;

public partial class ProteinInfo2 : System.Web.UI.Page
{
    string M_T;
    string M_V;
    string NoInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        string QueryType = Request.QueryString["QType"];
        M_T = QueryType;
        string QueryValue = Request.QueryString["QValue"];
        M_V = QueryValue;
        string queryShow = Request.QueryString["showInfo"];
        NoInfo = queryShow;

        //find correct intetactome
        displayInteractome(M_V);

        if (NoInfo == "T"){ // collapse
            informationShow.Text = @"<div id='helpNote' class='collapse'>";
            informationBtn.Text = @"<button type='button' class='close button collapsed' data-toggle='collapse' data-target='#helpNote'>&times;</button>";
        }
        else{
            informationShow.Text = @"<div id='helpNote' class='collapse in'>";
            informationBtn.Text = @"<button type='button' class='close button' data-toggle='collapse' data-target='#helpNote'>&times;</button>";
        }

        if ((!Page.IsPostBack) && QueryType != null && QueryValue != null)
        {
            Query(QueryType, QueryValue);
        }

        //lbEdit.Text = string.Format("<a href='DiseaseInfo.aspx?uniprot={0}' target='_blank'>More Information</a>  ", lbProtein_ID.Text.Trim());
        lbMoreInformation.Text = string.Format("<a href='WikiEdit.aspx?PageName={0}' target='_blank'>Edit</a>  ", lbProtein_ID.Text);
        AdditionalReport.Text = string.Format("<h5>iHOP <a href='http://www.ihop-net.org/UniPub/iHOP/?search={0}&field=UNIPROT__AC&ncbi_tax_id=0&organism_syn=' target='_blank'>{0}</a> </h5>", QueryValue);

    }

    private void displayInteractome(string M_V)
    {
        string webtext = "";
        WebRequest request;
        WebResponse response;
        System.IO.StreamReader reader;
        string[] files = new string[7] {"drosophila_mitochondria", 
                                        "human_mitochondria", 
                                        "human_proteosome",
                                        "mouse_cytosol",
                                        "mouse_mitochondria",
                                        "mouse_nucleus",
                                        "mouse_proteosome"};

        for(int i = 0; i < 7; i++){
            request = WebRequest.Create(string.Format("http://www.heartproteome.org/copa/js/copa_imex_interactome_{0}.js",files[i]));
            response = request.GetResponse();
            reader = new System.IO.StreamReader(response.GetResponseStream());
            webtext = reader.ReadToEnd();

            if (System.Text.RegularExpressions.Regex.IsMatch(webtext, M_V.ToUpper()))
            {
                if (i == 2)
                    interactomeDisplay.Text += string.Format("<a href='interactome_{0}.aspx?iid={1}'>View {0}</a><br />", "human_proteasome", M_V.ToUpper());
                else if (i == 6)
                    interactomeDisplay.Text += string.Format("<a href='interactome_{0}.aspx?iid={1}'>View {0}</a><br />", "mouse_proteasome", M_V.ToUpper());
                else
                    interactomeDisplay.Text += string.Format("<a href='interactome_{0}.aspx?iid={1}'>View {0}</a><br />", files[i], M_V.ToUpper());
            }
        }
    }

    private void Query(string QueryType, string QueryValue)
    {
        switch (QueryType)
        {
            case "Protein ID":
                QueryProteinID(QueryValue);

                break;
        }
    }

    private void QueryProteinID(string ProteinID)
    {
        ProteinID = ProteinID.ToUpper();
        if (!ProteinID.StartsWith("IPI"))
        {
            int pno;
            try
            {
                pno = int.Parse(ProteinID);
                ProteinID = string.Format("IPI{0:D8}", pno);
            }
            catch (Exception ex) { }
        }
        string strSQL = string.Format("select Protein_COP_ID,Ref_Protein_ID,Protein_name,Organism_Source,Protein_Sequence,sequence_length,Chromosome,Molecular_Weight,Isoelectric_Point,GRAVY,Transmembrane_Domain ,Gene_Symbol,REF_KB_ID,ensemblegeneid from protein_tbl where Protein_COP_ID = '{0}'", DBInterface.SQLValidString(ProteinID));
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            if (result.Tables[0].Rows.Count > 0)
            {
                ShowProteinInfo(result);
            }
            else
            {
                Response.Redirect(string.Format("ProteinList.aspx?Name={0}&T={1}&V={2}", ProteinID, M_T, M_V));
            }
        }
        QueryPeptides(lbProtein_ID.Text);
        ShowWiki(lbProtein_ID.Text.Trim()); 
    }

    private void ShowProteinInfo(DataSet result)
    {
        if (result == null)
            return;
        if (result.Tables[0].Rows.Count > 0)
        {
            // ID
            lbProtein_ID.Text = result.Tables[0].Rows[0].ItemArray[0].ToString();
            // Description
            lbProtein_Name.Text = result.Tables[0].Rows[0].ItemArray[2].ToString();
            // Gene Symbol
            if (result.Tables[0].Rows[0].IsNull(11))
                lbGeneSymbol.Text = "";
            else
                lbGeneSymbol.Text = result.Tables[0].Rows[0].ItemArray[11].ToString();
            // Uniprot w/ url
            lbUniprot.Text = lbProtein_ID.Text;
            // Species
            lbOrganism_Source.Text = result.Tables[0].Rows[0].ItemArray[3].ToString();
            // Seq Length
            lbSequence_Length.Text = result.Tables[0].Rows[0].ItemArray[5].ToString();
            // Seq
            lbSequence.Text = AlignSequence(result.Tables[0].Rows[0].ItemArray[4].ToString());

            if (result.Tables[0].Rows[0].IsNull(7))
                lbMolecuar_Weight.Text = "";
            else
                lbMolecuar_Weight.Text = result.Tables[0].Rows[0].ItemArray[7].ToString();

            if (result.Tables[0].Rows[0].IsNull(12))
                lbRefKB.Text = "";
            else
                lbRefKB.Text = GetKBURL(result.Tables[0].Rows[0].ItemArray[12].ToString());
            string HumanAtlas = "";

            if (result.Tables[0].Rows[0].IsNull(13))
            {
                string[] genes = lbGeneSymbol.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string gene in genes)
                {
                    HumanAtlas += string.Format("<a href='http://www.proteinatlas.org/search/{0}' target='_blank'>{0}</a> ", gene);
                }

                HPAImg.Visible = false;
                IHCImg.Visible = false;
            }
            else
            {
                string imageID = "";
                string imageID2 = "";
                string MainSubcelluar = "";
                string AdditionalSubcelluar = "";
                string IHC_summary = "";
                string IHC_heart_expression = "";
                string HPAENSGID = GetImageURL(result.Tables[0].Rows[0].ItemArray[13].ToString(), ref imageID, ref imageID2, ref MainSubcelluar, ref AdditionalSubcelluar, ref IHC_summary, ref IHC_heart_expression); //GetHPAENSGID(result.Tables[0].Rows[0].ItemArray[13].ToString(), ref imageID);


                if (HPAENSGID == "")
                {
                    HumanAtlas = string.Format("<a href='http://www.proteinatlas.org/{0}' target='_blank'>{0}</a> ", result.Tables[0].Rows[0].ItemArray[13].ToString());

                    HPAImg.Visible = false;
                    IHCImg.Visible = false;
                }
                else
                {
                    HumanAtlas = string.Format("<a href='http://www.proteinatlas.org/{0}' target='_blank'>{0}</a> ", HPAENSGID);
                    lb_main_subcelluar.Text = MainSubcelluar;
                    lb_additional_subcellular.Text = AdditionalSubcelluar;
                    lb_IHC_summary.Text = IHC_summary;

                    HPAImg.Visible = true;
                    IHCImg.Visible = true;

                    if (imageID != "_image/not_available.jpg")
                        imageID = string.Format("http://{0}", imageID);
                    HPAImg.ImageUrl = imageID;
                    IHCImg.ImageUrl = imageID2;
                    lb_IHC_heart_expression.Text = string.Format("<a href='http://www.proteinatlas.org/{0}/normal/heart+muscle' target='_blank'>{1}</a>", HPAENSGID, IHC_heart_expression);
                    if (imageID != "_image/not_available.jpg")
                    {
                        HPAImg.Attributes.Add("src", imageID.Replace("if_selected_medium", "if_selected"));
                        HPAImg.Attributes.Add("class", "thumb");
                        HPAImg.Attributes.Add("width", "200px");
                        HPAImg.Attributes.Add("height", "200px");
                        linkHPAImgStart.Text = @"<a href='" + imageID.Replace("if_selected_medium", "if_selected") + "'>";
                        linkHPAImgClose.Text = @"</a>";
                    }
                    if (imageID2 != "_image/not_available.jpg")
                    {
                        IHCImg.Attributes.Add("src", imageID2);
                        IHCImg.Attributes.Add("class", "thumb1");
                        IHCImg.Attributes.Add("width", "200px");
                        IHCImg.Attributes.Add("height", "200px");
                        linkIHCImgStart.Text = @"<a href='" + imageID2 + "'>";
                        linkIHCImgClose.Text = @"</a>";
                    }
                }
            }
            lbHumanProtAtlas.Text = HumanAtlas;

            ShowBiology(lbGeneSymbol.Text);
        }
    }

    private string GetImageURL(string ProteinID, ref string ImageID, ref string ImageID2, ref string mainsubcelluar, ref string additionsubcelluar, ref string IHCSummary, ref string IHCHeartExpression)
    {
        string strSQL = string.Format("select ensg_id,subcellular_image,main_subcellular,additional_subcellular, IHC_Summary,IHC_Heart_expression,IHC_IMAGE from hpa_crossref_tbl  where ensg_id= '{0}'", DBInterface.SQLValidString(ProteinID));
        DataSet result = DBInterface.QuerySQL2(strSQL);

        if (result != null)
        {
            if (result.Tables[0].Rows.Count > 0)
            {
                if (result.Tables[0].Rows[0].IsNull(0) || result.Tables[0].Rows[0].ItemArray[0].ToString() == "#N/A")
                    return "";
                else
                {
                    if (!result.Tables[0].Rows[0].IsNull(1))
                        ImageID = result.Tables[0].Rows[0].ItemArray[1].ToString();
                    else
                        ImageID = "_image/not_available.jpg";
                    if (!result.Tables[0].Rows[0].IsNull(6))
                        ImageID2 = result.Tables[0].Rows[0].ItemArray[6].ToString();
                    else
                        ImageID2 = "_image/not_available.jpg";
                    if (!result.Tables[0].Rows[0].IsNull(2))
                        mainsubcelluar = result.Tables[0].Rows[0].ItemArray[2].ToString();
                    else
                        mainsubcelluar = "N/A";
                    if (!result.Tables[0].Rows[0].IsNull(3))
                        additionsubcelluar = result.Tables[0].Rows[0].ItemArray[3].ToString();
                    else
                        additionsubcelluar = "N/A";
                    IHCSummary = result.Tables[0].Rows[0].ItemArray[4].ToString();
                    IHCHeartExpression = result.Tables[0].Rows[0].ItemArray[5].ToString();
                    return result.Tables[0].Rows[0].ItemArray[0].ToString();
                }
            }
        }
        return "";
    }

    private void QueryPeptides(string ProteinID)
    {
        string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
        string strSQL = string.Format("select a.peptide_sequence, a.peptide_sequence,b.prevaa,b.nextaa,a.Molecular_weight,b.location,a.Enzyme_Specificity from peptide_tbl a, pp_relation_tbl b where a.peptide_sequence = b.peptide_sequence and b.location<> -1 and b.protein_id='{0}' order by b.location", DBInterface.SQLValidString(ProteinID));
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {

            for (int j = 0; j < result.Tables[0].Rows.Count; j++)
            {
                TableRow trCaption = new TableRow();
                TableCell tcPepID = new TableCell();
                string PeptideID = result.Tables[0].Rows[j].ItemArray[0].ToString();
                tcPepID.Text = string.Format(PepIDURL, PeptideID, PeptideID);
                tcPepID.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcPepID);
                TableCell tcPreAA = new TableCell();
                tcPreAA.Text = result.Tables[0].Rows[j].ItemArray[2].ToString();
                tcPreAA.HorizontalAlign = HorizontalAlign.Center;
                trCaption.Cells.Add(tcPreAA);
                //TableCell tcSequence = new TableCell();
                //tcSequence.Text = result.Tables[0].Rows[j].ItemArray[1].ToString();
                //tcSequence.HorizontalAlign = HorizontalAlign.Left;
                //trCaption.Cells.Add(tcSequence);
                TableCell tcNextAA = new TableCell();
                tcNextAA.Text = result.Tables[0].Rows[j].ItemArray[3].ToString();
                tcNextAA.HorizontalAlign = HorizontalAlign.Center;
                trCaption.Cells.Add(tcNextAA);
                TableCell tcMW = new TableCell();
                tcMW.Text = result.Tables[0].Rows[j].ItemArray[4].ToString();
                tcMW.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcMW);
                TableCell tcFully = new TableCell();
                tcFully.Text = result.Tables[0].Rows[j].ItemArray[5].ToString();
                tcFully.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcFully);
                TableCell tcEnzyme = new TableCell();
                tcEnzyme.Text = result.Tables[0].Rows[j].ItemArray[6].ToString();
                tcEnzyme.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcEnzyme);
                tbPeptides.Rows.Add(trCaption);
            }

            lbDistinct_Peptides.Text = result.Tables[0].Rows.Count.ToString();
            imgPeptides.ImageUrl = String.Format("ProSeqImage.aspx?PID={0}&L={1}&NO={2:F0}", ProteinID, lbSequence_Length.Text, result.Tables[0].Rows.Count);
            lbPeptideNumber.Text = string.Format("Totals: {0:F0}", result.Tables[0].Rows.Count);
            try
            {
                PeptidesImageInfo PIF = new PeptidesImageInfo(ProteinID, int.Parse(lbSequence_Length.Text), result.Tables[0].Rows.Count);
                int totalHeight = 0;
                ArrayList RectArray = PIF.ComputeTheValues(ref totalHeight);
                int j = 0;
                for (j = 0; j < result.Tables[0].Rows.Count; j++)
                {
                    PeptidesImageInfo.PepRectInfo rect = (PeptidesImageInfo.PepRectInfo)RectArray[j];
                    RectangleHotSpot recthot = new RectangleHotSpot();
                    recthot.Left = rect.left;
                    recthot.Right = rect.right;
                    recthot.Top = rect.top;
                    recthot.Bottom = rect.bottom;
                    recthot.NavigateUrl = string.Format("PeptideInfo.aspx?QType=Peptide+ID&QValue={0}", rect.PepID);
                    recthot.AlternateText = string.Format("{0}-{1}, {2}", rect.Location, rect.Location + rect.Length, rect.Sequence);
                    imgPeptides.HotSpots.Add(recthot);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    //KBIDs will look like this: IPI:IPI00471246.2|SWISS-PROT:Q9JHI5|TREMBL:A3KGG9|ENSEMBL:ENSMUSP00000028807|REFSEQ:NP_062800|VEGA:OTTMUSP00000015718 
    private string GetKBURL(string KBIDs)
    {

        string[] tokens = KBIDs.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

        string URL = "";
        string KBRef = "{0}";
        string KBRecod = "<p>{0} {1}</p>";
        foreach (string kbid in tokens)
        {
            string kb = kbid.Substring(0, kbid.IndexOf(";")).Trim().ToUpper();
            string ids = kbid.Substring(kbid.IndexOf(";") + 1);
            if (ids.EndsWith("."))
                ids = ids.Substring(0, ids.Length - 1);

            string[] idtokens = ids.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            string IDString = "";
            switch (kb)
            {
                case "IPI":
                    //URL += "IPI:";
                    /*
                    foreach (string id in idtokens)
                    {
                        if (id.Trim ()!="-")
                        IDString += string.Format("<a href='http://srs.ebi.ac.uk/srsbin/cgi-bin/wgetz?-newId+[IPI-AllText:{0}*]+-lv+30+-view+SeqSimpleView+-page+qResult' target='_blank'>{0}</a>  ", id.Trim ());
                    }
                    URL += string.Format(KBRecod, kb, IDString);
                     */
                    break;

                case "SWISS-PROT":
                    //URL += "SWISS-PROT:";

                    foreach (string id in idtokens)
                    {
                        if (id.Trim() != "-")
                            IDString += string.Format("<a href='http://www.uniprot.org/uniprot/{0}' target='_blank'>{0}</a>  ", id.Trim());
                    }
                    URL += string.Format(KBRecod, kb, IDString);
                    break;

                case "TREMBL":
                    //URL += "TREMBL:";

                    foreach (string id in idtokens)
                    {
                        if (id.Trim() != "-")
                            IDString += string.Format("<a href='http://www.uniprot.org/uniprot/{0}' target='_blank'>{0}</a>  ", id.Trim());
                    }
                    URL += string.Format(KBRecod, kb, IDString);
                    break;
                case "ENSEMBL":
                    //URL += "ENSEMBL:";
                    /*
                     foreach (string id in idtokens)
                     {
                         if (id.Trim() != "-")
                         IDString += string.Format("<a href='http://www.ensembl.org/id/{0}' target='_blank'>{0}</a>  ", id.Trim ());
                     }
                     URL += string.Format(KBRecod, kb, IDString);
                     * */
                    break;
                case "REFSEQ":
                    //URL += "REFSEQ:";

                    foreach (string id in idtokens)
                    {
                        if (id.Trim() != "-")
                            IDString += string.Format("<a href='http://www.ncbi.nlm.nih.gov/protein/{0}' target='_blank'>{0}</a>  ", id.Trim());
                    }
                    URL += string.Format(KBRecod, kb, IDString);
                    break;
                case "VEGA":
                    //URL += "VEGA:";

                    foreach (string id in idtokens)
                    {
                        if (id.Trim() != "-")
                            IDString += string.Format("<a href='http://vega.sanger.ac.uk/Multi/Search/Results?species=all;idx=;q={0}' target='_blank'>{0}</a>  ", id.Trim());
                    }
                    URL += string.Format(KBRecod, kb, IDString);
                    break;
                case "PDB":
                    IDString += string.Format("<a href='http://www.rcsb.org/pdb/explore.do?structureId={0}' target='_blank'>{1}</a>  ", idtokens[0].Trim(), ids);
                    URL += string.Format(KBRecod, kb, IDString);
                    break;
                case "INTERPRO":
                    if (idtokens.Length == 2)
                    {
                        IDString += string.Format("<a href='http://www.ebi.ac.uk/interpro/ISearch?query={0}' target='_blank'>{1}</a>  ", idtokens[0].Trim(), idtokens[1].Trim());
                        URL += string.Format(KBRecod, kb, IDString);
                    }
                    break;


            }
        }
        return string.Format(KBRef, URL);
    }
    private string AlignSequence(string strSequence)
    {
        int iCount = 10;
        string strAlignedSequence = "";
        while (iCount < strSequence.Length)
        {
            strAlignedSequence += strSequence.Substring(iCount - 10, 10) + "  ";
            iCount += 10;
        }
        strAlignedSequence += strSequence.Substring(iCount - 10);
        return strAlignedSequence;
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("DiseaseInfo.aspx?uniprot={0}", lbProtein_ID.Text));
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("WikiEdit.aspx?PageName=" + lbProtein_ID.Text.Trim());
    }



    private void ShowWiki(string copaPID)
    {
        WikiPage wikiPage = WikiOperations.GetWikiPage(copaPID.Trim());
        if (wikiPage == null)
        {
            wikiPage = new WikiPage();
            wikiPage.PageName = copaPID.Trim();
            wikiPage.PageContent = "There is no Wiki page on record for this protein, please feel free to create one.";
            wikiPage.ModifiedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";
            wikiPage.LastModified = DateTime.Now;
            wikiPage.Created = DateTime.Now;
            wikiPage.IsPrivate = User.Identity.IsAuthenticated ? true : false;
            wikiPage.AllowAnonEdit = User.Identity.IsAuthenticated ? false : true;
        }
        SetPageContent(wikiPage);
    }

    private void SetPageContent(WikiPage wikiPage)
    {
        WikiTextFormatter wf = new WikiTextFormatter();
        litContent.Text = wf.FormatPageForDisplay(wikiPage.PageContent);
        if (DateTime.Now != wikiPage.Created)
            lastMod.Text = wf.FormatPageForDisplay("<div class=\"rowfluid\"><div class=\"span12\">This wiki was last modified by " + wikiPage.ModifiedBy + " on " + wikiPage.LastModified + "</div></div>");
    }

    private void ShowBiology(string gene_symbol)
    {
        string strPubmed = "<table width='100%'><tr><td><a class='pubtitle' href='http://www.ncbi.nlm.nih.gov/pubmed/{0}' target='_blank'>{1}</a></td></tr><tr><td>{2}</td></tr></table>";
        string strSQL = string.Format("select heartdisease, perturbation,description,pubmed_id,pubmed_title,pubmed_author from disease_tbl t where gene_symbol='{0}'", gene_symbol);
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            if (result.Tables[0].Rows.Count == 0)
            {
                TableRow trNoResult = new TableRow();
                TableCell tcNoResult = new TableCell();
                tcNoResult.Text = "";
                trNoResult.Cells.Add(tcNoResult);
                tbDisease.Rows.Clear();
                //tbDisease.Rows.Add(trNoResult);
                tbDisease.Visible = false;

            }
            else
            {
                for (int j = 0; j < result.Tables[0].Rows.Count; j++)
                {
                    TableRow trDisease = new TableRow();

                    TableCell tcPerturbation = new TableCell();
                    tcPerturbation.Text = result.Tables[0].Rows[j].ItemArray[1].ToString();
                    trDisease.Cells.Add(tcPerturbation);
                    TableCell tcDescription = new TableCell();
                    tcDescription.Text = result.Tables[0].Rows[j].ItemArray[2].ToString();
                    trDisease.Cells.Add(tcDescription);
                    TableCell tcHeartDiseasae = new TableCell();
                    tcHeartDiseasae.Text = result.Tables[0].Rows[j].ItemArray[0].ToString();
                    trDisease.Cells.Add(tcHeartDiseasae);
                    tbDisease.Rows.Add(trDisease);
                    TableRow trPubmed = new TableRow();
                    TableCell tcPubmed = new TableCell();
                    tcPubmed.Text = string.Format(strPubmed, result.Tables[0].Rows[j].ItemArray[3].ToString(), result.Tables[0].Rows[j].ItemArray[4].ToString(), result.Tables[0].Rows[j].ItemArray[5].ToString());
                    tcPubmed.ColumnSpan = 3;
                    trPubmed.Cells.Add(tcPubmed);
                    tbDisease.Rows.Add(trPubmed);
                }
            }
        }
    }
}
