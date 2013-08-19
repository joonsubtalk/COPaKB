using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZJU.COPDB;
using System.Data;

public partial class ProteinList : System.Web.UI.Page
{
    string M_T;
    string M_V;
    protected void Page_Load(object sender, EventArgs e)
    {
        string ProteinName = Request.QueryString["Name"];
        string T = Request.QueryString["T"];
        string V = Request.QueryString["V"];
        M_T = T; 
        M_V = V;

       
      
        if ((!Page.IsPostBack) && T!=null )
        {

            proteinKey.Text = V;
            if (ProteinName == null || V.StartsWith("\""))
            {
                QueryGeneSymbol(V);
            }
            else
            {
                if (M_T == "PLIST")
                    QueryLikelyProteinByPepSeq(M_V);
                else
                    QueryLikelyProtein(ProteinName);
            }
               
        } 
    }

    private void QueryGeneSymbol(string GSym)
    {
        string GSymbol = GSym.Substring(1, GSym.Length - 2);
        
        string ProCOPURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>";
        string strSQL = string.Format("select distinct t.protein_cop_id,t.gene_symbol,t.protein_name,t.organism_source from protein_tbl t where upper(t.gene_symbol) like '%{0}%' ", DBInterface.SQLValidString(GSymbol).ToUpper());
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        int i = 0;
        string IPI = "";
        if (result != null)
        {
            if (result.Tables[0].Rows.Count == 0)
            {
                Response.Redirect(string.Format("NoResult.aspx?T={0}&V={1}", M_T, M_V));
                Response.Redirect(string.Format("ProteinInfo.aspx?QType=Protein ID&QValue={0}", result.Tables[0].Rows[0].ItemArray[0].ToString()));
            }
                if (result.Tables[0].Rows.Count == 1)
            {
                Response.Redirect(string.Format("ProteinInfo.aspx?QType=Protein ID&QValue={0}", result.Tables[0].Rows[0].ItemArray[0].ToString()));
            }
            else
            {
                for (i = 0; i < result.Tables[0].Rows.Count && i < 50; i++)
                {
                    TableRow trCaption = new TableRow();
                    TableCell tcProCOPID = new TableCell();
                    IPI = result.Tables[0].Rows[i].ItemArray[0].ToString();
                    tcProCOPID.Text = string.Format(ProCOPURL, IPI, IPI);
                    tcProCOPID.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcProCOPID);

                    TableCell tcProName = new TableCell();
                    string ProteinFullName = result.Tables[0].Rows[i].ItemArray[2].ToString();
                    string ProteinBriefName = "";
                    if (ProteinFullName.Length > 45)
                        ProteinBriefName = ProteinFullName.Substring(0, 45);
                    else
                        ProteinBriefName = ProteinFullName;

                    //int iPos = ProteinBriefName.ToUpper().IndexOf(ProteinName);
                    //if (iPos >= 0)
                    //{
                    //    ProteinBriefName = ProteinBriefName.Insert(iPos + ProteinName.Length, "</font>");
                    //    ProteinBriefName = ProteinBriefName.Insert(iPos, "<font color='red'>");
                    //}
                    tcProName.Text = string.Format("<span title='{0}'>{1}...</span>", ProteinFullName, ProteinBriefName);
                    tcProName.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcProName);
                    TableCell tcOrganism = new TableCell();
                    if (!result.Tables[0].Rows[0].IsNull(3))
                    {
                        string species = result.Tables[0].Rows[i].ItemArray[3].ToString();
                        if (species.Length > 10)
                            tcOrganism.Text = string.Format("<span title='{0}'>{1}...</span>", species, species.Substring(0, 10));
                        else
                            tcOrganism.Text = species;
                    }
                    tcOrganism.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcOrganism);
                    /*
                    TableCell tcGene = new TableCell();
                    if (result.Tables[0].Rows[0].IsNull(4))
                        tcGene.Text = "-";
                    else
                        tcGene.Text = result.Tables[0].Rows[i].ItemArray[4].ToString();
                    tcGene.HorizontalAlign = HorizontalAlign.Center;
                    trCaption.Cells.Add(tcGene);
                     */
                    //TableCell tcProIPIID = new TableCell();
                    //tcProIPIID.Text = string.Format(ProIPIURL, result.GetString(1), result.GetString(1));
                    //tcProIPIID.HorizontalAlign = HorizontalAlign.Left;
                    //trCaption.Cells.Add(tcProIPIID);
                    TableCell tcGeneSymbol = new TableCell();
                    string GeneSymbol = result.Tables[0].Rows[i].ItemArray[1].ToString();
                    string BriefSymbol = "";
                    if (GeneSymbol.Length > 7)
                        BriefSymbol = GeneSymbol.Substring(0, 7) + "...";
                    else
                        BriefSymbol = GeneSymbol;
                    int iPos = BriefSymbol.ToUpper().IndexOf(GSymbol );
                    if (iPos >= 0)
                    {
                        BriefSymbol = BriefSymbol.Insert(iPos + GSymbol.Length, "</font>");
                        BriefSymbol = BriefSymbol.Insert(iPos, "<font color='red'>");
                    }
                    BriefSymbol = BriefSymbol.Replace(";", "<br/>");
                    tcGeneSymbol.Text = string.Format("<span title='{0}'>{1}</span>", GeneSymbol, BriefSymbol);
                    tcGeneSymbol.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcGeneSymbol);
                    tbRefProtein.Rows.Add(trCaption);

                }
            }

            //result.Close(); 
            //DBInterface.CloseDB();

        }
    }

    // Added per Peipei's request to be a fallback function to search thru protein's sequences for peptide seq that failed it's PeptideInfo/list list
    private void QueryLikelyProteinByPepSeq(string pepseq)
    {
        string ProCOPURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>";
        string strSQL = string.Format("select protein_cop_id, gene_symbol, protein_name, organism_source from protein_tbl WHERE PROTEIN_SEQUENCE LIKE '%{0}%'  AND rownum<=25", DBInterface.SQLValidString(pepseq));
        DataSet result = DBInterface.QuerySQL2(strSQL);
        int i = 0;
        string IPI = "";
        if (result != null)
        {
            if (result.Tables[0].Rows.Count == 0) // never will reach, but just in case...
            {
                Response.Redirect(string.Format("NoResult.aspx?T={0}&V={1}", "THIS IS A FAIL", M_V));
            }
            else if (result.Tables[0].Rows.Count == 1) // never will reach, but just in case...
            {
                Response.Redirect(string.Format("ProteinInfo.aspx?QType=Protein ID&QValue={0}", result.Tables[0].Rows[0].ItemArray[0].ToString()));
            }
            else
            {
                for (i = 0; i < result.Tables[0].Rows.Count; i++)
                {
                    TableRow trCaption = new TableRow();
                    TableCell tcProCOPID = new TableCell();
                    IPI = result.Tables[0].Rows[i].ItemArray[0].ToString();
                    tcProCOPID.Text = string.Format(ProCOPURL, IPI, IPI);
                    tcProCOPID.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcProCOPID);

                    TableCell tcProName = new TableCell();
                    string ProteinFullName = result.Tables[0].Rows[i].ItemArray[2].ToString();
                    string ProteinBriefName = "";
                    if (ProteinFullName.Length > 45)
                    {
                        ProteinBriefName = ProteinFullName.Substring(0, 45);
                        int iPos = ProteinBriefName.ToUpper().IndexOf(pepseq);
                        if (iPos >= 0)
                        {
                            ProteinBriefName = ProteinBriefName.Insert(iPos + pepseq.Length, "</font>");
                            ProteinBriefName = ProteinBriefName.Insert(iPos, "<font color='red'>");
                        }
                        tcProName.Text = string.Format("<span title='{0}'>{1}...</span>", ProteinFullName, ProteinBriefName);

                    }
                    else
                    {
                        ProteinBriefName = ProteinFullName;
                        int iPos = ProteinBriefName.ToUpper().IndexOf(pepseq);
                        if (iPos >= 0)
                        {
                            ProteinBriefName = ProteinBriefName.Insert(iPos + pepseq.Length, "</font>");
                            ProteinBriefName = ProteinBriefName.Insert(iPos, "<font color='red'>");
                        }
                        tcProName.Text = ProteinBriefName;
                    }


                    tcProName.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcProName);
                    TableCell tcOrganism = new TableCell();
                    if (!result.Tables[0].Rows[0].IsNull(3))
                    {
                        string species = result.Tables[0].Rows[i].ItemArray[3].ToString();
                        if (species.Length > 10)
                            tcOrganism.Text = string.Format("<span title='{0}'>{1}...</span>", species, species.Substring(0, 10));
                        else
                            tcOrganism.Text = species;
                    }
                    tcOrganism.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcOrganism);
                    TableCell tcGeneSymbol = new TableCell();
                    string GeneSymbol = result.Tables[0].Rows[i].ItemArray[1].ToString();
                    string BriefSymbol = "";
                    if (GeneSymbol.Length > 7)
                        BriefSymbol = GeneSymbol.Substring(0, 7) + "...";
                    else
                        BriefSymbol = GeneSymbol;
                    int iPos2 = BriefSymbol.ToUpper().IndexOf(pepseq);
                    if (iPos2 >= 0)
                    {
                        BriefSymbol = BriefSymbol.Insert(iPos2 + pepseq.Length, "</font>");
                        BriefSymbol = BriefSymbol.Insert(iPos2, "<font color='red'>");
                    }
                    BriefSymbol = BriefSymbol.Replace(";", "<br/>");
                    tcGeneSymbol.Text = string.Format("<span title='{0}'>{1}</span>", GeneSymbol, BriefSymbol);
                    tcGeneSymbol.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcGeneSymbol);
                    tbRefProtein.Rows.Add(trCaption);

                }
            }
        }
    }

    private void QueryLikelyProtein(string ProteinName)
    {
        string ProCOPURL = "<a href='ProteinInfo.aspx?QType=Protein ID&QValue={0}'>{1}</a>";
        //string ProIPIURL = "<a href='http://srs.ebi.ac.uk/srsbin/cgi-bin/wgetz?-newId+[IPI-AllText:{0}*]+-lv+30+-view+SeqSimpleView+-page+qResult' Target='_Blank'>{1}</a>";
        //string strSQL = string.Format("select distinct t.protein_cop_id,t.gene_symbol,t.protein_name,t.organism_source,t.chromosome from protein_tbl t,protein_info t2 where ( t.ref_protein_id = t2.pid and (upper(t2.synonym_name) like '%{0}%' or upper(t2.IPI_history) like '%{0}%') or upper(pdb) like '%{0}%') or (  upper(t.protein_name) like '%{0}%' or upper(t.gene_symbol) like '%{0}%' or upper(t.REF_KB_ID) like '%{0}%' )", DBInterface.SQLValidString(ProteinName));
        string strSQL = string.Format("select distinct t.protein_cop_id,t.gene_symbol,t.protein_name,t.organism_source from protein_tbl t where upper(t.protein_cop_id) like '%{0}%' or upper(t.ref_protein_id) like '%{0}%' or upper(t.protein_name) like '%{0}%' or upper(t.gene_symbol) like '%{0}%' or upper(t.REF_KB_ID) like '%{0}%' ", DBInterface.SQLValidString(ProteinName));
        //DBInterface.ConnectDB();
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        int i = 0;
        string IPI = "";
        if (result != null )
        {
            if (result.Tables[0].Rows.Count == 0)
            {
                Response.Redirect(string.Format("NoResult.aspx?T={0}&V={1}", M_T, M_V));
            }
            else if (result.Tables[0].Rows.Count == 1)
            {
                Response.Redirect(string.Format("ProteinInfo.aspx?QType=Protein ID&QValue={0}", result.Tables[0].Rows[0].ItemArray[0].ToString()));
            }
            else
            {
                for (i = 0; i < result.Tables[0].Rows.Count; i++)
                {
                    TableRow trCaption = new TableRow();
                    TableCell tcProCOPID = new TableCell();
                    IPI = result.Tables[0].Rows[i].ItemArray[0].ToString();
                    tcProCOPID.Text = string.Format(ProCOPURL, IPI, IPI);
                    tcProCOPID.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcProCOPID);

                    TableCell tcProName = new TableCell();
                    string ProteinFullName =  result.Tables[0].Rows[i].ItemArray[2].ToString ();
                    string ProteinBriefName = "";
                    if (ProteinFullName.Length > 45)
                    {
                        ProteinBriefName = ProteinFullName.Substring(0, 45);
                        int iPos = ProteinBriefName.ToUpper().IndexOf(ProteinName);
                        if (iPos >= 0)
                        {
                            ProteinBriefName = ProteinBriefName.Insert(iPos + ProteinName.Length, "</font>");
                            ProteinBriefName = ProteinBriefName.Insert(iPos, "<font color='red'>");
                        }   
                        tcProName.Text = string.Format ("<span title='{0}'>{1}...</span>",ProteinFullName ,ProteinBriefName );
             
                    }
                    else
                    {
                        ProteinBriefName = ProteinFullName;
                        int iPos = ProteinBriefName.ToUpper().IndexOf(ProteinName);
                        if (iPos >= 0)
                        {
                            ProteinBriefName = ProteinBriefName.Insert(iPos + ProteinName.Length, "</font>");
                            ProteinBriefName = ProteinBriefName.Insert(iPos, "<font color='red'>");
                        }
                        tcProName.Text = ProteinBriefName;
                    }
                    
                  
                    tcProName.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcProName);
                    TableCell tcOrganism = new TableCell();
                    if (!result.Tables[0].Rows[0].IsNull(3))
                    {
                        string species = result.Tables[0].Rows[i].ItemArray[3].ToString();
                        if (species.Length > 10)
                            tcOrganism.Text = string.Format("<span title='{0}'>{1}...</span>", species, species.Substring(0, 10));
                        else
                            tcOrganism.Text = species;
                    }
                    tcOrganism.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcOrganism);
                    /*
                    TableCell tcGene = new TableCell();
                    if (result.Tables[0].Rows[0].IsNull(4))
                        tcGene.Text = "-";
                    else
                        tcGene.Text = result.Tables[0].Rows[i].ItemArray[4].ToString();
                    tcGene.HorizontalAlign = HorizontalAlign.Center ;
                    trCaption.Cells.Add(tcGene);
                     */
                    //TableCell tcProIPIID = new TableCell();
                    //tcProIPIID.Text = string.Format(ProIPIURL, result.GetString(1), result.GetString(1));
                    //tcProIPIID.HorizontalAlign = HorizontalAlign.Left;
                    //trCaption.Cells.Add(tcProIPIID);
                    TableCell tcGeneSymbol = new TableCell();
                    string GeneSymbol = result.Tables[0].Rows[i].ItemArray[1].ToString();
                    string BriefSymbol = "";
                    if (GeneSymbol.Length > 7)
                        BriefSymbol = GeneSymbol.Substring(0, 7) + "...";
                    else
                        BriefSymbol = GeneSymbol;
                    int iPos2 = BriefSymbol.ToUpper().IndexOf(ProteinName);
                    if (iPos2 >= 0)
                    {
                        BriefSymbol = BriefSymbol.Insert(iPos2 + ProteinName.Length, "</font>");
                        BriefSymbol = BriefSymbol.Insert(iPos2, "<font color='red'>");
                    }
                    BriefSymbol = BriefSymbol.Replace(";", "<br/>");
                    tcGeneSymbol.Text = string.Format("<span title='{0}'>{1}</span>", GeneSymbol, BriefSymbol);
                    tcGeneSymbol.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcGeneSymbol);
                    tbRefProtein.Rows.Add(trCaption);

                }
            }

            //result.Close(); 
            //DBInterface.CloseDB();
                  

        }
       

       
    }
}
