using System;
using System.Web;

public partial class MyHome : System.Web.UI.Page
{
    public string sessionx;
    /// <summary>
    /// Use to Initilize session data for Personlity.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["SessionData"] != null)
        {
            sessionx = HttpContext.Current.Session["SessionData"].ToString();
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
}
