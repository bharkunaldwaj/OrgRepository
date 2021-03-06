﻿using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Miscellaneous;
using System.Net.Mail;

using Admin_BAO;
using Questionnaire_BAO;

public partial class Survey_Module_Feedback_AssignQuestionnaireList : System.Web.UI.Page
{
    Survey_AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new Survey_AssignQuestionnaire_BAO();
    //Survey_AssignQuestionnaire_BE AssignQuestionnaire_BE = new Survey_AssignQuestionnaire_BE();
    Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int AssignQuestionnaireCount = 0;
    string pageNo = "";
    WADIdentity identity;
    DataTable dataTableCompanyName;
    string projectid;
    string userid;
    string ProgramId;
    string ProgramName;
    string ProjectDesc;
    string Questionair;

    CodeBehindBase cBase = new CodeBehindBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Label ll = (Label)this.Master.FindControl("Current_location");
            //ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            identity = this.Page.User.Identity as WADIdentity;

            TextBox textBoxGoto = (TextBox)plcPaging.FindControl("txtGoto");

            if (textBoxGoto != null)
                textBoxGoto.Text = pageNo;

            if (!IsPostBack)
            {
                projectid = Request.QueryString["projectID"];
                userid = Request.QueryString["userid"];
                ProgramId = Request.QueryString["ProgId"];
               
                ProjectDesc = Request.QueryString["ProjectName"].Replace("~","'");
                ProgramName = Request.QueryString["ProgrammeName"];
                Questionair = Request.QueryString["Questionair"];
                //projectid = "84";
                //userid = "1149";
                DataTable dataTableCollegue = new DataTable();
                dataTableCollegue = assignQuestionnaireBusinessAccessObject.GetColleaguesListView(Convert.ToInt32(projectid), Convert.ToInt32(ProgramId));
                
                DataTable dtProgramme = new DataTable();
                dtProgramme = programmeBusinessAccessObject.GetProgrammeByID(Convert.ToInt32(ProgramId));

                /******************************************
                 * Assigning the progect/program details
                 * *************************************/
                lblProjectVal.Text = ProjectDesc;
                lblProgrammeName.Text = ProgramName;
                lblQuestionairVal.Text = Questionair;
                
                //DataTable dtRelationship = new DataTable();
               // Project_BAO project_BAO = new Project_BAO();

                //dtRelationship = project_BAO.GetProjectRelationship(Convert.ToInt32(dtProgramme.Rows[0]["ProjectID"]));
                //Session["Relationship"] = dtRelationship;

                //grdvAssignQuestionnaire.DataSource = dtCollegue;
                //grdvAssignQuestionnaire.DataBind();

                DataTable dataTableAnalysis = new DataTable();
                dataTableAnalysis = programmeBusinessAccessObject.GetAnalysis1(Convert.ToInt32(ProgramId));
                Session["dtAnalysis1"] = dataTableAnalysis;
                dataTableAnalysis = null;
                dataTableAnalysis = programmeBusinessAccessObject.GetAnalysis2(Convert.ToInt32(ProgramId));
                Session["dtAnalysis2"] = dataTableAnalysis;
                dataTableAnalysis = null;
                dataTableAnalysis = programmeBusinessAccessObject.GetAnalysis3(Convert.ToInt32(ProgramId));
                Session["dtAnalysis3"] = dataTableAnalysis;
                dataTableAnalysis = null;

                odsAssignQuestionnaire.SelectParameters.Clear();
                odsAssignQuestionnaire.SelectParameters.Add("ProjectID", projectid);
                odsAssignQuestionnaire.SelectParameters.Add("ProgID", ProgramId);
                odsAssignQuestionnaire.Select();

                grdvAssignQuestionnaire.PageSize = pageSize;
                ManagePaging();

                // Display the Save button only if the records exists.
                if (grdvAssignQuestionnaire.Rows.Count > 0)
                {
                    imbSave.Visible = true;
                }
                else
                {
                    imbSave.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void grdvAssignQuestionnaire_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ManagePaging();
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    //protected void grdvAssignQuestionnaire_Deleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        //HandleWriteLog("Start", new StackTrace(true));
    //        string str = e.Keys[0].ToString();
           
    //        AssignQuestionnaire_BAO.DeleteAssignQuestionnaire(Convert.ToInt32(str));
    //        //string javaScript =
    //        //   "<script language=JavaScript>\n" +
    //        //    " function refresh(){window.location.reload(); } " +
    //        //    "</script>";
            
    //        //RegisterStartupScript("Button1_ClickScript", javaScript); 
    //        //HandleWriteLog("Start", new StackTrace(true));
    //    }
    //    catch (Exception ex)
    //    {
    //        cBase.HandleExceptionError(ex);
    //    }
    //}

    /// <summary>
    /// Add Client side event to grid controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvAssignQuestionnaire_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkButtonEmail = (LinkButton)e.Row.Cells[7].Controls[0];
                linkButtonEmail.OnClientClick = "if (!window.confirm('Are you sure to resend email to this participant?')) return false";

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[8].Controls[0];
                linkButtonDelete.OnClientClick = "if (!window.confirm('Are you sure you want to delete this participant?')) return false";

                fillAnalysis(e);
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    /// <summary>
    /// Bind controls
    /// </summary>
    /// <param name="e"></param>
    public void fillAnalysis(GridViewRowEventArgs e)
    {
        DataTable dataTableAnalysis1 = new DataTable();

        dataTableAnalysis1 = (DataTable)Session["dtAnalysis1"];

        DropDownList dropDownListAnalysisI = (DropDownList)e.Row.FindControl("ddlAnalysisI");
        HiddenField hdnAnalysisI = (HiddenField)e.Row.FindControl("hdnAnalysisI");

        if (dropDownListAnalysisI != null)
        {

            dropDownListAnalysisI.DataSource = dataTableAnalysis1;
            dropDownListAnalysisI.DataValueField = "Category_detail";
            dropDownListAnalysisI.DataTextField = "Category_detail";
            dropDownListAnalysisI.DataBind();

            dropDownListAnalysisI.Items.Insert(0, new ListItem("Select", "0"));

            if (hdnAnalysisI.Value != "")
                dropDownListAnalysisI.SelectedValue = hdnAnalysisI.Value;
            else
                dropDownListAnalysisI.SelectedValue = "Select";
        }

        dataTableAnalysis1 = null;

        dataTableAnalysis1 = (DataTable)Session["dtAnalysis2"];
        DropDownList dropDownListAnalysisII = (DropDownList)e.Row.FindControl("ddlAnalysisII");
        HiddenField hdnAnalysisII = (HiddenField)e.Row.FindControl("hdnAnalysisII");

        if (dropDownListAnalysisII != null)
        {

            dropDownListAnalysisII.DataSource = dataTableAnalysis1;
            dropDownListAnalysisII.DataValueField = "Category_detail";
            dropDownListAnalysisII.DataTextField = "Category_detail";
            dropDownListAnalysisII.DataBind();

            dropDownListAnalysisII.Items.Insert(0, new ListItem("Select", "0"));

            if (hdnAnalysisII.Value != "")
                dropDownListAnalysisII.SelectedValue = hdnAnalysisII.Value;
            else
                dropDownListAnalysisII.SelectedValue = "Select";
        }

        dataTableAnalysis1 = null;

        dataTableAnalysis1 = dataTableAnalysis1 = (DataTable)Session["dtAnalysis3"];
        DropDownList dropDownListAnalysisIII = (DropDownList)e.Row.FindControl("ddlAnalysisIII");
        HiddenField hdnAnalysisIII = (HiddenField)e.Row.FindControl("hdnAnalysisIII");

        if (dropDownListAnalysisIII != null)
        {
            dropDownListAnalysisIII.DataSource = dataTableAnalysis1;
            dropDownListAnalysisIII.DataValueField = "Category_detail";
            dropDownListAnalysisIII.DataTextField = "Category_detail";
            dropDownListAnalysisIII.DataBind();

            dropDownListAnalysisIII.Items.Insert(0, new ListItem("Select", "0"));

            if (hdnAnalysisIII.Value != "")
                dropDownListAnalysisIII.SelectedValue = hdnAnalysisIII.Value;
            else
                dropDownListAnalysisIII.SelectedValue = "Select";
        }

        dataTableAnalysis1 = null;
    }

    #region Gridview Paging Related Methods
    /// <summary>
    /// Handle paging whengrid page index changes.
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        projectid = Request.QueryString["projectID"];
        ProgramId = Request.QueryString["ProgId"];
        userid = Request.QueryString["userid"];
        DataTable dataTableCollegue = new DataTable();
        //AssignQuestionnaireCount = assignQuestionnaire_BAO.GetAssignQuestionnaireListCount(userid, projectid);
        dataTableCollegue = assignQuestionnaireBusinessAccessObject.GetColleaguesListView(Convert.ToInt32(projectid), Convert.ToInt32(ProgramId));
        AssignQuestionnaireCount = Convert.ToInt32(dataTableCollegue.Rows.Count);
        plcPaging.Controls.Clear();

        if (AssignQuestionnaireCount > 0)
        {
            // Variable declaration
            int numberOfPages;
            int numberOfRecords = AssignQuestionnaireCount;
            int currentPage = (grdvAssignQuestionnaire.PageIndex);
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

            //plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));
            //TextBox objtxtGoto = new TextBox();
            //objtxtGoto.ID = "txtGoto";
            //objtxtGoto.ToolTip = "Enter Page No.";
            //objtxtGoto.MaxLength = 2;
            //objtxtGoto.SkinID = "grdvgoto";
            //objtxtGoto.Attributes.Add("onKeypress", "javascript:return NumberOnly(this);");
            //objtxtGoto.Text = pageNo;
            //plcPaging.Controls.Add(objtxtGoto);

            plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));

            //ImageButton objIbtnGo = new ImageButton();
            //objIbtnGo.ID = "ibtnGo";
            //objIbtnGo.ToolTip = "Goto Page";
            //objIbtnGo.ImageUrl = "~/Layouts/Resources/images/go-btn.png";
            //objIbtnGo.Click += new ImageClickEventHandler(objIbtnGo_Click);
            //plcPaging.Controls.Add(objIbtnGo);

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

            ManagePaging();
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

        grdvAssignQuestionnaire.PageIndex = (int.Parse(linkButtonNext.CommandArgument.ToString()) - 1);
        grdvAssignQuestionnaire.DataBind();

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

            grdvAssignQuestionnaire.PageIndex = Convert.ToInt32(textBoxGoto.Text.Trim()) - 1;
            grdvAssignQuestionnaire.DataBind();
            ManagePaging();

            textBoxGoto.Text = pageNo;
        }
    }
    #endregion

    /// <summary>
    /// Update Participant and Send Email
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvAssignQuestionnaire_RowCommand(Object sender, GridViewCommandEventArgs e )
    {
        // If multiple buttons are used in a GridView control, use the
        // CommandName property to determine which button was clicked.
        if (e.CommandName == "SendMail")
        {
            // Convert the row index stored in the CommandArgument
            // property to an Integer.
            string participantName = string.Empty;
            int index = Convert.ToInt32(e.CommandArgument);

            // Retrieve the row that contains the button clicked 
            // by the user from the Rows collection.
            GridViewRow row = grdvAssignQuestionnaire.Rows[index];
            
            Label lblAssignmentID = (Label)row.FindControl("AsgnDetailID");
            int AssignmentDetailsID = Convert.ToInt32(lblAssignmentID.Text);
            DropDownList dropDownListAnalysisI = (DropDownList)row.FindControl("ddlAnalysisI");
            DropDownList dropDownListAnalysisII = (DropDownList)row.FindControl("ddlAnalysisII");
            DropDownList dropDownListAnalysisIII = (DropDownList)row.FindControl("ddlAnalysisIII");
            TextBox textBoxCandidateEmail = (TextBox)row.FindControl("txtCandidateEmail");
            TextBox textBoxCandidateName = (TextBox)row.FindControl("txtCandidateName");


            UpdateParticipantsDetails(AssignmentDetailsID, dropDownListAnalysisI.SelectedValue, dropDownListAnalysisII.SelectedValue,
                dropDownListAnalysisIII.SelectedValue, textBoxCandidateEmail.Text.Trim(), textBoxCandidateName.Text.Trim());
           // Label lblAssignmentID = (Label)row.FindControl("lblAssignID");
           // Label lblAccountID = (Label)row.FindControl("lblAccountID");
           // Label lblTargetPersonID = (Label)row.FindControl("lblTargetPersonID");
           // Label lblAssignmentDetailsID = (Label)row.FindControl("lblAssignmentID");
           // Label lblProjectID = (Label)row.FindControl("lblProjectID");
            
            //int assignmentID = Convert.ToInt32(lblAssignmentID.Text);
           // int accountID = Convert.ToInt32(lblAccountID.Text);
           // int targetPersonID = Convert.ToInt32(lblTargetPersonID.Text);
           // int assignmentDetailsID = Convert.ToInt32(lblAssignmentDetailsID.Text);
            int projectID = Convert.ToInt32(projectid);

            //Send Email to Candidate
            
           // AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();
            DataTable dtResult = new DataTable();
            DataTable dtasgnmentId = new DataTable();
            int assignmentID=0;
            dtasgnmentId = assignQuestionnaireBusinessAccessObject.GetdtAssignmentId(AssignmentDetailsID);

            if (dtasgnmentId.Rows.Count > 0)
               assignmentID = Convert.ToInt32(dtasgnmentId.Rows[0]["AssignmentID"]);
            //AssignQuestionnaire_BAO assignquestionnaire_BAO = new AssignQuestionnaire_BAO();
            //dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireList(assignmentID);
            dtResult = assignQuestionnaireBusinessAccessObject.GetdtAssignQuestionnaireList(assignmentID);
            DataTable dtClone = new DataTable();
            dtClone = dtResult.Clone();

            DataRow[] result = dtResult.Select("AsgnDetailID=" + AssignmentDetailsID);

            foreach (DataRow dr in result)
                dtClone.ImportRow(dr);

            dtResult = dtClone;

            string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

            //Send mail to candidates

            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                DataTable dtAccountAdmin = new DataTable();
                //dtAccountAdmin = accountUser_BAO.GetdtAccountUserByID(accountID, targetPersonID);

                string Template = assignQuestionnaireBusinessAccessObject.FindTemplate(Convert.ToInt32(projectID));
                string Subject = assignQuestionnaireBusinessAccessObject.FindCandidateSubjectTemplate(Convert.ToInt32(projectID));
                // string Subject = assignQuestionnaire_BAO.FindCandidateSubjectTemplate(Convert.ToInt32(projectid));

                // Get Candidate Email Image Name & Will Combined with EmailImagePath
                DataTable dtCandidateEmailImage = new DataTable();
                string emailimagepath = "";

                //dtCandidateEmailImage = assignquestionnaire_BAO.GetCandidateEmailImageInfo(Convert.ToInt32(projectID));
                // if (dtCandidateEmailImage.Rows.Count > 0 && dtCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                //     emailimagepath = imagepath + dtCandidateEmailImage.Rows[0]["EmailImage"].ToString();

                string candidateEmail = "";
                string questionnaireID = "";
                string candidateID = "";
                string OrganisationName = "";
                string Startdate = "";
                string Enddate = "";
                string CandidateName = "";
                string FirstName = "";
                string emailPseudonym = string.Empty;

                candidateEmail = dtResult.Rows[i]["CandidateEmail"].ToString();
                questionnaireID = dtResult.Rows[i]["QuestionnaireID"].ToString();
                candidateID = dtResult.Rows[i]["AsgnDetailID"].ToString();
                OrganisationName = dtResult.Rows[i]["OrganisationName"].ToString();
                Startdate = Convert.ToDateTime(dtResult.Rows[i]["StartDate"]).ToString("dd-MMM-yyyy");
                Enddate = Convert.ToDateTime(dtResult.Rows[i]["Enddate"]).ToString("dd-MMM-yyyy");
                CandidateName = dtResult.Rows[i]["CandidateName"].ToString();
                emailPseudonym = dtResult.Rows[i].Field<string>("Pseudonym");
                participantName = CandidateName;

                string[] strFName = CandidateName.Split(' ');
                FirstName = strFName[0].ToString();

                questionnaireID = PasswordGenerator.EnryptString(questionnaireID);
                candidateID = PasswordGenerator.EnryptString(candidateID);

                string urlPath = ConfigurationManager.AppSettings["SurveyFeedbackURL"].ToString();

                string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                //if (dtResult.Rows[i]["RelationShip"].ToString() == "Self")
                //{
                //    string feedbackURL = urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID;
                //    assignquestionnaire_BAO.SetFeedbackURL(Convert.ToInt32(dtResult.Rows[i]["AsgnDetailID"].ToString()), Convert.ToInt32(dtResult.Rows[i]["AssignmentID"].ToString()), feedbackURL);
                //}

                Template = Template.Replace("[LINK]", link);
                Template = Template.Replace("[NAME]", CandidateName);
                Template = Template.Replace("[FIRSTNAME]", FirstName);
                Template = Template.Replace("[COMPANY]", OrganisationName);
                Template = Template.Replace("[STARTDATE]", Startdate);
                Template = Template.Replace("[CLOSEDATE]", Enddate);
                Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");

                Subject = Subject.Replace("[NAME]", CandidateName);
                Subject = Subject.Replace("[FIRSTNAME]", FirstName);
                Subject = Subject.Replace("[COMPANY]", OrganisationName);
                Subject = Subject.Replace("[STARTDATE]", Startdate);
                Subject = Subject.Replace("[CLOSEDATE]", Enddate);

                //if (dtResult.Rows[i]["RelationShip"].ToString() != "Self")
                //{
                MailAddress mailaddress = new MailAddress("admin@i-comment360.com",
                    (string.IsNullOrEmpty(emailPseudonym) ? "admin" : emailPseudonym));

                if (dtAccountAdmin.Rows.Count > 0)
                {
                    Template = Template.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                    Template = Template.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                    Subject = Subject.Replace("[PARTICIPANTNAME]", dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());
                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                    //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FirstName"].ToString() + " " + dtAccountAdmin.Rows[0]["LastName"].ToString());

                    SendEmail.Send(Subject, Template, candidateEmail, mailaddress, emailimagepath);
                }
                else
                {
                    Template = Template.Replace("[PARTICIPANTNAME]", "Participant");
                    Template = Template.Replace("[PARTICIPANTEMAIL]", "");

                    Subject = Subject.Replace("[PARTICIPANTNAME]", "Participant");
                    Subject = Subject.Replace("[PARTICIPANTEMAIL]", "");

                    SendEmail.Send(Subject, Template, candidateEmail, mailaddress, "");
                }
                //}
            }

            // Create a new ListItem object for the contact in the row.     
            ListItem item = new ListItem();
            string message = "Email sent successfully";
            lblMessage.Text = string.IsNullOrEmpty(participantName)? string.Format("{0}.",message):
                string.Format("{0}\t to {1}.", message, participantName);//+ Server.HtmlDecode(row.Cells[1].Text);
        }
        else if (e.CommandName == "Delete")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            // Retrieve the row that contains the button clicked 
            // by the user from the Rows collection.
            
            GridViewRow row = grdvAssignQuestionnaire.Rows[index];
            Label lblAssignmentID = (Label)row.FindControl("AsgnDetailID");
            int AssignmentDetailsID = Convert.ToInt32(lblAssignmentID.Text);
            
            odsAssignQuestionnaire.DeleteParameters.Clear();
            odsAssignQuestionnaire.DeleteParameters.Add("AssignmentId",AssignmentDetailsID.ToString());
            odsAssignQuestionnaire.DeleteParameters.Add("Flag", "D");
            odsAssignQuestionnaire.Delete();
        }
    }

    /// <summary>
    /// Update Analysis Type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string stringUpdateSql = "";

            foreach (GridViewRow grdvRow in grdvAssignQuestionnaire.Rows)
            {
                if (grdvRow.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hdnAnalysisI = (HiddenField)grdvRow.FindControl("hdnAnalysisI");
                    DropDownList dropDownListAnalysisI = (DropDownList)grdvRow.FindControl("ddlAnalysisI");

                    if (hdnAnalysisI.Value != dropDownListAnalysisI.SelectedValue && dropDownListAnalysisI.SelectedValue != "0")
                    {
                        stringUpdateSql = stringUpdateSql + "Analysis_I='" + dropDownListAnalysisI.SelectedValue+"'";
                    }

                    HiddenField hdnAnalysisII = (HiddenField)grdvRow.FindControl("hdnAnalysisII");
                    DropDownList dropDownListAnalysisII = (DropDownList)grdvRow.FindControl("ddlAnalysisII");

                    if (hdnAnalysisII.Value != dropDownListAnalysisII.SelectedValue && dropDownListAnalysisII.SelectedValue != "0")
                    {
                        if (stringUpdateSql == "")
                            stringUpdateSql = stringUpdateSql + "Analysis_II='" + dropDownListAnalysisII.SelectedValue+"'";
                        else
                            stringUpdateSql = stringUpdateSql + ",Analysis_II='" + dropDownListAnalysisII.SelectedValue + "'";
                    }

                    HiddenField hdnAnalysisIII = (HiddenField)grdvRow.FindControl("hdnAnalysisIII");
                    DropDownList dropDownListAnalysisIII = (DropDownList)grdvRow.FindControl("ddlAnalysisIII");

                    if (hdnAnalysisIII.Value != dropDownListAnalysisIII.SelectedValue && dropDownListAnalysisIII.SelectedValue != "0")
                    {
                        if (stringUpdateSql == "")
                            stringUpdateSql = stringUpdateSql + "Analysis_III='" + dropDownListAnalysisIII.SelectedValue + "'";
                        else
                            stringUpdateSql = stringUpdateSql + ",Analysis_III='" + dropDownListAnalysisIII.SelectedValue + "'";
                    }

                    Label labelAssignmentID = (Label)grdvRow.FindControl("AsgnDetailID");
                    int AssignmentDetailsID = Convert.ToInt32(labelAssignmentID.Text);


                    string stringCandidateName = ((TextBox)grdvRow.FindControl("txtCandidateName")).Text.Trim();

                    if (!string.IsNullOrEmpty(stringCandidateName))
                    {
                        if (stringUpdateSql == "")
                            stringUpdateSql = stringUpdateSql + "CandidateName='" + stringCandidateName.Replace("'","") + "'";
                        else
                            stringUpdateSql = stringUpdateSql + ",CandidateName='" + stringCandidateName.Replace("'", "") + "'";
                    }

                    TextBox textBoxCandidateEmail = (TextBox)grdvRow.FindControl("txtCandidateEmail");
                    string stringCandidateEmail = textBoxCandidateEmail.Text;

                    if (!string.IsNullOrEmpty(stringCandidateEmail))
                    {
                        if (stringUpdateSql == "")
                            stringUpdateSql = stringUpdateSql + "CandidateEmail='" + stringCandidateEmail.Replace("'", "") + "'";
                        else
                            stringUpdateSql = stringUpdateSql + ",CandidateEmail='" + stringCandidateEmail.Replace("'", "") + "'";
                    }


                    if (stringUpdateSql != "")
                        assignQuestionnaireBusinessAccessObject.UpdateAnalysis(AssignmentDetailsID, stringUpdateSql);

                    stringUpdateSql = "";

                }
            }

            lblMessage.Text = "Analysis changed successfully";
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    /// <summary>
    /// Update Participants Details
    /// </summary>
    /// <param name="assignmentDetailsID">assignment Details ID</param>
    /// <param name="analysisI">analysis I</param>
    /// <param name="analysisII">analysis II</param>
    /// <param name="analysisIII">analysis III</param>
    /// <param name="candidateEmail">candidate Email</param>
    /// <param name="candidateName">candidate Name</param>
    private void UpdateParticipantsDetails(int assignmentDetailsID, string analysisI, 
        string analysisII, string analysisIII, string candidateEmail, string candidateName)
    {
        string sqlQuery = string.Empty;

        if (analysisI != "0")
        {
            sqlQuery = sqlQuery + "Analysis_I='" + analysisI + "'";
        }

        if (analysisII != "0")
        {
            if (sqlQuery == "")
                sqlQuery = sqlQuery + "Analysis_II='" + analysisIII + "'";
            else
                sqlQuery = sqlQuery + ",Analysis_II='" + analysisIII + "'";
        }
        if (analysisIII != "0")
        {
            if (sqlQuery == "")
                sqlQuery = sqlQuery + "Analysis_III='" + analysisIII + "'";
            else
                sqlQuery = sqlQuery + ",Analysis_III='" + analysisIII + "'";
        }

        if (!string.IsNullOrEmpty(candidateName))
        {
            if (sqlQuery == "")
                sqlQuery = string.Format("{0}\tCandidateName='{1}'", sqlQuery, candidateName.Replace("'", ""));
            else
                sqlQuery = string.Format("{0}\t,CandidateName='{1}'", sqlQuery, candidateName.Replace("'", ""));
        }
       
      
        if (!string.IsNullOrEmpty(candidateEmail))
        {
            if (sqlQuery == "")
                sqlQuery = sqlQuery + "CandidateEmail='" + candidateEmail.Replace("'", "") + "'";
            else
                sqlQuery = sqlQuery + ",CandidateEmail='" + candidateEmail.Replace("'", "") + "'";
        }

        if (!string.IsNullOrEmpty(sqlQuery))
            assignQuestionnaireBusinessAccessObject.UpdateAnalysis(assignmentDetailsID, sqlQuery);
    }
}
