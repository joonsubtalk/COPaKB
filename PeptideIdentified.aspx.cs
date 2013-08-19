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
using System.Xml;
using System.Web.Configuration;
using System.IO;
public partial class PeptideIdentified : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsCallback)
        {
            string TaskID = Request.QueryString["TaskID"];
            string PID = Request.QueryString["PID"];

            ShowPeptideList(TaskID,PID);
            //this.hlViewSummary.NavigateUrl = string.Format("javascript:mySpl.loadPage('RightContent', 'Report.aspx?TaskID={0}')", TaskID);
        }
         
    }

    private void ShowPeptideList(string TaskID, string PID)
    {
        string strReportFile = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + TaskID + "\\Result.txt.xml";// +result.GetString(6) + ".xml";
        FileInfo fiReportFile = new FileInfo(strReportFile);
        if (!fiReportFile.Exists)
        {
            Response.Redirect("ErrorPage.aspx?Err=2");
            return;
        }
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        settings.IgnoreWhitespace = true;
        settings.IgnoreComments = true;

        XmlReader xr = XmlReader.Create(strReportFile, settings);
        
        object Proteins = xr.NameTable.Add("Proteins");
        object ScanPeptide = xr.NameTable.Add("Scan-Peptide");

        //string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
        //string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}'>{1}</a>";
             
      
        while (xr.Read())
        {
           
            if (xr.IsStartElement() && Proteins == xr.Name)
            {
                if (PID != xr["COPaID"])
                    continue;                
                
                while (xr.Read())//get the child tables
                {
                    if (xr.IsStartElement() && ScanPeptide == xr.Name)
                    {
                        TableRow trPeptide = new TableRow();
                        TableCell tcSequence = new TableCell();
                        string Sequence = xr["PeptideSequence"];
                        if (Sequence.Length > 16)
                            tcSequence.Text = string.Format("<span title='{0}'>{1}...</span>", Sequence, Sequence.Substring(0, 16));//string.Format("<a href=\"javascript:vSp1.loadPage('RightContent', 'ProteinInfo.aspx?QType=Protein ID&QValue={0}')\">{1}</a>", strIPI, strIPI);
                        else
                            tcSequence.Text = Sequence;
                        trPeptide.Cells.Add(tcSequence);
                        //TableCell tcModifiedType = new TableCell();
                        //string PTM = "";
                        //if (xr["ModifiedType"] != null)
                        //   PTM= xr["ModifiedType"] == "NULL" ? "" : xr["ModifiedType"];
                        //if (PTM.Length > 30)
                        //    tcModifiedType.Text = string.Format("<span title='{0}'>{1}</span>", PTM, PTM.Substring(0, 30));
                        //else
                        //    tcModifiedType.Text =  PTM;
                        //trPeptide.Cells.Add(tcModifiedType);
                        TableCell tcFinalScore = new TableCell();
                        tcFinalScore.Text = xr["FinalyScore"].Substring (0,4);
                        tcFinalScore.HorizontalAlign = HorizontalAlign.Right;
                        trPeptide.Cells.Add(tcFinalScore);
                        TableCell tcDetamz = new TableCell();
                        tcDetamz.Text = xr["DetaMZ"].Substring (0,4);
                        tcDetamz.HorizontalAlign = HorizontalAlign.Right;
                        trPeptide.Cells.Add(tcDetamz);
                        TableCell tcmzfile = new TableCell();
                        string mzfile = xr["mzFile"];
                        if (mzfile.Length > 20)
                            tcmzfile.Text = string.Format("<span title='{0}'>{1}</span>", mzfile, mzfile.Substring(0, 20));
                        else
                            tcmzfile.Text = mzfile;

                        trPeptide.Cells.Add(tcmzfile);
                        TableCell tcScan = new TableCell ();
                        tcScan.Text = xr["Scan"];
                        tcScan.HorizontalAlign = HorizontalAlign.Right;
                        trPeptide.Cells.Add (tcScan );
                        TableCell tcSimilarity = new TableCell ();
                        tcSimilarity.Text = xr["SimilarityScore"].Substring (0,4);
                        tcSimilarity.HorizontalAlign = HorizontalAlign.Right;
                        trPeptide.Cells.Add (tcSimilarity );

                        trPeptide.Attributes.Add("onclick", string.Format("HighLightTR(this,'#f2e8da','#000000');myRef = window.open('MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}&File={3}&SEQ={4}','Spectra','left=300,top=10,width=850,height=880,toolbar=0,resizable=1,scrollbars=1');myRef.focus();", xr["Spectrum"], xr["Scan"], TaskID, xr["mzFile"], Sequence));
                        tblPeptide.Rows.Add(trPeptide);                     
                                               
                    }
                    else
                    {
                       
                        break;
                    }
                }
                break;
               
            }
            
        }
        xr.Close();
    }
}
