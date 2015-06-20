using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
using feedbackFramework_DAO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Category_DAO:DAO_Base
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

        public int AddCategory(Category_BE category_BE)
        {
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[12] {null,
                                                category_BE.AccountID,
                                                category_BE.Name,
                                                category_BE.Title,
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

        public int UpdateCategory(Category_BE category_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[12] {category_BE.CategoryID,
                                                category_BE.AccountID,
                                                category_BE.Name,
                                                category_BE.Title,
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

        public int DeleteCategory(Category_BE category_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[12] {category_BE.CategoryID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspCategoryManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
    
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

                HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

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

                HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

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
                category_BE.Title = dtCategory.Rows[recordCounter]["CategoryTitle"].ToString();
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

        public DataTable GetdtnewCategoryList(string condition)
        {
            DataTable dtAllCategory = new DataTable();
            try
            {
            
                string sql = "SELECT [CategoryID], " +
                            "[Category].[AccountID], " +
                            "[CategoryName], " +
                            "[CategoryTitle], " +
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

    }





















    public class Survey_Category_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_Category_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Survey_Category_BE> category_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public int AddCategory(Survey_Category_BE category_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[14] {null,
                                                category_BE.AccountID,
                                                category_BE.Name,
                                                category_BE.Title,
                                                category_BE.Description,
                                                category_BE.Sequence,
                                                category_BE.ExcludeFromAnalysis,
                                                category_BE.Questionnaire,
                                                category_BE.ModifiedBy,
                                                category_BE.ModifiedDate,
                                                category_BE.IsActive,
                                                "I",
                                                category_BE.IntroImgFileName,
                                                category_BE.IntroPdfFileName};

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspCategoryManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateCategory(Survey_Category_BE category_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[14] {category_BE.CategoryID,
                                                category_BE.AccountID,
                                                category_BE.Name,
                                                category_BE.Title,
                                                category_BE.Description,
                                                category_BE.Sequence,
                                                category_BE.ExcludeFromAnalysis,
                                                category_BE.Questionnaire,
                                                category_BE.ModifiedBy,
                                                category_BE.ModifiedDate,
                                                category_BE.IsActive,
                                                "U" ,
                                                category_BE.IntroImgFileName,  
                                                category_BE.IntroPdfFileName};

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspCategoryManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteCategory(Survey_Category_BE category_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[12] {category_BE.CategoryID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspCategoryManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetCategoryByID(int accountID, int categoryID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllCategory = new DataTable();
                object[] param = new object[3] { accountID, categoryID, "I" };

                dtAllCategory = cDataSrc.ExecuteDataSet("Survey_UspCategorySelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllCategory);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetCategoryList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllCategory = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAllCategory = cDataSrc.ExecuteDataSet("Survey_UspCategorySelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllCategory);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtCategoryList(string accountID)
        {
            DataTable dtAllCategory = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), null, "A" };

                dtAllCategory = cDataSrc.ExecuteDataSet("Survey_UspCategorySelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllCategory;
        }

        #endregion

        private void ShiftDataTableToBEList(DataTable dtCategory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            category_BEList = new List<Survey_Category_BE>();

            for (int recordCounter = 0; recordCounter < dtCategory.Rows.Count; recordCounter++)
            {
                Survey_Category_BE category_BE = new Survey_Category_BE();

                category_BE.CategoryID = Convert.ToInt32(dtCategory.Rows[recordCounter]["CategoryID"].ToString());
                category_BE.AccountID = Convert.ToInt32(dtCategory.Rows[recordCounter]["AccountID"].ToString());
                category_BE.Name = dtCategory.Rows[recordCounter]["CategoryName"].ToString();
                category_BE.Title = dtCategory.Rows[recordCounter]["CategoryTitle"].ToString();
                category_BE.Questionnaire = Convert.ToInt32(dtCategory.Rows[recordCounter]["QuestionnaireID"].ToString());
                category_BE.Description = dtCategory.Rows[recordCounter]["Description"].ToString();
                category_BE.Sequence = Convert.ToInt32(dtCategory.Rows[recordCounter]["Sequence"].ToString());
                category_BE.ExcludeFromAnalysis = Convert.ToBoolean(dtCategory.Rows[recordCounter]["ExcludeFromAnalysis"].ToString());
                category_BE.ModifiedBy = Convert.ToInt32(dtCategory.Rows[recordCounter]["ModifiedBy"].ToString());
                category_BE.ModifiedDate = Convert.ToDateTime(dtCategory.Rows[recordCounter]["ModifiedDate"].ToString());
                category_BE.IsActive = Convert.ToInt32(dtCategory.Rows[recordCounter]["IsActive"].ToString());
                category_BE.IntroImgFileName = Convert.ToString(dtCategory.Rows[recordCounter]["IntroImgFileName"]);
                category_BE.IntroPdfFileName = Convert.ToString(dtCategory.Rows[recordCounter]["IntroPdfFileName"]);

                category_BEList.Add(category_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetCategoryListCount(string condition)
        {
            int categoryCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[3] {Convert.ToInt32(accountID), null, "C" };

                //categoryCount = (int)cDataSrc.ExecuteScalar("UspCategorySelect", param, null);

                string sql = "SELECT Count(CategoryID) " +
                           " FROM   [Survey_Category] INNER JOIN" +
                            "  dbo.Account ON [Survey_Category].AccountID = dbo.Account.AccountID INNER JOIN " +
                            " [Survey_Questionnaire] on [Survey_Questionnaire].[QuestionnaireID]=[Survey_Category].[QuestionnaireID]" +
                            " WHERE [Survey_Category].[AccountID] =" + condition;

                //object[] param = new object[2] { condition, "A" };
                categoryCount = (int)cDataSrc.ExecuteScalar(sql, null);





                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryCount;
        }

        public DataTable getcategory(int accountID, int questionnaireid)
        {
            DataTable categoryid = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), questionnaireid, "S" };

                categoryid = cDataSrc.ExecuteDataSet("Survey_UspCategorySelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryid;
        }

        public DataTable SelectQuestionnaireCategory(int accountID, int questionnaireid)
        {
            DataTable categoryid = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), questionnaireid, "T" };

                categoryid = cDataSrc.ExecuteDataSet("Survey_UspCategorySelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return categoryid;
        }

        public void ResequenceCategory(int accountID, int questionnaireID, int increment)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, questionnaireID, increment };

                cDataSrc.ExecuteNonQuery("Survey_UspUpdateCategorySequence", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public DataTable GetdtnewCategoryList(string condition)
        {
            DataTable dtAllCategory = new DataTable();
            try
            {

                string sql = "SELECT [CategoryID], " +
                            "[Survey_Category].[AccountID], " +
                            "[CategoryName], " +
                            "[CategoryTitle], " +
                            "[Survey_Category].[Description], " +
                            "[Sequence]," +
                            "[ExcludeFromAnalysis], " +
                            "[Survey_Category].[QuestionnaireID], " +
                            "[Survey_Questionnaire].[QSTNName], " +
                            "[ModifiedBy], " +
                            "[Survey_Category].[IsActive], " +
                            "[Account].[Code], " +
                            "[Survey_Category].[IntroPdfFileName] " +

                            " FROM   [Survey_Category] INNER JOIN" +
                            "  dbo.Account ON [Survey_Category].AccountID = dbo.Account.AccountID INNER JOIN " +
                            " [Survey_Questionnaire] on [Survey_Questionnaire].[QuestionnaireID]=[Survey_Category].[QuestionnaireID]";
                if (condition.Contains("Survey_Project"))
                    sql = sql + " Inner JOIN [Survey_Project] on [Survey_Project].[QuestionnaireID]=[Survey_Category].[QuestionnaireID]";

                sql = sql + " WHERE [Survey_Category].[AccountID] =" + condition +
                            " order by dbo.Account.[Code],Sequence, [Survey_Category].ModifiedDate, CategoryName Desc";

                dtAllCategory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllCategory;
        }

    }


}
