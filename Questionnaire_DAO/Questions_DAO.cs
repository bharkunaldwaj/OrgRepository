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
    public class Questions_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Questions_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Questions_BE> questions_BEList { get; set; }

        #endregion

        # region CRUD Operation


        public DataTable getrange_data()
        {
            DataTable ddt=null;
            try
            {
                ddt = cDataSrc.ExecuteDataSet("select Range_Title from Question_Range").Tables[0];
                cDataSrc = null;

            }

            catch
            { }
            return ddt;
        }

        public int AddQuestions(Questions_BE questions_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[28] {null,
                //                                questions_BE.AccountID,
                //                                questions_BE.CompanyID,
                //                                questions_BE.QuestionnaireID,
                //                                questions_BE.QuestionTypeID,
                //                                questions_BE.CateogryID,
                //                                questions_BE.Sequence,
                //                                questions_BE.Validation,
                //                                questions_BE.ValidationText,
                //                                questions_BE.Title,
                //                                questions_BE.Description,
                //                                questions_BE.DescriptionSelf,
                //                                questions_BE.Hint,
                //                                questions_BE.Token,
                //                                questions_BE.TokenText,
                //                                questions_BE.LengthMIN,
                //                                questions_BE.LengthMAX,
                //                                questions_BE.Multiline,
                //                                questions_BE.LowerLabel,
                //                                questions_BE.UpperLabel,
                //                                questions_BE.LowerBound,
                //                                questions_BE.UpperBound,
                //                                questions_BE.Increment,
                //                                questions_BE.Reverse,
                //                                questions_BE.ModifyBy,
                //                                questions_BE.ModifyDate,
                //                                questions_BE.IsActive,
                //                                "I" };

                // To support chiense / japnese language
                CNameValueList param = new CNameValueList();
                param.Add(new CNameValue("@AccountID", questions_BE.AccountID));
                param.Add(new CNameValue("@CompanyID", questions_BE.CompanyID));
                param.Add(new CNameValue("@QuestionnaireID", questions_BE.QuestionnaireID));
                param.Add(new CNameValue("@QuestionTypeID", questions_BE.QuestionTypeID));
                param.Add(new CNameValue("@CateogryID", questions_BE.CateogryID));
                param.Add(new CNameValue("@Sequence", questions_BE.Sequence));
                param.Add(new CNameValue("@Validation", questions_BE.Validation));
                param.Add(new CNameValue("@ValidationText", questions_BE.ValidationText));
                param.Add(new CNameValue("@Title", questions_BE.Title));
                param.Add(new CNameValue("@Description", questions_BE.Description));
                param.Add(new CNameValue("@DescriptionSelf", questions_BE.DescriptionSelf));
                param.Add(new CNameValue("@Hint", questions_BE.Hint));
                param.Add(new CNameValue("@Token", questions_BE.Token));
                param.Add(new CNameValue("@TokenText", questions_BE.TokenText));
                param.Add(new CNameValue("@LengthMIN", questions_BE.LengthMIN));
                param.Add(new CNameValue("@LengthMAX", questions_BE.LengthMAX));
                param.Add(new CNameValue("@Multiline", questions_BE.Multiline));
                param.Add(new CNameValue("@LowerLabel", questions_BE.LowerLabel));
                param.Add(new CNameValue("@UpperLabel", questions_BE.UpperLabel));
                param.Add(new CNameValue("@LowerBound", questions_BE.LowerBound));
                param.Add(new CNameValue("@UpperBound", questions_BE.UpperBound));
                param.Add(new CNameValue("@Increment", questions_BE.Increment));
                param.Add(new CNameValue("@Reverse", questions_BE.Reverse));
                param.Add(new CNameValue("@ModifyBy", questions_BE.ModifyBy));
                param.Add(new CNameValue("@ModifyDate", questions_BE.ModifyDate));
                param.Add(new CNameValue("@IsActive", questions_BE.IsActive));
                param.Add(new CNameValue("@Operation", "I"));

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspQuestionsManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateQuestions(Questions_BE questions_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[28] {questions_BE.QuestionID,
                //                                questions_BE.AccountID,
                //                                questions_BE.CompanyID,
                //                                questions_BE.QuestionnaireID,
                //                                questions_BE.QuestionTypeID,
                //                                questions_BE.CateogryID,
                //                                questions_BE.Sequence,
                //                                questions_BE.Validation,
                //                                questions_BE.ValidationText,
                //                                questions_BE.Title,
                //                                questions_BE.Description,
                //                                questions_BE.DescriptionSelf,
                //                                questions_BE.Hint,
                //                                questions_BE.Token,
                //                                questions_BE.TokenText,
                //                                questions_BE.LengthMIN,
                //                                questions_BE.LengthMAX,
                //                                questions_BE.Multiline,
                //                                questions_BE.LowerLabel,
                //                                questions_BE.UpperLabel,
                //                                questions_BE.LowerBound,
                //                                questions_BE.UpperBound,
                //                                questions_BE.Increment,
                //                                questions_BE.Reverse,
                //                                questions_BE.ModifyBy,
                //                                questions_BE.ModifyDate,
                //                                questions_BE.IsActive,
                //                                "U" };

                // To support chiense / japnese language
                CNameValueList param = new CNameValueList();
                param.Add(new CNameValue("@QuestionID", questions_BE.QuestionID));
                param.Add(new CNameValue("@AccountID", questions_BE.AccountID));
                param.Add(new CNameValue("@CompanyID", questions_BE.CompanyID));
                param.Add(new CNameValue("@QuestionnaireID", questions_BE.QuestionnaireID));
                param.Add(new CNameValue("@QuestionTypeID", questions_BE.QuestionTypeID));
                param.Add(new CNameValue("@CateogryID", questions_BE.CateogryID));
                param.Add(new CNameValue("@Sequence", questions_BE.Sequence));
                param.Add(new CNameValue("@Validation", questions_BE.Validation));
                param.Add(new CNameValue("@ValidationText", questions_BE.ValidationText));
                param.Add(new CNameValue("@Title", questions_BE.Title));
                param.Add(new CNameValue("@Description", questions_BE.Description));
                param.Add(new CNameValue("@DescriptionSelf", questions_BE.DescriptionSelf));
                param.Add(new CNameValue("@Hint", questions_BE.Hint));
                param.Add(new CNameValue("@Token", questions_BE.Token));
                param.Add(new CNameValue("@TokenText", questions_BE.TokenText));
                param.Add(new CNameValue("@LengthMIN", questions_BE.LengthMIN));
                param.Add(new CNameValue("@LengthMAX", questions_BE.LengthMAX));
                param.Add(new CNameValue("@Multiline", questions_BE.Multiline));
                param.Add(new CNameValue("@LowerLabel", questions_BE.LowerLabel));
                param.Add(new CNameValue("@UpperLabel", questions_BE.UpperLabel));
                param.Add(new CNameValue("@LowerBound", questions_BE.LowerBound));
                param.Add(new CNameValue("@UpperBound", questions_BE.UpperBound));
                param.Add(new CNameValue("@Increment", questions_BE.Increment));
                param.Add(new CNameValue("@Reverse", questions_BE.Reverse));
                param.Add(new CNameValue("@ModifyBy", questions_BE.ModifyBy));
                param.Add(new CNameValue("@ModifyDate", questions_BE.ModifyDate));
                param.Add(new CNameValue("@IsActive", questions_BE.IsActive));
                param.Add(new CNameValue("@Operation", "U"));


                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspQuestionsManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteQuestions(Questions_BE questions_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[28] {questions_BE.QuestionID,
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
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspQuestionsManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionsByID(int questionsID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllQuestions = new DataTable();
                object[] param = new object[3] { questionsID, null, "I" };

                dtAllQuestions = cDataSrc.ExecuteDataSet("UspQuestionsSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllQuestions);
                returnValue = 1;

                HandleWriteLogDAU("UspQuestionsSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionsList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllQuestions = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAllQuestions = cDataSrc.ExecuteDataSet("UspQuestionsSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllQuestions);
                returnValue = 1;

                HandleWriteLogDAU("UspQuestionsSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtQuestionsList(string accountID)
        {
            DataTable dtAllQuestions = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

                dtAllQuestions = cDataSrc.ExecuteDataSet("UspQuestionsSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllQuestions;
        }

        #endregion

        private void ShiftDataTableToBEList(DataTable dtQuestions)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            questions_BEList = new List<Questions_BE>();

            for (int recordCounter = 0; recordCounter < dtQuestions.Rows.Count; recordCounter++)
            {
                Questions_BE questions_BE = new Questions_BE();


                questions_BE.QuestionID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["QuestionID"].ToString());
                questions_BE.AccountID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["AccountID"].ToString());
                questions_BE.CompanyID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["CompanyID"].ToString());
                questions_BE.QuestionTypeID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["QuestionTypeID"].ToString());
                questions_BE.QuestionnaireID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["QuestionnaireID"].ToString());
                questions_BE.CateogryID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["CateogryID"].ToString());
                questions_BE.Sequence = Convert.ToInt32(dtQuestions.Rows[recordCounter]["Sequence"].ToString());
                questions_BE.Validation = Convert.ToInt32(dtQuestions.Rows[recordCounter]["Validation"].ToString());
                questions_BE.ValidationText = dtQuestions.Rows[recordCounter]["ValidationText"].ToString();
                questions_BE.Title = dtQuestions.Rows[recordCounter]["Title"].ToString();
                questions_BE.Description = dtQuestions.Rows[recordCounter]["Description"].ToString();
                questions_BE.DescriptionSelf = dtQuestions.Rows[recordCounter]["DescriptionSelf"].ToString();

                questions_BE.Hint = dtQuestions.Rows[recordCounter]["Hint"].ToString();
                questions_BE.Token = Convert.ToInt32(dtQuestions.Rows[recordCounter]["Token"].ToString());
                questions_BE.TokenText = dtQuestions.Rows[recordCounter]["TokenText"].ToString();
                questions_BE.LengthMIN = Convert.ToInt32(dtQuestions.Rows[recordCounter]["LengthMIN"].ToString());
                questions_BE.LengthMAX = Convert.ToInt32(dtQuestions.Rows[recordCounter]["LengthMAX"].ToString());
                questions_BE.Multiline = Convert.ToBoolean(dtQuestions.Rows[recordCounter]["Multiline"].ToString());
                questions_BE.LowerLabel = dtQuestions.Rows[recordCounter]["LowerLabel"].ToString();
                questions_BE.UpperLabel = dtQuestions.Rows[recordCounter]["UpperLabel"].ToString();
                questions_BE.LowerBound = Convert.ToInt32(dtQuestions.Rows[recordCounter]["LowerBound"].ToString());

                questions_BE.UpperBound = Convert.ToInt32(dtQuestions.Rows[recordCounter]["UpperBound"].ToString());
                questions_BE.Increment = Convert.ToInt32(dtQuestions.Rows[recordCounter]["Increment"].ToString());
                questions_BE.Reverse = Convert.ToBoolean(dtQuestions.Rows[recordCounter]["Reverse"].ToString());
                questions_BE.ModifyDate = Convert.ToDateTime(dtQuestions.Rows[recordCounter]["ModifyDate"].ToString());
                questions_BE.ModifyBy = Convert.ToInt32(dtQuestions.Rows[recordCounter]["ModifyBy"].ToString());
                questions_BE.IsActive = Convert.ToInt32(dtQuestions.Rows[recordCounter]["IsActive"].ToString());

                questions_BEList.Add(questions_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetQuestionsListCount(string condition)
        {
            int questionsCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[3] { null,Convert.ToInt32(accountID), "C" };

                //questionsCount = (int)cDataSrc.ExecuteScalar("UspQuestionsSelect", param, null);

                string sql = "SELECT Count(QuestionID) " +
                            " FROM  [Question] Inner Join [Account] on dbo.Question.CompanyID = Account.AccountID" +
                            " Inner Join Questionnaire on Question.QuestionnaireID = Questionnaire.QuestionnaireID " +
                            " Inner Join Category  on Question.CateogryID = Category.CategoryID" +
                            " Inner Join MSTQuestionType on Question.QuestionTypeID = MSTQuestionType.QuestionTypeID" +
                            " Where Account.[AccountID]=" + condition;

                //object[] param = new object[2] { condition, "A" };
                questionsCount = (int)cDataSrc.ExecuteScalar(sql, null);


                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return questionsCount;
        }




        public void ResequenceQuestion(int accountID, int questionnaireID, int increment)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, questionnaireID, increment };

                cDataSrc.ExecuteNonQuery("UspUpdateQuestionSequence", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        public DataTable GetdtQuestionsListnew(string condition)
        {
            DataTable dtAllQuestion = new DataTable();
            try
            {

                string sql = "SELECT [QuestionID], " +
                            "Question.AccountID AS AccountID, " +
                            "Account.Code as Code, " +
                            "Questionnaire.QSTNName as Questionnaire, " +
                            "MSTQuestionType.Name as Name," +
                            "Category.CategoryName as CategoryName, " +
                            "Question.Sequence  as Sequence, " +
                            "[Validation], " +
                            "[ValidationText], " +
                            "[Title], " +
                            "Question.Description as Descriptions, " +
                            "Question.DescriptionSelf as DescriptionSelf, " +
                            "[Hint], " +
                            "[Token]," +
                            "[TokenText]" +
                            ",[LengthMIN]" +
                            ",[LengthMAX]" +
                            ",[Multiline]" +
                            ",[LowerLabel]" +
                            ",[UpperLabel]" +
                            ",[LowerBound]" +
                            ",[UpperBound]" +
                            ",[Increment]" +
                            ",[Reverse]" +
                            ",Question.ModifyBy" +
                            ",Question.ModifyDate" +
                            ",Question.IsActive" +
                            " FROM  [Question] Inner Join [Account] on dbo.Question.CompanyID = Account.AccountID" +
                            "  Inner Join Questionnaire on Question.QuestionnaireID = Questionnaire.QuestionnaireID " +
                            " Inner Join Category  on Question.CateogryID = Category.CategoryID" +
                            " Inner Join MSTQuestionType on Question.QuestionTypeID = MSTQuestionType.QuestionTypeID" +
                            " Where Account.[AccountID]=" + condition +
                            " order by [Code],Sequence, Question.ModifyDate, Title  Desc";

                dtAllQuestion = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllQuestion;
        }
    }















    public class Survey_Questions_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_Questions_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Survey_Questions_BE> questions_BEList { get; set; }

        #endregion

        # region CRUD Operation


        public DataTable getrange_data()
        {
            DataTable ddt = null;
            //try
            //{
            ddt = cDataSrc.ExecuteDataSet("select Range_Name from Question_Range").Tables[0];
                cDataSrc = null;

            //}

            //catch
            //{ }
            return ddt;
        }

        public int AddQuestions(Survey_Questions_BE questions_BE)
        {
            ////try
            ////{
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[22] {null,
                                                questions_BE.AccountID,
                                                questions_BE.CompanyID,
                                                questions_BE.QuestionnaireID,
                                                questions_BE.QuestionTypeID,
                                                questions_BE.CateogryID,
                                                questions_BE.Sequence,
                                                questions_BE.Validation,
                                                questions_BE.ValidationText,
                                                questions_BE.Title,
                                                questions_BE.Description,
                                                //questions_BE.DescriptionSelf,
                                                questions_BE.Hint,
                                                questions_BE.Token,
                                                questions_BE.TokenText,
                                                questions_BE.LengthMIN,
                                                questions_BE.LengthMAX,
                                                questions_BE.Multiline,
                                                
                                                questions_BE.ModifyBy,
                                                questions_BE.ModifyDate,
                                                questions_BE.IsActive,
                                                questions_BE.Range_Name,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspQuestionsManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            ////}
            ////catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateQuestions(Survey_Questions_BE questions_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[22] {questions_BE.QuestionID,
                                                questions_BE.AccountID,
                                                questions_BE.CompanyID,
                                                questions_BE.QuestionnaireID,
                                                questions_BE.QuestionTypeID,
                                                questions_BE.CateogryID,
                                                questions_BE.Sequence,
                                                questions_BE.Validation,
                                                questions_BE.ValidationText,
                                                questions_BE.Title,
                                                questions_BE.Description,
                                               // questions_BE.DescriptionSelf,
                                                questions_BE.Hint,
                                                questions_BE.Token,
                                                questions_BE.TokenText,
                                                questions_BE.LengthMIN,
                                                questions_BE.LengthMAX,
                                                questions_BE.Multiline,
                                                
                                                questions_BE.ModifyBy,
                                                questions_BE.ModifyDate,
                                                questions_BE.IsActive,
                                                questions_BE.Range_Name,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspQuestionsManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteQuestions(Survey_Questions_BE questions_BE)
        {
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[22] {questions_BE.QuestionID,
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
                                              //  null,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspQuestionsManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionsByID(int questionsID)
        {
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllQuestions = new DataTable();
                object[] param = new object[3] { questionsID, null, "I" };

                dtAllQuestions = cDataSrc.ExecuteDataSet("Survey_UspQuestionsSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllQuestions);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspQuestionsSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetQuestionsList()
        {
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllQuestions = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAllQuestions = cDataSrc.ExecuteDataSet("Survey_UspQuestionsSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllQuestions);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspQuestionsSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtQuestionsList(string accountID)
        {
            DataTable dtAllQuestions = new DataTable();
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

                dtAllQuestions = cDataSrc.ExecuteDataSet("Survey_UspQuestionsSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtAllQuestions;
        }

        #endregion

        private void ShiftDataTableToBEList(DataTable dtQuestions)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            questions_BEList = new List<Survey_Questions_BE>();

            for (int recordCounter = 0; recordCounter < dtQuestions.Rows.Count; recordCounter++)
            {
Survey_Questions_BE questions_BE = new Survey_Questions_BE();


                questions_BE.QuestionID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["QuestionID"].ToString());
                questions_BE.AccountID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["AccountID"].ToString());
                questions_BE.CompanyID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["CompanyID"].ToString());
                questions_BE.QuestionTypeID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["QuestionTypeID"].ToString());
                questions_BE.QuestionnaireID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["QuestionnaireID"].ToString());
                questions_BE.CateogryID = Convert.ToInt32(dtQuestions.Rows[recordCounter]["CateogryID"].ToString());
                questions_BE.Sequence = Convert.ToInt32(dtQuestions.Rows[recordCounter]["Sequence"].ToString());
                questions_BE.Validation = Convert.ToInt32(dtQuestions.Rows[recordCounter]["Validation"].ToString());
                questions_BE.ValidationText = dtQuestions.Rows[recordCounter]["ValidationText"].ToString();
                questions_BE.Title = dtQuestions.Rows[recordCounter]["Title"].ToString();
                questions_BE.Description = dtQuestions.Rows[recordCounter]["Description"].ToString();
                //questions_BE.DescriptionSelf = dtQuestions.Rows[recordCounter]["DescriptionSelf"].ToString();

                questions_BE.Hint = dtQuestions.Rows[recordCounter]["Hint"].ToString();
                questions_BE.Token = Convert.ToInt32(dtQuestions.Rows[recordCounter]["Token"].ToString());
                questions_BE.TokenText = dtQuestions.Rows[recordCounter]["TokenText"].ToString();
                questions_BE.LengthMIN = Convert.ToInt32(dtQuestions.Rows[recordCounter]["LengthMIN"].ToString());
                questions_BE.LengthMAX = Convert.ToInt32(dtQuestions.Rows[recordCounter]["LengthMAX"].ToString());
                questions_BE.Multiline = Convert.ToBoolean(dtQuestions.Rows[recordCounter]["Multiline"].ToString());
               
                questions_BE.ModifyDate = Convert.ToDateTime(dtQuestions.Rows[recordCounter]["ModifyDate"].ToString());
                questions_BE.ModifyBy = Convert.ToInt32(dtQuestions.Rows[recordCounter]["ModifyBy"].ToString());
                questions_BE.IsActive = Convert.ToInt32(dtQuestions.Rows[recordCounter]["IsActive"].ToString());
                if (dtQuestions.Rows[recordCounter]["QuestionTypeID"].ToString()=="2")
                questions_BE.Range_Name = Convert.ToString(dtQuestions.Rows[recordCounter]["Range_Name"]);
                questions_BEList.Add(questions_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetQuestionsListCount(string condition)
        {
            int questionsCount = 0;
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[3] { null,Convert.ToInt32(accountID), "C" };

                //questionsCount = (int)cDataSrc.ExecuteScalar("UspQuestionsSelect", param, null);

                string sql = "SELECT Count(QuestionID) " +
                            " FROM  [Survey_Question] Inner Join [Account] on dbo.Survey_Question.CompanyID = Account.AccountID" +
                            " Inner Join Survey_Questionnaire on Survey_Question.QuestionnaireID = Survey_Questionnaire.QuestionnaireID " +
                            " Inner Join Survey_Category  on Survey_Question.CateogryID = Survey_Category.CategoryID" +
                            " Inner Join Survey_MSTQuestionType on Survey_Question.QuestionTypeID = Survey_MSTQuestionType.QuestionTypeID" +
                            " Where Account.[AccountID]=" + condition;

                //object[] param = new object[2] { condition, "A" };
                questionsCount = (int)cDataSrc.ExecuteScalar(sql, null);


                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return questionsCount;
        }




        public void ResequenceQuestion(int accountID, int questionnaireID, int increment)
        {
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, questionnaireID, increment };

                cDataSrc.ExecuteNonQuery("Survey_UspUpdateQuestionSequence", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
        }


        public DataTable GetdtQuestionsListnew(string condition)
        {
            DataTable dtAllQuestion = new DataTable();
            //try
            //{

                string sql = "SELECT [QuestionID], " +
                            "Survey_Question.AccountID AS AccountID, " +
                            "Account.Code as Code, " +
                            "Survey_Questionnaire.QSTNName as Questionnaire, " +
                            "Survey_MSTQuestionType.Name as Name," +
                            "Survey_Category.CategoryName as CategoryName, " +
                            "Survey_Question.Sequence  as Sequence, " +
                            "[Validation], " +
                            "[ValidationText], " +
                            "[Title], " +
                            "Survey_Question.Description as Descriptions, " +
                            //"Survey_Question.DescriptionSelf as DescriptionSelf, " +
                            "[Hint], " +
                            "[Token]," +
                            "[TokenText]" +
                            ",[LengthMIN]" +
                            ",[LengthMAX]" +
                            ",[Multiline]" +
                            ",[Range_Name]" +
                            ",Survey_Question.ModifyBy" +
                            ",Survey_Question.ModifyDate" +
                            ",Survey_Question.IsActive" +
                            " FROM  [Survey_Question] Inner Join [Account] on dbo.Survey_Question.CompanyID = Account.AccountID" +
                            "  Inner Join Survey_Questionnaire on Survey_Question.QuestionnaireID = Survey_Questionnaire.QuestionnaireID " +
                            " Inner Join Survey_Category  on Survey_Question.CateogryID = Survey_Category.CategoryID" +
                            " Inner Join Survey_MSTQuestionType on Survey_Question.QuestionTypeID = Survey_MSTQuestionType.QuestionTypeID" +
                            " Where Account.[AccountID]=" + condition +
                            " order by [Code],Sequence, Survey_Question.ModifyDate, Title  Desc";

                dtAllQuestion = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtAllQuestion;
        }
    }

}
