using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Module_Questionnaire_Questions : CodeBehindBase
{
    //Global variables
    Questions_BAO questionsBusinessAccessObject = new Questions_BAO();
    Questions_BE questionsBusinessEntity = new Questions_BE();
    Category_BAO categoryBusinessAccessObject = new Category_BAO();
    List<Questions_BE> questionsList = new List<Questions_BE>();

    DataTable dataTableCompanyName;
    // DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
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
                //Category_BAO category_BAO = new Category_BAO();
                //ddlQuestionCategory.DataSource = category_BAO.GetdtCategoryList();
                //ddlQuestionCategory.DataTextField = "CategoryName";
                //ddlQuestionCategory.DataValueField = "CategoryID";
                //ddlQuestionCategory.DataBind();

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get Account list by user account id and bind account dropdown.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();


                Questionnaire_BAO.Questionnaire_BAO questionnnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
                //Get Questionnaire list and bind Questionnaire dropdown by  user account Id.
                ddlQuestionnaire.DataSource = questionnnaireBusinessAccessObject.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                int questionsID = Convert.ToInt32(Request.QueryString["quesId"]);
                questionsList = questionsBusinessAccessObject.GetQuestionsByID(questionsID);

                if (questionsList.Count > 0)
                {
                    //Set value to controls
                    SetQuestionsValue(questionsList);

                    ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else
                {
                    //Hide show free text and range section according to question range type.
                    if (ddlQuestionType.SelectedIndex == 1)// If question type free text.
                    {
                        divFreeText.Visible = true;
                        divRange.Visible = false;
                    }
                    else if (ddlQuestionType.SelectedIndex == 2)// If question type range.
                    {
                        divFreeText.Visible = false;
                        divRange.Visible = true;
                    }
                    else
                    {
                        divFreeText.Visible = true;
                        divRange.Visible = false;
                    }
                }

                //If query string contains Mode="E" then Edit mode esle view mode and hide show controls accordingly. 
                if (Request.QueryString["Mode"] == "E")
                {
                    ibtnSave.Visible = true;
                    ibtnCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Question";
                }
                else if (Request.QueryString["Mode"] == "R") //If query string contains Mode="R" then view only.
                {
                    ibtnSave.Visible = false;
                    ibtnCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Question";
                }


                //If user is a Super Admin then show account detail section else hide and bind questionnaire dropdown.
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
            Questions_BE questionsBusinessEntity = new Questions_BE();
            Questions_BAO questionsBusinessAccessObject = new Questions_BAO();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                questionsBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                questionsBusinessEntity.AccountID = identity.User.AccountID;
            }

            if (identity.User.GroupID == 1)
            {
                questionsBusinessEntity.CompanyID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                questionsBusinessEntity.CompanyID = identity.User.AccountID;
            }

            questionsBusinessEntity.QuestionTypeID = Convert.ToInt32(ddlQuestionType.SelectedValue);
            questionsBusinessEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            questionsBusinessEntity.CateogryID = Convert.ToInt32(ddlQuestionCategory.SelectedValue);
            questionsBusinessEntity.Sequence = Convert.ToInt32(GetString(txtSequence.Text));
            questionsBusinessEntity.Validation = Convert.ToInt32(ddlValidation.SelectedValue);
            questionsBusinessEntity.ValidationText = ddlValidation.SelectedItem.Text;
            questionsBusinessEntity.Title = ""; //txtTitle.Text;
            questionsBusinessEntity.Token = Convert.ToInt32(ddlTokens.SelectedValue);
            questionsBusinessEntity.TokenText = ddlTokens.SelectedItem.Text;
            questionsBusinessEntity.Description = txtQuestionText.Text;
            questionsBusinessEntity.DescriptionSelf = txtQuestionSelfText.Text;
            questionsBusinessEntity.Hint = txtUsageHint.Text;

            questionsBusinessEntity.LengthMIN = (txtMinLength.Text == "" ? 0 : Convert.ToInt32(txtMinLength.Text));
            questionsBusinessEntity.LengthMAX = (txtMaxLength.Text == "" ? 0 : Convert.ToInt32(txtMaxLength.Text));
            questionsBusinessEntity.Multiline = chkmultiline.Checked;
            questionsBusinessEntity.LowerLabel = txtLowerLabel.Text;
            questionsBusinessEntity.UpperLabel = txtUpperLabel.Text;
            questionsBusinessEntity.LowerBound = (txtLowerBound.Text == "" ? 0 : Convert.ToInt32(txtLowerBound.Text));
            questionsBusinessEntity.UpperBound = (txtUpperBound.Text == "" ? 0 : Convert.ToInt32(txtUpperBound.Text));
            questionsBusinessEntity.Increment = (txtIncrement.Text == "" ? 0 : Convert.ToInt32(txtIncrement.Text));
            questionsBusinessEntity.Reverse = chkReverse.Checked;

            questionsBusinessEntity.ModifyBy = 1;
            questionsBusinessEntity.ModifyDate = DateTime.Now;
            questionsBusinessEntity.IsActive = 1;
            //If querey string contains mode="E" then update else Insert.
            if (Request.QueryString["Mode"] == "E")
            {
                questionsBusinessEntity.QuestionID = Convert.ToInt32(Request.QueryString["quesId"]);
                questionsBusinessAccessObject.UpdateQuestions(questionsBusinessEntity);
            }
            else
            {
                questionsBusinessAccessObject.AddQuestions(questionsBusinessEntity);
            }

            Response.Redirect("QuestionList.aspx", false);
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect to QuestionList when click on cancel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("QuestionList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    ///  Hide show free text and range section according to question range type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlQuestionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlQuestionType.SelectedIndex == 1)// If question type free text.
        {
            divFreeText.Visible = true;
            divRange.Visible = false;
        }
        else if (ddlQuestionType.SelectedIndex == 2)// If question type renge.
        {
            divFreeText.Visible = false;
            divRange.Visible = true;
        }
    }

    /// <summary>
    /// Bind ddlQuestionnaire by account id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //get company name by account id.
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //Bind comapny name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
            //bind ddlQuestionnaire by dropdown account id.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";
            //bind ddlQuestionnaire by user account id.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
    }

    /// <summary>
    /// Bind category by account id and Questionnaire id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlQuestionnaire_SelectedIndexChanged(object sender, EventArgs e)
    {
        int questionnaireid = 0;
        identity = this.Page.User.Identity as WADIdentity;

        ddlQuestionCategory.Items.Clear();
        ddlQuestionCategory.Items.Insert(0, new ListItem("Select", "0"));
        questionnaireid = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
        //Get category by account id and Questionnaire id.
        DataTable categoryid = categoryBusinessAccessObject.SelectCategory(Convert.ToInt32(identity.User.AccountID), questionnaireid);

        if (categoryid.Rows.Count > 0)
        {//Bind category drop down list
            ddlQuestionCategory.DataSource = categoryid;
            ddlQuestionCategory.DataTextField = "CategoryName";
            ddlQuestionCategory.DataValueField = "CategoryID";
            ddlQuestionCategory.DataBind();
        }
    }

    /// <summary>
    /// Bind control with values.
    /// </summary>
    /// <param name="questionsList"></param>
    private void SetQuestionsValue(List<Questions_BE> questionsList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;
            //If user is a Super Admin then use account drop dwon value else user account to get records.
            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = questionsList[0].AccountID.ToString();


                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {

                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO accountBusinessAccessObject = new Account_BAO();

                    dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = dataTableCompanyName.Select(Finalexpression);

                    DataTable dataTableAccount = dataTableCompanyName.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                    {
                        dataTableAccount.ImportRow(dataRowAccount);
                    }
                    //set company name.
                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

                    Questionnaire_BAO.Questionnaire_BAO questionnnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
                    DataTable dtResult = new DataTable();
                    dtResult = questionnnaire_BAO.GetdtQuestionnaireList(ddlAccountCode.SelectedValue);

                    if (dtResult.Rows.Count > 0)
                    {
                        ddlQuestionnaire.DataSource = dtResult;
                        ddlQuestionnaire.DataTextField = "QSTNName";
                        ddlQuestionnaire.DataValueField = "QuestionnaireID";
                        ddlQuestionnaire.DataBind();
                    }
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }

            ddlQuestionType.SelectedValue = questionsList[0].QuestionTypeID.ToString();
            ddlQuestionnaire.SelectedValue = questionsList[0].QuestionnaireID.ToString();

            ddlQuestionCategory.Items.Clear();
            ddlQuestionCategory.Items.Insert(0, new ListItem("Select", "0"));

            DataTable dataTableCategoryDetails = new DataTable();

            if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                dataTableCategoryDetails = categoryBusinessAccessObject.SelectCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue));
            else
                dataTableCategoryDetails = categoryBusinessAccessObject.SelectCategory(Convert.ToInt32(identity.User.AccountID), Convert.ToInt32(ddlQuestionnaire.SelectedValue));

            if (dataTableCategoryDetails.Rows.Count > 0)
            {
                ddlQuestionCategory.DataSource = dataTableCategoryDetails;
                ddlQuestionCategory.DataTextField = "CategoryName";
                ddlQuestionCategory.DataValueField = "CategoryID";
                ddlQuestionCategory.DataBind();
            }

            ddlQuestionCategory.SelectedValue = questionsList[0].CateogryID.ToString();
            txtSequence.Text = questionsList[0].Sequence.ToString();
            ddlValidation.SelectedValue = questionsList[0].Validation.ToString();
            //txtTitle.Text = ""; //questions_BEList[0].Title;
            ddlTokens.SelectedValue = questionsList[0].Token.ToString();
            txtQuestionText.Text = questionsList[0].Description;
            txtQuestionSelfText.Text = questionsList[0].DescriptionSelf;
            txtUsageHint.Text = questionsList[0].Hint;
            txtMinLength.Text = Convert.ToString(questionsList[0].LengthMIN);//for free text set min length vlaue.
            txtMaxLength.Text = Convert.ToString(questionsList[0].LengthMAX);//for free text set max length vlaue.
            chkmultiline.Checked = Convert.ToBoolean(questionsList[0].Multiline);////for free text set multiline or not.
            txtLowerLabel.Text = questionsList[0].LowerLabel;
            txtUpperLabel.Text = questionsList[0].UpperLabel;
            txtLowerBound.Text = Convert.ToString(questionsList[0].LowerBound);//set upper bound for range type
            txtUpperBound.Text = Convert.ToString(questionsList[0].UpperBound);//set lower bound for range type
            txtIncrement.Text = Convert.ToString(questionsList[0].Increment);//set increment for bound  type
            chkReverse.Checked = Convert.ToBoolean(questionsList[0].Reverse);

            if (ddlQuestionType.SelectedIndex == 1)//If question type is free text then hide range div.
            {
                divFreeText.Visible = true;
                divRange.Visible = false;
            }
            else if (ddlQuestionType.SelectedIndex == 2)//If question type is range type then hide free text div.
            {
                divFreeText.Visible = false;
                divRange.Visible = true;
            }
            else
            {
                divFreeText.Visible = true;
                divRange.Visible = false;
            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
}
