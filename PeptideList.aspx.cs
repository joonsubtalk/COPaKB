using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZJU.COPDB;
using System.Data;

public partial class PeptideList : System.Web.UI.Page
{
    string M_T;
    string M_V;

    protected void Page_Load(object sender, EventArgs e)
    {
        string PepSeq = Request.QueryString["Seq"];

        string T = Request.QueryString["T"];
        string V = Request.QueryString["V"];

        M_T = T;
        M_V = V;

        if ((!Page.IsPostBack) && PepSeq != null)
        { 
            /*DropDownList ddlQueryType = (DropDownList)(this.Master.FindControl("ddlQueryType"));
            ddlQueryType.Text = T;
            TextBox tbKeyWords = (TextBox)(this.Master.FindControl("tbKeyWords"));
            tbKeyWords.Text = V;
            */
            peptideKey.Text = V;
            QueryLikelyPeptide(PepSeq);
        }     
    }

    private void QueryLikelyPeptide(string PepSeq)
    {
        string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
        string strSQL = string.Format("select peptide_sequence,Molecular_weight,Sequence_length,Enzyme_Specificity from peptide_tbl  where peptide_sequence like '%{0}%'",DBInterface.SQLValidString( PepSeq.ToUpper ()));
        string briefSeq;
        //DBInterface.ConnectDB();
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        int i = 0;
        if (result != null)
        {
            if (result.Tables[0].Rows.Count == 0)
            {
                // Let's try this again and see if we can find the sequence in question in the Protein Tbl
                strSQL = string.Format("select * from protein_tbl WHERE PROTEIN_SEQUENCE LIKE '%{0}%'", DBInterface.SQLValidString(PepSeq.ToUpper()));

                result = DBInterface.QuerySQL2(strSQL);
                if (result != null)
                {
                    if (result.Tables[0].Rows.Count == 1) // Unique Find show Protein Info
                    {
                        string url = "ProteinInfo.aspx?";
                        url += "QType=" + "Protein ID" + "&";
                        url += "QValue=" + result.Tables[0].Rows[0].ItemArray[0].ToString();
                        Response.Redirect(url);
                    }
                    else
                    {
                        // Not unique show List of matches.
                        Response.Redirect(string.Format("ProteinList.aspx?Name={0}&T={1}&V={2}", M_V, "PLIST", M_V));
                    }
                }
                else
                    Response.Redirect(string.Format("NoResult.aspx?T={0}&V={1}", M_T, M_V));
            }
            else if (result.Tables[0].Rows.Count == 1)
            {
                Response.Redirect(string.Format("PeptideInfo.aspx?QType=Peptide+ID&QValue={0}", result.Tables[0].Rows[0].ItemArray[0].ToString()));
            }
            else
            {

                for (i = 0; i < result.Tables[0].Rows.Count && i < 50; i++)
                {
                    
                    TableRow trCaption = new TableRow();
                    TableCell tcPepID = new TableCell();
                    string PeptideID = result.Tables[0].Rows[i].ItemArray[0].ToString();
                    if (PeptideID.Length > 20)
                    {
                        briefSeq = PeptideID.Substring(0, 20);
                        briefSeq = briefSeq + "...";
                    }
                    else
                        briefSeq = PeptideID;
                    tcPepID.Text = string.Format(PepIDURL, PeptideID, briefSeq);
                    tcPepID.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcPepID);
                    //TableCell tcPreAA = new TableCell();
                    //tcPreAA.Text = result.GetString(2);
                    //tcPreAA.HorizontalAlign = HorizontalAlign.Center;
                    //trCaption.Cells.Add(tcPreAA);
                    TableCell tcSequence = new TableCell();
                    string PeptideSequence = result.Tables[0].Rows[i].ItemArray[0].ToString();
                    int iPos = PeptideSequence.ToUpper().IndexOf(PepSeq.ToUpper ());
                    if (iPos >= 0)
                    {
                        PeptideSequence = PeptideSequence.Insert(iPos + PepSeq.Length, "</font>");
                        PeptideSequence = PeptideSequence.Insert(iPos, "<font color='red'>");
                    }
                    tcSequence.Text = PeptideSequence;
                    tcSequence.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcSequence);
                    //TableCell tcNextAA = new TableCell();
                    //tcNextAA.Text = result.GetString(3);
                    //tcNextAA.HorizontalAlign = HorizontalAlign.Center;
                    //trCaption.Cells.Add(tcNextAA);
                    TableCell tcMW = new TableCell();
                    tcMW.Text = result.Tables[0].Rows[i].ItemArray[1].ToString();
                    tcMW.HorizontalAlign = HorizontalAlign.Right;
                    trCaption.Cells.Add(tcMW);
                    TableCell tcFully = new TableCell();
                    tcFully.Text = result.Tables[0].Rows[i].ItemArray[2].ToString();
                    tcFully.HorizontalAlign = HorizontalAlign.Center;
                    trCaption.Cells.Add(tcFully);
                    /*
                    TableCell tcEnzyme = new TableCell();
                    tcEnzyme.Text = result.Tables[0].Rows[i].ItemArray[3].ToString();
                    tcEnzyme.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcEnzyme);
                     */
                    //TableCell tcModel = new TableCell();
                    //if (!result.IsDBNull(5))
                    //    tcModel.Text = result.GetString(5);
                    //else
                    //    tcModel.Text = "";
                    //tcModel.HorizontalAlign = HorizontalAlign.Center;
                    //trCaption.Cells.Add(tcModel);

                    tbPeptides.Rows.Add(trCaption);

                }
            }
            //result.Close();            
            
        }
        
    }
}
