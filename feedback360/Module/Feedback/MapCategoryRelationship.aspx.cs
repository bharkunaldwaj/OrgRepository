using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin_BAO;
using Questionnaire_BAO;
using Questionnaire_BE;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;
using DAF_BAO;
using System.IO;
using System.Collections;
using System.Data;
using Administration_BE;

public partial class Survey_Module_Admin_MapCategoryRelationship : System.Web.UI.Page {

    WADIdentity identity;
    DataTable dtCompanyName;
    List<AssignedCategories_BE> assignedCategories_BEList = null;

    protected void Page_Load(object sender, EventArgs e) {
        try {
            identity = this.Page.User.Identity as WADIdentity;
            if (!IsPostBack) {
             

                FeedbackAccount_BAO account_BAO = new FeedbackAccount_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
            }
        }
        catch { }
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e) {
        // AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
        // ddlManager.Items.Clear();
        // ddlManager.Items.Insert(0, new ListItem("Select", "0"));
        ddlProject.Items.Clear();
        ddlProject.Items.Add(new ListItem("Select","0"));
        tblAssignCategories.Visible = false;

        imbAssign.Visible = false;
        lblSuccessMessage.Text = "";


        tblNoData.Visible = false;
        lblNoData.Text = "";

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0) {
            FeedbackAccount_BAO account_BAO = new FeedbackAccount_BAO();

            dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dtCompanyName.Clone();
            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            ViewState["AccountID"] = ddlAccountCode.SelectedValue;

            FeedbackProject_BAO project_BAO = new FeedbackProject_BAO();
            ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

         
        }
        else {
            lblcompanyname.Text = "";
            ddlProject.Items.Clear();
            ViewState["AccountID"] = "0";

        }
    }
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e) {
        FeedbackProject_BAO project_BAO = new FeedbackProject_BAO();
        try {
            List<FeedbackProject_BE> Survey_Projects = project_BAO.GetProjectByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue));


           



            if (Survey_Projects != null && Survey_Projects.Count() > 0) {


                DataTable outPutTable = new DataTable();
                outPutTable.Columns.Add(new DataColumn("PROJ_ID", typeof(System.Int32)));
                outPutTable.Columns.Add(new DataColumn("RelationShip", typeof(System.String)));

                DataRow dr1 = outPutTable.NewRow();
                dr1["PROJ_ID"] = ddlProject.SelectedValue;
                dr1["RelationShip"] = Survey_Projects[0].Relationship1;
                outPutTable.Rows.Add(dr1);

                DataRow dr2 = outPutTable.NewRow();
                dr2["PROJ_ID"] = ddlProject.SelectedValue;
                dr2["RelationShip"] = Survey_Projects[0].Relationship2;
                outPutTable.Rows.Add(dr2);

                DataRow dr3 = outPutTable.NewRow();
                dr3["PROJ_ID"] = ddlProject.SelectedValue;
                dr3["RelationShip"] = Survey_Projects[0].Relationship3;
                outPutTable.Rows.Add(dr3);

                DataRow dr4 = outPutTable.NewRow();
                dr4["PROJ_ID"] = ddlProject.SelectedValue;
                dr4["RelationShip"] = Survey_Projects[0].Relationship4;
                outPutTable.Rows.Add(dr4);

                DataRow dr5 = outPutTable.NewRow();
                dr5["PROJ_ID"] = ddlProject.SelectedValue;
                dr5["RelationShip"] = Survey_Projects[0].Relationship5;
                outPutTable.Rows.Add(dr5);

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
            else {
                tblAssignCategories.Visible = false;
                tblNoData.Visible = true;
                lblNoData.Text = "No Data";
            }
        }
        catch { tblAssignCategories.Visible = false;

        tblNoData.Visible = true;
        lblNoData.Text = "Some error has occured. Please contact your administrator.";
        }
    }
    protected void rptrRelationList_ItemDataBound(object sender, RepeaterItemEventArgs e) {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
            Label RelationShip = (Label)e.Item.FindControl("RelationShip");
            DataList dsCategories = (DataList)e.Item.FindControl("dsCategories");
            FeedbackProject_BAO project_BAO = new FeedbackProject_BAO();
            List<FeedbackProject_BE> Survey_Projects = project_BAO.GetProjectByID(Convert.ToInt32(ddlAccountCode.SelectedValue), Convert.ToInt32(ddlProject.SelectedValue));
            FeedbackCategory_BAO categories = new FeedbackCategory_BAO();
            
            if (assignedCategories_BEList == null) {
                assignedCategories_BEList = new List<AssignedCategories_BE>();
                AssignedCategories_BAO assignedCategories = new AssignedCategories_BAO();
                AssignedCategories_BE Categories_BE = new AssignedCategories_BE();

                Categories_BE.QuestionnaireID = Convert.ToInt32(Survey_Projects[0].QuestionnaireID.Value);
                Categories_BE.AccountID = Convert.ToInt32(Survey_Projects[0].AccountID.Value);
                Categories_BE.ProjectID = Convert.ToInt32(Survey_Projects[0].ProjectID.Value);

                AssignedCategories_BAO assignedCategories_BAO = new AssignedCategories_BAO();
                assignedCategories_BEList = assignedCategories_BAO.GetAssignCategory(Categories_BE);
            }

            if (RelationShip.Text.Trim().Length > 0) {
                dsCategories.DataSource = categories.SelectCategory(Survey_Projects[0].AccountID.Value, Survey_Projects[0].QuestionnaireID.Value);
                dsCategories.ItemDataBound += new DataListItemEventHandler(dsCategories_ItemDataBound);
                dsCategories.DataBind();
            }
        }
    }
    protected void imbAssign_Click(object sender, ImageClickEventArgs e) {

        int accountId = Convert.ToInt32(ddlAccountCode.SelectedValue);
        int projectId = Convert.ToInt32(ddlProject.SelectedValue);
        foreach (RepeaterItem item in rptrRelationList.Items) {
            Label RelationShip = (Label)item.FindControl("RelationShip");
            DataList dsCategories = (DataList)item.FindControl("dsCategories");
            

            Boolean hasCategories = false;
            string CategoriesID = string.Empty;
            HiddenField QuestionnaireID = null;
            foreach (DataListItem cats in dsCategories.Items) {

                CheckBox chkBoxCategory = (CheckBox)cats.FindControl("chkBoxCategory");
                HiddenField hdCategoryId = (HiddenField)cats.FindControl("hdCategoryId");
                QuestionnaireID = (HiddenField)cats.FindControl("QuestionnaireID");
                if (chkBoxCategory.Checked) {

                    CategoriesID = hdCategoryId.Value + "," + CategoriesID;
                    
                    hasCategories = true;
                }
            }
            if (RelationShip.Text.Trim().Length > 0) {
                //if (hasCategories) {
                AssignedCategories_BAO assignedCategories = new AssignedCategories_BAO();
                AssignedCategories_BE assignedCategories_BE = new AssignedCategories_BE();

                assignedCategories_BE.QuestionnaireID = Convert.ToInt32(QuestionnaireID.Value);
                assignedCategories_BE.AccountID = accountId;
                assignedCategories_BE.ProjectID = projectId;
                assignedCategories_BE.Name = RelationShip.Text;
                assignedCategories.AddAssignCategory(assignedCategories_BE, CategoriesID);
                // }
            }
            hasCategories = false;
            CategoriesID = string.Empty;
        }

        lblSuccessMessage.Text = "Changes has been successfully done.";

    }
    protected void dsCategories_ItemDataBound(object sender, DataListItemEventArgs e) {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
            Label RelationShip = (Label)((RepeaterItem)((DataList)sender).Parent).FindControl("RelationShip");
         
            CheckBox chkBoxCategory = (CheckBox)e.Item.FindControl("chkBoxCategory");
            HiddenField hdCategoryId = (HiddenField)e.Item.FindControl("hdCategoryId");
             HiddenField QuestionnaireID = (HiddenField)e.Item.FindControl("QuestionnaireID");
            HiddenField hdRelationShip = (HiddenField)e.Item.FindControl("hdRelationShip");
            
          IEnumerable<AssignedCategories_BE> currentListItem =  assignedCategories_BEList.Where(v => 
              v.AccountID==Convert.ToInt32(ddlAccountCode.SelectedValue) 
              && v.ProjectID == Convert.ToInt32(ddlProject.SelectedValue) 
              && v.QuestionnaireID == Convert.ToInt32( QuestionnaireID.Value)
              && v.CategoryID == Convert.ToInt32(hdCategoryId.Value) && v.Name == RelationShip.Text);

          if (currentListItem.Count() > 0)
              chkBoxCategory.Checked = true;

        }
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e) {
        tblAssignCategories.Visible = false;
        imbAssign.Visible = false;
                
    }
}
