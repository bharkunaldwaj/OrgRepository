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

public partial class Module_Questionnaire_UpdateAssignProgramme : CodeBehindBase
{
    //int i;
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string strName = string.Empty;
    //bool flag = true;
    //int j;
    //string file1;
    //string filename1;
    string expression1;
    //string expression2;
    string Finalexpression;
    //string Finalexpression2;
    //string expression6;
    //string Finalexpression6;
    //string email;
    //string Template;
    //string finalemail;
    //string Questionnaire_id;
    //string mailid;
    WADIdentity identity;
    DataTable CompanyName;
    //DataTable dtAllAccount;
    //string expression5;
    //string Finalexpression5;
    //int targetpersonid;
    //int UserAccountID;
    //string ProjectId1;
    //string ProjectId2;
    //string ProjectId3;
    //string ProjectId5;
    //string Subject;

    StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        if (!IsPostBack)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);
            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
            DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(userid);
            Project_BAO project_BAO = new Project_BAO();

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

                //Programme_BAO programme_BAO = new Programme_BAO();

                //ddlNewProgramme.Items.Clear();
                //ddlNewProgramme.Items.Insert(0, new ListItem("Select", "0"));

                //DataTable dtProgramme = new DataTable();
                //dtProgramme = programme_BAO.GetdtProgrammeList(Convert.ToString(identity.User.AccountID));

                //if (dtProgramme.Rows.Count > 0)
                //{
                //    ddlNewProgramme.DataSource = dtProgramme;
                //    ddlNewProgramme.DataTextField = "ProgrammeName";
                //    ddlNewProgramme.DataValueField = "ProgrammeID";
                //    ddlNewProgramme.DataBind();
                //}

                ddlProject.Enabled = true;
                ddlProgramme.Enabled = true;
                //ddlQuestionnaire.Enabled = true;
            }
            else
            {
                SetValues();

                trTargetPerson.Visible = false;

                AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                Image imgHeader = (Image)Master.FindControl("imgProjectLogo");
                DataTable dtParticipantInfo = new DataTable();
                dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                //Set Project Logo
                if (dtParticipantInfo.Rows[0]["Logo"].ToString() != "")
                {
                    imgHeader.Visible = true;
                    imgHeader.ImageUrl = "~/UploadDocs/" + dtParticipantInfo.Rows[0]["Logo"].ToString();
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

        ddlProject.SelectedValue = dtAssignDetails.Rows[0]["ProjecctID"].ToString();

        //ddlProject.SelectedIndex = 1;

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

            ddlProgramme.SelectedValue = dtAssignDetails.Rows[0]["ProgrammeID"].ToString(); 
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        
        ddlProject.Enabled = false;
        ddlProgramme.Enabled = false;
        //ddlQuestionnaire.Enabled = false;

        //Set Relationship
        //DataTable dtRelationship = new DataTable();

        //dtRelationship = project_BAO.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        //Session["Relationship"] = dtRelationship;
    }

    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            lblvalidation.Text = "";

            int count=0;
            foreach (ListItem lstItem in ddlTargetPerson.Items)
            {
                if (lstItem.Selected == true)
                    count = count + 1;
            }

            if (count > 0)
            {
                foreach (ListItem lstItem in ddlTargetPerson.Items)
                {
                    if (lstItem.Selected == true)
                    {
                        identity = this.Page.User.Identity as WADIdentity;

                        AssignQuestionnaire_BE assignquestionnaire_BE = new AssignQuestionnaire_BE();
                        AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();

                        assignquestionnaire_BE.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
                        assignquestionnaire_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
                        assignquestionnaire_BE.QuestionnaireID = 0;

                        //string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                        //if (ddlTargetPerson.Visible == false)
                        //{
                        //    assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(identity.User.UserID);
                        //}
                        //else
                        //{
                        //    assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
                        //}

                        assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(lstItem.Value);
                        assignquestionnaire_BE.Description = "";// txtDescription.Text.Trim();

                        if (identity.User.GroupID == 1)
                        {
                            assignquestionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                        }
                        else
                        {
                            assignquestionnaire_BE.AccountID = identity.User.AccountID;
                        }

                        assignquestionnaire_BE.CandidateNo = 0;
                        assignquestionnaire_BE.NewProgrammeID = Convert.ToInt32(ddlNewProgramme.SelectedValue);

                        assignquestionnaire_BAO.UpdateAssignProgramme(assignquestionnaire_BE);
                    }
                }

                lblMessage.Text = "Programme moved successfully";

                //ddlProject.SelectedIndex = 0;
                //ddlProgramme.SelectedIndex = 0;
                //ddlQuestionnaire.SelectedIndex = 0;
                //ddlTargetPerson.SelectedIndex = 0;
                //txtDescription.Text = "";

                //HandleWriteLog("Start", new StackTrace(true));
            }
            else
            {
                lblvalidation.Text = "Please select atleast one participant";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            lblvalidation.Text = "";
            lblcompanyname.Text = "";

            ddlAccountCode.SelectedIndex = 0;

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlNewProgramme.Items.Clear();
            ddlNewProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
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


        ddlNewProgramme.Items.Clear();
        ddlNewProgramme.Items.Insert(0, new ListItem("Select", "0"));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlNewProgramme.DataSource = dtProgramme;
            ddlNewProgramme.DataTextField = "ProgrammeName";
            ddlNewProgramme.DataValueField = "ProgrammeID";
            ddlNewProgramme.DataBind();
        }

        //if (ddlProgramme.Items.Count > 1)
        //    ddlProgramme.Items[1].Selected = true;

        //Set Relationship
        //Project_BAO project_BAO = new Project_BAO();
        //DataTable dtRelationship = new DataTable();

        //dtRelationship = project_BAO.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        //Session["Relationship"] = dtRelationship;

        ddlTargetPerson.Items.Clear();
        //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
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

            Programme_BAO programme_BAO = new Programme_BAO();

            ddlNewProgramme.Items.Clear();
            ddlNewProgramme.Items.Insert(0, new ListItem("Select", "0"));

            DataTable dtProgramme = new DataTable();
            dtProgramme = programme_BAO.GetdtProgrammeList(Convert.ToString(ddlAccountCode.SelectedValue));

            if (dtProgramme.Rows.Count > 0)
            {
                ddlNewProgramme.DataSource = dtProgramme;
                ddlNewProgramme.DataTextField = "ProgrammeName";
                ddlNewProgramme.DataValueField = "ProgrammeID";
                ddlNewProgramme.DataBind();
            }

            

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
            //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
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

            ddlNewProgramme.Items.Clear();
            ddlNewProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participant_BAO = new AssignQstnParticipant_BAO();
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

            Project_BAO project_BAO = new Project_BAO();

            if (dtParticipant.Rows.Count > 0)
            {
                ddlTargetPerson.Items.Clear();
                //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));

                ddlTargetPerson.DataSource = dtParticipant;
                ddlTargetPerson.DataTextField = "UserName";
                ddlTargetPerson.DataValueField = "UserID";
                ddlTargetPerson.DataBind();
            }
            else
            {
                ddlTargetPerson.Items.Clear();
                //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }
}
