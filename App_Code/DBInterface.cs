using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Data;
using System.Diagnostics ;
//using Oracle.DataAccess.Client;
//using MySql.Data.MySqlClient;

namespace ZJU.COPDB
{
    /// <summary>
    /// Summary description for DBInterface
    /// </summary>
    static public class DBInterface
    {
        public static void LogEvent(String Message, EventLogEntryType type)
        {
            String source = "COPaWebSite";
            String log = "Application";
            if (!EventLog.SourceExists(source))
            { EventLog.CreateEventSource(source, log); }
            EventLog eLog = new EventLog();
            eLog.Source = source;
            eLog.WriteEntry(Message, type);
        }

        public static String DbType  ="Oracle";
        static System.Data.Common.DbConnection DBConn;

        static public void ConnectDB()
        {
            if (DBConn != null)
            {
                if (DBConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        DBConn.Open();
                    }
                    catch (Exception ex)
                    {
                        //EventLog log = new EventLog();
                        //log.Source = "COPLibrary";
                        //log.WriteEntry(ex.Message , EventLogEntryType.Error);
                        LogEvent(ex.Message, EventLogEntryType.Error);
                    }
                }
                return;
            }
            if (WebConfigurationManager.ConnectionStrings["DBType"].ConnectionString == "Oracle")
            {
                DBConn = new OracleConnection();
                DbType = "Oracle";
            }
            //else if (WebConfigurationManager.ConnectionStrings["DBType"].ConnectionString == "MySql")
            //{
            //    DBConn = new MySql.Data.MySqlClient.MySqlConnection();
            //    DbType = "MySql";
            //}
            else
            {
                DBConn = new SqlConnection();
                DbType = "SQLServer";
            }
            if(DBConn.ConnectionString != null)
            DBConn.ConnectionString = WebConfigurationManager.ConnectionStrings["COP"].ConnectionString;
            //if (DBConn.State == ConnectionState.Open)
            //    DBConn.Close();
            try
            {
                DBConn.Open();
            }
            catch (Exception ex)
            {
                //EventLog log = new EventLog();
                //log.Source = "COPLibrary";
                //log.WriteEntry(ex.Message , EventLogEntryType.Error);
                LogEvent(ex.Message, EventLogEntryType.Error);
            }

        }
        static public void CloseDB()
        {
            if (DBConn.State == ConnectionState.Open)
                DBConn.Close();
        }

        static public DataSet QuerySQL2(string strSQL)
        {
            ConnectDB(); 
            DataSet ResultDataSet = new DataSet();
            try
            {
                System.Data.Common.DataAdapter sqlAdapter;
                if (DbType == "Oracle")
                    sqlAdapter = new OracleDataAdapter(strSQL, (OracleConnection)DBConn);
                //else if (DbType == "MySql")
                //    sqlAdapter = new MySqlDataAdapter(strSQL, (MySqlConnection)DBConn);
                else
                    sqlAdapter = new SqlDataAdapter(strSQL, (SqlConnection)DBConn);
               

                int i = sqlAdapter.Fill(ResultDataSet);
            }
            catch (Exception ex)
            {
                //EventLog log = new EventLog();
                //log.Source = "COPa Library";
                //log.WriteEntry(ex.Message, EventLogEntryType.Error);
                LogEvent(ex.Message + strSQL, EventLogEntryType.Error);
                return null;
            }
            finally
            {
                CloseDB();
            }
            return ResultDataSet;
        }

        static public IDataReader  QuerySQL(string strSQL)
        {
            ConnectDB();
            IDataReader ResultReader;
            try
            {
                System.Data.Common.DbCommand sqlCom;
                if (DbType == "Oracle")
                    sqlCom = new OracleCommand(strSQL, (OracleConnection)DBConn);
                //else if (DbType == "MySql")
                //    sqlCom = new MySqlCommand(strSQL, (MySqlConnection)DBConn);
                else
                    sqlCom = new SqlCommand(strSQL, (SqlConnection)DBConn);
                

                ResultReader = sqlCom.ExecuteReader();
            }
            catch (Exception ex)
            {
                //EventLog log = new EventLog();
                //log.Source = "COPa Library";
                //log.WriteEntry(ex.Message, EventLogEntryType.Error);
                LogEvent(ex.Message + strSQL, EventLogEntryType.Error);
                return null;
            }
            
            return ResultReader;

        }

        

        static public int UpdateSQL(string strSQL)
        {
            ConnectDB();
            try
            {
                System.Data.Common.DbCommand sqlCom;
                if (DbType == "Oracle")
                    sqlCom = new OracleCommand(strSQL, (OracleConnection)DBConn);
                //else if (DbType == "MySql")
                //    sqlCom = new MySqlCommand(strSQL, (MySqlConnection)DBConn);
                else
                    sqlCom = new SqlCommand(strSQL, (SqlConnection)DBConn);


                sqlCom.CommandText = strSQL;
                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //EventLog log = new EventLog();
                //log.Source = "COPa Library";
                //log.WriteEntry(ex.Message + strSQL, EventLogEntryType.Error);
                LogEvent(ex.Message + strSQL, EventLogEntryType.Error);
                return -1;
            }
            finally
            { CloseDB(); }

        }

        static public string SQLValidString(string strSrc)
        {
            string result;
            if (DbType == "Oralce")
            {
                result = strSrc.Replace("'", "''");
                result = result.Replace("%", "\\%");
            }
            else if (DbType == "MySql")
            {
                result = strSrc.Replace("\\", "\\\\");
                result = result.Replace("'", "\\'");
                result = result.Replace("%", "\\%");
                result = result.Replace("\"", "\\\"");
                result = result.Replace("_", "\\_");
            }
            else
                result = strSrc;
            return result;
        }

        static public bool UpdateSpectrum(string SpectraID, string taskID, string mzFile, string dtafile, string dta)
        {
            ConnectDB();
            System.Data.Common.DbCommand sqlCom;
            if (DbType == "Oracle")
                sqlCom = new OracleCommand("UpdateSpectra", (OracleConnection)DBConn);
            else
                sqlCom = new SqlCommand("UpdateSpectra", (SqlConnection)DBConn);
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new OracleParameter("spectraid", int.Parse (SpectraID)));
                sqlCom.Parameters.Add(new OracleParameter("taskid", int.Parse (taskID)));
                sqlCom.Parameters.Add(new OracleParameter("pmzfile", mzFile));
                sqlCom.Parameters.Add(new OracleParameter("pdtafile", dtafile));
                sqlCom.Parameters.Add(new OracleParameter("dtacontent", dta));
                sqlCom.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            { CloseDB(); }
        }
    }
}
