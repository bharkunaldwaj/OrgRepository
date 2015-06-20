using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Questionnaire_BE;
using DBHandler;

using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Project_DAO 
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Project_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"
        
        public List<Project_BE> project_BEList { get; set; }
        
        #endregion

        # region CRUD Operation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int AddProject(Project_BE project_BE)
        {
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[36] {null,
                                                project_BE.StatusID,
                                                project_BE.Reference,
                                                project_BE.Title,
                                                project_BE.Description,
                                                project_BE.AccountID,
                                                project_BE.ManagerID,
                                                project_BE.MaxCandidate,
                                                project_BE.Logo,
                                                project_BE.Password,
                                                project_BE.QuestionnaireID,
                                                project_BE.StartDate,
                                                project_BE.EndDate,
                                                project_BE.Reminder1Date,
                                                project_BE.Reminder2Date,
                                                project_BE.Reminder3Date,
                                                project_BE.ReportAvaliableFrom,
                                                project_BE.ReportAvaliableTo,
                                                project_BE.EmailTMPLStart,
                                                project_BE.EmailTMPLReminder1,
                                                project_BE.EmailTMPLReminder2,
                                                project_BE.EmailTMPLReminder3,
                                                project_BE.EmailTMPLReportAvalibale,
                                                project_BE.EmailTMPLParticipant,
                                                project_BE.EmailTMPPartReminder1,
                                                project_BE.EmailTMPPartReminder2,
                                                project_BE.Relationship1,
                                                project_BE.Relationship2,
                                                project_BE.Relationship3,
                                                project_BE.Relationship4,
                                                project_BE.Relationship5,
                                                project_BE.FaqText,
                                                project_BE.ModifyBy,
                                                project_BE.ModifyDate,
                                                project_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProjectManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int UpdateProject(Project_BE project_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[36] {project_BE.ProjectID,
                                                project_BE.StatusID,
                                                project_BE.Reference,
                                                project_BE.Title,
                                                project_BE.Description,
                                                project_BE.AccountID,
                                                project_BE.ManagerID,
                                                project_BE.MaxCandidate,
                                                project_BE.Logo,
                                                project_BE.Password,
                                                project_BE.QuestionnaireID,
                                                project_BE.StartDate,
                                                project_BE.EndDate,
                                                project_BE.Reminder1Date,
                                                project_BE.Reminder2Date,
                                                project_BE.Reminder3Date,
                                                project_BE.ReportAvaliableFrom,
                                                project_BE.ReportAvaliableTo,
                                                project_BE.EmailTMPLStart,
                                                project_BE.EmailTMPLReminder1,
                                                project_BE.EmailTMPLReminder2,
                                                project_BE.EmailTMPLReminder3,
                                                project_BE.EmailTMPLReportAvalibale,
                                                project_BE.EmailTMPLParticipant,
                                                project_BE.EmailTMPPartReminder1,
                                                project_BE.EmailTMPPartReminder2,
                                                project_BE.Relationship1,
                                                project_BE.Relationship2,
                                                project_BE.Relationship3,
                                                project_BE.Relationship4,
                                                project_BE.Relationship5,
                                                project_BE.FaqText,
                                                project_BE.ModifyBy,
                                                project_BE.ModifyDate,
                                                project_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProjectManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteProject(Project_BE project_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[36] {project_BE.ProjectID,
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
                                                 null,
                                                 null,
                                                 null,
                                                 null,
                                                 null,
                                                 null,
                                                 "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProjectManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        //public DataTable SearchProject(Project_Search project_Search)
        //{
        //    DataTable dtSearchProject = new DataTable();
        //    try
        //    {
        //        //HandleWriteLog("Start", new StackTrace(true));

        //        object[] param = new object[7] {  project_Search.ProjectID,
        //                                         project_Search.Title,
        //                                         project_Search.StatusID,
        //                                         project_Search.Reference,
        //                                         project_Search.ManagerID,
        //                                         project_Search.StartDate,
        //                                         project_Search.EndDate };

        //        dtSearchProject = cDataSrc.ExecuteDataSet("UspProjectSearch", param, null).Tables[0];

        //        //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
        //        //HandleWriteLog("End", new StackTrace(true));
        //    }
        //    catch (Exception ex) { HandleException(ex); }
        //    return dtSearchProject;
        //}

        public int GetProjectByID(int accountID,int projectID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[3] { projectID,accountID,"I" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspProjectSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetProjectList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspProjectSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtProjectList(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null,Convert.ToInt32(accountID), "A" };

                dtAllProject = cDataSrc.ExecuteDataSet("UspProjectSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }

        public DataTable GetdtProjectListNew(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            try
            {
                string sql = "Select [ProjectID]" +
                        ",Project.StatusID" +
                        ",Project.AccountID AS AccountID" +
                        ",[Reference]" +
                        ",[Title]" +
                        ",Project.Description" +
                        ",[ManagerID]" +
                        ",[MaxCandidate]" +
                        ",[Logo]" +
                        ",Project.Password" +
                        ",[QuestionnaireID]	" +
                        ",[StartDate]" +
                        ",[EndDate]" +
                        ",[Reminder1Date]" +
                        ",[Reminder2Date]" +
                        ",[Reminder3Date]" +
                        ",[ReportAvaliableFrom]" +
                        ",[ReportAvaliableTo]" +
                        ",[EmailTMPLStart]" +
                        ",[EmailTMPLReminder1]" +
                        ",[EmailTMPLReminder2]" +
                        ",[EmailTMPLReminder3]" +
                        ",[EmailTMPLReportAvalibale]" +
                        ",[EmailTMPLParticipant]" +
                        ",[EmailTMPPartReminder1]" +
                        ",[EmailTMPPartReminder2]" +
                        ",[Relationship1]" +
                        ",[Relationship2]" +
                        ",[Relationship3]" +
                        ",[Relationship4]" +
                        ",[Relationship5]" +
                        ",[FaqText]" +
                        ",Project.ModifyBy" +
                        ",Project.ModifyDate" +
                        ",Project.IsActive" +
                        ",[User].UserID " +
                        ",[User].FirstName as firstname" +
                        ",[User].LastName  as lastname" +
                        ", (firstname + ' ' + lastname) as finalname" +
                        ",MSTProjectStatus.Name as ProjectStatus" +
                        ",[Account].[Code] as Code" +
                        " FROM   [Project] Inner Join [User] on dbo.Project.ManagerID = [User].UserID" +
                        " Inner Join MSTProjectStatus on Project.StatusID = MSTProjectStatus.PRJStatusID" +
                        " INNER JOIN dbo.Account ON Project.AccountID = dbo.Account.AccountID" +
                        " Where  Project.[AccountID] = " + accountID +
                        " order by dbo.Account.[Code] ,[ProjectID] desc";

                dtAllProject = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }

        public DataTable GetAdminProjectList(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "F" };

                dtAllProject = cDataSrc.ExecuteDataSet("UspProjectSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }

        public void InsertprojID(string id, int accountid)
        {

            try
            {
                

                object[] param = new object[4] { 0, id, accountid, "C" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProjectCopy", param, null));

                cDataSrc = null;
            }
            catch (Exception ex) { HandleException(ex); }


        }

        #endregion 
         
        private void ShiftDataTableToBEList(DataTable dtCategory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            project_BEList = new List<Project_BE>();

            for (int recordCounter = 0; recordCounter < dtCategory.Rows.Count; recordCounter++)
            {
                Project_BE project_BE = new Project_BE();

                project_BE.ProjectID = Convert.ToInt32(dtCategory.Rows[recordCounter]["ProjectID"].ToString());
                project_BE.AccountID = Convert.ToInt32(dtCategory.Rows[recordCounter]["AccountID"].ToString());
                project_BE.StatusID = Convert.ToInt32(dtCategory.Rows[recordCounter]["StatusID"].ToString());
                project_BE.Reference = dtCategory.Rows[recordCounter]["Reference"].ToString();
                project_BE.Title = dtCategory.Rows[recordCounter]["Title"].ToString();
                project_BE.Description = dtCategory.Rows[recordCounter]["Description"].ToString();
                project_BE.ManagerID = Convert.ToInt32(dtCategory.Rows[recordCounter]["ManagerID"].ToString());
                project_BE.MaxCandidate = Convert.ToInt32(dtCategory.Rows[recordCounter]["MaxCandidate"].ToString());
                project_BE.Logo = dtCategory.Rows[recordCounter]["Logo"].ToString();
                project_BE.Password = dtCategory.Rows[recordCounter]["Password"].ToString();
                project_BE.QuestionnaireID = Convert.ToInt32(dtCategory.Rows[recordCounter]["QuestionnaireID"]);
                //project_BE.StartDate = Convert.ToDateTime(dtCategory.Rows[recordCounter]["StartDate"].ToString());
                //project_BE.EndDate = Convert.ToDateTime(dtCategory.Rows[recordCounter]["EndDate"].ToString());
                //project_BE.Reminder1Date = Convert.ToDateTime(dtCategory.Rows[recordCounter]["Reminder1Date"].ToString());
                //project_BE.Reminder2Date = Convert.ToDateTime(dtCategory.Rows[recordCounter]["Reminder2Date"].ToString());
                //project_BE.Reminder3Date = Convert.ToDateTime(dtCategory.Rows[recordCounter]["Reminder3Date"].ToString());
                //project_BE.ReportAvaliableFrom = Convert.ToDateTime(dtCategory.Rows[recordCounter]["ReportAvaliableFrom"].ToString());
                //project_BE.ReportAvaliableTo = Convert.ToDateTime(dtCategory.Rows[recordCounter]["ReportAvaliableTo"].ToString());
                project_BE.EmailTMPLStart = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPLStart"].ToString());
                project_BE.EmailTMPLReminder1 = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPLReminder1"].ToString());
                project_BE.EmailTMPLReminder2 = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPLReminder2"].ToString());
                project_BE.EmailTMPLReminder3 = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPLReminder3"].ToString());
                project_BE.EmailTMPLReportAvalibale = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPLReportAvalibale"].ToString());
                project_BE.EmailTMPLParticipant = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPLParticipant"].ToString());
                project_BE.EmailTMPPartReminder1 = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPPartReminder1"].ToString());
                project_BE.EmailTMPPartReminder2 = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPPartReminder2"].ToString());
                project_BE.Relationship1 = dtCategory.Rows[recordCounter]["Relationship1"].ToString();
                project_BE.Relationship2 = dtCategory.Rows[recordCounter]["Relationship2"].ToString();
                project_BE.Relationship3 = dtCategory.Rows[recordCounter]["Relationship3"].ToString();
                project_BE.Relationship4 = dtCategory.Rows[recordCounter]["Relationship4"].ToString();
                project_BE.Relationship5 = dtCategory.Rows[recordCounter]["Relationship5"].ToString();
                project_BE.FaqText = dtCategory.Rows[recordCounter]["FaqText"].ToString();
                project_BE.ModifyBy = Convert.ToInt32(dtCategory.Rows[recordCounter]["ModifyBy"].ToString());
                project_BE.ModifyDate = Convert.ToDateTime(dtCategory.Rows[recordCounter]["ModifyDate"].ToString());
                project_BE.IsActive = Convert.ToInt32(dtCategory.Rows[recordCounter]["IsActive"].ToString());
                project_BEList.Add(project_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetProjectListCount(string accountID)
        {
            int projectCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //object[] param = new object[3] { null, Convert.ToInt32(accountID), "C" };

                string sql = "Select Count([ProjectID])" +
                       " FROM   [Project] Inner Join [User] on dbo.Project.ManagerID = [User].UserID" +
                       " Inner Join MSTProjectStatus on Project.StatusID = MSTProjectStatus.PRJStatusID" +
                       " INNER JOIN dbo.Account ON Project.AccountID = dbo.Account.AccountID" +
                       " Where  Project.[AccountID] = " + accountID ;

                projectCount = (int)cDataSrc.ExecuteScalar(sql, null);

                //projectCount = (int)cDataSrc.ExecuteScalar("UspProjectSelect", param, null);
                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return projectCount;
        }

        public DataTable GetAccountProject(int accountID)
        {
            DataTable dtAccountProject = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                dtAccountProject = cDataSrc.ExecuteDataSet("SELECT [ProjectID],[Title] FROM [Project] where [IsActive]=1 and [AccountID]=" + accountID, null).Tables[0];

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountProject;
        }

        public DataTable GetdataProjectByID( int projectID)
        {
            DataTable dtAlluser1 = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { projectID, null, "P" };

                dtAlluser1 = cDataSrc.ExecuteDataSet("UspProjectSelect", param, null).Tables[0];

                
            
                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
             return dtAlluser1;
        }

        public DataTable GetProjectRelationship(int projectID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { projectID };

                dtResult = cDataSrc.ExecuteDataSet("UspGetProjectRelationship", param, null).Tables[0];

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtResult;
        }

        public string GetProjectFaqText(int projectId)
        {
            string dtResult="" ;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { projectId };

                dtResult = cDataSrc.ExecuteScalar("GetProjectFAQ", param, null).ToString();

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtResult;
        }

        public DataTable GetAccProject(int accountID)
        {
            DataTable dtAccProject = new DataTable();

            try
            {
                object[] param = new object[2] { accountID, "I" };

                dtAccProject = cDataSrc.ExecuteDataSet("UspAccProject", param, null).Tables[0];

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccProject;
        }
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


        
    }
}
