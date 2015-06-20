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
    public class AssignQuestionnaire_DAO : DAO_Base
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
        public AssignQuestionnaire_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<AssignQuestionnaire_BE> assignQuestionnaire_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public int AddAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE, IDbTransaction dbTransaction)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[13] {null,
                                                assignQuestionnaire_BE.AccountID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProgrammeID,
                                                assignQuestionnaire_BE.QuestionnaireID,
                                                assignQuestionnaire_BE.TargetPersonID,
                                                assignQuestionnaire_BE.FeedbackURL,
                                                assignQuestionnaire_BE.Description,
                                                assignQuestionnaire_BE.CandidateNo,
                                                assignQuestionnaire_BE.ModifiedBy,
                                                assignQuestionnaire_BE.ModifiedDate,
                                                assignQuestionnaire_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignQuestionnaireManagement", param, dbTransaction));

                // ADD RECORD OF CANDIDATES

                List<AssignmentDetails_BE> lstAssignmentDetails = new List<AssignmentDetails_BE>();
                lstAssignmentDetails = assignQuestionnaire_BE.AssignmentDetails;

                int newValue;

                for (int count = 0; count < lstAssignmentDetails.Count; count++)
                {
                    object[] newparam = new object[7] {returnValue, //assignment id
                                                lstAssignmentDetails[count].RelationShip,
                                                lstAssignmentDetails[count].CandidateName,
                                                lstAssignmentDetails[count].CandidateEmail,
                                                lstAssignmentDetails[count].SubmitFlag,
                                                lstAssignmentDetails[count].EmailSendFlag,
                                                "I" };

                    newValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignmentDetailsManagement", newparam, dbTransaction));
                }

                cDataSrc = null;

                //cDataSrc.ExecuteNonQuery("Update dbo.AssignQuestionnaire set FeedbackURL='Feedback.aspx?' where AssignmentID=" + returnValue + " and RelationShip='Self'", dbTransaction);
                
                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }


        public int?[] AddAssignQuestionnaireForColleagues(AssignQuestionnaire_BE assignQuestionnaire_BE, IDbTransaction dbTransaction)
        {
            int?[] asgnDetailedID = null;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[13] {null,
                                                assignQuestionnaire_BE.AccountID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProgrammeID,
                                                assignQuestionnaire_BE.QuestionnaireID,
                                                assignQuestionnaire_BE.TargetPersonID,
                                                assignQuestionnaire_BE.FeedbackURL,
                                                assignQuestionnaire_BE.Description,
                                                assignQuestionnaire_BE.CandidateNo,
                                                assignQuestionnaire_BE.ModifiedBy,
                                                assignQuestionnaire_BE.ModifiedDate,
                                                assignQuestionnaire_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignQuestionnaireManagement", param, dbTransaction));

                // ADD RECORD OF CANDIDATES

                List<AssignmentDetails_BE> lstAssignmentDetails = new List<AssignmentDetails_BE>();
                lstAssignmentDetails = assignQuestionnaire_BE.AssignmentDetails;

                int newValue;
                asgnDetailedID = new int?[lstAssignmentDetails.Count];

                for (int count = 0; count < lstAssignmentDetails.Count; count++)
                {
                    object[] newparam = new object[6] {returnValue, //assignment id
                                                lstAssignmentDetails[count].RelationShip,
                                                lstAssignmentDetails[count].CandidateName,
                                                lstAssignmentDetails[count].CandidateEmail,
                                                lstAssignmentDetails[count].SubmitFlag,
                                                lstAssignmentDetails[count].EmailSendFlag};

                    asgnDetailedID[count] = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignmentDetailsManagementForColleague", newparam, dbTransaction));
                    
                }

                cDataSrc = null;

                //cDataSrc.ExecuteNonQuery("Update dbo.AssignQuestionnaire set FeedbackURL='Feedback.aspx?' where AssignmentID=" + returnValue + " and RelationShip='Self'", dbTransaction);

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return asgnDetailedID;
        }


        /// <summary>
        /// Get assignment ID
        /// </summary>
        /// <param name="assignQuestionnaire_BE"></param>
        /// <returns></returns>
        public int GetAssignmentID(AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            int asgnID=0 ;
            try
            {

                object[] param = new object[5] {assignQuestionnaire_BE.AccountID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProgrammeID,
                                                assignQuestionnaire_BE.QuestionnaireID,
                                                assignQuestionnaire_BE.TargetPersonID };

                asgnID = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignmentIDSelect", param, null));
               

                cDataSrc = null;

            }
            catch (Exception ex) { HandleException(ex); }
            return asgnID;
        }




        public int UpdateAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {assignQuestionnaire_BE.AssignmentID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.QuestionnaireID,
                                                assignQuestionnaire_BE.TargetPersonID,
                                                assignQuestionnaire_BE.Description,
                                                assignQuestionnaire_BE.CandidateNo,
                                                assignQuestionnaire_BE.ModifiedBy,
                                                assignQuestionnaire_BE.ModifiedDate,
                                                assignQuestionnaire_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteAssignQuestionnaire(AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //assignQuestionnaire_BE.AssignmentDetails[0].AssignmentID;
                object[] param = new object[13] { assignQuestionnaire_BE.AssignmentID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

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

        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { assignmentID };

                dtResult = cDataSrc.ExecuteDataSet("UspAssignQuestionnaireSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public DataTable GetdtAssignQuestionnaireListDetails(string assignmentID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { assignmentID };

                dtResult = cDataSrc.ExecuteDataSet("UspAssignQuestionnaireDetailSelect", param, null).Tables[0];

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

        public DataTable chk_user_authority(int? grpID, int menuID)
        {
            DataTable result=null;
            try
            {
                
                object[] param = new object[2] { grpID,
                                                 menuID   
                                                    };

                result = cDataSrc.ExecuteDataSet("chk_user_authority", param, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }

            return result;
          
 }

        public DataTable GetAllAssignmentInfo(int candidateID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { candidateID };

                dtResult = cDataSrc.ExecuteDataSet("UspGetAllAssignmentInfo", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public DataTable GetdtAssignList(string userid, string projectid)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { userid, projectid, "A" };

                dtAllAssign = cDataSrc.ExecuteDataSet("UspAssignSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        /// <summary>
        /// Get Colleagues list based on userid and programmeId
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public DataTable GetdtAssignColleagueList(string userID, string programmeID)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { userID, programmeID };

                dtAllAssign = cDataSrc.ExecuteDataSet("UspAssignColleagueSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        public int GetAssignQuestionnaireListCount(string accountID, string projectid)
        {
            int AssignQuestionnaireCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));


                object[] param = new object[3] { accountID, projectid, "C" };

                AssignQuestionnaireCount = (int)cDataSrc.ExecuteScalar("UspAssignSelect", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return AssignQuestionnaireCount;
        }

        public DataTable GetdtAssignListNew(string condition)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                string sql = "SELECT dbo.[User].LoginID, dbo.[User].Password ,dbo.Account.Code, dbo.Project.Title ,dbo.Account.OrganisationName ,dbo.AssignmentDetails.AssignmentID AS AssiggnmentID,dbo.Programme.StartDate, dbo.Programme.EndDate, dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, " +
                      " dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignQuestionnaire.Description, dbo.AssignQuestionnaire.CandidateNo, " +
                      " dbo.AssignmentDetails.AsgnDetailID AS AssignmentID, dbo.AssignmentDetails.CandidateName, dbo.AssignmentDetails.CandidateEmail,dbo.AssignmentDetails.SubmitFlag, dbo.Project.ProjectID, " +
                      " dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, dbo.Account.Code, dbo.Questionnaire.QSTNName, dbo.[User].UserID, " +
                      " dbo.[User].FirstName + ' ' + dbo.[User].LastName AS FirstName, dbo.Programme.ProgrammeID, dbo.Programme.ProgrammeName" +
                      " FROM dbo.AssignQuestionnaire INNER JOIN " +
                      " dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN" +
                      " dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN" +
                      " dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN" +
                      " dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN" +
                      " dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN" +
                      " dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID" +
                     
                      " WHERE dbo.AssignQuestionnaire.AccountID=" + condition;

                dtAllAssign = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex) 
            { 
                HandleException(ex); 
            }
            return dtAllAssign;
        }

        public int GetAssignQuestionnaireListCount(string condition)
        {
            int AssignQuestionnaireCount = 0;
            try
            {
                string sql = "SELECT count(*)" +
                      " FROM dbo.AssignQuestionnaire INNER JOIN " +
                      " dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN" +
                      " dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN" +
                      " dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN" +
                      " dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN" +
                      " dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN" +
                      " dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID" + 
                      " WHERE dbo.AssignQuestionnaire.AccountID=" + condition;

                AssignQuestionnaireCount = (int)cDataSrc.ExecuteScalar(sql, null);
            }
            catch (Exception ex) 
            { 
                HandleException(ex); 
            }
            return AssignQuestionnaireCount;
        }

        public DataTable GetFeedbackURL(int TargetPersonID)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                dtAllAssign = cDataSrc.ExecuteDataSet("SELECT FeedbackUrl FROM AssignQuestionnaire WHERE TargetPersonID = " + TargetPersonID, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        public int GetAssignmentID(int TargetPersonID)
        {
            DataTable dtAllAssign = new DataTable();
            int assignmentID = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                string strQuery = "select	AsgnDetailID " +
                                  "from	dbo.AssignmentDetails  AD inner join AssignQuestionnaire AQ ON AQ.AssignmentID=AD.AssignmentID " +
                                  "where AQ.TargetPersonID=" + TargetPersonID.ToString() + " and AD.[RelationShip]='Self'";

                dtAllAssign = cDataSrc.ExecuteDataSet(strQuery, null).Tables[0];                                
                assignmentID = Convert.ToInt32(dtAllAssign.Rows[0][0].ToString());


                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return assignmentID;
        }

        public void SetFeedbackURL(int TargetPersonID, int AssignmentID, string FeedbackURL)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                int result = cDataSrc.ExecuteNonQuery("Update AssignQuestionnaire set FeedbackUrl='" + FeedbackURL + "' WHERE AssignmentID=" + AssignmentID, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        public DataTable GetParticipantAssignmentInfo(int TargetPersonID)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { TargetPersonID };
                dtAllAssign = cDataSrc.ExecuteDataSet("UspGetParticipantAllInfo", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        public DataTable GetCandidateEmailImageInfo(int ProjectID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "J"      };

                dtResult = cDataSrc.ExecuteDataSet("UspFindTemplate", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public DataTable GetParticipantEmailImageInfo(int ProjectID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "I"      };

                dtResult = cDataSrc.ExecuteDataSet("UspFindTemplate", param, null).Tables[0];

                 }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public void UpdateAssignProgramme(AssignQuestionnaire_BE assignquestionnaire_BE, IDbTransaction dbTransaction)
        {
            try
            {
                

                object[] newparam = new object[4] { assignquestionnaire_BE.AccountID, 
                                                    assignquestionnaire_BE.ProgrammeID,
                                                    assignquestionnaire_BE.NewProgrammeID, 
                                                    assignquestionnaire_BE.TargetPersonID,
                                                    };
                cDataSrc.ExecuteNonQuery("UspUpdateAssignProgramme", newparam, dbTransaction);                

                

                cDataSrc = null;
            }
            catch (Exception ex) { HandleException(ex); }
            
        }

        public void UpdateColleagueRelationship(int colleagueId, string relationship)
        {
            try
            {
                int result = cDataSrc.ExecuteNonQuery("Update AssignmentDetails set RelationShip='" + relationship + "' WHERE AsgnDetailID=" + colleagueId, null);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public DataTable GetColleaguesList(Int32 assignmentID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                object[] param = new object[2] { assignmentID, "A" };
                dtResult = cDataSrc.ExecuteDataSet("UspGetColleaguesList", param, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public DataTable GetUnsendEmailColleaguesList(Int32 assignmentID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                object[] param = new object[2] { assignmentID, "B" };
                dtResult = cDataSrc.ExecuteDataSet("UspGetColleaguesList", param, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public void UpdateEmailSendFlag(int candidateID)
        {
            try
            {
                int result = cDataSrc.ExecuteNonQuery("Update AssignmentDetails set EmailSendFlag=1 WHERE AsgnDetailID=" + candidateID, null);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void UpdateCandidateEmail(int AsgnDetailID,string newEmailValue,string CandidateName, string RelationShip)
        {
            
            try
            {
                object[] param = new object[4] { AsgnDetailID, newEmailValue, CandidateName, RelationShip };

                int result = cDataSrc.ExecuteNonQuery("uspAssignmentDetailsUpdateCandidateEmail",param,null);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }





    public class Survey_AssignQuestionnaire_DAO : DAO_Base
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
        public Survey_AssignQuestionnaire_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Survey_AssignQuestionnaire_BE> assignQuestionnaire_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public int AddAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaire_BE, IDbTransaction dbTransaction)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[12] {null,
                                                assignQuestionnaire_BE.AccountID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProgrammeID,
                                                assignQuestionnaire_BE.QuestionnaireID,

                                                assignQuestionnaire_BE.FeedbackURL,
                                                assignQuestionnaire_BE.Description,
                                                assignQuestionnaire_BE.CandidateNo,
                                                assignQuestionnaire_BE.ModifiedBy,
                                                assignQuestionnaire_BE.ModifiedDate,
                                                assignQuestionnaire_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspAssignQuestionnaireManagement", param, dbTransaction));

                // ADD RECORD OF CANDIDATES

                List<Survey_AssignmentDetails_BE> lstAssignmentDetails = new List<Survey_AssignmentDetails_BE>();
                lstAssignmentDetails = assignQuestionnaire_BE.AssignmentDetails;

                int newValue;

                for (int count = 0; count < lstAssignmentDetails.Count; count++)
                {
                    object[] newparam = new object[9] {returnValue, //assignment id
                                                lstAssignmentDetails[count].Analysis_I,
                                                lstAssignmentDetails[count].Analysis_II,
                                                lstAssignmentDetails[count].Analysis_III,
                                                lstAssignmentDetails[count].CandidateName,
                                                lstAssignmentDetails[count].CandidateEmail,
                                                lstAssignmentDetails[count].SubmitFlag,
                                                lstAssignmentDetails[count].EmailSendFlag,
                                                "I" };

                    newValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspAssignmentDetailsManagement", newparam, dbTransaction));
                }

                cDataSrc = null;

                //cDataSrc.ExecuteNonQuery("Update dbo.AssignQuestionnaire set FeedbackURL='Feedback.aspx?' where AssignmentID=" + returnValue + " and RelationShip='Self'", dbTransaction);

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[10] {assignQuestionnaire_BE.AssignmentID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.ProjecctID,
                                                assignQuestionnaire_BE.QuestionnaireID,
                                                
                                                assignQuestionnaire_BE.Description,
                                                assignQuestionnaire_BE.CandidateNo,
                                                assignQuestionnaire_BE.ModifiedBy,
                                                assignQuestionnaire_BE.ModifiedDate,
                                                assignQuestionnaire_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspAssignQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteAssignQuestionnaire(Survey_AssignQuestionnaire_BE assignQuestionnaire_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //assignQuestionnaire_BE.AssignmentDetails[0].AssignmentID;
                object[] param = new object[12] { assignQuestionnaire_BE.AssignmentID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspAssignQuestionnaireManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAssignQuestionnaireManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

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

        public DataTable GetdtAssignQuestionnaireList(Int32 assignmentID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { assignmentID };

                dtResult = cDataSrc.ExecuteDataSet("Survey_UspAssignQuestionnaireSelect", param, null).Tables[0];

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



        public String FindTemplate(int ProjectID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "A"      };

                Template = Convert.ToString(cDataSrc.ExecuteScalar("Survey_UspFindTemplate", param, null));

                cDataSrc = null;


            }
            catch (Exception ex) { HandleException(ex); }
            return Template;
        }


        public bool find_finish_email(string projectid,int copnayid)
        {
            bool bb=false;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { projectid, copnayid };

                bb = Convert.ToBoolean(cDataSrc.ExecuteScalar("Survey_find_finish_email", param, null));

                cDataSrc = null;


            }
            catch (Exception ex) { HandleException(ex); }
            return bb;

        }




        public DataTable GetAllAssignmentInfo(int candidateID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { candidateID };

                dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetAllAssignmentInfo", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }


        public DataTable GetdtAssignList(string userid, string projectid)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { userid, projectid, "A" };

                dtAllAssign = cDataSrc.ExecuteDataSet("Survey_UspAssignSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        public int GetAssignQuestionnaireListCount(string accountID, string projectid)
        {
            int AssignQuestionnaireCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));


                object[] param = new object[2] {projectid, "C" };

                AssignQuestionnaireCount = (int)cDataSrc.ExecuteScalar("Survey_QuestionairCount", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return AssignQuestionnaireCount;
        }

        public DataTable GetdtAssignListNew(string condition)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {

        string sql = "SELECT Survey_AssignQuestionnaire.AccountID,dbo.Survey_AssignmentDetails.AsgnDetailID AS AssignmentID,dbo.Survey_AssignmentDetails.CandidateName, dbo.Survey_Analysis_Sheet.ProgrammeName," +
 "dbo.Survey_AssignQuestionnaire.QuestionnaireID," +
 "dbo.Survey_AssignmentDetails.SubmitFlag,  dbo.Survey_Project.StatusID,dbo.Survey_Analysis_Sheet.ProgrammeID FROM " +
  "dbo.Survey_AssignQuestionnaire " +
  "INNER JOIN dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID =  dbo.Survey_AssignmentDetails.AssignmentID " +
   "INNER JOIN dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID= dbo.Survey_Project.ProjectID " +
   "INNER JOIN dbo.Survey_Analysis_Sheet ON dbo.Survey_Project.ProjectID = dbo.Survey_Analysis_Sheet.ProjectID " +
   "AND Survey_Analysis_Sheet.AccountID= Survey_AssignQuestionnaire.AccountID AND Survey_Analysis_Sheet.ProgrammeID = Survey_AssignQuestionnaire.ProgrammeID" +
   " WHERE dbo.Survey_AssignQuestionnaire.AccountID="+ condition;


            dtAllAssign = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtAllAssign;
        }

        public int GetAssignQuestionnaireListCount(string condition)
        {
            int AssignQuestionnaireCount = 0;
            try
            {
                    string sql = "SELECT count(*)" +
                      " FROM dbo.Survey_AssignQuestionnaire INNER JOIN " +
                      " dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID INNER JOIN" +
                      " dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN" +
                      " dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN" +
                      " dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaire.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN" +
                      
                      " dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaire.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID" +
                      " WHERE dbo.Survey_AssignQuestionnaire.AccountID=" + condition;

                AssignQuestionnaireCount = (int)cDataSrc.ExecuteScalar(sql, null);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return AssignQuestionnaireCount;
        }

        public DataTable GetFeedbackURL(int TargetPersonID)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                dtAllAssign = cDataSrc.ExecuteDataSet("SELECT FeedbackUrl FROM Survey_AssignQuestionnaire", null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        public void SetFeedbackURL(int TargetPersonID, int AssignmentID, string FeedbackURL)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                int result = cDataSrc.ExecuteNonQuery("Update Survey_AssignQuestionnaire set FeedbackUrl='" + FeedbackURL + "' WHERE AssignmentID=" + AssignmentID, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        public DataTable GetParticipantAssignmentInfo(int TargetPersonID)
        {
            DataTable dtAllAssign = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[1] { TargetPersonID };
                dtAllAssign = cDataSrc.ExecuteDataSet("Survey_UspGetParticipantAllInfo", param, null).Tables[0];

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAssign;
        }

        public DataTable GetCandidateEmailImageInfo(int ProjectID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "J"      };

                dtResult = cDataSrc.ExecuteDataSet("Survey_UspFindTemplate", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public DataTable GetParticipantEmailImageInfo(int ProjectID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { ProjectID,
                                                 "I"      };

                dtResult = cDataSrc.ExecuteDataSet("Survey_UspFindTemplate", param, null).Tables[0];

                //HandleWriteLogDAU("UspAssignQuestionnaireSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public void UpdateAssignProgramme(Survey_AssignQuestionnaire_BE assignquestionnaire_BE, IDbTransaction dbTransaction)
        {
            //object[] param = new object[6] {   assignquestionnaire_BE.AccountID,
            //                                    assignquestionnaire_BE.ProjecctID,
            //                                    assignquestionnaire_BE.ProgrammeID,
            //                                    assignquestionnaire_BE.NewProgrammeID,
            //                                    assignquestionnaire_BE.TargetPersonID,
            //                                    "U" };

            //cDataSrc.ExecuteNonQuery("UspUpdateAssignProgramme", param, null);

            try
            {
                //object[] param = new object[1] { assignquestionnaire_BE.TargetPersonID };
                //returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspUpdateAssignProgrammeSelect", param, dbTransaction));

                object[] newparam = new object[3] { assignquestionnaire_BE.AccountID, 
                                                    assignquestionnaire_BE.ProgrammeID,
                                                    assignquestionnaire_BE.NewProgrammeID, 
                                                   };
                cDataSrc.ExecuteNonQuery("UspUpdateAssignProgramme", newparam, dbTransaction);

                //string sql = "UPDATE [Feedback360_QA].[dbo].[AssignQuestionnaireParticipant]" +
                //            " SET [ProgrammeID] = " + assignquestionnaire_BE.NewProgrammeID +
                //            " WHERE [AssignmentID]= " + returnValue ;

                //cDataSrc.ExecuteNonQuery(sql, dbTransaction);

                cDataSrc = null;
            }
            catch (Exception ex) { HandleException(ex); }

        }


        public void UpdateColleagueRelationship(int colleagueId, string relationship)
        {
            try
            {
                int result = cDataSrc.ExecuteNonQuery("Update Survey_AssignmentDetails set RelationShip='" + relationship + "' WHERE AsgnDetailID=" + colleagueId, null);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public DataTable GetColleaguesList(Int32 assignmentID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                object[] param = new object[2] { assignmentID, "A" };
                dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetColleaguesList", param, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public DataTable GetColleaguesListView(Int32 projectID, Int32 progID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                object[] param = new object[2] { projectID, progID };
                dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetColleaguesListView", param, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public DataTable GetUnsendEmailColleaguesList(Int32 assignmentID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                object[] param = new object[2] { assignmentID, "B" };
                dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetColleaguesList", param, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }

            return dtResult;
        }

        public void Survey_ManageCollegue(string flag,int AssignmentId)
        {
            try
            {
                int result = cDataSrc.ExecuteNonQuery("Delete from Survey_AssignmentDetails WHERE AsgnDetailID=" + AssignmentId, null);
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
                int result = cDataSrc.ExecuteNonQuery("Update Survey_AssignmentDetails set EmailSendFlag=1 WHERE AsgnDetailID=" + candidateID, null);
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
                int result = cDataSrc.ExecuteNonQuery("Update Survey_AssignmentDetails set "+strsql+" WHERE AsgnDetailID=" + candidateID, null);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        public DataTable GetdtAssignmentId(int AssignmentDetailId)
        {
            DataTable dtAssignment = new DataTable();
            try
            {
                string sql = "SELECT AssignmentID from Survey_AssignmentDetails WHERE AsgnDetailID="+AssignmentDetailId;

                dtAssignment = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtAssignment;
        }
    }



}
