using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
//using ZJU.COPDB;
//using System.Data.SqlClient;
using System.Text.RegularExpressions;
//using System.Data;

namespace ZJU.COPLib
{
    
   
    /// <summary>
    /// Summary description for PeptideMW
    /// </summary>
    public class PeptideMW
    {

        public PeptideMW(string PeptideSequence)
        {
            LoadAAResidueValues();
            mPeptideSequence = ValidePepSeq ( PeptideSequence);
            mModifiedSequence = PeptideSequence;
        }
        string mPeptideSequence = "";
        string mModifiedSequence = "";

        public bool IsPhosphorylation()
        {
            if (mModifiedSequence.Contains("(79."))
                return true;
            else return false;
        }
        Hashtable AARV   ;

        private string ValidePepSeq(string Src_seq)
        {
            string newSeq = "";
            Regex reg = new Regex("[ACDEFGHIKLMNPQRSTVWY]");
            MatchCollection matchMade = reg.Matches(Src_seq);
            for (int i = 0; i < matchMade.Count; i++)
            {
                newSeq += matchMade[i].Value;
            }

            return newSeq;
        }

        //A	71.03711
        //C	103.00919
        //D	115.02694
        //E	129.04259
        //F	147.06841
        //G	57.02146
        //H	137.05891
        //I	113.08406
        //K	128.09496
        //L	113.08406
        //M	131.04048
        //N	114.04293
        //P	97.05276
        //Q	128.05858
        //R	156.10111
        //S	87.03202
        //T	101.04768
        //V	99.06841
        //W	186.07931
        //Y	163.06333
       private void LoadAAResidueValues()
       {
            AARV = new Hashtable();
            AARV.Add("A", 71.03711F);           
            AARV.Add ("C",	103.00919F);
            AARV.Add ("D",	115.02694F);
            AARV.Add ("E",	129.04259F);
            AARV.Add ("F",	147.06841F);
            AARV.Add ("G",	57.02146F);
            AARV.Add ("H",	137.05891F);
            AARV.Add ("I",	113.08406F);
            AARV.Add ("K",	128.09496F);
            AARV.Add ("L",	113.08406F);
            AARV.Add ("M",	131.04048F);
            AARV.Add ("N",	114.04293F);
            AARV.Add ("P",	97.05276F);
            AARV.Add ("Q",	128.05858F);
            AARV.Add ("R",	156.10111F);
            AARV.Add ("S",	87.03202F);
            AARV.Add ("T",	101.04768F);
            AARV.Add ("V",	99.06841F);
            AARV.Add ("W",	186.07931F);
            AARV.Add("Y", 163.06333F);
           //string strSQL = string.Format("select AACode,MONOMW from cop_webuser.aa_tbl");
           ////DBInterface.ConnectDB();
           //DataSet  result = DBInterface.QuerySQL2(strSQL);
           //if (result != null)
           //{
           //    for (int i = 0; i < result.Tables[0].Rows.Count;i++)
           //    {
           //        string AACode = result.Tables[0].Rows[i].ItemArray[0].ToString ();
           //        float MV = float.Parse (result.Tables[0].Rows[i].ItemArray[1].ToString ());
           //        AARV.Add(AACode, MV);
           //    }
           //    //result.Close();
           //}

           //DBInterface.CloseDB();
       }

       public float GetPrecurMz(int charge)
       {
           if (mPeptideSequence.Length == 0)
               return 0;
           int SequenceLength = mPeptideSequence.Length;
           int i = 0;
           float PreResult = 19.00F;
           for (i = 0; i < SequenceLength; i++)
           {
               float ModifedWeight = 0.0F;
               HadModifed(i, ref ModifedWeight);

               PreResult = PreResult + (float)AARV[mPeptideSequence.Substring(i, 1)] + ModifedWeight;

           }
           return (PreResult + charge - 1) / (float)charge;
       }

       public float getPrecurMz2(int charge)
       {
           if (mPeptideSequence.Length == 0)
               return 0;
           int SequenceLength = mPeptideSequence.Length;
           int i = 0;
           float PreResult = 19.00F + getModify();
           for (i = 0; i < SequenceLength; i++)
           {
               PreResult = PreResult + (float)AARV[mPeptideSequence.Substring(i, 1)];

           }
           return (PreResult + charge - 1) / (float)charge;
       }

       public float GetMW2()
       {
           if (mPeptideSequence.Length == 0)
               return 0;
           int SequenceLength = mPeptideSequence.Length;
           int i = 0;
           float PreResult = 18.0152F;// +getModify();
           for (i = 0; i < SequenceLength; i++)
           {


               PreResult = PreResult + (float)AARV[mPeptideSequence.Substring(i, 1)];

           }

           return PreResult;
       }

       public float getModify()
       {
           string newSeq = "";
           Regex reg = new Regex("[-+]?([0-9]*\\.[0-9]+|[0-9]+)");
           MatchCollection matchMade = reg.Matches(mModifiedSequence);
           float Modvalue = 0F;
           for (int i = 0; i < matchMade.Count; i++)
           {
               Modvalue += float.Parse(matchMade[i].Value);
           }

           return Modvalue;
       }
       public float GetMW()
       {
           if (mPeptideSequence.Length == 0)
               return 0;
           int SequenceLength = mPeptideSequence.Length;
           int i = 0;
           float PreResult = 18.0152F;
           for (i = 0; i < SequenceLength; i++)
           {
               float ModifedWeight = 0.0F;
               HadModifed(i, ref ModifedWeight);

               PreResult = PreResult + (float)AARV[mPeptideSequence.Substring(i, 1)] + ModifedWeight;

           }
           return PreResult;
       }
       public float[] GetPepFragmentBValues()
       {
           if (mPeptideSequence.Length == 0)
               return null;
           int SequenceLength = mPeptideSequence.Length;
           float[] Bvalues = new float[SequenceLength - 1];
           int i = 0;
           float PreResult = 1.00F;
           float ntermMod = 0;
           HadModifed2(0, ref ntermMod);
           PreResult += ntermMod;
           for (i = 0; i < SequenceLength - 1; i++)
           {
               float ModifedWeight = 0.0F;
               HadModifed2(i + 1, ref ModifedWeight);

               Bvalues[i] = PreResult + (float)AARV[mPeptideSequence.Substring(i, 1)] + ModifedWeight;
               PreResult = Bvalues[i];
           }
           return Bvalues;
       }
       bool HadModifed2(int site, ref float ModifiedWeight)
       {
           Regex reg = new Regex("[ACDEFGHIKLMNPQRSTVWY]");
           int iCurrent = 0;
           int iSite = 1;
           if (site == 0)
           {
               if (mModifiedSequence.StartsWith("("))
               {
                   string mod = mModifiedSequence.Substring(1, mModifiedSequence.IndexOf(")") - 1);
                   ModifiedWeight = float.Parse(mod);
                   return true;
               }
               return false;
           }
           while (true)
           {
               if (reg.IsMatch(mModifiedSequence.Substring(iCurrent, 1)))
               {

                   if (iSite == site)
                   {
                       if (reg.IsMatch(mModifiedSequence.Substring(iCurrent + 1, 1)))
                       {
                           return false;
                       }
                       else
                       {
                           string mod = mModifiedSequence.Substring(iCurrent + 2, mModifiedSequence.IndexOf(")", iCurrent) - iCurrent - 2);
                           ModifiedWeight = float.Parse(mod);
                           return true;
                       }
                   }
                   iSite++;
               }

               iCurrent++;
               if (iCurrent >= mModifiedSequence.Length - 1)
                   break;

           }
           return false;

       }

       bool HadModifed(int site, ref float ModifiedWeight)
       {
           Regex reg = new Regex("[ACDEFGHIKLMNPQRSTVWY]");
           int iCurrent = 0;
           int iSite = 0;
           while (true)
           {
               if (reg.IsMatch(mModifiedSequence.Substring(iCurrent, 1)))
               {

                   if (iSite == site)
                   {
                       if (reg.IsMatch(mModifiedSequence.Substring(iCurrent + 1, 1)))
                       {
                           return false;
                       }
                       else
                       {
                           if (mModifiedSequence.Substring(iCurrent + 1, 1) == "*" && iCurrent == 0 && mModifiedSequence.Substring(iCurrent, 1) != "M")
                           {
                               ModifiedWeight = 42.01F;
                               return true;
                           }

                           switch (mModifiedSequence.Substring(iCurrent, 1))
                           {
                               case "M":
                                   ModifiedWeight = 15.99F;
                                   break;
                               case "C":
                                   ModifiedWeight = 57.02F;
                                   break;
                               case "S":
                               case "T":
                               case "Y":
                                   ModifiedWeight = 79.96F;
                                   break;
                           }

                           return true;
                       }
                   }
                   iSite++;
               }
               iCurrent++;
               if (iCurrent >= mModifiedSequence.Length - 1)
                   break;

           }
           return false;

       }

       public float[] GetPepFragmentYValues()
       {
           if (mPeptideSequence.Length == 0)
               return null;
           int SequenceLength = mPeptideSequence.Length;
           float[] Yvalues = new float[SequenceLength - 1];
           int i = 0;
           float PreResult = 19.0F;
           for (i = 0; i < SequenceLength - 1; i++)
           {
               float modifedweight = 0.0F;
               HadModifed2(SequenceLength - i, ref modifedweight);
               Yvalues[i] = PreResult + (float)AARV[mPeptideSequence.Substring(SequenceLength - 1 - i, 1)] + modifedweight;
               PreResult = Yvalues[i];
           }
           return Yvalues;
       }
    }
}
