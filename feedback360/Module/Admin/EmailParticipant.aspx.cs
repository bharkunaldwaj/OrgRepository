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
using Miscellaneous;
using System.Net.Mail;

public partial class Module_Admin_EmailParticipant : CodeBehindBase
{
    #region Global Refrence & Variable
    //Global variables
    AssignQuestionnaire_BAO AssignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
    AssignQuestionnaire_BE AssignQuestionnaireBusinessEntity = new AssignQuestionnaire_BE();

    AssignQstnParticipant_BAO AssignQuestionnaireParticipantBusinessAccessObject = new AssignQstnParticipant_BAO();

    Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
    EmailTemplate_BAO emailTemplateBusinessAccessObject = new EmailTemplate_BAO();
    EmailTemplate_BE emailTemplateBusinessEntity = new EmailTemplate_BE();
    List<EmailTemplate_BE> emailtemplateList = new List<EmailTemplate_BE>();
    AssignQuestionnaire_BAO assignQuestionnaireTempleteBusinessAccessObject = new AssignQuestionnaire_BAO();

    AssignQstnParticipant_BAO assignQstnParticipantBusinessAccessObject = new AssignQstnParticipant_BAO();

    DataTable dataTableCompanyName;
    WADIdentity identity;
    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);
    string pageNo = "";
    int AssignQuestionnaireCount = 0;
    //string projectid;
    //string userid;
    string Template;
    string Subject;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Bind account dropdown by user account id,
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                odsCandidateStatus.SelectParameters.Clear();
                //Reset Candidate object data source  
                odsCandidateStatus.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue.ToString());
                odsCandidateStatus.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue.ToString());
                odsCandidateStatus.Select();
                //set grid page size to 500
                grdvCandidateStatus.PageSize = 500;
                //Handle paging while page index changes in grid.
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
    ///It is of NO use
    /// </summary>
    protected void ManagePaging()
    {
        //identity = this.Page.User.Identity as WADIdentity;

        //string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

        ////if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
        ////{
        //    AssignQuestionnaireCount = AssignQuestionnaire_BAO.GetAssignQuestionnaireListCount(GetCondition());
        ////}
        ////else
        ////{
        ////    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        ////    DataTable dtParticipantInfo = new DataTable();
        ////    dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

        ////    string condition = Convert.ToInt32(identity.User.AccountID) + " and [TargetPersonID]=" + Convert.ToInt32(identity.User.UserID) + " and Project.[ProjectID]=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProjecctID"]) + " and [Programme].ProgrammeID=" + Convert.ToInt32(dtParticipantInfo.Rows[0]["ProgrammeID"]);

        ////    AssignQuestionnaireCount = AssignQuestionnaire_BAO.GetAssignQuestionnaireListCount(condition);
        ////}

        //plcPaging.Controls.Clear();

        //if (AssignQuestionnaireCount > 0)
        //{
        //    // Variable declaration
        //    int numberOfPages;
        //    int numberOfRecords = AssignQuestionnaireCount;
        //    int currentPage = (grdvCandidateStatus.PageIndex);
        //    StringBuilder strSummary = new StringBuilder();

        //    // If number of records is more then the page size (specified in global variable)
        //    // Just to check either gridview have enough records to implement paging
        //    if (numberOfRecords > pageSize)
        //    {
        //        // Calculating the total number of pages
        //        numberOfPages = (int)Math.Ceiling((double)numberOfRecords / (double)pageSize);
        //    }
        //    else
        //    {
        //        numberOfPages = 1;
        //    }


        //    // Creating a small summary for records.
        //    strSummary.Append("Displaying <b>");

        //    // Creating X f X Records
        //    int floor = (currentPage * pageSize) + 1;
        //    strSummary.Append(floor.ToString());
        //    strSummary.Append("</b>-<b>");
        //    int ceil = ((currentPage * pageSize) + pageSize);

        //    //let say you have 26 records and you specified 10 page size, 
        //    // On the third page it will return 30 instead of 25 as that is based on pageSize
        //    // So this check will see if the ceil value is increasing the number of records. Consider numberOfRecords
        //    if (ceil > numberOfRecords)
        //    {
        //        strSummary.Append(numberOfRecords.ToString());
        //    }
        //    else
        //    {
        //        strSummary.Append(ceil.ToString());
        //    }

        //    // Displaying Total number of records Creating X of X of About X records.
        //    strSummary.Append("</b> of <b>");
        //    strSummary.Append(numberOfRecords.ToString());
        //    strSummary.Append("</b> records</br>");


        //    litPagingSummary.Text = ""; // strSummary.ToString();


        //    //Variable declaration 
        //    //these variables will used to calculate page number display
        //    int pageShowLimitStart = 1;
        //    int pageShowLimitEnd = 1;

        //    // Just to check, either there is enough pages to implement page number display logic.
        //    if (pageDispCount > numberOfPages)
        //    {
        //        pageShowLimitEnd = numberOfPages; // Setting the end limit to the number of pages. Means show all page numbers
        //    }
        //    else
        //    {
        //        if (currentPage > 4) // If page index is more then 4 then need to less the page numbers from start and show more on end.
        //        {
        //            //Calculating end limit to show more page numbers
        //            pageShowLimitEnd = currentPage + (int)(Math.Floor((decimal)pageDispCount / 2));
        //            //Calculating Start limit to hide previous page numbers
        //            pageShowLimitStart = currentPage - (int)(Math.Floor((decimal)pageDispCount / 2));
        //        }
        //        else
        //        {
        //            //Simply Displaying the 10 pages. no need to remove / add page numbers
        //            pageShowLimitEnd = pageDispCount;
        //        }
        //    }

        //    // Since the pageDispCount can be changed and limit calculation can cause < 0 values 
        //    // Simply, set the limit start value to 1 if it is less
        //    if (pageShowLimitStart < 1)
        //        pageShowLimitStart = 1;


        //    //Dynamic creation of link buttons

        //    // First Link button to display with paging
        //    LinkButton objLbFirst = new LinkButton();
        //    objLbFirst.Click += new EventHandler(objLb_Click);
        //    //objLbFirst.Text = "First";
        //    objLbFirst.CssClass = "first";
        //    objLbFirst.ToolTip = "First Page";
        //    objLbFirst.ID = "lb_FirstPage";
        //    objLbFirst.CommandName = "pgChange";
        //    objLbFirst.EnableViewState = true;
        //    objLbFirst.CommandArgument = "1";

        //    //Previous Link button to display with paging
        //    LinkButton objLbPrevious = new LinkButton();
        //    objLbPrevious.Click += new EventHandler(objLb_Click);
        //    //objLbPrevious.Text = "Previous";
        //    objLbPrevious.CssClass = "previous";
        //    objLbPrevious.ToolTip = "Previous Page";
        //    objLbPrevious.ID = "lb_PreviousPage";
        //    objLbPrevious.CommandName = "pgChange";
        //    objLbPrevious.EnableViewState = true;
        //    objLbPrevious.CommandArgument = currentPage.ToString();


        //    //of course if the page is the 1st page, then there is no need of First or Previous
        //    if (currentPage == 0)
        //    {
        //        objLbFirst.Enabled = false;
        //        objLbPrevious.Enabled = false;
        //    }
        //    else
        //    {
        //        objLbFirst.Enabled = true;
        //        objLbPrevious.Enabled = true;
        //    }

        //    plcPaging.Controls.Add(new LiteralControl("<table border=0><tr><td valign=middle>"));

        //    //Adding control in a place holder
        //    plcPaging.Controls.Add(objLbFirst);
        //    //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;")); // Just to give some space 
        //    plcPaging.Controls.Add(objLbPrevious);
        //    //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));


        //    // Creatig page numbers based on the start and end limit variables.
        //    for (int i = pageShowLimitStart; i <= pageShowLimitEnd; i++)
        //    {
        //        if ((Page.FindControl("lb_" + i.ToString()) == null) && i <= numberOfPages)
        //        {
        //            LinkButton objLb = new LinkButton();
        //            objLb.Click += new EventHandler(objLb_Click);
        //            objLb.Text = i.ToString();
        //            objLb.ID = "lb_" + i.ToString();
        //            objLb.CommandName = "pgChange";
        //            objLb.ToolTip = "Page " + i.ToString();
        //            objLb.EnableViewState = true;
        //            objLb.CommandArgument = i.ToString();

        //            if ((currentPage + 1) == i)
        //            {
        //                objLb.CssClass = "active";
        //                objLb.Enabled = false;

        //            }

        //            plcPaging.Controls.Add(objLb);
        //            //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));
        //        }
        //    }

        //    // Last Link button to display with paging
        //    LinkButton objLbLast = new LinkButton();
        //    objLbLast.Click += new EventHandler(objLb_Click);
        //    //objLbLast.Text = "Last";
        //    objLbLast.CssClass = "last";
        //    objLbLast.ToolTip = "Last Page";
        //    objLbLast.ID = "lb_LastPage";
        //    objLbLast.CommandName = "pgChange";
        //    objLbLast.EnableViewState = true;
        //    objLbLast.CommandArgument = numberOfPages.ToString();

        //    // Next Link button to display with paging
        //    LinkButton objLbNext = new LinkButton();
        //    objLbNext.Click += new EventHandler(objLb_Click);
        //    //objLbNext.Text = "Next";
        //    objLbNext.CssClass = "next";
        //    objLbNext.ToolTip = "Next Page";
        //    objLbNext.ID = "lb_NextPage";
        //    objLbNext.CommandName = "pgChange";
        //    objLbNext.EnableViewState = true;
        //    objLbNext.CommandArgument = (currentPage + 2).ToString();

        //    //of course if the page is the last page, then there is no need of last or next
        //    if ((currentPage + 1) == numberOfPages)
        //    {
        //        objLbLast.Enabled = false;
        //        objLbNext.Enabled = false;
        //    }
        //    else
        //    {
        //        objLbLast.Enabled = true;
        //        objLbNext.Enabled = true;
        //    }

        //    // Adding Control to the place holder
        //    plcPaging.Controls.Add(objLbNext);
        //    //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));
        //    plcPaging.Controls.Add(objLbLast);
        //    //plcPaging.Controls.Add(new LiteralControl("&nbsp; | &nbsp;"));

        //    plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));
        //    TextBox objtxtGoto = new TextBox();
        //    objtxtGoto.ID = "txtGoto";
        //    objtxtGoto.ToolTip = "Enter Page No.";
        //    objtxtGoto.MaxLength = 2;
        //    objtxtGoto.SkinID = "grdvgoto";
        //    objtxtGoto.Attributes.Add("onKeypress", "javascript:return NumberOnly(this);");
        //    objtxtGoto.Text = pageNo;
        //    plcPaging.Controls.Add(objtxtGoto);

        //    plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));

        //    ImageButton objIbtnGo = new ImageButton();
        //    objIbtnGo.ID = "ibtnGo";
        //    objIbtnGo.ToolTip = "Goto Page";
        //    objIbtnGo.ImageUrl = "~/Layouts/Resources/images/go-btn.png";
        //    objIbtnGo.Click += new ImageClickEventHandler(objIbtnGo_Click);
        //    plcPaging.Controls.Add(objIbtnGo);

        //    plcPaging.Controls.Add(new LiteralControl("</td></tr></table>"));
        //}
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
            if (Convert.ToInt32(ViewState["AccountID"]) > 0 && Convert.ToInt32(ViewState["ProgrammeID"]) > 0)
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
        {
            //Reset page index
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
    /// Bind row controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCandidateStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Find controls
            Label labelCandidateCount = (Label)e.Row.FindControl("lblCandidateCount");
            HiddenField hdnUserID = (HiddenField)e.Row.FindControl("hdnUserID");
            Label labelSubmissionCount = (Label)e.Row.FindControl("lblSubmissionCount");
            Label labelSelfAssessment = (Label)e.Row.FindControl("lblSelfAssessment");
            Label labelTargetPersonID = (Label)e.Row.FindControl("lblTargetPersonID");

            // Label lblCandidateCount = (Label)e.Row.FindControl("lblCandidateCount");
            DataRowView rowView = e.Row.DataItem as DataRowView;

            int targetPersonID = Convert.ToInt32(labelTargetPersonID.Text);
            labelCandidateCount.Text = AssignQuestionnaireParticipantBusinessAccessObject.GetCandidatesCount(targetPersonID).ToString();
            labelSubmissionCount.Text = AssignQuestionnaireParticipantBusinessAccessObject.GetSubmissionCount(targetPersonID).ToString();
            //Get whether it is self assistment
            int selfAssessmentFlag = AssignQuestionnaireParticipantBusinessAccessObject.GetSelfAssessment(targetPersonID);

            if (selfAssessmentFlag == 0)
                labelSelfAssessment.Text = "No"; //if it is not then set no else yes
            else
                labelSelfAssessment.Text = "Yes";

            if (labelCandidateCount != null)
            {
                //get number of canidate count
                labelCandidateCount.Text = assignQstnParticipantBusinessAccessObject.GetCandidatesCount(Convert.ToInt32(hdnUserID.Value)).ToString();
            }
        }
    }

    /// <summary>
    /// It is of no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvCandidateStatus_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    /// <summary>
    /// Create dnamic query
    /// </summary>
    /// <returns></returns>
    protected string GetCondition()
    {
        string param = "0";

        if (Convert.ToInt32(ViewState["AccountID"]) > 0 && Convert.ToInt32(ViewState["ProgrammeID"]) > 0)
        {
            string stringQuery = "";

            if (Convert.ToInt32(ViewState["AccountID"]) > 0)
                stringQuery = stringQuery + "" + ViewState["AccountID"] + " and ";
            else
                stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

            //if (ddlProject.SelectedIndex > 0)
            //    str = str + "dbo.AssignQuestionnaire.ProjecctID = " + ddlProject.SelectedValue + " and ";

            if (Convert.ToInt32(ViewState["ProgrammeID"]) > 0)
                stringQuery = stringQuery + "dbo.AssignQuestionnaire.ProgrammeID = " + ViewState["ProgrammeID"] + " and ";

            stringQuery = stringQuery + "dbo.AssignmentDetails.RelationShip = 'Self'    ";

            param = stringQuery.Substring(0, stringQuery.Length - 4);
        }

        return param;
    }

    #endregion

    #region Search Related Function
    /// <summary>
    /// Bind participant list with selected account
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Clear candidate object data source
            odsCandidateStatus.SelectParameters.Clear();
            //Reset object data source properties to account and program id.
            odsCandidateStatus.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue.ToString());
            odsCandidateStatus.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue.ToString());
            odsCandidateStatus.Select();

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;
            ViewState["ProgrammeID"] = ddlProgramme.SelectedValue;
            //Set grid default page index
            grdvCandidateStatus.PageIndex = 0;
            grdvCandidateStatus.DataBind();
            //Manage paging 
            ManagePaging();
            //Rebind email tempalte
            ReBindEmailContent();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

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

        odsCandidateStatus.FilterExpression = null;
        odsCandidateStatus.FilterParameters.Clear();

        ManagePaging();

        lblMessage.Text = "";
        txtFaqText.InnerHtml = string.Empty;
    }

    /// <summary>
    /// Send mail to selected participant.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSend_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            AssignQuestionnaire_BAO assignquestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();
            string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

            // Get Candidate Email Image Name & Will Combined with EmailImagePath
            DataTable dataTableCandidateEmailImage = new DataTable();
            string emailimagepath = "";
            dataTableCandidateEmailImage = assignquestionnaireBusinessAccessObject.GetCandidateEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));

            if (dataTableCandidateEmailImage.Rows.Count > 0 && dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                emailimagepath = imagepath + dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString();

            //Read grid row by row for selected participant
            foreach (GridViewRow row in grdvCandidateStatus.Rows)
            {
                Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
                List<Programme_BE> listProgramme = new List<Programme_BE>();
                listProgramme = programmeBusinessAccessObject.GetProgrammeByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlProgramme.SelectedValue));

                AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
                DataTable dataTableAccountAdmin = new DataTable();
                //Get account admin details
                dataTableAccountAdmin = accountUserBusinessAccessObject.GetdtAccountAdmin(Convert.ToInt32(ddlAccountCode.SelectedValue));


                CheckBox myCheckBox = (CheckBox)row.FindControl("myCheckBox");
                HiddenField hdnTitle = (HiddenField)row.FindControl("hdnTitle");
                Label labelParticipantEmail = (Label)row.FindControl("lblParticipantEmail");
                HiddenField hdnFirstName = (HiddenField)row.FindControl("hdnFirstName");
                HiddenField hdnLoginID = (HiddenField)row.FindControl("hdnLoginID");
                HiddenField hdnPassword = (HiddenField)row.FindControl("hdnPassword");
                HiddenField hdnCode = (HiddenField)row.FindControl("hdnCode");

                Template = txtFaqText.Value; //ViewState["Template"].ToString();
                Subject = lblEmailSubject.Text; // ViewState["Subject"].ToString();

                if (myCheckBox != null)
                {//If checked then get details and send mail
                    if (myCheckBox.Checked == true)
                    {
                        string Title = "";
                        string EmailID = "";
                        string FirstName = "";
                        string Loginid = "";
                        string password = "";
                        string Accountcode = "";

                        Title = hdnTitle.Value.ToString();
                        EmailID = labelParticipantEmail.Text.ToString();
                        FirstName = hdnFirstName.Value.ToString();
                        Loginid = hdnLoginID.Value.ToString();
                        password = hdnPassword.Value.ToString();
                        Accountcode = hdnCode.Value.ToString();
                        //get template path 
                        string urlPath = ConfigurationManager.AppSettings["ParticipantURL"].ToString();
                        urlPath += "Login.aspx?" + Utilities.CreateEncryptedQueryString(Loginid, password, Accountcode);
                        //create feedback link
                        string link = "<a Target='_BLANK' href= '" + urlPath + "' >Click Link</a> ";
                        //Replace tokens 
                        Template = Template.Replace("[LINK]", link);
                        Template = Template.Replace("[TITLE]", Title);
                        Template = Template.Replace("[EMAILID]", EmailID);
                        Template = Template.Replace("[FIRSTNAME]", FirstName);
                        Template = Template.Replace("[LOGINID]", Loginid);
                        Template = Template.Replace("[PASSWORD]", password);
                        Template = Template.Replace("[CODE]", Accountcode);
                        Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");

                        Subject = Subject.Replace("[TITLE]", Title);
                        Subject = Subject.Replace("[EMAILID]", EmailID);
                        Subject = Subject.Replace("[FIRSTNAME]", FirstName);
                        Subject = Subject.Replace("[LOGINID]", Loginid);
                        Subject = Subject.Replace("[PASSWORD]", password);
                        Subject = Subject.Replace("[CODE]", Accountcode);

                        if (listProgramme.Count > 0)//Replace program tokens
                        {
                            Template = Template.Replace("[STARTDATE]", Convert.ToDateTime(listProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                            Template = Template.Replace("[CLOSEDATE]", Convert.ToDateTime(listProgramme[0].EndDate).ToString("dd-MMM-yyyy"));

                            Template = Template.Replace("[REPORTSTARTDATE]", Convert.ToDateTime(listProgramme[0].ReportAvaliableFrom).ToString("dd-MMM-yyyy"));
                            Template = Template.Replace("[REPORTENDDATE]", Convert.ToDateTime(listProgramme[0].ReportAvaliableTo).ToString("dd-MMM-yyyy"));

                            Subject = Subject.Replace("[STARTDATE]", Convert.ToDateTime(listProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                            Subject = Subject.Replace("[CLOSEDATE]", Convert.ToDateTime(listProgramme[0].EndDate).ToString("dd-MMM-yyyy"));
                        }

                        if (dataTableAccountAdmin.Rows.Count > 0)//If it is account admin then repalce tempalte with its details.
                        {
                            Template = Template.Replace("[ACCOUNTADMIN]", dataTableAccountAdmin.Rows[0]["FullName"].ToString());
                            Template = Template.Replace("[ADMINEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                            Subject = Subject.Replace("[ACCOUNTADMIN]", dataTableAccountAdmin.Rows[0]["FullName"].ToString());
                            Subject = Subject.Replace("[ADMINEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                            MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");
                            //SendEmail.Send(Subject, Template, "ashishg1@damcogroup.com", maddr, emailimagepath);
                            SendEmail.Send(Subject, Template, EmailID, maddr, emailimagepath);//Send email
                        }
                        else
                        {
                            Template = Template.Replace("[ACCOUNTADMIN]", "Account Admin");
                            Template = Template.Replace("[ADMINEMAIL]", "");
                            Template = Template.Replace("<img src=cid:companylogo>", "");

                            Subject = Subject.Replace("[ACCOUNTADMIN]", "Account Admin");
                            Subject = Subject.Replace("[ADMINEMAIL]", "");

                            //SendEmail.Send(Subject, Template, "ashishg1@damcogroup.com");
                            SendEmail.Send(Subject, Template, EmailID, "");
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
            ReBindEmailContent();
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

        ddlEmailStart.Items.Clear();
        ddlEmailStart.Items.Insert(0, new ListItem("Select", "0"));

        //lblMessage.Text = "";
        lblEmailSubject.Text = "";
        txtFaqText.InnerHtml = "";

        odsCandidateStatus.SelectParameters.Clear();
        odsCandidateStatus.SelectParameters.Add("accountID", null);
        odsCandidateStatus.SelectParameters.Add("programmeID", null);
        odsCandidateStatus.Select();
    }
    #endregion

    #region Dropdown Function
    /// <summary>
    /// Bind project in an account and comapny details id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            //Set account id
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get company details in an account.
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //set company name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            Project_BAO projectBusinessAccessObject = new Project_BAO();
            DataTable dataTableProject = new DataTable();
            //Get project list in company.
            dataTableProject = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

            if (dataTableProject.Rows.Count > 0)
            {//Bind project in a company.
                ddlProject.DataSource = dataTableProject;
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();
            }
            //Clear program dropdown when project is changed
            ddlProgramme.Items.Clear();
            ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;

            lblMessage.Text = "";
            //Clear candidate object data source.
            odsCandidateStatus.SelectParameters.Clear();
            //Reset object datasource to properties to account id.
            odsCandidateStatus.SelectParameters.Add("accountID", ddlAccountCode.SelectedValue.ToString());
            odsCandidateStatus.SelectParameters.Add("programmeID", ddlProgramme.SelectedValue.ToString());
            odsCandidateStatus.Select();

            EmailTemplate_BAO emailTemplateBusinessAccessObject = new EmailTemplate_BAO();
            //Get all tempalte in an account
            DataTable dataTableEmailTemplate = emailTemplateBusinessAccessObject.GetdtEmailTemplateList(Convert.ToString(ddlAccountCode.SelectedValue));

            ddlEmailStart.Items.Clear();
            //Bind email tempate drop down
            ddlEmailStart.DataSource = dataTableEmailTemplate;
            ddlEmailStart.DataValueField = "EmailTemplateID";
            ddlEmailStart.DataTextField = "Title";
            ddlEmailStart.DataBind();

            ddlEmailStart.Items.Insert(0, new ListItem("Select", "0"));
            //Rebind Template text.
            ReBindEmailContent();
        }
        else
        {
            //IF account selected index is -1 reset controls.
            lblcompanyname.Text = "";

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
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get all program in a project .
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
            //Bind program in project.
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        //Rebind email template data.
        ReBindEmailContent();
    }

    /// <summary>
    /// Bind email text and subject by tempaltes 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlEmailStart_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Get email template details. 
        emailtemplateList = emailTemplateBusinessAccessObject.GetEmailTemplateByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlEmailStart.SelectedValue));
        txtFaqText.InnerHtml = Server.HtmlDecode(emailtemplateList[0].EmailText.ToString());
        string emailsubject = emailtemplateList[0].Subject.ToString();
        //Replace tokens
        emailsubject = emailsubject.Replace("[PARTICIPANTNAME]", "Participant");
        emailsubject = emailsubject.Replace("[PARTICIPANTEMAIL]", "");
        lblEmailSubject.Text = emailsubject;

        ViewState["Template"] = emailtemplateList[0].EmailText.ToString();
        ViewState["Subject"] = emailsubject;
        //Handle grid page related events.
        ManagePaging();
    }

    #endregion

    /// <summary>
    /// Rebind email editor
    /// </summary>
    private void ReBindEmailContent()
    {
        txtFaqText.InnerHtml = Server.HtmlDecode(txtFaqText.InnerHtml);
    }
}

