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

public partial class Survey_Module_Questionnaire_Company : CodeBehindBase
{
    Survey_Company_BAO company_BAO = new Survey_Company_BAO();
    //Survey_Company_BE company_BE = new Survey_Company_BE();

    WADIdentity identity;

    string filename;
    string file = null;
    DataTable dtCompanyName;
    DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
    string expression2;
    string Finalexpression2;


    protected void Page_Load(object sender, EventArgs e)
    {
        identity = this.Page.User.Identity as WADIdentity;

        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        try
        {
            if (!IsPostBack)
            {
                List<Survey_Company_BE> company_BEList = new List<Survey_Company_BE>();
                int companyID = Convert.ToInt32(Request.QueryString["CompId"]);
                if (companyID > 0)
                {
                    company_BEList = company_BAO.GetCompanyByID(companyID);
                    fillAccountCode();

                    if (company_BEList.Any())
                    {
                        fillEmailTemplate(Convert.ToString(company_BEList.FirstOrDefault().AccountID));
                        fillProject(Convert.ToString(company_BEList.FirstOrDefault().AccountID));
                        fillProjectManager(Convert.ToString(company_BEList.FirstOrDefault().AccountID));

                        fillCompanyDetails(company_BEList.FirstOrDefault());

                    }
                }
                else
                {
                    fillAccountCode();
                    fillEmailTemplate(Convert.ToString(identity.User.AccountID));
                    fillProject(Convert.ToString(identity.User.AccountID));
                    fillProjectManager(Convert.ToString(identity.User.AccountID));
                }

                if (Request.QueryString["Mode"] == "E")
                {
                    imbSave.Visible = true;
                    imbcancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Company";
                }
                else if (Request.QueryString["Mode"] == "R")
                {
                    imbSave.Visible = false;
                    imbcancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Company";
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
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// to fill in case of edit
    /// </summary>
    /// <param name="comapnyBE"></param>
    private void fillCompanyDetails(Survey_Company_BE comapnyBE)
    {
        try
        {
            if (comapnyBE.AccountID != null)
                ddlAccountCode.SelectedValue = Convert.ToString(comapnyBE.AccountID);

            txtcompanyname.Text = comapnyBE.CompanyName;

            if (comapnyBE.ProjectID != null)
                ddlProject.SelectedValue = Convert.ToString(comapnyBE.ProjectID);

            txtTitle.Text = comapnyBE.Title;

            if (comapnyBE.ManagerID != null)
                ddlProjectManager.SelectedValue = Convert.ToString(comapnyBE.ManagerID);

            txtDescription.Text = comapnyBE.Description;
            txtFaqText.Value = comapnyBE.FaqText;

            if (comapnyBE.StatusID != null)
                ddlStatus.SelectedValue = Convert.ToString(comapnyBE.StatusID);

            Finish_emailID_Txtbox.Text = comapnyBE.Finish_EmailID;

            Finish_Email_Chkbox.Checked = comapnyBE.Finish_EmailID_Chkbox ?? false;

            if (comapnyBE.EmailTMPLStart != null)
                ddlEmailStart.SelectedValue = Convert.ToString(comapnyBE.EmailTMPLStart);

            if (comapnyBE.EmailTMPLReminder1 != null)
                ddlEmailRemainder1.SelectedValue = Convert.ToString(comapnyBE.EmailTMPLReminder1);

            if (comapnyBE.EmailTMPLReminder2 != null)
                ddlEmailRemainder2.SelectedValue = Convert.ToString(comapnyBE.EmailTMPLReminder2);

            if (comapnyBE.EmailTMPLReminder3 != null)
                ddlEmailRemainder3.SelectedValue = Convert.ToString(comapnyBE.EmailTMPLReminder3);

            if (comapnyBE.EmailFinishEmailTemplate != null)
                ddlEmailTemplate.SelectedValue = Convert.ToString(comapnyBE.EmailFinishEmailTemplate);

            hdnQuestimage.Value = comapnyBE.QuestLogo;
            hdnReportimage.Value = comapnyBE.ReportLogo;


        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }



    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Survey_Company_BAO company_BAO = new Survey_Company_BAO();
                Survey_Company_BE company_BE = new Survey_Company_BE();
                if (ddlAccountCode.SelectedValue != null)
                    company_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

                company_BE.CompanyName = txtcompanyname.Text;

                if (ddlProject.SelectedValue != null)
                    company_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

                company_BE.Title = txtTitle.Text;

                if (ddlProjectManager.SelectedValue != null)
                    company_BE.ManagerID = Convert.ToInt32(ddlProjectManager.SelectedValue);

                company_BE.Description = txtDescription.Text;

                if (ddlStatus.SelectedValue != null)
                    company_BE.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                company_BE.Finish_EmailID = Finish_emailID_Txtbox.Text;
                company_BE.FaqText = txtFaqText.Value.Trim();

                company_BE.Finish_EmailID_Chkbox = Finish_Email_Chkbox.Checked;

                if (ddlEmailStart.SelectedValue != null)
                    company_BE.EmailTMPLStart = Convert.ToInt32(ddlEmailStart.SelectedValue);

                if (ddlEmailRemainder1.SelectedValue != null)
                    company_BE.EmailTMPLReminder1 = Convert.ToInt32(ddlEmailRemainder1.SelectedValue);

                if (ddlEmailRemainder2.SelectedValue != null)
                    company_BE.EmailTMPLReminder2 = Convert.ToInt32(ddlEmailRemainder2.SelectedValue);

                if (ddlEmailRemainder3.SelectedValue != null)
                    company_BE.EmailTMPLReminder3 = Convert.ToInt32(ddlEmailRemainder3.SelectedValue);


                if (ddlEmailTemplate.SelectedValue != null)
                    company_BE.EmailFinishEmailTemplate = Convert.ToInt32(ddlEmailTemplate.SelectedValue);


                if (qstFileUpload.HasFile)
                {
                    filename = System.IO.Path.GetFileName(qstFileUpload.PostedFile.FileName);
                    //filename = FileUpload.FileName;
                    file = GetUniqueFilename(filename);

                    string path = MapPath("~\\UploadDocs\\") + file;
                    qstFileUpload.SaveAs(path);
                    string name = file;
                    FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                    BinaryReader br1 = new BinaryReader(fs1);
                    Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                    br1.Close();
                    fs1.Close();
                    company_BE.QuestLogo = file;
                }
                else
                {
                    company_BE.QuestLogo = hdnQuestimage.Value;
                }

                if (reportFileUpload.HasFile)
                {
                    filename = System.IO.Path.GetFileName(reportFileUpload.PostedFile.FileName);
                    //filename = FileUpload.FileName;
                    file = GetUniqueFilename(filename);

                    string path = MapPath("~\\UploadDocs\\") + file;
                    reportFileUpload.SaveAs(path);
                    string name = file;
                    FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                    BinaryReader br1 = new BinaryReader(fs1);
                    Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                    br1.Close();
                    fs1.Close();
                    company_BE.ReportLogo = file;
                }
                else
                {
                    company_BE.ReportLogo = hdnReportimage.Value;
                }


                company_BE.ModifyBy = 1;
                company_BE.ModifyDate = DateTime.Now;
                company_BE.IsActive = 1;

                if (Request.QueryString["Mode"] == "E")
                {
                    company_BE.CompanyID = Convert.ToInt32(Request.QueryString["CompId"]);
                    //company_BAO.UpdateProject(company_BE);
                }


                company_BAO.AddCompany(company_BE);


                Response.Redirect("CompanyList.aspx", false);
                //HandleWriteLog("Start", new StackTrace(true));
                //}
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    protected void imbcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("CompanyList.aspx", false);

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


    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Survey_AccountUser_BAO accountUser_BAO = new Survey_AccountUser_BAO();
            Survey_EmailTemplate_BAO emailTemplate_BAO = new Survey_EmailTemplate_BAO();

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

                Account_BAO account_BAO = new Account_BAO();

                dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
                DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
                DataTable dtAccount = dtCompanyName.Clone();
                foreach (DataRow drAccount in resultsAccount)
                    dtAccount.ImportRow(drAccount);

                txtcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

                fillProject(ddlAccountCode.SelectedValue);
                fillProjectManager(ddlAccountCode.SelectedValue);

                fillEmailTemplate(ddlAccountCode.SelectedValue);
            }
            else
            {
                txtcompanyname.Text = "";

                fillProject(Convert.ToString(identity.User.AccountID));
                fillProjectManager(Convert.ToString(identity.User.AccountID));

                fillEmailTemplate(Convert.ToString(identity.User.AccountID));
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CompanyList.aspx", false);
    }


    #region private functions

    private void fillAccountCode()
    {
        Account_BAO account_BAO = new Account_BAO();
        ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
        ddlAccountCode.DataValueField = "AccountID";
        ddlAccountCode.DataTextField = "Code";
        ddlAccountCode.DataBind();

    }

    private void fillProject(string accountId)
    {
        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        ddlProject.DataSource = project_BAO.GetdtProjectList(accountId);
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

    }

    private void fillProjectManager(string accountId)
    {
        Survey_AccountUser_BAO accountUser_BAO = new Survey_AccountUser_BAO();
        ddlProjectManager.DataSource = accountUser_BAO.GetdtAccountUserList(accountId);
        ddlProjectManager.DataValueField = "UserID";
        ddlProjectManager.DataTextField = "UserName";
        ddlProjectManager.DataBind();

    }

    private void fillEmailTemplate(string accountId)
    {
        Survey_EmailTemplate_BAO emailTemplate_BAO = new Survey_EmailTemplate_BAO();
        DataTable dtEmailTemplate = emailTemplate_BAO.GetdtEmailTemplateList(accountId);

        DataRow[] resultsTemplate = dtEmailTemplate.Select("Title LIKE '%Invitation Template%'");

        DataTable dtmailtemp = dtEmailTemplate.Clone();

        foreach (DataRow drMail in resultsTemplate)
        {
            dtmailtemp.ImportRow(drMail);
        }

        int emailId = 0;
        if (dtmailtemp.Rows.Count > 0)
            emailId = Convert.ToInt32(dtmailtemp.Rows[0]["EmailTemplateID"]);

        ddlEmailStart.DataSource = dtEmailTemplate;
        ddlEmailStart.DataValueField = "EmailTemplateID";
        ddlEmailStart.DataTextField = "Title";
        ddlEmailStart.DataBind();

        if (emailId != null || emailId != 0)
        {
            ddlEmailStart.SelectedValue = Convert.ToString(emailId);
        }

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

        ddlEmailTemplate.DataSource = dtEmailTemplate;
        ddlEmailTemplate.DataValueField = "EmailTemplateID";
        ddlEmailTemplate.DataTextField = "Title";
        ddlEmailTemplate.DataBind();
        
    }

    #endregion private functions
}
