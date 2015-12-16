using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Text;

public partial class Module_Questionnaire_ViewCandidateStatus : CodeBehindBase
{
    //Global variables.
    AssignQuestionnaire_BAO AssignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
    AssignQuestionnaire_BE AssignQuestionnaireBusinessEntity = new AssignQuestionnaire_BE();
    Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

    DataTable dataTableCompanyName;
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
        int? groupID = Identity.User.GroupID;

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
                //If user is super admin then gropu id=1 then show account detalils section else hide.
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

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get all account list in user accout.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
                string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();
                //If it a participant
                if (identity.User.GroupID == Convert.ToInt32(participantRoleId))
                {
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlParticipant.Visible = false;
                    lblParticipant.Visible = true;

                    lblParticipant.Text = identity.User.FName + " " + identity.User.LName;
                    lblProjectName.Text = "Participant";
                    lblParticipantHeading.Visible = false;

                    AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
                    DataTable dataTableParticipantDetails = new DataTable();
                    // Get all participant in this user.
                    dataTableParticipantDetails = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

                    if (dataTableParticipantDetails.Rows.Count > 0)
                    {
                        //Reset Candidate object datasource.
                        odsCandidateStatus.SelectParameters.Clear();
                        odsCandidateStatus.SelectParameters.Add("condition", Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(dataTableParticipantDetails.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dataTableParticipantDetails.Rows[0]["ProgrammeID"]));
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
                    //Get all Questionnaire List
                    DataTable dataTableUserList = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(identity.User.UserID.ToString()));
                    Project_BAO project_BAO = new Project_BAO();

                    if (dataTableUserList.Rows.Count > 0)
                    {
                        //int projectid = Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]);

                        ddlProject.Items.Clear();
                        //ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                        if (dataTableUserList.Rows.Count > 0)
                        {
                            //get all project and bind project drop down
                            DataTable project = project_BAO.GetdataProjectByID(Convert.ToInt32(dataTableUserList.Rows[0]["ProjectID"]));

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
                else if (identity.User.GroupID == Convert.ToInt32(managerRoleId))//If Manager
                {
                    Project_BAO projectBusinessAccessObject = new Project_BAO();

                    DataTable dataTableManagerProject = new DataTable();
                    //Get all project by user account id.
                    dataTableManagerProject = projectBusinessAccessObject.GetManagerProject(identity.User.Email, Convert.ToInt32(identity.User.AccountID));

                    if (dataTableManagerProject.Rows.Count > 0)
                    {
                        //Bind project drop down.
                        ddlProject.DataSource = dataTableManagerProject;
                        ddlProject.DataValueField = "ProjectID";
                        ddlProject.DataTextField = "Title";
                        ddlProject.DataBind();

                        string projectIds = "";
                        if (dataTableManagerProject.Rows.Count > 0)
                        {
                            for (int i = 0; i < dataTableManagerProject.Rows.Count; i++)
                            {
                                projectIds = projectIds + dataTableManagerProject.Rows[i]["ProjectID"].ToString() + ",";
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

                    Project_BAO projectBusinessAccess = new Project_BAO();
                    //Get all project list and Bind project drop down.
                    ddlProject.DataSource = projectBusinessAccess.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ddlParticipant.Visible = true;
                    lblParticipant.Visible = false;
                }
            }

            grdvCandidateStatus.PageSize = pageSize;
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

    #region Gridview Paging Related Methods
    /// <summary>
    /// Handle paging on page index changeing of gridview.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

        if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
        {
            AssignQuestionnaireCount = AssignQuestionnaireBusinessAccessObject.GetAssignQuestionnaireListCount(GetCondition());
        }
        else
        {
            string condition;
            AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
            DataTable dataTableParticipantInfo = new DataTable();
            dataTableParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));
            if (dataTableParticipantInfo.Rows.Count > 0)
            {
                condition = Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(dataTableParticipantInfo.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dataTableParticipantInfo.Rows[0]["ProgrammeID"]);
                AssignQuestionnaireCount = AssignQuestionnaireBusinessAccessObject.GetAssignQuestionnaireListCount(condition);
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

    /// <summary>
    /// Save the view state for the page.
    /// </summary>
    /// <returns></returns>
    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, AssignQuestionnaireCount };
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

            if (ddlProject.SelectedValue == "0" || ddlProgramme.SelectedValue == "0" || ddlParticipant.SelectedValue == "0")
            {
                ManagePaging();
            }
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

        grdvCandidateStatus.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvCandidateStatus.DataBind();

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

            grdvCandidateStatus.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvCandidateStatus.DataBind();
            ManagePaging();

            textBoxGoto.Text = pageNo;
        }
    }

    #endregion

    #region Search Related Function
    /// <summary>
    /// Bind candidate grid with selected value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        odsCandidateStatus.SelectParameters.Clear();
        //Reset candidate object data source with dynamic query.
        odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
        odsCandidateStatus.Select();
        //set page index to 0.
        grdvCandidateStatus.PageIndex = 0;
        grdvCandidateStatus.DataBind();
        //set page index.
        ManagePaging();

    }

    /// <summary>
    /// Reset controls value to default
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Manage paging when click on grid header for sorting
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Bind candidate submit status in %. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCandidateStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label labelCandidateId = (Label)e.Row.FindControl("lblCandidateID");
                Label labelQuestionnaireId = (Label)e.Row.FindControl("lblQuestionnaireID");
                Label labelCompletion = (Label)e.Row.FindControl("lblComplete");
                Label labelSubmitFlag = (Label)e.Row.FindControl("lblSubmitFlag");
                Label labelRelationship = (Label)e.Row.FindControl("lblRelationship");
                LinkButton SubmitFlag = (LinkButton)e.Row.FindControl("SubmitFlag");

                if (HttpContext.Current.Session["GroupID"] != null && HttpContext.Current.Session["GroupID"].ToString() == "1")
                    SubmitFlag.Enabled = true;
                else
                    SubmitFlag.Enabled = false;
                //LinkButton lbtnStatus=(LinkButton)e.Row.FindControl("lbtnStatus");

                //RadioButtonList rblstSubmitFlag = (RadioButtonList)e.Row.FindControl("rblstSubmitFlag");

                //HtmlTable tblGraph = (HtmlTable)e.Row.FindControl("tbGraph");

                Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
                //Get number of question answered
                int answeredQuestion = questionnaire_BAO.CalculateGraph(Convert.ToInt32(labelQuestionnaireId.Text), Convert.ToInt32(labelCandidateId.Text));

                DataTable dataTableQuestion = new DataTable();
                //dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaire(Convert.ToInt32(lblQuestionnaireId.Text));
                //Get total questions
                dataTableQuestion = questionnaire_BAO.GetFeedbackQuestionnaireByRelationShip(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue), Convert.ToInt32(labelQuestionnaireId.Text), labelRelationship.Text);
                //Calculate % of survey complition
                double percentage = (answeredQuestion * 100) / Convert.ToInt32(dataTableQuestion.Rows.Count);
                string[] percent = percentage.ToString().Split('.');

                //percentage = percent[0];
                labelCompletion.Text = percent[0].ToString() + "%";

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

    /// <summary>
    /// Bind project by when account value changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

            //Get company id by account code.
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //get company list
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //Set comapny name
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

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

            Project_BAO projectBusinessAccessObject = new Project_BAO();
            DataTable dataTableProject = new DataTable();
            //Get project list by company id tp bind project dropdown
            dataTableProject = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

            if (dataTableProject.Rows.Count > 0)
            {
                ddlProject.DataSource = dataTableProject;
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
        else//if account is set to "select" then clear controls.
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

    /// <summary>
    /// Bind program by project value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //get program list in a project and bind program
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dataTableProgramme;
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

    /// <summary>
    /// Its of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Bind Participant according to program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participantBusinessAccessObject = new AssignQstnParticipant_BAO();

        if (ddlProgramme.SelectedIndex > 0)
        {
            DataTable dataTableParticipant = new DataTable();

            //If user is super Admin then user account id else user user Account Id to get participant list.
            if (identity.User.GroupID == 1)
            {
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
            }
            else
            {
                dataTableParticipant = participantBusinessAccessObject.GetdtAssignPartiList(identity.User.AccountID.ToString(), ddlProgramme.SelectedValue);
            }

            Project_BAO project_BAO = new Project_BAO();

            if (dataTableParticipant.Rows.Count > 0)
            {
                ddlParticipant.Items.Clear();
                ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
                //Bind participant list.
                ddlParticipant.DataSource = dataTableParticipant;
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
        {// if account drop down is set to "select" then clear control
            ddlParticipant.Items.Clear();
            ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    /// <summary>
    /// Build dynamic query
    /// </summary>
    /// <returns></returns>
    protected string GetCondition()
    {
        string stringQuery = "";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0)
            stringQuery = stringQuery + "" + ViewState["AccountID"] + " and ";
        else
            stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

        //if ddlParticipant isvisible then set target person value by its value
        if (ddlParticipant.Visible == true)
        {
            if (ddlParticipant.SelectedIndex > 0)
                stringQuery = stringQuery + "[TargetPersonID] = " + ddlParticipant.SelectedValue + " and ";

            if (ddlProject.SelectedIndex > 0)
                stringQuery = stringQuery + "dbo.AssignQuestionnaire.ProjecctID = " + ddlProject.SelectedValue + " and ";

            if (ddlProgramme.SelectedIndex > 0)
                stringQuery = stringQuery + "dbo.Programme.ProgrammeID = " + ddlProgramme.SelectedValue + " and ";
        }
        else
        {
            AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
            DataTable dataTableParticipantDetails = new DataTable();

            dataTableParticipantDetails = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

            if (dataTableParticipantDetails.Rows.Count > 0)
            {
                stringQuery = stringQuery + "[TargetPersonID] = " + Convert.ToInt32(identity.User.UserID) + " and ";

                stringQuery = stringQuery + "dbo.AssignQuestionnaire.ProjecctID = " + Convert.ToInt32(dataTableParticipantDetails.Rows[0]["ProjecctID"]) + " and ";

                stringQuery = stringQuery + "dbo.Programme.ProgrammeID = " + Convert.ToInt32(dataTableParticipantDetails.Rows[0]["ProgrammeID"]) + " and ";
            }
        }

        //str = str + "dbo.AssignQuestionnaire.ProgrammeID = " + ddlProgramme.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }

    /// <summary>
    /// Change the submit status of candidates fro mno to yes and viceversa.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCandidateStatus_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string id = e.CommandName;
            string submitFlag = e.CommandArgument.ToString();
            int result = 0;

            if (submitFlag == "True")
                result = questionnaireBusinessAccessObject.UpdateSubmitFlag(Convert.ToInt32(id), 0);
            else
                result = questionnaireBusinessAccessObject.UpdateSubmitFlag(Convert.ToInt32(id), 1);

            //Reset object datasource parameter.
            odsCandidateStatus.SelectParameters.Clear();
            odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
            odsCandidateStatus.Select();
            //Bind the candidate grid.
            grdvCandidateStatus.PageIndex = 0;
            grdvCandidateStatus.DataBind();

            ManagePaging();

        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// It is of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

