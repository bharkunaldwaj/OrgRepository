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
using System.Globalization;

public partial class Module_Feedback_Feedback : System.Web.UI.Page
{
    Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
    DataTable dtQuestion = new DataTable();
    DataTable dtCategory = new DataTable();
    Int32 questionCount = 0;
    Int32 currentCount = 0;
    Int32 categoryCount = 0;
    CodeBehindBase cBase = new CodeBehindBase();
    string candidateID;
    string questionnaireID;
    string Template;

    #region Protected Region
    protected void Page_Load(object sender, EventArgs e)
    {

        // Label ll = (Label)this.Master.FindControl("Current_location");
        //ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        ////Get the Candidate Information 
        //string qID = PasswordGenerator.EnryptString("140");
        //string cID = PasswordGenerator.EnryptString("3818");
        string candidateID = PasswordGenerator.DecryptString("MTQ4");

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

                    AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
                    dtResult = assignQuestionnaire_BAO.GetAllAssignmentInfo(Convert.ToInt32(candidateID));

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
                        hdnRelationship.Value = dtResult.Rows[0]["RelationShip"].ToString();

                        //Set Client Name
                        DataTable dtProgramme = new DataTable();
                        Programme_BAO programme_BAO = new Programme_BAO();
                        dtProgramme = programme_BAO.GetProgrammeByID(Convert.ToInt32(dtResult.Rows[0]["ProgrammeID"]));

                        hdnClientName.Value = dtProgramme.Rows[0]["ClientName"].ToString();

                        //Set Increment Value
                        hdnIncrementValue.Value = dtProjectInfo.Rows[0]["DisplayCategory"].ToString();

                        //Set Login User Name
                        lblUserName.Text = dtResult.Rows[0]["CandidateName"].ToString();

                        //Set Header Background Color
                        tdHeader.Attributes.Add("style", "background:" + dtResult.Rows[0]["HeaderBGColor"].ToString() + ";");

                        //Set Header Image
                        imgHeader.ImageUrl = "~/UploadDocs/" + dtResult.Rows[0]["CompanyLogo"].ToString();

                        //Set Footer Copyright Line
                        lblFooter.Text = dtResult.Rows[0]["CopyRightLine"].ToString();

                        ////Set Project Logo
                        //if (dtResult.Rows[0]["Logo"].ToString() != "")
                        //{
                        //    imgProjectLogo.Visible = true;
                        //    imgProjectLogo.ImageUrl = "~/UploadDocs/" + dtResult.Rows[0]["Logo"].ToString();
                        //}
                        //else
                        //{
                        //    imgProjectLogo.Visible = false;
                        //}

                        //Set Programme Logo
                        if (dtProgramme.Rows[0]["Logo"].ToString() != "")
                        {
                            imgProjectLogo.Visible = true;
                            imgProjectLogo.ImageUrl = "~/UploadDocs/" + dtProgramme.Rows[0]["Logo"].ToString();
                        }
                        else
                        {
                            imgProjectLogo.Visible = false;
                        }

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

    protected void SaveQuestionAnswer()
    {
        try
        {

            QuestionAnswer_BAO questionAnswer_BAO = new QuestionAnswer_BAO();

            string answer = "";
            int questionID = 0;

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");
                foreach (RepeaterItem item in rptrQuestionList.Items)
                {
                    QuestionAnswer_BE questionAnswer_BE = new QuestionAnswer_BE();

                    Label lblQType = (Label)item.FindControl("lblQType");
                    Label lblQId = (Label)item.FindControl("lblQId");
                    questionID = Convert.ToInt32(lblQId.Text);

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
                        //    answer = txtAnswer.Text;
                    }
                    else
                    {
                        RadioButtonList rblAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        if (rblAnswer != null)
                        {
                            if (rblAnswer.SelectedItem != null)
                            {
                                //   answer = rblAnswer.SelectedItem.Text;
                                answer = rblAnswer.SelectedItem.Value;
                            }
                            else
                                answer = "";
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
        int result = 1;
        try
        {
            QuestionAnswer_BAO questionAnswer_BAO = new QuestionAnswer_BAO();
            string answer = "";

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");
                foreach (RepeaterItem item in rptrQuestionList.Items)
                {
                    Label lblValidation = (Label)item.FindControl("lblValidation");
                    Label lblQType = (Label)item.FindControl("lblQType");
                    Label lblQId = (Label)item.FindControl("lblQId");

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

                            answer = Convert.ToString(txtAnswers.Text);
                        }

                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //    answer = Convert.ToString(txtAnswer.Text);
                    }
                    else
                    {
                        RadioButtonList rblAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        if (rblAnswer != null)
                        {
                            if (rblAnswer.SelectedItem != null)
                            {
                                //answer = rblAnswer.SelectedItem.Text;
                                answer = rblAnswer.SelectedItem.Value;
                            }
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

    protected void imbPrevious_Click(object sender, EventArgs e)
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
                    //divCategory.Visible = false;

                    imbStart.Visible = true;
                    imbFinish.Visible = false;
                    imbNext.Visible = false;
                    imbPrevious.Visible = false;
                }
                else if (Convert.ToInt32(Session["Count"]) == 0)
                {
                    divText.Visible = false;
                    rptrQuestionListMain.Visible = true;
                    //divCategory.Visible = true;

                    imbStart.Visible = false;
                    imbFinish.Visible = false;
                    imbNext.Visible = true;
                    imbPrevious.Visible = true;

                    DataTable dtCat = new DataTable();
                    dtCat = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);

                    BindQuestions(currentCount);
                    SetQuestionAnswer();

                }
                else
                {
                    divText.Visible = false;
                    rptrQuestionListMain.Visible = true;
                    //divCategory.Visible = true;

                    imbStart.Visible = false;
                    imbFinish.Visible = false;
                    imbNext.Visible = true;
                    imbPrevious.Visible = true;

                    DataTable dtCat = new DataTable();
                    dtCat = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);

                    BindQuestions(currentCount);
                    SetQuestionAnswer();

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

    protected void imbNext_Click(object sender, EventArgs e)
    {
        try
        {
            int result = CheckQuestionAnswer();

            if (result <= 1)
            {
                SaveQuestionAnswer();

                Session["Count"] = Convert.ToInt32(Session["Count"]) + 1;

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
                    List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Questionnaire_BE>();
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
                    SetQuestionAnswer();
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

    protected void imbFinish_Click(object sender, EventArgs e)
    {
        //if (cbxNotifyMail.Checked == true)
        //{

        DataTable dtProjectInfo = new DataTable();
        dtProjectInfo = questionnaire_BAO.GetProjectQuestionnaireInfo(Convert.ToInt32(hdnQuestionnaireId.Value), Convert.ToInt32(hdnCandidateId.Value));

        DataTable dtResult = new DataTable();
        AssignQuestionnaire_BAO assignQuestionnaire_BAO = new AssignQuestionnaire_BAO();
        dtResult = assignQuestionnaire_BAO.GetAllAssignmentInfo(Convert.ToInt32(hdnCandidateId.Value));

        //Send mail to candidates
        for (int i = 0; i < dtProjectInfo.Rows.Count; i++)
        {

            Template = System.IO.File.ReadAllText(Server.MapPath("~") + "\\UploadDocs\\FeedbackTemplate.txt");

            string candidatename = "";
            string candidateemail = "";
            string organizationname = "";
            string Projectname = "";
            string participantname = "";
            string participantemail = "";

            candidatename = dtResult.Rows[i]["CandidateName"].ToString();
            candidateemail = dtResult.Rows[i]["CandidateEmail"].ToString();
            organizationname = dtResult.Rows[i]["OrganisationName"].ToString();
            Projectname = dtProjectInfo.Rows[i]["Title"].ToString();
            participantname = dtProjectInfo.Rows[i]["Fullname"].ToString();
            participantemail = dtProjectInfo.Rows[i]["EmailID"].ToString();

            Template = Template.Replace("[TITLE]", Projectname);
            Template = Template.Replace("[EMAILID]", participantemail);
            Template = Template.Replace("[FIRSTNAME]", candidatename);
            Template = Template.Replace("[COMPANY]", organizationname);
            Template = Template.Replace("[PARTICIPANTNAME]", participantname);

            MailAddress maddr = new MailAddress(candidateemail, candidatename);
            //SendEmail.Send("Questionnaire Feedback Submitted", Template, participantemail,maddr,"");

            lblMessage.Text = "Questionnaire has been submitted and email sent successfully.";
        }

        int result = questionnaire_BAO.UpdateSubmitFlag(Convert.ToInt32(hdnCandidateId.Value), 1);
        //}
        //else
        //{
        //    lblMessage.Text = "Questionnaire has been submitted successfully.";
        //}
    }

    protected void imbStart_Click(object sender, EventArgs e)
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

            //divCategory.Visible = true;
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

            if (rpItem.ItemType == ListItemType.Footer)
            {
                Label lblNote2 = (Label)rpItem.FindControl("lblNote2");
                if (lblNote2 != null && hdnAccountId.Value.ToString() == "68")
                    lblNote2.Visible = false;

            }

            Label lblQType = (Label)rpItem.FindControl("lblQType");
            Label lblQstId = (Label)rpItem.FindControl("lblQId");
            Label lblQuestionText = (Label)rpItem.FindControl("lblQuestionText");
            Label lblQuestionHint = (Label)rpItem.FindControl("lblHint");
            HtmlTable tblGraph = (HtmlTable)rpItem.FindControl("tblGraph");

            if (lblQType != null)
            {

                if (lblQType.Text == "2" && hdnAccountId.Value == "34")
                    tblGraph.Visible = true;
                else
                    tblGraph.Visible = false;

                DataTable dt = new DataTable();
                dt = (DataTable)Session["Questions"];

                DataTable dtClone = dt.Clone();
                RadioButton rdbtnNA = (RadioButton)rpItem.FindControl("rbtnNotApplicable");

                DataRow[] result = dt.Select("QuestionID =" + lblQstId.Text);

                foreach (DataRow dr in result)
                    dtClone.ImportRow(dr);

                if (dtClone.Rows.Count > 0)
                {
                    lblQuestionText.Text = dtClone.Rows[0]["Description"].ToString();

                    if (dtClone.Rows[0]["Token"].ToString() == "1")
                        lblQuestionText.Text = lblQuestionText.Text.Replace("[TARGETNAME]", hdnFirstName.Value);
                    else if (dtClone.Rows[0]["Token"].ToString() == "2")
                        lblQuestionText.Text = lblQuestionText.Text.Replace("[TARGETNAME]", hdnLastName.Value);
                    else
                        lblQuestionText.Text = lblQuestionText.Text.Replace("[TARGETNAME]", lblParticipantName.Text);

                    //Replace Client Name keyword with actual client name
                    lblQuestionText.Text = lblQuestionText.Text.Replace("[CLIENTNAME]", hdnClientName.Value);

                }

                if (Convert.ToInt16(lblQType.Text) == 1)
                {

                    RadioButtonList rblAnswer = (RadioButtonList)rpItem.FindControl("rblAnswer");
                    rblAnswer.Visible = false;

                    CKEditor.NET.CKEditorControl txtAnswers = (CKEditor.NET.CKEditorControl)rpItem.FindControl("txtAnswers");
                    //TextBox txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                    //if (Convert.ToBoolean(dtClone.Rows[0]["Multiline"].ToString()) == true)
                    //{
                    //    txtAnswer.TextMode = TextBoxMode.MultiLine;
                    //    txtAnswer.Attributes.Add("onkeypress", "javascript:TextAreaMaxLengthCheck(this.id," + dtClone.Rows[0]["LengthMAX"].ToString() + ");");
                    //    txtAnswer.Rows = 3;
                    //}
                    //else
                    //{
                    //    txtAnswer.TextMode = TextBoxMode.SingleLine;
                    //}

                    //txtAnswer.Visible = true;

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
                        txtAnswers.Attributes.Add("onkeypress", "javascript:TextAreaMaxLengthCheck(this.id," + dtClone.Rows[0]["LengthMAX"].ToString() + ");");

                        txtAnswers.Visible = true;
                    }

                    rdbtnNA.Visible = false;
                }
                else
                {
                    Label lblLowerLabel = (Label)rpItem.FindControl("lblLowerLabel");
                    Label lblUpperLabel = (Label)rpItem.FindControl("lblUpperLabel");
                    Label lblLowerBound = (Label)rpItem.FindControl("lblLowerBound");
                    Label lblUpperBound = (Label)rpItem.FindControl("lblUpperBound");
                    Label lblIncrement = (Label)rpItem.FindControl("lblIncrement");

                    RadioButtonList rblAnswer = (RadioButtonList)rpItem.FindControl("rblAnswer");
                    rblAnswer.Visible = true;
                    rblAnswer.CellPadding = 5;
                    rblAnswer.CellSpacing = 5;

                    //TextBox txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                    //txtAnswer.Visible = false;
                    CKEditor.NET.CKEditorControl txtAnswers = (CKEditor.NET.CKEditorControl)rpItem.FindControl("txtAnswers");
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


                    DataTable dtValues = new DataTable();

                    dtValues.Columns.Add("Id");
                    dtValues.Columns.Add("Value");

                    for (int counter = Convert.ToInt32(lblLowerBound.Text); counter <= Convert.ToInt32(lblUpperBound.Text); counter = counter + Convert.ToInt32(lblIncrement.Text))
                        dtValues.Rows.Add(counter.ToString() + "&nbsp;&nbsp;", counter.ToString() + "&nbsp;&nbsp;");

                    dtValues.Rows.Add("N/A", "N/A");

                    dtValues.Rows[Convert.ToInt32(dtValues.Rows.Count) - 2]["Value"] = lblUpperBound.Text + "&nbsp;&nbsp;&nbsp;<b>" + dtClone.Rows[0]["UpperLabel"].ToString() + "</b>&nbsp;&nbsp;&nbsp;";

                    rblAnswer.DataSource = dtValues;
                    rblAnswer.DataValueField = "Id";
                    rblAnswer.DataTextField = "Value";
                    rblAnswer.DataBind();

                    rdbtnNA.Visible = false;
                    lblUpperLabel.Visible = false;
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
            Label LabelQuestionnaireDescription = (Label)e.Item.FindControl("LabelQuestionnaireDescription");
            Label lblCategoryTitle = (Label)e.Item.FindControl("lblCategoryTitle");
            Label lblCategoryID = (Label)e.Item.FindControl("lblCategoryID");

            if (lblCategoryID != null)
            {
                DataRow[] result = dt.Select("CategoryID=" + lblCategoryID.Text, "Sequence");

                foreach (DataRow dr in result)
                    dtClone.ImportRow(dr);

                if (dtClone.Rows.Count > 0)
                {
                    rptrQuestionList.DataSource = dtClone;
                    rptrQuestionList.DataBind();
                    //lblCategoryName.Text = dtClone.Rows[0]["CategoryName"].ToString();
                    lblCategoryTitle.Text = dtClone.Rows[0]["CategoryTitle"].ToString();
                    LabelQuestionnaireDescription.Text = dtClone.Rows[0].Field<string>("QuestionnaireCategoryDescription");
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }
    #endregion

    #region Private Region
    private void SetGraphData()
    {
        int answeredQuestion = questionnaire_BAO.CalculateGraph(Convert.ToInt32(hdnQuestionnaireId.Value), Convert.ToInt32(hdnCandidateId.Value));

        double percentage = (answeredQuestion * 100) / Convert.ToInt32(hdnQuestionCount.Value);
        percentage = Convert.ToInt32(Math.Abs(percentage));
        tbGraph.Width = percentage.ToString() + "%";

        lblCompletionStatus.Text = percentage + "%";
        //lblCompletionStatus.ForeColor = System.Drawing.Color.Red;

        //Set Prolog of questionnaire
        List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Questionnaire_BE>();
        questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(Convert.ToInt32(hdnQuestionnaireId.Value));
        lblQuestionnaireText.Text = questionnaire_BEList[0].QSTNPrologue.ToString();
    }

    private void BindQuestionInformation()
    {
        try
        {
            //Get questionnaire ID
            string questionnaireID = Convert.ToString(Request.QueryString["QID"]);
            questionnaireID = PasswordGenerator.DecryptString(questionnaireID);

            int AccountID = Convert.ToInt32(hdnAccountId.Value);
            int ProjectId = Convert.ToInt32(hdnProjectId.Value);
            string Relationship = hdnRelationship.Value;
            int qID = Convert.ToInt32(questionnaireID);

            if (hdnRelationship.Value != "Self")
                dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaireByRelationShip(AccountID, ProjectId, qID, Relationship);
            else
                dtQuestion = questionnaire_BAO.GetFeedbackQuestionnaireSelfByRelationShip(AccountID, ProjectId, qID, Relationship);

            Session["Questions"] = dtQuestion;

            dtCategory = questionnaire_BAO.GetQuestionnaireCategoriesByRelationShip(AccountID, ProjectId, qID, Relationship);
            Session["Category"] = dtCategory;

            Session["categoryCount"] = (Math.Abs(dtCategory.Rows.Count / Convert.ToInt32(hdnIncrementValue.Value)));

            if (dtCategory.Rows.Count % Convert.ToInt32(hdnIncrementValue.Value) > 0)
                Session["categoryCount"] = Convert.ToInt32(Session["categoryCount"]) + 1;

            questionCount = dtQuestion.Rows.Count;
            hdnQuestionCount.Value = questionCount.ToString(); ;

            if (questionCount > 0)
            {
                DataTable dtCat = new DataTable();
                dtCat = (DataTable)Session["Category"];
                currentCount = Convert.ToInt32(Session["Count"]);

                BindQuestions(currentCount);
                SetQuestionAnswer();

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
        try
        {
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
            rptrQuestionListMain.DataBind();

            //if (dtClone.Rows.Count > 0)
            //{
            //    foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            //    {
            //        Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");
            //        Label lblCategoryName = (Label)rptrItem.FindControl("lblCategoryName");

            //        rptrQuestionList.DataSource = dtClone;
            //        rptrQuestionList.DataBind();
            //        //lblCategoryName.Text = dtClone.Rows[0]["CategoryName"].ToString();
            //    }
            //}
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
                    Label lblQType = (Label)item.FindControl("lblQType");
                    Label lblLowerLabel = (Label)item.FindControl("lblLowerLabel");
                    Label lblUpperLabel = (Label)item.FindControl("lblUpperLabel");
                    Label lblLowerBound = (Label)item.FindControl("lblLowerBound");
                    Label lblUpperBound = (Label)item.FindControl("lblUpperBound");
                    Label lblIncrement = (Label)item.FindControl("lblIncrement");
                    RadioButton rdbtnNA = (RadioButton)item.FindControl("rbtnNotApplicable");

                    if (lblQId != null)
                        questionID = Convert.ToInt32(lblQId.Text);
                    candidateId = Convert.ToInt32(hdnCandidateId.Value);

                    QuestionAnswer_BAO questionAnswer_BAO = new QuestionAnswer_BAO();
                    string answer = questionAnswer_BAO.GetQuestionAnswer(candidateId, questionID);

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

                            txtAnswers.Text = answer;

                        }
                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //    txtAnswer.Text = answer;
                    }
                    else
                    {
                        RadioButtonList rblAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        rblAnswer.Visible = true;
                        rblAnswer.CellPadding = 5;
                        rblAnswer.CellSpacing = 5;

                        DataTable dtValues = new DataTable();

                        dtValues.Columns.Add("Id");
                        dtValues.Columns.Add("Value");

                        for (int counter = Convert.ToInt32(lblLowerBound.Text); counter <= Convert.ToInt32(lblUpperBound.Text);
                            counter = counter + Convert.ToInt32(lblIncrement.Text))
                            dtValues.Rows.Add(counter.ToString(), counter.ToString());

                        dtValues.Rows.Add("N/A", "N/A");

                        //dtValues.Rows[Convert.ToInt32(dtValues.Rows.Count) - 2]["Value"] = lblUpperBound.Text + "&nbsp;&nbsp;&nbsp;<b>" + lblUpperLabel.Text + "</b>&nbsp;&nbsp;&nbsp;";

                        dtValues.Rows[Convert.ToInt32(dtValues.Rows.Count) - 2]["Value"] = lblUpperBound.Text + "</label></td><td>&nbsp;&nbsp;</td><td ><b>" + lblUpperLabel.Text + "</b></td><td><label>&nbsp;&nbsp;";

                        rblAnswer.DataSource = dtValues;
                        rblAnswer.DataValueField = "Id";
                        rblAnswer.DataTextField = "Value";
                        rblAnswer.DataBind();

                        if (rblAnswer != null && answer != "")
                        {
                            for (int i = 0; i < dtValues.Rows.Count; i++)
                            {


                                if (rblAnswer.Items[i].Text == answer)
                                {
                                    rblAnswer.Items[i].Selected = true;
                                    break;
                                }
                                else
                                {
                                    if (rblAnswer.Items[i].Text.Contains("</label>"))
                                    {
                                        if ((rblAnswer.Items[i].Text.Substring(0, 2)).Trim() == answer)
                                        {
                                            rblAnswer.Items[i].Selected = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            //if (answer == "N/A")
                            //{
                            //    rdbtnNA.Checked = true;
                            //}
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
    #endregion

}
