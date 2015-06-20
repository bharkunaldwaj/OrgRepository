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
    public class QuestionAnswer_BAO:Base_BAO
    {
        int result;

        public int AddQuestionAnswer(QuestionAnswer_BE questionAnswer_BE)
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
                QuestionAnswer_DAO questionAnswer_DAO = new QuestionAnswer_DAO();
                result = questionAnswer_DAO.AddQuestionAnswer(questionAnswer_BE);
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
                QuestionAnswer_DAO questionAnswer_DAO = new QuestionAnswer_DAO();
                result = questionAnswer_DAO.GetQuestionAnswer(candidateId, questionID);
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

        public int AddQuestionAnswer(Survey_QuestionAnswer_BE questionAnswer_BE)
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
                Survey_QuestionAnswer_DAO questionAnswer_DAO = new Survey_QuestionAnswer_DAO();
                result = questionAnswer_DAO.AddQuestionAnswer(questionAnswer_BE);
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
                Survey_QuestionAnswer_DAO questionAnswer_DAO = new Survey_QuestionAnswer_DAO();
                result = questionAnswer_DAO.GetQuestionAnswer(candidateId, questionID);
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
