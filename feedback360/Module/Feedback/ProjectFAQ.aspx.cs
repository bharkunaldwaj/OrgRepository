using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Questionnaire_BE;
using Questionnaire_BAO;

public partial class Module_Feedback_ProjectFAQ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write( Request.QueryString["ProjectId"].ToString());

        //Label ll = (Label)this.Master.FindControl("Current_location");
        //ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>"; 
        Project_BAO project_BAO=new Project_BAO();

        string faqText = project_BAO.GetProjectFaqText(Convert.ToInt32(Request.QueryString["ProjectId"].ToString()));

        divFaqText.InnerHtml = faqText;
    }
}
