using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Diagnostics;
using DAF_BAO;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Survey_Module_Questionnaire_Questions : CodeBehindBase
{
    Survey_Questions_BAO questions_BAO = new Survey_Questions_BAO();
    Survey_Questions_BE questions_BE = new Survey_Questions_BE();
    Survey_Category_BAO category_BAO = new Survey_Category_BAO();
    List<Survey_Questions_BE> questions_BEList = new List<Survey_Questions_BE>();

    DataTable dtCompanyName;
    DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
    WADIdentity identity;
    static string testing;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable getrange = questions_BAO.getrange_data();
            DropDownList1.DataSource = getrange;
            DropDownList1.DataTextField = "Range_Name";

            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, new ListItem("Select", "0"));
            
        }
        try
        {
           Label llx = (Label)this.Master.FindControl("Current_location");
            llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {

                identity = this.Page.User.Identity as WADIdentity;
                //Category_BAO category_BAO = new Category_BAO();
                //ddlQuestionCategory.DataSource = category_BAO.GetdtCategoryList();
                //ddlQuestionCategory.DataTextField = "CategoryName";
                //ddlQuestionCategory.DataValueField = "CategoryID";
                //ddlQuestionCategory.DataBind();

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();


                Questionnaire_BAO.Survey_Questionnaire_BAO questionnnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
                ddlQuestionnaire.DataSource = questionnnaire_BAO.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                int questionsID = Convert.ToInt32(Request.QueryString["quesId"]);
                questions_BEList = questions_BAO.GetQuestionsByID(questionsID);

                if (questions_BEList.Count > 0)
                {
                    SetQuestionsValue(questions_BEList);

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

                    Account_BAO account1_BAO = new Account_BAO();

                    dtCompanyName = account1_BAO.GetdtAccountList(Convert.ToString(companycode));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = dtCompanyName.Select(Finalexpression);

                    DataTable dtAccount = dtCompanyName.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                    {
                        dtAccount.ImportRow(drAccount);
                    }

                    lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

                    Questionnaire_BAO.Survey_Questionnaire_BAO questionnnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
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


            ddlQuestionType.SelectedValue = questions_BEList[0].QuestionTypeID.ToString();
            ddlQuestionnaire.SelectedValue = questions_BEList[0].QuestionnaireID.ToString();

            ddlQuestionCategory.Items.Clear();
            ddlQuestionCategory.Items.Insert(0, new ListItem("Select", "0"));

            DataTable categoryid = new DataTable();

            if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                categoryid = category_BAO.SelectCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue));
            else
                categoryid = category_BAO.SelectCategory(Convert.ToInt32(identity.User.AccountID), Convert.ToInt32(ddlQuestionnaire.SelectedValue));

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
            Survey_Questions_BE questions_BE = new Survey_Questions_BE();
            Survey_Questions_BAO questions_BAO = new Survey_Questions_BAO();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {

                questions_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

            }
            else
            {
                questions_BE.AccountID = identity.User.AccountID;
            }

            if (identity.User.GroupID == 1)
            {

                questions_BE.CompanyID = Convert.ToInt32(ddlAccountCode.SelectedValue);

            }
            else
            {
                questions_BE.CompanyID = identity.User.AccountID;
            }

            questions_BE.QuestionTypeID = Convert.ToInt32(ddlQuestionType.SelectedValue);
            questions_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            questions_BE.CateogryID = Convert.ToInt32(ddlQuestionCategory.SelectedValue);
           questions_BE.Sequence = Convert.ToInt32(GetString(txtSequence.Text));
            questions_BE.Validation = Convert.ToInt32(ddlValidation.SelectedValue);
            questions_BE.ValidationText = ddlValidation.SelectedItem.Text;
            questions_BE.Title = ""; //txtTitle.Text;
            questions_BE.Token = Convert.ToInt32(ddlTokens.SelectedValue);
            questions_BE.TokenText = ddlTokens.SelectedItem.Text;
            questions_BE.Description = txtQuestionText.Text;
           // questions_BE.DescriptionSelf = txtQuestionSelfText.Text;
            questions_BE.Hint = txtUsageHint.Text;

            questions_BE.LengthMIN = (txtMinLength.Text == "" ? 0 : Convert.ToInt32(txtMinLength.Text));
            questions_BE.LengthMAX = (txtMaxLength.Text == "" ? 0 : Convert.ToInt32(txtMaxLength.Text));
            questions_BE.Multiline = chkmultiline.Checked;

            ////////questions_BE.LowerLabel = txtLowerLabel.Text;
            ////////questions_BE.UpperLabel = txtUpperLabel.Text;
            ////////questions_BE.LowerBound = (txtLowerBound.Text == "" ? 0 : Convert.ToInt32(txtLowerBound.Text));
            ////////questions_BE.UpperBound = (txtUpperBound.Text == "" ? 0 : Convert.ToInt32(txtUpperBound.Text));
            ////////questions_BE.Increment = (txtIncrement.Text == "" ? 0 : Convert.ToInt32(txtIncrement.Text));
            ////////questions_BE.Reverse = chkReverse.Checked;

            questions_BE.ModifyBy = 1;
            questions_BE.ModifyDate = DateTime.Now;
            questions_BE.IsActive = 1;
            questions_BE.Range_Name = DropDownList1.SelectedValue.ToString();
            if (Request.QueryString["Mode"] == "E")
            {
                questions_BE.QuestionID = Convert.ToInt32(Request.QueryString["quesId"]);
                
                questions_BAO.UpdateQuestions(questions_BE);
            }
            else
            {
                questions_BAO.AddQuestions(questions_BE);
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
    protected void ddlQuestionType_SelectedIndexChanged(object sender, EventArgs e)
    {

            if (ddlQuestionType.SelectedIndex == 0)
        {
            divFreeText.Visible = false;
            DropDownList1.Visible = false;
            RequiredFieldValidator1.Enabled =false;
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

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Questionnaire_BAO.Survey_Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO account_BAO = new Account_BAO();

            dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dtCompanyName.Clone();
            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

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

    protected void ddlQuestionnaire_SelectedIndexChanged(object sender, EventArgs e)
    {
        int questionnaireid = 0;
        identity = this.Page.User.Identity as WADIdentity;

        ddlQuestionCategory.Items.Clear();
        ddlQuestionCategory.Items.Insert(0, new ListItem("Select", "0"));
        questionnaireid = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

        DataTable categoryid = category_BAO.SelectCategory(Convert.ToInt32(identity.User.AccountID), questionnaireid);

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
