using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Survey_Module_Questionnaire_Questions : CodeBehindBase
{
    //Global variables
    Survey_Questions_BAO questionsBusinessAccessObject = new Survey_Questions_BAO();
    //Survey_Questions_BE questionsBusinesEntity = new Survey_Questions_BE();
    Survey_Category_BAO categoryBusinessAccessObject = new Survey_Category_BAO();
    List<Survey_Questions_BE> questionsBusinesEntityList = new List<Survey_Questions_BE>();

    DataTable dataTableCompanyName;
    //DataTable dataTableAllAccount;
    string expression1;
    string Finalexpression;
    WADIdentity identity;
    static string testing;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dataTableRange = questionsBusinessAccessObject.getrange_data();
            DropDownList1.DataSource = dataTableRange;
            DropDownList1.DataTextField = "Range_Name";

            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, new ListItem("Select", "0"));
        }
        try
        {
            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {

                identity = this.Page.User.Identity as WADIdentity;
                //Category_BAO category_BAO = new Category_BAO();
                //ddlQuestionCategory.DataSource = category_BAO.GetdtCategoryList();
                //ddlQuestionCategory.DataTextField = "CategoryName";
                //ddlQuestionCategory.DataValueField = "CategoryID";
                //ddlQuestionCategory.DataBind();

                //Bind Account drop down
                Account_BAO accountBusinessAccessObject = new Account_BAO();
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                //Bind Questionnaire drop down
                Survey_Questionnaire_BAO questionnnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
                ddlQuestionnaire.DataSource = questionnnaireBusinessAccessObject.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                int questionsID = Convert.ToInt32(Request.QueryString["quesId"]);
                questionsBusinesEntityList = questionsBusinessAccessObject.GetQuestionsByID(questionsID);

                if (questionsBusinesEntityList.Count > 0)
                {
                    SetQuestionsValue(questionsBusinesEntityList);

                    ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else
                {
                    if (ddlQuestionType.SelectedIndex == 1)
                    {
                        divFreeText.Visible = true;
                        // divRange.Visible = false;
                    }
                    else if (ddlQuestionType.SelectedIndex == 2)
                    {
                        divFreeText.Visible = false;
                        //divRange.Visible = true;
                        DropDownList1.Visible = true;
                    }
                    else
                    {
                        divFreeText.Visible = true;
                        // divRange.Visible = false;
                        DropDownList1.Visible = false;
                    }
                }
                //If Query String Mode ='E' then Edit else View
                if (Request.QueryString["Mode"] == "E")
                {
                    ibtnSave.Visible = true;
                    ibtnCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Question";
                }
                else if (Request.QueryString["Mode"] == "R")
                {
                    ibtnSave.Visible = false;
                    ibtnCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Question";
                }
                //If users group id=1 then Admin then show account drop down else hide.
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
            HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind Questionnaire and QuestionCategory drop down and other controls
    /// </summary>
    /// <param name="questions_BEList"></param>
    private void SetQuestionsValue(List<Survey_Questions_BE> questions_BEList)
    {
        try
        {
           HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = questions_BEList[0].AccountID.ToString();

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
                    //set company name
                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

                    Survey_Questionnaire_BAO questionnnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
                    DataTable dataTableResult = new DataTable();

                    dataTableResult = questionnnaireBusinessAccessObject.GetdtQuestionnaireList(ddlAccountCode.SelectedValue);

                    if (dataTableResult.Rows.Count > 0)
                    {
                        ddlQuestionnaire.DataSource = dataTableResult;
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

            ddlQuestionType.SelectedValue = questions_BEList[0].QuestionTypeID.ToString();
            ddlQuestionnaire.SelectedValue = questions_BEList[0].QuestionnaireID.ToString();

            ddlQuestionCategory.Items.Clear();
            ddlQuestionCategory.Items.Insert(0, new ListItem("Select", "0"));

            DataTable categoryid = new DataTable();

            if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                categoryid = categoryBusinessAccessObject.SelectCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue));
            else
                categoryid = categoryBusinessAccessObject.SelectCategory(Convert.ToInt32(identity.User.AccountID), Convert.ToInt32(ddlQuestionnaire.SelectedValue));

            if (categoryid.Rows.Count > 0)
            {
                ddlQuestionCategory.DataSource = categoryid;
                ddlQuestionCategory.DataTextField = "CategoryName";
                ddlQuestionCategory.DataValueField = "CategoryID";
                ddlQuestionCategory.DataBind();
            }

            ddlQuestionCategory.SelectedValue = questions_BEList[0].CateogryID.ToString();
            txtSequence.Text = questions_BEList[0].Sequence.ToString();
            ddlValidation.SelectedValue = questions_BEList[0].Validation.ToString();
            //txtTitle.Text = ""; //questions_BEList[0].Title;
            ddlTokens.SelectedValue = questions_BEList[0].Token.ToString();
            txtQuestionText.Text = questions_BEList[0].Description;
         //   txtQuestionSelfText.Text = questions_BEList[0].DescriptionSelf;
            txtUsageHint.Text = questions_BEList[0].Hint;
            txtMinLength.Text = Convert.ToString(questions_BEList[0].LengthMIN);
            txtMaxLength.Text = Convert.ToString(questions_BEList[0].LengthMAX);
            chkmultiline.Checked = Convert.ToBoolean(questions_BEList[0].Multiline);
            // txtLowerLabel.Text = questions_BEList[0].LowerLabel;
            // txtUpperLabel.Text = questions_BEList[0].UpperLabel;
            //  txtLowerBound.Text = Convert.ToString(questions_BEList[0].LowerBound);
            // txtUpperBound.Text = Convert.ToString(questions_BEList[0].UpperBound);
            // txtIncrement.Text = Convert.ToString(questions_BEList[0].Increment);
            // chkReverse.Checked = Convert.ToBoolean(questions_BEList[0].Reverse);

            if (ddlQuestionType.SelectedIndex == 1)
            {
                divFreeText.Visible = true;
                //  divRange.Visible = false;
                DropDownList1.Visible = false;
            }
            else if (ddlQuestionType.SelectedIndex == 2)
            {
                divFreeText.Visible = false;
                // divRange.Visible = true;
                DropDownList1.Visible = true;
                //DropDownList1.SelectedItem.Text  = questions_BEList[0].Range_Name.ToString();
                DropDownList1.Text = questions_BEList[0].Range_Name.ToString();
            }
            else
            {
                divFreeText.Visible = true;
                //  divRange.Visible = false;
                DropDownList1.Visible = false;
            }

            HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Save questions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
            HandleWriteLog("Start", new StackTrace(true));
            //if(ddlQuestionType.SelectedItem.Text=="Range")
            //{
            //if(DropDownList1.SelectedItem.Text=="Select")
            //{

            //}
            Survey_Questions_BE questionsBusinesEntity = new Survey_Questions_BE();
            Survey_Questions_BAO questionsBusinessAccessObject = new Survey_Questions_BAO();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {

                questionsBusinesEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

            }
            else
            {
                questionsBusinesEntity.AccountID = identity.User.AccountID;
            }

            if (identity.User.GroupID == 1)
            {

                questionsBusinesEntity.CompanyID = Convert.ToInt32(ddlAccountCode.SelectedValue);

            }
            else
            {
                questionsBusinesEntity.CompanyID = identity.User.AccountID;
            }

            questionsBusinesEntity.QuestionTypeID = Convert.ToInt32(ddlQuestionType.SelectedValue);
            questionsBusinesEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            questionsBusinesEntity.CateogryID = Convert.ToInt32(ddlQuestionCategory.SelectedValue);
           questionsBusinesEntity.Sequence = Convert.ToInt32(GetString(txtSequence.Text));
            questionsBusinesEntity.Validation = Convert.ToInt32(ddlValidation.SelectedValue);
            questionsBusinesEntity.ValidationText = ddlValidation.SelectedItem.Text;
            questionsBusinesEntity.Title = ""; //txtTitle.Text;
            questionsBusinesEntity.Token = Convert.ToInt32(ddlTokens.SelectedValue);
            questionsBusinesEntity.TokenText = ddlTokens.SelectedItem.Text;
            questionsBusinesEntity.Description = txtQuestionText.Text;
           // questions_BE.DescriptionSelf = txtQuestionSelfText.Text;
            questionsBusinesEntity.Hint = txtUsageHint.Text;

            questionsBusinesEntity.LengthMIN = (txtMinLength.Text == "" ? 0 : Convert.ToInt32(txtMinLength.Text));
            questionsBusinesEntity.LengthMAX = (txtMaxLength.Text == "" ? 0 : Convert.ToInt32(txtMaxLength.Text));
            questionsBusinesEntity.Multiline = chkmultiline.Checked;

            ////////questions_BE.LowerLabel = txtLowerLabel.Text;
            ////////questions_BE.UpperLabel = txtUpperLabel.Text;
            ////////questions_BE.LowerBound = (txtLowerBound.Text == "" ? 0 : Convert.ToInt32(txtLowerBound.Text));
            ////////questions_BE.UpperBound = (txtUpperBound.Text == "" ? 0 : Convert.ToInt32(txtUpperBound.Text));
            ////////questions_BE.Increment = (txtIncrement.Text == "" ? 0 : Convert.ToInt32(txtIncrement.Text));
            ////////questions_BE.Reverse = chkReverse.Checked;

            questionsBusinesEntity.ModifyBy = 1;
            questionsBusinesEntity.ModifyDate = DateTime.Now;
            questionsBusinesEntity.IsActive = 1;
            questionsBusinesEntity.Range_Name = DropDownList1.SelectedValue.ToString();

                //If query string contains "E" then update else insert.
            if (Request.QueryString["Mode"] == "E")
            {
                questionsBusinesEntity.QuestionID = Convert.ToInt32(Request.QueryString["quesId"]);
                
                questionsBusinessAccessObject.UpdateQuestions(questionsBusinesEntity);
            }
            else
            {
                questionsBusinessAccessObject.AddQuestions(questionsBusinesEntity);
            }

            ddlQuestionType.SelectedValue = "0";
            ddlQuestionCategory.SelectedValue = "0";
            txtSequence.Text = "";
            txtQuestionText.Text = "";
            txtMinLength.Text = "";
            txtMaxLength.Text = "";
            chkmultiline.Checked = false;
            DropDownList1.SelectedValue = "0";
            //Response.Redirect("/feedback360/Survey_Module/Questionnaire/QuestionList.aspx", false);
          //  Response.Redirect("QuestionList.aspx", false);
            HandleWriteLog("Start", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
       }
    }

    /// <summary>
    /// Redirect to QuestionList
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("QuestionList.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Hide show control according to question type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlQuestionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlQuestionType.SelectedIndex == 0)
        {
            divFreeText.Visible = false;
            DropDownList1.Visible = false;
            RequiredFieldValidator1.Enabled = false;
            //Lbtn_select_range.Visible = false;
            //Panel1.Visible = false;
        }
        else if (ddlQuestionType.SelectedIndex == 1)
        {
            divFreeText.Visible = true;
            DropDownList1.Visible = false;
            RequiredFieldValidator1.Enabled = false;
            //Lbtn_select_range.Visible = false;
            //Panel1.Visible = false;
        }
        else if (ddlQuestionType.SelectedIndex == 2)
        {
            divFreeText.Visible = false;

            //DropDownList1.Items.IndexOf(new ListItem("-Select One-"),0);
            DropDownList1.Visible = true;
            RequiredFieldValidator1.Enabled = true;
            //Lbtn_select_range.Visible = true;
        }
    }

    /// <summary>
    /// Bind Questionnaire
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_Questionnaire_BAO questionnaire_BAO = new Survey_Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();

            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);

            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";

            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
    }

    /// <summary>
    /// Bind Question Category
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

        DataTable categoryid = categoryBusinessAccessObject.SelectCategory(Convert.ToInt32(identity.User.AccountID), questionnaireid);

        if (categoryid.Rows.Count > 0)
        {
            ddlQuestionCategory.DataSource = categoryid;
            ddlQuestionCategory.DataTextField = "CategoryName";
            ddlQuestionCategory.DataValueField = "CategoryID";
            ddlQuestionCategory.DataBind();
        }
    }

    public void Get_selected_Range(object sender, EventArgs e)
    {
        //Lbtn_select_range.Text =  grdvRange.Rows(e.NewSelectedIndex).Cells(2).Text;
        //int ii=grdvRange.SelectedRow.RowIndex;
        //GridViewRow row = grdvRange.SelectedRow;
        //Lbtn_select_range.Text = row.Cells[0].Text.ToString();
    }

    //protected void grdvRange_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)

    //{

    //    Lbtn_select_range.Text = grdvRange.Rows[e.NewSelectedIndex].Cells[0].Text.ToString();
        
    //   // Response.Write(grdvRange.SelectedRow.FindControl("").ToString());
    //}


    ////protected void grdvRange_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////    Response.Write("i am getting called");

    ////    testing = grdvRange.SelectedRow.Cells[0].Text.ToString();
    ////    Lbtn_select_range.Text = testing;
        
    ////}




    ////protected void Lbtn_select_range_Click(object sender, EventArgs e)
    ////{
    ////    Panel1.Visible = true;
    ////}




    ////protected void grdvRange_Load(object sender, EventArgs e)
    ////{
    ////    grdvRange.AllowPaging = true;
    ////}


    ////protected void Lbtn_select_range_Click1(object sender, EventArgs e)
    ////{
    ////    Panel1.Visible = true;
    ////}
    ////protected void grdvRange_SelectedIndexChanged(object sender, EventArgs e)
    ////{

    ////}
    //protected void DropDownList1_Load(object sender, EventArgs e)
    //{
       
    //}
    //protected void DropDownList1_Init(object sender, EventArgs e)
    //{
       
    //}


    //protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //        if (args.Value.ToString() == "Select")
    //        args.IsValid = false;
    //    else
    //        args.IsValid = true;
    //}
}
