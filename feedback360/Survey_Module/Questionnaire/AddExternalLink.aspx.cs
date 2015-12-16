using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Questionnaire_BAO;
using Admin_BAO;
using DatabaseAccessUtilities;

public partial class Survey_Module_Questionnaire_AddExternalLink : CodeBehindBase
{
    //Add External link.
    Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;
            //Fill account details
            fillAccountCode();
            //
            EditLink();
        }
        //fillAnalysis();
    }

    /// <summary>
    /// Bind controls
    /// </summary>
    protected void EditLink()
    {
        //If query string cotains id 
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
            //Get Exernal link details
            DataTable dtLink = objCommon_BAO.GetDataTable("Survey_UspExternalLink", lstcname);

            int AccountId = Convert.ToInt32(dtLink.Rows[0]["AccountID"]);
            int ProjecId = Convert.ToInt32(dtLink.Rows[0]["ProjectId"]);
            int CompanyId = Convert.ToInt32(dtLink.Rows[0]["CompanyId"]);
            int ProgrammeId = Convert.ToInt32(dtLink.Rows[0]["ProgrammeId"]);
            int EmailTemplateId = Convert.ToInt32(dtLink.Rows[0]["EmailTemplateId"]);

            if (ddlAccountCode.Items.FindByValue(AccountId.ToString()) != null)
            {
                ddlAccountCode.SelectedIndex = -1;
                ddlAccountCode.Items.FindByValue(AccountId.ToString()).Selected = true;//Select account dropdown

                fillProject(ddlAccountCode.SelectedValue);
                fillEmailTemplate(ddlAccountCode.SelectedValue);
                ddlProject.SelectedIndex = -1;

                if (ddlEmailTemplate.Items.FindByValue(EmailTemplateId.ToString()) != null)
                {
                    ddlEmailTemplate.SelectedIndex = -1;
                    ddlEmailTemplate.Items.FindByValue(EmailTemplateId.ToString()).Selected = true;//Select template dropdown
                }

                if (ddlProject.Items.FindByValue(ProjecId.ToString()) != null)
                {
                    ddlProject.SelectedIndex = -1;
                    ddlProject.Items.FindByValue(ProjecId.ToString()).Selected = true;//Select project dropdown
                    fillCompany();

                    if (ddlCompany.Items.FindByValue(CompanyId.ToString()) != null)
                    {
                        ddlCompany.SelectedIndex = -1;
                        ddlCompany.Items.FindByValue(CompanyId.ToString()).Selected = true;//Select company dropdown
                        fillProgramme();

                        if (ddlProgrammeName.Items.FindByValue(ProgrammeId.ToString()) != null)
                        {
                            ddlProgrammeName.SelectedIndex = -1;
                            ddlProgrammeName.Items.FindByValue(ProgrammeId.ToString()).Selected = true;//Select program dropdown
                        }
                    }
                }

                txtInstructions.Text = Convert.ToString(dtLink.Rows[0]["Instructions"]);
                txtExternalLink.Text = Convert.ToString(dtLink.Rows[0]["ExternalLink"]);

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

                //Check or uncheck send email after complition checkbox
                chkSendEmailAfterCompletion.Checked = Convert.ToBoolean(dtLink.Rows[0]["SendEmailOnCompletion"]) == true ? true : false;
                //Check or uncheck send Send Report Participant checkbox
                chkSendReportParticipant.Checked = Convert.ToBoolean(dtLink.Rows[0]["SendReportToParticipant"]) == true ? true : false;
            }
        }
    }

    /// <summary>
    /// Save controls value to database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

            //Set Properties value
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

            Common_BAO objCommonBusinessAccessObject = new Common_BAO();
            //Insert update database.
            objCommonBusinessAccessObject.InsertAndUpdate("Survey_UspExternalLink", lstcname);
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

    /// <summary>
    /// Redirect to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Bind project and tempalte dropdown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillProject(ddlAccountCode.SelectedValue);
        fillEmailTemplate(ddlAccountCode.SelectedValue);
    }

    /// <summary>
    /// Bind company dropdown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCompany();
    }

    /// <summary>
    /// Bind program dropdown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillProgramme();
    }

    /// <summary>
    /// It is of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgrammeName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// It's of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {

    }

    private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);

    }

    /// <summary>
    /// Bind company dropdown by dynamic query.
    /// </summary>
    private void fillCompany()
    {
        Survey_Company_BAO companyBusinessAccessObject = new Survey_Company_BAO();
        var dataTableCompany = companyBusinessAccessObject.GetdtCompanyList(GetCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dataTableCompany;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();
    }

    /// <summary>
    /// Bind account drop down and template dropdown
    /// </summary>
    private void fillAccountCode()
    {
        Account_BAO accountBusinessAccessObject = new Account_BAO();
        //Get all account list by user account id  Bind account drop down.
        ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
        ddlAccountCode.DataValueField = "AccountID";
        ddlAccountCode.DataTextField = "Code";
        ddlAccountCode.DataBind();
        ddlAccountCode.SelectedIndex = 0;
        //Fill template dropdownlist.
        fillEmailTemplate(ddlAccountCode.SelectedValue);
    }

    /// <summary>
    /// Bind project drop down by account id.
    /// </summary>
    private void fillProject(string accountId)
    {
        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();

        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(accountId);
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();
    }

    /// <summary>
    /// Bind program drop down by account id.
    /// </summary>
    private void fillProgramme()
    {
        Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();

        string accountId = GetConditionProgramme();

        ddlProgrammeName.Items.Clear();
        ddlProgrammeName.Items.Insert(0, new ListItem("Select", "0"));

        ddlProgrammeName.DataSource = programmeBusinessAccessObject.GetdtProgrammeListNew(accountId);
        ddlProgrammeName.DataValueField = "ProgrammeID";
        ddlProgrammeName.DataTextField = "ProgrammeName";
        ddlProgrammeName.DataBind();
    }

    /// <summary>
    /// Bind template drop down in an account
    /// </summary>
    private void fillEmailTemplate(string accountId)
    {
        Survey_EmailTemplate_BAO emailTemplateBusinessAccessObject = new Survey_EmailTemplate_BAO();
        DataTable dataTableEmailTemplate = emailTemplateBusinessAccessObject.GetdtEmailTemplateList(accountId);

        ddlEmailTemplate.DataSource = dataTableEmailTemplate;
        ddlEmailTemplate.DataValueField = "EmailTemplateID";
        ddlEmailTemplate.DataTextField = "Title";
        ddlEmailTemplate.DataBind();
    }

    /// <summary>
    /// Generate dynamic query.
    /// </summary>
    /// <returns></returns>
    public string GetConditionProgramme()
    {
        string stringQuery = "";

        if (int.Parse(ddlAccountCode.SelectedValue) > 0)
            stringQuery = stringQuery + "" + ddlAccountCode.SelectedValue + " and ";
        else
            stringQuery = stringQuery + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlProject.SelectedIndex > 0)
            stringQuery = stringQuery + "[Survey_Project].[ProjectID] = " + ddlProject.SelectedValue + " and ";

        if (ddlCompany.SelectedIndex > 0)
            stringQuery = stringQuery + "Survey_Analysis_Sheet.[CompanyID] = " + ddlCompany.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }

    /// <summary>
    /// Generate dynamic query.
    /// </summary>
    /// <returns></returns>
    public string GetCondition()
    {
        string stringQuery = "";

        //if (Convert.ToInt32(ViewState["AccountID"]) > 0)
        //    str = str + "" + ViewState["AccountID"] + " and ";
        //else
        //    str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (int.Parse(ddlAccountCode.SelectedValue) > 0)
            stringQuery = stringQuery + "" + ddlAccountCode.SelectedValue + " and ";

        if (ddlProject.SelectedIndex > 0)
            stringQuery = stringQuery + "Survey_Project.[ProjectID] = " + ddlProject.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }
}
