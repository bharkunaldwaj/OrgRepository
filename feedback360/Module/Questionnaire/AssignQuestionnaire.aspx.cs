using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using DatabaseAccessUtilities;
using Miscellaneous;
using Admin_BE;
using System.Net.Mail;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;


public partial class Module_Questionnaire_AssignQuestionnaire : CodeBehindBase
{
    #region Private Constant
    const string ProjectTextField = "Title";
    const string ProjectValueField = "ProjectID";
    const string ProgramTextField = "ProgrammeName";
    const string ProgramValueField = "ProgrammeID";
    const string AccountTextField = "Code";
    const string AccountValueField = "AccountID";
    const string DefaulText = "Select";
    const string DefaulValue = "0";
    #endregion

    //Global variable

    //int i;
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string strName = string.Empty;
    //bool flag = true;
    //int j;
    //string file1;
    //string filename1;
    //string expression1;
    string expression2;
    //string Finalexpression;
    string Finalexpression2;
    //string expression6;
    //string Finalexpression6;
    string email;
    string Template;
    string finalemail;
    //string Questionnaire_id;
    //string mailid;
    WADIdentity identity;
    //DataTable CompanyName;
    //DataTable dtAllAccount;
    //string expression5;
    //string Finalexpression5;
    int targetpersonid;
    int UserAccountID;
    string ProjectId1;
    string ProjectId2;
    string ProjectId3;
    string ProjectId5;
    string Subject;
    bool isManager = false;
    bool duplicateManager = false;

    StringBuilder sb = new StringBuilder();
    DataTable dtCandidateList = null;

    string strFrontPage;
    string strReportType = string.Empty;
    string strReportIntroduction;
    string strConclusionPage;
    string strRadarChart;
    string strDetailedQst;
    string strCategoryQstlist;
    string strCategoryBarChart;
    string strSelfNameGrp;
    string strFullProjGrp;
    string strProgrammeGrp;
    string strGroupList;
    string strConHighLowRange;
    string strPreScoreVisibility = string.Empty;
    string strBenchMarkGrpVisibility = string.Empty;
    string strBenchMarkVisibility = string.Empty;
    string strBenchConclusionPageVisibility = string.Empty;
    string targetradarname = string.Empty;
    string targetradarPreviousScore = string.Empty;
    string targetradarBenchmark = string.Empty;

    string strColleagueNo = string.Empty;
    int colleagueRecordCount = 0;

    #region Protected Methods
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        Session["FeedbackType"] = "FeedBack360";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        identity = this.Page.User.Identity as WADIdentity;
        int? groupID = identity.User.GroupID;

        //AssignQuestionnaire_BAO chk_user = new AssignQuestionnaire_BAO();
        //DataTable ddd = chk_user.chk_user_authority(grpID, 12);
        //if (Convert.ToInt32(ddd.Rows[0][0]) == 0)
        //{
        //    Response.Redirect("UnAuthorized.aspx");
        //}

        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        if (!IsPostBack)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);
            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            Session["UnsavedColleagueTable"] = null;
            Session["ColleaguesIndex"] = null;
            Session["ColleagueTable"] = null;

            AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
            //Get all Questionnaire List by user id.
            DataTable dataTableUserList = assignquestionnaire.GetuseridAssignQuestionnaireList(userid);
            Project_BAO projectBusinessAccessObject = new Project_BAO();

            //Account_BAO account_BAO = new Account_BAO();
            //ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            //ddlAccountCode.DataValueField = "AccountID";
            //ddlAccountCode.DataTextField = "Code";
            //ddlAccountCode.DataBind();

            //Bind dropdown controls
            BindControls(identity.User.GroupID);

            //if (identity.User.GroupID == 1)
            //{
            //    divAccount.Visible = true;
            //    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
            //    ddlAccountCode_SelectedIndexChanged(sender, e);
            //}
            //else
            //{
            //    divAccount.Visible = false;
            //    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
            //}

            //Hide controls according to role
            if (identity.User.GroupID.ToString() != participantRoleId)
            {
                //ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                //ddlProject.DataValueField = "ProjectID";
                //ddlProject.DataTextField = "Title";
                //ddlProject.DataBind();

                tblParticipantUpload.Visible = true;
                ddlProject.Enabled = true;
                ddlProgramme.Enabled = true;
                lblMandatory.Visible = true;
                ibtnHelp.Visible = false;
            }
            else
            {
                tblParticipantUpload.Visible = false;
                lblMandatory.Visible = false;
                ibtnHelp.Visible = true;
                //Set the value for controls
                SetValues();

                trTargetPerson.Visible = false;

                AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
                System.Web.UI.WebControls.Image imgHeader = (System.Web.UI.WebControls.Image)Master.FindControl("imgProjectLogo");
               
                DataTable dataTableParticipantInfo = new DataTable();
                //get Participant details by userId
                dataTableParticipantInfo = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                if (dataTableParticipantInfo.Rows.Count > 0)
                {
                    string programmeId = dataTableParticipantInfo.Rows[0]["ProgrammeID"].ToString();
                    //get candidate list by program id
                    dtCandidateList = assignQuestionnaireBusinessAccessObject.GetdtAssignList(identity.User.UserID.ToString(), programmeId);


                    //Set Project Logo
                    if (dataTableParticipantInfo.Rows[0]["Logo"].ToString() != "")
                    {
                        imgHeader.Visible = true;
                        imgHeader.ImageUrl = "~/UploadDocs/" + dataTableParticipantInfo.Rows[0]["Logo"].ToString();
                    }
                }
            }
        }
    }

#if CommentOut
    //protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        imbAssign.Enabled = false;
    //        lblMessage.Text = "";
    //        lblMessage2.Text = "";
    //        lblvalidation.Text = "";
    //        //HandleWriteLog("Start", new StackTrace(true));
    //        identity = this.Page.User.Identity as WADIdentity;

    //        AssignQuestionnaire_BE assignquestionnaire_BE = new AssignQuestionnaire_BE();
    //        AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();

    //        assignquestionnaire_BE.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
    //        assignquestionnaire_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);

    //        //Changes here 
    //        //assignquestionnaire_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

    //        string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

    //        if (ddlTargetPerson.Visible == false)
    //        {
    //            assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(identity.User.UserID);
    //        }
    //        else
    //        {
    //            assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
    //        }

    //        assignquestionnaire_BE.Description = "";// txtDescription.Text.Trim();
    //        identity = this.Page.User.Identity as WADIdentity;

    //        if (identity.User.GroupID == 1)
    //        {
    //            assignquestionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
    //            dtCandidateList = assignquestionnaire_BAO.GetdtAssignList(ddlTargetPerson.SelectedValue, ddlProgramme.SelectedValue);
    //        }
    //        else
    //        {
    //            assignquestionnaire_BE.AccountID = identity.User.AccountID;
    //            dtCandidateList = assignquestionnaire_BAO.GetdtAssignList(identity.User.UserID.ToString(), ddlProgramme.SelectedValue);
    //        }
    //        if (txtCandidateNo.Text.Trim() != "" || txtCandidateNo.Text.Trim() == "0")
    //        {
    //            assignquestionnaire_BE.CandidateNo = Convert.ToInt32(txtCandidateNo.Text.Trim());
    //        }

    //        string qId = PasswordGenerator.EnryptString(assignquestionnaire_BE.QuestionnaireID.ToString());
    //        string cId = PasswordGenerator.EnryptString(assignquestionnaire_BE.TargetPersonID.ToString());
    //        string path = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
    //        string feedbackurl = path + "Feedback.aspx?QID=" + qId + "&CID=" + cId;

    //        //assignquestionnaire_BE.FeedbackURL = feedbackurl;
    //        assignquestionnaire_BE.ModifiedBy = 1;
    //        assignquestionnaire_BE.ModifiedDate = DateTime.Now;
    //        assignquestionnaire_BE.IsActive = 1;

    //        assignquestionnaire_BE.AssignmentDetails = GetCandidateList();

    //        //if ((assignquestionnaire_BE.AssignmentDetails.Count == assignquestionnaire_BE.CandidateNo + 1 || assignquestionnaire_BE.AssignmentDetails.Count > 1) && assignquestionnaire_BE.CandidateNo != null)
    //        //lblcompanyname.Text=assignquestionnaire_BE.AssignmentDetails.Count.ToString();
    //        if(assignquestionnaire_BE.AssignmentDetails.Count > 1 && assignquestionnaire_BE.CandidateNo != null)
    //        {
    //            string accountID = ConfigurationManager.AppSettings["AccountID"].ToString();

    //            if (assignquestionnaire_BE.AccountID == Convert.ToInt32(accountID))
    //            {
    //                int managerCount = CheckManagerCount();

    //                if (managerCount > 1)
    //                {
    //                    lblvalidation.Text = "Please select only one relationship as Manager.";
    //                    return;
    //                }
    //                if (managerCount < 1)
    //                {
    //                    lblvalidation.Text = "Please select one relationship as Manager.";
    //                    return;
    //                }
    //            }

    //            //Save Assign questionnaire
    //            Int32 assignmentID = assignquestionnaire_BAO.AddAssignQuestionnaire(assignquestionnaire_BE);

    //            DataTable dtResult = new DataTable();
    //            dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireList(assignmentID);

    //            int loopCount = 0;
    //            char loopFlag = 'N';

    //            for (int k = 0; k < Convert.ToInt32(txtCandidateNo.Text.Trim()); k++)
    //            {
    //                if (dtResult.Rows[k]["RelationShip"].ToString() == "Self")
    //                    loopFlag = 'Y';
    //            }

    //            if (loopFlag == 'Y')
    //                loopCount = Convert.ToInt32(assignquestionnaire_BE.CandidateNo) + 1;
    //            else
    //                loopCount = Convert.ToInt32(assignquestionnaire_BE.CandidateNo);

    //            if (assignquestionnaire_BE.AccountID != Convert.ToInt32(accountID))
    //            {
    //                //Send mail to candidates
    //                string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

    //                for (int i = 0; i < loopCount; i++)
    //                {
    //                    AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
    //                    DataTable dtAccountAdmin = new DataTable();

    //                    dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.TargetPersonID));

    //                    Template = assignquestionnaire_BAO.FindTemplate(Convert.ToInt32(ddlProject.SelectedValue));
    //                    Subject = assignquestionnaire_BAO.FindCandidateSubjectTemplate(Convert.ToInt32(ddlProject.SelectedValue));

    //                    // Get Candidate Email Image Name & Will Combined with EmailImagePath
    //                    DataTable dtCandidateEmailImage = new DataTable();
    //                    string emailimagepath = "";
    //                    dtCandidateEmailImage = assignquestionnaire_BAO.GetCandidateEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));
    //                    if (dtCandidateEmailImage.Rows.Count > 0 && dtCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
    //                        emailimagepath = imagepath + dtCandidateEmailImage.Rows[0]["EmailImage"].ToString();

    //                    string questionnaireID = "";
    //                    string candidateID = "";
    //                    string OrganisationName = "";
    //                    string Startdate = "";
    //                    string Enddate = "";
    //                    string CandidateName = "";
    //                    string FirstName = "";
    //                    string candidateEmail = "";

    //                    candidateEmail = dtResult.Rows[i]["CandidateEmail"].ToString();
    //                    questionnaireID = dtResult.Rows[i]["QuestionnaireID"].ToString();
    //                    candidateID = dtResult.Rows[i]["AsgnDetailID"].ToString();
    //                    OrganisationName = dtResult.Rows[i]["OrganisationName"].ToString();
    //                    Startdate = Convert.ToDateTime(dtResult.Rows[0]["StartDate"]).ToString("dd-MMM-yyyy");
    //                    Enddate = Convert.ToDateTime(dtResult.Rows[0]["Enddate"]).ToString("dd-MMM-yyyy");
    //                    CandidateName = dtResult.Rows[i]["CandidateName"].ToString();
    //                    string[] strFName = CandidateName.Split(' ');
    //                    FirstName = strFName[0].ToString();

    //                    questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
    //                    candidateID = PasswordGenerator.EnryptString(candidateID);

    //                    string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

    //                    string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

    //                    //if (dtResult.Rows[i]["RelationShip"].ToString() == "Self")
    //                    if (dtResult.Rows[i]["RelationShip"].ToString() == "Self")
    //                    {
    //                        string feedbackURL = urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + PasswordGenerator.EnryptString(dtResult.Rows[i]["AsgnDetailID"].ToString());
    //                        assignquestionnaire_BAO.SetFeedbackURL(Convert.ToInt32(dtResult.Rows[i]["AsgnDetailID"].ToString()), Convert.ToInt32(dtResult.Rows[i]["AssignmentID"].ToString()), feedbackURL);
    //                    }

    //                    Template = Template.Replace("[LINK]", link);
    //                    Template = Template.Replace("[NAME]", CandidateName);
    //                    Template = Template.Replace("[FIRSTNAME]", FirstName);
    //                    Template = Template.Replace("[COMPANY]", OrganisationName);
    //                    Template = Template.Replace("[STARTDATE]", Startdate);
    //                    Template = Template.Replace("[CLOSEDATE]", Enddate);
    //                    Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");

    //                    Subject = Subject.Replace("[NAME]", CandidateName);
    //                    Subject = Subject.Replace("[FIRSTNAME]", FirstName);
    //                    Subject = Subject.Replace("[COMPANY]", OrganisationName);
    //                    Subject = Subject.Replace("[STARTDATE]", Startdate);
    //                    Subject = Subject.Replace("[CLOSEDATE]", Enddate);

    //                    if (dtResult.Rows[i]["RelationShip"].ToString() != "Self")
    //                    {
    //                        if (dtAccountAdmin.Rows.Count > 0)
    //                        {
    //                            Template = Template.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
    //                            Template = Template.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

    //                            Subject = Subject.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
    //                            Subject = Subject.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

    //                            //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
    //                            MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");
    //                            SendEmail.Send(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString(), maddr, emailimagepath);
    //                        }
    //                        else
    //                        {
    //                            Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
    //                            Template = Template.Replace("[PARTICIPANTEMAIL]", "");

    //                            Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
    //                            Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

    //                            SendEmail.Send(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString());
    //                        }
    //                    }
    //                }

    //                lblMessage.Text = "Emails have been sent successfully to your selected colleagues";
    //                lblMessage2.Text = "Emails have been sent successfully to your selected colleagues";
    //                imbAssign.Enabled = true;
    //            }
    //            else
    //            {
    //                //List<AssignmentDetails_BE> assignmentDetails_BEList=assignquestionnaire_BE.AssignmentDetails;

    //                //for (int i = 0; i < assignmentDetails_BEList.Count(); i++)
    //                //{
    //                //if (assignmentDetails_BEList[i].RelationShip == "Manager")
    //                //{
    //                //    AccountUser_BE managerDetails_BE = new AccountUser_BE();
    //                //    string accountCode = "";

    //                //    managerDetails_BE.FirstName = assignmentDetails_BEList[i].CandidateName;
    //                //    managerDetails_BE.LastName = "";
    //                //    managerDetails_BE.EmailID = assignmentDetails_BEList[i].CandidateEmail;
    //                //    managerDetails_BE.IsActive = 1;
    //                //    managerDetails_BE.LoginID = assignmentDetails_BEList[i].CandidateName;
    //                //    managerDetails_BE.Password = assignmentDetails_BEList[i].CandidateName;
    //                //    managerDetails_BE.Salutation = "Mr.";
    //                //    managerDetails_BE.GroupID = Convert.ToInt32(ConfigurationManager.AppSettings["ManagerRoleID"]);
    //                //    managerDetails_BE.Notification = true;
    //                //    managerDetails_BE.ModifyDate = DateTime.Now;
    //                //    managerDetails_BE.ModifyBy = 1;
    //                //    managerDetails_BE.StatusID = 1;

    //                //    identity = this.Page.User.Identity as WADIdentity;

    //                //    if (identity.User.GroupID == 1)
    //                //    {
    //                //        managerDetails_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
    //                //        accountCode = ddlAccountCode.SelectedItem.Text;
    //                //    }
    //                //    else
    //                //    {
    //                //        managerDetails_BE.AccountID = identity.User.AccountID;
    //                //        accountCode = identity.User.AccountCode; 
    //                //    }

    //                //    AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
    //                //    int managerId = accountUser_BAO.SaveManagerUser(managerDetails_BE);

    //                //    //Login Credential Email 
    //                //    string mailText = System.IO.File.ReadAllText(Server.MapPath("~") + "\\UploadDocs\\CreateUserTemplate.txt");

    //                //    List<AccountUser_BE> accountuser_BEList = new List<AccountUser_BE>();
    //                //    accountuser_BEList = accountUser_BAO.GetAccountUserByID(Convert.ToInt32(managerDetails_BE.AccountID), managerId);

    //                //    string EmailID = "";
    //                //    string FirstName = "";
    //                //    string Loginid = "";
    //                //    string password = "";

    //                //    EmailID = accountuser_BEList[0].EmailID;
    //                //    FirstName = accountuser_BEList[0].FirstName;
    //                //    Loginid = accountuser_BEList[0].LoginID; //managerDetails_BE.FirstName + managerId.ToString();
    //                //    password = accountuser_BEList[0].Password; //managerDetails_BE.FirstName + managerId.ToString();

    //                //    string loginPath = ConfigurationManager.AppSettings["ParticipantURL"].ToString();

    //                //    string link = "<a Target='_BLANK' href= '" + loginPath + "' >Click Link</a> ";

    //                //    mailText = mailText.Replace("[LINK]", link);
    //                //    mailText = mailText.Replace("[TITLE]", Title);
    //                //    mailText = mailText.Replace("[EMAILID]", EmailID);
    //                //    mailText = mailText.Replace("[FIRSTNAME]", FirstName);
    //                //    mailText = mailText.Replace("[LOGINID]", Loginid);
    //                //    mailText = mailText.Replace("[PASSWORD]", password);
    //                //    mailText = mailText.Replace("[CODE]", accountCode);

    //                //    SendEmail.Send("Login Credentials", mailText, EmailID);
    //                //}
    //                //}

    //                Project_BAO project_BAO = new Project_BAO();
    //                List<Project_BE> project_BEList = new List<Project_BE>();
    //                project_BEList = project_BAO.GetProjectByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.ProjecctID));

    //                int managerEmailId = Convert.ToInt32(project_BEList[0].EmailTMPManager);

    //                EmailTemplate_BAO emailTemplate_BAO = new EmailTemplate_BAO();
    //                List<EmailTemplate_BE> emailTemplate_BEList = new List<EmailTemplate_BE>();
    //                emailTemplate_BEList = emailTemplate_BAO.GetEmailTemplateByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), managerEmailId);

    //                string emailText = emailTemplate_BEList[0].EmailText;
    //                string emailSubject = emailTemplate_BEList[0].Subject;

    //                //string emailText = "Dear [MANAGERFIRSTNAME] <br> [PARTICIPANTNAME] has requested that the following employees participate in their 360 Feedback process.  If you are happy with their selection, please click the [APPROVE] icon at the bottom of this email.  If you would like any amendments, please click the [DECLINE] icon and discuss the choice with the Participant. They should then update their choices via the live site and you will receive a new authorisation email. <br><br> [LISTOFNAMES] <br> If you have any queries please contact Sara Manchanda. <br><br> Kind regards, ";
    //                //string emailSubject = "Request for Authorisation feedback 360 process";

    //                StringBuilder candidatelist = new StringBuilder();
    //                candidatelist.Append("<table width='500' border='1' cellspacing='0'>");

    //                candidatelist.Append("<tr><td width='50%'><b>Name</b></td><td width='50%'><b>Relationship</b></td></tr>");

    //                //List<AssignmentDetails_BE> assignmentDetails_BEList = new List<AssignmentDetails_BE>();
    //                //assignmentDetails_BEList = assignquestionnaire_BE.AssignmentDetails;

    //                DataTable dtColleagueList = new DataTable();
    //                dtColleagueList = assignquestionnaire_BAO.GetColleaguesList(assignmentID);

    //                string lineManagerName = "";
    //                string lineManagerEmail = "";
    //                string participantName = "";

    //                for (int i = 0; i < dtColleagueList.Rows.Count; i++)
    //                {
    //                    candidatelist.Append("<tr>");
    //                    candidatelist.Append("<td>");
    //                    candidatelist.Append(dtColleagueList.Rows[i]["CandidateName"].ToString());
    //                    candidatelist.Append("</td>");
    //                    candidatelist.Append("<td>");
    //                    candidatelist.Append(dtColleagueList.Rows[i]["RelationShip"].ToString());
    //                    candidatelist.Append("</td>");
    //                    candidatelist.Append("</tr>");

    //                    if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Manager")
    //                    {
    //                        lineManagerName = dtColleagueList.Rows[i]["CandidateName"].ToString();
    //                        lineManagerEmail = dtColleagueList.Rows[i]["CandidateEmail"].ToString();
    //                    }

    //                    if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Self")
    //                        participantName = dtColleagueList.Rows[i]["CandidateName"].ToString();
    //                }

    //                candidatelist.Append("</table>");

    //                string listOfNames = Convert.ToString(candidatelist);
    //                string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
    //                string asgnmentID = PasswordGenerator.EnryptString(Convert.ToString(assignmentID));
    //                int candidateNumber = Convert.ToInt32(assignquestionnaire_BE.CandidateNo);

    //                emailText = emailText.Replace("[MANAGERFIRSTNAME]", lineManagerName);
    //                emailText = emailText.Replace("[PARTICIPANTNAME]", participantName);
    //                emailText = emailText.Replace("[LISTOFNAMES]", listOfNames);
    //                emailText = emailText.Replace("[ACCEPT]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("1") + "' >Accept</a>");
    //                emailText = emailText.Replace("[DECLINE]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("0") + "' >Decline</a>");

    //                emailSubject = emailSubject.Replace("[PARTICIPANTNAME]", participantName);

    //                SendEmail.Send(emailSubject, emailText, lineManagerEmail);
    //                //SendEmail.Send(emailSubject, emailText, "sumneshl@damcogroup.com");

    //                lblMessage.Text = "Email has been sent successfully to Manager for further approval";
    //                lblMessage2.Text = "Email has been sent successfully to Manager for further approval";
    //                imbAssign.Enabled = true;
    //            }

    //            txtCandidateNo.Text = "";

    //            rptrCandidateList.DataSource = null;
    //            rptrCandidateList.DataBind();
    //        }
    //        else
    //        {
    //            lblvalidation.Text = "Please  fill colleagues' information";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        HandleException(ex);
    //    }
    //}



    //protected void imbReset_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        //HandleWriteLog("Start", new StackTrace(true));

    //        //ddlProject.SelectedIndex = 0;
    //        //ddlProgramme.SelectedIndex = 0;
    //        //ddlQuestionnaire.SelectedIndex = 0;
    //        //ddlTargetPerson.SelectedIndex = 0;
    //        //txtDescription.Text = "";

    //        txtCandidateNo.Text = "";
    //        lblMessage.Text = "";
    //        lblMessage2.Text = "";
    //        lblvalidation.Text = "";

    //        rptrCandidateList.DataSource = null;
    //        rptrCandidateList.DataBind();

    //        //HandleWriteLog("Start", new StackTrace(true));
    //    }
    //    catch (Exception ex)
    //    {
    //        HandleException(ex);
    //    }
    //}
#endif
    /// <summary>
    /// It is of No use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSaveColleague_Click(object sender, ImageClickEventArgs e)
    {

    }

    /// <summary>
    /// When click on add new button add three blank row to the last of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        //imbAssign.Enabled = true;
        lblMessage.Text = "";
        lblMessage2.Text = "";
        lblvalidation.Text = "";
        //if (txtCandidateNo.Text.Trim() != "")
        //{
        //int candidateCount = Convert.ToInt32(txtCandidateNo.Text.Trim());
        BindCandidateList(3);
        //}
    }

    /// <summary>
    /// Bind program by project selected value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Set Questionnaire
        //Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

        //ddlQuestionnaire.Items.Clear();
        //DataTable dtQuestionnaire = new DataTable();
        //dtQuestionnaire = questionnaire_BAO.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        //if (dtQuestionnaire.Rows.Count > 0)
        //{
        //    ddlQuestionnaire.DataSource = dtQuestionnaire;
        //    ddlQuestionnaire.DataTextField = "QSTNName";
        //    ddlQuestionnaire.DataValueField = "QuestionnaireID";
        //    ddlQuestionnaire.DataBind();
        //}

        //ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        //if (ddlQuestionnaire.Items.Count > 1)
        //    ddlQuestionnaire.Items[1].Selected = true;

        //Set Programme
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get program list in project
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
            //Bind program
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        //if (ddlProgramme.Items.Count > 1)
        //    ddlProgramme.Items[1].Selected = true;

        //Set Relationship
        Project_BAO projectBusinessAccessObject = new Project_BAO();
        DataTable dataTableRelationship = new DataTable();

        dataTableRelationship = projectBusinessAccessObject.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        Session["Relationship"] = dataTableRelationship;

        ddlTargetPerson.Items.Clear();
        ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
    }

    /// <summary>
    /// upload participant logo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgUpload_click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string constr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection connection = new SqlConnection(constr);
            lblvalidation.Text = "";
            //If it has file
            if (FileUpload1.HasFile)
            {
                if (this.IsFileValid(this.FileUpload1))
                {
                    string filename = "";
                    string file = "";
                    //Get file name.
                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);

                    //Get file unique name.
                    file = GetUniqueFilename(filename);

                    Session["FinalName"] = file;
                    //Set Path for file directory.
                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    //UPload file 
                    FileUpload1.SaveAs(filename);

                    AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

                    DataTable dataTableColleagueList;
                    if (Session["ColleagueTable"] == null)
                        dataTableColleagueList = assignQuestionnaireBusinessAccessObject.GetdtAssignColleagueList((User.Identity as WADIdentity).User.UserID.ToString(), "-1");
                    else
                        dataTableColleagueList = Session["ColleagueTable"] as DataTable;

                    DataTable dataTableProspective = new DataTable();

                   // ClearGridData();

                        dataTableProspective = ReturnExcelDataTableMot(filename);

                        if (dataTableProspective.Rows.Count > 0)
                        {
                            for (int i = 0; i < dataTableColleagueList.Rows.Count; i++)
                            {
                                int? accountID = (dataTableColleagueList.Rows[i].Field<int?>("AccountID"));
                                int? projectID = (dataTableColleagueList.Rows[i].Field<int?>("ProjectID"));
                                int? targetPersonID = (dataTableColleagueList.Rows[i].Field<int?>("TargetPersonID"));
                                int? assignID = (dataTableColleagueList.Rows[i].Field<int?>("AssignID"));
                                string Relationship = (dataTableColleagueList.Rows[i].Field<string>("Relationship"));
                                string name = (dataTableColleagueList.Rows[i].Field<string>("Name"));
                                string EmailID = (dataTableColleagueList.Rows[i].Field<string>("EmailID"));
                                int? assignmentID = (dataTableColleagueList.Rows[i].Field<int?>("AssignmentID"));
                                bool? submitFlag = dataTableColleagueList.Rows[i].Field<bool?>("SubmitFlag");
                                bool? emailSendFlag = dataTableColleagueList.Rows[i].Field<bool?>("EmailSendFlag");

                                if ((accountID == null) && projectID == null && (targetPersonID == null)
                                    && (projectID == null) && (assignID == null)
                                    && string.IsNullOrEmpty(Relationship) && string.IsNullOrEmpty(name)
                                    && string.IsNullOrEmpty(EmailID) && (assignmentID == null)
                                   && (submitFlag == null) && (emailSendFlag == null))
                                {
                                    dataTableColleagueList.Rows.Remove(dataTableColleagueList.Rows[i]);
                                }

                                i = i - 1;
                            }
                            dataTableColleagueList.AcceptChanges();
                        }

                    for (int i = 0; i < dataTableProspective.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTableColleagueList.NewRow();

                        dataRow["Relationship"] = dataTableProspective.Rows[i]["Relationship"];
                        dataRow["Name"] = dataTableProspective.Rows[i]["Name"];
                        dataRow["EmailID"] = dataTableProspective.Rows[i]["EmailID"];

                        dataTableColleagueList.Rows.Add(dataRow);
                        dataTableColleagueList.AcceptChanges();
                    }

                    BindColleagueRepeter(dataTableColleagueList);
                    lblMessage.Text = "";
                    lblMessage2.Text = "";

                    //Session["Relation"] = dtProspective;

                    //if (dtProspective != null && dtProspective.Rows.Count > 0)
                    //{


                    //    //TODO check candidate count logic 
                    //    // txtCandidateNo.Text = Convert.ToString(dtProspective.Rows.Count);

                    //    //foreach (RepeaterItem item in rptrCandidateList.Items)
                    //    //{
                    //    //    TextBox txtRelation = (TextBox)item.FindControl("ddlYearInsured");
                    //    //    TextBox txtCandidate = (TextBox)item.FindControl("ddlCompanyName");
                    //    //    TextBox txtCandEmail = (TextBox)item.FindControl("txtCompAddress");


                    //    //}

                    //    rptrCandidateList.DataSource = dtProspective;
                    //    rptrCandidateList.DataBind();

                    //    lblMessage.Text = "";
                    //    lblMessage2.Text = "";

                    //    //TODO Review below statment
                    //    //int candidateCount = Convert.ToInt32(txtCandidateNo.Text.Trim());

                    //    int candidateCount = dtProspective.Rows.Count;

                    //    for (int i = 0; i < candidateCount; i++)
                    //    {
                    //        email += dtProspective.Rows[i]["EmailID"].ToString() + ";";

                    //    }

                    //    finalemail = email.TrimEnd(';');

                    //}
                    //else
                    //{
                    //    errorMessage(filename);
                    //}
                    File.Delete(filename);
                }
                else
                {
                    lblMessage.Text = "Invalid file type";
                    lblMessage2.Text = "Invalid file type";
                    // Page.RegisterStartupScript("FileTyp", "<script language='JavaScript'>alert('Invalid file type');</script>");
                }
            }
            else
            {
                lblvalidation.Text = "Please browse file to upload";
            }

            //imbAssign.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Validate wheter uploaded image is valide in size or type.
    /// </summary>
    /// <param name="uploadControl"></param>
    /// <returns></returns>
    protected bool IsFileValid(FileUpload uploadControl)
    {
        bool isFileOk = true;
        try
        {
            string FileExt = ".xls,.xlsx,.XLS,.XLSX";
            string[] AllowedExtensions = FileExt.Split(',');

            bool isExtensionError = false;

            int MaxSizeAllowed = 5 * 1048576;// Size Allow only in mb
            if (uploadControl.PostedFile != null)
            {
                bool isSizeError = false;
                // Validate for size less than MaxSizeAllowed...
                if (uploadControl.PostedFile.ContentLength > MaxSizeAllowed)
                {
                    isFileOk = false;
                    isSizeError = true;
                }
                else
                {
                    isFileOk = true;
                    isSizeError = false;
                }

                // If OK so far, validate the file extension...
                if (isFileOk)
                {
                    isFileOk = false;
                    isExtensionError = true;

                    // Get the file's extension...
                    string fileExtension = System.IO.Path.GetExtension(uploadControl.PostedFile.FileName).ToLower();

                    for (int i = 0; i < AllowedExtensions.Length; i++)
                    {
                        if (fileExtension.Trim() == AllowedExtensions[i].Trim())
                        {
                            isFileOk = true;
                            isExtensionError = false;

                            break;
                        }
                    }
                }

                if (isExtensionError)
                {
                    string errorMessage = "Invalid file type";

                }
                if (isSizeError)
                {
                    string errorMessage = "Maximum Size of the File exceeded";

                }
            }
            return isFileOk;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// It is of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkError_Click(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Session["FinalName"].ToString() + ".txt");
        Response.WriteFile(Server.MapPath("~") + "//UploadDocs//" + Session["FinalName"].ToString() + ".txt");
        Response.End();
    }

    /// <summary>
    /// Bind project according to account value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {

            //int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            //Account_BAO account_BAO = new Account_BAO();

            //CompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            //expression1 = "AccountID='" + companycode + "'";

            //Finalexpression = expression1;

            //DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            //DataTable dtAccount = CompanyName.Clone();

            //foreach (DataRow drAccount in resultsAccount)
            //{
            //    dtAccount.ImportRow(drAccount);
            //}

            //lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
            //Get comapny name.
            GetCompanyName();

            //ddlProject.Items.Clear();
            // ddlProject.Items.Insert(0, new ListItem("Select", "0"));


            Project_BAO projectBusinessAccessObject = new Project_BAO();
            //ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataBind();
            //Bind project dropdown.
            BindDropDownList(ddlProject, projectBusinessAccessObject.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue)),
                ProjectTextField, ProjectValueField);


            // ddlProgramme.Items.Clear();
            //  ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
            ClearControl(ddlProgramme);


            //  ddlTargetPerson.Items.Clear();
            //  ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
            ClearControl(ddlTargetPerson);
        }
        else
        {
            lblcompanyname.Text = string.Empty;

            //ddlProject.Items.Clear();
            //ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            //ddlProgramme.Items.Clear();
            //ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            //ddlTargetPerson.Items.Clear();
            //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
            ClearControl(ddlProject);
            ClearControl(ddlProgramme);
            ClearControl(ddlTargetPerson);
        }
    }

    /// <summary>
    /// bind gridview according to participant value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTargetPerson.SelectedIndex > 0)
        {

            Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
            strColleagueNo = programmeBusinessAccessObject.GetProgramColleagueNumber(Convert.ToInt32(ddlProgramme.SelectedValue));

            AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

            DataTable dataTableAssignColleagueList = assignQuestionnaireBusinessAccessObject.GetdtAssignColleagueList(ddlTargetPerson.SelectedValue, ddlProgramme.SelectedValue);

            int colleagueCount = dataTableAssignColleagueList.Rows.Count;

            string stringColleagueIndex = string.Empty;
            //get colleague index
            for (int i = 0; i < colleagueCount; i++)
            {
                stringColleagueIndex += i.ToString() + ",";
            }

            if (Session["ColleaguesIndex"] == null)
            {//store it in session to check colleague position in grid.
                Session["ColleaguesIndex"] = stringColleagueIndex;
            }

            if (!string.IsNullOrEmpty(strColleagueNo))
            {
                int colleagueNo = Convert.ToInt32(strColleagueNo);
                if (dataTableAssignColleagueList != null)
                {
                    if (dataTableAssignColleagueList.Rows.Count < colleagueNo)
                    {
                        int maxNo = (colleagueNo - dataTableAssignColleagueList.Rows.Count);
                        for (int i = 0; i < maxNo; i++)
                        {
                            DataRow row = dataTableAssignColleagueList.NewRow();
                            dataTableAssignColleagueList.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    dataTableAssignColleagueList = new DataTable();
                    dataTableAssignColleagueList.Columns.Add("Relationship");
                    dataTableAssignColleagueList.Columns.Add("Name");
                    dataTableAssignColleagueList.Columns.Add("EmailID");

                    for (int i = 0; i < colleagueNo; i++)
                        dataTableAssignColleagueList.Rows.Add("", "", "");

                }

                BindColleagueRepeter(dataTableAssignColleagueList);


                DataTable dataTableResult = new DataTable();
                dataTableResult = assignQuestionnaireBusinessAccessObject.GetFeedbackURL(Convert.ToInt32(ddlTargetPerson.SelectedValue));

                if (dataTableResult.Rows.Count > 0)
                {
                    string url = dataTableResult.Rows[0]["FeedbackUrl"].ToString();

                    if (colleagueCount > 0)
                        imbSelfAssessment.Enabled = true;
                    else
                        imbSelfAssessment.Enabled = false;
                }
                else
                    imbSelfAssessment.Enabled = false;
            }
        }
        //AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();

        //if (ddlTargetPerson.SelectedIndex > 0)
        //{
        //    DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(ddlTargetPerson.SelectedValue));
        //    Project_BAO project_BAO = new Project_BAO();

        //    if (dtuserlist.Rows.Count > 0)
        //    {
        //        int projectid = Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]);

        //        ddlProject.Items.Clear();
        //        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        //        DataTable project = project_BAO.GetdataProjectByID(projectid);
        //        ddlProject.DataSource = project;
        //        ddlProject.DataTextField = "Title";
        //        ddlProject.DataValueField = "ProjectID";
        //        ddlProject.DataBind();
        //    }
        //    else
        //    {
        //        ddlProject.Items.Clear();
        //        ddlProject.Items.Insert(0, new ListItem("Select", "0"));
        //    }
        //}
    }

    /// <summary>
    /// Add validation group to grid contrls, bind the  relation ship drop down in grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrCandidateList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem candidateListItem = e.Item;
        DropDownList ddlRelationship = (DropDownList)candidateListItem.FindControl("ddlRelationship");
        TextBox textBoxName = (TextBox)candidateListItem.FindControl("txtName");
        TextBox textBoxEmailID = (TextBox)candidateListItem.FindControl("txtEmailID");
        RequiredFieldValidator RequiredFieldRelationShip = (RequiredFieldValidator)candidateListItem.FindControl("rqfRelationShip");
        RequiredFieldValidator RequiredFieldTextBoxName = (RequiredFieldValidator)candidateListItem.FindControl("rfqTxtName");
        RequiredFieldValidator RequiredFieldTextBoxEmailID = (RequiredFieldValidator)candidateListItem.FindControl("rfqTxtEmailID");
        ImageButton imageButtonSaveColleague = (ImageButton)candidateListItem.FindControl("imbSaveColleague");
        ImageButton imageButtonSaveOnlyColleague = (ImageButton)candidateListItem.FindControl("imbSaveOnlyColleague");

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //Add validation group to controls.
            ddlRelationship.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            textBoxName.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            textBoxEmailID.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            RequiredFieldRelationShip.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            RequiredFieldTextBoxName.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            RequiredFieldTextBoxEmailID.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            imageButtonSaveColleague.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            imageButtonSaveOnlyColleague.ValidationGroup = "VGroup" + e.Item.ItemIndex;
        }
        if (ddlRelationship != null)
        {
            DataTable dataTableRelationShip = new DataTable();
            dataTableRelationShip = (DataTable)Session["Relationship"];

            if (dataTableRelationShip == null || dataTableRelationShip.Rows.Count < 1)
            {
                Project_BAO projectBusinessAccessObject = new Project_BAO();
                dataTableRelationShip = projectBusinessAccessObject.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
                Session["Relationship"] = dataTableRelationShip;
            }

            ddlRelationship.Items.Clear();
            ddlRelationship.Items.Insert(0, new ListItem("Select", "0"));
            //Bind the relation ship drop down list.
            ddlRelationship.DataSource = dataTableRelationShip;
            ddlRelationship.DataValueField = "value";
            ddlRelationship.DataTextField = "value";
            ddlRelationship.DataBind();

            HiddenField hddRelationship = (HiddenField)candidateListItem.FindControl("hddRelationShip");

            if (!string.IsNullOrEmpty(hddRelationship.Value))
                ddlRelationship.SelectedValue = hddRelationship.Value;
        }

        if (ddlRelationship != null && Session["Relation"] != null)
        {
            DataTable dataTableRelation = new DataTable();
            dataTableRelation = (DataTable)Session["Relation"];

            if (e.Item.ItemIndex < dataTableRelation.Rows.Count)
                ddlRelationship.SelectedValue = Convert.ToString(dataTableRelation.Rows[e.Item.ItemIndex]["Relationship"]);

        }

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //TextBox tb = rpItem.FindControl("txtEmailID") as TextBox;
            Label labelTargetPersonID = (Label)e.Item.FindControl("lblTargetPersonID");
            //Label lblAssignmentDetailsID = (Label)e.Item.FindControl("lblAssignmentID");

            if (string.IsNullOrEmpty(labelTargetPersonID.Text))
            {
                (candidateListItem.FindControl("imgColleagueSaved") as System.Web.UI.WebControls.Image).Visible = false;
                (candidateListItem.FindControl("imbSaveColleague") as ImageButton).Visible = true;
            }
            else
            {
                (candidateListItem.FindControl("imgColleagueSaved") as System.Web.UI.WebControls.Image).Visible = true;
                (candidateListItem.FindControl("imbSaveColleague") as ImageButton).Visible = false;
            }

            int userID = Convert.ToInt32((User.Identity as WADIdentity).User.UserID);
            if (userID == 3 && !string.IsNullOrEmpty(labelTargetPersonID.Text))
            {
                (candidateListItem.FindControl("imbSaveOnlyColleague") as ImageButton).Visible = true;
            }
            else
            {
                (candidateListItem.FindControl("imbSaveOnlyColleague") as ImageButton).Visible = false;
            }

            //Label lbl = rpItem.FindControl("lblSubmitStatus") as Label;

            //if (lbl.Text.ToLower() == "true")
            //    lbl.Text = "Yes";
            //else
            //    lbl.Text = "No";

            DataTable dataTableQuestion = new DataTable();
            Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

            int QuestionnaireID = questionnaireBusinessAccessObject.GetQuestionnaireID(ddlProject.SelectedValue);

            Label lblAssignmentID = candidateListItem.FindControl("lblAssignmentID") as Label;
            Label lblCompletion = candidateListItem.FindControl("lblCompletion") as Label;

            if (!string.IsNullOrEmpty(lblAssignmentID.Text))
            {

                int answeredQuestion = questionnaireBusinessAccessObject.CalculateGraph(QuestionnaireID, Convert.ToInt32(lblAssignmentID.Text));
                double percentage = 0.0;
                //dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaire(Convert.ToInt32(lblQuestionnaireId.Text));
                dataTableQuestion = questionnaireBusinessAccessObject.GetFeedbackQuestionnaireByRelationShip(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue), QuestionnaireID, ddlRelationship.SelectedValue);

                if (dataTableQuestion.Rows.Count > 0)
                {//calculate % of survey comlteted.
                    percentage = (answeredQuestion * 100) / Convert.ToInt32(dataTableQuestion.Rows.Count);
                }

                string[] percent = percentage.ToString().Split('.');


                //percentage = percent[0];
                if (percent[0].ToString() == "100") //if it is 100% then completion status is yes esle no.
                    lblCompletion.Text = "Yes";
                else
                    lblCompletion.Text = percent[0].ToString() + "%";
            }
            else
                lblCompletion.Text = "0%";

            ImageButton imageDeleteColleague = candidateListItem.FindControl("imbDeleteColleague") as ImageButton;
            //Add client side event to delete button.
            if (imageDeleteColleague != null)
            {
                TextBox txtBox = candidateListItem.FindControl("txtName") as TextBox;

                imageDeleteColleague.Attributes.Add("onclick", "javascript:return DeleteConfirmation('" + txtBox.ClientID + "');");
            }
        }
    }

    /// <summary>
    /// Open participant window to view participant details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbView_Click(object sender, ImageClickEventArgs e)
    {
        string userid = "";

        identity = this.Page.User.Identity as WADIdentity;

        if (ddlTargetPerson.Visible == false)
        {
            userid = Convert.ToString(identity.User.UserID);
        }
        else
        {
            userid = ddlTargetPerson.SelectedValue;
        }
        //Validate view event.
        if (ddlProject.SelectedValue == "0" || (ddlTargetPerson.Visible == true && ddlTargetPerson.SelectedValue == "0"))
        {

            lblMessage.Text = "Select Project & Target Person";
            lblMessage2.Text = "Select Project & Target Person";
        }
        else
        { //Open participant window to view details.
            ScriptManager.RegisterStartupScript(this, typeof(string), "print", "javascript:window.open('../../Module/Feedback/AssignQuestionnaireList.aspx?Projectid=" + ddlProgramme.SelectedValue + "&userid=" + userid + "', 'CustomPopUp', " + "'width=1000, height=450, menubar=no, resizable=yes');", true);
            lblMessage.Text = "";
            lblMessage2.Text = "";
        }

        if (ddlTargetPerson.Visible == false)
        {
            if (ddlProject.SelectedValue == "0")
            {
                lblMessage.Text = "";
                lblMessage.Text = "Select Project ";
                lblMessage2.Text = "Select Project ";
            }
            else
            {
                //Open participant window to view details.
                ScriptManager.RegisterStartupScript(this, typeof(string), "print", "javascript:window.open('../../Module/Feedback/AssignQuestionnaireList.aspx?Projectid=" + ddlProgramme.SelectedValue + "&userid=" + userid + "', 'CustomPopUp', " + "'width=200, height=550, menubar=no, resizable=yes');", true);
                lblMessage.Text = "";
                lblMessage2.Text = "";
            }
        }
    }

    protected void rptrCandidateList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        lblMessage2.Text = lblMessage.Text = "";

        try
        {
            if (e.CommandName.ToLower() == "assign")
            {
                if (Page.IsValid)
                {
                    Page.Validate("VGroupX");

                    if (!Page.IsValid)
                    {
                        return;
                    }
                }

                DropDownList dropDownRelationship;
                TextBox textBoxCandidateName;
                TextBox textBoxCandidateEmail;

                int i = e.Item.ItemIndex;

                if (Session["ColleaguesIndex"] != null)
                    Session["ColleaguesIndex"] = Session["ColleaguesIndex"].ToString().TrimStart(',') + i.ToString() + ",";
                else
                    Session["ColleaguesIndex"] = i.ToString() + ",";

                if (i < rptrCandidateList.Items.Count)
                {
                    DataTable dataTableCandidateClone;

                    DataTable dataTableCandidate = Session["ColleagueTable"] as DataTable;
                    dataTableCandidateClone = dataTableCandidate.Clone();

                    dataTableCandidateClone.Clear();

                    colleagueRecordCount = rptrCandidateList.Items.Count;

                    for (int j = 0; j < colleagueRecordCount; j++)
                    {
                        if (j != i)
                        {
                            dropDownRelationship = (DropDownList)rptrCandidateList.Items[j].FindControl("ddlRelationship");
                            textBoxCandidateName = (TextBox)rptrCandidateList.Items[j].FindControl("txtName");
                            textBoxCandidateEmail = (TextBox)rptrCandidateList.Items[j].FindControl("txtEmailID");
                            Label lblTargetPersonID = (Label)rptrCandidateList.Items[j].FindControl("lblTargetPersonID");

                            if (string.IsNullOrEmpty(lblTargetPersonID.Text))
                            {
                                DataRow dr = dataTableCandidateClone.NewRow();
                                dr["Relationship"] = dropDownRelationship.SelectedItem.Text;
                                dr["Name"] = textBoxCandidateName.Text;
                                dr["EmailID"] = textBoxCandidateEmail.Text;
                                dataTableCandidateClone.Rows.Add(dr);
                            }
                        }
                    }

                    dataTableCandidateClone.AcceptChanges();
                    Session["UnsavedColleagueTable"] = dataTableCandidateClone;
                }

                dropDownRelationship = (DropDownList)e.Item.FindControl("ddlRelationship");
                textBoxCandidateName = (TextBox)e.Item.FindControl("txtName");
                textBoxCandidateEmail = (TextBox)e.Item.FindControl("txtEmailID");

                SaveCandidate(dropDownRelationship.SelectedValue, dropDownRelationship.SelectedItem.Text, textBoxCandidateName.Text, textBoxCandidateEmail.Text);

                WADIdentity userIdentity = this.Page.User.Identity as WADIdentity;
                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                if (userIdentity.User.GroupID.ToString() != participantRoleId)
                    ddlTargetPerson_SelectedIndexChanged(this, EventArgs.Empty);
                else
                    ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);

            }
            else if (e.CommandName.ToLower() == "delete")//when click on delete button then delter records.
            {

                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    AssignQuestionnaire_BE assignQuestionnaireBusinessEntity = new AssignQuestionnaire_BE();
                    AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

                    assignQuestionnaireBusinessEntity.AssignmentID = Convert.ToInt32(e.CommandArgument);
                    //Delete Questionnaire
                    assignQuestionnaireBusinessAccessObject.DeleteAssignQuestionnaire(assignQuestionnaireBusinessEntity);

                    colleagueRecordCount = rptrCandidateList.Items.Count;
                    string[] stringColleaguesIndex = Session["ColleaguesIndex"].ToString().TrimEnd(',').Split(',');

                    if (stringColleaguesIndex.Length > 0)
                    {
                        string stringNewColleaguesIndex = string.Empty;
                        for (int i = 0; i < stringColleaguesIndex.Length; i++)
                        {
                            if (stringColleaguesIndex[i].ToString() != e.Item.ItemIndex.ToString())
                            {
                                if (!string.IsNullOrEmpty(stringColleaguesIndex[i]))
                                {
                                    if (e.Item.ItemIndex < Convert.ToInt32(stringColleaguesIndex[i]))
                                        stringNewColleaguesIndex += (Convert.ToInt32(stringColleaguesIndex[i]) - 1).ToString() + ",";
                                    else
                                        stringNewColleaguesIndex += stringColleaguesIndex[i] + ",";
                                }
                            }
                        }
                        Session["ColleaguesIndex"] = stringNewColleaguesIndex;
                    }

                    ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
            else if (e.CommandName.ToLower() == "save" || e.CommandName.ToLower() == "sendemail")//when click on save image amd resend image in grid then save and resend mail.
            {
                if (Page.IsValid)
                {
                    Page.Validate("VGroupX");

                    if (!Page.IsValid)
                    {
                        return;
                    }
                }
                DropDownList dropDownListRelationship = (DropDownList)e.Item.FindControl("ddlRelationship");
                Label labelAssignmentID = (Label)e.Item.FindControl("lblAssignID");
                Label labelAccountID = (Label)e.Item.FindControl("lblAccountID");
                Label labelTargetPersonID = (Label)e.Item.FindControl("lblTargetPersonID");
                Label labelAssignmentDetailsID = (Label)e.Item.FindControl("lblAssignmentID");
                Label labelProjectID = (Label)e.Item.FindControl("lblProjectID");
                TextBox textBoxCandidateName = (TextBox)e.Item.FindControl("txtName");
                TextBox textBoxCandidateEmail = (TextBox)e.Item.FindControl("txtEmailID");
                string relationship = dropDownListRelationship.SelectedItem.Text;

                if (!string.IsNullOrEmpty(labelAssignmentID.Text) && !string.IsNullOrEmpty(labelAccountID.Text) && !string.IsNullOrEmpty(labelTargetPersonID.Text)
                    && !string.IsNullOrEmpty(labelAssignmentDetailsID.Text) && !string.IsNullOrEmpty(labelProjectID.Text))
                {
                    int assignmentID = Convert.ToInt32(labelAssignmentID.Text);
                    int accountID = Convert.ToInt32(labelAccountID.Text);
                    int targetPersonID = Convert.ToInt32(labelTargetPersonID.Text);
                    int assignmentDetailsID = Convert.ToInt32(labelAssignmentDetailsID.Text);
                    int projectID = Convert.ToInt32(labelProjectID.Text);

                    if (e.CommandName.ToLower() == "save")//save and resend email.
                        ReSendColleagueEmail(assignmentID, accountID, targetPersonID, assignmentDetailsID, projectID, textBoxCandidateName.Text, textBoxCandidateEmail.Text, relationship, false);
                    else if (e.CommandName.ToLower() == "sendemail")//Resend mail only.
                        ReSendColleagueEmail(assignmentID, accountID, targetPersonID, assignmentDetailsID, projectID, textBoxCandidateName.Text, textBoxCandidateEmail.Text, relationship, true);
                }
            }

        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind gird accorgind to number of colleague in participant.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participantBusinessAccessObject = new AssignQstnParticipant_BAO();
         identity = this.Page.User.Identity as WADIdentity;
        // string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
        DataTable dataTableParticipant = null;

        if (ddlProgramme.SelectedIndex > 0)
        {
            Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
            //Get report available date to and from.
            DateTime[] dataTableReport = programmeBusinessAccessObject.GetProgramReportDate(Convert.ToInt32(ddlProgramme.SelectedValue));

            if (dataTableReport != null && dataTableReport.Count() == 2)
            {
                lblReportMSG.InnerText = "Report will be available from " + dataTableReport[0].ToString("dd/MM/yyyy");
                lblReportMSG.Visible = true;

                if (dataTableReport[0] <= DateTime.Now)
                    imbReportDownload.Visible = true;
                else
                    imbReportDownload.Visible = false;
            }
            else
            {
                lblReportMSG.Visible = false;
                imbReportDownload.Visible = false;
            }


            //Show instruction only for the participant 
            if (identity.User.GroupID.ToString() == ConfigurationManager.AppSettings["ParticipantRoleID"].ToString())
            {
                string strInstructions = programmeBusinessAccessObject.GetProgramInstructions(Convert.ToInt32(ddlProgramme.SelectedValue));
                lblInstruction.Text = strInstructions;
            }

            strColleagueNo = programmeBusinessAccessObject.GetProgramColleagueNumber(Convert.ToInt32(ddlProgramme.SelectedValue));

            AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

            DataTable dataTableColleagueList = assignQuestionnaireBusinessAccessObject.GetdtAssignColleagueList(identity.User.UserID.ToString(), ddlProgramme.SelectedValue);

            int colleagueCount = dataTableColleagueList.Rows.Count;

            string strColleagueIndex = string.Empty;

            for (int i = 0; i < colleagueCount; i++)
            {
                strColleagueIndex += i.ToString() + ",";
            }

            if (Session["ColleaguesIndex"] == null)
            {
                Session["ColleaguesIndex"] = strColleagueIndex;
            }

            if (!string.IsNullOrEmpty(strColleagueNo))
            {
                int colleagueNo = Convert.ToInt32(strColleagueNo);
                if (dataTableColleagueList != null)
                {
                    if (dataTableColleagueList.Rows.Count < colleagueNo)
                    {
                        int maxNo = (colleagueNo - dataTableColleagueList.Rows.Count);
                        for (int i = 0; i < maxNo; i++)
                        {
                            DataRow row = dataTableColleagueList.NewRow();
                            dataTableColleagueList.Rows.Add(row);
                        }

                        //if (ddlProgramme.Enabled)
                        //{
                        //    for (int i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        int? accountID = (dt.Rows[i].Field<int?>("AccountID"));
                        //        int? projectID = (dt.Rows[i].Field<int?>("ProjectID"));
                        //        int? targetPersonID = (dt.Rows[i].Field<int?>("TargetPersonID"));
                        //        int? assignID = (dt.Rows[i].Field<int?>("AssignID"));
                        //        string Relationship = (dt.Rows[i].Field<string>("Relationship"));
                        //        string name = (dt.Rows[i].Field<string>("Name"));
                        //        string EmailID = (dt.Rows[i].Field<string>("EmailID"));
                        //        int? assignmentID = (dt.Rows[i].Field<int?>("AssignmentID"));
                        //        bool? submitFlag = dt.Rows[i].Field<bool?>("SubmitFlag");
                        //        bool? emailSendFlag = dt.Rows[i].Field<bool?>("EmailSendFlag");

                        //        if ((accountID == null) && projectID == null && (targetPersonID == null)
                        //            && (projectID == null) && (assignID == null)
                        //            && string.IsNullOrEmpty(Relationship) && string.IsNullOrEmpty(name)
                        //            && string.IsNullOrEmpty(EmailID) && (assignmentID == null)
                        //           && (submitFlag == null) && (emailSendFlag == null))
                        //        {
                        //            dt.Rows.Remove(dt.Rows[i]);
                        //        }
                        //        i = i - 1;
                        //    }
                        //}
                        dataTableColleagueList.AcceptChanges();
                    }
                }
                else
                {
                    dataTableColleagueList = new DataTable();
                    dataTableColleagueList.Columns.Add("Relationship");
                    dataTableColleagueList.Columns.Add("Name");
                    dataTableColleagueList.Columns.Add("EmailID");

                    for (int i = 0; i < colleagueNo; i++)
                        dataTableColleagueList.Rows.Add("", "", "");

                }

                BindColleagueRepeter(dataTableColleagueList);

                //Enable Disable self assessment button
                //AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                DataTable dataTableResult = new DataTable();
                dataTableResult = assignQuestionnaireBusinessAccessObject.GetFeedbackURL(Convert.ToInt32(identity.User.UserID));

                if (dataTableResult.Rows.Count > 0)
                {
                    string url = dataTableResult.Rows[0]["FeedbackUrl"].ToString();

                    if (colleagueCount > 0)
                        imbSelfAssessment.Enabled = true;
                    else
                        imbSelfAssessment.Enabled = false;
                }
                else
                    imbSelfAssessment.Enabled = false;

                //BindCandidateList(Convert.ToInt32(strColleagueNo));
            }

            if (identity.User.GroupID == 1)
            {
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(Convert.ToString(ddlAccountCode.SelectedValue), ddlProgramme.SelectedValue);
            }
            else
            {
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(Convert.ToString(identity.User.AccountID), ddlProgramme.SelectedValue);
            }

            Project_BAO projectBusinessAccessObject = new Project_BAO();

            if (dataTableParticipant.Rows.Count > 0)
            {
                ddlTargetPerson.Items.Clear();

                ddlTargetPerson.DataSource = dataTableParticipant;
                ddlTargetPerson.DataTextField = "UserName";
                ddlTargetPerson.DataValueField = "UserID";
                ddlTargetPerson.DataBind();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlTargetPerson.Items.Clear();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }

    /// <summary>
    /// Download report pdf
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReportDownload_Click(object sender, ImageClickEventArgs e)
    {
        AssignQstnParticipant_BAO assignQstnParticipantBusinessAccessObject = new AssignQstnParticipant_BAO();


        //string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32((User.Identity as WADIdentity).User.UserID)).ToString();
        int userID = Convert.ToInt32(Convert.ToInt32(ddlTargetPerson.SelectedValue));
        //string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32((User.Identity as WADIdentity).User.UserID)).ToString();
        //Get report file name
        string reportFileName = assignQstnParticipantBusinessAccessObject.GetReportFileName(userID).ToString();

        //IF file name exists
        if (reportFileName != "")
        {
            //get folder path.
            string root = Server.MapPath("~") + "\\ReportGenerate\\";
            //Combine to create full path
            string openpdf = root + reportFileName;
            //If not file exists
            if (!File.Exists(openpdf))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ReportMSG", "alert('File not found');", true);
                return;
            }
            //Download file
            Byte[] bytes = File.ReadAllBytes(openpdf);
            Response.Buffer = true;
            Response.Charset = "";
            //Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment;filename=" + reportFileName);
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.Clear();
            Response.Close();
            Response.End();

        }
        else
        {
            //string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32(e.CommandArgument)).ToString();

            string strReportName = string.Empty;
            AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
            identity = this.Page.User.Identity as WADIdentity;
            //Get self assement details
            DataTable dataTableSelfName = accountUserBusinessAccessObject.GetdtAccountUserByID(identity.User.AccountID.Value, identity.User.UserID.Value);
            if (dataTableSelfName != null && dataTableSelfName.Rows.Count > 0)
            {
                strReportName = dataTableSelfName.Rows[0]["FirstName"].ToString() + dataTableSelfName.Rows[0]["LastName"].ToString() + '_' + DateTime.Now.ToString("ddMMyyHHmmss");
                //ViewState["strReportName"] = dtSelfName.Rows[0]["FirstName"].ToString() + dtSelfName.Rows[0]["LastName"].ToString() + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss");
            }

            string strTargetPersonID = string.Empty;
            string strAccountID = string.Empty;
            string strProjectID = string.Empty;
            string strProgrammeID = string.Empty;
            //string participantid = Convert.ToString(e.CommandArgument);
            GetDetailFromTargetPersonID(identity.User.UserID.Value.ToString(), out strTargetPersonID, out strProjectID, out strAccountID, out strProgrammeID);

            //if (Convert.ToString(ViewState["strTargetPersonID"]) != string.Empty)
            //    strTargetPersonID = Convert.ToString(ViewState["strTargetPersonID"]);
            //if (Convert.ToString(ViewState["strAccountID"]) != string.Empty)
            //    strAccountID = Convert.ToString(ViewState["strAccountID"]);
            //if (Convert.ToString(ViewState["strProjectID"]) != string.Empty)
            //    strProjectID = Convert.ToString(ViewState["strProjectID"]);
            ////if (Convert.ToString(ViewState["strReportName"]) != string.Empty)
            ////    strReportName = Convert.ToString(ViewState["strReportName"]);
            //if (Convert.ToString(ViewState["strProgrammeID"]) != string.Empty)
            //    strProgrammeID = Convert.ToString(ViewState["strProgrammeID"]);

            if (strTargetPersonID != null && strAccountID != null && strProjectID != null && strReportName != null)
            {
                GenerateReport("", strProjectID, strTargetPersonID, strReportName);
                try
                {
                    string root = Server.MapPath("~") + "\\ReportGenerate\\";
                    string openpdf = root + strReportName + ".pdf";
                    //Response.Write(openpdf);
                    //Response.Redirect("../../ReportGenerate/" + strReportName + ".pdf");

                    /*Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Clear();                    
                    
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + strReportName.Substring(0, strReportName.Length - 6) + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.TransmitFile(openpdf);
                    
                    Response.Flush();
                    Response.Close();*/
                    Response.Redirect("~/Module/Reports/download.aspx?filename=" + strReportName);


                }
                catch (Exception ex)
                { }
            }


            //ScriptManager.RegisterStartupScript(this, GetType(), "ReportMSG", "alert('Report is not available.');", true);
            return;
        }
    }

    /// <summary>
    /// Generate Pdf report for grid data
    /// </summary>
    /// <param name="dirName"></param>
    /// <param name="strProjectID"></param>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strReportName"></param>
    protected void GenerateReport(string dirName, string strProjectID, string strTargetPersonID, string strReportName)
    {
        ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
        try
        {
            string strStaticBarLabelVisibility = string.Empty;

            // Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();
            rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());

            string mimeType;
            string encoding;
            string extension;
            string[] streamids;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string root = string.Empty;
            root = Server.MapPath("~") + "\\ReportGenerate\\";

            /* Function : For Filling Paramters From Controls */
            ControlToParameter(strProjectID);

            if (ddlAccountCode.SelectedValue != string.Empty)
                strStaticBarLabelVisibility = ddlAccountCode.SelectedItem.ToString();
            else
                strStaticBarLabelVisibility = "";

            //If strReportType = 1 Then FeedbackReport will Call
            //If strReportType = 2 Then FeedbackReportClient1 will Call (In this Report We are Showing only Range & Text Type Question).
            if (strReportType == "1")
            {

                DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
                {
                    /*
                     * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                     * & Making Entry in Table with Radarchatname
                     * & Calling in RDL (RadarImage)
                     */
                    if (dataTableReportsetting.Rows[0]["RadarChart"].ToString() == "1")
                        Radar(strTargetPersonID, strGroupList);
                    else
                        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                        RadarPreviousScore(strTargetPersonID, strGroupList);
                    else
                        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //BenchMark Radar Chart.
                    if (dataTableReportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() == "1")
                        RadarBenchMark(strTargetPersonID);
                    else
                        targetradarBenchmark = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";
                }

                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();

                //rview.ServerReport.ReportPath = "/"+strReportPathPrefix +"/FeedbackReport";

                if (identity.User.AccountID == 68 || ddlAccountCode.SelectedValue == "68")
                {
                    rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/CapitaFeedbackReport";
                }
                else
                {
                    rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReport";
                }

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("RadarChartVisibility", strRadarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", strGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarName", targetradarname));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchMarkGrpVisibility", strBenchMarkGrpVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNameBenchmark", targetradarBenchmark));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchMarkVisibility", strBenchMarkVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BenchConclusionVisibility", strBenchConclusionPageVisibility));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "2")
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient1";
                //rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();

                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportClient1";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));            
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", strGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", "1"));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));            
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "3")
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient2";
                //  rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                //New Changes 
                //Changed by Amit Singh
                DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
                {
                    // if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                    RadarCPL(strTargetPersonID, strGroupList);
                    // else
                    //  targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                        RadarPreviousScoreCPL(strTargetPersonID, strGroupList);
                    else
                        targetradarPreviousScore = "RadarChartNoImage";

                }
                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();

                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportClient2";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                //paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarName", targetradarname));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetRadarNamePrevious", targetradarPreviousScore));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "4") // Old Mutual Report
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";
                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();

                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/CurFeedbackReport";
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("TargetPersonID", strTargetPersonID));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("GroupList", strGroupList));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("SelfNameGrpVisibility", strSelfNameGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FullProjGrpVisibility", strFullProjGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ParamConclusionHLRange", strConHighLowRange));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PreScoreVisibility", strPreScoreVisibility));
                rview.ServerReport.SetParameters(paramList);
            }
            //            rview.Visible = false;
            byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            //string PDF_path = root + dirName + "\\" + strReportName + ".pdf";
            string PDF_path = root + strReportName + ".pdf";
            FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create);
            objFs.Write(bytes, 0, bytes.Length);
            objFs.Close();
            objFs.Dispose();


            bytes = null;
            System.GC.Collect();
            rview.Dispose();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Initilize parameter for control report.
    /// </summary>
    /// <param name="projectid"></param>
    protected void ControlToParameter(string projectid)
    {
        if (projectid != null)
        {
            ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();

            DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));

            if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
            {
                // This parameter will Decide: which type of Report will Call                
                if (dataTableReportsetting.Rows[0]["ReportType"].ToString() != string.Empty)
                    strReportType = dataTableReportsetting.Rows[0]["ReportType"].ToString();

                if (dataTableReportsetting.Rows[0]["CoverPage"].ToString() != string.Empty)
                    strFrontPage = dataTableReportsetting.Rows[0]["CoverPage"].ToString();

                if (dataTableReportsetting.Rows[0]["ReportIntroduction"].ToString() != string.Empty)
                    strReportIntroduction = dataTableReportsetting.Rows[0]["ReportIntroduction"].ToString();

                if (dataTableReportsetting.Rows[0]["Conclusionpage"].ToString() != string.Empty)
                    strConclusionPage = dataTableReportsetting.Rows[0]["Conclusionpage"].ToString();

                if (dataTableReportsetting.Rows[0]["RadarChart"].ToString() != string.Empty)
                    strRadarChart = dataTableReportsetting.Rows[0]["RadarChart"].ToString();

                if (dataTableReportsetting.Rows[0]["QstTextResponses"].ToString() != string.Empty)
                    strDetailedQst = dataTableReportsetting.Rows[0]["QstTextResponses"].ToString();

                if (dataTableReportsetting.Rows[0]["CatQstList"].ToString() != string.Empty)
                    strCategoryQstlist = dataTableReportsetting.Rows[0]["CatQstList"].ToString();

                if (dataTableReportsetting.Rows[0]["CatDataChart"].ToString() != string.Empty)
                    strCategoryBarChart = dataTableReportsetting.Rows[0]["CatDataChart"].ToString();

                if (dataTableReportsetting.Rows[0]["CandidateSelfStatus"].ToString() != string.Empty)
                    strSelfNameGrp = dataTableReportsetting.Rows[0]["CandidateSelfStatus"].ToString();

                if (dataTableReportsetting.Rows[0]["FullProjectGrp"].ToString() != string.Empty)
                    strFullProjGrp = dataTableReportsetting.Rows[0]["FullProjectGrp"].ToString();

                if (dataTableReportsetting.Rows[0]["ProgrammeGrp"].ToString() != string.Empty)
                    strProgrammeGrp = dataTableReportsetting.Rows[0]["ProgrammeGrp"].ToString();

                if (dataTableReportsetting.Rows[0]["ProjectRelationGrp"].ToString() != string.Empty)
                    strGroupList = dataTableReportsetting.Rows[0]["ProjectRelationGrp"].ToString();

                if (dataTableReportsetting.Rows[0]["ConclusionHighLowRange"].ToString() != string.Empty)
                    strConHighLowRange = dataTableReportsetting.Rows[0]["ConclusionHighLowRange"].ToString();

                if (dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString() != string.Empty)
                    strPreScoreVisibility = dataTableReportsetting.Rows[0]["PreviousScoreVisible"].ToString();

                if (dataTableReportsetting.Rows[0]["BenchMarkGrpVisible"].ToString() != string.Empty)
                    strBenchMarkGrpVisibility = dataTableReportsetting.Rows[0]["BenchMarkGrpVisible"].ToString();

                if (dataTableReportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() != string.Empty)
                    strBenchMarkVisibility = dataTableReportsetting.Rows[0]["BenchMarkScoreVisible"].ToString();

                if (dataTableReportsetting.Rows[0]["BenchConclusionpage"].ToString() != string.Empty)
                    strBenchConclusionPageVisibility = dataTableReportsetting.Rows[0]["BenchConclusionpage"].ToString();

            }
        }
    }

    #region Radar Chart Method
    /// <summary>
    /// Generate Radar chart
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void Radar(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Get Radar chart data for self assement.
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get Radar chart data for full project group.
        DataTable dataTableFullProjectData = reportManagementBusinessAccessObject.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            xValues[i] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dataTableFullProjectData.Rows.Count];
        double[] yValues1 = new double[dataTableFullProjectData.Rows.Count];
        for (int i = 0; i < dataTableFullProjectData.Rows.Count; i++)
        {
            xValues1[i] = dataTableFullProjectData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dataTableFullProjectData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullProjectData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullProjectData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullProjectData.Rows.Count > 0)
            Series2 = dataTableFullProjectData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullProjectData.Rows.Count > 0 || dataTableFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
    }

    /// <summary>
    /// This shows the previous scores of the colleagues of the participant
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarPreviousScore(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Get previous scores of the self assement.
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get previous scores of the colleagues of the participant 
        DataTable dataTableFullPreviousData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            xValues[i] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dataTableFullPreviousData.Rows.Count];
        double[] yValues1 = new double[dataTableFullPreviousData.Rows.Count];
        for (int i = 0; i < dataTableFullPreviousData.Rows.Count; i++)
        {
            xValues1[i] = dataTableFullPreviousData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dataTableFullPreviousData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullPreviousData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullPreviousData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullPreviousData.Rows.Count > 0)
            Series2 = dataTableFullPreviousData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullPreviousData.Rows.Count > 0 || dataTableFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
    }

    /// <summary>
    /// Radar chart data for self and full project group
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarCPL(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();

        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Get radar chart data for self assement
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get radar chart data for full project group
        DataTable dataTableFullProjectData = reportManagementBusinessAccessObject.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            if (i == dataTableSelfData.Rows.Count - 1)
            {
                xValues[0] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[0] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues[i + 1] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i + 1] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
        }

        string[] xValues1 = new string[dataTableFullProjectData.Rows.Count];
        double[] yValues1 = new double[dataTableFullProjectData.Rows.Count];
        for (int i = 0; i < dataTableFullProjectData.Rows.Count; i++)
        {
            if (i == dataTableFullProjectData.Rows.Count - 1)
            {
                xValues1[0] = dataTableFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[0] = Convert.ToDouble(dataTableFullProjectData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues1[i + 1] = dataTableFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[i + 1] = Convert.ToDouble(dataTableFullProjectData.Rows[i]["Average"].ToString());
            }
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullProjectData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullProjectData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullProjectData.Rows.Count > 0)
            Series2 = dataTableFullProjectData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullProjectData.Rows.Count > 0 || dataTableFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
    }

    /// <summary>
    /// Get previous score for radar chart 
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarPreviousScoreCPL(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Get radar chart previous score data for self
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        //Get radar chart previous score data 
        DataTable dataTableFullPreviousData = reportManagementBusinessAccessObject.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            if (i == dataTableSelfData.Rows.Count - 1)
            {
                xValues[0] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[0] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues[i + 1] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i + 1] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
            }
        }

        string[] xValues1 = new string[dataTableFullPreviousData.Rows.Count];
        double[] yValues1 = new double[dataTableFullPreviousData.Rows.Count];
        for (int i = 0; i < dataTableFullPreviousData.Rows.Count; i++)
        {
            if (i == dataTableFullPreviousData.Rows.Count - 1)
            {
                xValues1[0] = dataTableFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[0] = Convert.ToDouble(dataTableFullPreviousData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues1[i + 1] = dataTableFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[i + 1] = Convert.ToDouble(dataTableFullPreviousData.Rows[i]["Average"].ToString());
            }
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableFullPreviousData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableFullPreviousData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableFullPreviousData.Rows.Count > 0)
            Series2 = dataTableFullPreviousData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableFullPreviousData.Rows.Count > 0 || dataTableFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
    }

    /// <summary>
    /// Get Radar chart benchmark data for self and full project group.
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    public void RadarBenchMark(string strTargetPersonID)
    {
        ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        //Radar chart benchmark data for self.
        DataTable dataTableSelfData = reportManagementBusinessAccessObject.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "S");
        //Radar chart benchmark data for previous project.
        DataTable dataTableBenchMarkData = reportManagementBusinessAccessObject.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "P");

        string[] xValues = new string[dataTableSelfData.Rows.Count];
        double[] yValues = new double[dataTableSelfData.Rows.Count];
        for (int i = 0; i < dataTableSelfData.Rows.Count; i++)
        {
            xValues[i] = dataTableSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dataTableSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dataTableBenchMarkData.Rows.Count];
        double[] yValues1 = new double[dataTableBenchMarkData.Rows.Count];
        for (int i = 0; i < dataTableBenchMarkData.Rows.Count; i++)
        {
            xValues1[i] = dataTableBenchMarkData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dataTableBenchMarkData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dataTableBenchMarkData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dataTableBenchMarkData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dataTableSelfData.Rows.Count > 0)
            Series1 = dataTableSelfData.Rows[0]["RelationShip"].ToString();
        if (dataTableBenchMarkData.Rows.Count > 0)
            Series2 = dataTableBenchMarkData.Rows[0]["RelationShip"].ToString();

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dataTableBenchMarkData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(193, 255, 193); //(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dataTableSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dataTableBenchMarkData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dataTableSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dataTableBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarBenchmark = Server.MapPath("~\\UploadDocs\\") + "RadarChartBenchMark" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dataTableBenchMarkData.Rows.Count > 0 || dataTableBenchMarkData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarBenchmark);
    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion

    #endregion

    /// <summary>
    /// Initilize parameters for participants
    /// </summary>
    /// <param name="targetid">user id</param>
    /// <param name="strTargetPersonID">participant id</param>
    /// <param name="strProjectID">return project id</param>
    /// <param name="strAccountID">return account id</param>
    /// <param name="strProgrammeID">return program id</param>
    protected void GetDetailFromTargetPersonID(string targetid, out string strTargetPersonID,
        out string strProjectID, out string strAccountID, out string strProgrammeID)
    {
        strTargetPersonID = targetid;
        ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
        AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
        //Get all Questionnaire List
        DataTable dataTableUserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(strTargetPersonID));
        if (dataTableUserlist != null && dataTableUserlist.Rows.Count > 0)
        {
            //Set project id value
            int projectid = Convert.ToInt32(dataTableUserlist.Rows[0]["ProjectID"]);
            strProjectID = dataTableUserlist.Rows[0]["ProjectID"].ToString();

            Project_BAO projectBusinessAccessObject = new Project_BAO();
            //get all projects
            DataTable dataTableProject = projectBusinessAccessObject.GetdataProjectByID(projectid);
            //Set Account id value
            if (dataTableProject != null && dataTableProject.Rows.Count > 0)
            {
                strAccountID = dataTableProject.Rows[0]["AccountID"].ToString();
            }
            else
                strAccountID = Convert.ToString(ViewState["accid"]);

            //Get all program by project id
            DataTable dataTableProgramme = reportManagementBusinessAccessObject.GetdataProjectByID(projectid);
            if (dataTableProgramme != null && dataTableProgramme.Rows.Count > 0)
            {
                //Set program id 
                strProgrammeID = dataTableProgramme.Rows[0]["ProgrammeID"].ToString();
            }
            else
                strProgrammeID = "0";
        }
        else
        {
            //Set previous value.
            strProjectID = Convert.ToString(ViewState["prjid"]);

            strAccountID = Convert.ToString(ViewState["accid"]);

            int projectid = Convert.ToInt32(ViewState["prjid"]);

            DataTable dataTableProgramme = reportManagementBusinessAccessObject.GetdataProjectByID(projectid);

            if (dataTableProgramme != null && dataTableProgramme.Rows.Count > 0)
            {
                strProgrammeID = dataTableProgramme.Rows[0]["ProgrammeID"].ToString();
            }
            else
                strProgrammeID = "0";
        }
    }

    /// <summary>
    /// Perform  self assement survey
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSelfAssessment_Click(object sender, ImageClickEventArgs e)
    {
        //Enable Disable self assessment button
        AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
        DataTable dataTableResult = new DataTable();
        WADIdentity identity = this.Page.User.Identity as WADIdentity;

        int userID = Convert.ToInt32(identity.User.UserID);

        if (ddlTargetPerson.Visible)
            userID = Convert.ToInt32(Convert.ToInt32(ddlTargetPerson.SelectedValue));

        dataTableResult = assignQuestionnaireBusinessAccessObject.GetFeedbackURL(userID);

        if (dataTableResult.Rows.Count > 0)
        {
            string url = dataTableResult.Rows[0]["FeedbackUrl"].ToString();
            if (string.IsNullOrEmpty(url))
            {
                int assignmentID = assignQuestionnaireBusinessAccessObject.GetAssignmentID(userID);
                string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                DataTable dataTableQuestionnaireList = new DataTable();
                dataTableQuestionnaireList = assignQuestionnaireBusinessAccessObject.GetdtAssignQuestionnaireListDetails(assignmentID.ToString());

                string questionnaireID = dataTableQuestionnaireList.Rows[0]["QuestionnaireID"].ToString();
                string candidateID = dataTableQuestionnaireList.Rows[0]["AsgnDetailID"].ToString();

                string path = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
                string feedbackURL = urlPath + "Feedback.aspx?QID=" + PasswordGenerator.EnryptString(questionnaireID) + "&CID=" + PasswordGenerator.EnryptString(dataTableQuestionnaireList.Rows[0]["AsgnDetailID"].ToString());
                assignQuestionnaireBusinessAccessObject.SetFeedbackURL(Convert.ToInt32(dataTableQuestionnaireList.Rows[0]["AsgnDetailID"].ToString()), Convert.ToInt32(dataTableQuestionnaireList.Rows[0]["AssignmentID"].ToString()), feedbackURL);

                url = feedbackURL;
            }

            Response.Redirect(url, false);
        }
    }

    /// <summary>
    /// SAve all recode in grid at once.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImageButtonSaveAll_Click(object sender, ImageClickEventArgs e)
    {
        Session["ColleaguesIndex"] = null;

        for (int i = 0; i < rptrCandidateList.Items.Count; i++)
        {
            ImageButton assignButton = (ImageButton)rptrCandidateList.Items[i].FindControl("imbSaveColleague");
            ImageButton saveButton = (ImageButton)rptrCandidateList.Items[i].FindControl("imbSaveOnlyColleague");
            DropDownList dropDownRelationship = (DropDownList)rptrCandidateList.Items[i].FindControl("ddlRelationship");
            Label labelAssignmentID = (Label)rptrCandidateList.Items[i].FindControl("lblAssignID");
            Label labelAccountID = (Label)rptrCandidateList.Items[i].FindControl("lblAccountID");
            Label labelTargetPersonID = (Label)rptrCandidateList.Items[i].FindControl("lblTargetPersonID");
            Label labelAssignmentDetailsID = (Label)rptrCandidateList.Items[i].FindControl("lblAssignmentID");
            Label labelProjectID = (Label)rptrCandidateList.Items[i].FindControl("lblProjectID");
            TextBox textBoxCandidateName = (TextBox)rptrCandidateList.Items[i].FindControl("txtName");
            TextBox textBoxCandidateEmail = (TextBox)rptrCandidateList.Items[i].FindControl("txtEmailID");

            if (!string.IsNullOrEmpty((string)Session["ColleaguesIndex"]))
                Session["ColleaguesIndex"] = string.Format("{0},{1},", Session["ColleaguesIndex"], i);
            else
                Session["ColleaguesIndex"] = i.ToString();

            string relationship = dropDownRelationship.SelectedItem.Text;

            if (assignButton.Visible)
            {
                Session["UnsavedColleagueTable"] = GetUnSavedCandidatureList();
                //save candidate and send email.
                SaveCandidate(dropDownRelationship.SelectedValue, dropDownRelationship.SelectedItem.Text,
                    textBoxCandidateName.Text, textBoxCandidateEmail.Text);
            }
            else
            {
                if (!string.IsNullOrEmpty(labelAssignmentID.Text) && !string.IsNullOrEmpty(labelAccountID.Text) && !string.IsNullOrEmpty(labelTargetPersonID.Text)
                    && !string.IsNullOrEmpty(labelAssignmentDetailsID.Text) && !string.IsNullOrEmpty(labelProjectID.Text))
                {
                    int assignmentID = int.Parse(labelAssignmentID.Text.Trim());
                    int accountID = int.Parse(labelAccountID.Text.Trim());
                    int targetPersonID = int.Parse(labelTargetPersonID.Text.Trim());
                    int assignmentDetailsID = int.Parse(labelAssignmentDetailsID.Text.Trim());
                    int projectID = int.Parse(labelProjectID.Text.Trim());

                    ReSendColleagueEmail(assignmentID, accountID, targetPersonID, assignmentDetailsID, projectID,
                           textBoxCandidateName.Text, textBoxCandidateEmail.Text, relationship, false);
                }
            }
        }

        ReBindGrid();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Find colleague list which are not save and are newly added.
    /// </summary>
    /// <returns></returns>
    private DataTable GetUnSavedCandidatureList()
    {
        DataTable dataTableCandidateClone = new DataTable();

        if (rptrCandidateList.Items.Count > 0)
        {
            DataTable dtCandidate = Session["ColleagueTable"] as DataTable;
            dataTableCandidateClone = dtCandidate.Clone();

            dataTableCandidateClone.Clear();

            for (int j = 0; j < rptrCandidateList.Items.Count; j++)
            {
                DropDownList dropDownRelationship = (DropDownList)rptrCandidateList.Items[j].FindControl("ddlRelationship");
                TextBox textBoxCandidateName = (TextBox)rptrCandidateList.Items[j].FindControl("txtName");
                TextBox textBoxCandidateEmail = (TextBox)rptrCandidateList.Items[j].FindControl("txtEmailID");
                Label labelTargetPersonID = (Label)rptrCandidateList.Items[j].FindControl("lblTargetPersonID");

                //if target person text is blank then newly added record
                if (string.IsNullOrEmpty(labelTargetPersonID.Text))
                {
                    DataRow dataRowCandidate = dataTableCandidateClone.NewRow();

                    dataRowCandidate["Relationship"] = dropDownRelationship.SelectedItem.Text;
                    dataRowCandidate["Name"] = textBoxCandidateName.Text;
                    dataRowCandidate["EmailID"] = textBoxCandidateEmail.Text;
                    dataTableCandidateClone.Rows.Add(dataRowCandidate);
                }
            }

            dataTableCandidateClone.AcceptChanges();
        }
        return dataTableCandidateClone;
    }

    /// <summary>
    /// Common methos to bind grid.
    /// </summary>
    private void ReBindGrid()
    {
        AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
        //when user login then participant drop down visible true and bind repeator .
        if (ddlTargetPerson.Visible == true)
        {
            rptrCandidateList.DataSource = assignQuestionnaireBusinessAccessObject.GetdtAssignColleagueList(ddlTargetPerson.SelectedValue, ddlProgramme.SelectedValue);
            rptrCandidateList.DataBind();
        }
        else
        {
            //else bind repeator by program.
            ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Get candidate list ot assign.
    /// </summary>
    private List<AssignmentDetails_BE> GetCandidateListToAssign(string relationShipID,
       string relationShip, string name, string emailID)
    {
        List<AssignmentDetails_BE> assignmentDetailsBusinessEntityList = new List<AssignmentDetails_BE>();

        AssignmentDetails_BE assignmentDetailsBusinessEntity = new AssignmentDetails_BE();

        //DropDownList ddlRelationship = (DropDownList)item.FindControl("ddlRelationship");
        //TextBox txtCandidateName = (TextBox)item.FindControl("txtName");
        //TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");
        if (relationShip.Trim() != "" && name.Trim() != "" && emailID.Trim() != "")
        {
            //if relationship is manager
            if (relationShip.ToUpper() == "MANAGER")
            {
                //if (this.isManager)
                //    this.duplicateManager = true;

                //this.isManager = true;
            }

            assignmentDetailsBusinessEntity.RelationShip = relationShipID;
            assignmentDetailsBusinessEntity.CandidateName = name.Trim();
            assignmentDetailsBusinessEntity.CandidateEmail = emailID.Trim();
            assignmentDetailsBusinessEntity.SubmitFlag = false;

            //If user is Super admin then account selected value esle user account id.
            if (identity.User.GroupID == 1)
                UserAccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                UserAccountID = Convert.ToInt32(identity.User.AccountID);

            if (UserAccountID == Convert.ToInt32(ConfigurationManager.AppSettings["AccountID"].ToString()))
                assignmentDetailsBusinessEntity.EmailSendFlag = 0;
            else
                assignmentDetailsBusinessEntity.EmailSendFlag = 1;

            if (assignmentDetailsBusinessEntity.RelationShip != "" && assignmentDetailsBusinessEntity.CandidateName != "" && assignmentDetailsBusinessEntity.CandidateEmail != "")
            {
                assignmentDetailsBusinessEntityList.Add(assignmentDetailsBusinessEntity);
                email += emailID.Trim() + ";";
            }
        }

        DataTable dataTableColleague = Session["ColleagueTable"] as DataTable;
        bool AddSelf = true;

        if (dataTableColleague != null && dataTableColleague.Rows.Count > 1)
        {
            //update colleague datatable if it is self assement
            DataRow[] dataRowColleague = dataTableColleague.Select("RelationShip = 'Self'");

            if (dataRowColleague.Count() > 0)
                AddSelf = false;
        }

        // if self assement
        if (AddSelf)
        {
            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (identity.User.GroupID.ToString() != participantRoleId)
                targetpersonid = Convert.ToInt32(ddlTargetPerson.SelectedValue);
            else
                targetpersonid = Convert.ToInt32(identity.User.UserID);

            if (identity.User.GroupID == 1)
                UserAccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                UserAccountID = Convert.ToInt32(identity.User.AccountID);

            AccountUser_BAO user = new AccountUser_BAO();
            //Get userlist by account id and set properties.
            List<AccountUser_BE> Userlist = user.GetAccountUserByID(UserAccountID, targetpersonid);
            AssignmentDetails_BE assignmentDetails = new AssignmentDetails_BE();

            assignmentDetails.CandidateEmail = Userlist[0].EmailID;
            assignmentDetails.CandidateName = Userlist[0].FirstName + " " + Userlist[0].LastName;
            assignmentDetails.RelationShip = "Self";
            assignmentDetails.SubmitFlag = false;
            assignmentDetails.EmailSendFlag = 1;

            assignmentDetailsBusinessEntityList.Add(assignmentDetails);

            email += Userlist[0].EmailID + ";";
        }
        finalemail = email.TrimEnd(';');

        return assignmentDetailsBusinessEntityList;
    }

    /// <summary>
    /// Initlize the template and send mail to colleague and participant.
    /// </summary>
    /// <param name="assignmentID">assignment id</param>
    /// <param name="accountID">account id</param>
    /// <param name="targetPersonID">participant id</param>
    /// <param name="assignmentDetailsID">assignmant detaisl id</param>
    /// <param name="projectID">project id</param>
    /// <param name="candidateName">candidate name</param>
    /// <param name="colleagueEmail">colleague email id</param>
    /// <param name="relationship">relationship to colleague.</param>
    /// <param name="sendEmail">is mail is send or not</param>
    private void ReSendColleagueEmail(int assignmentID, int accountID, int targetPersonID,
        int assignmentDetailsID, int projectID, string candidateName, string colleagueEmail, string relationship, bool sendEmail)
    {
        //Send Email to Candidate

        AssignQuestionnaire_BAO assignquestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
        DataTable dataTableResult = new DataTable();
        //Get questionnaire list by assignmentid.
        dataTableResult = assignquestionnaireBusinessAccessObject.GetdtAssignQuestionnaireList(assignmentID);

        DataTable dataTableClone = new DataTable();
        dataTableClone = dataTableResult.Clone();

        DataRow[] result = dataTableResult.Select("AsgnDetailID=" + assignmentDetailsID);

        foreach (DataRow dataRow in result)
            dataTableClone.ImportRow(dataRow);

        dataTableResult = dataTableClone;

        if (result.Count() > 0)
        {
            //chack if email id is canged or not.
            if (dataTableResult.Rows[0]["CandidateEmail"].ToString().ToLower() != colleagueEmail.ToLower() || dataTableResult.Rows[0]["CandidateName"].ToString().ToLower() != candidateName.ToLower() || dataTableResult.Rows[0]["Relationship"].ToString().ToLower() != relationship.ToLower())
            {
                assignquestionnaireBusinessAccessObject.UpdateCandidateEmail(assignmentDetailsID, colleagueEmail, candidateName, relationship);
                dataTableResult.Rows[0]["CandidateEmail"] = colleagueEmail;
                dataTableResult.AcceptChanges();
            }
        }

        string imagepath = Server.MapPath("~/EmailImages/");

        //Send mail to  all candidates     
        if (sendEmail)
        {
            for (int i = 0; i < dataTableResult.Rows.Count; i++)
            {
                AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
                DataTable dataTableAccountAdmin = new DataTable();
                dataTableAccountAdmin = accountUserBusinessAccessObject.GetdtAccountUserByID(accountID, targetPersonID);

                string Template = assignquestionnaireBusinessAccessObject.FindTemplate(Convert.ToInt32(projectID));
                string Subject = assignquestionnaireBusinessAccessObject.FindCandidateSubjectTemplate(Convert.ToInt32(projectID));

                // Get Candidate Email Image Name & Will Combined with EmailImagePath
                DataTable dataTableCandidateEmailImage = new DataTable();
                string emailimagepath = "";

                dataTableCandidateEmailImage = assignquestionnaireBusinessAccessObject.GetCandidateEmailImageInfo(Convert.ToInt32(projectID));

                if (dataTableCandidateEmailImage.Rows.Count > 0 && dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                    emailimagepath = imagepath + dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString();

                string candidateEmail = "";
                string questionnaireID = "";
                string candidateID = "";
                string OrganisationName = "";
                string Startdate = "";
                string Enddate = "";
                string CandidateName = "";
                string FirstName = "";

                candidateEmail = dataTableResult.Rows[i]["CandidateEmail"].ToString();
                questionnaireID = dataTableResult.Rows[i]["QuestionnaireID"].ToString();
                candidateID = dataTableResult.Rows[i]["AsgnDetailID"].ToString();
                OrganisationName = dataTableResult.Rows[i]["OrganisationName"].ToString();
                Startdate = Convert.ToDateTime(dataTableResult.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy");
                Enddate = Convert.ToDateTime(dataTableResult.Rows[i]["Enddate"]).ToString("dd-MMM-yyyy");
                CandidateName = dataTableResult.Rows[i]["CandidateName"].ToString();
                string[] strFName = CandidateName.Split(' ');
                FirstName = strFName[0].ToString();

                questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                candidateID = PasswordGenerator.EnryptString(candidateID);
                //set feedback link path.
                string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";
                //initilize the tempalte with token
                Template = Template.Replace("[LINK]", link);
                Template = Template.Replace("[NAME]", CandidateName);
                Template = Template.Replace("[FIRSTNAME]", FirstName);
                Template = Template.Replace("[COMPANY]", OrganisationName);
                Template = Template.Replace("[STARTDATE]", Startdate);
                Template = Template.Replace("[CLOSEDATE]", Enddate);
                Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");

                Subject = Subject.Replace("[NAME]", CandidateName);
                Subject = Subject.Replace("[FIRSTNAME]", FirstName);
                Subject = Subject.Replace("[COMPANY]", OrganisationName);
                Subject = Subject.Replace("[STARTDATE]", Startdate);
                Subject = Subject.Replace("[CLOSEDATE]", Enddate);


                if (dataTableAccountAdmin.Rows.Count > 0)
                {
                    Template = Template.Replace("[PARTICIPANTNAME]", dataTableAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dataTableAccountAdmin.Rows[0]["LastName"].ToString());
                    Template = Template.Replace("[PARTICIPANTEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                    Subject = Subject.Replace("[PARTICIPANTNAME]", dataTableAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dataTableAccountAdmin.Rows[0]["LastName"].ToString());
                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                    //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());


                    MailAddress eMailddress = new MailAddress("admin@i-comment360.com", dataTableAccountAdmin.Rows[0].Field<string>("Pseudonym") ?? "360 feedback");
                    //send mail 
                    SendEmail.Send(Subject, Template, candidateEmail, eMailddress, emailimagepath);
                }
                else
                {
                    Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                    Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                    Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");
                    //send mail
                    SendEmail.Send(Subject, Template, candidateEmail, null, string.Empty);
                }
            }

            // Create a new ListItem object for the contact in the row.     
            ListItem item = new ListItem();
            //lblMessage.Text = "Email sent successfully to " + candidateName;
            lblMessage2.Text = "Email sent successfully to " + candidateName;
        }
    }

    /// <summary>
    /// Save candidate and send mail and bind gridview.
    /// </summary>
    /// <param name="relationShipID"></param>
    /// <param name="relationShip"></param>
    /// <param name="name"></param>
    /// <param name="emailID"></param>
    private void SaveCandidate(string relationShipID, string relationShip, string name, string emailID)
    {
        try
        {
            //imbAssign.Enabled = false;
            lblMessage.Text = "";
            lblMessage2.Text = "";
            lblvalidation.Text = "";
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            AssignQuestionnaire_BE assignquestionnaireBusinessEntity = new AssignQuestionnaire_BE();
            AssignQuestionnaire_BAO assignquestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

            assignquestionnaireBusinessEntity.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
            assignquestionnaireBusinessEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);

            //Changes here 
            //assignquestionnaire_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
            assignquestionnaireBusinessEntity.QuestionnaireID = questionnaireBusinessAccessObject.GetQuestionnaireID(assignquestionnaireBusinessEntity.ProjecctID.ToString());

            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (ddlTargetPerson.Visible == false)
            {
                assignquestionnaireBusinessEntity.TargetPersonID = Convert.ToInt32(identity.User.UserID);
            }
            else
            {
                assignquestionnaireBusinessEntity.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
            }

            assignquestionnaireBusinessEntity.Description = "";
            identity = this.Page.User.Identity as WADIdentity;

            //If user is SuperAdmin then account drop down selected value else user account value.
            if (identity.User.GroupID == 1)
            {
                assignquestionnaireBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                dtCandidateList = assignquestionnaireBusinessAccessObject.GetdtAssignList(ddlTargetPerson.SelectedValue, ddlProgramme.SelectedValue);
            }
            else
            {
                assignquestionnaireBusinessEntity.AccountID = identity.User.AccountID;
                dtCandidateList = assignquestionnaireBusinessAccessObject.GetdtAssignList(identity.User.UserID.ToString(), ddlProgramme.SelectedValue);
            }
            //if (txtCandidateNo.Text.Trim() != "" || txtCandidateNo.Text.Trim() == "0")
            //{
            //    assignquestionnaire_BE.CandidateNo = Convert.ToInt32(txtCandidateNo.Text.Trim());
            //}

            //set just to run the code need to check this when work complete
            //assignquestionnaire_BE.CandidateNo = 1;

            //Encript questionnaire id and set the feedback url
            string QuestionnaireID = PasswordGenerator.EnryptString(assignquestionnaireBusinessEntity.QuestionnaireID.ToString());
            string candidateId = PasswordGenerator.EnryptString(assignquestionnaireBusinessEntity.TargetPersonID.ToString());
            string path = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
            string feedbackurl = path + "Feedback.aspx?QID=" + QuestionnaireID + "&CID=" + candidateId;

            assignquestionnaireBusinessEntity.ModifiedBy = 1;
            assignquestionnaireBusinessEntity.ModifiedDate = DateTime.Now;
            assignquestionnaireBusinessEntity.IsActive = 1;
            assignquestionnaireBusinessEntity.FeedbackURL = feedbackurl;
            //get candidate list 
            assignquestionnaireBusinessEntity.AssignmentDetails = GetCandidateListToAssign(relationShipID, relationShip, name, emailID);//GetCandidateList();

            if (assignquestionnaireBusinessEntity.AssignmentDetails.Count > 1)
            {
                string accountID = ConfigurationManager.AppSettings["AccountID"].ToString();

                if (assignquestionnaireBusinessEntity.AccountID == Convert.ToInt32(accountID))
                {
                    int managerCount = CheckManagerCount();

                    int count = (from Ad in assignquestionnaireBusinessEntity.AssignmentDetails
                                 where Ad.RelationShip.ToLower() == "manager"
                                 select Ad).Count();

                    managerCount = managerCount + count;
                    //validate the relation ship dropdown in grid.
                    if (managerCount > 1)
                    {
                        lblvalidation.Text = "Please select only one relationship as Manager.";
                        return;
                    }
                    if (managerCount < 1)
                    {
                        lblvalidation.Text = "Please select one relationship as Manager.";
                        return;
                    }
                }

                //Save Assign questionnaire
                int?[] assigDetailsID = assignquestionnaireBusinessAccessObject.AddAssignQuestionnaireForColleagues(assignquestionnaireBusinessEntity);

                string strassigDetailsIDs = string.Empty;
                for (int i = 0; i < assigDetailsID.Count(); i++)
                {
                    if (assigDetailsID[i].HasValue)
                        strassigDetailsIDs += assigDetailsID[i].Value.ToString() + ",";
                }

                strassigDetailsIDs = strassigDetailsIDs.TrimEnd(',');


                DataTable dataTableResult = new DataTable();
                dataTableResult = assignquestionnaireBusinessAccessObject.GetdtAssignQuestionnaireListDetails(strassigDetailsIDs);

                //int loopCount = 0;
                //char loopFlag = 'N';

                //for (int k = 0; k < Convert.ToInt32(txtCandidateNo.Text.Trim()); k++)
                //{
                //    if (dtResult.Rows[k]["RelationShip"].ToString() == "Self")
                //        loopFlag = 'Y';
                //}

                //if (loopFlag == 'Y')
                //    loopCount = Convert.ToInt32(assignquestionnaire_BE.CandidateNo) + 1;
                //else
                //    loopCount = Convert.ToInt32(assignquestionnaire_BE.CandidateNo);

                if (assignquestionnaireBusinessEntity.AccountID != Convert.ToInt32(accountID))
                {
                    //Send mail to candidates
                    string imagepath = Server.MapPath("~/EmailImages/");

                    for (int i = 0; i < dataTableResult.Rows.Count; i++)
                    {
                        AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
                        DataTable dataTableAccountAdmin = new DataTable();

                        dataTableAccountAdmin = accountUserBusinessAccessObject.GetdtAccountUserByID(Convert.ToInt32(assignquestionnaireBusinessEntity.AccountID), Convert.ToInt32(assignquestionnaireBusinessEntity.TargetPersonID));

                        Template = assignquestionnaireBusinessAccessObject.FindTemplate(Convert.ToInt32(ddlProject.SelectedValue));
                        Subject = assignquestionnaireBusinessAccessObject.FindCandidateSubjectTemplate(Convert.ToInt32(ddlProject.SelectedValue));

                        // Get Candidate Email Image Name & Will Combined with EmailImagePath
                        DataTable dataTableCandidateEmailImage = new DataTable();
                        string emailimagepath = "";
                        // Get Candidate Email
                        dataTableCandidateEmailImage = assignquestionnaireBusinessAccessObject.GetCandidateEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));

                        if (dataTableCandidateEmailImage.Rows.Count > 0 && dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                            emailimagepath = imagepath + dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString();

                        string questionnaireID = "";
                        string candidateID = "";
                        string OrganisationName = "";
                        string Startdate = "";
                        string Enddate = "";
                        string CandidateName = "";
                        string FirstName = "";
                        string candidateEmail = "";
                        //Initilize properties 
                        candidateEmail = dataTableResult.Rows[i]["CandidateEmail"].ToString();
                        questionnaireID = dataTableResult.Rows[i]["QuestionnaireID"].ToString();
                        candidateID = dataTableResult.Rows[i]["AsgnDetailID"].ToString();
                        OrganisationName = dataTableResult.Rows[i]["OrganisationName"].ToString();
                        Startdate = Convert.ToDateTime(dataTableResult.Rows[0]["StartDate"]).ToString("dd-MMM-yyyy");
                        Enddate = Convert.ToDateTime(dataTableResult.Rows[0]["Enddate"]).ToString("dd-MMM-yyyy");
                        CandidateName = dataTableResult.Rows[i]["CandidateName"].ToString();
                        string[] strFName = CandidateName.Split(' ');
                        FirstName = strFName[0].ToString();

                        questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                        candidateID = PasswordGenerator.EnryptString(candidateID);
                        //Set feed back url link
                        string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                        string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                        if (dataTableResult.Rows[i]["RelationShip"].ToString() == "Self")
                        {
                            string feedbackURL = urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + PasswordGenerator.EnryptString(dataTableResult.Rows[i]["AsgnDetailID"].ToString());
                            assignquestionnaireBusinessAccessObject.SetFeedbackURL(Convert.ToInt32(dataTableResult.Rows[i]["AsgnDetailID"].ToString()), Convert.ToInt32(dataTableResult.Rows[i]["AssignmentID"].ToString()), feedbackURL);
                        }
                        // Set template value and repalce token
                        Template = Template.Replace("[LINK]", link);
                        Template = Template.Replace("[NAME]", CandidateName);
                        Template = Template.Replace("[FIRSTNAME]", FirstName);
                        Template = Template.Replace("[COMPANY]", OrganisationName);
                        Template = Template.Replace("[STARTDATE]", Startdate);
                        Template = Template.Replace("[CLOSEDATE]", Enddate);
                        Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");

                        Subject = Subject.Replace("[NAME]", CandidateName);
                        Subject = Subject.Replace("[FIRSTNAME]", FirstName);
                        Subject = Subject.Replace("[COMPANY]", OrganisationName);
                        Subject = Subject.Replace("[STARTDATE]", Startdate);
                        Subject = Subject.Replace("[CLOSEDATE]", Enddate);

                        //If it is self assement
                        if (dataTableResult.Rows[i]["RelationShip"].ToString() != "Self")
                        {
                            if (dataTableAccountAdmin.Rows.Count > 0)
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", dataTableAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dataTableAccountAdmin.Rows[0]["LastName"].ToString());
                                Template = Template.Replace("[PARTICIPANTEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                                Subject = Subject.Replace("[PARTICIPANTNAME]", dataTableAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dataTableAccountAdmin.Rows[0]["LastName"].ToString());
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());
                                //get the mail Pseudonym.
                                string emailPseudonym = dataTableAccountAdmin.Rows[0].Field<string>("Pseudonym");

                                MailAddress emailAddress = new MailAddress("admin@i-comment360.com", string.IsNullOrEmpty(emailPseudonym) ? "360 feedback" : emailPseudonym);
                                //Send mail with template
                                SendEmail.SendMailAsync(Subject, Template, dataTableResult.Rows[i]["CandidateEmail"].ToString(), emailAddress, emailimagepath, "");
                            }
                            else
                            {
                                //if it is Participant
                                Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                                Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                                Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");
                                //send mail with template
                                SendEmail.SendMailAsync(Subject, Template, dataTableResult.Rows[i]["CandidateEmail"].ToString(), null, string.Empty, "");
                            }
                        }
                    }

                    //lblMessage.Text = "Email successfully sent";
                    lblMessage2.Text = "Email successfully sent to " + name;
                }
                else
                {
                    int assignmentID = assignquestionnaireBusinessAccessObject.GetAssignmentID(assignquestionnaireBusinessEntity);

                    Project_BAO project_BAO = new Project_BAO();
                    List<Project_BE> projectList = new List<Project_BE>();
                    projectList = project_BAO.GetProjectByID(Convert.ToInt32(assignquestionnaireBusinessEntity.AccountID), Convert.ToInt32(assignquestionnaireBusinessEntity.ProjecctID));

                    int managerEmailId = Convert.ToInt32(projectList[0].EmailTMPManager);

                    EmailTemplate_BAO emailTemplate_BAO = new EmailTemplate_BAO();
                    List<EmailTemplate_BE> emailTemplateList = new List<EmailTemplate_BE>();
                    emailTemplateList = emailTemplate_BAO.GetEmailTemplateByID(Convert.ToInt32(assignquestionnaireBusinessEntity.AccountID), managerEmailId);

                    string emailText = emailTemplateList[0].EmailText;
                    string emailSubject = emailTemplateList[0].Subject;

                    StringBuilder candidatelist = new StringBuilder();
                    candidatelist.Append("<table width='500' border='1' cellspacing='0'>");

                    candidatelist.Append("<tr><td width='50%'><b>Name</b></td><td width='50%'><b>Relationship</b></td></tr>");

                    DataTable dtColleagueList = new DataTable();
                    dtColleagueList = assignquestionnaireBusinessAccessObject.GetColleaguesList(assignmentID);

                    string lineManagerName = "";
                    string lineManagerEmail = "";
                    string participantName = "";

                    for (int i = 0; i < dtColleagueList.Rows.Count; i++)
                    {
                        candidatelist.Append("<tr>");
                        candidatelist.Append("<td>");
                        candidatelist.Append(dtColleagueList.Rows[i]["CandidateName"].ToString());
                        candidatelist.Append("</td>");
                        candidatelist.Append("<td>");
                        candidatelist.Append(dtColleagueList.Rows[i]["RelationShip"].ToString());
                        candidatelist.Append("</td>");
                        candidatelist.Append("</tr>");

                        if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Manager")
                        {
                            lineManagerName = dtColleagueList.Rows[i]["CandidateName"].ToString();
                            lineManagerEmail = dtColleagueList.Rows[i]["CandidateEmail"].ToString();
                        }

                        if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Self")
                            participantName = dtColleagueList.Rows[i]["CandidateName"].ToString();
                    }

                    candidatelist.Append("</table>");

                    string listOfNames = Convert.ToString(candidatelist);
                    string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
                    string asgnmentID = PasswordGenerator.EnryptString(Convert.ToString(assignmentID));
                    int candidateNumber = Convert.ToInt32(assignquestionnaireBusinessEntity.CandidateNo);

                    emailText = emailText.Replace("[MANAGERFIRSTNAME]", lineManagerName);
                    emailText = emailText.Replace("[PARTICIPANTNAME]", participantName);
                    emailText = emailText.Replace("[LISTOFNAMES]", listOfNames);
                    emailText = emailText.Replace("[ACCEPT]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("1") + "' >Accept</a>");
                    emailText = emailText.Replace("[DECLINE]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("0") + "' >Decline</a>");

                    emailSubject = emailSubject.Replace("[PARTICIPANTNAME]", participantName);
                    //send mail with tempalte.
                    SendEmail.SendMailAsync(Subject, Template, lineManagerEmail, null, string.Empty, "");

                    //lblMessage.Text = "Email has been sent successfully to Manager for further approval";
                    lblMessage2.Text = "Email has been sent successfully to Manager for further approval";
                    //imbAssign.Enabled = true;
                }

                //txtCandidateNo.Text = "";
            }
            else
            {
                // lblvalidation.Text = "Please  fill colleagues' information";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind candidate gridview.
    /// </summary>
    /// <param name="candidateCount"></param>
    private void BindCandidateList(int candidateCount)
    {
        try
        {
            //DataTable dtCandidate = new DataTable();
            //dtCandidate.Columns.Add("Relationship");
            //dtCandidate.Columns.Add("Name");
            //dtCandidate.Columns.Add("EmailID");

            //If paticipant has same candidate then 
            if (Session["ColleagueTable"] != null)
            {

                DataTable dataTableCandidate = Session["ColleagueTable"] as DataTable;
                //Add new row in dynamic colleague table by number of participant count. 
                for (int count = 0; count < candidateCount; count++)
                {
                    DataRow dataRowCandidate = dataTableCandidate.NewRow();
                    dataTableCandidate.Rows.Add(dataRowCandidate);
                }

                //then bind repeator with new colleague data table.
                BindColleagueRepeter(dataTableCandidate);
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind colleague repeator.
    /// </summary>
    /// <param name="dataTableCandidate"></param>
    private void BindColleagueRepeter(DataTable dataTableCandidate)
    {
        DataTable dataTableCandidateCopy = dataTableCandidate.Copy();


        DataTable dtUSC = Session["UnsavedColleagueTable"] as DataTable;
        //DataRow[] dr = dt.Select("Relationship='' AND Name='' AND EmailID=''"); 

        // if ColleagueTable is not null then remove blank rows and bind gridview.
        if (Session["UnsavedColleagueTable"] != null)
        {
            bool isRowDeleted = false;
            int removedRows = 0;

            for (int i = 0; i < dataTableCandidateCopy.Rows.Count; i++)
            {
                if ((dataTableCandidateCopy.Rows[i]["Relationship"] == DBNull.Value ||
                    string.IsNullOrEmpty(dataTableCandidateCopy.Rows[i]["Relationship"].ToString())) &&
                    (dataTableCandidateCopy.Rows[i]["Name"] == DBNull.Value ||
                    string.IsNullOrEmpty(dataTableCandidateCopy.Rows[i]["Name"].ToString())) &&
                    (dataTableCandidateCopy.Rows[i]["EmailID"] == DBNull.Value ||
                    string.IsNullOrEmpty(dataTableCandidateCopy.Rows[i]["EmailID"].ToString())))
                {
                    dataTableCandidateCopy.Rows[i].Delete();//remove blank rows
                    removedRows++;
                    i--;
                    isRowDeleted = true;
                    dataTableCandidateCopy.AcceptChanges();
                }
            }

            //if colleague index is not null make a copy of data table combind with colleagus table and bind gridview.
            if (Session["ColleaguesIndex"] != null)
            {
                string[] stringColleaguesIndex = null;
                if (!string.IsNullOrEmpty(Session["ColleaguesIndex"].ToString()))
                    stringColleaguesIndex = Session["ColleaguesIndex"].ToString().TrimEnd(',').Split(',');

                if (stringColleaguesIndex == null || dataTableCandidateCopy.Rows.Count == stringColleaguesIndex.Count())
                {
                    DataTable dataTableOrderedColleagues = dataTableCandidateCopy.Copy();
                    dataTableOrderedColleagues.Clear();
                    dataTableOrderedColleagues.Columns.Add(new DataColumn("ID", typeof(int)));
                    //add row to clloeague table
                    for (int i = 0; i < dataTableCandidateCopy.Rows.Count; i++)
                    {
                        DataRow dr = dataTableOrderedColleagues.NewRow();
                        dr["ID"] = Convert.ToInt32(stringColleaguesIndex[i]);
                        dr["TargetPersonID"] = dataTableCandidateCopy.Rows[i]["TargetPersonID"].ToString();
                        dr["ProjectID"] = dataTableCandidateCopy.Rows[i]["ProjectID"];
                        dr["AssignID"] = dataTableCandidateCopy.Rows[i]["AssignID"];
                        dr["Relationship"] = dataTableCandidateCopy.Rows[i]["Relationship"];
                        dr["Name"] = dataTableCandidateCopy.Rows[i]["Name"];
                        dr["EmailID"] = dataTableCandidateCopy.Rows[i]["EmailID"];
                        dr["AssignmentID"] = dataTableCandidateCopy.Rows[i]["AssignmentID"];
                        dr["SubmitFlag"] = dataTableCandidateCopy.Rows[i]["SubmitFlag"];
                        dr["EmailSendFlag"] = dataTableCandidateCopy.Rows[i]["EmailSendFlag"];

                        dataTableOrderedColleagues.Rows.Add(dr);
                    }
                    dataTableOrderedColleagues.AcceptChanges();

                    int totalRecords = dataTableCandidateCopy.Rows.Count + dtUSC.Rows.Count;

                    if (!dtUSC.Columns.Contains("ID"))
                        dtUSC.Columns.Add("ID", typeof(int));
                    int k = 0;

                    for (int i = 0; i < dtUSC.Rows.Count; i++)
                    {
                        if (k <= totalRecords)
                        {
                            if (stringColleaguesIndex != null)
                            {
                                for (int j = 0; j < stringColleaguesIndex.Count(); j++)
                                {
                                    if (k.ToString() == stringColleaguesIndex[j].ToString())
                                    {
                                        k++;
                                    }
                                }
                            }
                            dtUSC.Rows[i]["ID"] = k;
                            k++;
                        }
                    }
                    dtUSC.AcceptChanges();

                    dataTableOrderedColleagues.Merge(dtUSC);
                    dataTableOrderedColleagues.DefaultView.Sort = "ID";
                    DataView dvSorted = dataTableOrderedColleagues.DefaultView;

                    if (dvSorted.Table.Rows.Count > 0)
                    {
                        dataTableCandidateCopy.Clear();

                        foreach (DataRowView item in dvSorted)
                        {
                            dataTableCandidateCopy.ImportRow(item.Row);
                        }

                        //for (int i = 0; i < dvSorted.Count; i++)
                        //{
                        //    DataRow dr = dt.NewRow();
                        //    dr["TargetPersonID"] = dvSorted[i]["TargetPersonID"];
                        //    dr["ProjectID"] = dvSorted[i]["ProjectID"];
                        //    dr["AssignID"] = dvSorted[i]["AssignID"];
                        //    dr["Relationship"] = dvSorted[i]["Relationship"];
                        //    dr["Name"] = dvSorted[i]["Name"];
                        //    dr["EmailID"] = dvSorted[i]["EmailID"];
                        //    dr["AssignmentID"] = dvSorted[i]["AssignmentID"];
                        //    dr["SubmitFlag"] = dvSorted[i]["SubmitFlag"];
                        //    dr["EmailSendFlag"] = dvSorted[i]["EmailSendFlag"];

                        //    dt.Rows.Add(dr);
                        //}
                        dataTableCandidateCopy.AcceptChanges();
                    }
                }

                int addDeletedRowCount = removedRows - dtUSC.Rows.Count;

                if (addDeletedRowCount > 0)
                {
                    for (int i = 0; i < addDeletedRowCount; i++)
                    {
                        dataTableCandidateCopy.Rows.Add(dataTableCandidateCopy.NewRow());
                    }
                }
                else
                {
                    addDeletedRowCount = Convert.ToInt32(strColleagueNo) - dataTableCandidateCopy.Rows.Count;
                    if (addDeletedRowCount > 0)
                    {
                        for (int i = 0; i < addDeletedRowCount; i++)
                        {
                            dataTableCandidateCopy.Rows.Add(dataTableCandidateCopy.NewRow());
                        }
                    }
                }

                if (colleagueRecordCount > dataTableCandidateCopy.Rows.Count)
                {
                    //k
                    //addDeletedRowCount = iColleagueRecordCount - dt.Rows.Count;
                    //for (int i = 0; i < addDeletedRowCount; i++)
                    //{
                    //    dt.Rows.Add(dt.NewRow());

                    //}
                    dataTableCandidateCopy.Merge(dtUSC);
                    dataTableCandidateCopy.AcceptChanges();
                }
            }
            else
            {
                dataTableCandidateCopy.Merge(dtUSC);

                if (dtUSC.Rows.Count != removedRows)
                {
                    removedRows = (removedRows - dtUSC.Rows.Count);
                    if (isRowDeleted)
                    {
                        for (int i = 0; i < removedRows; i++)
                        {
                            dataTableCandidateCopy.Rows.Add(dataTableCandidateCopy.NewRow());
                        }
                        //K dt.AcceptChanges();
                    }
                }

                dataTableCandidateCopy.AcceptChanges();
            }
        }
        //Bind collrague grid view to new colleague table.
        rptrCandidateList.DataSource = dataTableCandidateCopy;
        rptrCandidateList.DataBind();

        //store table in session so that it can be rebind wtih this data again on page refersh.
        Session["ColleagueTable"] = dataTableCandidate;
    }

    /// <summary>
    /// Change to match managers
    /// </summary>
    /// <returns></returns>
    private int CheckManagerCount()
    {
        return (Session["ColleagueTable"] as DataTable).Select("RelationShip='Manager'").Count();

        //int managerCount = 0;

        //if (dtCandidateList != null)
        //{
        //    for (int i = 0; i < dtCandidateList.Rows.Count; i++)
        //    {
        //        if (dtCandidateList.Rows[i]["RelationShip"].ToString() == "Manager")
        //        {
        //            managerCount = managerCount + 1;
        //        }
        //    }

        //    List<AssignmentDetails_BE> lstCandidateList = new List<AssignmentDetails_BE>();
        //    lstCandidateList = GetCandidateList();

        //    foreach (AssignmentDetails_BE item in lstCandidateList)
        //    {
        //        if (item.RelationShip == "Manager")
        //        {
        //            managerCount = managerCount + 1;
        //        }
        //    }
        //}

        //return managerCount;
    }

    /// <summary>
    /// Remove after changes and full testing no use
    /// </summary>
    /// <returns></returns>
    private List<AssignmentDetails_BE> GetCandidateList()
    {
        List<AssignmentDetails_BE> assignmentDetailsBusinessEntityList = new List<AssignmentDetails_BE>();
        bool flag = true;

        foreach (RepeaterItem item in rptrCandidateList.Items)
        {
            DropDownList ddlRelationship = (DropDownList)item.FindControl("ddlRelationship");
            TextBox txtCandidateName = (TextBox)item.FindControl("txtName");
            TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");

            //if (ddlRelationship.SelectedValue == "0" || txtCandidateName.Text == "" || txtCandidateEmail.Text == "")
            //    flag = false;
        }

        if (flag != false)
        {
            foreach (RepeaterItem item in rptrCandidateList.Items)
            {
                AssignmentDetails_BE assignmentDetailsBusinessEntity = new AssignmentDetails_BE();

                DropDownList dropDownRelationship = (DropDownList)item.FindControl("ddlRelationship");
                TextBox textBoxCandidateName = (TextBox)item.FindControl("txtName");
                TextBox textBoxCandidateEmail = (TextBox)item.FindControl("txtEmailID");

                if (dropDownRelationship.SelectedValue != "0" && textBoxCandidateName.Text.Trim() != "" && textBoxCandidateEmail.Text.Trim() != "")
                {
                    if (dropDownRelationship.SelectedItem.Text.ToUpper() == "MANAGER")
                    {
                        if (this.isManager)
                            this.duplicateManager = true;

                        this.isManager = true;
                    }

                    assignmentDetailsBusinessEntity.RelationShip = dropDownRelationship.SelectedValue; //txtRelationship.Text.Trim();
                    assignmentDetailsBusinessEntity.CandidateName = textBoxCandidateName.Text.Trim();
                    assignmentDetailsBusinessEntity.CandidateEmail = textBoxCandidateEmail.Text.Trim();
                    assignmentDetailsBusinessEntity.SubmitFlag = false;

                    if (identity.User.GroupID == 1)
                        UserAccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    else
                        UserAccountID = Convert.ToInt32(identity.User.AccountID);

                    if (UserAccountID == Convert.ToInt32(ConfigurationManager.AppSettings["AccountID"].ToString()))
                        assignmentDetailsBusinessEntity.EmailSendFlag = 0;
                    else
                        assignmentDetailsBusinessEntity.EmailSendFlag = 1;

                    if (assignmentDetailsBusinessEntity.RelationShip != "" && assignmentDetailsBusinessEntity.CandidateName != "" && assignmentDetailsBusinessEntity.CandidateEmail != "")
                    {
                        assignmentDetailsBusinessEntityList.Add(assignmentDetailsBusinessEntity);
                        email += textBoxCandidateEmail.Text.Trim() + ";";
                    }
                }
            }

            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (identity.User.GroupID.ToString() != participantRoleId)
                targetpersonid = Convert.ToInt32(ddlTargetPerson.SelectedValue);
            else
                targetpersonid = Convert.ToInt32(identity.User.UserID);

            if (identity.User.GroupID == 1)
                UserAccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                UserAccountID = Convert.ToInt32(identity.User.AccountID);

            AccountUser_BAO user = new AccountUser_BAO();
            List<AccountUser_BE> Userlist = user.GetAccountUserByID(UserAccountID, targetpersonid);
            AssignmentDetails_BE assignmentDetails = new AssignmentDetails_BE();

            assignmentDetails.CandidateEmail = Userlist[0].EmailID;
            assignmentDetails.CandidateName = Userlist[0].FirstName + " " + Userlist[0].LastName;
            assignmentDetails.RelationShip = "Self";
            assignmentDetails.SubmitFlag = false;
            assignmentDetails.EmailSendFlag = 0;
            assignmentDetailsBusinessEntityList.Add(assignmentDetails);

            email += Userlist[0].EmailID + ";";
            finalemail = email.TrimEnd(';');
        }

        return assignmentDetailsBusinessEntityList;
    }

    /// <summary>
    /// Validatin for import error message
    /// </summary>
    /// <param name="filename"></param>
    private void errorMessage(string filename)
    {
        lblMessage.Text = "Upload Failed.Please fill the Correct Field Value";
        lblMessage2.Text = "Upload Failed.Please fill the Correct Field Value";
    }

    /// <summary>
    /// Common control to bind drop down list
    /// </summary>
    /// <param name="dropDownControl"></param>
    /// <param name="controlDataTable"></param>
    /// <param name="DataTextField"></param>
    /// <param name="DataValueField"></param>
    private void BindDropDownList(DropDownList dropDownControl, DataTable controlDataTable,
      string DataTextField, string DataValueField)
    {
        dropDownControl.Items.Clear();

        dropDownControl.DataSource = controlDataTable;
        dropDownControl.DataValueField = DataValueField;
        dropDownControl.DataTextField = DataTextField;
        dropDownControl.DataBind();

        dropDownControl.Items.Insert(0, new ListItem(DefaulText, DefaulValue));
    }

    /// <summary>
    /// common function to reset dropdown value.
    /// </summary>
    /// <param name="dropDownListControl"></param>
    private void ClearControl(DropDownList dropDownListControl)
    {
        dropDownListControl.Items.Clear();
        dropDownListControl.Items.Insert(0, new ListItem(DefaulText, DefaulValue));
    }

    /// <summary>
    /// Common function to Set company name.
    /// </summary>
    private void GetCompanyName()
    {
        Account_BAO accountBusinessAccessObject = new Account_BAO();
        int companycode = int.Parse(ddlAccountCode.SelectedValue);
        //Get company name
        DataTable DataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

        var companyName = (DataTableCompanyName.AsEnumerable()).
            Where(x => x.Field<int>("AccountID") == companycode).FirstOrDefault();
        //bind company name.
        lblcompanyname.Text = companyName.Field<string>("Organisationname");
    }

    /// <summary>
    /// common methos to bind all dropdowns
    /// </summary>
    /// <param name="groupID"></param>
    private void BindControls(int? groupID)
    {
        Account_BAO accountBusinessAccessObject = new Account_BAO();
        Project_BAO projectBusinessAccessObject = new Project_BAO();
        identity = this.Page.User.Identity as WADIdentity;

        //Bind account dropdown
        BindDropDownList(ddlAccountCode, accountBusinessAccessObject.GetdtAccountList((identity.User.AccountID.ToString())),
            AccountTextField, AccountValueField);
        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
        //set comapny name
        GetCompanyName();

        //If user is super admin show account dropdown else hide
        if (identity.User.GroupID == 1)
        {
            divAccount.Visible = true;
        }
        else
        {
            divAccount.Visible = false;
            lblcompanyname.Text = string.Empty;
        }
        //Bind project by  user account id
        BindDropDownList(ddlProject, projectBusinessAccessObject.GetdtProjectList((identity.User.AccountID.ToString())),
               ProjectTextField, ProjectValueField);
    }

    /// <summary>
    /// Rebind grid and remove data.
    /// </summary>
    private void ClearGridData()
    {
        DataTable emptyDataTable = new DataTable();
        emptyDataTable.Columns.Add("Relationship");
        emptyDataTable.Columns.Add("Name");
        emptyDataTable.Columns.Add("EmailID");

        rptrCandidateList.DataSource = emptyDataTable;
        rptrCandidateList.DataBind();
    }
    #endregion

    #region Public Methods
    public void RegisterPostbackTrigger(Control triggerOn)
    {
        ScriptManager1.RegisterPostBackControl(triggerOn);
    }

    /// <summary>
    /// Bind project by account value.
    /// </summary>
    public void SetValues()
    {
        identity = this.Page.User.Identity as WADIdentity;

        AssignQuestionnaire_BAO assignquestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
        DataTable dataTableAssignDetails = new DataTable();
        //Get all Participant by user ID.
        dataTableAssignDetails = assignquestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        //ddlProject.DataValueField = "ProjectID";
        //ddlProject.DataTextField = "Title";
        //ddlProject.DataBind();
        if (dataTableAssignDetails.Rows.Count > 0)
        {
            //bind project value.
            ddlProject.SelectedValue = dataTableAssignDetails.Rows[0]["ProjecctID"].ToString();
            hdnProjectId.Value = dataTableAssignDetails.Rows[0]["ProjecctID"].ToString();
        }
        Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

#if CommentOut
        //ddlQuestionnaire.Items.Clear();
        //DataTable dtQuestionnaire = new DataTable();
        //dtQuestionnaire = questionnaire_BAO.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        //if (dtQuestionnaire.Rows.Count > 0)
        //{
        //    ddlQuestionnaire.DataSource = dtQuestionnaire;
        //    ddlQuestionnaire.DataTextField = "QSTNName";
        //    ddlQuestionnaire.DataValueField = "QuestionnaireID";
        //    ddlQuestionnaire.DataBind();
        //    if (dtAssignDetails.Rows[0]["QuestionnaireID"]!=null)
        //    ddlQuestionnaire.SelectedValue = dtAssignDetails.Rows[0]["QuestionnaireID"].ToString();
        //}

        //ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
#endif

        //Set Programme
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //By project value get program data
        dataTableProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        ddlProject.Enabled = false;
        ddlProgramme.Enabled = false;

        if (dataTableProgramme.Rows.Count > 0)
        {
            //Bind program dropdown
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();

            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            if (dataTableAssignDetails.Rows[0]["ProgrammeID"] != null)
                ddlProgramme.SelectedValue = dataTableAssignDetails.Rows[0]["ProgrammeID"].ToString();
            //ddlProgramme.SelectedIndex = ddlProgramme.Items.IndexOf(ddlProgramme.Items.FindByValue(dtAssignDetails.Rows[0]["ProgrammeID"].ToString()));

            ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);
        }
        else
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));


        //ddlQuestionnaire.Enabled = false;

        //Set Relationship table
        DataTable dataTableRelationship = new DataTable();
        //get relation ship table details by project id.
        dataTableRelationship = projectBusinessAccessObject.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        Session["Relationship"] = dataTableRelationship;
    }

    /// <summary>
    /// Import colleague data by reading Excel
    /// </summary>
    /// <param name="FullFileNamePath"></param>
    /// <returns></returns>
    public DataTable ReturnExcelDataTableMot(string FullFileNamePath)
    {
        //DataTable dtExcel;
        DateTime dt3 = new DateTime();

        string SheetName = string.Empty;
        try
        {
            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;

            int index = 0;
            object rowIndex = 2;

            DataTable dtExcel = new DataTable();

            dtExcel.Columns.Add("Relationship", typeof(string));
            dtExcel.Columns.Add("Name", typeof(string));
            dtExcel.Columns.Add("EmailID", typeof(string));

            DataRow row;

            try
            {
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    //rowIndex = 2 + index;
                    row = dtExcel.NewRow();
                    DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

                    string projid = ddlProject.SelectedValue.ToString();

                    DataTable dataTableAllProject = new DataTable();
                    object[] param1 = new object[3] { projid, "2", 'P' };

                    dataTableAllProject = cDataSrc.ExecuteDataSet("UspProjectSelect", param1, null).Tables[0];

                    expression2 = "Relationship1='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                    Finalexpression2 = expression2;

                    DataRow[] results1 = dataTableAllProject.Select(Finalexpression2);

                    DataTable dataTableProject = dataTableAllProject.Clone();

                    foreach (DataRow resultdataRow in results1)
                    {
                        dataTableProject.ImportRow(resultdataRow);
                    }

                    if (dataTableProject.Rows.Count > 0 || dataTableProject == null)
                    {
                        string ProjectId = Convert.ToString(dataTableProject.Rows[0]["Relationship1"]);

                        row[0] = ProjectId;
                    }
                    else
                    {
                        expression2 = "Relationship2='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                        Finalexpression2 = expression2;

                        DataRow[] results2 = dataTableAllProject.Select(Finalexpression2);

                        DataTable dataTableProject1 = dataTableAllProject.Clone();

                        foreach (DataRow dataRow2 in results2)
                        {
                            dataTableProject1.ImportRow(dataRow2);
                        }

                        if (dataTableProject1.Rows.Count > 0 || dataTableProject1 == null)
                        {
                            ProjectId1 = Convert.ToString(dataTableProject1.Rows[0]["Relationship2"]);

                            row[0] = ProjectId1;
                        }
                        else
                        {
                            expression2 = "Relationship3='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                            Finalexpression2 = expression2;

                            DataRow[] results3 = dataTableAllProject.Select(Finalexpression2);

                            DataTable dataTableProject2 = dataTableAllProject.Clone();

                            foreach (DataRow datarow3 in results3)
                            {
                                dataTableProject2.ImportRow(datarow3);
                            }
                            if (dataTableProject2.Rows.Count > 0 || dataTableProject2 == null)
                            {
                                ProjectId2 = Convert.ToString(dataTableProject2.Rows[0]["Relationship3"]);

                                row[0] = ProjectId2;
                            }
                            else
                            {
                                expression2 = "Relationship4='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                                Finalexpression2 = expression2;

                                DataRow[] results4 = dataTableAllProject.Select(Finalexpression2);

                                DataTable dataTableProject4 = dataTableAllProject.Clone();

                                foreach (DataRow datarow4 in results4)
                                {
                                    dataTableProject4.ImportRow(datarow4);
                                }
                                if (dataTableProject4.Rows.Count > 0 || dataTableProject4 == null)
                                {
                                    ProjectId3 = Convert.ToString(dataTableProject4.Rows[0]["Relationship4"]);

                                    row[0] = ProjectId3;
                                }
                                else
                                {
                                    expression2 = "Relationship5='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                                    Finalexpression2 = expression2;

                                    DataRow[] results5 = dataTableAllProject.Select(Finalexpression2);

                                    DataTable dataTableProject5 = dataTableAllProject.Clone();

                                    foreach (DataRow datarow5 in results5)
                                    {
                                        dataTableProject5.ImportRow(datarow5);
                                    }

                                    if (dataTableProject5.Rows.Count > 0 || dataTableProject5 == null)
                                    {
                                        ProjectId5 = Convert.ToString(dataTableProject5.Rows[0]["Relationship5"]);

                                        row[0] = ProjectId5;
                                    }
                                }
                            }
                        }
                    }

                    //row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                    row[2] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);

                    index++;
                    rowIndex = 2 + index;
                    dtExcel.Rows.Add(row);
                }
            }
            catch
            {
                lblMessage.Text = "Please check your file data.";
                lblMessage2.Text = "Please check your file data.";

                dtExcel = null;
            }
            app.Workbooks.Close();

            return dtExcel;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Get unique file name for downloaded pdf.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniquefilename;
    }
    #endregion
}
