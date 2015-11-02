using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Diagnostics;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Module_Questionnaire_Questionnaire : CodeBehindBase
{    
    Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();
    Questionnaire_BE.Questionnaire_BE questionnaire_BE = new Questionnaire_BE.Questionnaire_BE();
    List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList = new List<Questionnaire_BE.Questionnaire_BE>();
    DataTable dtCompanyName;
    DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;

            //Project_BAO project_BAO = new Project_BAO();
            //ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(identity.User.AccountID));
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataBind();

            Account_BAO account_BAO = new Account_BAO();
            ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();


            

            if (identity.User.GroupID == 1)
            {
                divAccount.Visible = true;
                if (Request.QueryString["Mode"] == null)
                {
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
            }
            else
            {
                divAccount.Visible = false;
            }
        
        
        
        }


        int questionnaireID = Convert.ToInt32(Request.QueryString["QestId"]);
        questionnaire_BEList = questionnaire_BAO.GetQuestionnaireByID(questionnaireID);

        if (questionnaire_BEList.Count > 0)
        {
            SetQuestionnaireValue(questionnaire_BEList);

            ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
            ddlAccountCode_SelectedIndexChanged(sender, e);
        }

        if (Request.QueryString["Mode"] == "E")
        {
            ibtnSave.Visible = true;
            ibtnCancel.Visible = true;
            imbBack.Visible = false;
            lblheader.Text = "Edit Questionnaire";
        }
        else if (Request.QueryString["Mode"] == "R")
        {
            ibtnSave.Visible = false;
            ibtnCancel.Visible = false;
            imbBack.Visible = true;
            lblheader.Text = "View Questionnaire";
        }


    }

    private void SetQuestionnaireValue(List<Questionnaire_BE.Questionnaire_BE> questionnaire_BEList)
    {
        try
        {
            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = questionnaire_BEList[0].AccountID.ToString();

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    Account_BAO account1_BAO = new Account_BAO();
                    dtCompanyName = account1_BAO.GetdtAccountList(Convert.ToString(companycode));
                    DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");

                    DataTable dtAccount = dtCompanyName.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                    {
                        dtAccount.ImportRow(drAccount);
                    }

                    lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }

            ddlType.SelectedValue = questionnaire_BEList[0].QSTNType.ToString();
            txtquestionnairecode.Text = questionnaire_BEList[0].QSTNCode.ToString();
            txtquestionnairename.Text = questionnaire_BEList[0].QSTNName.ToString();
            txtDescription.Text = questionnaire_BEList[0].QSTNDescription.ToString();
            txtDisplayCategory.Text = questionnaire_BEList[0].DisplayCategory.ToString();
            //ddlProject.SelectedValue = questionnaire_BEList[0].ProjectID.ToString();            
            txtPrologueEditor.InnerHtml = Server.HtmlDecode(questionnaire_BEList[0].QSTNPrologue.ToString());
            txtEpilogueEditor.InnerHtml = Server.HtmlDecode(questionnaire_BEList[0].QSTNEpilogue.ToString());           

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

    }      

    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Questionnaire_BE.Questionnaire_BE questionnaire_BE = new Questionnaire_BE.Questionnaire_BE();
            Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {

                questionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

            }
            else
            {
                questionnaire_BE.AccountID = identity.User.AccountID;
            }

            questionnaire_BE.QSTNType = Convert.ToInt32(GetString(ddlType.SelectedValue));
            questionnaire_BE.QSTNCode = GetString(txtquestionnairecode.Text);
            questionnaire_BE.QSTNName = GetString(txtquestionnairename.Text);
            questionnaire_BE.QSTNDescription = GetString(txtDescription.Text);
            questionnaire_BE.DisplayCategory = Convert.ToInt32(txtDisplayCategory.Text.Trim());

            //questionnaire_BE.ProjectID = Convert.ToInt32(GetString(ddlProject.SelectedValue));
            questionnaire_BE.ManagerID = 3;
            questionnaire_BE.QSTNPrologue = GetString(Server.HtmlDecode(txtPrologueEditor.Value));
            questionnaire_BE.QSTNEpilogue = GetString(Server.HtmlDecode(txtEpilogueEditor.Value));
            questionnaire_BE.ModifyBy = 1;
            questionnaire_BE.ModifyDate = DateTime.Now;
            questionnaire_BE.IsActive = 1;

            if (Request.QueryString["Mode"] == "E")
            {
                questionnaire_BE.QuestionnaireID = Convert.ToInt32(Request.QueryString["QestId"]);
                questionnaire_BAO.UpdateQuestionnaire(questionnaire_BE);
            }
            else
            {
                questionnaire_BAO.AddQuestionnaire(questionnaire_BE);
            }

            Response.Redirect("QuestionnaireList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("QuestionnaireList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Project_BAO project_BAO = new Project_BAO();
        
        //ddlProject.Items.Clear();
        //ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO account_BAO = new Account_BAO();

            dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dtCompanyName.Clone();
            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            //ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(ddlAccountCode.SelectedValue));
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataBind();

            txtPrologueEditor.InnerHtml = Server.HtmlDecode(txtPrologueEditor.InnerHtml);
            txtEpilogueEditor.InnerHtml = Server.HtmlDecode(txtEpilogueEditor.InnerHtml);
        }
        else
        {
            lblcompanyname.Text = "";

            //ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(identity.User.AccountID));
            //ddlProject.DataTextField = "Title";
            //ddlProject.DataValueField = "ProjectID";
            //ddlProject.DataBind();
        }
    }

}
