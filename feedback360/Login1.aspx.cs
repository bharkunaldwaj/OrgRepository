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

using Administration_BE;
using Administration_BAO;

public partial class Login : CodeBehindBase {
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            //Clears Sessions, in case they exists
            Session.Abandon();
            //txtUserName.Focus(); //Focussing on the loginUser Text Box
        }
    }

    /// <summary>
    /// Function to Validate the user details
    ///             CASE (Valid User)   :- Create Session and store Details
    ///             CASE (Invalid User) :- Display Appropriate error message
    /// </summary>
    protected void btnLogin_Click(object sender, EventArgs e) {
        string loginName = string.Empty;
        string password = string.Empty;

        //Details Entered By User
        //loginName = txtUserName.Value.Trim();
        //password = txtPassword.Value.Trim();

        loginName = GetString(txtUserName.Value);
        password = GetString(txtPassword.Value);
        ValidateUser(loginName, password);
    }

    /// <summary>
    /// Function to validate user credentials
    /// </summary>
    /// <param name="p_loginName"></param>
    /// <param name="p_password"></param>
    private void ValidateUser(string p_loginName, string p_password) {
        User_BAO user_BAO = new User_BAO();
        //GroupRight_BAO groupRight_BAO = new GroupRight_BAO();

        User_BE user_BE = new User_BE();
        //GroupRight_BE groupRight_BE = new GroupRight_BE();


        try {
            p_password = PasswordGenerator.EnryptString(p_password);

            user_BE.UserCode = p_loginName;
            user_BE.Password = p_password;

            List<User_BE> user_BEList = user_BAO.GetUser(user_BE);
            user_BAO = null;

            //if (user_BEList[0] == null) {
            if(user_BEList.Count == 0){
                lblMessage.Visible = true;
                lblMessage.Text = "Either Username or Password entered is incorrect";                
                return;
            }
            else {
                GenerateTicket(p_loginName, user_BEList[0]);
                Response.Redirect(FormsAuthentication.GetRedirectUrl(p_loginName, false), false);
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
    private void GenerateTicket(string p_userName, User_BE p_userBE) {

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
            p_userBE.UserID.ToString() + "$" + p_userBE.GroupID.ToString());
        string cookieVal = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
        cookie.Value = cookieVal;
        cookie.Path = FormsAuthentication.FormsCookiePath;

        // cookie.Expires = DateTime.Now + timeOut;                       

        // Add the cookie to the response.
        Response.Cookies.Add(cookie);

    }

    /// <summary>
    /// When user clicks on Forgot password
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkForgotPassword_Click(object sender, EventArgs e) {
        string loginName = string.Empty;
        string password = string.Empty;

        //Details Entered By User
        loginName = txtUserName.Value.Trim();
        password = txtPassword.Value.Trim();
        SendMail(loginName, password);
    }


    /// <summary>
    /// Function to send email for forgot password
    /// </summary>
    /// <param name="p_loginName"></param>
    /// <param name="p_password"></param>
    private void SendMail(string p_loginName, string p_password) {
        User_BAO user_BAO = new User_BAO();
        //GroupRight_BAO groupRight_BAO = new GroupRight_BAO();

        User_BE user_BE = new User_BE();
        //GroupRight_BE groupRight_BE = new GroupRight_BE();


        try {
            p_password = PasswordGenerator.EnryptString(p_password);

            user_BE.UserCode = p_loginName;
            user_BE.Password = p_password;

            List<User_BE> user_BEList = user_BAO.GetUser(user_BE);
            user_BAO = null;

            //if (user_BEList[0] == null) {
            if (user_BEList.Count == 0) {
                lblMessage.Visible = true;
                lblMessage.CssClass = "error";
                lblMessage.Text = "Username is invalid";
                return;
            }
            else {
                //GenerateTicket(p_loginName, user_BEList[0]);
                StringBuilder bodyText = new StringBuilder();
                bodyText.Append("Dear "+user_BEList[0].FName+"<br>");
                bodyText.Append("Your password is " + user_BEList[0].Password + "<br><br><br>");
                bodyText.Append("Thanks,<br>Shamrock Team");
                string emailBodyText = bodyText.ToString();

                SendEmail.Send("Password Recovery", emailBodyText, user_BEList[0].Email,"");
                lblMessage.CssClass = "success";
                lblMessage.Visible = true;
                lblMessage.Text = "Your password has been send to your Email ID";
                
            }
        }
        catch (Exception ex) {
            HandleException(ex);
        }
    }
}
