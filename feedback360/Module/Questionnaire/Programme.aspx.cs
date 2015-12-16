using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.IO;
using Admin_BAO;

public partial class Module_Questionnaire_Programme : CodeBehindBase
{
    //Global variables
    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
    Programme_BE programmeBusinessEntity = new Programme_BE();
    List<Programme_BE> programmeBusinessEntityList = new List<Programme_BE>();
    WADIdentity identity;
    DataTable dataTableCompanyName;
    string filename;
    string file = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //Reset calender dates.
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

                if (programmeID > 0)//Get Program details by user account and program id.
                    programmeBusinessEntityList = programmeBusinessAccessObject.GetProgrammeByID(Convert.ToInt32(identity.User.AccountID), programmeID);

                Project_BAO projectBusinessAccessObject = new Project_BAO();
                //Get Project details by user account id and bind account deropdownlist.
                ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(identity.User.AccountID.ToString());
                ddlProject.DataValueField = "ProjectID";
                ddlProject.DataTextField = "Title";
                ddlProject.DataBind();

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get account details by user account id and bind account deropdownlist.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                // If Query string Contains "E" then allow Edit and hide show controls accordingly if "R"then view.
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

                //If user is a Super Admin then show account detail section else hide.
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

                if (programmeBusinessEntityList.Count > 0)
                {
                    ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
                    ddlAccountCode_SelectedIndexChanged(sender, e);
                    //Bind program control by program details
                    SetProgrammeValue(programmeBusinessEntityList);
                }
            }
            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Ser program controls by program details
    /// </summary>
    /// <param name="programmeBusinessEntityList"></param>
    private void SetProgrammeValue(List<Programme_BE> programmeBusinessEntityList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            identity = this.Page.User.Identity as WADIdentity;
           
            if (identity.User.GroupID == 1)
            { //If user is a Super Admin then use program detail account id.
                ddlAccountCode.SelectedValue = programmeBusinessEntityList[0].AccountID.ToString();

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    Account_BAO accountBusinessAccessObject = new Account_BAO();
                    //Get account list
                    dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));

                    DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
                    DataTable dataTableAccount = dataTableCompanyName.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                        dataTableAccount.ImportRow(dataRowAccount);
                    //Set company name.
                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }
            //Set control values.
            txtName.Text = programmeBusinessEntityList[0].ProgrammeName;
            txtDescription.Text = programmeBusinessEntityList[0].ProgrammeDescription;
            txtClientName.Text = programmeBusinessEntityList[0].ClientName;
            hdnimage.Value = programmeBusinessEntityList[0].Logo;
            hdnRemoveLogoImage.Value = programmeBusinessEntityList[0].ReportTopLogo;

            Project_BAO projectBusinessAccessObject = new Project_BAO();

            //Get Project list by account id and bind project dropdown.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(ddlAccountCode.SelectedValue);
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            ddlProject.SelectedValue = programmeBusinessEntityList[0].ProjectID.ToString();
            //Set value in date controls
            dtStartDate.Text = Convert.ToDateTime(programmeBusinessEntityList[0].StartDate).ToString("dd/MM/yyyy");
            dtEndDate.Text = Convert.ToDateTime(programmeBusinessEntityList[0].EndDate).ToString("dd/MM/yyyy");
            dtRemainderDate1.Text = Convert.ToDateTime(programmeBusinessEntityList[0].Reminder1Date).ToString("dd/MM/yyyy");

            dtRemainderDate2.Text = Convert.ToDateTime(programmeBusinessEntityList[0].Reminder2Date).ToString("dd/MM/yyyy");
            if (dtRemainderDate2.Text == "01/01/2000")
                dtRemainderDate2.Text = "";

            dtRemainderDate3.Text = Convert.ToDateTime(programmeBusinessEntityList[0].Reminder3Date).ToString("dd/MM/yyyy");
            if (dtRemainderDate3.Text == "01/01/2000")
                dtRemainderDate3.Text = "";

            dtPartReminder1.Text = Convert.ToDateTime(programmeBusinessEntityList[0].PartReminder1Date).ToString("dd/MM/yyyy");
            if (dtPartReminder1.Text == "01/01/2000")
                dtPartReminder1.Text = "";

            dtPartReminder2.Text = Convert.ToDateTime(programmeBusinessEntityList[0].PartReminder2Date).ToString("dd/MM/yyyy");
            if (dtPartReminder2.Text == "01/01/2000")
                dtPartReminder2.Text = "";

            dtAvailableFrom.Text = Convert.ToDateTime(programmeBusinessEntityList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            if (dtAvailableFrom.Text == "01/01/2000")
                dtAvailableFrom.Text = "";

            dtAvailableTo.Text = Convert.ToDateTime(programmeBusinessEntityList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");
            if (dtAvailableTo.Text == "01/01/2000")
                dtAvailableTo.Text = "";

            //dtAvailableFrom.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //dtAvailableTo.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");

            txtStartDate.Text = Convert.ToDateTime(programmeBusinessEntityList[0].StartDate).ToString("dd/MM/yyyy");
            txtEndDate.Text = Convert.ToDateTime(programmeBusinessEntityList[0].EndDate).ToString("dd/MM/yyyy");
            txtRemainderDate1.Text = Convert.ToDateTime(programmeBusinessEntityList[0].Reminder1Date).ToString("dd/MM/yyyy");

            txtRemainderDate2.Text = Convert.ToDateTime(programmeBusinessEntityList[0].Reminder2Date).ToString("dd/MM/yyyy");
            if (txtRemainderDate2.Text == "01/01/2000")
                txtRemainderDate2.Text = "";

            txtRemainderDate3.Text = Convert.ToDateTime(programmeBusinessEntityList[0].Reminder3Date).ToString("dd/MM/yyyy");
            if (txtRemainderDate3.Text == "01/01/2000")
                txtRemainderDate3.Text = "";

            //txtAvailableFrom.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            //txtAvailableTo.Text = Convert.ToDateTime(programme_BEList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");

            txtAvailableFrom.Text = Convert.ToDateTime(programmeBusinessEntityList[0].ReportAvaliableFrom).ToString("dd/MM/yyyy");
            if (txtAvailableFrom.Text == "01/01/2000")
                txtAvailableFrom.Text = "";

            txtAvailableTo.Text = Convert.ToDateTime(programmeBusinessEntityList[0].ReportAvaliableTo).ToString("dd/MM/yyyy");
            if (txtAvailableTo.Text == "01/01/2000")
                txtAvailableTo.Text = "";

            txtPartReminder1.Text = Convert.ToDateTime(programmeBusinessEntityList[0].PartReminder1Date).ToString("dd/MM/yyyy");
            if (txtPartReminder1.Text == "01/01/2000")
                txtPartReminder1.Text = "";

            txtPartReminder2.Text = Convert.ToDateTime(programmeBusinessEntityList[0].PartReminder2Date).ToString("dd/MM/yyyy");
            if (txtPartReminder2.Text == "01/01/2000")
                txtPartReminder2.Text = "";

            txtInstructionText.InnerHtml = Server.HtmlDecode(programmeBusinessEntityList[0].Instructions);
            txtColleaguesNo.Text = programmeBusinessEntityList[0].ColleagueNo.ToString();

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Insert or update according to query string value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Call Click event for date pickere controls.
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
                    Programme_BE programmeBusinessEntity = new Programme_BE();
                    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

                    identity = this.Page.User.Identity as WADIdentity;

                    if (identity.User.GroupID == 1)
                    {
                        programmeBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    }
                    else
                    {
                        programmeBusinessEntity.AccountID = identity.User.AccountID;
                    }

                    programmeBusinessEntity.ProgrammeName = txtName.Text.Trim();
                    programmeBusinessEntity.ProgrammeDescription = txtDescription.Text.Trim();
                    programmeBusinessEntity.ClientName = txtClientName.Text.Trim();

                    //programme_BE.Logo = "";

                    if (FileUpload.HasFile)
                    {
                        filename = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);
                        //filename = FileUpload.FileName;
                        //Get Unique name for  program image
                        file = GetUniqueFilename(filename);

                        //Set file path
                        string path = MapPath("~\\UploadDocs\\") + file;
                        FileUpload.SaveAs(path);
                        string name = file;
                        FileStream logoFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                        BinaryReader logoBinaryReader = new BinaryReader(logoFileStream);
                        Byte[] docbytes = logoBinaryReader.ReadBytes((Int32)logoFileStream.Length);
                        logoBinaryReader.Close();
                        logoFileStream.Close();
                        programmeBusinessEntity.Logo = file;
                    }
                    else
                    {
                        if (Request.QueryString["Mode"] == "E" && FileUpload.FileName == "" && hdnRemoveImage.Value != "")
                            programmeBusinessEntity.Logo = Convert.ToString(Session["FileName"]);
                        else
                            programmeBusinessEntity.Logo = "";
                    }

                    if (FileUploadReportLogo.HasFile)
                    {
                        filename = System.IO.Path.GetFileName(FileUploadReportLogo.PostedFile.FileName);
                        //filename = FileUpload.FileName;
                        //Get Unique name for report image
                        file = GetUniqueFilename(filename);
                        //Set path for report image
                        string path = MapPath("~\\UploadDocs\\") + file;
                        //Save File
                        FileUploadReportLogo.SaveAs(path);

                        string name = file;
                        FileStream reportLogoFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                        BinaryReader reportLogoBinaryReader = new BinaryReader(reportLogoFileStream);
                        Byte[] docbytes = reportLogoBinaryReader.ReadBytes((Int32)reportLogoFileStream.Length);
                        reportLogoBinaryReader.Close();
                        reportLogoFileStream.Close();
                        programmeBusinessEntity.ReportTopLogo = file;
                    }
                    else
                    {
                        if (Request.QueryString["Mode"] == "E" && FileUploadReportLogo.FileName == "" && hdnRemoveReportImage.Value != "")
                            programmeBusinessEntity.ReportTopLogo = Convert.ToString(Session["FileName"]);
                        else
                            programmeBusinessEntity.ReportTopLogo = "";
                    }

                    programmeBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

                    programmeBusinessEntity.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
                    programmeBusinessEntity.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
                    //programme_BE.ReportAvaliableFrom = Convert.ToDateTime(txtAvailableFrom.Text);
                    //programme_BE.ReportAvaliableTo = Convert.ToDateTime(txtAvailableTo.Text);
                    programmeBusinessEntity.Reminder1Date = Convert.ToDateTime(txtRemainderDate1.Text);

                    if (txtRemainderDate2.Text.Trim() != "")
                        programmeBusinessEntity.Reminder2Date = Convert.ToDateTime(txtRemainderDate2.Text);
                    else
                        programmeBusinessEntity.Reminder2Date = Convert.ToDateTime("01/01/2000");

                    if (txtRemainderDate3.Text.Trim() != "")
                        programmeBusinessEntity.Reminder3Date = Convert.ToDateTime(txtRemainderDate3.Text);
                    else
                        programmeBusinessEntity.Reminder3Date = Convert.ToDateTime("01/01/2000");

                    if (txtAvailableFrom.Text.Trim() != "")
                        programmeBusinessEntity.ReportAvaliableFrom = Convert.ToDateTime(txtAvailableFrom.Text);
                    else
                        programmeBusinessEntity.ReportAvaliableFrom = Convert.ToDateTime("01/01/2000");

                    if (txtAvailableTo.Text.Trim() != "")
                        programmeBusinessEntity.ReportAvaliableTo = Convert.ToDateTime(txtAvailableTo.Text);
                    else
                        programmeBusinessEntity.ReportAvaliableTo = Convert.ToDateTime("01/01/2000");

                    if (txtPartReminder1.Text.Trim() != "")
                        programmeBusinessEntity.PartReminder1Date = Convert.ToDateTime(txtPartReminder1.Text);
                    else
                        programmeBusinessEntity.PartReminder1Date = Convert.ToDateTime("01/01/2000");

                    if (txtPartReminder2.Text.Trim() != "")
                        programmeBusinessEntity.PartReminder2Date = Convert.ToDateTime(txtPartReminder2.Text);
                    else
                        programmeBusinessEntity.PartReminder2Date = Convert.ToDateTime("01/01/2000");

                    if (!string.IsNullOrEmpty(txtInstructionText.Value.Trim()))
                        programmeBusinessEntity.Instructions = Server.HtmlDecode(txtInstructionText.Value);

                    if (!string.IsNullOrEmpty(txtColleaguesNo.Text))
                        programmeBusinessEntity.ColleagueNo = Convert.ToInt32(txtColleaguesNo.Text);


                    programmeBusinessEntity.ModifyBy = 1;
                    programmeBusinessEntity.ModifyDate = DateTime.Now;
                    programmeBusinessEntity.IsActive = 1;

                    //If Mode "E" then Update esle Inser data.
                    if (Request.QueryString["Mode"] == "E")
                    {
                        programmeBusinessEntity.ProgrammeID = Convert.ToInt32(Request.QueryString["PrgId"]);
                        programmeBusinessAccessObject.UpdateProgramme(programmeBusinessEntity);//update
                    }
                    else
                    {
                        programmeBusinessAccessObject.AddProgramme(programmeBusinessEntity);//Insert.
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

    /// <summary>
    /// Redirect to Program page on back button click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Account selected event change then Rebind project by its value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Project_BAO projectBusinessAccessObject = new Project_BAO();

        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get Comapny name by Account Id.
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dataTableCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);
            //Set company name.
            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            //Bind project dropdown by Account id.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(ddlAccountCode.SelectedValue);
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            txtInstructionText.InnerHtml = Server.HtmlDecode(txtInstructionText.InnerHtml);
        }
        else
        {
            lblcompanyname.Text = "";
            //Bind project drop down by user Account id.
            ddlProject.DataSource = projectBusinessAccessObject.GetdtProjectList(identity.User.AccountID.ToString());
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();
        }
    }

    /// <summary>
    /// Redirect to Program page on back button click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProgrammeList.aspx", false);
    }

    /// <summary>
    /// validate Colleague reminder second date.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void ValCusReminder2(object source, ServerValidateEventArgs args)
    {
        //bool valid = false;
        identity = this.Page.User.Identity as WADIdentity;

        int Accountid;
        //If user is super Admin then account id is dropdown value else user account id.
        if (identity.User.GroupID == 1)
        {
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        }
        else
        {
            Accountid = Convert.ToInt32(identity.User.AccountID);
        }

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //Get project list in an account.
        List<Project_BE> listProjectDetails = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(listProjectDetails[0].EmailTMPLReminder2);

        if (listProjectDetails[0].EmailTMPLReminder2 != 0 && txtRemainderDate2.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;

        }
        //value.IsValid = valid;
    }

    /// <summary>
    /// validate Colleague reminder third date.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
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

        Project_BAO projectBusinessAccessObject = new Project_BAO();

        List<Project_BE> listProjectDetails = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(listProjectDetails[0].EmailTMPLReminder2);

        if (listProjectDetails[0].EmailTMPLReminder3 != 0 && txtRemainderDate3.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
        //value.IsValid = valid;
    }

    /// <summary>
    /// validate Report Available start date i.e from date.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void ValCusReportAvailableFrom(object source, ServerValidateEventArgs args)
    {
        //bool valid = false;
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;
        //If user is super Admin then account id is dropdown value else user account id.
        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //Get project list in an account.
        List<Project_BE> listProjectDetails = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        if (listProjectDetails[0].EmailTMPLReportAvalibale != 0 && txtAvailableFrom.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;

        //value.IsValid = valid;
    }

    /// <summary>
    /// validate Report Available end date i.e To date.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void ValCusReportAvailableTo(object source, ServerValidateEventArgs args)
    {
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;
        //If user is super Admin then account id is dropdown value else user account id.
        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //Get project list in an account.
        List<Project_BE> listProjectDetails = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        if (listProjectDetails[0].EmailTMPLReportAvalibale != 0 && txtAvailableTo.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    /// <summary>
    /// validate Participant reminder date one.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void ValCusPartReminder1(object source, ServerValidateEventArgs args)
    {
        //bool valid = false;
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;
        //If user is super Admin then account id is dropdown value else user account id.
        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //Get project list in an account.
        List<Project_BE> listProjectDetails = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        if (listProjectDetails[0].EmailTMPPartReminder1 != 0 && txtPartReminder1.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;

        //value.IsValid = valid;
    }

    /// <summary>
    /// validate Participant reminder date second when not perform first survey. 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void ValCusPartReminder2(object source, ServerValidateEventArgs args)
    {
        //bool valid = false;
        identity = this.Page.User.Identity as WADIdentity;
        int Accountid;
        //If user is super Admin then account id is dropdown value else user account id.
        if (identity.User.GroupID == 1)
            Accountid = Convert.ToInt32(ddlAccountCode.SelectedValue);
        else
            Accountid = Convert.ToInt32(identity.User.AccountID);

        Project_BAO projectBusinessAccessObject = new Project_BAO();
        //Get project list in an account.
        List<Project_BE> listProjectDetails = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        if (listProjectDetails[0].EmailTMPPartReminder2 != 0 && txtPartReminder2.Text == "")
            args.IsValid = false;
        else
            args.IsValid = true;

        //value.IsValid = valid;
    }

    private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
    }

    /// <summary>
    /// Use to check whetehr uploaded inage is valid or not by extension and size.
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
    /// Get Unique filename for uploaded image.
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
}
