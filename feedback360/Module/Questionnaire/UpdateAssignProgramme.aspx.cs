using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Configuration;

public partial class Module_Questionnaire_UpdateAssignProgramme : CodeBehindBase
{
    //int i;
    //string SqlType = string.Empty;
    //string filePath = string.Empty;
    //string strName = string.Empty;
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
    // StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        if (!IsPostBack)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);
            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            AssignQstnParticipant_BAO assignQuestionnaire = new AssignQstnParticipant_BAO();

            DataTable dataTableUserList = assignQuestionnaire.GetuseridAssignQuestionnaireList(userid);

            Project_BAO projectBusinessAccessObject = new Project_BAO();

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account list by user account id and bind accont dropdown.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            //If participant group and role is equal role
            if (identity.User.GroupID.ToString() != participantRoleId)
            {
                //get project by account id and bind Project list.
                ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
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

                AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
                Image imageHeader = (Image)Master.FindControl("imgProjectLogo");
                DataTable dataTableParticipantDetails = new DataTable();

                dataTableParticipantDetails = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                //Set Project Logo
                if (dataTableParticipantDetails.Rows[0]["Logo"].ToString() != "")
                {
                    imageHeader.Visible = true;
                    imageHeader.ImageUrl = "~/UploadDocs/" + dataTableParticipantDetails.Rows[0]["Logo"].ToString();
                }
            }

            //If user is super admin then show account dropdown else hide account dropdown.
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

    /// <summary>
    /// Save paricipant and program details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            lblvalidation.Text = "";

            int count = 0;
            // get selected participant count
            foreach (ListItem item in ddlTargetPerson.Items)
            {
                if (item.Selected == true)
                    count = count + 1;
            }

            if (count > 0)
            {
                foreach (ListItem targetPersonItem in ddlTargetPerson.Items)
                {//If participant is selected
                    if (targetPersonItem.Selected == true)
                    {
                        identity = this.Page.User.Identity as WADIdentity;

                        AssignQuestionnaire_BE assignquestionnaireBusinessEntity = new AssignQuestionnaire_BE();
                        AssignQuestionnaire_BAO assignquestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

                        assignquestionnaireBusinessEntity.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
                        assignquestionnaireBusinessEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
                        assignquestionnaireBusinessEntity.QuestionnaireID = 0;

                        //string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                        //if (ddlTargetPerson.Visible == false)
                        //{
                        //    assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(identity.User.UserID);
                        //}
                        //else
                        //{
                        //    assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
                        //}

                        assignquestionnaireBusinessEntity.TargetPersonID = Convert.ToInt32(targetPersonItem.Value);
                        assignquestionnaireBusinessEntity.Description = "";// txtDescription.Text.Trim();

                        //If user is super admin then account id is account dropdown value else user account id.
                        if (identity.User.GroupID == 1)
                        {
                            assignquestionnaireBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                        }
                        else
                        {
                            assignquestionnaireBusinessEntity.AccountID = identity.User.AccountID;
                        }

                        assignquestionnaireBusinessEntity.CandidateNo = 0;
                        assignquestionnaireBusinessEntity.NewProgrammeID = Convert.ToInt32(ddlNewProgramme.SelectedValue);
                        //Update moved program
                        assignquestionnaireBusinessAccessObject.UpdateAssignProgramme(assignquestionnaireBusinessEntity);
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

    /// <summary>
    /// Reset control's value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Bind program by project value.
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
        //Get project in a program and bind program and new program dropdown.
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
            //bind program drop down
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));


        ddlNewProgramme.Items.Clear();
        ddlNewProgramme.Items.Insert(0, new ListItem("Select", "0"));

        if (dataTableProgramme.Rows.Count > 0)
        { //bind new program drop down
            ddlNewProgramme.DataSource = dataTableProgramme;
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

    /// <summary>
    /// Bind company and project by account id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //get company details by account id.
            CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            DataTable dataTableAccount = CompanyName.Clone();

            foreach (DataRow datarowAccount in resultsAccount)
            {
                dataTableAccount.ImportRow(datarowAccount);
            }
            //set comapnyname
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

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

            Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

            ddlNewProgramme.Items.Clear();
            ddlNewProgramme.Items.Insert(0, new ListItem("Select", "0"));

            DataTable dataTableProgramme = new DataTable();
            //Get program details and bind new progran dropdown
            dataTableProgramme = programmeBusinessAccessObject.GetdtProgrammeList(Convert.ToString(ddlAccountCode.SelectedValue));

            if (dataTableProgramme.Rows.Count > 0)
            {
                ddlNewProgramme.DataSource = dataTableProgramme;
                ddlNewProgramme.DataTextField = "ProgrammeName";
                ddlNewProgramme.DataValueField = "ProgrammeID";
                ddlNewProgramme.DataBind();
            }

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Project_BAO projectBusinessAccessObject = new Project_BAO();
            //Get project list and bind project dropdown 
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
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
            //Ifaccount dropdown value is set to "select" then clear controls value.
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

    /// <summary>
    /// Bind target person by Program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participantBusinessAccessObject = new AssignQstnParticipant_BAO();
        identity = this.Page.User.Identity as WADIdentity;
        DataTable dataTableParticipant = null;

        if (ddlProgramme.SelectedIndex > 0)
        {
            //IF user is super admin then user account dropdown value else user account value.
            if (identity.User.GroupID == 1)
            {
                //Get participant value and bind participant dropdown
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(Convert.ToString(ddlAccountCode.SelectedValue), ddlProgramme.SelectedValue);
            }
            else
            {
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(Convert.ToString(identity.User.AccountID), ddlProgramme.SelectedValue);
            }

            Project_BAO project_BAO = new Project_BAO();

            if (dataTableParticipant.Rows.Count > 0)
            {
                ddlTargetPerson.Items.Clear();
                //ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
                //bind particpant dropdown
                ddlTargetPerson.DataSource = dataTableParticipant;
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

    /// <summary>
    /// Set value to controls
    /// </summary>
    public void SetValues()
    {
        identity = this.Page.User.Identity as WADIdentity;

        AssignQuestionnaire_BAO assignquestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
        DataTable dataTableAssignDetails = new DataTable();

        dataTableAssignDetails = assignquestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //Bind project by  user account id.
        ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

        ddlProject.SelectedValue = dataTableAssignDetails.Rows[0]["ProjecctID"].ToString();

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
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //get program by project selected
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();

            ddlProgramme.SelectedValue = dataTableAssignDetails.Rows[0]["ProgrammeID"].ToString();
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
}
