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

public partial class Survey_Module_Questionnaire_AddParticipantScores : CodeBehindBase 
{
    //global variable.
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string strName = string.Empty;
    
    WADIdentity identity;
    DataTable CompanyName;
    StringBuilder sb = new StringBuilder();
    DataTable dtCategoryScore = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);

            Account_BAO account_BAO = new Account_BAO();
            //Get account list by user Account Id 
            ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();
            
            //SetValues();
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
    /// Its of no use.
    /// </summary>
    public void SetValues()
    {
        identity = this.Page.User.Identity as WADIdentity;

        AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        DataTable dtAssignDetails = new DataTable();
        dtAssignDetails = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        Project_BAO project_BAO = new Project_BAO();
        ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

        ddlProject.SelectedValue = dtAssignDetails.Rows[0]["ProjecctID"].ToString();

        Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

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
            //Initilize properties
            ParticipantScore_BE participantScore_BE = new ParticipantScore_BE();
            ParticipantScore_BAO participantScore_BAO = new ParticipantScore_BAO();

            participantScore_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
            participantScore_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
            participantScore_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (ddlTargetPerson.Visible == false)
                participantScore_BE.TargetPersonID = Convert.ToInt32(identity.User.UserID);
            else
                participantScore_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);

            participantScore_BE.Description = "";
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
                participantScore_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                participantScore_BE.AccountID = identity.User.AccountID;

            participantScore_BE.ScoreMonth = 0;// Convert.ToInt32(ddlScoreMonth.SelectedValue);
            participantScore_BE.ScoreYear = 0;// Convert.ToInt32(ddlScoreYear.SelectedValue);

            participantScore_BE.ModifiedBy = 1;
            participantScore_BE.ModifiedDate = DateTime.Now;
            participantScore_BE.IsActive = 1;
            //Get participant score 1
            participantScore_BE.ParticipantScore1Details = GetParticipantScore1List();
            //Get participant score 2
            participantScore_BE.ParticipantScore2Details = GetParticipantScore2List();

            if (participantScore_BE.ParticipantScore1Details.Count > 0 && participantScore_BE.ParticipantScore2Details.Count > 0)
            {
                //Save Assign questionnaire
                Int32 assignmentID = participantScore_BAO.AddParticipantScore(participantScore_BE);

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
        List<ParticipantScoreDetails_BE> participantScoreDetails_BEList = new List<ParticipantScoreDetails_BE>();
        //Loop through category list to get category scores.
        foreach (RepeaterItem item in rptrCategoryList.Items)
        {
            ParticipantScoreDetails_BE participantScoreDetails_BE = new ParticipantScoreDetails_BE();

            Label lblCategoryId = (Label)item.FindControl("lblCategoryID");
            TextBox txtScore = (TextBox)item.FindControl("txtScore1");

            participantScoreDetails_BE.ScoreType = 1;
            //Get first month value
            participantScoreDetails_BE.Month = Convert.ToInt32(ddlScoreMonth1.SelectedValue);
            //Get second year value
            participantScoreDetails_BE.Year = Convert.ToInt32(ddlScoreYear1.SelectedValue);
            participantScoreDetails_BE.CategoryID =Convert.ToInt32(lblCategoryId.Text);
            
            if (txtScore.Text.Trim()!="")
                participantScoreDetails_BE.Score = Convert.ToDecimal(txtScore.Text.Trim());//Get Scores
            else
                participantScoreDetails_BE.Score = 0;
            //Add to list
            participantScoreDetails_BEList.Add(participantScoreDetails_BE);
        }

        return participantScoreDetails_BEList;
    }

    /// <summary>
    /// Get Participant score second
    /// </summary>
    /// <returns></returns>
    private List<ParticipantScoreDetails_BE> GetParticipantScore2List()
    {
        List<ParticipantScoreDetails_BE> participantScoreDetails_BEList = new List<ParticipantScoreDetails_BE>();
        //Loop through category list to get category scores.
        foreach (RepeaterItem item in rptrPreviousScore2.Items)
        {
            ParticipantScoreDetails_BE participantScoreDetails_BE = new ParticipantScoreDetails_BE();

            Label lblCategoryId = (Label)item.FindControl("lblCategoryID");
            TextBox txtScore = (TextBox)item.FindControl("txtScore2");

            participantScoreDetails_BE.ScoreType = 2;
            //Get first month value
            participantScoreDetails_BE.Month = Convert.ToInt32(ddlScoreMonth2.SelectedValue);
            //Get second year value
            participantScoreDetails_BE.Year = Convert.ToInt32(ddlScoreYear2.SelectedValue);
            participantScoreDetails_BE.CategoryID = Convert.ToInt32(lblCategoryId.Text);

            if (txtScore.Text.Trim() != "")
                participantScoreDetails_BE.Score = Convert.ToDecimal(txtScore.Text.Trim()); //Get Scores
            else
                participantScoreDetails_BE.Score = 0;

            participantScoreDetails_BEList.Add(participantScoreDetails_BE);//Add to list
        }

        return participantScoreDetails_BEList;
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
        Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

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
        //if (ddlQuestionnaire.Items.Count > 1)
        //    ddlQuestionnaire.Items[1].Selected = true;

        //Set Programme
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        //Get Program list by project id and bind program dropdown.
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
        //Get the project relation.
        dtRelationship = project_BAO.GetProjectRelationship(Convert.ToInt32(ddlProject.SelectedValue));
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
            Account_BAO account_BAO = new Account_BAO();

            CompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));
            DataRow[] resultsAccount = CompanyName.Select("AccountID='" + companycode + "'");
            DataTable dtAccount = CompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
            {
                dtAccount.ImportRow(drAccount);
            }
            //Set company name.
            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
            
            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Project_BAO project_BAO = new Project_BAO();
            //Get project list by account id and bind project dropdown.
            ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
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
            lblcompanyname.Text = "";
            //If account dropdown is set to "select" thens reset controls value.
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
    /// No user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    /// <summary>
    /// No user
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
        AssignQstnParticipant_BAO participant_BAO = new AssignQstnParticipant_BAO();

        if (ddlProgramme.SelectedIndex > 0)
        {
            //Get participant details 
            DataTable dtParticipant = participant_BAO.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
            Project_BAO project_BAO = new Project_BAO();

            if (dtParticipant.Rows.Count > 0)
            {
                ddlTargetPerson.Items.Clear();
                ddlTargetPerson.Items.Insert(0, new ListItem("Select", "0"));
                //Bind participant by project.
                ddlTargetPerson.DataSource = dtParticipant;
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
    /// Fiil category score table.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlQuestionnaire_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData1();
        FillCategoryScoreData2();
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
    /// Fill score second .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData1();
    }

    /// <summary>
    /// Bind category score list 
    /// </summary>
    protected void FillCategoryScoreData1()
    {
        Category_BAO category_BAO = new Category_BAO();
        //Get questionnaire category
        DataTable QuestionnaireCategory = category_BAO.SelectQuestionnaireCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue));
        QuestionnaireCategory.Columns.Add("Score1");
        
        ParticipantScore_BE participantScore_BE = new ParticipantScore_BE();
        //Set properties value.
        participantScore_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
        participantScore_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
        participantScore_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
        participantScore_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
        participantScore_BE.ScoreMonth = Convert.ToInt32(ddlScoreMonth1.SelectedValue);
        participantScore_BE.ScoreYear = Convert.ToInt32(ddlScoreYear1.SelectedValue);

        ParticipantScore_BAO participantScore_BAO = new ParticipantScore_BAO();
        dtCategoryScore = participantScore_BAO.GetCategoryScore1(participantScore_BE);
        //Bind grid with scores
        if (dtCategoryScore != null && dtCategoryScore.Rows.Count > 0)
        {
            rptrCategoryList.DataSource = dtCategoryScore;
            rptrCategoryList.DataBind();
        }
        else if (QuestionnaireCategory.Rows.Count > 0)
        {
            rptrCategoryList.DataSource = QuestionnaireCategory; //bind gris with category
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
    /// Bind category score  by second month
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlScoreMonth2_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryScoreData2();
    }

    /// <summary>
    /// bind category score by second year
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
        Category_BAO category_BAO = new Category_BAO();
        //Get category by account and questionnaire id.
        DataTable QuestionnaireCategory = category_BAO.SelectQuestionnaireCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue));
        QuestionnaireCategory.Columns.Add("Score2");

        ParticipantScore_BE participantScore_BE = new ParticipantScore_BE();
        //Set control value by details
        participantScore_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
        participantScore_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
        participantScore_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
        participantScore_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
        participantScore_BE.ScoreMonth = Convert.ToInt32(ddlScoreMonth2.SelectedValue);
        participantScore_BE.ScoreYear = Convert.ToInt32(ddlScoreYear2.SelectedValue);

        ParticipantScore_BAO participantScore_BAO = new ParticipantScore_BAO();
        dtCategoryScore = participantScore_BAO.GetCategoryScore2(participantScore_BE);
        //If category score is >0 then bind grid.
        if (dtCategoryScore != null && dtCategoryScore.Rows.Count > 0)
        {
            //bind gris with category
            rptrPreviousScore2.DataSource = dtCategoryScore;
            rptrPreviousScore2.DataBind();
        }
        else if (QuestionnaireCategory.Rows.Count > 0)
        {
            //bind gris with Questionnaire category
            rptrPreviousScore2.DataSource = QuestionnaireCategory;
            rptrPreviousScore2.DataBind();
        }
        else
        {
            //If no category and score then null
            rptrPreviousScore2.DataSource = null;
            rptrPreviousScore2.DataBind();
        }
    }
}
