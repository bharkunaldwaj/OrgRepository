using System;
using System.Collections.Generic;
using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;
using System.Data;

namespace Questionnaire_BAO
{
    public class Questions_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addQuestions;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Get range details
        /// </summary>
        /// <returns></returns>
        public DataTable getrange_data()
        {
            Questions_DAO QuestionsDataAccessObject = null;
            try
            {
                QuestionsDataAccessObject = new Questions_DAO();
                return QuestionsDataAccessObject.getrange_data();
            }
            catch
            { }
            return QuestionsDataAccessObject.getrange_data();
        }

        /// <summary>
        /// Insert Questions
        /// </summary>
        /// <param name="questionsBusinessEntity"></param>
        /// <returns></returns>
        public int AddQuestions(Questions_BE questionsBusinessEntity)
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
                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                addQuestions = questionsDataAccessObject.AddQuestions(questionsBusinessEntity);
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
            return addQuestions;
        }

        /// <summary>
        /// update Questions
        /// </summary>
        /// <param name="questionsBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateQuestions(Questions_BE questionsBusinessEntity)
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
                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                addQuestions = questionsDataAccessObject.UpdateQuestions(questionsBusinessEntity);
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
            return addQuestions;
        }

        /// <summary>
        /// Delete Questions
        /// </summary>
        /// <param name="questionsBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteQuestions(Questions_BE questionsBusinessEntity)
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
                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                addQuestions = questionsDataAccessObject.DeleteQuestions(questionsBusinessEntity);
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
            return addQuestions;
        }

        /// <summary>
        /// Get Questions by id
        /// </summary>
        /// <param name="questionsID">questions ID</param>
        /// <returns></returns>
        public List<Questions_BE> GetQuestionsByID(int questionsID)
        {
            List<Questions_BE> questionsBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                questionsDataAccessObject.GetQuestionsByID(questionsID);

                questionsBusinessEntityList = questionsDataAccessObject.questions_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionsBusinessEntityList;
        }

        /// <summary>
        /// Get Questions list
        /// </summary>
        /// <returns></returns>
        public List<Questions_BE> GetQuestionsList()
        {
            List<Questions_BE> questionsBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                questionsDataAccessObject.GetQuestionsList();

                questionsBusinessEntityList = questionsDataAccessObject.questions_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questionsBusinessEntityList;
        }

        /// <summary>
        /// Get Questions list by account id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtQuestionsList(string accountID)
        {
            DataTable dataTableQuestions = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                dataTableQuestions = questionsDataAccessObject.GetdtQuestionsList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableQuestions;
        }

        /// <summary>
        /// Get Questions List
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtQuestionsListnew(string accountID)
        {
            DataTable DataTableQuestions = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                DataTableQuestions = questionsDataAccessObject.GetdtQuestionsListnew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return DataTableQuestions;
        }

        /// <summary>
        /// Get Questions List Count
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int GetQuestionsListCount(string accountID)
        {
            int questionsCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questionsDataAccessObject = new Questions_DAO();
                questionsCount = questionsDataAccessObject.GetQuestionsListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionsCount;
        }
        #endregion

        /// <summary>
        /// Re sequence Question
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="increment">increment</param>
        public void ResequenceQuestion(int accountID, int questionnaireID, int increment)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questionDataAccessObject = new Questions_DAO();
                questionDataAccessObject.ResequenceQuestion(accountID, questionnaireID, increment);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }


    public class Survey_Questions_BAO : Base_BAO
    {

        #region "Private Member Variable"

        private int addQuestions;

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Get Range details
        /// </summary>
        /// <returns></returns>
        public DataTable getrange_data()
        {
            Survey_Questions_DAO questionDataAccess = null;
            //try
            //{
            questionDataAccess = new Survey_Questions_DAO();
            return questionDataAccess.getrange_data();
            //}
            //catch
            //{ }
            //return q_dao.getrange_data();
        }

        /// <summary>
        /// Insert Questions
        /// </summary>
        /// <param name="questionsBusinessEntity"></param>
        /// <returns></returns>
        public int AddQuestions(Survey_Questions_BE questionsBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            ////try
            ////{
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
            addQuestions = questionsDataAccessObject.AddQuestions(questionsBusinessEntity);
            //HandleWriteLog("End", new StackTrace(true));

            dbTransaction.Commit();
            conn.Close();
            ////}
            ////catch (Exception ex)
            ////{
            ////    if (dbTransaction != null)
            ////    {
            ////        dbTransaction.Rollback();
            ////    }

            ////    HandleException(ex);
            ////}
            return addQuestions;
        }

        /// <summary>
        /// Update Questions
        /// </summary>
        /// <param name="questionsBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateQuestions(Survey_Questions_BE questionsBusinessEntity)
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
                Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
                addQuestions = questionsDataAccessObject.UpdateQuestions(questionsBusinessEntity);
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
            return addQuestions;
        }

        /// <summary>
        /// delete Questions
        /// </summary>
        /// <param name="questionsBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteQuestions(Survey_Questions_BE questionsBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            //try
            //{
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
            addQuestions = questionsDataAccessObject.DeleteQuestions(questionsBusinessEntity);
            //HandleWriteLog("End", new StackTrace(true));

            dbTransaction.Commit();
            conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    if (dbTransaction != null)
            //    {
            //        dbTransaction.Rollback();
            //    }

            //    HandleException(ex);
            //}
            return addQuestions;
        }

        /// <summary>
        /// Get Questions by Questions id
        /// </summary>
        /// <param name="questionsID"></param>
        /// <returns></returns>
        public List<Survey_Questions_BE> GetQuestionsByID(int questionsID)
        {
            List<Survey_Questions_BE> questionsBusinessEntityList = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
            questionsDataAccessObject.GetQuestionsByID(questionsID);

            questionsBusinessEntityList = questionsDataAccessObject.questions_BEList;

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return questionsBusinessEntityList;
        }

        /// <summary>
        /// Get Questions list 
        /// </summary>
        /// <returns></returns>
        public List<Survey_Questions_BE> GetQuestionsList()
        {
            List<Survey_Questions_BE> questionsBusinessEntityList = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
            questionsDataAccessObject.GetQuestionsList();

            questionsBusinessEntityList = questionsDataAccessObject.questions_BEList;

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return questionsBusinessEntityList;
        }

        /// <summary>
        /// Get Questions list by account id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtQuestionsList(string accountID)
        {
            DataTable dataTableQuestions = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
            dataTableQuestions = questionsDataAccessObject.GetdtQuestionsList(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableQuestions;
        }

        /// <summary>
        /// Get Questions list  by account id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtQuestionsListnew(string accountID)
        {
            DataTable dataTableQuestions = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
            dataTableQuestions = questionsDataAccessObject.GetdtQuestionsListnew(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableQuestions;
        }

        /// <summary>
        /// Get Questions list count by account id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int GetQuestionsListCount(string accountID)
        {
            int questionsCount = 0;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Questions_DAO questionsDataAccessObject = new Survey_Questions_DAO();
            questionsCount = questionsDataAccessObject.GetQuestionsListCount(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return questionsCount;
        }
        #endregion

        /// <summary>
        /// Resequence Questions
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="increment">sequence number</param>
        public void ResequenceQuestion(int accountID, int questionnaireID, int increment)
        {
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Questions_DAO questionDataAccessObject = new Survey_Questions_DAO();
            questionDataAccessObject.ResequenceQuestion(accountID, questionnaireID, increment);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
        }
    }
}
