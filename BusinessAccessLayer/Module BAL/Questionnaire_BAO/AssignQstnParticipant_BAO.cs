using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class AssignQstnParticipant_BAO : Base_BAO
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
        public int AddAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
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
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.AddAssignQuestionnaire(assignQuestionnaireBusinessEntity, dbTransaction);
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
        public int UpdateAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
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
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.UpdateAssignQuestionnaire(assignQuestionnaireBusinessEntity);
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
        /// <param name="questionnaireBusinessEntity">questionnaire BE</param>
        /// <returns></returns>
        public int DeleteAssignQuestionnaire(Questionnaire_BE.AssignQuestionnaire_BE questionnaireBusinessEntity)
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
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.DeleteAssignQuestionnaire(questionnaireBusinessEntity);
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
            List<AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                assignQuestionnaireDataAccessObject.GetAssignQuestionnaireByID(assignQuestionnaireID);

                assignQuestionnaireBusinessEntityList = assignQuestionnaireDataAccessObject.assignQuestionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignQuestionnaireBusinessEntityList;
        }

        /// <summary>
        /// Get Assign Questionnaire lis
        /// </summary>
        public List<AssignQuestionnaire_BE> GetAssignQuestionnaireList()
        {
            List<AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                assignQuestionnaireDataAccessObject.GetAssignQuestionnaireList();

                assignQuestionnaireBusinessEntityList = assignQuestionnaireDataAccessObject.assignQuestionnaire_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignQuestionnaireBusinessEntityList;
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

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                dtAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetdtAssignQuestionnaireList(assignmentID);

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

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                assignQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignQuestionnaireListCount();

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
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindTemplate(ProjectID);
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
            DataTable dataTableUseridAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                dataTableUseridAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetdtuseridAssignQuestionnaireList(userid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableUseridAssignQuestionnaire;
        }

        /// <summary>
        /// Get Assign Participant List 
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <returns></returns>
        public DataTable GetdtAssignPartiList(string accountID, string programmeID)
        {
            DataTable dataTableAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO QuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                dataTableAssign = QuestionnaireDataAccessObject.GetdtAssignPartiList(accountID, programmeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssign;
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

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                assignPartiQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignPartiQuestionnaireListCount(accountID, Projectid);

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
            DataTable dataTableAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO QuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                dataTableAssign = QuestionnaireDataAccessObject.GetdtAssignProgrammePartiList(accountID, Programmeid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssign;
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

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                assignPartiQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignProgammePartiQuestionnaireListCount(accountID, Programmeid);

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

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaireDataAccessObject.GetCandidatesCount(targetPersonID);

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

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaireDataAccessObject.GetSubmissionCount(targetPersonID);

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

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaireDataAccessObject.GetSelfAssessment(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }

        /// <summary>
        /// Get Report File Name
        /// </summary>
        /// <param name="targetPersonID">target Person ID</param>
        /// <returns></returns>
        public string GetReportFileName(int targetPersonID)
        {
            string fileName = "";

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                fileName = assignQuestionnaireDataAccessObject.GetReportFileName(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return fileName;
        }

        /// <summary>
        /// Get Participant Report Info
        /// </summary>
        /// <param name="targetPersonID">target Person ID</param>
        /// <returns></returns>
        public DataTable GetParticipantReportInfo(int targetPersonID)
        {
            DataTable dataTableParticipantReportInformation = new DataTable();

            try
            {
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                dataTableParticipantReportInformation = assignQuestionnaireDataAccessObject.GetParticipantReportInfo(targetPersonID);
            }

            catch (Exception ex) { HandleException(ex); }
            return dataTableParticipantReportInformation;
        }
    }

    public class Survey_AssignQstnParticipant_BAO : Base_BAO
    {

        #region "Private Member Variable"

        private int addAssignQuestionnaire;
        private string Template;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Assign Questionnaire
        /// </summary>
        /// <param name="assignQuestionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int AddAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.AddAssignQuestionnaire(assignQuestionnaireBusinessEntity, dbTransaction);
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
        /// update  Assign Questionnaire
        /// </summary>
        /// <param name="assignQuestionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.UpdateAssignQuestionnaire(assignQuestionnaireBusinessEntity);
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
        /// Delete  Assign Questionnaire
        /// </summary>
        /// <param name="questionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteAssignQuestionnaire(Questionnaire_BE.Survey_AssignQuestionnaire_BE questionnaireBusinessEntity)
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.DeleteAssignQuestionnaire(questionnaireBusinessEntity);
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
        /// GEt  Assign Questionnaire by  Assign Questionnaire id
        /// </summary>
        /// <param name="assignQuestionnaireID"></param>
        /// <returns></returns>
        public List<Survey_AssignQuestionnaire_BE> GetAssignQuestionnaireByID(int assignQuestionnaireID)
        {
            List<Survey_AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                assignQuestionnaireDataAccessObject.GetAssignQuestionnaireByID(assignQuestionnaireID);

                assignQuestionnaireBusinessEntityList = assignQuestionnaireDataAccessObject.assignQuestionnaireBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignQuestionnaireBusinessEntityList;
        }

        /// <summary>
        /// Get  Assign Questionnaire list 
        /// </summary>
        /// <returns></returns>
        public List<Survey_AssignQuestionnaire_BE> GetAssignQuestionnaireList()
        {
            List<Survey_AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                assignQuestionnaireDataAccessObject.GetAssignQuestionnaireList();

                assignQuestionnaireBusinessEntityList = assignQuestionnaireDataAccessObject.assignQuestionnaireBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignQuestionnaireBusinessEntityList;
        }

        /// <summary>
        /// Get  Assign Questionnaire list by assignment id.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dataTableAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                dataTableAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetdtAssignQuestionnaireList(assignmentID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignQuestionnaire;
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

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                assignQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignQuestionnaireListCount();

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        /// <summary>
        /// Get Template by Project ID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindTemplate(ProjectID);
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
            DataTable dataTableuseridAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                dataTableuseridAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetdtuseridAssignQuestionnaireList(userid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableuseridAssignQuestionnaire;
        }

        /// <summary>
        /// Get Assign Participant List
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <returns></returns>
        public DataTable GetdtAssignPartiList(string accountID, string programmeID)
        {
            DataTable dataTableAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO QuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                dataTableAssign = QuestionnaireDataAccessObject.GetdtAssignPartiList(accountID, programmeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssign;
        }

        /// <summary>
        /// Get Assign Participant Questionnaire List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Projectid">Project Id</param>
        /// <returns></returns>
        public int GetAssignPartiQuestionnaireListCount(string accountID, string Projectid)
        {
            int assignPartiQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                assignPartiQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignPartiQuestionnaireListCount(accountID, Projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignPartiQuestionnaireCount;
        }

        /// <summary>
        /// Get Assign Programme Participant List
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="Programmeid"></param>
        /// <returns></returns>
        public DataTable GetdtAssignProgrammePartiList(string accountID, string Programmeid)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO QuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                dtCAssign = QuestionnaireDataAccessObject.GetdtAssignProgrammePartiList(accountID, Programmeid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }

        /// <summary>
        /// Get Assign Progamme Participant Questionnaire List Count
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Programmeid">Programme id</param>
        /// <returns></returns>
        public int GetAssignProgammePartiQuestionnaireListCount(string accountID, string Programmeid)
        {
            int assignPartiQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                assignPartiQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignProgammePartiQuestionnaireListCount(accountID, Programmeid);

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
        /// Get Candidates Count
        /// </summary>
        /// <param name="targetPersonID"></param>
        /// <returns></returns>
        public Int32 GetCandidatesCount(int targetPersonID)
        {
            int CandidatesCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaireDataAccessObject.GetCandidatesCount(targetPersonID);

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

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaireDataAccessObject.GetSubmissionCount(targetPersonID);

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

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaireDataAccessObject.GetSelfAssessment(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }

        /// <summary>
        /// Get Report FileName
        /// </summary>
        /// <param name="strAccountID">Account ID</param>
        /// <param name="strProjectID">Project ID</param>
        /// <param name="strProgrammeID">Programme ID</param>
        /// <returns></returns>
        public string GetReportFileName(int strAccountID, int strProjectID, int strProgrammeID)
        {
            string fileName = "";

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                fileName = assignQuestionnaireDataAccessObject.GetReportFileName(strAccountID, strProjectID, strProgrammeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return fileName;
        }

        /// <summary>
        /// Get Participant Report details
        /// </summary>
        /// <param name="targetPersonID">targetPerson ID</param>
        /// <returns></returns>
        public DataTable GetParticipantReportInfo(int targetPersonID)
        {
            DataTable dtParticipantReportInfo = new DataTable();

            try
            {
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                dtParticipantReportInfo = assignQuestionnaireDataAccessObject.GetParticipantReportInfo(targetPersonID);
            }

            catch (Exception ex) { HandleException(ex); }
            return dtParticipantReportInfo;
        }
    }
}
