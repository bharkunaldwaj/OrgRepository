using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Module_Questionnaire_Questionnaire : CodeBehindBase
{     //Global variables
    Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
    Questionnaire_BE.Questionnaire_BE questionnaireBusinessEntity = new Questionnaire_BE.Questionnaire_BE();
    List<Questionnaire_BE.Questionnaire_BE> questionnaireBusinessEntityList = new List<Questionnaire_BE.Questionnaire_BE>();
    DataTable dataTableCompanyName;
    // DataTable dtAllAccount;
    // string expression1;
    //  string Finalexpression;
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;

            //Project_BAO project_BAO = new Project_BAO();
            //ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(identity.User.AccountID));
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataBind();

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get Account details to bind account dropdown list.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            //If user is a Super Admin then show account detail section else hide.
            if (identity.User.GroupID == 1)
            {
                divAccount.Visible = true;

                if (Request.QueryString["Mode"] == null)
                {
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
            }
            else
            {
                divAccount.Visible = false;
            }
        }

        //Get questionnaire ID from query string.
        int questionnaireID = Convert.ToInt32(Request.QueryString["QestId"]);
        questionnaireBusinessEntityList = questionnaireBusinessAccessObject.GetQuestionnaireByID(questionnaireID);

        if (questionnaireBusinessEntityList.Count > 0)
        {
            //Binid control value.
            SetQuestionnaireValue(questionnaireBusinessEntityList);

            ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
            ddlAccountCode_SelectedIndexChanged(sender, e);
        }
        //If Query string contains Mode="E" then This is in Edit mode else view.
        if (Request.QueryString["Mode"] == "E")//Edit
        {
            ibtnSave.Visible = true;
            ibtnCancel.Visible = true;
            imbBack.Visible = false;
            lblheader.Text = "Edit Questionnaire";
        }
        else if (Request.QueryString["Mode"] == "R")//View
        {
            ibtnSave.Visible = false;
            ibtnCancel.Visible = false;
            imbBack.Visible = true;
            lblheader.Text = "View Questionnaire";
        }
    }

    /// <summary>
    /// Set values to the controls.
    /// </summary>
    /// <param name="questionnaireBusinessEntityList"></param>
    private void SetQuestionnaireValue(List<Questionnaire_BE.Questionnaire_BE> questionnaireBusinessEntityList)
    {
        try
        {
            //If user is super Admin then use account dropdown selected value.
            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = questionnaireBusinessEntityList[0].AccountID.ToString();

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    Account_BAO accountBusinessAccessObject = new Account_BAO();
                    //Get Company details.
                    dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));
                    DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");

                    DataTable dataTableAccount = dataTableCompanyName.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                    {
                        dataTableAccount.ImportRow(dataRowAccount);
                    }
                    //Bind company name.
                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }
            //Set values to controls
            ddlType.SelectedValue = questionnaireBusinessEntityList[0].QSTNType.ToString();
            txtquestionnairecode.Text = questionnaireBusinessEntityList[0].QSTNCode.ToString();
            txtquestionnairename.Text = questionnaireBusinessEntityList[0].QSTNName.ToString();
            txtDescription.Text = questionnaireBusinessEntityList[0].QSTNDescription.ToString();
            txtDisplayCategory.Text = questionnaireBusinessEntityList[0].DisplayCategory.ToString();
            //ddlProject.SelectedValue = questionnaire_BEList[0].ProjectID.ToString();            
            txtPrologueEditor.InnerHtml = Server.HtmlDecode(questionnaireBusinessEntityList[0].QSTNPrologue.ToString());
            txtEpilogueEditor.InnerHtml = Server.HtmlDecode(questionnaireBusinessEntityList[0].QSTNEpilogue.ToString());

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

    }

    /// <summary>
    /// Initilize properties and update databse.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Questionnaire_BE.Questionnaire_BE questionnaireBusinessEntity = new Questionnaire_BE.Questionnaire_BE();
            Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {

                questionnaireBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

            }
            else
            {
                questionnaireBusinessEntity.AccountID = identity.User.AccountID;
            }

            questionnaireBusinessEntity.QSTNType = Convert.ToInt32(GetString(ddlType.SelectedValue));
            questionnaireBusinessEntity.QSTNCode = GetString(txtquestionnairecode.Text);
            questionnaireBusinessEntity.QSTNName = GetString(txtquestionnairename.Text);
            questionnaireBusinessEntity.QSTNDescription = GetString(txtDescription.Text);
            questionnaireBusinessEntity.DisplayCategory = Convert.ToInt32(txtDisplayCategory.Text.Trim());

            //questionnaire_BE.ProjectID = Convert.ToInt32(GetString(ddlProject.SelectedValue));
            questionnaireBusinessEntity.ManagerID = 3;
            questionnaireBusinessEntity.QSTNPrologue = GetString(Server.HtmlDecode(txtPrologueEditor.Value));
            questionnaireBusinessEntity.QSTNEpilogue = GetString(Server.HtmlDecode(txtEpilogueEditor.Value));
            questionnaireBusinessEntity.ModifyBy = 1;
            questionnaireBusinessEntity.ModifyDate = DateTime.Now;
            questionnaireBusinessEntity.IsActive = 1;

            //If quesy string contains Mode="E" then Update records else Insert.
            if (Request.QueryString["Mode"] == "E")
            {
                questionnaireBusinessEntity.QuestionnaireID = Convert.ToInt32(Request.QueryString["QestId"]);
                questionnaireBusinessAccessObject.UpdateQuestionnaire(questionnaireBusinessEntity);
            }
            else
            {
                questionnaireBusinessAccessObject.AddQuestionnaire(questionnaireBusinessEntity);
            }

            Response.Redirect("QuestionnaireList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect back toQuestionnaireList.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("QuestionnaireList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Get Questionnaire details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Project_BAO projectBusinessAccessObject = new Project_BAO();

        //ddlProject.Items.Clear();
        //ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get Questionnaire details. 
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dataTableCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            //Bind comapny name
            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            //ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(ddlAccountCode.SelectedValue));
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataBind();

            //Set prologue data.
            txtPrologueEditor.InnerHtml = Server.HtmlDecode(txtPrologueEditor.InnerHtml);
            //Set epilogue data.
            txtEpilogueEditor.InnerHtml = Server.HtmlDecode(txtEpilogueEditor.InnerHtml);
        }
        else
        {
            lblcompanyname.Text = "";

            //ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(identity.User.AccountID));
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataBind();
        }
    }
}
