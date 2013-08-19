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


public partial class SpectrumList : System.Web.UI.Page
{
    string M_T;
    string M_V;
    protected void Page_Load(object sender, EventArgs e)
    {

        string T = Request.QueryString["QType"];
        string V = Request.QueryString["QValue"];
        string Sort = "4";
        if (Request.QueryString ["Sort"] != null )
            Sort = Request.QueryString ["Sort"];
        M_T = T;
        M_V = V;

        if ((!Page.IsPostBack) && V != null)
        {
            /*DropDownList ddlQueryType = (DropDownList)(this.Master.FindControl("ddlQueryType"));
            ddlQueryType.Text = T;
            TextBox tbKeyWords = (TextBox)(this.Master.FindControl("tbKeyWords"));
            tbKeyWords.Text = V;*/
            try
            {
                float pmz = float.Parse(V);
                lbPrecursorRange.Text = string.Format("{0}~{1}.", pmz - 1, pmz + 1);
                QuerySpectrum(pmz,int.Parse (Sort) );
            }
            catch (Exception ex)
            {
                Response.Redirect(string.Format("NoResult.aspx?T={0}&V={1}", M_T, M_V));
            }
            
        }     
    }

    void CreateTableHead( int sort)
    {
        TableRow trCaption = new TableRow();
       
        TableCell tcSpectrumID = new TableCell();
        int sortcol = sort;
        string SortIcon = "";
        string SortURL = "{0}<a href='SpectrumList.aspx?QType=" + M_T + "&QValue=" + M_V + "&Sort={1}'>{2}</a>";
        if (sortcol < 0)
        {
            SortIcon = "<img src='_image/down.png' width='10' height='10' />";
        }
        else
        {
            SortIcon = "<img src='_image/up.png'  width='10' height='10'/>";
        }

        tcSpectrumID.Text = "";
        tcSpectrumID.HorizontalAlign = HorizontalAlign.Left;
        tcSpectrumID.BorderStyle = BorderStyle.Solid;
        tcSpectrumID.BorderColor = System.Drawing.Color.White;
        tcSpectrumID.BorderWidth = 1;
        trCaption.Cells.Add(tcSpectrumID);

        TableCell tcSequence = new TableCell();
        if (System.Math.Abs(sortcol) == 1)
        {
            tcSequence.Text = string.Format(SortURL, SortIcon, -1*sortcol , "Sequence");
        }
        else
        {
            tcSequence.Text = string.Format(SortURL, "", "+1", "Sequence");
        }
        tcSequence.HorizontalAlign = HorizontalAlign.Left;
        tcSequence.BorderStyle = BorderStyle.Solid;
        tcSequence.BorderColor = System.Drawing.Color.White;
        tcSequence.BorderWidth = 1;
        trCaption.Cells.Add(tcSequence);
        TableCell tcPTM = new TableCell();
        TableCell tcCharge = new TableCell();
        if (System.Math.Abs(sortcol) == 3)
        {
            tcCharge.Text = string.Format(SortURL, SortIcon, -1 * sortcol, "Charge");
        }
        else
        {
            tcCharge.Text = string.Format(SortURL, "", "+3", "Charge");
        }
        
        tcCharge.HorizontalAlign = HorizontalAlign.Center;
        tcCharge.BorderStyle = BorderStyle.Solid;
        tcCharge.BorderColor = System.Drawing.Color.White;
        tcCharge.BorderWidth = 1;
        trCaption.Cells.Add(tcCharge);
        TableCell tcPrecurMZ = new TableCell();
        if (System.Math.Abs(sortcol) == 4)
        {
            tcPrecurMZ.Text = string.Format(SortURL, SortIcon, -1 * sortcol, "Precursor m/z");
        }
        else
        {
            tcPrecurMZ.Text = string.Format(SortURL, "", "+4", "Precursor m/z");
        }
        
        tcPrecurMZ.HorizontalAlign = HorizontalAlign.Left;
        tcPrecurMZ.BorderStyle = BorderStyle.Solid;
        tcPrecurMZ.BorderColor = System.Drawing.Color.White;
        tcPrecurMZ.BorderWidth = 1;
        trCaption.Cells.Add(tcPrecurMZ);

        TableCell tcxcorr = new TableCell();
        if (System.Math.Abs(sortcol) == 6)
        {
            tcxcorr.Text = string.Format(SortURL, SortIcon, -1 * sortcol, "Xcorr");
        }
        else
        {
            tcxcorr.Text = string.Format(SortURL, "", "+6", "Xcorr");
        }
       
        tcxcorr.HorizontalAlign = HorizontalAlign.Center;
        tcxcorr.BorderStyle = BorderStyle.Solid;
        tcxcorr.BorderColor = System.Drawing.Color.White;
        tcxcorr.BorderWidth = 1;
        trCaption.Cells.Add(tcxcorr);
        trCaption.CssClass = "DataGridHeader";
        tbSpectra.Rows.Add(trCaption);
    }

    private void QuerySpectrum(float pmz,int sort)
    {
        CreateTableHead(sort);
        string orderstring = "";
        switch (sort)
        {
            case 1:
                orderstring = "order by t.spectrum_seq";
                break;
            case -1:
                orderstring = "order by t.spectrum_seq desc";
                break;
            case 2:
                orderstring = "order by t.PTM_sequence";
                break;
            case -2:
                orderstring = "order by t.PTM_sequence desc";
                break;
            case 3:
                orderstring = "order by t.charge_state";
                break;
            case -3:
                orderstring = "order by t.charge_state desc";
                break;
            case 4:
                orderstring = "order by t.precur_mz";
                break;
            case -4:
                orderstring = "order by t.precur_mz desc";
                break;
            case 6:
                orderstring = "order by t.xcorr";
                break;
            case -6:
                orderstring = "order by t.xcorr desc";
                break;

        }

        string SpectrumURL = "<a href='SpectrumInfo.aspx?QValue={0}' Target='_blank'><img src='_image/spectrum.gif'/></a>";
        string strSQL = string.Format("select t.spectrum_seq, t.ptm_sequence, t.charge_state, t.th_precur_mz, t.xcorr from spectrum_tbl t where t.th_precur_mz>'{0}' and t.th_precur_mz<'{1}' {2}", (pmz-1), (pmz+1),orderstring);
        //DBInterface.ConnectDB();
        DataSet  result = DBInterface.QuerySQL2(strSQL);
        int i = 0;
        if (result != null)
        {
            if (result.Tables[0].Rows.Count == 0)
            {
                Response.Redirect(string.Format("NoResult.aspx?T={0}&V={1}", M_T, M_V));
            }
            else
            {
                for (i = 0; i < result.Tables[0].Rows.Count && i < 50; i++)
                {
                    TableRow trCaption = new TableRow();
                    TableCell tcSpectrumID = new TableCell();

                    tcSpectrumID.Text = string.Format(SpectrumURL, result.Tables[0].Rows[i].ItemArray[0].ToString());
                    tcSpectrumID.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcSpectrumID);
                    //TableCell tcPreAA = new TableCell();
                    //tcPreAA.Text = result.GetString(2);
                    //tcPreAA.HorizontalAlign = HorizontalAlign.Center;
                    //trCaption.Cells.Add(tcPreAA);
                    TableCell tcSequence = new TableCell(); 
                    string Sequence= result.Tables[0].Rows[i].ItemArray[1].ToString();
                    if (Sequence.Length > 20)
                        tcSequence.Text = string.Format ("<span title='{0}'>{1}...</span>",Sequence ,Sequence.Substring (0,20));
                    else
                        tcSequence.Text = Sequence ;
                    tcSequence.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcSequence);
                    //TableCell tcNextAA = new TableCell();
                    //tcNextAA.Text = result.GetString(3);
                    //tcNextAA.HorizontalAlign = HorizontalAlign.Center;
                    //trCaption.Cells.Add(tcNextAA);
                    /*TableCell tcPTM = new TableCell();
                    string PTM = result.Tables[0].Rows[i].ItemArray[2].ToString();
                    if (PTM.Length > 15)
                        tcPTM.Text = string.Format ("<span title='{0}'>{1}...</span>",PTM ,PTM.Substring (0,15));
                    else
                        tcPTM.Text = PTM=="NULL"?"":PTM;
                    tcPTM.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcPTM);
                    */
                    TableCell tcCharge = new TableCell();
                    tcCharge.Text = result.Tables[0].Rows[i].ItemArray[2].ToString();
                    tcCharge.HorizontalAlign = HorizontalAlign.Center;
                    trCaption.Cells.Add(tcCharge);
                    TableCell tcPrecurMZ = new TableCell();
                    tcPrecurMZ.Text = result.Tables[0].Rows[i].ItemArray[3].ToString();
                    tcPrecurMZ.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcPrecurMZ);
                    /*
                    TableCell tcInstrum = new TableCell();
                    string Instrument = result.Tables[0].Rows[i].ItemArray[4].ToString();
                    if (Instrument.Length > 15)
                        tcInstrum.Text = string.Format("<span title='{0}'>{1}...</span>", Instrument, Instrument.Substring(0, 15));
                    else
                        tcInstrum.Text = Instrument;
                    tcInstrum.HorizontalAlign = HorizontalAlign.Left;
                    trCaption.Cells.Add(tcInstrum);
                    */
                    TableCell tcxcorr = new TableCell();
                    tcxcorr.Text = result.Tables[0].Rows[i].ItemArray[4].ToString();
                    tcxcorr.HorizontalAlign = HorizontalAlign.Center;
                    trCaption.Cells.Add(tcxcorr);

                    tbSpectra.Rows.Add(trCaption);

                }
                //result.Close();

            }
            
        }
       

       // DBInterface.CloseDB();
    }
}
