/*
* PURPOSE: Display Error Page in case there is an error in the Application
* AUTHOR:  
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.Collections;
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

public partial class Error : System.Web.UI.Page
{
    CodeBehindBase codeBehindBase = new CodeBehindBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer != null)
        {
            string preURL = Convert.ToString(Request.UrlReferrer);
            if (preURL.Contains("Survey"))
                lnkBtnRedirect.PostBackUrl = "~/Survey_Default.aspx";
        }

    }

    /// <summary>
    /// Logout button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void lnlBtnLogout_Click(object sender, EventArgs e)
    {
        codeBehindBase.LogOut();
    }
}
