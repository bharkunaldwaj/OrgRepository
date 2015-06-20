
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;
using Administration_BE;
using Miscellaneous;
using System.Diagnostics;
using System.Drawing;

using Administration_BAO;

public partial class Layouts_MasterPages_Feedback360 : System.Web.UI.MasterPage
{
    WADIdentity identity;
    CodeBehindBase codeBehindBase = new CodeBehindBase();
    User_BAO user_BAO = new User_BAO();
    static int flag = 0;
   

    protected void Page_Load(object sender, EventArgs e)
    {

        Session["FeedbackType"] = "FeedBack360";
       // if (Page.PreviousPage != null)
       // {
        //    if (Page.PreviousPage.MasterPageFile.ToString() == "Survey.master")
        if (Request.QueryString["FeedbackType"] != null)
        {
         //   Response.Write(Request.QueryString["FeedbackType"].ToString());
            if (Request.QueryString["FeedbackType"].ToString() == "Survey")
                Session["FeedbackType"] = "Survey";
        }
       // }

        try
        {
      
            identity = this.Page.User.Identity as WADIdentity;
            ////if (!IsPostBack)
            ////{
                //Set Login User Name
                lblUserName.Text = identity.User.FName.ToString() + " " + identity.User.LName.ToString();

                //Set Header Background Color
                header.Attributes.Add("style", "background:" + identity.User.HeaderBGColor);

                //Set Header Image
                imgHeader.ImageUrl = "~/UploadDocs/" + identity.User.CompanyLogo.ToString();

                //Set Menu Background Color
                menurow.Attributes.Add("style", "background:" + identity.User.MenuBGColor);

                //Set Footer
                lblFooter.Text = identity.User.CopyRightLine;

                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "hdn", "return GetDarkenColor('#" + identity.User.MenuBGColor + "',2);", true);

                //Set dark color for menu background color
                string xCol = identity.User.MenuBGColor;
                Color clr = ColorTranslator.FromHtml(xCol);
                Color colorValue = Lighten(clr, 16);
                string htmlHexColorValueThree = String.Format("#{0:X2}{1:X2}{2:X2}", colorValue.R,
                                                  colorValue.G,
                                                  colorValue.B);
                hdnMenuHoverColor.Value = htmlHexColorValueThree;
                //}
                //Build role based menu

                if (identity.User.GroupID.ToString() != ConfigurationManager.AppSettings["ParticipantRoleID"].ToString())
                BuildMenu();
            //}
            
            //codeBehindBase.HandleWriteLog("End", new StackTrace(true));
        }
        catch (Exception ex)
        {
            codeBehindBase.HandleException(ex);
        }

    }

    private void BuildMenu()
    {
        //if (Request.Url.LocalPath.ToString() == "/feedback360/Home.aspx")
        //{

        //}
        //else
        //{
       // Response.Write(Request.Url.LocalPath.ToString());
        try
        {
         
           //if (Request.Url.LocalPath.ToString() == "/feedback360/Default.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/AddParticipantBenchScores.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/AddParticipantScores.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/AssignQstnPaticipant.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/AssignQstnPaticipant.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/AssignQuestionnaire.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/AssignTemplates.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/Category.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/CategoryList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/ImportQuestions.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/ImportUser.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/Programme.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/ProgrammeList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/ProjectList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/Projects.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/QuestionList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/Questionnaire.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/QuestionnaireList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/Questions.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/UpdateAssignProgramme.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Questionnaire/ViewCandidateStatus.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/AccountList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/Accounts.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/AccountUser.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/AccountUserList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/EmailCandidate.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/EmailParticipant.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/EmailTemplates.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/EmailTemplatesList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/GroupMaintainance.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/GroupMaintenanceList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Admin/ReminderEmailHistory.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/AssignPaticipantList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/AssignQstnPaticipantList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/AssignQuestionnaireList.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/Error.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/Feedback.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/ProcessConfirmation.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/ProjectFAQ.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/QuestionReview.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Feedback/UnAuthorisePage.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Reports/DownloadZip.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Reports/ExportData.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Reports/ReportManagement.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Reports/ReportViewer.aspx" || Request.Url.LocalPath.ToString() == "/feedback360/Module/Reports/ViewList.aspx" ) 
           // //&& (int)Session["flag"] == 1
           // {
               
             //   Response.Write(Request.Url.ToString());
                StringBuilder leftMenuData = new StringBuilder();
                identity = this.Page.User.Identity as WADIdentity;
                string feedbackType = string.Empty;    
                feedbackType = HttpContext.Current.Session["FeedbackType"].ToString();


                //Nullable<var> allMenusRights;

               var allMenusRights=  from menuRight in identity.Survey_Group
                                     where menuRight.FKMenuMaster_BE.ParentID == 0 && menuRight.FKMenuMaster_BE.Visibility == "0"
                                     orderby menuRight.FKMenuMaster_BE.SortOrder
                                     select new { menuRight.FKMenuMaster_BE.MenuID, menuRight.FKMenuMaster_BE.Name, menuRight.FKMenuMaster_BE.LinkedPage, menuRight.AccessRights, menuRight.FKMenuMaster_BE.Page }; 
                if (feedbackType == "Survey")
                {
                   allMenusRights = from menuRight in identity.Survey_Group
                                         where menuRight.FKMenuMaster_BE.ParentID == 0 && menuRight.FKMenuMaster_BE.Visibility == "0"
                                         orderby menuRight.FKMenuMaster_BE.SortOrder
                                         select new { menuRight.FKMenuMaster_BE.MenuID, menuRight.FKMenuMaster_BE.Name, menuRight.FKMenuMaster_BE.LinkedPage, menuRight.AccessRights, menuRight.FKMenuMaster_BE.Page };
                }
                else
                {
                    allMenusRights = from menuRight in identity.Group
                                         where menuRight.FKMenuMaster_BE.ParentID == 0 && menuRight.FKMenuMaster_BE.Visibility == "0"
                                         orderby menuRight.FKMenuMaster_BE.SortOrder
                                     select new { menuRight.FKMenuMaster_BE.MenuID, menuRight.FKMenuMaster_BE.Name, menuRight.FKMenuMaster_BE.LinkedPage, menuRight.AccessRights, menuRight.FKMenuMaster_BE.Page };
                }
                


                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

                if (identity.User.GroupID == Convert.ToInt32(participantRoleId))
                {
                    leftMenuData.Append("<ul>");
                    leftMenuData.Append("<li><a id='pHome' onmouseover=ChangeOverBGColor('pHome','" + hdnMenuHoverColor.Value + "'); onmouseout=ChangeOutBGColor('pHome','" + identity.User.MenuBGColor + "'); href='" + ResolveClientUrl("../../Default.aspx") + "' style=background:" + identity.User.MenuBGColor + ";  ><span>Home</span></a>");
                    //leftMenuData.Append("</ul>");
                }
                else
                {
                    leftMenuData.Append("<ul>");
                }

                int i = 1;
                int j = 1;
                foreach (var allParentMenu in allMenusRights)
                {

                    leftMenuData.Append("<li><a id='p" + j + "' onmouseover=ChangeOverBGColor('p" + j + "','" + hdnMenuHoverColor.Value + "'); onmouseout=ChangeOutBGColor('p" + j + "','" + identity.User.MenuBGColor + "'); href='" + (allParentMenu.LinkedPage == "#" ? "#" : ResolveUrl(allParentMenu.LinkedPage)) + "' style=background:" + identity.User.MenuBGColor + ";  ><span>" + allParentMenu.Name + "</span></a>");
                    leftMenuData.Append("<ul>");


                    var menusAtFirstLevel = from FirstMenuRights in identity.Group
                                            let PageName = FirstMenuRights.FKMenuMaster_BE.Page
                                            where FirstMenuRights.FKMenuMaster_BE.ParentID == allParentMenu.MenuID && FirstMenuRights.FKMenuMaster_BE.Visibility == "0"
                                            orderby FirstMenuRights.FKMenuMaster_BE.SortOrder
                                            select new { FirstMenuRights.FKMenuMaster_BE.Name, PageName, FirstMenuRights.FKMenuMaster_BE.MenuID, FirstMenuRights.AccessRights, FirstMenuRights.FKMenuMaster_BE.Page, FirstMenuRights.FKMenuMaster_BE.LinkedPage, FirstMenuRights.GroupID };


                    if (feedbackType == "Survey")
                    {
                        menusAtFirstLevel = from FirstMenuRights in identity.Survey_Group
                                                let PageName = FirstMenuRights.FKMenuMaster_BE.Page
                                                where FirstMenuRights.FKMenuMaster_BE.ParentID == allParentMenu.MenuID && FirstMenuRights.FKMenuMaster_BE.Visibility == "0"
                                                orderby FirstMenuRights.FKMenuMaster_BE.SortOrder
                                                select new { FirstMenuRights.FKMenuMaster_BE.Name, PageName, FirstMenuRights.FKMenuMaster_BE.MenuID, FirstMenuRights.AccessRights, FirstMenuRights.FKMenuMaster_BE.Page, FirstMenuRights.FKMenuMaster_BE.LinkedPage, FirstMenuRights.GroupID };
                    }
                    else
                    {
                        menusAtFirstLevel = from FirstMenuRights in identity.Group
                                                let PageName = FirstMenuRights.FKMenuMaster_BE.Page
                                                where FirstMenuRights.FKMenuMaster_BE.ParentID == allParentMenu.MenuID && FirstMenuRights.FKMenuMaster_BE.Visibility == "0"
                                                orderby FirstMenuRights.FKMenuMaster_BE.SortOrder
                                                select new { FirstMenuRights.FKMenuMaster_BE.Name, PageName, FirstMenuRights.FKMenuMaster_BE.MenuID, FirstMenuRights.AccessRights, FirstMenuRights.FKMenuMaster_BE.Page, FirstMenuRights.FKMenuMaster_BE.LinkedPage, FirstMenuRights.GroupID };
                    
                    }

                    foreach (var allChildMenu in menusAtFirstLevel)
                    {
                        leftMenuData.Append("<li  ><a id='" + i + "' onmouseover=ChangeOverBGColor('" + i + "','" + hdnMenuHoverColor.Value + "'); onmouseout=ChangeOutBGColor('" + i + "','" + identity.User.MenuBGColor + "'); href='" + (allChildMenu.LinkedPage == "#" ? "#" : ResolveUrl(allChildMenu.LinkedPage)) + "' style=background:" + identity.User.MenuBGColor + "; ><span>" + allChildMenu.Name + "</span></a></li>");
                        i++;
                    }

                    leftMenuData.Append("</ul>");
                    leftMenuData.Append("</li>");

                    j++;

                }

                leftMenuData.Append("</ul>");

                smoothmenu1.InnerHtml = leftMenuData.ToString();
            //}

           
            }
        catch (Exception ex)
        {
            codeBehindBase.HandleException(ex);
        }
    }

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            codeBehindBase.LogOut();
        }
        catch (Exception ex)
        {
            codeBehindBase.HandleException(ex);
        }
    }

    public static Color Lighten(Color inColor, double inAmount)
    {
        return Color.FromArgb(
          inColor.A,
          (int)(((inColor.R - inAmount) < 0) ? 0 : (inColor.R - inAmount)),
          (int)((inColor.G - inAmount) < 0 ? 0 : (inColor.G - inAmount)),
          (int)((inColor.B - inAmount)< 0 ? 0 :(inColor.B - inAmount))) ;

        //(int)Math.Min(255, inColor.R + 255 * inAmount),
        //  (int)Math.Min(255, inColor.G + 255 * inAmount),
        //  (int)Math.Min(255, inColor.B + 255 * inAmount));
    }

    //protected string GetDarkenColor(string hexColor, int factor) 
    //{
    //        if (factor < 0) factor = 0;

    //        string c = hexColor;

    //        if (c.Substring(0, 1) == "#") {
    //            c = c.Substring(1);;
    //        }

    //        if (c.Length == 3 || c.Length == 6) {
    //            var i = c.Length / 3;

    //            var f=0;  // the relative distance from white

    //            var r = Convert.ToDouble(c.Substring(0, i), 16);
    //            f = (factor * r / (256 + r));
    //            r = Math.Floor((256 * f) / (f + 1));

    //            string str_r="";
    //            str_r = String.Format( "{0:X2}", r);
    //            if (str_r.Length == 1) str_r = "0" + str_r;

    //            //var g = Convert.ToInt32((c.Substring(i, i), 16);
    //            //f = (factor * g / (256 + g));
    //            //g = Math.Floor((256 * f) / (f + 1));

    //            //string str_g="";
    //            //str_g = String.Format( "{0:X2}", g);
    //            //if (str_g.Length == 1) str_g = "0" + str_g;

    //            //var b = Convert.ToInt32(c.Substring(2 * i, i), 16);
    //            //f = (factor * b / (256 + b));
    //            //b = Math.Floor((256 * f) / (f + 1));

    //            //string str_b="";
    //            //str_b = String.Format( "{0:X2}", b);
    //            //if (str_b.Length == 1) str_b = "0" + str_b;


    //            c = str_r;// + str_g + str_b;
    //        }
    //        //document.getElementById('ctl00_hdnMenuHoverColor').value = "#" + c;
    //        return "#" + c;
    //        //alert("#" + c);
    //    }
}
