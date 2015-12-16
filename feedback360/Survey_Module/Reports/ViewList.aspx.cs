using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Microsoft.Reporting.WebForms;
using Questionnaire_BAO;
using Questionnaire_BE;
using Admin_BAO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;

public partial class Module_Reports_ViewList : CodeBehindBase
{
    #region Globalvariable
    //Global variable
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
    Survey_Company_BAO companyBusinessAccessObject = new Survey_Company_BAO();
    Survey_Programme_BAO programmeBusinessAccessObject = new Survey_Programme_BAO();
    Survey_AccountUser_BAO accountUserBusinessAccessObject = new Survey_AccountUser_BAO();
    Survey_AssignQstnParticipant_BAO assignquestionnaire = new Survey_AssignQstnParticipant_BAO();
    Survey_ReportManagement_BAO reportManagementBusinessAccessObject = new Survey_ReportManagement_BAO();
    Survey_ReportManagement_BE reportManagement_BE = new Survey_ReportManagement_BE();
    Survey_AssignQstnParticipant_BAO assignQstnParticipantBusinessAccessObject = new Survey_AssignQstnParticipant_BAO();

    DataTable dataTableCompanyName;
    DataTable dataTableGroupList;
    DataTable dataTableSelfName;
    DataTable dataTableReportsID;
    string stringGroupList;
    string stringFrontPage;
    string stringReportIntroduction;
    string stringConclusionPage;
    string stringRadarChart;
    string stringPageHeadingColor;
    string stringDetailedQst;
    string stringCategoryQstlist;
    string stringCategoryBarChart;
    string stringSelfNameGrp;
    string stringProgrammeGrp;
    string stringReportName;

    string stringConclusionHeading;

    string targetradarname = string.Empty;
    string targetradarPreviousScore = string.Empty;
    string targetradarBenchmark = string.Empty;
    string stringConHighLowRange;
    string stringReportType = string.Empty;
    string stringPreScoreVisibility = string.Empty;
    string stringStaticBarLabelVisibility = string.Empty;
    string stringBenchMarkGrpVisibility = string.Empty;
    string stringBenchMarkVisibility = string.Empty;
    string stringBenchConclusionPageVisibility = string.Empty;


    string stringProjectID;
    string stringAccountID;
    string stringProgrammeID;
    string stringAdmin;
    int rptCandidateCount = 0;

    Survey_Category_BAO categoryBusinessAccessObject = new Survey_Category_BAO();
    Survey_Category_BE categoryBusinessEntity = new Survey_Category_BE();

    Int32 pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
    Int32 pageDispCount = Convert.ToInt32(ConfigurationManager.AppSettings["PageDisplayCount"]);

    int reportCount = 0;
    string pageNo = "";

    string selfStatus = "";
    string filename = "";

    //string participantName;
    #endregion

    protected Label Label1;
    protected Label Label2;
    protected Label Label3;
    protected DropDownList ExplodedPointList;
    protected Label Label4;
    protected DropDownList HoleSizeList;

    protected void Page_Load(object sender, EventArgs e)
    {
        identity = this.Page.User.Identity as WADIdentity;
        int? groupID = Identity.User.GroupID;

        //AssignQuestionnaire_BAO survey_chk_user = new AssignQuestionnaire_BAO();
        //DataTable ddd = survey_chk_user.chk_user_authority(grpID, 43);
        //if (Convert.ToInt32(ddd.Rows[0][0]) == 0)
        //{
        //    Response.Redirect("../../UnAuthorized.aspx");
        //}

        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        try
        {
            System.GC.Collect();

            identity = this.Page.User.Identity as WADIdentity;

            //Handle paging in grid
            ManagePaging();

            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Bind account drop down by user account id.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();
                ddlAccountCode.SelectedValue = "0";

                //If user is super Admin then bind reminder grid with account id=1 else user account id.
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

                Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();

                string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();
                string managerRoleId = ConfigurationManager.AppSettings["ManagerRoleID"].ToString();
                //If user group is Participant
                if (identity.User.GroupID == Convert.ToInt32(participantRoleId))
                {
                    //Bind project by user account id.
                    ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ViewState["strAdmin"] = "N";

                    Survey_AssignQuestionnaire_BAO assignQuestionnaireBusinessAccessObject = new Survey_AssignQuestionnaire_BAO();
                    DataTable dtParticipantInfo = new DataTable();
                    //Get all participant in a project .
                    dtParticipantInfo = assignQuestionnaireBusinessAccessObject.GetParticipantAssignmentInfo(Convert.ToInt32(identity.User.UserID));
                    if (dtParticipantInfo.Rows.Count > 0)
                        ddlProject.SelectedValue = dtParticipantInfo.Rows[0]["ProjecctID"].ToString();

                    DataTable dtProgramme = new DataTable();
                    //Get all program in a project and program drop down list.
                    dtProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), 0, 0);

                    if (dtProgramme.Rows.Count > 0)
                    {
                        //Bind program drop down list.
                        ddlProgramme.DataSource = dtProgramme;
                        ddlProgramme.DataTextField = "ProgrammeName";
                        ddlProgramme.DataValueField = "ProgrammeID";
                        ddlProgramme.DataBind();
                        if (dtParticipantInfo.Rows.Count > 0)
                            ddlProgramme.SelectedValue = dtParticipantInfo.Rows[0]["ProgrammeID"].ToString();
                    }
                    //If user group is Participant then project andprogram drop down is disable.
                    ddlProject.Enabled = false;
                    ddlProgramme.Enabled = false;
                }
                else if (identity.User.GroupID == Convert.ToInt32(managerRoleId))//If Manager
                {
                    ViewState["strAdmin"] = "N";

                    DataTable dtManagerProject = new DataTable();
                    //Get all Project by user account id
                    dtManagerProject = projectBusinessAccessObject.GetManagerProject(identity.User.Email, Convert.ToInt32(identity.User.AccountID));

                    if (dtManagerProject.Rows.Count > 0)
                    {
                        //Bind project drop down.
                        ddlProject.DataSource = dtManagerProject;
                        ddlProject.DataValueField = "ProjectID";
                        ddlProject.DataTextField = "Title";
                        ddlProject.DataBind();
                    }

                    DataTable dataTableManagerProgramme = new DataTable();

                    dataTableManagerProgramme = projectBusinessAccessObject.GetManagerProgramme(identity.User.Email, Convert.ToInt32(identity.User.AccountID));
                }
                else
                {
                    //Get all project by user account id and bind account dropdown
                    ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataBind();

                    ViewState["strAdmin"] = "Y";
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #region Radar Chart Method
    /// <summary>
    /// It's of No use
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void Radar(string strTargetPersonID, string strGroupList)
    {

    }

    /// <summary>
    /// It's of No use
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarPreviousScore(string strTargetPersonID, string strGroupList)
    {

    }

    /// <summary>
    /// It's of No use
    /// </summary>
    /// <param name="strTargetPersonID"></param>
    /// <param name="strGroupList"></param>
    public void RadarBenchMark(string strTargetPersonID)
    {

    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion

    #endregion

    #region Image Button Function
    /// <summary>
    /// Generate report .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        string reportfilename = string.Empty;
        string root = Server.MapPath("~") + "\\ReportGenerate\\";
        string rootTemp = Server.MapPath("~") + "\\ReportGenerate\\" + Guid.NewGuid() + "\\";
        Directory.CreateDirectory(rootTemp);
        //rptGenerateLbl.Visible =true;
        //progress_circle.Visible = true;
        if (identity != null)
            stringAccountID = identity.User.AccountID.ToString();
        if (Convert.ToString(ViewState["prjid"]) != string.Empty)
            stringProjectID = Convert.ToString(ViewState["prjid"]);

        if (Convert.ToString(ViewState["prgid"]) != string.Empty)
            stringProgrammeID = Convert.ToString(ViewState["prgid"]);


        if (stringAccountID != null && stringProjectID != null && stringProgrammeID != null)
            reportfilename = btnExport("");
        //Get report file name
        string fName = assignQstnParticipantBusinessAccessObject.GetReportFileName(Convert.ToInt32(stringAccountID), Convert.ToInt32(stringProjectID), Convert.ToInt32(stringProgrammeID));
        fName = reportfilename;
        try
        {


            File.Copy(root + reportfilename, rootTemp + reportfilename);
            if (!string.IsNullOrEmpty(reportfilename) && !string.IsNullOrEmpty(fName))
                fName = ProcessPdfFile(reportfilename, rootTemp, fName);


            if (File.Exists(rootTemp + fName))
            {
                //write page number on footer
                byte[] b2 = WritePageNumber(new FileInfo(rootTemp + fName));
                if (File.Exists(root + fName))
                    File.Delete(root + fName);
                File.WriteAllBytes(root + fName, b2);

                Directory.Delete(rootTemp, true);
            }
            else
                fName = reportfilename;
        }
        catch (Exception ex)
        {
            fName = reportfilename;
            //File.Move(root + reportfilename, root + fName);
            //HandleException(ex);
        }
        //File.Move(rootTemp + fName, root + fName);
        ManagePaging();
        if (fName != "")
        {

            string openpdf = root + fName;

            /*Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment; filename=Survey_" +  ddlProject.SelectedItem.Text + ".pdf");
            Response.ContentType = "application/octet-stream";
            Response.TransmitFile(openpdf);
           // Response.
            Response.Flush();
            Response.Clear();
            Response.Close();*/
            Response.Redirect("download.aspx?filename=" + fName);
        }
        FillGridData();


        //  System.Threading.Thread.Sleep(5000);
    }

    /// <summary>
    /// Insert conclusion and introduction page in pdf.
    /// </summary>
    /// <param name="fName"></param>
    /// <param name="rootTemp"></param>
    /// <returns></returns>
    public string processIntroductionAndConclusion(string fName, string rootTemp)
    {
        //Get Report setting by project ID.
        DataTable dtreportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtreportsetting.Rows.Count > 0)
        {
            string PageHeadingIntro = String.Empty;
            string PageHeadingConclusionText = String.Empty;
            string ConclusionHeading = String.Empty;
            try
            {
                //set introduction text
                PageHeadingIntro = Convert.ToString(dtreportsetting.Rows[0]["PageHeadingIntro"]);
                //Set page heading conclusion text
                PageHeadingConclusionText = Convert.ToString(dtreportsetting.Rows[0]["PageHeadingConclusion"]);
                //Set conclusion text
                ConclusionHeading = Convert.ToString(dtreportsetting.Rows[0]["ConclusionHeading"]);
            }
            catch { }
            string Conclusionpage = "1";
            string ReportIntroduction = "1";// Convert.ToBoolean(dtreportsetting.Rows[0]["ReportIntroduction"]);
            try { Conclusionpage = Convert.ToString(dtreportsetting.Rows[0]["Conclusionpage"]); }
            catch { }
            try { ReportIntroduction = Convert.ToString(dtreportsetting.Rows[0]["ReportIntroduction"]); }
            catch { }


            string programmeTitle = string.Empty;
            string projectTitle = string.Empty;
            string companyTitle = string.Empty;
            try
            {
                DataTable dtProgramme = programmeBusinessAccessObject.GetProgrammeByID(Convert.ToInt32(ddlProgramme.SelectedValue));

                int intCompanyID = Convert.ToInt32(dtProgramme.Rows[0]["CompanyID"]);
                List<Survey_Project_BE> survey_Project_BE = projectBusinessAccessObject.GetProjectByID(Convert.ToInt32(ddlAccountCode.SelectedValue.ToString()), Convert.ToInt32(ddlProject.SelectedValue));
                List<Survey_Company_BE> Survey_Company_BE = companyBusinessAccessObject.GetCompanyByID(intCompanyID);

                if (dtProgramme != null && dtProgramme.Rows.Count > 0)
                {
                    programmeTitle = Convert.ToString(dtProgramme.Rows[0]["ProgrammeName"]);
                }

                if (survey_Project_BE != null && survey_Project_BE.Count > 0)
                {
                    projectTitle = survey_Project_BE.First().Title;
                }
                if (Survey_Company_BE != null && Survey_Company_BE.Count > 0)
                {
                    companyTitle = Survey_Company_BE.First().Title;
                }
            }
            catch { }
            if (ReportIntroduction == "1")
            {
                //Generate introduction dynamically.
                StringBuilder Introduction = new StringBuilder("<div style=\"font-size:18px;font-weight:bold;font-family:arial\">" + programmeTitle + "</div>");
                Introduction.AppendLine("<div style=\"padding-top:10px\"></div>");
                Introduction.AppendLine("<div style=\"border-top:1px solid #000\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:15px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;text-align:center\">Contents</div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;float:left\">Project Name</div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:normal;font-family:arial;color:#000;float:right;width:500px\"><div style=\"text-alighn:left\">" + projectTitle + "</div></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;float:left\">Company Name</div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:normal;font-family:arial;color:#000;float:right;width:500px\"><div style=\"text-alighn:left\">" + companyTitle + "</div></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;float:left\">Programme Name</div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:normal;font-family:arial;color:#000;float:right;width:500px\"><div style=\"text-alighn:left\">" + programmeTitle + "</div></div>");
                Introduction.AppendLine("<div style=\" clear: both;\"></div>");
                Introduction.AppendLine("<div style=\"padding-top:20px\"></div>");
                Introduction.AppendLine("<div style=\"font-size:18px;font-weight:bold;font-family:arial;color:DarkBlue;\">Introduction</div>");
                Introduction.AppendLine("<div style=\"padding-top:25px\"></div><div style=\"font-family:arial;\">");
                Introduction.AppendLine(PageHeadingIntro);
                Introduction.AppendLine("</div>");

                //Convert HTML to pdf
                string introFilePath = CreateReportImage(Introduction.ToString());

                Guid guidIntro = Guid.NewGuid();
                //Include the generated pdf at page number 2.
                IncludePage(fName, rootTemp, introFilePath, guidIntro + ".pdf", 2, "R");
                fName = guidIntro + ".pdf";
            }
            if (Conclusionpage == "1")
            {

                StringBuilder Conclusion = new StringBuilder("<div style=\"font-size:19px;font-weight:bold;font-family:arial\">" + programmeTitle + "</div>");
                Conclusion.AppendLine("<div style=\"padding-top:10px\"></div>");
                Conclusion.AppendLine("<div style=\"border-top:1px solid #000\"></div>");
                Conclusion.AppendLine("<div style=\"padding-top:15px\"></div>");
                Conclusion.AppendLine("<div style=\"font-size:19px;font-weight:bold;font-family:arial;color:DarkBlue;text-align:Left\">" + ConclusionHeading + "</div>");
                Conclusion.AppendLine("<div style=\"padding-top:20px\"></div>");

                Conclusion.AppendLine(PageHeadingConclusionText);
                Conclusion.AppendLine("</div>");
                //Convert HTML to pdf
                string conclusionFilePath = CreateReportImage(Conclusion.ToString());
                Guid guidConlusion = Guid.NewGuid();
                iTextSharp.text.pdf.PdfReader readerMain = new iTextSharp.text.pdf.PdfReader(rootTemp + "\\" + fName);
                // we retrieve the total number of pages
                int nMain = readerMain.NumberOfPages;
                readerMain.Close();
                readerMain = null;
                IncludePage(fName, rootTemp, conclusionFilePath, guidConlusion + ".pdf", nMain, "R");
                //rootPath + sourceFile path
                fName = guidConlusion + ".pdf";
            }

        }
        return fName;
    }

    /// <summary>
    /// It starts a service that convert HTMl to pdf.
    /// </summary>
    /// <param name="HTML"></param>
    /// <returns></returns>
    public string CreateReportImage(String HTML)
    {
        string ReportHtmlPath = Server.MapPath("~") + "\\ReportGenerate";
        //Get service path.
        string HtmlToPdfPathExe = ConfigurationSettings.AppSettings["HtmlToPdfPathExe"];

        Guid TempFolderID = Guid.NewGuid();
        Guid FileName = Guid.NewGuid();
        string tempFolder = ReportHtmlPath + "\\" + TempFolderID;
        if (!Directory.Exists(ReportHtmlPath + "\\" + TempFolderID))
            Directory.CreateDirectory(ReportHtmlPath + "\\" + TempFolderID);
        string FilePath = ReportHtmlPath + "\\" + TempFolderID + "\\" + FileName;


        System.IO.File.WriteAllText(FilePath + ".html", HTML);
        string str_Command = string.Empty;

        string ImageFileName = string.Format(@"{0}.pdf", FileName);
        string Image_File_Path = tempFolder + "\\" + ImageFileName;

        str_Command = "wkhtmltopdf\"    --disable-smart-shrinking  \"" + FilePath + ".html" + "\" \"" + Image_File_Path + "\"";

        try
        {
            //Start service
            ProcessStartInfo procStartInfo = new ProcessStartInfo("\"" + HtmlToPdfPathExe + "\\" + str_Command);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            proc.WaitForExit();
        }
        catch (Exception ex)
        {

        }

        return FilePath + ".pdf";
    }

    /// <summary>
    /// Perform pdf setting
    /// </summary>
    /// <param name="fileName">pdf file name</param>
    /// <param name="root">pdf file path</param>
    /// <param name="finalFileName">final pdf file name </param>
    /// <returns></returns>
    private string ProcessPdfFile(string fileName, string root, string finalFileName)
    {
        try
        {
            string fnameTemp = finalFileName;
            //set File path
            string uploadedFilePath = Server.MapPath("~") + "\\UploadDocs\\";
            string frontPageFilePath = string.Empty;//path of front page pdf which have to be inserted in Main report
            string fileNameWithFront = "F-" + finalFileName;//use to save file name which have front page inserted
            string PageHeading1 = "", PageHeading2 = "", PageHeading3 = "", PageHeadingColor = "", path = "";
            //Getreport setting by report id.
            DataTable dtreportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));

            if (dtreportsetting.Rows.Count > 0)
            {
                //Set report setting , front page image
                frontPageFilePath = Convert.ToString(dtreportsetting.Rows[0]["FrontPdfFileName"]);
                //set Heading one
                PageHeading1 = Convert.ToString(dtreportsetting.Rows[0]["PageHeading1"]);
                //set Heading two
                PageHeading2 = Convert.ToString(dtreportsetting.Rows[0]["PageHeading2"]);
                //set Heading three
                PageHeading3 = Convert.ToString(dtreportsetting.Rows[0]["PageHeading3"]);
                //set Heading color
                PageHeadingColor = Convert.ToString(dtreportsetting.Rows[0]["PageHeadingColor"]);
            }

            // --> 1.0.0.1 [Replacing the tokens with value]
            DataTable dtProgramme = programmeBusinessAccessObject.GetProgrammeByID(Convert.ToInt32(ddlProgramme.SelectedValue));
            int intCompanyID = Convert.ToInt32(dtProgramme.Rows[0]["CompanyID"]);
            //List<Survey_Project_BE> survey_Project_BE = project_BAO.GetProjectByID(Convert.ToInt32(ddlAccountCode.SelectedValue.ToString()), Convert.ToInt32(ddlProject.SelectedValue));
            List<Survey_Company_BE> Survey_Company_BE = companyBusinessAccessObject.GetCompanyByID(intCompanyID);

            if (dtProgramme != null && dtProgramme.Rows.Count > 0)
            {
                PageHeading3 = PageHeading3.Replace("[CLOSEDATE]", string.Format("{0:dd MMM yyyy}", dtProgramme.Rows[0]["EndDate"]));
            }
            if (Survey_Company_BE != null)
                PageHeading2 = PageHeading2.Replace("[COMPANYNAME]", Survey_Company_BE[0].Title);

            // 1.0.0.1 [Replacing the tokens with value] <--



            //insertIntroduction and ConclusionPage
            fileName = processIntroductionAndConclusion(fileName, root);


            //Insert Front Page
            if (!string.IsNullOrEmpty(frontPageFilePath))
            {
                try
                {
                    WriteContentToPdf(new FileInfo(uploadedFilePath + frontPageFilePath), PageHeading1, PageHeading2, PageHeading3, PageHeadingColor, 450f, out path);
                    IncludePage(fileName, root, path, fileNameWithFront, 1, "R");
                }
                catch (Exception ex)
                {

                }
            }

            Survey_Category_BAO objSurvey_Category_BAO = new Survey_Category_BAO();
            DataTable dtCategory = objSurvey_Category_BAO.GetdtnewCategoryList(ddlAccountCode.SelectedValue + " and [Survey_Project].ProjectID =" + ddlProject.SelectedValue);
            if (!string.IsNullOrEmpty(frontPageFilePath))
                finalFileName = !string.IsNullOrEmpty(fileNameWithFront) ? fileNameWithFront : fileName;
            foreach (DataRow item in dtCategory.Rows)
            {
                string categoryPageFileName = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(item["CategoryTitle"])))
                    categoryPageFileName = Convert.ToString(item["CategoryTitle"]);

                string catPageFilePath = uploadedFilePath + Convert.ToString(item["IntroPdfFileName"]);//category pdf to be insert
                //string pdf_donotdelete = root + "..\\pdf-donotdelete.pdf";
                if (!string.IsNullOrEmpty(Convert.ToString(item["IntroPdfFileName"])))
                {
                    if (File.Exists(catPageFilePath))
                    {
                        List<int> lstPageNo = ReadPdfFile(root + finalFileName, "@@" + categoryPageFileName + "@@");

                        if (lstPageNo.Any())
                        {
                            IncludePage(finalFileName, root, catPageFilePath, categoryPageFileName + ".pdf", lstPageNo.FirstOrDefault() + 1, "I");
                            finalFileName = categoryPageFileName + ".pdf";

                            if (File.Exists(root + fnameTemp))
                                File.Delete(root + fnameTemp);
                            File.Move(root + finalFileName, root + fnameTemp);
                            finalFileName = fnameTemp;
                        }
                    }

                }

            }

            return finalFileName;
        }
        catch (Exception ex)
        {
            return finalFileName;
            // throw ex;
        }


        //List<int> x = ReadPdfFile(rootPath + "\\" + "PeterHart_106222.pdf", "The pay and benefits I receive fairly reflect the work I do");
    }

    /// <summary>
    /// Reset control with default value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ResetControls();
    }

    #endregion

    #region dropdown event
    /// <summary>
    /// Fill project an company details by account .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetControls();

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO account_BAO = new Account_BAO();
            //Get account conapmy details.
            dataTableCompanyName = account_BAO.GetdtAccountList(Convert.ToString(companycode));

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);
            //set company name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            if (ddlAccountCode.SelectedIndex > 0)
            { //Get all project in a Account.
                DataTable dataTableProjectlist = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

                if (dataTableProjectlist.Rows.Count > 0)
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                    //Bind project dropdown
                    ddlProject.DataSource = dataTableProjectlist;
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataBind();

                    ddlProgramme.SelectedValue = "0";
                }
                else
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                }
            }
            ViewState["accid"] = ddlAccountCode.SelectedValue.ToString();
        }
    }

    /// <summary>
    /// Fill Program in a project .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //Get all program in a project.
        dataTableProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue), 0, 0);

        if (dataTableProgramme.Rows.Count > 0)
        {
            //Bind program drop down.
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

        if (ddlProgramme.Items.Count > 1)
            ddlProgramme.Items[1].Selected = true;


        ViewState["prjid"] = ddlProject.SelectedValue.ToString();
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }

    /// <summary>
    /// save program value to viewstate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["prgid"] = ddlProgramme.SelectedValue.ToString();
    }

    #endregion

    #region ReportMethods
    /// <summary>
    /// It is of no use.
    /// </summary>
    /// <param name="targetid"></param>
    protected void GetDetailFromTargetPersonID(string targetid)
    {

    }
    /// <summary>
    /// Initilize parameter for report controls.
    /// </summary>
    /// <param name="projectid"></param>
    protected void ControlToParameter(string projectid)
    {
        if (projectid != null)
        {
            DataTable dataTableReportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));

            if (dataTableReportsetting != null && dataTableReportsetting.Rows.Count > 0)
            {
                // This parameter will Decide: which type of Report will Call  

                if (dataTableReportsetting.Rows[0]["PageHeadingColor"].ToString() != string.Empty)
                    stringPageHeadingColor = dataTableReportsetting.Rows[0]["PageHeadingColor"].ToString();

                if (dataTableReportsetting.Rows[0]["ReportType"].ToString() != string.Empty)
                    stringReportType = dataTableReportsetting.Rows[0]["ReportType"].ToString();

                if (dataTableReportsetting.Rows[0]["CoverPage"].ToString() != string.Empty)
                    stringFrontPage = dataTableReportsetting.Rows[0]["CoverPage"].ToString();

                if (dataTableReportsetting.Rows[0]["ReportIntroduction"].ToString() != string.Empty)
                    stringReportIntroduction = dataTableReportsetting.Rows[0]["ReportIntroduction"].ToString();

                if (dataTableReportsetting.Rows[0]["Conclusionpage"].ToString() != string.Empty)
                    stringConclusionPage = dataTableReportsetting.Rows[0]["Conclusionpage"].ToString();

                if (dataTableReportsetting.Rows[0]["ConclusionHeading"].ToString() != string.Empty)
                    stringConclusionHeading = dataTableReportsetting.Rows[0]["ConclusionHeading"].ToString();


                if (dataTableReportsetting.Rows[0]["CatQstList"].ToString() != string.Empty)
                    stringCategoryQstlist = dataTableReportsetting.Rows[0]["CatQstList"].ToString();

                if (dataTableReportsetting.Rows[0]["CatDataChart"].ToString() != string.Empty)
                    stringCategoryBarChart = dataTableReportsetting.Rows[0]["CatDataChart"].ToString();
            }
        }
    }

    /// <summary>
    /// Export pdf report.
    /// </summary>
    /// <param name="dirName"></param>
    protected string btnExport(string dirName)
    {
        try
        {
            rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
            string[] streamids;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string root = string.Empty;
            root = Server.MapPath("~") + "\\ReportGenerate\\";

            /* Function : For Filling Paramters From Controls */
            ControlToParameter(stringProjectID);

            if (ddlAccountCode.SelectedValue != string.Empty)
                stringStaticBarLabelVisibility = ddlAccountCode.SelectedItem.ToString();
            else
                stringStaticBarLabelVisibility = "";

            //If strReportType = 1 Then FeedbackReport will Call
            //If strReportType = 2 Then FeedbackReportClient1 will Call (In this Report We are Showing only Range & Text Type Question).
            if (stringReportType == "1")
            {
                DataTable dtreportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(stringProjectID));
                if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
                {
                    /*
                     * Drawing Radarchat By MSCHartControl then Exporting Image(.png) in ReportGenerate
                     * & Making Entry in Table with Radarchatname
                     * & Calling in RDL (RadarImage)
                     */
                }
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReport";
                rview.ServerReport.ReportPath = "/Feedback360/FeedbackReport";
                //rview.ServerReport.ReportPath = "/Feedback360/FeedbackReport";

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", stringFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", stringConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", stringDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", stringCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", stringCategoryBarChart));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", stringProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", stringReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", stringStaticBarLabelVisibility));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (stringReportType == "2")
            {
                // rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient1";
                rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient1";
                //rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient1";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", "1"));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", "1"));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", "1"));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", "1"));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (stringReportType == "3")
            {
                rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient2";
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient2";
                //rview.ServerReport.ReportPath = "/Feedback360/FeedbackReportClient2";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", stringFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", stringConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", stringProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", stringReportIntroduction));

                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (stringReportType == "4") // Old Mutual Report
            {
                // rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
                rview.ServerReport.ReportPath = "/Feedback360/CurFeedbackReport";
                //rview.ServerReport.ReportPath = "/Feedback360/CurFeedbackReport";

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", stringFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", stringConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("DetailedQstVisibility", stringDetailedQst));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", stringCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", stringCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProgrammeVisibility", stringProgrammeGrp));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", stringReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("BarLabelVisibility", stringStaticBarLabelVisibility));
                rview.ServerReport.SetParameters(paramList);
            }
            else if (stringReportType == "5") // Old Mutual Report
            {
                rview.ServerReport.ReportPath = "/Survey_Prod/Srvey_FinalReport";

                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("FrontPageVisibility", stringFrontPage));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportIntroduction", stringReportIntroduction));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionVisibility", stringConclusionPage));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryQstlistVisibility", stringCategoryQstlist));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("CategoryBarChartVisibility", stringCategoryBarChart));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("PageHeadingColor", stringPageHeadingColor));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ConclusionHeading", stringConclusionHeading));

                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("accountid", ViewState["accid"].ToString()));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("projectid", ViewState["prjid"].ToString()));
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("programmeid", ViewState["prgid"].ToString()));

                rview.ServerReport.SetParameters(paramList);
            }

            rview.Visible = false;

            byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            string funiqueId = Convert.ToString(Guid.NewGuid());

            string PDF_path = root + "Survey_" + ddlAccountCode.SelectedItem.Value + ddlProject.SelectedItem.Value + ddlProgramme.SelectedItem.Value + "-" + funiqueId + ".pdf";

            FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create, FileAccess.ReadWrite);
            objFs.Write(bytes, 0, bytes.Length);
            objFs.Close();
            objFs.Dispose();

            bytes = null;
            System.GC.Collect();
            rview.Dispose();

            return "Survey_" + ddlAccountCode.SelectedItem.Value + ddlProject.SelectedItem.Value + ddlProgramme.SelectedItem.Value + "-" + funiqueId + ".pdf";
        }
        catch (Exception ex)
        {
            if (((ReportServerException)(ex)).ErrorCode == "rsExecutionNotFound")
            {
                Response.Redirect("~/Login.aspx");
            }

            HandleException(ex);
            return "";
        }
    }
    #endregion

    /// <summary>
    /// It is of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string participantid = string.Empty;

            string root = Server.MapPath("~") + "\\ReportGenerate\\";
            string newDir = ddlAccountCode.SelectedItem.Text + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");

            DirectoryInfo directoryInformation = new DirectoryInfo(root);
            directoryInformation.CreateSubdirectory(newDir);
        }
        catch (Exception ex)
        { }
    }

    #region Gridview Paging Related Methods
    /// <summary>
    /// This method set user identity value
    /// </summary>
    protected void ManagePaging()
    {
        identity = this.Page.User.Identity as WADIdentity;
    }

    /// <summary>
    /// Save view state of the page
    /// </summary>
    /// <returns></returns>
    protected override object SaveViewState()
    {
        object baseState = base.SaveViewState();
        return new object[] { baseState, reportCount };
    }

    /// <summary>
    ///  Reload the viewsate when view sate of the page expires.
    /// </summary>
    /// <param name="savedState"></param>
    protected override void LoadViewState(object savedState)
    {
        object[] myState = (object[])savedState;

        if (myState[0] != null)
            base.LoadViewState(myState[0]);

        if (myState[1] != null)
        {
            ManagePaging();
        }

    }

    /// <summary>
    /// Manage gridview paging when click on next button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objLb_Click(object sender, EventArgs e)
    {
        //  plcPaging.Controls.Clear();
        LinkButton objlb = (LinkButton)sender;
        ManagePaging();
    }

    /// <summary>
    /// It isof no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void objIbtnGo_Click(object sender, ImageClickEventArgs e)
    {
    }

    #endregion

    #region Grid Method
    /// <summary>
    /// It is of No use
    /// </summary>
    public void FillGridData()
    {
    }

    /// <summary>
    /// Reset controls value to default.
    /// </summary>
    protected void ResetControls()
    {
        try
        {
            string participantRoleId = ConfigurationManager.AppSettings["ParticipantRoleID"].ToString();

            if (identity.User.GroupID != Convert.ToInt32(participantRoleId))
            {
                ddlProject.SelectedValue = "0";
                ddlProgramme.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    #endregion

    #region ZipCode
    /// <summary>
    /// Create a Zip file for download.
    /// </summary>
    /// <param name="inputFolderPath"></param>
    /// <param name="outputPathAndFile"></param>
    /// <param name="password"></param>
    public static void ZipFiles(string inputFolderPath, string outputPathAndFile, string password)
    {
        ArrayList ar = GenerateFileList(inputFolderPath); // generate file list
        int TrimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;
        // find number of chars to remove     // from orginal file path
        TrimLength += 1; //remove '\'
        FileStream ostream;
        byte[] obuffer;
        string outPath = inputFolderPath + @"\" + outputPathAndFile;
        ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)); // create zip stream

        if (password != null && password != String.Empty)
            oZipStream.Password = password;

        oZipStream.SetLevel(9); // maximum compression
        ZipEntry oZipEntry;

        foreach (string Fil in ar) // for each file, generate a zipentry
        {
            oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
            oZipStream.PutNextEntry(oZipEntry);

            if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
            {
                ostream = File.OpenRead(Fil);
                obuffer = new byte[ostream.Length];
                ostream.Read(obuffer, 0, obuffer.Length);
                oZipStream.Write(obuffer, 0, obuffer.Length);
                ostream.Close();
            }
        }

        oZipStream.Finish();
        oZipStream.Close();
    }

    /// <summary>
    /// Add pdf files to directory and return file list in a directory.
    /// </summary>
    /// <param name="Dir"></param>
    /// <returns></returns>
    private static ArrayList GenerateFileList(string Dir)
    {
        ArrayList fils = new ArrayList();
        bool Empty = true;

        foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
        {
            if (Path.GetExtension(file) == ".pdf")//|| Path.GetExtension(file) == ".xml")
            {
                fils.Add(file);
            }

            Empty = false;
        }

        if (Empty)
        {
            if (Directory.GetDirectories(Dir).Length == 0)
            // if directory is completely empty, add it
            {
                fils.Add(Dir + @"/");
            }
        }

        return fils; // return file list
    }

    /// <summary>
    /// Unzip the  zip file
    /// </summary>
    /// <param name="zipPathAndFile"></param>
    /// <param name="outputFolder"></param>
    /// <param name="password"></param>
    /// <param name="deleteZipFile"></param>
    public static void UnZipFiles(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile)
    {
        ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipPathAndFile));
        if (password != null && password != String.Empty)
            zipStream.Password = password;
        ZipEntry theEntry;

        string tmpEntry = String.Empty;

        while ((theEntry = zipStream.GetNextEntry()) != null)
        {
            string directoryName = outputFolder;
            string fileName = Path.GetFileName(theEntry.Name);
            // create directory 
            if (directoryName != "")
            {
                Directory.CreateDirectory(directoryName);
            }

            if (fileName != String.Empty)
            {
                if (theEntry.Name.IndexOf(".ini") < 0)
                {
                    string fullPath = directoryName + "\\" + theEntry.Name;
                    fullPath = fullPath.Replace("\\ ", "\\");
                    string fullDirPath = Path.GetDirectoryName(fullPath);

                    if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                    FileStream streamWriter = File.Create(fullPath);

                    int size = 2048;
                    byte[] data = new byte[2048];

                    while (true)
                    {
                        size = zipStream.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                }
            }
        }

        zipStream.Close();

        if (deleteZipFile)
            File.Delete(zipPathAndFile);
    }

    #endregion

    #region PdfUtilityFunction
    /// <summary>
    /// Insert new pages to an existing pdf file
    /// </summary>
    /// <param name="sourcePdf">The full path to the source pdf</param>
    /// <param name="pagesToInsert">The dictionary contains the pages to be inserted in the source pdf. The key is the page number to be inserted. The value is the PdfImportedPage to insert</param>
    /// <param name="outPdf">The full path of the resulting output pdf file</param>
    /// <returns>True if the operation succeeded. False otherwise.</returns>
    /// <remarks>To create the pagesToInsert dictionary, you can use the iTextSharp.text.pdf.PdfCopy class to open
    /// an existing pdf file and call the GetImportedPage method</remarks>
    public static bool InsertorReplacePages(string sourcePdf, Dictionary<int, iTextSharp.text.pdf.PdfImportedPage> pagesToInsert, string outPdf, int PageNUmber, string flag)
    {
        bool result = false;
        iTextSharp.text.pdf.PdfReader reader = null;
        iTextSharp.text.Document doc = null;
        iTextSharp.text.pdf.PdfCopy copier = null;

        try
        {
            //int j = PageNUmber;
            reader = new iTextSharp.text.pdf.PdfReader(sourcePdf);
            doc = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
            copier = new iTextSharp.text.pdf.PdfCopy(doc, new System.IO.FileStream(outPdf, System.IO.FileMode.Create));
            doc.Open();

            int i = 1;

            for (; i <= reader.NumberOfPages; )
            {
                if (i == PageNUmber)
                {
                    if (flag != "D")
                    {
                        for (int j = 1; j <= pagesToInsert.Count; j++)
                        {
                            copier.AddPage(pagesToInsert[j]);
                        }

                        PageNUmber--;
                        if (flag == "R")
                            i++;
                    }
                }
                else
                {
                    copier.AddPage(copier.GetImportedPage(reader, i));
                    i++;
                }
            }

            if (i == PageNUmber)
            {
                for (int j = 1; j <= pagesToInsert.Count; j++)
                {
                    copier.AddPage(pagesToInsert[j]);
                }
                PageNUmber--;
            }

            doc.Close();
            reader.Close();
            result = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    /// <summary>
    /// Use to find page number based on search text in pdf
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="searthText"></param>
    /// <returns></returns>
    public List<int> ReadPdfFile(string fileName, String searthText)
    {
        List<int> pages = new List<int>();

        if (File.Exists(fileName))
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

                string currentPageText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                if (currentPageText.Contains(searthText))
                {
                    pages.Add(page);
                }
            }

            pdfReader.Close();
        }
        return pages;
    }

    /// <summary>
    /// Include pdf page in paticular page number. 
    /// </summary>
    /// <param name="sourceFile">source pdf file</param>
    /// <param name="rootPath"> path of mail pdf</param>
    /// <param name="insertPageFilePath">path of pdf which is to be inserted</param>
    /// <param name="OutputFileName">Resultant file name</param>
    /// <param name="pageNumber">page number where pdf is to be inserted</param>
    /// <param name="flag"></param>
    /// <returns></returns>
    protected string IncludePage(string sourceFile, string rootPath, string insertPageFilePath, string OutputFileName, int pageNumber, string flag)
    {
        try
        {
            //String ReportHtml = ConfigurationManager.AppSettings["ReportHtml"].ToString();
            //String ReportName = Request.QueryString["ReportName"].ToString();
            if (flag != "D")
            {
                iTextSharp.text.Document document = null;
                iTextSharp.text.pdf.PdfCopy writer = null;

                // we create a reader for a certain document
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(insertPageFilePath);
                // we retrieve the total number of pages
                int n = reader.NumberOfPages;
                // step 1: creation of a document-object
                document = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
                // step 2: we create a writer that listens to the document

                FileStream fs = new FileStream(rootPath + Guid.NewGuid() + ".pdf", FileMode.CreateNew, FileAccess.Write);
                System.IO.Stream strm = fs;
                writer = new iTextSharp.text.pdf.PdfCopy(document, strm);

                // step 3: we open the document
                document.Open();
                Dictionary<int, iTextSharp.text.pdf.PdfImportedPage> pagesToInsert = new Dictionary<int, iTextSharp.text.pdf.PdfImportedPage>();
                for (int i = 1; i <= n; i++)
                {
                    iTextSharp.text.pdf.PdfImportedPage page;
                    page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                    //insert page and  page number in list
                    pagesToInsert.Add(i, page);
                }
                //Insert page to existing pdf.
                bool status = InsertorReplacePages(rootPath + sourceFile, pagesToInsert, rootPath + OutputFileName, pageNumber, flag);

                document.Close();
                writer.Close();
                reader.Close();
            }
            else
            {
                Dictionary<int, iTextSharp.text.pdf.PdfImportedPage> pagesToInsert = new Dictionary<int, iTextSharp.text.pdf.PdfImportedPage>();
                bool status = InsertorReplacePages(rootPath + sourceFile, pagesToInsert, rootPath + OutputFileName, pageNumber, flag);
            }
            return "";
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {

        }
    }

    /// <summary>
    /// Write page number to the pdf file
    /// </summary>
    /// <param name="sourceFile"></param>
    /// <returns></returns>
    protected static byte[] WritePageNumber(FileInfo sourceFile)
    {
        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(sourceFile.FullName);

        using (MemoryStream memoryStream = new MemoryStream())
        {
            iTextSharp.text.pdf.PdfStamper pdfStamper = new iTextSharp.text.pdf.PdfStamper(reader, memoryStream);

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                iTextSharp.text.pdf.PdfContentByte pdfPageContents = pdfStamper.GetUnderContent(i);
                pdfPageContents.BeginText();
                iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, System.Text.Encoding.ASCII.EncodingName, false);
                pdfPageContents.SetFontAndSize(baseFont, 9);

                pdfPageContents.SetRGBColorFill(0, 0, 0);

                int pageNumber = i;

                pdfPageContents.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_RIGHT, "Page " + pageNumber.ToString() + " of " + reader.NumberOfPages, pageSize.Width - 50, 20,


                                                  0);

                pdfPageContents.EndText(); // Done working with text
            }

            pdfStamper.FormFlattening = true; // enable this if you want the PDF flattened. 
            pdfStamper.Close(); // Always close the stamper or you'll have a 0 byte stream. 
            return memoryStream.ToArray();
        }
    }
    #endregion

    /// <summary>
    /// Wrie content to pdf.
    /// </summary>
    /// <param name="sourceFile">source file name</param>
    /// <param name="heading1">Heading one</param>
    /// <param name="heading2">Heading two</param>
    /// <param name="heading3">Heading three</param>
    /// <param name="htmlcolor">Heading color</param>
    /// <param name="width"></param>
    /// <param name="outputFile"></param>
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
                iTextSharp.text.Rectangle rect = reader1.GetPageSize(1);

                iTextSharp.text.Rectangle pageRectangle = reader1.GetPageSizeWithRotation(1);
                //Write heading one
                watermark(stamper, layer, pageRectangle, heading1, 250, 18, 303, 715);
                //Write heading two
                watermark(stamper, layer, pageRectangle, heading2, 270, 16, 310, 685);
                //Write heading three
                watermark(stamper, layer, pageRectangle, heading3, 290, 14, 310, 658);

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
    /// It is of No use.
    /// </summary>
    /// <param name="stamper"></param>
    /// <param name="color"></param>
    private static void rectangle(PdfStamper stamper, string color)
    {
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        System.Drawing.Color bckgrndcol = System.Drawing.ColorTranslator.FromHtml(color);
        BaseColor bckgrndco = new BaseColor(bckgrndcol);

        PdfContentByte cb = stamper.GetOverContent(1);
        PdfGState gState = new PdfGState();
        gState.FillOpacity = 0.85f;
        cb.SetGState(gState);

        iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(83f, 750f, 555f, 650f);
        rectangle.BorderWidth = 1f;
        rectangle.Border = 15;
        rectangle.BorderColor = BaseColor.BLACK;
        rectangle.BackgroundColor = bckgrndco;

        cb.Rectangle(rectangle);
    }

    /// <summary>
    /// Write content to pdf.
    /// </summary>
    /// <param name="stamper">pdfstamper object</param>
    /// <param name="layer">pdflayer object</param>
    /// <param name="rect">rectangle object</param>
    /// <param name="text">text to write</param>
    /// <param name="location">location where to wrie</param>
    /// <param name="fontsize">what is the font size</param>
    /// <param name="xAxis">xaxis position</param>
    /// <param name="yAxis">yaxis position</param>
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

        var ps = rect; /*dc.PdfDocument.PageSize is not always correct*/
        var x = (ps.Right + ps.Left) / 2;
        var y = (ps.Bottom + ps.Top) / 2;

        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, x, yAxis, 0f);

        cb.EndText();

        cb.EndLayer();
    }
}
