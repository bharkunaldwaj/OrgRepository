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

public partial class Module_Reports_RegenerateProgrammeDocument : System.Web.UI.Page
{
    Account_BAO account_BAO = new Account_BAO();
    Project_BAO project_BAO = new Project_BAO();
    Programme_BAO programme_BAO = new Programme_BAO();
    ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
    WADIdentity identity;

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
                "ProjectID", "Title");

            DropDownListProgramme.Items.Clear();
            DropDownListProgramme.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            Labelcompanyname.Text = string.Empty;

            DropDownListProject.Items.Clear();
            DropDownListProject.Items.Insert(0, new ListItem("Select", "0"));

            DropDownListProgramme.Items.Clear();
            DropDownListProgramme.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void DropDownListProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListProgramme.Items.Clear();

        if (int.Parse(DropDownListProject.SelectedValue) > 0)
        {
            BindDropDownList(DropDownListProgramme, programme_BAO.GetProjectProgramme(int.Parse(DropDownListProject.SelectedValue)),
           "ProgrammeID", "ProgrammeName");
        }
    }
    #endregion

    #region Private Methods
    private void RestartSchedular()
    {
        string filePath = @"E:\Damco Projects\OrgRef\trunk\Feedback360ReportScheduler\FeedbackReportScheduler\bin\Debug\FeedbackReportScheduler.exe";
        Process process = new Process();
        process.StartInfo.FileName = filePath;
        process.StartInfo.Arguments = DropDownListProgramme.SelectedValue;
        process.Start();
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
           "AccountID", "Code");
        DropDownListAccountCode.SelectedValue = identity.User.AccountID.ToString();

        BindDropDownList(DropDownListProject, project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID)),
            "ProjectID", "Title");
    }

    private void BindDropDownList(DropDownList dropDownControl, DataTable controlDataTable,
        string DataValueField, string DataTextField)
    {
        dropDownControl.Items.Clear();
        dropDownControl.Items.Insert(0, new ListItem("Select", "0"));
        dropDownControl.DataSource = controlDataTable;
        dropDownControl.DataValueField = DataValueField;
        dropDownControl.DataTextField = DataTextField;
        dropDownControl.DataBind();
    }
    #endregion
}