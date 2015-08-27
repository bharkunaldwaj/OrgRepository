using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyHome : System.Web.UI.Page
{
    public string sessionx;
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
