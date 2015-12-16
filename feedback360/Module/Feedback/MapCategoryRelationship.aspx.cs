using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin_BAO;
using Questionnaire_BAO;
using Questionnaire_BE;
using System.Collections;
using System.Data;
using Administration_BE;

public partial class Survey_Module_Admin_MapCategoryRelationship : System.Web.UI.Page
{
    //Global variables
    WADIdentity identity;
    DataTable dataTableCompanyName;
    List<AssignedCategories_BE> assignedCategoriesBusinessEntityList = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            identity = this.Page.User.Identity as WADIdentity;

            if (!IsPostBack)
            {
                FeedbackAccount_BAO accountBusinessAccessObject = new FeedbackAccount_BAO();
                //Get Account Details by user Account Id.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
            }
        }
        catch { }
    }

    /// <summary>
    /// when Account selected index change then bind project and account details asections.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        // AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
        // ddlManager.Items.Clear();
        // ddlManager.Items.Insert(0, new ListItem("Select", "0"));
        ddlProject.Items.Clear();
        ddlProject.Items.Add(new ListItem("Select", "0"));
        tblAssignCategories.Visible = false;

        imbAssign.Visible = false;
        lblSuccessMessage.Text = "";

        tblNoData.Visible = false;
        lblNoData.Text = "";

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            FeedbackAccount_BAO accountBusinessAccessObject = new FeedbackAccount_BAO();

            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultentAccountDetail = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultentAccountDetail)
                dataTableAccount.ImportRow(dataRowAccount);

            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;

            FeedbackProject_BAO projectBusinessAccessObject = new FeedbackProject_BAO();
            //Get Project details by account id.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";
            ddlProject.Items.Clear();
            ViewState["AccountID"] = "0";
        }
    }

    /// <summary>
    /// Bind category relation ship details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        FeedbackProject_BAO projectBusinessAccessObject = new FeedbackProject_BAO();
        try
        {
            //Get project relationship details by Account and user Id.
            List<FeedbackProject_BE> listSurveyProjects = projectBusinessAccessObject.GetProjectByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue));

            if (listSurveyProjects != null && listSurveyProjects.Count() > 0)
            {
                DataTable outPutTable = new DataTable();
                outPutTable.Columns.Add(new DataColumn("PROJ_ID", typeof(System.Int32)));
                outPutTable.Columns.Add(new DataColumn("RelationShip", typeof(System.String)));

                //Add Row to dataTable.
                DataRow firstRow = outPutTable.NewRow();
                firstRow["PROJ_ID"] = ddlProject.SelectedValue;
                firstRow["RelationShip"] = listSurveyProjects[0].Relationship1;
                outPutTable.Rows.Add(firstRow);

                DataRow secondRow = outPutTable.NewRow();
                secondRow["PROJ_ID"] = ddlProject.SelectedValue;
                secondRow["RelationShip"] = listSurveyProjects[0].Relationship2;
                outPutTable.Rows.Add(secondRow);

                DataRow thirdRow = outPutTable.NewRow();
                thirdRow["PROJ_ID"] = ddlProject.SelectedValue;
                thirdRow["RelationShip"] = listSurveyProjects[0].Relationship3;
                outPutTable.Rows.Add(thirdRow);

                DataRow fourthRow = outPutTable.NewRow();
                fourthRow["PROJ_ID"] = ddlProject.SelectedValue;
                fourthRow["RelationShip"] = listSurveyProjects[0].Relationship4;
                outPutTable.Rows.Add(fourthRow);

                DataRow fifthRow = outPutTable.NewRow();
                fifthRow["PROJ_ID"] = ddlProject.SelectedValue;
                fifthRow["RelationShip"] = listSurveyProjects[0].Relationship5;
                outPutTable.Rows.Add(fifthRow);

                //Bind list Relationship 
                rptrRelationList.DataSource = outPutTable;
                rptrRelationList.DataBind();

                tblAssignCategories.Visible = true;
                imbAssign.Visible = true;

                tblNoData.Visible = false;
                lblNoData.Text = "";

                //Table tbl = new Table();
                //TableRow tr = new TableRow();
                //TableCell tc = new TableCell();
                //tbl.Rows.Add(tr);
                //tr.Controls.Add(tc);
                //tc.Text = Survey_Projects.First().Relationship1;
            }
            else
            {
                tblAssignCategories.Visible = false;
                tblNoData.Visible = true;
                lblNoData.Text = "No Data";
            }
        }
        catch (Exception ex)
        {
            tblAssignCategories.Visible = false;

            tblNoData.Visible = true;
            lblNoData.Text = "Some error has occured. Please contact your administrator.";
        }
    }

    /// <summary>
    /// Bind categories datalist by relationship.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptrRelationList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label RelationShip = (Label)e.Item.FindControl("RelationShip");
            DataList dataListCategories = (DataList)e.Item.FindControl("dsCategories");

            FeedbackProject_BAO projectBusinessAccessObject = new FeedbackProject_BAO();

            List<FeedbackProject_BE> listSurveyProjects = projectBusinessAccessObject.GetProjectByID(Convert.ToInt32(ddlAccountCode.SelectedValue),
                Convert.ToInt32(ddlProject.SelectedValue));

            FeedbackCategory_BAO categories = new FeedbackCategory_BAO();

            if (assignedCategoriesBusinessEntityList == null)
            {
                assignedCategoriesBusinessEntityList = new List<AssignedCategories_BE>();
                AssignedCategories_BAO assignedCategories = new AssignedCategories_BAO();
                AssignedCategories_BE CategoriesBusinessEntity = new AssignedCategories_BE();

                CategoriesBusinessEntity.QuestionnaireID = Convert.ToInt32(listSurveyProjects[0].QuestionnaireID.Value);
                CategoriesBusinessEntity.AccountID = Convert.ToInt32(listSurveyProjects[0].AccountID.Value);
                CategoriesBusinessEntity.ProjectID = Convert.ToInt32(listSurveyProjects[0].ProjectID.Value);

                AssignedCategories_BAO assignedCategoriesBusinessAccessObject = new AssignedCategories_BAO();
                assignedCategoriesBusinessEntityList = assignedCategoriesBusinessAccessObject.GetAssignCategory(CategoriesBusinessEntity);
            }

            //Bind category by account and questionnaire id .
            if (RelationShip.Text.Trim().Length > 0)
            {
                dataListCategories.DataSource = categories.SelectCategory(listSurveyProjects[0].AccountID.Value, listSurveyProjects[0].QuestionnaireID.Value);
                dataListCategories.ItemDataBound += new DataListItemEventHandler(dsCategories_ItemDataBound);
                dataListCategories.DataBind();
            }
        }
    }

    /// <summary>
    /// Save category relationship details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        int accountId = Convert.ToInt32(ddlAccountCode.SelectedValue);
        int projectId = Convert.ToInt32(ddlProject.SelectedValue);

        foreach (RepeaterItem item in rptrRelationList.Items)
        {
            Label RelationShip = (Label)item.FindControl("RelationShip");
            DataList dataListCategories = (DataList)item.FindControl("dsCategories");

            Boolean hasCategories = false;
            string CategoriesID = string.Empty;
            HiddenField hiddenQuestionnaireID = null;

            foreach (DataListItem listCategories in dataListCategories.Items)
            {

                CheckBox checkBoxCategory = (CheckBox)listCategories.FindControl("chkBoxCategory");
                HiddenField hiddenCategoryId = (HiddenField)listCategories.FindControl("hdCategoryId");
                hiddenQuestionnaireID = (HiddenField)listCategories.FindControl("QuestionnaireID");

                if (checkBoxCategory.Checked)
                {

                    CategoriesID = hiddenCategoryId.Value + "," + CategoriesID;

                    hasCategories = true;
                }
            }

            if (RelationShip.Text.Trim().Length > 0)
            {
                //if (hasCategories) {
                AssignedCategories_BAO assignedCategories = new AssignedCategories_BAO();
                AssignedCategories_BE assignedCategoriesBusinessEntity = new AssignedCategories_BE();

                assignedCategoriesBusinessEntity.QuestionnaireID = Convert.ToInt32(hiddenQuestionnaireID.Value);
                assignedCategoriesBusinessEntity.AccountID = accountId;
                assignedCategoriesBusinessEntity.ProjectID = projectId;
                assignedCategoriesBusinessEntity.Name = RelationShip.Text;
                //Save category relation ship details
                assignedCategories.AddAssignCategory(assignedCategoriesBusinessEntity, CategoriesID);
                // }
            }

            hasCategories = false;
            CategoriesID = string.Empty;
        }

        lblSuccessMessage.Text = "Changes has been successfully done.";
    }

    /// <summary>
    /// Bind category data list .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dsCategories_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label RelationShip = (Label)((RepeaterItem)((DataList)sender).Parent).FindControl("RelationShip");

            CheckBox checkBoxCategory = (CheckBox)e.Item.FindControl("chkBoxCategory");
            HiddenField hiddenCategoryId = (HiddenField)e.Item.FindControl("hdCategoryId");
            HiddenField hiddenQuestionnaireID = (HiddenField)e.Item.FindControl("QuestionnaireID");
            HiddenField hiddenRelationShip = (HiddenField)e.Item.FindControl("hdRelationShip");

            IEnumerable<AssignedCategories_BE> currentListItem = assignedCategoriesBusinessEntityList.Where(v =>
                v.AccountID == Convert.ToInt32(ddlAccountCode.SelectedValue)
                && v.ProjectID == Convert.ToInt32(ddlProject.SelectedValue)
                && v.QuestionnaireID == Convert.ToInt32(hiddenQuestionnaireID.Value)
                && v.CategoryID == Convert.ToInt32(hiddenCategoryId.Value) && v.Name == RelationShip.Text);

            // Mark check box as check or uncheck.
            if (currentListItem.Count() > 0)
                checkBoxCategory.Checked = true;
        }
    }

    /// <summary>
    /// Hide show controls on project selected index change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        tblAssignCategories.Visible = false;
        imbAssign.Visible = false;
    }
}
