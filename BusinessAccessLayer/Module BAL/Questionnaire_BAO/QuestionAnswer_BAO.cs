using System;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class QuestionAnswer_BAO : Base_BAO
    {
        int result;
        /// <summary>
        /// Insert Question Answer
        /// </summary>
        /// <param name="questionAnswerBusinessEntity"></param>
        /// <returns></returns>
        public int AddQuestionAnswer(QuestionAnswer_BE questionAnswerBusinessEntity)
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
                QuestionAnswer_DAO questionAnswerDataAccessObject = new QuestionAnswer_DAO();
                result = questionAnswerDataAccessObject.AddQuestionAnswer(questionAnswerBusinessEntity);
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
            return result;
        }

        /// <summary>
        /// Get  Question Answer
        /// </summary>
        /// <param name="candidateId">candidate Id</param>
        /// <param name="questionID">question ID</param>
        /// <returns></returns>
        public string GetQuestionAnswer(int candidateId, int questionID)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;
            string result = "";

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                QuestionAnswer_DAO questionAnswerDataAccessObject = new QuestionAnswer_DAO();
                result = questionAnswerDataAccessObject.GetQuestionAnswer(candidateId, questionID);
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
            return result;
        }
    }




    public class Survey_QuestionAnswer_BAO : Base_BAO
    {
        int result;
        /// <summary>
        /// Insert Question Answer
        /// </summary>
        /// <param name="questionAnswerBusinessEntity"></param>
        /// <returns></returns>
        public int AddQuestionAnswer(Survey_QuestionAnswer_BE questionAnswerBusinessEntity)
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
                Survey_QuestionAnswer_DAO questionAnswerDataAccessObject = new Survey_QuestionAnswer_DAO();
                result = questionAnswerDataAccessObject.AddQuestionAnswer(questionAnswerBusinessEntity);
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
            return result;
        }

        /// <summary>
        /// Get Question Answer
        /// </summary>
        /// <param name="candidateId">candidate Id</param>
        /// <param name="questionID">question ID</param>
        /// <returns></returns>
        public string GetQuestionAnswer(int candidateId, int questionID)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;
            string result = "";

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                Survey_QuestionAnswer_DAO questionAnswerDataAccessObject = new Survey_QuestionAnswer_DAO();
                result = questionAnswerDataAccessObject.GetQuestionAnswer(candidateId, questionID);
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
            return result;
        }
    }

}
