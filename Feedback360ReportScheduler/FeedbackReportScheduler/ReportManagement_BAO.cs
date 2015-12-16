using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;
using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class ReportManagement_BAO //: Base_BAO
    {
        #region "Private Member Variable"

        private int addReportManagement;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Participant Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int AddParticipantReport(ReportManagement_BE reportManagement_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                addReportManagement = ReportManagement_DAO.AddParticipantReport(reportManagement_BE);
                //HandleWriteLog("End", new StackTrace(true));

                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
            return addReportManagement;
        }
        /// <summary>
        /// Update Participant Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int UpdateParticipantReport(ReportManagement_BE reportManagement_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO reportManagement_DAO = new ReportManagement_DAO();
                addReportManagement = reportManagement_DAO.UpdateParticipantReport(reportManagement_BE);
                //HandleWriteLog("End", new StackTrace(true));

                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
            return addReportManagement;
        }
        /// <summary>
        /// Delete Participant Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int DeleteParticipantReport(ReportManagement_BE reportManagement_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO reportManagement_DAO = new ReportManagement_DAO();
                addReportManagement = reportManagement_DAO.DeleteParticipantReport(reportManagement_BE);
                //HandleWriteLog("End", new StackTrace(true));

                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
            return addReportManagement;
        }
        /// <summary>
        /// Get Report Management List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <param name="admin">admin</param>
        /// <returns></returns>
        public int GetReportManagementListCount(int accountID, int projectID, int programmeID, string admin)
        {
            int reportCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                reportCount = ReportManagement_DAO.GetReportManagementListCount(accountID, projectID, programmeID, admin);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return reportCount;
        }

        #endregion

        #region RadarChart Method
        /// <summary>
        /// Get Radar Chart Data
        /// </summary>
        /// <param name="targetpersonid">targetperson id</param>
        /// <param name="grp">group</param>
        /// <param name="operationtype">operationtype</param>
        /// <returns></returns>
        public DataTable GetRadarChartData(int targetpersonid, string grp, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetRadarChartData(targetpersonid, grp, operationtype);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Get Radar Chart Score Data CPL
        /// </summary>
        public DataTable GetRadarChartDataCPL(int targetpersonid, string grp, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetRadarChartDataCPL(targetpersonid, grp, operationtype);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Get Radar Chart Previous Score Data
        /// </summary>
        /// <param name="targetpersonid">targetperson id</param>
        /// <param name="grp">group</param>
        /// <param name="operationtype">operationtype</param>
        /// <returns></returns>
        public DataTable GetRadarChartPreviousScoreData(int targetpersonid, string grp, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetRadarChartPreviousScoreData(targetpersonid, grp, operationtype);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Get Radar Chart Previous Score Data CPL
        /// </summary>
        public DataTable GetRadarChartPreviousScoreDataCPL(int targetpersonid, string grp, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetRadarChartPreviousScoreDataCPL(targetpersonid, grp, operationtype);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Get Radar Chart BenchMark
        /// </summary>
        /// <param name="targetpersonid">targetperson id</param>
        /// <param name="operationtype">operationtype</param>
        /// <returns></returns>
        public DataTable GetRadarChartBenchMark(int targetpersonid, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetRadarChartBenchMark(targetpersonid, operationtype);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }



        #endregion
        /// <summary>
        /// Get data Project By ID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetdataProjectByID(projectID);



                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Get  Report Management details
        /// </summary>
        /// <returns></returns>
        public DataTable GetdataReportManagement()
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetdataReportManagement();



                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        //public DataTable GetReportCandidateName()
        /// <summary>
        /// Get Report Candidate Name
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <param name="admin">admin</param>
        /// <returns></returns>
        public DataTable GetReportCandidateName(string accountID, string projectID, string programmeID, string admin)        
        {
            DataTable project = new DataTable();
            try
            {
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetReportCandidateName(Convert.ToInt32(accountID), Convert.ToInt32(projectID), Convert.ToInt32(programmeID), admin);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Get  Report Management details By targetperson id
        /// </summary>
        /// <param name="targetpersonid"></param>
        /// <returns></returns>
        public DataTable GetdataReportManagementByID(int targetpersonid)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetdataReportManagementByID(targetpersonid);



                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Insert Project Setting Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int AddProjectSettingReport(ReportManagement_BE reportManagement_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                addReportManagement = ReportManagement_DAO.AddProjectSettingReport(reportManagement_BE);
                //HandleWriteLog("End", new StackTrace(true));

                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
            return addReportManagement;
        }
        /// <summary>
        /// Get  Project Setting Report details By projectID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetdataProjectSettingReportByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {                
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                project = ReportManagement_DAO.GetdataProjectSettingReportByID(projectID);                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Delete Project Setting Report
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public int DeleteProjectSettingReport(int projectID)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                ReportManagement_DAO ReportManagement_DAO = new ReportManagement_DAO();
                addReportManagement = ReportManagement_DAO.DeleteProjectSettingReport(projectID);
                //HandleWriteLog("End", new StackTrace(true));

                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
            return addReportManagement;
        }
        /// <summary>
        /// Use to Handlexceprion
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
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


        internal DataTable GetParticipantData(int? programID)
        {
            DataTable dtParticipant = new DataTable();

            try
            {
                ReportManagement_DAO reportManagement_DAO = new ReportManagement_DAO();
                dtParticipant = reportManagement_DAO.GetParticipantData(programID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtParticipant;
        }
    }
}
