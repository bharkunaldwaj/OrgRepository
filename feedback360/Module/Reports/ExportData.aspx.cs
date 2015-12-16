using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Questionnaire_BAO;
using Questionnaire_BE;
using Admin_BAO;

public partial class Module_Reports_ExportData : CodeBehindBase
{

    #region Globalvariable
    //string LogFilePath = string.Empty;
    //string mimeType;
    //string encoding;
    //string fileNameExtension;
    //string extension, deviceInfo, outputFileName = "";
    //string[] streams;
    //string defaultFileName = string.Empty;
    Warning[] warnings;
    WADIdentity identity;
    Project_BAO projectBusinessAccessObject = new Project_BAO();
    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
    AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
    AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
    ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
    ReportManagement_BE reportManagementBusinessEntity = new ReportManagement_BE();


    DataTable dtCompanyName;
    //DataTable dtGroupList;
    //DataTable dtSelfName;
    //DataTable dtReportsID;
    //string strGroupList;
    //string strFrontPage;
    //string strConclusionPage;
    //string strRadarChart;
    //string strDetailedQst;
    //string strCategoryQstlist;
    //string strCategoryBarChart;
    //string strFullProjGrp;
    //string strSelfNameGrp;
    string strReportName;

    //string strTargetPersonID;
    //string strProjectID;
    //string strAccountID;
    //string strProgrammeID;
    //string strAdmin;

    Category_BAO categoryBusinessAccessObject = new Category_BAO();
    Category_BE categoryBusinessEntity = new Category_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int reportCount = 0;
    string pageNo = "";
    //string participantName;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            //if (identity.User.AccountID.ToString() == "2")
            //    ViewState["strAdmin"] = "Y";
            //else
            //    ViewState["strAdmin"] = "N";

            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;
                //If user is super admin then show account drop down else hide
                if (identity.User.GroupID == 1)
                {
                    divAccount.Visible = true;
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else
                {
                    divAccount.Visible = false;
                }

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //bind account drop down by user account id.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                ddlAccountCode.SelectedValue = "0";

                Project_BAO projectBusinessAccessObject = new Project_BAO();
                //Bind project in user account id.
                ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region ReportMethods

    /// <summary>
    /// Export date by categoy and questions.
    /// </summary>
    protected void btnExport()
    {
        Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();
        rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
        string[] streamids;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string root = string.Empty;

        //set Document path
        root = Server.MapPath("~") + "\\ReportGenerate\\";
        string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();

        //To export by category
        if (ddlExportType.SelectedValue == "C")
        {
            rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportbyCategory";
            strReportName = "FeedbackReportbyCategory" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss");
        }
        else if (ddlExportType.SelectedValue == "Q") //To export by question
        {
            rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportbyQuestion";
            strReportName = "FeedbackReportbyQuestion" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss");
        }

        System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
        paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("AccountID", Convert.ToString(ddlAccountCode.SelectedValue)));
        paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectID", Convert.ToString(ddlProject.SelectedValue)));
        paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeID", Convert.ToString(ddlProgramme.SelectedValue)));
        rview.ServerReport.SetParameters(paramList);

        rview.Visible = false;
        //byte[] bytes = rview.ServerReport.Render("excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        //string PDF_path = root + strReportName + ".xls";
        //FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create);
        //objFs.Write(bytes, 0, bytes.Length);
        //objFs.Close();
        //objFs.Dispose();

        string mimeType;
        string encoding;
        string extension;
        byte[] bytes = rview.ServerReport.Render("EXCEL", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        //download document
        Response.Clear();
        Response.AppendHeader("Content-Type", "application/octet-stream");
        Response.AppendHeader("Content-disposition", "attachment; filename=DataByCategory.xls");
        Response.BinaryWrite(bytes);
        Response.End();
    }

    //protected void OpenReport()
    //{
    //    try
    //    {
    //        string root = Server.MapPath("~") + "\\ReportGenerate\\";

    //        string openpdf = root + Convert.ToString(ViewState["strReportName"]) + ".pdf";
    //        Response.Write(openpdf);
    //        Response.ClearContent();
    //        Response.ClearHeaders();
    //        Response.AddHeader("Content-Disposition", "attachment; filename=" + openpdf);
    //        Response.ContentType = "application/octet-stream";
    //        Response.WriteFile(openpdf);

    //        Response.Flush();
    //        Response.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        //ErrorHandlerBL errorBL = new ErrorHandlerBL();
    //        //errorBL.WriteErrorLog(Server.MapPath("~") + "//Error.log", ex);
    //    }

    //}

    #endregion

    /// <summary>
    /// Bind project in an account
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetControls();

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            //Set account id
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account details
            dtCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dtCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //set company name
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();


            if (ddlAccountCode.SelectedIndex > 0)
            {//get project list by comany code and bind project dropdown
                DataTable dtprojectlist = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

                if (dtprojectlist.Rows.Count > 0)
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));

                    ddlProject.DataSource = dtprojectlist;
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataBind();

                    ddlProgramme.SelectedValue = "0";
                }
                else
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            //FillGridData();
            ViewState["accid"] = ddlAccountCode.SelectedValue.ToString();
        }
    }

    /// <summary>
    /// Bind all program in a project
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExportType.SelectedValue = "0";
        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //get all program in a project and bind program dropdown
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {// bind program droopdown
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlProgramme.Items.Count > 1)
            ddlProgramme.Items[1].Selected = true;


        ViewState["prjid"] = ddlProject.SelectedValue.ToString();
        //ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }

    /// <summary>
    /// Export  category and question data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbExport_Click(object sender, ImageClickEventArgs e)
    {
        btnExport();
    }

    /// <summary>
    /// Reset controls value
    /// </summary>
    protected void ResetControls()
    {
        ddlProject.SelectedValue = "0";
        ddlProgramme.SelectedValue = "0";
        ddlExportType.SelectedValue = "0";
    }

    /// <summary>
    /// Handle program  index change event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
        ddlExportType.SelectedValue = "0";
    }
}
