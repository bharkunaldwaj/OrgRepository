using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class Category_BAO 
    {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        #region CRUD Operations

        public int AddCategory(Category_BE category_BE)
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
                Category_DAO category_DAO = new Category_DAO();
                addCategory = category_DAO.AddCategory(category_BE);
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
            return addCategory;
        }

        public int UpdateCategory(Category_BE category_BE)
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
                Category_DAO category_DAO = new Category_DAO();
                addCategory = category_DAO.UpdateCategory(category_BE);
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
            return addCategory;
        }

        public int DeleteCategory(Category_BE category_BE)
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
                Category_DAO category_DAO = new Category_DAO();
                addCategory = category_DAO.DeleteCategory(category_BE);
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
            return addCategory;
        }

        public List<Category_BE> GetCategoryByID(int accountID, int categoryID)
        {
            List<Category_BE> category_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO category_DAO = new Category_DAO();
                category_DAO.GetCategoryByID(accountID, categoryID);

                category_BEList = category_DAO.category_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return category_BEList;
        }

        public List<Category_BE> GetCategoryList()
        {
            List<Category_BE> category_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO category_DAO = new Category_DAO();
                category_DAO.GetCategoryList();

                category_BEList = category_DAO.category_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return category_BEList;
        }

        public DataTable GetdtCategoryList(string accountID)
        {
            DataTable dtCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO category_DAO = new Category_DAO();
                dtCategory = category_DAO.GetdtCategoryList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategory;
        }

        public DataTable GetdtnewCategoryList(string accountID)
        {
            DataTable dtCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO category_DAO = new Category_DAO();
                dtCategory = category_DAO.GetdtnewCategoryList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategory;
        }

        public int GetCategoryListCount(string accountID)
        {
            int categoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO category_DAO = new Category_DAO();
                categoryCount = category_DAO.GetCategoryListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryCount;
        }

        public DataTable SelectCategory(int accountID, int Questionnaireid)
        {
            DataTable categoryid = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO Category_DAO = new Category_DAO();
                categoryid = Category_DAO.getcategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryid;
        }

        public DataTable SelectQuestionnaireCategory(int accountID, int Questionnaireid)
        {
            DataTable categoryid = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO Category_DAO = new Category_DAO();
                categoryid = Category_DAO.SelectQuestionnaireCategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryid;
        }

        #endregion

        public void ResequenceCategory(int accountID, int questionnaireID, int increment)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO category_DAO = new Category_DAO();
                category_DAO.ResequenceCategory(accountID, questionnaireID, increment);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
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
