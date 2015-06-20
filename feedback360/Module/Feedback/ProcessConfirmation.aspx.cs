using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Admin_BE;
using Admin_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.Net.Mail;
using Miscellaneous;

public partial class Module_Feedback_ProcessConfirmation : System.Web.UI.Page
{
    AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            if (Request.QueryString["Act"] != null)
            {
                string action = PasswordGenerator.DecryptString(Request.QueryString["Act"].ToString());
                if (action == "1")
                    StartProcess();
                else
                    DeclineProcess();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeclineProcess()
    {
        if (Request.QueryString["AsgnID"] != null && Request.QueryString["CNo"] != null)
        {
            int assignmentID = Convert.ToInt32(PasswordGenerator.DecryptString(Request.QueryString["AsgnID"].ToString()));

            DataTable dtColleagueList = new DataTable();
            dtColleagueList = assignquestionnaire_BAO.GetColleaguesList(assignmentID);

            string lineManagerName = "";
            string lineManagerEmail = "";
            string participantName = "";

            for (int i = 0; i < dtColleagueList.Rows.Count; i++)
            {
                if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Manager")
                {
                    lineManagerName = dtColleagueList.Rows[i]["CandidateName"].ToString();
                    lineManagerEmail = dtColleagueList.Rows[i]["CandidateEmail"].ToString();
                }
                else if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Self")
                {
                    participantName = dtColleagueList.Rows[i]["CandidateName"].ToString();
                }
            }

            DataTable dtResult = new DataTable();
            dtResult = assignquestionnaire_BAO.GetUnsendEmailColleaguesList(assignmentID);

            if (dtResult.Rows.Count > 0)
            {
                int accountId = Convert.ToInt32(dtResult.Rows[0]["AccountID"]);
                int targetPersonId = Convert.ToInt32(dtResult.Rows[0]["TargetPersonID"]);

                AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                DataTable dtAccountAdmin = new DataTable();

                dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(accountId, targetPersonId);

                if (lineManagerEmail != "")
                {
                    MailAddress maddr = new MailAddress(lineManagerEmail, lineManagerName);

                    SendEmail.Send("Process Declined", "Dear " + participantName + "<br><br>Your participant list has been declined. Please contact your manager to agree a new selection and then update your choices via the live site. To do this login and select 'Set up your colleagues' where you can insert new participant details.  To delete any participants, select the ‘View colleagues’ option at the bottom of the page, and click the red cross that appears next to their details.<br><br>Regards,<br><br>" + lineManagerName, dtAccountAdmin.Rows[0]["EmailID"].ToString(), "");
                }
                else
                {
                    SendEmail.Send("Process Declined", "Dear " + participantName + "<br><br>Your participant list has been declined. Please contact your manager to agree a new selection and then update your choices via the live site. To do this login and select 'Set up your colleagues' where you can insert new participant details.  To delete any participants, select the ‘View colleagues’ option at the bottom of the page, and click the red cross that appears next to their details.<br><br>Manager", dtAccountAdmin.Rows[0]["EmailID"].ToString(), "");
                }
            }
        }

        lblMessage.Text = "Process declined";        
    }

    protected void StartProcess()
    {
        try
        {
            if (Request.QueryString["AsgnID"] != null && Request.QueryString["CNo"] != null)
            {
                int assignmentID = Convert.ToInt32(PasswordGenerator.DecryptString(Request.QueryString["AsgnID"].ToString()));
                int candidateNumber = Convert.ToInt32(PasswordGenerator.DecryptString(Request.QueryString["CNo"].ToString()));
                string Template = "";
                string Subject = "";

                DataTable dtResult = new DataTable();
                dtResult = assignquestionnaire_BAO.GetUnsendEmailColleaguesList(assignmentID);

                if (dtResult.Rows.Count > 0)
                {
                    int accountId = Convert.ToInt32(dtResult.Rows[0]["AccountID"]);
                    int projectId = Convert.ToInt32(dtResult.Rows[0]["ProjecctID"]);
                    int targetPersonId = Convert.ToInt32(dtResult.Rows[0]["TargetPersonID"]);
                    
                    candidateNumber = dtResult.Rows.Count;

                    int loopCount = 0;
                    //char loopFlag = 'N';

                    //for (int k = 0; k < Convert.ToInt32(candidateNumber); k++)
                    //{
                    //    if (dtResult.Rows[k]["RelationShip"].ToString() == "Self")
                    //        loopFlag = 'Y';
                    //}

                    //if (loopFlag == 'Y')
                    //    loopCount = Convert.ToInt32(candidateNumber) + 1;
                    //else
                        loopCount = Convert.ToInt32(candidateNumber);

                    //Send mail to candidates
                    string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

                    AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                    DataTable dtAccountAdmin = new DataTable();
                    
                    dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(accountId, targetPersonId);

                    for (int i = 0; i < loopCount; i++)
                    {
                        Template = assignquestionnaire_BAO.FindTemplate(projectId);
                        Subject = assignquestionnaire_BAO.FindCandidateSubjectTemplate(projectId);

                        // Get Candidate Email Image Name & Will Combined with EmailImagePath
                        DataTable dtCandidateEmailImage = new DataTable();
                        string emailimagepath = "";
                        dtCandidateEmailImage = assignquestionnaire_BAO.GetCandidateEmailImageInfo(projectId);
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

                        string urlPath = System.Configuration.ConfigurationManager.AppSettings["FeedbackURL"].ToString();

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

                                //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                                MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");
                                SendEmail.Send(Subject, Template, candidateEmail, maddr, emailimagepath);
                            }
                            else
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                                Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                                Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

                                SendEmail.Send(Subject, Template, candidateEmail, "");
                            }
                        }

                        assignquestionnaire_BAO.UpdateEmailSendFlag(Convert.ToInt32(PasswordGenerator.DecryptString(candidateID)));

                    }

                    lblMessage.Text = "Process approved successfully";

                    DataTable dtColleagueList = new DataTable();
                    dtColleagueList = assignquestionnaire_BAO.GetColleaguesList(assignmentID);

                    string lineManagerName = "";
                    string lineManagerEmail = "";
                    string participantName = "";

                    for (int i = 0; i < dtColleagueList.Rows.Count; i++)
                    {
                        if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Manager")
                        {
                            lineManagerName = dtColleagueList.Rows[i]["CandidateName"].ToString();
                            lineManagerEmail = dtColleagueList.Rows[i]["CandidateEmail"].ToString();
                        }
                        else if (dtColleagueList.Rows[i]["RelationShip"].ToString() == "Self")
                        {
                            participantName = dtColleagueList.Rows[i]["CandidateName"].ToString();
                        }
                    }

                    if (lineManagerEmail != "")
                    {
                        MailAddress maddr = new MailAddress(lineManagerEmail, lineManagerName);
                        SendEmail.Send("Process approved successfully", "Dear " + participantName + "<br><br>Your participant list has been approved and a link to the questionnaire has been sent to each of them. You can now logon to complete your self assessment.<br><br>Regards,<br><br>" + lineManagerName, dtAccountAdmin.Rows[0]["EmailID"].ToString(), "");
                    }
                    else
                    {
                        SendEmail.Send("Process approved successfully", "Dear " + participantName + "<br><br>Your participant list has been approved and a link to the questionnaire has been sent to each of them. You can now logon to complete your self assessment.<br><br>Manager", dtAccountAdmin.Rows[0]["EmailID"].ToString(), "");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}
