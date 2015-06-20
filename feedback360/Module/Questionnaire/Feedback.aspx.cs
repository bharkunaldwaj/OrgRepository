using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;

using Miscellaneous;
using Questionnaire_BE;
using Questionnaire_BAO;

public partial class Module_Questionnaire_Feedback : CodeBehindBase
{
    
    Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
    DataTable dtQuestion = new DataTable();
    Int32 questionCount = 0;
    Int32 currentCount = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //string str = PasswordGenerator.EnryptString("1");
                //str = PasswordGenerator.DecryptString(str);

                if (Request.QueryString["QID"] != null && Request.QueryString["CID"] != null)
                {
                    //Get the Candidate Information 
                    string candidateID = Convert.ToString(Request.QueryString["CID"]);
                    candidateID = PasswordGenerator.DecryptString(candidateID);
                    hdnCandidateId.Value = candidateID;

                    //Get questionnaire ID
                    string questionnaireID = Convert.ToString(Request.QueryString["QID"]);
                    questionnaireID = PasswordGenerator.DecryptString(questionnaireID);

                    //Get the Questionnaire/Question Information 
                    BindQuestionInformation();

                    //Set Prolog of questionnaire
                    List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Questionnaire_BE>();
                    questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(Convert.ToInt32(questionnaireID));
                    lblQuestionnaireText.Text = questionnaire_BEList[0].QSTNPrologue.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
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
            questionCount = dtQuestion.Rows.Count;
            hdnQuestionCount.Value = questionCount.ToString(); ;
            hdnCurrentCount.Value = "1";

            if (questionCount > 0)
            {
                BindQuestions(currentCount);
                SetQuestionAnswer();

                divText.Visible = true;
                divStartEndButton.Visible = true;
                imbStart.Visible = true;
                imbFinish.Visible = false;

                rptrQuestionList.Visible = false;
                divQuestionButton.Visible = false;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void rptrQuestionList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            RepeaterItem rpItem = e.Item;

            Label lblQType = (Label)rpItem.FindControl("lblQType");
            if (lblQType != null)
            {
                if (Convert.ToInt16(lblQType.Text) == 1)
                {

                    RadioButtonList rblAnswer = (RadioButtonList)rpItem.FindControl("rblAnswer");
                    rblAnswer.Visible = false;

                    TextBox txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                    txtAnswer.TextMode = TextBoxMode.MultiLine;
                    txtAnswer.Rows = 3;
                    txtAnswer.Visible = true;
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

                    TextBox txtAnswer = (TextBox)rpItem.FindControl("txtAnswer");
                    txtAnswer.Visible = false;

                    DataTable dtValues = new DataTable();

                    dtValues.Columns.Add("Id");
                    dtValues.Columns.Add("Value");

                    for (int counter = Convert.ToInt32(lblLowerBound.Text); counter <= Convert.ToInt32(lblUpperBound.Text); counter = counter + Convert.ToInt32(lblIncrement.Text))
                        dtValues.Rows.Add(counter.ToString(), counter.ToString());

                    rblAnswer.DataSource = dtValues;
                    rblAnswer.DataValueField = "Id";
                    rblAnswer.DataTextField = "Value";
                    rblAnswer.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void BindQuestions(int qstCount)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Questions"];

            DataTable dtClone = dt.Clone();

            DataRow[] result = dt.Select("RowNumber=" + qstCount);

            foreach (DataRow dr in result)
                dtClone.ImportRow(dr);

            if (dtClone.Rows.Count > 0)
            {
                rptrQuestionList.DataSource = dtClone;
                rptrQuestionList.DataBind();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }   

    private void SetQuestionAnswer()
    {
        try
        {
            int questionID = 0;
            int candidateId = 0;

            foreach (RepeaterItem item in rptrQuestionList.Items)
            {
                Label lblQId = (Label)item.FindControl("lblQId");
                Label lblQType = (Label)item.FindControl("lblQType");
                Label lblLowerLabel = (Label)item.FindControl("lblLowerLabel");
                Label lblUpperLabel = (Label)item.FindControl("lblUpperLabel");
                Label lblLowerBound = (Label)item.FindControl("lblLowerBound");
                Label lblUpperBound = (Label)item.FindControl("lblUpperBound");
                Label lblIncrement = (Label)item.FindControl("lblIncrement");

                if (lblQId != null)
                    questionID = Convert.ToInt32(lblQId.Text);
                candidateId = Convert.ToInt32(hdnCandidateId.Value);

                QuestionAnswer_BAO questionAnswer_BAO = new QuestionAnswer_BAO();
                string answer = questionAnswer_BAO.GetQuestionAnswer(candidateId, questionID);

                if (Convert.ToInt16(lblQType.Text) == 1)
                {
                    TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                    if (txtAnswer != null)
                        txtAnswer.Text = answer;
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

                    for (int counter = Convert.ToInt32(lblLowerBound.Text); counter <= Convert.ToInt32(lblUpperBound.Text); counter = counter + Convert.ToInt32(lblIncrement.Text))
                        dtValues.Rows.Add(counter.ToString(), counter.ToString());

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
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void SaveQuestionAnswer()
    {
        try
        {
            QuestionAnswer_BE questionAnswer_BE = new QuestionAnswer_BE();
            string answer = "";
            int questionID = 0;

            foreach (RepeaterItem item in rptrQuestionList.Items)
            {
                Label lblQType = (Label)item.FindControl("lblQType");
                Label lblQId = (Label)item.FindControl("lblQId");
                questionID = Convert.ToInt32(lblQId.Text);

                if (Convert.ToInt16(lblQType.Text) == 1)
                {
                    TextBox txtAnswer = (TextBox)item.FindControl("txtAnswer");
                    if (txtAnswer != null)
                        answer = txtAnswer.Text;
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
            }

            questionAnswer_BE.AssignDetId = Convert.ToInt32(hdnCandidateId.Value);
            questionAnswer_BE.QuestionID = questionID;
            questionAnswer_BE.Answer = answer;
            questionAnswer_BE.ModifyBy = 1;
            questionAnswer_BE.ModifyDate = DateTime.Now;
            questionAnswer_BE.IsActive = 1;

            QuestionAnswer_BAO questionAnswer_BAO = new QuestionAnswer_BAO();
            questionAnswer_BAO.AddQuestionAnswer(questionAnswer_BE);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void imbPrevious_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            SaveQuestionAnswer();
            BindQuestions(Convert.ToInt32(hdnCurrentCount.Value) - 1);

            SetQuestionAnswer();

            hdnCurrentCount.Value = Convert.ToString(Convert.ToInt32(hdnCurrentCount.Value) - 1);

            if (Convert.ToInt32(hdnCurrentCount.Value) == 0)
            {
                divText.Visible = true;
                divStartEndButton.Visible = true;
                imbStart.Visible = true;
                imbFinish.Visible = false;

                rptrQuestionList.Visible = false;
                divQuestionButton.Visible = false;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void imbNext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            SaveQuestionAnswer();
            BindQuestions(Convert.ToInt32(hdnCurrentCount.Value) + 1);

            SetQuestionAnswer();

            hdnCurrentCount.Value = Convert.ToString(Convert.ToInt32(hdnCurrentCount.Value) + 1);

            if (Convert.ToInt32(hdnCurrentCount.Value) == (Convert.ToInt32(hdnQuestionCount.Value) + 1))
            {
                divText.Visible = true;
                divStartEndButton.Visible = true;
                imbStart.Visible = false;
                imbFinish.Visible = true;

                rptrQuestionList.Visible = false;
                divQuestionButton.Visible = false;

                //Get questionnaire ID
                string questionnaireID = Convert.ToString(Request.QueryString["QID"]);
                questionnaireID = PasswordGenerator.DecryptString(questionnaireID);

                //Set Prolog of questionnaire
                List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Questionnaire_BE>();
                questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(Convert.ToInt32(questionnaireID));
                lblQuestionnaireText.Text = questionnaire_BEList[0].QSTNEpilogue.ToString();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void imbFinish_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void imbStart_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BindQuestionInformation();

            divText.Visible = false;
            divStartEndButton.Visible = false;

            rptrQuestionList.Visible = true;
            divQuestionButton.Visible = true;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
}
