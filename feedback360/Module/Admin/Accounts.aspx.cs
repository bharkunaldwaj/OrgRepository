using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data;
using System.Diagnostics;
//using DAF_BAO;
using Admin_BE;
using Admin_BAO;

public partial class Module_Admin_Accounts : CodeBehindBase
{
    Account_BAO account_BAO = new Account_BAO();
    Account_BE account_BE = new Account_BE();
    List<Account_BE> account_BEList = new List<Account_BE>();
    string filename;
    string file = null;
    WADIdentity identity;
    string expression1;
    string Finalexpression;
    string expression12;
    string Finalexpression12;

    protected void Page_Load(object sender, EventArgs e)
    {

     //   Label ll = (Label)this.Master.FindControl("Current_location");
      //  ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {
                int accountID = Convert.ToInt32(Request.QueryString["AccId"]);
                account_BEList = account_BAO.GetAccountByID(accountID);

                if (account_BEList.Count > 0)
                {
                    SetAccountValue(account_BEList);
                }

                if (Request.QueryString["Mode"] == "E")
                {
                    imbSave.Visible = true;
                    imbCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Account";
                    //txtLoginID.ReadOnly = true;
                }
                else if (Request.QueryString["Mode"] == "R")
                {
                    imbSave.Visible = false;
                    imbCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Account";
                }
            }

            //lblfilemsg.Text = "";

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void SetAccountValue(List<Account_BE> account_BEList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            txtCode.Text = account_BEList[0].Code;
            //txtLoginID.Text = account_BEList[0].LoginID;
            hdnPassword.Value = account_BEList[0].Password;
            //txtPassword.Text = account_BEList[0].Password;
            txtOrganisationName.Text = account_BEList[0].OrganisationName;
            ddlType.SelectedValue = Convert.ToString(account_BEList[0].AccountTypeID);
            txtDescription.Text = account_BEList[0].Description;
            txtEmail.Text = account_BEList[0].EmailID;
            txtWebsite.Text = account_BEList[0].Website;
            ddlStatus.SelectedValue = Convert.ToString(account_BEList[0].StatusID);
            lblUploadFileName.Text = account_BEList[0].CompanyLogo;
            txtBannerBGColor.Text = account_BEList[0].HeaderBGColor;
            txtMenuBGColor.Text = account_BEList[0].MenuBGColor;
            hdnimage.Value = account_BEList[0].CompanyLogo;
            txtCopyRight.Text = account_BEList[0].CopyRightLine;
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

    }

    protected bool validateuser()
    {
        identity = this.Page.User.Identity as WADIdentity;
        DataTable userlist = new DataTable();

       
       userlist = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));




       //expression12 = "LoginID='" + txtLoginID.Text.Trim() + "'";

        Finalexpression12 = expression12;

        DataRow[] resultsuserid = userlist.Select(Finalexpression12);

        if (resultsuserid.Count() > 0)
        {
            return true;

        }
        else
        {
            return false;
        }


    }

    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            Account_BE account_BE = new Account_BE();
            Account_BAO account_BAO = new Account_BAO();

            bool userflag = validateAccount();

             if (userflag == false || Request.QueryString["Mode"] == "E")
             {
                 if (this.IsFileValid(this.fuplCompanyLogo))
                 {

                     account_BE.Code = txtCode.Text;
                     //account_BE.LoginID = txtLoginID.Text;
                     //account_BE.Password = txtPassword.Text;
                     account_BE.OrganisationName = txtOrganisationName.Text;
                     account_BE.AccountTypeID = Convert.ToInt32(ddlType.SelectedValue);
                     account_BE.Description = txtDescription.Text;
                     account_BE.EmailID = txtEmail.Text;
                     account_BE.Website = txtWebsite.Text;
                     account_BE.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

                     if (fuplCompanyLogo.HasFile)
                     {
                         filename = System.IO.Path.GetFileName(fuplCompanyLogo.PostedFile.FileName);

                         //File Upload Code Starts
                         string uniqueFileName = GetUniqueFilename(filename);

                         string path = MapPath("~\\UploadDocs\\") + uniqueFileName;
                         fuplCompanyLogo.SaveAs(path);

                         account_BE.CompanyLogo = uniqueFileName;
                     }
                     else
                     {
                         if (Request.QueryString["Mode"] == "E" && lblUploadFileName.Text != "" && hdnRemoveImage.Value != "")
                             account_BE.CompanyLogo = lblUploadFileName.Text;
                         else
                             account_BE.CompanyLogo = "";
                     }

                     account_BE.CopyRightLine = txtCopyRight.Text;

                     if (txtBannerBGColor.Text.Trim()=="")
                         account_BE.HeaderBGColor = "#ADD8E6";
                     else
                        account_BE.HeaderBGColor = txtBannerBGColor.Text;

                     if (txtMenuBGColor.Text.Trim() == "")
                        account_BE.MenuBGColor = "#4169E1";
                     else
                        account_BE.MenuBGColor = txtMenuBGColor.Text;

                     account_BE.ModifyBy = 1;
                     account_BE.ModifyDate = DateTime.Now;
                     account_BE.IsActive = 1;

                     if (Request.QueryString["Mode"] == "E")
                     {
                         account_BE.AccountID = Convert.ToInt32(Request.QueryString["AccId"]);
                         account_BAO.UpdateAccount(account_BE);
                     }
                     else
                     {
                         account_BAO.AddAccount(account_BE);
                     }

                     Response.Redirect("AccountList.aspx", false);

                     lblusermsg.Text = "";

                 }
             }
             else
             {
                 lblusermsg.Text = "Account Code already exists";
             }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void imbCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("AccountList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region Upload File Validation

    protected bool IsFileValid(FileUpload uploadControl)
    {
        string[] AllowedExtensions = ConfigurationManager.AppSettings["Uploadextension"].Split(',');

        bool isFileOk = true;       
        bool isExtensionError = false;
        int MaxSizeAllowed = 5 * 1048576;// Size Allow only in mb
        string errorMessage = "";

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
                //lblfilemsg.Text = "Invalid file type (Only .png, .gif, .bmp, .jpg, .jpeg )  ";

            }
            else if (isSizeError)
            {
                //lblfilemsg.Text = "Maximum size of the file exceeded";
            }

            if (errorMessage !="")
                Page.RegisterStartupScript("FileTyp", "<script language='JavaScript'>alert('" + errorMessage + "');</script>");

        }

        return isFileOk;



    }

    public string GetUniqueFilename(string fileName)
    {
        string baseName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));
        string uniqueFileName = string.Format("{0}{1}{2}", baseName, DateTime.Now.Ticks, Path.GetExtension(fileName));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniqueFileName;
    }


  

    #endregion

    protected bool validateAccount()
    {
        identity = this.Page.User.Identity as WADIdentity;
        DataTable userlist = new DataTable();


        userlist = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));




        expression12 = "Code='" + txtCode.Text + "'";

        Finalexpression12 = expression12;

        DataRow[] resultsuserid = userlist.Select(Finalexpression12);

        if (resultsuserid.Count() > 0)
        {
            return true;

        }
        else
        {
            return false;
        }


    }
   
}
