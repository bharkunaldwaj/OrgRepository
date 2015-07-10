using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using DatabaseAccessUtilities;
using System.Diagnostics;
using Miscellaneous;
using DAF_BAO;
using Admin_BE;
using System.Net.Mail;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;


public partial class Module_Questionnaire_AssignQuestionnaire : CodeBehindBase
{
    int i;
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string strName = string.Empty;
    bool flag = true;
    int j;
    string file1;
    string filename1;
    string expression1;
    string expression2;
    string Finalexpression;
    string Finalexpression2;
    string expression6;
    string Finalexpression6;
    string email;
    string Template;
    string finalemail;
    string Questionnaire_id;
    string mailid;
    WADIdentity identity;
    DataTable CompanyName;
    DataTable dtAllAccount;
    string expression5;
    string Finalexpression5;
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
    int iColleagueRecordCount = 0;

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        Session["FeedbackType"] = "FeedBack360";
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        identity = this.Page.User.Identity as WADIdentity;
        int? grpID = identity.User.GroupID;


        //AssignQuestionnaire_BAO chk_user = new AssignQuestionnaire_BAO();
        //DataTable ddd = chk_user.chk_user_authority(grpID, 12);
        //if (Convert.ToInt32(ddd.Rows[0][0]) == 0)
        //{
        //    Response.Redirect("UnAuthorized.aspx");
        //}




        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";


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
            DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(userid);
            Project_BAO project_BAO = new Project_BAO();

            //DataTable dtCandidate = new DataTable();
            //dtCandidate.Columns.Add("Relationship");
            //dtCandidate.Columns.Add("Name");
            //dtCandidate.Columns.Add("EmailID");

            //ViewState["ColleagueTable"] = dtCandidate;

            Account_BAO account_BAO = new Account_BAO();
            ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            if (identity.User.GroupID == 1)
            {
                divAccount.Visible = true;
                ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                ddlAccountCode_SelectedIndexChanged(sender, e);
            }
            else
            {
                divAccount.Visible = false;
                ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
            }

            if (identity.User.GroupID.ToString() != participantRoleId)
            {
                ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();

                tblParticipantUpload.Visible = true;

                ddlProject.Enabled = true;
                ddlProgramme.Enabled = true;
                lblMandatory.Visible = true;
                ibtnHelp.Visible = false;
                //ddlQuestionnaire.Enabled = true;
            }
            else
            {
                tblParticipantUpload.Visible = false;
                lblMandatory.Visible = false;
                ibtnHelp.Visible = true;
                SetValues();

                trTargetPerson.Visible = false;

                AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                System.Web.UI.WebControls.Image imgHeader = (System.Web.UI.WebControls.Image)Master.FindControl("imgProjectLogo");
                DataTable dtParticipantInfo = new DataTable();
                dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                if (dtParticipantInfo.Rows.Count > 0)
                {
                    string programmeId = dtParticipantInfo.Rows[0]["ProgrammeID"].ToString();
                    dtCandidateList = assignQuestionnaire_BAO.GetdtAssignList(identity.User.UserID.ToString(), programmeId);


                    //Set Project Logo
                    if (dtParticipantInfo.Rows[0]["Logo"].ToString() != "")
                    {
                        imgHeader.Visible = true;
                        imgHeader.ImageUrl = "~/UploadDocs/" + dtParticipantInfo.Rows[0]["Logo"].ToString();
                    }
                }
            }
        }
    }

    public void RegisterPostbackTrigger(Control triggerOn)
    {
        ScriptManager1.RegisterPostBackControl(triggerOn);
    }

    public void SetValues()
    {
        identity = this.Page.User.Identity as WADIdentity;

        AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();
        DataTable dtAssignDetails = new DataTable();
        dtAssignDetails = assignquestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Project_BAO project_BAO = new Project_BAO();
        ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();
        if (dtAssignDetails.Rows.Count > 0)
        {
            ddlProject.SelectedValue = dtAssignDetails.Rows[0]["ProjecctID"].ToString();
            hdnProjectId.Value = dtAssignDetails.Rows[0]["ProjecctID"].ToString();
        }

        //ddlProject.SelectedIndex = 1;

        Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

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

        //Set Programme
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();

            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            if (dtAssignDetails.Rows[0]["ProgrammeID"] != null)
                ddlProgramme.SelectedValue = dtAssignDetails.Rows[0]["ProgrammeID"].ToString();
            //ddlProgramme.SelectedIndex = ddlProgramme.Items.IndexOf(ddlProgramme.Items.FindByValue(dtAssignDetails.Rows[0]["ProgrammeID"].ToString()));

            ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);
        }
        else
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ddlProject.Enabled = false;
        ddlProgramme.Enabled = false;
        //ddlQuestionnaire.Enabled = false;

        //Set Relationship
        DataTable dtRelationship = new DataTable();

        dtRelationship = project_BAO.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        Session["Relationship"] = dtRelationship;
    }

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
    /// Remove after changes and full testing   
    /// </summary>
    /// <returns></returns>
    private List<AssignmentDetails_BE> GetCandidateList()
    {
        List<AssignmentDetails_BE> assignmentDetails_BEList = new List<AssignmentDetails_BE>();
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
                AssignmentDetails_BE assignmentDetails_BE = new AssignmentDetails_BE();

                DropDownList ddlRelationship = (DropDownList)item.FindControl("ddlRelationship");
                TextBox txtCandidateName = (TextBox)item.FindControl("txtName");
                TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");
                if (ddlRelationship.SelectedValue != "0" && txtCandidateName.Text.Trim() != "" && txtCandidateEmail.Text.Trim() != "")
                {
                    if (ddlRelationship.SelectedItem.Text.ToUpper() == "MANAGER")
                    {
                        if (this.isManager)
                            this.duplicateManager = true;

                        this.isManager = true;
                    }

                    assignmentDetails_BE.RelationShip = ddlRelationship.SelectedValue; //txtRelationship.Text.Trim();
                    assignmentDetails_BE.CandidateName = txtCandidateName.Text.Trim();
                    assignmentDetails_BE.CandidateEmail = txtCandidateEmail.Text.Trim();
                    assignmentDetails_BE.SubmitFlag = false;

                    if (identity.User.GroupID == 1)
                        UserAccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    else
                        UserAccountID = Convert.ToInt32(identity.User.AccountID);

                    if (UserAccountID == Convert.ToInt32(ConfigurationManager.AppSettings["AccountID"].ToString()))
                        assignmentDetails_BE.EmailSendFlag = 0;
                    else
                        assignmentDetails_BE.EmailSendFlag = 1;

                    if (assignmentDetails_BE.RelationShip != "" && assignmentDetails_BE.CandidateName != "" && assignmentDetails_BE.CandidateEmail != "")
                    {
                        assignmentDetails_BEList.Add(assignmentDetails_BE);
                        email += txtCandidateEmail.Text.Trim() + ";";
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
            assignmentDetails_BEList.Add(assignmentDetails);

            email += Userlist[0].EmailID + ";";
            finalemail = email.TrimEnd(';');

        }

        return assignmentDetails_BEList;
    }

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

    protected void imbSaveColleague_Click(object sender, ImageClickEventArgs e)
    {

    }

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

    private void BindCandidateList(int candidateCount)
    {
        try
        {
            //DataTable dtCandidate = new DataTable();
            //dtCandidate.Columns.Add("Relationship");
            //dtCandidate.Columns.Add("Name");
            //dtCandidate.Columns.Add("EmailID");

            if (Session["ColleagueTable"] != null)
            {

                DataTable dtCandidate = Session["ColleagueTable"] as DataTable;

                for (int count = 0; count < candidateCount; count++)
                {
                    DataRow dr = dtCandidate.NewRow();
                    dtCandidate.Rows.Add(dr);
                }


                BindColleagueRepeter(dtCandidate);
            }



        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void BindColleagueRepeter(DataTable dtCandidate)
    {
        DataTable dt = dtCandidate.Copy();


        DataTable dtUSC = Session["UnsavedColleagueTable"] as DataTable;
        //DataRow[] dr = dt.Select("Relationship='' AND Name='' AND EmailID=''"); 

        if (Session["UnsavedColleagueTable"] != null)
        {
            bool isRowDeleted = false;
            int removedRows = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((dt.Rows[i]["Relationship"] == DBNull.Value || string.IsNullOrEmpty(dt.Rows[i]["Relationship"].ToString())) && (dt.Rows[i]["Name"] == DBNull.Value || string.IsNullOrEmpty(dt.Rows[i]["Name"].ToString())) && (dt.Rows[i]["EmailID"] == DBNull.Value || string.IsNullOrEmpty(dt.Rows[i]["EmailID"].ToString())))
                {
                    dt.Rows[i].Delete();
                    removedRows++;
                    i--;
                    isRowDeleted = true;
                }
            }

            if (Session["ColleaguesIndex"] != null)
            {
                string[] strColleaguesIndex = null;
                if (!string.IsNullOrEmpty(Session["ColleaguesIndex"].ToString()))
                    strColleaguesIndex = Session["ColleaguesIndex"].ToString().TrimEnd(',').Split(',');

                if (strColleaguesIndex == null || dt.Rows.Count == strColleaguesIndex.Count())
                {
                    DataTable dtOrderedColleagues = dt.Copy();
                    dtOrderedColleagues.Clear();
                    dtOrderedColleagues.Columns.Add(new DataColumn("ID", typeof(int)));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dtOrderedColleagues.NewRow();
                        dr["ID"] = Convert.ToInt32(strColleaguesIndex[i]);
                        dr["TargetPersonID"] = dt.Rows[i]["TargetPersonID"].ToString();
                        dr["ProjectID"] = dt.Rows[i]["ProjectID"];
                        dr["AssignID"] = dt.Rows[i]["AssignID"];
                        dr["Relationship"] = dt.Rows[i]["Relationship"];
                        dr["Name"] = dt.Rows[i]["Name"];
                        dr["EmailID"] = dt.Rows[i]["EmailID"];
                        dr["AssignmentID"] = dt.Rows[i]["AssignmentID"];
                        dr["SubmitFlag"] = dt.Rows[i]["SubmitFlag"];
                        dr["EmailSendFlag"] = dt.Rows[i]["EmailSendFlag"];

                        dtOrderedColleagues.Rows.Add(dr);
                    }
                    dtOrderedColleagues.AcceptChanges();

                    int totalRecords = dt.Rows.Count + dtUSC.Rows.Count;

                    if (!dtUSC.Columns.Contains("ID"))
                        dtUSC.Columns.Add("ID", typeof(int));
                    int k = 0;

                    for (int i = 0; i < dtUSC.Rows.Count; i++)
                    {
                        if (k <= totalRecords)
                        {
                            if (strColleaguesIndex != null)
                            {
                                for (int j = 0; j < strColleaguesIndex.Count(); j++)
                                {
                                    if (k.ToString() == strColleaguesIndex[j].ToString())
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

                    dtOrderedColleagues.Merge(dtUSC);
                    dtOrderedColleagues.DefaultView.Sort = "ID";
                    DataView dvSorted = dtOrderedColleagues.DefaultView;

                    if (dvSorted.Table.Rows.Count > 0)
                    {
                        dt.Clear();

                        foreach (DataRowView item in dvSorted)
                        {
                            dt.ImportRow(item.Row);
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
                        dt.AcceptChanges();
                    }
                }

                int addDeletedRowCount = removedRows - dtUSC.Rows.Count;

                if (addDeletedRowCount > 0)
                {
                    for (int i = 0; i < addDeletedRowCount; i++)
                    {
                        dt.Rows.Add(dt.NewRow());
                    }
                }
                else
                {
                    addDeletedRowCount = Convert.ToInt32(strColleagueNo) - dt.Rows.Count;
                    if (addDeletedRowCount > 0)
                    {
                        for (int i = 0; i < addDeletedRowCount; i++)
                        {
                            dt.Rows.Add(dt.NewRow());
                        }
                    }
                }

                if (iColleagueRecordCount > dt.Rows.Count)
                {
                    addDeletedRowCount = iColleagueRecordCount - dt.Rows.Count;
                    for (int i = 0; i < addDeletedRowCount; i++)
                    {
                        dt.Rows.Add(dt.NewRow());
                    }
                }
            }
            else
            {
                dt.Merge(dtUSC);

                if (dtUSC.Rows.Count != removedRows)
                {
                    removedRows = (removedRows - dtUSC.Rows.Count);
                    if (isRowDeleted)
                    {
                        for (int i = 0; i < removedRows; i++)
                        {
                            dt.Rows.Add(dt.NewRow());
                        }
                        dt.AcceptChanges();
                    }
                }
            }
        }

        rptrCandidateList.DataSource = dt;
        rptrCandidateList.DataBind();

        Session["ColleagueTable"] = dtCandidate;
    }

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
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        //if (ddlProgramme.Items.Count > 1)
        //    ddlProgramme.Items[1].Selected = true;

        //Set Relationship
        Project_BAO project_BAO = new Project_BAO();
        DataTable dtRelationship = new DataTable();

        dtRelationship = project_BAO.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        Session["Relationship"] = dtRelationship;

        ddlTargetPerson.Items.Clear();
        ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
    }

    protected void ImgUpload_click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string constr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection scon = new SqlConnection(constr);
            lblvalidation.Text = "";
            if (FileUpload1.HasFile)
            {
                if (this.IsFileValid(this.FileUpload1))
                {

                    string filename = "";
                    string file = "";

                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);



                    file = GetUniqueFilename(filename);

                    Session["FinalName"] = file;

                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename);


                    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();

                    DataTable dt;
                    if (Session["ColleagueTable"] == null)
                        dt = assignQuestionnaire_BAO.GetdtAssignColleagueList((User.Identity as WADIdentity).User.UserID.ToString(), "-1");
                    else
                        dt = Session["ColleagueTable"] as DataTable;



                    DataTable dtProspective = new DataTable();

                    dtProspective = ReturnExcelDataTableMot(filename);

                    for (int i = 0; i < dtProspective.Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();

                        dr["Relationship"] = dtProspective.Rows[i]["Relationship"];
                        dr["Name"] = dtProspective.Rows[i]["Name"];
                        dr["EmailID"] = dtProspective.Rows[i]["EmailID"];

                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }

                    BindColleagueRepeter(dt);
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

    public System.Data.DataTable ReturnExcelDataTableMot(string FullFileNamePath)
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

                    DataTable dtAllProject = new DataTable();
                    object[] param1 = new object[3] { projid, "2", 'P' };



                    dtAllProject = cDataSrc.ExecuteDataSet("UspProjectSelect", param1, null).Tables[0];

                    expression2 = "Relationship1='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                    Finalexpression2 = expression2;

                    DataRow[] results1 = dtAllProject.Select(Finalexpression2);



                    DataTable dtProject = dtAllProject.Clone();


                    foreach (DataRow dr1 in results1)
                    {
                        dtProject.ImportRow(dr1);
                    }

                    if (dtProject.Rows.Count > 0 || dtProject == null)
                    {

                        string ProjectId = Convert.ToString(dtProject.Rows[0]["Relationship1"]);



                        row[0] = ProjectId;


                    }
                    else
                    {


                        expression2 = "Relationship2='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                        Finalexpression2 = expression2;

                        DataRow[] results2 = dtAllProject.Select(Finalexpression2);


                        DataTable dtProject1 = dtAllProject.Clone();



                        foreach (DataRow dr2 in results2)
                        {
                            dtProject1.ImportRow(dr2);
                        }

                        if (dtProject1.Rows.Count > 0 || dtProject1 == null)
                        {
                            ProjectId1 = Convert.ToString(dtProject1.Rows[0]["Relationship2"]);




                            row[0] = ProjectId1;

                        }
                        else
                        {
                            expression2 = "Relationship3='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                            Finalexpression2 = expression2;

                            DataRow[] results3 = dtAllProject.Select(Finalexpression2);



                            DataTable dtProject2 = dtAllProject.Clone();


                            foreach (DataRow dr3 in results3)
                            {
                                dtProject2.ImportRow(dr3);
                            }
                            if (dtProject2.Rows.Count > 0 || dtProject2 == null)
                            {
                                ProjectId2 = Convert.ToString(dtProject2.Rows[0]["Relationship3"]);





                                row[0] = ProjectId2;

                            }
                            else
                            {
                                expression2 = "Relationship4='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                                Finalexpression2 = expression2;

                                DataRow[] results4 = dtAllProject.Select(Finalexpression2);



                                DataTable dtProject4 = dtAllProject.Clone();



                                foreach (DataRow dr4 in results4)
                                {
                                    dtProject4.ImportRow(dr4);
                                }
                                if (dtProject4.Rows.Count > 0 || dtProject4 == null)
                                {
                                    ProjectId3 = Convert.ToString(dtProject4.Rows[0]["Relationship4"]);



                                    row[0] = ProjectId3;
                                }
                                else
                                {
                                    expression2 = "Relationship5='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2) + "'";

                                    Finalexpression2 = expression2;

                                    DataRow[] results5 = dtAllProject.Select(Finalexpression2);



                                    DataTable dtProject5 = dtAllProject.Clone();


                                    foreach (DataRow dr5 in results5)
                                    {
                                        dtProject5.ImportRow(dr5);
                                    }
                                    if (dtProject5.Rows.Count > 0 || dtProject5 == null)
                                    {
                                        ProjectId5 = Convert.ToString(dtProject5.Rows[0]["Relationship5"]);



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

    private void errorMessage(string filename)
    {

        lblMessage.Text = "Upload Failed.Please fill the Correct Field Value";
        lblMessage2.Text = "Upload Failed.Please fill the Correct Field Value";


    }

    protected void lnkError_Click(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Session["FinalName"].ToString() + ".txt");
        Response.WriteFile(Server.MapPath("~") + "//UploadDocs//" + Session["FinalName"].ToString() + ".txt");
        Response.End();

    }

    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniquefilename;
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {

            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO account_BAO = new Account_BAO();

            CompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            DataTable dtAccount = CompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
            {
                dtAccount.ImportRow(drAccount);
            }

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            //ddlTargetPerson.Items.Clear();
            //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));

            //AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
            //DataTable dtParticipant = new DataTable();

            //dtParticipant = accountUser_BAO.GetParticipantList(Convert.ToString(companycode));
            //if (dtParticipant.Rows.Count > 0)
            //{
            //    ddlTargetPerson.DataSource = dtParticipant;
            //    ddlTargetPerson.DataValueField = "UserID";
            //    ddlTargetPerson.DataTextField = "UserName";
            //    ddlTargetPerson.DataBind();
            //}

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Project_BAO project_BAO = new Project_BAO();
            ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            //ddlQuestionnaire.Items.Clear();
            //ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            lblcompanyname.Text = "";

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            //ddlQuestionnaire.Items.Clear();
            //ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTargetPerson.SelectedIndex > 0)
        {

            Programme_BAO programme_BAO = new Programme_BAO();
            strColleagueNo = programme_BAO.GetProgramColleagueNumber(Convert.ToInt32(ddlProgramme.SelectedValue));

            AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();

            DataTable dt = assignQuestionnaire_BAO.GetdtAssignColleagueList(ddlTargetPerson.SelectedValue, ddlProgramme.SelectedValue);

            int colleagueCount = dt.Rows.Count;

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
                if (dt != null)
                {
                    if (dt.Rows.Count < colleagueNo)
                    {
                        int maxNo = (colleagueNo - dt.Rows.Count);
                        for (int i = 0; i < maxNo; i++)
                        {
                            DataRow row = dt.NewRow();
                            dt.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("Relationship");
                    dt.Columns.Add("Name");
                    dt.Columns.Add("EmailID");

                    for (int i = 0; i < colleagueNo; i++)
                        dt.Rows.Add("", "", "");

                }

                BindColleagueRepeter(dt);


                DataTable dtResult = new DataTable();
                dtResult = assignQuestionnaire_BAO.GetFeedbackURL(Convert.ToInt32(ddlTargetPerson.SelectedValue));

                if (dtResult.Rows.Count > 0)
                {
                    string url = dtResult.Rows[0]["FeedbackUrl"].ToString();

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

    protected void rptrCandidateList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem rpItem = e.Item;
        DropDownList ddlRelationship = (DropDownList)rpItem.FindControl("ddlRelationship");
        TextBox txtName = (TextBox)rpItem.FindControl("txtName");
        TextBox txtEmailID = (TextBox)rpItem.FindControl("txtEmailID");
        RequiredFieldValidator rqfRelationShip = (RequiredFieldValidator)rpItem.FindControl("rqfRelationShip");
        RequiredFieldValidator rfqTxtName = (RequiredFieldValidator)rpItem.FindControl("rfqTxtName");
        RequiredFieldValidator rfqTxtEmailID = (RequiredFieldValidator)rpItem.FindControl("rfqTxtEmailID");
        ImageButton imbSaveColleague = (ImageButton)rpItem.FindControl("imbSaveColleague");
        ImageButton imbSaveOnlyColleague = (ImageButton)rpItem.FindControl("imbSaveOnlyColleague");


        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            ddlRelationship.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            txtName.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            txtEmailID.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            rqfRelationShip.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            rfqTxtName.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            rfqTxtEmailID.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            imbSaveColleague.ValidationGroup = "VGroup" + e.Item.ItemIndex;
            imbSaveOnlyColleague.ValidationGroup = "VGroup" + e.Item.ItemIndex;
        }
        if (ddlRelationship != null)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Relationship"];

            if (dt == null || dt.Rows.Count < 1)
            {
                Project_BAO project_BAO = new Project_BAO();
                dt = project_BAO.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
                Session["Relationship"] = dt;
            }

            ddlRelationship.Items.Clear();
            ddlRelationship.Items.Insert(0, new ListItem("Select", "0"));

            ddlRelationship.DataSource = dt;
            ddlRelationship.DataValueField = "value";
            ddlRelationship.DataTextField = "value";
            ddlRelationship.DataBind();

            HiddenField hddRelationship = (HiddenField)rpItem.FindControl("hddRelationShip");

            if (!string.IsNullOrEmpty(hddRelationship.Value))
                ddlRelationship.SelectedValue = hddRelationship.Value;
        }

        if (ddlRelationship != null && Session["Relation"] != null)
        {
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)Session["Relation"];

            if (e.Item.ItemIndex < dt1.Rows.Count)
                ddlRelationship.SelectedValue = Convert.ToString(dt1.Rows[e.Item.ItemIndex]["Relationship"]);

        }

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //TextBox tb = rpItem.FindControl("txtEmailID") as TextBox;
            Label lblTargetPersonID = (Label)e.Item.FindControl("lblTargetPersonID");
            //Label lblAssignmentDetailsID = (Label)e.Item.FindControl("lblAssignmentID");

            if (string.IsNullOrEmpty(lblTargetPersonID.Text))
            {
                (rpItem.FindControl("imgColleagueSaved") as System.Web.UI.WebControls.Image).Visible = false;
                (rpItem.FindControl("imbSaveColleague") as ImageButton).Visible = true;
            }
            else
            {
                (rpItem.FindControl("imgColleagueSaved") as System.Web.UI.WebControls.Image).Visible = true;
                (rpItem.FindControl("imbSaveColleague") as ImageButton).Visible = false;
            }

            int userID = Convert.ToInt32((User.Identity as WADIdentity).User.UserID);
            if (userID == 3 && !string.IsNullOrEmpty(lblTargetPersonID.Text))
            {
                (rpItem.FindControl("imbSaveOnlyColleague") as ImageButton).Visible = true;
            }
            else
            {
                (rpItem.FindControl("imbSaveOnlyColleague") as ImageButton).Visible = false;
            }

            //Label lbl = rpItem.FindControl("lblSubmitStatus") as Label;

            //if (lbl.Text.ToLower() == "true")
            //    lbl.Text = "Yes";
            //else
            //    lbl.Text = "No";

            DataTable dtQuestion = new DataTable();
            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

            int QuestionnaireID = questionnaire_BAO.GetQuestionnaireID(ddlProject.SelectedValue);

            Label lblAssignmentID = rpItem.FindControl("lblAssignmentID") as Label;
            Label lblCompletion = rpItem.FindControl("lblCompletion") as Label;

            if (!string.IsNullOrEmpty(lblAssignmentID.Text))
            {

                int answeredQuestion = questionnaire_BAO.CalculateGraph(QuestionnaireID, Convert.ToInt32(lblAssignmentID.Text));

                //dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaire(Convert.ToInt32(lblQuestionnaireId.Text));
                dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaireByRelationShip(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue), QuestionnaireID, ddlRelationship.SelectedValue);

                double percentage = (answeredQuestion * 100) / Convert.ToInt32(dtQuestion.Rows.Count);
                string[] percent = percentage.ToString().Split('.');


                //percentage = percent[0];
                if (percent[0].ToString() == "100")
                    lblCompletion.Text = "Yes";
                else
                    lblCompletion.Text = percent[0].ToString() + "%";
            }
            else
                lblCompletion.Text = "0%";

            ImageButton imgbtn = rpItem.FindControl("imbDeleteColleague") as ImageButton;

            if (imgbtn != null)
            {
                TextBox txtBox = rpItem.FindControl("txtName") as TextBox;

                imgbtn.Attributes.Add("onclick", "javascript:return DeleteConfirmation('" + txtBox.ClientID + "');");
            }


        }


    }

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

        if (ddlProject.SelectedValue == "0" || (ddlTargetPerson.Visible == true && ddlTargetPerson.SelectedValue == "0"))
        {

            lblMessage.Text = "Select Project & Target Person";
            lblMessage2.Text = "Select Project & Target Person";
        }
        else
        {
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

                DropDownList ddlRelationship;
                TextBox txtCandidateName;
                TextBox txtCandidateEmail;

                int i = e.Item.ItemIndex;

                if (Session["ColleaguesIndex"] != null)
                    Session["ColleaguesIndex"] = Session["ColleaguesIndex"].ToString().TrimStart(',') + i.ToString() + ",";
                else
                    Session["ColleaguesIndex"] = i.ToString() + ",";

                if (i < rptrCandidateList.Items.Count)
                {
                    DataTable dtCandidateClone;

                    DataTable dtCandidate = Session["ColleagueTable"] as DataTable;
                    dtCandidateClone = dtCandidate.Clone();

                    dtCandidateClone.Clear();

                    iColleagueRecordCount = rptrCandidateList.Items.Count;

                    for (int j = 0; j < iColleagueRecordCount; j++)
                    {
                        if (j != i)
                        {
                            ddlRelationship = (DropDownList)rptrCandidateList.Items[j].FindControl("ddlRelationship");
                            txtCandidateName = (TextBox)rptrCandidateList.Items[j].FindControl("txtName");
                            txtCandidateEmail = (TextBox)rptrCandidateList.Items[j].FindControl("txtEmailID");
                            Label lblTargetPersonID = (Label)rptrCandidateList.Items[j].FindControl("lblTargetPersonID");

                            if (string.IsNullOrEmpty(lblTargetPersonID.Text))
                            {
                                DataRow dr = dtCandidateClone.NewRow();
                                dr["Relationship"] = ddlRelationship.SelectedItem.Text;
                                dr["Name"] = txtCandidateName.Text;
                                dr["EmailID"] = txtCandidateEmail.Text;
                                dtCandidateClone.Rows.Add(dr);
                            }
                        }
                    }

                    dtCandidateClone.AcceptChanges();
                    Session["UnsavedColleagueTable"] = dtCandidateClone;
                }

                ddlRelationship = (DropDownList)e.Item.FindControl("ddlRelationship");
                txtCandidateName = (TextBox)e.Item.FindControl("txtName");
                txtCandidateEmail = (TextBox)e.Item.FindControl("txtEmailID");

                SaveCandidate(ddlRelationship.SelectedValue, ddlRelationship.SelectedItem.Text, txtCandidateName.Text, txtCandidateEmail.Text);

                WADIdentity uIdentity = this.Page.User.Identity as WADIdentity;
                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                if (uIdentity.User.GroupID.ToString() != participantRoleId)
                    ddlTargetPerson_SelectedIndexChanged(this, EventArgs.Empty);
                else
                    ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);

            }
            else if (e.CommandName.ToLower() == "delete")
            {

                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    AssignQuestionnaire_BE assignQuestionnaire_BE = new AssignQuestionnaire_BE();
                    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();

                    assignQuestionnaire_BE.AssignmentID = Convert.ToInt32(e.CommandArgument);
                    assignQuestionnaire_BAO.DeleteAssignQuestionnaire(assignQuestionnaire_BE);
                    iColleagueRecordCount = rptrCandidateList.Items.Count;
                    string[] strCIndex = Session["ColleaguesIndex"].ToString().TrimEnd(',').Split(',');
                    if (strCIndex.Length > 0)
                    {
                        string strNewCIndex = string.Empty;
                        for (int i = 0; i < strCIndex.Length; i++)
                        {
                            if (strCIndex[i].ToString() != e.Item.ItemIndex.ToString())
                            {
                                if (!string.IsNullOrEmpty(strCIndex[i]))
                                {
                                    if (e.Item.ItemIndex < Convert.ToInt32(strCIndex[i]))
                                        strNewCIndex += (Convert.ToInt32(strCIndex[i]) - 1).ToString() + ",";
                                    else
                                        strNewCIndex += strCIndex[i] + ",";
                                }
                            }
                        }
                        Session["ColleaguesIndex"] = strNewCIndex;
                    }

                    ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
            else if (e.CommandName.ToLower() == "save" || e.CommandName.ToLower() == "sendemail")
            {
                if (Page.IsValid)
                {
                    Page.Validate("VGroupX");
                    if (!Page.IsValid)
                    {
                        return;
                    }
                }
                DropDownList ddlRelationship = (DropDownList)e.Item.FindControl("ddlRelationship");
                Label lblAssignmentID = (Label)e.Item.FindControl("lblAssignID");
                Label lblAccountID = (Label)e.Item.FindControl("lblAccountID");
                Label lblTargetPersonID = (Label)e.Item.FindControl("lblTargetPersonID");
                Label lblAssignmentDetailsID = (Label)e.Item.FindControl("lblAssignmentID");
                Label lblProjectID = (Label)e.Item.FindControl("lblProjectID");
                TextBox txtCandidateName = (TextBox)e.Item.FindControl("txtName");
                TextBox txtCandidateEmail = (TextBox)e.Item.FindControl("txtEmailID");
                string relationship = ddlRelationship.SelectedItem.Text;
                if (!string.IsNullOrEmpty(lblAssignmentID.Text) && !string.IsNullOrEmpty(lblAccountID.Text) && !string.IsNullOrEmpty(lblTargetPersonID.Text)
                    && !string.IsNullOrEmpty(lblAssignmentDetailsID.Text) && !string.IsNullOrEmpty(lblProjectID.Text))
                {
                    int assignmentID = Convert.ToInt32(lblAssignmentID.Text);
                    int accountID = Convert.ToInt32(lblAccountID.Text);
                    int targetPersonID = Convert.ToInt32(lblTargetPersonID.Text);
                    int assignmentDetailsID = Convert.ToInt32(lblAssignmentDetailsID.Text);
                    int projectID = Convert.ToInt32(lblProjectID.Text);
                    if (e.CommandName.ToLower() == "save")
                        ReSendColleagueEmail(assignmentID, accountID, targetPersonID, assignmentDetailsID, projectID, txtCandidateName.Text, txtCandidateEmail.Text, relationship, false);
                    else if (e.CommandName.ToLower() == "sendemail")
                        ReSendColleagueEmail(assignmentID, accountID, targetPersonID, assignmentDetailsID, projectID, txtCandidateName.Text, txtCandidateEmail.Text, relationship, true);
                }
            }

        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void ReSendColleagueEmail(int assignmentID, int accountID, int targetPersonID, int assignmentDetailsID, int projectID, string candidateName, string colleagueEmail, string relationship, bool sendEmail)
    {
        //Send Email to Candidate

        AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();
        DataTable dtResult = new DataTable();
        dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireList(assignmentID);

        DataTable dtClone = new DataTable();
        dtClone = dtResult.Clone();

        DataRow[] result = dtResult.Select("AsgnDetailID=" + assignmentDetailsID);

        foreach (DataRow dr in result)
            dtClone.ImportRow(dr);

        dtResult = dtClone;

        if (result.Count() > 0)
        {
            if (dtResult.Rows[0]["CandidateEmail"].ToString().ToLower() != colleagueEmail.ToLower() || dtResult.Rows[0]["CandidateName"].ToString().ToLower() != candidateName.ToLower() || dtResult.Rows[0]["Relationship"].ToString().ToLower() != relationship.ToLower())
            {
                assignquestionnaire_BAO.UpdateCandidateEmail(assignmentDetailsID, colleagueEmail, candidateName, relationship);
                dtResult.Rows[0]["CandidateEmail"] = colleagueEmail;
                dtResult.AcceptChanges();
            }
        }

        string imagepath = Server.MapPath("~/EmailImages/");

        //Send mail to candidates     
        if (sendEmail)
        {
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                DataTable dtAccountAdmin = new DataTable();
                dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(accountID, targetPersonID);

                string Template = assignquestionnaire_BAO.FindTemplate(Convert.ToInt32(projectID));
                string Subject = assignquestionnaire_BAO.FindCandidateSubjectTemplate(Convert.ToInt32(projectID));

                // Get Candidate Email Image Name & Will Combined with EmailImagePath
                DataTable dtCandidateEmailImage = new DataTable();
                string emailimagepath = "";
                dtCandidateEmailImage = assignquestionnaire_BAO.GetCandidateEmailImageInfo(Convert.ToInt32(projectID));
                if (dtCandidateEmailImage.Rows.Count > 0 && dtCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                    emailimagepath = imagepath + dtCandidateEmailImage.Rows[0]["EmailImage"].ToString();

                string candidateEmail = "";
                string questionnaireID = "";
                string candidateID = "";
                string OrganisationName = "";
                string Startdate = "";
                string Enddate = "";
                string CandidateName = "";
                string FirstName = "";

                candidateEmail = dtResult.Rows[i]["CandidateEmail"].ToString();
                questionnaireID = dtResult.Rows[i]["QuestionnaireID"].ToString();
                candidateID = dtResult.Rows[i]["AsgnDetailID"].ToString();
                OrganisationName = dtResult.Rows[i]["OrganisationName"].ToString();
                Startdate = Convert.ToDateTime(dtResult.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy");
                Enddate = Convert.ToDateTime(dtResult.Rows[i]["Enddate"]).ToString("dd-MMM-yyyy");
                CandidateName = dtResult.Rows[i]["CandidateName"].ToString();
                string[] strFName = CandidateName.Split(' ');
                FirstName = strFName[0].ToString();

                questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                candidateID = PasswordGenerator.EnryptString(candidateID);

                string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

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


                if (dtAccountAdmin.Rows.Count > 0)
                {
                    Template = Template.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                    Template = Template.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                    Subject = Subject.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                    //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                    MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");

                    SendEmail.SendMailAsync(Subject, Template, candidateEmail, maddr, emailimagepath, "");
                }
                else
                {
                    Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                    Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                    Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

                    SendEmail.SendMailAsync(Subject, Template, candidateEmail, null, string.Empty, "");
                }
            }

            // Create a new ListItem object for the contact in the row.     
            ListItem item = new ListItem();
            //lblMessage.Text = "Email sent successfully to " + candidateName;
            lblMessage2.Text = "Email sent successfully to " + candidateName;
        }
    }

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

            AssignQuestionnaire_BE assignquestionnaire_BE = new AssignQuestionnaire_BE();
            AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();

            assignquestionnaire_BE.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
            assignquestionnaire_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);

            //Changes here 
            //assignquestionnaire_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
            assignquestionnaire_BE.QuestionnaireID = questionnaire_BAO.GetQuestionnaireID(assignquestionnaire_BE.ProjecctID.ToString());

            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (ddlTargetPerson.Visible == false)
            {
                assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(identity.User.UserID);
            }
            else
            {
                assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
            }

            assignquestionnaire_BE.Description = "";
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                assignquestionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                dtCandidateList = assignquestionnaire_BAO.GetdtAssignList(ddlTargetPerson.SelectedValue, ddlProgramme.SelectedValue);
            }
            else
            {
                assignquestionnaire_BE.AccountID = identity.User.AccountID;
                dtCandidateList = assignquestionnaire_BAO.GetdtAssignList(identity.User.UserID.ToString(), ddlProgramme.SelectedValue);
            }
            //if (txtCandidateNo.Text.Trim() != "" || txtCandidateNo.Text.Trim() == "0")
            //{
            //    assignquestionnaire_BE.CandidateNo = Convert.ToInt32(txtCandidateNo.Text.Trim());
            //}

            //set just to run the code need to check this when work complete
            //assignquestionnaire_BE.CandidateNo = 1;

            string qId = PasswordGenerator.EnryptString(assignquestionnaire_BE.QuestionnaireID.ToString());
            string cId = PasswordGenerator.EnryptString(assignquestionnaire_BE.TargetPersonID.ToString());
            string path = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
            string feedbackurl = path + "Feedback.aspx?QID=" + qId + "&CID=" + cId;

            assignquestionnaire_BE.ModifiedBy = 1;
            assignquestionnaire_BE.ModifiedDate = DateTime.Now;
            assignquestionnaire_BE.IsActive = 1;
            assignquestionnaire_BE.FeedbackURL = feedbackurl;

            assignquestionnaire_BE.AssignmentDetails = GetCandidateListToAssign(relationShipID, relationShip, name, emailID);//GetCandidateList();

            if (assignquestionnaire_BE.AssignmentDetails.Count > 1)
            {
                string accountID = ConfigurationManager.AppSettings["AccountID"].ToString();

                if (assignquestionnaire_BE.AccountID == Convert.ToInt32(accountID))
                {
                    int managerCount = CheckManagerCount();

                    int count = (from Ad in assignquestionnaire_BE.AssignmentDetails
                                 where Ad.RelationShip.ToLower() == "manager"
                                 select Ad).Count();

                    managerCount = managerCount + count;

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
                int?[] assigDetailsID = assignquestionnaire_BAO.AddAssignQuestionnaireForColleagues(assignquestionnaire_BE);

                string strassigDetailsIDs = string.Empty;
                for (int i = 0; i < assigDetailsID.Count(); i++)
                {
                    if (assigDetailsID[i].HasValue)
                        strassigDetailsIDs += assigDetailsID[i].Value.ToString() + ",";
                }

                strassigDetailsIDs = strassigDetailsIDs.TrimEnd(',');


                DataTable dtResult = new DataTable();
                dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireListDetails(strassigDetailsIDs);

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

                if (assignquestionnaire_BE.AccountID != Convert.ToInt32(accountID))
                {
                    //Send mail to candidates
                    string imagepath = Server.MapPath("~/EmailImages/");

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                        DataTable dtAccountAdmin = new DataTable();

                        dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.TargetPersonID));

                        Template = assignquestionnaire_BAO.FindTemplate(Convert.ToInt32(ddlProject.SelectedValue));
                        Subject = assignquestionnaire_BAO.FindCandidateSubjectTemplate(Convert.ToInt32(ddlProject.SelectedValue));

                        // Get Candidate Email Image Name & Will Combined with EmailImagePath
                        DataTable dtCandidateEmailImage = new DataTable();
                        string emailimagepath = "";
                        dtCandidateEmailImage = assignquestionnaire_BAO.GetCandidateEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));
                        if (dtCandidateEmailImage.Rows.Count > 0 && dtCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                            emailimagepath = imagepath + dtCandidateEmailImage.Rows[0]["EmailImage"].ToString();

                        string questionnaireID = "";
                        string candidateID = "";
                        string OrganisationName = "";
                        string Startdate = "";
                        string Enddate = "";
                        string CandidateName = "";
                        string FirstName = "";
                        string candidateEmail = "";

                        candidateEmail = dtResult.Rows[i]["CandidateEmail"].ToString();
                        questionnaireID = dtResult.Rows[i]["QuestionnaireID"].ToString();
                        candidateID = dtResult.Rows[i]["AsgnDetailID"].ToString();
                        OrganisationName = dtResult.Rows[i]["OrganisationName"].ToString();
                        Startdate = Convert.ToDateTime(dtResult.Rows[0]["StartDate"]).ToString("dd-MMM-yyyy");
                        Enddate = Convert.ToDateTime(dtResult.Rows[0]["Enddate"]).ToString("dd-MMM-yyyy");
                        CandidateName = dtResult.Rows[i]["CandidateName"].ToString();
                        string[] strFName = CandidateName.Split(' ');
                        FirstName = strFName[0].ToString();

                        questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                        candidateID = PasswordGenerator.EnryptString(candidateID);

                        string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                        string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                        if (dtResult.Rows[i]["RelationShip"].ToString() == "Self")
                        {
                            string feedbackURL = urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + PasswordGenerator.EnryptString(dtResult.Rows[i]["AsgnDetailID"].ToString());
                            assignquestionnaire_BAO.SetFeedbackURL(Convert.ToInt32(dtResult.Rows[i]["AsgnDetailID"].ToString()), Convert.ToInt32(dtResult.Rows[i]["AssignmentID"].ToString()), feedbackURL);
                        }

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

                        if (dtResult.Rows[i]["RelationShip"].ToString() != "Self")
                        {
                            if (dtAccountAdmin.Rows.Count > 0)
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                                Template = Template.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                                Subject = Subject.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                                MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");

                                SendEmail.SendMailAsync(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString(), maddr, emailimagepath, "");
                            }
                            else
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                                Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                                Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

                                SendEmail.SendMailAsync(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString(), null, string.Empty, "");
                            }
                        }
                    }

                    //lblMessage.Text = "Email successfully sent";
                    lblMessage2.Text = "Email successfully sent to " + name;
                }
                else
                {
                    int assignmentID = assignquestionnaire_BAO.GetAssignmentID(assignquestionnaire_BE);


                    Project_BAO project_BAO = new Project_BAO();
                    List<Project_BE> project_BEList = new List<Project_BE>();
                    project_BEList = project_BAO.GetProjectByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.ProjecctID));

                    int managerEmailId = Convert.ToInt32(project_BEList[0].EmailTMPManager);

                    EmailTemplate_BAO emailTemplate_BAO = new EmailTemplate_BAO();
                    List<EmailTemplate_BE> emailTemplate_BEList = new List<EmailTemplate_BE>();
                    emailTemplate_BEList = emailTemplate_BAO.GetEmailTemplateByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), managerEmailId);

                    string emailText = emailTemplate_BEList[0].EmailText;
                    string emailSubject = emailTemplate_BEList[0].Subject;

                    StringBuilder candidatelist = new StringBuilder();
                    candidatelist.Append("<table width='500' border='1' cellspacing='0'>");

                    candidatelist.Append("<tr><td width='50%'><b>Name</b></td><td width='50%'><b>Relationship</b></td></tr>");

                    DataTable dtColleagueList = new DataTable();
                    dtColleagueList = assignquestionnaire_BAO.GetColleaguesList(assignmentID);

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
                    int candidateNumber = Convert.ToInt32(assignquestionnaire_BE.CandidateNo);

                    emailText = emailText.Replace("[MANAGERFIRSTNAME]", lineManagerName);
                    emailText = emailText.Replace("[PARTICIPANTNAME]", participantName);
                    emailText = emailText.Replace("[LISTOFNAMES]", listOfNames);
                    emailText = emailText.Replace("[ACCEPT]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("1") + "' >Accept</a>");
                    emailText = emailText.Replace("[DECLINE]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("0") + "' >Decline</a>");

                    emailSubject = emailSubject.Replace("[PARTICIPANTNAME]", participantName);

                    SendEmail.Send(emailSubject, emailText, lineManagerEmail, "");

                    //lblMessage.Text = "Email has been sent successfully to Manager for further approval";
                    lblMessage2.Text = "Email has been sent successfully to Manager for further approval";
                    //imbAssign.Enabled = true;
                }

                //txtCandidateNo.Text = "";

                rptrCandidateList.DataSource = null;
                rptrCandidateList.DataBind();
            }
            else
            {
                lblvalidation.Text = "Please  fill colleagues' information";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private List<AssignmentDetails_BE> GetCandidateListToAssign(string relationShipID, string relationShip, string name, string emailID)
    {
        List<AssignmentDetails_BE> assignmentDetails_BEList = new List<AssignmentDetails_BE>();

        AssignmentDetails_BE assignmentDetails_BE = new AssignmentDetails_BE();

        //DropDownList ddlRelationship = (DropDownList)item.FindControl("ddlRelationship");
        //TextBox txtCandidateName = (TextBox)item.FindControl("txtName");
        //TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");
        if (relationShip.Trim() != "" && name.Trim() != "" && emailID.Trim() != "")
        {
            if (relationShip.ToUpper() == "MANAGER")
            {
                //if (this.isManager)
                //    this.duplicateManager = true;

                //this.isManager = true;
            }

            assignmentDetails_BE.RelationShip = relationShipID;
            assignmentDetails_BE.CandidateName = name.Trim();
            assignmentDetails_BE.CandidateEmail = emailID.Trim();
            assignmentDetails_BE.SubmitFlag = false;

            if (identity.User.GroupID == 1)
                UserAccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                UserAccountID = Convert.ToInt32(identity.User.AccountID);

            if (UserAccountID == Convert.ToInt32(ConfigurationManager.AppSettings["AccountID"].ToString()))
                assignmentDetails_BE.EmailSendFlag = 0;
            else
                assignmentDetails_BE.EmailSendFlag = 1;

            if (assignmentDetails_BE.RelationShip != "" && assignmentDetails_BE.CandidateName != "" && assignmentDetails_BE.CandidateEmail != "")
            {
                assignmentDetails_BEList.Add(assignmentDetails_BE);
                email += emailID.Trim() + ";";
            }
        }

        DataTable dt = Session["ColleagueTable"] as DataTable;
        bool AddSelf = true;
        if (dt != null && dt.Rows.Count > 1)
        {
            DataRow[] dr = dt.Select("RelationShip = 'Self'");

            if (dr.Count() > 0)
                AddSelf = false;
        }

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
            List<AccountUser_BE> Userlist = user.GetAccountUserByID(UserAccountID, targetpersonid);
            AssignmentDetails_BE assignmentDetails = new AssignmentDetails_BE();

            assignmentDetails.CandidateEmail = Userlist[0].EmailID;
            assignmentDetails.CandidateName = Userlist[0].FirstName + " " + Userlist[0].LastName;
            assignmentDetails.RelationShip = "Self";
            assignmentDetails.SubmitFlag = false;
            assignmentDetails.EmailSendFlag = 1;
            assignmentDetails_BEList.Add(assignmentDetails);
            email += Userlist[0].EmailID + ";";
        }
        finalemail = email.TrimEnd(';');

        return assignmentDetails_BEList;
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participant_BAO = new AssignQstnParticipant_BAO();
        identity = this.Page.User.Identity as WADIdentity;
        DataTable dtParticipant = null;

        if (ddlProgramme.SelectedIndex > 0)
        {
            Programme_BAO programme_BAO = new Programme_BAO();

            DateTime[] dtReport = programme_BAO.GetProgramReportDate(Convert.ToInt32(ddlProgramme.SelectedValue));

            if (dtReport != null && dtReport.Count() == 2)
            {
                lblReportMSG.InnerText = "Report will be available from " + dtReport[0].ToString("dd/MM/yyyy");
                lblReportMSG.Visible = true;

                if (dtReport[0] <= DateTime.Now)
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
                string strInstructions = programme_BAO.GetProgramInstructions(Convert.ToInt32(ddlProgramme.SelectedValue));
                lblInstruction.Text = strInstructions;
            }

            strColleagueNo = programme_BAO.GetProgramColleagueNumber(Convert.ToInt32(ddlProgramme.SelectedValue));

            AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();

            DataTable dt = assignQuestionnaire_BAO.GetdtAssignColleagueList(identity.User.UserID.ToString(), ddlProgramme.SelectedValue);

            int colleagueCount = dt.Rows.Count;

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
                if (dt != null)
                {
                    if (dt.Rows.Count < colleagueNo)
                    {
                        int maxNo = (colleagueNo - dt.Rows.Count);
                        for (int i = 0; i < maxNo; i++)
                        {
                            DataRow row = dt.NewRow();
                            dt.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("Relationship");
                    dt.Columns.Add("Name");
                    dt.Columns.Add("EmailID");

                    for (int i = 0; i < colleagueNo; i++)
                        dt.Rows.Add("", "", "");

                }

                BindColleagueRepeter(dt);

                //Enable Disable self assessment button
                //AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                DataTable dtResult = new DataTable();
                dtResult = assignQuestionnaire_BAO.GetFeedbackURL(Convert.ToInt32(identity.User.UserID));

                if (dtResult.Rows.Count > 0)
                {
                    string url = dtResult.Rows[0]["FeedbackUrl"].ToString();

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
                dtParticipant = participant_BAO.GetdtAssignPartiList(Convert.ToString(ddlAccountCode.SelectedValue), ddlProgramme.SelectedValue);
            }
            else
            {
                dtParticipant = participant_BAO.GetdtAssignPartiList(Convert.ToString(identity.User.AccountID), ddlProgramme.SelectedValue);
            }

            Project_BAO project_BAO = new Project_BAO();

            if (dtParticipant.Rows.Count > 0)
            {
                ddlTargetPerson.Items.Clear();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));

                ddlTargetPerson.DataSource = dtParticipant;
                ddlTargetPerson.DataTextField = "UserName";
                ddlTargetPerson.DataValueField = "UserID";
                ddlTargetPerson.DataBind();
            }
            else
            {
                ddlTargetPerson.Items.Clear();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }

    protected void imbReportDownload_Click(object sender, ImageClickEventArgs e)
    {
        AssignQstnParticipant_BAO assignQstnParticipant_BAO = new AssignQstnParticipant_BAO();


        //string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32((User.Identity as WADIdentity).User.UserID)).ToString();
        int userID = Convert.ToInt32(Convert.ToInt32(ddlTargetPerson.SelectedValue));
        //string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32((User.Identity as WADIdentity).User.UserID)).ToString();
        string fName = assignQstnParticipant_BAO.GetReportFileName(userID).ToString();

        if (fName != "")
        {
            string root = Server.MapPath("~") + "\\ReportGenerate\\";
            string openpdf = root + fName;

            if (!File.Exists(openpdf))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ReportMSG", "alert('File not found');", true);
                return;
            }

            Byte[] bytes = File.ReadAllBytes(openpdf);
            Response.Buffer = true;
            Response.Charset = "";
            //Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment;filename=" + fName);
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
            AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
            identity = this.Page.User.Identity as WADIdentity;
            DataTable dtSelfName = accountUser_BAO.GetdtAccountUserByID(identity.User.AccountID.Value, identity.User.UserID.Value);
            if (dtSelfName != null && dtSelfName.Rows.Count > 0)
            {
                strReportName = dtSelfName.Rows[0]["FirstName"].ToString() + dtSelfName.Rows[0]["LastName"].ToString() + '_' + DateTime.Now.ToString("ddMMyyHHmmss");
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

    protected void GenerateReport(string dirName, string strProjectID, string strTargetPersonID, string strReportName)
    {
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
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

                DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                {
                    /*
                     * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                     * & Making Entry in Table with Radarchatname
                     * & Calling in RDL (RadarImage)
                     */
                    if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                        Radar(strTargetPersonID, strGroupList);
                    else
                        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                        RadarPreviousScore(strTargetPersonID, strGroupList);
                    else
                        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //BenchMark Radar Chart.
                    if (dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() == "1")
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
                DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));
                if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                {
                    // if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
                    RadarCPL(strTargetPersonID, strGroupList);
                    // else
                    //  targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChartNoImage" + ".jpg";

                    //Previous ScoreRadar Chart.
                    if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
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

    protected void ControlToParameter(string projectid)
    {
        if (projectid != null)
        {
            ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
            DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));
            if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
            {
                // This parameter will Decide: which type of Report will Call                
                if (dtreportsetting.Rows[0]["ReportType"].ToString() != string.Empty)
                    strReportType = dtreportsetting.Rows[0]["ReportType"].ToString();

                if (dtreportsetting.Rows[0]["CoverPage"].ToString() != string.Empty)
                    strFrontPage = dtreportsetting.Rows[0]["CoverPage"].ToString();

                if (dtreportsetting.Rows[0]["ReportIntroduction"].ToString() != string.Empty)
                    strReportIntroduction = dtreportsetting.Rows[0]["ReportIntroduction"].ToString();

                if (dtreportsetting.Rows[0]["Conclusionpage"].ToString() != string.Empty)
                    strConclusionPage = dtreportsetting.Rows[0]["Conclusionpage"].ToString();

                if (dtreportsetting.Rows[0]["RadarChart"].ToString() != string.Empty)
                    strRadarChart = dtreportsetting.Rows[0]["RadarChart"].ToString();

                if (dtreportsetting.Rows[0]["QstTextResponses"].ToString() != string.Empty)
                    strDetailedQst = dtreportsetting.Rows[0]["QstTextResponses"].ToString();

                if (dtreportsetting.Rows[0]["CatQstList"].ToString() != string.Empty)
                    strCategoryQstlist = dtreportsetting.Rows[0]["CatQstList"].ToString();

                if (dtreportsetting.Rows[0]["CatDataChart"].ToString() != string.Empty)
                    strCategoryBarChart = dtreportsetting.Rows[0]["CatDataChart"].ToString();

                if (dtreportsetting.Rows[0]["CandidateSelfStatus"].ToString() != string.Empty)
                    strSelfNameGrp = dtreportsetting.Rows[0]["CandidateSelfStatus"].ToString();

                if (dtreportsetting.Rows[0]["FullProjectGrp"].ToString() != string.Empty)
                    strFullProjGrp = dtreportsetting.Rows[0]["FullProjectGrp"].ToString();

                if (dtreportsetting.Rows[0]["ProgrammeGrp"].ToString() != string.Empty)
                    strProgrammeGrp = dtreportsetting.Rows[0]["ProgrammeGrp"].ToString();

                if (dtreportsetting.Rows[0]["ProjectRelationGrp"].ToString() != string.Empty)
                    strGroupList = dtreportsetting.Rows[0]["ProjectRelationGrp"].ToString();

                if (dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString() != string.Empty)
                    strConHighLowRange = dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString();

                if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() != string.Empty)
                    strPreScoreVisibility = dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString();


                if (dtreportsetting.Rows[0]["BenchMarkGrpVisible"].ToString() != string.Empty)
                    strBenchMarkGrpVisibility = dtreportsetting.Rows[0]["BenchMarkGrpVisible"].ToString();

                if (dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() != string.Empty)
                    strBenchMarkVisibility = dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString();

                if (dtreportsetting.Rows[0]["BenchConclusionpage"].ToString() != string.Empty)
                    strBenchConclusionPageVisibility = dtreportsetting.Rows[0]["BenchConclusionpage"].ToString();

            }
        }
    }

    #region Radar Chart Method

    public void Radar(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        DataTable dtSelfData = reportManagement_BAO.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        DataTable dtFullProjectData = reportManagement_BAO.GetRadarChartData(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

        string[] xValues = new string[dtSelfData.Rows.Count];
        double[] yValues = new double[dtSelfData.Rows.Count];
        for (int i = 0; i < dtSelfData.Rows.Count; i++)
        {
            xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dtFullProjectData.Rows.Count];
        double[] yValues1 = new double[dtFullProjectData.Rows.Count];
        for (int i = 0; i < dtFullProjectData.Rows.Count; i++)
        {
            xValues1[i] = dtFullProjectData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dtFullProjectData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dtSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullProjectData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dtSelfData.Rows.Count > 0)
            Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
        if (dtFullProjectData.Rows.Count > 0)
            Series2 = dtFullProjectData.Rows[0]["RelationShip"].ToString();

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


        //Change Color Of Graph
        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dtFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullProjectData.Rows.Count > 0 || dtFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
    }

    public void RadarPreviousScore(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        DataTable dtSelfData = reportManagement_BAO.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        DataTable dtFullPreviousData = reportManagement_BAO.GetRadarChartPreviousScoreData(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

        string[] xValues = new string[dtSelfData.Rows.Count];
        double[] yValues = new double[dtSelfData.Rows.Count];
        for (int i = 0; i < dtSelfData.Rows.Count; i++)
        {
            xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dtFullPreviousData.Rows.Count];
        double[] yValues1 = new double[dtFullPreviousData.Rows.Count];
        for (int i = 0; i < dtFullPreviousData.Rows.Count; i++)
        {
            xValues1[i] = dtFullPreviousData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dtFullPreviousData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dtSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullPreviousData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dtSelfData.Rows.Count > 0)
            Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
        if (dtFullPreviousData.Rows.Count > 0)
            Series2 = dtFullPreviousData.Rows[0]["RelationShip"].ToString();

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dtFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullPreviousData.Rows.Count > 0 || dtFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
    }

    public void RadarCPL(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();

        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        DataTable dtSelfData = reportManagement_BAO.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        DataTable dtFullProjectData = reportManagement_BAO.GetRadarChartDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "F");

        string[] xValues = new string[dtSelfData.Rows.Count];
        double[] yValues = new double[dtSelfData.Rows.Count];
        for (int i = 0; i < dtSelfData.Rows.Count; i++)
        {
            if (i == dtSelfData.Rows.Count - 1)
            {
                xValues[0] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[0] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues[i + 1] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i + 1] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }
        }

        string[] xValues1 = new string[dtFullProjectData.Rows.Count];
        double[] yValues1 = new double[dtFullProjectData.Rows.Count];
        for (int i = 0; i < dtFullProjectData.Rows.Count; i++)
        {
            if (i == dtFullProjectData.Rows.Count - 1)
            {
                xValues1[0] = dtFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[0] = Convert.ToDouble(dtFullProjectData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues1[i + 1] = dtFullProjectData.Rows[i]["CategoryName"].ToString();
                yValues1[i + 1] = Convert.ToDouble(dtFullProjectData.Rows[i]["Average"].ToString());
            }
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dtSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dtFullProjectData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullProjectData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dtSelfData.Rows.Count > 0)
            Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
        if (dtFullProjectData.Rows.Count > 0)
            Series2 = dtFullProjectData.Rows[0]["RelationShip"].ToString();

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;


        //Change Color Of Graph
        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dtFullProjectData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dtFullProjectData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarname = Server.MapPath("~\\UploadDocs\\") + "RadarChart" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullProjectData.Rows.Count > 0 || dtFullProjectData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarname);

        //dtSelfData.Dispose();
        //Chart1.Dispose();             
    }

    public void RadarPreviousScoreCPL(string strTargetPersonID, string strGroupList)
    {
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        DataTable dtSelfData = reportManagement_BAO.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "S");
        DataTable dtFullPreviousData = reportManagement_BAO.GetRadarChartPreviousScoreDataCPL(Convert.ToInt32(strTargetPersonID), strGroupList, "P");

        string[] xValues = new string[dtSelfData.Rows.Count];
        double[] yValues = new double[dtSelfData.Rows.Count];
        for (int i = 0; i < dtSelfData.Rows.Count; i++)
        {
            if (i == dtSelfData.Rows.Count - 1)
            {
                xValues[0] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[0] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues[i + 1] = dtSelfData.Rows[i]["CategoryName"].ToString();
                yValues[i + 1] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
            }
        }

        string[] xValues1 = new string[dtFullPreviousData.Rows.Count];
        double[] yValues1 = new double[dtFullPreviousData.Rows.Count];
        for (int i = 0; i < dtFullPreviousData.Rows.Count; i++)
        {
            if (i == dtFullPreviousData.Rows.Count - 1)
            {
                xValues1[0] = dtFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[0] = Convert.ToDouble(dtFullPreviousData.Rows[i]["Average"].ToString());
            }
            else
            {
                xValues1[i + 1] = dtFullPreviousData.Rows[i]["CategoryName"].ToString();
                yValues1[i + 1] = Convert.ToDouble(dtFullPreviousData.Rows[i]["Average"].ToString());
            }
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dtSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dtFullPreviousData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtFullPreviousData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dtSelfData.Rows.Count > 0)
            Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
        if (dtFullPreviousData.Rows.Count > 0)
            Series2 = dtFullPreviousData.Rows[0]["RelationShip"].ToString();

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dtFullPreviousData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dtFullPreviousData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarPreviousScore = Server.MapPath("~\\UploadDocs\\") + "RadarChartPreviousScore" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtFullPreviousData.Rows.Count > 0 || dtFullPreviousData.Rows.Count > 0)
            Chart1.SaveImage(@targetradarPreviousScore);
    }

    public void RadarBenchMark(string strTargetPersonID)
    {
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
        Chart1.Series.Clear();
        string Series1 = string.Empty;
        string Series2 = string.Empty;
        DataTable dtSelfData = reportManagement_BAO.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "S");
        DataTable dtBenchMarkData = reportManagement_BAO.GetRadarChartBenchMark(Convert.ToInt32(strTargetPersonID), "P");

        string[] xValues = new string[dtSelfData.Rows.Count];
        double[] yValues = new double[dtSelfData.Rows.Count];
        for (int i = 0; i < dtSelfData.Rows.Count; i++)
        {
            xValues[i] = dtSelfData.Rows[i]["CategoryName"].ToString();
            yValues[i] = Convert.ToDouble(dtSelfData.Rows[i]["Average"].ToString());
        }

        string[] xValues1 = new string[dtBenchMarkData.Rows.Count];
        double[] yValues1 = new double[dtBenchMarkData.Rows.Count];
        for (int i = 0; i < dtBenchMarkData.Rows.Count; i++)
        {
            xValues1[i] = dtBenchMarkData.Rows[i]["CategoryName"].ToString();
            yValues1[i] = Convert.ToDouble(dtBenchMarkData.Rows[i]["Average"].ToString());
        }

        //Can Set Y-Axis Scale from here.
        Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 3;
        if (dtSelfData.Rows.Count > 0)
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtSelfData.Rows[0]["UpperBound"].ToString());
        else
        {
            if (dtBenchMarkData.Rows.Count > 0)
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToInt32(dtBenchMarkData.Rows[0]["UpperBound"].ToString());
            else
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 10; // Default value.
        }

        //Adding Series in RadarChart 
        if (dtSelfData.Rows.Count > 0)
            Series1 = dtSelfData.Rows[0]["RelationShip"].ToString();
        if (dtBenchMarkData.Rows.Count > 0)
            Series2 = dtBenchMarkData.Rows[0]["RelationShip"].ToString();

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series.Add(Series1);
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series.Add(Series2);

        // Defining Series Type
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].ChartType = SeriesChartType.Radar;
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].ChartType = SeriesChartType.Radar;

        //Change Color Of Graph
        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].Color = System.Drawing.Color.FromArgb(220, 65, 140, 240);
            Chart1.Series[Series1].BackGradientStyle = GradientStyle.DiagonalRight;
        }
        if (dtBenchMarkData.Rows.Count > 0)
        {
            Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(193, 255, 193); //(240, 128, 128);
            Chart1.Series[Series2].BackGradientStyle = GradientStyle.DiagonalRight;
        }

        //Chart1.Series[Series2].Color = System.Drawing.Color.FromArgb(220, 252, 180, 65);


        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderColor = System.Drawing.Color.Black;
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderColor = System.Drawing.Color.Black;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderDashStyle = ChartDashStyle.Solid;
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderDashStyle = ChartDashStyle.Solid;

        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].BorderWidth = 1;
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].BorderWidth = 1;

        // Populate series data
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1].Points.DataBindXY(xValues, yValues);
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2].Points.DataBindXY(xValues1, yValues1);

        // Set radar chart style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["RadarDrawingStyle"] = "Area";
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["RadarDrawingStyle"] = "Area";

        if (dtSelfData.Rows.Count > 0)
        {
            Chart1.Series[Series1].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series1].BorderWidth = 1;
        }
        if (dtBenchMarkData.Rows.Count > 0)
        {
            Chart1.Series[Series2].BorderColor = Color.FromArgb(100, 100, 100);
            Chart1.Series[Series2].BorderWidth = 1;
        }

        // Set circular area drawing style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["AreaDrawingStyle"] = "Polygon";
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["AreaDrawingStyle"] = "Polygon";

        // Set labels style
        if (dtSelfData.Rows.Count > 0)
            Chart1.Series[Series1]["CircularLabelsStyle"] = "Horizontal";
        if (dtBenchMarkData.Rows.Count > 0)
            Chart1.Series[Series2]["CircularLabelsStyle"] = "Horizontal";
        //Chart1.SaveImage(@"c:\Images\RadarChart.jpg");

        targetradarBenchmark = Server.MapPath("~\\UploadDocs\\") + "RadarChartBenchMark" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".jpg";
        if (dtBenchMarkData.Rows.Count > 0 || dtBenchMarkData.Rows.Count > 0)
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

    protected void GetDetailFromTargetPersonID(string targetid, out string strTargetPersonID, out string strProjectID, out string strAccountID, out string strProgrammeID)
    {
        strTargetPersonID = targetid;
        ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
        AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
        DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(strTargetPersonID));
        if (dtuserlist != null && dtuserlist.Rows.Count > 0)
        {
            int projectid = Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]);
            strProjectID = dtuserlist.Rows[0]["ProjectID"].ToString();

            Project_BAO project_BAO = new Project_BAO();
            DataTable project = project_BAO.GetdataProjectByID(projectid);
            if (project != null && project.Rows.Count > 0)
            {
                strAccountID = project.Rows[0]["AccountID"].ToString();
            }
            else
                strAccountID = Convert.ToString(ViewState["accid"]);

            DataTable programme = reportManagement_BAO.GetdataProjectByID(projectid);
            if (programme != null && programme.Rows.Count > 0)
            {
                strProgrammeID = programme.Rows[0]["ProgrammeID"].ToString();
            }
            else
                strProgrammeID = "0";
        }
        else
        {
            strProjectID = Convert.ToString(ViewState["prjid"]);

            strAccountID = Convert.ToString(ViewState["accid"]);

            int projectid = Convert.ToInt32(ViewState["prjid"]);
            DataTable programme = reportManagement_BAO.GetdataProjectByID(projectid);
            if (programme != null && programme.Rows.Count > 0)
            {
                strProgrammeID = programme.Rows[0]["ProgrammeID"].ToString();
            }
            else
                strProgrammeID = "0";
        }

    }

    protected void imbSelfAssessment_Click(object sender, ImageClickEventArgs e)
    {
        //Enable Disable self assessment button
        AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        DataTable dtResult = new DataTable();
        WADIdentity identity = this.Page.User.Identity as WADIdentity;

        int userID = Convert.ToInt32(identity.User.UserID);

        if (ddlTargetPerson.Visible)
            userID = Convert.ToInt32(Convert.ToInt32(ddlTargetPerson.SelectedValue));

        dtResult = assignQuestionnaire_BAO.GetFeedbackURL(userID);

        if (dtResult.Rows.Count > 0)
        {
            string url = dtResult.Rows[0]["FeedbackUrl"].ToString();
            if (string.IsNullOrEmpty(url))
            {
                int assignmentID = assignQuestionnaire_BAO.GetAssignmentID(userID);
                string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                DataTable dt = new DataTable();
                dt = assignQuestionnaire_BAO.GetdtAssignQuestionnaireListDetails(assignmentID.ToString());

                string questionnaireID = dt.Rows[0]["QuestionnaireID"].ToString();
                string candidateID = dt.Rows[0]["AsgnDetailID"].ToString();

                string path = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
                string feedbackURL = urlPath + "Feedback.aspx?QID=" + PasswordGenerator.EnryptString(questionnaireID) + "&CID=" + PasswordGenerator.EnryptString(dt.Rows[0]["AsgnDetailID"].ToString());
                assignQuestionnaire_BAO.SetFeedbackURL(Convert.ToInt32(dt.Rows[0]["AsgnDetailID"].ToString()), Convert.ToInt32(dt.Rows[0]["AssignmentID"].ToString()), feedbackURL);

                url = feedbackURL;
            }

            Response.Redirect(url, false);
        }

    }

    protected void ImageButtonSaveAll_Click(object sender, ImageClickEventArgs e)
    {
        string participantRoleId = string.Empty;

        for (int i = 0; i < rptrCandidateList.Items.Count; i++)
        {
            ImageButton assignButton = (ImageButton)rptrCandidateList.Items[i].FindControl("imbSaveColleague");
            ImageButton saveButton = (ImageButton)rptrCandidateList.Items[i].FindControl("imbSaveOnlyColleague");
            DropDownList ddlRelationship = (DropDownList)rptrCandidateList.Items[i].FindControl("ddlRelationship");
            Label lblAssignmentID = (Label)rptrCandidateList.Items[i].FindControl("lblAssignID");
            Label lblAccountID = (Label)rptrCandidateList.Items[i].FindControl("lblAccountID");
            Label lblTargetPersonID = (Label)rptrCandidateList.Items[i].FindControl("lblTargetPersonID");
            Label lblAssignmentDetailsID = (Label)rptrCandidateList.Items[i].FindControl("lblAssignmentID");
            Label lblProjectID = (Label)rptrCandidateList.Items[i].FindControl("lblProjectID");
            TextBox txtCandidateName = (TextBox)rptrCandidateList.Items[i].FindControl("txtName");
            TextBox txtCandidateEmail = (TextBox)rptrCandidateList.Items[i].FindControl("txtEmailID");

            string relationship = ddlRelationship.SelectedItem.Text;

            if (assignButton.Visible)
            {
                Session["UnsavedColleagueTable"] = GetUnSavedCandidatureList();

                SaveCandidate(ddlRelationship.SelectedValue, ddlRelationship.SelectedItem.Text,
                    txtCandidateName.Text, txtCandidateEmail.Text);

                WADIdentity uIdentity = this.Page.User.Identity as WADIdentity;
                participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                if (uIdentity.User.GroupID.ToString() != participantRoleId)
                    ddlTargetPerson_SelectedIndexChanged(this, EventArgs.Empty);
                else
                    ddlProgramme_SelectedIndexChanged(this, EventArgs.Empty);
            }
            else
            {
                if (!string.IsNullOrEmpty(lblAssignmentID.Text) && !string.IsNullOrEmpty(lblAccountID.Text) && !string.IsNullOrEmpty(lblTargetPersonID.Text)
                    && !string.IsNullOrEmpty(lblAssignmentDetailsID.Text) && !string.IsNullOrEmpty(lblProjectID.Text))
                {
                    int assignmentID = int.Parse(lblAssignmentID.Text.Trim());
                    int accountID = int.Parse(lblAccountID.Text.Trim());
                    int targetPersonID = int.Parse(lblTargetPersonID.Text.Trim());
                    int assignmentDetailsID = int.Parse(lblAssignmentDetailsID.Text.Trim());
                    int projectID = int.Parse(lblProjectID.Text.Trim());

                    ReSendColleagueEmail(assignmentID, accountID, targetPersonID, assignmentDetailsID, projectID,
                           txtCandidateName.Text, txtCandidateEmail.Text, relationship, false);
                }
            }
        }
    }

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
                DropDownList ddlRelationship = (DropDownList)rptrCandidateList.Items[j].FindControl("ddlRelationship");
                TextBox txtCandidateName = (TextBox)rptrCandidateList.Items[j].FindControl("txtName");
                TextBox txtCandidateEmail = (TextBox)rptrCandidateList.Items[j].FindControl("txtEmailID");
                Label lblTargetPersonID = (Label)rptrCandidateList.Items[j].FindControl("lblTargetPersonID");

                if (string.IsNullOrEmpty(lblTargetPersonID.Text))
                {
                    DataRow dr = dataTableCandidateClone.NewRow();

                    dr["Relationship"] = ddlRelationship.SelectedItem.Text;
                    dr["Name"] = txtCandidateName.Text;
                    dr["EmailID"] = txtCandidateEmail.Text;
                    dataTableCandidateClone.Rows.Add(dr);
                }
            }

            dataTableCandidateClone.AcceptChanges();
        }
        return dataTableCandidateClone;
    }
}
