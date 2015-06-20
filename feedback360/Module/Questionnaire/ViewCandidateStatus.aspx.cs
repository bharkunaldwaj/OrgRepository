using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class Module_Questionnaire_ViewCandidateStatus : CodeBehindBase
{
    AssignQuestionnaire_BAO AssignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
    AssignQuestionnaire_BE AssignQuestionnaire_BE = new AssignQuestionnaire_BE();
    Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

    DataTable dtCompanyName;
    WADIdentity identity;
    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);
    string pageNo = "";
    int AssignQuestionnaireCount = 0;
    string projectid;
    string userid;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        identity = this.Page.User.Identity as WADIdentity;
        int? grpID = Identity.User.GroupID;

        //AssignQuestionnaire_BAO survey_chk_user = new AssignQuestionnaire_BAO();
        //DataTable ddd = survey_chk_user.chk_user_authority(grpID, 25);
        //if (Convert.ToInt32(ddd.Rows[0][0]) == 0)
        //{
        //    Response.Redirect("UnAuthorized.aspx");
        //}





        try
        {
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

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

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
                string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();

                if (identity.User.GroupID == Convert.ToInt32(participantRoleId))
                {
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlParticipant.Visible = false;
                    lblParticipant.Visible = true;

                    lblParticipant.Text = identity.User.FName + " " + identity.User.LName;
                    lblProjectName.Text = "Participant";
                    lblParticipantHeading.Visible = false;

                    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                    DataTable dtParticipantInfo = new DataTable();
                    dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));
                    if (dtParticipantInfo.Rows.Count > 0)
                    {
                        odsCandidateStatus.SelectParameters.Clear();
                        odsCandidateStatus.SelectParameters.Add("condition", Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProgrammeID"]));
                        odsCandidateStatus.Select();
                    }
                    else
                    {
                        odsCandidateStatus.SelectParameters.Clear();
                        odsCandidateStatus.SelectParameters.Add("condition", Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(0) + " and [Programme].ProgrammeID=" + Convert.ToInt32(0));
                        odsCandidateStatus.Select();
                    }

                    ddlProject.Visible = false;
                    ddlProgramme.Visible = false;

                    lblProjectName.Visible = true;
                    lblProgrammeName.Visible = false;

                    imbReset.Visible = false;
                    imbSubmit.Visible = false;

                    AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();

                    DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(identity.User.UserID.ToString()));
                    Project_BAO project_BAO = new Project_BAO();

                    if (dtuserlist.Rows.Count > 0)
                    {
                        //int projectid = Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]);

                        ddlProject.Items.Clear();
                        //ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                        if (dtuserlist.Rows.Count > 0)
                        {
                            DataTable project = project_BAO.GetdataProjectByID(Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]));

                            ddlProject.DataSource = project;
                            ddlProject.DataTextField = "Title";
                            ddlProject.DataValueField = "ProjectID";
                            ddlProject.DataBind();
                        }
                    }
                    else
                    {
                        ddlProject.Items.Clear();
                        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

                        ddlProgramme.Items.Clear();
                        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
                else if (identity.User.GroupID == Convert.ToInt32(managerRoleId))
                {
                    Project_BAO project_BAO = new Project_BAO();

                    DataTable dtManagerProject = new DataTable();
                    dtManagerProject = project_BAO.GetManagerProject(identity.User.Email, Convert.ToInt32(identity.User.AccountID));

                    if (dtManagerProject.Rows.Count > 0)
                    {
                        ddlProject.DataSource = dtManagerProject;
                        ddlProject.DataValueField = "ProjectID";
                        ddlProject.DataTextField = "Title";
                        ddlProject.DataBind();

                        string projectIds = "";
                        if (dtManagerProject.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtManagerProject.Rows.Count; i++)
                            {
                                projectIds = projectIds + dtManagerProject.Rows[i]["ProjectID"].ToString() + ",";
                            }

                            odsCandidateStatus.SelectParameters.Clear();
                            odsCandidateStatus.SelectParameters.Add("condition", Convert.ToInt32(identity.User.AccountID) + " and Project.[ProjectID] in (" + projectIds.TrimEnd(',') + ")");
                            odsCandidateStatus.Select();
                        }
                    }
                }
                else 
                {
                    //AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                    //ddlParticipant.DataSource = accountUser_BAO.GetParticipantList(Convert.ToString(identity.User.AccountID));
                    //ddlParticipant.DataValueField = "UserID";
                    //ddlParticipant.DataTextField = "UserName";
                    //ddlParticipant.DataBind();

                    odsCandidateStatus.SelectParameters.Clear();
                    odsCandidateStatus.SelectParameters.Add("condition", identity.User.AccountID.ToString());
                    odsCandidateStatus.Select();

                    Project_BAO project_BAO = new Project_BAO();
                    ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ddlParticipant.Visible = true;
                    lblParticipant.Visible = false;
                }

            }

            grdvCandidateStatus.PageSize = pageSize;
            ManagePaging();

            TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (txtGoto != null)
                txtGoto.Text = pageNo;

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

    }

    #region Gridview Paging Related Methods

    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

        if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
        {
            AssignQuestionnaireCount = AssignQuestionnaire_BAO.GetAssignQuestionnaireListCount(GetCondition());
        }
        else
        {
            string condition;
            AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
            DataTable dtParticipantInfo = new DataTable();
            dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));
            if (dtParticipantInfo.Rows.Count > 0)
            {
                condition = Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProgrammeID"]);
                AssignQuestionnaireCount = AssignQuestionnaire_BAO.GetAssignQuestionnaireListCount(condition);
            }
        }

        plcPaging.Controls.Clear();

        if (AssignQuestionnaireCount > 0)
        {
            // Variable declaration
            int numberOfPages;
            int numberOfRecords = AssignQuestionnaireCount;
            int currentPage = (grdvCandidateStatus.PageIndex);
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

    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, AssignQuestionnaireCount };
    }

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

            if (ddlProject.SelectedValue == "0" || ddlProgramme.SelectedValue == "0" || ddlParticipant.SelectedValue == "0")
            {
                ManagePaging();
            } 
        }

    }

    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();
        LinkButton objlb = (LinkButton)sender;

        grdvCandidateStatus.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
        grdvCandidateStatus.DataBind();

        ManagePaging();

    }

    protected void objIbtnGo_Click(object sender, ImageClickEventArgs e)
    {
        TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
        if (txtGoto.Text.Trim() != "")
        {
            pageNo = txtGoto.Text;
            plcPaging.Controls.Clear();

            grdvCandidateStatus.PageIndex = Convert.ToInt32(txtGoto.Text.Trim()) - 1;
            grdvCandidateStatus.DataBind();
            ManagePaging();

            txtGoto.Text = pageNo;
        }
    }

    #endregion

    #region Search Related Function

    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        odsCandidateStatus.SelectParameters.Clear();
        odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
        odsCandidateStatus.Select();

        grdvCandidateStatus.PageIndex = 0;
        grdvCandidateStatus.DataBind();

        ManagePaging();

    }

    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {

        ddlAccountCode.SelectedIndex = 0;
        ddlAccountCode_SelectedIndexChanged(sender, e);

        ddlProject.SelectedValue = "0";
        ddlProgramme.SelectedValue = "0";
        ddlParticipant.SelectedValue = "0";

        odsCandidateStatus.FilterExpression = null;
        odsCandidateStatus.FilterParameters.Clear();

        ManagePaging();
    }

    #endregion

    protected void grdvCandidateStatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            grdvCandidateStatus.PageIndex = 0;
            grdvCandidateStatus.DataBind();

            ManagePaging();

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void grdvCandidateStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCandidateId = (Label)e.Row.FindControl("lblCandidateID");
                Label lblQuestionnaireId = (Label)e.Row.FindControl("lblQuestionnaireID");
                Label lblCompletion = (Label)e.Row.FindControl("lblComplete");
                Label lblSubmitFlag = (Label)e.Row.FindControl("lblSubmitFlag");
                Label lblRelationship = (Label)e.Row.FindControl("lblRelationship");
                LinkButton SubmitFlag = (LinkButton)e.Row.FindControl("SubmitFlag");

                if (HttpContext.Current.Session["GroupID"] != null && HttpContext.Current.Session["GroupID"].ToString() == "1")
                    SubmitFlag.Enabled = true;
                else
                    SubmitFlag.Enabled = false;
                //LinkButton lbtnStatus=(LinkButton)e.Row.FindControl("lbtnStatus");

                //RadioButtonList rblstSubmitFlag = (RadioButtonList)e.Row.FindControl("rblstSubmitFlag");

                //HtmlTable tblGraph = (HtmlTable)e.Row.FindControl("tbGraph");

                Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO(); 
                int answeredQuestion = questionnaire_BAO.CalculateGraph(Convert.ToInt32(lblQuestionnaireId.Text), Convert.ToInt32(lblCandidateId.Text));

                DataTable dtQuestion = new DataTable();
                //dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaire(Convert.ToInt32(lblQuestionnaireId.Text));
                dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaireByRelationShip(Convert.ToInt32(ddlAccountCode.SelectedValue),Convert.ToInt32(ddlProject.SelectedValue), Convert.ToInt32(lblQuestionnaireId.Text),lblRelationship.Text);

                double percentage = (answeredQuestion * 100) / Convert.ToInt32(dtQuestion.Rows.Count);
                string[] percent = percentage.ToString().Split('.');

                //percentage = percent[0];
                lblCompletion.Text = percent[0].ToString() + "%";

                //if (lblSubmitFlag.Text == "True")
                //    //rblstSubmitFlag.Items[0].Selected = true;
                //    lbtnStatus.Text="Yes";
                //else 
                //    //rblstSubmitFlag.Items[1].Selected = true;
                //    lbtnStatus.Text="No";

                //lbtnStatus.Click += new EventHandler(objrblstSubmitFlag_Click);
                //tblGraph.Width = percent[0].ToString() + "%";
            }

            //HandleWriteLog("Start", new StackTrace(true));
                }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            //string str = "";
            //if (ddlAccountCode.SelectedIndex > 0)
            //    str = str + "[AccountID] = " + ddlAccountCode.SelectedValue + " and ";

            //if (str.Trim().Length != 0)
            //{
            //    string param = str.Substring(0, str.Length - 4);
            //    odsCandidateStatus.FilterExpression = param;
            //    odsCandidateStatus.FilterParameters.Clear();
            //}
            //else
            //{
            //    odsCandidateStatus.FilterExpression = null;
            //    odsCandidateStatus.FilterParameters.Clear();
            //}

            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO account_BAO = new Account_BAO();
            dtCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dtAccount = dtCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            ddlParticipant.Items.Clear();
            ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));

            //AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
            //DataTable dtParticipant = new DataTable();
            //dtParticipant = accountUser_BAO.GetParticipantList(Convert.ToString(companycode));

            //if (dtParticipant.Rows.Count > 0)
            //{
            //    ddlParticipant.DataSource = dtParticipant;
            //    ddlParticipant.DataValueField = "UserID";
            //    ddlParticipant.DataTextField = "UserName";
            //    ddlParticipant.DataBind();
            //}

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Project_BAO project_BAO = new Project_BAO();
            DataTable dtProject = new DataTable();
            dtProject = project_BAO.GetdtProjectList(Convert.ToString(companycode));

            if (dtProject.Rows.Count > 0)
            {
                ddlProject.DataSource = dtProject;
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();
            }

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;

            //Bind Data
            /*odsCandidateStatus.SelectParameters.Clear();
            odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
            odsCandidateStatus.Select();

            grdvCandidateStatus.PageIndex = 0;
            grdvCandidateStatus.DataBind();

            ManagePaging();*/
        }
        else
        {
            lblcompanyname.Text = "";

            ddlParticipant.Items.Clear();
            ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
            
            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ViewState["AccountID"] = "0";
        }
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        //if (ddlProgramme.Items.Count > 1)
            //ddlProgramme.Items[1].Selected = true;

        ddlParticipant.Items.Clear();
        ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
    }

    protected void ddlParticipant_SelectedIndexChanged(object sender, EventArgs e)
    {
        //AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();

        //if (ddlParticipant.SelectedIndex > 0)
        //{
        //    odsCandidateStatus.SelectParameters.Clear();
        //    odsCandidateStatus.SelectParameters.Add("UserID", ddlParticipant.SelectedValue);
        //    odsCandidateStatus.SelectParameters.Add("ProjectID", ddlProject.SelectedValue);
        //    odsCandidateStatus.Select();

        //    ManagePaging();
        //}
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participant_BAO = new AssignQstnParticipant_BAO();

        if (ddlProgramme.SelectedIndex > 0)
        {
            DataTable dtParticipant = new DataTable();

            if (identity.User.GroupID == 1)
            {
                dtParticipant = participant_BAO.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
            }
            else
            {
                dtParticipant = participant_BAO.GetdtAssignPartiList(identity.User.AccountID.ToString(), ddlProgramme.SelectedValue);
            }

            Project_BAO project_BAO = new Project_BAO();

            if (dtParticipant.Rows.Count > 0)
            {
                ddlParticipant.Items.Clear();
                ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));

                ddlParticipant.DataSource = dtParticipant;
                ddlParticipant.DataTextField = "UserName";
                ddlParticipant.DataValueField = "UserID";
                ddlParticipant.DataBind();
            }
            else
            {
                ddlParticipant.Items.Clear();
                ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        else
        {
            ddlParticipant.Items.Clear();
            ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected string GetCondition()
    {
        string str = "";
        
        if (Convert.ToInt32(ViewState["AccountID"]) > 0)
            str = str + "" + ViewState["AccountID"] + " and ";
        else
            str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlParticipant.Visible == true)
        {
            if (ddlParticipant.SelectedIndex > 0)
                str = str + "[TargetPersonID] = " + ddlParticipant.SelectedValue + " and ";

            if (ddlProject.SelectedIndex > 0)
                str = str + "dbo.AssignQuestionnaire.ProjecctID = " + ddlProject.SelectedValue + " and ";

            if (ddlProgramme.SelectedIndex > 0)
                str = str + "dbo.Programme.ProgrammeID = " + ddlProgramme.SelectedValue + " and ";
        }
        else
        {
            AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
            DataTable dtParticipantInfo = new DataTable();
            dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));
            if (dtParticipantInfo.Rows.Count > 0)
            {
                str = str + "[TargetPersonID] = " + Convert.ToInt32(identity.User.UserID) + " and ";

                str = str + "dbo.AssignQuestionnaire.ProjecctID = " + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProjecctID"]) + " and ";

                str = str + "dbo.Programme.ProgrammeID = " + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProgrammeID"]) + " and ";
            }
        }

        //str = str + "dbo.AssignQuestionnaire.ProgrammeID = " + ddlProgramme.SelectedValue + " and ";
        
        string param = str.Substring(0, str.Length - 4);

        return param;
    }

    protected void grdvCandidateStatus_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string id = e.CommandName;
            string submitFlag = e.CommandArgument.ToString();
            int result=0;
            if (submitFlag == "True")
                result = questionnaire_BAO.UpdateSubmitFlag(Convert.ToInt32(id), 0);
            else
                result = questionnaire_BAO.UpdateSubmitFlag(Convert.ToInt32(id), 1);

            odsCandidateStatus.SelectParameters.Clear();
            odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
            odsCandidateStatus.Select();

            grdvCandidateStatus.PageIndex = 0;
            grdvCandidateStatus.DataBind();

            ManagePaging();
            
        }
        catch(Exception ex)
        {
            
        }

        
    }

    protected void objrblstSubmitFlag_Click(object sender, EventArgs e)
    {

        LinkButton objrblstSubmitFlag = (LinkButton)sender;
        string str = objrblstSubmitFlag.ID;

        //if (objrblstSubmitFlag.Items[0].Selected == true)
        //{
        //}
        //else
        //{ 
        //}

    }

}

