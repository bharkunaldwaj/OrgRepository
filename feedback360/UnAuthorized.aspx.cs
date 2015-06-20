/*--==========================================================================================
 Project              : DamcoWebTemplate
 File Name            : AuthorizationPage.aspx.cs
 Program Description  : Class file which contains Server Side Function & Logic of Authorization.aspx page
 Programmed By        : SubrataM
 Programmed On        : 25th May 2009
 Modification History :
 
==========================================================================================--*/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class UnAuthorized : System.Web.UI.Page {
    CodeBehindBase codeBehindBase = new CodeBehindBase();
    protected void Page_Load(object sender, EventArgs e) {

    }

    protected void lnlBtnLogout_Click(object sender, EventArgs e) {

        codeBehindBase.LogOut();
    }

}
