using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Data;
//using DAF_BAO;
using Admin_BE;
using Admin_BAO;
using Administration_BAO;
using System.Configuration;
using Miscellaneous;

public partial class Module_Admin_AccountUser : CodeBehindBase
{
    //Global Variables
    AccountUser_BAO accountuserBusinessAccessObject = new AccountUser_BAO();
    AccountUser_BE accountUserBusinessEntity = new AccountUser_BE();
    List<AccountUser_BE> accountUserBusinessEntityList = new List<AccountUser_BE>();
    WADIdentity identity;

    DataTable CompanyName;
    //DataTable dtAllAccount;
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

                //Get Account user Details by account Id and usser Id.
                accountUserBusinessEntityList = accountuserBusinessAccessObject.GetAccountUserByID(Convert.ToInt32(identity.User.AccountID), accountuserID);

                Group_BAO groupBusinessAccessObject = new Group_BAO();
                //Get Group Details by Account Id and Bind Group Drop Down.
                ddlType.DataSource = groupBusinessAccessObject.GetdtGroupList(Convert.ToString(identity.User.AccountID));
                ddlType.DataTextField = "GroupName";
                ddlType.DataValueField = "GroupID";
                ddlType.DataBind();

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get Account list Details by Account Id and Bind Account Drop Down.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                lblAccountCodeValue.Text = identity.User.AccountCode.ToString();
                lblAccountNameValue.Text = identity.User.AccountName.ToString();

                if (accountUserBusinessEntityList.Count > 0)
                {
                    //Bind Account User Control details.
                    SetAccountUserValue(accountUserBusinessEntityList);
                }

                //If user is SuperAdmin then Set Account dropdown visible tru else false.
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

                //If query string contains "E" then Edit mode and if contains "R" then View mode and then
                //Hide show control Accordingly.
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

    /// <summary>
    /// Save User Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            AccountUser_BE accountuserBusinessEntity = new AccountUser_BE();
            AccountUser_BAO accountuserBusinessAccessObject = new AccountUser_BAO();
            lblusermsg.Text = "";
            accountuserBusinessEntity.GroupID = Convert.ToInt32(ddlType.SelectedValue);

            identity = this.Page.User.Identity as WADIdentity;

            bool userflag = Validateuser();

            if (userflag == false || Request.QueryString["Mode"] == "E")
            {
                if (identity.User.GroupID == 1)
                {
                    accountuserBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                }
                else
                {
                    accountuserBusinessEntity.AccountID = identity.User.AccountID;
                }

                accountuserBusinessEntity.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                accountuserBusinessEntity.Notification = Convert.ToBoolean(chkNotification.Checked);
                accountuserBusinessEntity.Salutation = ddlSalutation.SelectedItem.Text;
                accountuserBusinessEntity.FirstName = txtFirstName.Text.Trim();
                accountuserBusinessEntity.LastName = txtLastName.Text.Trim();
                accountuserBusinessEntity.LoginID = txtUserId.Text;
                accountuserBusinessEntity.Password = txtPassword.Text;
                accountuserBusinessEntity.EmailID = txtEmail.Text.Trim();
                accountuserBusinessEntity.ModifyBy = 1;
                accountuserBusinessEntity.ModifyDate = DateTime.Now;
                accountuserBusinessEntity.IsActive = 1;

                //If Query string contains "E" then it is Edit mode and allow update of record else Insert.
                if (Request.QueryString["Mode"] == "E" && accountuserBusinessEntity.Notification == true)
                {
                    identity = this.Page.User.Identity as WADIdentity;
                    int accountuserID = Convert.ToInt32(Request.QueryString["UsrId"]);
                    //Get Account detailss by account userId.
                    accountUserBusinessEntityList = accountuserBusinessAccessObject.GetAccountUserByID(Convert.ToInt32(identity.User.AccountID), accountuserID);

                    if (accountUserBusinessEntityList[0].Password != accountuserBusinessEntity.Password)
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
                        Accountcode = accountUserBusinessEntityList[0].Code;

                        string participantURLPath = ConfigurationManager.AppSettings["ParticipantURL"].ToString();

                        string link = "<a Target='_BLANK' href= '" + participantURLPath + "' >Click Link</a> ";

                        Template = Template.Replace("[LINK]", link);
                        Template = Template.Replace("[TITLE]", Title);
                        Template = Template.Replace("[EMAILID]", EmailID);
                        Template = Template.Replace("[FIRSTNAME]", FirstName);
                        Template = Template.Replace("[LOGINID]", Loginid);
                        Template = Template.Replace("[PASSWORD]", password);
                        Template = Template.Replace("[CODE]", Accountcode);
                        //Send Email
                        SendEmail.Send("Password Change Notification", Template, EmailID, "");
                    }
                }
                //Ifquery string Mode is null
                if (Request.QueryString["Mode"] == null)
                {
                    identity = this.Page.User.Identity as WADIdentity;
                    int accountuserID = Convert.ToInt32(Request.QueryString["UsrId"]);
                    //Get Account detailss by account userId.
                    accountUserBusinessEntityList = accountuserBusinessAccessObject.GetAccountUserByID(Convert.ToInt32(identity.User.AccountID), accountuserID);

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

                //If Query string contains "E" then it is Edit mode and allow update of record else Insert.
                if (Request.QueryString["Mode"] == "E")
                {
                    accountuserBusinessEntity.UserID = Convert.ToInt32(Request.QueryString["UsrId"]);
                    accountuserBusinessAccessObject.UpdateAccountUser(accountuserBusinessEntity);
                }
                else
                {
                    accountuserBusinessAccessObject.AddAccountUser(accountuserBusinessEntity);
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

    /// <summary>
    /// Redirect to Account user List page when clicked on cancel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Bind Account details section .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        int companyCode = Convert.ToInt32(ddlAccountCode.SelectedValue);

        Account_BAO accountBusinessAccessObject = new Account_BAO();

        //Get Account details.
        CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companyCode));

        //Set Company Code.
        expression1 = "AccountID='" + companyCode + "'";

        Finalexpression = expression1;

        DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

        DataTable dataTableAccount = CompanyName.Clone();

        foreach (DataRow datarowAccount in resultsAccount)
        {
            dataTableAccount.ImportRow(datarowAccount);
        }

        lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
    }

    /// <summary>
    /// Validate user Account.
    /// </summary>
    /// <returns></returns>
    protected bool Validateuser()
    {
        identity = this.Page.User.Identity as WADIdentity;
        DataTable userlist = new DataTable();

        //If User is super Admin then get account details by account selected value else by user account id.
        if (identity.User.GroupID == 1)
        {
            userlist = accountuserBusinessAccessObject.GetdtAccountUserList(ddlAccountCode.SelectedValue);
        }
        else
        {
            userlist = accountuserBusinessAccessObject.GetdtAccountUserList(Convert.ToString(identity.User.AccountID));
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

    /// <summary>
    /// Set Account controls by user account details.
    /// </summary>
    /// <param name="accountUserBusinessEntityList"></param>
    private void SetAccountUserValue(List<AccountUser_BE> accountUserBusinessEntityList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                //ddlAccountCode.SelectedValue = category_BEList[0].AccountID.ToString();
                string accountID = accountUserBusinessEntityList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = accountID;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

                    Account_BAO accountBusinessAccessObject = new Account_BAO();

                    //Get Account details by Account Id.
                    CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));

                    expression1 = "AccountID='" + companycode + "'";

                    Finalexpression = expression1;

                    DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

                    DataTable dataTableAccount = CompanyName.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                    {
                        dataTableAccount.ImportRow(drAccount);
                    }

                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }

            lblAccountCodeValue.Text = ddlAccountCode.SelectedItem.Text;
            lblAccountNameValue.Text = lblcompanyname.Text;

            hdnPassword.Value = accountUserBusinessEntityList[0].Password;
            ddlType.SelectedValue = Convert.ToString(accountUserBusinessEntityList[0].GroupID);
            ddlStatus.SelectedValue = Convert.ToString(accountUserBusinessEntityList[0].StatusID);
            chkNotification.Checked = Convert.ToBoolean(accountUserBusinessEntityList[0].Notification);

            //Set Title.
            if (Convert.ToString(accountUserBusinessEntityList[0].Salutation) == "Mr.")
            {
                ddlSalutation.SelectedValue = "1";
            }
            if (Convert.ToString(accountUserBusinessEntityList[0].Salutation) == "Mrs.")
            {
                ddlSalutation.SelectedValue = "2";
            }
            if (Convert.ToString(accountUserBusinessEntityList[0].Salutation) == "Ms.")
            {
                ddlSalutation.SelectedValue = "3";
            }
            if (Convert.ToString(accountUserBusinessEntityList[0].Salutation) == "Miss")
            {
                ddlSalutation.SelectedValue = "4";
            }
            if (Convert.ToString(accountUserBusinessEntityList[0].Salutation) == "Dr.")
            {
                ddlSalutation.SelectedValue = "5";
            }
            if (Convert.ToString(accountUserBusinessEntityList[0].Salutation) == "Prof.")
            {
                ddlSalutation.SelectedValue = "6";
            }

            //ddlSalutation.SelectedItem.Text = Convert.ToString(accountuser_BEList[0].Salutation);
            txtFirstName.Text = accountUserBusinessEntityList[0].FirstName;
            txtLastName.Text = accountUserBusinessEntityList[0].LastName;
            txtUserId.Text = accountUserBusinessEntityList[0].LoginID;
            txtPassword.Text = accountUserBusinessEntityList[0].Password;
            txtConfirmPassword.Text = accountUserBusinessEntityList[0].Password;

            txtEmail.Text = accountUserBusinessEntityList[0].EmailID;

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
}
