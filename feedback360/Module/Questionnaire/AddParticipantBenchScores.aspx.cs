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

public partial class Module_Questionnaire_AddParticipantBenchScores : CodeBehindBase 
{
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string strName = string.Empty;
    
    WADIdentity identity;
    DataTable CompanyName;
    StringBuilder sb = new StringBuilder();
    DataTable dtCategoryScore = new DataTable();
   
    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;
            int userid = Convert.ToInt16(identity.User.UserID);

            Account_BAO account_BAO = new Account_BAO();
            ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();
            
            //SetValues();
            
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

    }

    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            

            lblMessage.Text = "";
            lblvalidation.Text = "";

            identity = this.Page.User.Identity as WADIdentity;

            ParticipantBenchScore_BE participantBenchScore_BE = new ParticipantBenchScore_BE();
            ParticipantBenchScore_BAO participantBenchScore_BAO = new ParticipantBenchScore_BAO();

            participantBenchScore_BE.BenchmarkName = txtBenchScoreName.Text.Trim();
            participantBenchScore_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
            participantBenchScore_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
            participantBenchScore_BE.QuestionnaireID = Convert.ToInt32(Session["QuestionnaireID"].ToString());
            //participantBenchScore_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
            participantBenchScore_BE.Description = "";

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
                participantBenchScore_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            else
                participantBenchScore_BE.AccountID = identity.User.AccountID;

            participantBenchScore_BE.ScoreMonth = 0; // Convert.ToInt32(ddlScoreMonth.SelectedValue);
            participantBenchScore_BE.ScoreYear = 0; // Convert.ToInt32(ddlScoreYear.SelectedValue);

            participantBenchScore_BE.ModifiedBy = 1;
            participantBenchScore_BE.ModifiedDate = DateTime.Now;
            participantBenchScore_BE.IsActive = 1;

            participantBenchScore_BE.ParticipantBenchScoreDetails = GetParticipantBenchScoreList();

            if (participantBenchScore_BE.ParticipantBenchScoreDetails.Count > 0)
            {
                if (ddlTargetPerson.SelectedIndex > 0)
                {
                    participantBenchScore_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
                    Int32 assignmentID = participantBenchScore_BAO.AddParticipantBenchScore(participantBenchScore_BE);
                }
                else
                {
                    foreach (ListItem itm in ddlTargetPerson.Items)
                    {
                        if (Convert.ToInt32(itm.Value) > 0)
                        {
                            participantBenchScore_BE.TargetPersonID = Convert.ToInt32(itm.Value);
                            Int32 assignmentID = participantBenchScore_BAO.AddParticipantBenchScore(participantBenchScore_BE);
                        }
                    }
                }

                lblMessage.Text = "Participant's benchmark comparison scores saved successfully";

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

    private List<ParticipantBenchScoreDetails_BE> GetParticipantBenchScoreList()
    {
        List<ParticipantBenchScoreDetails_BE> participantBenchScoreDetails_BEList = new List<ParticipantBenchScoreDetails_BE>();

        foreach (RepeaterItem item in rptrCategoryList.Items)
        {
            ParticipantBenchScoreDetails_BE participantBenchScoreDetails_BE = new ParticipantBenchScoreDetails_BE();

            Label lblCategoryId = (Label)item.FindControl("lblCategoryID");
            TextBox txtScore = (TextBox)item.FindControl("txtScore");

            participantBenchScoreDetails_BE.CategoryID =Convert.ToInt32(lblCategoryId.Text);
            
            if (txtScore.Text.Trim()!="")
                participantBenchScoreDetails_BE.Score = Convert.ToDecimal(txtScore.Text.Trim());
            else
                participantBenchScoreDetails_BE.Score = 0;

            participantBenchScoreDetails_BEList.Add(participantBenchScoreDetails_BE);
        }

        return participantBenchScoreDetails_BEList;
    }

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

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Set Questionnaire
        Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

        //ddlQuestionnaire.Items.Clear();
        DataTable dtQuestionnaire = new DataTable();
        dtQuestionnaire = questionnaire_BAO.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtQuestionnaire.Rows.Count > 0)
            Session["QuestionnaireID"] = dtQuestionnaire.Rows[0]["QuestionnaireID"].ToString();
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

        FillCategoryBenchScoreData();
    }

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

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
            
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

            txtBenchScoreName.Text = "";

            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
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

            txtBenchScoreName.Text = "";

            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }
    }

    protected void ddlTargetPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    protected void rptrCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participant_BAO = new AssignQstnParticipant_BAO();

        if (ddlProgramme.SelectedIndex > 0)
        {
            DataTable dtParticipant = participant_BAO.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
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
    
    protected void ddlQuestionnaire_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    protected void ddlScoreMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    protected void ddlScoreYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategoryBenchScoreData();
    }

    protected void FillCategoryBenchScoreData()
    {
        Category_BAO category_BAO = new Category_BAO();
        DataTable QuestionnaireCategory = category_BAO.SelectQuestionnaireCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(Session["QuestionnaireID"].ToString()));
        QuestionnaireCategory.Columns.Add("Score");
        
        ParticipantBenchScore_BE participantBenchScore_BE = new ParticipantBenchScore_BE();

        participantBenchScore_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
        participantBenchScore_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
        participantBenchScore_BE.QuestionnaireID = Convert.ToInt32(Session["QuestionnaireID"].ToString()); ; // Convert.ToInt32(ddlQuestionnaire.SelectedValue);
        participantBenchScore_BE.TargetPersonID = Convert.ToInt32(ddlTargetPerson.SelectedValue);
        participantBenchScore_BE.ScoreMonth = 0; // Convert.ToInt32(ddlScoreMonth.SelectedValue);
        participantBenchScore_BE.ScoreYear = 0; // Convert.ToInt32(ddlScoreYear.SelectedValue);

        ParticipantBenchScore_BAO participantBenchScore_BAO = new ParticipantBenchScore_BAO();
        dtCategoryScore = participantBenchScore_BAO.GetCategoryScore(participantBenchScore_BE);

        if (dtCategoryScore != null && dtCategoryScore.Rows.Count > 0)
        {
            rptrCategoryList.DataSource = dtCategoryScore;
            rptrCategoryList.DataBind();

            txtBenchScoreName.Text = dtCategoryScore.Rows[0]["BenchmarkName"].ToString();
        }
        else if (QuestionnaireCategory.Rows.Count > 0)
        {
            rptrCategoryList.DataSource = QuestionnaireCategory;
            rptrCategoryList.DataBind();
        }
        else
        {
            rptrCategoryList.DataSource = null;
            rptrCategoryList.DataBind();
        }


        
    }
}
