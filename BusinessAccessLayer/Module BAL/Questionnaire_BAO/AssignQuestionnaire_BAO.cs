using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class AssignQuestionnaire_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addAssignQuestionnaire;
        private string Template;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Assign Questionnaire
        /// </summary>
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
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
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
        /// Insert Assign Questionnaire for colleague
        /// </summary>
        public int?[] AddAssignQuestionnaireForColleagues(AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
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
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                asgnDetailedID = assignQuestionnaireDataAccessObject.AddAssignQuestionnaireForColleagues(assignQuestionnaireBusinessEntity, dbTransaction);
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

        /// <summary>
        /// Get Assignment ID
        /// </summary>
        public int GetAssignmentID(AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
        {
            int assignmentID = 0;
            try
            {
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                assignmentID = assignQuestionnaireDataAccessObject.GetAssignmentID(assignQuestionnaireBusinessEntity);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return assignmentID;
        }

        /// <summary>
        /// Update Assign Questionnaire
        /// </summary>
        /// <param name="assignQuestionnaireBusinessEntity"></param>
        /// <returns></returns>
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
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
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
        /// <returns></returns>
        public int DeleteAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
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
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.DeleteAssignQuestionnaire(assignQuestionnaireBusinessEntity);
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
        /// Get Assign Questionnaire By ID
        /// </summary>
        /// <returns></returns>
        public List<AssignQuestionnaire_BE> GetAssignQuestionnaireByID(int assignQuestionnaireID)
        {
            List<AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
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
        /// Get Assign Questionnaire List
        /// </summary>
        /// <returns></returns>
        public List<AssignQuestionnaire_BE> GetAssignQuestionnaireList()
        {
            List<AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
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
        /// Get Assign Questionnaire List by assignment id.
        /// </summary>
        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dataTableAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
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
        /// Get Assign Questionnaire List Details by assignment id.
        /// </summary>
        /// <param name="assignmentID"> assignment id.</param>
        /// <returns></returns>
        public DataTable GetdtAssignQuestionnaireListDetails(string assignmentID)
        {
            DataTable dataTableAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetdtAssignQuestionnaireListDetails(assignmentID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignQuestionnaire;
        }

        /// <summary>
        /// Get Assign Questionnaire List Count
        /// </summary>
        /// <param name="accountID">accountID</param>
        /// <param name="Projectid">Projectid</param>
        /// <returns></returns>
        public int GetAssignQuestionnaireListCount(string accountID, string Projectid)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignQuestionnaireListCount(accountID, Projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        /// <summary>
        /// Get All Assignment Information by candidate id.
        /// </summary>
        /// <param name="candidateID">candidateID</param>
        /// <returns></returns>
        public DataTable GetAllAssignmentInfo(Int32 candidateID)
        {
            DataTable dataTableAssignmentInformation = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssignmentInformation = assignQuestionnaireDataAccessObject.GetAllAssignmentInfo(candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInformation;
        }

        /// <summary>
        /// Get Template details by projectid.
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
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
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
        /// Find Participant Template by Project ID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
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
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindParticipantTemplate(ProjectID);
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
        /// Find Participant Subject Template by Project ID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
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
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindParticipantSubjectTemplate(ProjectID);
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
        /// Find Candidate Subject Template by Project ID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
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
                AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindCandidateSubjectTemplate(ProjectID);
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
        /// Get Assign List
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="projectid">project id</param>
        /// <returns></returns>
        public DataTable GetdtAssignList(string userID, string projectid)
        {
            DataTable dataTableAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO QuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssign = QuestionnaireDataAccessObject.GetdtAssignList(userID, projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssign;
        }

        /// <summary>
        /// Get Colleagues list based on userid and programmeId
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public DataTable GetdtAssignColleagueList(string userID, string programmeID)
        {
            DataTable dataTableAssign = null;

            try
            {
                AssignQuestionnaire_DAO QuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssign = QuestionnaireDataAccessObject.GetdtAssignColleagueList(userID, programmeID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssign;
        }

        /// <summary>
        /// Get Assign List
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetdtAssignListNew(string condition)
        {
            DataTable dataTableAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO QuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssign = QuestionnaireDataAccessObject.GetdtAssignListNew(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssign;
        }

        /// <summary>
        /// Get Assign Questionnaire List Count
        /// </summary>
        public int GetAssignQuestionnaireListCount(string condition)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignQuestionnaireListCount(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        /// <summary>
        /// Get Feedback URL by Target Person ID
        /// </summary>
        /// <param name="TargetPersonID">Target Person ID</param>
        /// <returns></returns>
        public DataTable GetFeedbackURL(int TargetPersonID)
        {
            DataTable dataTableAssignmentInformation = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssignmentInformation = assignQuestionnaireDataAccessObject.GetFeedbackURL(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInformation;
        }

        /// <summary>
        /// Get Assignment ID by Target PersonID
        /// </summary>
        /// <param name="TargetPersonID"></param>
        /// <returns></returns>
        public int GetAssignmentID(int TargetPersonID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                return assignQuestionnaireDataAccessObject.GetAssignmentID(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        /// <summary>
        /// update Feedback URL
        /// </summary>
        /// <param name="TargetPersonID">TargetPersonID</param>
        /// <param name="AssignmentID">AssignmentID</param>
        /// <param name="FeedbackURL">FeedbackURL</param>
        public void SetFeedbackURL(int TargetPersonID, int AssignmentID, string FeedbackURL)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.SetFeedbackURL(TargetPersonID, AssignmentID, FeedbackURL);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }


        #endregion

        /// <summary>
        /// Get Participant Assignment details
        /// </summary>
        /// <param name="TargetPersonID"></param>
        /// <returns></returns>
        public DataTable GetParticipantAssignmentInfo(Int32 TargetPersonID)
        {
            DataTable dataTableAssignmentInformation = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssignmentInformation = assignQuestionnaireDataAccessObject.GetParticipantAssignmentInfo(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInformation;
        }

        /// <summary>
        /// Get Candidate Email Image details by project id.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable GetCandidateEmailImageInfo(int ProjectID)
        {
            DataTable dataTableAssignmentInformation = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssignmentInformation = assignQuestionnaireDataAccessObject.GetCandidateEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInformation;
        }

        /// <summary>
        /// Get Participant Email Image details by project id.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable GetParticipantEmailImageInfo(int ProjectID)
        {
            DataTable dataTableAssignmentInformation = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dataTableAssignmentInformation = assignQuestionnaireDataAccessObject.GetParticipantEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInformation;
        }

        /// <summary>
        /// Update Assign Programme
        /// </summary>
        public void UpdateAssignProgramme(AssignQuestionnaire_BE assignquestionnaireBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.UpdateAssignProgramme(assignquestionnaireBusinessEntity, dbTransaction);

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

        /// <summary>
        /// Update Colleague Relationship
        /// </summary>
        /// <param name="colleagueId">colleague Id</param>
        /// <param name="relationship">relationship</param>
        public void UpdateColleagueRelationship(int colleagueId, string relationship)
        {
            try
            {
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.UpdateColleagueRelationship(colleagueId, relationship);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// check user authority to access menus
        /// </summary>
        /// <param name="grpID"></param>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public DataTable chk_user_authority(int? grpID, int menuID)
        {

            DataTable result = null;
            try
            {
                AssignQuestionnaire_DAO checkUser = new AssignQuestionnaire_DAO();
                result = checkUser.chk_user_authority(grpID, menuID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;

        }

        /// <summary>
        /// Get Colleagues List by assignment ID
        /// </summary>
        /// <param name="assignmentID">assignment ID</param>
        /// <returns></returns>
        public DataTable GetColleaguesList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        /// <summary>
        /// Get Unsend Email Colleagues List
        /// </summary>
        /// <param name="assignmentID">Assignment ID</param>
        /// <returns></returns>
        public DataTable GetUnsendEmailColleaguesList(Int32 assignmentID)
        {
            DataTable dtAssignQuestionnaire = null;

            try
            {
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                dtAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetUnsendEmailColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAssignQuestionnaire;
        }

        /// <summary>
        /// Update Email Send Flag by candidate ID
        /// </summary>
        /// <param name="candidateID">candidate ID</param>
        public void UpdateEmailSendFlag(int candidateID)
        {
            try
            {
                AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.UpdateEmailSendFlag(candidateID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Update Candidate Email
        /// </summary>
        /// <param name="AsgnDetailID">AsgnDetail ID</param>
        /// <param name="newEmailValue">new Email Value</param>
        /// <param name="CandidateName">Candidate Name</param>
        /// <param name="RelationShip">RelationShip</param>
        public void UpdateCandidateEmail(int AsgnDetailID, string newEmailValue, string CandidateName, string RelationShip)
        {
            try
            {
                AssignQuestionnaire_DAO assignQuestionnairDataAccessObject = new AssignQuestionnaire_DAO();
                assignQuestionnairDataAccessObject.UpdateCandidateEmail(AsgnDetailID, newEmailValue, CandidateName, RelationShip);
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
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
        /// <param name="assignQuestionnaireBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaireBusinessEntity)
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                addAssignQuestionnaire = assignQuestionnaireDataAccessObject.DeleteAssignQuestionnaire(assignQuestionnaireBusinessEntity);
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
        /// Get Assign Questionnaire by id.
        /// </summary>
        /// <param name="assignQuestionnaireID"></param>
        /// <returns></returns>
        public List<Survey_AssignQuestionnaire_BE> GetAssignQuestionnaireByID(int assignQuestionnaireID)
        {
            List<Survey_AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
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
        /// Get Assign Questionnaire List
        /// </summary>
        /// <returns></returns>
        public List<Survey_AssignQuestionnaire_BE> GetAssignQuestionnaireList()
        {
            List<Survey_AssignQuestionnaire_BE> assignQuestionnaireBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
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
        /// Get Finish email
        /// </summary>
        /// <param name="projectId">project Id</param>
        /// <param name="companyid">company id</param>
        /// <returns></returns>
        public bool find_finish_email(string projectId, int companyid)
        {
            Survey_AssignQuestionnaire_DAO get_email_finish_info = new Survey_AssignQuestionnaire_DAO();
            bool bb = get_email_finish_info.find_finish_email(projectId, companyid);
            return bb;

        }

        /// <summary>
        /// Get Assign Questionnaire List by assignment ID
        /// </summary>
        /// <param name="assignmentID">assignment ID</param>
        /// <returns></returns>
        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dataTableAssignQuestionnaire = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
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
        /// Get Assign Questionnaire List count
        /// </summary>
        public int GetAssignQuestionnaireListCount(string accountID, string Projectid)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignQuestionnaireListCount(accountID, Projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        /// <summary>
        /// Get All Assignment details by  candidateID
        /// </summary>
        /// <param name="candidateID">candidate ID</param>
        /// <returns></returns>
        public DataTable GetAllAssignmentInfo(Int32 candidateID)
        {
            DataTable dataTableAssignmentInformation = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                dataTableAssignmentInformation = assignQuestionnaireDataAccessObject.GetAllAssignmentInfo(candidateID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInformation;
        }

        /// <summary>
        /// Get Email template by ProjectID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
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
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
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
        /// Find Participant Template by project id.
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindParticipantTemplate(ProjectID);
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
        /// Find Participant Subject Template by ProjectID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindParticipantSubjectTemplate(ProjectID);
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
        /// Find Candidate Subject Template by Project ID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
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
                Survey_AssignQstnParticipant_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQstnParticipant_DAO();
                Template = assignQuestionnaireDataAccessObject.FindCandidateSubjectTemplate(ProjectID);
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
        /// Get Assignment List
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="projectid">project id</param>
        /// <returns></returns>
        public DataTable GetdtAssignList(string userID, string projectid)
        {
            DataTable BusinessEntityAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO QuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                BusinessEntityAssign = QuestionnaireDataAccessObject.GetdtAssignList(userID, projectid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return BusinessEntityAssign;
        }

        /// <summary>
        /// Get Assign List 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetdtAssignListNewSurvey(string condition)
        {
            DataTable BusinessEntityAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO QuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                BusinessEntityAssign = QuestionnaireDataAccessObject.GetdtAssignListNew(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return BusinessEntityAssign;
        }

        /// <summary>
        /// Get Assignment Id
        /// </summary>
        /// <param name="AssignmentDteailId"></param>
        /// <returns></returns>
        public DataTable GetdtAssignmentId(int AssignmentDteailId)
        {
            DataTable BusinessEntityAssign = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO QuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                BusinessEntityAssign = QuestionnaireDataAccessObject.GetdtAssignmentId(AssignmentDteailId);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return BusinessEntityAssign;
        }

        /// <summary>
        /// Get Assign Questionnaire List Count
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int GetAssignQuestionnaireListCount(string condition)
        {
            int assignQuestionnaireCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireCount = assignQuestionnaireDataAccessObject.GetAssignQuestionnaireListCount(condition);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return assignQuestionnaireCount;
        }

        /// <summary>
        /// Get Feedback URL by TargetPerson ID
        /// </summary>
        /// <param name="TargetPersonID">TargetPerson ID</param>
        /// <returns></returns>
        public DataTable GetFeedbackURL(int TargetPersonID)
        {
            DataTable BusinessEntityAssignmentInformation = null;
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
            BusinessEntityAssignmentInformation = assignQuestionnaireDataAccessObject.GetFeedbackURL(TargetPersonID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return BusinessEntityAssignmentInformation;
        }

        /// <summary>
        /// Update Feedback URL
        /// </summary>
        /// <param name="TargetPersonID">TargetPerson ID</param>
        /// <param name="AssignmentID">Assignment ID</param>
        /// <param name="FeedbackURL">FeedbackURL</param>
        public void SetFeedbackURL(int TargetPersonID, int AssignmentID, string FeedbackURL)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.SetFeedbackURL(TargetPersonID, AssignmentID, FeedbackURL);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }
        #endregion

        /// <summary>
        /// Get Participant Assignment details by TargetPerson ID
        /// </summary>
        /// <param name="TargetPersonID">TargetPerson ID</param>
        /// <returns></returns>
        public DataTable GetParticipantAssignmentInfo(Int32 TargetPersonID)
        {
            DataTable dataTableAssignmentInformation = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                dataTableAssignmentInformation = assignQuestionnaireDataAccessObject.GetParticipantAssignmentInfo(TargetPersonID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInformation;
        }

        /// <summary>
        /// Get Candidate Email Image details by project id
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public DataTable GetCandidateEmailImageInfo(int ProjectID)
        {
            DataTable dataTableAssignmentInforamtion = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                dataTableAssignmentInforamtion = assignQuestionnaireDataAccessObject.GetCandidateEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInforamtion;
        }

        /// <summary>
        /// Get Participant Email Image details by Project ID
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public DataTable GetParticipantEmailImageInfo(int ProjectID)
        {
            DataTable dataTableAssignmentInforamtion = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                dataTableAssignmentInforamtion = assignQuestionnaireDataAccessObject.GetParticipantEmailImageInfo(ProjectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignmentInforamtion;
        }

        /// <summary>
        /// Update Assign Programme
        /// </summary>
        /// <param name="assignquestionnaireBusinessEntity"></param>
        public void UpdateAssignProgramme(Survey_AssignQuestionnaire_BE assignquestionnaireBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.UpdateAssignProgramme(assignquestionnaireBusinessEntity, dbTransaction);

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

        /// <summary>
        /// Update Colleague Relationship
        /// </summary>
        /// <param name="colleagueId"></param>
        /// <param name="relationship"></param>
        public void UpdateColleagueRelationship(int colleagueId, string relationship)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.UpdateColleagueRelationship(colleagueId, relationship);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Get Colleagues List by assignmentID
        /// </summary>
        /// <param name="assignmentID">assignment ID</param>
        /// <returns></returns>
        public DataTable GetColleaguesList(Int32 assignmentID)
        {
            DataTable dataTableAssignQuestionnaire = null;

            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                dataTableAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignQuestionnaire;
        }

        /// <summary>
        /// Get Colleagues List 
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="progID">program ID</param>
        /// <returns></returns>
        public DataTable GetColleaguesListView(Int32 projectID, Int32 progID)
        {
            DataTable dataTableAssignQuestionnaire = null;

            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                dataTableAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetColleaguesListView(projectID, progID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignQuestionnaire;
        }

        /// <summary>
        /// Get Unsend Email Colleagues List by assignment ID
        /// </summary>
        /// <param name="assignmentID">assignment ID</param>
        /// <returns></returns>
        public DataTable GetUnsendEmailColleaguesList(Int32 assignmentID)
        {
            DataTable dataTableAssignQuestionnaire = null;

            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                dataTableAssignQuestionnaire = assignQuestionnaireDataAccessObject.GetUnsendEmailColleaguesList(assignmentID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAssignQuestionnaire;
        }

        /// <summary>
        /// Manage Survey Collegue
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="AssignmentId"></param>
        public void Survey_ManageCollegue(string flag, int AssignmentId)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.Survey_ManageCollegue(flag, AssignmentId);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Update Email Send Flag
        /// </summary>
        /// <param name="candidateID"></param>
        public void UpdateEmailSendFlag(int candidateID)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.UpdateEmailSendFlag(candidateID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Update Analysis by candidate ID
        /// </summary>
        /// <param name="candidateID"></param>
        /// <param name="strsql"></param>
        public void UpdateAnalysis(int candidateID, string strsql)
        {
            try
            {
                Survey_AssignQuestionnaire_DAO assignQuestionnaireDataAccessObject = new Survey_AssignQuestionnaire_DAO();
                assignQuestionnaireDataAccessObject.UpdateAnalysis(candidateID, strsql);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}
