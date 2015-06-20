/*  
* PURPOSE: This is the Class which is inherited by all web pages except Login and Dashboard
* AUTHOR: 
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using Miscellaneous;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Summary description for CodeBehindBase
/// </summary>
public class CodeBehindBase : System.Web.UI.Page
{
    WADIdentity identity;
    
    private FileStream FS;
    private StreamWriter SW;
    private string fpath;

    //public string errorLogPath = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
    

    public CodeBehindBase()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public WADIdentity Identity
    {
        get
        {
            return this.Page.User.Identity as WADIdentity;
        }
    }

    public WADPrincipal Principal
    {
        get
        {
            return this.Page.User as WADPrincipal;
        }
    }

    /// <summary>
    /// Pulls the authentication timeout value from the config file.
    /// Unfortunately, and for some unknown reason, Timeout isn't exposed on FormsAuthentication, although everything else is.
    /// It must be read via SelectSingleNode.
    /// See the post below for a person smarter than me coming to the same conclusion.
    /// http://www.hanselman.com/blog/AccessingTheASPNETFormsAuthenticationTimeoutValue.aspx
    /// </summary>
    /// <returns>The timeout <see cref="T:System.TimeSpan"/></returns>    /// 
    protected TimeSpan GetTimeout()
    {
        // Default to 30 minutes.
        TimeSpan vRet = new TimeSpan(0, 0, 30, 0, 0);

        XmlDocument vConfigDoc = new XmlDocument();
        try
        {
            vConfigDoc.Load(Server.MapPath(Request.ApplicationPath + "/web.config"));
        }
        catch (Exception ex)
        {
            // ShowMessage(ex);
            HandleException(ex);
            return vRet;
        }

        XmlNode vNode = null;
        try
        {
            vNode = vConfigDoc.SelectSingleNode("/configuration/system.web/authentication/forms");
        }
        catch (Exception ex)
        {
            // ShowMessage(ex);
            HandleException(ex);
            return vRet;
        }

        try
        {
            vRet = new TimeSpan(0, Convert.ToInt32(vNode.Attributes["timeout"].Value), 0);
        }
        catch (Exception ex)
        {
            // ShowMessage(ex);
            HandleException(ex);
            return vRet;
        }

        return vRet;
    }

    protected override void OnLoad(EventArgs e)
    {

        if ((this.Request.FilePath.ToLower().Contains("login.aspx")) || (this.Request.FilePath.ToLower().Contains("dashboard.aspx")))
        {
            return;
        }

        ////////////try {
            if (!IsPostBack)
            {
                // Be sure to call the base class's OnLoad method!
                base.OnLoad(e);
                CheckSecurity(false);

                // ... add custom logic here ...
                   }

        ////////////}
        ////////////catch (Exception ex)
        ////////////{
        ////////////    HandleException(ex);
        ////////////}
        //CheckSecurity(true);
    }

    public void CurrentPrincipalThread()
    {
        try
        {
            WADPrincipal identity = this.Page.User as WADPrincipal;
            System.Threading.Thread.CurrentPrincipal = identity;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    public void HandleException(Exception ex)
    {
        //ExceptionLogger.Write(ex.ToString());

        string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                    "Error Application: Feedback 360 - UI" + Environment.NewLine + "," +
                    "Error Function: " + ex.TargetSite + Environment.NewLine + "," +
                    "Error Line: " + ex.StackTrace + Environment.NewLine + "," +
                    "Error Source: " + ex.Source + Environment.NewLine + "," +
                    "Error Message: " + ex.Message + Environment.NewLine;

        fpath = HttpContext.Current.Server.MapPath("~") + "//Error.log";

        if (File.Exists(fpath))
        { FS = new FileStream(fpath, FileMode.Append, FileAccess.Write); }
        else
        { FS = new FileStream(fpath, FileMode.Create, FileAccess.Write); }

        string msg = str.Replace(",", "");

        SW = new StreamWriter(FS);
        SW.WriteLine(msg);

        SW.Close();
        FS.Close();

        string errorPage = "";
        errorPage = HttpContext.Current.Request.ApplicationPath + "\\Error.aspx";
        HttpContext.Current.Response.Redirect(errorPage, false);
   }

    public void HandleExceptionError(Exception ex)
   {
       //ExceptionLogger.Write(ex.ToString());

       string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                   "Error Application: Feedback 360 " + Environment.NewLine + "," +
                   "Error Function: " + ex.TargetSite + Environment.NewLine + "," +
                   "Error Line: " + ex.StackTrace + Environment.NewLine + "," +
                   "Error Source: " + ex.Source + Environment.NewLine + "," +
                   "Error Message: " + ex.Message + Environment.NewLine;

       fpath = HttpContext.Current.Server.MapPath("~") + "//Error.log";

       if (File.Exists(fpath))
       { FS = new FileStream(fpath, FileMode.Append, FileAccess.Write); }
       else
       { FS = new FileStream(fpath, FileMode.Create, FileAccess.Write); }

       string msg = str.Replace(",", "");

       SW = new StreamWriter(FS);
       SW.WriteLine(msg);

       SW.Close();
       FS.Close();

       string errorPage = "";
       errorPage = "Error.aspx";
       HttpContext.Current.Response.Redirect(errorPage, false);
    }

    public void LogOut()
    {
        try
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            string loginPagePath = "";
            loginPagePath = HttpContext.Current.Request.ApplicationPath + "\\Login.aspx";
            HttpContext.Current.Response.Redirect(loginPagePath,false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    public void HandleWriteLog(string Log,StackTrace stackTrace)
    {
        try
        {
            //identity = this.Page.User.Identity as WADIdentity;
            //TraceLogger.WriteLog(identity.Name, Enumerators.LogLevel.UI, stackTrace, Log);

        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Kumar Rakesh
    /// Security Check
    /// </summary>
    /// <param name="iUserId"></param>
    protected void CheckSecurity(bool GridOnly)
    {
        try
        {

            string sCurPage = System.IO.Path.GetFileName(HttpContext.Current.Request.FilePath).ToLower();
            WADPrincipal identity = this.User as WADPrincipal;

            string canView = identity.CanPerform(sCurPage.ToUpper());

            if (canView == "")
            {
                string unAuthorizedPage = "";
                unAuthorizedPage = HttpContext.Current.Request.ApplicationPath + "\\UnAuthorized.aspx";
                HttpContext.Current.Response.Redirect(unAuthorizedPage, false);
             
            }
            else
            {
                foreach (Control masterControl in Page.Controls)
                {
                    if (masterControl is MasterPage)
                    {
                        foreach (Control formControl in masterControl.Controls)
                        {
                            if (formControl is System.Web.UI.HtmlControls.HtmlForm)
                            {
                                //Parse the page HTML Form
                                PageParser.ParsePage(formControl, canView, GridOnly);

                            }

                        }
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
    /// This method accepts string as a parameter and does the string manipulation
    /// </summary>
    /// <param name="p_value"></param>
    /// <returns></returns>
    public string GetString(string p_value) {
        string returnValue;

        //try {
            p_value = p_value.ToString().Replace("'", "").Trim();
            returnValue = p_value;
        //}
       // catch (NullReferenceException ex) {
        //    returnValue = null;
        //}
        return returnValue;
    }
}
