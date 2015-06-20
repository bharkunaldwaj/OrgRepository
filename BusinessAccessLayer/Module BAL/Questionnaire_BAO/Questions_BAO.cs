using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;
using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class Questions_BAO : Base_BAO
    {

        #region "Private Member Variable"

        private int addQuestions;

        #endregion

        #region CRUD Operations


        public DataTable getrange_data()
        {
            Questions_DAO q_dao=null;
            try
            {
                q_dao = new Questions_DAO();
                return q_dao.getrange_data();
            }
            catch
            { }
            return q_dao.getrange_data();
        }




        public int AddQuestions(Questions_BE questions_BE)
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
                Questions_DAO questions_DAO = new Questions_DAO();
                addQuestions = questions_DAO.AddQuestions(questions_BE);
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

        public int UpdateQuestions(Questions_BE questions_BE)
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
                Questions_DAO questions_DAO = new Questions_DAO();
                addQuestions = questions_DAO.UpdateQuestions(questions_BE);
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

        public int DeleteQuestions(Questions_BE questions_BE)
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
                Questions_DAO questions_DAO = new Questions_DAO();
                addQuestions = questions_DAO.DeleteQuestions(questions_BE);
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

        public List<Questions_BE> GetQuestionsByID(int questionsID)
        {
            List<Questions_BE> questions_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questions_DAO = new Questions_DAO();
                questions_DAO.GetQuestionsByID(questionsID);

                questions_BEList = questions_DAO.questions_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questions_BEList;
        }

        public List<Questions_BE> GetQuestionsList()
        {
            List<Questions_BE> questions_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questions_DAO = new Questions_DAO();
                questions_DAO.GetQuestionsList();

                questions_BEList = questions_DAO.questions_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return questions_BEList;
        }

        public DataTable GetdtQuestionsList(string accountID)
        {
            DataTable dtQuestions = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questions_DAO = new Questions_DAO();
                dtQuestions = questions_DAO.GetdtQuestionsList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtQuestions;
        }



        public DataTable GetdtQuestionsListnew(string accountID)
        {
            DataTable dtQuestions = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questions_DAO = new Questions_DAO();
                dtQuestions = questions_DAO.GetdtQuestionsListnew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtQuestions;
        }



        public int GetQuestionsListCount(string accountID)
        {
            int questionsCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO questions_DAO = new Questions_DAO();
                questionsCount = questions_DAO.GetQuestionsListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionsCount;
        }

       

        #endregion

        public void ResequenceQuestion(int accountID, int questionnaireID, int increment)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Questions_DAO question_DAO = new Questions_DAO();
                question_DAO.ResequenceQuestion(accountID, questionnaireID, increment);

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


        public DataTable getrange_data()
        {
            Survey_Questions_DAO q_dao = null;
            //try
            //{
                q_dao = new Survey_Questions_DAO();
                return q_dao.getrange_data();
            //}
            //catch
            //{ }
            //return q_dao.getrange_data();
        }




        public int AddQuestions(Survey_Questions_BE questions_BE)
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
                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                addQuestions = questions_DAO.AddQuestions(questions_BE);
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

        public int UpdateQuestions(Survey_Questions_BE questions_BE)
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
                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                addQuestions = questions_DAO.UpdateQuestions(questions_BE);
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

        public int DeleteQuestions(Survey_Questions_BE questions_BE)
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
                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                addQuestions = questions_DAO.DeleteQuestions(questions_BE);
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

        public List<Survey_Questions_BE> GetQuestionsByID(int questionsID)
        {
            List<Survey_Questions_BE> questions_BEList = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                questions_DAO.GetQuestionsByID(questionsID);

                questions_BEList = questions_DAO.questions_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return questions_BEList;
        }

        public List<Survey_Questions_BE> GetQuestionsList()
        {
            List<Survey_Questions_BE> questions_BEList = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                questions_DAO.GetQuestionsList();

                questions_BEList = questions_DAO.questions_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return questions_BEList;
        }

        public DataTable GetdtQuestionsList(string accountID)
        {
            DataTable dtQuestions = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                dtQuestions = questions_DAO.GetdtQuestionsList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtQuestions;
        }



        public DataTable GetdtQuestionsListnew(string accountID)
        {
            DataTable dtQuestions = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                dtQuestions = questions_DAO.GetdtQuestionsListnew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtQuestions;
        }



        public int GetQuestionsListCount(string accountID)
        {
            int questionsCount = 0;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Questions_DAO questions_DAO = new Survey_Questions_DAO();
                questionsCount = questions_DAO.GetQuestionsListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return questionsCount;
        }



        #endregion

        public void ResequenceQuestion(int accountID, int questionnaireID, int increment)
        {
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Questions_DAO question_DAO = new Survey_Questions_DAO();
                question_DAO.ResequenceQuestion(accountID, questionnaireID, increment);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
        }
    }







}
