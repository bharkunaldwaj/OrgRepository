using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Miscellaneous;
using System.Net.Mail;

using Questionnaire_BE;
using Questionnaire_BAO;

public partial class Module_Feedback_Feedback : System.Web.UI.Page
{
    //Global variables
    Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();
    DataTable dataTableQuestion = new DataTable();
    DataTable dataTableCategory = new DataTable();
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

        DataTable dataTableResult = new DataTable();
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
                //If questionnaireId and candidate Id is not null
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

                    AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

                    dataTableResult = assignQuestionnaireBusinessAccessObject.GetAllAssignmentInfo(Convert.ToInt32(candidateID));

                    if (dataTableResult.Rows.Count > 0)
                    {
                        //Setting Questionnaire Project Information
                        DataTable dataTableProjectInformation = new DataTable();

                        dataTableProjectInformation = questionnaireBusinessAccessObject.GetProjectQuestionnaireInfo(Convert.ToInt32(questionnaireID), Convert.ToInt32(candidateID));

                        hdnAccountId.Value = dataTableResult.Rows[0]["AccountID"].ToString();

                        hdnProjectId.Value = dataTableProjectInformation.Rows[0]["ProjectID"].ToString();
                        lblProjectName.Text = dataTableProjectInformation.Rows[0]["Title"].ToString();
                        lblParticipantName.Text = dataTableProjectInformation.Rows[0]["FullName"].ToString();

                        hdnFirstName.Value = dataTableProjectInformation.Rows[0]["FirstName"].ToString();
                        hdnLastName.Value = dataTableProjectInformation.Rows[0]["LastName"].ToString();
                        hdnRelationship.Value = dataTableResult.Rows[0]["RelationShip"].ToString();

                        //Set Client Name
                        DataTable dataTableProgramme = new DataTable();
                        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
                        dataTableProgramme = programmeBusinessAccessObject.GetProgrammeByID(Convert.ToInt32(dataTableResult.Rows[0]["ProgrammeID"]));

                        hdnClientName.Value = dataTableProgramme.Rows[0]["ClientName"].ToString();

                        //Set Increment Value
                        hdnIncrementValue.Value = dataTableProjectInformation.Rows[0]["DisplayCategory"].ToString();

                        //Set Login User Name
                        lblUserName.Text = dataTableResult.Rows[0]["CandidateName"].ToString();

                        //Set Header Background Color
                        tdHeader.Attributes.Add("style", "background:" + dataTableResult.Rows[0]["HeaderBGColor"].ToString() + ";");

                        //Set Header Image
                        imgHeader.ImageUrl = "~/UploadDocs/" + dataTableResult.Rows[0]["CompanyLogo"].ToString();

                        //Set Footer Copyright Line
                        lblFooter.Text = dataTableResult.Rows[0]["CopyRightLine"].ToString();

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
                        if (dataTableProgramme.Rows[0]["Logo"].ToString() != "")
                        {
                            imgProjectLogo.Visible = true;
                            imgProjectLogo.ImageUrl = "~/UploadDocs/" + dataTableProgramme.Rows[0]["Logo"].ToString();
                        }
                        else
                        {
                            imgProjectLogo.Visible = false;
                        }

                        //Set Menu Background Color
                        tdMenuBar.Attributes.Add("style", "background:" + dataTableResult.Rows[0]["MenuBGColor"].ToString() + ";text-align:right;padding-right:15px; padding-top:3px; color:#ffffff;");

                        Session["Count"] = 0;
                        //Bind question information
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
            if (dataTableResult.Rows.Count > 0)
                SetGraphData();

        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    /// <summary>
    /// Save Question answers
    /// </summary>
    protected void SaveQuestionAnswer()
    {
        try
        {
            QuestionAnswer_BAO questionAnswerBusinessAccessObject = new QuestionAnswer_BAO();

            string answer = "";
            int questionID = 0;

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater rptrQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");

                foreach (RepeaterItem item in rptrQuestionList.Items)
                {
                    QuestionAnswer_BE questionAnswerBusinessEntity = new QuestionAnswer_BE();

                    Label labelQuestionType = (Label)item.FindControl("lblQType");
                    Label labelQuestionId = (Label)item.FindControl("lblQId");
                    questionID = Convert.ToInt32(labelQuestionId.Text);

                    if (Convert.ToInt16(labelQuestionType.Text) == 1)
                    {
                        CKEditor.NET.CKEditorControl textBoxAnswers = (CKEditor.NET.CKEditorControl)item.FindControl("txtAnswers");

                        if (textBoxAnswers != null)
                        {
                            textBoxAnswers.config.toolbar = new object[] { };
                            textBoxAnswers.config.keystrokes = new object[] { };
                            textBoxAnswers.CssClass = "";
                            textBoxAnswers.AutoParagraph = false;
                            textBoxAnswers.ScaytAutoStartup = true;
                            textBoxAnswers.BrowserContextMenuOnCtrl = false;
                            textBoxAnswers.ForcePasteAsPlainText = true;
                            textBoxAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";
                            textBoxAnswers.AutoCompleteType = AutoCompleteType.None;
                            textBoxAnswers.AutoParagraph = false;
                            textBoxAnswers.ScaytAutoStartup = true;
                            textBoxAnswers.BrowserContextMenuOnCtrl = false;
                            textBoxAnswers.ForcePasteAsPlainText = false;
                            textBoxAnswers.IgnoreEmptyParagraph = true;
                            textBoxAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            textBoxAnswers.EnableTabKeyTools = false;
                            textBoxAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            textBoxAnswers.Entities = false;
                            textBoxAnswers.PasteFromWordNumberedHeadingToList = false;
                            textBoxAnswers.PasteFromWordRemoveStyles = true;

                            answer = textBoxAnswers.Text;
                        }

                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //    answer = txtAnswer.Text;
                    }
                    else
                    {
                        RadioButtonList radioButtonAnswer = (RadioButtonList)item.FindControl("rblAnswer");

                        if (radioButtonAnswer != null)
                        {
                            if (radioButtonAnswer.SelectedItem != null)
                            {
                                //   answer = rblAnswer.SelectedItem.Text;
                                answer = radioButtonAnswer.SelectedItem.Value;
                            }
                            else
                                answer = "";
                        }
                    }

                    questionAnswerBusinessEntity.AssignDetId = Convert.ToInt32(hdnCandidateId.Value);
                    questionAnswerBusinessEntity.QuestionID = questionID;
                    questionAnswerBusinessEntity.Answer = answer;
                    questionAnswerBusinessEntity.ModifyBy = 1;
                    questionAnswerBusinessEntity.ModifyDate = DateTime.Now;
                    questionAnswerBusinessEntity.IsActive = 1;
                    //Insert Answer
                    questionAnswerBusinessAccessObject.AddQuestionAnswer(questionAnswerBusinessEntity);
                }
            }

            SetGraphData();
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    /// <summary>
    /// Set Editor property by question type or set answers checkbox.
    /// </summary>
    /// <returns></returns>
    protected Int32 CheckQuestionAnswer()
    {
        int result = 1;
        try
        {
            QuestionAnswer_BAO questionAnswerBusinessAccessObject = new QuestionAnswer_BAO();
            string answer = "";

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater repeaterQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");

                foreach (RepeaterItem item in repeaterQuestionList.Items)
                {
                    Label labelValidation = (Label)item.FindControl("lblValidation");
                    Label labelQuestionType = (Label)item.FindControl("lblQType");
                    Label lblQuestionId = (Label)item.FindControl("lblQId");

                    if (Convert.ToInt16(labelQuestionType.Text) == 1)
                    {
                        CKEditor.NET.CKEditorControl textBoxAnswers = (CKEditor.NET.CKEditorControl)item.FindControl("txtAnswers");

                        if (textBoxAnswers != null)
                        {
                            textBoxAnswers.config.toolbar = new object[] { };
                            textBoxAnswers.config.keystrokes = new object[] { };
                            textBoxAnswers.CssClass = "";
                            textBoxAnswers.AutoParagraph = false;
                            textBoxAnswers.ScaytAutoStartup = true;
                            textBoxAnswers.BrowserContextMenuOnCtrl = false;
                            textBoxAnswers.ForcePasteAsPlainText = true;
                            textBoxAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";
                            textBoxAnswers.AutoCompleteType = AutoCompleteType.None;
                            textBoxAnswers.AutoParagraph = false;
                            textBoxAnswers.ScaytAutoStartup = true;
                            textBoxAnswers.BrowserContextMenuOnCtrl = false;
                            textBoxAnswers.ForcePasteAsPlainText = false;
                            textBoxAnswers.IgnoreEmptyParagraph = true;
                            textBoxAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            textBoxAnswers.EnableTabKeyTools = false;
                            textBoxAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            textBoxAnswers.Entities = false;
                            textBoxAnswers.PasteFromWordNumberedHeadingToList = false;
                            textBoxAnswers.PasteFromWordRemoveStyles = true;

                            answer = Convert.ToString(textBoxAnswers.Text);
                        }

                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //    answer = Convert.ToString(txtAnswer.Text);
                    }
                    else
                    {
                        RadioButtonList radioButtonAnswer = (RadioButtonList)item.FindControl("rblAnswer");

                        if (radioButtonAnswer != null)
                        {
                            if (radioButtonAnswer.SelectedItem != null)
                            {
                                //answer = rblAnswer.SelectedItem.Text;
                                answer = radioButtonAnswer.SelectedItem.Value;
                            }
                            else
                                answer = "";
                        }
                    }

                    if (labelValidation.Text == "2" && answer == string.Empty && result < 3)
                    {
                        if (Convert.ToString(ViewState["ValidationCheck"]) == "0" && result < 2)
                            result = 1;
                        else
                            result = 2;
                    }
                    else if (labelValidation.Text == "3" && answer == string.Empty)
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

    /// <summary>
    /// When click on prevoius save answer and bind question
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            int result = CheckQuestionAnswer();

            if (imbFinish.Visible == true) result = 1;

            if (result <= 1)
            {
                //Save answers
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

                    DataTable dataTableCategory = new DataTable();
                    dataTableCategory = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);
                    //Bind Questions
                    BindQuestions(currentCount);
                    //Set answer
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

                    DataTable dataTableCategory = new DataTable();
                    dataTableCategory = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);
                    //Bind Questions
                    BindQuestions(currentCount);
                    //Set answer
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

    /// <summary>
    /// When click on prevoius save answer and bind question
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbNext_Click(object sender, EventArgs e)
    {
        try
        {
            int result = CheckQuestionAnswer();

            if (result <= 1)
            {
                //Save questions answer
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
                    List<Questionnaire_BE.Questionnaire_BE> questionnaireBusinessEntityList = new List<Questionnaire_BE.Questionnaire_BE>();
                    questionnaireBusinessEntityList = questionnaireBusinessAccessObject.GetQuestionnaireByID(Convert.ToInt32(questionnaireID));
                    lblQuestionnaireText.Text = questionnaireBusinessEntityList[0].QSTNEpilogue.ToString();
                }
                else
                {
                    imbNext.Visible = true;
                    imbPrevious.Visible = true;

                    DataTable dataTableCategory = new DataTable();
                    dataTableCategory = (DataTable)Session["Category"];
                    currentCount = Convert.ToInt32(Session["Count"]);
                    //Bind questions
                    BindQuestions(currentCount);
                    //Set Answer
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

    /// <summary>
    /// Update Submit flag
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbFinish_Click(object sender, EventArgs e)
    {
        //if (cbxNotifyMail.Checked == true)
        //{
        DataTable dataTableProjectInformation = new DataTable();

        dataTableProjectInformation = questionnaireBusinessAccessObject.GetProjectQuestionnaireInfo(Convert.ToInt32(hdnQuestionnaireId.Value),
            Convert.ToInt32(hdnCandidateId.Value));

        DataTable dataTableResult = new DataTable();

        AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new AssignQuestionnaire_BAO();

        dataTableResult = assignQuestionnaireBusinessAccessObject.GetAllAssignmentInfo(Convert.ToInt32(hdnCandidateId.Value));

        //Send mail to candidates
        for (int i = 0; i < dataTableProjectInformation.Rows.Count; i++)
        {
            Template = System.IO.File.ReadAllText(Server.MapPath("~") + "\\UploadDocs\\FeedbackTemplate.txt");

            string candidatename = "";
            string candidateemail = "";
            string organizationname = "";
            string Projectname = "";
            string participantname = "";
            string participantemail = "";

            candidatename = dataTableResult.Rows[i]["CandidateName"].ToString();
            candidateemail = dataTableResult.Rows[i]["CandidateEmail"].ToString();
            organizationname = dataTableResult.Rows[i]["OrganisationName"].ToString();
            Projectname = dataTableProjectInformation.Rows[i]["Title"].ToString();
            participantname = dataTableProjectInformation.Rows[i]["Fullname"].ToString();
            participantemail = dataTableProjectInformation.Rows[i]["EmailID"].ToString();

            Template = Template.Replace("[TITLE]", Projectname);
            Template = Template.Replace("[EMAILID]", participantemail);
            Template = Template.Replace("[FIRSTNAME]", candidatename);
            Template = Template.Replace("[COMPANY]", organizationname);
            Template = Template.Replace("[PARTICIPANTNAME]", participantname);

            MailAddress eMailaddress = new MailAddress(candidateemail, candidatename);
            //SendEmail.Send("Questionnaire Feedback Submitted", Template, participantemail,maddr,"");

            lblMessage.Text = "Questionnaire has been submitted and email sent successfully.";
        }

        int result = questionnaireBusinessAccessObject.UpdateSubmitFlag(Convert.ToInt32(hdnCandidateId.Value), 1);
        //}
        //else
        //{
        //    lblMessage.Text = "Questionnaire has been submitted successfully.";
        //}
    }

    /// <summary>
    /// Bind Questions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbStart_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Count"] = 0;
            //Bind Questions
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

    /// <summary>
    /// Set Editor peoperty according to question type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrQuestionList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            RepeaterItem rpItem = e.Item;

            if (rpItem.ItemType == ListItemType.Footer)
            {
                Label labelNote2 = (Label)rpItem.FindControl("lblNote2");

                if (labelNote2 != null && hdnAccountId.Value.ToString() == "68")
                    labelNote2.Visible = false;
            }

            Label labelQuestionType = (Label)rpItem.FindControl("lblQType");
            Label labelQuestiontId = (Label)rpItem.FindControl("lblQId");
            Label labelQuestionText = (Label)rpItem.FindControl("lblQuestionText");
            Label labelQuestionHint = (Label)rpItem.FindControl("lblHint");
            HtmlTable tabelGraph = (HtmlTable)rpItem.FindControl("tblGraph");

            if (labelQuestionType != null)
            {
                if (labelQuestionType.Text == "2" && hdnAccountId.Value == "34")
                    tabelGraph.Visible = true;
                else
                    tabelGraph.Visible = false;

                DataTable dataTableQuestion = new DataTable();
                dataTableQuestion = (DataTable)Session["Questions"];

                DataTable dataTableQuestionClone = dataTableQuestion.Clone();
                RadioButton radioButtonNotApplicable = (RadioButton)rpItem.FindControl("rbtnNotApplicable");

                DataRow[] result = dataTableQuestion.Select("QuestionID =" + labelQuestiontId.Text);

                foreach (DataRow dataRow in result)
                    dataTableQuestionClone.ImportRow(dataRow);

                if (dataTableQuestionClone.Rows.Count > 0)
                {
                    labelQuestionText.Text = dataTableQuestionClone.Rows[0]["Description"].ToString();

                    if (dataTableQuestionClone.Rows[0]["Token"].ToString() == "1")
                        labelQuestionText.Text = labelQuestionText.Text.Replace("[TARGETNAME]", hdnFirstName.Value);
                    else if (dataTableQuestionClone.Rows[0]["Token"].ToString() == "2")
                        labelQuestionText.Text = labelQuestionText.Text.Replace("[TARGETNAME]", hdnLastName.Value);
                    else
                        labelQuestionText.Text = labelQuestionText.Text.Replace("[TARGETNAME]", lblParticipantName.Text);

                    //Replace Client Name keyword with actual client name
                    labelQuestionText.Text = labelQuestionText.Text.Replace("[CLIENTNAME]", hdnClientName.Value);

                }

                if (Convert.ToInt16(labelQuestionType.Text) == 1)
                {

                    RadioButtonList radioButtonAnswer = (RadioButtonList)rpItem.FindControl("rblAnswer");
                    radioButtonAnswer.Visible = false;

                    CKEditor.NET.CKEditorControl textBoxAnswers = (CKEditor.NET.CKEditorControl)rpItem.FindControl("txtAnswers");
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

                    if (textBoxAnswers != null)
                    {
                        textBoxAnswers.config.toolbar = new object[] { };
                        textBoxAnswers.config.keystrokes = new object[] { };
                        textBoxAnswers.CssClass = "";
                        textBoxAnswers.AutoParagraph = false;
                        textBoxAnswers.ScaytAutoStartup = true;
                        textBoxAnswers.BrowserContextMenuOnCtrl = false;
                        textBoxAnswers.ForcePasteAsPlainText = true;
                        textBoxAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";
                        textBoxAnswers.AutoCompleteType = AutoCompleteType.None;
                        textBoxAnswers.AutoParagraph = false;
                        textBoxAnswers.ScaytAutoStartup = true;
                        textBoxAnswers.BrowserContextMenuOnCtrl = false;
                        textBoxAnswers.ForcePasteAsPlainText = false;
                        textBoxAnswers.IgnoreEmptyParagraph = true;
                        textBoxAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                        textBoxAnswers.EnableTabKeyTools = false;
                        textBoxAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                        textBoxAnswers.Entities = false;
                        textBoxAnswers.PasteFromWordNumberedHeadingToList = false;
                        textBoxAnswers.PasteFromWordRemoveStyles = true;
                        textBoxAnswers.Attributes.Add("onkeypress", "javascript:TextAreaMaxLengthCheck(this.id," + dataTableQuestionClone.Rows[0]["LengthMAX"].ToString() + ");");

                        textBoxAnswers.Visible = true;
                    }

                    radioButtonNotApplicable.Visible = false;
                }
                else
                {
                    Label labelLowerLabel = (Label)rpItem.FindControl("lblLowerLabel");
                    Label labelUpperLabel = (Label)rpItem.FindControl("lblUpperLabel");
                    Label labelLowerBound = (Label)rpItem.FindControl("lblLowerBound");
                    Label labelUpperBound = (Label)rpItem.FindControl("lblUpperBound");
                    Label labelIncrement = (Label)rpItem.FindControl("lblIncrement");

                    RadioButtonList radioButtonListAnswer = (RadioButtonList)rpItem.FindControl("rblAnswer");
                    radioButtonListAnswer.Visible = true;
                    radioButtonListAnswer.CellPadding = 5;
                    radioButtonListAnswer.CellSpacing = 5;

                    //TextBox txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                    //txtAnswer.Visible = false;
                    CKEditor.NET.CKEditorControl textBoxAnswers = (CKEditor.NET.CKEditorControl)rpItem.FindControl("txtAnswers");
                    textBoxAnswers.config.toolbar = new object[] { };
                    textBoxAnswers.config.keystrokes = new object[] { };
                    textBoxAnswers.CssClass = "";
                    textBoxAnswers.AutoParagraph = false;
                    textBoxAnswers.ScaytAutoStartup = true;
                    textBoxAnswers.BrowserContextMenuOnCtrl = false;
                    textBoxAnswers.ForcePasteAsPlainText = true;
                    textBoxAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                    textBoxAnswers.AutoCompleteType = AutoCompleteType.None;
                    textBoxAnswers.AutoParagraph = false;
                    textBoxAnswers.ScaytAutoStartup = true;
                    textBoxAnswers.BrowserContextMenuOnCtrl = false;
                    textBoxAnswers.ForcePasteAsPlainText = false;
                    textBoxAnswers.IgnoreEmptyParagraph = true;
                    textBoxAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                    textBoxAnswers.EnableTabKeyTools = false;
                    textBoxAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                    textBoxAnswers.Entities = false;
                    textBoxAnswers.PasteFromWordNumberedHeadingToList = false;
                    textBoxAnswers.PasteFromWordRemoveStyles = true;

                    textBoxAnswers.Visible = false;

                    DataTable dataTableValues = new DataTable();

                    dataTableValues.Columns.Add("Id");
                    dataTableValues.Columns.Add("Value");

                    for (int counter = Convert.ToInt32(labelLowerBound.Text); counter <= Convert.ToInt32(labelUpperBound.Text); counter = counter + Convert.ToInt32(labelIncrement.Text))
                        dataTableValues.Rows.Add(counter.ToString() + "&nbsp;&nbsp;", counter.ToString() + "&nbsp;&nbsp;");

                    dataTableValues.Rows.Add("N/A", "N/A");

                    dataTableValues.Rows[Convert.ToInt32(dataTableValues.Rows.Count) - 2]["Value"] = labelUpperBound.Text + "&nbsp;&nbsp;&nbsp;<b>" + dataTableQuestionClone.Rows[0]["UpperLabel"].ToString() + "</b>&nbsp;&nbsp;&nbsp;";

                    radioButtonListAnswer.DataSource = dataTableValues;
                    radioButtonListAnswer.DataValueField = "Id";
                    radioButtonListAnswer.DataTextField = "Value";
                    radioButtonListAnswer.DataBind();

                    radioButtonNotApplicable.Visible = false;
                    labelUpperLabel.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }

    /// <summary>
    /// Bind Question list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrQuestionListMain_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            DataTable dataTableQuestions = new DataTable();
            dataTableQuestions = (DataTable)Session["Questions"];

            DataTable dataTableQuestionsClone = dataTableQuestions.Clone();

            Repeater repeaterQuestionList = (Repeater)e.Item.FindControl("rptrQuestionList");
            Label LabelQuestionnaireDescription = (Label)e.Item.FindControl("LabelQuestionnaireDescription");
            Label labelCategoryTitle = (Label)e.Item.FindControl("lblCategoryTitle");
            Label labelCategoryID = (Label)e.Item.FindControl("lblCategoryID");

            if (labelCategoryID != null)
            {
                DataRow[] result = dataTableQuestions.Select("CategoryID=" + labelCategoryID.Text, "Sequence");

                foreach (DataRow dataRow in result)
                    dataTableQuestionsClone.ImportRow(dataRow);

                if (dataTableQuestionsClone.Rows.Count > 0)
                {
                    repeaterQuestionList.DataSource = dataTableQuestionsClone;
                    repeaterQuestionList.DataBind();
                    //lblCategoryName.Text = dtClone.Rows[0]["CategoryName"].ToString();
                    labelCategoryTitle.Text = dataTableQuestionsClone.Rows[0]["CategoryTitle"].ToString();
                    LabelQuestionnaireDescription.Text = dataTableQuestionsClone.Rows[0].Field<string>("QuestionnaireCategoryDescription");
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
    /// <summary>
    /// Calculate % of survey completion ans ser prologue text.
    /// </summary>
    private void SetGraphData()
    {
        int answeredQuestion = questionnaireBusinessAccessObject.CalculateGraph(Convert.ToInt32(hdnQuestionnaireId.Value), Convert.ToInt32(hdnCandidateId.Value));

        double percentage = answeredQuestion == 0 ? 0 : (answeredQuestion * 100) / Convert.ToInt32(hdnQuestionCount.Value);
        percentage = Convert.ToInt32(Math.Abs(percentage));
        tbGraph.Width = percentage.ToString() + "%";

        lblCompletionStatus.Text = percentage + "%";
        //lblCompletionStatus.ForeColor = System.Drawing.Color.Red;

        //Set Prologue of questionnaire
        List<Questionnaire_BE.Questionnaire_BE> questionnaireBusinessEntityList = new List<Questionnaire_BE.Questionnaire_BE>();

        questionnaireBusinessEntityList = questionnaireBusinessAccessObject.GetQuestionnaireByID(Convert.ToInt32(hdnQuestionnaireId.Value));

        lblQuestionnaireText.Text = questionnaireBusinessEntityList[0].QSTNPrologue.ToString();
    }

    /// <summary>
    /// Bind questions and set answer
    /// </summary>
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
                dataTableQuestion = questionnaireBusinessAccessObject.GetFeedbackQuestionnaireByRelationShip(AccountID, ProjectId, qID, Relationship);
            else
                dataTableQuestion = questionnaireBusinessAccessObject.GetFeedbackQuestionnaireSelfByRelationShip(AccountID, ProjectId, qID, Relationship);

            Session["Questions"] = dataTableQuestion;

            dataTableCategory = questionnaireBusinessAccessObject.GetQuestionnaireCategoriesByRelationShip(AccountID, ProjectId, qID, Relationship);

            Session["Category"] = dataTableCategory;

            Session["categoryCount"] = (Math.Abs(dataTableCategory.Rows.Count / Convert.ToInt32(hdnIncrementValue.Value)));

            if (dataTableCategory.Rows.Count % Convert.ToInt32(hdnIncrementValue.Value) > 0)
                Session["categoryCount"] = Convert.ToInt32(Session["categoryCount"]) + 1;

            questionCount = dataTableQuestion.Rows.Count;
            hdnQuestionCount.Value = questionCount.ToString(); ;

            if (questionCount > 0)
            {
                DataTable CategorydataTable = new DataTable();
                CategorydataTable = (DataTable)Session["Category"];
                currentCount = Convert.ToInt32(Session["Count"]);

                //Bind Question
                BindQuestions(currentCount);
                //Set Answer if given
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

    /// <summary>
    /// Bind questions
    /// </summary>
    /// <param name="qstCount"></param>
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

            DataTable dataTableCategory = new DataTable();
            dataTableCategory = (DataTable)Session["Category"];

            DataTable dataTableCategoryClone = dataTableCategory.Clone();

            DataRow[] result = dataTableCategory.Select("RowNumber >=" + countFrom + " and RowNumber <=" + countTo);

            //DataRow[] result = dt.Select("CategoryID=" + qstCount,"Sequence" );

            foreach (DataRow dataRow in result)
                dataTableCategoryClone.ImportRow(dataRow);

            rptrQuestionListMain.DataSource = dataTableCategoryClone;
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

    /// <summary>
    /// Set Answers
    /// </summary>
    private void SetQuestionAnswer()
    {
        try
        {
            int questionID = 0;
            int candidateId = 0;

            foreach (RepeaterItem rptrItem in rptrQuestionListMain.Items)
            {
                Repeater repeaterQuestionList = (Repeater)rptrItem.FindControl("rptrQuestionList");

                foreach (RepeaterItem item in repeaterQuestionList.Items)
                {
                    Label labelQuestionId = (Label)item.FindControl("lblQId");
                    Label labelQuestionType = (Label)item.FindControl("lblQType");
                    Label labelLowerLabel = (Label)item.FindControl("lblLowerLabel");
                    Label labelUpperLabel = (Label)item.FindControl("lblUpperLabel");
                    Label labelLowerBound = (Label)item.FindControl("lblLowerBound");
                    Label labelUpperBound = (Label)item.FindControl("lblUpperBound");
                    Label labelIncrement = (Label)item.FindControl("lblIncrement");
                    RadioButton rdbtnNA = (RadioButton)item.FindControl("rbtnNotApplicable");

                    if (labelQuestionId != null)
                        questionID = Convert.ToInt32(labelQuestionId.Text);

                    candidateId = Convert.ToInt32(hdnCandidateId.Value);

                    QuestionAnswer_BAO questionAnswerBusinessAccessObject = new QuestionAnswer_BAO();

                    string answer = questionAnswerBusinessAccessObject.GetQuestionAnswer(candidateId, questionID);

                    if (Convert.ToInt16(labelQuestionType.Text) == 1)
                    {
                        CKEditor.NET.CKEditorControl textBoxAnswers = (CKEditor.NET.CKEditorControl)item.FindControl("txtAnswers");

                        if (textBoxAnswers != null)
                        {
                            textBoxAnswers.config.toolbar = new object[] { };
                            textBoxAnswers.config.keystrokes = new object[] { };
                            textBoxAnswers.CssClass = "";
                            textBoxAnswers.AutoParagraph = false;
                            textBoxAnswers.ScaytAutoStartup = true;
                            textBoxAnswers.BrowserContextMenuOnCtrl = false;
                            textBoxAnswers.ForcePasteAsPlainText = true;
                            textBoxAnswers.config.removeFormatTags = "b,big,code,del,dfn,em,font,i,ins,kbd,q,samp,small,span,strike,strong,sub,sup,tt,u,var";

                            textBoxAnswers.AutoCompleteType = AutoCompleteType.None;
                            textBoxAnswers.AutoParagraph = false;
                            textBoxAnswers.ScaytAutoStartup = true;
                            textBoxAnswers.BrowserContextMenuOnCtrl = false;
                            textBoxAnswers.ForcePasteAsPlainText = false;
                            textBoxAnswers.IgnoreEmptyParagraph = true;
                            textBoxAnswers.ContentsLangDirection = CKEditor.NET.contentsLangDirections.Ltr;
                            textBoxAnswers.EnableTabKeyTools = false;
                            textBoxAnswers.EnterMode = CKEditor.NET.EnterMode.BR;
                            textBoxAnswers.Entities = false;
                            textBoxAnswers.PasteFromWordNumberedHeadingToList = false;
                            textBoxAnswers.PasteFromWordRemoveStyles = true;

                            textBoxAnswers.Text = answer;

                        }
                        //TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                        //if (txtAnswer != null)
                        //    txtAnswer.Text = answer;
                    }
                    else
                    {
                        RadioButtonList radioButtonListAnswer = (RadioButtonList)item.FindControl("rblAnswer");
                        radioButtonListAnswer.Visible = true;
                        radioButtonListAnswer.CellPadding = 5;
                        radioButtonListAnswer.CellSpacing = 5;

                        DataTable dataTableValues = new DataTable();

                        dataTableValues.Columns.Add("Id");
                        dataTableValues.Columns.Add("Value");

                        for (int counter = Convert.ToInt32(labelLowerBound.Text); counter <= Convert.ToInt32(labelUpperBound.Text);
                            counter = counter + Convert.ToInt32(labelIncrement.Text))

                            dataTableValues.Rows.Add(counter.ToString(), counter.ToString());

                        dataTableValues.Rows.Add("N/A", "N/A");

                        //dtValues.Rows[Convert.ToInt32(dtValues.Rows.Count) - 2]["Value"] = lblUpperBound.Text + "&nbsp;&nbsp;&nbsp;<b>" + lblUpperLabel.Text + "</b>&nbsp;&nbsp;&nbsp;";

                        dataTableValues.Rows[Convert.ToInt32(dataTableValues.Rows.Count) - 2]["Value"] = labelUpperBound.Text + "</label></td><td>&nbsp;&nbsp;</td><td ><b>" + labelUpperLabel.Text + "</b></td><td><label>&nbsp;&nbsp;";

                        radioButtonListAnswer.DataSource = dataTableValues;
                        radioButtonListAnswer.DataValueField = "Id";
                        radioButtonListAnswer.DataTextField = "Value";
                        radioButtonListAnswer.DataBind();

                        if (radioButtonListAnswer != null && answer != "")
                        {
                            for (int i = 0; i < dataTableValues.Rows.Count; i++)
                            {
                                if (radioButtonListAnswer.Items[i].Text == answer)
                                {
                                    radioButtonListAnswer.Items[i].Selected = true;
                                    break;
                                }
                                else
                                {
                                    if (radioButtonListAnswer.Items[i].Text.Contains("</label>"))
                                    {
                                        if (((radioButtonListAnswer.Items[i].Text.Split('<'))[0]).Trim() == answer)
                                        {
                                            radioButtonListAnswer.Items[i].Selected = true;
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
