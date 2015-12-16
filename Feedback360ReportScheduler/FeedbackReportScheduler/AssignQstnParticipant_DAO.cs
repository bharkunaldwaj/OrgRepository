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

//using feedbackFramework_BE;
//using feedbackFramework_DAO;

using Admin_DAO;
using Admin_BE;
using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class AssignQstnParticipant_DAO
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        private string Template;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public AssignQstnParticipant_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"
        
        public List<AssignQuestionnaire_BE> assignQuestionnaire_BEList { get; set; }
        
        #endregion

        # region CRUD Operation
        /// <summary>
        /// Insert Assign Questionnaire
        /// </summary>
        /// <param name="assignQuestionnaire_BE"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public int AddAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE, IDbTransaction dbTransaction)
        {
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {null,
                                                assignQuestionnaire_BE.AccountID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProgrammeID,
                                                assignQuestionnaire_BE.QuestionnaireID,
                                                assignQuestionnaire_BE.Description,
                                                assignQuestionnaire_BE.CandidateNo,
                                                assignQuestionnaire_BE.ModifiedBy,
                                                assignQuestionnaire_BE.ModifiedDate,
                                                assignQuestionnaire_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignQstnParticipantManagement", param, dbTransaction));

                // ADD RECORD OF Participants

                List<AccountUser_BE> lstAssignmentDetails = new List<AccountUser_BE>();
                lstAssignmentDetails = assignQuestionnaire_BE.AssignmentUserDetails;

                int newValue;
                int newValue1;

                for (int count = 0; count < lstAssignmentDetails.Count; count++)
                {
                    object[] newparam = new object[15] {null,
                                                lstAssignmentDetails[count].LoginID,
                                                lstAssignmentDetails[count].Password,
                                                lstAssignmentDetails[count].GroupID,
                                                lstAssignmentDetails[count].AccountID,
                                                lstAssignmentDetails[count].StatusID,
                                                lstAssignmentDetails[count].Salutation,
                                                lstAssignmentDetails[count].FirstName,
                                                lstAssignmentDetails[count].LastName,
                                                lstAssignmentDetails[count].EmailID,
                                                lstAssignmentDetails[count].Notification,
                                                lstAssignmentDetails[count].ModifyBy,
                                                lstAssignmentDetails[count].ModifyDate,
                                                lstAssignmentDetails[count].IsActive,
                                                
                                                
                                                "I" };


                    newValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticiantUserManagement", newparam, dbTransaction));


                    object[] newparam1 = new object[5] {
                        
                                                    null,
                                                    returnValue,
                                                    assignQuestionnaire_BE.ProjecctID,
                                                    newValue,
                                                       "I" 
                                                        };

                    newValue1 = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAddPartictDetailsManagement", newparam1, dbTransaction));

                }

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Update  Assign Questionnaire
        /// </summary>
        /// <param name="assignQuestionnaire_BE"></param>
        /// <returns></returns>
        public int UpdateAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {assignQuestionnaire_BE.AssignmentID,
                                                assignQuestionnaire_BE.AccountID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProgrammeID,
                                                assignQuestionnaire_BE.QuestionnaireID,
                                                assignQuestionnaire_BE.Description,
                                                assignQuestionnaire_BE.CandidateNo,
                                                assignQuestionnaire_BE.ModifiedBy,
                                                assignQuestionnaire_BE.ModifiedDate,
                                                assignQuestionnaire_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignQstnParticipantManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Delete  Assign Questionnaire
        /// </summary>
        /// <param name="questionnaireBE"></param>
        /// <returns></returns>
        public int DeleteAssignQuestionnaire(Questionnaire_BE.AssignQuestionnaire_BE questionnaireBE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {questionnaireBE.AssignmentID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Get  Assign Questionnaire by  Assign Questionnaire id
        /// </summary>
        /// <param name="assignQuestionnaireID"></param>
        /// <returns></returns>
        public int GetAssignQuestionnaireByID(int assignQuestionnaireID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //DataTable dtAllAssignQuestionnaire = new DataTable();
                //object[] param = new object[2] { assignQuestionnaireID,"I" };

                //dtAllAssignQuestionnaire = cDataSrc.ExecuteDataSet("UspAssignQuestionnaireSelect", param, null).Tables[0];

                //ShiftDataTableToBEList(dtAllAssignQuestionnaire);
                //returnValue = 1;

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Get  Assign Questionnaire list
        /// </summary>
        /// <returns></returns>
        public int GetAssignQuestionnaireList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //DataTable dtAllAssignQuestionnaire = new DataTable();
                //object[] param = new object[2] { null, "A" };

                //dtAllAssignQuestionnaire = cDataSrc.ExecuteDataSet("UspAssignQuestionnaireSelect", param, null).Tables[0];

                //ShiftDataTableToBEList(dtAllAssignQuestionnaire);
                //returnValue = 1;

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        /// <summary>
        /// Get  Assign Questionnaire list by assignment id.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { assignmentID };

                dtResult = cDataSrc.ExecuteDataSet("UspAssignPartQuestionnaireSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtResult;
        }

        #endregion 

        private void ShiftDataTableToBEList(DataTable dtAssignQuestionnaire)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //assignQuestionnaire_BEList = new List<AssignQuestionnaire_BE>();

            //for (int recordCounter = 0; recordCounter < dtAssignQuestionnaire.Rows.Count; recordCounter++)
            //{
            //    AssignQuestionnaire_BE assignQuestionnaire_BE = new AssignQuestionnaire_BE();

            //    assignQuestionnaire_BE.AssignQuestionnaireID = Convert.ToInt32(dtAssignQuestionnaire.Rows[recordCounter]["AssignQuestionnaireID"].ToString());
            //    assignQuestionnaire_BE.Name = dtAssignQuestionnaire.Rows[recordCounter]["AssignQuestionnaireName"].ToString();
            //    assignQuestionnaire_BE.Description = dtAssignQuestionnaire.Rows[recordCounter]["Description"].ToString();
            //    assignQuestionnaire_BE.Sequence = Convert.ToInt32(dtAssignQuestionnaire.Rows[recordCounter]["Sequence"].ToString());
            //    assignQuestionnaire_BE.ExcludeFromAnalysis = Convert.ToBoolean(dtAssignQuestionnaire.Rows[recordCounter]["ExcludeFromAnalysis"].ToString());
            //    assignQuestionnaire_BE.ModifiedBy = Convert.ToInt32(dtAssignQuestionnaire.Rows[recordCounter]["ModifiedBy"].ToString());
            //    assignQuestionnaire_BE.ModifiedDate = Convert.ToDateTime(dtAssignQuestionnaire.Rows[recordCounter]["ModifiedDate"].ToString());
            //    assignQuestionnaire_BE.IsActive = Convert.ToInt32(dtAssignQuestionnaire.Rows[recordCounter]["IsActive"].ToString());

            //    assignQuestionnaire_BEList.Add(assignQuestionnaire_BE);
            //}

            //HandleWriteLog("End", new StackTrace(true));
        }
        /// <summary>
        /// Get  Assign Questionnaire list count
        /// </summary>
        /// <returns></returns>
        public int GetAssignQuestionnaireListCount()
        {
            int assignQuestionnaireCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[2] { null, "C" };

                //assignQuestionnaireCount = (int)cDataSrc.ExecuteScalar("UspAssignQuestionnaireSelect", param, null);

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return assignQuestionnaireCount;
        }

        /// <summary>
        /// Get Email tempalte
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public String FindTemplate(int ProjectID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "A"      };

                Template = Convert.ToString(cDataSrc.ExecuteScalar("UspFindTemplate", param, null));

                cDataSrc = null;


            }
            catch (Exception ex) { HandleException(ex); }
            return Template;
        }

        /// <summary>
        /// Get Participant Template by Project ID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public String FindParticipantTemplate(int ProjectID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "P"      };

                Template = Convert.ToString(cDataSrc.ExecuteScalar("UspFindTemplate", param, null));

                cDataSrc = null;


            }
            catch (Exception ex) { HandleException(ex); }
            return Template;
        }

        /// <summary>
        /// Find Participant Subject Template by Project ID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public String FindParticipantSubjectTemplate(int ProjectID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "S"      };

                Template = Convert.ToString(cDataSrc.ExecuteScalar("UspFindTemplate", param, null));

                cDataSrc = null;


            }
            catch (Exception ex) { HandleException(ex); }
            return Template;
        }
        /// <summary>
        /// Find Candidate Subject Template
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public String FindCandidateSubjectTemplate(int ProjectID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "Q"      };

                Template = Convert.ToString(cDataSrc.ExecuteScalar("UspFindTemplate", param, null));

                cDataSrc = null;


            }
            catch (Exception ex) { HandleException(ex); }
            return Template;
        }
        /// <summary>
        /// Get user id Assign Questionnaire List
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetdtuseridAssignQuestionnaireList(int userid)
        {
            DataTable dtuseridAssignQuestionnaire = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[5] { null,
                                                 null,
                                                null,
                                                userid,
                                                 "R"}; //For Report Scheduler only

                dtuseridAssignQuestionnaire = cDataSrc.ExecuteDataSet("UspAddPartictDetailsManagement", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtuseridAssignQuestionnaire;
        }

        /// <summary>
        /// Get Assign Participant List
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Projectid">Project id</param>
        /// <returns></returns>
        public DataTable GetdtAssignPartiList(string accountID,string Projectid)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), Projectid, "A" };

                dtAllAssign = cDataSrc.ExecuteDataSet("UspAssignPartiSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        /// <summary>
        /// Get Assign Participant Questionnaire List Count
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="Projectid"></param>
        /// <returns></returns>
        public int GetAssignPartiQuestionnaireListCount(string accountID, string Projectid)
        {
            int AssignQuestionnaireCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), Projectid, "C" };

                AssignQuestionnaireCount = (int)cDataSrc.ExecuteScalar("UspAssignPartiSelect", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return AssignQuestionnaireCount;
        }
        /// <summary>
        /// Get Assign Programme Participant List
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="Programmeid"></param>
        /// <returns></returns>
        public DataTable GetdtAssignProgrammePartiList(string accountID, string Programmeid)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), Programmeid, "Z" };

                dtAllAssign = cDataSrc.ExecuteDataSet("UspAssignProgrammeParticipantSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }
        /// <summary>
        /// Get Assign Progamme Participant Questionnaire List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Projectid">Project id</param>
        /// <returns></returns>
        public int GetAssignProgammePartiQuestionnaireListCount(string accountID, string Projectid)
        {
            int AssignQuestionnaireCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { Convert.ToInt32(accountID), Projectid, "Z" };

                AssignQuestionnaireCount = (int)cDataSrc.ExecuteScalar("UspparticipantSelect", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return AssignQuestionnaireCount;
        }

        /// <summary>
        /// Get Candidates Count
        /// </summary>
        /// <param name="targetPersonID"></param>
        /// <returns></returns>
        public int GetCandidatesCount(int targetPersonID)
        {
            int CandidatesCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { targetPersonID, "C" };
                CandidatesCount = (int)cDataSrc.ExecuteScalar("GetQuestionnaireSubmitStatus", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return CandidatesCount;
        }
        /// <summary>
        /// Get answer Submission Count
        /// </summary>
        /// <param name="targetPersonID">targetPerson ID</param>
        /// <returns></returns>
        public int GetSubmissionCount(int targetPersonID)
        {
            int CandidatesCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { targetPersonID, "T" };
                CandidatesCount = (int)cDataSrc.ExecuteScalar("GetQuestionnaireSubmitStatus", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return CandidatesCount;
        }
        /// <summary>
        /// Get Self Assessment by target Person ID
        /// </summary>
        /// <param name="targetPersonID">Target Person ID</param>
        /// <returns></returns>
        public int GetSelfAssessment(int targetPersonID)
        {
            int CandidatesCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { targetPersonID, "S" };
                CandidatesCount = (int)cDataSrc.ExecuteScalar("GetQuestionnaireSubmitStatus", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return CandidatesCount;
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
