using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using Admin_BAO;
using Questionnaire_BAO;
using System.Configuration;
using System.Diagnostics;
using DatabaseAccessUtilities;

public partial class Survey_Module_Questionnaire_ExternalLinkList : CodeBehindBase
{
    //Global variables.
    Survey_ExternalLink_BAO externalLinkBusinessAccessObject = new Survey_ExternalLink_BAO();
    Common_BAO commonBusinessAccessObject = new Common_BAO();

    WADIdentity identity;

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int programmeCount = 0;
    string pageNo = "";

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

            //set page default value.
            grdvExternalLink.PageSize = pageSize;

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");

            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get user account list by user account id and bind account drop down.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            if (!IsPostBack)
            {
                Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
                //get all Project in an account and bind project drop down.
                ddlproject.DataSource = projectBusinessAccessObject.GetAccProject(Convert.ToInt32(identity.User.AccountID));
                ddlproject.DataValueField = "ProjectID";
                ddlproject.DataTextField = "Title";
                ddlproject.DataBind();

                //If user Group =1 then user is a super Admin and we show account details section else hide.
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
                //
                BindExternalLinkGrid(true);
                //Handle paging when page index changes.
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
    /// Sort link when click on header and mamage paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvExternalLink_Sorting(object sender, GridViewSortEventArgs e)
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
    /// Redirect to Add new External link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("AddExternalLink.aspx", false);
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
        programmeCount = BindExternalLinkGrid(false);

        plcPaging.Controls.Clear();

        if (programmeCount > 0)
        {

            // Variable declaration
            int numberOfPages;
            int numberOfRecords = programmeCount;
            int currentPage = (grdvExternalLink.PageIndex);
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

            // ManagePaging();
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
        LinkButton linkButtonNext = (LinkButton)sender;
        //Reset Gridview page index to new index.
        grdvExternalLink.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvExternalLink.DataBind();
        //Handle paging
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
            grdvExternalLink.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvExternalLink.DataBind();
            //Handle paging
            ManagePaging();

            textBoxGoto.Text = pageNo;
        }
    }

    #endregion

    #region Search Related Function
    /// <summary>
    /// Bind External link grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["ProjectID"] = ddlproject.SelectedValue;

        BindExternalLinkGrid(true);
        //set page index to 0 and bind grid.
        grdvExternalLink.PageIndex = 0;
        grdvExternalLink.DataBind();
        //Handle paging
        ManagePaging();
    }

    /// <summary>
    /// Reset controls with default value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ddlproject.SelectedValue = "0";

        ViewState["ProjectID"] = "0";
        ViewState["Programme"] = "";

        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
        ddlAccountCode_SelectedIndexChanged(sender, e);

        BindExternalLinkGrid(true);

        ManagePaging();
    }

    /// <summary>
    /// Bind project by account
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
        ddlproject.Items.Clear();
        ddlproject.Items.Insert(0, new ListItem("Select", "S"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account list by account id.
            var dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);
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
            //Get all project in an account  by account id and bind project drop down.
            ddlproject.DataSource = projectBusinessAccessObject.GetAccProject(Convert.ToInt32(ddlAccountCode.SelectedValue));
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
            //Get all project in an account by user account id and bind project drop down.
            ddlproject.DataSource = projectBusinessAccessObject.GetAccProject(Convert.ToInt32(identity.User.AccountID));
            ddlproject.DataValueField = "ProjectID";
            ddlproject.DataTextField = "Title";
            ddlproject.DataBind();
        }
    }

    /// <summary>
    ///    Enable or disable link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnEnable_Click(object sender, EventArgs e)
    {
        LinkButton enableDisableLink = sender as LinkButton;
        GridViewRow row = (GridViewRow)enableDisableLink.NamingContainer;
        var uniqueId = enableDisableLink.CommandArgument;
        EnableDisableLink(true, uniqueId);
        BindExternalLinkGrid(true);
        //updPanel.Update();
    }

    /// <summary>
    ///     lbtnEnable_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnDisable_Click(object sender, EventArgs e)
    {

        LinkButton enableDisableLink = sender as LinkButton;
        GridViewRow row = (GridViewRow)enableDisableLink.NamingContainer;
        var uniqueId = enableDisableLink.CommandArgument;
        EnableDisableLink(false, uniqueId);
        BindExternalLinkGrid(true);
        //updPanel.Update();
    }

    /// <summary>
    /// Enable disable links
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="uniqueId"></param>
    private void EnableDisableLink(bool flag, string uniqueId)
    {
        CNameValueList listcname = new CNameValueList();
        listcname.Add("@Operation", "UPDEXLINK");
        listcname.Add("@UniqueID", uniqueId);
        listcname.Add("@Status", flag);

        Common_BAO objCommonBusinessAccessObject = new Common_BAO();
        // update link status.
        objCommonBusinessAccessObject.InsertAndUpdate("Survey_UspExternalLink", listcname);

    }
    #endregion

    /// <summary>
    /// Bind company drop down
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_Company_BAO company_BAO = new Survey_Company_BAO();
        //Get company details
        var dataTableCompany = company_BAO.GetdtCompanyList(GetCompanyCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "S"));
        //Bind company dropdown
        ddlCompany.DataSource = dataTableCompany;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();
    }

    /// <summary>
    /// Generate Dynamic query.
    /// </summary>
    /// <returns></returns>
    public string GetCompanyCondition()
    {
        string stringQuery = "";

        //if (Convert.ToInt32(ViewState["AccountID"]) > 0)
        //    str = str + "" + ViewState["AccountID"] + " and ";
        //else
        //    str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (int.Parse(ddlAccountCode.SelectedValue) > 0)
            stringQuery = stringQuery + "" + ddlAccountCode.SelectedValue + " and ";

        if (ddlproject.SelectedIndex > 0)
            stringQuery = stringQuery + "Survey_Project.[ProjectID] = " + ddlproject.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }

    /// <summary>
    /// Bind grid view 
    /// </summary>
    /// <param name="IsBind"></param>
    /// <returns></returns>
    private int BindExternalLinkGrid(bool IsBind)
    {
        int count = 0;
        int accountId = 0;
        int companyId = 0;
        int projectId = 0;
        bool status;

        CNameValueList listcname = new CNameValueList();
        listcname.Add("@Operation", "GETEXLINK");

        if (Int32.TryParse(ddlAccountCode.SelectedValue, out accountId))//get account id
            listcname.Add("@AccountID", accountId);
        if (Int32.TryParse(ddlCompany.SelectedValue, out companyId))//get company id
            listcname.Add("@CompanyID", companyId);
        if (Int32.TryParse(ddlproject.SelectedValue, out projectId))//get project id
            listcname.Add("@ProjectID", projectId);
        if (bool.TryParse(ddlStatus.SelectedValue, out status))//get status id
            listcname.Add("@Status", status);

        Common_BAO objCommonBusinessAccessObject = new Common_BAO();
        //Get External link details.
        var dataTableExternalLink = objCommonBusinessAccessObject.GetDataTable("Survey_UspExternalLink", listcname);

        if (dataTableExternalLink != null && IsBind)
        {
            if (IsBind)
            {
                //Bind grid with links
                grdvExternalLink.DataSource = dataTableExternalLink;
                grdvExternalLink.DataBind();
            }
            count = dataTableExternalLink.Rows.Count;
        }
        return count;
    }
}
