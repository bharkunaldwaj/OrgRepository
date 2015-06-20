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
    Survey_Project_BAO project_BAO = new Survey_Project_BAO();
    Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
    Survey_AccountUser_BAO accountUser_BAO = new Survey_AccountUser_BAO();
    Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
    Survey_ReportManagement_BAO reportManagement_BAO = new Survey_ReportManagement_BAO();
    Survey_ReportManagement_BE reportManagement_BE = new Survey_ReportManagement_BE();


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
    string filename;
    string file = null;

    #endregion

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
        initEditor(txtPageIntroduction);
        initEditor(txtPageConclusion);

        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        Page.Form.Attributes.Add("enctype", "multipart/form-data");
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

            if (identity.User.GroupID == 1)
            {
                Survey_Project_BAO project_BAO = new Survey_Project_BAO();
                ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();
            }

        }


    }

    #region Image Button Function



    protected void lnkbtnFrontPdf_Click(object sender, EventArgs e)
    {
      
        ProcessFrontPdf("D");
    }

    protected void LinkPreview_Click(object sender, EventArgs e)
    {
        imbSubmit_Click(null, null);
    }

    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {



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

            DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));
            if (dtreportsetting.Rows.Count > 0)
            {
                lastLogo = dtreportsetting.Rows[0]["PageLogo"].ToString();

                frontPageLogo2 = dtreportsetting.Rows[0]["FrontPageLogo2"].ToString();

                frontPageLogo3 = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();

                frontPdfFileName = dtreportsetting.Rows[0]["FrontPdfFileName"].ToString();

                footer = dtreportsetting.Rows[0]["FooterImage"].ToString();
                scoretable = dtreportsetting.Rows[0]["ScoreTableImage"].ToString();
            }

            int c = reportManagement_BAO.DeleteProjectSettingReport(Convert.ToInt32(ddlProject.SelectedValue));

            /*
             * New Insertion Strart 
             */
            reportManagement_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            reportManagement_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

            reportManagement_BE.ReportType = ddlReportType.SelectedValue;
            reportManagement_BE.PageHeading1 = txtPageHeading1.Text.Trim();
            reportManagement_BE.PageHeading2 = txtPageHeading2.Text.Trim();
            reportManagement_BE.PageHeading3 = txtPageHeading3.Text.Trim();
            reportManagement_BE.PageHeadingColor = txtPageHeadingColor.Text.Trim();
            reportManagement_BE.PageHeadingCopyright = txtPageCopyright.Text.Trim();
            reportManagement_BE.PageHeadingIntro = txtPageIntroduction.Text.Trim();


            int RadarGraphCategoryCount;
            bool res = int.TryParse(txtRadarGraphCategoryCount.Text, out RadarGraphCategoryCount);
            if (res == true)
            {
                reportManagement_BE.RadarGraphCategoryCount = RadarGraphCategoryCount;
            }
            else
                reportManagement_BE.RadarGraphCategoryCount = 4;

            string sss = txtPageConclusion.Text.Trim();

            reportManagement_BE.PageHeadingConclusion = sss;
            reportManagement_BE.ConclusionHeading = txtConclusionHeading.Text.Trim();

            //////If Admin does't specify the value for Scroes Range then "2" will be insert Default
            //////if (txtConHighLowRange.Text.Trim() != string.Empty)
            //////    reportManagement_BE.ConclusionHighLowRange = txtConHighLowRange.Text.Trim();
            //////else
            //////    reportManagement_BE.ConclusionHighLowRange = "2";

            if (chkCoverPage.Checked == true)
                reportManagement_BE.CoverPage = "1";
            else
                reportManagement_BE.CoverPage = "0";

            if (chkShowScoreRespondents.Checked == true)
                reportManagement_BE.ShowScoreRespondents = true;
            else
                reportManagement_BE.ShowScoreRespondents = false;


            if (chkReportIntro.Checked == true)
                reportManagement_BE.ReportIntroduction = "1";
            else
                reportManagement_BE.ReportIntroduction = "0";



            if (chkConclusion.Checked == true)
                reportManagement_BE.Conclusionpage = "1";
            else
                reportManagement_BE.Conclusionpage = "0";

            ////////if (chkPreviousScore.Checked == true)
            ////////    reportManagement_BE.PreviousScoreVisible = "1";
            ////////else
            ////////    reportManagement_BE.PreviousScoreVisible = "0";



            //////if (chkCatQstText.Checked == true)
            //////    reportManagement_BE.QstTextResponses = "1";
            //////else
            //////    reportManagement_BE.QstTextResponses = "0";

            if (chkCatQstlist.Checked == true)
                reportManagement_BE.CatQstList = "1";
            else
                reportManagement_BE.CatQstList = "0";

            if (chkCatQstChart.Checked == true)
                reportManagement_BE.CatDataChart = "1";
            else
                reportManagement_BE.CatDataChart = "0";


            RetrieveCheckBoxValue();

            if (chkFullPrjGrp.Checked == true)
                reportManagement_BE.FullProjectGrp = "1";
            else
                reportManagement_BE.FullProjectGrp = "0";

            if (chkBoxFreeText.Checked == true)
                reportManagement_BE.FreeTextResponse = "1";
            else
                reportManagement_BE.FreeTextResponse = "0";


            if (AnalysisI_Chkbox.Checked == true)
                reportManagement_BE.AnalysisI = "1";
            else
                reportManagement_BE.AnalysisI = "0";

            if (AnalysisII_Chkbox.Checked == true)
                reportManagement_BE.AnalysisII = "1";
            else
                reportManagement_BE.AnalysisII = "0";

            if (AnalysisIII_Chkbox.Checked == true)
                reportManagement_BE.AnalysisIII = "1";
            else
                reportManagement_BE.AnalysisIII = "0";


            if (Programme_Avg_Chkbox.Checked == true)
                reportManagement_BE.Programme_Average = "1";
            else
                reportManagement_BE.Programme_Average = "0";

            reportManagement_BE.ShowRadar = chkRadar.Checked;
            reportManagement_BE.ShowTable = chkTable.Checked;
            reportManagement_BE.ShowPreviousScore1 = chkPrvScore1.Checked;
            reportManagement_BE.ShowPreviousScore2 = chkPrvScore2.Checked;
            reportManagement_BE.ShowBarGraph = chkBarGraph.Checked;
            reportManagement_BE.ShowLineChart = chkLineChart.Checked;


            if (fuplTopImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuplTopImage.PostedFile.FileName);

                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                fuplTopImage.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagement_BE.PageLogo = file;
            }
            else
            {
                if (lastLogo != "" && hdnTopImage.Value != "")
                    reportManagement_BE.PageLogo = lastLogo;
                else if (Request.QueryString["Mode"] == "E" && fuplTopImage.FileName == "" && hdnTopImage.Value != "")
                    reportManagement_BE.PageLogo = Convert.ToString(Session["FileName"]);
                else
                    reportManagement_BE.PageLogo = "";

            }

            if (fuplMiddleImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuplMiddleImage.PostedFile.FileName);

                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                fuplMiddleImage.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagement_BE.FrontPageLogo2 = file;
            }
            else
            {
                if (frontPageLogo2 != "" && hdnMiddleImage.Value != "")
                    reportManagement_BE.FrontPageLogo2 = frontPageLogo2;
                else if (Request.QueryString["Mode"] == "E" && fuplMiddleImage.FileName == "" && hdnMiddleImage.Value != "")
                    reportManagement_BE.FrontPageLogo2 = Convert.ToString(Session["FrontPageLogo2"]);
                else
                    reportManagement_BE.FrontPageLogo2 = "";

                //string path = MapPath("~\\UploadDocs\\blank.jpg");
                //fuplTopImage.SaveAs(path);

                //FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\blank.jpg"), FileMode.Open, FileAccess.Read);
                //BinaryReader br1 = new BinaryReader(fs1);
                //Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                //br1.Close();
                //fs1.Close();
                //reportManagement_BE.FrontPageLogo2 = "blank.jpg";
            }

            if (fuplBottomImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuplBottomImage.PostedFile.FileName);

                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                fuplBottomImage.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagement_BE.FrontPageLogo3 = file;
            }
            else
            {
                if (frontPageLogo3 != "" && hdnBottomImage.Value != "")
                    reportManagement_BE.FrontPageLogo3 = frontPageLogo3;
                else if (Request.QueryString["Mode"] == "E" && fuplBottomImage.FileName == "" && hdnBottomImage.Value != "")
                    reportManagement_BE.FrontPageLogo3 = Convert.ToString(Session["FrontPageLogo3"]);
                else
                    reportManagement_BE.FrontPageLogo3 = "";

                //string path = MapPath("~\\UploadDocs\\blank.jpg");
                //fuplTopImage.SaveAs(path);

                //FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\blank.jpg"), FileMode.Open, FileAccess.Read);
                //BinaryReader br1 = new BinaryReader(fs1);
                //Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                //br1.Close();
                //fs1.Close();
                //reportManagement_BE.FrontPageLogo3 = "blank.jpg";



            }

            if (pdfFileUpload.HasFile)
            {
                filename = System.IO.Path.GetFileName(pdfFileUpload.PostedFile.FileName);

                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                pdfFileUpload.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagement_BE.FrontPdfFileName = file;
            }
            else
            {



                if (!string.IsNullOrEmpty(frontPdfFileName) && hdnFrontPDF.Value != "")
                    reportManagement_BE.FrontPdfFileName = frontPdfFileName;
                else if (Request.QueryString["Mode"] == "E" && pdfFileUpload.FileName == "" && hdnMiddleImage.Value != "")
                    reportManagement_BE.FrontPdfFileName = Convert.ToString(Session["frontPdfFileName"]);
                else
                    reportManagement_BE.FrontPdfFileName = "";

                //if (!string.IsNullOrEmpty(frontPdfFileName))
                //    reportManagement_BE.FrontPdfFileName = frontPdfFileName;

            }

            if (fuScoreTable.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuScoreTable.PostedFile.FileName);
                //filename = FileUpload.FileName;
                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                fuScoreTable.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagement_BE.ScoreTableImage = file;
            }
            else
            {



                if (!string.IsNullOrEmpty(scoretable) && hdnimgScoreTable.Value != "")
                    reportManagement_BE.ScoreTableImage = scoretable;
                else if (Request.QueryString["Mode"] == "E" && fuScoreTable.FileName == "" && hdnimgScoreTable.Value != "")
                    reportManagement_BE.ScoreTableImage = Convert.ToString(Session["ScoreTableImage"]);
                else
                    reportManagement_BE.ScoreTableImage = "";

                //if (!string.IsNullOrEmpty(scoretable))
                //    reportManagement_BE.ScoreTableImage = scoretable;
            }

            if (fuFooter.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuFooter.PostedFile.FileName);
                //filename = FileUpload.FileName;
                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                fuFooter.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagement_BE.FooterImage = file;
            }
            else
            {
                //if (!string.IsNullOrEmpty(footer))
                //    reportManagement_BE.FooterImage = footer;

                if (!string.IsNullOrEmpty(footer) && hdnimgFooter.Value != "")
                    reportManagement_BE.FooterImage = footer;
                else if (Request.QueryString["Mode"] == "E" && fuFooter.FileName == "" && hdnimgFooter.Value != "")
                    reportManagement_BE.FooterImage = Convert.ToString(Session["FooterImage"]);
                else
                    reportManagement_BE.FooterImage = "";

            }

            int i = reportManagement_BAO.AddProjectSettingReport(reportManagement_BE);



            if (sender == null && e == null)
            {
                DataTable dtreportsetting2 = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));
                String ProjectReportSettingID = dtreportsetting2.Rows[0]["ProjectReportSettingID"].ToString();
                SavePreview(ProjectReportSettingID.ToString());
            }
            else
            {
                ClearAllConrols();

                lblMessage.Text = "Report settings saved successfully";
            }


            //ClearAllConrols();

            // lblMessage.Text = "Report settings saved successfully";
        }
    }
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

                rview.ServerReport.ReportPath = "/Feedback360_UAT/PrvCurFrontPage";
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                rview.ServerReport.SetParameters(paramList);
            }
            else if (strReportType == "5") // Old Mutual Report
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

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
            {
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


    public string Strip(string text)
    {
        string s = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        s = s.Replace("&nbsp;", " ");
        s = Regex.Replace(s, @"\s+", " ");
        s = Regex.Replace(s, @"\n+", "\n");
        return s;
    }





    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ClearAllConrols();
    }

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
        txtPageConclusion.Text = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Text = string.Empty;
        txtConclusionHeading.Text = string.Empty;
        txtRadarGraphCategoryCount.Text = string.Empty;
        Session["PageLogo"]= null;
        Session["FileName"] = null;
        Session["frontPdfFileName"]= null;
        Session["FrontPageLogo3"]= null;
        Session["ScoreTableImage"]= null;
        Session["FooterImage"] = null;

    }

    #endregion

    #region dropdown event
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        txtPageConclusion.Text = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Text = string.Empty;
        //////txtConHighLowRange.Text = string.Empty;
        txtConclusionHeading.Text = string.Empty;
        ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";

        //TODO: Visibility ON

        catintro.Visible = true;
        catQstText.Visible = true;
        //////prevscr.Visible = true;


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
                    ddlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                    ddlProject.DataSource = dtprojectlist;
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
        txtPageConclusion.Text = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Text = string.Empty;

        //////txtConHighLowRange.Text = string.Empty;
        txtConclusionHeading.Text = string.Empty;

        ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";

        strProjectID = ddlProject.SelectedValue;


        //Controls Visibility  Hide/Show Only for Report3.
        ControlHideShow(strProjectID);


        SaveSettingShow(strProjectID);
    }

    /*
     * TODO: This Function will Hide/Show the ReportsSettings- Controls for Report3.
     */
    protected void ControlHideShow(string projectid)
    {
        DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));
        if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
        {
            //TODO: Here will Check If the Report is Report3(in db 3 will be there for report3) then
            // Only will Change the controls Visiblity show/hide.
            if (dtreportsetting.Rows[0]["ReportType"].ToString() == "3")
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

    protected void GroupCheckBoxListBind()
    {
        dtGroupList = project_BAO.GetProjectRelationship(Convert.ToInt32(strProjectID));

    }

    protected void RetrieveCheckBoxValue()
    {
        strGroupList = "";

        reportManagement_BE.ProjectRelationGrp = strGroupList;
    }

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

    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));

        return uniquefilename;
    }

    protected void SaveSettingShow(string projectid)
    {
        DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));
        if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
        {
            if (dtreportsetting.Rows[0]["ReportType"].ToString() != String.Empty)
                ddlReportType.SelectedValue = dtreportsetting.Rows[0]["ReportType"].ToString();
            else
                ddlReportType.SelectedValue = "0";

            if (dtreportsetting.Rows[0]["PageHeading1"].ToString() != String.Empty)
                txtPageHeading1.Text = dtreportsetting.Rows[0]["PageHeading1"].ToString();
            else
                txtPageHeading1.Text = "";

            if (dtreportsetting.Rows[0]["PageHeading2"].ToString() != String.Empty)
                txtPageHeading2.Text = dtreportsetting.Rows[0]["PageHeading2"].ToString();
            else
                txtPageHeading2.Text = "";

            if (dtreportsetting.Rows[0]["PageHeading2"].ToString() != String.Empty)
                txtPageHeading3.Text = dtreportsetting.Rows[0]["PageHeading3"].ToString();
            else
                txtPageHeading3.Text = "";

            if (dtreportsetting.Rows[0]["PageHeadingColor"].ToString() != String.Empty)
                txtPageHeadingColor.Text = dtreportsetting.Rows[0]["PageHeadingColor"].ToString();
            else
                txtPageHeadingColor.Text = "";





            if (dtreportsetting.Rows[0]["ConclusionHeading"].ToString() != String.Empty)
                txtConclusionHeading.Text = dtreportsetting.Rows[0]["ConclusionHeading"].ToString();
            else
                txtConclusionHeading.Text = "";



            /*To Show the Image*/
            if (dtreportsetting.Rows[0]["PageLogo"].ToString() != String.Empty)
            {
                hdnImgTopImage.Value = dtreportsetting.Rows[0]["PageLogo"].ToString();
                Session["PageLogo"] = dtreportsetting.Rows[0]["PageLogo"].ToString();
            }
            else
                hdnImgTopImage.Value = "";

            if (hdnImgTopImage.Value != "")
                ImgTopImage.Src = "../../UploadDocs/" + hdnImgTopImage.Value;
            else
                ImgTopImage.Src = "../../UploadDocs/noImage.jpg";


            /*To Show the Front Page Logo 2*/
            if (dtreportsetting.Rows[0]["FrontPageLogo2"].ToString() != String.Empty)
            {
                hdnImgMiddleImage.Value = dtreportsetting.Rows[0]["FrontPageLogo2"].ToString();
                Session["FileName"] = dtreportsetting.Rows[0]["FrontPageLogo2"].ToString();
            }
            else
                hdnImgMiddleImage.Value = "";

            if (hdnImgMiddleImage.Value != "")
                ImgMiddleImage.Src = "../../UploadDocs/" + hdnImgMiddleImage.Value;
            else
                ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";


            
                /*To Show the Front Page Logo 3*/
            if (dtreportsetting.Rows[0]["frontPdfFileName"].ToString() != String.Empty)
            {
                hdnFrontPDF.Value = dtreportsetting.Rows[0]["frontPdfFileName"].ToString();
                Session["frontPdfFileName"] = dtreportsetting.Rows[0]["frontPdfFileName"].ToString();
            }
            else
                hdnFrontPDF.Value = "";



            /*To Show the Front Page Logo 3*/
            if (dtreportsetting.Rows[0]["FrontPageLogo3"].ToString() != String.Empty)
            {
                hdnImgBottomImage.Value = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();
                Session["FrontPageLogo3"] = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();
            }
            else
                hdnImgBottomImage.Value = "";

            if (dtreportsetting.Rows[0]["ScoreTableImage"].ToString() != String.Empty)
            {
                hdnimgScoreTable.Value = dtreportsetting.Rows[0]["ScoreTableImage"].ToString();
                Session["ScoreTableImage"] = dtreportsetting.Rows[0]["ScoreTableImage"].ToString();
            }
            else
                hdnimgScoreTable.Value = "";

            if (hdnimgScoreTable.Value != "")
                imgScoreTable.Src = "../../UploadDocs/" + hdnimgScoreTable.Value;
            else
                imgScoreTable.Src = "../../UploadDocs/noImage.jpg";


            if (dtreportsetting.Rows[0]["FooterImage"].ToString() != String.Empty)
            {
                hdnimgFooter.Value = dtreportsetting.Rows[0]["FooterImage"].ToString();
                Session["FooterImage"] = dtreportsetting.Rows[0]["FooterImage"].ToString();
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


            if (dtreportsetting.Rows[0]["PageHeadingCopyright"].ToString() != String.Empty)
                txtPageCopyright.Text = dtreportsetting.Rows[0]["PageHeadingCopyright"].ToString();
            else
                txtPageCopyright.Text = "";

            if (dtreportsetting.Rows[0]["PageHeadingIntro"].ToString() != String.Empty)
                txtPageIntroduction.Text = dtreportsetting.Rows[0]["PageHeadingIntro"].ToString();
            else
                txtPageIntroduction.Text = "";

            if (dtreportsetting.Rows[0]["PageHeadingConclusion"].ToString() != String.Empty)
                txtPageConclusion.Text = dtreportsetting.Rows[0]["PageHeadingConclusion"].ToString();
            else
                txtPageConclusion.Text = "";

            //////if (dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString() != String.Empty)
            //////    txtConHighLowRange.Text = dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString();
            //////else
            //////    txtConHighLowRange.Text = "";

            if (dtreportsetting.Rows[0]["RadarGraphCategoryCount"].ToString() != String.Empty)
                txtRadarGraphCategoryCount.Text = dtreportsetting.Rows[0]["RadarGraphCategoryCount"].ToString();
            else
                txtRadarGraphCategoryCount.Text = "";

            


            if (dtreportsetting.Rows[0]["CoverPage"].ToString() == "1")
                chkCoverPage.Checked = true;
            else
                chkCoverPage.Checked = false;

            if (dtreportsetting.Rows[0]["ReportIntroduction"].ToString() == "1")
                chkReportIntro.Checked = true;
            else
                chkReportIntro.Checked = false;

            if (dtreportsetting.Rows[0]["ShowScoreRespondents"].ToString() == "True")
                chkShowScoreRespondents.Checked = true;
            else
                chkShowScoreRespondents.Checked = false;

            //////if (dtreportsetting.Rows[0]["QstTextResponses"].ToString() == "1")
            //////    chkCatQstText.Checked = true;
            //////else
            //////    chkCatQstText.Checked = false;

            if (dtreportsetting.Rows[0]["CatQstList"].ToString() == "1")
                chkCatQstlist.Checked = true;
            else
                chkCatQstlist.Checked = false;

            if (dtreportsetting.Rows[0]["CatDataChart"].ToString() == "1")
                chkCatQstChart.Checked = true;
            else
                chkCatQstChart.Checked = false;


            if (dtreportsetting.Rows[0]["Conclusionpage"].ToString() == "1")
                chkConclusion.Checked = true;
            else
                chkConclusion.Checked = false;


            if (chkCatQstChart.Checked == true || chkCatQstlist.Checked == true)
                chkCategoryIntro.Checked = true;
            else
                chkCategoryIntro.Checked = false;

            if (dtreportsetting.Rows[0]["FullProjectGrp"].ToString() == "1")

                chkFullPrjGrp.Checked = true;
            else
                chkFullPrjGrp.Checked = false;


            if (dtreportsetting.Rows[0]["FreeTextResponses"].ToString() == "1")

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


            if (dtreportsetting.Rows[0]["AnalysisI"].ToString() == "1")

                AnalysisI_Chkbox.Checked = true;
            else
                AnalysisI_Chkbox.Checked = false;


            if (dtreportsetting.Rows[0]["AnalysisII"].ToString() == "1")

                AnalysisII_Chkbox.Checked = true;
            else
                AnalysisII_Chkbox.Checked = false;



            if (dtreportsetting.Rows[0]["AnalysisIII"].ToString() == "1")

                AnalysisIII_Chkbox.Checked = true;
            else
                AnalysisIII_Chkbox.Checked = false;

            if (dtreportsetting.Rows[0]["Programme_Average"].ToString() == "1")

                Programme_Avg_Chkbox.Checked = true;
            else
                Programme_Avg_Chkbox.Checked = false;

            if (dtreportsetting.Rows[0]["ShowRadar"] != null && Convert.ToString(dtreportsetting.Rows[0]["ShowRadar"]) == "True")

                chkRadar.Checked = true;
            else
                chkRadar.Checked = false;

            if (dtreportsetting.Rows[0]["ShowTable"] != null && Convert.ToString(dtreportsetting.Rows[0]["ShowTable"]) == "True")

                chkTable.Checked = true;
            else
                chkTable.Checked = false;

            if (dtreportsetting.Rows[0]["ShowPreviousScore1"] != null && Convert.ToString(dtreportsetting.Rows[0]["ShowPreviousScore1"]) == "True")

                chkPrvScore1.Checked = true;
            else
                chkPrvScore1.Checked = false;

            if (dtreportsetting.Rows[0]["ShowPreviousScore2"] != null && Convert.ToString(dtreportsetting.Rows[0]["ShowPreviousScore2"]) == "True")

                chkPrvScore2.Checked = true;
            else
                chkPrvScore2.Checked = false;

            if (dtreportsetting.Rows[0]["ShowBarGraph"] != null  && Convert.ToString(dtreportsetting.Rows[0]["ShowBarGraph"]) == "True")

                chkBarGraph.Checked = true;
            else
                chkBarGraph.Checked = false;

            if (dtreportsetting.Rows[0]["ShowLineChart"] != null && Convert.ToString(dtreportsetting.Rows[0]["ShowLineChart"]) == "True")

                chkLineChart.Checked = true;
            else
                chkLineChart.Checked = false;


            ////if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
            ////    chkPreviousScore.Checked = true;
            ////else
            ////    chkPreviousScore.Checked = false;


            string[] group = Regex.Split(dtreportsetting.Rows[0]["ProjectRelationGrp"].ToString(), ",");
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

    protected static void WriteContentToPdf(FileInfo sourceFile, string heading1, string heading2, string heading3, string htmlcolor, float width, out string outputFile)
    {


        DirectoryInfo di = sourceFile.Directory;
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
                watermark(stamper, layer, rect, heading1, 250, 18, 310, 715);
                watermark(stamper, layer, rect, heading2, 270, 16, 310, 685);
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

    public string ProcessFrontPdf(string flag)
    {
        string frontPDFPath = "";
        if (IsFileValid(pdfFileUpload))
        {
            if (pdfFileUpload.HasFile)
            {
                filename = System.IO.Path.GetFileName(pdfFileUpload.PostedFile.FileName);

                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                pdfFileUpload.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                frontPDFPath = file;

                WriteContentToPdf(new FileInfo(path), txtPageHeading1.Text, txtPageHeading2.Text, txtPageHeading3.Text, txtPageHeadingColor.Text, 450f, out path);

                if (flag == "D")
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

                if (hdnFrontPDF.Value != "")
                {
                    string name = hdnFrontPDF.Value;
                    FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + hdnFrontPDF.Value, FileMode.Open, FileAccess.Read);
                    BinaryReader br1 = new BinaryReader(fs1);
                    Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                    br1.Close();
                    fs1.Close();
                   
                    string path = MapPath("~\\UploadDocs\\") + name;
                    WriteContentToPdf(new FileInfo(path), txtPageHeading1.Text, txtPageHeading2.Text, txtPageHeading3.Text, txtPageHeadingColor.Text, 450f, out path);

                    if (flag == "D")
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

    private static void rectangle(PdfStamper stamper, string color)
    {
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        System.Drawing.Color bckgrndcol = System.Drawing.ColorTranslator.FromHtml(color);
        BaseColor bckgrndco = new BaseColor(bckgrndcol);

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

        cb.SetColorFill(BaseColor.BLACK);
        cb.BeginText();
        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, xAxis, yAxis, 0f);

        cb.EndText();

        cb.EndLayer();
    }
}

