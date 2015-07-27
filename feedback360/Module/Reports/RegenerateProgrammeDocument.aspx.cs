using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin_BAO;
using Questionnaire_BAO;
using System.Data;
using System.Diagnostics;
using System.Configuration;

public partial class Module_Reports_RegenerateProgrammeDocument : System.Web.UI.Page
{
    #region Private Constant
    const string ProjectTextField = "Title";
    const string ProjectValueField = "ProjectID";
    const string ProgramTextField = "ProgrammeName";
    const string ProgramValueField = "ProgrammeID";
    const string AccountTextField = "Code";
    const string AccountValueField = "AccountID";
    const string DefaulText = "Select";
    const string DefaulValue = "0";
    const string FeedBackSchedularPath = "FeedBackSchedularPath";
    #endregion

    #region Global Variable
    string SchedularPath = ConfigurationManager.AppSettings[FeedBackSchedularPath];
    Account_BAO account_BAO = new Account_BAO();
    Project_BAO project_BAO = new Project_BAO();
    Programme_BAO programme_BAO = new Programme_BAO();
    ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
    WADIdentity identity;
    #endregion

    #region Protected Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindControls();
            GetCompanyName();
        }
    }

    protected void ButtonGenerateReport_Click(object sender, ImageClickEventArgs e)
    {
        reportManagement_BAO.DeleteDynamicReport(int.Parse(DropDownListAccountCode.SelectedValue),
             int.Parse(DropDownListProgramme.SelectedValue));

        RestartSchedular();
    }

    protected void DropDownListAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (int.Parse(DropDownListAccountCode.SelectedValue) > 0)
        {
            GetCompanyName();

            BindDropDownList(DropDownListProject, project_BAO.GetdtProjectList(DropDownListAccountCode.SelectedValue),
                ProjectValueField, ProjectTextField);

            ClearControl(DropDownListProgramme);
        }
        else
        {
            Labelcompanyname.Text = string.Empty;

            ClearControl(DropDownListProject);
            ClearControl(DropDownListProgramme);
        }
    }

    protected void DropDownListProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListProgramme.Items.Clear();

        if (int.Parse(DropDownListProject.SelectedValue) > 0)
        {
            BindDropDownList(DropDownListProgramme, programme_BAO.GetProjectProgramme(int.Parse(DropDownListProject.SelectedValue)),
           ProgramValueField, ProgramTextField);
        }
    }
    #endregion

    #region Private Methods
    private void RestartSchedular()
    {
        string filePath = string.Format(@"{0}", SchedularPath);
        /*Process process = new Process();
        process.StartInfo.FileName = filePath;
        process.StartInfo.Arguments = DropDownListProgramme.SelectedValue;
        process.Start();
         */

        ProcessStartInfo startInfo = new ProcessStartInfo(filePath);
        startInfo.Arguments = DropDownListProgramme.SelectedValue;
        Process.Start(startInfo);
    }

    private void GetCompanyName()
    {
        int companycode = int.Parse(DropDownListAccountCode.SelectedValue);

        DataTable DataTableCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

        var companyName = (DataTableCompanyName.AsEnumerable()).
            Where(x => x.Field<int>("AccountID") == companycode).FirstOrDefault();

        Labelcompanyname.Text = companyName.Field<string>("Organisationname");
    }

    private void BindControls()
    {
        identity = this.Page.User.Identity as WADIdentity;

        BindDropDownList(DropDownListAccountCode, account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID)),
           AccountValueField, AccountTextField);
        DropDownListAccountCode.SelectedValue = identity.User.AccountID.ToString();

        BindDropDownList(DropDownListProject, project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID)),
            ProjectValueField, ProjectTextField);
    }

    private void BindDropDownList(DropDownList dropDownControl, DataTable controlDataTable,
        string DataValueField, string DataTextField)
    {
        dropDownControl.Items.Clear();
        dropDownControl.Items.Insert(0, new ListItem(DefaulText, DefaulValue));

        dropDownControl.DataSource = controlDataTable;
        dropDownControl.DataValueField = DataValueField;
        dropDownControl.DataTextField = DataTextField;
        dropDownControl.DataBind();
    }

    private void ClearControl(DropDownList dropDownListControl)
    {
        dropDownListControl.Items.Clear();
        dropDownListControl.Items.Insert(0, new ListItem(DefaulText, DefaulValue));
    }
    #endregion
}