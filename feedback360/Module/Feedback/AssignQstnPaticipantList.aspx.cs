using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text;
using Miscellaneous;
using System.Net.Mail;
using DAF_BAO;
using Admin_BAO;
using Questionnaire_BAO;
using Questionnaire_BE;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;
//using Root.Reports;
using System.Data.SqlClient;

public partial class Module_Feedback_AssignQstnPaticipantList : System.Web.UI.Page
{
    #region Global Variable
    public int quote_no;    
    string LogFilePath = string.Empty;
    string mimeType;
    string encoding;
    string fileNameExtension;
    string[] streams;
    string defaultFileName = string.Empty;
    Warning[] warnings;
    #endregion

    AssignQstnParticipant_BAO AssignQuestionnaire_BAO = new AssignQstnParticipant_BAO();
    AssignQuestionnaire_BE AssignQuestionnaire_BE = new AssignQuestionnaire_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int AssignQuestionnaireCount = 0;
    string pageNo = "";
    WADIdentity identity;
    string programmeid;
    string Accountid;
    CodeBehindBase cBase = new CodeBehindBase();

    protected void Page_Load(object sender, EventArgs e)
    {

        //Label ll = (Label)this.Master.FindControl("Current_location");
       // ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            programmeid = Request.QueryString["PrgramId"];
            Accountid = Request.QueryString["AccountID"];
            
            //programmeid = "137";
            ViewState["AccountID"] = Accountid;
            Session["Count"] = 0;

            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            odsAssignQstnParticipant.SelectParameters.Clear();
            odsAssignQstnParticipant.SelectParameters.Add("accountID", Accountid);
            odsAssignQstnParticipant.SelectParameters.Add("programmeID", programmeid);
            odsAssignQstnParticipant.Select();

            AssignQstnParticipant_BAO assignQstnParticipant_BAO=new AssignQstnParticipant_BAO();
            DataTable dtDetails = assignQstnParticipant_BAO.GetdtAssignPartiList(Accountid,programmeid);

            if (dtDetails.Rows.Count > 0)
            {
                lblAccountCode.Text = dtDetails.Rows[0]["Code"].ToString();
                lblProgrammeName.Text = dtDetails.Rows[0]["ProgrammeName"].ToString();
                lblProjectName.Text = dtDetails.Rows[0]["Title"].ToString();
            }

            grdvAssignQuestionnaire.PageSize = pageSize;
            ManagePaging();

            TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
            if (txtGoto != null)
                txtGoto.Text = pageNo;

            if (!IsPostBack)
            {
                //ddlSeqQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                //ddlSeqQuestionnaire.DataValueField = "QuestionnaireID";
                //ddlSeqQuestionnaire.DataTextField = "QSTNName";
                //ddlSeqQuestionnaire.DataBind();
            }
            
            //HandleWriteLog("Start", new StackTrace(true));
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
            //HandleWriteLog("Start", new StackTrace(true));

            ManagePaging();

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void grdvAssignQuestionnaire_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibtnEmail = (LinkButton)e.Row.Cells[7].Controls[0];
                ibtnEmail.OnClientClick = "if (!window.confirm('Are you sure to resend email to this participant?')) return false";

                LinkButton ibtnDelete = (LinkButton)e.Row.Cells[8].Controls[0];
                ibtnDelete.OnClientClick = "if (!window.confirm('Are you sure you want to delete this participant?')) return false";

                Label lblTargetPersonID = (Label)e.Row.FindControl("lblTargetPersonID");
                
                Label lblCandidateCount = (Label)e.Row.FindControl("lblCandidateCount");
                Label lblSubmissionCount = (Label)e.Row.FindControl("lblSubmissionCount");
                Label lblSelfAssessment = (Label)e.Row.FindControl("lblSelfAssessment");

                int targetPersonID = Convert.ToInt32(lblTargetPersonID.Text);

                lblCandidateCount.Text = AssignQuestionnaire_BAO.GetCandidatesCount(targetPersonID).ToString();
                lblSubmissionCount.Text = AssignQuestionnaire_BAO.GetSubmissionCount(targetPersonID).ToString();

                int selfAssessmentFlag = AssignQuestionnaire_BAO.GetSelfAssessment(targetPersonID);
                
                if (selfAssessmentFlag == 0)
                    lblSelfAssessment.Text = "No";
                else
                    lblSelfAssessment.Text = "Yes";

            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void grdvAssignQuestionnaire_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        // If multiple buttons are used in a GridView control, use the
        // CommandName property to determine which button was clicked.
        if (e.CommandName == "SendMail")
        {
            // Convert the row index stored in the CommandArgument
            // property to an Integer.
            int index = Convert.ToInt32(e.CommandArgument);

            // Retrieve the row that contains the button clicked 
            // by the user from the Rows collection.
            GridViewRow row = grdvAssignQuestionnaire.Rows[index];

            Label lblAssignmentID = (Label)row.FindControl("lblAssignID");
            Label lblAccountID = (Label)row.FindControl("lblAccountID");
            Label lblTargetPersonID = (Label)row.FindControl("lblTargetPersonID");
            Label lblProgrammeID = (Label)row.FindControl("lblProgrammeID");
            Label lblProjectID = (Label)row.FindControl("lblProjectID");

			TextBox txtEmail = (TextBox)row.FindControl("txtEmail");

            int assignmentID = Convert.ToInt32(lblAssignmentID.Text);
            int accountID = Convert.ToInt32(lblAccountID.Text);
            int targetPersonID = Convert.ToInt32(lblTargetPersonID.Text);
            int programmeID = Convert.ToInt32(lblProgrammeID.Text);
            int projectID = Convert.ToInt32(lblProjectID.Text);

            //Send Email to Candidate

            AssignQuestionnaire_BAO assignquestionnaireTemplete_BAO = new AssignQuestionnaire_BAO();
            AssignQstnParticipant_BAO assignquestionnaire_BAO = new AssignQstnParticipant_BAO();

			SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
			
			SqlCommand cmd = new SqlCommand("Update [User] Set EmailID='"+txtEmail.Text+"' WHERE UserId=" + targetPersonID ,con);
			con.Open();
			cmd.ExecuteScalar();
			con.Close();



            DataTable dtResult = new DataTable();
            dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireList(assignmentID);

            DataTable dtClone = new DataTable();
            dtClone = dtResult.Clone();
		
			
			

            DataRow[] result = dtResult.Select("UserID=" + targetPersonID);

            foreach (DataRow dr in result)
                dtClone.ImportRow(dr);

            dtResult = dtClone;

            string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

            //Send mail to candidates

            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                Programme_BAO programme_BAO = new Programme_BAO();
                List<Programme_BE> lstProgramme = new List<Programme_BE>();
                lstProgramme = programme_BAO.GetProgrammeByID(Convert.ToInt32(accountID), Convert.ToInt32(programmeID));

                AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                DataTable dtAccountAdmin = new DataTable();
                dtAccountAdmin = accountUser_BAO.GetdtAccountAdmin(Convert.ToInt32(accountID));

                string Template = assignquestionnaireTemplete_BAO.FindParticipantTemplate(Convert.ToInt32(projectID));
                string Subject = assignquestionnaireTemplete_BAO.FindParticipantSubjectTemplate(Convert.ToInt32(projectID));

                // Get Candidate Email Image Name & Will Combined with EmailImagePath
                DataTable dtCandidateEmailImage = new DataTable();
                string emailimagepath = "";
                dtCandidateEmailImage = assignquestionnaireTemplete_BAO.GetParticipantEmailImageInfo(Convert.ToInt32(projectID));
                if (dtCandidateEmailImage.Rows.Count > 0 && dtCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                    emailimagepath = imagepath + dtCandidateEmailImage.Rows[0]["EmailImage"].ToString();

                string Title = "";
                string EmailID = "";
                string FirstName = "";
                string Loginid = "";
                string password = "";
                string Accountcode = "";

                Title = dtResult.Rows[i]["Title"].ToString();
                EmailID = dtResult.Rows[i]["EmailID"].ToString();
                FirstName = dtResult.Rows[i]["FirstName"].ToString();
                Loginid = dtResult.Rows[i]["LoginID"].ToString();
                password = dtResult.Rows[i]["Password"].ToString();
                Accountcode = dtResult.Rows[i]["Code"].ToString();

                string urlPath = ConfigurationManager.AppSettings["ParticipantURL"].ToString();

                /*Change History 
                     * Author : Rudra Prakash Mishra
                     * Date : 02/06/2014
                     * Reason: To bypass login screen for the participants
                     */

                urlPath += "Login.aspx?" + Utilities.CreateEncryptedQueryString(Loginid, password, Accountcode);

                string link = "<a Target='_BLANK' href= '" + urlPath + "' >Click Link</a> ";

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

                if (lstProgramme.Count > 0)
                {
                    Template = Template.Replace("[STARTDATE]", Convert.ToDateTime(lstProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                    Template = Template.Replace("[CLOSEDATE]", Convert.ToDateTime(lstProgramme[0].EndDate).ToString("dd-MMM-yyyy"));

                    Subject = Subject.Replace("[STARTDATE]", Convert.ToDateTime(lstProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                    Subject = Subject.Replace("[CLOSEDATE]", Convert.ToDateTime(lstProgramme[0].EndDate).ToString("dd-MMM-yyyy"));
                }

                if (dtAccountAdmin.Rows.Count > 0)
                {
                    Template = Template.Replace("[ACCOUNTADMIN]", dtAccountAdmin.Rows[0]["FullName"].ToString());
                    Template = Template.Replace("[ADMINEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                    Subject = Subject.Replace("[ACCOUNTADMIN]", dtAccountAdmin.Rows[0]["FullName"].ToString());
                    Subject = Subject.Replace("[ADMINEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                    //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FullName"].ToString());
                    MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");
                    SendEmail.Send(Subject, Template, EmailID, maddr, emailimagepath);
                }
                else
                {
                    Template = Template.Replace("[ACCOUNTADMIN]", "Account Admin");
                    Template = Template.Replace("[ADMINEMAIL]", "");
                    Template = Template.Replace("<img src=cid:companylogo>","");

                    Subject = Subject.Replace("[ACCOUNTADMIN]", "Account Admin");
                    Subject = Subject.Replace("[ADMINEMAIL]", "");

                    SendEmail.Send(Subject, Template, EmailID, "");
                }

            }

            lblMessage.Text = "Email sent successfully to " + dtResult.Rows[0]["FirstName"].ToString();
        }
        else if (e.CommandName.ToLower().Trim() == "selfassessment")
        {
            int index = Convert.ToInt32(e.CommandArgument);

            // Retrieve the row that contains the button clicked 
            // by the user from the Rows collection.
            GridViewRow row = grdvAssignQuestionnaire.Rows[index];

            Label lblTargetPersonID = (Label)row.FindControl("lblTargetPersonID");
            int targetPersonID = Convert.ToInt32(lblTargetPersonID.Text);

            AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
            DataTable dtResult = new DataTable();

            dtResult = assignQuestionnaire_BAO.GetFeedbackURL(targetPersonID);

            if (dtResult.Rows.Count > 0)
            {
                string url = dtResult.Rows[0]["FeedbackUrl"].ToString();

                if (string.IsNullOrEmpty(url))
                {
                    int assignmentID = assignQuestionnaire_BAO.GetAssignmentID(targetPersonID);
                    string urlPath = ConfigurationManager.AppSettings["FeedbackURL"].ToString();

                    DataTable dt = new DataTable();
                    dt = assignQuestionnaire_BAO.GetdtAssignQuestionnaireListDetails(assignmentID.ToString());

                    string questionnaireID = dt.Rows[0]["QuestionnaireID"].ToString();
                    string candidateID = dt.Rows[0]["AsgnDetailID"].ToString();

                    string path = ConfigurationManager.AppSettings["FeedbackURL"].ToString();
                    string feedbackURL = urlPath + "Feedback.aspx?QID=" + PasswordGenerator.EnryptString(questionnaireID) + "&CID=" + PasswordGenerator.EnryptString(dt.Rows[0]["AsgnDetailID"].ToString());
                    assignQuestionnaire_BAO.SetFeedbackURL(Convert.ToInt32(dt.Rows[0]["AsgnDetailID"].ToString()), Convert.ToInt32(dt.Rows[0]["AssignmentID"].ToString()), feedbackURL);

                    url = feedbackURL;
                }

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + url + "','_blank')", true);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "popup", "window.open('" + url + "','_blank')", true);
            }
        }
    }

    

    #region Gridview Paging Related Methods

    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;

        programmeid = Request.QueryString["PrgramId"];
        Accountid = Request.QueryString["AccountID"];

        AssignQuestionnaireCount = AssignQuestionnaire_BAO.GetAssignPartiQuestionnaireListCount(Accountid, programmeid);

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

            //plcPaging.Controls.Add(new LiteralControl("</td><td valign=middle>"));

            //ImageButton objIbtnGo = new ImageButton();
            //objIbtnGo.ID = "ibtnGo";
            //objIbtnGo.ToolTip = "Goto Page";
            //objIbtnGo.ImageUrl = "~/Layouts/Resources/images/go-btn.png";
            //objIbtnGo.Click += new ImageClickEventHandler(objIbtnGo_Click);
            //plcPaging.Controls.Add(objIbtnGo);

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

            ManagePaging();
        }

    }

    protected void objLb_Click(object sender, EventArgs e)
    {
        plcPaging.Controls.Clear();
        LinkButton objlb = (LinkButton)sender;

        grdvAssignQuestionnaire.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
        grdvAssignQuestionnaire.DataBind();

        ManagePaging();

    }

    protected void objIbtnGo_Click(object sender, ImageClickEventArgs e)
    {
        TextBox txtGoto = (TextBox)plcPaging.FindControl("txtGoto");
        if (txtGoto.Text.Trim() != "")
        {
            pageNo = txtGoto.Text;
            plcPaging.Controls.Clear();

            grdvAssignQuestionnaire.PageIndex = Convert.ToInt32(txtGoto.Text.Trim()) - 1;
            grdvAssignQuestionnaire.DataBind();
            ManagePaging();

            txtGoto.Text = pageNo;
        }
    }

    #endregion
    
    protected void imbExportData_Click(object sender, ImageClickEventArgs e)
    {
        ExportDataTableToExcel();
    }

    protected void ExportDataTableToExcel()
    {
        try
        {
            programmeid = Request.QueryString["PrgramId"];
            Accountid = Request.QueryString["AccountID"];

            ViewState["AccountID"] = Accountid;
            Session["Count"] = 0;
            
            string CandidatesCount = string.Empty;
            string SubmissionCount = string.Empty;
            string SelfAssessment = string.Empty;

            DataTable dtExcelData = new DataTable();
            dtExcelData.Columns.Add("FirstName");
            dtExcelData.Columns.Add("LastName");
            dtExcelData.Columns.Add("Email");
            dtExcelData.Columns.Add("Candidates'Count");
            dtExcelData.Columns.Add("SubmissionCount");
            dtExcelData.Columns.Add("SelfAssessment");
            dtExcelData.Columns.Add("AccountCode");
            dtExcelData.Columns.Add("Project");
            dtExcelData.Columns.Add("Programme");

            DataTable dtGrid = AssignQuestionnaire_BAO.GetdtAssignPartiList(Accountid, programmeid);
            int count = dtGrid.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                DataRow dtExcelDataRow = dtExcelData.NewRow();
                int targetPersonID = Convert.ToInt32(dtGrid.Rows[i]["AssignmentID"].ToString());
                CandidatesCount = AssignQuestionnaire_BAO.GetCandidatesCount(targetPersonID).ToString();
                SubmissionCount = AssignQuestionnaire_BAO.GetSubmissionCount(targetPersonID).ToString();
                SelfAssessment = AssignQuestionnaire_BAO.GetSelfAssessment(targetPersonID).ToString();

                dtExcelDataRow["FirstName"] = dtGrid.Rows[i]["FirstName"].ToString();
                dtExcelDataRow["LastName"] = dtGrid.Rows[i]["LastName"].ToString();
                dtExcelDataRow["Email"] = dtGrid.Rows[i]["EmailID"].ToString();
                dtExcelDataRow["Candidates'Count"] = CandidatesCount;
                dtExcelDataRow["SubmissionCount"] = SubmissionCount;
                dtExcelDataRow["SelfAssessment"] = SelfAssessment;
                dtExcelDataRow["AccountCode"] = dtGrid.Rows[i]["Code"].ToString();
                dtExcelDataRow["Project"] = dtGrid.Rows[i]["Title"].ToString();
                dtExcelDataRow["Programme"] = dtGrid.Rows[i]["ProgrammeName"].ToString();

                dtExcelData.Rows.Add(dtExcelDataRow);
            }

            rview.LocalReport.ReportPath = Server.MapPath("~\\UploadDocs\\") + "Participantlist.rdlc";
            rview.Width = Unit.Pixel(1);
            rview.Height = Unit.Pixel(1);
            rview.Visible = false;

            ReportDataSource rdsDetails = new ReportDataSource("DSParticipant_dtParticipantList", dtExcelData);

            rview.LocalReport.DataSources.Clear();
            rview.LocalReport.DataSources.Add(rdsDetails);
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = rview.LocalReport.Render("EXCEL", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            Response.Clear();
            Response.AppendHeader("Content-Type", "application/octet-stream");
            Response.AppendHeader("Content-disposition", "attachment; filename=ParticipantList.xls");
            Response.BinaryWrite(bytes);
       
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
        
    }

}
