using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using Miscellaneous;
using Admin_BE;
using System.Net.Mail;


public partial class Module_Questionnaire_AssignQuestionnaire : CodeBehindBase
{
    //Global variable
    int i;
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string strName = string.Empty;
    bool flag = true;
    int j;
    //string file1;
    //string filename1;
    string expression1;
    //string expression2;
    string Finalexpression;
    //string Finalexpression2;
    //string expression6;
    //string Finalexpression6;
    string email;
    string Template;
    string finalemail;
    //string Questionnaire_id;
    //string mailid;
    WADIdentity identity;
    DataTable dataTableCompanyName;
    //DataTable dtAllAccount;
    //string expression5;
    //string Finalexpression5;
    //int targetpersonid;
    int UserAccountID;
    //string ProjectId1;
    //string ProjectId2;
    //string ProjectId3;
    //string ProjectId5;
    string Subject;
    int ccount;
    StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        System.GC.Collect();

        identity = this.Page.User.Identity as WADIdentity;
        int? grpID = Identity.User.GroupID;

        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        if (!IsPostBack)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);

            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
            //Get all Questionnaire List by user id.
            DataTable dataTableUserList = assignquestionnaire.GetuseridAssignQuestionnaireList(userid);
            Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account details by user account id and bind account dropdown.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            //Hide controls according to role
            if (identity.User.GroupID.ToString() != participantRoleId)
            {
                //Get project in an account and bind project dropdown.
                ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();

                tblParticipantUpload.Visible = true;

                ddlProject.Enabled = true;
                ddlProgramme.Enabled = true;
                ddlQuestionnaire.Enabled = true;
            }
            else
            {
                tblParticipantUpload.Visible = false;
                //Set the value for controls
                SetValues();

                trTargetPerson.Visible = false;

                Survey_AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new Survey_AssignQuestionnaire_BAO();
                Image imgHeader = (Image)Master.FindControl("imgProjectLogo");
                DataTable dtParticipantInfo = new DataTable();

                dtParticipantInfo = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                //Set Project Logo
                if (dtParticipantInfo.Rows.Count > 0)
                {
                    if (dtParticipantInfo.Rows[0]["Logo"].ToString() != "")
                    {
                        imgHeader.Visible = true;
                        imgHeader.ImageUrl = "~/UploadDocs/" + dtParticipantInfo.Rows[0]["Logo"].ToString();
                    }
                }
            }

            //If user is a super Admin GroupID == 1 then show account details section else hide.
            if (identity.User.GroupID == 1)
            {
                divAccount.Visible = true;
                ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                ddlAccountCode_SelectedIndexChanged(sender, e);
            }
            else
            {
                divAccount.Visible = false;
            }

        }
    }

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

        Survey_AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new Survey_AssignQuestionnaire_BAO();
        DataTable dtAssignDetails = new DataTable();
        //Get all Participant by user ID.
        dtAssignDetails = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
        //Get all user project by account id abd bind project drop down.
        ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

        if (dtAssignDetails.Rows.Count > 0)
            ddlProject.SelectedValue = dtAssignDetails.Rows[0]["ProjecctID"].ToString();
        else
            ddlProject.SelectedValue = "";

        Questionnaire_BAO.Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Survey_Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dtQuestionnaire = new DataTable();
        //Get all user Questionnaire by project id abd bind Questionnaire drop down.
        dtQuestionnaire = questionnaireBusinessAccessObject.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtQuestionnaire.Rows.Count > 0)
        {
            ddlQuestionnaire.DataSource = dtQuestionnaire;
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();

            ddlQuestionnaire.SelectedValue = dtAssignDetails.Rows[0]["QuestionnaireID"].ToString();
        }

        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        //Set Programme
        Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get all user program by project id abd bind program drop down.
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), 0, 0);

        if (dataTableProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();

            ddlProgramme.SelectedValue = dtAssignDetails.Rows[0]["ProgrammeID"].ToString();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ddlProject.Enabled = false;
        ddlProgramme.Enabled = false;
        ddlQuestionnaire.Enabled = false;

        //Set Relationship

        DataTable dtAnalysis2 = new DataTable();
        DataTable dtAnalysis3 = new DataTable();
        //Get analysis 1 value and store in session
        dtAnalysis2 = programmeBusinessAccessObject.GetAnalysis2(Convert.ToInt32(ddlProgramme.SelectedValue));
        Session["Analysis2"] = dtAnalysis2;
        //Get analysis 2 value and store in session
        dtAnalysis3 = programmeBusinessAccessObject.GetAnalysis3(Convert.ToInt32(ddlProgramme.SelectedValue));
        Session["Analysis3"] = dtAnalysis3;
    }

    /// <summary>
    /// Save particiant details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            imbAssign.Enabled = false;
            //If grid has participant.
            if (rptrCandidateList.Items.Count != 0)
            {
                lblMessage.Text = "";
                lblMessage2.Text = "";
                lblvalidation.Text = "";
                HandleWriteLog("Start", new StackTrace(true));
                identity = this.Page.User.Identity as WADIdentity;
                //Set Properties.
                Survey_AssignQuestionnaire_BE assignQuestionnaireBusinesEntity = new Survey_AssignQuestionnaire_BE();
                Survey_AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new Survey_AssignQuestionnaire_BAO();

                assignQuestionnaireBusinesEntity.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
                assignQuestionnaireBusinesEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
                assignQuestionnaireBusinesEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                assignQuestionnaireBusinesEntity.Description = "";// txtDescription.Text.Trim();
                identity = this.Page.User.Identity as WADIdentity;
                //If user is super admin then group =1 then account dropdown value else user account id.
                if (identity.User.GroupID == 1)
                {
                    assignQuestionnaireBusinesEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                }
                else
                {
                    assignQuestionnaireBusinesEntity.AccountID = identity.User.AccountID;
                }

                if (txtCandidateNo.Text.Trim() != "" || txtCandidateNo.Text.Trim() == "0")
                {
                    assignQuestionnaireBusinesEntity.CandidateNo = Convert.ToInt32(txtCandidateNo.Text.Trim());
                }

                string QuestionnaireID = PasswordGenerator.EnryptString(assignQuestionnaireBusinesEntity.QuestionnaireID.ToString());

                string path = ConfigurationManager.AppSettings["SurveyFeedbackURL"].ToString();
                string feedbackurl = path + "Feedback.aspx?QID=" + QuestionnaireID;

                assignQuestionnaireBusinesEntity.ModifiedBy = 1;
                assignQuestionnaireBusinesEntity.ModifiedDate = DateTime.Now;
                assignQuestionnaireBusinesEntity.IsActive = 1;

                assignQuestionnaireBusinesEntity.AssignmentDetails = GetCandidateList();
                Int32 assignmentID = 0;

                if ((assignQuestionnaireBusinesEntity.AssignmentDetails.Count == assignQuestionnaireBusinesEntity.CandidateNo + 1
                    || assignQuestionnaireBusinesEntity.AssignmentDetails.Count >= 1) && assignQuestionnaireBusinesEntity.CandidateNo != null)
                {
                    //Save Assign questionnaire
                    assignmentID = assignQuestionnaireBusinessAccessObject.AddAssignQuestionnaire(assignQuestionnaireBusinesEntity);

                    DataTable dataTableResult = new DataTable();
                    dataTableResult = assignQuestionnaireBusinessAccessObject.GetdtAssignQuestionnaireList(assignmentID);
                    DateTime dataTableStartdate = Convert.ToDateTime(dataTableResult.Rows[0]["StartDate"]).Date;
                    DateTime dataTableEnddate = Convert.ToDateTime(dataTableResult.Rows[0]["Enddate"]).Date;
                    DateTime dataTableToday = DateTime.Now.Date;
                    int loopCount = 0;
                    char loopFlag = 'N';
                    /**** "Self" check not required for Survey
                    for (int k = 0; k < Convert.ToInt32(txtCandidateNo.Text.Trim()); k++)
                    {
                        if (dtResult.Rows[k]["RelationShip"].ToString() == "Self")
                            loopFlag = 'Y';
                    }
                    */
                    if (loopFlag == 'Y')
                        loopCount = Convert.ToInt32(rptrCandidateList.Items.Count) + 1;
                    else
                        loopCount = Convert.ToInt32(rptrCandidateList.Items.Count);

                    string accountID = ConfigurationManager.AppSettings["AccountID"].ToString();

                    if (assignQuestionnaireBusinesEntity.AccountID != Convert.ToInt32(accountID))
                    {
                        if (dataTableStartdate == dataTableToday && dataTableToday <= dataTableEnddate)
                        {
                            //Send mail to candidates
                            string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

                            for (int i = 0; i < loopCount; i++)
                            {
                                Survey_AccountUser_BAO accountUserBusinessAccessObject = new Survey_AccountUser_BAO();
                                DataTable dataTableAccountAdmin = new DataTable();

                                //  dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.TargetPersonID));

                                Template = assignQuestionnaireBusinessAccessObject.FindTemplate(Convert.ToInt32(ddlProject.SelectedValue));
                                Subject = assignQuestionnaireBusinessAccessObject.FindCandidateSubjectTemplate(Convert.ToInt32(ddlProject.SelectedValue));

                                // Get Candidate Email Image Name & Will Combined with EmailImagePath
                                DataTable dataTableCandidateEmailImage = new DataTable();
                                string emailimagepath = "";

                                dataTableCandidateEmailImage = assignQuestionnaireBusinessAccessObject.GetCandidateEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));

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
                                string accountCode = "";
                                string stringLogin = "";
                                string stringPassword = "";

                                candidateEmail = dataTableResult.Rows[i]["CandidateEmail"].ToString();
                                questionnaireID = dataTableResult.Rows[i]["QuestionnaireID"].ToString();
                                candidateID = dataTableResult.Rows[i]["AsgnDetailID"].ToString();
                                OrganisationName = dataTableResult.Rows[i]["OrganisationName"].ToString();
                                Startdate = Convert.ToDateTime(dataTableResult.Rows[0]["StartDate"]).ToString("dd-MMM-yyyy");
                                Enddate = Convert.ToDateTime(dataTableResult.Rows[0]["Enddate"]).ToString("dd-MMM-yyyy");
                                CandidateName = dataTableResult.Rows[i]["CandidateName"].ToString();
                                string[] stringFirstName = CandidateName.Split(' ');
                                FirstName = stringFirstName[0].ToString();
                                accountCode = ddlAccountCode.SelectedItem.Text;
                                stringLogin = stringPassword = FirstName;
                                questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                                candidateID = PasswordGenerator.EnryptString(candidateID);

                                string urlPath = ConfigurationManager.AppSettings["SurveyFeedbackURL"].ToString();

                                string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                                //Remove template tokens.
                                Template = Template.Replace("[LINK]", link);
                                Template = Template.Replace("[NAME]", CandidateName);
                                Template = Template.Replace("[FIRSTNAME]", FirstName);
                                Template = Template.Replace("[COMPANY]", OrganisationName);
                                Template = Template.Replace("[STARTDATE]", Startdate);
                                Template = Template.Replace("[CLOSEDATE]", Enddate);
                                Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");
                                Template = Template.Replace("[CODE]", accountCode);
                                Template = Template.Replace("[LOGINID]", stringLogin);
                                Template = Template.Replace("[PASSWORD]", stringPassword);
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

                                    MailAddress eMailAddress = new MailAddress("admin@i-comment360.com",
                                        string.IsNullOrEmpty(dataTableResult.Rows[0].Field<string>("Pseudonym")) ? "admin" :
                                        dataTableResult.Rows[0].Field<string>("Pseudonym"));
                                    //Send email.
                                    SendEmail.Send(Subject, Template, dataTableResult.Rows[i]["CandidateEmail"].ToString(), eMailAddress, emailimagepath);
                                }
                                else
                                {
                                    Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                                    Template = Template.Replace("[PARTICIPANTEMAIL]", "");
                                    Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");
                                    //Send email.
                                    SendEmail.Send(Subject, Template, dataTableResult.Rows[i]["CandidateEmail"].ToString(), "");
                                }
                            }

                            lblMessage.Text = "Emails have been sent successfully to your selected participants.";
                            lblMessage2.Text = "Emails have been sent successfully to your selected participants.";
                            imbAssign.Enabled = true;
                        }
                        else if (dataTableToday != dataTableStartdate && dataTableStartdate >= dataTableToday)
                        {

                            lblMessage2.Text = "Emails will be send to your selected participants on date:" + dataTableStartdate.ToShortDateString();
                            //      imbAssign.Enabled = true;
                        }
                    }
                    else
                    {
                        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
                        List<Survey_Project_BE> projectBusinesEntityList = new List<Survey_Project_BE>();
                        //Get all project details by account id.
                        projectBusinesEntityList = projectBusinessAccessObject.GetProjectByID(Convert.ToInt32(assignQuestionnaireBusinesEntity.AccountID), Convert.ToInt32(assignQuestionnaireBusinesEntity.ProjecctID));

                        Survey_EmailTemplate_BAO emailTemplateBusinessAccessObject = new Survey_EmailTemplate_BAO();
                        List<Survey_EmailTemplate_BE> emailTemplateBusinesEntityList = new List<Survey_EmailTemplate_BE>();

                        string emailText = emailTemplateBusinesEntityList[0].EmailText;
                        string emailSubject = emailTemplateBusinesEntityList[0].Subject;

                        StringBuilder candidatelist = new StringBuilder();
                        candidatelist.Append("<table width='500' border='1' cellspacing='0'>");

                        candidatelist.Append("<tr><td width='50%'><b>Name</b></td><td width='50%'><b>Relationship</b></td></tr>");

                        DataTable dataTableColleagueList = new DataTable();
                        //Get all colleague
                        dataTableColleagueList = assignQuestionnaireBusinessAccessObject.GetColleaguesList(assignmentID);

                        string lineManagerName = "";
                        string lineManagerEmail = "";
                        string participantName = "";

                        //Create dynamic table by number of colleague.
                        for (int i = 0; i < dataTableColleagueList.Rows.Count; i++)
                        {
                            candidatelist.Append("<tr>");
                            candidatelist.Append("<td>");
                            candidatelist.Append(dataTableColleagueList.Rows[i]["CandidateName"].ToString());
                            candidatelist.Append("</td>");
                            candidatelist.Append("<td>");
                            candidatelist.Append(dataTableColleagueList.Rows[i]["Analysis_I"].ToString());
                            candidatelist.Append("</td>");

                            candidatelist.Append("<td>");
                            candidatelist.Append(dataTableColleagueList.Rows[i]["Analysis_II"].ToString());
                            candidatelist.Append("</td>");

                            candidatelist.Append("<td>");
                            candidatelist.Append(dataTableColleagueList.Rows[i]["Analysis_III"].ToString());
                            candidatelist.Append("</td>");

                            candidatelist.Append("</tr>");
                        }

                        candidatelist.Append("</table>");

                        string listOfNames = Convert.ToString(candidatelist);
                        string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
                        string asgnmentID = PasswordGenerator.EnryptString(Convert.ToString(assignmentID));
                        int candidateNumber = Convert.ToInt32(assignQuestionnaireBusinesEntity.CandidateNo);

                        emailText = emailText.Replace("[MANAGERFIRSTNAME]", lineManagerName);
                        emailText = emailText.Replace("[PARTICIPANTNAME]", participantName);
                        emailText = emailText.Replace("[LISTOFNAMES]", listOfNames);
                        emailText = emailText.Replace("[ACCEPT]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("1") + "' >Accept</a>");
                        emailText = emailText.Replace("[DECLINE]", "<a Target='_BLANK' href= '" + urlPath + "ProcessConfirmation.aspx?AsgnID=" + asgnmentID + "&CNo=" + PasswordGenerator.EnryptString(candidateNumber.ToString()) + "&Act=" + PasswordGenerator.EnryptString("0") + "' >Decline</a>");

                        //Send email
                        SendEmail.Send(emailSubject, emailText, lineManagerEmail, "");

                        lblMessage.Text = "Email has been sent successfully to Manager for further approval";
                        lblMessage2.Text = "Email has been sent successfully to Manager for further approval";
                        imbAssign.Enabled = true;
                    }

                    txtCandidateNo.Text = "";

                    rptrCandidateList.DataSource = null;
                    rptrCandidateList.DataBind();
                    /**Once Saved and emailed blank the Session of Imported value from EXCEL:bik****/
                    if (Session["Relation"] != null)
                    {
                        Session["Relation"] = null;
                    }
                }
                else
                {
                    lblvalidation.Text = "Please  fill Participant's information";
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////

                Survey_ReportManagement_BE reportManagementBusinesEntity = new Survey_ReportManagement_BE();
                reportManagementBusinesEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedItem.Value);
                reportManagementBusinesEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedItem.Value);
                reportManagementBusinesEntity.ProgramID = Convert.ToInt32(ddlProgramme.SelectedItem.Value);
                reportManagementBusinesEntity.ReportName = "Survey_" + reportManagementBusinesEntity.AccountID + reportManagementBusinesEntity.ProjectID + reportManagementBusinesEntity.ProgramID + ".pdf";

                Survey_ReportManagement_BAO reportManagementBusinessAccessObject = new Survey_ReportManagement_BAO();

                reportManagementBusinessAccessObject.AddParticipantReport(reportManagementBusinesEntity);

                /////////////////////////////////////update participant details////////////////////////////////////////////////////////////
                CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
                identity = this.Page.User.Identity as WADIdentity;
                int userid = Convert.ToInt16(identity.User.UserID);

                object[] newparam1 = new object[5] {
                        
                                                    null,
                                                    assignmentID,
                                                    ddlProject.SelectedItem.Value,
                                                    userid,
                                                       "I" 
                                                        };

                int newValue1 = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspAddPartictDetailsManagement", newparam1, null));
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Get all candidate list added to grid.
    /// </summary>
    /// <returns></returns>
    private List<Survey_AssignmentDetails_BE> GetCandidateList()
    {
        List<Survey_AssignmentDetails_BE> assignmentDetailsBusinesEntityList = new List<Survey_AssignmentDetails_BE>();
        bool flag = true;
        //Loop through repeator to find controls .
        foreach (RepeaterItem item in rptrCandidateList.Items)
        {
            DropDownList dropDownListAnalysis1 = (DropDownList)item.FindControl("ddlAnalysis1");
            DropDownList dropDownListAnalysis2 = (DropDownList)item.FindControl("ddlAnalysis2");
            DropDownList dropDownListAnalysis3 = (DropDownList)item.FindControl("ddlAnalysis3");
            TextBox textBoxCandidateName = (TextBox)item.FindControl("txtName");
            TextBox textBoxCandidateEmail = (TextBox)item.FindControl("txtEmailID");
        }

        //Loop through repeator to find controls .
        foreach (RepeaterItem item in rptrCandidateList.Items)
        {
            Survey_AssignmentDetails_BE assignmentDetailsBusinesEntity = new Survey_AssignmentDetails_BE();

            DropDownList dropDownListAnalysis1 = (DropDownList)item.FindControl("ddlAnalysis1");
            DropDownList dropDownListAnalysis2 = (DropDownList)item.FindControl("ddlAnalysis2");
            DropDownList dropDownListAnalysis3 = (DropDownList)item.FindControl("ddlAnalysis3");
            TextBox textBoxCandidateName = (TextBox)item.FindControl("txtName");
            TextBox textBoxCandidateEmail = (TextBox)item.FindControl("txtEmailID");
            //Set Properties value
            assignmentDetailsBusinesEntity.Analysis_I = dropDownListAnalysis1.SelectedValue;
            assignmentDetailsBusinesEntity.Analysis_II = dropDownListAnalysis2.SelectedValue;
            assignmentDetailsBusinesEntity.Analysis_III = dropDownListAnalysis3.SelectedValue;
            assignmentDetailsBusinesEntity.CandidateName = textBoxCandidateName.Text.Trim();
            assignmentDetailsBusinesEntity.CandidateEmail = textBoxCandidateEmail.Text.Trim();
            assignmentDetailsBusinesEntity.SubmitFlag = false;

            if (identity.User.GroupID == 1)//If user is super admin then account drop down value else user account id.
                UserAccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                UserAccountID = Convert.ToInt32(identity.User.AccountID);

            if (UserAccountID == Convert.ToInt32(ConfigurationManager.AppSettings["AccountID"].ToString()))
                assignmentDetailsBusinesEntity.EmailSendFlag = 0;
            else
                assignmentDetailsBusinesEntity.EmailSendFlag = 1;

            //Add to list
            assignmentDetailsBusinesEntityList.Add(assignmentDetailsBusinesEntity);
            email += textBoxCandidateEmail.Text.Trim() + ";";

            finalemail = email.TrimEnd(';');

        }

        return assignmentDetailsBusinesEntityList;
    }

    /// <summary>
    /// Reset controls value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtCandidateNo.Text = "";
            lblMessage.Text = "";
            lblMessage2.Text = "";
            lblvalidation.Text = "";

            rptrCandidateList.DataSource = null;
            rptrCandidateList.DataBind();

            if (Session["Relation"] != null)
            {
                Session["Relation"] = null;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// When click on add new button add three blank row to the last of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        imbAssign.Enabled = true;
        lblMessage.Text = "";
        lblMessage2.Text = "";
        lblvalidation.Text = "";

        if (txtCandidateNo.Text.Trim() != "" && Convert.ToInt32(ddlProgramme.SelectedValue) > 0)
        {
            int candidateCount = Convert.ToInt32(txtCandidateNo.Text.Trim());
            BindCandidateList(candidateCount);
        }
        else
            lblvalidation.Text = "Please select Program.";
    }

    /// <summary>
    /// Bind candidate gridview.
    /// </summary>
    /// <param name="candidateCount"></param>
    private void BindCandidateList(int candidateCount)
    {
        try
        {
            //Initilize candidate table 
            DataTable dataTableCandidate = new DataTable();
            dataTableCandidate.Columns.Add("ddlAnalysis1");
            dataTableCandidate.Columns.Add("ddlAnalysis2");
            dataTableCandidate.Columns.Add("ddlAnalysis3");
            dataTableCandidate.Columns.Add("Name");
            dataTableCandidate.Columns.Add("EmailAddress");
            /***************If imported from excel and adding more candidates:bik*********/
            if (Session["Relation"] != null)
            {
                dataTableCandidate = (DataTable)Session["Relation"];
                int intExistingRow = dataTableCandidate.Rows.Count;
                candidateCount = candidateCount - intExistingRow;
            }

            for (int count = 0; count < candidateCount; count++)
                dataTableCandidate.Rows.Add("", "", "", "", "");
            //then bind repeator with new colleague data table.
            rptrCandidateList.DataSource = dataTableCandidate;
            rptrCandidateList.DataBind();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind program by project selected value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Set Questionnaire
        Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dataTableQuestionnaire = new DataTable();
        //Get Questionnaire list in project
        dataTableQuestionnaire = questionnaireBusinessAccessObject.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableQuestionnaire.Rows.Count > 0)
        {
            //Bind Questionnaire drop down list
            ddlQuestionnaire.DataSource = dataTableQuestionnaire;
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();
        }

        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        if (ddlQuestionnaire.Items.Count > 1)
            ddlQuestionnaire.Items[1].Selected = true;

        //Set Programme
        Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();
        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get program list in project
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), 0, 0);

        if (dataTableProgramme.Rows.Count > 0)
        {
            //Bind program
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
    }

    /// <summary>
    ///  Upload participant logo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                    //Get file name.
                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);

                    //Get file unique name.
                    file = GetUniqueFilename(filename);

                    Session["FinalName"] = file;
                    //Set Path for file directory.
                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename); //UPload file 

                    DataTable dataTableProspective = new DataTable();

                    dataTableProspective = ReturnExcelDataTableMot(filename);
                    Session["Relation"] = dataTableProspective;
                    ccount = dataTableProspective.Rows.Count;

                    if (dataTableProspective != null && dataTableProspective.Rows.Count > 0)
                    {
                        rptrCandidateList.DataSource = dataTableProspective;
                        rptrCandidateList.DataBind();
                        txtCandidateNo.Text = ccount.ToString();
                    }
                    else
                    {
                        errorMessage(filename);
                    }

                    File.Delete(filename);
                }
                else
                {
                    lblMessage.Text = "Invalid file type";
                    lblMessage2.Text = "Invalid file type";
                }
            }
            else
            {
                lblvalidation.Text = "Please browse file to upload";
            }

            imbAssign.Enabled = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Validate wheter uploaded image is valide iin size or type.
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
    /// Import colleague data by reading Excel
    /// </summary>
    /// <param name="FullFileNamePath"></param>
    /// <returns></returns>
    public DataTable ReturnExcelDataTableMot(string FullFileNamePath)
    {
        DateTime dt3 = new DateTime();

        string SheetName = string.Empty;
        try
        {
            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
            int index = 0;
            object rowIndex = 2;

            DataTable dataTableExcel = new DataTable();

            dataTableExcel.Columns.Add("Analysis1", typeof(string));
            dataTableExcel.Columns.Add("Analysis2", typeof(string));
            dataTableExcel.Columns.Add("Analysis3", typeof(string));
            dataTableExcel.Columns.Add("Name", typeof(string));
            dataTableExcel.Columns.Add("EmailAddress", typeof(string));


            DataRow row;
            try
            {
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    //rowIndex = 2 + index;
                    row = dataTableExcel.NewRow();
                    DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

                    string projectId = ddlProject.SelectedValue.ToString();

                    DataTable dataTableAllProject = new DataTable();
                    object[] param1 = new object[3] { projectId, "2", 'P' };

                    dataTableAllProject = cDataSrc.ExecuteDataSet("Survey_UspProjectSelect", param1, null).Tables[0];

                    row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                    row[2] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);
                    row[3] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2);
                    row[4] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2);

                    index++;
                    rowIndex = 2 + index;
                    dataTableExcel.Rows.Add(row);
                }
            }
            catch
            {
                lblMessage.Text = "Please check your file data.";
                lblMessage2.Text = "Please check your file data.";

                dataTableExcel = null;
            }
            app.Workbooks.Close();

            return dataTableExcel;

            ////////////////string cDataSrc = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FullFileNamePath + ";Extended Properties=Excel 12.0";
            //////////////// OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
            //////////////// OleDbConnection objConn = new OleDbConnection(cDataSrc);
            //////////////// OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [Sheet1$]", objConn);
            //////////////// objAdapter1.SelectCommand = objCmdSelect;
            //////////////// // Create new DataSet to hold information from the worksheet.
            //////////////// DataSet objDataset1 = new DataSet();
            //////////////// // Fill the DataSet with the information from the worksheet.
            //////////////// objAdapter1.Fill(objDataset1, "XLData");
            //////////////// dtExcel = objDataset1.Tables[0];
            //////////////// return dtExcel;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Validation while uploading file.
    /// </summary>
    /// <param name="filename"></param>
    private void errorMessage(string filename)
    {
        lblMessage.Text = "Upload Failed.Please fill the Correct Field Value";
        lblMessage2.Text = "Upload Failed.Please fill the Correct Field Value";
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

    /// <summary>
    /// Bind project according to account value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account list by account id.
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = dataTableCompanyName.Select(Finalexpression);

            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
            {
                dataTableAccount.ImportRow(dataRowAccount);
            }
            //set company name
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
            //Get all project in an account and bind project dropdown.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();
            //When project drop down is bied then set Questionnaire and Programme dropdown to its default value.
            ddlQuestionnaire.Items.Clear();
            ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            //If account drop down value is 0 then reset other dropdown values. 
            lblcompanyname.Text = "";

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlQuestionnaire.Items.Clear();
            ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    /// <summary>
    /// It is of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /// <summary>
    ///Bind the  Analysis dropdown in grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrCandidateList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string stringAnalysis1 = "";
        string stringAnalysis2 = "";
        string stringAnalysis3 = "";
        RepeaterItem rpItem = e.Item;

        /***Showing Selected analysis if Imported : Bik*************/
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            stringAnalysis1 = Convert.ToString(dr[0]);
            stringAnalysis2 = Convert.ToString(dr[1]);
            stringAnalysis3 = Convert.ToString(dr[2]);
        }

        Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();
        DataTable dataTableCheckNoOfAnalysis = new DataTable();
        //Get number of analysis in an program.
        dataTableCheckNoOfAnalysis = programmeBusinessAccessObject.No_of_Analysis(Convert.ToInt32(ddlProgramme.SelectedValue));

        if (Convert.ToInt32(dataTableCheckNoOfAnalysis.Rows[0][0]) != 0)
        {
            DropDownList dropDownListAnalysis1 = (DropDownList)rpItem.FindControl("ddlAnalysis1");

            if (dropDownListAnalysis1 != null)
            {
                Survey_Programme_BAO programmeBusiness_Object = new Survey_Programme_BAO();
                DataTable dataTableAnalysis1 = new DataTable();
                //get Analysis 1 in a program.
                dataTableAnalysis1 = programmeBusiness_Object.GetAnalysis1(Convert.ToInt32(ddlProgramme.SelectedValue));

                dropDownListAnalysis1.Items.Clear();
                dropDownListAnalysis1.Items.Insert(0, new ListItem("Select", "0"));

                dropDownListAnalysis1.DataSource = dataTableAnalysis1;  // dt;
                //Bind analysis dropdown list.
                dropDownListAnalysis1.DataTextField = "Category_Detail";
                dropDownListAnalysis1.DataBind();
                dropDownListAnalysis1.SelectedValue = stringAnalysis1;
            }
        }

        if (Convert.ToInt32(dataTableCheckNoOfAnalysis.Rows[0][1]) != 0)
        {
            DropDownList dropDownListAnalysis2 = (DropDownList)rpItem.FindControl("ddlAnalysis2");

            if (dropDownListAnalysis2 != null)
            {

                Survey_Programme_BAO programmeBusiness_AccessObject = new Survey_Programme_BAO();
                DataTable dataTableAnalysis2 = new DataTable();
                //get Analysis 2 in a program.
                dataTableAnalysis2 = programmeBusiness_AccessObject.GetAnalysis2(Convert.ToInt32(ddlProgramme.SelectedValue));

                DataTable dataTable = new DataTable();

                dropDownListAnalysis2.Items.Clear();
                dropDownListAnalysis2.Items.Insert(0, new ListItem("Select", "0"));

                dropDownListAnalysis2.DataSource = dataTableAnalysis2;
                //Bind analysis dropdown list.
                dropDownListAnalysis2.DataTextField = "Category_Detail";
                dropDownListAnalysis2.DataBind();
                dropDownListAnalysis2.SelectedValue = stringAnalysis2;
            }
        }

        if (Convert.ToInt32(dataTableCheckNoOfAnalysis.Rows[0][2]) != 0)
        {
            DropDownList dropDownListAnalysis3 = (DropDownList)rpItem.FindControl("ddlAnalysis3");

            if (dropDownListAnalysis3 != null)
            {
                Survey_Programme_BAO programmeBusinessObject = new Survey_Programme_BAO();
                DataTable dataTableAnalysis3 = new DataTable();
                //get Analysis 3 in a program.
                dataTableAnalysis3 = programmeBusinessObject.GetAnalysis3(Convert.ToInt32(ddlProgramme.SelectedValue));

                DataTable dataTable = new DataTable();
                //  dt = (DataTable)Session["Analysis3"];

                dropDownListAnalysis3.Items.Clear();
                dropDownListAnalysis3.Items.Insert(0, new ListItem("Select", "0"));

                dropDownListAnalysis3.DataSource = dataTableAnalysis3;
                //Bind analysis dropdown list.
                dropDownListAnalysis3.DataTextField = "Category_Detail";
                dropDownListAnalysis3.DataBind();
                dropDownListAnalysis3.SelectedValue = stringAnalysis3;
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
        userid = Convert.ToString(identity.User.UserID);
        //Validate view event.
        if (ddlProject.SelectedValue == "0" || ddlProject.SelectedValue == "")
        {
            lblMessage2.Text = "Please Select Project to view the participant list.";
        }
        else
        {
            string stringProgramName = ddlProgramme.SelectedItem.ToString();
            string stringProject = ddlProject.SelectedItem.ToString();
            string stringQuestion = ddlQuestionnaire.SelectedItem.ToString();
            lblMessage.Text = "";
            lblMessage2.Text = "";
            //Open participant window to view details.
            ScriptManager.RegisterStartupScript(this, typeof(string), "print", "javascript:window.open('../../Survey_Module/Feedback/AssignQuestionnaireList.aspx?ProgrammeName=" + stringProgramName + "&ProjectName=" + stringProject.Replace("'", "~") + "&Questionair=" + stringQuestion + "&ProgId=" + ddlProgramme.SelectedValue + "&projectID=" + ddlProject.SelectedValue + "&userid=" + userid + "', 'CustomPopUp', " + "'width=1000, height=550, menubar=no, resizable=yes');", true);

        }
    }

    /// <summary>
    /// Bind gird accorgind to number of colleague in participant.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_AssignQstnParticipant_BAO participantBusinessAccessObject = new Survey_AssignQstnParticipant_BAO();
        identity = this.Page.User.Identity as WADIdentity;
        DataTable dataTableParticipant = null;

        if (ddlProgramme.SelectedIndex > 0)
        {
            //If user is super admin then dropdown accout value else user account value.
            if (identity.User.GroupID == 1)
            {
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(Convert.ToString(ddlAccountCode.SelectedValue), ddlProgramme.SelectedValue);
            }
            else
            {
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(Convert.ToString(identity.User.AccountID), ddlProgramme.SelectedValue);
            }

            Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
        }
    }

    /// <summary>
    /// It is of no use.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptrCandidateList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
}
