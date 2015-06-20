using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class AssignQstnParticipant_BAO : Base_BAO 
    {

        #region "Private Member Variable"

        private int addAssignQuestionnaire;
        private string Template;

        #endregion

        #region CRUD Operations

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

        public string GetReportFileName(int targetPersonID)
        {
            string fileName = "";

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                fileName = assignQuestionnaire_DAO.GetReportFileName(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return fileName;
        }

        public DataTable GetParticipantReportInfo(int targetPersonID)
        {
            DataTable dtParticipantReportInfo = new DataTable();

            try
            {
                AssignQstnParticipant_DAO assignQuestionnaire_DAO = new AssignQstnParticipant_DAO();
                dtParticipantReportInfo = assignQuestionnaire_DAO.GetParticipantReportInfo(targetPersonID);
            }

            catch (Exception ex) { HandleException(ex); }
            return dtParticipantReportInfo;
        }
    }


















    public class Survey_AssignQstnParticipant_BAO : Base_BAO
    {

        #region "Private Member Variable"

        private int addAssignQuestionnaire;
        private string Template;

        #endregion

        #region CRUD Operations

        public int AddAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaire_BE)
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
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

        public int UpdateAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaire_BE)
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
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

        public int DeleteAssignQuestionnaire(Questionnaire_BE.Survey_AssignQuestionnaire_BE questionnaireBE)
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
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

        public List<Survey_AssignQuestionnaire_BE> GetAssignQuestionnaireByID(int assignQuestionnaireID)
        {
            List<Survey_AssignQuestionnaire_BE> assignQuestionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
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

        public List<Survey_AssignQuestionnaire_BE> GetAssignQuestionnaireList()
        {
            List<Survey_AssignQuestionnaire_BE> assignQuestionnaire_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
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

        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetdtAssignQuestionnaireList(assignmentID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        public int GetAssignQuestionnaireListCount()
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                assignQuestionnaireCount = assignQuestionnaire_DAO.GetAssignQuestionnaireListCount();

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }


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
                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
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


        public DataTable GetuseridAssignQuestionnaireList(int userid)
        {
            DataTable dtuseridAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                dtuseridAssignQuestionnaire = assignQuestionnaire_DAO.GetdtuseridAssignQuestionnaireList(userid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtuseridAssignQuestionnaire;
        }

        public DataTable GetdtAssignPartiList(string accountID, string programmeID)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO Questionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignPartiList(accountID, programmeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }

        public int GetAssignPartiQuestionnaireListCount(string accountID, string Projectid)
        {
            int assignPartiQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                assignPartiQuestionnaireCount = assignQuestionnaire_DAO.GetAssignPartiQuestionnaireListCount(accountID, Projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignPartiQuestionnaireCount;
        }

        public DataTable GetdtAssignProgrammePartiList(string accountID, string Programmeid)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO Questionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignProgrammePartiList(accountID, Programmeid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }

        public int GetAssignProgammePartiQuestionnaireListCount(string accountID, string Programmeid)
        {
            int assignPartiQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
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

        public Int32 GetCandidatesCount(int targetPersonID)
        {
            int CandidatesCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaire_DAO.GetCandidatesCount(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }

        public Int32 GetSubmissionCount(int targetPersonID)
        {
            int CandidatesCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaire_DAO.GetSubmissionCount(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }

        public Int32 GetSelfAssessment(int targetPersonID)
        {
            int CandidatesCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                CandidatesCount = assignQuestionnaire_DAO.GetSelfAssessment(targetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return CandidatesCount;
        }

        public string GetReportFileName(int strAccountID,int strProjectID,int strProgrammeID)
        {
            string fileName = "";

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                fileName = assignQuestionnaire_DAO.GetReportFileName( strAccountID,strProjectID,strProgrammeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return fileName;
        }

        public DataTable GetParticipantReportInfo(int targetPersonID)
        {
            DataTable dtParticipantReportInfo = new DataTable();

            try
            {
                Survey_AssignQstnParticipant_DAO assignQuestionnaire_DAO = new Survey_AssignQstnParticipant_DAO();
                dtParticipantReportInfo = assignQuestionnaire_DAO.GetParticipantReportInfo(targetPersonID);
            }

            catch (Exception ex) { HandleException(ex); }
            return dtParticipantReportInfo;
        }
    }




















}
