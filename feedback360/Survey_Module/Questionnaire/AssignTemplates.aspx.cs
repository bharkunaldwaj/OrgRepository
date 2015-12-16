using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin_BAO;
using Questionnaire_BAO;
using System.Data;


public partial class Module_Questionnaire_AssignTemplates : System.Web.UI.Page
{
    //Global variable.
    Survey_Project_BAO ProjectBusinessObject = new Survey_Project_BAO();
    DataTable dataTableProject = new DataTable();

    Survey_EmailTemplate_BAO mail = new Survey_EmailTemplate_BAO();
    DataTable dataTableMail = new DataTable();
    string email;
    string finalemail;
    WADIdentity identity;
    string finalProject;
    string Projectid;
    DataTable CompanyName;
    DataTable dataTableAllAccount;
    string expression1;
    string Finalexpression;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        // grdvProjects.RowDataBound += new GridViewRowEventHandler(grdvProjects_RowDataBound);
        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;

            dataTableProject = ProjectBusinessObject.GetAdminProjectList(identity.User.AccountID.ToString());
            ViewState["Project"] = dataTableProject;
            //Get User Tempalte List  by user account id.
            dataTableMail = mail.GetAdminEmailTemplate(identity.User.AccountID.ToString());

            filldata();

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get User Project list by user account id and bind account drop down list.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(identity.User.AccountID.ToString());
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();
            //If user is super Admin then show acount div section else hide account div section.
            if (identity.User.GroupID == 1)
            {
                divAccount.Visible = true;
            }
            else
            {
                divAccount.Visible = false;
            }
            //divAccount.Visible = false;
        }
    }

    /// <summary>
    /// Bind project and Tempalte gridview.
    /// </summary>
    protected void filldata()
    {
        //Bind project grid.
        grdvProjects.DataSource = dataTableProject;
        grdvProjects.DataBind();
        //Bind template  grid.
        grdvmail.DataSource = dataTableMail;
        grdvmail.DataBind();
    }

    /// <summary>
    /// Assign template to paticular project .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnAssign_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridViewRow row in grdvmail.Rows)
        {
            CheckBox checkBoxMail = (CheckBox)row.FindControl("chkmail");
            //IF template value is chacked then assigned it to selected project.
            if (checkBoxMail.Checked == true)
            {
                DataTable dtemailTemplate = new DataTable();

                Label labelTitle = (Label)row.FindControl("Lblfmail");
                Label labelID = (Label)row.FindControl("lblfID");

                int ID = Convert.ToInt32(labelID.Text);

                //addauthority.UsrAutTyp = lblTitle.Text;
                email += ID + ",";
            }
        }

        foreach (GridViewRow ProjectRow in grdvProjects.Rows)
        {
            CheckBox checkBoxProjet = (CheckBox)ProjectRow.FindControl("chkProject");

            if (checkBoxProjet.Checked == true)
            {
                DataTable dataTableProjectTemplate = new DataTable();

                Label lblTitle = (Label)ProjectRow.FindControl("Lblfdesc");
                Label lblpID = (Label)ProjectRow.FindControl("lblpID");

                int PID = Convert.ToInt32(lblpID.Text);

                //addauthority.UsrAutTyp = lblTitle.Text;

                Projectid += PID + ",";
            }
        }

        if (email != null)
        {
            foreach (int index in ddlAccountCode.GetSelectedIndices())
            {
                int Account = Convert.ToInt32(ddlAccountCode.Items[index].Value);
                finalemail = email.TrimEnd(',');
                mail.InsertMailTemplateID(finalemail, Account);
            }
        }

        if (Projectid != null)
        {
            //Insert Tempalte details with Project.
            foreach (int index in ddlAccountCode.GetSelectedIndices())
            {
                int Account = Convert.ToInt32(ddlAccountCode.Items[index].Value);
                finalProject = Projectid.TrimEnd(',');

                ProjectBusinessObject.InsertProjID(finalProject, Account);
            }
        }

        if (email != null || Projectid != null)
        {
            lblMessage.Text = "Templetes Assigned Successfully";

            foreach (GridViewRow ProjectRow in grdvProjects.Rows)
            {
                CheckBox chkIsPrjchecked = (CheckBox)ProjectRow.FindControl("chkProject");
                if (chkIsPrjchecked.Checked == true)
                {
                    chkIsPrjchecked.Checked = false;
                }
            }

            foreach (GridViewRow row in grdvmail.Rows)
            {
                CheckBox chkIschecked = (CheckBox)row.FindControl("chkmail");
                if (chkIschecked.Checked == true)
                {
                    chkIschecked.Checked = false;
                }
            }
        }
    }

#if Commentout
    //protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    lblMessage.Text = "";

    //    if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
    //    {

    //        int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

    //        Account_BAO account1_BAO = new Account_BAO();

    //        CompanyName = account1_BAO.GetdtAccountList(Convert.ToString(companycode));

    //        expression1 = "AccountID='" + companycode + "'";

    //        Finalexpression = expression1;

    //        DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

    //        DataTable dtAccount = CompanyName.Clone();

    //        foreach (DataRow drAccount in resultsAccount)
    //        {
    //            dtAccount.ImportRow(drAccount);
    //        }

    //        lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
    //    }
    //    else
    //    {
    //        lblcompanyname.Text = "";
    //    }
    //}
#endif

    /// <summary>
    /// Reset Controls value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {

        //HandleWriteLog("Start", new StackTrace(true));
        //identity = this.Page.User.Identity as WADIdentity;
        //dsProj = Proj.GetdtProjectList(identity.User.AccountID.ToString());
        //dsmail = mail.GetdtEmailTemplateList(identity.User.AccountID.ToString());
        //filldata();
        //lblcompanyname.Text = "";
        //HandleWriteLog("Start", new StackTrace(true));

        ddlAccountCode.SelectedIndex = 0;
        //Loop all the project check box and deselect.
        foreach (GridViewRow ProjectRow in grdvProjects.Rows)
        {
            CheckBox chkIsPrjchecked = (CheckBox)ProjectRow.FindControl("chkProject");

            if (chkIsPrjchecked.Checked == true)
            {
                chkIsPrjchecked.Checked = false;
            }
        }

        //Loop all the tempalte check box and deselect.
        foreach (GridViewRow row in grdvmail.Rows)
        {
            CheckBox chkIschecked = (CheckBox)row.FindControl("chkmail");
            if (chkIschecked.Checked == true)
            {
                chkIschecked.Checked = false;
            }
        }

        lblMessage.Text = "";
    }
}
