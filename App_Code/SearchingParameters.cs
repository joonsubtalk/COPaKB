using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// Summary description for SearchingParameters
/// </summary>
namespace ZJU.COPLib
{
    public class SearchingParameters
    {
        private string ParameterFile = "";
        public SearchingParameters(string FileLocation)
        {
            if (LoadSettings(FileLocation))
                ParameterFile = FileLocation;

        }

        public SearchingParameters()
        {
            ParameterFile = "";
        }
        private bool SettingChanged;

        private bool _UseNoiseLibrary;

        public bool UseNoiseLibrary
        {
            get { return _UseNoiseLibrary; }
            set
            {
                if (value != _UseNoiseLibrary)
                {
                    _UseNoiseLibrary = value;
                    SettingChanged = true;
                }

            }
        }

        private string _ScoreCriteria;

        public string ScoreCriteria
        {
            get { return _ScoreCriteria; }
            set
            {
                if (value != _ScoreCriteria)
                {
                    _ScoreCriteria = value;
                    SettingChanged = true;
                }
            }
        }

        private bool _bOnlyTop1;
        public bool OnlyTop1
        {
            get { return _bOnlyTop1; }
            set
            {
                if (value != _bOnlyTop1)
                {
                    _bOnlyTop1 = value;
                    SettingChanged = true;
                }
            }

        }



        private bool _UseStatisticalMode;
        public bool UseStatisticalMode
        {
            get { return _UseStatisticalMode; }
            set
            {
                if (value != _UseStatisticalMode)
                {
                    _UseStatisticalMode = value;
                    SettingChanged = true;

                }
            }
        }

        private float _DefaultPossibility;

        public float ConfidenceLevel
        {
            get { return _DefaultPossibility; }
            set
            {
                if (value != _DefaultPossibility)
                {
                    _DefaultPossibility = value;
                    SettingChanged = true;
                }
            }
        }

        private int _SlideSize;
        public int SlideSize
        {
            get { return _SlideSize; }
            set
            {
                if (value != _SlideSize)
                {
                    _SlideSize = value;
                    SettingChanged = true;
                }
            }
        }

        private float _PeptideTolerance;
        public float Peptidetolerance
        {
            get { return _PeptideTolerance; }
            set
            {
                if (value != _PeptideTolerance)
                {
                    _PeptideTolerance = value;
                    SettingChanged = true;
                }
            }
        }

        private bool _HighResolution;
        public bool HighResolution
        {
            get { return _HighResolution; }
            set
            {
                if (value != _HighResolution)
                {
                    _HighResolution = value;
                    SettingChanged = true;
                }
            }
        }

        private float _PeptideToleranceHR;
        public float PeptideToleranceHR
        {
            get { return _PeptideToleranceHR; }
            set
            {
                if (value != _PeptideToleranceHR)
                {
                    _PeptideToleranceHR = value;
                    SettingChanged = true;
                }
            }
        }

        private int _IsotopePeaks;
        public int IsotopePeaks
        {
            get { return _IsotopePeaks; }
            set
            {
                if (value != _IsotopePeaks)
                {
                    _IsotopePeaks = value;
                    SettingChanged = true;
                }
            }
        }

        private bool _BonusDetaMz;
        public bool BonusDetaMz
        {
            get { return _BonusDetaMz; }
            set
            {
                if (value != _BonusDetaMz)
                {
                    _BonusDetaMz = value;
                    SettingChanged = true;
                }
            }
        }

        private string _LibraryModule;

        public string LibraryModule
        {
            get { return _LibraryModule; }
            set
            {
                if (value != _LibraryModule)
                {
                    _LibraryModule = value;
                    SettingChanged = true;
                }
            }
        }
        private float _PTMShift;

        public float PTMShift
        {
            get { return _PTMShift; }
            set
            {
                if (value != _PTMShift)
                {
                    _PTMShift = value;
                    SettingChanged = true;
                }
            }
        }



        private int _DistinctPeptide;
        public int DistinctPeptide
        {
            get { return _DistinctPeptide; }
            set
            {
                if (value != _DistinctPeptide)
                {
                    _DistinctPeptide = value;
                    SettingChanged = true;
                }
            }
        }

        private int _UniquePeptide;
        public int UniquePeptide
        {
            get { return _UniquePeptide; }
            set
            {
                if (value != _UniquePeptide)
                {
                    _UniquePeptide = value;
                    SettingChanged = true;
                }
            }
        }

        private bool _bUsePTMModule;
        public bool UsePTMModule
        {
            get { return _bUsePTMModule; }
            set
            {
                if (value != _bUsePTMModule)
                {
                    _bUsePTMModule = value;
                    SettingChanged = true;
                }
            }
        }

        private string _PTMType;
        public string PTMType
        {
            get { return _PTMType; }
            set
            {
                if (value != _PTMType)
                {
                    _PTMType = value;
                    SettingChanged = true;
                }
            }
        }

        private bool _bCustomizedLibraryFDR;
        public bool CustomizedLibraryFDR
        {
            get { return _bCustomizedLibraryFDR; }
            set
            {
                if (value != _bCustomizedLibraryFDR)
                {
                    _bCustomizedLibraryFDR = value;
                    SettingChanged = true;
                }
            }
        }

        private float _LibraryFDR;
        public float LibraryFDR
        {
            get { return _LibraryFDR; }
            set
            {
                if (value != _LibraryFDR)
                {
                    _LibraryFDR = value;
                    SettingChanged = true;
                }
            }
        }

        public bool SaveSettings()
        {
            return SaveSettings(ParameterFile);
        }

        public bool SaveSettings(string FileLocation)
        {
            if (this.SettingChanged)
            {
                StreamWriter myWriter = null;
                XmlSerializer mySerializer = null;
                try
                {
                    mySerializer = new XmlSerializer(typeof(SearchingParameters));
                    myWriter = new StreamWriter(FileLocation, false);
                    mySerializer.Serialize(myWriter, this);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (myWriter != null)
                        myWriter.Close();
                }
            }
            return SettingChanged;
        }

        public bool LoadSettings(string FileLocation)
        {
            XmlSerializer mySerializer = null;
            FileStream myFileStream = null;
            bool fileExisits = false;
            try
            {
                mySerializer = new XmlSerializer(typeof(SearchingParameters));
                FileInfo fi = new FileInfo(FileLocation);
                if (fi.Exists)
                {
                    myFileStream = fi.OpenRead();
                    SearchingParameters mySetting = (SearchingParameters)mySerializer.Deserialize(myFileStream);
                    this._bOnlyTop1 = mySetting.OnlyTop1;
                    this._DefaultPossibility = mySetting.ConfidenceLevel;
                    this._DistinctPeptide = mySetting.DistinctPeptide;
                    this._LibraryModule = mySetting.LibraryModule;
                    this._PeptideTolerance = mySetting.Peptidetolerance;
                    this._PTMShift = mySetting.PTMShift;
                    this._ScoreCriteria = mySetting.ScoreCriteria;
                    this._SlideSize = mySetting.SlideSize;
                    this._UseNoiseLibrary = mySetting.UseNoiseLibrary;
                    this._UseStatisticalMode = mySetting.UseStatisticalMode;
                    this._PeptideToleranceHR = mySetting.PeptideToleranceHR;
                    this._IsotopePeaks = mySetting.IsotopePeaks;
                    this._HighResolution = mySetting.HighResolution;
                    this._UniquePeptide = mySetting.UniquePeptide;
                    this._bUsePTMModule = mySetting.UsePTMModule;
                    this._PTMType = mySetting.PTMType;
                    this._bCustomizedLibraryFDR = mySetting.CustomizedLibraryFDR;
                    this._LibraryFDR = mySetting.LibraryFDR;
                    this.SettingChanged = false;
                    fileExisits = true;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (myFileStream != null)
                    myFileStream.Close();
            }
            return fileExisits;
        }
    }
}
