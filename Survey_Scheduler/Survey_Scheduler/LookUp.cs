using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using DBHandler;
using System.IO;

namespace Survey_Scheduler
{
    public static class Survey_LookUp
    {
        /// <summary>
        /// Get details for reminder
        /// </summary>
        /// <returns></returns>
        public static DataTable FetchDataForReminder_start()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "Survey_USPSchGetReminderData1", null).Tables[0];
                return dtResult;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oConn.Close();
                oConn = null;
            }
        }
        /// <summary>
        /// Get Reminder 1 details
        /// </summary>
        /// <returns></returns>
        public static DataTable FetchDataForReminder1()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "Survey_USPSchGetReminderData1", null).Tables[0];
                return dtResult;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oConn.Close();
                oConn = null;
            }
        }
        /// <summary>
        /// Get Reminder 2 details
        /// </summary>
        /// <returns></returns>
        public static DataTable FetchDataForReminder2()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "Survey_USPSchGetReminderData2", null).Tables[0];
                return dtResult;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oConn.Close();
                oConn = null;
            }
        }
        /// <summary>
        /// Get Reminder 3 details
        /// </summary>
        /// <returns></returns>
        public static DataTable FetchDataForReminder3()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "Survey_USPSchGetReminderData3", null).Tables[0];
                return dtResult;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oConn.Close();
                oConn = null;
            }
        }
        /// <summary>
        /// Insert Reminder details
        /// </summary>
        /// <returns></returns>
        public static int InsertReminderData(int Type, int AccountId, string AccountName, int ParticipantId, string ParticipantName,  int ProjectId, string ProjectName, int ProgrammeId, string ProgrammeName, DateTime EmailDate, bool EmailStatus)
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                SqlParameter[] param = new SqlParameter[11];
                int index = 0;
                param[index++] = new SqlParameter("@Type", Type);
                param[index++] = new SqlParameter("@AccountId", AccountId);
                param[index++] = new SqlParameter("@AccountName", AccountName);
                param[index++] = new SqlParameter("@ParticipantId", ParticipantId);
                param[index++] = new SqlParameter("@ParticipantName", ParticipantName);

                param[index++] = new SqlParameter("@ProjectId", ProjectId);
                param[index++] = new SqlParameter("@ProjectName", ProjectName);
                param[index++] = new SqlParameter("@ProgrammeId", ProgrammeId);
                param[index++] = new SqlParameter("@ProgrammeName", ProgrammeName);
                param[index++] = new SqlParameter("@EmailDate", EmailDate);
                param[index++] = new SqlParameter("@EmailStatus", EmailStatus);

                return SqlHelper.ExecuteNonQuery(oConn, CommandType.StoredProcedure, "Survey_USPReminderEmailHistoryManagement", param);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oConn.Close();
                oConn = null;
            }
        }
        /// <summary>
        /// Use to handle exception
        /// </summary>
        /// <param name="ex"></param>
        public static void HandleException(Exception ex)
        {
            //ExceptionLogger.Write(ex.ToString());
            FileStream FS;
            StreamWriter SW;
            string fpath;

            string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                        "Error Application: Feedback 360 - UI" + Environment.NewLine + "," +
                        "Error Function: " + ex.TargetSite + Environment.NewLine + "," +
                        "Error Line: " + ex.StackTrace + Environment.NewLine + "," +
                        "Error Source: " + ex.Source + Environment.NewLine + "," +
                        "Error Message: " + ex.Message + Environment.NewLine;

            fpath = System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"].ToString() + "ErrorLog.txt";

            if (File.Exists(fpath))
            { FS = new FileStream(fpath, FileMode.Append, FileAccess.Write); }
            else
            { FS = new FileStream(fpath, FileMode.Create, FileAccess.Write); }

            string msg = str.Replace(",", "");

            SW = new StreamWriter(FS);
            SW.WriteLine(msg);

            SW.Close();
            FS.Close();

        }

    }













}
