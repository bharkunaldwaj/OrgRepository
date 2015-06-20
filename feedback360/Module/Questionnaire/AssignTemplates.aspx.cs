using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Admin_BAO;
using System.Diagnostics;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.Data;


public partial class Module_Questionnaire_AssignTemplates : System.Web.UI.Page
{

    Project_BAO Proj = new Project_BAO();
    DataTable dsProj = new DataTable();

    EmailTemplate_BAO mail = new EmailTemplate_BAO();
    DataTable dsmail = new DataTable();
    string email;
    string finalemail;
    WADIdentity identity;
    string finalprj;
    string Projectid;
    DataTable CompanyName;
    DataTable dtAllAccount;
    string expression1;
    string Finalexpression;

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        // grdvProjects.RowDataBound += new GridViewRowEventHandler(grdvProjects_RowDataBound);
        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;

            dsProj = Proj.GetAdminProjectList(identity.User.AccountID.ToString());
            ViewState["Project"] = dsProj;

            dsmail = mail.GetAdminEmailTemplate(identity.User.AccountID.ToString());

            filldata();

            Account_BAO account_BAO = new Account_BAO();
            ddlAccountCode.DataSource = account_BAO.GetdtAccountList(identity.User.AccountID.ToString());
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

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

    protected void filldata()
    {
        grdvProjects.DataSource = dsProj;
        grdvProjects.DataBind();

        grdvmail.DataSource = dsmail;
        grdvmail.DataBind();
    }
    protected void ibtnAssign_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridViewRow row in grdvmail.Rows)
        {
            CheckBox chkIschecked = (CheckBox)row.FindControl("chkmail");
            if (chkIschecked.Checked == true)
            {
                DataTable dtemailTemplate = new DataTable();

                Label lblTitle = (Label)row.FindControl("Lblfmail");
                Label lblID = (Label)row.FindControl("lblfID");

                int ID = Convert.ToInt32(lblID.Text);

                //addauthority.UsrAutTyp = lblTitle.Text;

                email += ID + ",";
               
                
            }
        }

        foreach (GridViewRow Prjrow in grdvProjects.Rows)
        {
            CheckBox chkIsPrjchecked = (CheckBox)Prjrow.FindControl("chkProject");
            if (chkIsPrjchecked.Checked == true)
            {
                DataTable dtprjTemplate = new DataTable();

                Label lblTitle = (Label)Prjrow.FindControl("Lblfdesc");
                Label lblpID = (Label)Prjrow.FindControl("lblpID");

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

            foreach (int index in ddlAccountCode.GetSelectedIndices())
            {
                int Account = Convert.ToInt32(ddlAccountCode.Items[index].Value);
                finalprj = Projectid.TrimEnd(',');

                Proj.InsertProjID(finalprj, Account);
            }
            
            
           
        }
        if (email != null || Projectid != null)
        {
            lblMessage.Text = "Templetes Assigned Successfully";


            foreach (GridViewRow Prjrow in grdvProjects.Rows)
            {
                CheckBox chkIsPrjchecked = (CheckBox)Prjrow.FindControl("chkProject");
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
        
        foreach (GridViewRow Prjrow in grdvProjects.Rows)
        {
            CheckBox chkIsPrjchecked = (CheckBox)Prjrow.FindControl("chkProject");
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
        lblMessage.Text = "";

    }

}
