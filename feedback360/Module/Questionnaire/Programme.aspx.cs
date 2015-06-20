using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.IO;
using System.Collections;
using Admin_BAO;

public partial class Module_Questionnaire_Programme : CodeBehindBase
{
    Programme_BAO programme_BAO = new Programme_BAO();
    Programme_BE programme_BE = new Programme_BE();
    List<Programme_BE> programme_BEList = new List<Programme_BE>();
    WADIdentity identity;
    DataTable dtCompanyName;
    string filename;
    string file = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (txtStartDate.Text != "")
                SetDTPicker(updPanel, "dtStartDate", "txtStartDate");

            if (txtAvailableFrom.Text == "01/01/2000")
                txtAvailableFrom.Text = "";

            if (txtAvailableTo.Text == "01/01/2000")
                txtAvailableTo.Text = "";

            if (txtAvailableFrom.Text != "")
                dtAvailableFrom.Text = txtAvailableFrom.Text;

            if (txtAvailableTo.Text != "")
                dtAvailableTo.Text = txtAvailableTo.Text;

            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;

                
                int programmeID = Convert.ToInt32(Request.QueryString["PrgId"]);

                if (programmeID > 0)
                    programme_BEList = programme_BAO.GetProgrammeByID(Convert.ToInt32(identity.User.AccountID), programmeID);

                Project_BAO project_BAO = new Project_BAO();
                ddlProject.DataSource = project_BAO.GetdtProjectList(identity.User.AccountID.ToString());
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                if (Request.QueryString["Mode"] == "E")
                {
                    imbSave.Visible = true;
                    imbcancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Programme";
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }
                else if (Request.QueryString["Mode"] == "R")
                {
                    imbSave.Visible = false;
                    imbcancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Programme";
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                }

                if (identity.User.GroupID == 1)
                {
                    divAccount.Visible = true;
                    if (Request.QueryString["Mode"] == null)
                    {
                        ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                        ddlAccountCode_SelectedIndexChanged(sender, e);
                    }
                }
                else
                {
                    divAccount.Visible = false;
                }

                if (programme_BEList.Count > 0)
                {
                    ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
                    ddlAccountCode_SelectedIndexChanged(sender, e);

                    SetProgrammeValue(programme_BEList);
                }
            }

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void SetProgrammeValue(List<Programme_BE> programme_BEList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = programme_BEList[0].AccountID.ToString();

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    Account_BAO account1_BAO = new Account_BAO();
                    dtCompanyName = account1_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                    DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");
                    DataTable dtAccount = dtCompanyName.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                        dtAccount.ImportRow(drAccount);

                    lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }

            txtName.Text = programme_BEList[0].ProgrammeName;
            txtDescription.Text = programme_BEList[0].ProgrammeDescription;
            txtClientName.Text = programme_BEList[0].ClientName;
            hdnimage.Value = programme_BEList[0].Logo;
            hdnRemoveLogoImage.Value = programme_BEList[0].ReportTopLogo;

            Project_BAO project_BAO = new Project_BAO();

            ddlProject.DataSource = project_BAO.GetdtProjectList(ddlAccountCode.SelectedValue);
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            ddlProject.SelectedValue = programme_BEList[0].ProjectID.ToString();

            dtStartDate.Text = Convert.ToDateTime(programme_BEList[0].StartDate).ToString("dd/MM/yyyy");
            dtEndDate.Text = Convert.ToDateTime(programme_BEList[0].EndDate).ToString("dd/MM/yyyy");
            dtRemainderDate1.Text = Convert.ToDateTime(programme_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");

            dtRemainderDate2.Text = Convert.ToDateTime(programme_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
            if (dtRemainderDate2.Text == "01/01/2000")
                dtRemainderDate2.Text = "";

            dtRemainderDate3.Text = Convert.ToDateTime(programme_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
            if (dtRemainderDate3.Text == "01/01/2000")
                dtRemainderDate3.Text = "";

            dtPartReminder1.Text = Convert.ToDateTime(programme_BEList[0].PartReminder1Date).ToString("dd/MM/yyyy");
            if (dtPartReminder1.Text == "01/01/2000")
                dtPartReminder1.Text = "";

            dtPartReminder2.Text = Convert.ToDateTime(programme_BEList[0].PartReminder2Date).ToString("dd/MM/yyyy");
            if (dtPartReminder2.Text == "01/01/2000")
                dtPartReminder2.Text = "";

            dtAvailableFrom.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            if (dtAvailableFrom.Text == "01/01/2000")
                dtAvailableFrom.Text = "";

            dtAvailableTo.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");
            if (dtAvailableTo.Text == "01/01/2000")
                dtAvailableTo.Text = "";

            //dtAvailableFrom.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //dtAvailableTo.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");

            txtStartDate.Text = Convert.ToDateTime(programme_BEList[0].StartDate).ToString("dd/MM/yyyy");
            txtEndDate.Text = Convert.ToDateTime(programme_BEList[0].EndDate).ToString("dd/MM/yyyy");
            txtRemainderDate1.Text = Convert.ToDateTime(programme_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");

            txtRemainderDate2.Text = Convert.ToDateTime(programme_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
            if (txtRemainderDate2.Text == "01/01/2000")
                txtRemainderDate2.Text = "";

            txtRemainderDate3.Text = Convert.ToDateTime(programme_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
            if (txtRemainderDate3.Text == "01/01/2000")
                txtRemainderDate3.Text = "";

            //txtAvailableFrom.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //txtAvailableTo.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");

            txtAvailableFrom.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            if (txtAvailableFrom.Text == "01/01/2000")
                txtAvailableFrom.Text = "";

            txtAvailableTo.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");
            if (txtAvailableTo.Text == "01/01/2000")
                txtAvailableTo.Text = "";

            txtPartReminder1.Text = Convert.ToDateTime(programme_BEList[0].PartReminder1Date).ToString("dd/MM/yyyy");
            if (txtPartReminder1.Text == "01/01/2000")
                txtPartReminder1.Text = "";

            txtPartReminder2.Text = Convert.ToDateTime(programme_BEList[0].PartReminder2Date).ToString("dd/MM/yyyy");
            if (txtPartReminder2.Text == "01/01/2000")
                txtPartReminder2.Text = "";

            txtInstructionText.Value = programme_BEList[0].Instructions;
            txtColleaguesNo.Text = programme_BEList[0].ColleagueNo.ToString();

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

    }

    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (txtStartDate.Text != "")
                SetDTPicker(updPanel, "dtStartDate", "txtStartDate");

            if (txtAvailableFrom.Text == "01/01/2000")
                txtAvailableFrom.Text = "";

            if (txtAvailableTo.Text == "01/01/2000")
                txtAvailableTo.Text = "";

            if (txtAvailableFrom.Text != "")
                dtAvailableFrom.Text = txtAvailableFrom.Text;

            if (txtAvailableTo.Text != "")
                dtAvailableTo.Text = txtAvailableTo.Text;

            if (Page.IsValid == true)
            {
                if (this.IsFileValid(this.FileUpload))
                {
                    Programme_BE programme_BE = new Programme_BE();
                    Programme_BAO programme_BAO = new Programme_BAO();

                    identity = this.Page.User.Identity as WADIdentity;

                    if (identity.User.GroupID == 1)
                    {
                        programme_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    }
                    else
                    {
                        programme_BE.AccountID = identity.User.AccountID;
                    }

                    programme_BE.ProgrammeName = txtName.Text.Trim();
                    programme_BE.ProgrammeDescription = txtDescription.Text.Trim();
                    programme_BE.ClientName = txtClientName.Text.Trim();
                
                    //programme_BE.Logo = "";

                    if (FileUpload.HasFile)
                    {
                        filename = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);
                        //filename = FileUpload.FileName;
                        file = GetUniqueFilename(filename);

                        string path = MapPath("~\\UploadDocs\\") + file;
                        FileUpload.SaveAs(path);
                        string name = file;
                        FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                        BinaryReader br1 = new BinaryReader(fs1);
                        Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                        br1.Close();
                        fs1.Close();
                        programme_BE.Logo = file;
                    }
                    else
                    {
                        if (Request.QueryString["Mode"] == "E" && FileUpload.FileName == "" && hdnRemoveImage.Value != "")
                            programme_BE.Logo = Convert.ToString(Session["FileName"]);
                        else
                            programme_BE.Logo = "";
                    }

                    if (FileUploadReportLogo.HasFile)
                    {
                        filename = System.IO.Path.GetFileName(FileUploadReportLogo.PostedFile.FileName);
                        //filename = FileUpload.FileName;
                        file = GetUniqueFilename(filename);

                        string path = MapPath("~\\UploadDocs\\") + file;
                        FileUploadReportLogo.SaveAs(path);
                        string name = file;
                        FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                        BinaryReader br1 = new BinaryReader(fs1);
                        Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                        br1.Close();
                        fs1.Close();
                        programme_BE.ReportTopLogo = file;
                    }
                    else
                    {
                        if (Request.QueryString["Mode"] == "E" && FileUploadReportLogo.FileName == "" && hdnRemoveReportImage.Value != "")
                            programme_BE.ReportTopLogo = Convert.ToString(Session["FileName"]);
                        else
                            programme_BE.ReportTopLogo = "";
                    }

                    


                    programme_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

                    programme_BE.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
                    programme_BE.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
                    //programme_BE.ReportAvaliableFrom = Convert.ToDateTime(txtAvailableFrom.Text);
                    //programme_BE.ReportAvaliableTo = Convert.ToDateTime(txtAvailableTo.Text);
                    programme_BE.Reminder1Date = Convert.ToDateTime(txtRemainderDate1.Text);

                    if (txtRemainderDate2.Text.Trim() != "")
                        programme_BE.Reminder2Date = Convert.ToDateTime(txtRemainderDate2.Text);
                    else
                        programme_BE.Reminder2Date = Convert.ToDateTime("01/01/2000");

                    if (txtRemainderDate3.Text.Trim() != "")
                        programme_BE.Reminder3Date = Convert.ToDateTime(txtRemainderDate3.Text);
                    else
                        programme_BE.Reminder3Date = Convert.ToDateTime("01/01/2000");

                    if (txtAvailableFrom.Text.Trim() != "")
                        programme_BE.ReportAvaliableFrom = Convert.ToDateTime(txtAvailableFrom.Text);
                    else
                        programme_BE.ReportAvaliableFrom = Convert.ToDateTime("01/01/2000");

                    if (txtAvailableTo.Text.Trim() != "")
                        programme_BE.ReportAvaliableTo = Convert.ToDateTime(txtAvailableTo.Text);
                    else
                        programme_BE.ReportAvaliableTo = Convert.ToDateTime("01/01/2000");

                    if (txtPartReminder1.Text.Trim() != "")
                        programme_BE.PartReminder1Date = Convert.ToDateTime(txtPartReminder1.Text);
                    else
                        programme_BE.PartReminder1Date = Convert.ToDateTime("01/01/2000");

                    if (txtPartReminder2.Text.Trim() != "")
                        programme_BE.PartReminder2Date = Convert.ToDateTime(txtPartReminder2.Text);
                    else
                        programme_BE.PartReminder2Date = Convert.ToDateTime("01/01/2000");

                    if (!string.IsNullOrEmpty(txtInstructionText.Value.Trim()))
                        programme_BE.Instructions = txtInstructionText.Value;

                    if (!string.IsNullOrEmpty(txtColleaguesNo.Text))
                        programme_BE.ColleagueNo = Convert.ToInt32(txtColleaguesNo.Text);


                    programme_BE.ModifyBy = 1;
                    programme_BE.ModifyDate = DateTime.Now;
                    programme_BE.IsActive = 1;

                    if (Request.QueryString["Mode"] == "E")
                    {
                        programme_BE.ProgrammeID = Convert.ToInt32(Request.QueryString["PrgId"]);
                        programme_BAO.UpdateProgramme(programme_BE);
                    }
                    else
                    {
                        programme_BAO.AddProgramme(programme_BE);
                    }

                    Response.Redirect("ProgrammeList.aspx", false);

                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void imbcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("ProgrammeList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Project_BAO project_BAO = new Project_BAO();

        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO account_BAO = new Account_BAO();

            dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dtCompanyName.Clone();
            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            ddlProject.DataSource = project_BAO.GetdtProjectList(ddlAccountCode.SelectedValue);
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

        }
        else
        {
            lblcompanyname.Text = "";

            ddlProject.DataSource = project_BAO.GetdtProjectList(identity.User.AccountID.ToString());
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();
        }
    }

    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProgrammeList.aspx", false);
    }

    protected void ValCusReminder2(object source, ServerValidateEventArgs args)
    {

        //bool valid = false;

        identity = this.Page.User.Identity as WADIdentity;

        int Accountid;

        if (identity.User.GroupID == 1)
        {
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        }
        else
        {
            Accountid = Convert.ToInt32(identity.User.AccountID);
        }



        Project_BAO project_BAO = new Project_BAO();

        List<Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(projectInfo[0].EmailTMPLReminder2);

        if (projectInfo[0].EmailTMPLReminder2 != 0 && txtRemainderDate2.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;

        }


        //value.IsValid = valid;

    }

    protected void ValCusReminder3(object source, ServerValidateEventArgs args)
    {

        //bool valid = false;

        identity = this.Page.User.Identity as WADIdentity;

        int Accountid;

        if (identity.User.GroupID == 1)
        {
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        }
        else
        {
            Accountid = Convert.ToInt32(identity.User.AccountID);
        }



        Project_BAO project_BAO = new Project_BAO();

        List<Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(projectInfo[0].EmailTMPLReminder2);

        if (projectInfo[0].EmailTMPLReminder3 != 0 && txtRemainderDate3.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;

        }


        //value.IsValid = valid;

    }

    protected void ValCusReportAvailableFrom(object source, ServerValidateEventArgs args)
    {
        //bool valid = false;
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;

        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO project_BAO = new Project_BAO();
        List<Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        if (projectInfo[0].EmailTMPLReportAvalibale != 0 && txtAvailableFrom.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;

        //value.IsValid = valid;
    }

    protected void ValCusReportAvailableTo(object source, ServerValidateEventArgs args)
    {
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;

        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO project_BAO = new Project_BAO();
        List<Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        if (projectInfo[0].EmailTMPLReportAvalibale != 0 && txtAvailableTo.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    protected void ValCusPartReminder1(object source, ServerValidateEventArgs args)
    {
        //bool valid = false;
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;

        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO project_BAO = new Project_BAO();
        List<Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        if (projectInfo[0].EmailTMPPartReminder1 != 0 && txtPartReminder1.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;

        //value.IsValid = valid;
    }

    protected void ValCusPartReminder2(object source, ServerValidateEventArgs args)
    {
        //bool valid = false;
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;

        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO project_BAO = new Project_BAO();
        List<Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        if (projectInfo[0].EmailTMPPartReminder2 != 0 && txtPartReminder2.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;

        //value.IsValid = valid;
    }

    private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
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
}
