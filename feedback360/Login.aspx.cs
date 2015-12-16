using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using Administration_BAO;
using Administration_BE;

public partial class Login : CodeBehindBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Set focus to Account textbox.
        if (!IsPostBack)
            txtAccountCode.Focus();
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        try
        {
            if (!string.IsNullOrEmpty(Page.ClientQueryString))
            {
                //Check if contains return url
                if (!Page.ClientQueryString.Contains("ReturnUrl="))
                {
                    //Decrypt the URL
                    string[] strQueryString = EnCryptDecrypt.CryptorEngine.Decrypt(HttpUtility.UrlDecode(Page.ClientQueryString),
                        true).Split(',');

                    if (strQueryString.Length == 3)
                    {
                        string strUserName = string.Empty;
                        string strUserPassword = string.Empty;
                        string strUserAccount = string.Empty;

                        foreach (var item in strQueryString)
                        {
                            string[] strKeyValue = item.Split('=');

                            if (strKeyValue[0].ToLower() == "username")
                                strUserName = strKeyValue[1].Trim();
                            else if (strKeyValue[0].ToLower() == "password")
                                strUserPassword = strKeyValue[1].Trim();
                            else if (strKeyValue[0].ToLower() == "accountcode")
                                strUserAccount = strKeyValue[1].Trim();
                        }

                        UserLogin(strUserName, strUserPassword, strUserAccount);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Check login details 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnLogin_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            UserLogin(txtUserName.Text.Trim(), txtPassword.Text.Trim(), txtAccountCode.Text.Trim());
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Validate lofin details
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="password"></param>
    /// <param name="accountCode"></param>
    private void UserLogin(string loginName, string password, string accountCode)
    {
        //string loginName = string.Empty;
        //string password = string.Empty;
        //string accountCode = string.Empty;

        //loginName = GetString(txtUserName.Text.Trim());
        //password = GetString(txtPassword.Text.Trim());
        //accountCode = GetString(txtAccountCode.Text.Trim());

        if (loginName != "" || password != "" || accountCode != "")
        {
            ValidateUser(loginName, password, accountCode);
        }
        else
        {
            lblMessage.Text = "Please enter the login credentials";
        }
    }

    /// <summary>
    /// Function to validate user credentials
    /// </summary>
    /// <param name="p_loginName"></param>
    /// <param name="p_password"></param>
    private void ValidateUser(string loginName, string password, string accountCode)
    {
        User_BAO userBusinessAccessObject = new User_BAO();
        //GroupRight_BAO groupRight_BAO = new GroupRight_BAO();

        User_BE userBusinessEntity = new User_BE();
        //GroupRight_BE groupRight_BE = new GroupRight_BE();

        try
        {
            //p_password = PasswordGenerator.EnryptString(p_password);
            //Initilize properties
            userBusinessEntity.LoginID = loginName;
            userBusinessEntity.Password = password;
            userBusinessEntity.AccountCode = accountCode;

            List<User_BE> userBusinessEntityList = userBusinessAccessObject.GetUser(userBusinessEntity);
            userBusinessAccessObject = null;

            //if (user_BEList[0] == null) {
            if (userBusinessEntityList.Count == 0)
            {
                //lblMessage.Visible = true;
                lblMessage.Text = "Please check the login credentials";
                return;
            }
            else
            {
                //Initilize session
                GenerateTicket(loginName, userBusinessEntityList[0]);
                //If participant then redirect to Questionnaire page.
                if (userBusinessEntityList[0].GroupID.ToString() == ConfigurationManager.AppSettings["ParticipantRoleID"].ToString())
                {
                    Response.Redirect("Module/Questionnaire/AssignQuestionnaire.aspx", false);
                }
                else
                {
                    //Redirect to login
                    string pageUrl = FormsAuthentication.GetRedirectUrl(loginName, false);

                    if (pageUrl.Contains("Error.aspx"))
                    {
                        pageUrl = "Default.aspx";
                    }
                    Response.Redirect(pageUrl, false);
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Function to generate the ticket and authorization cookie
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="p_user_BE"></param>
    private void GenerateTicket(string userName, User_BE userBusinessEntity)
    {

        // Get the timeout value from web.config.
        TimeSpan timeOut = this.GetTimeout();

        // Create a forms ticket and attach it to a forms auth cookie.
        // We make this cookie ourselves because we want control over the user data section.
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1,
            userName,
            DateTime.Now,
            DateTime.Now + timeOut,
            false,
            userBusinessEntity.LoginID.ToString() + "$" + userBusinessEntity.Password.ToString() + "$" + userBusinessEntity.AccountCode.ToString() + "$" +
            userBusinessEntity.GroupID.ToString());
        //p_userBE.UserID.ToString() + "$" + p_userBE.GroupID.ToString());

        //Set session data.
        HttpContext.Current.Session["accountCode"] = userBusinessEntity.AccountCode.ToString();
        HttpContext.Current.Session["GroupID"] = userBusinessEntity.GroupID.ToString();
        HttpContext.Current.Session["AccountID"] = userBusinessEntity.AccountID;
        HttpContext.Current.Session["UserID"] = userBusinessEntity.UserID;

        String sessionx = "1=" + HttpContext.Current.Session["accountCode"] + "-" +
            HttpContext.Current.Session["GroupID"] + "-" + HttpContext.Current.Session["AccountID"] + "-" +
            HttpContext.Current.Session["UserID"];

        sessionx = EnCryptDecrypt.CryptorEngine.Encrypt(sessionx, true);

        sessionx = Guid.NewGuid().ToString();

        HttpContext.Current.Session["SessionData"] = sessionx;

        userBusinessEntity.SessionData = sessionx;

        User_BAO userBusinessAccessObject = new User_BAO();

        userBusinessAccessObject.UpdateUserSession(userBusinessEntity);

        string cookieVal = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
        cookie.Value = cookieVal;
        cookie.Path = FormsAuthentication.FormsCookiePath;

        // cookie.Expires = DateTime.Now + timeOut;                       

        // Add the cookie to the response.
        Response.Cookies.Add(cookie);
    }
}
