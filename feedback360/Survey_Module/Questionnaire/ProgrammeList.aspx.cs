using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using Admin_BAO;
using Questionnaire_BAO;
using System.Configuration;
using System.Diagnostics;

public partial class Survey_Module_Questionnaire_ProgrammeList : CodeBehindBase
{
    //Global variables
    Survey_Programme_BAO programmeBusinessObject = new Survey_Programme_BAO();
    // Survey_Project_BE project_BE = new Survey_Project_BE();
    WADIdentity identity;

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int programmeCount = 0;
    string pageNo = "";
    DataTable dataTableCompanyName;
    //DataTable dtAllAccount;
    //string expression1;
    //string Finalexpression;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;
            //odsProgramme.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            //odsProgramme.Select();

            grdvProgramme.PageSize = pageSize;

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;

            Account_BAO accountBusinessObject = new Account_BAO();
            //Get account details by user account id and bind account drop downlist.
            ddlAccountCode.DataSource = accountBusinessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            if (!IsPostBack)
            {
                Survey_Project_BAO projectBusinessObject = new Survey_Project_BAO();
                //Get project details by user account id and bind project drop downlist.
                ddlproject.DataSource = projectBusinessObject.GetAccProject(Convert.ToInt32(identity.User.AccountID));
                ddlproject.DataValueField = "ProjectID";
                ddlproject.DataTextField = "Title";
                ddlproject.DataBind();
                //If User is Super Admin  then show account details section else hide.
                if (identity.User.GroupID == 1)
                {
                    divAccount.Visible = true;
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else
                {
                    divAccount.Visible = false;
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }

                ViewState["ProjectID"] = "0";
                ViewState["Programme"] = "";
                //Set Parameter for gridview object dateasource with dynamic query.
                odsProgramme.SelectParameters.Clear();
                odsProgramme.SelectParameters.Add("accountID", GetCondition());
                odsProgramme.Select();
                //Manage Pagaing for gridview.
                ManagePaging();
            }

            HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind client side event to gridview controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvProgramme_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[8].Controls[0];
                linkButtonDelete.OnClientClick = "if (!window.confirm('Are you sure you want to delete this programme?')) return false";
            }

            if (identity.User.GroupID == 2)
            {
                if (e.Row.RowType != DataControlRowType.EmptyDataRow)
                    e.Row.Cells[8].Visible = false;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Sort gridview when clicked on heading.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvProgramme_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ManagePaging();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect to Add new program .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("Programme.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region Gridview Paging Related Methods
    /// <summary>
    /// Manage Paging when gridview page index changes.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        //if (Convert.ToInt32(ViewState["AccountID"]) > 0)
        //    programmeCount = programme_BAO.GetProgrammeListCount(ViewState["AccountID"].ToString());
        //else
        programmeCount = programmeBusinessObject.GetProgrammeListCount(GetCondition());

        plcPaging.Controls.Clear();

        if (programmeCount > 0)
        {
            // Variable declaration
            int numberOfPages;
            int numberOfRecords = programmeCount;
            int currentPage = (grdvProgramme.PageIndex);
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
        return new object[] { baseState, programmeCount };
    }

    /// <summary>
    /// Load the view state for the page when expires.
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
    /// Reset Gridview page index  whaen click on prevoius and next button of gridview pagain.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();

        LinkButton objlb = (LinkButton)sender;
        //Set Gridview page index and bind grid.
        grdvProgramme.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
        grdvProgramme.DataBind();

        ManagePaging();
    }

    /// <summary>
    /// Rebind gridview when click on go button to paticular page.
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
            //Set Gridview page index and bind grid.
            grdvProgramme.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvProgramme.DataBind();
            ManagePaging();

            textBoxGoto.Text = pageNo;
        }
    }

    #endregion

    #region Search Related Function
    /// <summary>
    /// Search Program details and bind gridview according to build query.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["ProjectID"] = ddlproject.SelectedValue;
        ViewState["Programme"] = txtprogramme.Text.Trim();
        //Set gridview object data source to build query.
        odsProgramme.SelectParameters.Clear();
        string strParam = GetCondition();
        odsProgramme.SelectParameters.Add("accountID", strParam);
        odsProgramme.Select();
        //Bind gridview
        grdvProgramme.PageIndex = 0;
        grdvProgramme.DataBind();

        ManagePaging();
    }

    /// <summary>
    /// Reset Control values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ddlproject.SelectedValue = "0";
        txtprogramme.Text = "";

        ViewState["ProjectID"] = "0";
        ViewState["Programme"] = "";

        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
        ddlAccountCode_SelectedIndexChanged(sender, e);

        odsProgramme.SelectParameters.Clear();
        odsProgramme.SelectParameters.Add("accountID", GetCondition());
        odsProgramme.Select();

        ManagePaging();
    }

    /// <summary>
    /// Rebind Program controls when by account id .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_Project_BAO projectBusinessObject = new Survey_Project_BAO();
        ddlproject.Items.Clear();
        ddlproject.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessObject = new Account_BAO();

            dataTableCompanyName = accountBusinessObject.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);

            //lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            //odsProgramme.SelectParameters.Clear();
            //odsProgramme.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue);
            //odsProgramme.Select();

            //ManagePaging();
            ViewState["AccountID"] = ddlAccountCode.SelectedValue;
            //GEt all project in that account and bind project drop down.
            ddlproject.DataSource = projectBusinessObject.GetAccProject(Convert.ToInt32(ddlAccountCode.SelectedValue));
            ddlproject.DataValueField = "ProjectID";
            ddlproject.DataTextField = "Title";
            ddlproject.DataBind();
        }
        else
        {
            //lblcompanyname.Text = "";
            ViewState["AccountID"] = "0";
            //odsProgramme.SelectParameters.Clear();
            //odsProgramme.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            //odsProgramme.Select();

            //ManagePaging();
            //GEt all project in that account and bind project drop down.
            ddlproject.DataSource = projectBusinessObject.GetAccProject(Convert.ToInt32(identity.User.AccountID));
            ddlproject.DataValueField = "ProjectID";
            ddlproject.DataTextField = "Title";
            ddlproject.DataBind();
        }
    }

    /// <summary>
    /// Build quey for gridview data source.
    /// </summary>
    /// <returns></returns>
    public string GetCondition()
    {
        string stringQuery = "";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0)
            stringQuery = stringQuery + "" + ViewState["AccountID"] + " and ";
        else
            stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

        if (Convert.ToInt32(ViewState["ProjectID"]) > 0)
            stringQuery = stringQuery + "[Survey_Project].[ProjectID] = " + ViewState["ProjectID"].ToString() + " and ";

        if (ViewState["Programme"] != string.Empty)
            stringQuery = stringQuery + "[ProgrammeName] like '" + ViewState["Programme"].ToString() + "%' and ";

        if (ddlCompany.SelectedIndex > 0)
            stringQuery = stringQuery + "Survey_Analysis_Sheet.[CompanyID] = " + ddlCompany.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }

    #endregion

    /// <summary>
    /// Its of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Bind company when project is changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_Company_BAO companyBusinessObject = new Survey_Company_BAO();
        var dataTableCompany = companyBusinessObject.GetdtCompanyList(GetCompanyCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dataTableCompany;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();
    }

    /// <summary>
    /// Build quey for gridview data source.
    /// </summary>
    /// <returns></returns>
    public string GetCompanyCondition()
    {
        string stringQuery = "";

        //if (Convert.ToInt32(ViewState["AccountID"]) > 0)
        //    str = str + "" + ViewState["AccountID"] + " and ";
        //else
        //    str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlAccountCode.SelectedIndex > 0)
            stringQuery = stringQuery + "" + ddlAccountCode.SelectedValue + " and ";

        if (ddlproject.SelectedIndex > 0)
            stringQuery = stringQuery + "Survey_Project.[ProjectID] = " + ddlproject.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }
}
