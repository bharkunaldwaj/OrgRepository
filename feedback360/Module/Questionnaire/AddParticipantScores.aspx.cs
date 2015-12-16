using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Configuration;

public partial class Module_Questionnaire_AddParticipantScores : CodeBehindBase
{
    //Global variable
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
            //Get account list by user Account Id 
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            //SetValues();
            //Bind score drop down list first
            BindScoreDropDown(ddlScoreYear1);
            //Bind score drop down list second
            BindScoreDropDown(ddlScoreYear2);

            //If user is super admin show account section else hide.
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
    /// Bind control values
    /// </summary>
    public void SetValues()
    {
        identity = this.Page.User.Identity as WADIdentity;

        AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
        DataTable dataTableAssignDetails = new DataTable();
        //Get assignmant information by user id.
        dataTableAssignDetails = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Project_BAO project_BAO = new Project_BAO();
        //Get project value by  user account id and bind project dropdown.
        ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

        ddlProject.SelectedValue = dataTableAssignDetails.Rows[0]["ProjecctID"].ToString();

        Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dataTableQuestionnaire = new DataTable();
        //Get questionnaire list by user account id and bind questionnaire dropdown.
        dataTableQuestionnaire = questionnaireBusinessAccessObject.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableQuestionnaire.Rows.Count > 0)
        {
            ddlQuestionnaire.DataSource = dataTableQuestionnaire;
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();

            ddlQuestionnaire.SelectedValue = dataTableAssignDetails.Rows[0]["QuestionnaireID"].ToString();
        }

        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        //Set Programme
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get program list by Project Id and bind program dropdownlist.
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
        ddlQuestionnaire.Enabled = false;

    }

    /// <summary>
    /// Save category score details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            lblvalidation.Text = "";

            identity = this.Page.User.Identity as WADIdentity;

            ParticipantScore_BE participantScoreBusinessEntity = new ParticipantScore_BE();
            ParticipantScore_BAO participantScoreBusinessAccessObject = new ParticipantScore_BAO();

            participantScoreBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
            participantScoreBusinessEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
            participantScoreBusinessEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (ddlTargetPerson.Visible == false)
                participantScoreBusinessEntity.TargetPersonID = Convert.ToInt32(identity.User.UserID);
            else
                participantScoreBusinessEntity.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);

            participantScoreBusinessEntity.Description = "";
            identity = this.Page.User.Identity as WADIdentity;

            //If user is super Admin then Account drop down value else userAccount id.
            if (identity.User.GroupID == 1)
                participantScoreBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                participantScoreBusinessEntity.AccountID = identity.User.AccountID;

            participantScoreBusinessEntity.ScoreMonth = 0;// Convert.ToInt32(ddlScoreMonth.SelectedValue);
            participantScoreBusinessEntity.ScoreYear = 0;// Convert.ToInt32(ddlScoreYear.SelectedValue);

            participantScoreBusinessEntity.ModifiedBy = 1;
            participantScoreBusinessEntity.ModifiedDate = DateTime.Now;
            participantScoreBusinessEntity.IsActive = 1;

            //Get participant score 1
            participantScoreBusinessEntity.ParticipantScore1Details = GetParticipantScore1List();
            //Get participant score 2
            participantScoreBusinessEntity.ParticipantScore2Details = GetParticipantScore2List();

            //If score one and score two has value > 0
            if (participantScoreBusinessEntity.ParticipantScore1Details.Count > 0 || participantScoreBusinessEntity.ParticipantScore2Details.Count > 0)
            {
                //Save Assign questionnaire
                Int32 assignmentID = participantScoreBusinessAccessObject.AddParticipantScore(participantScoreBusinessEntity);

                lblMessage.Text = "Participant's score saved successfully";

                //Bind category grid with blank.
                rptrCategoryList.DataSource = null;
                rptrCategoryList.DataBind();

                //Bind score grid with blank.
                rptrPreviousScore2.DataSource = null;
                rptrPreviousScore2.DataBind();
            }
            else
            {
                lblvalidation.Text = "Please  fill participant's score information";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Get Participant score one
    /// </summary>
    /// <returns></returns>
    private List<ParticipantScoreDetails_BE> GetParticipantScore1List()
    {
        List<ParticipantScoreDetails_BE> participantScoreDetailsList = new List<ParticipantScoreDetails_BE>();
        //Loop through category list to get category scores.
        foreach (RepeaterItem item in rptrCategoryList.Items)
        {
            ParticipantScoreDetails_BE participantScoreDetailsBusinessEntity = new ParticipantScoreDetails_BE();

            Label labelCategoryId = (Label)item.FindControl("lblCategoryID");
            TextBox textBoxScore = (TextBox)item.FindControl("txtScore1");

            participantScoreDetailsBusinessEntity.ScoreType = 1;
            //Get first month value
            participantScoreDetailsBusinessEntity.Month = Convert.ToInt32(ddlScoreMonth1.SelectedValue);
            //Get second year value
            participantScoreDetailsBusinessEntity.Year = Convert.ToInt32(ddlScoreYear1.SelectedValue);
            participantScoreDetailsBusinessEntity.CategoryID = Convert.ToInt32(labelCategoryId.Text);

            if (textBoxScore.Text.Trim() != "")
            {
                //Get Scores
                participantScoreDetailsBusinessEntity.Score = Convert.ToDecimal(textBoxScore.Text.Trim());
                //Add to list
                participantScoreDetailsList.Add(participantScoreDetailsBusinessEntity);
            }
            else
                participantScoreDetailsBusinessEntity.Score = 0;


        }

        return participantScoreDetailsList;
    }

    /// <summary>
    /// Get Participant score second
    /// </summary>
    /// <returns></returns>
    private List<ParticipantScoreDetails_BE> GetParticipantScore2List()
    {
        List<ParticipantScoreDetails_BE> participantScoreDetailsList = new List<ParticipantScoreDetails_BE>();

        foreach (RepeaterItem item in rptrPreviousScore2.Items)
        {
            ParticipantScoreDetails_BE participantScoreDetailsBusinessEntity = new ParticipantScoreDetails_BE();

            Label labelCategoryId = (Label)item.FindControl("lblCategoryID");
            TextBox textBoxScore = (TextBox)item.FindControl("txtScore2");

            participantScoreDetailsBusinessEntity.ScoreType = 2;
            participantScoreDetailsBusinessEntity.Month = Convert.ToInt32(ddlScoreMonth2.SelectedValue);
            participantScoreDetailsBusinessEntity.Year = Convert.ToInt32(ddlScoreYear2.SelectedValue);
            participantScoreDetailsBusinessEntity.CategoryID = Convert.ToInt32(labelCategoryId.Text);

            if (textBoxScore.Text.Trim() != "")
            {
                participantScoreDetailsBusinessEntity.Score = Convert.ToDecimal(textBoxScore.Text.Trim());
                participantScoreDetailsList.Add(participantScoreDetailsBusinessEntity);
            }
            else
            {
                participantScoreDetailsBusinessEntity.Score = 0;
            }
        }

        return participantScoreDetailsList;
    }

    /// <summary>
    /// Reset control value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            lblvalidation.Text = "";

            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind Questionnaire on project selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Set Questionnaire
        Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dataTableQuestionnaire = new DataTable();
        //Get Questionnaire
        dataTableQuestionnaire = questionnaireBusinessAccessObject.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableQuestionnaire.Rows.Count > 0)
        {
            ddlQuestionnaire.DataSource = dataTableQuestionnaire;
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();
        }

        //ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        //if (ddlQuestionnaire.Items.Count > 1)
        //    ddlQuestionnaire.Items[1].Selected = true;

        //Set Programme
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get Program list by project id and bind program dropdown.
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
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
        DataTable dtRelationship = new DataTable();
        //Get the project relation
        dtRelationship = projectBusinessAccessObject.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
        Session["Relationship"] = dtRelationship;

        ddlTargetPerson.Items.Clear();
        ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
    }

    /// <summary>
    /// Bind project on Account selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO accountBusinessAccessObject = new Account_BAO();

            CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = CompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = CompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
            {
                dataTableAccount.ImportRow(dataRowAccount);
            }
            //Set company name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Project_BAO projectBusinessAccessObject = new Project_BAO();
            //Get project list by account id and bind project dropdown.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            ddlQuestionnaire.Items.Clear();
            ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            //If account dropdown is set to "select" thens reset controls value.
            lblcompanyname.Text = "";

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlQuestionnaire.Items.Clear();
            ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ddlTargetPerson.Items.Clear();
            ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    /// <summary>
    /// Fill both month and year dropdown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData1();
        FillCategoryScoreData2();
    }

    /// <summary>
    /// It is of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }

    /// <summary>
    /// Bind participant by program id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participantBusinessAccessObject = new AssignQstnParticipant_BAO();

        if (ddlProgramme.SelectedIndex > 0)
        {
            //Get participant details 
            DataTable dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
            Project_BAO projectBusinessAccessObject = new Project_BAO();

            if (dataTableParticipant.Rows.Count > 0)
            {
                ddlTargetPerson.Items.Clear();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
                //Bind participant by project.
                ddlTargetPerson.DataSource = dataTableParticipant;
                ddlTargetPerson.DataTextField = "UserName";
                ddlTargetPerson.DataValueField = "UserID";
                ddlTargetPerson.DataBind();
            }
            else
            {
                //if no participant then blank.
                ddlTargetPerson.Items.Clear();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }

    /// <summary>
    /// It is of No use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlQuestionnaire_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Fill score one by month
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData1();
    }

    /// <summary>
    /// Fill score second by month
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData1();
    }

    /// <summary>
    /// Bind category list 
    /// </summary>
    protected void FillCategoryScoreData1()
    {
        Category_BAO categoryBusinessAccessObject = new Category_BAO();
        //Get questionnaire category
        DataTable QuestionnaireCategory = categoryBusinessAccessObject.SelectQuestionnaireCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue));
        QuestionnaireCategory.Columns.Add("Score1");

        ParticipantScore_BE participantScoreBusinessEntity = new ParticipantScore_BE();
        //Set properties value.
        participantScoreBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
        participantScoreBusinessEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
        participantScoreBusinessEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
        participantScoreBusinessEntity.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
        participantScoreBusinessEntity.ScoreMonth = Convert.ToInt32(ddlScoreMonth1.SelectedValue);
        participantScoreBusinessEntity.ScoreYear = Convert.ToInt32(ddlScoreYear1.SelectedValue);

        ParticipantScore_BAO participantScore_BAO = new ParticipantScore_BAO();
        dataTableCategoryScore = participantScore_BAO.GetCategoryScore1(participantScoreBusinessEntity);

        //Bind gridwith scores
        if (dataTableCategoryScore != null && dataTableCategoryScore.Rows.Count > 0)
        {
            // Select dropdown with existing record
            ddlScoreMonth1.ClearSelection();
            ddlScoreYear1.ClearSelection();

            ListItem litem = ddlScoreMonth1.Items.FindByValue(Convert.ToString(dataTableCategoryScore.Rows[0]["ScoreMonth"]));

            if (litem != null)
                litem.Selected = true;
            litem = ddlScoreYear1.Items.FindByValue(Convert.ToString(dataTableCategoryScore.Rows[0]["ScoreYear"]));

            if (litem != null)
                litem.Selected = true;
            //Bind category repetor
            rptrCategoryList.DataSource = dataTableCategoryScore;
            rptrCategoryList.DataBind();
        }
        else if (QuestionnaireCategory.Rows.Count > 0)
        {
            //bind grid with category
            rptrCategoryList.DataSource = QuestionnaireCategory;
            rptrCategoryList.DataBind();
        }
        else
        {
            //If no category and score then null
            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }
    }

    /// <summary>
    /// Bind category score by month
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreMonth2_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData2();
    }

    /// <summary>
    /// bind category score by year
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreYear2_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData2();
    }

    /// <summary>
    /// Fill category score if no score then 0.0
    /// </summary>
    protected void FillCategoryScoreData2()
    {
        Category_BAO categoryBusinessAccessObject = new Category_BAO();
        //Get category by account and questionnaire id.
        DataTable QuestionnaireCategory = categoryBusinessAccessObject.SelectQuestionnaireCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue));

        QuestionnaireCategory.Columns.Add("Score2");

        ParticipantScore_BE participantScoreBusinessEntity = new ParticipantScore_BE();
        //Set control value by details
        participantScoreBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
        participantScoreBusinessEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
        participantScoreBusinessEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
        participantScoreBusinessEntity.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
        participantScoreBusinessEntity.ScoreMonth = Convert.ToInt32(ddlScoreMonth2.SelectedValue);
        participantScoreBusinessEntity.ScoreYear = Convert.ToInt32(ddlScoreYear2.SelectedValue);

        ParticipantScore_BAO participantScore_BAO = new ParticipantScore_BAO();

        dataTableCategoryScore = participantScore_BAO.GetCategoryScore2(participantScoreBusinessEntity);

        //If category score is >0 then bind grid.
        if (dataTableCategoryScore != null && dataTableCategoryScore.Rows.Count > 0)
        {
            // Select dropdown with existing record
            ddlScoreMonth2.ClearSelection();
            ddlScoreYear2.ClearSelection();

            ListItem litem = ddlScoreMonth2.Items.FindByValue(Convert.ToString(dataTableCategoryScore.Rows[0]["ScoreMonth"]));

            if (litem != null)
                litem.Selected = true;

            litem = ddlScoreYear2.Items.FindByValue(Convert.ToString(dataTableCategoryScore.Rows[0]["ScoreYear"]));

            if (litem != null)
                litem.Selected = true;

            rptrPreviousScore2.DataSource = dataTableCategoryScore;
            rptrPreviousScore2.DataBind();
        }
        else if (QuestionnaireCategory.Rows.Count > 0)
        {
            //if questionnaire has score then bindscore grid
            rptrPreviousScore2.DataSource = QuestionnaireCategory;
            rptrPreviousScore2.DataBind();
        }
        else
        {
            //if questionnaire has no score then bind blank grid
            rptrPreviousScore2.DataSource = null;
            rptrPreviousScore2.DataBind();
        }
    }

    /// <summary>
    /// Bind score drop down from year 2008 to current year
    /// </summary>
    /// <param name="dropDownListControl"></param>
    private void BindScoreDropDown(DropDownList dropDownListControl)
    {
        dropDownListControl.Items.Add(new ListItem("Select", "0"));

        for (int i = 2008; i <= DateTime.Now.Year; i++)//Loop from 2008 to current year for every new year
        {
            string dataField = i.ToString().Trim();
            dropDownListControl.Items.Add(new ListItem(dataField, dataField));
        }
    }
}
