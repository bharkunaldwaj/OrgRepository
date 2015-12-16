using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BE;
using Admin_BAO;
using System.Text;
using Miscellaneous;

public partial class Module_Admin_EmailCandidate : CodeBehindBase
{
    #region Global Refrence & Variable

    AssignQuestionnaire_BAO AssignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
    AssignQuestionnaire_BE AssignQuestionnaire_BE = new AssignQuestionnaire_BE();
    Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
    EmailTemplate_BAO emailtemplate_BAO = new EmailTemplate_BAO();
    EmailTemplate_BE emailtemplate_BE = new EmailTemplate_BE();
    List<EmailTemplate_BE> emailtemplateList = new List<EmailTemplate_BE>();
    AssignQuestionnaire_BAO assignquestionnaireTemplete_BAO = new AssignQuestionnaire_BAO();

    DataTable dataTableCompanyName;
    WADIdentity identity;
    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);
    string pageNo = "";
    int AssignQuestionnaireCount = 0;
    string projectid;
    string userid;
    string Template;
    string Subject;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        try
        {
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                //if (identity.User.GroupID == 1)
                //{
                //    divAccount.Visible = true;
                //    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                //    ddlAccountCode_SelectedIndexChanged(sender, e);
                //}
                //else
                //{
                //    divAccount.Visible = false;
                //}

                Account_BAO account_BAO = new Account_BAO();
                //Bind account drop down by user account id.
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
                {
                    //AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                    //ddlParticipant.DataSource = accountUser_BAO.GetParticipantList(Convert.ToString(identity.User.AccountID));
                    //ddlParticipant.DataValueField = "UserID";
                    //ddlParticipant.DataTextField = "UserName";
                    //ddlParticipant.DataBind();

                    //Set the parameter for candidateobject data source.
                    odsCandidateStatus.SelectParameters.Clear();
                    odsCandidateStatus.SelectParameters.Add("condition", "-1");
                    odsCandidateStatus.Select();

                    Project_BAO project_BAO = new Project_BAO();
                    //Bind project drop down by user account id.
                    ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString("-1"));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    //ddlParticipant.Visible = true;
                    //lblParticipant.Visible = false;
                }
                else
                {
                    //ddlParticipant.Visible = false;
                    //lblParticipant.Visible = true;

                    //Else set participant name instead of account name.
                    lblParticipant.Text = identity.User.FName + " " + identity.User.LName;
                    lblProjectName.Text = "Participant";
                    lblParticipantHeading.Visible = false;

                    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                    DataTable dataTableParticipantInfo = new DataTable();

                    dataTableParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32("-1"));

                    //odsCandidateStatus.SelectParameters.Clear();
                    //odsCandidateStatus.SelectParameters.Add("condition", Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProgrammeID"]));
                    //odsCandidateStatus.Select();

                    ddlProject.Visible = false;
                    ddlProgramme.Visible = false;

                    lblProjectName.Visible = true;
                    lblProgrammeName.Visible = false;

                    imbReset.Visible = false;
                    imbSubmit.Visible = false;

                    AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();

                    DataTable dtuserlist = assignquestionnaire.GetuseridAssignQuestionnaireList(Convert.ToInt32("-1"));
                    Project_BAO project_BAO = new Project_BAO();

                    if (dtuserlist.Rows.Count > 0)
                    {
                        //int projectid = Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]);

                        ddlProject.Items.Clear();
                        ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                        //Get all project by users and bind project drop downlist.
                        DataTable project = project_BAO.GetdataProjectByID(Convert.ToInt32(dtuserlist.Rows[0]["ProjectID"]));
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
                //Reset grid page index .
                grdvCandidateStatus.PageSize = pageSize;
                ManagePaging();
            }

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");

            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region Gridview Paging Related Methods
    /// <summary>
    ///  Manage Paging when gridview page index changes.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        //string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

        //if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
        //{
        AssignQuestionnaireCount = AssignQuestionnaire_BAO.GetAssignQuestionnaireListCount(GetCondition());
        //}
        //else
        //{
        //    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        //    DataTable dtParticipantInfo = new DataTable();
        //    dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        //    string condition = Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProgrammeID"]);

        //    AssignQuestionnaireCount = AssignQuestionnaire_BAO.GetAssignQuestionnaireListCount(condition);
        //}

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

        ReBindEmailContent();
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
            if (Convert.ToInt32(ViewState["AccountID"]) > 0 && Convert.ToInt32(ViewState["TargetPersonID"]) > 0 && Convert.ToInt32(ViewState["ProgrammeID"]) > 0)
            {
                ManagePaging();
            }
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
        //Reset Gridview page index to new index.
        grdvCandidateStatus.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
        grdvCandidateStatus.DataBind();

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
            grdvCandidateStatus.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvCandidateStatus.DataBind();
            ManagePaging();
            //show page number on page number text.
            textBoxGoto.Text = pageNo;
        }
    }

    /// <summary>
    /// Sort gridview when clicked on heading.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCandidateStatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {//Reset page index
            grdvCandidateStatus.PageIndex = 0;
            grdvCandidateStatus.DataBind();
            //Manage paging
            ManagePaging();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// No user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCandidateStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    /// <summary>
    /// No use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCandidateStatus_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    #endregion

    #region Search Related Function
    /// <summary>
    /// Bind candidate list with selected account
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["AccountID"] = ddlAccountCode.SelectedValue;
        ViewState["TargetPersonID"] = ddlParticipant.SelectedValue;
        ViewState["ProgrammeID"] = ddlProgramme.SelectedValue;
        //Clear candidate object data source
        odsCandidateStatus.SelectParameters.Clear();
        //Reset object data source properties dynamic query.
        odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
        odsCandidateStatus.Select();
        //Set grid default page index
        grdvCandidateStatus.PageIndex = 0;
        grdvCandidateStatus.DataBind();
        //Manage paging
        ManagePaging();
    }

    /// <summary>
    /// Reset controls value
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

        lblMessage.Text = "";
    }

    /// <summary>
    /// Send mail to selected candidate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSend_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();
            AssignQuestionnaire_BE assignquestionnaire_BE = new AssignQuestionnaire_BE();
            string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

            // Get Candidate Email Image Name & Will Combined with EmailImagePath
            DataTable dataTableCandidateEmailImage = new DataTable();
            string emailimagepath = "";
            dataTableCandidateEmailImage = assignquestionnaire_BAO.GetCandidateEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));

            if (dataTableCandidateEmailImage.Rows.Count > 0 && dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                emailimagepath = imagepath + dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString();

            identity = this.Page.User.Identity as WADIdentity;
            //If it is a super Admin set account id to account drop down value else user account value.
            if (identity.User.GroupID == 1)
            {
                assignquestionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                assignquestionnaire_BE.AccountID = identity.User.AccountID;
            }

            assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(ddlParticipant.SelectedValue);
            //Read grid row by row for selected candidates
            foreach (GridViewRow row in grdvCandidateStatus.Rows)
            {
                CheckBox myCheckBox = (CheckBox)row.FindControl("myCheckBox");
                Label labelCandidateEmail = (Label)row.FindControl("lblCandidateEmail");

                HiddenField hdnQuestionnaireId = (HiddenField)row.FindControl("hdnQuestionnaireId");
                HiddenField hdnCandidateId = (HiddenField)row.FindControl("hdnCandidateId");
                HiddenField hdnCandidateName = (HiddenField)row.FindControl("hdnCandidateName");
                HiddenField hdnOrganizationName = (HiddenField)row.FindControl("hdnOrganizationName");
                HiddenField hdnStartDate = (HiddenField)row.FindControl("hdnStartDate");
                HiddenField hdnEndDate = (HiddenField)row.FindControl("hdnEndDate");
                HiddenField hdnRelationShip = (HiddenField)row.FindControl("hdnRelationShip");
                HiddenField hdnAsgnDetailID = (HiddenField)row.FindControl("hdnAsgnDetailID");
                HiddenField hdnAssiggnmentID = (HiddenField)row.FindControl("hdnAssiggnmentID");

                HiddenField hdnLoginID = (HiddenField)row.FindControl("hdnLoginID");
                HiddenField hdnPassword = (HiddenField)row.FindControl("hdnPassword");
                HiddenField hdnCode = (HiddenField)row.FindControl("hdnCode");
                HiddenField hdnTitle = (HiddenField)row.FindControl("hdnTitle");


                Template = txtFaqText.Value; //ViewState["Template"].ToString();
                Subject = lblEmailSubject.Text; // ViewState["Subject"].ToString();

                AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                DataTable dataTableAccountAdmin = new DataTable();
                //Get account admin details
                dataTableAccountAdmin = accountUser_BAO.GetdtAccountUserByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.TargetPersonID));

                //Template = assignquestionnaireTemplete_BAO.FindParticipantTemplate(Convert.ToInt32(ddlProject.SelectedValue));
                //Subject = assignquestionnaireTemplete_BAO.FindParticipantSubjectTemplate(Convert.ToInt32(ddlProject.SelectedValue));

                if (myCheckBox != null)
                {
                    if (myCheckBox.Checked == true)
                    {
                        // checkedCandidate.Add(lblCandidateEmail.Text);
                        //checkedCandidate.Add(hdnQuestionnaireId.Value.ToString());
                        string questionnaireID = "";
                        string candidateID = "";
                        string OrganisationName = "";
                        string Startdate = "";
                        string Enddate = "";
                        string CandidateName = "";
                        string FirstName = "";
                        string candidateEmail = "";
                        string RelationShip = "";
                        string AsgnDetailID = "";
                        string AssignmentID = "";
                        string Title = "";
                        string Loginid = "";
                        string password = "";
                        string Accountcode = "";

                        candidateEmail = labelCandidateEmail.Text.ToString();
                        questionnaireID = hdnQuestionnaireId.Value.ToString();
                        candidateID = hdnCandidateId.Value.ToString();
                        OrganisationName = hdnOrganizationName.Value.ToString();
                        Startdate = Convert.ToDateTime(hdnStartDate.Value).ToString("dd-MMM-yyyy");
                        Enddate = Convert.ToDateTime(hdnEndDate.Value).ToString("dd-MMM-yyyy");
                        CandidateName = hdnCandidateName.Value.ToString();
                        string[] strFName = CandidateName.Split(' ');
                        FirstName = strFName[0].ToString();
                        RelationShip = hdnRelationShip.Value.ToString();
                        AsgnDetailID = hdnAsgnDetailID.Value.ToString();
                        AssignmentID = hdnAssiggnmentID.Value.ToString();

                        Title = hdnTitle.Value.ToString();
                        Loginid = hdnLoginID.Value.ToString();
                        password = hdnPassword.Value.ToString();
                        Accountcode = hdnCode.Value.ToString();

                        questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                        candidateID = PasswordGenerator.EnryptString(candidateID);
                        //get tempalte path 
                        string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
                        //create feedback link
                        string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                        //if (dtResult.Rows[i]["RelationShip"].ToString() == "Self")
                        if (RelationShip == "Self")
                        {
                            string feedbackURL = urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + PasswordGenerator.EnryptString(AsgnDetailID);
                            assignquestionnaire_BAO.SetFeedbackURL(Convert.ToInt32(AsgnDetailID.ToString()), Convert.ToInt32(AssignmentID.ToString()), feedbackURL);
                        }
                        //Replace tokens 
                        Template = Template.Replace("[LINK]", link);
                        Template = Template.Replace("[NAME]", CandidateName);
                        Template = Template.Replace("[FIRSTNAME]", FirstName);
                        Template = Template.Replace("[COMPANY]", OrganisationName);
                        Template = Template.Replace("[STARTDATE]", Startdate);
                        Template = Template.Replace("[CLOSEDATE]", Enddate);
                        Template = Template.Replace("[TITLE]", Title);
                        Template = Template.Replace("[EMAILID]", candidateEmail);
                        Template = Template.Replace("[LOGINID]", Loginid);
                        Template = Template.Replace("[PASSWORD]", password);
                        Template = Template.Replace("[CODE]", Accountcode);

                        Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");

                        Subject = Subject.Replace("[NAME]", CandidateName);
                        Subject = Subject.Replace("[FIRSTNAME]", FirstName);
                        Subject = Subject.Replace("[COMPANY]", OrganisationName);
                        Subject = Subject.Replace("[STARTDATE]", Startdate);
                        Subject = Subject.Replace("[CLOSEDATE]", Enddate);

                        //if (dtResult.Rows[i]["RelationShip"].ToString() != "Self")
                        if (RelationShip != "Self")//If not self assest then replace token with participant details 
                        {
                            if (dataTableAccountAdmin.Rows.Count > 0)
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", dataTableAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dataTableAccountAdmin.Rows[0]["LastName"].ToString());
                                Template = Template.Replace("[PARTICIPANTEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                                Subject = Subject.Replace("[PARTICIPANTNAME]", dataTableAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dataTableAccountAdmin.Rows[0]["LastName"].ToString());
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                                //    MailAddress maddr = new MailAddress("admin@i-comment360.com", "admin");
                                //    SendEmail.Send(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString(), maddr, emailimagepath);
                                SendEmail.Send(Subject, Template, candidateEmail, "");
                            }
                            else
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                                Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                                Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

                                //Send Email
                                //SendEmail.Send(Subject, Template, "ashishg1@damcogroup.com");
                                SendEmail.Send(Subject, Template, candidateEmail, "");
                            }
                        }
                    }
                }
            }

            lblMessage.Text = "Email sent successfully";

            ClearControls();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Reset email text control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            txtFaqText.InnerHtml = Server.HtmlDecode(txtFaqText.InnerHtml);
            //Response.Redirect("ProjectList.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Reset control value.
    /// </summary>
    protected void ClearControls()
    {
        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        ddlProgramme.Items.Clear();
        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ddlParticipant.Items.Clear();
        ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailStart.Items.Clear();
        ddlEmailStart.Items.Insert(0, new ListItem("Select", "0"));

        //lblMessage.Text = "";
        lblEmailSubject.Text = "";
        txtFaqText.Value = "";

        identity = this.Page.User.Identity as WADIdentity;
        string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

        if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
        {
            odsCandidateStatus.SelectParameters.Clear();
            odsCandidateStatus.SelectParameters.Add("condition", "-1");
            odsCandidateStatus.Select();
        }
    }

    #endregion

    #region Dropdown Function
    /// <summary>
    /// Bind project in an account and comapny details.
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

            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO account_BAO = new Account_BAO();
            //Get company details in an account.
            dataTableCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //set company name.
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

            Project_BAO project_BAO = new Project_BAO();
            DataTable dataTableProject = new DataTable();
            //Get project list in company.
            dataTableProject = project_BAO.GetdtProjectList(Convert.ToString(companycode));

            if (dataTableProject.Rows.Count > 0)
            {
                //Bind project dropdown in a company.
                ddlProject.DataSource = dataTableProject;
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();
            }
            //Clear program dropdown  when project is changed
            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;

            lblMessage.Text = "";
            //Bind Data
            //odsCandidateStatus.SelectParameters.Clear();
            //odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
            //odsCandidateStatus.Select();

            //grdvCandidateStatus.PageIndex = 0;
            //grdvCandidateStatus.DataBind();

            //ManagePaging();

            EmailTemplate_BAO emailTemplate_BAO = new EmailTemplate_BAO();
            //Get all tempalte in an account
            DataTable dataTableEmailTemplate = emailTemplate_BAO.GetdtEmailTemplateList(Convert.ToString(ddlAccountCode.SelectedValue));

            //DataRow[] resultsTemplate = dtEmailTemplate.Select("Title LIKE '%Invitation Template%'");

            //DataTable dtmailtemp = dtEmailTemplate.Clone();

            //foreach (DataRow drMail in resultsTemplate)
            //{
            //    dtmailtemp.ImportRow(drMail);
            //}

            //int emailId = 0;
            //if (dtmailtemp.Rows.Count > 0)
            //    emailId = Convert.ToInt32(dtmailtemp.Rows[0]["EmailTemplateID"]);
            ddlEmailStart.Items.Clear();
            //Bind email tempate drop down
            ddlEmailStart.DataSource = dataTableEmailTemplate;
            ddlEmailStart.DataValueField = "EmailTemplateID";
            ddlEmailStart.DataTextField = "Title";
            ddlEmailStart.DataBind();

            ddlEmailStart.Items.Insert(0, new ListItem("Select", "0"));
            //if (emailId != 0)
            //{
            //    ddlEmailStart.SelectedValue = Convert.ToString(emailId);
            //    //ddlEmailStart.Enabled = false;
            //}

            //emailtemplate_BEList = emailtemplate_BAO.GetEmailTemplateByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlEmailStart.SelectedValue));
            //txtFaqText.Value = emailtemplate_BEList[0].EmailText.ToString();

            //Rebind Template text.
            ReBindEmailContent();
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

            ddlEmailStart.Items.Clear();
            ddlEmailStart.Items.Insert(0, new ListItem("Select", "0"));

            lblEmailSubject.Text = "";
            ViewState["AccountID"] = "0";
        }
    }

    /// <summary>
    /// Get all program in a project.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get all program in a project .
        dataTableProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
            //Bind program in project.
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }
        //Rebind email template data.
        ReBindEmailContent();
        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        //if (ddlProgramme.Items.Count > 1)
        //ddlProgramme.Items[1].Selected = true;

        //Clear Participant dropdown.
        ddlParticipant.Items.Clear();
        ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
    }

    /// <summary>
    /// Bind Template data when participant value changes.
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
        ReBindEmailContent();
    }

    /// <summary>
    /// Get all program in a project.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignQstnParticipant_BAO participant_BAO = new AssignQstnParticipant_BAO();

        if (ddlProgramme.SelectedIndex > 0)
        {
            DataTable dataTableParticipant = new DataTable();

            //if (identity.User.GroupID == 1)
            //{

            //Get all Participant in a project .
            dataTableParticipant = participant_BAO.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
            //}
            //else
            //{
            //    dtParticipant = participant_BAO.GetdtAssignPartiList(identity.User.AccountID.ToString(), ddlProgramme.SelectedValue);
            //}

            Project_BAO project_BAO = new Project_BAO();

            if (dataTableParticipant.Rows.Count > 0)
            {
                ddlParticipant.Items.Clear();
                ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
                //Bind Participant in project.
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
            //Rebind email template data.
            ReBindEmailContent();
        }
        else
        {
            ddlParticipant.Items.Clear();
            ddlParticipant.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    /// <summary>
    /// Bind email text and subject by tempaltes id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlEmailStart_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Get email template details. 
        emailtemplateList = emailtemplate_BAO.GetEmailTemplateByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlEmailStart.SelectedValue));
        txtFaqText.Value = Server.HtmlDecode(emailtemplateList[0].EmailText.ToString());
        string emailsubject = emailtemplateList[0].Subject.ToString();
        //emailsubject = emailsubject.Replace("[PARTICIPANTNAME]", "Participant");
        //emailsubject = emailsubject.Replace("[PARTICIPANTEMAIL]", "");
        lblEmailSubject.Text = emailsubject;//Bind subject

        ViewState["Template"] = emailtemplateList[0].EmailText.ToString();
        ViewState["Subject"] = emailsubject;
        //Manage rid paging
        ManagePaging();
    }

    #endregion

    /// <summary>
    /// Create dnamic query
    /// </summary>
    /// <returns></returns>
    protected string GetCondition()
    {
        string param = "0";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0 && Convert.ToInt32(ViewState["TargetPersonID"]) > 0 && Convert.ToInt32(ViewState["ProgrammeID"]) > 0)
        {
            string stringQuery = "";

            if (Convert.ToInt32(ViewState["AccountID"]) > 0)
                stringQuery = stringQuery + "" + ViewState["AccountID"] + " and ";
            else
                stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

            if (ddlParticipant.SelectedIndex > 0)
                stringQuery = stringQuery + "[TargetPersonID] = " + ViewState["TargetPersonID"] + " and ";

            //if (ddlProject.SelectedIndex > 0)
            //    str = str + "dbo.AssignQuestionnaire.ProjecctID = " + ddlProject.SelectedValue + " and ";

            if (ddlProgramme.SelectedIndex > 0)
                stringQuery = stringQuery + "dbo.Programme.ProgrammeID = " + ViewState["ProgrammeID"] + " and ";

            stringQuery = stringQuery + "dbo.AssignmentDetails.RelationShip != 'Self'    ";

            param = stringQuery.Substring(0, stringQuery.Length - 4);
        }

        return param;
    }

    /// <summary>
    /// Rebind Emailtepmalte data
    /// </summary>
    private void ReBindEmailContent()
    {
        txtFaqText.InnerHtml = Server.HtmlDecode(txtFaqText.InnerHtml);
    }
}

