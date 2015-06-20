using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;
using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class ReportManagement_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addReportManagement;

        #endregion

        #region CRUD Operations

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

    }



























    public class Survey_ReportManagement_BAO : Base_BAO
    {
        private int addReportManagement;
        public DataTable Sur_GetCategory_or_analysis(Survey_ReportManagement_BE Sur_Rptmgmt)
        {
            DataTable dd = null;
            Survey_ReportManagement_DAO project_DAO = new Survey_ReportManagement_DAO();
            dd = project_DAO.Sur_GetCategory_or_analysis(Sur_Rptmgmt);
            return dd;

        }

        public DataTable list_data_by_category(string accountid, string projectid, string companyId, string programmeid, string AnalysisType, string AnalysisValue)
        {
            DataTable dd = null;
            Survey_ReportManagement_DAO project_DAO = new Survey_ReportManagement_DAO();
            dd = project_DAO.list_data_by_category(accountid, projectid,companyId, programmeid, AnalysisType, AnalysisValue);
            return dd;

        }



        public DataTable get_final_report_data(string ddlAccountCode, string ddlProject, string companyId, string ddlProgramme, string str)
        {
            DataTable dd = null;
            Survey_ReportManagement_DAO project_DAO = new Survey_ReportManagement_DAO();
            dd = project_DAO.get_final_report_data(ddlAccountCode, ddlProject,companyId, ddlProgramme, str);
            return dd;
        }

        public DataTable get_final_report_data_for_question(string ddlAccountCode, string ddlProject,string companyId, string ddlProgramme, string str)
        {
            DataTable dd = null;
            Survey_ReportManagement_DAO project_DAO = new Survey_ReportManagement_DAO();
            dd = project_DAO.get_final_report_data_for_question(ddlAccountCode, ddlProject,companyId, ddlProgramme, str);
            return dd;
        }



        public DataTable list_data_by_question(string accountid, string projectid,string companyId, string programmeid, string AnalysisType, string AnalysisValue)
        {
            DataTable dd = null;
            Survey_ReportManagement_DAO project_DAO = new Survey_ReportManagement_DAO();
            dd = project_DAO.list_data_by_question(accountid, projectid,companyId, programmeid, AnalysisType, AnalysisValue);
            return dd;

        }





        public int AddParticipantReport(Survey_ReportManagement_BE reportManagement_BE)
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
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
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

        public int UpdateParticipantReport(Survey_ReportManagement_BE reportManagement_BE)
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
                Survey_ReportManagement_DAO reportManagement_DAO = new Survey_ReportManagement_DAO();
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

        public int DeleteParticipantReport(Survey_ReportManagement_BE reportManagement_BE)
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
                Survey_ReportManagement_DAO reportManagement_DAO = new Survey_ReportManagement_DAO();
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

        public int GetReportManagementListCount(int accountID, int projectID, int programmeID, string admin)
        {
            int reportCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
                reportCount = ReportManagement_DAO.GetReportManagementListCount(accountID, projectID, programmeID, admin);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return reportCount;
        }



        #region RadarChart Method
        public DataTable GetRadarChartData(int targetpersonid, string grp, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
                project = ReportManagement_DAO.GetRadarChartData(targetpersonid, grp, operationtype);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        public DataTable GetRadarChartPreviousScoreData(int targetpersonid, string grp, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
                project = ReportManagement_DAO.GetRadarChartPreviousScoreData(targetpersonid, grp, operationtype);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        public DataTable GetRadarChartBenchMark(int targetpersonid, string operationtype)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
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

        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
                project = ReportManagement_DAO.GetdataProjectByID(projectID);



                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        public DataTable GetdataReportManagement()
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
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
        public DataTable GetReportCandidateName(string accountID, string projectID, string programmeID, string admin)
        {
            DataTable project = new DataTable();
            try
            {
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
                project = ReportManagement_DAO.GetReportCandidateName(Convert.ToInt32(accountID), Convert.ToInt32(projectID), Convert.ToInt32(programmeID), admin);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        public DataTable GetdataReportManagementByID(int targetpersonid)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
                project = ReportManagement_DAO.GetdataReportManagementByID(targetpersonid);



                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        public int AddProjectSettingReport(Survey_ReportManagement_BE reportManagement_BE)
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
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
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

        public DataTable GetdataProjectSettingReportByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
                project = ReportManagement_DAO.GetdataProjectSettingReportByID(projectID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

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
                Survey_ReportManagement_DAO ReportManagement_DAO = new Survey_ReportManagement_DAO();
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
    }


}







