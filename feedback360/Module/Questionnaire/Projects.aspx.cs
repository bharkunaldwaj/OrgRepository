using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.IO;
using System.Collections;
using Admin_BAO;

public partial class Module_Questionnaire_Projects : CodeBehindBase
{
    Project_BAO project_BAO = new Project_BAO();
    Project_BE project_BE = new Project_BE();
    List<Project_BE> project_BEList = new List<Project_BE>();
    WADIdentity identity;

    string filename;
    string file = null;
    DataTable dtCompanyName;
    DataTable dtAllAccount;
    string    expression1;
    string Finalexpression;
    string expression2;
    string Finalexpression2;

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                

                int projectID = Convert.ToInt32(Request.QueryString["PrjId"]);
                project_BEList = project_BAO.GetProjectByID(Convert.ToInt32(identity.User.AccountID), projectID);

                AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                ddlProjectManager.DataSource = accountUser_BAO.GetdtAccountUserList(identity.User.AccountID.ToString());
                ddlProjectManager.DataValueField = "UserID";
                ddlProjectManager.DataTextField = "UserName";
                ddlProjectManager.DataBind();

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
                ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                if (Request.QueryString["Mode"] == "E")
                {
                    imbSave.Visible = true;
                    imbcancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Project";
                }
                else if (Request.QueryString["Mode"] == "R")
                {
                    imbSave.Visible = false;
                    imbcancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Project";
                }

                if (identity.User.GroupID == 1)
                {
                    divAccount.Visible = true;
                    if (Request.QueryString["Mode"] == null)
                    {
                        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                        ddlAccountCode_SelectedIndexChanged(sender, e);
                    }
                }
                else
                {
                    divAccount.Visible = false;
                }

                EmailTemplate_BAO emailTemplate_BAO = new EmailTemplate_BAO();
                DataTable dtEmailTemplate = emailTemplate_BAO.GetdtEmailTemplateList(Convert.ToString(identity.User.AccountID));

                DataRow[] resultsTemplate = dtEmailTemplate.Select("Title LIKE '%Invitation Template%'");

                DataTable dtmailtemp = dtEmailTemplate.Clone();

                foreach (DataRow drMail in resultsTemplate)
                {
                    dtmailtemp.ImportRow(drMail);
                }

                int emailId=0;
                if (dtmailtemp.Rows.Count > 0)
                     emailId= Convert.ToInt32(dtmailtemp.Rows[0]["EmailTemplateID"]);
                
                ddlEmailStart.DataSource = dtEmailTemplate;
                ddlEmailStart.DataValueField = "EmailTemplateID";
                ddlEmailStart.DataTextField = "Title";
                ddlEmailStart.DataBind();

                if (emailId != null || emailId != 0)
                {
                    ddlEmailStart.SelectedValue = Convert.ToString(emailId);
                    //ddlEmailStart.Enabled = false;
                }

                ddlEmailAvailable.DataSource = dtEmailTemplate;
                ddlEmailAvailable.DataValueField = "EmailTemplateID";
                ddlEmailAvailable.DataTextField = "Title";
                ddlEmailAvailable.DataBind();

                ddlEmailRemainder1.DataSource = dtEmailTemplate;
                ddlEmailRemainder1.DataValueField = "EmailTemplateID";
                ddlEmailRemainder1.DataTextField = "Title";
                ddlEmailRemainder1.DataBind();

                ddlEmailRemainder2.DataSource = dtEmailTemplate;
                ddlEmailRemainder2.DataValueField = "EmailTemplateID";
                ddlEmailRemainder2.DataTextField = "Title";
                ddlEmailRemainder2.DataBind();

                ddlEmailRemainder3.DataSource = dtEmailTemplate;
                ddlEmailRemainder3.DataValueField = "EmailTemplateID";
                ddlEmailRemainder3.DataTextField = "Title";
                ddlEmailRemainder3.DataBind();

                ddlEmailParticipant.DataSource = dtEmailTemplate;
                ddlEmailParticipant.DataValueField = "EmailTemplateID";
                ddlEmailParticipant.DataTextField = "Title";
                ddlEmailParticipant.DataBind();

                ddlParticipantRem1.DataSource = dtEmailTemplate;
                ddlParticipantRem1.DataValueField = "EmailTemplateID";
                ddlParticipantRem1.DataTextField = "Title";
                ddlParticipantRem1.DataBind();

                ddlParticipantRem2.DataSource = dtEmailTemplate;
                ddlParticipantRem2.DataValueField = "EmailTemplateID";
                ddlParticipantRem2.DataTextField = "Title";
                ddlParticipantRem2.DataBind();

                ddlEmailManager.DataSource = dtEmailTemplate;
                ddlEmailManager.DataValueField = "EmailTemplateID";
                ddlEmailManager.DataTextField = "Title";
                ddlEmailManager.DataBind();

                ddlSelfAssessmentRem.DataSource = dtEmailTemplate;
                ddlSelfAssessmentRem.DataValueField = "EmailTemplateID";
                ddlSelfAssessmentRem.DataTextField = "Title";
                ddlSelfAssessmentRem.DataBind();
                
            }

            if (project_BEList.Count > 0)
            {
                SetProjectValue(project_BEList);
                
                ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
                ddlAccountCode_SelectedIndexChanged(sender, e);
            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void SetProjectValue(List<Project_BE> project_BEList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = project_BEList[0].AccountID.ToString();


                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {

                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO account1_BAO = new Account_BAO();

                    dtCompanyName = account1_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = dtCompanyName.Select(Finalexpression);

                    DataTable dtAccount = dtCompanyName.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                    {
                        dtAccount.ImportRow(drAccount);
                    }

                   

                    lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }


            }

            ddlStatus.SelectedValue = project_BEList[0].StatusID.ToString();
            txtReference.Text = project_BEList[0].Reference;
            txtTitle.Text = project_BEList[0].Title;
            ddlProjectManager.SelectedValue = project_BEList[0].ManagerID.ToString();
            //ddlMaxCandidate.SelectedValue = project_BEList[0].MaxCandidate.ToString();
            txtDescription.Text = project_BEList[0].Description;
            //txtPassowrd.Text = project_BEList[0].Password;
            hdnPassword.Value = project_BEList[0].Password;

            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();

            ddlQuestionnaire.SelectedValue = project_BEList[0].QuestionnaireID.ToString();

            //dtStartDate.Text =  Convert.ToDateTime(project_BEList[0].StartDate).ToString("dd/MM/yyyy");
            //dtEndDate.Text = Convert.ToDateTime(project_BEList[0].EndDate).ToString("dd/MM/yyyy");
            //dtRemainderDate1.Text = Convert.ToDateTime(project_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");
            //dtRemainderDate2.Text = Convert.ToDateTime(project_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
            //dtRemainderDate3.Text = Convert.ToDateTime(project_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
            //dtAvailableFrom.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //dtAvailableTo.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");
            hdnimage.Value = project_BEList[0].Logo;

            //txtStartDate.Text = Convert.ToDateTime(project_BEList[0].StartDate).ToString("dd/MM/yyyy");
            //txtEndDate.Text = Convert.ToDateTime(project_BEList[0].EndDate).ToString("dd/MM/yyyy");
            //txtRemainderDate1.Text = Convert.ToDateTime(project_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");
            //txtRemainderDate2.Text = Convert.ToDateTime(project_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
            //txtRemainderDate3.Text = Convert.ToDateTime(project_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
            //txtAvailableFrom.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //txtAvailableTo.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");

            ddlEmailStart.SelectedValue = project_BEList[0].EmailTMPLStart.ToString();
            ddlEmailRemainder1.SelectedValue = project_BEList[0].EmailTMPLReminder1.ToString();
            ddlEmailRemainder2.SelectedValue = project_BEList[0].EmailTMPLReminder2.ToString();
            ddlEmailRemainder3.SelectedValue = project_BEList[0].EmailTMPLReminder3.ToString();
            ddlEmailAvailable.SelectedValue = project_BEList[0].EmailTMPLReportAvalibale.ToString();
            ddlEmailParticipant.SelectedValue = project_BEList[0].EmailTMPLParticipant.ToString();
            ddlParticipantRem1.SelectedValue = project_BEList[0].EmailTMPPartReminder1.ToString();
            ddlParticipantRem2.SelectedValue = project_BEList[0].EmailTMPPartReminder2.ToString();
            ddlEmailManager.SelectedValue = project_BEList[0].EmailTMPManager.ToString();
            ddlSelfAssessmentRem.SelectedValue = project_BEList[0].EmailTMPSelfReminder.ToString();

            txtRelationship1.Text = project_BEList[0].Relationship1.ToString();
            txtRelationship2.Text = project_BEList[0].Relationship2.ToString();
            txtRelationship3.Text = project_BEList[0].Relationship3.ToString();
            txtRelationship4.Text = project_BEList[0].Relationship4.ToString();
            txtRelationship5.Text = project_BEList[0].Relationship5.ToString();

            txtFaqText.InnerText = Server.HtmlDecode(project_BEList[0].FaqText.ToString());

            Session["FileName"] = project_BEList[0].Logo;

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "setimage", "SetImage();", true);
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

    }

    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Project_BE project_BE = new Project_BE();
            Project_BAO project_BAO = new Project_BAO();

            //if (this.IsFileValid(this.FileUpload))
            //{

                identity = this.Page.User.Identity as WADIdentity;

                if (identity.User.GroupID == 1)
                {

                    project_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

                }
                else
                {
                    project_BE.AccountID = identity.User.AccountID;
                }

                project_BE.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                project_BE.Reference = txtReference.Text;
                project_BE.Title = GetString(txtTitle.Text);
                project_BE.ManagerID = Convert.ToInt32(ddlProjectManager.SelectedValue);
                project_BE.MaxCandidate = Convert.ToInt32(100);
                project_BE.Description = txtDescription.Text;
                //project_BE.Password = txtPassowrd.Text;
                project_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
                project_BE.EmailTMPLStart = Convert.ToInt32(ddlEmailStart.SelectedValue);
                project_BE.EmailTMPLReminder1 = Convert.ToInt32(ddlEmailRemainder1.SelectedValue);
                project_BE.EmailTMPLReminder2 = Convert.ToInt32(ddlEmailRemainder2.SelectedValue);
                project_BE.EmailTMPLReminder3 = Convert.ToInt32(ddlEmailRemainder3.SelectedValue);
                project_BE.EndDate = Convert.ToDateTime("01/01/2000");
                project_BE.StartDate = Convert.ToDateTime("01/01/2000");
                project_BE.ReportAvaliableFrom = Convert.ToDateTime("01/01/2000");
                project_BE.ReportAvaliableTo = Convert.ToDateTime("01/01/2000");
                project_BE.Reminder1Date = Convert.ToDateTime("01/01/2000");
                project_BE.Reminder2Date = Convert.ToDateTime("01/01/2000");
                project_BE.Reminder3Date = Convert.ToDateTime("01/01/2000");
                project_BE.EmailTMPLReportAvalibale = Convert.ToInt32(ddlEmailAvailable.SelectedValue);
                project_BE.EmailTMPLParticipant = Convert.ToInt32(ddlEmailParticipant.SelectedValue);
                project_BE.EmailTMPPartReminder1 = Convert.ToInt32(ddlParticipantRem1.SelectedValue);
                project_BE.EmailTMPPartReminder2 = Convert.ToInt32(ddlParticipantRem2.SelectedValue);
                project_BE.EmailTMPManager = Convert.ToInt32(ddlEmailManager.SelectedValue);
                project_BE.EmailTMPSelfReminder = Convert.ToInt32(ddlSelfAssessmentRem.SelectedValue);
                project_BE.Relationship1 = txtRelationship1.Text.Trim();
                project_BE.Relationship2 = txtRelationship2.Text.Trim();
                project_BE.Relationship3= txtRelationship3.Text.Trim();
                project_BE.Relationship4 = txtRelationship4.Text.Trim();
                project_BE.Relationship5 = txtRelationship5.Text.Trim();
                project_BE.FaqText = Server.HtmlDecode(txtFaqText.Value.Trim());
                project_BE.Logo = "";

                //if (FileUpload.HasFile)
                //{
                //    filename = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);
                //    //filename = FileUpload.FileName;
                //    file = GetUniqueFilename(filename);

                //    string path = MapPath("~\\UploadDocs\\") + file;
                //    FileUpload.SaveAs(path);
                //    string name = file;
                //    FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                //    BinaryReader br1 = new BinaryReader(fs1);
                //    Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                //    br1.Close();
                //    fs1.Close();
                //    project_BE.Logo = file;
                //}
                //else
                //{
                //    if (Request.QueryString["Mode"] == "E" && FileUpload.FileName == "")
                //        project_BE.Logo = Convert.ToString(Session["FileName"]);
                //    else
                //        project_BE.Logo = "";
                //}

                project_BE.ModifyBy = 1;
                project_BE.ModifyDate = DateTime.Now;
                project_BE.IsActive = 1;

                if (Request.QueryString["Mode"] == "E")
                {
                    project_BE.ProjectID = Convert.ToInt32(Request.QueryString["PrjId"]);
                    project_BAO.UpdateProject(project_BE);
                }
                else
                {
                    project_BAO.AddProject(project_BE);
                }

                Response.Redirect("ProjectList.aspx", false);
                //HandleWriteLog("Start", new StackTrace(true));
            //}
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
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("ProjectList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected bool IsFileValid(FileUpload uploadControl)
    {
        bool isFileOk = true;

        string[] AllowedExtensions = ConfigurationManager.AppSettings["Fileextension"].Split(',');
        bool isExtensionError = false;
        int MaxSizeAllowed = 5 * 1048576;// Size Allow only in mb
        if (uploadControl.HasFile)
        {
            bool isSizeError = false;
            // Validate for size less than MaxSizeAllowed...
            if (uploadControl.PostedFile.ContentLength > MaxSizeAllowed)
            {
                isFileOk = false;
                isSizeError = true;
            }
            else
            {
                isFileOk = true;
                isSizeError = false;
            }

            // If OK so far, validate the file extension...
            if (isFileOk)
            {
                isFileOk = false;
                isExtensionError = true;

                // Get the file's extension...
                string fileExtension = System.IO.Path.GetExtension(uploadControl.PostedFile.FileName).ToLower();

                for (int i = 0; i < AllowedExtensions.Length; i++)
                {
                    if (fileExtension.Trim() == AllowedExtensions[i].Trim())
                    {
                        isFileOk = true;
                        isExtensionError = false;

                        break;
                    }
                }
            }

            if (isExtensionError)
            {
                string errorMessage = "Invalid file type";

            }
            if (isSizeError)
            {
                string errorMessage = "Maximum Size of the File exceeded";

            }
        }
        return isFileOk;



    }

    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniquefilename;
    }

    //private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    //{
    //    ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "purchasedate", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
    //}

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
        EmailTemplate_BAO emailTemplate_BAO = new EmailTemplate_BAO();

        ddlProjectManager.Items.Clear();
        ddlProjectManager.Items.Insert(0, new ListItem("Select", "0"));

        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailStart.Items.Clear();
        ddlEmailStart.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailAvailable.Items.Clear();
        ddlEmailAvailable.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailRemainder1.Items.Clear();
        ddlEmailRemainder1.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailRemainder2.Items.Clear();
        ddlEmailRemainder2.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailRemainder3.Items.Clear();
        ddlEmailRemainder3.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailParticipant.Items.Clear();
        ddlEmailParticipant.Items.Insert(0, new ListItem("Select", "0"));

        ddlParticipantRem1.Items.Clear();
        ddlParticipantRem1.Items.Insert(0, new ListItem("Select", "0"));

        ddlParticipantRem2.Items.Clear();
        ddlParticipantRem2.Items.Insert(0, new ListItem("Select", "0"));

        ddlEmailManager.Items.Clear();
        ddlEmailManager.Items.Insert(0, new ListItem("Select", "0"));

        ddlSelfAssessmentRem.Items.Clear();
        ddlSelfAssessmentRem.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO account_BAO = new Account_BAO();

            dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dtCompanyName.Clone();
            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            ddlProjectManager.DataSource = accountUser_BAO.GetdtAccountUserList(ddlAccountCode.SelectedValue);
            ddlProjectManager.DataValueField = "UserID";
            ddlProjectManager.DataTextField = "UserName";
            ddlProjectManager.DataBind();

            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();

            DataTable dtEmailTemplate = emailTemplate_BAO.GetdtEmailTemplateList(ddlAccountCode.SelectedValue);

            if (dtEmailTemplate.Rows.Count > 0)
            {
                ddlEmailStart.DataSource = dtEmailTemplate;
                ddlEmailStart.DataValueField = "EmailTemplateID";
                ddlEmailStart.DataTextField = "Title";
                ddlEmailStart.DataBind();

                ddlEmailAvailable.DataSource = dtEmailTemplate;
                ddlEmailAvailable.DataValueField = "EmailTemplateID";
                ddlEmailAvailable.DataTextField = "Title";
                ddlEmailAvailable.DataBind();

                ddlEmailRemainder1.DataSource = dtEmailTemplate;
                ddlEmailRemainder1.DataValueField = "EmailTemplateID";
                ddlEmailRemainder1.DataTextField = "Title";
                ddlEmailRemainder1.DataBind();

                ddlEmailRemainder2.DataSource = dtEmailTemplate;
                ddlEmailRemainder2.DataValueField = "EmailTemplateID";
                ddlEmailRemainder2.DataTextField = "Title";
                ddlEmailRemainder2.DataBind();

                ddlEmailRemainder3.DataSource = dtEmailTemplate;
                ddlEmailRemainder3.DataValueField = "EmailTemplateID";
                ddlEmailRemainder3.DataTextField = "Title";
                ddlEmailRemainder3.DataBind();

                ddlEmailParticipant.DataSource = dtEmailTemplate;
                ddlEmailParticipant.DataValueField = "EmailTemplateID";
                ddlEmailParticipant.DataTextField = "Title";
                ddlEmailParticipant.DataBind();

                ddlParticipantRem1.DataSource = dtEmailTemplate;
                ddlParticipantRem1.DataValueField = "EmailTemplateID";
                ddlParticipantRem1.DataTextField = "Title";
                ddlParticipantRem1.DataBind();

                ddlParticipantRem2.DataSource = dtEmailTemplate;
                ddlParticipantRem2.DataValueField = "EmailTemplateID";
                ddlParticipantRem2.DataTextField = "Title";
                ddlParticipantRem2.DataBind();

                ddlEmailManager.DataSource = dtEmailTemplate;
                ddlEmailManager.DataValueField = "EmailTemplateID";
                ddlEmailManager.DataTextField = "Title";
                ddlEmailManager.DataBind();

                ddlSelfAssessmentRem.DataSource = dtEmailTemplate;
                ddlSelfAssessmentRem.DataValueField = "EmailTemplateID";
                ddlSelfAssessmentRem.DataTextField = "Title";
                ddlSelfAssessmentRem.DataBind();
                
            }

            txtFaqText.InnerHtml = Server.HtmlDecode(txtFaqText.InnerHtml);
        }
        else
        {
            lblcompanyname.Text = "";

            ddlProjectManager.DataSource = accountUser_BAO.GetdtAccountUserList(identity.User.AccountID.ToString());
            ddlProjectManager.DataValueField = "UserID";
            ddlProjectManager.DataTextField = "UserName";
            ddlProjectManager.DataBind();

            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(identity.User.AccountID.ToString());
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();

            DataTable dtEmailTemplate = emailTemplate_BAO.GetdtEmailTemplateList(Convert.ToString(identity.User.AccountID));

            if (dtEmailTemplate.Rows.Count > 0)
            {
                ddlEmailStart.DataSource = dtEmailTemplate;
                ddlEmailStart.DataValueField = "EmailTemplateID";
                ddlEmailStart.DataTextField = "Title";
                ddlEmailStart.DataBind();

                ddlEmailAvailable.DataSource = dtEmailTemplate;
                ddlEmailAvailable.DataValueField = "EmailTemplateID";
                ddlEmailAvailable.DataTextField = "Title";
                ddlEmailAvailable.DataBind();

                ddlEmailRemainder1.DataSource = dtEmailTemplate;
                ddlEmailRemainder1.DataValueField = "EmailTemplateID";
                ddlEmailRemainder1.DataTextField = "Title";
                ddlEmailRemainder1.DataBind();

                ddlEmailRemainder2.DataSource = dtEmailTemplate;
                ddlEmailRemainder2.DataValueField = "EmailTemplateID";
                ddlEmailRemainder2.DataTextField = "Title";
                ddlEmailRemainder2.DataBind();

                ddlEmailRemainder3.DataSource = dtEmailTemplate;
                ddlEmailRemainder3.DataValueField = "EmailTemplateID";
                ddlEmailRemainder3.DataTextField = "Title";
                ddlEmailRemainder3.DataBind();

                ddlEmailParticipant.DataSource = dtEmailTemplate;
                ddlEmailParticipant.DataValueField = "EmailTemplateID";
                ddlEmailParticipant.DataTextField = "Title";
                ddlEmailParticipant.DataBind();

                ddlParticipantRem1.DataSource = dtEmailTemplate;
                ddlParticipantRem1.DataValueField = "EmailTemplateID";
                ddlParticipantRem1.DataTextField = "Title";
                ddlParticipantRem1.DataBind();

                ddlParticipantRem2.DataSource = dtEmailTemplate;
                ddlParticipantRem2.DataValueField = "EmailTemplateID";
                ddlParticipantRem2.DataTextField = "Title";
                ddlParticipantRem2.DataBind();

                ddlEmailManager.DataSource = dtEmailTemplate;
                ddlEmailManager.DataValueField = "EmailTemplateID";
                ddlEmailManager.DataTextField = "Title";
                ddlEmailManager.DataBind();

                ddlSelfAssessmentRem.DataSource = dtEmailTemplate;
                ddlSelfAssessmentRem.DataValueField = "EmailTemplateID";
                ddlSelfAssessmentRem.DataTextField = "Title";
                ddlSelfAssessmentRem.DataBind();
            }

            txtFaqText.InnerHtml = string.Empty;
        }
    }

    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProjectList.aspx", false);
    }

    
}
