﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Administration_BAO;
using Administration_BE;
using Miscellaneous;
using Questionnaire_BAO;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        WADIdentity identity;
        identity = this.Page.User.Identity as WADIdentity;

        string participantRoleId= ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
        string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();

        Image imgHeader = (Image)Master.FindControl("imgProjectLogo");

        if (identity.User.GroupID.ToString() == participantRoleId)
        {
            divNavigation.Visible = false;
            divParticipant.Visible = true;

            DataTable dtParticipantInfo = new DataTable();
            dtParticipantInfo = assignQuestionnaire_BAO.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));
            
            if (dtParticipantInfo.Rows.Count > 0)
            {
            hdnProjectId.Value = dtParticipantInfo.Rows[0]["ProjecctID"].ToString();

            //Set Project Logo
            
                if (dtParticipantInfo.Rows[0]["Logo"].ToString() != "")
                {
                    imgHeader.Visible = true;
                    imgHeader.ImageUrl = "~/UploadDocs/" + dtParticipantInfo.Rows[0]["Logo"].ToString();
                }
            }
            divWaterMark.Visible = false;

        }
        else if (identity.User.GroupID.ToString() == managerRoleId)
        {
            divManager.Visible = true;
            divNavigation.Visible = false;
            divParticipant.Visible = false;
            imgHeader.Visible = false;
            divWaterMark.Visible = true;
        }
        else
        {
            divNavigation.Visible = true;
            divParticipant.Visible = false;
            imgHeader.Visible = false;
            divWaterMark.Visible = true;
        }

        //AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        DataTable dtResult = new DataTable();
        dtResult = assignQuestionnaire_BAO.GetFeedbackURL(Convert.ToInt32(identity.User.UserID));

        if (dtResult.Rows.Count > 0)
        {
            string url=dtResult.Rows[0]["FeedbackUrl"].ToString();
            hdnLink.Value = url;           
        }
    }

    protected void hdnLink_ValueChanged(object sender, EventArgs e)
    {


    }
}
