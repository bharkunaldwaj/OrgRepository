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
using Admin_BE;
using Admin_BAO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections;
using Miscellaneous;
using System.Net.Mail;

public partial class Module_Admin_EmailCandidate : CodeBehindBase
{
    #region Global Refrence & Variable

    AssignQuestionnaire_BAO AssignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
    AssignQuestionnaire_BE AssignQuestionnaire_BE = new AssignQuestionnaire_BE();
    Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
    EmailTemplate_BAO emailtemplate_BAO = new EmailTemplate_BAO();
    EmailTemplate_BE emailtemplate_BE = new EmailTemplate_BE();
    List<EmailTemplate_BE> emailtemplate_BEList = new List<EmailTemplate_BE>();
    AssignQuestionnaire_BAO assignquestionnaireTemplete_BAO = new AssignQuestionnaire_BAO();

    DataTable dtCompanyName;
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

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
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

                    odsCandidateStatus.SelectParameters.Clear();
                    odsCandidateStatus.SelectParameters.Add("condition", "-1");
                    odsCandidateStatus.Select();

                    Project_BAO project_BAO = new Project_BAO();
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

                    lblParticipant.Text = identity.User.FName + " " + identity.User.LName;
                    lblProjectName.Text = "Participant";
                    lblParticipantHeading.Visible = false;

                    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                    DataTable dtParticipantInfo = new DataTable();
                    dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32("-1"));

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
            grdvCandidateStatus.PageSize = pageSize;
            ManagePaging();
            }

            TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (txtGoto != null)
                txtGoto.Text = pageNo;

           
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
            if (Convert.ToInt32(ViewState["AccountID"]) > 0 && Convert.ToInt32(ViewState["TargetPersonID"]) > 0 && Convert.ToInt32(ViewState["ProgrammeID"]) > 0)
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

    protected void grdvCandidateStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
    }

    protected void grdvCandidateStatus_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    #endregion

    #region Search Related Function

    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["AccountID"] = ddlAccountCode.SelectedValue;
        ViewState["TargetPersonID"] = ddlParticipant.SelectedValue;
        ViewState["ProgrammeID"] = ddlProgramme.SelectedValue;

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

        lblMessage.Text = "";
    }

    protected void imbSend_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();
            AssignQuestionnaire_BE assignquestionnaire_BE = new AssignQuestionnaire_BE();
            string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

            // Get Candidate Email Image Name & Will Combined with EmailImagePath
            DataTable dtCandidateEmailImage = new DataTable();
            string emailimagepath = "";
            dtCandidateEmailImage = assignquestionnaire_BAO.GetCandidateEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));
            if (dtCandidateEmailImage.Rows.Count > 0 && dtCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                emailimagepath = imagepath + dtCandidateEmailImage.Rows[0]["EmailImage"].ToString();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                assignquestionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);                
            }
            else
            {
                assignquestionnaire_BE.AccountID = identity.User.AccountID;                
            }

            assignquestionnaire_BE.TargetPersonID = Convert.ToInt32(ddlParticipant.SelectedValue);

            foreach (GridViewRow row in grdvCandidateStatus.Rows)
            {
                CheckBox myCheckBox = (CheckBox)row.FindControl("myCheckBox");
                Label lblCandidateEmail = (Label)row.FindControl("lblCandidateEmail");

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
                DataTable dtAccountAdmin = new DataTable();

                dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.TargetPersonID));


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



                        candidateEmail = lblCandidateEmail.Text.ToString();
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

                        string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                        string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                        //if (dtResult.Rows[i]["RelationShip"].ToString() == "Self")
                        if (RelationShip == "Self")
                        {
                            string feedbackURL = urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + PasswordGenerator.EnryptString(AsgnDetailID);
                            assignquestionnaire_BAO.SetFeedbackURL(Convert.ToInt32(AsgnDetailID.ToString()), Convert.ToInt32(AssignmentID.ToString()), feedbackURL);
                        }

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
                        if (RelationShip != "Self")
                        {
                            if (dtAccountAdmin.Rows.Count > 0)
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                                Template = Template.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                               Subject = Subject.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());
                                                                
                            //    MailAddress maddr = new MailAddress("admin@i-comment360.com", "admin");
                            //    SendEmail.Send(Subject, Template, dtResult.Rows[i]["CandidateEmail"].ToString(), maddr, emailimagepath);
                                SendEmail.Send(Subject, Template, candidateEmail,"");
                            }
                            else
                            {
                                Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                                Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                                Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                                Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

                                
                                //SendEmail.Send(Subject, Template, "ashishg1@damcogroup.com");
                                SendEmail.Send(Subject, Template, candidateEmail,"");
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

    protected void imbcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            //Response.Redirect("ProjectList.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

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

            lblMessage.Text = "";
            //Bind Data
            //odsCandidateStatus.SelectParameters.Clear();
            //odsCandidateStatus.SelectParameters.Add("condition", GetCondition());
            //odsCandidateStatus.Select();

            //grdvCandidateStatus.PageIndex = 0;
            //grdvCandidateStatus.DataBind();

            //ManagePaging();

            EmailTemplate_BAO emailTemplate_BAO = new EmailTemplate_BAO();
            DataTable dtEmailTemplate = emailTemplate_BAO.GetdtEmailTemplateList(Convert.ToString(ddlAccountCode.SelectedValue));

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

            ddlEmailStart.DataSource = dtEmailTemplate;
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

            //if (identity.User.GroupID == 1)
            //{
                dtParticipant = participant_BAO.GetdtAssignPartiList(ddlAccountCode.SelectedValue, ddlProgramme.SelectedValue);
            //}
            //else
            //{
            //    dtParticipant = participant_BAO.GetdtAssignPartiList(identity.User.AccountID.ToString(), ddlProgramme.SelectedValue);
            //}

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

    protected void ddlEmailStart_SelectedIndexChanged(object sender, EventArgs e)
    {
        emailtemplate_BEList = emailtemplate_BAO.GetEmailTemplateByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlEmailStart.SelectedValue));
        txtFaqText.Value = emailtemplate_BEList[0].EmailText.ToString();
        string emailsubject = emailtemplate_BEList[0].Subject.ToString();
        //emailsubject = emailsubject.Replace("[PARTICIPANTNAME]", "Participant");
        //emailsubject = emailsubject.Replace("[PARTICIPANTEMAIL]", "");
        lblEmailSubject.Text = emailsubject;

        ViewState["Template"] = emailtemplate_BEList[0].EmailText.ToString();
        ViewState["Subject"] = emailsubject;
    
        ManagePaging();
    }

    #endregion

    protected string GetCondition()
    {
        string param="0";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0 && Convert.ToInt32(ViewState["TargetPersonID"]) > 0 && Convert.ToInt32(ViewState["ProgrammeID"]) > 0)
        {
            string str = "";

            if (Convert.ToInt32(ViewState["AccountID"]) > 0)
                str = str + "" + ViewState["AccountID"] + " and ";
            else
                str = str + "" + identity.User.AccountID.ToString() + " and ";

            if (ddlParticipant.SelectedIndex > 0)
                str = str + "[TargetPersonID] = " + ViewState["TargetPersonID"] + " and ";

            //if (ddlProject.SelectedIndex > 0)
            //    str = str + "dbo.AssignQuestionnaire.ProjecctID = " + ddlProject.SelectedValue + " and ";

            if (ddlProgramme.SelectedIndex > 0)
                str = str + "dbo.Programme.ProgrammeID = " + ViewState["ProgrammeID"] + " and ";

            str = str + "dbo.AssignmentDetails.RelationShip != 'Self'    ";

            param = str.Substring(0, str.Length - 4);
        }

        return param;
    }
}

