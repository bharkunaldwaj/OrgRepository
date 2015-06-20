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

public partial class Survey_Module_Questionnaire_Programme : CodeBehindBase
{

    Survey_Programme_BAO programme_BAO = new Survey_Programme_BAO();
    Survey_Programme_BE programme_BE = new Survey_Programme_BE();
    Survey_Programme_BE2 programme_BE2 = new Survey_Programme_BE2();

    List<Survey_Programme_BE> programme_BEList = new List<Survey_Programme_BE>();
    DataSet dsprogramme_Analysis_List = new DataSet();
    WADIdentity identity;
    DataTable dtCompanyName;  //,dtCategory;
    DataTable dtAnalysis1;
    DataTable dtAnalysis2;
    DataTable dtAnalysis3;





    string filename;
    string file = null;

    protected void Page_Load(object sender, EventArgs e)
    {


        //try
        //{
        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
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
                programme_BEList = programme_BAO.GetProgrammeByID(Convert.ToInt32(identity.User.AccountID), programmeID);

            }


            Survey_Project_BAO project_BAO = new Survey_Project_BAO();
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
                ddlAccountCode.SelectedValue = identity.User.AccountID.ToString();
                ddlAccountCode_SelectedIndexChanged(sender, e);
            }




            if (programme_BEList.Count > 0)
            {
               
                ddlAccountCode_SelectedIndexChanged(sender, e);

                SetProgrammeValue(programme_BEList);
            }

        }

        //HandleWriteLog("Start", new StackTrace(true));
        //}
        //catch (Exception ex)
        //{
        //    HandleException(ex);
        //}
    }

    private void SetProgrammeValue(List<Survey_Programme_BE> programme_BEList)
    {
        //try
        //{
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

                //lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
            }
            //else
            //{
            //    lblcompanyname.Text = "";
            //}
        }

        txtName.Text = programme_BEList[0].ProgrammeName;
        txtDescription.Text = programme_BEList[0].ProgrammeDescription;
        txtClientName.Text = programme_BEList[0].ClientName;
        hdnimage.Value = programme_BEList[0].Logo;

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();

        ddlProject.DataSource = project_BAO.GetdtProjectList(ddlAccountCode.SelectedValue);
        ddlProject.DataValueField = "ProjectID";
        ddlProject.DataTextField = "Title";
        ddlProject.DataBind();

        ddlProject.SelectedValue = programme_BEList[0].ProjectID.ToString();
        ddlProject_SelectedIndexChanged(ddlProject, null);
        if (!string.IsNullOrEmpty(programme_BEList[0].CompanyID.ToString()))
            ddlCompany.SelectedValue = programme_BEList[0].CompanyID.ToString();
        //}




        //protected void Page_Init(object sender, EventArgs e)
        //{
        dtStartDate.Text = Convert.ToDateTime(programme_BEList[0].StartDate).ToString("dd/MM/yyyy");
        dtEndDate.Text = Convert.ToDateTime(programme_BEList[0].EndDate).ToString("dd/MM/yyyy");
        dtRemainderDate1.Text = Convert.ToDateTime(programme_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");

        dtRemainderDate2.Text = Convert.ToDateTime(programme_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
        if (dtRemainderDate2.Text == "01/01/2000")
            dtRemainderDate2.Text = "";

        dtRemainderDate3.Text = Convert.ToDateTime(programme_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
        if (dtRemainderDate3.Text == "01/01/2000")
            dtRemainderDate3.Text = "";



        txtStartDate.Text = Convert.ToDateTime(programme_BEList[0].StartDate).ToString("dd/MM/yyyy");
        txtEndDate.Text = Convert.ToDateTime(programme_BEList[0].EndDate).ToString("dd/MM/yyyy");
        txtRemainderDate1.Text = Convert.ToDateTime(programme_BEList[0].Reminder1Date).ToString("dd/MM/yyyy");

        txtRemainderDate2.Text = Convert.ToDateTime(programme_BEList[0].Reminder2Date).ToString("dd/MM/yyyy");
        if (txtRemainderDate2.Text == "01/01/2000")
            txtRemainderDate2.Text = "";

        txtRemainderDate3.Text = Convert.ToDateTime(programme_BEList[0].Reminder3Date).ToString("dd/MM/yyyy");
        if (txtRemainderDate3.Text == "01/01/2000")
            txtRemainderDate3.Text = "";



        if (String.IsNullOrEmpty(programme_BEList[0].Analysis_I_Name) != true)
            Txt_name_Analysis1.Text = programme_BEList[0].Analysis_I_Name.ToString();



        if (String.IsNullOrEmpty(programme_BEList[0].Analysis_I_Category.ToString()) != true)
            txt_catagory_Analysis1.Text = Convert.ToInt32(programme_BEList[0].Analysis_I_Category).ToString();

        if (String.IsNullOrEmpty(programme_BEList[0].Analysis_II_Name) != true)
            txt_name_Analysis2.Text = programme_BEList[0].Analysis_II_Name.ToString();

        if (String.IsNullOrEmpty(programme_BEList[0].Analysis_II_Category.ToString()) != true)
            txt_category_Analysis2.Text = Convert.ToInt32(programme_BEList[0].Analysis_II_Category).ToString();

        if (String.IsNullOrEmpty(programme_BEList[0].Analysis_III_Name) != true)
            txt_name_Analysis3.Text = programme_BEList[0].Analysis_III_Name.ToString();

        if (String.IsNullOrEmpty(programme_BEList[0].Analysis_III_Category.ToString()) != true)
            txt_category_Analysis3.Text = Convert.ToInt32(programme_BEList[0].Analysis_III_Category).ToString();

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
            dtAnalysis1 = programme_BAO.GetAnalysis1(programmeID);
            Repeater0.DataSource = dtAnalysis1;
            Repeater0.DataBind();
            Repeater0.Visible = true;

            dtAnalysis2 = programme_BAO.GetAnalysis2(programmeID);
            Repeater1.DataSource = dtAnalysis2;
            Repeater1.DataBind();
            Repeater1.Visible = true;

            dtAnalysis3 = programme_BAO.GetAnalysis3(programmeID);
            Repeater2.DataSource = dtAnalysis3;
            Repeater2.DataBind();
            Repeater2.Visible = true;

        }


        //HandleWriteLog("Start", new StackTrace(true));
        //}
        //catch (Exception ex)
        //{
        //    HandleException(ex);
        //}

    }

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
                Survey_Programme_BE programme_BE = new Survey_Programme_BE();
                //Programme_BE programme_BE = new Programme_BE();

                //Programm

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

                programme_BE.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);

                if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
                    programme_BE.CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);

                programme_BE.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim().ToString());
                programme_BE.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim().ToString());
                programme_BE.Reminder1Date = Convert.ToDateTime(txtRemainderDate1.Text.Trim().ToString());

                if (txtRemainderDate2.Text.Trim() != "")
                    programme_BE.Reminder2Date = Convert.ToDateTime(txtRemainderDate2.Text.Trim().ToString());
                else
                    programme_BE.Reminder2Date = Convert.ToDateTime("01/01/2000");

                if (txtRemainderDate3.Text.Trim() != "")
                    programme_BE.Reminder3Date = Convert.ToDateTime(txtRemainderDate3.Text.Trim().ToString());
                else
                    programme_BE.Reminder3Date = Convert.ToDateTime("01/01/2000");

                programme_BE.ProgrammeID = 0;
                programme_BE.ModifyBy = 1;
                programme_BE.ModifyDate = DateTime.Now;
                programme_BE.IsActive = 1;

                programme_BE.Analysis_I_Name = Txt_name_Analysis1.Text.Trim();


                if (String.IsNullOrEmpty(txt_catagory_Analysis1.Text) == true)
                { }
                else
                    programme_BE.Analysis_I_Category = Convert.ToInt32(txt_catagory_Analysis1.Text.Trim().ToString());

                programme_BE.Analysis_II_Name = txt_name_Analysis2.Text.Trim();

                if (String.IsNullOrEmpty(txt_category_Analysis2.Text) == true)
                { }
                else
                    programme_BE.Analysis_II_Category = Convert.ToInt32(txt_category_Analysis2.Text);

                programme_BE.Analysis_III_Name = txt_name_Analysis3.Text.Trim();

                if (String.IsNullOrEmpty(txt_category_Analysis3.Text) == true)
                { }
                else
                    programme_BE.Analysis_III_Category = Convert.ToInt32(txt_category_Analysis3.Text.Trim());

                int prog_id = 0;
                if (Request.QueryString["Mode"] == "E")
                {
                    prog_id = Convert.ToInt32(Request.QueryString["PrgId"]);
                    programme_BE.ProgrammeID = Convert.ToInt32(Request.QueryString["PrgId"]);
                    programme_BAO.UpdateProgramme(programme_BE);

                }
                else
                {
                    prog_id = programme_BAO.AddProgramme(programme_BE);

                }

                ////Response.Write(prog_id);

                string[] Analysis_type = { Label9.Text, Label10.Text, Label12.Text.Trim() };
                string[] name_of_category = { Txt_name_Analysis1.Text.Trim(), txt_name_Analysis2.Text.Trim(), txt_name_Analysis3.Text.Trim() };
                if (txt_catagory_Analysis1.Text == "")
                    txt_catagory_Analysis1.Text = "0";
                if (txt_catagory_Analysis1.Text == "")
                    txt_category_Analysis2.Text = "0";
                if (txt_catagory_Analysis1.Text == "")
                    txt_category_Analysis3.Text = "0";



                TextBox category_txt;


                string Category_Detail_list = "";
                string Analysis_Type_list = "";
                string Category_Name_list = "";


                foreach (RepeaterItem item in Repeater0.Items)
                {

                    category_txt = (TextBox)item.FindControl("txt_category");
                    if (category_txt.Text == "")
                        category_txt.Text = "   ";

                    Category_Detail_list = Category_Detail_list + category_txt.Text.Trim() + ",";
                    Analysis_Type_list = Analysis_Type_list + Analysis_type[0] + ",";
                    Category_Name_list = Category_Name_list + name_of_category[0] + ",";
                }



                foreach (RepeaterItem item1 in Repeater1.Items)
                {
                    category_txt = (TextBox)item1.FindControl("txt_category");
                    if (category_txt.Text == "")
                        category_txt.Text = "   ";

                    Category_Detail_list = Category_Detail_list + category_txt.Text.Trim() + ",";
                    Analysis_Type_list = Analysis_Type_list + Analysis_type[1] + ",";
                    Category_Name_list = Category_Name_list + name_of_category[1] + ",";
                }


                foreach (RepeaterItem item2 in Repeater2.Items)
                {
                    category_txt = (TextBox)item2.FindControl("txt_category");
                    if (category_txt.Text == "")
                        category_txt.Text = "   ";

                    Category_Detail_list = Category_Detail_list + category_txt.Text.Trim() + ",";
                    Analysis_Type_list = Analysis_Type_list + Analysis_type[2] + ",";
                    Category_Name_list = Category_Name_list + name_of_category[2] + ",";
                }




                int rr = programme_BAO.save_category_for_analysis_list(prog_id, Category_Detail_list, Analysis_Type_list, Category_Name_list);
                if (rr >= 1)
                {
                    lbl_save_message.Visible = true;

                    txtName.Text = txtDescription.Text = txtClientName.Text = dtStartDate.Text = dtEndDate.Text = dtRemainderDate1.Text = dtRemainderDate2.Text = dtRemainderDate3.Text = Txt_name_Analysis1.Text = txt_catagory_Analysis1.Text = txt_name_Analysis2.Text = txt_category_Analysis2.Text = txt_name_Analysis3.Text = txt_category_Analysis3.Text = "";

                    Repeater0.Visible = Repeater1.Visible = Repeater2.Visible = false;
                    ddlProject.ClearSelection();
                    rr = 0;

                }
                lbl_save_message.Text = "Survey Programme Saved Successfully";
                Response.Redirect("ProgrammeList.aspx", false);
            }
            //}

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
        Survey_Project_BAO project_BAO = new Survey_Project_BAO();

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

            //lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            ddlProject.DataSource = project_BAO.GetdtProjectList(ddlAccountCode.SelectedValue);
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataTextField = "Title";
            ddlProject.DataBind();

        }
        else
        {
            //lblcompanyname.Text = "";

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



        Survey_Project_BAO project_BAO = new Survey_Project_BAO();

        List<Survey_Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(projectInfo[0].EmailTMPLReminder2);

        if (projectInfo[0].EmailTMPLReminder2 != 0 && txtRemainderDate2.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;

        }




    }

    protected void ValCusReminder3(object source, ServerValidateEventArgs args)
    {



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



        Survey_Project_BAO project_BAO = new Survey_Project_BAO();

        List<Survey_Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        hdnReminder2.Value = Convert.ToString(projectInfo[0].EmailTMPLReminder2);

        if (projectInfo[0].EmailTMPLReminder3 != 0 && txtRemainderDate3.Text == "")
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;

        }




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

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        List<Survey_Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        //if (projectInfo[0].EmailTMPLReportAvalibale != 0 && txtAvailableFrom.Text == "")
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;

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

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        List<Survey_Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //if (projectInfo[0].EmailTMPLReportAvalibale != 0 && txtAvailableTo.Text == "")
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;
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

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        List<Survey_Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

        //hdnPartReminder1.Value = Convert.ToString(projectInfo[0].EmailTMPPartReminder1);

        //if (projectInfo[0].EmailTMPPartReminder1 != 0 && txtPartReminder1.Text == "")
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;

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

        Survey_Project_BAO project_BAO = new Survey_Project_BAO();
        List<Survey_Project_BE> projectInfo = project_BAO.GetProjectByID(Accountid, Convert.ToInt32(ddlProject.SelectedValue));

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
        Page.RegisterClientScriptBlock( "test", "ResetDTPickerDate('" + HtmlDate + "','" + aspDate + "');");

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


            DataTable dtCategory = new DataTable();
            Repeater0.Visible = true;
            if (txt_catagory_Analysis1.Text.Trim() != "" || txt_catagory_Analysis1.Text.Trim() != Convert.ToString(0))
                dtCategory = BindCandidateList("A1", Convert.ToInt32(txt_catagory_Analysis1.Text.Trim()));
            Repeater0.DataSource = dtCategory;
            Repeater0.DataBind();
        }
        //}
    }

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

            DataTable dtCategory = new DataTable();
            Repeater1.Visible = true;
            if (txt_category_Analysis2.Text.Trim() != "" || txt_category_Analysis2.Text.Trim() != Convert.ToString(0))
                dtCategory = BindCandidateList("A2", Convert.ToInt32(txt_category_Analysis2.Text.Trim()));
            Repeater1.DataSource = dtCategory;
            Repeater1.DataBind();
        }
        //}
    }
    protected void imbSubmit3_Click(object sender, ImageClickEventArgs e)
    {
        //Page.Validate();
        //if (Page.IsValid)
        //{
        if (String.IsNullOrEmpty(txt_category_Analysis3.Text) == false)
        {
            if (Convert.ToInt32(txt_category_Analysis3.Text) >= 1)
            {
                Repeater0.Visible = true;
            }

            DataTable dtCategory = new DataTable();
            Repeater2.Visible = true;
            if (txt_category_Analysis3.Text.Trim() != "" || txt_category_Analysis3.Text.Trim() != Convert.ToString(0))
                dtCategory = BindCandidateList("A3", Convert.ToInt32(txt_category_Analysis3.Text.Trim()));
            Repeater2.DataSource = dtCategory;
            Repeater2.DataBind();
        }
        //}
    }



    private DataTable BindCandidateList(string AnalysisType, int candidateCount)
    {
        DataTable dtCandidate;
        dtCandidate = new DataTable();
        //    dtCandidate = null;

        string strtdate = dtStartDate.Text;
        int programmeID = 0;
        if (Request.QueryString["PrgId"] != null)
            programmeID = Convert.ToInt32(Request.QueryString["PrgId"]);
        if (programmeID > 0)
        {
            if (AnalysisType == "A1")
                dtCandidate = programme_BAO.GetAnalysis1(programmeID);
            else if (AnalysisType == "A2")
                dtCandidate = programme_BAO.GetAnalysis2(programmeID);
            else if (AnalysisType == "A3")
                dtCandidate = programme_BAO.GetAnalysis3(programmeID);
        }

        //if (dtCandidate != null)
        //{
        //dtCandidate.Columns.Add("Category_Detail");
        int intExistingrows = Convert.ToInt32(dtCandidate.Rows.Count);
        //int intExistingColumns = Convert.ToInt32(dtCandidate.Columns.Count);
        if (intExistingrows <= 0 && programmeID == 0)  // && intExistingColumns <= 0)
            dtCandidate.Columns.Add("Category_Detail");

        candidateCount = candidateCount - intExistingrows;
        for (int count = 0; count < candidateCount; count++)
        {
            dtCandidate.Rows.Add("");
        }
        //}
        return dtCandidate;

        //catch (Exception ex)
        //{
        //    //  HandleException(ex);
        //    return dtCandidate;
        //}
    }
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
    protected void dtEndDate_TextChanged(object sender, EventArgs e)
    {

    }

    private void fillCompany()
    {
        Survey_Company_BAO company_BAO = new Survey_Company_BAO();
        var dt = company_BAO.GetdtCompanyList(GetCondition());
        // ddlCompany.Items.Clear();
        ddlCompany.Items.Clear();
        ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        ddlCompany.DataSource = dt;
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "Title";
        ddlCompany.DataBind();
    }

    public string GetCondition()
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
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCompany();
    }
}
