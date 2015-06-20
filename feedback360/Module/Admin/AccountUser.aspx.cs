using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
//using DAF_BAO;
using Admin_BE;
using Admin_BAO;
using Administration_BAO;
using System.Collections;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using DatabaseAccessUtilities;
using Miscellaneous;

public partial class Module_Admin_AccountUser : CodeBehindBase
{
    AccountUser_BAO accountuser_BAO = new AccountUser_BAO();
    AccountUser_BE accountuser_BE = new AccountUser_BE();
    List<AccountUser_BE> accountuser_BEList = new List<AccountUser_BE>();
    WADIdentity identity;

    DataTable CompanyName;
    DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
    string expression12;
    string Finalexpression12;
    string Template;

    protected void Page_Load(object sender, EventArgs e)
    {

      //  Label ll = (Label)this.Master.FindControl("Current_location");
     //   ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {

            lblusermsg.Text = "";
            
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;
                int accountuserID = Convert.ToInt32(Request.QueryString["UsrId"]);
                
                accountuser_BEList = accountuser_BAO.GetAccountUserByID(Convert.ToInt32(identity.User.AccountID), accountuserID);

                Group_BAO group_BAO=new Group_BAO();
                ddlType.DataSource = group_BAO.GetdtGroupList(Convert.ToString(identity.User.AccountID));
                ddlType.DataTextField = "GroupName";
                ddlType.DataValueField = "GroupID";
                ddlType.DataBind();

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                
                lblAccountCodeValue.Text = identity.User.AccountCode.ToString();
                lblAccountNameValue.Text = identity.User.AccountName.ToString();

                if (accountuser_BEList.Count > 0)
                {
                    SetAccountUserValue(accountuser_BEList);
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

                if (Request.QueryString["Mode"] == "E")
                {
                    imbSave.Visible = true;
                    imbCancel.Visible = true;
                    imbBack.Visible = false;
                    txtUserId.ReadOnly = true;
                    lblheader.Text = "Edit User";
                }
                else if (Request.QueryString["Mode"] == "R")
                {
                    imbSave.Visible = false;
                    imbCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View User";
                }
            }
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void SetAccountUserValue(List<AccountUser_BE> accountuser_BEList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                //ddlAccountCode.SelectedValue = category_BEList[0].AccountID.ToString();
                string abc = accountuser_BEList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = abc;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {

                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO account1_BAO = new Account_BAO();

                    CompanyName = account1_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));

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

            lblAccountCodeValue.Text = ddlAccountCode.SelectedItem.Text;
            lblAccountNameValue.Text = lblcompanyname.Text;

            hdnPassword.Value = accountuser_BEList[0].Password;
            ddlType.SelectedValue = Convert.ToString(accountuser_BEList[0].GroupID);
            ddlStatus.SelectedValue = Convert.ToString(accountuser_BEList[0].StatusID);
            chkNotification.Checked=Convert.ToBoolean(accountuser_BEList[0].Notification);

            if (Convert.ToString(accountuser_BEList[0].Salutation) == "Mr.")
            {
                ddlSalutation.SelectedValue = "1";
            }
            if (Convert.ToString(accountuser_BEList[0].Salutation) == "Mrs.")
            {
                ddlSalutation.SelectedValue = "2";
            }
            if (Convert.ToString(accountuser_BEList[0].Salutation) == "Ms.")
            {
                ddlSalutation.SelectedValue = "3";
            }
            if (Convert.ToString(accountuser_BEList[0].Salutation) == "Miss")
            {
                ddlSalutation.SelectedValue = "4";
            }
            if (Convert.ToString(accountuser_BEList[0].Salutation) == "Dr.")
            {
                ddlSalutation.SelectedValue = "5";
            }
            if (Convert.ToString(accountuser_BEList[0].Salutation) == "Prof.")
            {
                ddlSalutation.SelectedValue = "6";
            }

            //ddlSalutation.SelectedItem.Text = Convert.ToString(accountuser_BEList[0].Salutation);
            txtFirstName.Text = accountuser_BEList[0].FirstName;
            txtLastName.Text = accountuser_BEList[0].LastName;
            txtUserId.Text = accountuser_BEList[0].LoginID;
            txtPassword.Text = accountuser_BEList[0].Password;
            txtConfirmPassword.Text = accountuser_BEList[0].Password;
            
            txtEmail.Text = accountuser_BEList[0].EmailID;

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
            AccountUser_BE accountuser_BE = new AccountUser_BE();
            AccountUser_BAO accountuser_BAO = new AccountUser_BAO();
            lblusermsg.Text = "";
            accountuser_BE.GroupID = Convert.ToInt32(ddlType.SelectedValue);

            identity = this.Page.User.Identity as WADIdentity;


            bool userflag = validateuser();

            if (userflag == false || Request.QueryString["Mode"] == "E")
            {


                if (identity.User.GroupID == 1)
                {
                    accountuser_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                }
                else
                {
                    accountuser_BE.AccountID = identity.User.AccountID;
                }

                accountuser_BE.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                accountuser_BE.Notification = Convert.ToBoolean(chkNotification.Checked);
                accountuser_BE.Salutation = ddlSalutation.SelectedItem.Text;
                accountuser_BE.FirstName = txtFirstName.Text.Trim();
                accountuser_BE.LastName = txtLastName.Text.Trim();
                accountuser_BE.LoginID = txtUserId.Text;
                accountuser_BE.Password = txtPassword.Text;
                accountuser_BE.EmailID = txtEmail.Text.Trim();
                accountuser_BE.ModifyBy = 1;
                accountuser_BE.ModifyDate = DateTime.Now;
                accountuser_BE.IsActive = 1;

                if (Request.QueryString["Mode"] == "E" && accountuser_BE.Notification == true)
                {
                    identity = this.Page.User.Identity as WADIdentity;
                    int accountuserID = Convert.ToInt32(Request.QueryString["UsrId"]);
                    accountuser_BEList = accountuser_BAO.GetAccountUserByID(Convert.ToInt32(identity.User.AccountID), accountuserID);
                    if (accountuser_BEList[0].Password != accountuser_BE.Password)
                    {
                        Template = System.IO.File.ReadAllText(Server.MapPath("~") + "\\UploadDocs\\PasswordTemplate.txt");

                        string Title = "";
                        string EmailID = "";
                        string FirstName = "";
                        string Loginid = "";
                        string password = "";
                        string Accountcode = "";


                        EmailID = txtEmail.Text.Trim();
                        FirstName = txtFirstName.Text.Trim();
                        Loginid = txtUserId.Text;
                        password = txtPassword.Text;
                        Accountcode = accountuser_BEList[0].Code;

                        string urlPath = ConfigurationManager.AppSettings["ParticipantURL"].ToString();

                        string link = "<a Target='_BLANK' href= '" + urlPath + "' >Click Link</a> ";

                        Template = Template.Replace("[LINK]", link);
                        Template = Template.Replace("[TITLE]", Title);
                        Template = Template.Replace("[EMAILID]", EmailID);
                        Template = Template.Replace("[FIRSTNAME]", FirstName);
                        Template = Template.Replace("[LOGINID]", Loginid);
                        Template = Template.Replace("[PASSWORD]", password);
                        Template = Template.Replace("[CODE]", Accountcode);


                        SendEmail.Send("Password Change Notification", Template, EmailID,"");
                    }
                }


                if (Request.QueryString["Mode"] == null )
                {
                    identity = this.Page.User.Identity as WADIdentity;
                    int accountuserID = Convert.ToInt32(Request.QueryString["UsrId"]);
                    accountuser_BEList = accountuser_BAO.GetAccountUserByID(Convert.ToInt32(identity.User.AccountID), accountuserID);
                   
                        Template = System.IO.File.ReadAllText(Server.MapPath("~") + "\\UploadDocs\\CreateUserTemplate.txt");

                        string Title = "";
                        string EmailID = "";
                        string FirstName = "";
                        string Loginid = "";
                        string password = "";
                        string Accountcode = "";


                        EmailID = txtEmail.Text.Trim();
                        FirstName = txtFirstName.Text.Trim();
                        Loginid = txtUserId.Text;
                        password = txtPassword.Text;
                        Accountcode = ddlAccountCode.SelectedItem.Text;

                        string urlPath = ConfigurationManager.AppSettings["ParticipantURL"].ToString();

                        string link = "<a Target='_BLANK' href= '" + urlPath + "' >Click Link</a> ";

                        Template = Template.Replace("[LINK]", link);
                        Template = Template.Replace("[TITLE]", Title);
                        Template = Template.Replace("[EMAILID]", EmailID);
                        Template = Template.Replace("[FIRSTNAME]", FirstName);
                        Template = Template.Replace("[LOGINID]", Loginid);
                        Template = Template.Replace("[PASSWORD]", password);
                        Template = Template.Replace("[CODE]", Accountcode);


                        //SendEmail.Send("Login Credentials", Template, EmailID);
                    
                }
                
                
                if (Request.QueryString["Mode"] == "E")
                {
                    accountuser_BE.UserID = Convert.ToInt32(Request.QueryString["UsrId"]);
                    accountuser_BAO.UpdateAccountUser(accountuser_BE);
                }
                else
                {
                    accountuser_BAO.AddAccountUser(accountuser_BE);
                }
                lblusermsg.Text = "";
                Response.Redirect("AccountUserList.aspx", false);
            }
            else
            {
                lblusermsg.Text = "Login ID already exists";
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

            Response.Redirect("AccountUserList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
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


    protected bool validateuser()
    {
       identity = this.Page.User.Identity as WADIdentity;
       DataTable userlist = new DataTable();
        
        if (identity.User.GroupID == 1)
        {
            userlist = accountuser_BAO.GetdtAccountUserList(ddlAccountCode.SelectedValue);
        }
        else
        {
            userlist = accountuser_BAO.GetdtAccountUserList(Convert.ToString(identity.User.AccountID));
           
        }


        expression12 = "LoginID='" + txtUserId.Text.Trim() + "'";

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
