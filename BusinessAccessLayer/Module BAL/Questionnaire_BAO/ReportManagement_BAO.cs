using System;
using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;
using System.Data;

namespace Questionnaire_BAO
{
    public class ReportManagement_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addReportManagement;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Participant Report
        /// </summary>
        /// <param name="reportManagementBusinessEntity"></param>
        /// <returns></returns>
        public int AddParticipantReport(ReportManagement_BE reportManagementBusinessEntity)
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
                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                addReportManagement = ReportManagementDataAccessObject.AddParticipantReport(reportManagementBusinessEntity);
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
        /// <param name="reportManagementBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateParticipantReport(ReportManagement_BE reportManagementBusinessEntity)
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
                ReportManagement_DAO reportManagementDataAccessObject = new ReportManagement_DAO();
                addReportManagement = reportManagementDataAccessObject.UpdateParticipantReport(reportManagementBusinessEntity);
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
        /// <param name="reportManagementBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteParticipantReport(ReportManagement_BE reportManagementBusinessEntity)
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
                ReportManagement_DAO reportManagementDataAccessObject = new ReportManagement_DAO();
                addReportManagement = reportManagementDataAccessObject.DeleteParticipantReport(reportManagementBusinessEntity);
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

                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                reportCount = ReportManagementDataAccessObject.GetReportManagementListCount(accountID, projectID, programmeID, admin);

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
                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetRadarChartData(targetpersonid, grp, operationtype);
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
                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetRadarChartPreviousScoreData(targetpersonid, grp, operationtype);
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
                ReportManagement_DAO reportManagementDataAccessObject = new ReportManagement_DAO();
                project = reportManagementDataAccessObject.GetRadarChartDataCPL(targetpersonid, grp, operationtype);
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
                ReportManagement_DAO reportManagementDataAccessObject = new ReportManagement_DAO();
                project = reportManagementDataAccessObject.GetRadarChartPreviousScoreDataCPL(targetpersonid, grp, operationtype);
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
                ReportManagement_DAO reportManagementDataAccessObject = new ReportManagement_DAO();
                project = reportManagementDataAccessObject.GetRadarChartBenchMark(targetpersonid, operationtype);
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

                ReportManagement_DAO reportManagementDataAccessObject = new ReportManagement_DAO();
                project = reportManagementDataAccessObject.GetdataProjectByID(projectID);



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

                ReportManagement_DAO reportManagementDataAccessObject = new ReportManagement_DAO();
                project = reportManagementDataAccessObject.GetdataReportManagement();



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
                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetReportCandidateName(Convert.ToInt32(accountID), Convert.ToInt32(projectID), Convert.ToInt32(programmeID), admin);
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

                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetdataReportManagementByID(targetpersonid);



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
                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                addReportManagement = ReportManagementDataAccessObject.AddProjectSettingReport(reportManagement_BE);
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
                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetdataProjectSettingReportByID(projectID);
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
                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                addReportManagement = ReportManagementDataAccessObject.DeleteProjectSettingReport(projectID);
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
        /// Delete Dynamic Report
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="programID">program ID</param>
        /// <returns></returns>
        public int DeleteDynamicReport(int accountID, int programID)
        {
            int result = 0;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                ReportManagement_DAO ReportManagementDataAccessObject = new ReportManagement_DAO();
                result = ReportManagementDataAccessObject.DeleteDynamicReport(accountID, programID);

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
            return result;
        }

    }

    public class Survey_ReportManagement_BAO : Base_BAO
    {
        private int addReportManagement;
        /// <summary>
        /// Get Category or analysis
        /// </summary>
        /// <param name="Sur_Rptmgmt"></param>
        /// <returns></returns>
        public DataTable Sur_GetCategory_or_analysis(Survey_ReportManagement_BE Sur_Rptmgmt)
        {
            DataTable dataTableCategoryAnalysis = null;

            Survey_ReportManagement_DAO projectDataAccessObject = new Survey_ReportManagement_DAO();

            dataTableCategoryAnalysis = projectDataAccessObject.Sur_GetCategory_or_analysis(Sur_Rptmgmt);

            return dataTableCategoryAnalysis;

        }

        /// <summary>
        /// List data by category
        /// </summary>
        /// <param name="accountid">accountid</param>
        /// <param name="projectid">projectid</param>
        /// <param name="companyId">companyId</param>
        /// <param name="programmeid">programmeid</param>
        /// <param name="AnalysisType">AnalysisType</param>
        /// <param name="AnalysisValue">AnalysisValue</param>
        /// <returns></returns>
        public DataTable list_data_by_category(string accountid, string projectid, string companyId, string programmeid, string AnalysisType, string AnalysisValue)
        {
            DataTable dataTableCategory = null;
            Survey_ReportManagement_DAO projectDataAccessObject = new Survey_ReportManagement_DAO();
            dataTableCategory = projectDataAccessObject.list_data_by_category(accountid, projectid, companyId, programmeid, AnalysisType, AnalysisValue);
            return dataTableCategory;

        }

        /// <summary>
        /// Get Final report data
        /// </summary>
        /// <param name="ddlAccountCode">AccountCode</param>
        /// <param name="ddlProject">Project</param>
        /// <param name="companyId">companyId</param>
        /// <param name="ddlProgramme">Programme</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable get_final_report_data(string ddlAccountCode, string ddlProject, string companyId, string ddlProgramme, string str)
        {
            DataTable dataTableReportData = null;
            Survey_ReportManagement_DAO projectDataAccessObject = new Survey_ReportManagement_DAO();
            dataTableReportData = projectDataAccessObject.get_final_report_data(ddlAccountCode, ddlProject, companyId, ddlProgramme, str);
            return dataTableReportData;
        }

        /// <summary>
        ///  Get Final report data for question
        /// </summary>
        /// <param name="ddlAccountCode">AccountCode</param>
        /// <param name="ddlProject">Project</param>
        /// <param name="companyId">companyId</param>
        /// <param name="ddlProgramme">Programme</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable get_final_report_data_for_question(string ddlAccountCode, string ddlProject, string companyId, string ddlProgramme, string str)
        {
            DataTable dataTableReportData = null;
            Survey_ReportManagement_DAO projectDataAccessObject = new Survey_ReportManagement_DAO();
            dataTableReportData = projectDataAccessObject.get_final_report_data_for_question(ddlAccountCode, ddlProject, companyId, ddlProgramme, str);
            return dataTableReportData;
        }

        /// <summary>
        /// List data by question
        /// </summary>
        /// <param name="accountid">accountid</param>
        /// <param name="projectid">projectid</param>
        /// <param name="companyId">companyId</param>
        /// <param name="programmeid">programmeid</param>
        /// <param name="AnalysisType">AnalysisType</param>
        /// <param name="AnalysisValue">AnalysisValue</param>
        /// <returns></returns>
        public DataTable list_data_by_question(string accountid, string projectid, string companyId, string programmeid, string AnalysisType, string AnalysisValue)
        {
            DataTable dataTableQuestion = null;
            Survey_ReportManagement_DAO projectDataAccessObject = new Survey_ReportManagement_DAO();
            dataTableQuestion = projectDataAccessObject.list_data_by_question(accountid, projectid, companyId, programmeid, AnalysisType, AnalysisValue);
            return dataTableQuestion;

        }

        /// <summary>
        /// Insert Participant Report
        /// </summary>
        /// <param name="reportManagementBusinessEntity"></param>
        /// <returns></returns>
        public int AddParticipantReport(Survey_ReportManagement_BE reportManagementBusinessEntity)
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
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                addReportManagement = ReportManagementDataAccessObject.AddParticipantReport(reportManagementBusinessEntity);
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
        /// update Insert Participant Report
        /// </summary>
        /// <param name="reportManagementBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateParticipantReport(Survey_ReportManagement_BE reportManagementBusinessEntity)
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
                Survey_ReportManagement_DAO reportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                addReportManagement = reportManagementDataAccessObject.UpdateParticipantReport(reportManagementBusinessEntity);
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
        /// Delete Insert Participant Report
        /// </summary>
        /// <param name="reportManagementBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteParticipantReport(Survey_ReportManagement_BE reportManagementBusinessEntity)
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
                Survey_ReportManagement_DAO reportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                addReportManagement = reportManagementDataAccessObject.DeleteParticipantReport(reportManagementBusinessEntity);
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

                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                reportCount = ReportManagementDataAccessObject.GetReportManagementListCount(accountID, projectID, programmeID, admin);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return reportCount;
        }

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
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetRadarChartData(targetpersonid, grp, operationtype);
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
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetRadarChartPreviousScoreData(targetpersonid, grp, operationtype);
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
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetRadarChartBenchMark(targetpersonid, operationtype);
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
        /// Get Project details By project ID
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetdataProjectByID(projectID);



                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        /// <summary>
        /// Get Report Management
        /// </summary>
        /// <returns></returns>
        public DataTable GetdataReportManagement()
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetdataReportManagement();



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
        /// <param name="accountID">accountID</param>
        /// <param name="projectID">projectID</param>
        /// <param name="programmeID">programmeID</param>
        /// <param name="admin">admin</param>
        /// <returns></returns>
        public DataTable GetReportCandidateName(string accountID, string projectID, string programmeID, string admin)
        {
            DataTable project = new DataTable();
            try
            {
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetReportCandidateName(Convert.ToInt32(accountID), Convert.ToInt32(projectID), Convert.ToInt32(programmeID), admin);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        /// <summary>
        /// Get Report Management By ID
        /// </summary>
        /// <param name="targetpersonid">targetpersonid</param>
        /// <returns></returns>
        public DataTable GetdataReportManagementByID(int targetpersonid)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetdataReportManagementByID(targetpersonid);



                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        /// <summary>
        /// Add Project Setting Report
        /// </summary>
        /// <param name="ReportManagement_BE"></param>
        /// <returns></returns>
        public int AddProjectSettingReport(Survey_ReportManagement_BE reportManagementBusinessEntity)
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
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                addReportManagement = ReportManagementDataAccessObject.AddProjectSettingReport(reportManagementBusinessEntity);
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
        /// Get Project Setting Report By ID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetdataProjectSettingReportByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                project = ReportManagementDataAccessObject.GetdataProjectSettingReportByID(projectID);
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
                Survey_ReportManagement_DAO ReportManagementDataAccessObject = new Survey_ReportManagement_DAO();
                addReportManagement = ReportManagementDataAccessObject.DeleteProjectSettingReport(projectID);
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
    }
}







