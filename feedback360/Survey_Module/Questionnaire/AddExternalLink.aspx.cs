using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.IO;
using System.Collections;
using Admin_BAO;
using DatabaseAccessUtilities;

public partial class Survey_Module_Questionnaire_AddExternalLink : CodeBehindBase
{

    Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;
            fillAccountCode();
            EditLink();
        }
        //fillAnalysis();
    }

    protected void EditLink()
    {
        if (Request.QueryString["Id"] != null)
        {

            ddlAccountCode.Enabled = false;
            ddlProject.Enabled = false;
            ddlCompany.Enabled = false;
            ddlProgrammeName.Enabled = false;
            


            CNameValueList lstcname = new CNameValueList();

            lstcname.Add("@Operation", "GETLINK");

            lstcname.Add("@UniqueID", new Guid(Request.QueryString["Id"].ToString()));

            Common_BAO objCommon_BAO = new Common_BAO();
            DataTable dtLink = objCommon_BAO.GetDataTable("Survey_UspExternalLink", lstcname);

            int AccountId = Convert.ToInt32(dtLink.Rows[0]["AccountID"]);
            int ProjecId = Convert.ToInt32(dtLink.Rows[0]["ProjectId"]);
            int CompanyId = Convert.ToInt32(dtLink.Rows[0]["CompanyId"]);
            int ProgrammeId = Convert.ToInt32(dtLink.Rows[0]["ProgrammeId"]);
            int EmailTemplateId = Convert.ToInt32(dtLink.Rows[0]["EmailTemplateId"]);

            if (ddlAccountCode.Items.FindByValue(AccountId.ToString())!=null)
            {
                ddlAccountCode.SelectedIndex = -1;
                ddlAccountCode.Items.FindByValue(AccountId.ToString()).Selected = true;

                fillProject(ddlAccountCode.SelectedValue);
                fillEmailTemplate(ddlAccountCode.SelectedValue);
                ddlProject.SelectedIndex = -1;
                if (ddlEmailTemplate.Items.FindByValue(EmailTemplateId.ToString()) != null)
                {
                    ddlEmailTemplate.SelectedIndex = -1;
                    ddlEmailTemplate.Items.FindByValue(EmailTemplateId.ToString()).Selected = true;
                }


                if (ddlProject.Items.FindByValue(ProjecId.ToString()) != null)
                {
                    ddlProject.SelectedIndex = -1;
                    ddlProject.Items.FindByValue(ProjecId.ToString()).Selected = true;
                    fillCompany();

                    if (ddlCompany.Items.FindByValue(CompanyId.ToString()) != null)
                    {
                        ddlCompany.SelectedIndex = -1;
                        ddlCompany.Items.FindByValue(CompanyId.ToString()).Selected = true;
                        fillProgramme();

                        if (ddlProgrammeName.Items.FindByValue(ProgrammeId.ToString()) != null)
                        {
                            ddlProgrammeName.SelectedIndex = -1;
                            ddlProgrammeName.Items.FindByValue(ProgrammeId.ToString()).Selected = true;
                        }
                    }

                }

                txtInstructions.Text = Convert.ToString(dtLink.Rows[0]["Instructions"]);
                txtExternalLink.Text =  Convert.ToString(dtLink.Rows[0]["ExternalLink"]);

                string EmailTo = Convert.ToString(dtLink.Rows[0]["EmailTo"]);

                if (ddlEmailTo.Items.FindByValue(EmailTo.ToString()) != null)
                {
                    ddlEmailTo.SelectedIndex = -1;
                    ddlEmailTo.Items.FindByValue(EmailTo.ToString()).Selected = true;
                    if (EmailTo == "Email")
                    {
                        txtCustomEmail.Visible = true;
                        txtCustomEmail.Text = Convert.ToString(dtLink.Rows[0]["CustomEmail"]);
                        lblCustomEmail.Attributes.Remove("style");
                        txtCustomEmail.Attributes.Remove("style");
                    }
                }

                chkSendEmailAfterCompletion.Checked = Convert.ToBoolean(dtLink.Rows[0]["SendEmailOnCompletion"]) == true ? true : false;
                chkSendReportParticipant.Checked = Convert.ToBoolean(dtLink.Rows[0]["SendReportToParticipant"]) == true ? true : false;
                
            }

        }
    }

    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Guid uniqueId = new Guid();
            uniqueId = Guid.NewGuid();
            CNameValueList lstcname = new CNameValueList();
           
            if (Request.QueryString["Id"] == null)
            {
                lstcname.Add("@Operation", "ADDEXLINK");
                lstcname.Add("@UniqueID", uniqueId);
                txtExternalLink.Text = Convert.ToString(ConfigurationManager.AppSettings["ExternalLinkURL"]) + "/Survey_Module/Register.aspx?LinkId=" + uniqueId;
            }
            else
            {
                lstcname.Add("@Operation", "UPDATEEXLINK");
                lstcname.Add("@UniqueID", new Guid(Request.QueryString["Id"].ToString()));
                txtExternalLink.Text = Convert.ToString(ConfigurationManager.AppSettings["ExternalLinkURL"]) + "/Survey_Module/Register.aspx?LinkId=" + Request.QueryString["Id"].ToString();
            }
            lstcname.Add("@AccountID", Convert.ToInt32(ddlAccountCode.SelectedValue));
            lstcname.Add("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue));
            lstcname.Add("@ProgrammeID", Convert.ToInt32(ddlProgrammeName.SelectedValue));
            lstcname.Add("@ProjectID", Convert.ToInt32(ddlProject.SelectedValue));
            lstcname.Add("@CreatedBy", "");
            lstcname.Add("@EmailTemplateId", Convert.ToInt32(ddlEmailTemplate.SelectedValue));
            lstcname.Add("@EmailTo", ddlEmailTo.SelectedValue);
            lstcname.Add("@ExternalLink", txtExternalLink.Text);
            lstcname.Add("@CustomEmail", txtCustomEmail.Text);
            lstcname.Add("@IsActive", true);
            
            lstcname.Add("@SendEmailOnCompletion", chkSendEmailAfterCompletion.Checked ? true : false);
            lstcname.Add("@Instructions", txtInstructions.Text);
            lstcname.Add("@SendReportToParticipant", chkSendReportParticipant.Checked ? true : false);
            
            Common_BAO objCommon_BAO = new Common_BAO();
            objCommon_BAO.InsertAndUpdate("Survey_UspExternalLink", lstcname);
            //if (chkSendEmailAfterCompletion.Checked)
            //{ 
            if (Request.QueryString["Id"] == null)
            {
                lblMessage.Text = "Link generated successfully.";
            }
            else
            {
                lblMessage.Text = "Link upddated successfully.";
            }
            lblMessage.Visible = true;
            //}
            //Response.Redirect("~/Survey_Default.aspx", false);

        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }


    protected void imbcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("ExternalLinkList.aspx", false);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillProject(ddlAccountCode.SelectedValue);
        fillEmailTemplate(ddlAccountCode.SelectedValue);
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCompany();
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillProgramme();
    }

    protected void ddlProgrammeName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }



    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {

    }

    private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);

    }

    private void fillCompany()
    {
        Survey_Company_BAO company_BAO = new Survey_Company_BAO();
        var dt = company_BAO.GetdtCompanyList(GetCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dt;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();
    }


    private void fillAccountCode()
    {
        Account_BAO account_BAO = new Account_BAO();
        ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
        ddlAccountCode.DataValueField = "AccountID";
        ddlAccountCode.DataTextField = "Code";
        ddlAccountCode.DataBind();
        ddlAccountCode.SelectedIndex = 0;
        fillEmailTemplate(ddlAccountCode.SelectedValue);

    }

    private void fillProject(string accountId)
    {
        Survey_Project_BAO project_BAO = new Survey_Project_BAO();

        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        ddlProject.DataSource = project_BAO.GetdtProjectList(accountId);
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

    }


    private void fillProgramme()
    {
        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();

        string accountId = GetConditionProgramme();

        ddlProgrammeName.Items.Clear();
        ddlProgrammeName.Items.Insert(0, new ListItem("Select", "0"));

        ddlProgrammeName.DataSource = programme_BAO.GetdtProgrammeListNew(accountId);
        ddlProgrammeName.DataValueField = "ProgrammeID";
        ddlProgrammeName.DataTextField = "ProgrammeName";
        ddlProgrammeName.DataBind();
    }

    private void fillEmailTemplate(string accountId)
    {
        Survey_EmailTemplate_BAO emailTemplate_BAO = new Survey_EmailTemplate_BAO();
        DataTable dtEmailTemplate = emailTemplate_BAO.GetdtEmailTemplateList(accountId);

        ddlEmailTemplate.DataSource = dtEmailTemplate;
        ddlEmailTemplate.DataValueField = "EmailTemplateID";
        ddlEmailTemplate.DataTextField = "Title";
        ddlEmailTemplate.DataBind();

    }

    public string GetConditionProgramme()
    {
        string str = "";

        if (ddlAccountCode.SelectedIndex > 0)
            str = str + "" + ddlAccountCode.SelectedValue + " and ";
        else
            str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlProject.SelectedIndex > 0)
            str = str + "[Survey_Project].[ProjectID] = " + ddlProject.SelectedValue + " and ";

        if (ddlCompany.SelectedIndex > 0)
            str = str + "Survey_Analysis_Sheet.[CompanyID] = " + ddlCompany.SelectedValue + " and ";

        string param = str.Substring(0, str.Length - 4);

        return param;
    }

    public string GetCondition()
    {
        string str = "";

        //if (Convert.ToInt32(ViewState["AccountID"]) > 0)
        //    str = str + "" + ViewState["AccountID"] + " and ";
        //else
        //    str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlAccountCode.SelectedIndex > 0)
            str = str + "" + ddlAccountCode.SelectedValue + " and ";

        if (ddlProject.SelectedIndex > 0)
            str = str + "Survey_Project.[ProjectID] = " + ddlProject.SelectedValue + " and ";

        string param = str.Substring(0, str.Length - 4);

        return param;
    }
}
