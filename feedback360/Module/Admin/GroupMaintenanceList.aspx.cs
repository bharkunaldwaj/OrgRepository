﻿using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Administration_BE;
using Administration_BAO;

public partial class Module_Admin_GroupMaintenanceList : CodeBehindBase
{
    //Global variables
    Group_BAO groupBusinessAccessObject = new Group_BAO();
    Group_BE groupBusinessEntity = new Group_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int groupCount = 0;
    string pageNo = "";

    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        //     Label ll = (Label)this.Master.FindControl("Current_location");
        //      ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            //Reset Gridview object data source Parameter. 
            odsGroup.SelectParameters.Clear();
            odsGroup.SelectParameters.Add("accountID", identity.User.AccountID.ToString());
            odsGroup.Select();

            //Reset gridview pagesize.
            grdvGroup.PageSize = pageSize;
            //Manage gridview pagaing.
            ManagePaging();

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Add contorl attribute to every row of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            //Add client side event to delete button.
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[4].Controls[0];
                linkButtonDelete.OnClientClick = "if (!window.confirm('Are you sure you want to delete this group?')) return false";
            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect to Group maintance page when click on Add new.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("GroupMaintenance.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region Gridview Paging Related Methods
    /// <summary>
    /// Manage Pagaing to gridview on page index change.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        groupCount = groupBusinessAccessObject.GetGroupListCount(identity.User.AccountID.ToString());

        if (groupCount > 0)
        {
            // Variable declaration
            int numberOfPages;
            int numberOfRecords = groupCount;
            int currentPage = (grdvGroup.PageIndex);
            StringBuilder stringSummary = new StringBuilder();

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
            stringSummary.Append("Displaying <b>");

            // Creating X f X Records
            int floor = (currentPage * pageSize) + 1;
            stringSummary.Append(floor.ToString());
            stringSummary.Append("</b>-<b>");
            int ceil = ((currentPage * pageSize) + pageSize);

            //let say you have 26 records and you specified 10 page size, 
            // On the third page it will return 30 instead of 25 as that is based on pageSize
            // So this check will see if the ceil value is increasing the number of records. Consider numberOfRecords
            if (ceil > numberOfRecords)
            {
                stringSummary.Append(numberOfRecords.ToString());
            }
            else
            {
                stringSummary.Append(ceil.ToString());
            }

            // Displaying Total number of records Creating X of X of About X records.
            stringSummary.Append("</b> of <b>");
            stringSummary.Append(numberOfRecords.ToString());
            stringSummary.Append("</b> records</br>");

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
            objLbLast.ID = "lb_LastPage";
            objLbLast.ToolTip = "Last Page";
            objLbLast.CommandName = "pgChange";
            objLbLast.EnableViewState = true;
            objLbLast.CommandArgument = numberOfPages.ToString();

            // Next Link button to display with paging
            LinkButton objLbNext = new LinkButton();
            objLbNext.Click += new EventHandler(objLb_Click);
            //objLbNext.Text = "Next";
            objLbNext.CssClass = "next";
            objLbNext.ID = "lb_NextPage";
            objLbNext.ToolTip = "Next Page";
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
    /// Save the view state of the page.
    /// </summary>
    /// <returns></returns>
    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, groupCount };
    }

    /// <summary>
    /// Reload the viewsate when view sate of the page expires.
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
            //grdvGroup.DataSourceID = odsGroup.ID;
            //grdvGroup.DataBind();

            ManagePaging();
        }
    }

    /// <summary>
    /// Gridview Paging Next previous button event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();
        LinkButton linkButtonNext = (LinkButton)sender;

        //Reset Page index to new index.
        grdvGroup.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvGroup.DataBind();

        ManagePaging();

    }

    /// <summary>
    /// Handel record search in gridview by page number.
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

            //Reset Page index to new index.
            grdvGroup.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvGroup.DataBind();

            //Handel pageing in Gridview.
            ManagePaging();
            //set page number.
            textBoxGoto.Text = pageNo;
        }
    }
    #endregion
}
