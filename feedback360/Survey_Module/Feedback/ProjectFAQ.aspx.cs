using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Questionnaire_BE;
using Questionnaire_BAO;

public partial class Survey_Module_Feedback_ProjectFAQ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Label ll = (Label)this.Master.FindControl("Current_location");
        //ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        //Response.Write( Request.QueryString["ProjectId"].ToString());
        string faqText = string.Empty;

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();

        faqText = project_BAO.GetCompanyFaqText(int.Parse(Request.QueryString["ProjectId"].ToString()));

        if (string.IsNullOrEmpty(faqText))
        {
            faqText = project_BAO.GetProjectFaqText(Convert.ToInt32(Request.QueryString["ProjectId"].ToString()));
        }

        divFaqText.InnerHtml = faqText;
    }
}
