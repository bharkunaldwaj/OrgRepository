using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Survey_Module_Questionnaire_Questionnaire : CodeBehindBase
{
    Survey_Questionnaire_BAO questionnaireBusinessObject = new Survey_Questionnaire_BAO();
    //Questionnaire_BE.Survey_Questionnaire_BE questionnaire_BE = new Questionnaire_BE.Survey_Questionnaire_BE();
    List<Survey_Questionnaire_BE> questionnaireBusinessEntityList = new List<Survey_Questionnaire_BE>();
    DataTable dataTableCompanyName;
    //DataTable dtAllAccount;
    //string expression1;
    //string Finalexpression;
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

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
        questionnaireBusinessEntityList = questionnaireBusinessObject.GetQuestionnaireByID(questionnaireID);

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
    /// <param name="questionnaireBusinesEntityList"></param>
    private void SetQuestionnaireValue(List<Survey_Questionnaire_BE> questionnaireBusinesEntityList)
    {
        try
        {
            //If user is super Admin then use account dropdown selected value.
            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = questionnaireBusinesEntityList[0].AccountID.ToString();

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
            ddlType.SelectedValue = questionnaireBusinesEntityList[0].QSTNType.ToString();
            txtquestionnairecode.Text = questionnaireBusinesEntityList[0].QSTNCode.ToString();
            txtquestionnairename.Text = questionnaireBusinesEntityList[0].QSTNName.ToString();
            txtDescription.Text = questionnaireBusinesEntityList[0].QSTNDescription.ToString();
            txtDisplayCategory.Text = questionnaireBusinesEntityList[0].DisplayCategory.ToString();
            //ddlProject.SelectedValue = questionnaire_BEList[0].ProjectID.ToString();            
            txtPrologueEditor.Value = Server.HtmlDecode(questionnaireBusinesEntityList[0].QSTNPrologue.ToString());
            txtEpilogueEditor.Value = Server.HtmlDecode(questionnaireBusinesEntityList[0].QSTNEpilogue.ToString());

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
            Survey_Questionnaire_BE questionnaireBusinesEntity = new Survey_Questionnaire_BE();
            Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                questionnaireBusinesEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                questionnaireBusinesEntity.AccountID = identity.User.AccountID;
            }

            //Initilize properties with controls value
            questionnaireBusinesEntity.QSTNType = Convert.ToInt32(GetString(ddlType.SelectedValue));
            questionnaireBusinesEntity.QSTNCode = GetString(txtquestionnairecode.Text);
            questionnaireBusinesEntity.QSTNName = GetString(txtquestionnairename.Text);
            questionnaireBusinesEntity.QSTNDescription = GetString(txtDescription.Text);
            questionnaireBusinesEntity.DisplayCategory = Convert.ToInt32(txtDisplayCategory.Text.Trim());

            //questionnaire_BE.ProjectID = Convert.ToInt32(GetString(ddlProject.SelectedValue));
            questionnaireBusinesEntity.ManagerID = 3;
            questionnaireBusinesEntity.QSTNPrologue = (Server.HtmlDecode(txtPrologueEditor.InnerHtml));
            questionnaireBusinesEntity.QSTNEpilogue = (Server.HtmlDecode(txtEpilogueEditor.InnerHtml));
            questionnaireBusinesEntity.ModifyBy = 1;
            questionnaireBusinesEntity.ModifyDate = DateTime.Now;
            questionnaireBusinesEntity.IsActive = 1;
            //If query string contains Mode="E" then Update records else Insert.
            if (Request.QueryString["Mode"] == "E")//Edit mode
            {
                questionnaireBusinesEntity.QuestionnaireID = Convert.ToInt32(Request.QueryString["QestId"]);
                questionnaireBusinessAccessObject.UpdateQuestionnaire(questionnaireBusinesEntity);//update Questionnaire
            }
            else
            {
                questionnaireBusinessAccessObject.AddQuestionnaire(questionnaireBusinesEntity);//Insert Questionnaire
            }

            Response.Redirect("QuestionnaireList.aspx", false);
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
            Response.Redirect("QuestionnaireList.aspx", false);
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
        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();

        //ddlProject.Items.Clear();
        //ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get Questionnaire details. 
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //Bind comapny name
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            //ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(ddlAccountCode.SelectedValue));
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataBind();

            //Set prologue ,epilogue data.
            ReBindEditorContent();
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

    /// <summary>
    /// Rebind prologue and epilogue editors.
    /// </summary>
    private void ReBindEditorContent()
    {
        txtPrologueEditor.InnerHtml = Server.HtmlDecode(txtPrologueEditor.InnerHtml);
        txtEpilogueEditor.InnerHtml = Server.HtmlDecode(txtEpilogueEditor.InnerHtml);
    }
}
