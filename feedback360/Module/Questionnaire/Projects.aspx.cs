using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.IO;
using Admin_BAO;

public partial class Module_Questionnaire_Projects : CodeBehindBase
{
    //Global variables
    Project_BAO projectBusinessAccessObject = new Project_BAO();
    Project_BE projectBusinessEntity = new Project_BE();
    List<Project_BE> projectBusinessEntityList = new List<Project_BE>();
    WADIdentity identity;
    //Define global variable.
    string filename;
    string file = null;
    DataTable dataTableCompanyName;
    DataTable dataTableAllAccount;
    string expression1;
    string Finalexpression;
    string expression2;
    string Finalexpression2;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                int projectID = Convert.ToInt32(Request.QueryString["PrjId"]);
                //Get all project list in an user account.
                projectBusinessEntityList = projectBusinessAccessObject.GetProjectByID(Convert.ToInt32(identity.User.AccountID), projectID);

                AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
                //Get Account user List by user account id and Bind Manager Dropdown.
                ddlProjectManager.DataSource = accountUserBusinessAccessObject.GetdtAccountUserList(identity.User.AccountID.ToString());
                ddlProjectManager.DataValueField = "UserID";
                ddlProjectManager.DataTextField = "UserName";
                ddlProjectManager.DataBind();

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get Account user List by user account id and Bind Account Dropdown.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
                //Get Account user List by user account id and Bind Questionnaire Dropdown.
                ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                //If QuesyString Contains Mode "E" then Edit and if "R" then View , and hide show controls accordingly.
                if (Request.QueryString["Mode"] == "E")
                {
                    imbSave.Visible = true;
                    imbcancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Project";
                }
                else if (Request.QueryString["Mode"] == "R")//view Mode 
                {
                    imbSave.Visible = false;
                    imbcancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Project";
                }

                //If User is Super Admin  then show account details section else hide.
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

                EmailTemplate_BAO emailTemplateBusinessAccessObject = new EmailTemplate_BAO();
                //Get EmailTempalte List by user Id and Bind dropdown controls.
                DataTable dataTableEmailTemplate = emailTemplateBusinessAccessObject.GetdtEmailTemplateList(Convert.ToString(identity.User.AccountID));

                DataRow[] resultsTemplate = dataTableEmailTemplate.Select("Title LIKE '%Invitation Template%'");

                DataTable copyMailTemplate = dataTableEmailTemplate.Clone();

                foreach (DataRow rowMail in resultsTemplate)
                {
                    copyMailTemplate.ImportRow(rowMail);
                }

                int emailId = 0;

                if (copyMailTemplate.Rows.Count > 0)
                    emailId = Convert.ToInt32(copyMailTemplate.Rows[0]["EmailTemplateID"]);
                
                ddlEmailStart.DataSource = dataTableEmailTemplate;
                ddlEmailStart.DataValueField = "EmailTemplateID";
                ddlEmailStart.DataTextField = "Title";
                ddlEmailStart.DataBind();

                if (emailId != null || emailId != 0)
                {
                    ddlEmailStart.SelectedValue = Convert.ToString(emailId);
                    //ddlEmailStart.Enabled = false;
                }

                ddlEmailAvailable.DataSource = dataTableEmailTemplate;
                ddlEmailAvailable.DataValueField = "EmailTemplateID";
                ddlEmailAvailable.DataTextField = "Title";
                ddlEmailAvailable.DataBind();

                ddlEmailRemainder1.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder1.DataValueField = "EmailTemplateID";
                ddlEmailRemainder1.DataTextField = "Title";
                ddlEmailRemainder1.DataBind();

                ddlEmailRemainder2.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder2.DataValueField = "EmailTemplateID";
                ddlEmailRemainder2.DataTextField = "Title";
                ddlEmailRemainder2.DataBind();

                ddlEmailRemainder3.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder3.DataValueField = "EmailTemplateID";
                ddlEmailRemainder3.DataTextField = "Title";
                ddlEmailRemainder3.DataBind();

                ddlEmailParticipant.DataSource = dataTableEmailTemplate;
                ddlEmailParticipant.DataValueField = "EmailTemplateID";
                ddlEmailParticipant.DataTextField = "Title";
                ddlEmailParticipant.DataBind();

                ddlParticipantRem1.DataSource = dataTableEmailTemplate;
                ddlParticipantRem1.DataValueField = "EmailTemplateID";
                ddlParticipantRem1.DataTextField = "Title";
                ddlParticipantRem1.DataBind();

                ddlParticipantRem2.DataSource = dataTableEmailTemplate;
                ddlParticipantRem2.DataValueField = "EmailTemplateID";
                ddlParticipantRem2.DataTextField = "Title";
                ddlParticipantRem2.DataBind();

                ddlEmailManager.DataSource = dataTableEmailTemplate;
                ddlEmailManager.DataValueField = "EmailTemplateID";
                ddlEmailManager.DataTextField = "Title";
                ddlEmailManager.DataBind();

                ddlSelfAssessmentRem.DataSource = dataTableEmailTemplate;
                ddlSelfAssessmentRem.DataValueField = "EmailTemplateID";
                ddlSelfAssessmentRem.DataTextField = "Title";
                ddlSelfAssessmentRem.DataBind();
            }

            if (projectBusinessEntityList.Count > 0)
            {
                //Bind Project controls by project list details.
                SetProjectValue(projectBusinessEntityList);
                
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

    /// <summary>
    /// Set Project controls value.
    /// </summary>
    /// <param name="projectBusinessEntityList"></param>
    private void SetProjectValue(List<Project_BE> projectBusinessEntityList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = projectBusinessEntityList[0].AccountID.ToString();

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO accountBusinessAccessObject = new Account_BAO();

                    dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = dataTableCompanyName.Select(Finalexpression);

                    DataTable dataTableAccount = dataTableCompanyName.Clone();

                    foreach (DataRow datarowAccount in resultsAccount)
                    {
                        dataTableAccount.ImportRow(datarowAccount);
                    }

                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }

            ddlStatus.SelectedValue = projectBusinessEntityList[0].StatusID.ToString();
            txtReference.Text = projectBusinessEntityList[0].Reference;
            txtTitle.Text = projectBusinessEntityList[0].Title;
            ddlProjectManager.SelectedValue = projectBusinessEntityList[0].ManagerID.ToString();
            //ddlMaxCandidate.SelectedValue = project_BEList[0].MaxCandidate.ToString();
            txtDescription.Text = projectBusinessEntityList[0].Description;
            //txtPassowrd.Text = project_BEList[0].Password;
            hdnPassword.Value = projectBusinessEntityList[0].Password;

            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
            //Get Questionnaire List
            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();

            ddlQuestionnaire.SelectedValue = projectBusinessEntityList[0].QuestionnaireID.ToString();

            //dtStartDate.Text =  Convert.ToDateTime(project_BEList[0].StartDate).ToString("dd/MM/yyyy");
            //dtEndDate.Text = Convert.ToDateTime(project_BEList[0].EndDate).ToString("dd/MM/yyyy");
            //dtRemainderDate1.Text = Convert.ToDateTime(project_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");
            //dtRemainderDate2.Text = Convert.ToDateTime(project_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
            //dtRemainderDate3.Text = Convert.ToDateTime(project_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
            //dtAvailableFrom.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //dtAvailableTo.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");
            hdnimage.Value = projectBusinessEntityList[0].Logo;

            //txtStartDate.Text = Convert.ToDateTime(project_BEList[0].StartDate).ToString("dd/MM/yyyy");
            //txtEndDate.Text = Convert.ToDateTime(project_BEList[0].EndDate).ToString("dd/MM/yyyy");
            //txtRemainderDate1.Text = Convert.ToDateTime(project_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");
            //txtRemainderDate2.Text = Convert.ToDateTime(project_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
            //txtRemainderDate3.Text = Convert.ToDateTime(project_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
            //txtAvailableFrom.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //txtAvailableTo.Text = Convert.ToDateTime(project_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");

            ddlEmailStart.SelectedValue = projectBusinessEntityList[0].EmailTMPLStart.ToString();
            ddlEmailRemainder1.SelectedValue = projectBusinessEntityList[0].EmailTMPLReminder1.ToString();
            ddlEmailRemainder2.SelectedValue = projectBusinessEntityList[0].EmailTMPLReminder2.ToString();
            ddlEmailRemainder3.SelectedValue = projectBusinessEntityList[0].EmailTMPLReminder3.ToString();
            ddlEmailAvailable.SelectedValue = projectBusinessEntityList[0].EmailTMPLReportAvalibale.ToString();
            ddlEmailParticipant.SelectedValue = projectBusinessEntityList[0].EmailTMPLParticipant.ToString();
            ddlParticipantRem1.SelectedValue = projectBusinessEntityList[0].EmailTMPPartReminder1.ToString();
            ddlParticipantRem2.SelectedValue = projectBusinessEntityList[0].EmailTMPPartReminder2.ToString();
            ddlEmailManager.SelectedValue = projectBusinessEntityList[0].EmailTMPManager.ToString();
            ddlSelfAssessmentRem.SelectedValue = projectBusinessEntityList[0].EmailTMPSelfReminder.ToString();

            txtRelationship1.Text = projectBusinessEntityList[0].Relationship1.ToString();
            txtRelationship2.Text = projectBusinessEntityList[0].Relationship2.ToString();
            txtRelationship3.Text = projectBusinessEntityList[0].Relationship3.ToString();
            txtRelationship4.Text = projectBusinessEntityList[0].Relationship4.ToString();
            txtRelationship5.Text = projectBusinessEntityList[0].Relationship5.ToString();

            txtFaqText.InnerText = Server.HtmlDecode(projectBusinessEntityList[0].FaqText.ToString());

            Session["FileName"] = projectBusinessEntityList[0].Logo;

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "setimage", "SetImage();", true);
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Insert and update project details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Project_BE projectBusinessEntity = new Project_BE();
            Project_BAO projectBusinessAccessObject = new Project_BAO();

            //if (this.IsFileValid(this.FileUpload))
            //{
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                projectBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                projectBusinessEntity.AccountID = identity.User.AccountID;
            }
            //Initilize properties.
            projectBusinessEntity.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            projectBusinessEntity.Reference = txtReference.Text;
            projectBusinessEntity.Title = GetString(txtTitle.Text);
            projectBusinessEntity.ManagerID = Convert.ToInt32(ddlProjectManager.SelectedValue);
            projectBusinessEntity.MaxCandidate = Convert.ToInt32(100);
            projectBusinessEntity.Description = txtDescription.Text;
            //project_BE.Password = txtPassowrd.Text;
            projectBusinessEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            projectBusinessEntity.EmailTMPLStart = Convert.ToInt32(ddlEmailStart.SelectedValue);
            projectBusinessEntity.EmailTMPLReminder1 = Convert.ToInt32(ddlEmailRemainder1.SelectedValue);
            projectBusinessEntity.EmailTMPLReminder2 = Convert.ToInt32(ddlEmailRemainder2.SelectedValue);
            projectBusinessEntity.EmailTMPLReminder3 = Convert.ToInt32(ddlEmailRemainder3.SelectedValue);
            projectBusinessEntity.EndDate = Convert.ToDateTime("01/01/2000");
            projectBusinessEntity.StartDate = Convert.ToDateTime("01/01/2000");
            projectBusinessEntity.ReportAvaliableFrom = Convert.ToDateTime("01/01/2000");
            projectBusinessEntity.ReportAvaliableTo = Convert.ToDateTime("01/01/2000");
            projectBusinessEntity.Reminder1Date = Convert.ToDateTime("01/01/2000");
            projectBusinessEntity.Reminder2Date = Convert.ToDateTime("01/01/2000");
            projectBusinessEntity.Reminder3Date = Convert.ToDateTime("01/01/2000");
            projectBusinessEntity.EmailTMPLReportAvalibale = Convert.ToInt32(ddlEmailAvailable.SelectedValue);
            projectBusinessEntity.EmailTMPLParticipant = Convert.ToInt32(ddlEmailParticipant.SelectedValue);
            projectBusinessEntity.EmailTMPPartReminder1 = Convert.ToInt32(ddlParticipantRem1.SelectedValue);
            projectBusinessEntity.EmailTMPPartReminder2 = Convert.ToInt32(ddlParticipantRem2.SelectedValue);
            projectBusinessEntity.EmailTMPManager = Convert.ToInt32(ddlEmailManager.SelectedValue);
            projectBusinessEntity.EmailTMPSelfReminder = Convert.ToInt32(ddlSelfAssessmentRem.SelectedValue);
            projectBusinessEntity.Relationship1 = txtRelationship1.Text.Trim();
            projectBusinessEntity.Relationship2 = txtRelationship2.Text.Trim();
            projectBusinessEntity.Relationship3 = txtRelationship3.Text.Trim();
            projectBusinessEntity.Relationship4 = txtRelationship4.Text.Trim();
            projectBusinessEntity.Relationship5 = txtRelationship5.Text.Trim();
            projectBusinessEntity.FaqText = Server.HtmlDecode(txtFaqText.Value.Trim());
            projectBusinessEntity.Logo = "";

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

            projectBusinessEntity.ModifyBy = 1;
            projectBusinessEntity.ModifyDate = DateTime.Now;
            projectBusinessEntity.IsActive = 1;

            //If QuesyString Contains Mode "E" then Update else Insert  Project value.
            if (Request.QueryString["Mode"] == "E")
            {
                projectBusinessEntity.ProjectID = Convert.ToInt32(Request.QueryString["PrjId"]);
                projectBusinessAccessObject.UpdateProject(projectBusinessEntity);
            }
            else
            {
                projectBusinessAccessObject.AddProject(projectBusinessEntity);
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

    /// <summary>
    /// Redirect to Project List page when click on cancel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// No use
    /// </summary>
    /// <param name="uploadControl"></param>
    /// <returns></returns>
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

    /// <summary>
    /// No use
    /// </summary>
    /// <param name="uploadControl"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Bind project manager and questionnaire by account.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
        EmailTemplate_BAO emailTemplateBusinessAccessObject = new EmailTemplate_BAO();

        //set default value to all dropdowns controls.
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
            Account_BAO accountBusinessAccessObject = new Account_BAO();

            //Get Company Details by Account Id.
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);
            //Get company name.
            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable accountDataTable = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                accountDataTable.ImportRow(dataRowAccount);

            //Bind Company name.
            lblcompanyname.Text = accountDataTable.Rows[0]["OrganisationName"].ToString();

            // Get account user details and bind Manager.
            ddlProjectManager.DataSource = accountUserBusinessAccessObject.GetdtAccountUserList(ddlAccountCode.SelectedValue);
            ddlProjectManager.DataValueField = "UserID";
            ddlProjectManager.DataTextField = "UserName";
            ddlProjectManager.DataBind();

            // Get Questionnaire List details and bind Questionnaire.
            Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();

            //Get Template list by Account Id.
            DataTable dataTableEmailTemplate = emailTemplateBusinessAccessObject.GetdtEmailTemplateList(ddlAccountCode.SelectedValue);

           if (dataTableEmailTemplate.Rows.Count > 0)
            {
                //Bind Email Template Dropdowns.
                ddlEmailStart.DataSource = dataTableEmailTemplate;
                ddlEmailStart.DataValueField = "EmailTemplateID";
                ddlEmailStart.DataTextField = "Title";
                ddlEmailStart.DataBind();

                ddlEmailAvailable.DataSource = dataTableEmailTemplate;
                ddlEmailAvailable.DataValueField = "EmailTemplateID";
                ddlEmailAvailable.DataTextField = "Title";
                ddlEmailAvailable.DataBind();

                ddlEmailRemainder1.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder1.DataValueField = "EmailTemplateID";
                ddlEmailRemainder1.DataTextField = "Title";
                ddlEmailRemainder1.DataBind();

                ddlEmailRemainder2.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder2.DataValueField = "EmailTemplateID";
                ddlEmailRemainder2.DataTextField = "Title";
                ddlEmailRemainder2.DataBind();

                ddlEmailRemainder3.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder3.DataValueField = "EmailTemplateID";
                ddlEmailRemainder3.DataTextField = "Title";
                ddlEmailRemainder3.DataBind();

                ddlEmailParticipant.DataSource = dataTableEmailTemplate;
                ddlEmailParticipant.DataValueField = "EmailTemplateID";
                ddlEmailParticipant.DataTextField = "Title";
                ddlEmailParticipant.DataBind();

                ddlParticipantRem1.DataSource = dataTableEmailTemplate;
                ddlParticipantRem1.DataValueField = "EmailTemplateID";
                ddlParticipantRem1.DataTextField = "Title";
                ddlParticipantRem1.DataBind();

                ddlParticipantRem2.DataSource = dataTableEmailTemplate;
                ddlParticipantRem2.DataValueField = "EmailTemplateID";
                ddlParticipantRem2.DataTextField = "Title";
                ddlParticipantRem2.DataBind();

                ddlEmailManager.DataSource = dataTableEmailTemplate;
                ddlEmailManager.DataValueField = "EmailTemplateID";
                ddlEmailManager.DataTextField = "Title";
                ddlEmailManager.DataBind();

                ddlSelfAssessmentRem.DataSource = dataTableEmailTemplate;
                ddlSelfAssessmentRem.DataValueField = "EmailTemplateID";
                ddlSelfAssessmentRem.DataTextField = "Title";
                ddlSelfAssessmentRem.DataBind();
                
            }

            txtFaqText.InnerHtml = Server.HtmlDecode(txtFaqText.InnerHtml);
        }
        else
        {
            lblcompanyname.Text = "";

            // Get account user details and bind Manager.
            ddlProjectManager.DataSource = accountUserBusinessAccessObject.GetdtAccountUserList(identity.User.AccountID.ToString());
            ddlProjectManager.DataValueField = "UserID";
            ddlProjectManager.DataTextField = "UserName";
            ddlProjectManager.DataBind();

            // Get Questionnaire List details and bind Questionnaire.
            Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(identity.User.AccountID.ToString());
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();
            //Get Email tempalte List
            DataTable dataTableEmailTemplate = emailTemplateBusinessAccessObject.GetdtEmailTemplateList(Convert.ToString(identity.User.AccountID));

            if (dataTableEmailTemplate.Rows.Count > 0)
            {
                //Bind Email Template Dropdowns.
                ddlEmailStart.DataSource = dataTableEmailTemplate;
                ddlEmailStart.DataValueField = "EmailTemplateID";
                ddlEmailStart.DataTextField = "Title";
                ddlEmailStart.DataBind();

                ddlEmailAvailable.DataSource = dataTableEmailTemplate;
                ddlEmailAvailable.DataValueField = "EmailTemplateID";
                ddlEmailAvailable.DataTextField = "Title";
                ddlEmailAvailable.DataBind();

                ddlEmailRemainder1.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder1.DataValueField = "EmailTemplateID";
                ddlEmailRemainder1.DataTextField = "Title";
                ddlEmailRemainder1.DataBind();

                ddlEmailRemainder2.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder2.DataValueField = "EmailTemplateID";
                ddlEmailRemainder2.DataTextField = "Title";
                ddlEmailRemainder2.DataBind();

                ddlEmailRemainder3.DataSource = dataTableEmailTemplate;
                ddlEmailRemainder3.DataValueField = "EmailTemplateID";
                ddlEmailRemainder3.DataTextField = "Title";
                ddlEmailRemainder3.DataBind();

                ddlEmailParticipant.DataSource = dataTableEmailTemplate;
                ddlEmailParticipant.DataValueField = "EmailTemplateID";
                ddlEmailParticipant.DataTextField = "Title";
                ddlEmailParticipant.DataBind();

                ddlParticipantRem1.DataSource = dataTableEmailTemplate;
                ddlParticipantRem1.DataValueField = "EmailTemplateID";
                ddlParticipantRem1.DataTextField = "Title";
                ddlParticipantRem1.DataBind();

                ddlParticipantRem2.DataSource = dataTableEmailTemplate;
                ddlParticipantRem2.DataValueField = "EmailTemplateID";
                ddlParticipantRem2.DataTextField = "Title";
                ddlParticipantRem2.DataBind();

                ddlEmailManager.DataSource = dataTableEmailTemplate;
                ddlEmailManager.DataValueField = "EmailTemplateID";
                ddlEmailManager.DataTextField = "Title";
                ddlEmailManager.DataBind();

                ddlSelfAssessmentRem.DataSource = dataTableEmailTemplate;
                ddlSelfAssessmentRem.DataValueField = "EmailTemplateID";
                ddlSelfAssessmentRem.DataTextField = "Title";
                ddlSelfAssessmentRem.DataBind();
            }

            txtFaqText.InnerHtml = string.Empty;
        }
    }

    /// <summary>
    /// Redirect to Project List page when click on back.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProjectList.aspx", false);
    }
}
