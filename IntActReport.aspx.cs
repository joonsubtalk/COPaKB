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

public partial class IntActReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pType = Request.QueryString["PType"];
        string Plists = Request.QueryString["PLists"];
        if ((!Page.IsPostBack) && Plists != null)
        {            
            string[] Proteins = Plists.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (pType == "IPI")
            {
                for (int i = 0; i < Proteins.Length; i++)
                {
                    Proteins[i] = getUniprot(Proteins[i]);
                    this.tbProteinLists.Text += Proteins[i] + "\r\n";
                  
                }  
                
                pType = "UNIPROT";
            }
            //if (pType == "UNIPROT")
            //    Proteins = DeRedundance(Proteins);
            InterActQuery2(Proteins ,pType); 
        }        
    }

    private string[] DeRedundance(string[] Proteins)
    {
        Hashtable ProteinList = new Hashtable();
        for (int i = 0; i < Proteins.Length; i++)
        {
            if (Proteins[i].Contains("-"))
            {
                Proteins[i] = Proteins[i].Substring(0, Proteins[i].IndexOf("-"));
            }

            if (!ProteinList.ContainsKey(Proteins[i]))
            {
                ProteinList.Add(Proteins[i], 1);
            }
        }
        string[] newProteins = new string[ProteinList.Keys.Count];
        int j=0;
        foreach (string key in ProteinList.Keys)
        {
            newProteins[j] = key;
                j++;
        }
        return newProteins ;


    }
    private string getGeneName(string Protein)
    {
        string strSQL = string.Format("select GENE_SYMBOL from protein_tbl  WHERE PROTEIN_COP_ID = '{0}'", Protein);
        DataSet   result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            if (result.Tables[0].Rows.Count > 0)
                return result.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper();
            else
                return Protein;
        }
        return Protein;    
    }

    private string getUniprot(string Protein)
    {
        string strSQL = string.Format("select UNIPROT from protein_tbl  WHERE PROTEIN_COP_ID = '{0}'", Protein);
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            if (result.Tables[0].Rows.Count > 0)
            { 
                if (! result.Tables[0].Rows[0].IsNull (0))
                {
                    string unprot =   result.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper();
                    if (unprot.Contains(";"))
                    {
                        unprot = unprot.Substring(0, unprot.IndexOf(";"));
                    }
                    if (unprot.Contains("-"))
                    {
                        unprot = unprot.Substring(0, unprot.IndexOf("-"));
                    }
                    return unprot;
                }
            }
            else
                return Protein;
        }
        return Protein;    
    }

    private string getUniprotByGene(string Genename)
    {
        string strSQL = string.Format("select Protein_cop_id from protein_tbl  WHERE GENE_SYMBOL = '{0}' and ORGANISM_SOURCE='Human'", Genename );
        DataSet result = DBInterface.QuerySQL2(strSQL);
        if (result != null)
        {
            if (result.Tables[0].Rows.Count > 0)
            {
                if (!result.Tables[0].Rows[0].IsNull(0))
                {
                    string unprot = result.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper();
                    if (unprot.Contains(";"))
                    {
                        unprot = unprot.Substring(0, unprot.IndexOf(";"));
                    }
                    if (unprot.Contains("-"))
                    {
                        unprot = unprot.Substring(0, unprot.IndexOf("-"));
                    }
                    return unprot;
                }
            }
            else
                return Genename ;
        }
        return Genename ;    
    }

    private void InterActQuery2(string[] Proteins, string pType)
    {
        if (pType == "IPI")
            this.rbIPI.Checked = true;
        else if (pType == "UNIPROT")
            this.rbUniprot.Checked = true;
        else
            this.rbGeneName.Checked = true;
      
        string ProteinList = "";
        for (int i = 0; i < Proteins.Length; i++)
        {
            if (ProteinList == "")
                ProteinList = string.Format("'{0}'", Proteins[i]);
            else
                ProteinList += string.Format(",'{0}'", Proteins[i]);
        }

        Hashtable IntactsList = new Hashtable();

        string strSQL = string.Format("select distinct INTACT_ID,interactorA_id,interactorB_id from INTACT_T where INTERACTORA_ID in ({0}) or INTERACTORB_ID in ({0}) order by interactorA_ID", ProteinList);
        if (pType == "GeneName")
        {
            strSQL = string.Format("select distinct INTACT_ID,ALIAS_INT_A,ALIAS_INT_B from INTACT_T where ALIAS_INT_A in ({0}) or ALIAS_INT_B in ({0}) order by ALIAS_INT_A", ProteinList);
        }
        DataSet Intactresult = DBInterface.QuerySQL2(strSQL);
        if (Intactresult != null)
        {
            for (int i = 0; i < Intactresult.Tables[0].Rows.Count; i++)
            {
                string IntactID = Intactresult.Tables[0].Rows[i].ItemArray[0].ToString();
                string IntactorA = Intactresult.Tables[0].Rows[i].ItemArray[1].ToString();
                string IntactorB = Intactresult.Tables[0].Rows[i].ItemArray[2].ToString();
                if (!IntactsList.ContainsKey(string.Format ("{0}_{1}",IntactorA ,IntactorB)))
                    IntactsList.Add (string.Format ("{0}_{1}",IntactorA ,IntactorB),IntactID);

                //TableRow trIntact = new TableRow();
                //TableCell tcFlag1 = new TableCell();
                //tcFlag1.Text = string.Format("<a href='http://www.ebi.ac.uk/intact/interaction/{0}' target='_blank'>{0}</a>",IntactID);
                //trIntact.Cells.Add(tcFlag1);
                //TableCell tcInteractorA1 = new TableCell();
                //tcInteractorA1.Text = IntactorA;
                //trIntact.Cells.Add(tcInteractorA1);
                //TableCell tcInteractorB1 = new TableCell();
                //tcInteractorB1.Text = IntactorB;
                //trIntact.Cells.Add(tcInteractorB1);


                //tblInteract.Rows.Add(trIntact);
            }
        }
        TableRow trHeader = new TableRow();
        TableCell tcFlag = new TableCell();
        tcFlag.Text = "<a href='http://www.ebi.ac.uk/intact/'><img src=\"_image\\intact-logo.gif\" title=\"EBI IntAct\"></a>";
        trHeader.Cells.Add(tcFlag);

        for (int i = 0; i < Proteins.Length; i++)
        {
            TableCell tcP = new TableCell();
            tcP.Text = string.Format("<div class=\"verticaltext\" height=\"100px\" width=\"20px\">{0}</div> ", Proteins[i]);
            tcP.Height = new Unit(100);
            //tcP.VerticalAlign = VerticalAlign.Top ;
            trHeader.Cells.Add(tcP);
        }
        trHeader.BackColor = System.Drawing.Color.FromArgb(234, 237, 238);

        tblInteract.Rows.Add(trHeader);
        trHeader.Height = new Unit(100);

        for (int i = 0; i < Proteins.Length; i++)
        {
            TableRow trProteins = new TableRow();
            TableCell tcProtein = new TableCell();
            tcProtein.Text = Proteins[i];
            tcProtein.BackColor = System.Drawing.Color.FromArgb(234, 237, 238);
            trProteins.Cells.Add(tcProtein);
            bool bWithAct = false;

            for (int j = 0; j < Proteins.Length; j++)
            {
                TableCell tcIntAct = new TableCell();
                tcIntAct.Width = new Unit("3px");
                if (IntactsList.ContainsKey(string.Format("{0}_{1}", Proteins[i], Proteins[j])))
                {
                    tcIntAct.Text = string.Format("<a href='http://www.ebi.ac.uk/intact/interaction/{0}' target='_blank'><img src=\"_image\\check.gif\" title=\"{1}-vs-{2}\" width=\"5px\"></a>", IntactsList[string.Format("{0}_{1}", Proteins[i], Proteins[j])], Proteins[i], Proteins[j]);
                    bWithAct = true;
                    tcIntAct.BackColor = System.Drawing.Color.FromArgb(0, 0, 245);
                }
                else
                {
                    tcIntAct.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                }               
                
                tcIntAct.HorizontalAlign = HorizontalAlign.Center;
              
                trProteins.Cells.Add(tcIntAct);
            }  
            if (bWithAct )
            tblInteract.Rows.Add(trProteins);
        }

        if (pType != "IPI")
        {
            Hashtable Pathways = new Hashtable();

            if (pType == "GeneName")
            {
                ProteinList = "";
                for (int i = 0; i < Proteins.Length; i++)
                {
                    if (ProteinList == "")
                        ProteinList = string.Format("'{0}'", getUniprotByGene(Proteins[i]));
                    else
                        ProteinList += string.Format(",'{0}'", getUniprotByGene(Proteins[i]));
                }
            }

             string strRecSQL = string.Format ("select distinct REACT_ID,PATHWAY,URL,uniprot from unipro2pathway_t where uniprot in ({0})", ProteinList );
             DataSet result = DBInterface.QuerySQL2(strRecSQL);
             if (result != null)
             {
                 for (int j = 0; j < result.Tables[0].Rows.Count; j++)
                 {
                     PathWay pw = new PathWay();
                     pw.React_ID = result.Tables[0].Rows[j].ItemArray[0].ToString();
                     pw.PathWayName = result.Tables[0].Rows[j].ItemArray[1].ToString();
                     pw.PahtwayURL = result.Tables[0].Rows[j].ItemArray[2].ToString();
                     string uniprotid = result.Tables[0].Rows[j].ItemArray[3].ToString();
                     if (Pathways.Contains(pw))
                     {
                         Pathways[pw] += "|" + uniprotid + ";" + uniprotid;
                     }
                     else
                         Pathways.Add(pw, uniprotid  + ";" + uniprotid);
                 }
             }
            //// show the pathway information
            //for (int i = 0; i < Proteins.Length; i++)
            //{
            //    string uniprotid = Proteins[i];
            //    if (pType == "GeneName")
            //        uniprotid = getUniprotByGene(Proteins[i]);
            //    string strRecSQL = string.Format("select REACT_ID,PATHWAY,URL from unipro2pathway_t where uniprot='{0}'", uniprotid);
            //    DataSet result = DBInterface.QuerySQL2(strRecSQL);
            //    
            //    {
            //        for (int j = 0; j < result.Tables[0].Rows.Count; j++)
            //        {
            //            PathWay pw = new PathWay();
            //            pw.React_ID = result.Tables[0].Rows[j].ItemArray[0].ToString();
            //            pw.PathWayName = result.Tables[0].Rows[j].ItemArray[1].ToString();
            //            pw.PahtwayURL = result.Tables[0].Rows[j].ItemArray[2].ToString();

            //            if (Pathways.Contains(pw))
            //            {
            //                Pathways[pw] += "|" + Proteins[i] + ";" + uniprotid;
            //            }
            //            else
            //                Pathways.Add(pw, Proteins[i] + ";" + uniprotid);
            //        }
            //    }
            //}
            TableRow trPathHeader = new TableRow();
            TableCell tcPathHeader = new TableCell();
            tcPathHeader.Text = "<a href='http://www.reactome.org/'><img src=\"_image\\R-purple-fly.png\" title=\"Reactome\" width=\"64px\" height=\"64px\"></a>";
            
            trPathHeader.Cells.Add(tcPathHeader);
            TableCell tcProteinsInPath = new TableCell();
            tcProteinsInPath.Text = "Proteins in the pathway";
            trPathHeader.Cells.Add(tcProteinsInPath);
            tbPathways.Rows.Add(trPathHeader);
            foreach (PathWay pw in Pathways.Keys)
            {
                TableRow trPath = new TableRow();
                TableCell tcPathWay = new TableCell();
                tcPathWay.Text = string.Format("<a href=\"{0}\" target='_blank'>{1}</a>", pw.PahtwayURL, pw.PathWayName);
                trPath.Cells.Add(tcPathWay);
                TableCell tcProteins = new TableCell();
                string[] Prots = Pathways[pw].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < Prots.Length; i++)
                {
                    string protname = Prots[i].Substring(0, Prots[i].IndexOf(";"));
                    string uniprotname = Prots[i].Substring(Prots[i].IndexOf(";") + 1);
                    tcProteins.Text += string.Format("<a href=\"http://www.reactome.org/cgi-bin/link?SOURCE=UniProt&ID={0}\" target='_blank'>{1}</a> ", uniprotname, protname);
                }
                trPath.Cells.Add(tcProteins);
                tbPathways.Rows.Add(trPath);
            }
        }

    }
    private void InterActQuery(string[] Proteins,string pType)
    {
        if (pType == "IPI")
            this.rbIPI.Checked = true;
        else if (pType == "UNIPROT")
            this.rbUniprot.Checked = true;
        else
            this.rbGeneName.Checked = true;

        TableRow trHeader = new TableRow();
        TableCell tcFlag = new TableCell();
        tcFlag.Text = "Proteins";
        trHeader.Cells.Add(tcFlag);
        
        for (int i = 0; i < Proteins.Length; i++)
        {
            TableCell tcP = new TableCell();
            tcP.Text = string.Format("<div class=\"verticaltext\" height=\"100px\" width=\"100px\">{0}</div> ", Proteins[i]);
            tcP.Height = new Unit(100);
            //tcP.VerticalAlign = VerticalAlign.Top ;
            trHeader.Cells.Add(tcP);           
        }
        trHeader.BackColor = System.Drawing.Color.FromArgb(234,237,238);

        tblInteract.Rows.Add(trHeader);
       
        trHeader.Height = new Unit(100);
        
        for (int i = 0; i < Proteins.Length; i++)
        {
            TableRow trProteins = new TableRow();
            TableCell tcProtein = new TableCell();
            tcProtein.Text = Proteins[i];
            tcProtein.BackColor = System.Drawing.Color.FromArgb(234, 237, 238);
            trProteins.Cells.Add(tcProtein);

            for (int j = 0; j < Proteins.Length; j++)
            {
                string IntactID = GetIntActID(Proteins[i], Proteins[j],pType);
               
                TableCell tcIntAct = new TableCell();
                if (IntactID != null)
                {                    
                    tcIntAct.Text = string.Format("<a href='http://www.ebi.ac.uk/intact/interaction/{0}' target='_blank'><img src=\"_image\\check.gif\" title=\"{1}-vs-{2}\"></a>",IntactID,Proteins [i],Proteins[j] );
                }
                else
                {
                    
                }
                tcIntAct.HorizontalAlign = HorizontalAlign.Center;
                trProteins.Cells.Add(tcIntAct);
            }
            tblInteract.Rows.Add(trProteins);
        }
        

        if (pType != "IPI")
        {
            Hashtable Pathways = new Hashtable();
            // show the pathway information
            for (int i = 0; i < Proteins.Length; i++)
            {
                string uniprotid = Proteins[i];
                if (pType == "GeneName")
                    uniprotid = getUniprotByGene(Proteins [i]);
                string strRecSQL = string.Format("select REACT_ID,PATHWAY,URL from unipro2pathway_t where uniprot='{0}'", uniprotid);
                 DataSet result = DBInterface.QuerySQL2(strRecSQL);
                 if (result != null)
                 {
                     for (int j=0;j<result.Tables[0].Rows.Count;j++)
                     {
                         PathWay pw = new PathWay();
                         pw.React_ID = result.Tables[0].Rows[j].ItemArray[0].ToString ();
                         pw.PathWayName = result.Tables[0].Rows[j].ItemArray[1].ToString ();
                         pw.PahtwayURL = result.Tables[0].Rows[j].ItemArray[2].ToString ();

                         if (Pathways.Contains(pw))
                         {
                             Pathways[pw] += "|" + Proteins[i]+";"+uniprotid  ;
                         }
                         else
                             Pathways.Add(pw, Proteins[i] + ";" + uniprotid);
                     }
                 }
            }
            TableRow trPathHeader = new TableRow();
            TableCell tcPathHeader = new TableCell();
            tcPathHeader.Text = "Pathway";
            trPathHeader.Cells.Add(tcPathHeader);
            TableCell tcProteinsInPath = new TableCell();
            tcProteinsInPath.Text = "Proteins in the pathway";
            trPathHeader.Cells.Add(tcProteinsInPath);
            tbPathways.Rows.Add(trPathHeader);
            foreach (PathWay pw in Pathways.Keys )
            {
                TableRow trPath = new TableRow();
                TableCell tcPathWay = new TableCell();
                tcPathWay.Text = string.Format("<a href=\"{0}\" target='_blank'>{1}</a>", pw.PahtwayURL, pw.PathWayName);
                trPath.Cells.Add(tcPathWay);
                TableCell tcProteins = new TableCell();
                string[] Prots = Pathways[pw].ToString ().Split (new string[]{"|"},StringSplitOptions.RemoveEmptyEntries );
                for (int i=0;i<Prots.Length ;i++)
                {
                    string protname = Prots[i].Substring(0, Prots[i].IndexOf(";"));
                    string uniprotname = Prots[i].Substring(Prots[i].IndexOf(";") + 1);
                    tcProteins.Text += string.Format("<a href=\"http://www.reactome.org/cgi-bin/link?SOURCE=UniProt&ID={0}\" target='_blank'>{1}</a> ", uniprotname ,protname );
                }
                trPath.Cells.Add(tcProteins);
                tbPathways.Rows.Add(trPath);
            }
        }
    }

    private struct PathWay
    {
        public string React_ID;
        public string PathWayName;
        public string PahtwayURL;
    }

    private string GetIntActID(string interactorA, string interactorB,string pType)
    {
        string strSQL = String.Format("select INTACT_ID from INTACT_T where ALIAS_INT_A='{0}' and ALIAS_INT_B='{1}'", interactorA.ToUpper (), interactorB.ToUpper ());
        if (pType == "UNIPROT")
        {
            strSQL = String.Format("select INTACT_ID from INTACT_T where INTERACTORA_ID='{0}' and INTERACTORB_ID='{1}'", interactorA, interactorB);
        }
        DataSet   result = DBInterface.QuerySQL2(strSQL);
        if (result == null)
        {
            return null;
        }
        else
        {
            if (result.Tables[0].Rows.Count > 0)
            {
                return result.Tables[0].Rows[0].ItemArray[0].ToString ();
            }
            else
            {
                return null;
            }
        }
    }
    protected void btShowInteract_Click(object sender, EventArgs e)
    {
        string pType = "IPI";
        if (this.rbIPI.Checked)
            pType = "IPI";
        else if (this.rbGeneName.Checked)
            pType = "GeneName";
        else
            pType = "UNIPROT";
        string[] Proteins = this.tbProteinLists.Text.Split(new string[] { "|", " ", "\r\n","\t",",",";"}, StringSplitOptions.RemoveEmptyEntries);
        if (pType == "IPI")
        {
            this.tbProteinLists.Text = "";
            for (int i = 0; i < Proteins.Length; i++)
            {
                Proteins[i] = getUniprot(Proteins[i]);
                this.tbProteinLists.Text += Proteins[i] + "\r\n";

            }

            pType = "UNIPROT";
        }
        InterActQuery(Proteins, pType);
    }
}
