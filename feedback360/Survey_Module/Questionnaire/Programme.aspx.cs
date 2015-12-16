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

public partial class Survey_Module_Questionnaire_Programme : CodeBehindBase
{
    //Global variables
    Survey_Programme_BAO programmeBusinessObject = new Survey_Programme_BAO();
    //Survey_Programme_BE programme_BE = new Survey_Programme_BE();
    //Survey_Programme_BE2 programme_BE2 = new Survey_Programme_BE2();

    List<Survey_Programme_BE> programmeBusinessEntityList = new List<Survey_Programme_BE>();
    //DataSet dsprogramme_Analysis_List = new DataSet();
    WADIdentity identity;
    DataTable dataTableCompanyName;  //,dtCategory;
    DataTable dataTableAnalysis1;
    DataTable dataTableAnalysis2;
    DataTable dataTableAnalysis3;

    string filename;
    string file = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        //HandleWriteLog("Start", new StackTrace(true));
        if (txtStartDate.Text != "")
            SetDTPicker(null, "dtStartDate", "txtStartDate");

        identity = this.Page.User.Identity as WADIdentity;

        if (!IsPostBack)
        {
            identity = this.Page.User.Identity as WADIdentity;

            int programmeID = Convert.ToInt32(Request.QueryString["PrgId"]);

            if (programmeID > 0)
            {
                //Get Program details by user account and program id.
                programmeBusinessEntityList = programmeBusinessObject.GetProgrammeByID(Convert.ToInt32(identity.User.AccountID), programmeID);
            }

            Survey_Project_BAO projectBusinessObject = new Survey_Project_BAO();
            //Get Project details by user account id and bind account deropdownlist.
            ddlProject.DataSource = projectBusinessObject.GetdtProjectList(identity.User.AccountID.ToString());
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

            Account_BAO accountBusinessObject = new Account_BAO();
            //Get account details by user account id and bind account deropdownlist.
            ddlAccountCode.DataSource = accountBusinessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            // If Query string Contains "E" then allow Edit and hide show controls accordingly if "R"then view.
            if (Request.QueryString["Mode"] == "E")//Edit mode
            {
                imbSave.Visible = true;
                imbcancel.Visible = true;
                imbBack.Visible = false;
                lblheader.Text = "Edit Programme";
                ddlAccountCode_SelectedIndexChanged(sender, e);
            }
            else if (Request.QueryString["Mode"] == "R")//View mode.
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
                ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                ddlAccountCode_SelectedIndexChanged(sender, e);
            }

            if (programmeBusinessEntityList.Count > 0)
            {
                //Bind project bu account id and set comapny name.
                ddlAccountCode_SelectedIndexChanged(sender, e);
                //Bind program control by program details
                SetProgrammeValue(programmeBusinessEntityList);
            }
        }
    }

    /// <summary>
    /// Set program controls by program details
    /// </summary>
    /// <param name="programmeListBusinessEntity"></param>
    private void SetProgrammeValue(List<Survey_Programme_BE> programmeListBusinessEntity)
    {
        identity = this.Page.User.Identity as WADIdentity;

        if (identity.User.GroupID == 1)
        {
            //If user is a Super Admin then use program detail account id.
            ddlAccountCode.SelectedValue = programmeListBusinessEntity[0].AccountID.ToString();

            if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
            {
                int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                Account_BAO accountBusinessObject = new Account_BAO();

                dataTableCompanyName = accountBusinessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));

                DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");

                DataTable dataTableAccount = dataTableCompanyName.Clone();

                foreach (DataRow dataRowAccount in resultsAccount)
                    dataTableAccount.ImportRow(dataRowAccount);

                //lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
            }
            //else
            //{
            //    lblcompanyname.Text = "";
            //}
        }
        //Set control values.
        txtName.Text = programmeListBusinessEntity[0].ProgrammeName;
        txtDescription.Text = programmeListBusinessEntity[0].ProgrammeDescription;
        txtClientName.Text = programmeListBusinessEntity[0].ClientName;
        hdnimage.Value = programmeListBusinessEntity[0].Logo;

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        //Get Project list by account id and bind project dropdown.
        ddlProject.DataSource = project_BAO.GetdtProjectList(ddlAccountCode.SelectedValue);
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

        ddlProject.SelectedValue = programmeListBusinessEntity[0].ProjectID.ToString();
        ddlProject_SelectedIndexChanged(ddlProject, null);

        if (!string.IsNullOrEmpty(programmeListBusinessEntity[0].CompanyID.ToString()))
            ddlCompany.SelectedValue = programmeListBusinessEntity[0].CompanyID.ToString();
        //}
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //Set value in date controls
        dtStartDate.Text = Convert.ToDateTime(programmeListBusinessEntity[0].StartDate).ToString("dd/MM/yyyy");
        dtEndDate.Text = Convert.ToDateTime(programmeListBusinessEntity[0].EndDate).ToString("dd/MM/yyyy");
        dtRemainderDate1.Text = Convert.ToDateTime(programmeListBusinessEntity[0].Reminder1Date).ToString("dd/MM/yyyy");

        dtRemainderDate2.Text = Convert.ToDateTime(programmeListBusinessEntity[0].Reminder2Date).ToString("dd/MM/yyyy");

        if (dtRemainderDate2.Text == "01/01/2000")
            dtRemainderDate2.Text = "";

        dtRemainderDate3.Text = Convert.ToDateTime(programmeListBusinessEntity[0].Reminder3Date).ToString("dd/MM/yyyy");

        if (dtRemainderDate3.Text == "01/01/2000")
            dtRemainderDate3.Text = "";

        txtStartDate.Text = Convert.ToDateTime(programmeListBusinessEntity[0].StartDate).ToString("dd/MM/yyyy");
        txtEndDate.Text = Convert.ToDateTime(programmeListBusinessEntity[0].EndDate).ToString("dd/MM/yyyy");
        txtRemainderDate1.Text = Convert.ToDateTime(programmeListBusinessEntity[0].Reminder1Date).ToString("dd/MM/yyyy");

        txtRemainderDate2.Text = Convert.ToDateTime(programmeListBusinessEntity[0].Reminder2Date).ToString("dd/MM/yyyy");

        if (txtRemainderDate2.Text == "01/01/2000")
            txtRemainderDate2.Text = "";

        txtRemainderDate3.Text = Convert.ToDateTime(programmeListBusinessEntity[0].Reminder3Date).ToString("dd/MM/yyyy");

        if (txtRemainderDate3.Text == "01/01/2000")
            txtRemainderDate3.Text = "";

        if (String.IsNullOrEmpty(programmeListBusinessEntity[0].Analysis_I_Name) != true)
            Txt_name_Analysis1.Text = programmeListBusinessEntity[0].Analysis_I_Name.ToString();

        if (String.IsNullOrEmpty(programmeListBusinessEntity[0].Analysis_I_Category.ToString()) != true)
            txt_catagory_Analysis1.Text = Convert.ToInt32(programmeListBusinessEntity[0].Analysis_I_Category).ToString();

        if (String.IsNullOrEmpty(programmeListBusinessEntity[0].Analysis_II_Name) != true)
            txt_name_Analysis2.Text = programmeListBusinessEntity[0].Analysis_II_Name.ToString();

        if (String.IsNullOrEmpty(programmeListBusinessEntity[0].Analysis_II_Category.ToString()) != true)
            txt_category_Analysis2.Text = Convert.ToInt32(programmeListBusinessEntity[0].Analysis_II_Category).ToString();

        if (String.IsNullOrEmpty(programmeListBusinessEntity[0].Analysis_III_Name) != true)
            txt_name_Analysis3.Text = programmeListBusinessEntity[0].Analysis_III_Name.ToString();

        if (String.IsNullOrEmpty(programmeListBusinessEntity[0].Analysis_III_Category.ToString()) != true)
            txt_category_Analysis3.Text = Convert.ToInt32(programmeListBusinessEntity[0].Analysis_III_Category).ToString();

        /*Assigning the datasource to category repeater
        string mode=Request.QueryString["Mode"];
        if (mode == "R")
        {
            txt_catagory_Analysis1.ReadOnly = true;
            txt_category_Analysis2.ReadOnly = true;
            txt_category_Analysis3.ReadOnly = true;
            imbSubmit1.Enabled = false;
            imbSubmit2.Enabled = false;
            imbSubmit3.Enabled = false;
        }
        else
        {
            txt_catagory_Analysis1.ReadOnly = false;
            txt_category_Analysis2.ReadOnly = false;
            txt_category_Analysis3.ReadOnly = false;
            imbSubmit1.Enabled = true;
            imbSubmit2.Enabled = true;
            imbSubmit3.Enabled = true;
        }
        */
        int programmeID = Convert.ToInt32(Request.QueryString["PrgId"]);

        if (programmeID > 0)
        {
            dataTableAnalysis1 = programmeBusinessObject.GetAnalysis1(programmeID);
            Repeater0.DataSource = dataTableAnalysis1;
            Repeater0.DataBind();
            Repeater0.Visible = true;

            dataTableAnalysis2 = programmeBusinessObject.GetAnalysis2(programmeID);
            Repeater1.DataSource = dataTableAnalysis2;
            Repeater1.DataBind();
            Repeater1.Visible = true;

            dataTableAnalysis3 = programmeBusinessObject.GetAnalysis3(programmeID);
            Repeater2.DataSource = dataTableAnalysis3;
            Repeater2.DataBind();
            Repeater2.Visible = true;
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
            foreach (RepeaterItem item in Repeater0.Items)
            {
                TextBox txt = (TextBox)item.FindControl("MyLabel");
            }

            if (txtStartDate.Text != "")
                SetDTPicker(null, "dtStartDate", "txtStartDate");

            //if (Page.IsValid == true)
            //{
            if (this.IsFileValid(this.FileUpload))
            {
                Survey_Programme_BE programmeBusinessEntity = new Survey_Programme_BE();
                //Programme_BE programme_BE = new Programme_BE();

                //Programm
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
                    FileStream programFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                    BinaryReader programBinaryReader = new BinaryReader(programFileStream);
                    Byte[] docbytes = programBinaryReader.ReadBytes((Int32)programFileStream.Length);
                    programBinaryReader.Close();
                    programFileStream.Close();
                    programmeBusinessEntity.Logo = file;
                }
                else
                {
                    if (Request.QueryString["Mode"] == "E" && FileUpload.FileName == "" && hdnRemoveImage.Value != "")
                        programmeBusinessEntity.Logo = Convert.ToString(Session["FileName"]);
                    else
                        programmeBusinessEntity.Logo = "";
                }

                programmeBusinessEntity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

                if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
                    programmeBusinessEntity.CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);

                programmeBusinessEntity.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim().ToString());
                programmeBusinessEntity.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim().ToString());
                programmeBusinessEntity.Reminder1Date = Convert.ToDateTime(txtRemainderDate1.Text.Trim().ToString());

                if (txtRemainderDate2.Text.Trim() != "")
                    programmeBusinessEntity.Reminder2Date = Convert.ToDateTime(txtRemainderDate2.Text.Trim().ToString());
                else
                    programmeBusinessEntity.Reminder2Date = Convert.ToDateTime("01/01/2000");

                if (txtRemainderDate3.Text.Trim() != "")
                    programmeBusinessEntity.Reminder3Date = Convert.ToDateTime(txtRemainderDate3.Text.Trim().ToString());
                else
                    programmeBusinessEntity.Reminder3Date = Convert.ToDateTime("01/01/2000");

                programmeBusinessEntity.ProgrammeID = 0;
                programmeBusinessEntity.ModifyBy = 1;
                programmeBusinessEntity.ModifyDate = DateTime.Now;
                programmeBusinessEntity.IsActive = 1;

                programmeBusinessEntity.Analysis_I_Name = Txt_name_Analysis1.Text.Trim();

                if (String.IsNullOrEmpty(txt_catagory_Analysis1.Text) == true)
                { }
                else
                    programmeBusinessEntity.Analysis_I_Category = Convert.ToInt32(txt_catagory_Analysis1.Text.Trim().ToString());

                programmeBusinessEntity.Analysis_II_Name = txt_name_Analysis2.Text.Trim();

                if (String.IsNullOrEmpty(txt_category_Analysis2.Text) == true)
                { }
                else
                    programmeBusinessEntity.Analysis_II_Category = Convert.ToInt32(txt_category_Analysis2.Text);

                programmeBusinessEntity.Analysis_III_Name = txt_name_Analysis3.Text.Trim();

                if (String.IsNullOrEmpty(txt_category_Analysis3.Text) == true)
                { }
                else
                    programmeBusinessEntity.Analysis_III_Category = Convert.ToInt32(txt_category_Analysis3.Text.Trim());

                int programID = 0;
                //If Mode "E" then Update esle Inser data.
                if (Request.QueryString["Mode"] == "E")
                {
                    programID = Convert.ToInt32(Request.QueryString["PrgId"]);
                    programmeBusinessEntity.ProgrammeID = Convert.ToInt32(Request.QueryString["PrgId"]);
                    programmeBusinessObject.UpdateProgramme(programmeBusinessEntity);//Update
                }
                else
                {
                    programID = programmeBusinessObject.AddProgramme(programmeBusinessEntity);//Insert
                }

                string[] analysisTypeArray = { Label9.Text, Label10.Text, Label12.Text.Trim() };
                string[] nameOfCategoryArray = { Txt_name_Analysis1.Text.Trim(), txt_name_Analysis2.Text.Trim(), txt_name_Analysis3.Text.Trim() };

                if (txt_catagory_Analysis1.Text == "")
                    txt_catagory_Analysis1.Text = "0";

                if (txt_catagory_Analysis1.Text == "")
                    txt_category_Analysis2.Text = "0";

                if (txt_catagory_Analysis1.Text == "")
                    txt_category_Analysis3.Text = "0";

                TextBox textBoxCategory;
                string categoryDetailList = "";
                string analysisTypeList = "";
                string categoryNameList = "";

                foreach (RepeaterItem item in Repeater0.Items)
                {
                    textBoxCategory = (TextBox)item.FindControl("txt_category");

                    if (textBoxCategory.Text == "")
                        textBoxCategory.Text = "   ";

                    categoryDetailList = categoryDetailList + textBoxCategory.Text.Trim() + ",";
                    analysisTypeList = analysisTypeList + analysisTypeArray[0] + ",";
                    categoryNameList = categoryNameList + nameOfCategoryArray[0] + ",";
                }

                foreach (RepeaterItem item1 in Repeater1.Items)
                {
                    textBoxCategory = (TextBox)item1.FindControl("txt_category");

                    if (textBoxCategory.Text == "")
                        textBoxCategory.Text = "   ";

                    categoryDetailList = categoryDetailList + textBoxCategory.Text.Trim() + ",";
                    analysisTypeList = analysisTypeList + analysisTypeArray[1] + ",";
                    categoryNameList = categoryNameList + nameOfCategoryArray[1] + ",";
                }

                foreach (RepeaterItem item2 in Repeater2.Items)
                {
                    textBoxCategory = (TextBox)item2.FindControl("txt_category");

                    if (textBoxCategory.Text == "")
                        textBoxCategory.Text = "   ";

                    categoryDetailList = categoryDetailList + textBoxCategory.Text.Trim() + ",";
                    analysisTypeList = analysisTypeList + analysisTypeArray[2] + ",";
                    categoryNameList = categoryNameList + nameOfCategoryArray[2] + ",";
                }

                int result = programmeBusinessObject.save_category_for_analysis_list(programID, categoryDetailList, analysisTypeList, categoryNameList);

                if (result >= 1)
                {
                    lbl_save_message.Visible = true;

                    txtName.Text = txtDescription.Text = txtClientName.Text = dtStartDate.Text = dtEndDate.Text = dtRemainderDate1.Text = dtRemainderDate2.Text = dtRemainderDate3.Text = Txt_name_Analysis1.Text = txt_catagory_Analysis1.Text = txt_name_Analysis2.Text = txt_category_Analysis2.Text = txt_name_Analysis3.Text = txt_category_Analysis3.Text = "";

                    Repeater0.Visible = Repeater1.Visible = Repeater2.Visible = false;
                    ddlProject.ClearSelection();
                    result = 0;
                }

                lbl_save_message.Text = "Survey Programme Saved Successfully";
                Response.Redirect("ProgrammeList.aspx", false);
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
            Response.Redirect("ProgrammeList.aspx", false);
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
        Survey_Project_BAO projectBusinessObject = new Survey_Project_BAO();

        ddlProject.Items.Clear();
        ddlProject.Items.Insert(0, new ListItem("Select", "0"));

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO account_BAO = new Account_BAO();
            //Get Comapny name by Account Id.
            dataTableCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");

            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
                dataTableAccount.ImportRow(dataRowAccount);

            //lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
            //Bind project dropdown by Account id.
            ddlProject.DataSource = projectBusinessObject.GetdtProjectList(ddlAccountCode.SelectedValue);
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();
        }
        else
        {
            //lblcompanyname.Text = "";
            //Bind project dropdown by Account id.
            ddlProject.DataSource = projectBusinessObject.GetdtProjectList(identity.User.AccountID.ToString());
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

        Survey_Project_BAO projectBusinessObject = new Survey_Project_BAO();
        //Get project list in an account.
        List<Survey_Project_BE> projectInformation = projectBusinessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(projectInformation[0].EmailTMPLReminder2);

        if (projectInformation[0].EmailTMPLReminder2 != 0 && txtRemainderDate2.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    /// <summary>
    /// validate Colleague reminder third date.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void ValCusReminder3(object source, ServerValidateEventArgs args)
    {
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

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        //Get project list in an account.
        List<Survey_Project_BE> projectInformation = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(projectInformation[0].EmailTMPLReminder2);

        if (projectInformation[0].EmailTMPLReminder3 != 0 && txtRemainderDate3.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
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
        //Get project list in an account.
        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
        List<Survey_Project_BE> projectInfo = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        //if (projectInfo[0].EmailTMPLReportAvalibale != 0 && txtAvailableFrom.Text == "")
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;

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

        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
        //Get project list in an account.
        List<Survey_Project_BE> projectInfo = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //if (projectInfo[0].EmailTMPLReportAvalibale != 0 && txtAvailableTo.Text == "")
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;
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
        //Get project list in an account.
        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
        List<Survey_Project_BE> projectInfo = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        //if (projectInfo[0].EmailTMPPartReminder1 != 0 && txtPartReminder1.Text == "")
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;

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

        Survey_Project_BAO projectBusinessAccessObject = new Survey_Project_BAO();
        //Get project list in an account.
        List<Survey_Project_BE> projectInfo = projectBusinessAccessObject.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        //if (projectInfo[0].EmailTMPPartReminder2 != 0 && txtPartReminder2.Text == "")
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;

        //value.IsValid = valid;
    }

    private void SetDTPicker(Control btn, string HtmlDate, string aspDate)//instance of button clicked
    {
        //ScriptManager.RegisterClientScriptBlock(btn, btn.GetType(), "test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');", true);
        Page.RegisterClientScriptBlock("test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');");

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

    /// <summary>
    /// Add Category to Analysis I.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit1_Click(object sender, ImageClickEventArgs e)
    {
        //Page.Validate();
        //if (Page.IsValid)
        //{
        //dtStartDate.EnableViewState = true;
        string strtdate = dtStartDate.Text;

        if (String.IsNullOrEmpty(txt_catagory_Analysis1.Text) == false)
        {
            if (Convert.ToInt32(txt_catagory_Analysis1.Text) >= 1)
            {
                Repeater0.Visible = true;
            }


            DataTable dataTableCategory = new DataTable();
            Repeater0.Visible = true;

            if (txt_catagory_Analysis1.Text.Trim() != "" || txt_catagory_Analysis1.Text.Trim() != Convert.ToString(0))
                dataTableCategory = BindCandidateList("A1", Convert.ToInt32(txt_catagory_Analysis1.Text.Trim()));

            Repeater0.DataSource = dataTableCategory;
            Repeater0.DataBind();//Bind category repeator.
        }
    }

    /// <summary>
    /// Add Category to Analysis II.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit2_Click(object sender, ImageClickEventArgs e)
    {
        //Page.Validate();
        //if (Page.IsValid)
        //{
        if (String.IsNullOrEmpty(txt_category_Analysis2.Text) == false)
        {
            if (Convert.ToInt32(txt_category_Analysis2.Text) >= 1)
            {
                Repeater0.Visible = true;
            }

            DataTable dataTableCategory = new DataTable();
            Repeater1.Visible = true;

            if (txt_category_Analysis2.Text.Trim() != "" || txt_category_Analysis2.Text.Trim() != Convert.ToString(0))
                dataTableCategory = BindCandidateList("A2", Convert.ToInt32(txt_category_Analysis2.Text.Trim()));

            Repeater1.DataSource = dataTableCategory;
            Repeater1.DataBind();//Bind category repeator.
        }
    }

    /// <summary>
    /// Add Category to Analysis III.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit3_Click(object sender, ImageClickEventArgs e)
    {
        if (String.IsNullOrEmpty(txt_category_Analysis3.Text) == false)
        {
            if (Convert.ToInt32(txt_category_Analysis3.Text) >= 1)
            {
                Repeater0.Visible = true;
            }

            DataTable dataTableCategory = new DataTable();
            Repeater2.Visible = true;

            if (txt_category_Analysis3.Text.Trim() != "" || txt_category_Analysis3.Text.Trim() != Convert.ToString(0))
                dataTableCategory = BindCandidateList("A3", Convert.ToInt32(txt_category_Analysis3.Text.Trim()));

            Repeater2.DataSource = dataTableCategory;
            Repeater2.DataBind();//Bind category repeator.
        }
    }

    /// <summary>
    /// Bind Candidate repeator by analysis type.
    /// </summary>
    /// <param name="AnalysisType"></param>
    /// <param name="candidateCount"></param>
    /// <returns></returns>
    private DataTable BindCandidateList(string AnalysisType, int candidateCount)
    {
        DataTable dataTableCandidate;
        dataTableCandidate = new DataTable();

        string strtdate = dtStartDate.Text;
        int programmeID = 0;

        if (Request.QueryString["PrgId"] != null)
            programmeID = Convert.ToInt32(Request.QueryString["PrgId"]);

        if (programmeID > 0)
        {
            if (AnalysisType == "A1")
                dataTableCandidate = programmeBusinessObject.GetAnalysis1(programmeID);//Get category for Analysis I.
            else if (AnalysisType == "A2")
                dataTableCandidate = programmeBusinessObject.GetAnalysis2(programmeID);//Get category for Analysis II.
            else if (AnalysisType == "A3")
                dataTableCandidate = programmeBusinessObject.GetAnalysis3(programmeID);//Get category for Analysis III.
        }
        //if (dtCandidate != null)
        //{
        //dtCandidate.Columns.Add("Category_Detail");
        int intExistingrows = Convert.ToInt32(dataTableCandidate.Rows.Count);
        //int intExistingColumns = Convert.ToInt32(dtCandidate.Columns.Count);
        if (intExistingrows <= 0 && programmeID == 0)  // && intExistingColumns <= 0)
            dataTableCandidate.Columns.Add("Category_Detail");//Add column by number of category.

        candidateCount = candidateCount - intExistingrows;

        for (int count = 0; count < candidateCount; count++)
        {
            dataTableCandidate.Rows.Add("");
        }
        //}
        return dataTableCandidate;
    }

    /// <summary>
    ///  Show the Error Label (if no data is present), for Analysis I.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Repeater0_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rptDemo = sender as Repeater; // Get the Repeater control object.
        string strtdate = dtStartDate.Text;
        // If the Repeater contains no data.
        if (Convert.ToInt32(txt_catagory_Analysis1.Text) < 1)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                // Show the Error Label (if no data is present).
                Label lblErrorMsg = e.Item.FindControl("lblErrorMsg") as Label;

                if (lblErrorMsg != null)
                {
                    lblErrorMsg.Visible = true;
                }
            }
        }
    }

    /// <summary>
    ///  Show the Error Label (if no data is present), for Analysis II.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rptDemo = sender as Repeater; // Get the Repeater control object.

        // If the Repeater contains no data.
        if (Convert.ToInt32(txt_category_Analysis2.Text) < 1)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                // Show the Error Label (if no data is present).
                Label lblErrorMsg = e.Item.FindControl("lblErrorMsg1") as Label;

                if (lblErrorMsg != null)
                {
                    lblErrorMsg.Visible = true;
                }
            }
        }
    }

    /// <summary>
    ///  Show the Error Label (if no data is present), for Analysis III.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rptDemo = sender as Repeater; // Get the Repeater control object.

        // If the Repeater contains no data.
        if (Convert.ToInt32(txt_category_Analysis3.Text) < 1)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                // Show the Error Label (if no data is present).
                Label lblErrorMsg = e.Item.FindControl("lblErrorMsg2") as Label;
                if (lblErrorMsg != null)
                {
                    lblErrorMsg.Visible = true;
                }
            }
        }
    }

    /// <summary>
    /// Its of no use.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dtEndDate_TextChanged(object sender, EventArgs e)
    {

    }

    private void fillCompany()
    {
        Survey_Company_BAO companyBusinessObject = new Survey_Company_BAO();
        var dataTableCompany = companyBusinessObject.GetdtCompanyList(GetCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dataTableCompany;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();
    }

    /// <summary>
    /// Generate dynamic query.
    /// </summary>
    /// <returns></returns>
    public string GetCondition()
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
    /// Bind company on the basis of project.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCompany();
    }
}
