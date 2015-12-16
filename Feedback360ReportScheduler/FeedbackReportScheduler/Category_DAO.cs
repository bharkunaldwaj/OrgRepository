using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Category_DAO
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Category_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"
        
        public List<Category_BE> category_BEList { get; set; }
        
        #endregion

        # region CRUD Operation
        /// <summary>
        /// Insert category
        /// </summary>
        /// <param name="category_BE"></param>
        /// <returns></returns>
        public int AddCategory(Category_BE category_BE)
        {
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {null,
                                                category_BE.AccountID,
                                                category_BE.Name,
                                                category_BE.Description,
                                                category_BE.Sequence,
                                                category_BE.ExcludeFromAnalysis,
                                                category_BE.Questionnaire,
                                                category_BE.ModifiedBy,
                                                category_BE.ModifiedDate,
                                                category_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspCategoryManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// update Category
        /// </summary>
        /// <param name="category_BE"></param>
        /// <returns></returns>
        public int UpdateCategory(Category_BE category_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {category_BE.CategoryID,
                                                category_BE.AccountID,
                                                category_BE.Name,
                                                category_BE.Description,
                                                category_BE.Sequence,
                                                category_BE.ExcludeFromAnalysis,
                                                category_BE.Questionnaire,
                                                category_BE.ModifiedBy,
                                                category_BE.ModifiedDate,
                                                category_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspCategoryManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="category_BE"></param>
        /// <returns></returns>
        public int DeleteCategory(Category_BE category_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {category_BE.CategoryID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspCategoryManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Get Category by Category id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="categoryID">Category id</param>
        /// <returns></returns>
        public int GetCategoryByID(int accountID, int categoryID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllCategory = new DataTable();
                object[] param = new object[3] {accountID, categoryID,"I" };

                dtAllCategory = cDataSrc.ExecuteDataSet("UspCategorySelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllCategory);
                returnValue = 1;

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Get Category List
        /// </summary>
        /// <returns></returns>
        public int GetCategoryList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllCategory = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAllCategory = cDataSrc.ExecuteDataSet("UspCategorySelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllCategory);
                returnValue = 1;

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Get Category List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtCategoryList(string accountID)
        {
            DataTable dtAllCategory = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[3] { Convert.ToInt32(accountID), null, "A" };

                dtAllCategory = cDataSrc.ExecuteDataSet("UspCategorySelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllCategory;
        }

        #endregion 
        /// <summary>
        /// Shift Category Data Table To BE List
        /// </summary>
        private void ShiftDataTableToBEList(DataTable dtCategory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            category_BEList = new List<Category_BE>();

            for (int recordCounter = 0; recordCounter < dtCategory.Rows.Count; recordCounter++)
            {
                Category_BE category_BE = new Category_BE();

                category_BE.CategoryID = Convert.ToInt32(dtCategory.Rows[recordCounter]["CategoryID"].ToString());
                category_BE.AccountID = Convert.ToInt32(dtCategory.Rows[recordCounter]["AccountID"].ToString());
                category_BE.Name = dtCategory.Rows[recordCounter]["CategoryName"].ToString();
                category_BE.Questionnaire = Convert.ToInt32(dtCategory.Rows[recordCounter]["QuestionnaireID"].ToString());
                category_BE.Description = dtCategory.Rows[recordCounter]["Description"].ToString();
                category_BE.Sequence = Convert.ToInt32(dtCategory.Rows[recordCounter]["Sequence"].ToString());
                category_BE.ExcludeFromAnalysis = Convert.ToBoolean(dtCategory.Rows[recordCounter]["ExcludeFromAnalysis"].ToString());
                category_BE.ModifiedBy = Convert.ToInt32(dtCategory.Rows[recordCounter]["ModifiedBy"].ToString());
                category_BE.ModifiedDate = Convert.ToDateTime(dtCategory.Rows[recordCounter]["ModifiedDate"].ToString());
                category_BE.IsActive = Convert.ToInt32(dtCategory.Rows[recordCounter]["IsActive"].ToString());

                category_BEList.Add(category_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }
        /// <summary>
        /// Get Category List Count by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetCategoryListCount(string condition)
        {
            int categoryCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[3] {Convert.ToInt32(accountID), null, "C" };

                //categoryCount = (int)cDataSrc.ExecuteScalar("UspCategorySelect", param, null);

                string sql = "SELECT Count(CategoryID) " +
                           " FROM   [Category] INNER JOIN" +
                            "  dbo.Account ON [Category].AccountID = dbo.Account.AccountID INNER JOIN " +
                            " [Questionnaire] on [Questionnaire].[QuestionnaireID]=[Category].[QuestionnaireID]" +
                            " WHERE [Category].[AccountID] =" + condition;

                //object[] param = new object[2] { condition, "A" };
                categoryCount = (int)cDataSrc.ExecuteScalar(sql, null);





                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryCount;
        }
        /// <summary>
        /// Select Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Questionnaireid">Questionnaire id</param>
        /// <returns></returns>
        public DataTable getcategory(int accountID, int questionnaireid)
        {
            DataTable categoryid = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] {Convert.ToInt32(accountID), questionnaireid, "S" };

                categoryid = cDataSrc.ExecuteDataSet("UspCategorySelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryid;
        }
        /// <summary>
        /// Select Questionnaire Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Questionnaireid">Questionnaire id</param>
        /// <returns></returns>
        public DataTable SelectQuestionnaireCategory(int accountID, int questionnaireid)
        {
            DataTable categoryid = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), questionnaireid, "T" };

                categoryid = cDataSrc.ExecuteDataSet("UspCategorySelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryid;
        }
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

                object[] param = new object[3] { accountID, questionnaireID, increment };

                cDataSrc.ExecuteNonQuery("UspUpdateCategorySequence", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) 
            { 
                HandleException(ex); 
            }
        }
        /// <summary>
        /// Get Category List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtnewCategoryList(string condition)
        {
            DataTable dtAllCategory = new DataTable();
            try
            {
            
                string sql = "SELECT [CategoryID], " +
                            "[Category].[AccountID], " +
                            "[CategoryName], " +
                            "[Category].[Description], " +
                            "[Sequence]," +
                            "[ExcludeFromAnalysis], " +
                            "[Category].[QuestionnaireID], " +
                            "[Questionnaire].[QSTNName], " +
                            "[ModifiedBy], " +
                            "[Category].[IsActive], " +
                            "[Account].[Code] " +
                           
                            " FROM   [Category] INNER JOIN" +
                            "  dbo.Account ON [Category].AccountID = dbo.Account.AccountID INNER JOIN " +
                            " [Questionnaire] on [Questionnaire].[QuestionnaireID]=[Category].[QuestionnaireID]" +
                            " WHERE [Category].[AccountID] =" + condition +
                            " order by dbo.Account.[Code],Sequence, [Category].ModifiedDate, CategoryName Desc";

                dtAllCategory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllCategory;
        }
        /// <summary>
        /// Use to handle exception
        /// </summary>
        /// <param name="ex"></param>
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
