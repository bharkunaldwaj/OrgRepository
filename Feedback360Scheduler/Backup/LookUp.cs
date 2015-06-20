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

namespace Feedback360Scheduler
{
    public static class LookUp
    {

        public static DataTable FetchDataForReminder1()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "USPSchGetReminderData1", null).Tables[0];
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

        public static DataTable FetchDataForReminder2()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "USPSchGetReminderData2", null).Tables[0];
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

        public static DataTable FetchDataForReminder3()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "USPSchGetReminderData3", null).Tables[0];
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

        public static DataTable FetchReportAvaliableData()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "USPSchGetReportAvailableData", null).Tables[0];
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

        public static DataTable FetchParticipantReminder1Data()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "USPSchGetParticipantReminderData1", null).Tables[0];
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

        public static DataTable FetchParticipantReminder2Data()
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                DataTable dtResult = SqlHelper.ExecuteDataset(oConn, CommandType.StoredProcedure, "USPSchGetParticipantReminderData2", null).Tables[0];
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

        public static int InsertReminderData(int Type, int AccountId, string AccountName, int ParticipantId, string ParticipantName, int CandidateId, string CandidateName, int ProjectId, string ProjectName, int ProgrammeId, string ProgrammeName, DateTime EmailDate, bool EmailStatus)
        {
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                SqlParameter[] param = new SqlParameter[13];

                param[0] = new SqlParameter("@Type", Type);
                param[1] = new SqlParameter("@AccountId", AccountId);
                param[2] = new SqlParameter("@AccountName", AccountName);
                param[3] = new SqlParameter("@ParticipantId", ParticipantId);
                param[4] = new SqlParameter("@ParticipantName", ParticipantName);
                param[5] = new SqlParameter("@CandidateId", CandidateId);
                param[6] = new SqlParameter("@CandidateName", CandidateName);
                param[7] = new SqlParameter("@ProjectId", ProjectId);
                param[8] = new SqlParameter("@ProjectName", ProjectName);
                param[9] = new SqlParameter("@ProgrammeId", ProgrammeId);
                param[10] = new SqlParameter("@ProgrammeName", ProgrammeName);
                param[11] = new SqlParameter("@EmailDate", EmailDate);
                param[12] = new SqlParameter("@EmailStatus", EmailStatus);

                return SqlHelper.ExecuteNonQuery(oConn, CommandType.StoredProcedure, "USPReminderEmailHistoryManagement", param);
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
