using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
using feedbackFramework_DAO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class FeedbackProject_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public FeedbackProject_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"
        
        public List<FeedbackProject_BE> project_BEList { get; set; }
        
        #endregion

        # region CRUD Operation


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

                HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
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

        

        #endregion 
         
        private void ShiftDataTableToBEList(DataTable dtCategory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            project_BEList = new List<FeedbackProject_BE>();

            for (int recordCounter = 0; recordCounter < dtCategory.Rows.Count; recordCounter++)
            {
                FeedbackProject_BE project_BE = new FeedbackProject_BE();

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
                project_BE.QuestionnaireID = dtCategory.Rows[recordCounter]["QuestionnaireID"].ToString().Length>0?  Convert.ToInt32(dtCategory.Rows[recordCounter]["QuestionnaireID"]) :0;
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
                project_BE.EmailTMPManager = Convert.ToInt32(dtCategory.Rows[recordCounter]["EmailTMPManager"].ToString());
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

        
    }






}
