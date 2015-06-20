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
    public class FeedbackCategory_DAO:DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public FeedbackCategory_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"
        
        public List<FeedbackCategory_BE> category_BEList { get; set; }
        
        #endregion

        

        private void ShiftDataTableToBEList(DataTable dtCategory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            category_BEList = new List<FeedbackCategory_BE>();

            for (int recordCounter = 0; recordCounter < dtCategory.Rows.Count; recordCounter++)
            {
                FeedbackCategory_BE category_BE = new FeedbackCategory_BE();

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


}
