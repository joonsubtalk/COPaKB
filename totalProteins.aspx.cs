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

public partial class totalProteins : System.Web.UI.Page
{
    ArrayList allProteins = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSQL = string.Format("select PEPTIDE_COP_ID from SPECTRUM_TBL where LIB_MOD='mouse_heart_mitochondria'");
        DataSet result = DBInterface.QuerySQL2(strSQL);
        TextBox1.Text = result.Tables[0].Rows.Count.ToString();
        ArrayList rmRedundant = new ArrayList();
        ArrayList rmSpecies = new ArrayList();
        
        if (result != null)
        {
            for (int i = 0; i < result.Tables[0].Rows.Count; i++ )
            {
                string proSQL = string.Format("select PROTEIN_ID from PP_RELATION_TBL where PEPTIDE_SEQUENCE = '{0}'", result.Tables[0].Rows[i].ItemArray[0].ToString());
                DataSet proResult = DBInterface.QuerySQL2(proSQL);
                TextBox2.Text = proResult.Tables[0].Rows.Count.ToString();
                
                if (proResult != null)
                {
                    for (int n = 0; n < proResult.Tables[0].Rows.Count; n++)
                    {
                        allProteins.Add(proResult.Tables[0].Rows[n].ItemArray[0].ToString());
                    }
                }
                

            }
            

            for (int u = 0; u < allProteins.Count; u++ )
            {
                allProteins[u] = allProteins[u].ToString().Substring(0, 6);
            }
            
                rmRedundant.Add(allProteins[0]);
                for (int k = 1; k < allProteins.Count; k++)
                {
                    bool unique = true;
                    for (int l = 0; l < rmRedundant.Count; l++)
                    {
                        if (allProteins[k].ToString().Equals(rmRedundant[l].ToString()))
                        {
                            unique = false;
                            break;
                        }
                    }
                    if (unique)
                    {
                        rmRedundant.Add(allProteins[k]);
                    }
                }
                for (int i = 0; i < rmRedundant.Count; i++)
                {
                    string organismSQL = string.Format("select ORGANISM_SOURCE from PROTEIN_TBL where PROTEIN_COP_ID = '{0}'",rmRedundant[i].ToString());
                    DataSet organismResult = DBInterface.QuerySQL2(organismSQL);
                    if (organismResult != null && organismResult.Tables[0].Rows.Count != 0)
                    {
                        if (organismResult.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Equals("MOUSE"))
                        {
                            rmSpecies.Add(rmRedundant[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
          
        }

        TextBox3.Text = rmSpecies.Count.ToString();
        rmSpecies.Sort();
        for (int j = 0; j < rmSpecies.Count; j++)
        {
        TableRow proteinRow = new TableRow();
        TableCell proteinCell = new TableCell();
        proteinCell.Text = rmSpecies[j].ToString();
        proteinRow.Cells.Add(proteinCell);
        tbInfo.Rows.Add(proteinRow);
        }
    }
}