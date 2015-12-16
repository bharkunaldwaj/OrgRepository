using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;

using Admin_BAO;
using Questionnaire_BAO;
using Questionnaire_BE;

public partial class Module_Questionnaire_QuestionList : CodeBehindBase
{
    //Global variables
    Questions_BAO questionsBusinessAccessObject = new Questions_BAO();
    Questions_BE questionsBusinessEntity = new Questions_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int questionsCount = 0;
    string pageNo = "";
    WADIdentity identity;
    DataTable dataTableCompanyName;
    //DataTable dtAllAccount;
    // string expression1;
    // string Finalexpression;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            //odsQuestions.SelectParameters.Clear();
            //odsQuestions.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            //odsQuestions.Select();
            if (!IsPostBack)
            {
                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get Account list by user account id and bind account dropdown.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
                //Get Questionnaire list and bind Questionnaire dropdown by user account Id.
                ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataBind();

                //HandleWriteLog("Start", new StackTrace(true));

                //If user is a Super Admin then show account detail section else hide.
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
                //Initilize the question grid data source parameter with dynamic query.
                odsQuestions.SelectParameters.Clear();
                odsQuestions.SelectParameters.Add("accountID", GetCondition());
                odsQuestions.Select();
                //Manage paging.
                ManagePaging();

            }

            grdvQuestions.PageSize = pageSize;

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");

            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    ///  Add client side event to gridview view controls row by row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvQuestions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibtn = (LinkButton)e.Row.Cells[7].Controls[0];
                ibtn.OnClientClick = "if (!window.confirm('Are you sure you want to delete this question?')) return false";
            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    ///Handle paging while Sorting grid by click on headings .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvQuestions_Sorting(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            ManagePaging();

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect to Question page when click on Add new.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("Questions.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region Gridview Paging Related Methods
    /// <summary>
    /// Handle paging whengrid page index changes.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        //if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        //    questionsCount = questions_BAO.GetQuestionsListCount(ddlAccountCode.SelectedValue);
        //else
        questionsCount = questionsBusinessAccessObject.GetQuestionsListCount(GetCondition());

        plcPaging.Controls.Clear();

        if (questionsCount > 0)
        {

            // Variable declaration
            int numberOfPages;
            int numberOfRecords = questionsCount;
            int currentPage = (grdvQuestions.PageIndex);
            StringBuilder strSummary = new StringBuilder();

            // If number of records is more then the page size (specified in global variable)
            // Just to check either gridview have enough records to implement paging
            if (numberOfRecords > pageSize)
            {
                // Calculating the total number of pages
                numberOfPages = (int)Math.Ceiling((double)numberOfRecords / (double)pageSize);
            }
            else
            {
                numberOfPages = 1;
            }

            // Creating a small summary for records.
            strSummary.Append("Displaying <b>");

            // Creating X f X Records
            int floor = (currentPage * pageSize) + 1;
            strSummary.Append(floor.ToString());
            strSummary.Append("</b>-<b>");
            int ceil = ((currentPage * pageSize) + pageSize);

            //let say you have 26 records and you specified 10 page size, 
            // On the third page it will return 30 instead of 25 as that is based on pageSize
            // So this check will see if the ceil value is increasing the number of records. Consider numberOfRecords
            if (ceil > numberOfRecords)
            {
                strSummary.Append(numberOfRecords.ToString());
            }
            else
            {
                strSummary.Append(ceil.ToString());
            }

            // Displaying Total number of records Creating X of X of About X records.
            strSummary.Append("</b> of <b>");
            strSummary.Append(numberOfRecords.ToString());
            strSummary.Append("</b> records</br>");


            litPagingSummary.Text = ""; // strSummary.ToString();


            //Variable declaration 
            //these variables will used to calculate page number display
            int pageShowLimitStart = 1;
            int pageShowLimitEnd = 1;

            // Just to check, either there is enough pages to implement page number display logic.
            if (pageDispCount > numberOfPages)
            {
                pageShowLimitEnd = numberOfPages; // Setting the end limit to the number of pages. Means show all page numbers
            }
            else
            {
                if (currentPage > 4) // If page index is more then 4 then need to less the page numbers from start and show more on end.
                {
                    //Calculating end limit to show more page numbers
                    pageShowLimitEnd = currentPage + (int)(Math.Floor((decimal)pageDispCount / 2));
                    //Calculating Start limit to hide previous page numbers
                    pageShowLimitStart = currentPage - (int)(Math.Floor((decimal)pageDispCount / 2));
                }
                else
                {
                    //Simply Displaying the 10 pages. no need to remove / add page numbers
                    pageShowLimitEnd = pageDispCount;
                }
            }

            // Since the pageDispCount can be changed and limit calculation can cause < 0 values 
            // Simply, set the limit start value to 1 if it is less
            if (pageShowLimitStart < 1)
                pageShowLimitStart = 1;


            //Dynamic creation of link buttons

            // First Link button to display with paging
            LinkButton objLbFirst = new LinkButton();
            objLbFirst.Click += new EventHandler(objLb_Click);
            //objLbFirst.Text = "First";
            objLbFirst.CssClass = "first";
            objLbFirst.ToolTip = "First Page";
            objLbFirst.ID = "lb_FirstPage";
            objLbFirst.CommandName = "pgChange";
            objLbFirst.EnableViewState = true;
            objLbFirst.CommandArgument = "1";

            //Previous Link button to display with paging
            LinkButton objLbPrevious = new LinkButton();
            objLbPrevious.Click += new EventHandler(objLb_Click);
            //objLbPrevious.Text = "Previous";
            objLbPrevious.CssClass = "previous";
            objLbPrevious.ToolTip = "Previous Page";
            objLbPrevious.ID = "lb_PreviousPage";
            objLbPrevious.CommandName = "pgChange";
            objLbPrevious.EnableViewState = true;
            objLbPrevious.CommandArgument = currentPage.ToString();


            //of course if the page is the 1st page, then there is no need of First or Previous
            if (currentPage == 0)
            {
                objLbFirst.Enabled = false;
                objLbPrevious.Enabled = false;
            }
            else
            {
                objLbFirst.Enabled = true;
                objLbPrevious.Enabled = true;
            }

            plcPaging.Controls.Add(new LiteralControl("<table border=0><tr><td valign=middle>"));

            //Adding control in a place holder
            plcPaging.Controls.Add(objLbFirst);
            //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;")); // Just to give some space 
            plcPaging.Controls.Add(objLbPrevious);
            //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));


            // Creatig page numbers based on the start and end limit variables.
            for (int i = pageShowLimitStart; i <= pageShowLimitEnd; i++)
            {
                if ((Page.FindControl("lb_" + i.ToString()) == null) && i <= numberOfPages)
                {
                    LinkButton objLb = new LinkButton();
                    objLb.Click += new EventHandler(objLb_Click);
                    objLb.Text = i.ToString();
                    objLb.ID = "lb_" + i.ToString();
                    objLb.ToolTip = "Page " + i.ToString();
                    objLb.CommandName = "pgChange";
                    objLb.EnableViewState = true;
                    objLb.CommandArgument = i.ToString();

                    if ((currentPage + 1) == i)
                    {
                        objLb.CssClass = "active";
                        objLb.Enabled = false;

                    }

                    plcPaging.Controls.Add(objLb);
                    //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));
                }
            }

            // Last Link button to display with paging
            LinkButton objLbLast = new LinkButton();
            objLbLast.Click += new EventHandler(objLb_Click);
            //objLbLast.Text = "Last";
            objLbLast.CssClass = "last";
            objLbLast.ToolTip = "Last Page";
            objLbLast.ID = "lb_LastPage";
            objLbLast.CommandName = "pgChange";
            objLbLast.EnableViewState = true;
            objLbLast.CommandArgument = numberOfPages.ToString();

            // Next Link button to display with paging
            LinkButton objLbNext = new LinkButton();
            objLbNext.Click += new EventHandler(objLb_Click);
            //objLbNext.Text = "Next";
            objLbNext.CssClass = "next";
            objLbNext.ToolTip = "Next Page";
            objLbNext.ID = "lb_NextPage";
            objLbNext.CommandName = "pgChange";
            objLbNext.EnableViewState = true;
            objLbNext.CommandArgument = (currentPage + 2).ToString();

            //of course if the page is the last page, then there is no need of last or next
            if ((currentPage + 1) == numberOfPages)
            {
                objLbLast.Enabled = false;
                objLbNext.Enabled = false;
            }
            else
            {
                objLbLast.Enabled = true;
                objLbNext.Enabled = true;
            }

            // Adding Control to the place holder
            plcPaging.Controls.Add(objLbNext);
            //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));
            plcPaging.Controls.Add(objLbLast);
            //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));

            plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));
            TextBox objtxtGoto = new TextBox();
            objtxtGoto.ID = "txtGoto";
            objtxtGoto.ToolTip = "Enter Page No.";
            objtxtGoto.MaxLength = 2;
            objtxtGoto.SkinID = "grdvgoto";
            objtxtGoto.Attributes.Add("onKeypress", "javascript:return NumberOnly(this);");
            objtxtGoto.Text = pageNo;
            plcPaging.Controls.Add(objtxtGoto);

            plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));

            ImageButton objIbtnGo = new ImageButton();
            objIbtnGo.ID = "ibtnGo";
            objIbtnGo.ToolTip = "Goto Page";
            objIbtnGo.ImageUrl = "~/Layouts/Resources/images/go-btn.png";
            objIbtnGo.Click += new ImageClickEventHandler(objIbtnGo_Click);
            plcPaging.Controls.Add(objIbtnGo);

            plcPaging.Controls.Add(new LiteralControl("</td></tr></table>"));
        }
    }

    /// <summary>
    /// Save the view state for the page.
    /// </summary>
    /// <returns></returns>
    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, questionsCount };
    }

    /// <summary>
    /// Load the view state for the page when view of the page expires.
    /// </summary>
    /// <param name="savedState"></param>
    protected override void LoadViewState(object savedState)
    {
        object[] myState = (object[])savedState;
        if (myState[0] != null)
            base.LoadViewState(myState[0]);

        if (myState[1] != null)
        {
            //dt = (DataTable)myState[1];
            //grdvCategory.DataSourceID = odsCategory.ID;
            //grdvCategory.DataBind();

            ManagePaging();
        }
    }

    /// <summary>
    /// Handle prvious and next button click of grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();
        LinkButton linkButtonNext = (LinkButton)sender;
        //Reset gridview page index.
        grdvQuestions.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvQuestions.DataBind();

        ManagePaging();
    }

    /// <summary>
    /// Handle gridview page index event to move to new page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objIbtnGo_Click(object sender, ImageClickEventArgs e)
    {
        TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");
        if (textBoxGoto.Text.Trim() != "")
        {
            pageNo = textBoxGoto.Text;
            plcPaging.Controls.Clear();
            //Reset gridview page index.
            grdvQuestions.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvQuestions.DataBind();//Rebind grid.
            ManagePaging();

            textBoxGoto.Text = pageNo;
        }
    }
    #endregion

    /// <summary>
    /// Bind category grid when account selected index changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        //If it is a supeer Admin then ddlaccount's value else user Account value is used.
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();

            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //Set comapny name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            //odsQuestions.SelectParameters.Clear();
            //odsQuestions.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue);
            //odsQuestions.Select();

            //ManagePaging();
            //Bind Question drop down when account selected index changes.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;
        }
        else
        {
            lblcompanyname.Text = "";
            ViewState["AccountID"] = "0";
            //odsQuestions.SelectParameters.Clear();
            //odsQuestions.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            //odsQuestions.Select();

            //ManagePaging();
            //Bind Question drop down by user account ID.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
    }

    /// <summary>
    /// Search question list and bind question Grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        odsQuestions.SelectParameters.Clear();
        // Get object datasource parameters value by dynamic query.
        odsQuestions.SelectParameters.Add("accountID", GetCondition());
        odsQuestions.Select();
        //Set grid page index to 0.
        grdvQuestions.PageIndex = 0;
        grdvQuestions.DataBind();
        ManagePaging();
    }

    /// <summary>
    /// Reset control value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ddlQuestionnaire.SelectedIndex = 0;
        ddlQuestionType.SelectedIndex = 0;
        lblMessage.Text = "";
        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
        ddlAccountCode_SelectedIndexChanged(sender, e);

        //Reset object datasource parameters value.
        odsQuestions.SelectParameters.Clear();
        odsQuestions.SelectParameters.Add("accountID", GetCondition());
        odsQuestions.Select();

        ManagePaging();
    }

    /// <summary>
    /// Change the order number of the question by specified order number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReSequence_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            //server side Validate for Sequence number. 
            if (ddlQuestionnaire.SelectedIndex == 0 && txtSequenceIncrement.Text.Trim() == "")
            {
                lblMessage.Text = "Please select questionnaire <br>Please enter sequence increment value";
                ddlQuestionnaire.Focus();
            }
            else if (ddlQuestionnaire.SelectedIndex == 0)
            {
                lblMessage.Text = "Please select questionnaire";
                ddlQuestionnaire.Focus();
            }
            else if (txtSequenceIncrement.Text.Trim() == "")
            {
                lblMessage.Text = "Please enter sequence increment value";
                txtSequenceIncrement.Focus();
            }
            else
            {
                identity = this.Page.User.Identity as WADIdentity;
                Questions_BAO question_BAO = new Questions_BAO();
                lblMessage.Text = "";
                //If user is Super Admin then group Id==1 then reinitilize the object datsource for question.
                if (identity.User.GroupID == 1)
                {
                    question_BAO.ResequenceQuestion(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue), Convert.ToInt32(txtSequenceIncrement.Text.Trim()));
                    odsQuestions.SelectParameters.Clear();
                    // Reinitilize the object datsource for question.
                    odsQuestions.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue.ToString());
                    odsQuestions.Select();
                }
                else
                {
                    question_BAO.ResequenceQuestion(Convert.ToInt32(identity.User.AccountID), Convert.ToInt32(ddlQuestionnaire.SelectedValue), Convert.ToInt32(txtSequenceIncrement.Text.Trim()));
                    odsQuestions.SelectParameters.Clear();
                    // Reinitilize the object datsource for question.
                    odsQuestions.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
                    odsQuestions.Select();
                }
            }

            imbSubmit_Click(sender, e);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Generate dynamic query .
    /// </summary>
    /// <returns></returns>
    protected string GetCondition()
    {
        string stringQuery = "";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0)
            stringQuery = stringQuery + "" + ViewState["AccountID"] + " and ";
        else
            stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlQuestionnaire.SelectedIndex > 0)
            stringQuery = stringQuery + "[QSTNName] = '" + ddlQuestionnaire.SelectedItem.Text.Trim() + "' and ";

        if (ddlQuestionType.SelectedIndex > 0)
            stringQuery = stringQuery + "[Name] = '" + ddlQuestionType.SelectedItem.Text.Trim() + "' and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }
}
