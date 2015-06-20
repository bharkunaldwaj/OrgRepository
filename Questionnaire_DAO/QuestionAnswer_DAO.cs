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
    public class QuestionAnswer_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        int returnValue;

        public int AddQuestionAnswer(QuestionAnswer_BE questionAnswer_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[7] {questionAnswer_BE.AssignDetId,
                                                questionAnswer_BE.QuestionID,
                                                questionAnswer_BE.Answer,
                                                questionAnswer_BE.ModifyBy,
                                                questionAnswer_BE.ModifyDate,
                                                questionAnswer_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspQuestionAnswerManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public string GetQuestionAnswer(int candidateId, int questionID)
        {
            string returnValue = "";
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                string sql = "select answer from QuestionAnswer where AssignDetId=" + candidateId + " and QuestionID=" + questionID;
                returnValue = Convert.ToString(cDataSrc.ExecuteScalar(sql, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
    }


    public class Survey_QuestionAnswer_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        int returnValue;

        public int AddQuestionAnswer(Survey_QuestionAnswer_BE questionAnswer_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[7] {questionAnswer_BE.AssignDetId,
                                                questionAnswer_BE.QuestionID,
                                                questionAnswer_BE.Answer,
                                                questionAnswer_BE.ModifyBy,
                                                questionAnswer_BE.ModifyDate,
                                                questionAnswer_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspQuestionAnswerManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public string GetQuestionAnswer(int candidateId, int questionID)
        {
            string returnValue = "";
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                string sql = "select answer from Survey_QuestionAnswer where AssignDetId=" + candidateId + " and QuestionID=" + questionID;
                returnValue = Convert.ToString(cDataSrc.ExecuteScalar(sql, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
    }
}