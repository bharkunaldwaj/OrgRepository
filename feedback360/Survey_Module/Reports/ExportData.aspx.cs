using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Questionnaire_BAO;
using Questionnaire_BE;
using Admin_BAO;

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
    Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
    Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();
    Survey_AccountUser_BAO accountUseBusinessAccessObject = new Survey_AccountUser_BAO();
    Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
    Survey_ReportManagement_BAO reportManagementBusinessAccessObject = new Survey_ReportManagement_BAO();
    Survey_ReportManagement_BE reportManagementBusinesEntity = new Survey_ReportManagement_BE();


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

    Survey_Category_BAO categoryBusinessAccessObject = new Survey_Category_BAO();
    //Survey_Category_BE category_BE = new Survey_Category_BE();
    Account_BAO accountBusinessAccessObject = new Account_BAO();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    //int reportCount = 0;
    //string pageNo = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
            labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;

            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                //Get all account details in a user account id.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                ddlAccountCode.SelectedValue = "0";

                //If user ia super admin then GroupID = 1 then show acount detals section else hide.
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
    /// <summary>
    /// Export date by categoy and questions.
    /// </summary>
    protected void btnExport()
    {
        string AccountID = ddlAccountCode.SelectedValue;
        string ProjectID = ddlProject.SelectedValue;
        string CompanyId = ddlCompany.SelectedValue;
        string ProgramID = ddlProgramme.SelectedValue;
        string Analysis = DDList_analysis.SelectedItem.Text;

        Survey_ReportManagement_BAO CategoryBusinessAccessObject = new Survey_ReportManagement_BAO();
        reportManagementBusinesEntity.ddlAccountCode = ddlAccountCode.SelectedValue;
        reportManagementBusinesEntity.ddlProgramme = ddlProgramme.SelectedValue;
        reportManagementBusinesEntity.DDList_analysis = DDList_analysis.SelectedItem.Text.Trim();

        DataTable dataTableAnalysis = new DataTable();
        reportManagementBusinesEntity.SelectFlag = "A";
        dataTableAnalysis = CategoryBusinessAccessObject.Sur_GetCategory_or_analysis(reportManagementBusinesEntity);
        // string str = dt_ana.Rows[0][1].ToString();
        DataTable dataTablefinal = new DataTable();
        // dtfinal = null;
        string filename = "";
        int iCnt = 0;
        for (int i = 0; i < dataTableAnalysis.Rows.Count; i++)
        {
            DataTable dataTableExportData = new DataTable();
            //If export by actegory
            if (ddlExportType.SelectedValue == "C")
            {
                //dtExportData = reportManagement_BAO.list_data_by_category(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlProgramme.SelectedValue, DDList_analysis.SelectedValue.ToString(), dt_ana.Rows[i][1].ToString());
                dataTableExportData = reportManagementBusinessAccessObject.list_data_by_category(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, dataTableAnalysis.Rows[i][4].ToString(), dataTableAnalysis.Rows[i][1].ToString());
                filename = "DataByCategory.xls";
            }
            else
            {
                //Export by question.
                DataTable dataTableQuestion = new DataTable();
                //dtExportData = Sur_GetCategory.list_data_by_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlProgramme.SelectedValue, DDList_analysis.SelectedValue.ToString(), dt_ana.Rows[i][1].ToString());
                dataTableExportData = CategoryBusinessAccessObject.list_data_by_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, dataTableAnalysis.Rows[i][4].ToString(), dataTableAnalysis.Rows[i][1].ToString());
                filename = "DataByQuestion.xls";

                dataTableExportData.Columns.Remove("QuestionID");
                DataColumn dc = new DataColumn("QuestionID");
                dataTableExportData.Columns.Add(dc);
                dataTableExportData.Columns["QuestionID"].SetOrdinal(0);

                for (int j = 0; j < dataTableExportData.Rows.Count; j++)
                {
                    dataTableExportData.Rows[j][0] = "Q" + (j + 1).ToString();
                }
            }

            if (dataTableExportData != null && dataTableExportData.Rows.Count > 0)
            {
                iCnt++;
                DataTable dataTablebyCatategory = new DataTable();

                dataTablebyCatategory = GenerateTransposedTable(dataTableExportData);

                if (ddlExportType.SelectedValue == "C")
                    dataTablebyCatategory.Columns.Remove("CategoryName");
                else
                    dataTablebyCatategory.Columns.Remove("QuestionID");

                dataTablebyCatategory.Columns.Add(" ");
                dataTablebyCatategory.Columns[" "].SetOrdinal(0);
                dataTablebyCatategory.Rows[0][" "] = dataTableAnalysis.Rows[i][1].ToString();

                if (iCnt == 1)
                {
                    dataTablefinal = dataTablebyCatategory.Copy();
                }
                else
                {
                    foreach (DataRow dataRowCategory in dataTablebyCatategory.Rows)
                    {
                        dataTablefinal.ImportRow(dataRowCategory);
                    }
                }
            }
        }
        //To export by category
        if (ddlExportType.SelectedValue == "C")
        {

            // --> 1.0.0.1.3 [Export data]
            DataTable Full_Programme_Group_by_category = reportManagementBusinessAccessObject.get_final_report_data(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "cp");
            Full_Programme_Group_by_category = GenerateTransposedTable(Full_Programme_Group_by_category);
            if (dataTablefinal != null && dataTablefinal.Rows.Count > 0)
            {
                foreach (DataRow dc in Full_Programme_Group_by_category.Rows)
                {
                    dataTablefinal.ImportRow(dc);
                }
                dataTablefinal.Rows[dataTablefinal.Rows.Count - 1][0] = "Programme Average";
                noData = true;
                lbl_no_data_to_export_message.Text = "";
            }
            else
            {

                DataColumn dc = new DataColumn("No Row Found");
                dataTablefinal.Columns.Add(dc);
                DataRow dr = dataTablefinal.NewRow();
                dr[0] = "";
                dataTablefinal.Rows.Add(dr);
                dataTablefinal.AcceptChanges();
            }


            // noData = true;
            lbl_no_data_to_export_message.Text = "";
            DataTable Full_Project_Group_by_category = reportManagementBusinessAccessObject.get_final_report_data(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "cf");

            Full_Project_Group_by_category = GenerateTransposedTable(Full_Project_Group_by_category);

            if (dataTablefinal != null && dataTablefinal.Rows.Count > 0)
            {
                foreach (DataRow dc in Full_Project_Group_by_category.Rows)
                {
                    dataTablefinal.ImportRow(dc);
                }
                dataTablefinal.Rows[dataTablefinal.Rows.Count - 1][0] = "Full Project Group";
                noData = true;
                lbl_no_data_to_export_message.Text = "";
            }
            else
            {
                DataColumn dc = new DataColumn("No Row Found");
                dataTablefinal.Columns.Add(dc);
                DataRow dr = dataTablefinal.NewRow();
                dr[0] = "";
                dataTablefinal.Rows.Add(dr);
                dataTablefinal.AcceptChanges();
                // noData = false;
                //  return;
            }
            // [Export data] 1.0.0.1.3 <--
        }
        //To export by Question
        if (ddlExportType.SelectedValue == "Q")
        {
            // --> 1.0.0.1.3 [Data Export]
            DataTable Programme_Group_qf_or_qg = reportManagementBusinessAccessObject.get_final_report_data_for_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "qp");
            Programme_Group_qf_or_qg = GenerateTransposedTable(Programme_Group_qf_or_qg);

            if (dataTablefinal != null && dataTablefinal.Rows.Count > 0)
            {
                dataTablefinal.Rows.Add(Programme_Group_qf_or_qg.Rows[0].ItemArray);
                dataTablefinal.Rows[dataTablefinal.Rows.Count - 1][0] = "Programme Average";
            }
            else
            {
                DataColumn dc = new DataColumn("No Row Found");
                dataTablefinal.Columns.Add(dc);
                DataRow dr = dataTablefinal.NewRow();
                dr[0] = "";
                dataTablefinal.Rows.Add(dr);
                dataTablefinal.AcceptChanges();
            }
            // 1.0.0.1.3 [Data Export] <--
            // noData = true; Full Project Group
            DataTable Project_Group_qf_or_qg = reportManagementBusinessAccessObject.get_final_report_data_for_question(ddlAccountCode.SelectedValue, ddlProject.SelectedValue, ddlCompany.SelectedValue, ddlProgramme.SelectedValue, "qf");
            Project_Group_qf_or_qg = GenerateTransposedTable(Project_Group_qf_or_qg);

            if (dataTablefinal != null && dataTablefinal.Rows.Count > 0)
            {
                dataTablefinal.Rows.Add(Project_Group_qf_or_qg.Rows[0].ItemArray);
                dataTablefinal.Rows[dataTablefinal.Rows.Count - 1][0] = "Full Project Group";
            }
            else
            {
                DataColumn dc = new DataColumn("No Row Found");
                dataTablefinal.Columns.Add(dc);
                DataRow dr = dataTablefinal.NewRow();
                dr[0] = "";
                dataTablefinal.Rows.Add(dr);
                dataTablefinal.AcceptChanges();
                //   lbl_no_data_to_export_message.Text = "There is no data to export.";
                //    return;
            }
        }
        //if (noData == false)
        //{
        //    lbl_no_data_to_export_message.Text = "There is no data to export.";
        //    return;
        //}

        GridView1.DataSource = dataTablefinal;
        GridView1.DataBind();

        var rows = GridView1.Rows;

        foreach (GridViewRow row in rows)
        {
            row.Cells[0].Font.Bold = true;
        }

        GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        TableCell tbCell = new TableCell();
        tbCell.ColumnSpan = dataTablefinal.Columns.Count;
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

        //Download.
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

    /// <summary>
    /// Bind project in an account
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDList_analysis.Items.Clear();
        DDList_analysis.Items.Insert(0, new ListItem("Select", "0"));
        ResetControls();

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            //Set account id
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO account_BAO = new Account_BAO();
            //Get account details
            dtCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dtCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //set company name
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            if (ddlAccountCode.SelectedIndex > 0)
            {
                //get project list by comany code and bind project dropdown
                DataTable dataTableProjectlist = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

                if (dataTableProjectlist.Rows.Count > 0)
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));

                    ddlProject.DataSource = dataTableProjectlist;
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
        Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();
        Survey_Company_BAO companyBusinessAccessObject = new Survey_Company_BAO();

        //get all company  and bind company dropdown
        var dataTableCompany = companyBusinessAccessObject.GetdtCompanyList(GetCompanyCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dataTableCompany;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();

        ddlProgramme.Items.Clear();
        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ViewState["prjid"] = ddlProject.SelectedValue.ToString();
        //ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }

    /// <summary>
    /// Generate dynamic query.
    /// </summary>
    /// <returns></returns>
    public string GetCompanyCondition()
    {
        string stringQuery = "";

        //if (Convert.ToInt32(ViewState["AccountID"]) > 0)
        //    str = str + "" + ViewState["AccountID"] + " and ";
        //else
        //    str = str + "" + identity.User.AccountID.ToString() + " and ";

        if (ddlAccountCode.SelectedIndex > 0)
            stringQuery = stringQuery + "" + ddlAccountCode.SelectedValue + " and ";

        if (ddlProject.SelectedIndex > 0)
            stringQuery = stringQuery + "Survey_Project.[ProjectID] = " + ddlProject.SelectedValue + " and ";

        string param = stringQuery.Substring(0, stringQuery.Length - 4);

        return param;
    }

    /// <summary>
    ///  Export  category and question data
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
        //ddlExportType.Items.Clear();
        //ddlExportType.Items.Insert(0, new ListItem("Select", "0"));
        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        ddlProgramme.Items.Clear();
        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
    }

    /// <summary>
    /// Bind  analysis type in program.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
        ddlExportType.SelectedValue = "0";
        FillAnalysis();
    }

    /// <summary>
    /// Generate table for category and question type.
    /// </summary>
    /// <param name="inputTable"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Bind program in a comany
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get all program in a project and bind progrma dropdown
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), Convert.ToInt32(ddlCompany.SelectedValue), 0);

        if (dataTableProgramme.Rows.Count > 0)
        {
            //bind progrma dropdown
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        if (ddlProgramme.Items.Count > 1)
            ddlProgramme.Items[1].Selected = true;

        //Fill analysis type drop down.
        FillAnalysis();
    }

    /// <summary>
    /// Fill analysis type drop down
    /// </summary>
    private void FillAnalysis()
    {
        DataTable dataTableProgramme = new DataTable();
        //Get all program and insert analysis type
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue),
            Convert.ToInt32(ddlCompany.SelectedValue), Convert.ToInt32(ddlProgramme.SelectedValue));

        if (dataTableProgramme != null && dataTableProgramme.Rows.Count > 0)
        {
            if (dataTableProgramme.Rows[0]["Analysis_I_Name"] != null && dataTableProgramme.Rows[0]["Analysis_II_Name"] != null && dataTableProgramme.Rows[0]["Analysis_III_Name"] != null)
            {
                String[] analysis_data = new String[3] { dataTableProgramme.Rows[0]["Analysis_I_Name"].ToString(), dataTableProgramme.Rows[0]["Analysis_II_Name"].ToString(), dataTableProgramme.Rows[0]["Analysis_III_Name"].ToString() };

                DDList_analysis.Items.Clear();
                DDList_analysis.Items.Insert(0, new ListItem("Select", "0"));

                if (!string.IsNullOrEmpty(Convert.ToString(dataTableProgramme.Rows[0]["Analysis_I_Name"]).Trim()))
                    DDList_analysis.Items.Insert(1, new ListItem(dataTableProgramme.Rows[0]["Analysis_I_Name"].ToString(), "Analysis_I"));
                if (!string.IsNullOrEmpty(Convert.ToString(dataTableProgramme.Rows[0]["Analysis_II_Name"]).Trim()))
                    DDList_analysis.Items.Insert(2, new ListItem(dataTableProgramme.Rows[0]["Analysis_II_Name"].ToString(), "Analysis_II"));
                if (!string.IsNullOrEmpty(Convert.ToString(dataTableProgramme.Rows[0]["Analysis_III_Name"]).Trim()))
                    DDList_analysis.Items.Insert(3, new ListItem(dataTableProgramme.Rows[0]["Analysis_III_Name"].ToString(), "Analysis_III"));

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
