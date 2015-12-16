using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Module_Feedback_QuestionReview : System.Web.UI.Page
{
    CodeBehindBase cBase = new CodeBehindBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
            List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Questionnaire_BE>();

            int questionnaireID = Convert.ToInt32(Request.QueryString["QstnId"]);
            questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(questionnaireID);

            lblQuestionnaireName.Text = questionnaire_BEList[0].QSTNName.ToString();

            DataTable dtResult = new DataTable();
            //Get Questionnaire Categories list by Questionnaire id.
            dtResult = questionnaire_BAO.GetQuestionnaireCategories(questionnaireID);

            if (dtResult.Rows.Count > 0)
            {
                //Bind question repeator.
                rptrQstCategory.DataSource = dtResult;
                rptrQstCategory.DataBind();
            }
            else
            {
                lblMessage.Text = "No record found";
            }
        }
    }

    /// <summary>
    /// Bind question type by category.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrQstCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            RepeaterItem rpItem = e.Item;

            Label labelCategoryID = (Label)rpItem.FindControl("lblCategoryID");
            Repeater repeaterQuestion = (Repeater)rpItem.FindControl("rptrQuestion");

            if (repeaterQuestion != null)
            {
                Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
                DataTable dataTableResult = new DataTable();
                //Get Question by category or by category Id.
                dataTableResult = questionnaire_BAO.GetCategoryQuestions(Convert.ToInt32(labelCategoryID.Text));
                if (dataTableResult.Rows.Count > 0)
                {
                    //Bind question grid.
                    repeaterQuestion.DataSource = dataTableResult;
                    repeaterQuestion.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }
}
