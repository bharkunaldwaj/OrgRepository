using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class Questionnaire_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addQuestionnaire;

        #endregion

        #region CRUD Operations

        public int AddQuestionnaire(Questionnaire_BE.Questionnaire_BE questionnaire_BE)
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
                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                addQuestionnaire = questionnaire_DAO.AddQuestionnaire(questionnaire_BE);
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
            return addQuestionnaire;
        }

        public int UpdateQuestionnaire(Questionnaire_BE.Questionnaire_BE questionnaire_BE)
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
                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                addQuestionnaire = questionnaire_DAO.UpdateQuestionnaire(questionnaire_BE);
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
            return addQuestionnaire;
        }

        public int DeleteQuestionnaire(Questionnaire_BE.Questionnaire_BE questionnaire_BE)
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
                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                addQuestionnaire = questionnaire_DAO.DeleteQuestionnaire(questionnaire_BE);
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
            return addQuestionnaire;
        }

        public List<Questionnaire_BE.Questionnaire_BE> GetQuestionnaireByID(int questionnaireID)
        {
            List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                questionnaire_DAO.GetQuestionnaireByID(questionnaireID);

                questionnaire_BEList = questionnaire_DAO.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaire_BEList;
        }

        public List<Questionnaire_BE.Questionnaire_BE> GetQuestionnaireList()
        {
            List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                questionnaire_DAO.GetQuestionnaireList();

                questionnaire_BEList = questionnaire_DAO.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaire_BEList;
        }

        public DataTable GetdtQuestionnaireList(string accountID)
        {
            DataTable dtQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtQuestionnaire = questionnaire_DAO.GetdtQuestionnaireList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtQuestionnaire;
        }

        public int GetQuestionnaireListCount(string accountID)
        {
            int questionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                questionnaireCount = questionnaire_DAO.GetQuestionnaireListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionnaireCount;
        }

        public DataTable GetProjectQuestionnaire(int projectID)
        {
            DataTable dtProjectQuestionnaire = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO projectauestionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtProjectQuestionnaire = projectauestionnaire_DAO.GetProjectQuestionnaire(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProjectQuestionnaire;
        }

        #endregion

        public DataTable GetFeedbackQuestionnaire( int questionnaireID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaire(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public DataTable GetFeedbackQuestionnaireByRelationShip(int accountID, int projectID, int questionnaireID, string relationship) {
            DataTable dtResult = new DataTable();

            try {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaireByRelationShip(accountID, projectID, questionnaireID, relationship);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }

            return dtResult;
        }


        public DataTable GetFeedbackQuestionnaireSelf( int questionnaireID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaireSelf(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public DataTable GetFeedbackQuestionnaireSelfByRelationShip(int accountID, int projectID, int questionnaireID, string relationship)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaireSelfByRelationShip(accountID, projectID, questionnaireID, relationship);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public int GetQuestionListCount(string accountID, int questionnaireID)
        {
            int questionCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                questionCount = questionnaire_DAO.GetQuestionListCount(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionCount;
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
                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                QuestionnaireID = questionnaire_DAO.GetQuestionnaireID(projectID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return QuestionnaireID;
        }

        public DataTable GetProjectQuestionnaireInfo(int questionnaireID, int candidateID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaire(questionnaireID, candidateID);

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
            int answerCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                answerCount = questionnaire_DAO.CalculateGraph(questionnaireID, candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return answerCount;
        }

        public DataTable GetQuestionnaireCategories(int questionnaireID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetQuestionnaireCategories(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }


        public DataTable GetQuestionnaireCategoriesByRelationShip(int accountID, int projectID, int questionnaireID, string relationship ) {
            DataTable dtResult = new DataTable();

            try {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetQuestionnaireCategoriesByRelationShip(accountID, projectID, questionnaireID, relationship);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }

            return dtResult;
        }

        public DataTable GetCategoryQuestions(int categoryID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetCategoryQuestions(categoryID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public int UpdateSubmitFlag(int candidateID,int submitFlag)
        {
            int result=0 ;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Questionnaire_DAO();
                result = questionnaire_DAO.UpdateSubmitFlag(candidateID,submitFlag);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

    }

















    public class Survey_Questionnaire_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addQuestionnaire;

        #endregion

        #region CRUD Operations

        public int AddQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE questionnaire_BE)
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
                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                addQuestionnaire = questionnaire_DAO.AddQuestionnaire(questionnaire_BE);
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
            return addQuestionnaire;
        }

        public int UpdateQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE questionnaire_BE)
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
                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                addQuestionnaire = questionnaire_DAO.UpdateQuestionnaire(questionnaire_BE);
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
            return addQuestionnaire;
        }

        public int DeleteQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE questionnaire_BE)
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
                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                addQuestionnaire = questionnaire_DAO.DeleteQuestionnaire(questionnaire_BE);
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
            return addQuestionnaire;
        }

        public List<Questionnaire_BE.Survey_Questionnaire_BE> GetQuestionnaireByID(int questionnaireID)
        {
            List<Questionnaire_BE.Survey_Questionnaire_BE> questionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionnaire_DAO.GetQuestionnaireByID(questionnaireID);

                questionnaire_BEList = questionnaire_DAO.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaire_BEList;
        }

        public List<Questionnaire_BE.Survey_Questionnaire_BE> GetQuestionnaireList()
        {
            List<Questionnaire_BE.Survey_Questionnaire_BE> questionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionnaire_DAO.GetQuestionnaireList();

                questionnaire_BEList = questionnaire_DAO.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaire_BEList;
        }

        public DataTable GetdtQuestionnaireList(string accountID)
        {
            DataTable dtQuestionnaire = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtQuestionnaire = questionnaire_DAO.GetdtQuestionnaireList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtQuestionnaire;
        }

        public int GetQuestionnaireListCount(string accountID)
        {
            int questionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionnaireCount = questionnaire_DAO.GetQuestionnaireListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionnaireCount;
        }

        public DataTable GetProjectQuestionnaire(int projectID)
        {
            DataTable dtProjectQuestionnaire = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO projectauestionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtProjectQuestionnaire = projectauestionnaire_DAO.GetProjectQuestionnaire(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProjectQuestionnaire;
        }

        #endregion

        public DataTable GetFeedbackQuestionnaire(int questionnaireID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaire(questionnaireID);

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

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetRangeDetails(RangeName);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public DataTable GetFeedbackQuestionnaireSelf(int questionnaireID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaireSelf(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public int GetQuestionListCount(string accountID, int questionnaireID)
        {
            int questionCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionCount = questionnaire_DAO.GetQuestionListCount(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionCount;
        }

        public DataTable GetProjectQuestionnaireInfo(int questionnaireID, int candidateID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetFeedbackQuestionnaire(questionnaireID, candidateID);

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
            int answerCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                answerCount = questionnaire_DAO.CalculateGraph(questionnaireID, candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return answerCount;
        }

        public DataTable GetQuestionnaireCategories(int questionnaireID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetQuestionnaireCategories(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public DataTable GetCategoryQuestions(int categoryID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dtResult = questionnaire_DAO.GetCategoryQuestions(categoryID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public int UpdateSubmitFlag(int candidateID, int submitFlag)
        {
            int result = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaire_DAO = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                result = questionnaire_DAO.UpdateSubmitFlag(candidateID, submitFlag);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

    }






}
