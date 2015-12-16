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

public partial class Module_Reports_ReportManagement : CodeBehindBase
{
    #region Global Variable
    //Global Variables
    string LogFilePath = string.Empty;
    string mimeType;
    string encoding;
    string fileNameExtension;
    string extension, deviceInfo, outputFileName = "";
    string[] streams;
    string defaultFileName = string.Empty;
    Warning[] warnings;
    WADIdentity identity;
    Project_BAO projectBusinessAccessObject = new Project_BAO();
    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
    AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
    AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
    ReportManagement_BAO reportManagementBusinessAccessObject = new ReportManagement_BAO();
    ReportManagement_BE reportManagementBusinessEntity = new ReportManagement_BE();

    DataTable dataTableCompanyName;
    DataTable dataTableGroupList;
    DataTable dataTableSelfName;
    DataTable dataTableReportsID;
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

    protected void Page_Load(object sender, EventArgs e)
    {

        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        //strTargetPersonID = "304";//"298";
        //strAccountID = "29";
        //strProjectID = "178";               
        identity = this.Page.User.Identity as WADIdentity;

        if (!IsPostBack)
        {
            //If user is Super Admin then show account section else hide.
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
            //Get all account list in a user account and bind account drop down.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();
            ddlAccountCode.SelectedValue = "0";

            //if (identity.User.GroupID == 1)
            //{
            Project_BAO projectBusinessAccessObject = new Project_BAO();
            //Get all project in an user account.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(identity.User.AccountID));
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();
            //}            
            //GroupCheckBoxListBind();
        }

        lblSelfNameGrp.Text = "Self";

        if (ddlProject.SelectedItem.Text.Trim() != "Select")
        {
            //reportselection.Visible = true;
            //divReportSettings.Visible = true;
        }
        else
        {
            //reportselection.Visible = false;
            //divReportSettings.Visible = false;
        }

    }

    #region Image Button Function
    /// <summary>
    /// Save data to database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        if (IsFileValid(fuplTopImage) && IsFileValid(this.fuplMiddleImage) && IsFileValid(this.fuplBottomImage) && IsFileValid(this.FileUploadRightImage))
        {
            /*
             * Here We are Deleting First the existing Record in Table & then New Insertion will be process 
             */
            string lastLogo = "";
            string frontPageLogo2 = "";
            string frontPageLogo3 = "";
            string FrontPageLogo4 = "";
            //Get all project setting in a project.
            DataTable dtreportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));
            //set the page logo.
            if (dtreportsetting.Rows.Count > 0)
                lastLogo = dtreportsetting.Rows[0]["PageLogo"].ToString();
            //set the front logo 2.
            if (dtreportsetting.Rows.Count > 0)
                frontPageLogo2 = dtreportsetting.Rows[0]["FrontPageLogo2"].ToString();
            //set the front logo 3.
            if (dtreportsetting.Rows.Count > 0)
                frontPageLogo3 = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();
            //set the front logo 4.
            if (dtreportsetting.Rows.Count > 0)
                FrontPageLogo4 = dtreportsetting.Rows[0]["FrontPageLogo4"].ToString();

            int result = reportManagementBusinessAccessObject.DeleteProjectSettingReport(Convert.ToInt32(ddlProject.SelectedValue));

            /*
             * New Insertion Strart ,set controls value.
             */
            reportManagementBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            reportManagementBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

            reportManagementBusinessEntity.ReportType = ddlReportType.SelectedValue;
            reportManagementBusinessEntity.PageHeading1 = txtPageHeading1.Text.Trim();
            reportManagementBusinessEntity.PageHeading2 = txtPageHeading2.Text.Trim();
            reportManagementBusinessEntity.PageHeading3 = txtPageHeading3.Text.Trim();
            reportManagementBusinessEntity.PageHeadingColor = txtPageHeadingColor.Text.Trim();
            reportManagementBusinessEntity.PageHeadingCopyright = txtPageCopyright.Text.Trim();
            reportManagementBusinessEntity.PageHeadingIntro = txtPageIntroduction.Value.Trim();
            reportManagementBusinessEntity.PageHeadingConclusion = Server.HtmlDecode(txtPageConclusion.Value.Trim());
            reportManagementBusinessEntity.ConclusionHeading = Server.HtmlDecode(txtConclusionHeading.Text.Trim());

            //If Admin does't specify the value for Scroes Range then "2" will be insert Default
            if (txtConHighLowRange.Text.Trim() != string.Empty)
                reportManagementBusinessEntity.ConclusionHighLowRange = txtConHighLowRange.Text.Trim();
            else
                reportManagementBusinessEntity.ConclusionHighLowRange = "2";

            //Hide show report control setting , if checked then 1 else 0 will be inserted.
            if (chkCoverPage.Checked == true)
                reportManagementBusinessEntity.CoverPage = "1";
            else
                reportManagementBusinessEntity.CoverPage = "0";

            if (chkReportIntro.Checked == true)
                reportManagementBusinessEntity.ReportIntroduction = "1";
            else
                reportManagementBusinessEntity.ReportIntroduction = "0";

            if (chkBenchConclusionPage.Checked == true)
                reportManagementBusinessEntity.BenchConclusionpage = "1";
            else
                reportManagementBusinessEntity.BenchConclusionpage = "0";


            if (chkConclusion.Checked == true)
                reportManagementBusinessEntity.Conclusionpage = "1";
            else
                reportManagementBusinessEntity.Conclusionpage = "0";

            if (chkPreviousScore.Checked == true)
                reportManagementBusinessEntity.PreviousScoreVisible = "1";
            else
                reportManagementBusinessEntity.PreviousScoreVisible = "0";

            if (chkBenchMark.Checked == true)
                reportManagementBusinessEntity.BenchMarkScoreVisible = "1";
            else
                reportManagementBusinessEntity.BenchMarkScoreVisible = "0";

            if (chkBenchMarkGrp.Checked == true)
                reportManagementBusinessEntity.BenchMarkGrpVisible = "1";
            else
                reportManagementBusinessEntity.BenchMarkGrpVisible = "0";


            if (chkRadarChart.Checked == true)
                reportManagementBusinessEntity.RadarChart = "1";
            else
                reportManagementBusinessEntity.RadarChart = "0";

            if (chkCatQstText.Checked == true)
                reportManagementBusinessEntity.QstTextResponses = "1";
            else
                reportManagementBusinessEntity.QstTextResponses = "0";

            if (chkCatQstlist.Checked == true)
                reportManagementBusinessEntity.CatQstList = "1";
            else
                reportManagementBusinessEntity.CatQstList = "0";

            if (chkCatQstChart.Checked == true)
                reportManagementBusinessEntity.CatDataChart = "1";
            else
                reportManagementBusinessEntity.CatDataChart = "0";

            if (chkSelfNameGrp.Checked == true)
                reportManagementBusinessEntity.CandidateSelfStatus = "1";
            else
                reportManagementBusinessEntity.CandidateSelfStatus = "0";

            RetrieveCheckBoxValue();

            if (chkFullPrjGrp.Checked == true)
                reportManagementBusinessEntity.FullProjectGrp = "1";
            else
                reportManagementBusinessEntity.FullProjectGrp = "0";

            if (chkProgrammeGrp.Checked == true)
                reportManagementBusinessEntity.ProgrammeGrp = "1";
            else
                reportManagementBusinessEntity.ProgrammeGrp = "0";

            if (chkPreviousScore.Checked == true)
                reportManagementBusinessEntity.PreviousScoreVisible = "1";
            else
                reportManagementBusinessEntity.PreviousScoreVisible = "0";

            //Upload top image.
            if (fuplTopImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuplTopImage.PostedFile.FileName);
                //Get unique file name.
                file = GetUniqueFilename(filename);
                //Set file folder path 
                string path = MapPath("~\\UploadDocs\\") + file;
                fuplTopImage.SaveAs(path);
                string name = file;
                FileStream topImageFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader topImageBinaryReader = new BinaryReader(topImageFileStream);
                Byte[] docbytes = topImageBinaryReader.ReadBytes((Int32)topImageFileStream.Length);
                topImageBinaryReader.Close();
                topImageFileStream.Close();
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

            //Upload middle image.
            if (fuplMiddleImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuplMiddleImage.PostedFile.FileName);
                //Get unique file name.
                file = GetUniqueFilename(filename);
                //Set file folder path 
                string path = MapPath("~\\UploadDocs\\") + file;
                fuplMiddleImage.SaveAs(path);

                string name = file;
                FileStream middleImageFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader middleImageBinaryReader = new BinaryReader(middleImageFileStream);
                Byte[] docbytes = middleImageBinaryReader.ReadBytes((Int32)middleImageFileStream.Length);
                middleImageBinaryReader.Close();
                middleImageFileStream.Close();
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
            }
            //Upload bottom image.
            if (fuplBottomImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(fuplBottomImage.PostedFile.FileName);
                //Get unique file name.
                file = GetUniqueFilename(filename);
                //Set file folder path. 
                string path = MapPath("~\\UploadDocs\\") + file;
                fuplBottomImage.SaveAs(path);

                string name = file;
                FileStream bottomImageFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader bottomImageBinaryStream = new BinaryReader(bottomImageFileStream);
                Byte[] docbytes = bottomImageBinaryStream.ReadBytes((Int32)bottomImageFileStream.Length);
                bottomImageBinaryStream.Close();
                bottomImageFileStream.Close();
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
            }

            //Upload right image.
            if (FileUploadRightImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(FileUploadRightImage.PostedFile.FileName);
                //Get unique file name.
                file = GetUniqueFilename(filename);
                //Set file folder path. 
                string path = MapPath("~\\UploadDocs\\") + file;
                FileUploadRightImage.SaveAs(path);

                string name = file;
                FileStream rightImageFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader rightImageBinaryStream = new BinaryReader(rightImageFileStream);
                Byte[] docbytes = rightImageBinaryStream.ReadBytes((Int32)rightImageFileStream.Length);
                rightImageBinaryStream.Close();
                rightImageFileStream.Close();
                reportManagementBusinessEntity.FrontPageLogo4 = file;
            }
            else
            {
                if (FrontPageLogo4 != "" && hdnRightImage.Value != "")
                    reportManagementBusinessEntity.FrontPageLogo4 = FrontPageLogo4;
                else if (Request.QueryString["Mode"] == "E" && FileUploadRightImage.FileName == "" && hdnRightImage.Value != "")
                    reportManagementBusinessEntity.FrontPageLogo4 = Convert.ToString(Session["FrontPageLogo4"]);
                else
                    reportManagementBusinessEntity.FrontPageLogo4 = "";
            }

            int i = reportManagementBusinessAccessObject.AddProjectSettingReport(reportManagementBusinessEntity);

            if (sender == null && e == null)
            {
                DataTable dtreportsetting2 = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));
                String ProjectReportSettingID = dtreportsetting2.Rows[0]["ProjectReportSettingID"].ToString();
                //Save preview details.
                SavePreview(ProjectReportSettingID.ToString());
            }
            else
            {
                //Clear controls and bind with default value.
                ClearAllConrols();

                lblMessage.Text = "Report settings saved successfully";
            }
        }
    }

    /// <summary>
    /// Reset controls value to default value.
    /// </summary>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ClearAllConrols();
    }

    /// <summary>
    /// Reset controls value to default value.
    /// </summary>
    protected void ClearAllConrols()
    {
        ddlProject.SelectedValue = "0";

        hdnImgTopImage.Value = "";
        hdnImgMiddleImage.Value = "";
        hdnImgBottomImage.Value = "";
        hdnImgRightImage.Value = "";
        chkCoverPage.Checked = false;
        chkReportIntro.Checked = false;
        chkRadarChart.Checked = false;
        chkCategoryIntro.Checked = false;
        chkCatQstlist.Checked = false;
        chkCatQstChart.Checked = false;
        chkCatQstText.Checked = false;
        chkConclusion.Checked = false;
        chkBenchConclusionPage.Checked = false;
        chkSelfNameGrp.Checked = false;
        chkProgrammeGrp.Checked = false;
        chkFullPrjGrp.Checked = false;
        chkPreviousScore.Checked = false;
        chkBenchMark.Checked = false;
        chkBenchMarkGrp.Checked = false;

        for (int i = 0; i < chkGroupList.Items.Count; i++)
        {
            if (chkGroupList.Items[i].Selected)
            {
                chkGroupList.Items[i].Selected = false;
            }
        }

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
    }

    #endregion

    #region dropdown event
    /// <summary>
    /// Reset controls with default value and bind project drop down on account basis.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProject.SelectedValue = "0";
        chkCoverPage.Checked = false;
        chkReportIntro.Checked = false;
        chkRadarChart.Checked = false;
        chkCategoryIntro.Checked = false;
        chkCatQstlist.Checked = false;
        chkCatQstChart.Checked = false;
        chkCatQstText.Checked = false;
        chkConclusion.Checked = false;
        chkBenchConclusionPage.Checked = false;
        chkSelfNameGrp.Checked = false;
        chkFullPrjGrp.Checked = false;
        chkProgrammeGrp.Checked = false;
        chkGroupList.Visible = false;
        chkPreviousScore.Checked = false;
        chkBenchMark.Checked = false;
        chkBenchMarkGrp.Checked = false;
        lblavailable.Text = " ";
        lblMessage.Text = " ";
        ddlReportType.SelectedValue = "0";
        txtPageHeading1.Text = string.Empty;
        txtPageHeading2.Text = string.Empty;
        txtPageHeading3.Text = string.Empty;
        txtPageCopyright.Text = string.Empty;
        txtPageConclusion.Value = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Value = string.Empty;
        txtConHighLowRange.Text = string.Empty;
        txtConclusionHeading.Text = string.Empty;
        ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";

        //TODO: Visibility ON
        radarchart.Visible = true;
        catintro.Visible = true;
        catQstText.Visible = true;
        prevscr.Visible = true;
        selfname.Visible = true;
        grouplist.Visible = true;
        lblavailable.Visible = true;

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get company details by account id.
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));


            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dataTableAccount.ImportRow(drAccount);
            //Set comapny name.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();


            if (ddlAccountCode.SelectedIndex > 0)
            {
                //Get all project in current account.
                DataTable dtprojectlist = projectBusinessAccessObject.GetdtProjectList(Convert.ToString(companycode));

                if (dtprojectlist.Rows.Count > 0)
                {
                    //Bind project dropdown list.
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                    ddlProject.DataSource = dtprojectlist;
                    ddlProject.DataTextField = "Title";
                    ddlProject.DataValueField = "ProjectID";
                    ddlProject.DataBind();
                }
                else
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
    }

    /// <summary>
    /// Get all report setting and bind controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkCoverPage.Checked = false;
        chkReportIntro.Checked = false;
        chkRadarChart.Checked = false;
        chkCategoryIntro.Checked = false;
        chkCatQstlist.Checked = false;
        chkCatQstChart.Checked = false;
        chkCatQstText.Checked = false;
        chkConclusion.Checked = false;
        chkBenchConclusionPage.Checked = false;
        chkPreviousScore.Checked = false;
        chkBenchMark.Checked = false;
        chkBenchMarkGrp.Checked = false;
        chkSelfNameGrp.Checked = false;
        chkFullPrjGrp.Checked = false;
        chkProgrammeGrp.Checked = false;
        chkGroupList.Visible = true;

        for (int i = 0; i < chkGroupList.Items.Count; i++)
        {
            if (chkGroupList.Items[i].Selected)
            {
                chkGroupList.Items[i].Selected = false;
            }
        }

        lblMessage.Text = " ";
        ddlReportType.SelectedValue = "0";
        txtPageHeading1.Text = string.Empty;
        txtPageHeading2.Text = string.Empty;
        txtPageHeading3.Text = string.Empty;
        txtPageCopyright.Text = string.Empty;
        txtPageConclusion.Value = string.Empty;
        txtPageHeadingColor.Text = string.Empty;
        txtPageIntroduction.Value = string.Empty;
        txtConHighLowRange.Text = string.Empty;
        ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";

        strProjectID = ddlProject.SelectedValue;


        //Controls Visibility  Hide/Show Only for Report3.
        ControlHideShow(strProjectID);

        chkGroupList.Items.Clear();
        GroupCheckBoxListBind();
        lblSelfNameGrp.Text = "Self";
        SaveSettingShow(strProjectID);
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

                radarchart.Visible = false;
                catintro.Visible = false;
                catQstText.Visible = false;
                chkBenchMark.Visible = false;
                chkBenchConclusionPage.Visible = false;
                chkBenchMarkGrp.Visible = false;
                prevscr.Visible = true;
                selfname.Visible = false;
                grouplist.Visible = false;
                lblavailable.Visible = false;
                benchrelation.Visible = false;
                benchscoregraph.Visible = false;
                benchconclusion.Visible = false;
            }
            else
            {
                radarchart.Visible = true;
                catintro.Visible = true;
                catQstText.Visible = true;
                chkBenchMark.Visible = true;
                chkBenchConclusionPage.Visible = true;
                chkBenchMarkGrp.Visible = true;
                prevscr.Visible = true;
                selfname.Visible = true;
                grouplist.Visible = true;
                lblavailable.Visible = true;
                benchrelation.Visible = true;
                benchscoregraph.Visible = true;
                benchconclusion.Visible = true;
            }
        }
    }

    /// <summary>
    ///  This Function will Hide/Show the ReportsSettings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue != "3")
        {
            radarchart.Visible = true;
            catintro.Visible = true;
            catQstText.Visible = true;
            chkBenchMark.Visible = true;
            chkBenchConclusionPage.Visible = true;
            chkBenchMarkGrp.Visible = true;
            prevscr.Visible = true;
            selfname.Visible = true;
            grouplist.Visible = true;
            lblavailable.Visible = true;
            benchrelation.Visible = true;
            benchscoregraph.Visible = true;
            benchconclusion.Visible = true;
        }
        else if (ddlReportType.SelectedValue == "3")
        {
            chkPreviousScore.Visible = true;
            radarchart.Visible = false;
            catintro.Visible = false;
            catQstText.Visible = false;
            chkBenchMark.Visible = false;
            chkBenchConclusionPage.Visible = false;
            chkBenchMarkGrp.Visible = false;
            prevscr.Visible = false;
            selfname.Visible = false;
            grouplist.Visible = false;
            lblavailable.Visible = false;
            benchrelation.Visible = false;
            benchscoregraph.Visible = false;
            benchconclusion.Visible = false;
        }

        ReBindTemplateContent();
    }

    #endregion

    #region Check Box Methods
    /// <summary>
    /// Bind group list check box.
    /// </summary>
    protected void GroupCheckBoxListBind()
    {
        //Get group list check box.
        dataTableGroupList = projectBusinessAccessObject.GetProjectRelationship(Convert.ToInt32(strProjectID));

        if (dataTableGroupList.Rows.Count > 0)
        {
            chkGroupList.DataSource = dataTableGroupList;
            //chkGroupList.DataTextField = "";
            chkGroupList.DataValueField = "Value";
            chkGroupList.DataBind();

            lblavailable.Text = Convert.ToString(dataTableGroupList.Rows.Count + 3) + " available selections.";
        }
        else
        {
            lblavailable.Text = "2" + " available selections.";
        }
    }

    /// <summary>
    /// Get comma seperated group value.
    /// </summary>
    protected void RetrieveCheckBoxValue()
    {
        strGroupList = "";
        for (int i = 0; i < chkGroupList.Items.Count; i++)
        {
            if (chkGroupList.Items[i].Selected)
            {
                strGroupList += chkGroupList.Items[i].Text + ",";
            }
        }
        //strGroupList = "'" + strGroupList + "'";

        reportManagementBusinessEntity.ProjectRelationGrp = strGroupList;
    }

    /// <summary>
    /// Check if uploaded file is valid or not .
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
    /// Get unique name for files.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniquefilename;
    }

    /// <summary>
    /// Bind report setting with database report value.
    /// </summary>
    /// <param name="projectid"></param>
    protected void SaveSettingShow(string projectid)
    {
        DataTable dtreportsetting = reportManagementBusinessAccessObject.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));

        if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
        {
           //Set report type
            if (dtreportsetting.Rows[0]["ReportType"].ToString() != String.Empty)
                ddlReportType.SelectedValue = dtreportsetting.Rows[0]["ReportType"].ToString();
            else
                ddlReportType.SelectedValue = "0";

            //set page headings 1 .
            if (dtreportsetting.Rows[0]["PageHeading1"].ToString() != String.Empty)
                txtPageHeading1.Text = dtreportsetting.Rows[0]["PageHeading1"].ToString();
            else
                txtPageHeading1.Text = "";

            //set page headings 2 .
            if (dtreportsetting.Rows[0]["PageHeading2"].ToString() != String.Empty)
                txtPageHeading2.Text = dtreportsetting.Rows[0]["PageHeading2"].ToString();
            else
                txtPageHeading2.Text = "";

            //set page headings 3 .
            if (dtreportsetting.Rows[0]["PageHeading2"].ToString() != String.Empty)
                txtPageHeading3.Text = dtreportsetting.Rows[0]["PageHeading3"].ToString();
            else
                txtPageHeading3.Text = "";

            if (dtreportsetting.Rows[0]["PageHeadingColor"].ToString() != String.Empty)
                txtPageHeadingColor.Text = dtreportsetting.Rows[0]["PageHeadingColor"].ToString();
            else
                txtPageHeadingColor.Text = "";

            //set conclusion headings.
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
            //Set middle image path.
            if (hdnImgMiddleImage.Value != "")
                ImgMiddleImage.Src = "../../UploadDocs/" + hdnImgMiddleImage.Value;
            else
                ImgMiddleImage.Src = "../../UploadDocs/noImage.jpg";


            /*To Show the Front Page Logo 3*/
            if (dtreportsetting.Rows[0]["FrontPageLogo3"].ToString() != String.Empty)
            {
                hdnImgBottomImage.Value = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();
                Session["FrontPageLogo3"] = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();
            }
            else
                hdnImgBottomImage.Value = "";



            //Set bottom image path.
            if (hdnImgBottomImage.Value != "")
                ImgBottomImage.Src = "../../UploadDocs/" + hdnImgBottomImage.Value;
            else
                ImgBottomImage.Src = "../../UploadDocs/noImage.jpg";


            /*To Show the Front Page Logo 4*/
            if (dtreportsetting.Rows[0]["FrontPageLogo4"].ToString() != String.Empty)
            {
                hdnImgRightImage.Value = dtreportsetting.Rows[0]["FrontPageLogo4"].ToString();
                Session["FrontPageLogo4"] = dtreportsetting.Rows[0]["FrontPageLogo4"].ToString();
            }
            else
                hdnImgRightImage.Value = "";

            //Set right image path.
            if (hdnImgRightImage.Value != "")
                ImgRightImage.Src = "../../UploadDocs/" + hdnImgRightImage.Value;
            else
                ImgRightImage.Src = "../../UploadDocs/noImage.jpg";


            if (dtreportsetting.Rows[0]["PageHeadingCopyright"].ToString() != String.Empty)
                txtPageCopyright.Text = dtreportsetting.Rows[0]["PageHeadingCopyright"].ToString();
            else
                txtPageCopyright.Text = "";

            if (dtreportsetting.Rows[0]["PageHeadingIntro"].ToString() != String.Empty)
                txtPageIntroduction.Value = Server.HtmlDecode(dtreportsetting.Rows[0]["PageHeadingIntro"].ToString());
            else
                txtPageIntroduction.Value = "";

            if (dtreportsetting.Rows[0]["PageHeadingConclusion"].ToString() != String.Empty)
                txtPageConclusion.Value = Server.HtmlDecode(dtreportsetting.Rows[0]["PageHeadingConclusion"].ToString());
            else
                txtPageConclusion.Value = "";

            if (dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString() != String.Empty)
                txtConHighLowRange.Text = dtreportsetting.Rows[0]["ConclusionHighLowRange"].ToString();
            else
                txtConHighLowRange.Text = "";

            if (dtreportsetting.Rows[0]["CoverPage"].ToString() == "1")
                chkCoverPage.Checked = true;
            else
                chkCoverPage.Checked = false;

            if (dtreportsetting.Rows[0]["ReportIntroduction"].ToString() == "1")
                chkReportIntro.Checked = true;
            else
                chkReportIntro.Checked = false;

            if (dtreportsetting.Rows[0]["Conclusionpage"].ToString() == "1")
                chkConclusion.Checked = true;
            else
                chkConclusion.Checked = false;

            if (dtreportsetting.Rows[0]["BenchConclusionPage"].ToString() == "1")
                chkBenchConclusionPage.Checked = true;
            else
                chkBenchConclusionPage.Checked = false;

            if (dtreportsetting.Rows[0]["RadarChart"].ToString() == "1")
            {
                chkRadarChart.Checked = true;
                //chkReportIntro.Checked = true;
            }
            else
            {
                chkRadarChart.Checked = false;
                //chkReportIntro.Checked = false;
            }

            if (dtreportsetting.Rows[0]["QstTextResponses"].ToString() == "1")
                chkCatQstText.Checked = true;
            else
                chkCatQstText.Checked = false;

            if (dtreportsetting.Rows[0]["CatQstList"].ToString() == "1")
                chkCatQstlist.Checked = true;
            else
                chkCatQstlist.Checked = false;

            if (dtreportsetting.Rows[0]["CatDataChart"].ToString() == "1")
                chkCatQstChart.Checked = true;
            else
                chkCatQstChart.Checked = false;

            if (chkCatQstChart.Checked == true || chkCatQstlist.Checked == true)
                chkCategoryIntro.Checked = true;
            else
                chkCategoryIntro.Checked = false;

            if (dtreportsetting.Rows[0]["CandidateSelfStatus"].ToString() == "1")
                chkSelfNameGrp.Checked = true;
            else
                chkSelfNameGrp.Checked = false;

            if (dtreportsetting.Rows[0]["FullProjectGrp"].ToString() == "1")
                chkFullPrjGrp.Checked = true;
            else
                chkFullPrjGrp.Checked = false;

            if (dtreportsetting.Rows[0]["ProgrammeGrp"].ToString() == "1")
                chkProgrammeGrp.Checked = true;
            else
                chkProgrammeGrp.Checked = false;

            if (dtreportsetting.Rows[0]["PreviousScoreVisible"].ToString() == "1")
                chkPreviousScore.Checked = true;
            else
                chkPreviousScore.Checked = false;

            if (dtreportsetting.Rows[0]["BenchMarkScoreVisible"].ToString() == "1")
                chkBenchMark.Checked = true;
            else
                chkBenchMark.Checked = false;

            if (dtreportsetting.Rows[0]["BenchMarkGrpVisible"].ToString() == "1")
                chkBenchMarkGrp.Checked = true;
            else
                chkBenchMarkGrp.Checked = false;

            //Bind report section 
            string[] group = Regex.Split(dtreportsetting.Rows[0]["ProjectRelationGrp"].ToString(), ",");
            if (group.Length > 1)
            {
                group[0] = group[0].Replace("'", "");
                for (int i = 0; i < chkGroupList.Items.Count; i++)
                {
                    for (int j = 0; j < group.Length; j++)
                    {
                        if (chkGroupList.Items[i].Value.ToString() == group[j])
                        {
                            chkGroupList.Items[i].Selected = true;
                        }
                    }
                }
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
    /// Show hide category list.
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
    /// Show hide cateory intoduction.
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
    /// Show hide cateory intoduction.
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
    /// Preview the pdf structure.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LinkPreview_Click(object sender, EventArgs e)
    {
        imbSubmit_Click(null, null);
    }

    /// <summary>
    /// Save report preview data.
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
            if (strReportType == "1")
            {


                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";
                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                identity = this.Page.User.Identity as WADIdentity;
                if (identity.User.AccountID == 68 || ddlAccountCode.SelectedValue == "68")
                {
                    rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/PrvCapitaFrontPage";
                }
                else
                {
                    rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/PrvFrontPage";
                }


                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                //set parameter to report server.
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "2")
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient1";
                //rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/PrvFrontPageClient1";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "3")
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/FeedbackReportClient2";
                //  rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                //New Changes 
                //Changed by Amit Singh
                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/PrvFrontPageClient2";

                //If Client Want Setting Should be Configurable then Uncomment the comeented below statement 
                // In that case no need to send hardcord values as Parameter & Comments/Remove all harcord parameters.
                System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
                paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ProjectReportSettingID", strTargetPersonID));
                rview.ServerReport.SetParameters(paramList);
                //for Unauthorized error , make change in web.config( path key="ReportServerUrl").
            }
            else if (strReportType == "4") // Old Mutual Report
            {
                //rview.ServerReport.ReportPath = "/Feedback360_UAT/CurFeedbackReport";
                // rview.ServerReport.ReportPath = "/SURVEY_Feedback_Prod";

                string strReportPathPrefix = ConfigurationManager.AppSettings["ReportPathPreFix"].ToString();
                rview.ServerReport.ReportPath = "/" + strReportPathPrefix + "/PrvCurFrontPage";

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

    /// <summary>
    /// Rebind introducton and conclusion template.
    /// </summary>
    private void ReBindTemplateContent()
    {
        txtPageIntroduction.InnerHtml = Server.HtmlDecode(txtPageIntroduction.InnerHtml);
        txtPageConclusion.InnerHtml = Server.HtmlDecode(txtPageConclusion.InnerHtml);
    }
}

