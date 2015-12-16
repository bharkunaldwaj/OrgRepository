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
using System.Net.Mail;
using Miscellaneous;
#endregion

public partial class Survey_Module_Admin_EmailTemplates : CodeBehindBase
{
    Survey_EmailTemplate_BAO emailtemplateBusinessAccessObject = new Survey_EmailTemplate_BAO();
    // Survey_EmailTemplate_BE emailtemplate_BE = new Survey_EmailTemplate_BE();
    List<Survey_EmailTemplate_BE> emailTemplateBusinessEntityList = new List<Survey_EmailTemplate_BE>();

    WADIdentity identity;
    DataTable CompanyName;
    DataTable dataTableAllAccount;
    string expression1;
    string Finalexpression;
    string filename;
    string file = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

            if (!Page.IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                int emailtemplateID = Convert.ToInt32(Request.QueryString["EmailTempID"]);
                //Get Email tempalte details by account id.
                emailTemplateBusinessEntityList = emailtemplateBusinessAccessObject.GetEmailTemplateByID(Convert.ToInt32(identity.User.AccountID), emailtemplateID);

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Bind Account Dropdown by User account ID.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                // If Querystring Contain "E" then it is in Edit mode.
                if (Request.QueryString["Mode"] == "E")
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

                //If it is a Super Admin Account Details Section is Visible else not. 
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

                if (emailTemplateBusinessEntityList.Count > 0)
                {
                    //Bind email tempalte controls by its value by Account id.
                    SetEmailTemplateValue(emailTemplateBusinessEntityList);
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind Template Values.
    /// </summary>
    /// <param name="emailTemplateBusinessEntityList">List contains Template details.</param>
    private void SetEmailTemplateValue(List<Survey_EmailTemplate_BE> emailTemplateBusinessEntityList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                //ddlAccountCode.SelectedValue = category_BEList[0].AccountID.ToString();
                string accountID = emailTemplateBusinessEntityList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = accountID;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {

                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO accountBusinessAccessObject = new Account_BAO();

                    CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

                    DataTable dataTableAccount = CompanyName.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                    {
                        dataTableAccount.ImportRow(dataRowAccount);
                    }

                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }

            txttitle.Text = emailTemplateBusinessEntityList[0].Title;
            txtDescription.Text = emailTemplateBusinessEntityList[0].Description;
            txtSubject.Text = emailTemplateBusinessEntityList[0].Subject;
            txtEmailText.Value = Server.HtmlDecode(emailTemplateBusinessEntityList[0].EmailText);

            /*To Show the Image*/
            hdnimage.Value = emailTemplateBusinessEntityList[0].EmailImage.ToString();
            Session["FileName"] = emailTemplateBusinessEntityList[0].EmailImage.ToString();

            //if (hdnimage.Value != "")
            //    imagelogo.Src = "../../EmailImages/" + hdnimage.Value;
            //else
            //    imagelogo.Src = "../../EmailImages/noImage.jpg";
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

            string Template = txtEmailText.Value;
            Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");
            string imagepath = Server.MapPath("~/EmailImages/");
            string emailimagepath = imagepath + hdnimage.Value;

            if (!File.Exists(emailimagepath))
            {
                emailimagepath = "";
            }

            MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");
            SendEmail.Send(txtSubject.Text, Server.HtmlDecode(Template), txtEmail.Text, maddr, emailimagepath);
        }

        ReBindEmailContent();
    }

    /// <summary>
    /// Save tempalte detail to dataBase.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_EmailTemplate_BE emailTemplateBusinessEntity = new Survey_EmailTemplate_BE();
            Survey_EmailTemplate_BAO emailtemplateBusinessDataAccessObject = new Survey_EmailTemplate_BAO();

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

                    emailTemplateBusinessEntity.Title = (txttitle.Text);
                    emailTemplateBusinessEntity.Description = GetString(txtDescription.Text);
                    emailTemplateBusinessEntity.Subject = (txtSubject.Text);
                    emailTemplateBusinessEntity.EmailText = (Server.HtmlDecode(txtEmailText.Value.Trim()));
                    // Upload Image.
                    if (FileUpload.HasFile)
                    {
                        //Get uploaded file name.
                        filename = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);
                        //Generate Unique file name for uploaded Image.
                        file = GetUniqueFilename(filename);

                        string path = MapPath("~\\EmailImages\\") + file;
                        //Upload file.
                        FileUpload.SaveAs(path);

                        string name = file;
                        FileStream fs1 = new FileStream(Server.MapPath("~\\EmailImages\\") + file, FileMode.Open, FileAccess.Read);
                        BinaryReader br1 = new BinaryReader(fs1);
                        Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                        br1.Close();
                        fs1.Close();
                        emailTemplateBusinessEntity.EmailImage = file;
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
                    //If Querystring Contains "E" then Update else Insert details.
                    if (Request.QueryString["Mode"] == "E")
                    {
                        emailTemplateBusinessEntity.EmailTemplateID = Convert.ToInt32(Request.QueryString["EmailTempID"]);
                        emailtemplateBusinessDataAccessObject.UpdateEmailTemplate(emailTemplateBusinessEntity);
                    }
                    else
                    {
                        emailtemplateBusinessDataAccessObject.AddEmailTemplate(emailTemplateBusinessEntity);
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
            Response.Redirect("EmailTemplatesList.aspx", false);
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
            Response.Redirect("EmailTemplatesList.aspx", false);
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
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO accountBusinessAccessObject = new Account_BAO();

            CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            DataTable dataTableAccount = CompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
            {
                dataTableAccount.ImportRow(dataRowAccount);
            }

            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            ReBindEmailContent();
        }
        else
        {
            lblcompanyname.Text = "";
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

    /// <summary>
    /// Rebind Email text control.
    /// </summary>
    private void ReBindEmailContent()
    {
        txtEmailText.InnerHtml = Server.HtmlDecode(txtEmailText.InnerHtml);
    }
}