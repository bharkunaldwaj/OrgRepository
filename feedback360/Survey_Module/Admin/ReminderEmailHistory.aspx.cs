﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

using Admin_BAO;

public partial class Survey_Module_Admin_ReminderEmailHistory : CodeBehindBase
{
    //Global variables
    Survey_ReminderEmailHistory_BAO reminderEmailHistoryBusinessAccessObject = new Survey_ReminderEmailHistory_BAO();
    //Survey_ReminderEmailHistory_BE reminderEmailHistory_BE = new Survey_ReminderEmailHistory_BE();
    WADIdentity identity;

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int emailHistoryCount = 0;
    string pageNo = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            //if (txtFromDate.Text != "")
            //    SetDTPicker(updPanel, "dtFromDate", "txtFromDate");

            //if (txtToDate.Text != "")
            //    SetDTPicker(updPanel, "dtToDate", "txtToDate");

            if (!IsPostBack)
            {
                ViewState["FromDate"] = "";
                ViewState["ToDate"] = "";

                //Set object datasource for reminder grid
                odsReminderMailHistory.SelectParameters.Clear();
                odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1));
                odsReminderMailHistory.Select();

                ManagePaging();
            }

            grdvReminderMailHistory.PageSize = pageSize;

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");

            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region "Gridview Related Methods"
    /// <summary>
    /// Manage Paging when gridview page index changes.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        emailHistoryCount = reminderEmailHistoryBusinessAccessObject.GetQuestionnaireListCount(GetSqlCondition(2));

        if (emailHistoryCount > 0)
        {

            plcPaging.Controls.Clear();

            // Variable declaration
            int numberOfPages;
            int numberOfRecords = emailHistoryCount;
            int currentPage = (grdvReminderMailHistory.PageIndex);
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
        else
        {
            plcPaging.Controls.Clear();
        }
    }

    /// <summary>
    /// Save the view state for the page.
    /// </summary>
    /// <returns></returns>
    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, emailHistoryCount };
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
            //grdvGroup.DataSourceID = odsGroup.ID;
            //grdvGroup.DataBind();

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
        LinkButton linkButtonNext = (LinkButton)sender;

        grdvReminderMailHistory.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvReminderMailHistory.DataBind();

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

            grdvReminderMailHistory.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvReminderMailHistory.DataBind();
            ManagePaging();

            textBoxGoto.Text = pageNo;
        }
    }

    #endregion

    #region Search Related Function
    /// <summary>
    /// Bind Reminder Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ViewState["FromDate"] = txtFromDate.Text;
            ViewState["ToDate"] = txtToDate.Text;

            odsReminderMailHistory.SelectParameters.Clear();
            odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1));
            odsReminderMailHistory.Select();

            grdvReminderMailHistory.PageIndex = 0;
            grdvReminderMailHistory.DataBind();

            ManagePaging();


            if (txtFromDate.Text != "")
                SetDTPicker1(updPanel, "dtFromDate", "txtFromDate");

            if (txtToDate.Text != "")
                SetDTPicker2(updPanel, "dtToDate", "txtToDate");
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Rebindd Controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";

        ViewState["FromDate"] = "";
        ViewState["ToDate"] = "";

        odsReminderMailHistory.SelectParameters.Clear();
        odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1));
        odsReminderMailHistory.Select();

        grdvReminderMailHistory.PageIndex = 0;
        grdvReminderMailHistory.DataBind();

        ManagePaging();
    }

    #endregion

    /// <summary>
    /// Set Grid Label Value
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvReminderMailHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label labelStatus = (Label)e.Row.FindControl("lblStatus");
            HiddenField hdnStatus = (HiddenField)e.Row.FindControl("hdnStatus");

            Label labelType = (Label)e.Row.FindControl("lblType");
            HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");

            if (hdnStatus != null)
            {
                if (hdnStatus.Value == "True")
                    labelStatus.Text = "Success";
                else
                    labelStatus.Text = "Failure";

                if (hdnType.Value == "1")
                    labelType.Text = "Candidate Reminder 1";
                else if (hdnType.Value == "2")
                    labelType.Text = "Candidate Reminder 2";
                else if (hdnType.Value == "3")
                    labelType.Text = "Candidate Reminder 3";
                else if (hdnType.Value == "4")
                    labelType.Text = "Report Available";
                else if (hdnType.Value == "5")
                    labelType.Text = "Participant Reminder 1";
                else if (hdnType.Value == "6")
                    labelType.Text = "Participant Reminder 2";
                else if (hdnType.Value == "7")
                    labelType.Text = "Participant Reminder 3";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Generate Dynamic Query
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public string GetSqlCondition(int flag)
    {
        string sqlData = "";

        if (flag == 1)
        {
            sqlData = "select * from Survey_ReminderEmailHistory where 1=1 ";

            if (ViewState["FromDate"].ToString() != "")
                sqlData = sqlData + "and (CONVERT(Date, EmailDate, 103) >= CONVERT(Date, '" + ViewState["FromDate"].ToString() + "', 103)) ";

            if (ViewState["ToDate"].ToString() != "")
                sqlData = sqlData + " and (CONVERT(Date, EmailDate, 103) <= CONVERT(Date, '" + ViewState["ToDate"].ToString() + "', 103))";

            if ((ViewState["FromDate"].ToString() == "") && (ViewState["ToDate"].ToString() == ""))
                sqlData = sqlData + " and (CONVERT(Date, EmailDate, 103) = CONVERT(Date, '" + DateTime.Today.ToString("dd/MM/yyyy") + "', 103))";

        }
        else
        {
            sqlData = "select Count(*) from Survey_ReminderEmailHistory where 1=1 ";

            if (ViewState["FromDate"].ToString() != "")
                sqlData = sqlData + "and (CONVERT(Date, EmailDate, 103) >= CONVERT(Date, '" + ViewState["FromDate"].ToString() + "', 103)) ";

            if (ViewState["ToDate"].ToString() != "")
                sqlData = sqlData + " and (CONVERT(Date, EmailDate, 103) <= CONVERT(Date, '" + ViewState["ToDate"].ToString() + "', 103))";

            if ((ViewState["FromDate"].ToString() == "") && (ViewState["ToDate"].ToString() == ""))
                sqlData = sqlData + " and (CONVERT(Date, EmailDate, 103) = CONVERT(Date, '" + DateTime.Today.ToString("dd/MM/yyyy") + "', 103))";

        }

        txtFromDate.Text = ViewState["FromDate"].ToString();
        dtFromDate.Text = ViewState["FromDate"].ToString();

        txtToDate.Text = ViewState["ToDate"].ToString();
        dtToDate.Text = ViewState["ToDate"].ToString();

        return sqlData;
    }

    /// <summary>
    /// Bind Calender 
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="HtmlDate"></param>
    /// <param name="aspDate"></param>
    private void SetDTPicker1(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test1", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
    }

    /// <summary>
    /// Bind Calender
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="HtmlDate"></param>
    /// <param name="aspDate"></param>
    private void SetDTPicker2(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test2", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
    }

    //public string GetSqlCondition(int flag)
    //{
    //    string sqlData = "";

    //    if (flag == 1)
    //    {
    //        sqlData = "select * from ReminderEmailHistory where 1=1 ";

    //        if (txtFromDate.Text != "")
    //            sqlData = sqlData + "and (EmailDate > CONVERT(DATETIME, '" + txtFromDate.Text + "', 102)) ";

    //        if (txtToDate.Text != "")
    //            sqlData = sqlData + " and (EmailDate < CONVERT(DATETIME, '" + txtToDate.Text + "', 102))";
    //    }
    //    else
    //    {
    //        sqlData = "select Count(*) from ReminderEmailHistory where 1=1 ";

    //        if (txtFromDate.Text != "")
    //            sqlData = sqlData + "and (EmailDate > CONVERT(DATETIME, '" + txtFromDate.Text + "', 102)) ";

    //        if (txtToDate.Text != "")
    //            sqlData = sqlData + " and (EmailDate < CONVERT(DATETIME, '" + txtToDate.Text + "', 102))";
    //    }

    //    return sqlData;
    //}
}
