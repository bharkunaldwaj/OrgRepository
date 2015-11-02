using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.Text;
using Miscellaneous;
using System.Net.Mail;

using Questionnaire_BE;
using Questionnaire_BAO;
using Administration_BE;
using Administration_BAO;
using System.IO;
using Microsoft.Reporting.WebForms;
using Admin_BAO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Admin_BE;

public partial class Survey_Module_Feedback_Feedback : System.Web.UI.Page
{

    string LogFilePath = string.Empty;
    string mimeType;
    string encoding;
    string fileNameExtension;
    string extension, deviceInfo, outputFileName = "";
    string[] streams;
    string defaultFileName = string.Empty;
    Warning[] warnings;

    Questionnaire_BAO.Survey_Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
    Survey_AssignQuestionnaire_BAO Survey_assignQuestionnaire_BAO = new Survey_AssignQuestionnaire_BAO();
    Survey_Project_BAO project_BAO = new Survey_Project_BAO();
    Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
    Survey_AccountUser_BAO accountUser_BAO = new Survey_AccountUser_BAO();
    Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
    Survey_ReportManagement_BAO reportManagement_BAO = new Survey_ReportManagement_BAO();
    Survey_ReportManagement_BE reportManagement_BE = new Survey_ReportManagement_BE();
    Survey_AssignQstnParticipant_BAO assignQstnParticipant_BAO = new Survey_AssignQstnParticipant_BAO();
    DataTable dtQuestion = new DataTable();
    DataTable dtCategory = new DataTable();
    DataTable dtRange = new DataTable();
    Int32 questionCount = 0;
    Int32 currentCount = 0;
    Int32 categoryCount = 0;
    CodeBehindBase cBase = new CodeBehindBase();
    string candidateID;
    string questionnaireID;
    string Template;
    int x = 1;

    string strGroupList;
    string strFrontPage;
    string strReportIntroduction;
    string strConclusionPage;
    string strRadarChart;
    string strPageHeadingColor;
    string strDetailedQst;
    string strCategoryQstlist;
    string strCategoryBarChart;
    string strSelfNameGrp;
    string strProgrammeGrp;
    string strReportName;
    string strReportType;
    string strStaticBarLabelVisibility;
    string strConclusionHeading;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //Label ll = (Label)this.Master.FindControl("Current_location");
        //ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        ////Get the Candidate Information 
        //string qID = PasswordGenerator.EnryptString("140");
        //string cID = PasswordGenerator.EnryptString("3818");

        DataTable dtResult = new DataTable();
        try
        {
            if (!IsPostBack)
            {
                ViewState["ValidationCheck"] = "";

                string str = "<table>";

                for (int i = 455; i <= 679; i++)
                {
                    str = str + "<tr><td>" + i.ToString() + "</td><td>" + PasswordGenerator.EnryptString(i.ToString()) + "</td></tr>";
                }

                str = str + "</table>";

                if (Request.QueryString["QID"] != null && Request.QueryString["CID"] != null)
                {

                    candidateID = Convert.ToString(Request.QueryString["CID"]);
                    candidateID = PasswordGenerator.DecryptString(candidateID);
                    hdnCandidateId.Value = candidateID;

                    //Get questionnaire ID
                    questionnaireID = Convert.ToString(Request.QueryString["QID"]);
                    questionnaireID = PasswordGenerator.DecryptString(questionnaireID);
                    hdnQuestionnaireId.Value = questionnaireID;

                    imbStart.Visible = true;

                    dtResult = Survey_assignQuestionnaire_BAO.GetAllAssignmentInfo(Convert.ToInt32(candidateID));

                    if (dtResult.Rows.Count > 0)
                    {
                        //Setting Questionnaire Project Information
                        DataTable dtProjectInfo = new DataTable();
                        dtProjectInfo = questionnaire_BAO.GetProjectQuestionnaireInfo(Convert.ToInt32(questionnaireID), Convert.ToInt32(candidateID));

                        hdnAccountId.Value = dtResult.Rows[0]["AccountID"].ToString();

                        hdnProjectId.Value = dtProjectInfo.Rows[0]["ProjectID"].ToString();
                        lblProjectName.Text = dtProjectInfo.Rows[0]["Title"].ToString();
                        lblParticipantName.Text = dtProjectInfo.Rows[0]["FullName"].ToString();

                        hdnFirstName.Value = dtProjectInfo.Rows[0]["FirstName"].ToString();
                        hdnLastName.Value = dtProjectInfo.Rows[0]["LastName"].ToString();
                        // hdnRelationship.Value = dtResult.Rows[0]["RelationShip"].ToString();

                        //Set Client Name
                        DataTable dtProgramme = new DataTable();
                        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
                        dtProgramme = programme_BAO.GetProgrammeByID(Convert.ToInt32(dtResult.Rows[0]["ProgrammeID"]));

                        hdnClientName.Value = dtProgramme.Rows[0]["ClientName"].ToString();

                        //Set Increment Value
                        hdnIncrementValue.Value = dtProjectInfo.Rows[0]["DisplayCategory"].ToString();

                        //Set Login User Name
                        lblUserName.Text = dtResult.Rows[0]["CandidateName"].ToString();

                        //Set Header Background Color
                        tdHeader.Attributes.Add("style", "background:" + dtResult.Rows[0]["HeaderBGColor"].ToString() + ";");

                        //Set Header Image
                        imgHeader.ImageUrl = "~/UploadDocs/" + dtResult.Rows[0]["CompanyLogo"].ToString().Trim();
                       
                        imgProjectLogo.ImageUrl = string.Format("~/UploadDocs/{0}", dtResult.Rows[0].Field<string>("QuestLogo").Trim());

                        if (string.IsNullOrEmpty(dtResult.Rows[0].Field<string>("QuestLogo")))
                        {
                            imgProjectLogo.Visible = false;
                        }

                        //Set Footer Copyright Line
                        lblFooter.Text = dtResult.Rows[0]["CopyRightLine"].ToString();

                        ////Set Project Logo
                        if (dtResult.Rows[0]["Logo"].ToString() != "")
                        {
                            imgProjectLogo.Visible = true;
                            imgProjectLogo.ImageUrl = "~/UploadDocs/" + dtResult.Rows[0]["Logo"].ToString();
                        }
                        //else
                        //{
                        //    imgProjectLogo.Visible = false;
                        //}

                        if (Session["DTEXLINK"] != null)
                        {
                            DataTable dtExLink = (DataTable)Session["DTEXLINK"];
                           
                            //Set Programme logo
                            // DataTable dtCompany = new DataTable();
                            Survey_Company_BAO company_BAO = new Survey_Company_BAO();
                            List<Survey_Company_BE> dtCompany = company_BAO.GetCompanyByID(Convert.ToInt32(dtExLink.Rows[0]["CompanyID"]));
                            if (dtCompany != null && dtCompany.Count > 0)
                            {
                                Survey_Company_BE company = dtCompany.First();
                                if (company.QuestLogo != null && company.QuestLogo != "")
                                {
                                    imgProjectLogo.Visible = true;
                                    imgProjectLogo.ImageUrl = "~/UploadDocs/" + company.QuestLogo.ToString();
                                }
                                else
                                {
                                    imgProjectLogo.Visible = false;
                                }
                            }

                        }
                        

                        //Set Programme Logo
                        //Ak

                        if (dtProgramme.Rows[0]["Logo"].ToString() != "")
                        {
                            imgProjectLogo.Visible = true;
                            imgProjectLogo.ImageUrl = "~/UploadDocs/" + dtProgramme.Rows[0]["Logo"].ToString();
                        }
                        //else
                        //{
                        //    if (imgProjectLogo.ImageUrl == "")
                        //        imgProjectLogo.Visible = false;
                        //}

                        //Set Menu Background Color
                        tdMenuBar.Attributes.Add("style", "background:" + dtResult.Rows[0]["MenuBGColor"].ToString() + ";text-align:right;padding-right:15px; padding-top:3px; color:#ffffff;");

                        Session["Count"] = 0;
                        BindQuestionInformation();
                    }
                    else
                    {
                        //lblUnAuthorisedMessage.Text = "You are not an authorised candidate";
                        Response.Redirect("UnAuthorisePage.aspx", false);
                    }
                }
            }

            //Set Graph Details
            if (dtResult.Rows.Count > 0)
                SetGraphData();

        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    private void SetGraphData()
    {
        int answeredQuestion = questionnaire_BAO.CalculateGraph(Convert.ToInt32(hdnQuestionnaireId.Value), Convert.ToInt32(hdnCandidateId.Value));


        if (hdnQuestionCount.Value == "0")
            hdnQuestionCount.Value = "1";

        double percentage = (answeredQuestion * 100) / Convert.ToInt32(hdnQuestionCount.Value);
        percentage = Convert.ToInt32(Math.Abs(percentage));
        tbGraph.Width = percentage.ToString() + "%";

        lblCompletionStatus.Text = percentage + "%";
        //lblCompletionStatus.ForeColor = System.Drawing.Color.Red;

        //Set Prolog of questionnaire
        List<Questionnaire_BE.Survey_Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Survey_Questionnaire_BE>();
        questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(Convert.ToInt32(hdnQuestionnaireId.Value));
        lblQuestionnaireText.Text = Server.HtmlDecode(questionnaire_BEList[0].QSTNPrologue.ToString());
    }

    private void BindQuestionInformation()
    {
        try
        {
            //Get questionnaire ID
            string questionnaireID = Convert.ToString(Request.QueryString["QID"]);
            questionnaireID = PasswordGenerator.DecryptString(questionnaireID);


            dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaire(Convert.ToInt32(questionnaireID));
            Session["Questions"] = dtQuestion;

            dtCategory = questionnaire_BAO.GetQuestionnaireCategories(Convert.ToInt32(questionnaireID));
            Session["Category"] = dtCategory;


            Session["categoryCount"] = (Math.Abs(dtCategory.Rows.Count / Convert.ToInt32(hdnIncrementValue.Value)));

            if (dtCategory.Rows.Count % Convert.ToInt32(hdnIncrementValue.Value) > 0)
                Session["categoryCount"] = Convert.ToInt32(Session["categoryCount"]) + 1;
            //  int tt=        Convert.ToInt32(Session["categoryCount"]) + 1;
            questionCount = dtQuestion.Rows.Count;
            hdnQuestionCount.Value = questionCount.ToString();

            if (questionCount > 0)
            {
                DataTable dtCat = new DataTable();
                dtCat = (DataTable)Session["Category"];
                currentCount = Convert.ToInt32(Session["Count"]);

                BindQuestions(currentCount);
                SetQuestionAnswer2();

                divText.Visible = true;
                cbxNotifyMail.Visible = false;

                rptrQuestionListMain.Visible = false;
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    private void BindQuestions(int qstCount)
    {
        //try
        //{
        int countFrom, countTo;

        if (qstCount > 0)
        {
            countFrom = (qstCount * Convert.ToInt32(hdnIncrementValue.Value)) + 1;
            countTo = countFrom + Convert.ToInt32(hdnIncrementValue.Value) - 1;
        }
        else
        {
            countFrom = 1;
            countTo = Convert.ToInt32(hdnIncrementValue.Value);
        }

        DataTable dt = new DataTable();
        dt = (DataTable)Session["Category"];

        DataTable dtClone = dt.Clone();

        DataRow[] result = dt.Select("RowNumber >=" + countFrom + " and RowNumber <=" + countTo);

        //DataRow[] result = dt.Select("CategoryID=" + qstCount,"Sequence" );

        foreach (DataRow dr in result)
            dtClone.ImportRow(dr);

        rptrQuestionListMain.DataSource = dtClone;
        Session["CategoryId"] = dtClone.Rows[0]["CategoryID"].ToString();
        rptrQuestionListMain.DataBind();


    }
    
    private void SetQuestionAnswer2()
    {
        try
        {
            int questionID = 0;
            int candidateId = 0;

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");
                foreach (RepeaterItem item in rptrQuestionList.Items)
                {
                    Label lblQuestionID = (Label)item.FindControl("lblQuestionID");
                    Label lblQuestionType = (Label)item.FindControl("lblQuestionType");


                    if (lblQuestionID != null)
                        questionID = Convert.ToInt32(lblQuestionID.Text);
                    candidateId = Convert.ToInt32(hdnCandidateId.Value);

                    Survey_QuestionAnswer_BAO questionAnswer_BAO = new Survey_QuestionAnswer_BAO();
                    string answer = questionAnswer_BAO.GetQuestionAnswer(candidateId, questionID);

                    if (Convert.ToInt16(lblQuestionType.Text) == 1)
                    {
                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //{
                        //    txtAnswer.Text = answer;
                        //}

                        CKEditor.NET.CKEditorControl txtAnswers = (CKEditor.NET.CKEditorControl)item.FindControl("txtAnswers");
                        if (txtAnswers != null)
                        {
                            txtAnswers.config.toolbar = new object[] { };
                            txtAnswers.config.keystrokes = new object[] { };
                            txtAnswers.CssClass = "";
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = true;
                            txtAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                            txtAnswers.AutoCompleteType = AutoCompleteType.None;
                            txtAnswers.AutoParagraph=false;
                            txtAnswers.ScaytAutoStartup=true;
                            txtAnswers.BrowserContextMenuOnCtrl=false;
                            txtAnswers.ForcePasteAsPlainText=false;
                            txtAnswers.IgnoreEmptyParagraph=true;
                            txtAnswers.ContentsLangDirection= CKEditor.NET.contentsLangDirections.Ltr;
                            txtAnswers.EnableTabKeyTools=false;
                            txtAnswers.EnterMode= CKEditor.NET.EnterMode.BR;
                            txtAnswers.Entities=false;
                            txtAnswers.PasteFromWordNumberedHeadingToList=false;
                            txtAnswers.PasteFromWordRemoveStyles= true;


                            txtAnswers.Text = answer;
                        }
                    }
                    else
                    {
                        RadioButtonList rblAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        rblAnswer.Visible = true;
                        rblAnswer.CellPadding = 5;
                        rblAnswer.CellSpacing = 5;



                        if (rblAnswer != null && answer != "")
                        {
                            for (int i = 0; i < rblAnswer.Items.Count; i++)
                            {
                                if (rblAnswer.Items[i].Value == answer)
                                {
                                    rblAnswer.Items[i].Selected = true;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }
    
    private void SetQuestionAnswer()
    {
        try
        {
            int questionID = 0;
            int candidateId = 0;

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");
                foreach (RepeaterItem item in rptrQuestionList.Items)
                {
                    Label lblQId = (Label)item.FindControl("lblQId");
                    Label lblQuestionType = (Label)item.FindControl("lblQuestionType");


                    if (lblQId != null)
                        questionID = Convert.ToInt32(lblQId.Text);
                    candidateId = Convert.ToInt32(hdnCandidateId.Value);

                    Survey_QuestionAnswer_BAO questionAnswer_BAO = new Survey_QuestionAnswer_BAO();
                    string answer = questionAnswer_BAO.GetQuestionAnswer(candidateId, questionID);

                    if (Convert.ToInt16(lblQuestionType.Text) == 1)
                    {
                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //{
                        //    txtAnswer.Text = answer;
                        //}
                        CKEditor.NET.CKEditorControl txtAnswers = (CKEditor.NET.CKEditorControl)item.FindControl("txtAnswers");
                        if (txtAnswers != null)
                        {
                            txtAnswers.config.toolbar = new object[] { };
                            txtAnswers.config.keystrokes = new object[] { };
                            txtAnswers.CssClass = "";
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = true;
                            txtAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                            txtAnswers.AutoCompleteType = AutoCompleteType.None;
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = false;
                            txtAnswers.IgnoreEmptyParagraph = true;
                            txtAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            txtAnswers.EnableTabKeyTools = false;
                            txtAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            txtAnswers.Entities = false;
                            txtAnswers.PasteFromWordNumberedHeadingToList = false;
                            txtAnswers.PasteFromWordRemoveStyles = true;

                            txtAnswers.Text = answer;
                        }
                    }
                    else
                    {
                        RadioButtonList rblAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        rblAnswer.Visible = true;
                        rblAnswer.CellPadding = 5;
                        rblAnswer.CellSpacing = 5;



                        if (rblAnswer != null && answer != "")
                        {
                            for (int i = 0; i < rblAnswer.Items.Count; i++)
                            {
                                if (rblAnswer.Items[i].Text == answer)
                                {
                                    rblAnswer.Items[i].Selected = true;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void SaveQuestionAnswer()
    {
        try
        {

            Survey_QuestionAnswer_BAO questionAnswer_BAO = new Survey_QuestionAnswer_BAO();

            string answer = "";
            int questionID = 0;

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");
                foreach (RepeaterItem item in rptrQuestionList.Items)
                {
                    Survey_QuestionAnswer_BE questionAnswer_BE = new Survey_QuestionAnswer_BE();

                    Label lblQType = (Label)item.FindControl("lblQuestionType");
                    Label lblQId = (Label)item.FindControl("lblQId");

                    Label lblQuestionID = (Label)item.FindControl("lblQuestionID");
                    questionID = Convert.ToInt32(lblQuestionID.Text);

                    if (Convert.ToInt16(lblQType.Text) == 1)
                    {
                        CKEditor.NET.CKEditorControl txtAnswers = (CKEditor.NET.CKEditorControl)item.FindControl("txtAnswers");
                        if (txtAnswers != null)
                        {
                            txtAnswers.config.toolbar = new object[] { };
                            
                            txtAnswers.config.keystrokes = new object[] { };
                            txtAnswers.CssClass = "";
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = true;
                            txtAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                            txtAnswers.AutoCompleteType = AutoCompleteType.None;
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = false;
                            txtAnswers.IgnoreEmptyParagraph = true;
                            txtAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            txtAnswers.EnableTabKeyTools = false;
                            txtAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            txtAnswers.Entities = false;
                            txtAnswers.PasteFromWordNumberedHeadingToList = false;
                            txtAnswers.PasteFromWordRemoveStyles = true;

                            answer = txtAnswers.Text;
                        }

                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //{
                        //    answer = txtAnswer.Text;
                        //}
                    }
                    else
                    {
                        RadioButtonList rblAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        if (rblAnswer != null)
                        {
                            for (int i = 0; i < rblAnswer.Items.Count; i++)
                            {
                                if (rblAnswer.Items[i].Selected == true)
                                {
                                    answer = rblAnswer.Items[i].Value.ToString();
                                    break;
                                }
                                else
                                    answer = "";
                            }

                        }
                    }

                    questionAnswer_BE.AssignDetId = Convert.ToInt32(hdnCandidateId.Value);
                    questionAnswer_BE.QuestionID = questionID;
                    questionAnswer_BE.Answer = answer;
                    questionAnswer_BE.ModifyBy = 1;
                    questionAnswer_BE.ModifyDate = DateTime.Now;
                    questionAnswer_BE.IsActive = 1;

                    questionAnswer_BAO.AddQuestionAnswer(questionAnswer_BE);
                }
            }
            SetGraphData();
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected Int32 CheckQuestionAnswer()
    {
        int result = 0;
        try
        {
            Survey_QuestionAnswer_BAO questionAnswer_BAO = new Survey_QuestionAnswer_BAO();
            string answer = "";

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");
                foreach (RepeaterItem item in rptrQuestionList.Items)
                {
                    Label lblValidation = (Label)item.FindControl("lblValidation");
                    Label lblQType = (Label)item.FindControl("lblQuestionType");
                    Label lblQId = (Label)item.FindControl("lblQId");
                    Label lblQuestionID = (Label)item.FindControl("lblQuestionID");


                    if (Convert.ToInt16(lblQType.Text) == 1)
                    {
                        CKEditor.NET.CKEditorControl txtAnswers = (CKEditor.NET.CKEditorControl)item.FindControl("txtAnswers");
                        if (txtAnswers != null)
                        {
                            txtAnswers.config.toolbar = new object[] { };

                            txtAnswers.config.keystrokes = new object[] { };
                            txtAnswers.CssClass = "";
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = true;
                            txtAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                            txtAnswers.AutoCompleteType = AutoCompleteType.None;
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = false;
                            txtAnswers.IgnoreEmptyParagraph = true;
                            txtAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            txtAnswers.EnableTabKeyTools = false;
                            txtAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            txtAnswers.Entities = false;
                            txtAnswers.PasteFromWordNumberedHeadingToList = false;
                            txtAnswers.PasteFromWordRemoveStyles = true;

                            answer = txtAnswers.Text;
                        }

                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //{
                        //    answer = txtAnswer.Text;
                        //}
                    }
                    else
                    {
                        RadioButtonList rblAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        if (rblAnswer != null)
                        {
                            if (rblAnswer.SelectedItem != null)
                                answer = rblAnswer.SelectedItem.Text;
                            else
                                answer = "";
                        }
                    }

                    if (lblValidation.Text == "2" && answer == string.Empty && result < 3)
                    {
                        if (Convert.ToString(ViewState["ValidationCheck"]) == "0" && result < 2)
                            result = 1;
                        else
                            result = 2;
                    }
                    else if (lblValidation.Text == "3" && answer == string.Empty)
                        result = 3;

                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
        return result;
    }

    protected void imbPrevious_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int result = CheckQuestionAnswer();

            if (imbFinish.Visible == true) result = 1;

            if (result <= 1)
            {
                SaveQuestionAnswer();

                Session["Count"] = Convert.ToInt32(Session["Count"]) - 1;

                if (Convert.ToInt32(Session["Count"]) < 0)
                {
                    divText.Visible = true;
                    cbxNotifyMail.Visible = false;
                    rptrQuestionListMain.Visible = false;


                    imbStart.Visible = true;
                    imbFinish.Visible = false;
                    imbNext.Visible = false;
                    imbPrevious.Visible = false;
                }
                else if (Convert.ToInt32(Session["Count"]) == 0)
                {
                    divText.Visible = false;
                    rptrQuestionListMain.Visible = true;


                    imbStart.Visible = false;
                    imbFinish.Visible = false;
                    imbNext.Visible = true;
                    imbPrevious.Visible = true;

                    DataTable dtCat = new DataTable();
                    dtCat = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);

                    BindQuestions(currentCount);
                    SetQuestionAnswer2();

                }
                else
                {
                    divText.Visible = false;
                    rptrQuestionListMain.Visible = true;


                    imbStart.Visible = false;
                    imbFinish.Visible = false;
                    imbNext.Visible = true;
                    imbPrevious.Visible = true;

                    DataTable dtCat = new DataTable();
                    dtCat = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);

                    BindQuestions(currentCount);
                    SetQuestionAnswer2();

                }

                ViewState["ValidationCheck"] = "";
                lblMessage.Text = "";
            }
            else
            {
                if (result == 2)
                    ViewState["ValidationCheck"] = "0";

                lblMessage.Text = "<br><b>Warning: You will have to complete the answer before moving further</b>";
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void imbNext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            int result = CheckQuestionAnswer();

            if (result <= 1)
            {
                SaveQuestionAnswer();
                int x = Convert.ToInt32(Session["Count"]);
                Session["Count"] = Convert.ToInt32(Session["Count"]) + 1;
                x = Convert.ToInt32(Session["Count"]);
                int y = Convert.ToInt32(Session["categoryCount"]);
                if ((Convert.ToInt32(Session["Count"])) == Convert.ToInt32(Session["categoryCount"]))
                {
                    divText.Visible = true;
                    rptrQuestionListMain.Visible = false;
                    imbStart.Visible = false;
                    imbFinish.Visible = true;
                    imbNext.Visible = false;
                    imbPrevious.Visible = true;
                    cbxNotifyMail.Visible = true;

                    //Get questionnaire ID
                    string questionnaireID = Convert.ToString(Request.QueryString["QID"]);
                    questionnaireID = PasswordGenerator.DecryptString(questionnaireID);

                    //Set Prolog of questionnaire
                    List<Questionnaire_BE.Survey_Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Survey_Questionnaire_BE>();
                    questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(Convert.ToInt32(questionnaireID));
                    lblQuestionnaireText.Text = questionnaire_BEList[0].QSTNEpilogue.ToString();
                }
                else
                {
                    imbNext.Visible = true;
                    imbPrevious.Visible = true;

                    DataTable dtCat = new DataTable();
                    dtCat = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);

                    BindQuestions(currentCount);
                    SetQuestionAnswer2();
                }

                ViewState["ValidationCheck"] = "";
                lblMessage.Text = "";
            }
            else
            {
                if (result == 2)
                    ViewState["ValidationCheck"] = "0";

                lblMessage.Text = "<br><b>Warning: You will have to complete the answer before moving further</b>";
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void imbFinish_Click(object sender, ImageClickEventArgs e)
    {
        imbFinish.Visible = false;
        imbPrevious.Visible = false;
        string urlPath = ConfigurationManager.AppSettings["SurveyFeedbackURL"].ToString();
        DataTable dtProjectInfo = new DataTable();
        dtProjectInfo = questionnaire_BAO.GetProjectQuestionnaireInfo(Convert.ToInt32(hdnQuestionnaireId.Value), Convert.ToInt32(hdnCandidateId.Value));

        DataTable dtResult = new DataTable();
        Survey_AssignQuestionnaire_BAO assignQuestionnaire_BAO = new Survey_AssignQuestionnaire_BAO();
        dtResult = assignQuestionnaire_BAO.GetAllAssignmentInfo(Convert.ToInt32(hdnCandidateId.Value));
        int result = questionnaire_BAO.UpdateSubmitFlag(Convert.ToInt32(hdnCandidateId.Value), 1);
        //Send mail to candidates
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {

            String strFinishEmailTemplateID =   dtResult.Rows[i]["EmailFinishEmailTemplate"].ToString();//System.IO.File.ReadAllText(Server.MapPath("~") + "\\UploadDocs\\Survey_Feedback_Template.txt");
            List<Survey_EmailTemplate_BE> emailtemplate_BEList = new List<Survey_EmailTemplate_BE>();
            Survey_EmailTemplate_BAO emailtemplate_BAO = new Survey_EmailTemplate_BAO();
            emailtemplate_BEList = emailtemplate_BAO.GetEmailTemplateByID(0, Convert.ToInt32(strFinishEmailTemplateID));
            string TemplateSubject = string.Empty;

            if (emailtemplate_BEList.Any())
            {
                Template = emailtemplate_BEList.FirstOrDefault().EmailText;
                TemplateSubject = emailtemplate_BEList.FirstOrDefault().Subject;
            }

            
            string candidatename = "";
            string candidateemail = "";
            string organizationname = "";
            string Projectname = "";
            string participantname = "";
            string finishEMailID = "";
            string ProgrammeName = "";
          
            candidatename = dtResult.Rows[i]["CandidateName"].ToString();
            candidateemail = dtResult.Rows[i]["CandidateEmail"].ToString();
            organizationname = dtResult.Rows[i]["OrganisationName"].ToString();
            Projectname = dtProjectInfo.Rows[i]["Title"].ToString();
            ProgrammeName = dtResult.Rows[i]["ProgrammeName"].ToString();
            finishEMailID = dtProjectInfo.Rows[i]["Finish_EmailID"].ToString();

            string[] candnameArr = candidatename.Split(new char[] { ' ' });

            Template = Template.Replace("[TITLE]", Projectname);
            Template = Template.Replace("[EMAILID]", candidateemail);
            if (candnameArr.Length>1)
                Template = Template.Replace("[FIRSTNAME]", candnameArr[0]);
            else
                Template = Template.Replace("[FIRSTNAME]", candidatename);

            Template = Template.Replace("[COMPANY]", organizationname);
            Template = Template.Replace("[PARTICIPANTNAME]", candidatename);
            Template = Template.Replace("[PROGRAMMENAME]", ProgrammeName);


            //Upodate Subject for email template..
            TemplateSubject = TemplateSubject.Replace("[TITLE]", Projectname);
            TemplateSubject = TemplateSubject.Replace("[EMAILID]", candidateemail);
            if (candnameArr.Length>1)
                TemplateSubject = TemplateSubject.Replace("[FIRSTNAME]", candnameArr[0]);
            else
                TemplateSubject = TemplateSubject.Replace("[FIRSTNAME]", candidatename);

            TemplateSubject = TemplateSubject.Replace("[COMPANY]", organizationname);
            TemplateSubject = TemplateSubject.Replace("[PARTICIPANTNAME]", candidatename);
            TemplateSubject = TemplateSubject.Replace("[PROGRAMMENAME]", ProgrammeName);
             

              MailAddress maddr = new MailAddress(candidateemail, candidatename);
            // string feedbackurl = urlPath + "Feedback.aspx";

            Survey_AssignQuestionnaire_BAO get_email_finish_info = new Survey_AssignQuestionnaire_BAO();
            int intCompanyId = 0;
            if (Session["DTEXLINK"] != null)
            {
                DataTable dtExLink = (DataTable)Session["DTEXLINK"];
                intCompanyId = Convert.ToInt32(dtExLink.Rows[0]["CompanyID"]);

            }

            bool bb = get_email_finish_info.find_finish_email(hdnProjectId.Value, intCompanyId);
            if (bb)
            {
                //MailAddress maddr = new MailAddress(participantemail, participantname);
                SendEmail.Send(TemplateSubject, Template, finishEMailID, "");  //, maddr, urlPath);

                lblMessage.Text = "Survey Questionnaire has been submitted and Thank you for completing the questionnaire.";
            }

            else
            {
                MailAddress mailaddress = new MailAddress(candidateemail, candidatename);
                //SendEmail.Send("Survey Questionnaire Feedback Submitted", Template, candidateemail, "");
                SendEmail.Send(TemplateSubject, Template, finishEMailID, mailaddress, "");  

                lblMessage.Text = "Survey Questionnaire has been submitted and Thank you for completing the questionnaire.";
            }
        }

        if (Session["DTEXLINK"] != null)
        {
            DataTable dtExLink = (DataTable)Session["DTEXLINK"];
            bool chkSendEmailOnCompletion = Convert.ToBoolean(dtExLink.Rows[0]["SendEmailOnCompletion"].ToString());
            if (chkSendEmailOnCompletion)
            {
                if (dtExLink != null && dtExLink.Rows.Count > 0)
                {
                    string reportPath = GenerateReport(Convert.ToString(dtExLink.Rows[0]["AccountId"]), Convert.ToString(dtExLink.Rows[0]["ProjectId"]), Convert.ToString(dtExLink.Rows[0]["ProgrammeId"]));

                    string participantemail = "";
                    string participantname = "";
                    string emailtemplateId = "";
                    string templateString = "";
                    string emailSubject = "";
                    
                    string accountCode = "";
                    string programmeName = "";
                    string OrganisationName = "";
                    string Startdate = "";
                    string Enddate = "";
                    string FirstName = "";
                    string strLogin = "";
                    string strPassword = "";
                    string link = "";

                    if (Session["ParticpantEmail"] != null)
                        participantemail = Convert.ToString(Session["ParticpantEmail"]);
                    if (Session["ParticpantName"] != null)
                        participantname = Convert.ToString(Session["ParticpantName"]);
                    emailtemplateId = Convert.ToString(dtExLink.Rows[0]["EmailTemplateId"]);

                    List<Survey_EmailTemplate_BE> emailtemplate_BEList = new List<Survey_EmailTemplate_BE>();
                    Survey_EmailTemplate_BAO emailtemplate_BAO = new Survey_EmailTemplate_BAO();
                    emailtemplate_BEList = emailtemplate_BAO.GetEmailTemplateByID(0, Convert.ToInt32(emailtemplateId));
                    if (emailtemplate_BEList.Any())
                    {
                        templateString = emailtemplate_BEList.FirstOrDefault().EmailText;
                        emailSubject = emailtemplate_BEList.FirstOrDefault().Subject;
                    }
                    //-----------------------------------------------------------------
                    //Save Assign questionnaire
                    //assignmentID = assignquestionnaire_BAO.AddAssignQuestionnaire(assignquestionnaire_BE);

                    //DataTable dtResult = new DataTable();
                    //dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireList(assignmentID);
                        DateTime dtStartdate = Convert.ToDateTime(dtExLink.Rows[0]["StartDate"]).Date;
                        DateTime dtEnddate = Convert.ToDateTime(dtExLink.Rows[0]["Enddate"]).Date;
                    DateTime dtToday = DateTime.Now.Date;
                    
                    link = Convert.ToString(dtExLink.Rows[0]["ExternalLink"].ToString());
                   
                    OrganisationName = Convert.ToString(dtExLink.Rows[0]["CompanyName"].ToString());
                    Startdate = Convert.ToDateTime(dtExLink.Rows[0]["StartDate"]).ToString("dd-MMM-yyyy");
                    Enddate = Convert.ToDateTime(dtExLink.Rows[0]["Enddate"]).ToString("dd-MMM-yyyy");
                    accountCode = Convert.ToString(dtExLink.Rows[0]["AccountCode"].ToString());
                    programmeName = Convert.ToString(dtExLink.Rows[0]["ProgrammeName"].ToString());
                    string[] strFName = participantname.Split(' ');
                    FirstName = strFName[0].ToString();
                    //accountCode = ddlAccountCode.SelectedItem.Text;
                    strLogin = strPassword = FirstName;
                    bool blnSendReportToParticipant = Convert.ToBoolean(dtExLink.Rows[0]["SendReportToParticipant"].ToString());

                    
                    templateString = templateString.Replace("[LINK]", link);
                    templateString = templateString.Replace("[NAME]", participantname);
                    templateString = templateString.Replace("[FIRSTNAME]", FirstName);
                    templateString = templateString.Replace("[COMPANY]", OrganisationName);
                    templateString = templateString.Replace("[STARTDATE]", Startdate);
                    templateString = templateString.Replace("[CLOSEDATE]", Enddate);
                    templateString = templateString.Replace("[IMAGE]", "<img src=cid:companylogo>");
                    templateString = templateString.Replace("[CODE]", accountCode);
                    templateString = templateString.Replace("[LOGINID]", strLogin);
                    templateString = templateString.Replace("[PASSWORD]", strPassword);
                    templateString = templateString.Replace("[ACCOUNTCODE]", accountCode);
                    templateString = templateString.Replace("[PROGRAMMENAME]", programmeName);
                    templateString = templateString.Replace("[PARTICIPANTNAME]", participantname);

                    emailSubject = emailSubject.Replace("[NAME]", participantname);
                    emailSubject = emailSubject.Replace("[PARTICIPANTNAME]", participantname);
                    emailSubject = emailSubject.Replace("[FIRSTNAME]", FirstName);
                    emailSubject = emailSubject.Replace("[COMPANY]", OrganisationName);
                    emailSubject = emailSubject.Replace("[STARTDATE]", Startdate);
                    emailSubject = emailSubject.Replace("[CLOSEDATE]", Enddate);
                    emailSubject = emailSubject.Replace("[PROGRAMMENAME]", programmeName);
                    //-----------------------------------------------------------------------
                    MailAddress maddr = new MailAddress(participantemail, participantname);

                    if (blnSendReportToParticipant == false)
                        reportPath = "";
                    if (Convert.ToString(dtExLink.Rows[0]["EmailTo"]).ToUpper() == "BOTH")
                    {
                        SendEmail.Send(emailSubject, templateString, participantemail, reportPath);
                        string Finish_EmailID = Convert.ToString(dtExLink.Rows[0]["Finish_EmailID"]);
                        SendEmail.Send(emailSubject, templateString, Finish_EmailID, reportPath);
                    }
                    else if (Convert.ToString(dtExLink.Rows[0]["EmailTo"]).ToUpper() == "PARTICIPANT")
                    {
                        SendEmail.Send(emailSubject, templateString, participantemail, reportPath);
                    }
                    else if (Convert.ToString(dtExLink.Rows[0]["EmailTo"]).ToUpper() == "EMAIL")
                    {
                        SendEmail.Send(emailSubject, templateString, Convert.ToString(dtExLink.Rows[0]["CustomEmail"]), reportPath);
                    }
                }
            }
        }
    }

    protected void imbStart_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["Count"] = 0;
            BindQuestionInformation();

            divText.Visible = false;
            rptrQuestionListMain.Visible = true;

            imbStart.Visible = false;
            imbFinish.Visible = false;
            imbNext.Visible = true;
            imbPrevious.Visible = true;


        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void rptrQuestionList_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        string btnID = ((RadioButton)e.CommandSource).ID.ToString();
        if (btnID == "rbtnNotApplicable")
        {

        }
    }

    protected void rptrQuestionList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {


        try
        {
            RepeaterItem rpItem = e.Item;
            int itenIndex = Convert.ToInt32(e.Item.ItemIndex);
            Label lblQType = (Label)rpItem.FindControl("lblQuestionType");
            Label lblQstId = (Label)rpItem.FindControl("lblQId");
            Label lblQuestionText = (Label)rpItem.FindControl("lblQuestionText");
            Label lblQuestionHint = (Label)rpItem.FindControl("lblHint");
            HtmlTable tblGraph = (HtmlTable)rpItem.FindControl("tblGraph");

            if (lblQType != null)
            {

                DataTable dt = new DataTable();
                dt = (DataTable)Session["Questions"];

                DataTable dtClone = dt.Clone();

                DataTable dtCat = new DataTable();
                dtCat = (DataTable)Session["Category"];

                DataRow[] result = dt.Select("QuestionnaireID =" + lblQstId.Text + "and CategoryID=" + Session["CategoryID"]);

                foreach (DataRow dr in result)

                    dtClone.ImportRow(dr);





                for (int ii = 0; ii < dtClone.Rows.Count; ii++)
                {

                    if (dtClone.Rows.Count > 0)
                    {
                        lblQuestionText.Text = dtClone.Rows[itenIndex]["Description"].ToString();

                        if (dtClone.Rows[itenIndex]["Token"].ToString() == "1")
                            lblQuestionText.Text = lblQuestionText.Text.Replace("[TARGETNAME]", hdnFirstName.Value);
                        else if (dtClone.Rows[itenIndex]["Token"].ToString() == "2")
                            lblQuestionText.Text = lblQuestionText.Text.Replace("[TARGETNAME]", hdnLastName.Value);
                        else
                            lblQuestionText.Text = lblQuestionText.Text.Replace("[TARGETNAME]", lblParticipantName.Text);

                        //Replace Client Name keyword with actual client name
                        lblQuestionText.Text = lblQuestionText.Text.Replace("[CLIENTNAME]", hdnClientName.Value);

                    }

                    //TextBox txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                    CKEditor.NET.CKEditorControl txtAnswers = (CKEditor.NET.CKEditorControl)rpItem.FindControl("txtAnswers");
                    if (Convert.ToInt16(lblQType.Text) == 1)
                    {

                        RadioButtonList rblAnswer = (RadioButtonList)rpItem.FindControl("rblAnswer");
                        rblAnswer.Visible = false;

                        //txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                        //txtAnswer.Attributes.Add("onkeypress", "javascript:TextAreaMaxLengthCheck(this.id," + dtClone.Rows[itenIndex]["LengthMAX"].ToString() + ");");

                        //if (Convert.ToBoolean(dtClone.Rows[itenIndex]["Multiline"].ToString()) == true)
                        //{
                        //    txtAnswer.TextMode = TextBoxMode.MultiLine;
                        //    txtAnswer.Rows = 3;
                        //}
                        //else
                        //{
                        //    txtAnswer.TextMode = TextBoxMode.SingleLine;
                        //    //    txtAnswer.Attributes.Add("onkeypress", "javascript:TextAreaMaxLengthCheck(this.id," + dtClone.Rows[ii]["LengthMAX"].ToString() + ");");
                        //}

                        //txtAnswer.Visible = true;
                        if (txtAnswers != null)
                        {
                            txtAnswers.Visible = true;
                            txtAnswers.config.toolbar = new object[] { };
                            txtAnswers.config.keystrokes = new object[] { };
                            txtAnswers.CssClass = "";
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = true;
                            txtAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                            txtAnswers.AutoCompleteType = AutoCompleteType.None;
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = false;
                            txtAnswers.IgnoreEmptyParagraph = true;
                            txtAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            txtAnswers.EnableTabKeyTools = false;
                            txtAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            txtAnswers.Entities = false;
                            txtAnswers.PasteFromWordNumberedHeadingToList = false;
                            txtAnswers.PasteFromWordRemoveStyles = true;
                            
                            txtAnswers.Attributes.Add("onkeypress", "javascript:TextAreaMaxLengthCheck(this.id," + dtClone.Rows[itenIndex]["LengthMAX"].ToString() + ");");
                        }
                    }
                    else
                    {
                        dtRange = questionnaire_BAO.GetRangeDetails(dtClone.Rows[itenIndex]["Range_Name"].ToString());
                        RadioButtonList rblAnswer = (RadioButtonList)rpItem.FindControl("rblAnswer");
                        rblAnswer.Visible = true;

                        rblAnswer.CellPadding = 5;
                        rblAnswer.CellSpacing = 5;

                        rblAnswer.RepeatDirection.Equals("Horizontal");
                        DataTable dtValues = new DataTable();
                        dtValues.Columns.Add("Id");
                        dtValues.Columns.Add("Value");
                        dtValues.Columns.Add("Text");
                        int RangeUpto = 0;
                        if (dtRange.Rows.Count > 0)
                            RangeUpto = Convert.ToInt32(dtRange.Rows[0]["Range_upto"]);
                        int cntIndex = 1;
                        for (int xx = 0; xx < RangeUpto; xx++)
                        {
                            dtValues.Rows.Add(cntIndex, cntIndex + "<BR>" + dtRange.Rows[xx]["Rating_Text"].ToString());
                            cntIndex++;
                        }

                        //txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                        //txtAnswer.Visible = false;
                        txtAnswers = (CKEditor.NET.CKEditorControl)rpItem.FindControl("txtAnswers");
                        if (txtAnswers != null)
                        {
                            txtAnswers.config.toolbar = new object[] { };

                            txtAnswers.config.keystrokes = new object[] { };
                            txtAnswers.CssClass = "";
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = true;
                            txtAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                            txtAnswers.AutoCompleteType = AutoCompleteType.None;
                            txtAnswers.AutoParagraph = false;
                            txtAnswers.ScaytAutoStartup = true;
                            txtAnswers.BrowserContextMenuOnCtrl = false;
                            txtAnswers.ForcePasteAsPlainText = false;
                            txtAnswers.IgnoreEmptyParagraph = true;
                            txtAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            txtAnswers.EnableTabKeyTools = false;
                            txtAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            txtAnswers.Entities = false;
                            txtAnswers.PasteFromWordNumberedHeadingToList = false;
                            txtAnswers.PasteFromWordRemoveStyles = true;

                            txtAnswers.Visible = false;
                        }

                        if (Convert.ToInt32(dtValues.Rows.Count) > 0)
                        {
                            rblAnswer.DataSource = dtValues;
                            rblAnswer.DataValueField = "Id";
                            rblAnswer.DataTextField = "Value";

                            rblAnswer.DataBind();
                            rblAnswer.Visible = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected void rptrQuestionListMain_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Questions"];

            DataTable dtClone = dt.Clone();

            Repeater rptrQuestionList = (Repeater)e.Item.FindControl("rptrQuestionList");

            Label lblCategoryTitle = (Label)e.Item.FindControl("lblCategoryTitle");
            Label lblCategoryID = (Label)e.Item.FindControl("lblCategoryID");
            if (lblCategoryID != null)
                Session["CategoryId"] = lblCategoryID.Text;
            if (lblCategoryID != null)
            {
                DataRow[] result = dt.Select("CategoryID=" + lblCategoryID.Text, "Sequence");

                foreach (DataRow dr in result)
                    dtClone.ImportRow(dr);

                if (dtClone.Rows.Count > 0)
                {
                    rptrQuestionList.DataSource = dtClone;
                    rptrQuestionList.DataBind();

                    lblCategoryTitle.Text = dtClone.Rows[0]["CategoryTitle"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    protected string btnExport(string dirName, int accountId, int projectId, int programmeId)
    {
        try
        {
            ReportViewer rview = new ReportViewer();
            rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
            string[] streamids;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string root = string.Empty;
            root = Server.MapPath("~") + "\\ReportGenerate\\";

            /* Function : For Filling Paramters From Controls */
            ControlToParameter(Convert.ToString(projectId));

            //If strReportType = 1 Then FeedbackReport will Call
            //If strReportType = 2 Then FeedbackReportClient1 will Call (In this Report We are Showing only Range & Text Type Question).
            if (strReportType == "1")
            {
                DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(projectId);
                if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                {
                    /*
                     * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                     * & Making Entry in Table with Radarchatname
                     * & Calling in RDL (RadarImage)
                     */

                }

                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReport";
                rview.ServerReport.ReportPath = "/Feedback360/FeedbackReport";
                //rview.ServerReport.ReportPath = "/Feedback360/FeedbackReport";

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "2")
            {
                // rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient1";
                rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient1";
                //rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient1";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", "1"));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", "1"));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", "1"));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "3")
            {
                rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient2";
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient2";
                //rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient2";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "4") // Old Mutual Report
            {
                // rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
                rview.ServerReport.ReportPath = "/Feedback360/CurFeedbackReport";
                //rview.ServerReport.ReportPath = "/Feedback360/CurFeedbackReport";

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", strDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", strProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", strStaticBarLabelVisibility));
                rview.ServerReport.SetParameters(paramList);
            }
            else if (strReportType == "5") // Old Mutual Report
            {

                rview.ServerReport.ReportPath = "/Survey_Prod/Srvey_FinalReport";

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", strFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", strReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", strConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", strCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", strCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PageHeadingColor", strPageHeadingColor));


                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionHeading", strConclusionHeading));


                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("accountid", Convert.ToString(accountId)));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("projectid", Convert.ToString(projectId)));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("programmeid", Convert.ToString(programmeId)));



                rview.ServerReport.SetParameters(paramList);
            }

            rview.Visible = false;

            byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            string funiqueId = Convert.ToString(Guid.NewGuid());
            string PDF_path = root + "Survey_" + accountId + projectId + programmeId + "-" + funiqueId + ".pdf";
            FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create, FileAccess.ReadWrite);
            objFs.Write(bytes, 0, bytes.Length);
            objFs.Close();
            objFs.Dispose();



            bytes = null;
            System.GC.Collect();
            rview.Dispose();
            return "Survey_" + accountId + projectId + programmeId + "-" + funiqueId + ".pdf";
        }
        catch (Exception ex)
        {
            // HandleException(ex);
            return "";
        }
    }

    protected void ControlToParameter(string projectid)
    {
        if (projectid != null)
        {
            DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));
            if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
            {
                // This parameter will Decide: which type of Report will Call  

                if (dtreportsetting.Rows[0]["PageHeadingColor"].ToString() != string.Empty)
                    strPageHeadingColor = dtreportsetting.Rows[0]["PageHeadingColor"].ToString();

                if (dtreportsetting.Rows[0]["ReportType"].ToString() != string.Empty)
                    strReportType = dtreportsetting.Rows[0]["ReportType"].ToString();

                if (dtreportsetting.Rows[0]["CoverPage"].ToString() != string.Empty)
                    strFrontPage = dtreportsetting.Rows[0]["CoverPage"].ToString();

                if (dtreportsetting.Rows[0]["ReportIntroduction"].ToString() != string.Empty)
                    strReportIntroduction = dtreportsetting.Rows[0]["ReportIntroduction"].ToString();

                if (dtreportsetting.Rows[0]["Conclusionpage"].ToString() != string.Empty)
                    strConclusionPage = dtreportsetting.Rows[0]["Conclusionpage"].ToString();




                if (dtreportsetting.Rows[0]["CatQstList"].ToString() != string.Empty)
                    strCategoryQstlist = dtreportsetting.Rows[0]["CatQstList"].ToString();

                if (dtreportsetting.Rows[0]["CatDataChart"].ToString() != string.Empty)
                    strCategoryBarChart = dtreportsetting.Rows[0]["CatDataChart"].ToString();




            }
        }
    }

    protected string GenerateReport(string strAccountID, string strProjectID, string strProgrammeID)
    {
        string reportfilename = string.Empty;
        string root = Server.MapPath("~") + "\\ReportGenerate\\";
        string rootTemp = Server.MapPath("~") + "\\ReportGenerate\\" + Guid.NewGuid() + "\\";
        Directory.CreateDirectory(rootTemp);



        if (strAccountID != null && strProjectID != null && strProgrammeID != null)
            reportfilename = btnExport("", Convert.ToInt32(strAccountID), Convert.ToInt32(strProjectID), Convert.ToInt32(strProgrammeID));

        string fName = assignQstnParticipant_BAO.GetReportFileName(Convert.ToInt32(strAccountID), Convert.ToInt32(strProjectID), Convert.ToInt32(strProgrammeID));
        fName = reportfilename;
        try
        {
            File.Copy(root + reportfilename, rootTemp + reportfilename);
            if (!string.IsNullOrEmpty(reportfilename) && !string.IsNullOrEmpty(fName))
                fName = ProcessPdfFile(reportfilename, rootTemp, fName, Convert.ToInt32(strAccountID), Convert.ToInt32(strProjectID), Convert.ToInt32(strProgrammeID));
            //throw new Exception();

           
            if (File.Exists(rootTemp + fName))
            {
                //write page number on footer
                byte[] b2 = WritePageNumber(new FileInfo(rootTemp + fName));
                if (File.Exists(root + fName))
                    File.Delete(root + fName);
                File.WriteAllBytes(root + fName, b2);

                Directory.Delete(rootTemp, true);
            }
            else
                fName = reportfilename;
        }
        catch (Exception ex)
        {
            fName = reportfilename;

            //File.Move(root + reportfilename, root + fName);
            //HandleException(ex);
        }
        return root + fName;
    }
    
    public string processIntroductionAndConclusion(string fName, string rootTemp, 
        string strProjectID, string strProgrammeID, string strAccountID)
    {
        DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(strProjectID));

        if (dtreportsetting.Rows.Count > 0)
        {
            string PageHeadingIntro = String.Empty;
            string PageHeadingConclusionText = String.Empty;
            string ConclusionHeading = String.Empty;
            try
            {
                PageHeadingIntro = Convert.ToString(dtreportsetting.Rows[0]["PageHeadingIntro"]);
                PageHeadingConclusionText = Convert.ToString(dtreportsetting.Rows[0]["PageHeadingConclusion"]);
                ConclusionHeading = Convert.ToString(dtreportsetting.Rows[0]["ConclusionHeading"]);
            }
            catch { }
            string Conclusionpage = "1";
            string ReportIntroduction = "1";// Convert.ToBoolean(dtreportsetting.Rows[0]["ReportIntroduction"]);
            try { Conclusionpage = Convert.ToString(dtreportsetting.Rows[0]["Conclusionpage"]); }
            catch { }
            try { ReportIntroduction = Convert.ToString(dtreportsetting.Rows[0]["ReportIntroduction"]); }
            catch { }


            string programmeTitle = string.Empty;
            string projectTitle = string.Empty;
            string companyTitle = string.Empty;
            try
            {
                DataTable dtProgramme = programme_BAO.GetProgrammeByID(Convert.ToInt32(strProgrammeID));
                List<Survey_Project_BE> survey_Project_BE = project_BAO.GetProjectByID(Convert.ToInt32(strAccountID), Convert.ToInt32(strProjectID));

                int intCompanyID = Convert.ToInt32(dtProgramme.Rows[0]["CompanyID"]);
                Survey_Company_BAO company_BAO = new Survey_Company_BAO();
                List<Survey_Company_BE> Survey_Company_BE = company_BAO.GetCompanyByID(intCompanyID);

                if (dtProgramme != null && dtProgramme.Rows.Count > 0)
                {
                    programmeTitle = Convert.ToString(dtProgramme.Rows[0]["ProgrammeName"]);
                }

                if (survey_Project_BE != null && survey_Project_BE.Count > 0)
                {
                    projectTitle = survey_Project_BE.First().Title;
                }
                if (Survey_Company_BE != null && Survey_Company_BE.Count > 0)
                {
                    companyTitle = Survey_Company_BE.First().Title;
                }
            }
            catch { }
            if (ReportIntroduction == "1")
            {
                StringBuilder Introduction = new StringBuilder("<div style=\"font-size:18px;font-weight:bold;font-family:arial\">" + programmeTitle + "</div>");
                Introduction.AppendLine("<div style=\"padding-top:10px\"></div>");
                Introduction.AppendLine("<div style=\"border-top:1px solid #000\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:15px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;text-align:center\">Contents</div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;float:left\">Project Name</div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:normal;font-family:arial;color:#000;float:right;width:500px\"><div style=\"text-alighn:left\">" + projectTitle + "</div></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;float:left\">Company Name</div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:normal;font-family:arial;color:#000;float:right;width:500px\"><div style=\"text-alighn:left\">" + companyTitle + "</div></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;float:left\">Programme Name</div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:normal;font-family:arial;color:#000;float:right;width:500px\"><div style=\"text-alighn:left\">" + programmeTitle + "</div></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;\">Introduction</div>");
                Introduction.AppendLine("<div style=\"padding-top:25px\"></div><div style=\"font-family:arial;\">");
                Introduction.AppendLine(PageHeadingIntro);
                Introduction.AppendLine("</div>");

                string introFilePath = CreateReportImage(Introduction.ToString());

                Guid guidIntro = Guid.NewGuid();
                IncludePage(fName, rootTemp, introFilePath, guidIntro + ".pdf", 2, "R");
                fName = guidIntro + ".pdf";
            }
            if (Conclusionpage == "1")
            {

                StringBuilder Conclusion = new StringBuilder("<div style=\"font-size:19px;font-weight:bold;font-family:arial\">" + programmeTitle + "</div>");
                Conclusion.AppendLine("<div style=\"padding-top:10px\"></div>");
                Conclusion.AppendLine("<div style=\"border-top:1px solid #000\"></div>");
                Conclusion.AppendLine("<div style=\"padding-top:15px\"></div>");
                Conclusion.AppendLine("<div style=\"font-size:19px;font-weight:bold;font-family:arial;color:DarkBlue;text-align:Left\">" + ConclusionHeading + "</div>");
                Conclusion.AppendLine("<div style=\"padding-top:20px\"></div>");

                Conclusion.AppendLine(PageHeadingConclusionText);
                Conclusion.AppendLine("</div>");

                string conclusionFilePath = CreateReportImage(Conclusion.ToString());
                Guid guidConlusion = Guid.NewGuid();
                iTextSharp.text.pdf.PdfReader readerMain = new iTextSharp.text.pdf.PdfReader(rootTemp + "\\" + fName);
                // we retrieve the total number of pages
                int nMain = readerMain.NumberOfPages;
                readerMain.Close();
                readerMain = null;
                IncludePage(fName, rootTemp, conclusionFilePath, guidConlusion + ".pdf", nMain, "R");
                //rootPath + sourceFile
                fName = guidConlusion + ".pdf";
            }

        }
        return fName;
    }
   
    public string CreateReportImage(String HTML)
    {
        string ReportHtmlPath = Server.MapPath("~") + "\\ReportGenerate";

        string HtmlToPdfPathExe = ConfigurationSettings.AppSettings["HtmlToPdfPathExe"];

        Guid TempFolderID = Guid.NewGuid();
        Guid FileName = Guid.NewGuid();
        string tempFolder = ReportHtmlPath + "\\" + TempFolderID;
        if (!Directory.Exists(ReportHtmlPath + "\\" + TempFolderID))
            Directory.CreateDirectory(ReportHtmlPath + "\\" + TempFolderID);
        string FilePath = ReportHtmlPath + "\\" + TempFolderID + "\\" + FileName;


        System.IO.File.WriteAllText(FilePath + ".html", HTML);
        string str_Command = string.Empty;

        string ImageFileName = string.Format(@"{0}.pdf", FileName);
        string Image_File_Path = tempFolder + "\\" + ImageFileName;

        str_Command = "wkhtmltopdf\"    --disable-smart-shrinking  \"" + FilePath + ".html" + "\" \"" + Image_File_Path + "\"";

        ProcessStartInfo procStartInfo = new ProcessStartInfo("\"" + HtmlToPdfPathExe + "\\" + str_Command);
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        proc.WaitForExit();

        return FilePath + ".pdf";
    }

    private string ProcessPdfFile(string fileName, string root, string finalFileName,
        int accountId, int projectId, int programmeId)
    {
        try
        {
            string fnameTemp = finalFileName;

            string uploadedFilePath = Server.MapPath("~") + "\\UploadDocs\\";
            string frontPageFilePath = string.Empty;//path of front page pdf which have to be inserted in Main report
            string fileNameWithFront = "F-" + finalFileName;//use to save file name which have front page inserted
            string PageHeading1 = "", PageHeading2 = "", PageHeading3 = "", PageHeadingColor = "", path = "";

            DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(projectId);

            if (dtreportsetting.Rows.Count > 0)
            {
                frontPageFilePath = Convert.ToString(dtreportsetting.Rows[0]["FrontPdfFileName"]);
                PageHeading1 = Convert.ToString(dtreportsetting.Rows[0]["PageHeading1"]);
                PageHeading2 = Convert.ToString(dtreportsetting.Rows[0]["PageHeading2"]);
                PageHeading3 = Convert.ToString(dtreportsetting.Rows[0]["PageHeading3"]);
                PageHeadingColor = Convert.ToString(dtreportsetting.Rows[0]["PageHeadingColor"]);
            }

             DataTable dtExLink = (DataTable)Session["DTEXLINK"];
             if (dtExLink != null && dtExLink.Rows.Count > 0)
             {
                 string strAccountID = Convert.ToString(dtExLink.Rows[0]["AccountId"]);
                 string strProjectID = Convert.ToString(dtExLink.Rows[0]["ProjectId"]);
                 string strProgrammeID = Convert.ToString(dtExLink.Rows[0]["ProgrammeId"]);
                 processIntroductionAndConclusion(fileName, root, strProjectID, strProgrammeID, strAccountID);
             }

            //Insert Front Page
            if (!string.IsNullOrEmpty(frontPageFilePath))
            {
                WriteContentToPdf(new FileInfo(uploadedFilePath + frontPageFilePath), PageHeading1, PageHeading2, PageHeading3, PageHeadingColor, 450f, out path);
                IncludePage(fileName, root, path, fileNameWithFront, 1, "R");
            }

            Survey_Category_BAO objSurvey_Category_BAO = new Survey_Category_BAO();
            DataTable dtCategory = objSurvey_Category_BAO.GetdtnewCategoryList(Convert.ToString(accountId) + " and [Survey_Project].ProjectID =" + Convert.ToString(projectId));
            if (!string.IsNullOrEmpty(frontPageFilePath))
                finalFileName = !string.IsNullOrEmpty(fileNameWithFront) ? fileNameWithFront : fileName;
            foreach (DataRow item in dtCategory.Rows)
            {
                string categoryPageFileName = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(item["CategoryName"])))
                    categoryPageFileName = Convert.ToString(item["CategoryName"]);

                string catPageFilePath = uploadedFilePath + Convert.ToString(item["IntroPdfFileName"]);//category pdf to be insert
                //string pdf_donotdelete = root + "..\\pdf-donotdelete.pdf";
                if (!string.IsNullOrEmpty(Convert.ToString(item["IntroPdfFileName"])))
                {
                    if (File.Exists(catPageFilePath))
                    {
                        List<int> lstPageNo = ReadPdfFile(root + finalFileName, "@@" + categoryPageFileName + "@@");

                        if (lstPageNo.Any())
                        {
                            IncludePage(finalFileName, root, catPageFilePath, categoryPageFileName + ".pdf", lstPageNo.FirstOrDefault() + 1, "I");
                            finalFileName = categoryPageFileName + ".pdf";

                            if (File.Exists(root + fnameTemp))
                                File.Delete(root + fnameTemp);
                            File.Move(root + finalFileName, root + fnameTemp);
                            finalFileName = fnameTemp;
                        }
                    }

                }

            }

            return finalFileName;
        }
        catch (Exception ex)
        {

            return finalFileName;
        }


        //List<int> x = ReadPdfFile(rootPath + "\\" + "PeterHart_106222.pdf", "The pay and benefits I receive fairly reflect the work I do");
    }

    /// <summary>
    /// Use to find page number based on search text in pdf
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="searthText"></param>
    /// <returns></returns>
    public List<int> ReadPdfFile(string fileName, String searthText)
    {
        List<int> pages = new List<int>();
        if (File.Exists(fileName))
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName);
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

                string currentPageText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                if (currentPageText.Contains(searthText))
                {
                    pages.Add(page);
                }
            }
            pdfReader.Close();
        }
        return pages;
    }

    protected string IncludePage(string sourceFile, string rootPath, string insertPageFilePath,
        string OutputFileName, int pageNumber, string flag)
    {
        try
        {


            //String ReportHtml = ConfigurationManager.AppSettings["ReportHtml"].ToString();
            //String ReportName = Request.QueryString["ReportName"].ToString();
            if (flag != "D")
            {
                iTextSharp.text.Document document = null;
                iTextSharp.text.pdf.PdfCopy writer = null;

                // we create a reader for a certain document
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(insertPageFilePath);
                // we retrieve the total number of pages
                int n = reader.NumberOfPages;
                // step 1: creation of a document-object
                document = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
                // step 2: we create a writer that listens to the document

                FileStream fs = new FileStream(rootPath + Guid.NewGuid() + ".pdf", FileMode.CreateNew, FileAccess.Write);
                System.IO.Stream strm = fs;
                writer = new iTextSharp.text.pdf.PdfCopy(document, strm);


                // step 3: we open the document
                document.Open();
                Dictionary<int, iTextSharp.text.pdf.PdfImportedPage> pagesToInsert = new Dictionary<int, iTextSharp.text.pdf.PdfImportedPage>();
                for (int i = 1; i <= n; i++)
                {
                    iTextSharp.text.pdf.PdfImportedPage page;
                    page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);

                    pagesToInsert.Add(i, page);
                }
                bool status = InsertorReplacePages(rootPath + sourceFile, pagesToInsert, rootPath + OutputFileName, pageNumber, flag);

                document.Close();
                writer.Close();
                reader.Close();

            }
            else
            {
                Dictionary<int, iTextSharp.text.pdf.PdfImportedPage> pagesToInsert = new Dictionary<int, iTextSharp.text.pdf.PdfImportedPage>();
                bool status = InsertorReplacePages(rootPath + sourceFile, pagesToInsert, rootPath + OutputFileName, pageNumber, flag);
            }
            return "";
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {

        }
    }

    protected static byte[] WritePageNumber(FileInfo sourceFile)
    {
        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(sourceFile.FullName);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            iTextSharp.text.pdf.PdfStamper pdfStamper = new iTextSharp.text.pdf.PdfStamper(reader, memoryStream);
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                iTextSharp.text.pdf.PdfContentByte pdfPageContents = pdfStamper.GetUnderContent(i);
                pdfPageContents.BeginText();
                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, System.Text.Encoding.ASCII.EncodingName, false);
                pdfPageContents.SetFontAndSize(baseFont, 9);

                pdfPageContents.SetRGBColorFill(0, 0, 0);

                int pageNumber = i;

                pdfPageContents.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT, "Page " + pageNumber.ToString() + " of " + reader.NumberOfPages, pageSize.Width - 50, 20,


                                                  0);

                pdfPageContents.EndText(); // Done working with text
            }

            pdfStamper.FormFlattening = true; // enable this if you want the PDF flattened. 
            pdfStamper.Close(); // Always close the stamper or you'll have a 0 byte stream. 
            return memoryStream.ToArray();
        }
    }
    
    /// <summary>
    /// Insert new pages to an existing pdf file
    /// </summary>
    /// <param name="sourcePdf">The full path to the source pdf</param>
    /// <param name="pagesToInsert">The dictionary contains the pages to be inserted in the source pdf. The key is the page number to be inserted. The value is the PdfImportedPage to insert</param>
    /// <param name="outPdf">The full path of the resulting output pdf file</param>
    /// <returns>True if the operation succeeded. False otherwise.</returns>
    /// <remarks>To create the pagesToInsert dictionary, you can use the iTextSharp.text.pdf.PdfCopy class to open
    /// an existing pdf file and call the GetImportedPage method</remarks>
    public static bool InsertorReplacePages(string sourcePdf, Dictionary<int, iTextSharp.text.pdf.PdfImportedPage> pagesToInsert,
        string outPdf, int PageNUmber, string flag)
    {


        bool result = false;
        iTextSharp.text.pdf.PdfReader reader = null;
        iTextSharp.text.Document doc = null;
        iTextSharp.text.pdf.PdfCopy copier = null;
        try
        {
            //int j = PageNUmber;
            reader = new iTextSharp.text.pdf.PdfReader(sourcePdf);
            doc = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
            copier = new iTextSharp.text.pdf.PdfCopy(doc, new System.IO.FileStream(outPdf, System.IO.FileMode.Create));
            doc.Open();
            int i = 1;
            for (; i <= reader.NumberOfPages; )
            {
                if (i == PageNUmber)
                {
                    if (flag != "D")
                    {
                        for (int j = 1; j <= pagesToInsert.Count; j++)
                        {
                            copier.AddPage(pagesToInsert[j]);
                        }

                        PageNUmber--;
                        if (flag == "R")
                            i++;
                    }
                }
                else
                {
                    copier.AddPage(copier.GetImportedPage(reader, i));
                    i++;
                }
            }

            if (i == PageNUmber)
            {
                for (int j = 1; j <= pagesToInsert.Count; j++)
                {
                    copier.AddPage(pagesToInsert[j]);
                }
                PageNUmber--;
            }

            doc.Close();
            reader.Close();
            result = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    protected static void WriteContentToPdf(FileInfo sourceFile, string heading1,
        string heading2, string heading3, string htmlcolor, float width, out string outputFile)
    {


        DirectoryInfo di = sourceFile.Directory;
        string watermarkedFile = di.FullName + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf";
        //File.Copy(sourceFile.FullName, di.FullName + "\\" + watermarkedFile);

        PdfReader reader1 = new PdfReader(sourceFile.FullName);
        using (FileStream fs = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (PdfStamper stamper = new PdfStamper(reader1, fs))
            {
                PdfLayer layer = new PdfLayer("WatermarkLayer", stamper.Writer);

                //rectangle(stamper, htmlcolor);
                iTextSharp.text.Rectangle rect = reader1.GetPageSize(1);
                iTextSharp.text.Rectangle pageRectangle = reader1.GetPageSizeWithRotation(1);

                watermark(stamper, layer, pageRectangle, heading1, 250, 18, 310, 715);
                watermark(stamper, layer, pageRectangle, heading2, 270, 16, 310, 685);
                watermark(stamper, layer, pageRectangle, heading3, 290, 14, 310, 658);

            }
        }

        outputFile = watermarkedFile;
        //string originalFileName = sourceFile.FullName;
        // if (File.Exists(originalFileName))
        // {
        //      File.Delete(originalFileName);

        //  }
        //  File.Move(watermarkedFile, originalFileName);

    }

    private static void rectangle(PdfStamper stamper, string color)
    {
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        System.Drawing.Color bckgrndcol = System.Drawing.ColorTranslator.FromHtml(color);
        BaseColor bckgrndco = new BaseColor(bckgrndcol);

        PdfContentByte cb = stamper.GetOverContent(1);
        PdfGState gState = new PdfGState();
        gState.FillOpacity = 0.85f;
        cb.SetGState(gState);

        iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(83f, 750f, 555f, 650f);
        rectangle.BorderWidth = 1f;
        rectangle.Border = 15;
        rectangle.BorderColor = BaseColor.BLACK;
        rectangle.BackgroundColor = bckgrndco;

        cb.Rectangle(rectangle);

    }

    private static void watermark(PdfStamper stamper, PdfLayer layer, iTextSharp.text.Rectangle rect,
        string text, int location, int fontsize, float xAxis, float yAxis)
    {
        PdfContentByte cb = stamper.GetOverContent(1);

        // Tell the cb that the next commands should be "bound" to this new layer
        cb.BeginLayer(layer);
        cb.SetFontAndSize(BaseFont.CreateFont(
          BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), fontsize);

        PdfGState gState = new PdfGState();
        //gState.FillOpacity = 0.25f;
        cb.SetGState(gState);

        cb.SetColorFill(BaseColor.BLACK);
        cb.BeginText();

        var ps = rect; /*dc.PdfDocument.PageSize is not always correct*/
        var x = (ps.Right + ps.Left) / 2;
        var y = (ps.Bottom + ps.Top) / 2;

        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, x, yAxis, 0f);

        cb.EndText();

        cb.EndLayer();
    }
}
