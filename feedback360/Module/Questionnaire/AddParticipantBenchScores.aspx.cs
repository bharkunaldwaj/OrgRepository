using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Module_Questionnaire_AddParticipantBenchScores : CodeBehindBase
{
    //Global variables.
    //string SqlType = string.Empty;
    //string filePath = string.Empty;
    //string strName = string.Empty;

    WADIdentity identity;
    DataTable CompanyName;
    //StringBuilder sb = new StringBuilder();
    DataTable dataTableCategoryScore = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get Account list by user account id.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            //SetValues();
            //If user is Super Admin then show accout dropdown else hide.
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
    /// Save bench mark score
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Initilize properties
            lblMessage.Text = "";
            lblvalidation.Text = "";

            identity = this.Page.User.Identity as WADIdentity;

            ParticipantBenchScore_BE participantBenchScoreBusinessEntity = new ParticipantBenchScore_BE();
            ParticipantBenchScore_BAO participantBenchScoreBusinessAccessObject = new ParticipantBenchScore_BAO();

            participantBenchScoreBusinessEntity.BenchmarkName = txtBenchScoreName.Text.Trim();
            participantBenchScoreBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
            participantBenchScoreBusinessEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
            participantBenchScoreBusinessEntity.QuestionnaireID = Convert.ToInt32(Session["QuestionnaireID"].ToString());
            //participantBenchScore_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
            participantBenchScoreBusinessEntity.Description = "";

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
                participantBenchScoreBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                participantBenchScoreBusinessEntity.AccountID = identity.User.AccountID;

            participantBenchScoreBusinessEntity.ScoreMonth = 0; // Convert.ToInt32(ddlScoreMonth.SelectedValue);
            participantBenchScoreBusinessEntity.ScoreYear = 0; // Convert.ToInt32(ddlScoreYear.SelectedValue);

            participantBenchScoreBusinessEntity.ModifiedBy = 1;
            participantBenchScoreBusinessEntity.ModifiedDate = DateTime.Now;
            participantBenchScoreBusinessEntity.IsActive = 1;

            participantBenchScoreBusinessEntity.ParticipantBenchScoreDetails = GetParticipantBenchScoreList();

            if (participantBenchScoreBusinessEntity.ParticipantBenchScoreDetails.Count > 0)
            {
                if (ddlTargetPerson.SelectedIndex > 0)
                {
                    participantBenchScoreBusinessEntity.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
                    Int32 assignmentID = participantBenchScoreBusinessAccessObject.AddParticipantBenchScore(participantBenchScoreBusinessEntity);
                }
                else
                {
                    foreach (ListItem itm in ddlTargetPerson.Items)
                    {
                        if (Convert.ToInt32(itm.Value) > 0)
                        {
                            participantBenchScoreBusinessEntity.TargetPersonID = Convert.ToInt32(itm.Value);
                            //Save the benchmark score.
                            Int32 assignmentID = participantBenchScoreBusinessAccessObject.AddParticipantBenchScore(participantBenchScoreBusinessEntity);
                        }
                    }
                }

                lblMessage.Text = "Participant's benchmark comparison scores saved successfully";
                //Bind category grid view to default value.
                rptrCategoryList.DataSource = null;
                rptrCategoryList.DataBind();
            }
            else
            {
                lblvalidation.Text = "Please fill participant's benchmark comparison score information";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
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
            lblMessage.Text = "";
            lblvalidation.Text = "";
            lblcompanyname.Text = "";

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));

            txtBenchScoreName.Text = "";

            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind program on project value change and fill bench mark score.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Set Questionnaire
        Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

        //ddlQuestionnaire.Items.Clear();
        DataTable dataTableQuestionnaire = new DataTable();
        dataTableQuestionnaire = questionnaireBusinessAccessObject.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableQuestionnaire.Rows.Count > 0)
            Session["QuestionnaireID"] = dataTableQuestionnaire.Rows[0]["QuestionnaireID"].ToString();
        else
            Session["QuestionnaireID"] = "0";

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
        DataTable dtProgramme = new DataTable();
        //Bind program dropdown list by project id. 
        dtProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

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
        Project_BAO projectBusinessAccessObject = new Project_BAO();
        DataTable dataTableRelationship = new DataTable();

        dataTableRelationship = projectBusinessAccessObject.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        Session["Relationship"] = dataTableRelationship;

        ddlTargetPerson.Items.Clear();
        ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
        //Fill bench mark score 
        FillCategoryBenchScoreData();
    }

    /// <summary>
    /// Bind project on account value change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account company by 
            CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));
            DataRow[] resultsAccount = CompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = CompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
            {
                dataTableAccount.ImportRow(drAccount);
            }
            //Set comapny name
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Project_BAO projectBusinessAccessObject = new Project_BAO();
            //Get project by account id and bind project drp down list.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            //ddlQuestionnaire.Items.Clear();
            //ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            //When project is binded clear program and participant.
            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));

            txtBenchScoreName.Text = "";
            //Bind repeator to default value.
            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }
        else
        {
            //If accoount value is set to "select" then reset all controls value.
            lblcompanyname.Text = "";

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            //ddlQuestionnaire.Items.Clear();
            //ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));

            txtBenchScoreName.Text = "";

            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }
    }

    /// <summary>
    /// Bind benchmark score and category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    protected void rptrCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }

    /// <summary>
    /// Bind participant by Program id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participantBusinessAccessObject = new AssignQstnParticipant_BAO();

        if (ddlProgramme.SelectedIndex > 0)
        {
            //Get participant details by account nand program id and bind participant dropdown list.
            DataTable dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
            Project_BAO projectBusinessAccessObject = new Project_BAO();

            if (dataTableParticipant.Rows.Count > 0)
            {
                ddlTargetPerson.Items.Clear();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));

                ddlTargetPerson.DataSource = dataTableParticipant;
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

    /// <summary>
    /// This is of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlQuestionnaire_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    /// <summary>
    /// This is of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    /// <summary>
    /// This is of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    /// <summary>
    /// Fill category by bench mark score.
    /// </summary>
    protected void FillCategoryBenchScoreData()
    {
        Category_BAO categoryBusinessAccessObject = new Category_BAO();
        //Get all category in account
        DataTable QuestionnaireCategory = categoryBusinessAccessObject.SelectQuestionnaireCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(Session["QuestionnaireID"].ToString()));
        QuestionnaireCategory.Columns.Add("Score");

        ParticipantBenchScore_BE participantBenchScore = new ParticipantBenchScore_BE();

        participantBenchScore.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
        participantBenchScore.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
        participantBenchScore.QuestionnaireID = Convert.ToInt32(Session["QuestionnaireID"].ToString()); ; // Convert.ToInt32(ddlQuestionnaire.SelectedValue);
        participantBenchScore.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
        participantBenchScore.ScoreMonth = 0; // Convert.ToInt32(ddlScoreMonth.SelectedValue);
        participantBenchScore.ScoreYear = 0; // Convert.ToInt32(ddlScoreYear.SelectedValue);

        ParticipantBenchScore_BAO participantBenchScoreBusinessAccessObject = new ParticipantBenchScore_BAO();
        //Get all scores
        dataTableCategoryScore = participantBenchScoreBusinessAccessObject.GetCategoryScore(participantBenchScore);

        //If datatable score has value then bind grid 
        if (dataTableCategoryScore != null && dataTableCategoryScore.Rows.Count > 0)
        {
            rptrCategoryList.DataSource = dataTableCategoryScore;
            rptrCategoryList.DataBind();

            txtBenchScoreName.Text = dataTableCategoryScore.Rows[0]["BenchmarkName"].ToString();
        }
        else if (QuestionnaireCategory.Rows.Count > 0)
        {
            //Bind category repetor with category only.
            rptrCategoryList.DataSource = QuestionnaireCategory;
            rptrCategoryList.DataBind();
        }
        else
        {
            //Bind with default value.
            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }
    }

    /// <summary>
    /// Bind bench mark value
    /// </summary>
    public void SetValues()
    {
        identity = this.Page.User.Identity as WADIdentity;

        AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
        DataTable dataTableAssignDetails = new DataTable();
        //Get Assignment information by user account Id.
        dataTableAssignDetails = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //bind project dropdown
        ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

        ddlProject.SelectedValue = dataTableAssignDetails.Rows[0]["ProjecctID"].ToString();

        Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

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
        DataTable dtProgramme = new DataTable();
        //By project value bind program dropdown
        dtProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();

            ddlProgramme.SelectedValue = dataTableAssignDetails.Rows[0]["ProgrammeID"].ToString();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ddlProject.Enabled = false;
        ddlProgramme.Enabled = false;
        //ddlQuestionnaire.Enabled = false;
    }

    /// <summary>
    /// Get score list of participant from gridview
    /// </summary>
    /// <returns></returns>
    private List<ParticipantBenchScoreDetails_BE> GetParticipantBenchScoreList()
    {
        List<ParticipantBenchScoreDetails_BE> participantBenchScoreDetailsList = new List<ParticipantBenchScoreDetails_BE>();
        //Add to list repeator score value.
        foreach (RepeaterItem item in rptrCategoryList.Items)
        {
            ParticipantBenchScoreDetails_BE participantBenchScoreDetails_BE = new ParticipantBenchScoreDetails_BE();

            Label labelCategoryId = (Label)item.FindControl("lblCategoryID");
            TextBox textBoxScore = (TextBox)item.FindControl("txtScore");

            participantBenchScoreDetails_BE.CategoryID = Convert.ToInt32(labelCategoryId.Text);

            if (textBoxScore.Text.Trim() != "")
                participantBenchScoreDetails_BE.Score = Convert.ToDecimal(textBoxScore.Text.Trim());
            else
                participantBenchScoreDetails_BE.Score = 0;

            participantBenchScoreDetailsList.Add(participantBenchScoreDetails_BE);
        }

        return participantBenchScoreDetailsList;
    }
}
