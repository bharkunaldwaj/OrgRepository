using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;

using Admin_BAO;
using Questionnaire_BAO;

public partial class Module_Questionnaire_CategoryList : CodeBehindBase
{
    //Global variables
    Survey_Category_BAO categoryBusinessAccessObject = new Survey_Category_BAO();
    //Survey_Category_BE category_BE = new Survey_Category_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int categoryCount = 0;
    string pageNo = "";
    WADIdentity identity;
    DataTable dataTableCompanyName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            if (!IsPostBack)
            {
                Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
                //Get Questionnaire details by user account id  to bind Questionnaire dropdown.
                ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataBind();

                //ddlSeqQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                //ddlSeqQuestionnaire.DataValueField = "QuestionnaireID";
                //ddlSeqQuestionnaire.DataTextField = "QSTNName";
                //ddlSeqQuestionnaire.DataBind();

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get Account details by user account id  to bind account dropdown.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                //If user is a Super Admin then show account detail section else hide.
                if (identity.User.GroupID == 1)//Super Admin.
                {
                    divAccount.Visible = true;
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else
                {
                    divAccount.Visible = false;
                }
                //Set the Parameter for category object data source.
                odsCategory.SelectParameters.Clear();
                odsCategory.SelectParameters.Add("accountID", GetCondition());
                odsCategory.Select();
                //Manageing pagain when grid bind.
                ManagePaging();
            }

            grdvCategory.PageSize = pageSize;

            TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");

            if (txtGoto != null)
                txtGoto.Text = pageNo;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    ///  Add client side event to gridview view controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[9].Controls[0];
                linkButtonDelete.OnClientClick = "if (!window.confirm('Are you sure you want to delete this category?')) return false";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Handle paging while Sorting grid by click on headings .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCategory_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            grdvCategory.PageIndex = 0;
            grdvCategory.DataBind();

            ManagePaging();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect to Category page when click on Add new.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("Category.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region Gridview Paging Related Methods
    /// <summary>
    ///  Handle paging related events when moving from grid view one page to another.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        //if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        //    categoryCount = category_BAO.GetCategoryListCount(ddlAccountCode.SelectedValue);
        //else
        categoryCount = categoryBusinessAccessObject.GetCategoryListCount(GetCondition());

        plcPaging.Controls.Clear();

        if (categoryCount > 0)
        {

            // Variable declaration
            int numberOfPages;
            int numberOfRecords = categoryCount;
            int currentPage = (grdvCategory.PageIndex);
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
                    objLb.CommandName = "pgChange";
                    objLb.ToolTip = "Page " + i.ToString();
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
        return new object[] { baseState, categoryCount };
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
    /// Handle previous and next button click of grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();

        LinkButton linkButtonNext = (LinkButton)sender;
        //Reset gridview page index.
        grdvCategory.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvCategory.DataBind();

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
            grdvCategory.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvCategory.DataBind();
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
        Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        //If it is a supeer Admin then ddlaccount's value else user Account value.
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

            //odsCategory.SelectParameters.Clear();
            //odsCategory.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue);
            //odsCategory.Select();

            //ManagePaging();
            ViewState["AccountID"] = ddlAccountCode.SelectedValue;
            //Bind Questionnaire drop down when account selected index changes.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";
            ViewState["AccountID"] = "0";
            //odsCategory.SelectParameters.Clear();
            //odsCategory.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            //odsCategory.Select();

            //ManagePaging();
            //Bind Questionnaire drop down by user account ID.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
    }

    #region Search Related Function
    /// <summary>
    /// Search category list and bind category Grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        //string str = "";

        //if (txtCategoryName.Text.Trim() != string.Empty)
        //    str = str + "[CategoryName] like '" + txtCategoryName.Text.Trim() + "%' and ";

        //if (ddlQuestionnaire.SelectedIndex > 0)
        //    str = str + "[QSTNName] = '" + ddlQuestionnaire.SelectedItem.Text.Trim() + "' and ";

        //if (str.Trim().Length != 0)
        //{
        //    string param = str.Substring(0, str.Length - 4);
        //    odsCategory.FilterExpression = param;
        //    odsCategory.FilterParameters.Clear();
        //}
        //else
        //{
        //    odsCategory.FilterExpression = null;
        //    odsCategory.FilterParameters.Clear();
        //}

        //Initilize the object data source value 
        odsCategory.SelectParameters.Clear();
        odsCategory.SelectParameters.Add("accountID", GetCondition());
        odsCategory.Select();
        //Set page index to  0.
        grdvCategory.PageIndex = 0;
        //Bind category grid.
        grdvCategory.DataBind();
        //Manage pagaing.
        ManagePaging();
    }

    /// <summary>
    /// Reset control value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        txtCategoryName.Text = "";
        ddlQuestionnaire.SelectedIndex = 0;

        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
        ddlAccountCode_SelectedIndexChanged(sender, e);
        //Reset object datasource parameters value.
        odsCategory.SelectParameters.Clear();
        odsCategory.SelectParameters.Add("accountID", GetCondition());
        odsCategory.Select();

        ManagePaging();
    }
    #endregion

    /// <summary>
    /// Change the order number of the category by specified order number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReSequence_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
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
                Survey_Category_BAO categoryBusinessAccessObject = new Survey_Category_BAO();
                lblMessage.Text = "";
                //If user is Super Admin then group Id==1 then reinitilize the object datsource for category by account selected value else user account id.
                if (identity.User.GroupID == 1)
                {
                    // Reinitilize the object datsource for category.
                    categoryBusinessAccessObject.ResequenceCategory(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlQuestionnaire.SelectedValue), Convert.ToInt32(txtSequenceIncrement.Text.Trim()));
                    odsCategory.SelectParameters.Clear();
                    odsCategory.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue.ToString());
                    odsCategory.Select();
                }
                else
                {
                    // Reinitilize the object datsource for category.
                    categoryBusinessAccessObject.ResequenceCategory(Convert.ToInt32(identity.User.AccountID), Convert.ToInt32(ddlQuestionnaire.SelectedValue), Convert.ToInt32(txtSequenceIncrement.Text.Trim()));
                    odsCategory.SelectParameters.Clear();
                    odsCategory.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
                    odsCategory.Select();
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

        if (txtCategoryName.Text.Trim() != string.Empty)
            stringQuery = stringQuery + "[CategoryName] like '" + txtCategoryName.Text.Trim() + "%' and ";

        if (ddlQuestionnaire.SelectedIndex > 0)
            stringQuery = stringQuery + "[QSTNName] = '" + ddlQuestionnaire.SelectedItem.Text.Trim() + "' and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }
}
