using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;

using System.Data;

namespace Questionnaire_BAO
{
    public class Questionnaire_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addQuestionnaire;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Questionnaire
        /// </summary>
        /// <param name="questionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int AddQuestionnaire(Questionnaire_BE.Questionnaire_BE questionnaireBusinessEntity)
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
                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                addQuestionnaire = questionnaireDataAccessObject.AddQuestionnaire(questionnaireBusinessEntity);
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

        /// <summary>
        /// update Questionnaire
        /// </summary>
        /// <param name="questionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateQuestionnaire(Questionnaire_BE.Questionnaire_BE questionnaireBusinessEntity)
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
                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                addQuestionnaire = questionnaireDataAccessObject.UpdateQuestionnaire(questionnaireBusinessEntity);
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

        /// <summary>
        /// Delete Questionnaire
        /// </summary>
        /// <param name="questionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteQuestionnaire(Questionnaire_BE.Questionnaire_BE questionnaireBusinessEntity)
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
                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                addQuestionnaire = questionnaireDataAccessObject.DeleteQuestionnaire(questionnaireBusinessEntity);
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

        /// <summary>
        /// Get Questionnaire by Questionnaire id
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public List<Questionnaire_BE.Questionnaire_BE> GetQuestionnaireByID(int questionnaireID)
        {
            List<Questionnaire_BE.Questionnaire_BE> questionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                questionnaireDataAccessObject.GetQuestionnaireByID(questionnaireID);

                questionnaireBusinessEntityList = questionnaireDataAccessObject.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaireBusinessEntityList;
        }

        /// <summary>
        /// Get Questionnaire list
        /// </summary>
        /// <returns></returns>
        public List<Questionnaire_BE.Questionnaire_BE> GetQuestionnaireList()
        {
            List<Questionnaire_BE.Questionnaire_BE> questionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                questionnaireDataAccessObject.GetQuestionnaireList();

                questionnaireBusinessEntityList = questionnaireDataAccessObject.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaireBusinessEntityList;
        }

        /// <summary>
        /// Get Questionnaire list by account id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtQuestionnaireList(string accountID)
        {
            DataTable dataTableQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableQuestionnaire = questionnaireDataAccessObject.GetdtQuestionnaireList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableQuestionnaire;
        }

        /// <summary>
        /// Get Questionnaire list count
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int GetQuestionnaireListCount(string accountID)
        {
            int questionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                questionnaireCount = questionnaireDataAccessObject.GetQuestionnaireListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionnaireCount;
        }

        /// <summary>
        /// Get project Questionnaire
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetProjectQuestionnaire(int projectID)
        {
            DataTable dataTableProjectQuestionnaire = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO projectauestionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableProjectQuestionnaire = projectauestionnaireDataAccessObject.GetProjectQuestionnaire(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProjectQuestionnaire;
        }

        #endregion

        /// <summary>
        /// Get feedback Questionnaire by Questionnaire id
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public DataTable GetFeedbackQuestionnaire(int questionnaireID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaire(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Feedback Questionnaire By RelationShip
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="relationship">relationship</param>
        /// <returns></returns>
        public DataTable GetFeedbackQuestionnaireByRelationShip(int accountID, int projectID,
            int questionnaireID, string relationship)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaireByRelationShip(accountID, projectID, questionnaireID, relationship);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Feed back Questionnaire Self
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public DataTable GetFeedbackQuestionnaireSelf(int questionnaireID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaireSelf(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Feedback Questionnaire Self By RelationShip
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="relationship">relationship</param>
        /// <returns></returns>
        public DataTable GetFeedbackQuestionnaireSelfByRelationShip(int accountID, int projectID,
            int questionnaireID, string relationship)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaireSelfByRelationShip(accountID, projectID, questionnaireID, relationship);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Question List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <returns></returns>
        public int GetQuestionListCount(string accountID, int questionnaireID)
        {
            int questionCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                questionCount = questionnaireDataAccessObject.GetQuestionListCount(questionnaireID);

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
                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                QuestionnaireID = questionnaireDataAccessObject.GetQuestionnaireID(projectID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return QuestionnaireID;
        }

        /// <summary>
        /// Get Project Questionnaire details
        /// </summary>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="candidateID">candidate ID</param>
        /// <returns></returns>
        public DataTable GetProjectQuestionnaireInfo(int questionnaireID, int candidateID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaire(questionnaireID, candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Calculate Graph details
        /// </summary>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="candidateID">candidate ID</param>
        /// <returns></returns>
        public int CalculateGraph(int questionnaireID, int candidateID)
        {
            int answerCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                answerCount = questionnaireDataAccessObject.CalculateGraph(questionnaireID, candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return answerCount;
        }

        /// <summary>
        /// Get Questionnaire Categories
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public DataTable GetQuestionnaireCategories(int questionnaireID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetQuestionnaireCategories(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Questionnaire Categories By Relation Ship
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="projectID"></param>
        /// <param name="questionnaireID"></param>
        /// <param name="relationship"></param>
        /// <returns></returns>
        public DataTable GetQuestionnaireCategoriesByRelationShip(int accountID, int projectID,
            int questionnaireID, string relationship)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetQuestionnaireCategoriesByRelationShip(accountID, projectID, questionnaireID, relationship);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Category Questions by categoryID
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <returns></returns>
        public DataTable GetCategoryQuestions(int categoryID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetCategoryQuestions(categoryID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Update Submit Flag
        /// </summary>
        /// <param name="candidateID">candidate ID</param>
        /// <param name="submitFlag">submit Flag</param>
        /// <returns></returns>
        public int UpdateSubmitFlag(int candidateID, int submitFlag)
        {
            int result = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Questionnaire_DAO();
                result = questionnaireDataAccessObject.UpdateSubmitFlag(candidateID, submitFlag);

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
        /// <summary>
        /// Insert Questionnaire
        /// </summary>
        /// <param name="questionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int AddQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE questionnaireBusinessEntity)
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
                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                addQuestionnaire = questionnaireDataAccessObject.AddQuestionnaire(questionnaireBusinessEntity);
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

        /// <summary>
        /// Update Questionnaire
        /// </summary>
        /// <param name="questionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE questionnaireBusinessEntity)
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
                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                addQuestionnaire = questionnaireDataAccessObject.UpdateQuestionnaire(questionnaireBusinessEntity);
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

        /// <summary>
        /// Delete Questionnaire
        /// </summary>
        /// <param name="questionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteQuestionnaire(Questionnaire_BE.Survey_Questionnaire_BE questionnaireBusinessEntity)
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
                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                addQuestionnaire = questionnaireDataAccessObject.DeleteQuestionnaire(questionnaireBusinessEntity);
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

        /// <summary>
        /// Get Questionnaire by Questionnaire id
        /// </summary>
        /// <param name="questionnaireID">Questionnaire id</param>
        /// <returns></returns>
        public List<Questionnaire_BE.Survey_Questionnaire_BE> GetQuestionnaireByID(int questionnaireID)
        {
            List<Questionnaire_BE.Survey_Questionnaire_BE> questionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionnaireDataAccessObject.GetQuestionnaireByID(questionnaireID);

                questionnaireBusinessEntityList = questionnaireDataAccessObject.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaireBusinessEntityList;
        }

        /// <summary>
        /// Get Questionnaire list
        /// </summary>
        /// <returns></returns>
        public List<Questionnaire_BE.Survey_Questionnaire_BE> GetQuestionnaireList()
        {
            List<Questionnaire_BE.Survey_Questionnaire_BE> questionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionnaireDataAccessObject.GetQuestionnaireList();

                questionnaireBusinessEntityList = questionnaireDataAccessObject.Questionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionnaireBusinessEntityList;
        }

        /// <summary>
        /// Get Questionnaire list by account id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtQuestionnaireList(string accountID)
        {
            DataTable dataTableQuestionnaire = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
            dataTableQuestionnaire = questionnaireDataAccessObject.GetdtQuestionnaireList(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableQuestionnaire;
        }

        /// <summary>
        /// Get Questionnaire list count by account id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int GetQuestionnaireListCount(string accountID)
        {
            int questionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionnaireCount = questionnaireDataAccessObject.GetQuestionnaireListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionnaireCount;
        }

        /// <summary>
        /// Get project Questionnaire
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetProjectQuestionnaire(int projectID)
        {
            DataTable dataTableProjectQuestionnaire = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO projectauestionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dataTableProjectQuestionnaire = projectauestionnaireDataAccessObject.GetProjectQuestionnaire(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProjectQuestionnaire;
        }

        #endregion

        /// <summary>
        /// Get feedback Questionnaire by Questionnaire id
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public DataTable GetFeedbackQuestionnaire(int questionnaireID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaire(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Range details
        /// </summary>
        /// <param name="RangeName"></param>
        /// <returns></returns>
        public DataTable GetRangeDetails(string RangeName)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetRangeDetails(RangeName);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Feedback Questionnaire for Self by questionnaire id
        /// </summary>
        /// <param name="questionnaireID">questionnaire ID </param>
        /// <returns></returns>
        public DataTable GetFeedbackQuestionnaireSelf(int questionnaireID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaireSelf(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Question List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <returns></returns>
        public int GetQuestionListCount(string accountID, int questionnaireID)
        {
            int questionCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                questionCount = questionnaireDataAccessObject.GetQuestionListCount(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionCount;
        }

        /// <summary>
        /// Get Project Questionnaire details
        /// </summary>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="candidateID">candidate ID</param>
        /// <returns></returns>
        public DataTable GetProjectQuestionnaireInfo(int questionnaireID, int candidateID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetFeedbackQuestionnaire(questionnaireID, candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Calculated Graph
        /// </summary>
        /// <param name="questionnaireID"></param>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        public int CalculateGraph(int questionnaireID, int candidateID)
        {
            int answerCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                answerCount = questionnaireDataAccessObject.CalculateGraph(questionnaireID, candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return answerCount;
        }

        /// <summary>
        /// Get Questionnaire Categories by questionnaireID
        /// </summary>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <returns></returns>
        public DataTable GetQuestionnaireCategories(int questionnaireID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetQuestionnaireCategories(questionnaireID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Category Questions by categoryID
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <returns></returns>
        public DataTable GetCategoryQuestions(int categoryID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                dataTableResult = questionnaireDataAccessObject.GetCategoryQuestions(categoryID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Update Update Submit Flag
        /// </summary>
        /// <param name="candidateID">candidate ID</param>
        /// <param name="submitFlag">submitFlag</param>
        /// <returns></returns>
        public int UpdateSubmitFlag(int candidateID, int submitFlag)
        {
            int result = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questionnaire_DAO.Survey_Questionnaire_DAO questionnaireDataAccessObject = new Questionnaire_DAO.Survey_Questionnaire_DAO();
                result = questionnaireDataAccessObject.UpdateSubmitFlag(candidateID, submitFlag);

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
