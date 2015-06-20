using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using feedbackFramework_BE;
using feedbackFramework_DAO;
using Questionnaire_BE;
using DatabaseAccessUtilities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace Questionnaire_DAO
{
    public class ReportManagement_DAO : DAO_Base
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

        public int AddParticipantReport(ReportManagement_BE ReportManagement_BE)
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
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspReportManagement", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

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

        public int GetReportManagementListCount(int accountID, int projectID, int programmeID, string admin)
        {
            int categoryCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, accountID, projectID, programmeID, null, null, admin, "C" };

                categoryCount = (int)cDataSrc.ExecuteScalar("UspReportManagement", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryCount;
        }

        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, null, projectID, null, null, null, null, "P" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspReportManagement", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

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

        public DataTable GetReportCandidateName(int accountID, int projectID, int programmeID, string admin)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, accountID, projectID, programmeID, null, null, admin, "UL" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspReportManagement", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

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

        public int AddProjectSettingReport(ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[33] {null,
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
                                                ReportManagement_BE.FrontPageLogo2,
                                                ReportManagement_BE.FrontPageLogo3,
                                                ReportManagement_BE.ConclusionHighLowRange,
                                                ReportManagement_BE.PreviousScoreVisible,
                                                ReportManagement_BE.BenchMarkScoreVisible,
                                                ReportManagement_BE.BenchMarkGrpVisible,
                                                ReportManagement_BE.BenchConclusionpage,
                                                ReportManagement_BE.ConclusionHeading,
                                                "I",
                                                ReportManagement_BE.FrontPageLogo4
                                                };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProjectReportSetting", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

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

        public int DeleteProjectSettingReport(int projectID)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[32] {null,
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

    }





















    public class Survey_ReportManagement_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());



        public Survey_ReportManagement_DAO()
        {

        }

        public DataTable Sur_GetCategory_or_analysis(Survey_ReportManagement_BE Sur_Rptmgmt)
        {
            DataTable dd = null;

            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[4] {Sur_Rptmgmt.ddlAccountCode,
                                                Sur_Rptmgmt.ddlProgramme,
                                                Sur_Rptmgmt.DDList_analysis,
                                                Sur_Rptmgmt.SelectFlag
                                                };

                dd = cDataSrc.ExecuteDataSet("Survey_GetCategory_or_analysis", param, null).Tables[0];

                cDataSrc = null;

            }
            catch (Exception ex) { HandleException(ex); }
            return dd;


        }



        public DataTable list_data_by_category(string accountid, string projectid, string companyId, string programmeid, string AnalysisType, string AnalysisValue)
        {
            DataTable dd = null;

            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[6] {accountid,
                                                projectid,
                                                programmeid,
                                                AnalysisType,
                                                AnalysisValue,
                                                companyId
                                                };

                dd = cDataSrc.ExecuteDataSet("Survey_RspFeedbackByCateogry", param, null).Tables[0];

                cDataSrc = null;

            }
            catch (Exception ex) { HandleException(ex); }
            return dd;
        }





        public DataTable get_final_report_data(string accountid, string projectid, string companyId, string programmeid, string str)
        {
            DataTable dd = null;

            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[5] {accountid,
                                                projectid,
                                                programmeid,
                                                str,
                                                companyId
                                                };

                dd = cDataSrc.ExecuteDataSet("Survey_RspFeedbackByCateogry_cf_or_cg", param, null).Tables[0];

                cDataSrc = null;

            }
            catch (Exception ex) { HandleException(ex); }
            return dd;


        }



        public DataTable get_final_report_data_for_question(string accountid, string projectid, string companyId, string programmeid, string str)
        {
            DataTable dd = null;

            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[5] {accountid,
                                                projectid,
                                                programmeid,
                                                str,
                                                companyId
                                                };

                dd = cDataSrc.ExecuteDataSet("Survey_RspFeedbackByQuestion_qf_or_qg", param, null).Tables[0];

                cDataSrc = null;

            }
            catch (Exception ex) { HandleException(ex); }
            return dd;


        }

        public DataTable list_data_by_question(string accountid, string projectid,string companyId, string programmeid, string AnalysisType, string AnalysisValue)
        {
            DataTable dd = null;

            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[6] {accountid,
                                                projectid,
                                                programmeid,
                                                AnalysisType,
                                                AnalysisValue,
                                                companyId
                                                };

                dd = cDataSrc.ExecuteDataSet("Survey_RspFeedbackByQuestion", param, null).Tables[0];
                cDataSrc = null;
            }
            catch (Exception ex) { HandleException(ex); }
            return dd;
        }




        private int returnValue;


        public List<Survey_ReportManagement_BE> reportManagement_BEList { get; set; }


        # region CRUD Operation

        public int AddParticipantReport(Survey_ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[7] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.ProgramID,
                                            
                                                ReportManagement_BE.ReportName,
                                                null,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspReportManagement", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateParticipantReport(Survey_ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[7] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.ProgramID,
                                              //  ReportManagement_BE.TargetPersonID,
                                                ReportManagement_BE.ReportName,
                                                null,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspReportManagement", param, null));

                cDataSrc = null;


            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteParticipantReport(Survey_ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[7] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.ProgramID,
                                             //   ReportManagement_BE.TargetPersonID,
                                                ReportManagement_BE.ReportName,
                                                null,
                                                "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspReportManagement", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetReportManagementListCount(int accountID, int projectID, int programmeID, string admin)
        {
            int categoryCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, accountID, projectID, programmeID, null, null, admin, "C" };

                categoryCount = (int)cDataSrc.ExecuteScalar("Survey_UspReportManagement", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryCount;
        }

        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, null, projectID, null, null, null, null, "P" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("Survey_UspReportManagement", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

        public DataTable GetdataReportManagement()
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, null, null, null, null, null, null, "R" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("Survey_UspReportManagement", param, null).Tables[0];



                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

        public DataTable GetReportCandidateName(int accountID, int projectID, int programmeID, string admin)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, accountID, projectID, programmeID, null, null, admin, "UL" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("Survey_UspReportManagement", param, null).Tables[0];

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

        public DataTable GetdataReportManagementByID(int targetpersonid)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[8] { null, null, null, null, targetpersonid, null, null, "T" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("Survey_UspReportManagement", param, null).Tables[0];



                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

        public int AddProjectSettingReport(Survey_ReportManagement_BE ReportManagement_BE)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[39] {null,
                                                ReportManagement_BE.AccountID,
                                                ReportManagement_BE.ProjectID,
                                                ReportManagement_BE.CoverPage,
                                                ReportManagement_BE.ReportIntroduction,
                                             
                                                ReportManagement_BE.CatQstList,
                                                ReportManagement_BE.CatDataChart,
                                            
                                                ReportManagement_BE.Conclusionpage,
                                             
                                                ReportManagement_BE.ProjectRelationGrp,
                                           
                                                ReportManagement_BE.ReportType,
                                                ReportManagement_BE.PageHeading1,
                                                ReportManagement_BE.PageHeading2,
                                                ReportManagement_BE.PageHeading3,
                                                ReportManagement_BE.PageHeadingColor,
                                                ReportManagement_BE.PageHeadingCopyright,
                                                ReportManagement_BE.PageHeadingIntro,
                                                ReportManagement_BE.PageHeadingConclusion,
                                                ReportManagement_BE.PageLogo,
                                                ReportManagement_BE.FrontPageLogo2,
                                                ReportManagement_BE.FrontPageLogo3,                                                
                                           
                                                ReportManagement_BE.ConclusionHeading,
                                                ReportManagement_BE.FullProjectGrp,
                                                ReportManagement_BE.AnalysisI,
                                                ReportManagement_BE.AnalysisII,
                                                ReportManagement_BE.AnalysisIII,
                                                ReportManagement_BE.Programme_Average,
                                                ReportManagement_BE.FreeTextResponse,
                                               
                                                "I",
                                                ReportManagement_BE.ShowScoreRespondents,
                                                ReportManagement_BE.ShowRadar,
                                                ReportManagement_BE.ShowTable,
                                                ReportManagement_BE.ShowPreviousScore1,
                                                ReportManagement_BE.ShowPreviousScore2,
                                                ReportManagement_BE.ShowBarGraph,
                                                ReportManagement_BE.ShowLineChart,
                                                ReportManagement_BE.FrontPdfFileName,
                                                ReportManagement_BE.ScoreTableImage,
                                                ReportManagement_BE.FooterImage,
                            ReportManagement_BE.RadarGraphCategoryCount};

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspProjectReportSetting", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdataProjectSettingReportByID(int projectID)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[35] { null, null, projectID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "S", null, null, null, null, null,null, null };

                dtAlluser1 = cDataSrc.ExecuteDataSet("Survey_UspProjectReportSetting", param, null).Tables[0];

                HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }

            catch (Exception ex) { HandleException(ex); }
            return dtAlluser1;
        }

        public int DeleteProjectSettingReport(int projectID)
        {
            try
            {
                ////HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[35] {null,
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
                                                "D" ,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null};

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspProjectReportSetting", param, null));

                cDataSrc = null;

                ////HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                ////HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        #endregion

        #region Radarchart Method
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














    }
}
