using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text;
using Miscellaneous;

using Administration_BAO;
using Administration_BE;


public partial class Login : CodeBehindBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
                string[] strQS = EnCryptDecrypt.CryptorEngine.Decrypt(HttpUtility.UrlDecode(Page.ClientQueryString), true).Split(',');

                if (strQS.Length == 3)
                {
                    string strUName = string.Empty;
                    string strUPass = string.Empty;
                    string strUAcc = string.Empty;

                    foreach (var item in strQS)
                    {
                        string[] strKeyValue = item.Split('=');

                        if (strKeyValue[0].ToLower() == "username")
                            strUName = strKeyValue[1].Trim();
                        else if (strKeyValue[0].ToLower() == "password")
                            strUPass = strKeyValue[1].Trim();
                        else if (strKeyValue[0].ToLower() == "accountcode")
                            strUAcc = strKeyValue[1].Trim();
                    }

                    UserLogin(strUName, strUPass, strUAcc);

                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }       

    } 

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
    private void ValidateUser(string loginName, string password, string accountCode) {
        User_BAO user_BAO = new User_BAO();
        //GroupRight_BAO groupRight_BAO = new GroupRight_BAO();

        User_BE user_BE = new User_BE();
        //GroupRight_BE groupRight_BE = new GroupRight_BE();


        try {
            //p_password = PasswordGenerator.EnryptString(p_password);

            user_BE.LoginID = loginName;
            user_BE.Password = password;
            user_BE.AccountCode = accountCode;

            List<User_BE> user_BEList = user_BAO.GetUser(user_BE);
            user_BAO = null;

            //if (user_BEList[0] == null) {
            if(user_BEList.Count == 0){
                //lblMessage.Visible = true;
                lblMessage.Text = "Please check the login credentials";                
                return;
            }
            else {
                GenerateTicket(loginName, user_BEList[0]);
                if (user_BEList[0].GroupID.ToString() == ConfigurationManager.AppSettings["ParticipantRoleID"].ToString())
                {
                    Response.Redirect("Module/Questionnaire/AssignQuestionnaire.aspx", false);                    
                }
                else
                    Response.Redirect(FormsAuthentication.GetRedirectUrl(loginName, false), false);
            }
        }
        catch (Exception ex) {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Function to generate the ticket and authorization cookie
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="p_user_BE"></param>
    private void GenerateTicket(string p_userName, User_BE p_userBE)
    {

        // Get the timeout value from web.config.
        TimeSpan timeOut = this.GetTimeout();

        // Create a forms ticket and attach it to a forms auth cookie.
        // We make this cookie ourselves because we want control over the user data section.
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1,
            p_userName,
            DateTime.Now,
            DateTime.Now + timeOut,
            false,
            p_userBE.LoginID.ToString() + "$" + p_userBE.Password.ToString() + "$" + p_userBE.AccountCode.ToString() + "$" + p_userBE.GroupID.ToString());
            //p_userBE.UserID.ToString() + "$" + p_userBE.GroupID.ToString());

        HttpContext.Current.Session["accountCode"] = p_userBE.AccountCode.ToString();
        HttpContext.Current.Session["GroupID"] = p_userBE.GroupID.ToString();
        HttpContext.Current.Session["AccountID"] = p_userBE.AccountID;
        HttpContext.Current.Session["UserID"] = p_userBE.UserID;
        String sessionx = "1=" + HttpContext.Current.Session["accountCode"] + "-" + HttpContext.Current.Session["GroupID"] + "-" + HttpContext.Current.Session["AccountID"] + "-" + HttpContext.Current.Session["UserID"];
        sessionx = EnCryptDecrypt.CryptorEngine.Encrypt(sessionx, true);
        sessionx = Guid.NewGuid().ToString();
        HttpContext.Current.Session["SessionData"] = sessionx;
        p_userBE.SessionData = sessionx;

        User_BAO b = new User_BAO();
        b.UpdateUserSession(p_userBE);

        string cookieVal = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
        cookie.Value = cookieVal;
        cookie.Path = FormsAuthentication.FormsCookiePath;

        // cookie.Expires = DateTime.Now + timeOut;                       

        // Add the cookie to the response.
        Response.Cookies.Add(cookie);

    }


    
}
