using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class Category_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert category
        /// </summary>
        /// <param name="categoryBusinessEntity"></param>
        /// <returns></returns>
        public int AddCategory(Category_BE categoryBusinessEntity)
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
                Category_DAO categoryDataAccessObject = new Category_DAO();
                addCategory = categoryDataAccessObject.AddCategory(categoryBusinessEntity);
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

        /// <summary>
        /// update Category
        /// </summary>
        /// <param name="categoryBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateCategory(Category_BE categoryBusinessEntity)
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
                Category_DAO categoryDataAccessObject = new Category_DAO();
                addCategory = categoryDataAccessObject.UpdateCategory(categoryBusinessEntity);
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

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="categoryBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteCategory(Category_BE categoryBusinessEntity)
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
                Category_DAO categoryDataAccessObject = new Category_DAO();
                addCategory = categoryDataAccessObject.DeleteCategory(categoryBusinessEntity);
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

        /// <summary>
        /// Get Category by Category id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="categoryID">Category id</param>
        /// <returns></returns>
        public List<Category_BE> GetCategoryByID(int accountID, int categoryID)
        {
            List<Category_BE> categoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO category_DAO = new Category_DAO();
                category_DAO.GetCategoryByID(accountID, categoryID);

                categoryBusinessEntityList = category_DAO.category_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return categoryBusinessEntityList;
        }

        /// <summary>
        /// Get Category List
        /// </summary>
        /// <returns></returns>
        public List<Category_BE> GetCategoryList()
        {
            List<Category_BE> categoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO categoryDataAccessObject = new Category_DAO();
                categoryDataAccessObject.GetCategoryList();

                categoryBusinessEntityList = categoryDataAccessObject.category_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return categoryBusinessEntityList;
        }

        /// <summary>
        /// Get Category List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtCategoryList(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO categoryDataAccessObject = new Category_DAO();
                dataTableCategory = categoryDataAccessObject.GetdtCategoryList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
        }

        /// <summary>
        /// Get Category List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtnewCategoryList(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO categoryDataAccessObject = new Category_DAO();
                dataTableCategory = categoryDataAccessObject.GetdtnewCategoryList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
        }

        /// <summary>
        /// Get Category List Count by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetCategoryListCount(string accountID)
        {
            int categoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO categoryDataAccessObject = new Category_DAO();
                categoryCount = categoryDataAccessObject.GetCategoryListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryCount;
        }

        /// <summary>
        /// Select Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Questionnaireid">Questionnaire id</param>
        /// <returns></returns>
        public DataTable SelectCategory(int accountID, int Questionnaireid)
        {
            DataTable categoryid = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO CategoryDataAccessObject = new Category_DAO();
                categoryid = CategoryDataAccessObject.getcategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryid;
        }

        /// <summary>
        /// Select Questionnaire Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Questionnaireid">Questionnaire id</param>
        /// <returns></returns>
        public DataTable SelectQuestionnaireCategory(int accountID, int Questionnaireid)
        {
            DataTable categoryid = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO CategoryDataAccessObject = new Category_DAO();
                categoryid = CategoryDataAccessObject.SelectQuestionnaireCategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryid;
        }

        #endregion
        /// <summary>
        /// Resequence Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="increment">sequence number</param>
        public void ResequenceCategory(int accountID, int questionnaireID, int increment)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Category_DAO categoryDataAccessObject = new Category_DAO();
                categoryDataAccessObject.ResequenceCategory(accountID, questionnaireID, increment);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    public class Survey_Category_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert category
        /// </summary>
        /// <param name="categoryBusinessEntity"></param>
        /// <returns></returns>
        public int AddCategory(Survey_Category_BE categoryBusinessEntity)
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
                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                addCategory = categoryDataAccessObject.AddCategory(categoryBusinessEntity);
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

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="categoryBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateCategory(Survey_Category_BE categoryBusinessEntity)
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
                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                addCategory = categoryDataAccessObject.UpdateCategory(categoryBusinessEntity);
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

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="categoryBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteCategory(Survey_Category_BE categoryBusinessEntity)
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
                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                addCategory = categoryDataAccessObject.DeleteCategory(categoryBusinessEntity);
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

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="categoryID">category ID</param>
        /// <returns></returns>
        public List<Survey_Category_BE> GetCategoryByID(int accountID, int categoryID)
        {
            List<Survey_Category_BE> categoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                categoryDataAccessObject.GetCategoryByID(accountID, categoryID);

                categoryBusinessEntityList = categoryDataAccessObject.category_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return categoryBusinessEntityList;
        }

        /// <summary>
        /// Get Category List
        /// </summary>
        /// <returns></returns>
        public List<Survey_Category_BE> GetCategoryList()
        {
            List<Survey_Category_BE> categoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                categoryDataAccessObject.GetCategoryList();

                categoryBusinessEntityList = categoryDataAccessObject.category_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return categoryBusinessEntityList;
        }

        /// <summary> 
        /// Get Category List by account id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtCategoryList(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                dataTableCategory = categoryDataAccessObject.GetdtCategoryList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
        }

        /// <summary> 
        /// Get Category List by account id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtnewCategoryList(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                dataTableCategory = categoryDataAccessObject.GetdtnewCategoryList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
        }

        /// <summary> 
        /// Get Category List count by account id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetCategoryListCount(string accountID)
        {
            int categoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                categoryCount = categoryDataAccessObject.GetCategoryListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryCount;
        }

        /// <summary>
        /// Get Category 
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Questionnaireid">Questionnaire id</param>
        /// <returns></returns>
        public DataTable SelectCategory(int accountID, int Questionnaireid)
        {
            DataTable categoryid = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO CategoryDataAccessObject = new Survey_Category_DAO();
                categoryid = CategoryDataAccessObject.getcategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryid;
        }

        /// <summary>
        /// Select Questionnaire Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Questionnaireid">Questionnaire id</param>
        /// <returns></returns>
        public DataTable SelectQuestionnaireCategory(int accountID, int Questionnaireid)
        {
            DataTable categoryid = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO CategoryDataAccessObject = new Survey_Category_DAO();
                categoryid = CategoryDataAccessObject.SelectQuestionnaireCategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryid;
        }

        #endregion
        /// <summary>
        /// Resequence Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="questionnaireID">questionnaire ID</param>
        /// <param name="increment">sequence number</param>
        public void ResequenceCategory(int accountID, int questionnaireID, int increment)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Category_DAO categoryDataAccessObject = new Survey_Category_DAO();
                categoryDataAccessObject.ResequenceCategory(accountID, questionnaireID, increment);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

    }
}
