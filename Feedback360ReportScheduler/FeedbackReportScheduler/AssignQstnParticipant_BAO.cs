using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class AssignQstnParticipant_BAO //: Base_BAO 
    {

        #region "Private Member Variable"

        private int addAssignQuestionnaire;
        private string Template;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Assign Questionnaire
        /// </summary>
        /// <returns></returns>
        public int AddAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE)
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
                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaire_DAO.AddAssignQuestionnaire(assignQuestionnaire_BE, dbTransaction);
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
            return addAssignQuestionnaire;
        }
        /// <summary>
        /// Update Assign Questionnaire
        /// </summary>
        public int UpdateAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE)
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
                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaire_DAO.UpdateAssignQuestionnaire(assignQuestionnaire_BE);
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
            return addAssignQuestionnaire;
        }
        /// <summary>
        /// Delete Assign Questionnaire
        /// </summary>
        /// <param name="questionnaireBE">questionnaire BE</param>
        /// <returns></returns>
        public int DeleteAssignQuestionnaire(Questionnaire_BE.AssignQuestionnaire_BE questionnaireBE)
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
                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaire_DAO.DeleteAssignQuestionnaire(questionnaireBE);
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
            return addAssignQuestionnaire;
        }
        /// <summary>
        /// Get Assign Questionnaire by assign Questionnaire ID
        /// </summary>
        public List<AssignQuestionnaire_BE> GetAssignQuestionnaireByID(int assignQuestionnaireID)
        {
            List<AssignQuestionnaire_BE> assignQuestionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                assignQuestionnaire_DAO.GetAssignQuestionnaireByID(assignQuestionnaireID);

                assignQuestionnaire_BEList = assignQuestionnaire_DAO.assignQuestionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignQuestionnaire_BEList;
        }
        /// <summary>
        /// Get Assign Questionnaire lis
        /// </summary>
        public List<AssignQuestionnaire_BE> GetAssignQuestionnaireList()
        {
            List<AssignQuestionnaire_BE> assignQuestionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                assignQuestionnaire_DAO.GetAssignQuestionnaireList();

                assignQuestionnaire_BEList = assignQuestionnaire_DAO.assignQuestionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignQuestionnaire_BEList;
        }
        /// <summary>
        /// Get Assign Questionnaire list by assign Questionnaire ID
        /// </summary>
        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetdtAssignQuestionnaireList(assignmentID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }
        /// <summary>
        /// Get Assign Questionnaire list count
        /// </summary>
        public int GetAssignQuestionnaireListCount()
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                assignQuestionnaireCount = assignQuestionnaire_DAO.GetAssignQuestionnaireListCount();

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        /// <summary>
        ///Find Template by project id
        /// </summary>
        public String FindTemplate(int ProjectID)
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
                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                Template = assignQuestionnaire_DAO.FindTemplate(ProjectID);
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
            return Template;
        }

        /// <summary>
        /// Get Assign Questionnaire List by user id
        /// </summary>
        /// <param name="userid">user id</param>
        /// <returns></returns>
        public DataTable GetuseridAssignQuestionnaireList(int userid)
        {
            DataTable dtuseridAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                dtuseridAssignQuestionnaire = assignQuestionnaire_DAO.GetdtuseridAssignQuestionnaireList(userid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtuseridAssignQuestionnaire;
        }
        /// <summary>
        /// Get Assign Participant List 
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <returns></returns>
        public DataTable GetdtAssignPartiList(string accountID, string programmeID)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO Questionnaire_DAO = new AssignQstnParticipant_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignPartiList(accountID, programmeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }
        /// <summary>
        /// Get Assign Participant Questionnaire List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Projectid">Project id</param>
        /// <returns></returns>
        public int GetAssignPartiQuestionnaireListCount(string accountID, string Projectid)
        {
            int assignPartiQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                assignPartiQuestionnaireCount = assignQuestionnaire_DAO.GetAssignPartiQuestionnaireListCount(accountID, Projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignPartiQuestionnaireCount;
        }
        /// <summary>
        /// Get Assign program Participant  List 
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Projectid">Project id</param>
        /// <returns></returns>
        public DataTable GetdtAssignProgrammePartiList(string accountID, string Programmeid)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO Questionnaire_DAO = new AssignQstnParticipant_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignProgrammePartiList(accountID, Programmeid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }
        /// <summary>
        /// Get Assign  program Participant Questionnaire List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Projectid">Project id</param>
        /// <returns></returns>
        public int GetAssignProgammePartiQuestionnaireListCount(string accountID, string Programmeid)
        {
            int assignPartiQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                assignPartiQuestionnaireCount = assignQuestionnaire_DAO.GetAssignProgammePartiQuestionnaireListCount(accountID, Programmeid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignPartiQuestionnaireCount;
        }



        #endregion
        /// <summary>
        /// Get candidates Count
        /// </summary>
        public Int32 GetCandidatesCount(int targetPersonID)
        {
            int CandidatesCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaire_DAO.GetCandidatesCount(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }
        /// <summary>
        /// Get Submission Count
        /// </summary>
        /// <param name="targetPersonID">target Person ID</param>
        /// <returns></returns>
        public Int32 GetSubmissionCount(int targetPersonID)
        {
            int CandidatesCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaire_DAO.GetSubmissionCount(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }
        /// <summary>
        /// Get Self Assessment
        /// </summary>
        /// <param name="targetPersonID">target Person ID</param>
        /// <returns></returns>
        public Int32 GetSelfAssessment(int targetPersonID)
        {
            int CandidatesCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaire_DAO.GetSelfAssessment(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }
        /// <summary>
        /// Use to Handle exceptions
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
