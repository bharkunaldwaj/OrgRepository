<%@ Application Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.Security.Principal" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        Application["range_flag"] = 0;
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Session["dtStartDate"] = null;
        Session["dtEndDate"] = null;
        Session["dtRemainderDate1"] = null;
        Session["dtRemainderDate2"] = null;
        Session["dtRemainderDate3"] = null;
            
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    
     void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
    {
        // Get a reference to the current User
        IPrincipal usr = HttpContext.Current.User;
        // If we are dealing with an authenticated forms authentication request
        if (usr.Identity.IsAuthenticated && usr.Identity.AuthenticationType == "Forms")
        {
            FormsIdentity fIdent = usr.Identity as FormsIdentity;
            // Create a WADIdentity based on the FormsAuthenticationTicket
            WADIdentity ci = new WADIdentity(fIdent.Ticket);
            //Object o = ci.User;
            
            // Create the WADPrincipal
            WADPrincipal p = new WADPrincipal(ci);
            // Attach the WADPrincipal to HttpContext.User and Thread.CurrentPrincipal
            HttpContext.Current.User = p;
            //Thread.CurrentPrincipal = p;
        }
    }

     protected void Application_BeginRequest(object sender, EventArgs e)
     {
         string culture = string.Empty;

         //string[] languages = HttpContext.Current.Request.UserLanguages;
         //if (languages != null)
         //{
             culture = "en-GB"; // languages[0].Trim();

             System.Threading.Thread.CurrentThread.CurrentCulture =
             System.Globalization.CultureInfo.CreateSpecificCulture(culture);

             System.Threading.Thread.CurrentThread.CurrentUICulture =
             new System.Globalization.CultureInfo(culture);
         //}
     }


</script>
