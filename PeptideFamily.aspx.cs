using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZJU.COPDB;

public partial class PeptideFamily : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string PeptideSeq =  Request.QueryString["PepSeq"];
        if (PeptideSeq != null)
            GetPeptideFamily(PeptideSeq);
    }

    private void GetPeptideFamily(string PeptideSeq)
    {
        string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
        string strSQL = string.Format("select p.peptide_cop_id,p.peptide_sequence,p.molecular_weight, t.score from peptide_tbl p,TABLE(blastp_match('{0}',cursor(select peptide_cop_id, peptide_sequence from peptide_tbl))) t where p.peptide_cop_id = t.T_SEQ_ID", PeptideSeq);
        //DBInterface.ConnectDB();
        IDataReader result = DBInterface.QuerySQL(strSQL);
        int i = 0;
        if (result != null)
        {

            while (result.Read())
            {
                TableRow trCaption = new TableRow();
                TableCell tcPepID = new TableCell();

                tcPepID.Text = string.Format(PepIDURL, result.GetString(0), result.GetString(0));
                tcPepID.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcPepID);

                TableCell tcSequence = new TableCell();
                tcSequence.Text = result.GetString(1);
                tcSequence.HorizontalAlign = HorizontalAlign.Left;
                trCaption.Cells.Add(tcSequence);

                TableCell tcMW = new TableCell();
                tcMW.Text = result.GetDouble(2).ToString();
                tcMW.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcMW);

                TableCell tcScore = new TableCell();
                tcScore.Text = result.GetValue(3).ToString();
                tcScore.HorizontalAlign = HorizontalAlign.Right;
                trCaption.Cells.Add(tcScore);

                tbPeptides.Rows.Add(trCaption);
                i += 1;
            }
            result.Close();


        }

        DBInterface.CloseDB();

    }
}
