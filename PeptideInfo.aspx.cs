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
using ZJU.COPDB;
using Faust.Andrew.LiteWiki.DataAccess;
using Faust.Andrew.LiteWiki.TextFormatting;
using ZJU.COPLib;
public partial class PeptideInfo2 : System.Web.UI.Page
{
    string M_T;
    string M_V;

    protected void Page_Load(object sender, EventArgs e)
    {
        string QueryType = Request.QueryString["QType"] ;
        M_T = QueryType;
        string QueryValue = Request.QueryString["QValue"];
        M_V = QueryValue;

        if ((!Page.IsPostBack) && QueryValue != null && QueryType != null)
        {
            Query(QueryType, QueryValue.ToUpper ());
        }
    }
    

    private void Query(string QueryType, string QueryValue)
    {
        switch (QueryType)
        {
            case "Peptide ID":
                QueryPeptideID(QueryValue);

                break;
            case "Amino Acid Sequence":
                QueryPeptideSequence(QueryValue);
                break;
        }
        //((COP)this.Master).QueryType = QueryType;
        //((COP)this.Master).QueryValue = QueryValue;
    }

    private void ShowPeptideInfo(DataSet result)
    {
        if (result == null)
            return;
        if (result.Tables[0].Rows.Count > 0)
        {
            lbPeptide_ID.Text = result.Tables[0].Rows[0].ItemArray[0].ToString();
            lbPepetide_Sequence.Text = string.Format("<SPAN class=pa_sequence_font>{0}</SPAN>", result.Tables[0].Rows[0].ItemArray[0].ToString());
            //if (result.Tables[0].Rows[0].IsNull(2))
            //    lbMolecular_Formula.Text = "";
            //else
            //    lbMolecular_Formula.Text = result.Tables[0].Rows[0].ItemArray[2].ToString();
            lbMolecular_Weight.Text = result.Tables[0].Rows[0].ItemArray[1].ToString();
            //lbDigestion.Text = result.Tables[0].Rows[0].ItemArray[4].ToString();
            //lbEnzyme.Text = result.Tables[0].Rows[0].ItemArray[4].ToString();
            //lbModel.Text = result.GetString(6);
            //if (result.Tables[0].Rows.Count > 1)
            //{
            //    for (int i = 1; i < result.Tables[0].Rows.Count; i++)
            //    {
            //        this.lbOtherPID.Text += string.Format(" | <a href='PeptideInfo.aspx?QType=Peptide ID&QValue={0}'>{1}</a>", result.Tables[0].Rows[i].ItemArray[0].ToString(), result.Tables[0].Rows[i].ItemArray[0].ToString());
            //    }

            //}
        }
    
    }

    private void QueryPeptideSequence(string PepSeq)
    {
        string strSQL = string.Format("select peptide_sequence,molecular_weight from peptide_tbl where Peptide_Sequence = '{0}'", DBInterface.SQLValidString (PepSeq));
        //DBInterface.ConnectDB();
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            //lbSysInfo.Text = "";
            if (result.Tables[0].Rows.Count >0  )
            {
                ShowPeptideInfo(result);
                ShowWiki(lbPeptide_ID.Text.Trim()); 
                QueryPTM(lbPeptide_ID.Text);
                QueryRefProtein(lbPeptide_ID.Text);

            }
            else
            {
                Response.Redirect(string.Format("PeptideList.aspx?Seq={0}&T={1}&V={2}", PepSeq,M_T,M_V ));
               
            } 
            //result.Close();
        }
       
        //DBInterface.CloseDB();
       
    }


    private void QueryPeptideID(string PeptideID)
    {
        string strSQL = string.Format("select peptide_sequence,molecular_weight from peptide_tbl where Peptide_sequence = '{0}'", DBInterface.SQLValidString(PeptideID));
        //DBInterface.ConnectDB();
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            //lbSysInfo.Text = "";
            if (result.Tables[0].Rows.Count > 0)
            {
                ShowPeptideInfo(result);
                ShowWiki(lbPeptide_ID.Text.Trim());
                QueryPTM(PeptideID);
                QueryRefProtein(PeptideID);
            }
            else
            {
                    Response.Redirect(string.Format ("NoResult.aspx?T={0}&V={1}",M_T,M_V));              
            }  
            //result.Close();
        }      
        //DBInterface.CloseDB();
       
    }

    private string AliganSequence(string Sequence)
    {
        int iCount = 20;
        //string strAlignedSequence = "";
        while (iCount < Sequence.Length)
        {            
            Sequence = Sequence.Insert(iCount ,"<br/>") ;
            iCount += 20;
            iCount += 5;
        }
       
        return string.Format ("<SPAN class=pa_sequence_font>{0}</SPAN>",Sequence);
    }

    private void QueryPTM(string PepID)
    {
        string SpectrumURL = "<img src='_image/spectrum.gif'/>";
        string strSQL = string.Format("select ptm_sequence, charge_state, ptm_type,instrumentation,Spectrum_SEQ ,lib_mod,xcorr from  spectrum_tbl b where  peptide_cop_id ='{0}' order by lib_mod", DBInterface.SQLValidString(PepID));
        //DBInterface.ConnectDB();
        string strImage = "<div class=\"spectra-padding\"><a href='SpectrumInfo.aspx?QValue={0}' Target='_blank'><img src='_spectra/{1}/{2}/{3}.gif'></a></div>";
        string strSpectrumInfo = "<table width='100%'><tr bgcolor='#DDDDDD' class='Table_Title'><td width='30%'>Attributes</td><td>Value</td></tr><tr><td align=>Observed Sequence</td><td>{0}</td></tr><tr><td>Charge State</td><td><b>{1}</b></td></tr><tr><td>Instrument</td><td><b>{2}</b></td></tr><tr><td>Library Module</td><td><b>{3}</b></td></tr><tr><td>Xcorr</td><td><b>{4}</b></td></tr></table>";
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        int i = 0;
        if (result != null)
        {

            for (i = 0; i < result.Tables[0].Rows.Count;i++ )
            {
                TableRow trCaption = new TableRow();
                TableCell tcImage = new TableCell();
                string seq = result.Tables[0].Rows[i].ItemArray[4].ToString ();
                string FullGif = string.Format ("{0:000000000}",int.Parse (seq) );
                tcImage.Text = string.Format(strImage,seq, FullGif.Substring (0,3),FullGif.Substring (3,3),FullGif.Substring (6,3));
                trCaption.Cells.Add(tcImage);

                TableCell tcInfo = new TableCell();
                tcInfo.Text = string.Format(strSpectrumInfo, AliganSequence(result.Tables[0].Rows[i].ItemArray[0].ToString()), result.Tables[0].Rows[i].ItemArray[1].ToString(), result.Tables[0].Rows[i].ItemArray[3].ToString(), result.Tables[0].Rows[i].ItemArray[5].ToString(), result.Tables[0].Rows[i].ItemArray[6].ToString());
                tcInfo.VerticalAlign = VerticalAlign.Top;
                tcInfo.Width = new Unit("599");
                trCaption.Cells.Add (tcInfo );

                //TableCell tcModifiedSeq = new TableCell();

                //tcModifiedSeq.Text = ;
                //tcModifiedSeq.HorizontalAlign = HorizontalAlign.Left;

                //trCaption.Cells.Add(tcModifiedSeq);
               
                //TableCell tcModification = new TableCell();
                //string ptm = "";
                //if (!result.Tables[0].Rows[i].IsNull(2))
                //     ptm = result.Tables[0].Rows[i].ItemArray[2].ToString();
                //ptm = ptm=="NULL"?"":ptm;
                //if (ptm.Length > 10)
                //    tcModification.Text = string.Format("<span title='{0}'>{1}...</span>", ptm, ptm.Substring(0, 10));
                //else
                //    tcModification.Text = ptm;
                //tcModification.HorizontalAlign = HorizontalAlign.Center;
                //trCaption.Cells.Add(tcModification);

                //TableCell tcCharge = new TableCell();
                //tcCharge.Text = result.Tables[0].Rows[i].ItemArray[1].ToString();
                //tcCharge.HorizontalAlign = HorizontalAlign.Center;
                //trCaption.Cells.Add(tcCharge);

                //TableCell tcInstrumentation = new TableCell();
                //string instrument = "";
                //if (!result.Tables[0].Rows[i].IsNull(3))
                //   instrument = result.Tables[0].Rows[i].ItemArray[3].ToString();
                //if (instrument.Length > 15)
                //    tcInstrumentation.Text  = string.Format ("<span title='{0}'>{1}...</span>",instrument,instrument.Substring (0,15));
                //else
                //    tcInstrumentation.Text = instrument ;
                //   tcInstrumentation.HorizontalAlign = HorizontalAlign.Center;
                //trCaption.Cells.Add(tcInstrumentation);
                //TableCell tcModel = new TableCell();
                //string libmod = result.Tables[0].Rows[i].ItemArray[5].ToString();
                //if (libmod.Length >10)
                //    tcModel.Text = string.Format("<span title='{0}'>{1}...</span>", libmod , libmod.Substring(0, 10));
                //else
                //    tcModel.Text = libmod ;
                //tcModel.HorizontalAlign = HorizontalAlign.Center;
                //trCaption.Cells.Add(tcModel);
                //TableCell tcSpectra = new TableCell();
                //tcSpectra.Text = string.Format(SpectrumURL, result.Tables[0].Rows[i].ItemArray[4].ToString());
                //tcSpectra.HorizontalAlign = HorizontalAlign.Center;
                //trCaption.Cells.Add(tcSpectra);
                tbModified.Rows.Add(trCaption);
               
            }
            //result.Close();
        }
        // lbPeptideNumber.Text = string.Format("totals:{0:F0}", i);

        //DBInterface.CloseDB();
    }

    private void QueryRefProtein(string PepID)
    {
        string ProCOPURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>";
        string ProIPIURL = "<a href='http://srs.ebi.ac.uk/srsbin/cgi-bin/wgetz?-newId+[IPI-AllText:{0}*]+-lv+30+-view+SeqSimpleView+-page+qResult' Target='_Blank'>{1}</a>";
        string strSQL = string.Format("select b.protein_cop_id,b.ref_protein_id,b.protein_name,b.organism_source,b.chromosome,a.location from pp_relation_tbl a,protein_tbl b where b.protein_cop_id= a.protein_id and peptide_sequence='{0}' order by b.organism_source", DBInterface.SQLValidString(PepID));
        //DBInterface.ConnectDB();
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        int i = 0;
        if (result != null)
        {

            for (i = 0; i < result.Tables[0].Rows.Count;i++)
            {
                TableRow trCaption = new TableRow();
                TableCell tcProCOPID = new TableCell();
                string ProteinID = result.Tables[0].Rows[i].ItemArray[0].ToString();
                tcProCOPID.Text = string.Format(ProCOPURL, ProteinID, ProteinID);
                tcProCOPID.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcProCOPID);

                TableCell tcProName = new TableCell();
                tcProName.Text = result.Tables[0].Rows[i].ItemArray[2].ToString();
                tcProName.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcProName);
                TableCell tcOrganism = new TableCell();
                if (!result.Tables[0].Rows [i]. IsNull(3))
                    tcOrganism.Text = result.Tables[0].Rows[i].ItemArray[3].ToString();
                tcOrganism.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcOrganism);
                TableCell tcGene = new TableCell();
                if (result.Tables[0].Rows[i].IsNull(4))
                    tcGene.Text = "";
                else
                    tcGene.Text = result.Tables[0].Rows[i].ItemArray[4].ToString();
                tcGene.HorizontalAlign = HorizontalAlign.Center;
                trCaption.Cells.Add(tcGene);
                //TableCell tcProIPIID = new TableCell();
                //tcProIPIID.Text = string.Format(ProIPIURL, result.GetString(1), result.GetString(1));
                //tcProIPIID.HorizontalAlign = HorizontalAlign.Left;
                //trCaption.Cells.Add(tcProIPIID);

                TableCell tcLocation = new TableCell();
                tcLocation.Text = result.Tables[0].Rows[i].ItemArray[5].ToString();
                tcLocation.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcLocation);
                tbRefProtein.Rows.Add(trCaption);
                
            }
            //result.Close();
        }
      
        //DBInterface.CloseDB();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("WikiEdit.aspx?PageName=" + lbPeptide_ID.Text.Trim ());
    }

    private void ShowWiki(string copaPepID)
    {
        WikiPage wikiPage = WikiOperations.GetWikiPage(copaPepID.Trim());
        if (wikiPage == null)
        {
            wikiPage = new WikiPage();
            wikiPage.PageName = copaPepID.Trim();
            wikiPage.PageContent = "There is no Wiki page on record for this peptide, please feel free to create one.";
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
}
