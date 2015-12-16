using System;
using System.Web.UI.WebControls;
using System.Configuration;
using Questionnaire_BAO;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["flag"] = 1;
        //if ((int)Session["flag"] == 1)
        //    Session["flag"] = 2;
        //if ((int)Session["flag"] == 2)
        //    Session["flag"] = 1;
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        WADIdentity identity;
        identity = this.Page.User.Identity as WADIdentity;

        //If user Not Admin
        if (identity.User.GroupID != 1 && identity.User.LoginID != "admin")
        {
            divNavigation.Visible = false;
        }

        string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
        string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();

        Image imageHeader = (Image)Master.FindControl("imgProjectLogo");

        //If group id equals to participant role id
        if (identity.User.GroupID.ToString() == participantRoleId)
        {
            //////divNavigation.Visible = false;
            //////divParticipant.Visible = true;
            Response.Redirect("Module/Questionnaire/AssignQuestionnaire.aspx", false);

            DataTable dataTableParticipantInformation = new DataTable();
            dataTableParticipantInformation = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));

            if (dataTableParticipantInformation.Rows.Count > 0)
            {
                hdnProjectId.Value = dataTableParticipantInformation.Rows[0]["ProjecctID"].ToString();

                //Set Project Logo
                if (dataTableParticipantInformation.Rows[0]["Logo"].ToString() != "")
                {
                    imageHeader.Visible = true;
                    imageHeader.ImageUrl = "~/UploadDocs/" + dataTableParticipantInformation.Rows[0]["Logo"].ToString();
                }
            }

            divWaterMark.Visible = false;
        }
        else if (identity.User.GroupID.ToString() == managerRoleId)//If manager hide show controls 
        {
            divManager.Visible = true;
            divNavigation.Visible = false;
            divParticipant.Visible = false;
            imageHeader.Visible = false;
            divWaterMark.Visible = true;
        }
        else
        {
            divNavigation.Visible = true;
            divParticipant.Visible = false;
            imageHeader.Visible = false;
            divWaterMark.Visible = true;
        }

        //AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        DataTable datatableResult = new DataTable();
        datatableResult = assignQuestionnaire_BAO.GetFeedbackURL(Convert.ToInt32(identity.User.UserID));

        if (datatableResult.Rows.Count > 0)
        {
            string url = datatableResult.Rows[0]["FeedbackUrl"].ToString();
            hdnLink.Value = url;
        }
    }
}
