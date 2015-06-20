using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Text;
using Microsoft.Reporting.WebForms;
using Questionnaire_BAO;
using Questionnaire_BE;
using Admin_BAO;
using Microsoft.ReportingServices;
using System.Linq;

public partial class Module_Reports_ExportData : CodeBehindBase
{
    
    #region Globalvariable

    string LogFilePath = string.Empty;
    string mimeType;
    string encoding;
    string fileNameExtension;
    string extension, deviceInfo, outputFileName = "";
    string[] streams;
    string defaultFileName = string.Empty;
    Warning[] warnings;
    WADIdentity identity;
    Project_BAO project_BAO = new Project_BAO();
    Programme_BAO programme_BAO = new Programme_BAO();
    AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
    AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
    ReportManagement_BAO reportManagement_BAO = new ReportManagement_BAO();
    ReportManagement_BE reportManagement_BE = new ReportManagement_BE();


    DataTable dtCompanyName;
    DataTable dtGroupList;
    DataTable dtSelfName;
    DataTable dtReportsID;
    string strGroupList;
    string strFrontPage;
    string strConclusionPage;
    string strRadarChart;
    string strDetailedQst;
    string strCategoryQstlist;
    string strCategoryBarChart;
    string strFullProjGrp;
    string strSelfNameGrp;
    string strReportName;

    string strTargetPersonID;
    string strProjectID;
    string strAccountID;
    string strProgrammeID;
    string strAdmin;

    Category_BAO category_BAO = new Category_BAO();
    Category_BE category_BE = new Category_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int reportCount = 0;
    string pageNo = "";
    //string participantName;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
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

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                ddlAccountCode.SelectedValue = "0";


                Project_BAO project_BAO = new Project_BAO();
                ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
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

 

    protected void btnExport()
    {
        Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();
        rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
        string[] streamids;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string root = string.Empty;


        root = Server.MapPath("~") + "\\ReportGenerate\\";
        string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();

        if (ddlExportType.SelectedValue == "C")
        {
            rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/FeedbackReportbyCategory";
            strReportName = "FeedbackReportbyCategory" + '_' + DateTime.Now.ToString("ddMMyyyy-HHmmss"); 
        }
        else if (ddlExportType.SelectedValue == "Q")
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

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetControls();
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO account_BAO = new Account_BAO();
            dtCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));


            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dtAccount = dtCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();


            if (ddlAccountCode.SelectedIndex > 0)
            {
                DataTable dtprojectlist = project_BAO.GetdtProjectList(Convert.ToString(companycode));

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

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExportType.SelectedValue = "0";
        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
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

    protected void imbExport_Click(object sender, ImageClickEventArgs e)
    {
        btnExport();
    }

    protected void ResetControls()
    {        
        ddlProject.SelectedValue = "0";
        ddlProgramme.SelectedValue = "0";
        ddlExportType.SelectedValue = "0";
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
        ddlExportType.SelectedValue = "0";
    }

}
