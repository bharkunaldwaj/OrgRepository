using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.Reporting.WebForms;
using Questionnaire_BAO;
using Questionnaire_BE;
using Admin_BAO;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using System.Drawing;

public partial class Module_Reports_ReportManagement : CodeBehindBase
{
    #region Global Variable

    string LogFilePath = string.Empty;
    string mimeType;
    string encoding;
    string fileNameExtension;
    string extension, deviceInfo, outputFileName = "";
    string[] streams;
    string defaultFileName = string.Empty;
    Warning[] warnings;
    WADIdentity identity;
    Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
    Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();
    Survey_AccountUser_BAO accountUserBusinessAccessObject = new Survey_AccountUser_BAO();
    Survey_AssignQstnParticipant_BAO assignQuestionnaireBusinessAccessObject = new Survey_AssignQstnParticipant_BAO();
    Survey_ReportManagement_BAO reportManagementBusinessAccessObject = new Survey_ReportManagement_BAO();
    Survey_ReportManagement_BE reportManagementBusinessEntity = new Survey_ReportManagement_BE();

    DataTable dataTableCompanyName;
    DataTable dataTableGroupList;
    DataTable dtSelfName;
    DataTable dtReportsID;
    string stringGroupList;
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
    string stringProjectID;
    string strAccountID;
    string strProgrammeID;
    string filename;
    string file = null;

    #endregion

    /// <summary>
    /// Initilize editor
    /// </summary>
    /// <param name="editor"></param>
    public void initEditor(CKEditor.NET.CKEditorControl editor)
    {
        editor.config.toolbar = new object[]
{
    //new object[] { "Source" },
    //new object[] { "Bold", "Italic", "Underline", "Strike", "-", "Subscript", "Superscript" },
    //new object[] { "NumberedList", "BulletedList", "-", "Outdent", "Indent" },
    //"/",
    //new object[] {"JustifyLeft","JustifyCenter","JustifyRight","JustifyFull"},

    //new object[] { "Styles", "Format", "Font", "FontSize", "TextColor", "BGColor", "-", "About" },


new object[] {"Source"},//,"DocProps","-","Save","NewPage","Preview","-","Templates"
new object[] {"Cut","Copy","Paste"},
new object[] {"Undo","Redo"},
//new object[] {"Form","Checkbox","Radio","TextField","Textarea","Select","Button","ImageButton","HiddenField"},
"/",
new object[] {"Bold","Italic","Underline","RemoveFormat"},
new object[] {"NumberedList","BulletedList","-","Outdent","Indent","Blockquote","JustifyLeft","JustifyCenter","JustifyRight","JustifyBlock"},
//new object[] {"JustifyLeft","JustifyCenter","JustifyRight","JustifyFull"},
new object[] {"Link","Unlink","Anchor"},
"/",
//new object[] {"Format","Styles""Image","Flash","Table","Rule","Smiley","SpecialChar","PageBreak"},
new object[] { "Font","FontSize","TextColor","BGColor"},
new object[] {"UIColor","Maximize","ShowBlocks"} // No comma for the last row.


};
        editor.config.uiColor = "#DADADA";

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // initEditor(txtPageIntroduction);
        // initEditor(txtPageConclusion);

        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        Page.Form.Attributes.Add("enctype", "multipart/form-data");

        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;
            //If GroupID == 1 then user is super Admin then show account section else hide.
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
            //Get all account list by account id.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();
            ddlAccountCode.SelectedValue = "0";
            //If GroupID == 1 then user is super Admin.
            if (identity.User.GroupID == 1)
            {
                Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
                //Get all Project by user account id.
                ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();
            }
        }
    }

    #region Image Button Function
    /// <summary>
    /// show font page image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnFrontPdf_Click(object sender, EventArgs e)
    {
        ProcessFrontPdf("D");
    }

    /// <summary>
    /// Provide preview of the pdf.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LinkPreview_Click(object sender, EventArgs e)
    {
        imbSubmit_Click(null, null);
    }

    /// <summary>
    /// Insert upddate report setting byreport type to batabase
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        //Check if top , middle and bottom image is valid
        if (IsFileValid(fuplTopImage) && IsFileValid(this.fuplMiddleImage) && IsFileValid(this.fuplBottomImage))
        {
            /*
             * Here We are Deleting First the existing Record in Table & then New Insertion will be process 
             */
            string lastLogo = "";
            string frontPageLogo2 = "";
            string frontPageLogo3 = "";
            string frontPdfFileName = "";
            string footer = "";
            string scoretable = "";
            //Get report setting by project ID
            DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));

            if (dataTableReportsetting.Rows.Count > 0)
            {
                lastLogo = dataTableReportsetting.Rows[0]["PageLogo"].ToString();

                frontPageLogo2 = dataTableReportsetting.Rows[0]["FrontPageLogo2"].ToString();

                frontPageLogo3 = dataTableReportsetting.Rows[0]["FrontPageLogo3"].ToString();

                frontPdfFileName = dataTableReportsetting.Rows[0]["FrontPdfFileName"].ToString();

                footer = dataTableReportsetting.Rows[0]["FooterImage"].ToString();
                scoretable = dataTableReportsetting.Rows[0]["ScoreTableImage"].ToString();
            }
            //Delete previous project setting.
            int c = reportManagementBusinessAccessObject.DeleteProjectSettingReport(Convert.ToInt32(ddlProject.SelectedValue));

            /*
             * New Insertion Strart, Initilize properties
             */
            reportManagementBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            reportManagementBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

            reportManagementBusinessEntity.ReportType = ddlReportType.SelectedValue;
            reportManagementBusinessEntity.PageHeading1 = txtPageHeading1.Text.Trim();
            reportManagementBusinessEntity.PageHeading2 = txtPageHeading2.Text.Trim();
            reportManagementBusinessEntity.PageHeading3 = txtPageHeading3.Text.Trim();
            reportManagementBusinessEntity.PageHeadingColor = txtPageHeadingColor.Text.Trim();
            reportManagementBusinessEntity.PageHeadingCopyright = txtPageCopyright.Text.Trim();
            reportManagementBusinessEntity.PageHeadingIntro = Server.HtmlDecode(txtPageIntroduction.Value.Trim());


            int RadarGraphCategoryCount;
            bool res = int.TryParse(txtRadarGraphCategoryCount.Text, out RadarGraphCategoryCount);

            if (res == true)
            {
                reportManagementBusinessEntity.RadarGraphCategoryCount = RadarGraphCategoryCount;
            }
            else
                reportManagementBusinessEntity.RadarGraphCategoryCount = 4;

            string decodeConclusion = Server.HtmlDecode(txtPageConclusion.Value.Trim());

            reportManagementBusinessEntity.PageHeadingConclusion = decodeConclusion;
            reportManagementBusinessEntity.ConclusionHeading = txtConclusionHeading.Text.Trim();

            //////If Admin does't specify the value for Scroes Range then "2" will be insert Default
            //////if (txtConHighLowRange.Text.Trim() != string.Empty)
            //////    reportManagement_BE.ConclusionHighLowRange = txtConHighLowRange.Text.Trim();
            //////else
            //////    reportManagement_BE.ConclusionHighLowRange = "2";

            if (chkCoverPage.Checked == true)
                reportManagementBusinessEntity.CoverPage = "1";
            else
                reportManagementBusinessEntity.CoverPage = "0";

            if (chkShowScoreRespondents.Checked == true)
                reportManagementBusinessEntity.ShowScoreRespondents = true;
            else
                reportManagementBusinessEntity.ShowScoreRespondents = false;

            if (chkReportIntro.Checked == true)
                reportManagementBusinessEntity.ReportIntroduction = "1";
            else
                reportManagementBusinessEntity.ReportIntroduction = "0";

            if (chkConclusion.Checked == true)
                reportManagementBusinessEntity.Conclusionpage = "1";
            else
                reportManagementBusinessEntity.Conclusionpage = "0";

            ////////if (chkPreviousScore.Checked == true)
            ////////    reportManagement_BE.PreviousScoreVisible = "1";
            ////////else
            ////////    reportManagement_BE.PreviousScoreVisible = "0";



            //////if (chkCatQstText.Checked == true)
            //////    reportManagement_BE.QstTextResponses = "1";
            //////else
            //////    reportManagement_BE.QstTextResponses = "0";

            if (chkCatQstlist.Checked == true)
                reportManagementBusinessEntity.CatQstList = "1";
            else
                reportManagementBusinessEntity.CatQstList = "0";

            if (chkCatQstChart.Checked == true)
                reportManagementBusinessEntity.CatDataChart = "1";
            else
                reportManagementBusinessEntity.CatDataChart = "0";

            RetrieveCheckBoxValue();

            if (chkFullPrjGrp.Checked == true)
                reportManagementBusinessEntity.FullProjectGrp = "1";
            else
                reportManagementBusinessEntity.FullProjectGrp = "0";

            if (chkBoxFreeText.Checked == true)
                reportManagementBusinessEntity.FreeTextResponse = "1";
            else
                reportManagementBusinessEntity.FreeTextResponse = "0";

            if (AnalysisI_Chkbox.Checked == true)
                reportManagementBusinessEntity.AnalysisI = "1";
            else
                reportManagementBusinessEntity.AnalysisI = "0";

            if (AnalysisII_Chkbox.Checked == true)
                reportManagementBusinessEntity.AnalysisII = "1";
            else
                reportManagementBusinessEntity.AnalysisII = "0";

            if (AnalysisIII_Chkbox.Checked == true)
                reportManagementBusinessEntity.AnalysisIII = "1";
            else
                reportManagementBusinessEntity.AnalysisIII = "0";

            if (Programme_Avg_Chkbox.Checked == true)
                reportManagementBusinessEntity.Programme_Average = "1";
            else
                reportManagementBusinessEntity.Programme_Average = "0";

            reportManagementBusinessEntity.ShowRadar = chkRadar.Checked;
            reportManagementBusinessEntity.ShowTable = chkTable.Checked;
            reportManagementBusinessEntity.ShowPreviousScore1 = chkPrvScore1.Checked;
            reportManagementBusinessEntity.ShowPreviousScore2 = chkPrvScore2.Checked;
            reportManagementBusinessEntity.ShowBarGraph = chkBarGraph.Checked;
            reportManagementBusinessEntity.ShowLineChart = chkLineChart.Checked;

            //Check if top image has file
            if (fuplTopImage.HasFile)
            {
                //get uploaded file name
                filename = System.IO.Path.GetFileName(fuplTopImage.PostedFile.FileName);
                //Get file unique name
                file = GetUniqueFilename(filename);
                //Get file path
                string path = MapPath("~\\UploadDocs\\") + file;
                //Save the file at specified path.
                fuplTopImage.SaveAs(path);
                string name = file;
                FileStream topFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader topBinaryReader = new BinaryReader(topFileStream);
                Byte[] docbytes = topBinaryReader.ReadBytes((Int32)topFileStream.Length);
                topBinaryReader.Close();
                topFileStream.Close();
                reportManagementBusinessEntity.PageLogo = file;
            }
            else
            {
                if (lastLogo != "" && hdnTopImage.Value != "")
                    reportManagementBusinessEntity.PageLogo = lastLogo;
                else if (Request.QueryString["Mode"] == "E" && fuplTopImage.FileName == "" && hdnTopImage.Value != "")
                    reportManagementBusinessEntity.PageLogo = Convert.ToString(Session["FileName"]);
                else
                    reportManagementBusinessEntity.PageLogo = "";

            }
            //Check if middle image has file
            if (fuplMiddleImage.HasFile)
            {
                //get uploaded file name
                filename = System.IO.Path.GetFileName(fuplMiddleImage.PostedFile.FileName);
                //Get file unique name
                file = GetUniqueFilename(filename);
                //Get file path
                string path = MapPath("~\\UploadDocs\\") + file;
                //Save the file at specified path.
                fuplMiddleImage.SaveAs(path);
                string name = file;
                FileStream middleFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader middleBinaryReader = new BinaryReader(middleFileStream);
                Byte[] docbytes = middleBinaryReader.ReadBytes((Int32)middleFileStream.Length);
                middleBinaryReader.Close();
                middleFileStream.Close();
                reportManagementBusinessEntity.FrontPageLogo2 = file;
            }
            else
            {
                if (frontPageLogo2 != "" && hdnMiddleImage.Value != "")
                    reportManagementBusinessEntity.FrontPageLogo2 = frontPageLogo2;
                else if (Request.QueryString["Mode"] == "E" && fuplMiddleImage.FileName == "" && hdnMiddleImage.Value != "")
                    reportManagementBusinessEntity.FrontPageLogo2 = Convert.ToString(Session["FrontPageLogo2"]);
                else
                    reportManagementBusinessEntity.FrontPageLogo2 = "";

                //string path = MapPath("~\\UploadDocs\\blank.jpg");
                //fuplTopImage.SaveAs(path);

                //FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\blank.jpg"), FileMode.Open, FileAccess.Read);
                //BinaryReader br1 = new BinaryReader(fs1);
                //Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                //br1.Close();
                //fs1.Close();
                //reportManagement_BE.FrontPageLogo2 = "blank.jpg";
            }
            //Check if bottom image has file
            if (fuplBottomImage.HasFile)
            {
                //get uploaded file name
                filename = System.IO.Path.GetFileName(fuplBottomImage.PostedFile.FileName);
                //Get file unique name
                file = GetUniqueFilename(filename);
                //Get file path
                string path = MapPath("~\\UploadDocs\\") + file;
                //Save the file at specified path.
                fuplBottomImage.SaveAs(path);
                string name = file;
                FileStream bottomFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader bottomBinaryReader = new BinaryReader(bottomFileStream);
                Byte[] docbytes = bottomBinaryReader.ReadBytes((Int32)bottomFileStream.Length);
                bottomBinaryReader.Close();
                bottomFileStream.Close();
                reportManagementBusinessEntity.FrontPageLogo3 = file;
            }
            else
            {
                if (frontPageLogo3 != "" && hdnBottomImage.Value != "")
                    reportManagementBusinessEntity.FrontPageLogo3 = frontPageLogo3;
                else if (Request.QueryString["Mode"] == "E" && fuplBottomImage.FileName == "" && hdnBottomImage.Value != "")
                    reportManagementBusinessEntity.FrontPageLogo3 = Convert.ToString(Session["FrontPageLogo3"]);
                else
                    reportManagementBusinessEntity.FrontPageLogo3 = "";

                //string path = MapPath("~\\UploadDocs\\blank.jpg");
                //fuplTopImage.SaveAs(path);

                //FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\blank.jpg"), FileMode.Open, FileAccess.Read);
                //BinaryReader br1 = new BinaryReader(fs1);
                //Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                //br1.Close();
                //fs1.Close();
                //reportManagement_BE.FrontPageLogo3 = "blank.jpg";
            }
            //Upload front page image.
            if (pdfFileUpload.HasFile)
            {
                //get uploaded file name
                filename = System.IO.Path.GetFileName(pdfFileUpload.PostedFile.FileName);
                //Get file unique name
                file = GetUniqueFilename(filename);
                //Get file path
                string path = MapPath("~\\UploadDocs\\") + file;
                //Save the file at specified path.
                pdfFileUpload.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagementBusinessEntity.FrontPdfFileName = file;
            }
            else
            {
                if (!string.IsNullOrEmpty(frontPdfFileName) && hdnFrontPDF.Value != "")
                    reportManagementBusinessEntity.FrontPdfFileName = frontPdfFileName;
                else if (Request.QueryString["Mode"] == "E" && pdfFileUpload.FileName == "" && hdnMiddleImage.Value != "")
                    reportManagementBusinessEntity.FrontPdfFileName = Convert.ToString(Session["frontPdfFileName"]);
                else
                    reportManagementBusinessEntity.FrontPdfFileName = "";

                //if (!string.IsNullOrEmpty(frontPdfFileName))
                //    reportManagement_BE.FrontPdfFileName = frontPdfFileName;

            }
            //Upload score table image.
            if (fuScoreTable.HasFile)
            {
                //Get uploaded file name
                filename = System.IO.Path.GetFileName(fuScoreTable.PostedFile.FileName);
                //filename = FileUpload.FileName;
                //Get file unique name
                file = GetUniqueFilename(filename);
                //Get file path
                string path = MapPath("~\\UploadDocs\\") + file;
                //Save the file at specified path.
                fuScoreTable.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagementBusinessEntity.ScoreTableImage = file;
            }
            else
            {
                if (!string.IsNullOrEmpty(scoretable) && hdnimgScoreTable.Value != "")
                    reportManagementBusinessEntity.ScoreTableImage = scoretable;
                else if (Request.QueryString["Mode"] == "E" && fuScoreTable.FileName == "" && hdnimgScoreTable.Value != "")
                    reportManagementBusinessEntity.ScoreTableImage = Convert.ToString(Session["ScoreTableImage"]);
                else
                    reportManagementBusinessEntity.ScoreTableImage = "";

                //if (!string.IsNullOrEmpty(scoretable))
                //    reportManagement_BE.ScoreTableImage = scoretable;
            }

            //Upload footer table image.
            if (fuFooter.HasFile)
            {
                //Get uploaded file name
                filename = System.IO.Path.GetFileName(fuFooter.PostedFile.FileName);
                //filename = FileUpload.FileName;
                //Get file unique name
                file = GetUniqueFilename(filename);
                //Get file path
                string path = MapPath("~\\UploadDocs\\") + file;
                //Save the file at specified path.
                fuFooter.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagementBusinessEntity.FooterImage = file;
            }
            else
            {
                //if (!string.IsNullOrEmpty(footer))
                //    reportManagement_BE.FooterImage = footer;

                if (!string.IsNullOrEmpty(footer) && hdnimgFooter.Value != "")
                    reportManagementBusinessEntity.FooterImage = footer;
                else if (Request.QueryString["Mode"] == "E" && fuFooter.FileName == "" && hdnimgFooter.Value != "")
                    reportManagementBusinessEntity.FooterImage = Convert.ToString(Session["FooterImage"]);
                else
                    reportManagementBusinessEntity.FooterImage = "";

            }
            //Save the report settings.
            int i = reportManagementBusinessAccessObject.AddProjectSettingReport(reportManagementBusinessEntity);



            if (sender == null && e == null)
            {
                DataTable dtreportsetting2 = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));
                String ProjectReportSettingID = dtreportsetting2.Rows[0]["ProjectReportSettingID"].ToString();
                //SAve preview file
                SavePreview(ProjectReportSettingID.ToString());
            }
            else
            {
                ClearAllConrols();

                lblMessage.Text = "Report settings saved successfully";
            }
        }
    }

    /// <summary>
    /// download  preview pdf file by report type.
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    private void SavePreview(String strTargetPersonID)
    {
        string strReportType = ddlReportType.SelectedValue;
        try
        {
            //  Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();
            rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
            string[] streamids;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string root = string.Empty;
            root = Server.MapPath("~") + "\\ReportGenerate\\";

            /* Function : For Filling Paramters From Controls */
            //If strReportType = 1 Then FeedbackReport will Call
            //If strReportType = 2 Then FeedbackReportClient1 will Call (In this Report We are Showing only Range & Text Type Question).
            if (strReportType == "-1")
            {

                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";
                //set pdf path
                rview.ServerReport.ReportPath = "/Feedback360_UAT/PrvFrontPage";

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "-2")
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient1";
                //rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                //set pdf path
                rview.ServerReport.ReportPath = "/Feedback360_UAT/PrvFrontPageClient1";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "-3")
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient2";
                //  rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                //New Changes 
                //Changed by Amit Singh
                //set pdf path
                rview.ServerReport.ReportPath = "/Feedback360_UAT/PrvFrontPageClient2";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "-4") // Old Mutual Report
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";
                //set pdf path
                rview.ServerReport.ReportPath = "/Feedback360_UAT/PrvCurFrontPage";
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                rview.ServerReport.SetParameters(paramList);
            }
            else if (strReportType == "5") // Old Mutual Report
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";
                //set pdf path
                rview.ServerReport.ReportPath = "/Survey_Prod/Survey_FrontPage_Preview";
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                rview.ServerReport.SetParameters(paramList);
            }
            rview.Visible = false;
            byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            //string PDF_path = root + dirName + "\\" + strReportName + ".pdf";
            string PDF_path = root + Guid.NewGuid() + ".pdf";
            FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create);
            objFs.Write(bytes, 0, bytes.Length);
            objFs.Close();
            objFs.Dispose();

            try
            {//Downloadpdf file
                //string root = Server.MapPath("~") + "\\ReportGenerate\\";
                string openpdf = PDF_path;
                //Response.Write(openpdf);
                Response.ClearContent();
                Response.ClearHeaders();

                Response.AddHeader("Content-Disposition", "attachment; filename=" + PDF_path);
                Response.ContentType = "application/pdf";
                Response.TransmitFile(openpdf);

                Response.Flush();
                Response.Clear();
                Response.Close();

                //This Code Will Delete RadarImage & Pdf After save

                //  File.Delete((root + strReportName + ".pdf"));
            }
            catch (Exception ex)
            { }

            bytes = null;
            System.GC.Collect();
            rview.Dispose();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// It is of no use.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string Strip(string text)
    {
        string stringNewText = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        stringNewText = stringNewText.Replace("&nbsp;", " ");
        stringNewText = Regex.Replace(stringNewText, @"\s+", " ");
        stringNewText = Regex.Replace(stringNewText, @"\n+", "\n");

        return stringNewText;
    }

    /// <summary>
    ///Reset controls Default value.
    /// </summary>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ClearAllConrols();
    }

    /// <summary>
    ///Reset controls Default value.
    /// </summary>
    protected void ClearAllConrols()
    {
        ddlProject.SelectedValue = "0";

        hdnImgTopImage.Value = "";
        hdnImgMiddleImage.Value = "";
        hdnImgBottomImage.Value = "";
        hdnimgScoreTable.Value = "";
        hdnimgFooter.Value = "";

        chkCoverPage.Checked = false;
        chkShowScoreRespondents.Checked = false;
        chkReportIntro.Checked = false;
        chkFullPrjGrp.Checked = false;
        chkBoxFreeText.Checked = false;
        chkCategoryIntro.Checked = false;
        chkCatQstlist.Checked = false;
        chkCatQstChart.Checked = false;
        //////chkCatQstText.Checked = false;
        chkConclusion.Checked = false;
        AnalysisI_Chkbox.Checked = false;
        AnalysisII_Chkbox.Checked = false;
        AnalysisIII_Chkbox.Checked = false;
        Programme_Avg_Chkbox.Checked = false;

        chkRadar.Checked = false;
        chkTable.Checked = false;
        chkPrvScore1.Checked = false;
        chkPrvScore2.Checked = false;
        chkBarGraph.Checked = false;
        chkLineChart.Checked = false;
        //////chkPreviousScore.Checked = false;

        lblMessage.Text = " ";
        ddlReportType.SelectedValue = "0";
        txtPageHeading1.Text = string.Empty;
        txtPageHeading2.Text = string.Empty;
        txtPageHeading3.Text = string.Empty;
        txtPageCopyright.Text = string.Empty;
        txtPageConclusion.Value = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Value = string.Empty;
        txtConclusionHeading.Text = string.Empty;
        txtRadarGraphCategoryCount.Text = string.Empty;
        Session["PageLogo"] = null;
        Session["FileName"] = null;
        Session["frontPdfFileName"] = null;
        Session["FrontPageLogo3"] = null;
        Session["ScoreTableImage"] = null;
        Session["FooterImage"] = null;
    }

    #endregion

    #region dropdown event
    /// <summary>
    /// Bind Project by account id.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Reset controls
        ddlProject.SelectedValue = "0";
        chkCoverPage.Checked = false;
        chkShowScoreRespondents.Checked = false;
        chkReportIntro.Checked = false;

        chkCategoryIntro.Checked = false;
        chkCatQstlist.Checked = false;
        chkCatQstChart.Checked = false;
        //////chkCatQstText.Checked = false;
        chkConclusion.Checked = false;
        chkFullPrjGrp.Checked = false;
        chkBoxFreeText.Checked = false;
        //////chkPreviousScore.Checked = false;

        lblMessage.Text = " ";
        ddlReportType.SelectedValue = "0";
        txtPageHeading1.Text = string.Empty;
        txtPageHeading2.Text = string.Empty;
        txtPageHeading3.Text = string.Empty;
        txtPageCopyright.Text = string.Empty;
        txtPageConclusion.Value = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Value = string.Empty;
        //////txtConHighLowRange.Text = string.Empty;
        txtConclusionHeading.Text = string.Empty;
        ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";

        //TODO: Visibility ON

        catintro.Visible = true;
        catQstText.Visible = true;
        //////prevscr.Visible = true;
        //Rebind editors
        ReBindEditorContent();

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            //Get account id
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO account_BAO = new Account_BAO();
            //Get account details by account id.
            dataTableCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //set company Name
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            if (ddlAccountCode.SelectedIndex > 0)
            {
                //Get all project in an account.
                DataTable dataTableProjectList = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

                if (dataTableProjectList.Rows.Count > 0)
                {
                    //Bind project dropdown
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                    ddlProject.DataSource = dataTableProjectList;
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataBind();
                }
                else
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                }
            }
        }
    }

    /// <summary>
    /// Bind report controls on change of project by project type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkCoverPage.Checked = false;
        chkShowScoreRespondents.Checked = false;
        chkReportIntro.Checked = false;
        chkCategoryIntro.Checked = false;
        chkCatQstlist.Checked = false;
        chkCatQstChart.Checked = false;
        //////chkCatQstText.Checked = false;
        chkConclusion.Checked = false;
        chkFullPrjGrp.Checked = false;
        chkBoxFreeText.Checked = false;

        AnalysisI_Chkbox.Checked = false;
        AnalysisII_Chkbox.Checked = false;
        AnalysisIII_Chkbox.Checked = false;
        Programme_Avg_Chkbox.Checked = false;
        chkRadar.Checked = false;
        chkTable.Checked = false;
        chkPrvScore1.Checked = false;
        chkPrvScore2.Checked = false;
        chkBarGraph.Checked = false;
        chkLineChart.Checked = false;
        ////////chkPreviousScore.Checked = false;

        lblMessage.Text = " ";
        ddlReportType.SelectedValue = "0";
        txtPageHeading1.Text = string.Empty;
        txtPageHeading2.Text = string.Empty;
        txtPageHeading3.Text = string.Empty;
        txtPageCopyright.Text = string.Empty;
        txtPageConclusion.Value = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Value = string.Empty;

        //////txtConHighLowRange.Text = string.Empty;
        txtConclusionHeading.Text = string.Empty;

        ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";

        stringProjectID = ddlProject.SelectedValue;


        //Controls Visibility  Hide/Show Only for Report3.
        ControlHideShow(stringProjectID);

        //Bind report controls
        SaveSettingShow(stringProjectID);
        //REbind Editor
        ReBindEditorContent();
    }

    /*
     * TODO: This Function will Hide/Show the ReportsSettings- Controls for Report3.
     */
    protected void ControlHideShow(string projectid)
    {
        DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));

        if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
        {
            //TODO: Here will Check If the Report is Report3(in db 3 will be there for report3) then
            // Only will Change the controls Visiblity show/hide.
            if (dataTableReportsetting.Rows[0]["ReportType"].ToString() == "3")
            {
                catintro.Visible = false;
                catQstText.Visible = false;

                //////prevscr.Visible = false;
            }
            else
            {
                catintro.Visible = true;
                catQstText.Visible = true;

                //////prevscr.Visible = true;
            }
        }
    }

    /// <summary>
    /// Hide show category introduction section from report section.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue != "3")
        {
            catintro.Visible = true;
            catQstText.Visible = true;
            //////prevscr.Visible = true;
        }
        else if (ddlReportType.SelectedValue == "3")
        {
            catintro.Visible = false;
            catQstText.Visible = false;
            //////prevscr.Visible = false;
        }
    }

    #endregion

    #region Check Box Methods
    /// <summary>
    /// Bind report section 
    /// </summary>
    protected void GroupCheckBoxListBind()
    {
        dataTableGroupList = projectBusinessAccessObject.GetProjectRelationship(Convert.ToInt32(stringProjectID));
    }

    /// <summary>
    /// Get report section value
    /// </summary>
    protected void RetrieveCheckBoxValue()
    {
        stringGroupList = "";

        reportManagementBusinessEntity.ProjectRelationGrp = stringGroupList;
    }

    /// <summary>
    /// Check whether uploaded document is valid or not.
    /// </summary>
    /// <param name="uploadControl"></param>
    /// <returns></returns>
    protected bool IsFileValid(FileUpload uploadControl)
    {
        bool isFileOk = true;

        string[] AllowedExtensions = ConfigurationManager.AppSettings["Fileextension"].Split(',');
        bool isExtensionError = false;
        int MaxSizeAllowed = 5 * 1048576;// Size Allow only in mb

        if (uploadControl.HasFile)
        {
            bool isSizeError = false;
            // Validate for size less than MaxSizeAllowed...
            if (uploadControl.PostedFile.ContentLength > MaxSizeAllowed)
            {
                isFileOk = false;
                isSizeError = true;
            }
            else
            {
                isFileOk = true;
                isSizeError = false;
            }

            // If OK so far, validate the file extension...
            if (isFileOk)
            {
                isFileOk = false;
                isExtensionError = true;

                // Get the file's extension...
                string fileExtension = System.IO.Path.GetExtension(uploadControl.PostedFile.FileName).ToLower();

                for (int i = 0; i < AllowedExtensions.Length; i++)
                {
                    if (fileExtension.Trim() == AllowedExtensions[i].Trim())
                    {
                        isFileOk = true;
                        isExtensionError = false;

                        break;
                    }
                }
            }

            if (isExtensionError)
            {
                string errorMessage = "Invalid file type";

            }
            if (isSizeError)
            {
                string errorMessage = "Maximum Size of the File exceeded";

            }
        }
        return isFileOk;
    }

    /// <summary>
    /// Get unique name for uploaded  images
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));

        return uniquefilename;
    }

    /// <summary>
    /// Bind report controls.
    /// </summary>
    /// <param name="projectid"></param>
    protected void SaveSettingShow(string projectid)
    {
        DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));

        if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
        {
            if (dataTableReportsetting.Rows[0]["ReportType"].ToString() != String.Empty)
                ddlReportType.SelectedValue = dataTableReportsetting.Rows[0]["ReportType"].ToString();
            else
                ddlReportType.SelectedValue = "0";

            if (dataTableReportsetting.Rows[0]["PageHeading1"].ToString() != String.Empty)
                txtPageHeading1.Text = dataTableReportsetting.Rows[0]["PageHeading1"].ToString();
            else
                txtPageHeading1.Text = "";

            if (dataTableReportsetting.Rows[0]["PageHeading2"].ToString() != String.Empty)
                txtPageHeading2.Text = dataTableReportsetting.Rows[0]["PageHeading2"].ToString();
            else
                txtPageHeading2.Text = "";

            if (dataTableReportsetting.Rows[0]["PageHeading2"].ToString() != String.Empty)
                txtPageHeading3.Text = dataTableReportsetting.Rows[0]["PageHeading3"].ToString();
            else
                txtPageHeading3.Text = "";

            if (dataTableReportsetting.Rows[0]["PageHeadingColor"].ToString() != String.Empty)
                txtPageHeadingColor.Text = dataTableReportsetting.Rows[0]["PageHeadingColor"].ToString();
            else
                txtPageHeadingColor.Text = "";

            if (dataTableReportsetting.Rows[0]["ConclusionHeading"].ToString() != String.Empty)
                txtConclusionHeading.Text = dataTableReportsetting.Rows[0]["ConclusionHeading"].ToString();
            else
                txtConclusionHeading.Text = "";

            /*To Show the Image*/
            if (dataTableReportsetting.Rows[0]["PageLogo"].ToString() != String.Empty)
            {
                hdnImgTopImage.Value = dataTableReportsetting.Rows[0]["PageLogo"].ToString();
                Session["PageLogo"] = dataTableReportsetting.Rows[0]["PageLogo"].ToString();
            }
            else
                hdnImgTopImage.Value = "";

            //Set image path.
            if (hdnImgTopImage.Value != "")
                ImgTopImage.Src = "../../UploadDocs/" + hdnImgTopImage.Value;
            else
                ImgTopImage.Src = "../../UploadDocs/noImage.jpg";

            /*To Show the Front Page Logo 2*/
            if (dataTableReportsetting.Rows[0]["FrontPageLogo2"].ToString() != String.Empty)
            {
                hdnImgMiddleImage.Value = dataTableReportsetting.Rows[0]["FrontPageLogo2"].ToString();
                Session["FileName"] = dataTableReportsetting.Rows[0]["FrontPageLogo2"].ToString();
            }
            else
                hdnImgMiddleImage.Value = "";

            //set middle image path
            if (hdnImgMiddleImage.Value != "")
                ImgMiddleImage.Src = "../../UploadDocs/" + hdnImgMiddleImage.Value;
            else
                ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";

            /*To Show the Front Page Logo 3*/
            if (dataTableReportsetting.Rows[0]["frontPdfFileName"].ToString() != String.Empty)
            {
                hdnFrontPDF.Value = dataTableReportsetting.Rows[0]["frontPdfFileName"].ToString();
                Session["frontPdfFileName"] = dataTableReportsetting.Rows[0]["frontPdfFileName"].ToString();
            }
            else
                hdnFrontPDF.Value = "";

            /*To Show the Front Page Logo 3*/
            if (dataTableReportsetting.Rows[0]["FrontPageLogo3"].ToString() != String.Empty)
            {
                hdnImgBottomImage.Value = dataTableReportsetting.Rows[0]["FrontPageLogo3"].ToString();
                Session["FrontPageLogo3"] = dataTableReportsetting.Rows[0]["FrontPageLogo3"].ToString();
            }
            else
                hdnImgBottomImage.Value = "";

            if (dataTableReportsetting.Rows[0]["ScoreTableImage"].ToString() != String.Empty)
            {
                hdnimgScoreTable.Value = dataTableReportsetting.Rows[0]["ScoreTableImage"].ToString();
                Session["ScoreTableImage"] = dataTableReportsetting.Rows[0]["ScoreTableImage"].ToString();
            }
            else
                hdnimgScoreTable.Value = "";

            if (hdnimgScoreTable.Value != "")
                imgScoreTable.Src = "../../UploadDocs/" + hdnimgScoreTable.Value;
            else
                imgScoreTable.Src = "../../UploadDocs/noImage.jpg";


            if (dataTableReportsetting.Rows[0]["FooterImage"].ToString() != String.Empty)
            {
                hdnimgFooter.Value = dataTableReportsetting.Rows[0]["FooterImage"].ToString();
                Session["FooterImage"] = dataTableReportsetting.Rows[0]["FooterImage"].ToString();
            }
            else
                hdnimgFooter.Value = "";

            if (hdnimgFooter.Value != "")
                imgFooter.Src = "../../UploadDocs/" + hdnimgFooter.Value;
            else
                imgFooter.Src = "../../UploadDocs/noImage.jpg";

            if (hdnImgBottomImage.Value != "")
                ImgBottomImage.Src = "../../UploadDocs/" + hdnImgBottomImage.Value;
            else
                ImgBottomImage.Src = "../../UploadDocs/noImage.jpg";

            if (dataTableReportsetting.Rows[0]["PageHeadingCopyright"].ToString() != String.Empty)
                txtPageCopyright.Text = dataTableReportsetting.Rows[0]["PageHeadingCopyright"].ToString();
            else
                txtPageCopyright.Text = "";

            if (dataTableReportsetting.Rows[0]["PageHeadingIntro"].ToString() != String.Empty)
                txtPageIntroduction.Value = Server.HtmlDecode(dataTableReportsetting.Rows[0]["PageHeadingIntro"].ToString());
            else
                txtPageIntroduction.Value = "";

            if (dataTableReportsetting.Rows[0]["PageHeadingConclusion"].ToString() != String.Empty)
                txtPageConclusion.Value = Server.HtmlDecode(dataTableReportsetting.Rows[0]["PageHeadingConclusion"].ToString());
            else
                txtPageConclusion.Value = "";

            //////if (dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString() != String.Empty)
            //////    txtConHighLowRange.Text = dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString();
            //////else
            //////    txtConHighLowRange.Text = "";

            if (dataTableReportsetting.Rows[0]["RadarGraphCategoryCount"].ToString() != String.Empty)
                txtRadarGraphCategoryCount.Text = dataTableReportsetting.Rows[0]["RadarGraphCategoryCount"].ToString();
            else
                txtRadarGraphCategoryCount.Text = "";

            //Check uncheck report selection
            if (dataTableReportsetting.Rows[0]["CoverPage"].ToString() == "1")
                chkCoverPage.Checked = true;
            else
                chkCoverPage.Checked = false;

            if (dataTableReportsetting.Rows[0]["ReportIntroduction"].ToString() == "1")
                chkReportIntro.Checked = true;
            else
                chkReportIntro.Checked = false;

            if (dataTableReportsetting.Rows[0]["ShowScoreRespondents"].ToString() == "True")
                chkShowScoreRespondents.Checked = true;
            else
                chkShowScoreRespondents.Checked = false;

            //////if (dtreportsetting.Rows[0]["QstTextResponses"].ToString() == "1")
            //////    chkCatQstText.Checked = true;
            //////else
            //////    chkCatQstText.Checked = false;

            if (dataTableReportsetting.Rows[0]["CatQstList"].ToString() == "1")
                chkCatQstlist.Checked = true;
            else
                chkCatQstlist.Checked = false;

            if (dataTableReportsetting.Rows[0]["CatDataChart"].ToString() == "1")
                chkCatQstChart.Checked = true;
            else
                chkCatQstChart.Checked = false;

            if (dataTableReportsetting.Rows[0]["Conclusionpage"].ToString() == "1")
                chkConclusion.Checked = true;
            else
                chkConclusion.Checked = false;


            if (chkCatQstChart.Checked == true || chkCatQstlist.Checked == true)
                chkCategoryIntro.Checked = true;
            else
                chkCategoryIntro.Checked = false;

            if (dataTableReportsetting.Rows[0]["FullProjectGrp"].ToString() == "1")

                chkFullPrjGrp.Checked = true;
            else
                chkFullPrjGrp.Checked = false;


            if (dataTableReportsetting.Rows[0]["FreeTextResponses"].ToString() == "1")

                chkBoxFreeText.Checked = true;
            else
                chkBoxFreeText.Checked = false;

            //int RadarGraphCategoryCount;
            //bool res = int.TryParse(txtRadarGraphCategoryCount.Text, out RadarGraphCategoryCount);
            //if (res == false)
            //{
            //    reportManagement_BE.RadarGraphCategoryCount = RadarGraphCategoryCount;
            //}
            //else
            //    reportManagement_BE.RadarGraphCategoryCount = 4;


            if (dataTableReportsetting.Rows[0]["AnalysisI"].ToString() == "1")

                AnalysisI_Chkbox.Checked = true;
            else
                AnalysisI_Chkbox.Checked = false;


            if (dataTableReportsetting.Rows[0]["AnalysisII"].ToString() == "1")

                AnalysisII_Chkbox.Checked = true;
            else
                AnalysisII_Chkbox.Checked = false;

            if (dataTableReportsetting.Rows[0]["AnalysisIII"].ToString() == "1")

                AnalysisIII_Chkbox.Checked = true;
            else
                AnalysisIII_Chkbox.Checked = false;

            if (dataTableReportsetting.Rows[0]["Programme_Average"].ToString() == "1")

                Programme_Avg_Chkbox.Checked = true;
            else
                Programme_Avg_Chkbox.Checked = false;

            if (dataTableReportsetting.Rows[0]["ShowRadar"] != null && Convert.ToString(dataTableReportsetting.Rows[0]["ShowRadar"]) == "True")

                chkRadar.Checked = true;
            else
                chkRadar.Checked = false;

            if (dataTableReportsetting.Rows[0]["ShowTable"] != null && Convert.ToString(dataTableReportsetting.Rows[0]["ShowTable"]) == "True")

                chkTable.Checked = true;
            else
                chkTable.Checked = false;

            if (dataTableReportsetting.Rows[0]["ShowPreviousScore1"] != null && Convert.ToString(dataTableReportsetting.Rows[0]["ShowPreviousScore1"]) == "True")

                chkPrvScore1.Checked = true;
            else
                chkPrvScore1.Checked = false;

            if (dataTableReportsetting.Rows[0]["ShowPreviousScore2"] != null && Convert.ToString(dataTableReportsetting.Rows[0]["ShowPreviousScore2"]) == "True")

                chkPrvScore2.Checked = true;
            else
                chkPrvScore2.Checked = false;

            if (dataTableReportsetting.Rows[0]["ShowBarGraph"] != null && Convert.ToString(dataTableReportsetting.Rows[0]["ShowBarGraph"]) == "True")

                chkBarGraph.Checked = true;
            else
                chkBarGraph.Checked = false;

            if (dataTableReportsetting.Rows[0]["ShowLineChart"] != null && Convert.ToString(dataTableReportsetting.Rows[0]["ShowLineChart"]) == "True")

                chkLineChart.Checked = true;
            else
                chkLineChart.Checked = false;

            ////if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
            ////    chkPreviousScore.Checked = true;
            ////else
            ////    chkPreviousScore.Checked = false;

            string[] group = Regex.Split(dataTableReportsetting.Rows[0]["ProjectRelationGrp"].ToString(), ",");
            if (group.Length > 1)
            {
                group[0] = group[0].Replace("'", "");

            }
        }
    }
    #endregion

    #region ChekboxEvent

    //protected void chkReportIntro_CheckedChanged(object sender, EventArgs e)
    //{
    //    //if(chkReportIntro.Checked == true)
    //    //    chkRadarChart.Checked = true;
    //    //else
    //    //    chkRadarChart.Checked = false;
    //}
    //protected void chkRadarChart_CheckedChanged(object sender, EventArgs e)
    //{
    //    //if (chkRadarChart.Checked == true)
    //    //    chkReportIntro.Checked = true;
    //    //else
    //    //    chkReportIntro.Checked = false;
    //}

    /// <summary>
    /// check if category introduction is checked then check category question and category data chart list else uncheck.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkCategoryIntro_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCategoryIntro.Checked == true)
        {
            chkCatQstlist.Checked = true;
            chkCatQstChart.Checked = true;
        }
        else
        {
            chkCatQstlist.Checked = false;
            chkCatQstChart.Checked = false;
        }
    }

    /// <summary>
    /// check if category question list  is checked then check category introduction else uncheck.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkCatQstlist_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCatQstlist.Checked == true)
            chkCategoryIntro.Checked = true;
        else
        {
            if (chkCatQstChart.Checked == false)
                chkCategoryIntro.Checked = false;
            else
                chkCategoryIntro.Checked = true;
        }
    }

    /// <summary>
    /// check if category data list  is checked then check category introduction else uncheck.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkCatQstChart_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCatQstChart.Checked == true)
            chkCategoryIntro.Checked = true;
        else
        {
            if (chkCatQstlist.Checked == false)
                chkCategoryIntro.Checked = false;
            else
                chkCategoryIntro.Checked = true;
        }
    }

    #endregion
    /// <summary>
    /// Write file content to pdf
    /// </summary>
    /// <param name="sourceFile">source file details</param>
    /// <param name="heading1"> first pdf heading</param>
    /// <param name="heading2">second pdf heading</param>
    /// <param name="heading3">third pdf heading</param>
    /// <param name="htmlcolor">color</param>
    /// <param name="width"></param>
    /// <param name="outputFile"></param>
    protected static void WriteContentToPdf(FileInfo sourceFile, string heading1, string heading2, string heading3, string htmlcolor, float width, out string outputFile)
    {
        DirectoryInfo di = sourceFile.Directory;
        //set pdf file Name.
        string watermarkedFile = di.FullName + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf";
        //File.Copy(sourceFile.FullName, di.FullName + "\\" + watermarkedFile);

        PdfReader reader1 = new PdfReader(sourceFile.FullName);
        using (FileStream fs = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (PdfStamper stamper = new PdfStamper(reader1, fs))
            {
                PdfLayer layer = new PdfLayer("WatermarkLayer", stamper.Writer);

                //rectangle(stamper, htmlcolor);
                Rectangle rect = reader1.GetPageSize(1);
                //Write first heading
                watermark(stamper, layer, rect, heading1, 250, 18, 310, 715);
                //Write second heading
                watermark(stamper, layer, rect, heading2, 270, 16, 310, 685);
                //Write third heading
                watermark(stamper, layer, rect, heading3, 290, 14, 310, 658);

            }
        }

        outputFile = watermarkedFile;
        //string originalFileName = sourceFile.FullName;
        // if (File.Exists(originalFileName))
        // {
        //      File.Delete(originalFileName);

        //  }
        //  File.Move(watermarkedFile, originalFileName);

    }

    /// <summary>
    /// Download pdf if flag is "D"
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public string ProcessFrontPdf(string flag)
    {
        string frontPDFPath = "";
        //If file upload control has file
        if (IsFileValid(pdfFileUpload))
        {
            if (pdfFileUpload.HasFile)
            {
                //Get uploaded file name
                filename = System.IO.Path.GetFileName(pdfFileUpload.PostedFile.FileName);
                //get unique file name
                file = GetUniqueFilename(filename);
                //Get file path
                string path = MapPath("~\\UploadDocs\\") + file;
                //Save uploaded file.
                pdfFileUpload.SaveAs(path);
                string name = file;
                //Read file
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                frontPDFPath = file;
                //Write header to pdf.
                WriteContentToPdf(new FileInfo(path), txtPageHeading1.Text, txtPageHeading2.Text, txtPageHeading3.Text, txtPageHeadingColor.Text, 450f, out path);

                if (flag == "D")//Download pdf
                {
                    string openpdf = path;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file);
                    Response.ContentType = "application/pdf";
                    Response.TransmitFile(openpdf);

                    Response.Flush();
                    Response.Clear();
                    Response.Close();
                }

            }
            else
            {
                //Get front page pdf file name.
                if (hdnFrontPDF.Value != "")
                {
                    string name = hdnFrontPDF.Value;
                    //Get file path
                    FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + hdnFrontPDF.Value, FileMode.Open, FileAccess.Read);
                    BinaryReader br1 = new BinaryReader(fs1);
                    //read in bytes
                    Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                    br1.Close();
                    fs1.Close();

                    string path = MapPath("~\\UploadDocs\\") + name;
                    //Write header to pdf.
                    WriteContentToPdf(new FileInfo(path), txtPageHeading1.Text, txtPageHeading2.Text, txtPageHeading3.Text, txtPageHeadingColor.Text, 450f, out path);

                    if (flag == "D")//Download pdf
                    {

                        string openpdf = path;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + openpdf);
                        Response.ContentType = "application/pdf";
                        Response.TransmitFile(openpdf);

                        Response.Flush();
                        Response.Clear();
                        Response.Close();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "noPreview", "alert('No front page pdf file to preview.');", true);
                }
            }
        }
        return frontPDFPath;
    }

    /// <summary>
    /// It is of No use.
    /// </summary>
    /// <param name="stamper"></param>
    /// <param name="color"></param>
    private static void rectangle(PdfStamper stamper, string color)
    {
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        System.Drawing.Color backgroundcolor = System.Drawing.ColorTranslator.FromHtml(color);
        BaseColor bckgrndco = new BaseColor(backgroundcolor);

        PdfContentByte cb = stamper.GetOverContent(1);
        PdfGState gState = new PdfGState();
        gState.FillOpacity = 0.85f;
        cb.SetGState(gState);

        iTextSharp.text.Rectangle rectangle = new Rectangle(83f, 750f, 555f, 650f);
        rectangle.BorderWidth = 1f;
        rectangle.Border = 15;
        rectangle.BorderColor = BaseColor.BLACK;
        rectangle.BackgroundColor = bckgrndco;

        cb.Rectangle(rectangle);
    }

    /// <summary>
    /// Set content to pdf
    /// </summary>
    /// <param name="stamper">Pdf stamper object</param>
    /// <param name="layer">pdfLayer</param>
    /// <param name="rect">Rectangle</param>
    /// <param name="text">text</param>
    /// <param name="location">locations</param>
    /// <param name="fontsize">fonr size</param>
    /// <param name="xAxis">xAxis position</param>
    /// <param name="yAxis">yAxis position</param>
    private static void watermark(PdfStamper stamper, PdfLayer layer, Rectangle rect, string text, int location, int fontsize, float xAxis, float yAxis)
    {
        PdfContentByte cb = stamper.GetOverContent(1);

        // Tell the cb that the next commands should be "bound" to this new layer
        cb.BeginLayer(layer);
        cb.SetFontAndSize(BaseFont.CreateFont(
          BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), fontsize);

        PdfGState gState = new PdfGState();
        //gState.FillOpacity = 0.25f;
        cb.SetGState(gState);
        //Fill color to content
        cb.SetColorFill(BaseColor.BLACK);
        cb.BeginText();
        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, xAxis, yAxis, 0f);

        cb.EndText();

        cb.EndLayer();
    }

    /// <summary>
    /// Rebind Introduction and Conclusion editor .
    /// </summary>
    private void ReBindEditorContent()
    {
        txtPageIntroduction.Value = Server.HtmlDecode(txtPageIntroduction.InnerHtml);
        txtPageConclusion.Value = Server.HtmlDecode(txtPageConclusion.InnerHtml);
    }
}

