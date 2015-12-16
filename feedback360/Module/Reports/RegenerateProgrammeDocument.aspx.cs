using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin_BAO;
using Questionnaire_BAO;
using System.Data;
using System.Diagnostics;
using System.Configuration;

public partial class Module_Reports_RegenerateProgrammeDocument : Page
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
    //Global variables
    string SchedularPath = ConfigurationManager.AppSettings[FeedBackSchedularPath];
    Account_BAO accountBusinessAccessObject = new Account_BAO();
    Project_BAO projectBusinessAccessObject = new Project_BAO();
    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
    ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
    WADIdentity identity;
    #endregion

    #region Protected Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Bind dropdownlist control.
            BindControls();
            //Set companyname according to account.
            GetCompanyName();
        }
    }

    /// <summary>
    /// Regenerate Report document.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonGenerateReport_Click(object sender, ImageClickEventArgs e)
    {
        //Delete previous record.
        reportManagementBusinessAccessObject.DeleteDynamicReport(int.Parse(DropDownListAccountCode.SelectedValue),
             int.Parse(DropDownListProgramme.SelectedValue));

        //Restart schedular to regenerate Report document.
        RestartSchedular();

        LabelMessge.Text = "Please wait, this process may take several minutes, depending on the number of files to be regenerated.";
        ButtonGenerateReport.Enabled = false;
    }

    /// <summary>
    /// Bind project by account selected value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DropDownListAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (int.Parse(DropDownListAccountCode.SelectedValue) > 0)
        {
            //Bind company name by account.
            GetCompanyName();
            //By project dropdown by account.
            BindDropDownList(DropDownListProject, projectBusinessAccessObject.GetdtProjectList(DropDownListAccountCode.SelectedValue),
                ProjectValueField, ProjectTextField);
            //Reset control.
            ClearControl(DropDownListProgramme);
        }
        else
        {
            Labelcompanyname.Text = string.Empty;
            //Reset controls.
            ClearControl(DropDownListProject);
            ClearControl(DropDownListProgramme);
        }
    }

    /// <summary>
    ///  Bind Programme by Project selected value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DropDownListProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListProgramme.Items.Clear();

        if (int.Parse(DropDownListProject.SelectedValue) > 0)
        {
           // Bind Programme dropdown.
            BindDropDownList(DropDownListProgramme, programmeBusinessAccessObject.GetProjectProgramme(int.Parse(DropDownListProject.SelectedValue)),
           ProgramValueField, ProgramTextField);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Restart Schedular.
    /// </summary>
    private void RestartSchedular()
    {
        string filePath = string.Format(@"{0}", SchedularPath);//Set schedular path.

        ProcessStartInfo startInfo = new ProcessStartInfo(filePath);//set path to process.
        startInfo.Arguments = DropDownListProgramme.SelectedValue; //set the schedulat argument to program id.
        Process.Start(startInfo);//start service
    }

    /// <summary>
    /// Bind company name by account id.
    /// </summary>
    private void GetCompanyName()
    {
        int companycode = int.Parse(DropDownListAccountCode.SelectedValue);
        //Get company details by account id.
        DataTable DataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

        var companyName = (DataTableCompanyName.AsEnumerable()).
            Where(x => x.Field<int>("AccountID") == companycode).FirstOrDefault();
        //Set Company Name
        Labelcompanyname.Text = companyName.Field<string>("Organisationname");
    }

    /// <summary>
    /// Bind dropdown controls.
    /// </summary>
    private void BindControls()
    {
        identity = this.Page.User.Identity as WADIdentity;
        //Bind account drop down.
        BindDropDownList(DropDownListAccountCode, accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID)),
           AccountValueField, AccountTextField);
        DropDownListAccountCode.SelectedValue = identity.User.AccountID.ToString();
        //Bind project drop down.
        BindDropDownList(DropDownListProject, projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID)),
            ProjectValueField, ProjectTextField);
    }

    /// <summary>
    /// Common methods to bind dropdown.
    /// </summary>
    /// <param name="dropDownControl"></param>
    /// <param name="controlDataTable"></param>
    /// <param name="DataValueField"></param>
    /// <param name="DataTextField"></param>
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

    /// <summary>
    /// Clear control.
    /// </summary>
    /// <param name="dropDownListControl"></param>
    private void ClearControl(DropDownList dropDownListControl)
    {
        dropDownListControl.Items.Clear();
        dropDownListControl.Items.Insert(0, new ListItem(DefaulText, DefaulValue));
    }
    #endregion
}