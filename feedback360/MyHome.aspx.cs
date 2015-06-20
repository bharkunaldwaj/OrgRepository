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
        sessionx = HttpContext.Current.Session["SessionData"].ToString();
    }
}
