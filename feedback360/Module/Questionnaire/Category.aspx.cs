using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.Data;

public partial class Module_Questionnaire_Category : CodeBehindBase
{
    //Global variables
    Category_BAO categoryBusinessAccessObject = new Category_BAO();
    Category_BE categoryBusinessEntity = new Category_BE();
    List<Category_BE> categoryBusinessEntityList = new List<Category_BE>();

    DataTable dataTableCompanyName;
    // DataTable dtAllAccount;
    // string expression1;
    // string Finalexpression;
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;
                int categoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                //Get all category List by user account id and category id.
                categoryBusinessEntityList = categoryBusinessAccessObject.GetCategoryByID(Convert.ToInt32(identity.User.AccountID), categoryID);

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get Account list by user account id  and bind account dropdown.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                Questionnaire_BAO.Questionnaire_BAO questionnnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
                //Get Questionnaire list and bind Questionnaire dropdown by user account Id.
                ddlQuestionnaire.DataSource = questionnnaireBusinessAccessObject.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                if (categoryBusinessEntityList.Count > 0)
                {
                    //Bind controls by value.
                    SetCategoryValue(categoryBusinessEntityList);

                    ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }

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

                //If query string contains Mode="E" then Edit mode else view mode and hide show controls accordingly. 
                if (Request.QueryString["Mode"] == "E")//Edit Mode.
                {
                    ibtnSave.Visible = true;
                    ibtnCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Category";
                }
                else if (Request.QueryString["Mode"] == "R")//View Mode.
                {
                    ibtnSave.Visible = false;
                    ibtnCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Category";
                }
            }
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Initilize properties and save to the data base. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Category_BE categoryBusinessEntity = new Category_BE();
            Category_BAO categoryBusinessAccessObject = new Category_BAO();

            identity = this.Page.User.Identity as WADIdentity;
            //If user is a Super Admin then use account drop down value else user account .
            if (identity.User.GroupID == 1)
            {
                categoryBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                categoryBusinessEntity.AccountID = identity.User.AccountID;
            }

            categoryBusinessEntity.Name = GetString(txtCategoryName.Text);
            categoryBusinessEntity.Title = GetString(txtCategoryTitle.Text);
            categoryBusinessEntity.Description = GetString(txtDescription.Text);
            categoryBusinessEntity.Sequence = Convert.ToInt32(GetString(txtSequence.Text));
            categoryBusinessEntity.ExcludeFromAnalysis = chkExcludeFromAnalysis.Checked;
            categoryBusinessEntity.Questionnaire = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            categoryBusinessEntity.ModifiedBy = 1;
            categoryBusinessEntity.ModifiedDate = DateTime.Now;
            categoryBusinessEntity.IsActive = 1;
            categoryBusinessEntity.ReportCategoryDescription = TextBoxReportDescription.Text.Trim();
            categoryBusinessEntity.QuestionnaireCategoryDescription = TextBoxQuestionnaireDescription.Text.Trim();

            //If querey string contains mode="E" then update else Insert.
            if (Request.QueryString["Mode"] == "E")
            {
                categoryBusinessEntity.CategoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                categoryBusinessAccessObject.UpdateCategory(categoryBusinessEntity);
                lblMessage.Text = "Category updated successfully";
                Response.Redirect("CategoryList.aspx", false);
            }
            else
            {
                categoryBusinessAccessObject.AddCategory(categoryBusinessEntity);
                lblMessage.Text = "Category saved successfully";
            }

            txtCategoryName.Text = "";
            txtDescription.Text = "";
            txtSequence.Text = "";
            TextBoxReportDescription.Text = string.Empty;
            TextBoxQuestionnaireDescription.Text = string.Empty;
            chkExcludeFromAnalysis.Checked = false;

            //Response.Redirect("CategoryList.aspx", false);
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect to category list page when click on calcel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("CategoryList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind questionnaire dropdown by account.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        identity = this.Page.User.Identity as WADIdentity;

        //If account dropdown is selected value >0
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO account_BAO = new Account_BAO();
            //Get account details
            dataTableCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");

            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dataTableAccount.ImportRow(drAccount);

            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
            //Get QuestionnaireList by account Id and bind Questionnaire dropdown.
            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";
            //Get QuestionnaireList by user account Id and bind Questionnaire dropdown.
            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
    }

    /// <summary>
    /// Set value to controls
    /// </summary>
    /// <param name="categoryList"></param>
    private void SetCategoryValue(List<Category_BE> categoryList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            identity = this.Page.User.Identity as WADIdentity;
            //If user is a Super Admin then use account drop down value else user account to get records.
            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = categoryList[0].AccountID.ToString();
                string accountID = categoryList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = accountID;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    Account_BAO accountBusinessAccessObject = new Account_BAO();
                    
                    dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));
                    //expression1 = "AccountID='" + companycode + "'";
                    //Finalexpression = expression1;

                    DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
                    DataTable dataTableAccount = dataTableCompanyName.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                    {
                        dataTableAccount.ImportRow(dataRowAccount);
                    }
                    //bind comapny name.
                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }
            //Set values to control.
            txtCategoryName.Text = categoryList[0].Name;
            txtCategoryTitle.Text = categoryList[0].Title;
            txtDescription.Text = categoryList[0].Description;
            ddlQuestionnaire.SelectedValue = categoryList[0].Questionnaire.ToString();
            txtSequence.Text = categoryList[0].Sequence.ToString();
            chkExcludeFromAnalysis.Checked = Convert.ToBoolean(categoryList[0].ExcludeFromAnalysis);
            TextBoxQuestionnaireDescription.Text = categoryList[0].QuestionnaireCategoryDescription;
            TextBoxReportDescription.Text = categoryList[0].ReportCategoryDescription;

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
}
