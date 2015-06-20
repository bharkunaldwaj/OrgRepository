using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
using feedbackFramework_DAO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Questionnaire_DAO:DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Questionnaire_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"
        
        public List<Questionnaire_BE.Questionnaire_BE> Questionnaire_BEList { get; set; }
        
        #endregion

        # region CRUD Operation

        public int AddQuestionnaire(Questionnaire_BE.Questionnaire_BE Questionnaire_BE)
        {
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {null,
                                                Questionnaire_BE.AccountID,
                                                Questionnaire_BE.QSTNType,
                                                Questionnaire_BE.QSTNCode,
                                                Questionnaire_BE.QSTNName,
                                                Questionnaire_BE.QSTNDescription,
                                                Questionnaire_BE.DisplayCategory,
                                                Questionnaire_BE.ProjectID,
                                                Questionnaire_BE.ManagerID,
                                                Questionnaire_BE.QSTNPrologue,
                                                Questionnaire_BE.QSTNEpilogue,
                                                Questionnaire_BE.ModifyBy,
                                                Questionnaire_BE.ModifyDate,
                                                Questionnaire_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateQuestionnaire(Questionnaire_BE.Questionnaire_BE Questionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {Questionnaire_BE.QuestionnaireID,
                                                Questionnaire_BE.AccountID,
                                                Questionnaire_BE.QSTNType,
                                                Questionnaire_BE.QSTNCode,
                                                Questionnaire_BE.QSTNName,
                                                Questionnaire_BE.QSTNDescription,
                                                Questionnaire_BE.DisplayCategory,
                                                Questionnaire_BE.ProjectID,
                                                Questionnaire_BE.ManagerID,
                                                Questionnaire_BE.QSTNPrologue,
                                                Questionnaire_BE.QSTNEpilogue,
                                                Questionnaire_BE.ModifyBy,
                                                Questionnaire_BE.ModifyDate,
                                                Questionnaire_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteQuestionnaire(Questionnaire_BE.Questionnaire_BE Questionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {Questionnaire_BE.QuestionnaireID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionnaireByID(int QuestionnaireID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[3] { QuestionnaireID,null, "I" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspQuestionnaireSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionnaireList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspQuestionnaireSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtQuestionnaireList(string accountID)
        {
            DataTable dtAllQuestionnaire = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[3] { null,Convert.ToInt32(accountID), "A" };

                dtAllQuestionnaire = cDataSrc.ExecuteDataSet("UspQuestionnaireSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllQuestionnaire;
        }

        #endregion 

        private void ShiftDataTableToBEList(DataTable dtQuestionnaire)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Questionnaire_BEList = new List<Questionnaire_BE.Questionnaire_BE>();

            for (int recordCounter = 0; recordCounter < dtQuestionnaire.Rows.Count; recordCounter++)
            {
                Questionnaire_BE.Questionnaire_BE Questionnaire_BE = new Questionnaire_BE.Questionnaire_BE(); 

                Questionnaire_BE.QuestionnaireID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["QuestionnaireID"].ToString());
                Questionnaire_BE.AccountID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["AccountID"].ToString());
                Questionnaire_BE.QSTNType = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["QSTNType"].ToString());
                Questionnaire_BE.QSTNCode = dtQuestionnaire.Rows[recordCounter]["QSTNCode"].ToString();
                Questionnaire_BE.QSTNName = dtQuestionnaire.Rows[recordCounter]["QSTNName"].ToString();
                Questionnaire_BE.QSTNDescription = dtQuestionnaire.Rows[recordCounter]["QSTNDescription"].ToString();
                Questionnaire_BE.DisplayCategory = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["DisplayCategory"].ToString());
                //Questionnaire_BE.ProjectID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["ProjectID"].ToString());
                Questionnaire_BE.ManagerID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["ManagerID"].ToString());
                Questionnaire_BE.QSTNPrologue = dtQuestionnaire.Rows[recordCounter]["QSTNPrologue"].ToString();
                Questionnaire_BE.QSTNEpilogue = dtQuestionnaire.Rows[recordCounter]["QSTNEpilogue"].ToString();
                Questionnaire_BE.ModifyBy = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["ModifyBy"].ToString());
                Questionnaire_BE.ModifyDate = Convert.ToDateTime(dtQuestionnaire.Rows[recordCounter]["ModifyDate"].ToString());
                Questionnaire_BE.IsActive = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["IsActive"].ToString());
                
                Questionnaire_BEList.Add(Questionnaire_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetQuestionnaireListCount(string accountID)
        {
            int QuestionnaireCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, accountID, "C" };

                QuestionnaireCount = (int)cDataSrc.ExecuteScalar("UspQuestionnaireSelect", param, null);

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return QuestionnaireCount;
        }

        /// <summary>
        /// Return questionnaire ID based on project ID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public int GetQuestionnaireID(string projectID)
        {
            int QuestionnaireID = 0;
            try
            {
                object[] param = new object[1] { projectID };

                QuestionnaireID = (int)cDataSrc.ExecuteScalar("UspQuestionnaireIDSelect", param, null);
            }
            catch (Exception ex) { HandleException(ex); }
            return QuestionnaireID;
        }

        public DataTable GetProjectQuestionnaire(int projectID)
        {
            DataTable dtProjectQuestionnaire = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                string sql="SELECT dbo.Questionnaire.QuestionnaireID, dbo.Questionnaire.QSTNName FROM dbo.Project INNER JOIN dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID WHERE (dbo.Project.ProjectID = " + projectID + ")";
                dtProjectQuestionnaire = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProjectQuestionnaire;
        }

        public DataTable GetFeedbackQuestionnaire(int questionnaireID) {
            DataTable dtQuestionnaire = new DataTable();
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, "A" };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }

        public DataTable GetFeedbackQuestionnaireByRelationShip(int accountID, int projectID, int questionnaireID, string relationship) {
            DataTable dtQuestionnaire = new DataTable();
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[5] { questionnaireID, "A", projectID, accountID, relationship };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }
        
        public DataTable GetFeedbackQuestionnaireSelf( int questionnaireID)
        {
            DataTable dtQuestionnaire = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[2] { questionnaireID, "N" };
               // object[] param = new object[2] { questionnaireID, "N" };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }


        public DataTable GetFeedbackQuestionnaireSelfByRelationShip(int accountID, int projectID, int questionnaireID, string relationship) {
            DataTable dtQuestionnaire = new DataTable();
            try {
                //HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[5] { questionnaireID, "N", projectID, accountID, relationship };
                // object[] param = new object[2] { questionnaireID, "N" };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }

        public int GetQuestionListCount(int questionnaireID)
        {
            int result=0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID,"C" };

                result = (int)cDataSrc.ExecuteScalar("UspFeedbackQuestionSelect", param, null);

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public DataTable GetFeedbackQuestionnaire(int questionnaireID, int candidateID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, candidateID };

                dtResult = cDataSrc.ExecuteDataSet("UspProjectInfoSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtResult;
        }

        public int CalculateGraph(int questionnaireID, int candidateID)
        {
            int result = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, candidateID };

                result = (int)cDataSrc.ExecuteScalar("UspCalculateGraph", param, null);

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public DataTable GetQuestionnaireCategories(int questionnaireID)
        {
            DataTable dtQuestionnaire = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, "S" };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }

        public DataTable GetQuestionnaireCategoriesByRelationShip(int accountID, int projectID, int questionnaireID, string relationship) {
            DataTable dtQuestionnaire = new DataTable();
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[5] { questionnaireID, "S", projectID,accountID, relationship };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }

        public DataTable GetCategoryQuestions(int categoryID)
        {
            DataTable dtQuestion = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { categoryID};

                dtQuestion = cDataSrc.ExecuteDataSet("UspCategoryQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtQuestion;
        }

        public int UpdateSubmitFlag(int candidateID,int submitFlag)
        {
            int result = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //result = (int)cDataSrc.ExecuteScalar("update AssignmentDetails set SubmitFlag=" + submitFlag + " where AsgnDetailID=" + candidateID, null);
                cDataSrc.ExecuteNonQuery("update AssignmentDetails set SubmitFlag=" + submitFlag + " where AsgnDetailID=" + candidateID, null);
                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return result;
        }
    }




















    public class Survey_Questionnaire_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_Questionnaire_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Questionnaire_BE.Survey_Questionnaire_BE> Questionnaire_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public int AddQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE Questionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {null,
                                                Questionnaire_BE.AccountID,
                                                Questionnaire_BE.QSTNType,
                                                Questionnaire_BE.QSTNCode,
                                                Questionnaire_BE.QSTNName,
                                                Questionnaire_BE.QSTNDescription,
                                                Questionnaire_BE.DisplayCategory,
                                                Questionnaire_BE.ProjectID,
                                                Questionnaire_BE.ManagerID,
                                                Questionnaire_BE.QSTNPrologue,
                                                Questionnaire_BE.QSTNEpilogue,
                                                Questionnaire_BE.ModifyBy,
                                                Questionnaire_BE.ModifyDate,
                                                Questionnaire_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE Questionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {Questionnaire_BE.QuestionnaireID,
                                                Questionnaire_BE.AccountID,
                                                Questionnaire_BE.QSTNType,
                                                Questionnaire_BE.QSTNCode,
                                                Questionnaire_BE.QSTNName,
                                                Questionnaire_BE.QSTNDescription,
                                                Questionnaire_BE.DisplayCategory,
                                                Questionnaire_BE.ProjectID,
                                                Questionnaire_BE.ManagerID,
                                                Questionnaire_BE.QSTNPrologue,
                                                Questionnaire_BE.QSTNEpilogue,
                                                Questionnaire_BE.ModifyBy,
                                                Questionnaire_BE.ModifyDate,
                                                Questionnaire_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE Questionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {Questionnaire_BE.QuestionnaireID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionnaireByID(int QuestionnaireID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[3] { QuestionnaireID, null, "I" };

                dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspQuestionnaireSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionnaireList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspQuestionnaireSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtQuestionnaireList(string accountID)
        {
            DataTable dtAllQuestionnaire = new DataTable();
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

                dtAllQuestionnaire = cDataSrc.ExecuteDataSet("Survey_UspQuestionnaireSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtAllQuestionnaire;
        }

        #endregion

        private void ShiftDataTableToBEList(DataTable dtQuestionnaire)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Questionnaire_BEList = new List<Questionnaire_BE.Survey_Questionnaire_BE>();

            for (int recordCounter = 0; recordCounter < dtQuestionnaire.Rows.Count; recordCounter++)
            {
                Questionnaire_BE.Survey_Questionnaire_BE Questionnaire_BE = new Questionnaire_BE.Survey_Questionnaire_BE();

                Questionnaire_BE.QuestionnaireID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["QuestionnaireID"].ToString());
                Questionnaire_BE.AccountID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["AccountID"].ToString());
                Questionnaire_BE.QSTNType = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["QSTNType"].ToString());
                Questionnaire_BE.QSTNCode = dtQuestionnaire.Rows[recordCounter]["QSTNCode"].ToString();
                Questionnaire_BE.QSTNName = dtQuestionnaire.Rows[recordCounter]["QSTNName"].ToString();
                Questionnaire_BE.QSTNDescription = dtQuestionnaire.Rows[recordCounter]["QSTNDescription"].ToString();
                Questionnaire_BE.DisplayCategory = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["DisplayCategory"].ToString());
                //Questionnaire_BE.ProjectID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["ProjectID"].ToString());
                Questionnaire_BE.ManagerID = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["ManagerID"].ToString());
                Questionnaire_BE.QSTNPrologue = dtQuestionnaire.Rows[recordCounter]["QSTNPrologue"].ToString();
                Questionnaire_BE.QSTNEpilogue = dtQuestionnaire.Rows[recordCounter]["QSTNEpilogue"].ToString();
                Questionnaire_BE.ModifyBy = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["ModifyBy"].ToString());
                Questionnaire_BE.ModifyDate = Convert.ToDateTime(dtQuestionnaire.Rows[recordCounter]["ModifyDate"].ToString());
                Questionnaire_BE.IsActive = Convert.ToInt32(dtQuestionnaire.Rows[recordCounter]["IsActive"].ToString());

                Questionnaire_BEList.Add(Questionnaire_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetQuestionnaireListCount(string accountID)
        {
            int QuestionnaireCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, accountID, "C" };

                QuestionnaireCount = (int)cDataSrc.ExecuteScalar("Survey_UspQuestionnaireSelect", param, null);

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return QuestionnaireCount;
        }

        public DataTable GetProjectQuestionnaire(int projectID)
        {
            DataTable dtProjectQuestionnaire = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                string sql = "SELECT dbo.Survey_Questionnaire.QuestionnaireID, dbo.Survey_Questionnaire.QSTNName FROM dbo.Survey_Project INNER JOIN dbo.Survey_Questionnaire ON dbo.Survey_Project.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID WHERE (dbo.Survey_Project.ProjectID = " + projectID + ")";
                dtProjectQuestionnaire = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProjectQuestionnaire;
        }

        public DataTable GetFeedbackQuestionnaire(int questionnaireID)
        {
            DataTable dtQuestionnaire = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, "A" };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("Survey_UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }

        public DataTable GetFeedbackQuestionnaireSelf(int questionnaireID)
        {
            DataTable dtQuestionnaire = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, "N" };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("Survey_UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }

        public int GetQuestionListCount(int questionnaireID)
        {
            int result = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, "C" };

                result = (int)cDataSrc.ExecuteScalar("Survey_UspFeedbackQuestionSelect", param, null);

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public DataTable GetFeedbackQuestionnaire(int questionnaireID, int candidateID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, candidateID };

                dtResult = cDataSrc.ExecuteDataSet("Survey_UspProjectInfoSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtResult;
        }


        public DataTable GetRangeDetails(string RangeName)
        {
            DataTable dtResult = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { RangeName };

                dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetRangeDetails", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtResult;
        }


        public int CalculateGraph(int questionnaireID, int candidateID)
        {
            int result = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, candidateID };

                result = (int)cDataSrc.ExecuteScalar("Survey_UspCalculateGraph", param, null);

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public DataTable GetQuestionnaireCategories(int questionnaireID)
        {
            DataTable dtQuestionnaire = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { questionnaireID, "S" };

                dtQuestionnaire = cDataSrc.ExecuteDataSet("Survey_UspFeedbackQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtQuestionnaire;
        }

        public DataTable GetCategoryQuestions(int categoryID)
        {
            DataTable dtQuestion = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { categoryID };

                dtQuestion = cDataSrc.ExecuteDataSet("Survey_UspCategoryQuestionSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtQuestion;
        }

        public int UpdateSubmitFlag(int candidateID, int submitFlag)
        {
            int result = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //result = (int)cDataSrc.ExecuteScalar("update AssignmentDetails set SubmitFlag=" + submitFlag + " where AsgnDetailID=" + candidateID, null);
                cDataSrc.ExecuteNonQuery("update Survey_AssignmentDetails set SubmitFlag=" + submitFlag + " where AsgnDetailID=" + candidateID, null);
                //HandleWriteLogDAU("UspQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return result;
        }
    }











}
