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
    public class AssignQuestionnaire_BAO:Base_BAO 
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
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                addAssignQuestionnaire = assignQuestionnaire_DAO.AddAssignQuestionnaire(assignQuestionnaire_BE,dbTransaction);
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

        public int?[] AddAssignQuestionnaireForColleagues(AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;
            int?[] asgnDetailedID = null;
            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                

                //HandleWriteLog("Start", new StackTrace(true));
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                asgnDetailedID = assignQuestionnaire_DAO.AddAssignQuestionnaireForColleagues(assignQuestionnaire_BE, dbTransaction);
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
            return asgnDetailedID;
        }


        public int GetAssignmentID(AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            int assignmentID=0;
            try
            {
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignmentID= assignQuestionnaire_DAO.GetAssignmentID(assignQuestionnaire_BE);
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignmentID;
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
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
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

        public int DeleteAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE)
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
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                addAssignQuestionnaire = assignQuestionnaire_DAO.DeleteAssignQuestionnaire(assignQuestionnaire_BE);
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

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
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

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
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

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetdtAssignQuestionnaireList(assignmentID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        public DataTable GetdtAssignQuestionnaireListDetails(string assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetdtAssignQuestionnaireListDetails(assignmentID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        public int GetAssignQuestionnaireListCount(string accountID,string Projectid)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaire_DAO.GetAssignQuestionnaireListCount(accountID, Projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        public DataTable GetAllAssignmentInfo(Int32 candidateID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetAllAssignmentInfo(candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
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
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
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


        public String FindParticipantTemplate(int ProjectID)
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
                Template = assignQuestionnaire_DAO.FindParticipantTemplate(ProjectID);
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

        public String FindParticipantSubjectTemplate(int ProjectID)
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
                Template = assignQuestionnaire_DAO.FindParticipantSubjectTemplate(ProjectID);
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


        public String FindCandidateSubjectTemplate(int ProjectID)
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
                Template = assignQuestionnaire_DAO.FindCandidateSubjectTemplate(ProjectID);
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


        public DataTable GetdtAssignList(string userID, string projectid)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO Questionnaire_DAO = new AssignQuestionnaire_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignList(userID, projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }

        /// <summary>
        /// Get Colleagues list based on userid and programmeId
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public DataTable GetdtAssignColleagueList(string userID, string programmeID)
        {
            DataTable dtCAssign = null;

            try
            {
                AssignQuestionnaire_DAO Questionnaire_DAO = new AssignQuestionnaire_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignColleagueList(userID, programmeID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }

        public DataTable GetdtAssignListNew(string condition)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO Questionnaire_DAO = new AssignQuestionnaire_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignListNew(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }

        public int GetAssignQuestionnaireListCount(string condition)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaire_DAO.GetAssignQuestionnaireListCount(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        public DataTable GetFeedbackURL(int TargetPersonID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetFeedbackURL(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
        }

        public int GetAssignmentID(int TargetPersonID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                return assignQuestionnaire_DAO.GetAssignmentID(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public void SetFeedbackURL(int TargetPersonID, int AssignmentID, string FeedbackURL)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.SetFeedbackURL(TargetPersonID,AssignmentID,FeedbackURL);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }


        #endregion

        public DataTable GetParticipantAssignmentInfo(Int32 TargetPersonID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetParticipantAssignmentInfo(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
        }


        public DataTable GetCandidateEmailImageInfo(int ProjectID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetCandidateEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
        }

        public DataTable GetParticipantEmailImageInfo(int ProjectID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetParticipantEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
        }

        public void UpdateAssignProgramme(AssignQuestionnaire_BE assignquestionnaire_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateAssignProgramme(assignquestionnaire_BE,dbTransaction);

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
        }

        public void UpdateColleagueRelationship(int colleagueId, string relationship)
        {
            try
            {
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateColleagueRelationship(colleagueId, relationship);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public DataTable chk_user_authority(int? grpID, int menuID)
        {

            DataTable result = null;
            try
            {
                AssignQuestionnaire_DAO chk_user = new AssignQuestionnaire_DAO();
                result = chk_user.chk_user_authority(grpID, menuID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;

        }

        public DataTable GetColleaguesList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        public DataTable GetUnsendEmailColleaguesList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetUnsendEmailColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }
        
        public void UpdateEmailSendFlag(int candidateID)
        {
            try
            {
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateEmailSendFlag(candidateID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void UpdateCandidateEmail(int AsgnDetailID, string newEmailValue, string CandidateName, string RelationShip)
        {
            try
            {
                AssignQuestionnaire_DAO assignQuestionnaire_DAO = new AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateCandidateEmail(AsgnDetailID, newEmailValue, CandidateName, RelationShip);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        
    }





    



    public class Survey_AssignQuestionnaire_BAO : Base_BAO
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
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

        public int DeleteAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaire_BE)
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                addAssignQuestionnaire = assignQuestionnaire_DAO.DeleteAssignQuestionnaire(assignQuestionnaire_BE);
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

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
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

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
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


        public bool find_finish_email(string projectId,int companyid)
        {
            Survey_AssignQuestionnaire_DAO get_email_finish_info = new Survey_AssignQuestionnaire_DAO();
            bool bb = get_email_finish_info.find_finish_email(projectId, companyid);
            return bb;

        }



        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetdtAssignQuestionnaireList(assignmentID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        public int GetAssignQuestionnaireListCount(string accountID, string Projectid)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaire_DAO.GetAssignQuestionnaireListCount(accountID, Projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        public DataTable GetAllAssignmentInfo(Int32 candidateID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetAllAssignmentInfo(candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
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


        public String FindParticipantTemplate(int ProjectID)
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
                Template = assignQuestionnaire_DAO.FindParticipantTemplate(ProjectID);
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

        public String FindParticipantSubjectTemplate(int ProjectID)
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
                Template = assignQuestionnaire_DAO.FindParticipantSubjectTemplate(ProjectID);
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


        public String FindCandidateSubjectTemplate(int ProjectID)
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
                Template = assignQuestionnaire_DAO.FindCandidateSubjectTemplate(ProjectID);
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


        public DataTable GetdtAssignList(string userID, string projectid)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO Questionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignList(userID, projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }

        public DataTable GetdtAssignListNewSurvey(string condition)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO Questionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignListNew(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }
        public DataTable GetdtAssignmentId(int AssignmentDteailId)
        {
            DataTable dtCAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO Questionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtCAssign = Questionnaire_DAO.GetdtAssignmentId(AssignmentDteailId);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCAssign;
        }
        public int GetAssignQuestionnaireListCount(string condition)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaire_DAO.GetAssignQuestionnaireListCount(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        public DataTable GetFeedbackURL(int TargetPersonID)
        {
            DataTable dtAssignmentInfo = null;
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetFeedbackURL(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtAssignmentInfo;
        }

        public void SetFeedbackURL(int TargetPersonID, int AssignmentID, string FeedbackURL)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.SetFeedbackURL(TargetPersonID, AssignmentID, FeedbackURL);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }


        #endregion

        public DataTable GetParticipantAssignmentInfo(Int32 TargetPersonID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetParticipantAssignmentInfo(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
        }


        public DataTable GetCandidateEmailImageInfo(int ProjectID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetCandidateEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
        }

        public DataTable GetParticipantEmailImageInfo(int ProjectID)
        {
            DataTable dtAssignmentInfo = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignmentInfo = assignQuestionnaire_DAO.GetParticipantEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignmentInfo;
        }

        public void UpdateAssignProgramme(Survey_AssignQuestionnaire_BE assignquestionnaire_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateAssignProgramme(assignquestionnaire_BE, dbTransaction);

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
        }

        public void UpdateColleagueRelationship(int colleagueId, string relationship)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateColleagueRelationship(colleagueId, relationship);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public DataTable GetColleaguesList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }
        public DataTable GetColleaguesListView(Int32 projectID, Int32 progID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetColleaguesListView(projectID, progID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        
        public DataTable GetUnsendEmailColleaguesList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaire_DAO.GetUnsendEmailColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }


        public void Survey_ManageCollegue(string flag,int AssignmentId)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.Survey_ManageCollegue(flag,AssignmentId);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void UpdateEmailSendFlag(int candidateID)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateEmailSendFlag(candidateID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        public void UpdateAnalysis(int candidateID,string strsql)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaire_DAO = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaire_DAO.UpdateAnalysis(candidateID,strsql);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


    }

    
}
