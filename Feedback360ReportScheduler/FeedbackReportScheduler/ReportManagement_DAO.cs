using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Questionnaire_BE;
using DatabaseAccessUtilities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace Questionnaire_DAO
{
    public class ReportManagement_DAO 
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        //DatabaseAccessUtilities.CDataSrc cDataSrc183 = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString183"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public ReportManagement_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"

        public List<ReportManagement_BE> reportManagement_BEList { get; set; }
        
        #endregion

        # region CRUD Operation
        /// <summary>
        /// Insert Participant Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int AddParticipantReport(ReportManagement_BE ReportManagement_BE)
        {
            try {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[10] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.ProgramID,
                                                ReportManagement_BE.TargetPersonID,
                                                ReportManagement_BE.TotalCount,
                                                ReportManagement_BE.SubmitCount,
                                                ReportManagement_BE.SelfAssessment,
                                                ReportManagement_BE.ReportName,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspReportSchedulerManagement", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Update Participant Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int UpdateParticipantReport(ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.ProgramID,
                                                ReportManagement_BE.TargetPersonID,
                                                ReportManagement_BE.ReportName,
                                                null,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspReportManagement", param, null));

                cDataSrc = null;

                
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Delete Participant Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int DeleteParticipantReport(ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.ProgramID,
                                                ReportManagement_BE.TargetPersonID,
                                                ReportManagement_BE.ReportName,
                                                null,
                                                "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspReportManagement", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
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
            int categoryCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, accountID, projectID, programmeID, null, null,admin, "C" };

                categoryCount = (int)cDataSrc.ExecuteScalar("UspReportManagement", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryCount;
        }
        /// <summary>
        /// Get data Project By ID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, null, projectID, null, null, null,null, "P" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspReportManagement", param, null).Tables[0];
                                
                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Get  Report Management details
        /// </summary>
        /// <returns></returns>
        public DataTable GetdataReportManagement()
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, null, null, null, null, null, null, "R" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspReportManagement", param, null).Tables[0];



                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Get Report Candidate Name
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <param name="admin">admin</param>
        /// <returns></returns>
        public DataTable GetReportCandidateName(int accountID, int projectID, int programmeID, string admin)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, accountID, projectID, programmeID, null, null,admin, "UL" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspReportManagement", param, null).Tables[0];



                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Get  Report Management details By targetperson id
        /// </summary>
        /// <param name="targetpersonid"></param>
        /// <returns></returns>
        public DataTable GetdataReportManagementByID(int targetpersonid)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, null, null, null, targetpersonid, null, null, "T" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspReportManagement", param, null).Tables[0];



                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Insert Project Setting Report
        /// </summary>
        /// <param name="reportManagement_BE"></param>
        /// <returns></returns>
        public int AddProjectSettingReport(ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[29] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.CoverPage,
                                                ReportManagement_BE.ReportIntroduction,
                                                ReportManagement_BE.RadarChart,
                                                ReportManagement_BE.CatQstList,
                                                ReportManagement_BE.CatDataChart,
                                                ReportManagement_BE.QstTextResponses,
                                                ReportManagement_BE.Conclusionpage,
                                                ReportManagement_BE.CandidateSelfStatus,
                                                ReportManagement_BE.ProjectRelationGrp,
                                                ReportManagement_BE.FullProjectGrp,
                                                ReportManagement_BE.ProgrammeGrp,
                                                ReportManagement_BE.ReportType,
                                                ReportManagement_BE.PageHeading1,
                                                ReportManagement_BE.PageHeading2,
                                                ReportManagement_BE.PageHeading3,
                                                ReportManagement_BE.PageHeadingColor,
                                                ReportManagement_BE.PageHeadingCopyright,
                                                ReportManagement_BE.PageHeadingIntro,
                                                ReportManagement_BE.PageHeadingConclusion,
                                                ReportManagement_BE.PageLogo,
                                                ReportManagement_BE.ConclusionHighLowRange,
                                                ReportManagement_BE.PreviousScoreVisible,
                                                ReportManagement_BE.BenchMarkScoreVisible,
                                                ReportManagement_BE.BenchMarkGrpVisible,
                                                ReportManagement_BE.BenchConclusionpage,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProjectReportSetting", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Get  Project Setting Report details By projectID
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public DataTable GetdataProjectSettingReportByID(int projectID)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[32] { null, null, projectID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "S" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspProjectReportSetting", param, null).Tables[0];



                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Delete Project Setting Report
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public int DeleteProjectSettingReport(int projectID)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[29] {null,
                                                null,
                                                projectID,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null, 
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProjectReportSetting", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        #endregion 

        #region Radarchart Method
        /// <summary>
        /// Get Radar Chart Data
        /// </summary>
        /// <param name="targetpersonid">targetperson id</param>
        /// <param name="grp">group</param>
        /// <param name="operationtype">operationtype</param>
        /// <returns></returns>
        public DataTable GetRadarChartData(int targetpersonid, string grp, string operationtype)
        {
            /*
             * Operation Type is Coming From aspx.cs page
             */
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[3] { targetpersonid, grp, operationtype };
                dtAlluser1 = cDataSrc.ExecuteDataSet("RspRadarChartData", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Get Radar Chart Score Data CPL
        /// </summary>
        public DataTable GetRadarChartDataCPL(int targetpersonid, string grp, string operationtype)
        {
            /*
             * Operation Type is Coming From aspx.cs page
             */
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { targetpersonid, grp, operationtype };
                dtAlluser1 = cDataSrc.ExecuteDataSet("RspRadarChartDataCPL", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
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
            /*
             * Operation Type is Coming From aspx.cs page
             */
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { targetpersonid, grp, operationtype };
                dtAlluser1 = cDataSrc.ExecuteDataSet("RspPreviousScoreRadarChartData", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Get Radar Chart Previous Score Data CPL
        /// </summary>
        public DataTable GetRadarChartPreviousScoreDataCPL(int targetpersonid, string grp, string operationtype)
        {
            /*
             * Operation Type is Coming From aspx.cs page
             */
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { targetpersonid, grp, operationtype };
                dtAlluser1 = cDataSrc.ExecuteDataSet("RspPreviousScoreRadarChartData", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }
        /// <summary>
        /// Get Radar Chart BenchMark
        /// </summary>
        /// <param name="targetpersonid">targetperson id</param>
        /// <param name="operationtype">operationtype</param>
        /// <returns></returns>
        public DataTable GetRadarChartBenchMark(int targetpersonid, string operationtype)
        {
            /*
             * Operation Type is Coming From aspx.cs page
             */
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { targetpersonid, operationtype };
                dtAlluser1 = cDataSrc.ExecuteDataSet("RspBenchMarkScoreRadarChart", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

        #endregion
        /// <summary>
        /// Use to handle exception
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

        /// <summary>
        /// Get Participant details by program id.
        /// </summary>
        /// <param name="programID">program id</param>
        /// <returns></returns>
        public DataTable GetParticipantData(int? programID)
        {
            DataTable dtParticipant = new DataTable();
            try
            {
                object[] param = new object[1] {programID};
                dtParticipant = cDataSrc.ExecuteDataSet("UspGetParticipantData",param, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }
            return dtParticipant;
        }
    }
}
