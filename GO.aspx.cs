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

using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Globalization;

public partial class GO : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pType = Request.QueryString["PType"];
        string Plists = Request.QueryString["PLists"];
        string bQuant = Request.QueryString["Quant"];
        if ((!Page.IsPostBack) && Plists != null)
        {
            if (bQuant == "True")
            {
                Dictionary<string, float> ProteinQuants = new Dictionary<string, float>();
                string[] ProteinQuantitations = Plists.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
               
                for (int i = 0; i < ProteinQuantitations.Length; i++)
                {
                    string protein = ProteinQuantitations[i].Substring(0, ProteinQuantitations[i].IndexOf(";"));
                    float Quant = float.Parse(ProteinQuantitations[i].Substring(ProteinQuantitations[i].IndexOf(";") + 1));
                    ProteinQuants.Add(protein, Quant);
                }
                
                ShowGOAnnotation(ProteinQuants , pType);
            }
            else
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

                ShowGOAnnotation(Proteins, pType);
            }
        }        
    }

    private string getUniprot(string Protein)
    {
        string strSQL = string.Format("select UNIPROT from protein_tbl  WHERE PROTEIN_COP_ID = '{0}'", Protein);
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
                return Protein;
        }
        return Protein;
    }

    private void ShowGOAnnotation(string[] Proteins, string pType)
    {
        if (Proteins.Length == 0)
            return;
        string ProteinList = string.Format ("'{0}'",Proteins[0]);
        for (int i = 1; i < Proteins.Length; i++)
        {
            ProteinList += string.Format(",'{0}'", Proteins[i]);
        }
        string strSQL = "";
        if (pType == "UNIPROT")
        {            
            strSQL = string.Format("select distinct protein_cop_id, go from Protein_tbl t where protein_cop_id in ({0})", ProteinList);
        }
        else if (pType == "GeneName")
        {
            strSQL = string.Format("select distinct Gene_symbol, go from Protein_tbl t where gene_symbol in ({0})",ProteinList );
        }

        IDataReader GOResult = DBInterface.QuerySQL(strSQL );
        Dictionary<string, int> CellComponent = new Dictionary<string, int>();
        Dictionary<string, int> BiologyProcess = new Dictionary<string, int>();
        Dictionary<string, int> MolecularFunction = new Dictionary<string, int>();
        int CellComponentCount = 0;
        int BiologyProcessCount = 0;
        int MolecularFunctionCount = 0;

        if (GOResult != null)
        { 
           while (GOResult.Read ())
           {
               if (GOResult.IsDBNull(1))
                   continue;
               string GOString = GOResult.GetString(1);
               string[] GOAnnotations = GOString.Split(new string[] { "|" },StringSplitOptions.RemoveEmptyEntries );
               foreach (string GOAnnotation in GOAnnotations)
               {
                   string[] GOTerms = GOAnnotation.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                   if (GOTerms[0] == "C")
                   { // go annotation about cellcomponent
                       if (CellComponent.ContainsKey(GOTerms[2]))
                       {
                           CellComponent[GOTerms[2]]= (int)CellComponent[GOTerms[2]] +1;
                       }
                       else
                           CellComponent.Add (GOTerms[2],1);

                       CellComponentCount++;
                        
                   }
                   else if (GOTerms [0]=="F")
                   {
                       if (MolecularFunction.ContainsKey(GOTerms[2]))
                       {
                           MolecularFunction[GOTerms[2]] = (int)MolecularFunction[GOTerms[2]] + 1;
                       }
                       else
                           MolecularFunction.Add(GOTerms[2], 1);
                       MolecularFunctionCount++;
                   }
                   else if (GOTerms[0]== "P")
                   {
                       if (BiologyProcess.ContainsKey(GOTerms[2]))
                       {
                           BiologyProcess[GOTerms[2]] = (int)BiologyProcess[GOTerms[2]] + 1;
                       }
                       else
                           BiologyProcess.Add(GOTerms[2], 1);

                       BiologyProcessCount++;
                   }
                   else
                       continue ;
               }
            }
           string ImgURL = "PieChart.aspx?Partitions={0}";
           string definecolors = ConfigurationSettings.AppSettings["PieColors"];
           string[] colors = definecolors.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
           ArrayList PieColors = new ArrayList();
           foreach (string color in colors)
           {
               string[] rgbs = color.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
               int R = int.Parse(rgbs[0]);
               int G = int.Parse(rgbs[1]);
               int B = int.Parse(rgbs[2]);
               System.Drawing.Color newcolor = Color.FromArgb(R, G, B);

               PieColors.Add(newcolor);
           }

           if (CellComponent.Count > 0)
           {
               var sortedDict = (from entry in CellComponent orderby entry.Value descending select entry);
               int count = sortedDict.Count();

               string Partitions = "";
               int Others = 0;
               bool bOthers = false;
               if (count > 20)
               {
                   count = 20;
                   bOthers = true;
               }
               for (int i = 0; i < count; i++)
               { 
                   KeyValuePair<string, int> ipiv = sortedDict.ElementAt(i);
                   Others += ipiv.Value;
                   Partitions += string.Format("|{0};{1}", ipiv.Key, ((float)ipiv.Value) / CellComponentCount);
                   TableRow trAnnotation = new TableRow();
                   TableCell tcColor = new TableCell();
                   tcColor.Text = " ";
                   tcColor.Width = new Unit ("10px");
                   tcColor.BackColor = (Color)PieColors[i % PieColors.Count];
                   trAnnotation.Cells.Add(tcColor );
                   TableCell tcAnnotation = new TableCell();
                   tcAnnotation.Text = string.Format("{0} ({1})", ipiv.Key, (((float)ipiv.Value) / CellComponentCount).ToString("P", CultureInfo.InvariantCulture));
                   tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                   trAnnotation.Cells.Add(tcAnnotation);
                   tbCC.Rows.Add(trAnnotation);

                  
               }
               if (bOthers)
               {
                   Partitions += string.Format("|others;{0}", ((float)(CellComponentCount - Others)) / CellComponentCount);
                   TableRow trAnnotation = new TableRow();
                   TableCell tcColor = new TableCell();
                   tcColor.Text = " ";
                   tcColor.Width = new Unit("10px");
                   tcColor.BackColor = (Color)PieColors[20%PieColors.Count ];
                   trAnnotation.Cells.Add(tcColor);
                   TableCell tcAnnotation = new TableCell();
                   tcAnnotation.Text = string.Format("{0} ({1})", "others", (((float)(CellComponentCount - Others)) / CellComponentCount).ToString("P", CultureInfo.InvariantCulture));

                   tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                   trAnnotation.Cells.Add(tcAnnotation);
                   tbCC.Rows.Add(trAnnotation);
               }
               imgCellComponent.ImageUrl = string.Format(ImgURL, Partitions);
               
               
           }

           if (BiologyProcess.Count > 0)
           {
               var sortedDict = (from entry in BiologyProcess orderby entry.Value descending select entry);
               int count = sortedDict.Count();

               string Partitions = "";
               int Others = 0;
               bool bOthers = false;
               if (count > 20)
               {
                   count = 20;
                   bOthers = true;
               }
               for (int i = 0; i < count; i++)
               {
                   KeyValuePair<string, int> ipiv = sortedDict.ElementAt(i);
                   Others += ipiv.Value;
                   Partitions += string.Format("|{0};{1}", ipiv.Key, ((float)ipiv.Value) / BiologyProcessCount);
                   TableRow trAnnotation = new TableRow();
                   TableCell tcColor = new TableCell();
                   tcColor.Text = " ";
                   tcColor.Width = new Unit("10px");
                   tcColor.BackColor = (Color)PieColors[i % PieColors.Count];
                   trAnnotation.Cells.Add(tcColor);
                   TableCell tcAnnotation = new TableCell();
                   tcAnnotation.Text = string.Format("{0} ({1})", ipiv.Key, (((float)ipiv.Value) / BiologyProcessCount).ToString("P", CultureInfo.InvariantCulture));
                 
                   tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                   trAnnotation.Cells.Add(tcAnnotation);
                   tbBP.Rows.Add(trAnnotation);
               }
               if (bOthers)
               {
                   Partitions += string.Format("|others;{0}", ((float)(BiologyProcessCount - Others)) / BiologyProcessCount);
                   TableRow trAnnotation = new TableRow();
                   TableCell tcColor = new TableCell();
                   tcColor.Text = " ";
                   tcColor.Width = new Unit("10px");
                   tcColor.BackColor = (Color)PieColors[20 % PieColors.Count];
                   trAnnotation.Cells.Add(tcColor);
                   TableCell tcAnnotation = new TableCell();
                   tcAnnotation.Text = string.Format("{0} ({1})", "others", (((float)(CellComponentCount - Others)) / CellComponentCount).ToString("P", CultureInfo.InvariantCulture));

                   tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                   trAnnotation.Cells.Add(tcAnnotation);
                   tbBP.Rows.Add(trAnnotation);
               }
     
               imgBiologicalProcess.ImageUrl = string.Format(ImgURL, Partitions);
           }
           if (MolecularFunction.Count > 0)
           {
               var sortedDict = (from entry in MolecularFunction orderby entry.Value descending select entry);
               int count = sortedDict.Count();

               string Partitions = "";
               int Others = 0;
               bool bOthers = false;
               if (count > 20)
               {
                   count = 20;
                   bOthers = true;
               }
               for (int i = 0; i < count; i++)
               {
                   KeyValuePair<string, int> ipiv = sortedDict.ElementAt(i);
                   Others += ipiv.Value ;
                   Partitions += string.Format("|{0};{1}", ipiv.Key, ((float)ipiv.Value) / MolecularFunctionCount);
                   TableRow trAnnotation = new TableRow();
                   TableCell tcColor = new TableCell();
                   tcColor.Text = " ";
                   tcColor.Width = new Unit("10px");
                   tcColor.BackColor = (Color)PieColors[i % PieColors.Count];
                   trAnnotation.Cells.Add(tcColor);
                   TableCell tcAnnotation = new TableCell();
                   tcAnnotation.Text = string.Format("{0} ({1})", ipiv.Key, (((float)ipiv.Value) / MolecularFunctionCount).ToString("P", CultureInfo.InvariantCulture));
                 
                   tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                   trAnnotation.Cells.Add(tcAnnotation);
                   tbMF.Rows.Add(trAnnotation);

               }
               if (bOthers)
               {
                   Partitions += string.Format("|others;{0}", ((float)(MolecularFunctionCount - Others)) / MolecularFunctionCount);
                   TableRow trAnnotation = new TableRow();
                   TableCell tcColor = new TableCell();
                   tcColor.Text = " ";
                   tcColor.Width = new Unit("10px");
                   tcColor.BackColor = (Color)PieColors[20 % PieColors.Count];
                   trAnnotation.Cells.Add(tcColor);
                   TableCell tcAnnotation = new TableCell();
                   tcAnnotation.Text = string.Format("{0} ({1})", "others", (((float)(CellComponentCount - Others)) / CellComponentCount).ToString("P", CultureInfo.InvariantCulture));

                   tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                   trAnnotation.Cells.Add(tcAnnotation);
                   tbMF.Rows.Add(trAnnotation);
               }
               imgMolecularFunction.ImageUrl = string.Format(ImgURL, Partitions);
           }

        }
    }

    private void ShowGOAnnotation(Dictionary<string, float> ProteinQuants, string pType)
    {
        if (ProteinQuants.Count  == 0)
            return;
        string ProteinList = "";
        foreach (string Protein in ProteinQuants.Keys)
        {
            if (ProteinList == "")
                ProteinList = string.Format("'{0}'", Protein);
            else
                ProteinList += string.Format(",'{0}'", Protein);
        }
         
        string strSQL = "";
        if (pType == "UNIPROT")
        {
            strSQL = string.Format("select distinct protein_cop_id, go from Protein_tbl t where protein_cop_id in ({0})", ProteinList);
        }
        else if (pType == "GeneName")
        {
            strSQL = string.Format("select distinct Gene_symbol, go from Protein_tbl t where gene_symbol in ({0})", ProteinList);
        }

        IDataReader GOResult = DBInterface.QuerySQL(strSQL);
        Dictionary<string, float> CellComponent = new Dictionary<string, float>();
        Dictionary<string, float> BiologyProcess = new Dictionary<string, float>();
        Dictionary<string, float> MolecularFunction = new Dictionary<string, float>();
        float CellComponentCount = 0;
        float BiologyProcessCount = 0;
        float MolecularFunctionCount = 0;

        if (GOResult != null)
        {
            while (GOResult.Read())
            {
                if (GOResult.IsDBNull(1))
                    continue;
                string GOString = GOResult.GetString(1);
                string[] GOAnnotations = GOString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string GOAnnotation in GOAnnotations)
                {
                    string[] GOTerms = GOAnnotation.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (GOTerms[0] == "C")
                    { // go annotation about cellcomponent
                        if (CellComponent.ContainsKey(GOTerms[2]))
                        {
                            CellComponent[GOTerms[2]] = (float)CellComponent[GOTerms[2]] + (float)ProteinQuants[GOResult.GetString(0)];
                        }
                        else
                            CellComponent.Add(GOTerms[2], (float)ProteinQuants[GOResult.GetString(0)]);

                        CellComponentCount += (float)ProteinQuants[GOResult.GetString(0)];

                    }
                    else if (GOTerms[0] == "F")
                    {
                        if (MolecularFunction.ContainsKey(GOTerms[2]))
                        {
                            MolecularFunction[GOTerms[2]] = (float)MolecularFunction[GOTerms[2]] + (float)ProteinQuants[GOResult.GetString(0)];
                        }
                        else
                            MolecularFunction.Add(GOTerms[2], (float)ProteinQuants[GOResult.GetString(0)]);
                        MolecularFunctionCount += (float)ProteinQuants[GOResult.GetString(0)];
                    }
                    else if (GOTerms[0] == "P")
                    {
                        if (BiologyProcess.ContainsKey(GOTerms[2]))
                        {
                            BiologyProcess[GOTerms[2]] = (float)BiologyProcess[GOTerms[2]] + (float)ProteinQuants[GOResult.GetString(0)];
                        }
                        else
                            BiologyProcess.Add(GOTerms[2], (float)ProteinQuants[GOResult.GetString(0)]);

                        BiologyProcessCount += (float)ProteinQuants[GOResult.GetString(0)];
                    }
                    else
                        continue;
                }
            }
            string ImgURL = "PieChart.aspx?Partitions={0}";
            string definecolors = ConfigurationSettings.AppSettings["PieColors"];
            string[] colors = definecolors.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            ArrayList PieColors = new ArrayList();
            foreach (string color in colors)
            {
                string[] rgbs = color.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int R = int.Parse(rgbs[0]);
                int G = int.Parse(rgbs[1]);
                int B = int.Parse(rgbs[2]);
                System.Drawing.Color newcolor = Color.FromArgb(R, G, B);

                PieColors.Add(newcolor);
            }

            if (CellComponent.Count > 0)
            {
                var sortedDict = (from entry in CellComponent orderby entry.Value descending select entry);
                int count = sortedDict.Count();

                string Partitions = "";
                float Others = 0;
                bool bOthers = false;
                if (count > 20)
                {
                    count = 20;
                    bOthers = true;
                }
                for (int i = 0; i < count; i++)
                {
                    KeyValuePair<string, float> ipiv = sortedDict.ElementAt(i);
                    Others += ipiv.Value;
                    Partitions += string.Format("|{0};{1}", ipiv.Key, ((float)ipiv.Value) / CellComponentCount);
                    TableRow trAnnotation = new TableRow();
                    TableCell tcColor = new TableCell();
                    tcColor.Text = " ";
                    tcColor.Width = new Unit("10px");
                    tcColor.BackColor = (Color)PieColors[i % PieColors.Count];
                    trAnnotation.Cells.Add(tcColor);
                    TableCell tcAnnotation = new TableCell();
                    tcAnnotation.Text = ipiv.Key;
                    tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                    trAnnotation.Cells.Add(tcAnnotation);
                    tbCC.Rows.Add(trAnnotation);
                }
                if (bOthers)
                {
                    Partitions += string.Format("|others;{0}", ((float)(CellComponentCount - Others)) / CellComponentCount);
                    TableRow trAnnotation = new TableRow();
                    TableCell tcColor = new TableCell();
                    tcColor.Text = " ";
                    tcColor.Width = new Unit("10px");
                    tcColor.BackColor = (Color)PieColors[20 % PieColors.Count];
                    trAnnotation.Cells.Add(tcColor);
                    TableCell tcAnnotation = new TableCell();
                    tcAnnotation.Text = "others";
                    tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                    trAnnotation.Cells.Add(tcAnnotation);
                    tbCC.Rows.Add(trAnnotation);
                }
                imgCellComponent.ImageUrl = string.Format(ImgURL, Partitions);

            }

            if (BiologyProcess.Count > 0)
            {
                var sortedDict = (from entry in BiologyProcess orderby entry.Value descending select entry);
                int count = sortedDict.Count();

                string Partitions = "";
                float Others = 0;
                bool bOthers = false;
                if (count > 20)
                {
                    count = 20;
                    bOthers = true;
                }
                for (int i = 0; i < count; i++)
                {
                    KeyValuePair<string, float> ipiv = sortedDict.ElementAt(i);
                    Others += ipiv.Value;
                    Partitions += string.Format("|{0};{1}", ipiv.Key, ((float)ipiv.Value) / BiologyProcessCount);
                    TableRow trAnnotation = new TableRow();
                    TableCell tcColor = new TableCell();
                    tcColor.Text = " ";
                    tcColor.Width = new Unit("10px");
                    tcColor.BackColor = (Color)PieColors[i % PieColors.Count];
                    trAnnotation.Cells.Add(tcColor);
                    TableCell tcAnnotation = new TableCell();
                    tcAnnotation.Text = ipiv.Key;
                    tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                    trAnnotation.Cells.Add(tcAnnotation);
                    tbBP.Rows.Add(trAnnotation);
                }
                if (bOthers)
                {
                    Partitions += string.Format("|others;{0}", ((float)(BiologyProcessCount - Others)) / BiologyProcessCount);
                    TableRow trAnnotation = new TableRow();
                    TableCell tcColor = new TableCell();
                    tcColor.Text = " ";
                    tcColor.Width = new Unit("10px");
                    tcColor.BackColor = (Color)PieColors[20 % PieColors.Count];
                    trAnnotation.Cells.Add(tcColor);
                    TableCell tcAnnotation = new TableCell();
                    tcAnnotation.Text = "others";
                    tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                    trAnnotation.Cells.Add(tcAnnotation);
                    tbBP.Rows.Add(trAnnotation);
                }

                imgBiologicalProcess.ImageUrl = string.Format(ImgURL, Partitions);
            }
            if (MolecularFunction.Count > 0)
            {
                var sortedDict = (from entry in MolecularFunction orderby entry.Value descending select entry);
                int count = sortedDict.Count();

                string Partitions = "";
                float Others = 0;
                bool bOthers = false;
                if (count > 20)
                {
                    count = 20;
                    bOthers = true;
                }
                for (int i = 0; i < count; i++)
                {
                    KeyValuePair<string, float> ipiv = sortedDict.ElementAt(i);
                    Others += ipiv.Value;
                    Partitions += string.Format("|{0};{1}", ipiv.Key, ((float)ipiv.Value) / MolecularFunctionCount);
                    TableRow trAnnotation = new TableRow();
                    TableCell tcColor = new TableCell();
                    tcColor.Text = " ";
                    tcColor.Width = new Unit("10px");
                    tcColor.BackColor = (Color)PieColors[i % PieColors.Count];
                    trAnnotation.Cells.Add(tcColor);
                    TableCell tcAnnotation = new TableCell();
                    tcAnnotation.Text = ipiv.Key;
                    tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                    trAnnotation.Cells.Add(tcAnnotation);
                    tbMF.Rows.Add(trAnnotation);

                }
                if (bOthers)
                {
                    Partitions += string.Format("|others;{0}", ((float)(MolecularFunctionCount - Others)) / MolecularFunctionCount);
                    TableRow trAnnotation = new TableRow();
                    TableCell tcColor = new TableCell();
                    tcColor.Text = " ";
                    tcColor.Width = new Unit("10px");
                    tcColor.BackColor = (Color)PieColors[20 % PieColors.Count];
                    trAnnotation.Cells.Add(tcColor);
                    TableCell tcAnnotation = new TableCell();
                    tcAnnotation.Text = "others";
                    tcAnnotation.HorizontalAlign = HorizontalAlign.Left;
                    trAnnotation.Cells.Add(tcAnnotation);
                    tbMF.Rows.Add(trAnnotation);
                }
                imgMolecularFunction.ImageUrl = string.Format(ImgURL, Partitions);
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
        string[] Proteins = this.tbProteinLists.Text.Split(new string[] { "|", " ", "\r\n", "\t", ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
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
        ShowGOAnnotation(Proteins, pType);
    }
}
