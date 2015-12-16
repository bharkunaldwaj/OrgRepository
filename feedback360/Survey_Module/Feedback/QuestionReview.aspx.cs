using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BAO;
using Questionnaire_BE;

public partial class Survey_Module_Feedback_QuestionReview : System.Web.UI.Page
{
    CodeBehindBase cBase = new CodeBehindBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Label ll = (Label)this.Master.FindControl("Current_location");
        //ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        if (!IsPostBack)
        {
            Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
            List<Survey_Questionnaire_BE> questionnaireBusinesEntityList = new List<Survey_Questionnaire_BE>();

            int questionnaireID = Convert.ToInt32(Request.QueryString["QstnId"]);
            //Get Questionnaire By ID
            questionnaireBusinesEntityList = questionnaireBusinessAccessObject.GetQuestionnaireByID(questionnaireID);
            //set Questionnaire Name
            lblQuestionnaireName.Text = questionnaireBusinesEntityList[0].QSTNName.ToString();

            DataTable dataTableResult = new DataTable();
            //Get Questionnaire category
            dataTableResult = questionnaireBusinessAccessObject.GetQuestionnaireCategories(questionnaireID);

            if (dataTableResult.Rows.Count > 0)
            {
                rptrQstCategory.DataSource = dataTableResult;
                rptrQstCategory.DataBind();
            }
            else
            {
                lblMessage.Text = "No record found";
            }
        }
    }

    /// <summary>
    /// Bind Question grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrQstCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            RepeaterItem rpItem = e.Item;
            //Find repeater controls
            Label labelCategoryID = (Label)rpItem.FindControl("lblCategoryID");
            Repeater repeaterQuestion = (Repeater)rpItem.FindControl("rptrQuestion");

            if (repeaterQuestion != null)
            {
                Survey_Questionnaire_BAO questionnaire_BAO = new Survey_Questionnaire_BAO();
                DataTable dataTableResult = new DataTable();
                //Get Question by category
                dataTableResult = questionnaire_BAO.GetCategoryQuestions(Convert.ToInt32(labelCategoryID.Text));

                if (dataTableResult.Rows.Count > 0)
                {
                    //Bind grid
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
