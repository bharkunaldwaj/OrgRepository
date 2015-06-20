﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;

using Admin_BAO;
using Admin_BE;
using Administration_BAO;

public partial class Module_Admin_ReminderEmailHistory : CodeBehindBase
{
    ReminderEmailHistory_BAO reminderEmailHistory_BAO = new ReminderEmailHistory_BAO();
    ReminderEmailHistory_BE reminderEmailHistory_BE = new ReminderEmailHistory_BE();
    WADIdentity identity;

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int emailHistoryCount = 0;
    string pageNo = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //if (txtFromDate.Text != "")
            //    SetDTPicker(updPanel, "dtFromDate", "txtFromDate");

            //if (txtToDate.Text != "")
            //    SetDTPicker(updPanel, "dtToDate", "txtToDate");

            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                ViewState["FromDate"] = "";
                ViewState["ToDate"] = "";

                odsReminderMailHistory.SelectParameters.Clear();

                if (identity.User.GroupID == 1)
                {
                    odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1, 1));
                }
                else
                {
                    odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1, Convert.ToInt32(identity.User.AccountID)));
                }

                odsReminderMailHistory.Select();

                ManagePaging();
            }
            grdvReminderMailHistory.PageSize = pageSize;

            TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (txtGoto != null)
                txtGoto.Text = pageNo;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region "Gridview Related Methods"

    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        if (identity.User.GroupID == 1)
        {
            emailHistoryCount = reminderEmailHistory_BAO.GetQuestionnaireListCount(GetSqlCondition(2, 1));
        }
        else
        {
            emailHistoryCount = reminderEmailHistory_BAO.GetQuestionnaireListCount(GetSqlCondition(2, Convert.ToInt32(identity.User.AccountID)));
        }

        //emailHistoryCount = reminderEmailHistory_BAO.GetQuestionnaireListCount(GetSqlCondition(2));

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

    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, emailHistoryCount };
    }

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

    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();
        LinkButton objlb = (LinkButton)sender;

        grdvReminderMailHistory.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
        grdvReminderMailHistory.DataBind();

        ManagePaging();

    }

    protected void objIbtnGo_Click(object sender, ImageClickEventArgs e)
    {
        TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
        if (txtGoto.Text.Trim() != "")
        {

            pageNo = txtGoto.Text;
            plcPaging.Controls.Clear();

            grdvReminderMailHistory.PageIndex = Convert.ToInt32(txtGoto.Text.Trim()) - 1;
            grdvReminderMailHistory.DataBind();
            ManagePaging();

            txtGoto.Text = pageNo;
        }
    }

    #endregion

    #region Search Related Function

    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ViewState["FromDate"] = txtFromDate.Text;
            ViewState["ToDate"] = txtToDate.Text;

            odsReminderMailHistory.SelectParameters.Clear();

            if (identity.User.GroupID == 1)
            {
                odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1, 1));
            }
            else
            {
                odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1, Convert.ToInt32(identity.User.AccountID)));
            }

            //odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1));
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

    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";

        ViewState["FromDate"] = "";
        ViewState["ToDate"] = "";

        odsReminderMailHistory.SelectParameters.Clear();

        if (identity.User.GroupID == 1)
        {
            odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1, 1));
        }
        else
        {
            odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1, Convert.ToInt32(identity.User.AccountID)));
        }

        //odsReminderMailHistory.SelectParameters.Add("sql", GetSqlCondition(1));
        odsReminderMailHistory.Select();

        grdvReminderMailHistory.PageIndex = 0;
        grdvReminderMailHistory.DataBind();

        ManagePaging();
    }

    #endregion

    protected void grdvReminderMailHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            HiddenField hdnStatus = (HiddenField)e.Row.FindControl("hdnStatus");

            Label lblType = (Label)e.Row.FindControl("lblType");
            HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");

            if (hdnStatus != null)
            {
                if (hdnStatus.Value == "True")
                    lblStatus.Text = "Success";
                else
                    lblStatus.Text = "Failure";

                if (hdnType.Value == "1")
                    lblType.Text = "Candidate Reminder 1";
                else if (hdnType.Value == "2")
                    lblType.Text = "Candidate Reminder 2";
                else if (hdnType.Value == "3")
                    lblType.Text = "Candidate Reminder 3";
                else if (hdnType.Value == "4")
                    lblType.Text = "Report Available";
                else if (hdnType.Value == "5")
                    lblType.Text = "Participant Reminder 1";
                else if (hdnType.Value == "6")
                    lblType.Text = "Participant Reminder 2";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    public string GetSqlCondition(int flag, int accountID)
    {
        string sqlData="";

        if (flag == 1)
        {
            sqlData = "select * from ReminderEmailHistory where 1=1 ";

            if (accountID != 1)
            {
                sqlData = sqlData + " and AccountId = " + accountID;
            }

            if (ViewState["FromDate"].ToString() != "")
                sqlData = sqlData + " and (EmailDate >= CONVERT(datetime, '" + ViewState["FromDate"].ToString() + "', 103)) ";

            if (ViewState["ToDate"].ToString() != "")
                sqlData = sqlData + " and (EmailDate <= CONVERT(datetime, '" + ViewState["ToDate"].ToString() + "', 103))";

            if ((ViewState["FromDate"].ToString() == "") && (ViewState["ToDate"].ToString() == ""))
                sqlData = sqlData + " and (EmailDate = CONVERT(datetime, '" + DateTime.Today.ToString("dd/MM/yyyy") + "', 103))";

        }
        else
        {
            sqlData = "select Count(*) from ReminderEmailHistory where 1=1 ";

            if (accountID != 1)
            {
                sqlData = sqlData + " and AccountId = " + accountID;
            }

            if (ViewState["FromDate"].ToString() != "")
                sqlData = sqlData + " and (EmailDate >= CONVERT(datetime, '" + ViewState["FromDate"].ToString() + "', 103)) ";

            if (ViewState["ToDate"].ToString() != "")
                sqlData = sqlData + " and (EmailDate <= CONVERT(datetime, '" + ViewState["ToDate"].ToString() + "', 103))";

            if ((ViewState["FromDate"].ToString() == "") && (ViewState["ToDate"].ToString() == ""))
                sqlData = sqlData + " and (EmailDate = CONVERT(datetime, '" + DateTime.Today.ToString("dd/MM/yyyy") + "', 103))";

        }

        txtFromDate.Text = ViewState["FromDate"].ToString();
        dtFromDate.Text = ViewState["FromDate"].ToString();

        txtToDate.Text = ViewState["ToDate"].ToString();
        dtToDate.Text = ViewState["ToDate"].ToString();

        return sqlData;
    }

    private void SetDTPicker1(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test1", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
    }

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
