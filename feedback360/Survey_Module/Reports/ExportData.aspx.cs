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
    //string mimeType;
    //string encoding;
    //string fileNameExtension;
    //string extension, deviceInfo, outputFileName = "";
    //string[] streams;
    string defaultFileName = string.Empty;
    //Warning[] warnings;
    WADIdentity identity;
    Survey_Project_BAO project_BAO = new Survey_Project_BAO();
    Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
    Survey_AccountUser_BAO accountUser_BAO = new Survey_AccountUser_BAO();
    Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
    Survey_ReportManagement_BAO reportManagement_BAO = new Survey_ReportManagement_BAO();
    Survey_ReportManagement_BE reportManagement_BE = new Survey_ReportManagement_BE();


    DataTable dtCompanyName;
    //  DataTable dtGroupList;
    //  DataTable dtSelfName;
    //  DataTable dtReportsID;
    //  DataTable analysis_list_dt;

    //  string strGroupList;
    //  string strFrontPage;
    //  string strConclusionPage;
    //  string strRadarChart;
    //  string strDetailedQst;
    //  string strCategoryQstlist;
    //  string strCategoryBarChart;
    //  string strFullProjGrp;
    //  string strSelfNameGrp;
    //  string strReportName;

    ////  string strTargetPersonID;
    //  string strProjectID;
    //  string strAccountID;
    //  string strProgrammeID;
    //  string strAdmin;

    public bool noData;

    Survey_Category_BAO category_BAO = new Survey_Category_BAO();
    Survey_Category_BE category_BE = new Survey_Category_BE();
    Account_BAO account_BAO = new Account_BAO();



    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    //int reportCount = 0;
    //string pageNo = "";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //  noData = true;
            Label ll = (Label)this.Master.FindControl("Current_location");
            ll.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;



            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;


                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                ddlAccountCode.SelectedValue = "0";

                if (identity.User.GroupID == 1)
                {
                    divAccount.Visible = true;
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else
                {
                    divAccount.Visible = false;
                    ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }



                //DataTable get_different_data;
                //get_different_data = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                //ddlProject.DataSource = get_different_data;
                //ddlProject.DataValueField = "ProjectID";
                //ddlProject.DataTextField = "Title";
                //ddlProject.DataBind();

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

        string AccountID = ddlAccountCode.SelectedValue;
        string ProjectID = ddlProject.SelectedValue;
        string CompanyId = ddlCompany.SelectedValue;
        string ProgramID = ddlProgramme.SelectedValue;
        string Analysis = DDList_analysis.SelectedItem.Text;


        Survey_ReportManagement_BAO Sur_GetCategory = new Survey_ReportManagement_BAO();
        reportManagement_BE.ddlAccountCode = ddlAccountCode.SelectedValue;
        reportManagement_BE.ddlProgramme = ddlProgramme.SelectedValue;
        reportManagement_BE.DDList_analysis = DDList_analysis.SelectedItem.Text.Trim();

        DataTable dt_ana = new DataTable();
        reportManagement_BE.SelectFlag = "A";
        dt_ana = Sur_GetCategory.Sur_GetCategory_or_analysis(reportManagement_BE);
        // string str = dt_ana.Rows[0][1].ToString();
        DataTable dtfinal = new DataTable();
        // dtfinal = null;
        string filename = "";
        int iCnt = 0;
        for (int i = 0; i < dt_ana.Rows.Count; i++)
        {
            DataTable dtExportData = new DataTable();
            if (ddlExportType.SelectedValue == "C")
            {
                //dtExportData = reportManagement_BAO.list_data_by_category(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlProgramme.SelectedValue, DDList_analysis.SelectedValue.ToString(), dt_ana.Rows[i][1].ToString());
                dtExportData = reportManagement_BAO.list_data_by_category(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, dt_ana.Rows[i][4].ToString(), dt_ana.Rows[i][1].ToString());
                filename = "DataByCategory.xls";
            }
            else
            {
                DataTable dtQuestion = new DataTable();
                //dtExportData = Sur_GetCategory.list_data_by_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlProgramme.SelectedValue, DDList_analysis.SelectedValue.ToString(), dt_ana.Rows[i][1].ToString());
                dtExportData = Sur_GetCategory.list_data_by_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, dt_ana.Rows[i][4].ToString(), dt_ana.Rows[i][1].ToString());
                filename = "DataByQuestion.xls";

                dtExportData.Columns.Remove("QuestionID");
                DataColumn dc = new DataColumn("QuestionID");
                dtExportData.Columns.Add(dc);
                dtExportData.Columns["QuestionID"].SetOrdinal(0);

                for (int j = 0; j < dtExportData.Rows.Count; j++)
                {
                    dtExportData.Rows[j][0] = "Q" + (j + 1).ToString();

                }
            }
            if (dtExportData != null && dtExportData.Rows.Count > 0)
            {
                iCnt++;
                DataTable dtbyCat = new DataTable();
                dtbyCat = GenerateTransposedTable(dtExportData);
                if (ddlExportType.SelectedValue == "C")
                    dtbyCat.Columns.Remove("CategoryName");
                else
                    dtbyCat.Columns.Remove("QuestionID");

                dtbyCat.Columns.Add(" ");
                dtbyCat.Columns[" "].SetOrdinal(0);
                dtbyCat.Rows[0][" "] = dt_ana.Rows[i][1].ToString();
                if (iCnt == 1)
                {
                    dtfinal = dtbyCat.Copy();
                }
                else
                {
                    foreach (DataRow dc in dtbyCat.Rows)
                    {
                        dtfinal.ImportRow(dc);
                    }
                }
            }
        }

        if (ddlExportType.SelectedValue == "C")
        {
            // noData = true;
            lbl_no_data_to_export_message.Text = "";
            DataTable Full_Project_Group_by_category = reportManagement_BAO.get_final_report_data(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "cf");
            Full_Project_Group_by_category = GenerateTransposedTable(Full_Project_Group_by_category);
            if (dtfinal != null && dtfinal.Rows.Count > 0)
            {
                foreach (DataRow dc in Full_Project_Group_by_category.Rows)
                {
                    dtfinal.ImportRow(dc);
                }
                dtfinal.Rows[dtfinal.Rows.Count - 1][0] = "Full Project Group";
                noData = true;
                lbl_no_data_to_export_message.Text = "";

            }
            else
            {

                DataColumn dc = new DataColumn("No Row Found");
                dtfinal.Columns.Add(dc);
                DataRow dr = dtfinal.NewRow();
                dr[0] = "";
                dtfinal.Rows.Add(dr);
                dtfinal.AcceptChanges();

                // noData = false;
                //  return;
            }

            // --> 1.0.0.1.3 [Export data]
            DataTable Full_Programme_Group_by_category = reportManagement_BAO.get_final_report_data(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "cp");
            Full_Programme_Group_by_category = GenerateTransposedTable(Full_Programme_Group_by_category);
            if (dtfinal != null && dtfinal.Rows.Count > 0)
            {
                foreach (DataRow dc in Full_Programme_Group_by_category.Rows)
                {
                    dtfinal.ImportRow(dc);
                }
                dtfinal.Rows[dtfinal.Rows.Count - 1][0] = "Full Programme Group";
                noData = true;
                lbl_no_data_to_export_message.Text = "";
            }
            else
            {

                DataColumn dc = new DataColumn("No Row Found");
                dtfinal.Columns.Add(dc);
                DataRow dr = dtfinal.NewRow();
                dr[0] = "";
                dtfinal.Rows.Add(dr);
                dtfinal.AcceptChanges();
            }

            // [Export data] 1.0.0.1.3 <--
        }

        if (ddlExportType.SelectedValue == "Q")
        {
            // noData = true;
            DataTable Project_Group_qf_or_qg = reportManagement_BAO.get_final_report_data_for_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "qf");
            Project_Group_qf_or_qg = GenerateTransposedTable(Project_Group_qf_or_qg);
            if (dtfinal != null && dtfinal.Rows.Count > 0)
            {
                dtfinal.Rows.Add(Project_Group_qf_or_qg.Rows[0].ItemArray);
                dtfinal.Rows[dtfinal.Rows.Count - 1][0] = "Full Project Group";

            }
            else
            {

                DataColumn dc = new DataColumn("No Row Found");
                dtfinal.Columns.Add(dc);
                DataRow dr = dtfinal.NewRow();
                dr[0] = "";
                dtfinal.Rows.Add(dr);
                dtfinal.AcceptChanges();

                //   lbl_no_data_to_export_message.Text = "There is no data to export.";
                //    return;
            }

            // --> 1.0.0.1.3 [Data Export]

            DataTable Programme_Group_qf_or_qg = reportManagement_BAO.get_final_report_data_for_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "qp");
            Programme_Group_qf_or_qg = GenerateTransposedTable(Programme_Group_qf_or_qg);
            if (dtfinal != null && dtfinal.Rows.Count > 0)
            {
                dtfinal.Rows.Add(Programme_Group_qf_or_qg.Rows[0].ItemArray);
                dtfinal.Rows[dtfinal.Rows.Count - 1][0] = "Full Project Group";
            }
            else
            {
                DataColumn dc = new DataColumn("No Row Found");
                dtfinal.Columns.Add(dc);
                DataRow dr = dtfinal.NewRow();
                dr[0] = "";
                dtfinal.Rows.Add(dr);
                dtfinal.AcceptChanges();
            }

            // 1.0.0.1.3 [Data Export] <--
        }



        //if (noData == false)
        //{
        //    lbl_no_data_to_export_message.Text = "There is no data to export.";
        //    return;
        //}


        GridView1.DataSource = dtfinal;
        GridView1.DataBind();


        var rows = GridView1.Rows;

        foreach (GridViewRow row in rows)
        {
            row.Cells[0].Font.Bold = true;
        }
        GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        TableCell tbCell = new TableCell();
        tbCell.ColumnSpan = dtfinal.Columns.Count;
        tbCell.Text = "<b>Project: " + ddlProject.SelectedItem.Text + "<br>Company: " + ddlCompany.SelectedItem.Text + "<br>Program: " + ddlProgramme.SelectedItem.Text + "<br>Analysis By: " + DDList_analysis.SelectedItem.Text + "</b>";
        tbCell.Attributes.Add("style", "text-align:center");
        gvr.Cells.Add(tbCell);
        GridView1.Controls[0].Controls.AddAt(0, gvr);

        Response.Clear();
        //Export(filename, GridView1, ddlProject.SelectedItem.Text, ddlProgramme.SelectedItem.Text, dtfinal.Columns.Count);

        string style = @"<style> .text { mso-number-format:0\.0;text-align:center;width:100px; } </style> ";
        // string style = @"<style> .text { mso-number-format:\@;} </style> ";

        foreach (GridViewRow row in GridView1.Rows)
        {
            // add numeric style for each cell
            foreach (TableCell cell in row.Cells)
            {
                cell.Attributes.Add("class", "text");
            }
        }


        Response.AppendHeader("Content-Type", "application/octet-stream");
        Response.AppendHeader("Content-disposition", "attachment; filename=" + filename);
        Response.Charset = "";
        Response.ContentEncoding = Encoding.ASCII;
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        HttpContext.Current.Response.Write(style);



        HtmlForm frm = new HtmlForm();
        this.GridView1.Parent.Controls.Add(frm);
        frm.Attributes["runat"] = "server";
        frm.Controls.Add(this.GridView1);
        frm.RenderControl(htmlWrite);


        Response.Write(stringWrite.ToString());
        Response.End();
    }
    #endregion




    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDList_analysis.Items.Clear();
        DDList_analysis.Items.Insert(0, new ListItem("Select", "0"));
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
        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();


        Survey_Company_BAO company_BAO = new Survey_Company_BAO();
        var dt = company_BAO.GetdtCompanyList(GetCompanyCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dt;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();


        ddlProgramme.Items.Clear();
        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ViewState["prjid"] = ddlProject.SelectedValue.ToString();
        //ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }

    public string GetCompanyCondition()
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

    protected void imbExport_Click(object sender, ImageClickEventArgs e)
    {
        btnExport();
    }

    protected void ResetControls()
    {

        //ddlExportType.Items.Clear();
        //ddlExportType.Items.Insert(0, new ListItem("Select", "0"));

        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        ddlProgramme.Items.Clear();
        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
    }

    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
        ddlExportType.SelectedValue = "0";
        FillAnalysis();
    }
    private DataTable GenerateTransposedTable(DataTable inputTable)
    {
        DataTable outputTable = new DataTable();

        // Add columns by looping rows

        // Header row's first column is same as in inputTable
        outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

        // Header row's second column onwards, 'inputTable's first column taken
        foreach (DataRow inRow in inputTable.Rows)
        {
            string newColName = inRow[0].ToString();
            outputTable.Columns.Add(newColName);
        }

        // Add rows by looping columns        
        for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
        {
            DataRow newRow = outputTable.NewRow();

            // First column is inputTable's Header row's second column
            newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
            for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
            {
                string colValue = inputTable.Rows[cCount][rCount].ToString();
                newRow[cCount + 1] = colValue;
            }
            outputTable.Rows.Add(newRow);
        }

        return outputTable;
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProgramme.Items.Clear();
        DataTable dtProgramme1 = new DataTable();
        dtProgramme1 = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), Convert.ToInt32(ddlCompany.SelectedValue),0);

        if (dtProgramme1.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme1;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlProgramme.Items.Count > 1)
            ddlProgramme.Items[1].Selected = true;

        FillAnalysis();

    }

    private void FillAnalysis()
    {
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), Convert.ToInt32(ddlCompany.SelectedValue), Convert.ToInt32(ddlProgramme.SelectedValue));

        if (dtProgramme != null && dtProgramme.Rows.Count > 0)
        {

            if (dtProgramme.Rows[0]["Analysis_I_Name"] != null && dtProgramme.Rows[0]["Analysis_II_Name"] != null && dtProgramme.Rows[0]["Analysis_III_Name"] != null)
            {
                String[] analysis_data = new String[3] { dtProgramme.Rows[0]["Analysis_I_Name"].ToString(), dtProgramme.Rows[0]["Analysis_II_Name"].ToString(), dtProgramme.Rows[0]["Analysis_III_Name"].ToString() };

                DDList_analysis.Items.Clear();
                DDList_analysis.Items.Insert(0, new ListItem("Select", "0"));
                if (!string.IsNullOrEmpty(Convert.ToString(dtProgramme.Rows[0]["Analysis_I_Name"]).Trim()))
                    DDList_analysis.Items.Insert(1, new ListItem(dtProgramme.Rows[0]["Analysis_I_Name"].ToString(), "Analysis_I"));
                if (!string.IsNullOrEmpty(Convert.ToString(dtProgramme.Rows[0]["Analysis_II_Name"]).Trim()))
                    DDList_analysis.Items.Insert(2, new ListItem(dtProgramme.Rows[0]["Analysis_II_Name"].ToString(), "Analysis_II"));
                if (!string.IsNullOrEmpty(Convert.ToString(dtProgramme.Rows[0]["Analysis_III_Name"]).Trim()))
                    DDList_analysis.Items.Insert(3, new ListItem(dtProgramme.Rows[0]["Analysis_III_Name"].ToString(), "Analysis_III"));

                //DDList_analysis.DataSource = analysis_data;
                //DDList_analysis.DataTextField = "";
                //DDList_analysis.DataBind();
            }
        }
        else
        {
            DDList_analysis.Items.Clear();
            DDList_analysis.Items.Insert(0, new ListItem("Select", "0"));
        }
    }


}
