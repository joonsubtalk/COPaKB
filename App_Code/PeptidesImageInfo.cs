using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using ZJU.COPDB;

namespace ZJU.COPLib
{
    /// <summary>
    /// Summary description for PeptidesImageInfo
    /// </summary>
    public class PeptidesImageInfo
    {
        
        public const int WIDTH = 600;
        public const int NETWIDTH = 580;
        public const int MARGIN = 5;
        public const int AXISY = 20;
        public int BIGSEG = 100;
        public int SMALLSEG = 10;
        public const int PEPROWHEIGHT = 12;
        public int Height = 80;
        public float UNIT = 1.0F;
        public int HOTSPOTMARGIN = 3;
        public int RIGHTHOTSPOTMARGIN = 50;

        public PeptidesImageInfo(string PID, int Length, int Number)
        {
            //
            // TODO: Add constructor logic here
            //
            ProteinID = PID;
            SequenceLength = Length;
            TotalPeptides = Number;

        }

        private string _proteinID;
        public string ProteinID
        {
            get
            { return _proteinID; }
            set
            { _proteinID = value; }
        }

        private int _sequenceLength;
        public int SequenceLength
        {
            get
            { return _sequenceLength; }
            set
            { _sequenceLength = value; }
        }

        private int _totalPeptides;
        public int TotalPeptides
        {
            get
            { return _totalPeptides; }
            set
            { _totalPeptides = value; }
        }



        public struct PepRectInfo
        {
            public string PepID;
            public int Location;
            public int Length;
            public int top;
            public int bottom;
            public int left;
            public int right;
            public string Sequence;
            public bool bUnique;
        }

        public ArrayList ComputeTheValues(ref int TotalHeight)
        {
            if (SequenceLength >= 2000 && SequenceLength < 5000)
            {
                BIGSEG = 500;
                SMALLSEG = 50;
            }
            else if (SequenceLength > 5000 && SequenceLength < 20000)
            {
                BIGSEG = 1000;
                SMALLSEG = 100;
            }
            else if (SequenceLength >= 20000)
            {
                BIGSEG = 5000;
                SMALLSEG = 500;
            }
            else
            {
                BIGSEG = 100;
                SMALLSEG = 10;
            }

            //Height += TotalPeptides * PEPROWHEIGHT;

            int EndLength = ((SequenceLength / SMALLSEG) + 1) * SMALLSEG;
            int smallStepNumber = EndLength / SMALLSEG;
            float smallstep = NETWIDTH / (float)smallStepNumber;
            UNIT = smallstep / SMALLSEG;

            string strSQL = string.Format("select a.pp_relation_seq,a.location,b.peptide_sequence,b.sequence_length,b.peptide_sequence,a.unique_peptide from pp_relation_tbl a, peptide_tbl b where b.peptide_sequence= a.peptide_sequence and a.location <> -1 and a.protein_id='{0}' order by a.location", ProteinID);
            //DBInterface.ConnectDB();
            DataSet   result = DBInterface.QuerySQL2(strSQL);
            int i = 0;
            ArrayList PepRectArray = new ArrayList();
            int MaxRight = 0;
            int row = 0;
            int MaxRow = 1;
            if (result != null)
            {
                for (int j=0;j<result.Tables[0].Rows.Count ;j++)
                {
                    int location = int.Parse (result.Tables[0].Rows[j].ItemArray[1].ToString ()) ;// GetInt64(1);
                    string pepid = result.Tables[0].Rows[j].ItemArray[2].ToString ();
                    int length = int.Parse (result.Tables[0].Rows[j].ItemArray[3].ToString ());

                    if (location >= MaxRight)
                    {
                        row = 0;
                    }

                    PepRectInfo pri = new PepRectInfo();
                    pri.Sequence = result.Tables[0].Rows[j].ItemArray[4].ToString ();
                    pri.PepID = pepid;
                    pri.Location = location;
                    pri.Length = length;
                    pri.left = (int)(location * UNIT + MARGIN) ;
                    pri.top = row * PEPROWHEIGHT + AXISY + 30;
                    pri.bottom = pri.top + (int)(PEPROWHEIGHT / 2);
                    pri.right = MARGIN + (int)((length+location ) * UNIT) ;
                    if (pri.right - pri.left < 2)
                        pri.right = pri.left + 2;
                    if (result.Tables[0].Rows[j].ItemArray[5].ToString()  =="0")
                        pri.bUnique = false;
                    else
                        pri.bUnique = true;
                    PepRectArray.Add(pri);
                    i += 1;               
                   
                    row += 1;
                    if (row > MaxRow)
                    {
                        MaxRow = row;
                    }
                    if (location + length > MaxRight)
                    {
                        MaxRight = location + length;
                    }
                    
                }
                
            }
           
            //DBInterface.CloseDB();
            TotalHeight = Height + MaxRow * PEPROWHEIGHT;
            return PepRectArray;
        }

    

      
    }
}