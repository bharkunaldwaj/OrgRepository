using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using Admin_BAO;
using Questionnaire_BAO;
using System.Configuration;

public partial class Survey_Module_Questionnaire_ProjectList : CodeBehindBase
{
    Survey_Project_BAO projectBusinessObject = new Survey_Project_BAO();
    //Survey_Project_BE projectBusinessEntity = new Survey_Project_BE();
    WADIdentity identity;

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int projectCount = 0;
    string pageNo = "";
    DataTable dataTableCompanyName;
    //DataTable dataTableAllAccount;
    //string expression1;
    //string Finalexpression;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        //HandleWriteLog("Start", new StackTrace(true));
        identity = this.Page.User.Identity as WADIdentity;

        //odsProject.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
        //odsProject.Select();

        grdvProject.PageSize = pageSize;

        TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");

        if (textBoxGoto != null)
            textBoxGoto.Text = pageNo;

        Account_BAO accountBusinessObject = new Account_BAO();
        //Get Account details by Account ID and bind Account Dropdown.
        ddlAccountCode.DataSource = accountBusinessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
        ddlAccountCode.DataValueField = "AccountID";
        ddlAccountCode.DataTextField = "Code";
        ddlAccountCode.DataBind();

        if (!IsPostBack)
        {
            Survey_AccountUser_BAO accountUserBusinessObject = new Survey_AccountUser_BAO();
            //Get Account user details by Account ID and bind Manager Dropdown.
            ddlManager.DataSource = accountUserBusinessObject.GetdtAccountUserList(identity.User.AccountID.ToString());
            ddlManager.DataValueField = "UserID";
            ddlManager.DataTextField = "UserName";
            ddlManager.DataBind();

            //If User Is Super Admin then show account details section else hide.
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
            //Reset project object datasource property value with dynamic query .
            odsProject.SelectParameters.Clear();
            odsProject.SelectParameters.Add("accountID", GetCondition());
            odsProject.Select();
            //Manage Pagaing on Gridview page index change.
            ManagePaging();
        }
        //HandleWriteLog("Start", new StackTrace(true));
        //}
        //catch (Exception ex)
        //{
        //    HandleException(ex);
        //}
    }

    /// <summary>
    /// Add client side event to project list view controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvProject_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton LinkButtonDelete = (LinkButton)e.Row.Cells[7].Controls[0];
            LinkButtonDelete.OnClientClick = "if (!window.confirm('Are you sure you want to delete this project?')) return false";
        }
    }

    /// <summary>
    /// Sort grid by click on headings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvProject_Sorting(object sender, GridViewSortEventArgs e)
    {
        ManagePaging();
    }

    /// <summary>
    /// Redirect to Add new Project page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Projects.aspx", false);
    }

    #region Gridview Paging Related Methods
    /// <summary>
    /// Handle pagain on page index changeing of gridview.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        //if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        //    projectCount = project_BAO.GetProjectListCount(ddlAccountCode.SelectedValue);
        //else

        projectCount = projectBusinessObject.GetProjectListCount(GetCondition());

        plcPaging.Controls.Clear();

        if (projectCount > 0)
        {

            // Variable declaration
            int numberOfPages;
            int numberOfRecords = projectCount;
            int currentPage = (grdvProject.PageIndex);
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
        return new object[] { baseState, projectCount };
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

        grdvProject.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvProject.DataBind();

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

            grdvProject.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvProject.DataBind();
            ManagePaging();

            textBoxGoto.Text = pageNo;
        }
    }

    private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "datepicker", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
    }

    private void SetDTPicker1(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "datepicker1", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
    }

    #endregion

    #region Search Related Function
    /// <summary>
    ///Fill Gridview by geting selected values data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        odsProject.SelectParameters.Clear();
        odsProject.SelectParameters.Add("accountID", GetCondition());
        odsProject.Select();

        grdvProject.PageIndex = 0;
        grdvProject.DataBind();
        ManagePaging();
    }

    /// <summary>
    /// Reset controls value with default value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ddlstatus.SelectedValue = "0";
        ddlManager.SelectedValue = "0";
        txtreference.Text = "";
        txttitle.Text = "";

        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
        ddlAccountCode_SelectedIndexChanged(sender, e);

        odsProject.SelectParameters.Clear();
        odsProject.SelectParameters.Add("accountID", GetCondition());
        odsProject.Select();

        ManagePaging();
    }

    /// <summary>
    /// Bind manager when account selected index changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_AccountUser_BAO accountUserBusinessObject = new Survey_AccountUser_BAO();
        ddlManager.Items.Clear();
        ddlManager.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessObject = new Account_BAO();
            //Get account details
            dataTableCompanyName = accountBusinessObject.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //Set company name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
            //odsProject.SelectParameters.Clear();
            //odsProject.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue);
            //odsProject.Select();

            //ManagePaging();
            //Get account user list by account id and bind Manager dropdown.
            ddlManager.DataSource = accountUserBusinessObject.GetdtAccountUserList(ddlAccountCode.SelectedValue);
            ddlManager.DataValueField = "UserID";
            ddlManager.DataTextField = "UserName";
            ddlManager.DataBind();

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;
        }
        else
        {
            lblcompanyname.Text = "";

            //odsProject.SelectParameters.Clear();
            //odsProject.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            //odsProject.Select();

            //ManagePaging();

            ddlManager.DataSource = accountUserBusinessObject.GetdtAccountUserList(identity.User.AccountID.ToString());
            ddlManager.DataValueField = "UserID";
            ddlManager.DataTextField = "UserName";
            ddlManager.DataBind();

            ViewState["AccountID"] = "0";
        }
    }

    /// <summary>
    /// Build dynamic Query
    /// </summary>
    /// <returns></returns>
    public string GetCondition()
    {
        string stringQuery = "";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0)
            stringQuery = stringQuery + "" + ViewState["AccountID"] + " and ";
        else
            stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlstatus.SelectedIndex > 0)
            stringQuery = stringQuery + "Survey_MSTProjectStatus.[Name] = '" + ddlstatus.SelectedItem.Text.Trim() + "' and ";

        if (txtreference.Text.Trim() != string.Empty)
            stringQuery = stringQuery + "[Reference] like '" + txtreference.Text.Trim() + "%' and ";

        if (txttitle.Text.Trim() != string.Empty)
            stringQuery = stringQuery + "[Title] like '" + txttitle.Text.Trim() + "%' and ";

        if (ddlManager.SelectedIndex > 0)
            stringQuery = stringQuery + "[User].[UserID] = " + ddlManager.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }
    #endregion
}
