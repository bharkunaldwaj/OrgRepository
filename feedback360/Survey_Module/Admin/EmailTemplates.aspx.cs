#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Diagnostics;
using DAF_BAO;
using Admin_BE;
using Admin_BAO;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Net.Mail;
using Miscellaneous;
#endregion

public partial class Survey_Module_Admin_EmailTemplates : CodeBehindBase
{
    Survey_EmailTemplate_BAO emailtemplate_BAO = new Survey_EmailTemplate_BAO();
    Survey_EmailTemplate_BE emailtemplate_BE = new Survey_EmailTemplate_BE();
    List<Survey_EmailTemplate_BE> emailtemplate_BEList = new List<Survey_EmailTemplate_BE>();

    WADIdentity identity;
    DataTable CompanyName;
    DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
    string filename;
    string file = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label ll = (Label)this.Master.FindControl("Current_location");
            ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            if (!Page.IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                int emailtemplateID = Convert.ToInt32(Request.QueryString["EmailTempID"]);

                emailtemplate_BEList = emailtemplate_BAO.GetEmailTemplateByID(Convert.ToInt32(identity.User.AccountID), emailtemplateID);

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                if (Request.QueryString["Mode"] == "E")
                {
                    ibtnSave.Visible = true;
                    ibtnCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Email Templates";
                }
                else if (Request.QueryString["Mode"] == "R")
                {
                    ibtnSave.Visible = false;
                    ibtnCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "Edit Email Templates";
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

                if (emailtemplate_BEList.Count > 0)
                {
                    SetEmailTemplateValue(emailtemplate_BEList);
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void SetEmailTemplateValue(List<Survey_EmailTemplate_BE> emailtemplate_BEList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                //ddlAccountCode.SelectedValue = category_BEList[0].AccountID.ToString();
                string abc = emailtemplate_BEList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = abc;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {

                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO account_BAO = new Account_BAO();

                    CompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

                    DataTable dtAccount = CompanyName.Clone();

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
            txttitle.Text = emailtemplate_BEList[0].Title;
            txtDescription.Text = emailtemplate_BEList[0].Description;
            txtSubject.Text = emailtemplate_BEList[0].Subject;
            txtEmailText.Value = Server.HtmlDecode(emailtemplate_BEList[0].EmailText);

            /*To Show the Image*/
            hdnimage.Value = emailtemplate_BEList[0].EmailImage.ToString();
            Session["FileName"] = emailtemplate_BEList[0].EmailImage.ToString();

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

    protected void previewEmail_Click(object sender, ImageClickEventArgs e)
    {
        if (this.IsFileValid(this.FileUpload))
        {
            ibtnSave_Click(null, null);

            String Template = txtEmailText.Value;
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

    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_EmailTemplate_BE emailtemplate_BE = new Survey_EmailTemplate_BE();
            Survey_EmailTemplate_BAO emailtemplate_BAO = new Survey_EmailTemplate_BAO();

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
                        emailtemplate_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    }
                    else
                    {
                        emailtemplate_BE.AccountID = identity.User.AccountID;
                    }

                    emailtemplate_BE.Title = (txttitle.Text);
                    emailtemplate_BE.Description = GetString(txtDescription.Text);
                    emailtemplate_BE.Subject = (txtSubject.Text);
                    emailtemplate_BE.EmailText = (Server.HtmlDecode(txtEmailText.Value.Trim()));
                    if (FileUpload.HasFile)
                    {
                        filename = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);
                        file = GetUniqueFilename(filename);

                        string path = MapPath("~\\EmailImages\\") + file;
                        FileUpload.SaveAs(path);
                        string name = file;
                        FileStream fs1 = new FileStream(Server.MapPath("~\\EmailImages\\") + file, FileMode.Open, FileAccess.Read);
                        BinaryReader br1 = new BinaryReader(fs1);
                        Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                        br1.Close();
                        fs1.Close();
                        emailtemplate_BE.EmailImage = file;
                    }
                    else
                    {
                        if (Request.QueryString["Mode"] == "E" && FileUpload.FileName == "")
                            emailtemplate_BE.EmailImage = Convert.ToString(Session["FileName"]);
                        else
                            emailtemplate_BE.EmailImage = "";
                    }
                    emailtemplate_BE.ModifyBy = 1;
                    emailtemplate_BE.ModifyDate = DateTime.Now;
                    emailtemplate_BE.IsActive = 1;

                    if (Request.QueryString["Mode"] == "E")
                    {
                        emailtemplate_BE.EmailTemplateID = Convert.ToInt32(Request.QueryString["EmailTempID"]);
                        emailtemplate_BAO.UpdateEmailTemplate(emailtemplate_BE);
                    }
                    else
                    {
                        emailtemplate_BAO.AddEmailTemplate(emailtemplate_BE);
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
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {

            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO account_BAO = new Account_BAO();

            CompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            DataTable dtAccount = CompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
            {
                dtAccount.ImportRow(drAccount);
            }

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            ReBindEmailContent();
        }
        else
        {
            lblcompanyname.Text = "";
        }
    }

    private void ReBindEmailContent()
    {
        txtEmailText.InnerHtml = Server.HtmlDecode(txtEmailText.InnerHtml);
    }
}