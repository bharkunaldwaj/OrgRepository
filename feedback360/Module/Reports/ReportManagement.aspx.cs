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
    string filename;
    string file = null;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        //strTargetPersonID = "304";//"298";
        //strAccountID = "29";
        //strProjectID = "178";               
        identity = this.Page.User.Identity as WADIdentity;
        if (!IsPostBack)
        {
           

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

            //if (identity.User.GroupID == 1)
            //{
                Project_BAO project_BAO = new Project_BAO();
                ddlProject.DataSource = project_BAO.GetdtProjectList(Convert.ToString(identity.User.AccountID));
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

    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        if (IsFileValid(fuplTopImage) && IsFileValid(this.fuplMiddleImage) && IsFileValid(this.fuplBottomImage) && IsFileValid(this.FileUploadRightImage))
        {
            /*
             * Here We are Deleting First the existing Record in Table & then New Insertion will be process 
             */
            string lastLogo="";
            string frontPageLogo2="";
            string frontPageLogo3 = "";
            string FrontPageLogo4 = "";

            DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(ddlProject.SelectedValue));

            if (dtreportsetting.Rows.Count >0)
                lastLogo = dtreportsetting.Rows[0]["PageLogo"].ToString();

            if (dtreportsetting.Rows.Count > 0)
                frontPageLogo2 = dtreportsetting.Rows[0]["FrontPageLogo2"].ToString();

            if (dtreportsetting.Rows.Count > 0)
                frontPageLogo3 = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();

            if (dtreportsetting.Rows.Count > 0)
                FrontPageLogo4 = dtreportsetting.Rows[0]["FrontPageLogo4"].ToString();

            

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
            reportManagement_BE.PageHeadingIntro = txtPageIntroduction.Value.Trim();
            reportManagement_BE.PageHeadingConclusion = txtPageConclusion.Value.Trim();
            reportManagement_BE.ConclusionHeading = txtConclusionHeading.Text.Trim();

            //If Admin does't specify the value for Scroes Range then "2" will be insert Default
            if (txtConHighLowRange.Text.Trim() != string.Empty)
                reportManagement_BE.ConclusionHighLowRange = txtConHighLowRange.Text.Trim();
            else
                reportManagement_BE.ConclusionHighLowRange = "2";

            if (chkCoverPage.Checked == true)
                reportManagement_BE.CoverPage = "1";
            else
                reportManagement_BE.CoverPage = "0";
             
            if (chkReportIntro.Checked == true)
                reportManagement_BE.ReportIntroduction = "1";
            else
                reportManagement_BE.ReportIntroduction = "0";

            if (chkBenchConclusionPage.Checked == true)
                reportManagement_BE.BenchConclusionpage = "1";
            else
                reportManagement_BE.BenchConclusionpage = "0";


            if (chkConclusion.Checked == true)
                reportManagement_BE.Conclusionpage = "1";
            else
                reportManagement_BE.Conclusionpage = "0";

            if (chkPreviousScore.Checked == true)
                reportManagement_BE.PreviousScoreVisible = "1";
            else
                reportManagement_BE.PreviousScoreVisible = "0";

            if (chkBenchMark.Checked == true)
                reportManagement_BE.BenchMarkScoreVisible = "1";
            else
                reportManagement_BE.BenchMarkScoreVisible = "0";

            if (chkBenchMarkGrp.Checked == true)
                reportManagement_BE.BenchMarkGrpVisible = "1";
            else
                reportManagement_BE.BenchMarkGrpVisible = "0";


            if (chkRadarChart.Checked == true)
                reportManagement_BE.RadarChart = "1";
            else
                reportManagement_BE.RadarChart = "0";

            if (chkCatQstText.Checked == true)
                reportManagement_BE.QstTextResponses = "1";
            else
                reportManagement_BE.QstTextResponses = "0";

            if (chkCatQstlist.Checked == true)
                reportManagement_BE.CatQstList = "1";
            else
                reportManagement_BE.CatQstList = "0";

            if (chkCatQstChart.Checked == true)
                reportManagement_BE.CatDataChart = "1";
            else
                reportManagement_BE.CatDataChart = "0";

            if (chkSelfNameGrp.Checked == true)
                reportManagement_BE.CandidateSelfStatus = "1";
            else
                reportManagement_BE.CandidateSelfStatus = "0";

            RetrieveCheckBoxValue();

            if (chkFullPrjGrp.Checked == true)
                reportManagement_BE.FullProjectGrp = "1";
            else
                reportManagement_BE.FullProjectGrp = "0";

            if (chkProgrammeGrp.Checked == true)
                reportManagement_BE.ProgrammeGrp = "1";
            else
                reportManagement_BE.ProgrammeGrp = "0";

            if (chkPreviousScore.Checked == true)
                reportManagement_BE.PreviousScoreVisible = "1";
            else
                reportManagement_BE.PreviousScoreVisible = "0";

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
            }

            if (FileUploadRightImage.HasFile)
            {
                filename = System.IO.Path.GetFileName(FileUploadRightImage.PostedFile.FileName);
                
                file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                FileUploadRightImage.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                reportManagement_BE.FrontPageLogo4 = file;
            }
            else
            {
                if (FrontPageLogo4 != "" && hdnRightImage.Value != "")
                    reportManagement_BE.FrontPageLogo4 = FrontPageLogo4;
                else if (Request.QueryString["Mode"] == "E" && FileUploadRightImage.FileName == "" && hdnRightImage.Value != "")
                    reportManagement_BE.FrontPageLogo4 = Convert.ToString(Session["FrontPageLogo4"]);
                else
                    reportManagement_BE.FrontPageLogo4 = "";
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
        }
    }

    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        ClearAllConrols();
    }

    protected void ClearAllConrols()
    {
        ddlProject.SelectedValue = "0";

        hdnImgTopImage.Value="";
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
                }
                else
                {
                    ddlProject.Items.Clear();
                    ddlProject.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
    }

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
        DataTable dtreportsetting = reportManagement_BAO.GetdataProjectSettingReportByID(Convert.ToInt32(projectid));
        if (dtreportsetting != null && dtreportsetting.Rows.Count > 0)
        {
            //TODO: Here will Check If the Report is Report3(in db 3 will be there for report3) then
            // Only will Change the controls Visiblity show/hide.
            if (dtreportsetting.Rows[0]["ReportType"].ToString() == "3")
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
    }

    #endregion    
    
    #region Check Box Methods
        
    protected void GroupCheckBoxListBind()
    {        
        dtGroupList = project_BAO.GetProjectRelationship(Convert.ToInt32(strProjectID));

        if (dtGroupList.Rows.Count > 0)
        {
            chkGroupList.DataSource = dtGroupList;
            //chkGroupList.DataTextField = "";
            chkGroupList.DataValueField = "Value";
            chkGroupList.DataBind();

            lblavailable.Text = Convert.ToString(dtGroupList.Rows.Count + 3) + " available selections.";
        }
        else
        {
            lblavailable.Text = "2" + " available selections."; 
        }                
    }

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
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
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
            if (dtreportsetting.Rows[0]["FrontPageLogo3"].ToString() != String.Empty)
            {
                hdnImgBottomImage.Value = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();
                Session["FrontPageLogo3"] = dtreportsetting.Rows[0]["FrontPageLogo3"].ToString();
            }
            else
                hdnImgBottomImage.Value = "";


            

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


            if (hdnImgRightImage.Value != "")
                ImgRightImage.Src = "../../UploadDocs/" + hdnImgRightImage.Value;
            else
                ImgRightImage.Src = "../../UploadDocs/noImage.jpg";


            if (dtreportsetting.Rows[0]["PageHeadingCopyright"].ToString() != String.Empty)
                txtPageCopyright.Text = dtreportsetting.Rows[0]["PageHeadingCopyright"].ToString();
            else
                txtPageCopyright.Text = "";

            if (dtreportsetting.Rows[0]["PageHeadingIntro"].ToString() != String.Empty)
                txtPageIntroduction.Value = dtreportsetting.Rows[0]["PageHeadingIntro"].ToString();
            else
                txtPageIntroduction.Value = "";

            if (dtreportsetting.Rows[0]["PageHeadingConclusion"].ToString() != String.Empty)
                txtPageConclusion.Value = dtreportsetting.Rows[0]["PageHeadingConclusion"].ToString();
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
    protected void LinkPreview_Click(object sender, EventArgs e)
    {
        imbSubmit_Click(null, null);
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

                Response.AddHeader("Content-Disposition", "attachment; filename=" + PDF_path );
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
}

