using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BAO;
using Miscellaneous;
using Administration_BAO;

public partial class Survey_Module_Feedback_QuestionReview : System.Web.UI.Page
{
    
    CodeBehindBase cBase = new CodeBehindBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Label ll = (Label)this.Master.FindControl("Current_location");
        //ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        if (!IsPostBack)
        {
            Questionnaire_BAO.Survey_Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
            List<Questionnaire_BE.Survey_Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Survey_Questionnaire_BE>();

            int questionnaireID = Convert.ToInt32(Request.QueryString["QstnId"]);
            questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(questionnaireID);

            lblQuestionnaireName.Text = questionnaire_BEList[0].QSTNName.ToString();

            DataTable dtResult = new DataTable();
            dtResult = questionnaire_BAO.GetQuestionnaireCategories(questionnaireID);

            if (dtResult.Rows.Count > 0)
            {
                rptrQstCategory.DataSource = dtResult;
                rptrQstCategory.DataBind();
            }
            else
            {
                lblMessage.Text = "No record found";
            }
        }
    }

    protected void rptrQstCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            RepeaterItem rpItem = e.Item;

            Label lblCatID = (Label)rpItem.FindControl("lblCategoryID");
            Repeater rptrQst = (Repeater)rpItem.FindControl("rptrQuestion");

            if (rptrQst != null)
            {
                Questionnaire_BAO.Survey_Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
                DataTable dtResult = new DataTable();
                dtResult = questionnaire_BAO.GetCategoryQuestions(Convert.ToInt32(lblCatID.Text));
                if (dtResult.Rows.Count > 0)
                {
                    rptrQst.DataSource = dtResult;
                    rptrQst.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            cBase.HandleExceptionError(ex);
        }
    }
    
}
