using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Questionnaire_BAO;
using Admin_BAO;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class Survey_Module_Questionnaire_ViewCandidateStatus : CodeBehindBase
{
    //Global variables.
    Survey_AssignQuestionnaire_BAO AssignQuestionnaireBusinessAccessObject = new Survey_AssignQuestionnaire_BAO();
    //Survey_AssignQuestionnaire_BE AssignQuestionnaire_BE = new Survey_AssignQuestionnaire_BE();
    Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();

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

        try
        {
            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

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
                    lblParticipant.Text = identity.User.FName + " " + identity.User.LName;
                    lblProjectName.Text = "Participant";

                    Survey_AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new Survey_AssignQuestionnaire_BAO();
                    DataTable dataTableParticipantInformation = new DataTable();
                    // Get all participant in this user.
                    dataTableParticipantInformation = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));
                    //Reset Candidate object datasource.
                    odsCandidateStatus.SelectParameters.Clear();
                    odsCandidateStatus.SelectParameters.Add("condition", Convert.ToInt32(identity.User.AccountID) + " and Survey_Project.[ProjectID]=" + Convert.ToInt32(dataTableParticipantInformation.Rows[0]["ProjecctID"]) + " and [Survey_Analysis_Sheet].ProgrammeID=" + Convert.ToInt32(dataTableParticipantInformation.Rows[0]["ProgrammeID"]));
                    odsCandidateStatus.Select();

                    ddlProject.Visible = false;
                    ddlProgramme.Visible = false;

                    lblProjectName.Visible = true;
                    lblProgrammeName.Visible = false;

                    imbReset.Visible = false;
                    imbSubmit.Visible = false;

                    Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
                    //Get all Questionnaire List
                    DataTable dataTableUserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32(identity.User.UserID.ToString()));
                    Survey_Project_BAO project_BAO = new Survey_Project_BAO();

                    if (dataTableUserlist.Rows.Count > 0)
                    {
                        //int projectid = Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]);

                        ddlProject.Items.Clear();
                        ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                        //get all project and bind project drop down
                        DataTable project = project_BAO.GetdataProjectByID(Convert.ToInt32(dataTableUserlist.Rows[0]["ProjectID"]));
                        ddlProject.DataSource = project;
                        ddlProject.DataTextField = "Title";
                        ddlProject.DataValueField = "ProjectID";
                        ddlProject.DataBind();
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
                    Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();

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

                        for (int i = 0; i < dataTableManagerProject.Rows.Count; i++)
                        {
                            projectIds = projectIds + dataTableManagerProject.Rows[i]["ProjectID"].ToString() + ",";
                        }

                        odsCandidateStatus.SelectParameters.Clear();
                        odsCandidateStatus.SelectParameters.Add("condition", Convert.ToInt32(identity.User.AccountID) + " and Project.[ProjectID] in (" + projectIds.TrimEnd(',') + ")");
                        odsCandidateStatus.Select();
                    }
                }
                else
                {
                    odsCandidateStatus.SelectParameters.Clear();
                    odsCandidateStatus.SelectParameters.Add("condition", identity.User.AccountID.ToString());
                    odsCandidateStatus.Select();

                    Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
                    //Get all project list and Bind project drop down.
                    ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    lblParticipant.Visible = false;
                }
            }

            grdvCandidateStatus.PageSize = pageSize;
            ManagePaging();

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");

            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;
        }
        catch (Exception ex)
        {
            //HandleException(ex);
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
            Survey_AssignQuestionnaire_BAO assignQuestionnaire_BAO = new Survey_AssignQuestionnaire_BAO();
            DataTable dtParticipantInfo = new DataTable();
            dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

            string condition = Convert.ToInt32(identity.User.AccountID) + " and Project.[ProjectID]=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProgrammeID"]);

            AssignQuestionnaireCount = AssignQuestionnaireBusinessAccessObject.GetAssignQuestionnaireListCount(condition);
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

            objLbFirst.CssClass = "first";
            objLbFirst.ToolTip = "First Page";
            objLbFirst.ID = "lb_FirstPage";
            objLbFirst.CommandName = "pgChange";
            objLbFirst.EnableViewState = true;
            objLbFirst.CommandArgument = "1";

            //Previous Link button to display with paging
            LinkButton objLbPrevious = new LinkButton();
            objLbPrevious.Click += new EventHandler(objLb_Click);

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

            plcPaging.Controls.Add(objLbPrevious);



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

                }
            }

            // Last Link button to display with paging
            LinkButton objLbLast = new LinkButton();
            objLbLast.Click += new EventHandler(objLb_Click);

            objLbLast.CssClass = "last";
            objLbLast.ToolTip = "Last Page";
            objLbLast.ID = "lb_LastPage";
            objLbLast.CommandName = "pgChange";
            objLbLast.EnableViewState = true;
            objLbLast.CommandArgument = numberOfPages.ToString();

            // Next Link button to display with paging
            LinkButton objLbNext = new LinkButton();
            objLbNext.Click += new EventHandler(objLb_Click);

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

            plcPaging.Controls.Add(objLbLast);


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
            if (ddlProject.SelectedValue == "0" || ddlProgramme.SelectedValue == "0")
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

    }

    /// <summary>
    /// Reset contrls value to default
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ddlAccountCode.SelectedIndex = 0;
        ddlAccountCode_SelectedIndexChanged(sender, e);

        ddlProject.SelectedValue = "0";
        ddlProgramme.SelectedValue = "0";

        odsCandidateStatus.SelectParameters.Clear();
        odsCandidateStatus.SelectParameters.Add("condition", "0");
        odsCandidateStatus.Select();

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
            grdvCandidateStatus.PageIndex = 0;
            grdvCandidateStatus.DataBind();

            ManagePaging();
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label labelCandidateId = (Label)e.Row.FindControl("lblCandidateID");
                Label labelQuestionnaireId = (Label)e.Row.FindControl("lblQuestionnaireID");
                Label labelCompletion = (Label)e.Row.FindControl("lblComplete");
                Label labelSubmitFlag = (Label)e.Row.FindControl("lblSubmitFlag");

                Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
                //Get total questions answered
                int answeredQuestion = questionnaireBusinessAccessObject.CalculateGraph(Convert.ToInt32(labelQuestionnaireId.Text), Convert.ToInt32(labelCandidateId.Text));

                DataTable dtQuestion = new DataTable();
                //Get total questions
                dtQuestion = questionnaireBusinessAccessObject.GetFeedbackQuestionnaire(Convert.ToInt32(labelQuestionnaireId.Text));
                //Calculate % of survey complition
                double percentage = (answeredQuestion * 100) / Convert.ToInt32(dtQuestion.Rows.Count);
                string[] percent = percentage.ToString().Split('.');

                labelCompletion.Text = percent[0].ToString() + "%";
            }
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

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
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
        }
        else
        {
            lblcompanyname.Text = "";
            //if account is set to "select" then clear controls.
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
        Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //get program list in a project and bind program
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), 0, 0);

        if (dataTableProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
    }

    /// <summary>
    /// Generate dynamic query
    /// </summary>
    /// <returns></returns>
    protected string GetCondition()
    {
        string stringQuery = "";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0)
            stringQuery = stringQuery + "" + ViewState["AccountID"] + " and ";
        else
            stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlProject.SelectedIndex > 0)
            stringQuery = stringQuery + "dbo.Survey_AssignQuestionnaire.ProjecctID = " + ddlProject.SelectedValue + " and ";

        if (ddlProgramme.SelectedIndex > 0)
            stringQuery = stringQuery + "dbo.Survey_AssignQuestionnaire.ProgrammeID = " + ddlProgramme.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        if (ddlProgramme.SelectedIndex > 0)
            param = param + "and dbo.Survey_Analysis_Sheet.ProgrammeID=" + ddlProgramme.SelectedValue;

        return param;
    }

    /// <summary>
    /// Change the submit status of candidates from no to yes and viceversa.
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
            //Reset paging.
            ManagePaging();
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// Its of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objrblstSubmitFlag_Click(object sender, EventArgs e)
    {
        LinkButton objrblstSubmitFlag = (LinkButton)sender;
        string str = objrblstSubmitFlag.ID;
    }

    /// <summary>
    /// Export participant list ot excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Export_ParticepantList_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dataTableParticepantList = AssignQuestionnaireBusinessAccessObject.GetdtAssignListNewSurvey(GetCondition());

        grdvCandidateStatus.PageSize = dataTableParticepantList.Rows.Count;

        odsCandidateStatus.SelectParameters.Clear();
        //Reset object data source
        odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
        odsCandidateStatus.Select();
        //Reset paga indexand bind grid.
        grdvCandidateStatus.PageIndex = 0;
        grdvCandidateStatus.DataBind();

        var rows = grdvCandidateStatus.Rows;

        foreach (GridViewRow row in rows)
        {
            row.Cells[0].Font.Bold = true;
        }

        GridViewRow gridControl = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        TableCell tbCell = new TableCell();
        tbCell.ColumnSpan = grdvCandidateStatus.Columns.Count;  // get_ParticepantList.Columns.Count;

        tbCell.Attributes.Add("style", "text-align:center");
        gridControl.Cells.Add(tbCell);
        grdvCandidateStatus.Controls[0].Controls.AddAt(0, gridControl);

        Response.Clear();
        string style = @"<style> .text { mso-number-format:\.@;text-align:center;width:100px; } </style> ";

        foreach (GridViewRow row in grdvCandidateStatus.Rows)
        {
            // add numeric style for each cell
            foreach (TableCell cell in row.Cells)
            {
                cell.Attributes.Add("class", "text");
            }
        }
        //Set the content type and export.

        Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
        Response.AppendHeader("Content-disposition", "attachment; filename=" + ddlAccountCode.SelectedValue + "aa.xls");
        Response.Charset = "";
        Response.ContentEncoding = Encoding.ASCII;
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        HttpContext.Current.Response.Write(style);

        HtmlForm form = new HtmlForm();
        this.grdvCandidateStatus.Parent.Controls.Add(form);
        form.Attributes["runat"] = "server";
        form.Controls.Add(this.grdvCandidateStatus);
        form.RenderControl(htmlWrite);

        Response.Write(stringWrite.ToString());
        Response.End();
    }
}
