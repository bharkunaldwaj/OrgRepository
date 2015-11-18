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
    int ccount;
    StringBuilder sb = new StringBuilder();


    protected void Page_Load(object sender, EventArgs e)
    {
        System.GC.Collect();

        identity = this.Page.User.Identity as WADIdentity;
        int? grpID = Identity.User.GroupID;



        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        if (!IsPostBack)
        {




            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);

            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
            DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(userid);
            Survey_Project_BAO project_BAO = new Survey_Project_BAO();

            Account_BAO account_BAO = new Account_BAO();
            ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            if (identity.User.GroupID.ToString() != participantRoleId)
            {
                ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
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
                SetValues();

                trTargetPerson.Visible = false;

                Survey_AssignQuestionnaire_BAO assignQuestionnaire_BAO = new Survey_AssignQuestionnaire_BAO();
                Image imgHeader = (Image)Master.FindControl("imgProjectLogo");
                DataTable dtParticipantInfo = new DataTable();
                dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

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

    public void SetValues()
    {
        identity = this.Page.User.Identity as WADIdentity;

        Survey_AssignQuestionnaire_BAO assignquestionnaire_BAO = new Survey_AssignQuestionnaire_BAO();
        DataTable dtAssignDetails = new DataTable();
        dtAssignDetails = assignquestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();
        if (dtAssignDetails.Rows.Count > 0)
            ddlProject.SelectedValue = dtAssignDetails.Rows[0]["ProjecctID"].ToString();
        else
            ddlProject.SelectedValue = "";


        Questionnaire_BAO.Survey_Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dtQuestionnaire = new DataTable();
        dtQuestionnaire = questionnaire_BAO.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

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
        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue),0,0);

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
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

        dtAnalysis2 = programme_BAO.GetAnalysis2(Convert.ToInt32(ddlProgramme.SelectedValue));
        Session["Analysis2"] = dtAnalysis2;

        dtAnalysis3 = programme_BAO.GetAnalysis3(Convert.ToInt32(ddlProgramme.SelectedValue));
        Session["Analysis3"] = dtAnalysis3;
    }

    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            imbAssign.Enabled = false;

            if (rptrCandidateList.Items.Count != 0)
            {

                lblMessage.Text = "";
                lblMessage2.Text = "";
                lblvalidation.Text = "";
                HandleWriteLog("Start", new StackTrace(true));
                identity = this.Page.User.Identity as WADIdentity;

                Survey_AssignQuestionnaire_BE assignquestionnaire_BE = new Survey_AssignQuestionnaire_BE();
                Survey_AssignQuestionnaire_BAO assignquestionnaire_BAO = new Survey_AssignQuestionnaire_BAO();

                assignquestionnaire_BE.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
                assignquestionnaire_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
                assignquestionnaire_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();


                assignquestionnaire_BE.Description = "";// txtDescription.Text.Trim();
                identity = this.Page.User.Identity as WADIdentity;

                if (identity.User.GroupID == 1)
                {
                    assignquestionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                }
                else
                {
                    assignquestionnaire_BE.AccountID = identity.User.AccountID;
                }
                if (txtCandidateNo.Text.Trim() != "" || txtCandidateNo.Text.Trim() == "0")
                {
                    assignquestionnaire_BE.CandidateNo = Convert.ToInt32(txtCandidateNo.Text.Trim());
                }

                string qId = PasswordGenerator.EnryptString(assignquestionnaire_BE.QuestionnaireID.ToString());

                string path = ConfigurationManager.AppSettings["SurveyFeedbackURL"].ToString();
                string feedbackurl = path + "Feedback.aspx?QID=" + qId;


                assignquestionnaire_BE.ModifiedBy = 1;
                assignquestionnaire_BE.ModifiedDate = DateTime.Now;
                assignquestionnaire_BE.IsActive = 1;

                assignquestionnaire_BE.AssignmentDetails = GetCandidateList();
                Int32 assignmentID = 0;
                if ((assignquestionnaire_BE.AssignmentDetails.Count == assignquestionnaire_BE.CandidateNo + 1 || assignquestionnaire_BE.AssignmentDetails.Count >= 1) && assignquestionnaire_BE.CandidateNo != null)
                {
                    //Save Assign questionnaire
                    assignmentID = assignquestionnaire_BAO.AddAssignQuestionnaire(assignquestionnaire_BE);

                    DataTable dtResult = new DataTable();
                    dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireList(assignmentID);
                    DateTime dtStartdate = Convert.ToDateTime(dtResult.Rows[0]["StartDate"]).Date;
                    DateTime dtEnddate = Convert.ToDateTime(dtResult.Rows[0]["Enddate"]).Date;
                    DateTime dtToday = DateTime.Now.Date;
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

                    if (assignquestionnaire_BE.AccountID != Convert.ToInt32(accountID))
                    {
                        if (dtStartdate == dtToday && dtToday <= dtEnddate)
                        {
                            //Send mail to candidates
                            string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

                            for (int i = 0; i < loopCount; i++)
                            {
                                Survey_AccountUser_BAO accountUser_BAO = new Survey_AccountUser_BAO();
                                DataTable dtAccountAdmin = new DataTable();

                                //  dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.TargetPersonID));

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
                                string accountCode = "";
                                string strLogin = "";
                                string strPassword = "";

                                candidateEmail = dtResult.Rows[i]["CandidateEmail"].ToString();
                                questionnaireID = dtResult.Rows[i]["QuestionnaireID"].ToString();
                                candidateID = dtResult.Rows[i]["AsgnDetailID"].ToString();
                                OrganisationName = dtResult.Rows[i]["OrganisationName"].ToString();
                                Startdate = Convert.ToDateTime(dtResult.Rows[0]["StartDate"]).ToString("dd-MMM-yyyy");
                                Enddate = Convert.ToDateTime(dtResult.Rows[0]["Enddate"]).ToString("dd-MMM-yyyy");
                                CandidateName = dtResult.Rows[i]["CandidateName"].ToString();
                                string[] strFName = CandidateName.Split(' ');
                                FirstName = strFName[0].ToString();
                                accountCode = ddlAccountCode.SelectedItem.Text;
                                strLogin = strPassword = FirstName;
                                questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                                candidateID = PasswordGenerator.EnryptString(candidateID);

                                string urlPath = ConfigurationManager.AppSettings["SurveyFeedbackURL"].ToString();

                                string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";


                                Template = Template.Replace("[LINK]", link);
                                Template = Template.Replace("[NAME]", CandidateName);
                                Template = Template.Replace("[FIRSTNAME]", FirstName);
                                Template = Template.Replace("[COMPANY]", OrganisationName);
                                Template = Template.Replace("[STARTDATE]", Startdate);
                                Template = Template.Replace("[CLOSEDATE]", Enddate);
                                Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");
                                Template = Template.Replace("[CODE]", accountCode);
                                Template = Template.Replace("[LOGINID]", strLogin);
                                Template = Template.Replace("[PASSWORD]", strPassword);
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


                                    MailAddress maddr = new MailAddress("admin@i-comment360.com",
                                        string.IsNullOrEmpty(dtResult.Rows[0].Field<string>("Pseudonym")) ? "admin" :
                                        dtResult.Rows[0].Field<string>("Pseudonym"));

                                    SendEmail.Send(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString(), maddr, emailimagepath);
                                }
                                else
                                {
                                    Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                                    Template = Template.Replace("[PARTICIPANTEMAIL]", "");
                                    Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

                                    SendEmail.Send(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString(), "");
                                }

                            }

                            lblMessage.Text = "Emails have been sent successfully to your selected participants.";
                            lblMessage2.Text = "Emails have been sent successfully to your selected participants.";
                            imbAssign.Enabled = true;
                        }
                        else if (dtToday != dtStartdate && dtStartdate >= dtToday)
                        {

                            lblMessage2.Text = "Emails will be send to your selected participants on date:" + dtStartdate.ToShortDateString();
                            //      imbAssign.Enabled = true;
                        }



                    }
                    else
                    {


                        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
                        List<Survey_Project_BE> project_BEList = new List<Survey_Project_BE>();
                        project_BEList = project_BAO.GetProjectByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.ProjecctID));



                        Survey_EmailTemplate_BAO emailTemplate_BAO = new Survey_EmailTemplate_BAO();
                        List<Survey_EmailTemplate_BE> emailTemplate_BEList = new List<Survey_EmailTemplate_BE>();


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
                            candidatelist.Append(dtColleagueList.Rows[i]["Analysis_I"].ToString());
                            candidatelist.Append("</td>");

                            candidatelist.Append("<td>");
                            candidatelist.Append(dtColleagueList.Rows[i]["Analysis_II"].ToString());
                            candidatelist.Append("</td>");

                            candidatelist.Append("<td>");
                            candidatelist.Append(dtColleagueList.Rows[i]["Analysis_III"].ToString());
                            candidatelist.Append("</td>");

                            candidatelist.Append("</tr>");

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

                Survey_ReportManagement_BE rb = new Survey_ReportManagement_BE();
                rb.AccountID = Convert.ToInt32(ddlAccountCode.SelectedItem.Value);
                rb.ProjectID = Convert.ToInt32(ddlProject.SelectedItem.Value);
                rb.ProgramID = Convert.ToInt32(ddlProgramme.SelectedItem.Value);
                rb.ReportName = "Survey_" + rb.AccountID + rb.ProjectID + rb.ProgramID + ".pdf";
                Survey_ReportManagement_BAO r_bao = new Survey_ReportManagement_BAO();
                r_bao.AddParticipantReport(rb);

                /////////////////////////////////////////////////////////////////////////////////////////////////
                DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
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

    private List<Survey_AssignmentDetails_BE> GetCandidateList()
    {
        List<Survey_AssignmentDetails_BE> assignmentDetails_BEList = new List<Survey_AssignmentDetails_BE>();
        bool flag = true;

        foreach (RepeaterItem item in rptrCandidateList.Items)
        {
            DropDownList ddlAnalysis1 = (DropDownList)item.FindControl("ddlAnalysis1");
            DropDownList ddlAnalysis2 = (DropDownList)item.FindControl("ddlAnalysis2");
            DropDownList ddlAnalysis3 = (DropDownList)item.FindControl("ddlAnalysis3");
            TextBox txtCandidateName = (TextBox)item.FindControl("txtName");
            TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");
        }


        foreach (RepeaterItem item in rptrCandidateList.Items)
        {
            Survey_AssignmentDetails_BE assignmentDetails_BE = new Survey_AssignmentDetails_BE();

            DropDownList ddlAnalysis1 = (DropDownList)item.FindControl("ddlAnalysis1");
            DropDownList ddlAnalysis2 = (DropDownList)item.FindControl("ddlAnalysis2");
            DropDownList ddlAnalysis3 = (DropDownList)item.FindControl("ddlAnalysis3");
            TextBox txtCandidateName = (TextBox)item.FindControl("txtName");
            TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");

            assignmentDetails_BE.Analysis_I = ddlAnalysis1.SelectedValue;
            assignmentDetails_BE.Analysis_II = ddlAnalysis2.SelectedValue;
            assignmentDetails_BE.Analysis_III = ddlAnalysis3.SelectedValue;
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


            assignmentDetails_BEList.Add(assignmentDetails_BE);
            email += txtCandidateEmail.Text.Trim() + ";";

            finalemail = email.TrimEnd(';');

        }

        return assignmentDetails_BEList;
    }

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

    private void BindCandidateList(int candidateCount)
    {
        try
        {
            DataTable dtCandidate = new DataTable();
            dtCandidate.Columns.Add("ddlAnalysis1");
            dtCandidate.Columns.Add("ddlAnalysis2");
            dtCandidate.Columns.Add("ddlAnalysis3");
            dtCandidate.Columns.Add("Name");
            dtCandidate.Columns.Add("EmailAddress");
            /***************If imported from excel and adding more candidates:bik*********/
            if (Session["Relation"] != null)
            {
                dtCandidate = (DataTable)Session["Relation"];
                int intExistingRow = dtCandidate.Rows.Count;
                candidateCount = candidateCount - intExistingRow;
            }

            for (int count = 0; count < candidateCount; count++)
                dtCandidate.Rows.Add("", "", "", "", "");

            rptrCandidateList.DataSource = dtCandidate;
            rptrCandidateList.DataBind();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }


    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Set Questionnaire
        Questionnaire_BAO.Survey_Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dtQuestionnaire = new DataTable();
        dtQuestionnaire = questionnaire_BAO.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtQuestionnaire.Rows.Count > 0)
        {
            ddlQuestionnaire.DataSource = dtQuestionnaire;
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();
        }

        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlQuestionnaire.Items.Count > 1)
            ddlQuestionnaire.Items[1].Selected = true;

        //Set Programme
        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue),0,0);

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
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

                    DataTable dtProspective = new DataTable();
                    dtProspective = ReturnExcelDataTableMot(filename);
                    Session["Relation"] = dtProspective;
                    ccount = dtProspective.Rows.Count;

                    if (dtProspective != null && dtProspective.Rows.Count > 0)
                    {
                        rptrCandidateList.DataSource = dtProspective;
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

            dtExcel.Columns.Add("Analysis1", typeof(string));
            dtExcel.Columns.Add("Analysis2", typeof(string));
            dtExcel.Columns.Add("Analysis3", typeof(string));
            dtExcel.Columns.Add("Name", typeof(string));
            dtExcel.Columns.Add("EmailAddress", typeof(string));


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



                    dtAllProject = cDataSrc.ExecuteDataSet("Survey_UspProjectSelect", param1, null).Tables[0];

                   

                    row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                    row[2] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);
                    row[3] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2);
                    row[4] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2);


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



            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Survey_Project_BAO project_BAO = new Survey_Project_BAO();
            ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            ddlQuestionnaire.Items.Clear();
            ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));


        }
        else
        {
            lblcompanyname.Text = "";

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlQuestionnaire.Items.Clear();
            ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));


        }
    }

    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void rptrCandidateList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string strAnalysis1 = "";
        string strAnalysis2 = "";
        string strAnalysis3 = "";
        RepeaterItem rpItem = e.Item;


        /***Showing Selected analysis if Imported : Bik*************/
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            strAnalysis1 = Convert.ToString(dr[0]);
            strAnalysis2 = Convert.ToString(dr[1]);
            strAnalysis3 = Convert.ToString(dr[2]);
        }

        Survey_Programme_BAO programme_BAO_ana = new Survey_Programme_BAO();
        DataTable dtCheck_no_of_Analysis = new DataTable();
        dtCheck_no_of_Analysis = programme_BAO_ana.No_of_Analysis(Convert.ToInt32(ddlProgramme.SelectedValue));
        if (Convert.ToInt32(dtCheck_no_of_Analysis.Rows[0][0]) != 0)
        {
            DropDownList ddlAnalysis1 = (DropDownList)rpItem.FindControl("ddlAnalysis1");
            if (ddlAnalysis1 != null)
            {
                Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
                DataTable dtAnalysis1 = new DataTable();
                dtAnalysis1 = programme_BAO.GetAnalysis1(Convert.ToInt32(ddlProgramme.SelectedValue));


                ddlAnalysis1.Items.Clear();
                ddlAnalysis1.Items.Insert(0, new ListItem("Select", "0"));

                ddlAnalysis1.DataSource = dtAnalysis1;  // dt;

                ddlAnalysis1.DataTextField = "Category_Detail";
                ddlAnalysis1.DataBind();
                ddlAnalysis1.SelectedValue = strAnalysis1;
            }
        }


        if (Convert.ToInt32(dtCheck_no_of_Analysis.Rows[0][1]) != 0)
        {
            DropDownList ddlAnalysis2 = (DropDownList)rpItem.FindControl("ddlAnalysis2");
            if (ddlAnalysis2 != null)
            {

                Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
                DataTable dtAnalysis2 = new DataTable();
                dtAnalysis2 = programme_BAO.GetAnalysis2(Convert.ToInt32(ddlProgramme.SelectedValue));

                DataTable dt = new DataTable();


                ddlAnalysis2.Items.Clear();
                ddlAnalysis2.Items.Insert(0, new ListItem("Select", "0"));

                ddlAnalysis2.DataSource = dtAnalysis2;

                ddlAnalysis2.DataTextField = "Category_Detail";
                ddlAnalysis2.DataBind();
                ddlAnalysis2.SelectedValue = strAnalysis2;
            }
        }


        if (Convert.ToInt32(dtCheck_no_of_Analysis.Rows[0][2]) != 0)
        {
            DropDownList ddlAnalysis3 = (DropDownList)rpItem.FindControl("ddlAnalysis3");
            if (ddlAnalysis3 != null)
            {

                Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
                DataTable dtAnalysis3 = new DataTable();
                dtAnalysis3 = programme_BAO.GetAnalysis3(Convert.ToInt32(ddlProgramme.SelectedValue));

                DataTable dt = new DataTable();
                //  dt = (DataTable)Session["Analysis3"];

                ddlAnalysis3.Items.Clear();
                ddlAnalysis3.Items.Insert(0, new ListItem("Select", "0"));

                ddlAnalysis3.DataSource = dtAnalysis3;

                ddlAnalysis3.DataTextField = "Category_Detail";
                ddlAnalysis3.DataBind();
                ddlAnalysis3.SelectedValue = strAnalysis3;
            }
        }
    }


    protected void imbView_Click(object sender, ImageClickEventArgs e)
    {
        string userid = "";
        identity = this.Page.User.Identity as WADIdentity;
        userid = Convert.ToString(identity.User.UserID);

        if (ddlProject.SelectedValue == "0" || ddlProject.SelectedValue == "")
        {
            lblMessage2.Text = "Please Select Project to view the participant list.";
        }
        else
        {
            string strProgName = ddlProgramme.SelectedItem.ToString();
            string strProjct = ddlProject.SelectedItem.ToString();
            string strQuest = ddlQuestionnaire.SelectedItem.ToString();
            lblMessage.Text = "";
            lblMessage2.Text = "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "print", "javascript:window.open('../../Survey_Module/Feedback/AssignQuestionnaireList.aspx?ProgrammeName=" + strProgName + "&ProjectName=" + strProjct.Replace("'","~") + "&Questionair=" + strQuest + "&ProgId=" + ddlProgramme.SelectedValue + "&projectID=" + ddlProject.SelectedValue + "&userid=" + userid + "', 'CustomPopUp', " + "'width=1000, height=550, menubar=no, resizable=yes');", true);

        }
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_AssignQstnParticipant_BAO participant_BAO = new Survey_AssignQstnParticipant_BAO();
        identity = this.Page.User.Identity as WADIdentity;
        DataTable dtParticipant = null;

        if (ddlProgramme.SelectedIndex > 0)
        {
            if (identity.User.GroupID == 1)
            {
                dtParticipant = participant_BAO.GetdtAssignPartiList(Convert.ToString(ddlAccountCode.SelectedValue), ddlProgramme.SelectedValue);
            }
            else
            {
                dtParticipant = participant_BAO.GetdtAssignPartiList(Convert.ToString(identity.User.AccountID), ddlProgramme.SelectedValue);
            }

            Survey_Project_BAO project_BAO = new Survey_Project_BAO();

        }
    }
    protected void rptrCandidateList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
}
