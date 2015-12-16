using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.IO;
using Admin_BAO;

public partial class Survey_Module_Questionnaire_Company : CodeBehindBase
{
    //Global variable.
    Survey_Company_BAO companyBusinessObject = new Survey_Company_BAO();
    //Survey_Company_BE company_BE = new Survey_Company_BE();

    WADIdentity identity;

    string filename;
    string file = null;
    DataTable dataTableCompanyName;
    //DataTable dtAllAccount;
    //string expression1;
    //string Finalexpression;
    //string expression2;
    //string Finalexpression2;

    protected void Page_Load(object sender, EventArgs e)
    {
        identity = this.Page.User.Identity as WADIdentity;

        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        try
        {
            if (!IsPostBack)
            {
                List<Survey_Company_BE> companyBusinessEntityList = new List<Survey_Company_BE>();
                int companyID = Convert.ToInt32(Request.QueryString["CompId"]);

                if (companyID > 0)
                {
                    //Get all company list in an account.
                    companyBusinessEntityList = companyBusinessObject.GetCompanyByID(companyID);
                    fillAccountCode();

                    if (companyBusinessEntityList.Any())
                    {
                        //Bind finish email template by account id.
                        fillEmailTemplate(Convert.ToString(companyBusinessEntityList.FirstOrDefault().AccountID));
                        //Bind project dropdown by account id.
                        fillProject(Convert.ToString(companyBusinessEntityList.FirstOrDefault().AccountID));
                        //Bind manager dropdown by account id.
                        fillProjectManager(Convert.ToString(companyBusinessEntityList.FirstOrDefault().AccountID));
                        //Bind controls with company details.
                        fillCompanyDetails(companyBusinessEntityList.FirstOrDefault());
                    }
                }
                else
                {
                    fillAccountCode();
                    fillEmailTemplate(Convert.ToString(identity.User.AccountID));
                    fillProject(Convert.ToString(identity.User.AccountID));
                    fillProjectManager(Convert.ToString(identity.User.AccountID));
                }
                //IF query string contains mode="E" then edit mode 
                if (Request.QueryString["Mode"] == "E")
                {
                    imbSave.Visible = true;
                    imbcancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Company";
                }
                else if (Request.QueryString["Mode"] == "R") //IF query string contains mode="R" then view mode 
                {
                    imbSave.Visible = false;
                    imbcancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Company";
                }
                //If user is a Super Admin then show account detail section else hide.
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
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// To fill controls in case of edit
    /// </summary>
    /// <param name="companyBusinessEntity"></param>
    private void fillCompanyDetails(Survey_Company_BE companyBusinessEntity)
    {
        try
        {
            if (companyBusinessEntity.AccountID != null)
                ddlAccountCode.SelectedValue = Convert.ToString(companyBusinessEntity.AccountID);

            txtcompanyname.Text = companyBusinessEntity.CompanyName;

            if (companyBusinessEntity.ProjectID != null)
                ddlProject.SelectedValue = Convert.ToString(companyBusinessEntity.ProjectID);

            txtTitle.Text = companyBusinessEntity.Title;

            if (companyBusinessEntity.ManagerID != null)
                ddlProjectManager.SelectedValue = Convert.ToString(companyBusinessEntity.ManagerID);

            txtDescription.Text = companyBusinessEntity.Description;
            txtFaqText.Value = Server.HtmlDecode(companyBusinessEntity.FaqText);

            if (companyBusinessEntity.StatusID != null)
                ddlStatus.SelectedValue = Convert.ToString(companyBusinessEntity.StatusID);

            Finish_emailID_Txtbox.Text = companyBusinessEntity.Finish_EmailID;

            Finish_Email_Chkbox.Checked = companyBusinessEntity.Finish_EmailID_Chkbox ?? false;

            if (companyBusinessEntity.EmailTMPLStart != null)
                ddlEmailStart.SelectedValue = Convert.ToString(companyBusinessEntity.EmailTMPLStart);

            if (companyBusinessEntity.EmailTMPLReminder1 != null)
                ddlEmailRemainder1.SelectedValue = Convert.ToString(companyBusinessEntity.EmailTMPLReminder1);

            if (companyBusinessEntity.EmailTMPLReminder2 != null)
                ddlEmailRemainder2.SelectedValue = Convert.ToString(companyBusinessEntity.EmailTMPLReminder2);

            if (companyBusinessEntity.EmailTMPLReminder3 != null)
                ddlEmailRemainder3.SelectedValue = Convert.ToString(companyBusinessEntity.EmailTMPLReminder3);

            if (companyBusinessEntity.EmailFinishEmailTemplate != null)
                ddlEmailTemplate.SelectedValue = Convert.ToString(companyBusinessEntity.EmailFinishEmailTemplate);

            hdnQuestimage.Value = companyBusinessEntity.QuestLogo;
            hdnReportimage.Value = companyBusinessEntity.ReportLogo;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Insert and update company details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        Page.Validate();

        if (Page.IsValid)
        {
            try
            {
                //Initilize propeties.
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Company_BAO companyBusinessObject = new Survey_Company_BAO();
                Survey_Company_BE companyBusinessEntity = new Survey_Company_BE();

                if (ddlAccountCode.SelectedValue != null)
                    companyBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

                companyBusinessEntity.CompanyName = txtcompanyname.Text;

                if (ddlProject.SelectedValue != null)
                    companyBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

                companyBusinessEntity.Title = txtTitle.Text;

                if (ddlProjectManager.SelectedValue != null)
                    companyBusinessEntity.ManagerID = Convert.ToInt32(ddlProjectManager.SelectedValue);

                companyBusinessEntity.Description = txtDescription.Text;

                if (ddlStatus.SelectedValue != null)
                    companyBusinessEntity.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                companyBusinessEntity.Finish_EmailID = Finish_emailID_Txtbox.Text;
                companyBusinessEntity.FaqText = Server.HtmlDecode(txtFaqText.Value.Trim());

                companyBusinessEntity.Finish_EmailID_Chkbox = Finish_Email_Chkbox.Checked;

                if (ddlEmailStart.SelectedValue != null)
                    companyBusinessEntity.EmailTMPLStart = Convert.ToInt32(ddlEmailStart.SelectedValue);

                if (ddlEmailRemainder1.SelectedValue != null)
                    companyBusinessEntity.EmailTMPLReminder1 = Convert.ToInt32(ddlEmailRemainder1.SelectedValue);

                if (ddlEmailRemainder2.SelectedValue != null)
                    companyBusinessEntity.EmailTMPLReminder2 = Convert.ToInt32(ddlEmailRemainder2.SelectedValue);

                if (ddlEmailRemainder3.SelectedValue != null)
                    companyBusinessEntity.EmailTMPLReminder3 = Convert.ToInt32(ddlEmailRemainder3.SelectedValue);

                if (ddlEmailTemplate.SelectedValue != null)
                    companyBusinessEntity.EmailFinishEmailTemplate = Convert.ToInt32(ddlEmailTemplate.SelectedValue);

                //If company logo is uploaded.
                if (qstFileUpload.HasFile)
                {
                    filename = System.IO.Path.GetFileName(qstFileUpload.PostedFile.FileName);
                    //filename = FileUpload.FileName;
                    file = GetUniqueFilename(filename);//Get unique name ,for file.
                    //Get file path.
                    string path = MapPath("~\\UploadDocs\\") + file;
                    qstFileUpload.SaveAs(path);
                    string name = file;

                    FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                    BinaryReader br1 = new BinaryReader(fs1);
                    Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                    br1.Close();
                    fs1.Close();
                    companyBusinessEntity.QuestLogo = file;
                }
                else
                {
                    companyBusinessEntity.QuestLogo = hdnQuestimage.Value;
                }
                // uopload Report logo
                if (reportFileUpload.HasFile)
                {
                    filename = System.IO.Path.GetFileName(reportFileUpload.PostedFile.FileName);
                    //filename = FileUpload.FileName;
                    file = GetUniqueFilename(filename);//Get unique name ,for file.
                    //Get file path.
                    string path = MapPath("~\\UploadDocs\\") + file;
                    reportFileUpload.SaveAs(path);

                    string name = file;
                    FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                    BinaryReader br1 = new BinaryReader(fs1);
                    Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                    br1.Close();
                    fs1.Close();
                    companyBusinessEntity.ReportLogo = file;
                }
                else
                {
                    companyBusinessEntity.ReportLogo = hdnReportimage.Value;
                }

                companyBusinessEntity.ModifyBy = 1;
                companyBusinessEntity.ModifyDate = DateTime.Now;
                companyBusinessEntity.IsActive = 1;

                //IF query string Mode contains "E" then update else insert.
                if (Request.QueryString["Mode"] == "E")
                {
                    companyBusinessEntity.CompanyID = Convert.ToInt32(Request.QueryString["CompId"]);
                    //company_BAO.UpdateProject(company_BE);
                }

                companyBusinessObject.AddCompany(companyBusinessEntity);

                Response.Redirect("CompanyList.aspx", false);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    /// <summary>
    /// Redirect to previous page 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("CompanyList.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Check whether uploaded company logo is vaid or not.
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
    /// Unique name for uploaded file.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniquefilename;
    }

    /// <summary>
    /// Bind project and comapny name on account selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Survey_AccountUser_BAO accountUserBusinessObject = new Survey_AccountUser_BAO();
            Survey_EmailTemplate_BAO emailTemplateBusinessObject = new Survey_EmailTemplate_BAO();
            //Reset controls value to default.
            ddlProjectManager.Items.Clear();
            ddlProjectManager.Items.Insert(0, new ListItem("Select", "0"));


            ddlEmailStart.Items.Clear();
            ddlEmailStart.Items.Insert(0, new ListItem("Select", "0"));

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlEmailRemainder1.Items.Clear();
            ddlEmailRemainder1.Items.Insert(0, new ListItem("Select", "0"));

            ddlEmailRemainder2.Items.Clear();
            ddlEmailRemainder2.Items.Insert(0, new ListItem("Select", "0"));

            ddlEmailRemainder3.Items.Clear();
            ddlEmailRemainder3.Items.Insert(0, new ListItem("Select", "0"));

            ddlEmailTemplate.Items.Clear();
            ddlEmailTemplate.Items.Insert(0, new ListItem("Select", "0"));

            if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
            {
                Account_BAO accountBusinessObject = new Account_BAO();

                dataTableCompanyName = accountBusinessObject.GetdtAccountList(ddlAccountCode.SelectedValue);
                DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
                DataTable dtAccount = dataTableCompanyName.Clone();

                foreach (DataRow drAccount in resultsAccount)
                    dtAccount.ImportRow(drAccount);
                //set company name.
                txtcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
                //Bind project drop down.
                fillProject(ddlAccountCode.SelectedValue);
                //Bind manager drop down.
                fillProjectManager(ddlAccountCode.SelectedValue);
                //Bind finish email template drop down.
                fillEmailTemplate(ddlAccountCode.SelectedValue);
                //bind FAQ text.
                ReBindFAQContent();
            }
            else
            {
                txtcompanyname.Text = "";
                //Bind project drop down.
                fillProject(Convert.ToString(identity.User.AccountID));
                //Bind manager drop down.
                fillProjectManager(Convert.ToString(identity.User.AccountID));
                //Bind finish email template drop down.
                fillEmailTemplate(Convert.ToString(identity.User.AccountID));
            }
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// REdirect to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CompanyList.aspx", false);
    }

    #region private functions
    /// <summary>
    /// Bind account drop down by user account id.
    /// </summary>
    private void fillAccountCode()
    {
        Account_BAO accountBusinessObject = new Account_BAO();
        ddlAccountCode.DataSource = accountBusinessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
        ddlAccountCode.DataValueField = "AccountID";
        ddlAccountCode.DataTextField = "Code";
        ddlAccountCode.DataBind();
    }

    /// <summary>
    /// Bind project drop down by user account id.
    /// </summary>
    private void fillProject(string accountId)
    {
        Survey_Project_BAO projectBusinessObject = new Survey_Project_BAO();
        ddlProject.DataSource = projectBusinessObject.GetdtProjectList(accountId);
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();
    }

    /// <summary>
    /// Bind program manager drop down by user account id.
    /// </summary>
    private void fillProjectManager(string accountId)
    {
        Survey_AccountUser_BAO accountUserBusinessObject = new Survey_AccountUser_BAO();
        ddlProjectManager.DataSource = accountUserBusinessObject.GetdtAccountUserList(accountId);
        ddlProjectManager.DataValueField = "UserID";
        ddlProjectManager.DataTextField = "UserName";
        ddlProjectManager.DataBind();
    }

    /// <summary>
    /// Bind finish email template drop down by user account id.
    /// </summary>
    private void fillEmailTemplate(string accountId)
    {
        Survey_EmailTemplate_BAO emailTemplateBusinessObject = new Survey_EmailTemplate_BAO();
        DataTable dataTableEmailTemplate = emailTemplateBusinessObject.GetdtEmailTemplateList(accountId);

        DataRow[] resultsTemplate = dataTableEmailTemplate.Select("Title LIKE '%Invitation Template%'");

        DataTable dataTablEMailTemplateClone = dataTableEmailTemplate.Clone();

        foreach (DataRow dataRowMail in resultsTemplate)
        {
            dataTablEMailTemplateClone.ImportRow(dataRowMail);
        }

        int emailId = 0;

        if (dataTablEMailTemplateClone.Rows.Count > 0)
            emailId = Convert.ToInt32(dataTablEMailTemplateClone.Rows[0]["EmailTemplateID"]);

        //Bind participant email template template.
        ddlEmailStart.DataSource = dataTableEmailTemplate;
        ddlEmailStart.DataValueField = "EmailTemplateID";
        ddlEmailStart.DataTextField = "Title";
        ddlEmailStart.DataBind();

        if (emailId != null || emailId != 0)
        {
            ddlEmailStart.SelectedValue = Convert.ToString(emailId);
        }
        //Bind reminder 1 template.
        ddlEmailRemainder1.DataSource = dataTableEmailTemplate;
        ddlEmailRemainder1.DataValueField = "EmailTemplateID";
        ddlEmailRemainder1.DataTextField = "Title";
        ddlEmailRemainder1.DataBind();
        //Bind reminder 2 template.
        ddlEmailRemainder2.DataSource = dataTableEmailTemplate;
        ddlEmailRemainder2.DataValueField = "EmailTemplateID";
        ddlEmailRemainder2.DataTextField = "Title";
        ddlEmailRemainder2.DataBind();
        //Bind reminder 3 template.
        ddlEmailRemainder3.DataSource = dataTableEmailTemplate;
        ddlEmailRemainder3.DataValueField = "EmailTemplateID";
        ddlEmailRemainder3.DataTextField = "Title";
        ddlEmailRemainder3.DataBind();
        //Bind finish email  template.
        ddlEmailTemplate.DataSource = dataTableEmailTemplate;
        ddlEmailTemplate.DataValueField = "EmailTemplateID";
        ddlEmailTemplate.DataTextField = "Title";
        ddlEmailTemplate.DataBind();
    }

    /// <summary>
    /// Rebind FAQ text.
    /// </summary>
    private void ReBindFAQContent()
    {
        txtFaqText.InnerHtml = Server.HtmlDecode(txtFaqText.InnerHtml);
    }
    #endregion private functions
}
