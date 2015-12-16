#region Namespaces
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Admin_BE;
using Admin_BAO;
using System.IO;
using System.Configuration;
using Miscellaneous;
using System.Net.Mail;
#endregion

public partial class Module_Admin_EmailTemplates : CodeBehindBase
{
    //Global Variables
    EmailTemplate_BAO emailTemplateBusinessAccessObject = new EmailTemplate_BAO();
    EmailTemplate_BE emailtemplateBusinessEntity = new EmailTemplate_BE();
    List<EmailTemplate_BE> emailTemplateBusinessEntityList = new List<EmailTemplate_BE>();

    WADIdentity identity;
    DataTable CompanyName;
    // DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
    string filename;
    string file = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        try
        {
            if (!Page.IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                int emailtemplateID = Convert.ToInt32(Request.QueryString["EmailTempID"]);

                //Get Template details by user Account ID.
                emailTemplateBusinessEntityList = emailTemplateBusinessAccessObject.GetEmailTemplateByID(Convert.ToInt32(identity.User.AccountID), emailtemplateID);

                //Bind Account Dropdown by User account ID.
                Account_BAO accountBusinessAccessObject = new Account_BAO();
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                if (Request.QueryString["Mode"] == "E")// If Querystring Contain "E" then it is in Edit mode.
                {
                    ibtnSave.Visible = true;
                    ibtnCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Email Templates";
                }
                else if (Request.QueryString["Mode"] == "R")// If Querystring Contain "R" then it is in View mode.
                {
                    ibtnSave.Visible = false;
                    ibtnCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "Edit Email Templates";
                }

                if (identity.User.GroupID == 1)//If it is a Super Admin Account Details Section is Visible else not. 
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

                if (emailTemplateBusinessEntityList.Count > 0)
                {
                    //Bind email tempalte controls by its value by Account id.
                    SetEmailTemplateValue(emailTemplateBusinessEntityList);//Bind Email Template.
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Preview Email to see Email Text structure in Mail.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void previewEmail_Click(object sender, ImageClickEventArgs e)
    {
        if (this.IsFileValid(this.FileUpload))
        {
            ibtnSave_Click(null, null);

            String Template = txtEmailText.Value;
            Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");
            string imagepath = Server.MapPath("~/EmailImages/");
            string emailImagepath = imagepath + hdnimage.Value;

            if (!File.Exists(emailImagepath))
            {
                emailImagepath = "";
            }
            MailAddress eMailaddress = new MailAddress("admin@i-comment360.com", "360 feedback");
            //Send mail to specified mail.
            SendEmail.Send(txtSubject.Text, Server.HtmlDecode(Template), txtEmail.Text, eMailaddress, emailImagepath);

            txtEmailText.InnerHtml = txtEmailText.Value;
        }
    }

    /// <summary>
    /// Initilize  Tempalte properties and Save Template Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            EmailTemplate_BE emailTemplateBusinessEntity = new EmailTemplate_BE();
            EmailTemplate_BAO emailTemplateBusinessAccessObject = new EmailTemplate_BAO();

            if (this.IsFileValid(this.FileUpload))
            {
                if (txtEmailText.Value.Trim() == "")
                {
                    //lblMessage.Text = "Please Enter Email Text";
                    //txtEmailText.Focus();
                    //return;
                }
                else
                {
                    identity = this.Page.User.Identity as WADIdentity;

                    if (identity.User.GroupID == 1)
                    {
                        emailTemplateBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    }
                    else
                    {
                        emailTemplateBusinessEntity.AccountID = identity.User.AccountID;
                    }

                    emailTemplateBusinessEntity.Title = GetString(txttitle.Text);
                    emailTemplateBusinessEntity.Description = GetString(txtDescription.Text);
                    emailTemplateBusinessEntity.Subject = GetString(txtSubject.Text);
                    emailTemplateBusinessEntity.EmailText = GetString(Server.HtmlDecode(txtEmailText.Value.Trim()));

                    // Upload Image.
                    if (FileUpload.HasFile)
                    {
                        //Get uploaded file name.
                        filename = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);

                        //Generate Unique file name for uploaded Image.
                        file = GetUniqueFilename(filename);

                        string path = MapPath("~\\EmailImages\\") + file;

                        FileUpload.SaveAs(path);

                        string name = file;

                        FileStream streamOfFile = new FileStream(Server.MapPath("~\\EmailImages\\") + file, FileMode.Open, FileAccess.Read);
                        BinaryReader binaryStream = new BinaryReader(streamOfFile);

                        Byte[] docbytes = binaryStream.ReadBytes((Int32)streamOfFile.Length);
                        binaryStream.Close();
                        streamOfFile.Close();
                        emailTemplateBusinessEntity.EmailImage = file;
                        hdnimage.Value = file;
                    }
                    else
                    {
                        if (Request.QueryString["Mode"] == "E" && FileUpload.FileName == "")
                            emailTemplateBusinessEntity.EmailImage = Convert.ToString(Session["FileName"]);
                        else
                            emailTemplateBusinessEntity.EmailImage = "";
                    }

                    emailTemplateBusinessEntity.ModifyBy = 1;
                    emailTemplateBusinessEntity.ModifyDate = DateTime.Now;
                    emailTemplateBusinessEntity.IsActive = 1;

                    if (Request.QueryString["Mode"] == "E") //If Querystring Contains "E" then Update else Insert details.
                    {
                        emailTemplateBusinessEntity.EmailTemplateID = Convert.ToInt32(Request.QueryString["EmailTempID"]);
                        emailTemplateBusinessAccessObject.UpdateEmailTemplate(emailTemplateBusinessEntity);
                    }
                    else
                    {
                        emailTemplateBusinessAccessObject.AddEmailTemplate(emailTemplateBusinessEntity);
                    }


                    if (e != null && sender != null)
                        Response.Redirect("EmailTemplatesList.aspx", false);
                    //HandleWriteLog("Start", new StackTrace(true));
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect Back to Previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("EmailTemplatesList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect Back to Previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("EmailTemplatesList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Check whether uploaded file is valid or not. 
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
    /// Bind Account and Comapny detail section on change of Account dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            identity = this.Page.User.Identity as WADIdentity;
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO accountBusinessAccessObject = new Account_BAO();

            CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            DataTable dataTableAccount = CompanyName.Clone();

            foreach (DataRow datarowAccount in resultsAccount)
            {
                dataTableAccount.ImportRow(datarowAccount);
            }

            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            txtEmailText.InnerHtml = Server.HtmlDecode(txtEmailText.InnerHtml);
        }
        else
        {
            lblcompanyname.Text = "";
        }
    }

    /// <summary>
    /// Bind Template Values.
    /// </summary>
    /// <param name="emailtemplateBusinessEntityList">List contains Template details.</param>
    private void SetEmailTemplateValue(List<EmailTemplate_BE> emailtemplateBusinessEntityList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1) //If it is a Super Admin then set Account details section, Company Name.
            {
                //ddlAccountCode.SelectedValue = category_BEList[0].AccountID.ToString();
                string accountID = emailtemplateBusinessEntityList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = accountID;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO accountBusinessAccessObject = new Account_BAO();

                    CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

                    DataTable dtAccount = CompanyName.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                    {
                        dtAccount.ImportRow(drAccount);
                    }
                    //Set Company Name
                    lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }

            txttitle.Text = emailtemplateBusinessEntityList[0].Title;
            txtDescription.Text = emailtemplateBusinessEntityList[0].Description;
            txtSubject.Text = emailtemplateBusinessEntityList[0].Subject;
            txtEmailText.InnerHtml = Server.HtmlDecode(emailtemplateBusinessEntityList[0].EmailText);

            /*To Show the Image*/
            hdnimage.Value = emailtemplateBusinessEntityList[0].EmailImage.ToString();
            Session["FileName"] = emailtemplateBusinessEntityList[0].EmailImage.ToString();

            //if (hdnimage.Value != "")
            //    imagelogo.Src = "../../EmailImages/" + hdnimage.Value;
            //else
            //    imagelogo.Src = "../../EmailImages/noImage.jpg";

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Generate Unique file name for uploaded Image.
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
}
