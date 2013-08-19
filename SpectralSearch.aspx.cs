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
using System.Text;
using ZJU.COPDB;
using System.Web.Configuration;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Net;

public partial class SpectralSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string SamplePath = WebConfigurationManager.ConnectionStrings["SampleData"].ConnectionString;
        
        if (Directory.Exists(SamplePath) && ddlSamples.Items.Count ==0)
        {  
            string[] Files = Directory.GetFiles(SamplePath);
            foreach (string samplefile in Files)
            {
                string FileName = samplefile.Substring(samplefile.LastIndexOf("\\")+1); 
                this.ddlSamples.Items.Add(FileName);
               
            }
        }
        if (!Page.IsPostBack)
        {
            ddlModules.Items.Clear();
            try
            {
                COPaWS.COPaWS ws = new COPaWS.COPaWS();
                NetworkCredential objCredential = new NetworkCredential(ConfigurationSettings.AppSettings["COPaWSUser"], ConfigurationSettings.AppSettings["COPaWSPassword"]);
                ws.Credentials = objCredential;
                string[] Mods = ws.GetLibraryModules();
                foreach (string mod in Mods)
                {
                    this.ddlModules.Items.Add(mod);
                }
                this.ddlModules.Text = this.ddlModules.Items[0].Text;
            }
            catch (Exception ex)
            {
                this.lbMessage.Text = "Open the COPa Web Service with error. Please contact with COPa Library!";
            }
        }
    }
    DataTable SearchResult ;
    protected void btUpload_Click(object sender, EventArgs e)
    {
     
        if (FileUpload1.HasFile)
        {
            //savePath += FileUpload1.FileName;
            //FileUpload1.SaveAs(savePath);
           
            DisplayFileContents(FileUpload1.PostedFile);
            lbMessage.Text = "You file was uploaded successfully.";
        }
        else
        { lbMessage.Text = "You did not specifiy a file to upload.";}
    }
   static string SessionID = "";
   static string IndexID = "";
    void DisplayFileContents(HttpPostedFile file)
    {
        System.IO.Stream myStream;
        Int32 fileLen;
        StringBuilder displayString = new StringBuilder();
        fileLen = file.ContentLength;
        Byte[] Input = new Byte[fileLen];
        myStream = FileUpload1.FileContent;
        myStream.Read(Input, 0, fileLen);
        for (int i = 0; i < fileLen; i++)
        {
            displayString.Append((char)Input[i]); 
        }
        tbDTA.Text = displayString.ToString();
        SessionID = DateTime.Now.ToLongTimeString();
        Session[SessionID] = displayString.ToString();
        //get the precursor mz from dta files
        if (tbDTA.Text.StartsWith("S"))
        {
            string firstline = tbDTA.Text.Substring(0, tbDTA.Text.IndexOf("\r\n") - 1);
            string Precursormz = firstline.Substring(firstline.LastIndexOf("\t") + 1);
            this.tbPrecursorMZ.Text = Precursormz;
        }
    }

    struct MZintensitiy
    {
        public float mz;
        public float intensity;
    }

    static double NoiseCutoff = 2;
    Guid RequestId;
    protected void btSearch_Click(object sender, EventArgs e)
    {
        //ThreadStart threadDelegate = new ThreadStart(DoSearch);
        //Thread newThread = new Thread(threadDelegate);
        //newThread.Start();
        //DoSearch();
        DoSearchInWS();
       // RequestId = Guid.NewGuid();
                       
    }

    private void DoSearchInWS()
    {
        double[] NormalizedSpectrum = new double[4096];
        string[] lines = this.tbDTA.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
        {
            lbMessage.Text = "Please input spectrum data.";
            return;
        }
        SessionID = DateTime.Now.ToString("yyyy_MM_dd") ;
        IndexID = DateTime.Now.ToString("fffffff");
        

      //  string UploadPath = WebConfigurationManager.ConnectionStrings["UploadPath"].ConnectionString + string.Format ("WebDta\\{0}\\",SessionID );
      //  DirectoryInfo newpath = new DirectoryInfo(UploadPath);
      //  if (!newpath.Exists)
      //  {
      //      newpath.Create();
      //  }

        
      //string dtaFilename = UploadPath   + IndexID + ".dta";
      //try
      //{
      //    using (StreamWriter sw = new StreamWriter(dtaFilename, true))
      //    {
            foreach (string line in lines)
            {
                //sw.WriteLine(line);
                if (char.IsDigit(line, 0))
                {
                    String [] sperator = line.Split (new string []{" ","\t"},StringSplitOptions.RemoveEmptyEntries );
                    string mz = sperator[0];
                    string intensity = sperator[1];
                    MZintensitiy mzi = new MZintensitiy();
                    try
                    {
                        mzi.mz = float.Parse(mz);
                        mzi.intensity = float.Parse(intensity);

                        if (mzi.mz > NoiseCutoff)
                        {
                            double dintensity = (double)mzi.intensity;
                            NormalizedSpectrum[(int)mzi.mz] += dintensity;
                        }
                        //SpectrumData.Add(mzi);
                    }
                    catch (Exception ex)
                    { }
                }

            }
      //      sw.Close();
      //    }
      //}
      //  catch (Exception ex)
      //{
      //      this.lbMessage.Text = string.Format ("Write dta file to server with error:{0} ",ex.ToString ());
      //  }
       

        //double TotalNormal = 0;
        //foreach (double mz in NormalizedSpectrum)
        //{
        //    TotalNormal += mz * mz;
        //}
        //TotalNormal = Math.Sqrt(TotalNormal);

        //for (int i = 0; i < 4096; i++)
        //{
        //    NormalizedSpectrum[i] = NormalizedSpectrum[i] / TotalNormal;
        //}
        
        int slideSize = int.Parse(this.ddlChargeState.Text);
        float PrecursorMZ = float.Parse(this.tbPrecursorMZ.Text);
        float mzTolerance = float.Parse(this.tbTolerance.Text);

        COPaWS.COPaWS webserv = new COPaWS.COPaWS();
        NetworkCredential objCredential = new NetworkCredential(ConfigurationSettings.AppSettings["COPaWSUser"], ConfigurationSettings.AppSettings["COPaWSPassword"]);
        webserv.Credentials = objCredential;
        COPaWS.SearchingCondition sc = new COPaWS.SearchingCondition ();
        sc.bUseNoiseLibrary = true ;
        sc.bStatisticSearching = false ;
        sc.bHighResolution = false;
        sc.fPrecursorWindow = mzTolerance ;
        sc.iSlideSize = slideSize  ;
        COPaWS.SearchingThreadPair tp = new COPaWS.SearchingThreadPair() ;
        tp.DetaDecoyScore = float.Parse (this.tbCutoff.Text );
        tp.MatchScore = 0.0F;
        sc.NormalSearchCondition = new COPaWS.SearchingThreadPair[] {tp} ;
        COPaWS.Module lib_mod = COPaWS.Module.human_heart_mitochondria ;
        if (this.ddlModules.Text != null)
        {
            try
            {
                lib_mod = (COPaWS.Module)(int.Parse(this.ddlModules.Text.Substring(0, this.ddlModules.Text.IndexOf("."))) - 1);
            }
            catch (Exception ex)
            {
                lbMessage.Text = "Could not recognize the library module!";
                return;
            }
        }
        COPaWS.Module[] Modules = new COPaWS.Module[] { lib_mod };
        COPaWS.MSPepetideInfo[] results = webserv.SearchDTA2("WebDta", SessionID, IndexID, NormalizedSpectrum, PrecursorMZ, Modules, sc, 2);
        if (results != null)
        {
            string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}' Target='_blank'>{1}</a>";
            string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=RUN&TaskID={2}&File={3}' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
            foreach (COPaWS.MSPepetideInfo mspep in results)
            {
                TableRow trResult = new TableRow();

                TableCell tcPepID = new TableCell();
                string strPepID = mspep.COPaPeptideID;
                tcPepID.Text = string.Format(PepIDURL, strPepID, strPepID);
                tcPepID.HorizontalAlign = HorizontalAlign.Left;
                trResult.Cells.Add(tcPepID);

                TableCell tcPepSeq = new TableCell();
                string PeptideSeq = mspep.PeptideSequence;
                //string ModifiedType = mspep.ModifiedType;
                tcPepSeq.Text = PeptideSeq;
                tcPepSeq.HorizontalAlign = HorizontalAlign.Left;
                trResult.Cells.Add(tcPepSeq);

                TableCell tcModifiedType = new TableCell();

                tcModifiedType.Text = "";// ModifiedType;
                tcModifiedType.HorizontalAlign = HorizontalAlign.Left;
                trResult.Cells.Add(tcModifiedType);

                //TableCell tcDotProduct = new TableCell();
                //double dp = double.Parse(foundRows[i].ItemArray[1].ToString());
                //tcDotProduct.Text = string.Format("{0:F3}", dp);
                //tcDotProduct.HorizontalAlign = HorizontalAlign.Right;
                //trResult.Cells.Add(tcDotProduct);

                //TableCell tcDotBias = new TableCell();
                //double db = double.Parse(foundRows[i].ItemArray[2].ToString());
                //tcDotBias.Text = string.Format("{0:F3}", db);
                //tcDotBias.HorizontalAlign = HorizontalAlign.Right;
                //trResult.Cells.Add(tcDotBias);

                TableCell tcFscore = new TableCell();
                double finalScore = double.Parse(mspep.DecoyScore);// double.Parse(foundRows[i].ItemArray[3].ToString());            
                tcFscore.Text = string.Format("{0:F3}", finalScore);
                tcFscore.HorizontalAlign = HorizontalAlign.Right;
                trResult.Cells.Add(tcFscore);

                TableCell tcSpectrum = new TableCell();
                tcSpectrum.Text = string.Format(SpectrumURL, mspep.LibSpectraSeq, IndexID, "WebDta", SessionID);
                tcSpectrum.HorizontalAlign = HorizontalAlign.Center;
                trResult.Cells.Add(tcSpectrum);

                tbPeptides.Rows.Add(trResult);
            }
            if (results.Length > 0)
                lbMessage.Text = string.Format(" {0} spectra matched!", results.Length);
            else
                lbMessage.Text = string.Format("no spectrum was matched under your criteria, you could adjust the cutoff and try again!");
        }
        else
            lbMessage.Text = string.Format("no spectrum was matched under your criteria, you could adjust the cutoff and try again!");

    }

    //public delegate void ProgressHandler(object sender, EventArgs e);
    //public event ProgressHandler Progress;


    //private void DoSearch()
    //{
    //    ProcessingStatus.add(RequestId, 0);

    //    //load query spectral
    //    //ArrayList SpectrumData = new ArrayList();
    //    double[] NormalizedSpectrum = new double[4096];
    //    string[] lines = this.tbDTA.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    //    if (lines.Length == 0)
    //    {
    //        lbMessage.Text = "Please input spectrum data.";
    //        return;
    //    }
    //    SessionID = DateTime.Now.ToLongTimeString();
    //    Session[SessionID] = tbDTA.Text;

    //    foreach (string line in lines)
    //    {
    //        if (char.IsDigit(line, 0))
    //        {
    //            int sperator = line.IndexOf(" ");
    //            string mz = line.Substring(0, sperator);
    //            string intensity = line.Substring(sperator);
    //            MZintensitiy mzi = new MZintensitiy();
    //            try
    //            {
    //                mzi.mz = float.Parse(mz);
    //                mzi.intensity = float.Parse(intensity);

    //                if (mzi.mz > NoiseCutoff)
    //                {
    //                    double dintensity = Math.Sqrt((double)mzi.intensity);
    //                    NormalizedSpectrum[(int)mzi.mz] += dintensity;
    //                }
    //                //SpectrumData.Add(mzi);
    //            }
    //            catch (Exception ex)
    //            { }
    //        }
           
    //    }

    //    double TotalNormal = 0;
    //    foreach (double mz in NormalizedSpectrum)
    //    {
    //        TotalNormal += mz * mz;
    //    }
    //    TotalNormal = Math.Sqrt(TotalNormal);

    //    for (int i = 0; i < 4096; i++)
    //    {
    //        NormalizedSpectrum[i] = NormalizedSpectrum[i] / TotalNormal;
    //    }

    //    //load library
    //    string strSQL = "select peptide_cop_id,file_index,spectrum_seq, file_path from spectrum_tbl where precur_mz > {0} and precur_mz < {1} and Charge_State={2} and file_path= '{3}'";
    //    string UnknownChargeSQL = "select peptide_cop_id,file_index,spectrum_seq, file_path from spectrum_tbl where precur_mz > {0} and precur_mz < {1} and file_path='{2}'";
    //    string strSQL2 = "select peptide_cop_id,file_index,spectrum_seq, file_path from spectrum_tbl where Charge_State={0} and file_path='{1}' ";
    //    string UnknownChargeSQL2 = "select peptide_cop_id,file_index,spectrum_seq, file_path from spectrum_tbl where file_path='{0}'";
    //    int Charge_state = 2;
    //    float PrecursorMZ;
    //    float mzTolerance;
    //    bool bUnknownCharge = false;
    //    string file_path = "MOUSE\\";
    //    if (this.ddlModules.SelectedIndex == 1)
    //        file_path = "HUMAN\\";
    //    if (this.ddlChargeState.Text == "unknown")
    //        bUnknownCharge = true;
    //    try
    //    {
    //        if (!bUnknownCharge)
    //            Charge_state = int.Parse(this.ddlChargeState.Text);
    //        PrecursorMZ = float.Parse(this.tbPrecursorMZ.Text);
    //        mzTolerance = float.Parse(this.tbTolerance.Text);
    //    }
    //    catch (Exception ex)
    //    {
    //        lbMessage.Text = "invalid input, please check!";
    //        return;
    //    }
    //    string mzFileFolder = WebConfigurationManager.ConnectionStrings["NORMALIZESPECTRUM"].ConnectionString;
    //    if (PrecursorMZ == 0)
    //    {
    //        if (bUnknownCharge)
    //            strSQL = string.Format (UnknownChargeSQL2,file_path );
    //        else
    //            strSQL = string.Format(strSQL2, Charge_state,file_path );
    //    }
    //    else
    //    {
    //        if (bUnknownCharge)
    //            strSQL = string.Format(UnknownChargeSQL, PrecursorMZ - mzTolerance, PrecursorMZ + mzTolerance,file_path );
    //        else
    //            strSQL = string.Format(strSQL, PrecursorMZ - mzTolerance, PrecursorMZ + mzTolerance, Charge_state,file_path );
    //    }
    //    //DBInterface.ConnectDB();
    //    IDataReader result = DBInterface.QuerySQL(strSQL);

    //    SearchResult = new DataTable();
    //    SearchResult.Columns.Add("COPa Peptide ID");
    //    SearchResult.Columns.Add("Dot Product");
    //    SearchResult.Columns.Add("Dot Bias");
    //    SearchResult.Columns.Add("Match Score");
    //    SearchResult.Columns.Add("Spectrum");

    //    if (result != null)
    //    {

    //        while (result.Read())
    //        {
    //            //get the normalizedlibrary data
    //            string peptide_cop_id = result.GetString(0);
    //            string fileindex = string.Format("{0:D5}", result.GetInt32(1)); 
    //            string relative_path = result.GetString(3); 
    //            string mzfilepath = String.Format("{0}{1}.dta", mzFileFolder+relative_path , fileindex);
    //            string SpectrumSeq = result.GetInt32(2).ToString();
               
    //            double[] libSpectrum = LoadLibSpectrum(mzfilepath);
    //            if (libSpectrum != null)
    //            {
    //                double dp, db = 0;
    //                dp = DotProduct(NormalizedSpectrum, libSpectrum, 4096, ref db);
    //                if (dp > float.Parse(this.tbCutoff.Text))
    //                    SearchResult.Rows.Add(new object[] { peptide_cop_id, dp, db, 0, SpectrumSeq });
    //            }
    //            //if (dp > maxDP)
    //            //{
    //            //    MatchPeptide_ID = peptide_cop_id;
    //            //    maxDP = dp;
    //            //}
    //        }
    //    }
    //    DBInterface.CloseDB();

    //    if (SearchResult.Rows.Count == 0)
    //    {
    //        lbMessage.Text = "no spectrum was matched under your criteria, you could adjust the cutoff and try again!";
    //        return;
    //    }
    //    //SearchResult.DefaultView.Sort = "["+SearchResult.Columns[1].ColumnName+ "] desc";
    //    DataTable newTable = new DataTable();
    //    newTable.Columns.Add("COPa Peptide ID");
    //    newTable.Columns.Add("Dot Product");
    //    newTable.Columns.Add("Dot Bias");
    //    newTable.Columns.Add("Match Score");
    //    newTable.Columns.Add("Spectrum");
    //    DataRow[] foundRows = SearchResult.Select(null, SearchResult.Columns[1].ColumnName + " desc"); // Sort with Column name
    //    //first sort the table by dot product, and calculate the finalscore

    //    double firstDP = double.Parse(foundRows[0].ItemArray[1].ToString());
    //    double secondDP = 0;
    //    if (foundRows.GetUpperBound(0) > 1)
    //    {
    //        secondDP = double.Parse(foundRows[1].ItemArray[1].ToString());
    //    }
    //    for (int i = 0; i < SearchResult.Rows.Count; i++)
    //    {
    //        double DetaDP = 0.0;
    //        double dp = double.Parse(SearchResult.Rows[i].ItemArray[1].ToString());
    //        if (dp == firstDP)
    //            DetaDP = firstDP - secondDP;
    //        else
    //            DetaDP = dp - firstDP;

    //        double db = double.Parse(SearchResult.Rows[i].ItemArray[2].ToString());
    //        double penality = 0;
    //        if (db < 0.1)
    //            penality = 0.12;
    //        if (db > 0.35 && db <= 0.4)
    //            penality = 0.12;
    //        if (db > 0.4 && db < 0.45)
    //            penality = 0.18;
    //        if (db > 0.45)
    //            penality = 0.45;
    //        double finalScore = 0.6 * dp + 0.4 * DetaDP - penality;
    //        SearchResult.Rows[i]["Match Score"] = finalScore;
    //        if (finalScore > float.Parse(this.tbCutoff.Text))
    //            newTable.Rows.Add(new object[] { SearchResult.Rows[i].ItemArray[0], SearchResult.Rows[i].ItemArray[1], SearchResult.Rows[i].ItemArray[2], SearchResult.Rows[i].ItemArray[3], SearchResult.Rows[i].ItemArray[4] });
    //        ////update status
    //        //int percent = (i*100/SearchResult.Rows.Count);
    //        //if (Progress != null )
    //        //    Progress(this,new EventArgs ());
    //        //ProcessingStatus.update(RequestId, percent);
    //    }
    //    //sort the table by final score
    //    SearchResult.Clear();
    //    foundRows = null;
    //    foundRows = newTable.Select(null, newTable.Columns[3].ColumnName + " desc");

    //    string PepIDURL = "<a href='PeptideInfo.aspx?QType=Peptide+ID&QValue={0}' Target='_blank'>{1}</a>";
    //    string SpectrumURL = "<a href='MatchSpectrum.aspx?QValue={0}&SessionID={1}&ST=DTA' Target='_blank'><img src='_image/mirror_spectrum.gif' /></a>";
    //    for (int i = 0; i <= foundRows.GetUpperBound(0); i++)
    //    {
    //        TableRow trResult = new TableRow();

    //        TableCell tcPepID = new TableCell();
    //        string strPepID = foundRows[i].ItemArray[0].ToString();
    //        tcPepID.Text = string.Format(PepIDURL, strPepID, strPepID);
    //        tcPepID.HorizontalAlign = HorizontalAlign.Left;
    //        trResult.Cells.Add(tcPepID);

    //        TableCell tcPepSeq = new TableCell();
    //        string PeptideSeq = "";
    //        string ModifiedType = "";
    //        GetModifedPeptide (foundRows[i].ItemArray[4].ToString(),ref PeptideSeq, ref ModifiedType );
    //        tcPepSeq.Text = PeptideSeq;
    //        tcPepSeq.HorizontalAlign = HorizontalAlign.Left ;
    //        trResult.Cells.Add(tcPepSeq);

    //        TableCell tcModifiedType= new TableCell();
           
    //        tcModifiedType.Text = ModifiedType ;
    //        tcModifiedType.HorizontalAlign = HorizontalAlign.Left;
    //        trResult.Cells.Add(tcModifiedType);

    //        //TableCell tcDotProduct = new TableCell();
    //        //double dp = double.Parse(foundRows[i].ItemArray[1].ToString());
    //        //tcDotProduct.Text = string.Format("{0:F3}", dp);
    //        //tcDotProduct.HorizontalAlign = HorizontalAlign.Right;
    //        //trResult.Cells.Add(tcDotProduct);

    //        //TableCell tcDotBias = new TableCell();
    //        //double db = double.Parse(foundRows[i].ItemArray[2].ToString());
    //        //tcDotBias.Text = string.Format("{0:F3}", db);
    //        //tcDotBias.HorizontalAlign = HorizontalAlign.Right;
    //        //trResult.Cells.Add(tcDotBias);

    //        TableCell tcFscore = new TableCell();
    //        double finalScore = double.Parse(foundRows[i].ItemArray[3].ToString());
    //        if (finalScore < float.Parse(tbCutoff.Text))
    //            break;//when the sorted score is below the cutoff, then quit.
    //        tcFscore.Text = string.Format("{0:F3}", finalScore);
    //        tcFscore.HorizontalAlign = HorizontalAlign.Right;
    //        trResult.Cells.Add(tcFscore);

    //        TableCell tcSpectrum = new TableCell();
    //        tcSpectrum.Text = string.Format(SpectrumURL, foundRows[i].ItemArray[4].ToString(), SessionID);
    //        tcSpectrum.HorizontalAlign = HorizontalAlign.Center;
    //        trResult.Cells.Add(tcSpectrum);

    //        tbPeptides.Rows.Add(trResult);
    //    }
    //    if (foundRows.GetUpperBound(0) >= 1)
    //        lbMessage.Text = string.Format(" {0} spectra matched!", foundRows.GetUpperBound(0) + 1);
    //    else if (foundRows.GetUpperBound(0) == 0)
    //        lbMessage.Text = " 1 spectrum matched!";
    //    else
    //        lbMessage.Text = string.Format("no spectrum was matched under your criteria, you could adjust the cutoff and try again!");

    //}

    //string GetPeptideSequence(string PeptideID)
    //{
    //    String strSQL = string.Format("SELECT PEPTIDE_SEQUENCE FROM PEPTIDE_TBL WHERE PEPTIDE_COP_ID='{0}'",PeptideID );
    //    //DBInterface.ConnectDB();
    //    IDataReader result = DBInterface.QuerySQL(strSQL);

    //    if (result != null)
    //    {
    //        if (result.Read())
    //        {
    //            string PeptideSequence = result.GetString(0);
    //            return PeptideSequence;
    //        }
    //    }
    //    return "";

    //}

    //bool GetModifedPeptide(string PTMseq, ref string peptideSeq, ref string ModifiedType)
    //{
    //    string strSQL = string.Format(" select modified_sequence, type_of_modification from ptm_tbl t, spectrum_tbl t2 where t2.spectrum_seq = {0}  and t.ptm_seq = t2.ptm_seq ", PTMseq);
    //    //DBInterface.ConnectDB();
    //    IDataReader result = DBInterface.QuerySQL(strSQL);

    //    if (result != null)
    //    {
    //        if (result.Read())
    //        {
    //            peptideSeq = result.GetString(0);
    //            if (result.IsDBNull(1))
    //                ModifiedType = "";
    //            else
    //                ModifiedType = result.GetString(1);
    //            return true;
    //        }
    //    }
    //    return false ;
    //}

    //double[] LoadLibSpectrum(string FilePath)
    //{
    //    if (File.Exists(FilePath))
    //    {
    //        double[] libSpectrum = new double[4096]; 
    //        //multiuser-fridendly streamreader 
    //        FileStream fs = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
    //        StreamReader sr = new StreamReader(fs);
    //        string line;

    //        while ((line = sr.ReadLine()) != null)
    //        {
    //            if (char.IsDigit(line, 0))
    //            {
    //                int sperator = line.IndexOf(" ");
    //                string mz = line.Substring(0, sperator);
    //                string intensity = line.Substring(sperator);
    //                try
    //                {
    //                    libSpectrum[int.Parse(mz)] = double.Parse(intensity);
    //                }
    //                catch
    //                { }
    //            }
    //        }
    //        sr.Close();
    //        return libSpectrum;
    //    }
    //    else
    //        return null;
    //}

    //double DotProduct(double[] data1, double[] data2, int nLength, ref double DotBias)
    //{
    //    double result = 0;
    //    double dbResult = 0;
    //    for (int i = 0; i < nLength; i++)
    //    {
    //        result += data1[i] * data2[i];
    //        dbResult += data1[i] * data1[i] * data2[i] * data2[i];
    //    }

    //    dbResult = Math.Sqrt(dbResult) / result;
    //    DotBias = dbResult;
    //    return result;
    //}
    protected void btClearData_Click(object sender, EventArgs e)
    {
        this.tbDTA.Text = "";
        this.lbMessage.Text = "";
    }
    protected void LoadSample_Click(object sender, EventArgs e)
    {
        string SamplePath = WebConfigurationManager.ConnectionStrings["SampleData"].ConnectionString;
        SamplePath += ddlSamples.Text;
        if (File.Exists(SamplePath))
        {
             FileStream fs = File.Open(SamplePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
             StreamReader sr = new StreamReader(fs);
            string firstline =  sr.ReadLine();
            string precursormz = firstline.Substring(0, firstline.IndexOf (" ")-1);
            string charge = firstline.Substring(firstline.IndexOf (" ") + 1);
            this.tbPrecursorMZ.Text = precursormz;
            this.ddlChargeState.Text = charge;
            tbDTA.Text = sr.ReadToEnd ();
            //SessionID = DateTime.Now.ToLongTimeString();
            //Session[SessionID] = tbDTA.Text ;
            sr.Close();
        }
        lbMessage.Text = "";

    }
}
