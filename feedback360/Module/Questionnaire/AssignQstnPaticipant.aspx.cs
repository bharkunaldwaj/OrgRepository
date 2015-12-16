using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Miscellaneous;
using Admin_BE;
using System.Net.Mail;

public partial class Module_Questionnaire_AssignQstnPaticipant : CodeBehindBase
{
    //Global variables.
    // int i;
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string stringName = string.Empty;
    //bool flag = true;
    //int j;
    //string file1;
    //string filename1;
    string expression1;
    //string expression2;
    string Finalexpression;
    //string Finalexpression2;
    //string expression6;
    //string Finalexpression6;
    string email;
    string Template;
    string finalemail;
    //string TemplateLink;
    //string ProjectTitle;
    //string ParticipantEmailID;
    //string ParticipantFirstName;
    string Subject;
    //string Questionnaire_id;
    //string mailid;
    WADIdentity identity;
    DataTable CompanyName;
    //DataTable dtAllAccount;
    //string expression5;
    //string Finalexpression5;

    StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        if (!IsPostBack)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            identity = this.Page.User.Identity as WADIdentity;

            AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
            Project_BAO projectBusinessAccessObject = new Project_BAO();
            //get project details by useraccount id.
            ddlProject.DataSource = projectBusinessAccessObject.GetAccountProject(Convert.ToInt32(identity.User.AccountID));
            ddlProject.DataTextField = "Title";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //get user account list.
            ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

            //If user is a Super Admin then show account detail section else hide.
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
        }
    }
    
    /// <summary>
    /// Save candidate list 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            lblMessage.Text = "";
            lblvalidation.Text = "";

            AssignQuestionnaire_BE assignquestionnaireBusinessEntity = new AssignQuestionnaire_BE();
            AssignQstnParticipant_BAO assignquestionnaireBusinessAccessObject = new AssignQstnParticipant_BAO();
            AssignQuestionnaire_BAO assignquestionnaireTempleteBusinessAccessObject = new AssignQuestionnaire_BAO();

            assignquestionnaireBusinessEntity.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
            assignquestionnaireBusinessEntity.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
            assignquestionnaireBusinessEntity.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

            assignquestionnaireBusinessEntity.Description = txtDescription.Text.Trim();

            identity = this.Page.User.Identity as WADIdentity;

            //IF user is super Admin then user account drop down value else user account id.
            if (identity.User.GroupID == 1)
            {
                assignquestionnaireBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                assignquestionnaireBusinessEntity.AccountID = identity.User.AccountID;
            }

            if (txtCandidateNo.Text.Trim() != "" || txtCandidateNo.Text.Trim() == "0")
            {//set number of colleagues
                assignquestionnaireBusinessEntity.CandidateNo = Convert.ToInt32(txtCandidateNo.Text.Trim());
            }
            assignquestionnaireBusinessEntity.ModifiedBy = 1;
            assignquestionnaireBusinessEntity.ModifiedDate = DateTime.Now;
            assignquestionnaireBusinessEntity.IsActive = 1;

            assignquestionnaireBusinessEntity.AssignmentUserDetails = GetCandidateList();

            if (assignquestionnaireBusinessEntity.AssignmentUserDetails.Count == assignquestionnaireBusinessEntity.CandidateNo || assignquestionnaireBusinessEntity.AssignmentUserDetails.Count != 0)
            {
                //Save Assign questionnaire
                Int32 assignmentID = assignquestionnaireBusinessAccessObject.AddAssignQuestionnaire(assignquestionnaireBusinessEntity);

                DataTable dtResult = new DataTable();
                dtResult = assignquestionnaireBusinessAccessObject.GetdtAssignQuestionnaireList(assignmentID);

                string imagepath = Server.MapPath("~/EmailImages/"); //ConfigurationSettings.AppSettings["EmailImagePath"].ToString();

                //Patch for duplicate emails

                //DataTable dtClone = dtResult.Clone();

                //string strUserList = "'',";

                //foreach (AccountUser_BE acUserBE in assignquestionnaire_BE.AssignmentUserDetails)
                //{
                //    strUserList = strUserList + "'" + acUserBE.FirstName + "',";
                //}

                //strUserList = strUserList.TrimEnd(',');
                //DataRow[] result = dtResult.Select("FirstName IN ('" + strUserList + "')");

                //foreach (DataRow dr in result)
                //    dtClone.ImportRow(dr);

                //dtResult = dtClone;

                //Patch for duplicate emails

                //Send mail to candidates
                for (int i = 0; i < assignquestionnaireBusinessEntity.CandidateNo; i++)
                {
                    Programme_BAO programmeBusinessAccessObject = new Programme_BAO();
                    List<Programme_BE> listProgramme = new List<Programme_BE>();
                    listProgramme = programmeBusinessAccessObject.GetProgrammeByID(Convert.ToInt32(assignquestionnaireBusinessEntity.AccountID), Convert.ToInt32(assignquestionnaireBusinessEntity.ProgrammeID));

                    AccountUser_BAO accountUserBusinessAccessObject = new AccountUser_BAO();
                    DataTable dataTableAccountAdmin = new DataTable();
                    dataTableAccountAdmin = accountUserBusinessAccessObject.GetdtAccountAdmin(Convert.ToInt32(assignquestionnaireBusinessEntity.AccountID));

                    Template = assignquestionnaireTempleteBusinessAccessObject.FindParticipantTemplate(Convert.ToInt32(ddlProject.SelectedValue));
                    Subject = assignquestionnaireTempleteBusinessAccessObject.FindParticipantSubjectTemplate(Convert.ToInt32(ddlProject.SelectedValue));

                    // Get Candidate Email Image Name & Will Combined with EmailImagePath
                    DataTable dataTableCandidateEmailImage = new DataTable();
                    string emailimagepath = "";
                    dataTableCandidateEmailImage = assignquestionnaireTempleteBusinessAccessObject.GetParticipantEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));

                    if (dataTableCandidateEmailImage.Rows.Count > 0 && dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                        emailimagepath = imagepath + dataTableCandidateEmailImage.Rows[0]["EmailImage"].ToString();

                    string Title = "";
                    string EmailID = "";
                    string FirstName = "";
                    string Loginid = "";
                    string password = "";
                    string Accountcode = "";

                    Title = dtResult.Rows[i]["Title"].ToString();
                    EmailID = dtResult.Rows[i]["EmailID"].ToString();
                    FirstName = dtResult.Rows[i]["FirstName"].ToString();
                    Loginid = dtResult.Rows[i]["LoginID"].ToString();
                    password = dtResult.Rows[i]["Password"].ToString();
                    Accountcode = dtResult.Rows[i]["Code"].ToString();

                    string urlPath = ConfigurationManager.AppSettings["ParticipantURL"].ToString();

                    /*Change History 
                     * Author : Rudra Prakash Mishra
                     * Date : 02/06/2014
                     * Reason: To bypass login screen for the participants
                     */

                    //Set Quesstionnaire url path 
                    urlPath += "Login.aspx?" + Utilities.CreateEncryptedQueryString(Loginid, password, Accountcode);
                    //Create a questionnare link.
                    string link = "<a Target='_BLANK' href= '" + urlPath + "' >Click Link</a> ";

                    Template = Template.Replace("[LINK]", link);
                    Template = Template.Replace("[TITLE]", Title);
                    Template = Template.Replace("[EMAILID]", EmailID);
                    Template = Template.Replace("[FIRSTNAME]", FirstName);
                    Template = Template.Replace("[LOGINID]", Loginid);
                    Template = Template.Replace("[PASSWORD]", password);
                    Template = Template.Replace("[CODE]", Accountcode);
                    Template = Template.Replace("[IMAGE]", "<img src=cid:companylogo>");

                    Subject = Subject.Replace("[TITLE]", Title);
                    Subject = Subject.Replace("[EMAILID]", EmailID);
                    Subject = Subject.Replace("[FIRSTNAME]", FirstName);
                    Subject = Subject.Replace("[LOGINID]", Loginid);
                    Subject = Subject.Replace("[PASSWORD]", password);
                    Subject = Subject.Replace("[CODE]", Accountcode);

                    if (listProgramme.Count > 0)
                    {
                        Template = Template.Replace("[STARTDATE]", Convert.ToDateTime(listProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                        Template = Template.Replace("[CLOSEDATE]", Convert.ToDateTime(listProgramme[0].EndDate).ToString("dd-MMM-yyyy"));

                        Subject = Subject.Replace("[STARTDATE]", Convert.ToDateTime(listProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                        Subject = Subject.Replace("[CLOSEDATE]", Convert.ToDateTime(listProgramme[0].EndDate).ToString("dd-MMM-yyyy"));
                    }

                    if (dataTableAccountAdmin.Rows.Count > 0)
                    {
                        Template = Template.Replace("[ACCOUNTADMIN]", dataTableAccountAdmin.Rows[0]["FullName"].ToString());
                        Template = Template.Replace("[ADMINEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                        Subject = Subject.Replace("[ACCOUNTADMIN]", dataTableAccountAdmin.Rows[0]["FullName"].ToString());
                        Subject = Subject.Replace("[ADMINEMAIL]", dataTableAccountAdmin.Rows[0]["EmailID"].ToString());

                        //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FullName"].ToString());
                        MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");
                        //Send Mail
                        SendEmail.Send(Subject, Template, EmailID, maddr, emailimagepath);
                    }
                    else
                    {
                        Template = Template.Replace("[ACCOUNTADMIN]", "Account Admin");
                        Template = Template.Replace("[ADMINEMAIL]", "");
                        Template = Template.Replace("<img src=cid:companylogo>", "");

                        Subject = Subject.Replace("[ACCOUNTADMIN]", "Account Admin");
                        Subject = Subject.Replace("[ADMINEMAIL]", "");
                        //Send Email
                        SendEmail.Send(Subject, Template, EmailID, "");
                    }
                }

                lblMessage.Text = "Questionnnaire assigned successfully";
                //Set controls default value.
                ddlProject.SelectedIndex = 0;
                ddlProgramme.SelectedIndex = 0;
                ddlQuestionnaire.SelectedIndex = 0;

                txtDescription.Text = "";

                txtCandidateNo.Text = "";
                //Bind candidate repeter.
                rptrCandidateList.DataSource = null;
                rptrCandidateList.DataBind();
            }
            else
            {
                lblvalidation.Text = "Please  fill Participants' information";
            }

            //foreach (string to in finalemail.Split(';'))
            //{


            //    Questionnaire_id = QueryStringModule.Encrypt(ddlQuestionnaire.SelectedValue);
            //    mailid = QueryStringModule.Encrypt(to);

            //    string link = "<a Target='_BLANK' href= 'EmailTemplatesList.aspx?Questionnaireid=" + Questionnaire_id + "&Emailid =" + mailid + "' >Click Link</a> ";

            //    Template = Template.Replace("[Link]", link);

            //    SendEmail.Send("Your Questionnaire", Template, to);

            //}


            //SendEmail.Send("Your Questionnaire", Template, finalemail);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    
    /// <summary>
    /// Reset control value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbReset_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            ddlProject.SelectedIndex = 0;
            ddlQuestionnaire.SelectedIndex = 0;
            ddlProgramme.SelectedIndex = 0;
            txtDescription.Text = "";

            txtCandidateNo.Text = "";
            lblMessage.Text = "";
            lblvalidation.Text = "";

            rptrCandidateList.DataSource = null;
            rptrCandidateList.DataBind();

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind candidate grid view with numberof canidate mention
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbSubmit_Click(object sender, ImageClickEventArgs e)
    {
        lblMessage.Text = "";
        lblvalidation.Text = "";

        if (txtCandidateNo.Text.Trim() != "")
        {
            int candidateCount = Convert.ToInt32(txtCandidateNo.Text.Trim());
            BindCandidateList(candidateCount);
        }
    }

    /// <summary>
    /// Bind candidate grid with candidate count
    /// </summary>
    /// <param name="candidateCount"></param>
    private void BindCandidateList(int candidateCount)
    {
        try
        {
            //Initilize table and bind candidate grid.
            DataTable dtCandidate = new DataTable();
            dtCandidate.Columns.Add("Relationship");
            dtCandidate.Columns.Add("Name");
            dtCandidate.Columns.Add("EmailID");

            for (int count = 0; count < candidateCount; count++)
                dtCandidate.Rows.Add("", "", "");
            //bind candidate grid.
            rptrCandidateList.DataSource = dtCandidate;
            rptrCandidateList.DataBind();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind Questionnaire and program according to project
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Questionnaire_BAO.Questionnaire_BAO questionnaireBusinessAccessObject = new Questionnaire_BAO.Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dtQuestionnaire = new DataTable();
        //get Questionnaire details project id
        dtQuestionnaire = questionnaireBusinessAccessObject.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtQuestionnaire.Rows.Count > 0)
        {
            //bind Questionnaire dropdown list
            ddlQuestionnaire.DataSource = dtQuestionnaire;
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();
        }

        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));

        if (ddlQuestionnaire.Items.Count > 1)
            ddlQuestionnaire.Items[1].Selected = true;

        Programme_BAO programmeBusinessAccessObject = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dataTableProgramme = new DataTable();
        //get programme details project id
        dataTableProgramme = programmeBusinessAccessObject.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dataTableProgramme.Rows.Count > 0)
        {
            //bind programme  dropdown list
            ddlProgramme.DataSource = dataTableProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));

        if (ddlProgramme.Items.Count > 1)
            ddlProgramme.Items[1].Selected = true;

    }

    /// <summary>
    /// Upload image
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgUpload_click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string constr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection connection = new SqlConnection(constr);

            if (FileUpload1.HasFile)
            {
                if (this.IsFileValid(this.FileUpload1))
                {
                    string filename = "";
                    string file = "";
                    //Get Uploaded file name.
                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                    //Get Unique name for file
                    file = GetUniqueFilename(filename);

                    Session["FinalName"] = file;

                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename);//Save file.

                    //filename = FileUpload1.FileName;

                    //filename = FileUpload1.PostedFile.FileName;

                    DataTable dataTableProspective = new DataTable();

                    dataTableProspective = ReturnExcelDataTable(filename);

                    if (dataTableProspective != null && dataTableProspective.Rows.Count > 0)
                    {
                        txtCandidateNo.Text = Convert.ToString(dataTableProspective.Rows.Count);

                        //foreach (RepeaterItem item in rptrCandidateList.Items)
                        //{
                        //    TextBox txtRelation = (TextBox)item.FindControl("ddlYearInsured");
                        //    TextBox txtCandidate = (TextBox)item.FindControl("ddlCompanyName");
                        //    TextBox txtCandEmail = (TextBox)item.FindControl("txtCompAddress");


                        //}

                        rptrCandidateList.DataSource = dataTableProspective;
                        rptrCandidateList.DataBind();

                        lblMessage.Text = "";

                        int candidateCount = Convert.ToInt32(txtCandidateNo.Text.Trim());

                        for (int i = 0; i < candidateCount; i++)
                        {
                            email += dataTableProspective.Rows[i]["EmailID"].ToString() + ";";
                        }

                        finalemail = email.TrimEnd(';');
                    }
                    else
                    {
                        errorMessage(filename);
                    }

                    File.Delete(filename);
                }
                else
                {
                    lblMessage.Text = "Invalid file type";
                    // Page.RegisterStartupScript("FileTyp", "<script language='JavaScript'>alert('Invalid file type');</script>");
                }
            }
            else
            {
                lblvalidation.Text = "Please browse file to upload";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Check if uploaded file is valid or not according to size and extension
    /// </summary>
    /// <param name="uploadControl"></param>
    /// <returns></returns>
    protected bool IsFileValid(FileUpload uploadControl)
    {
        bool isFileOk = true;
        try
        {
            string FileExt = ".xls,.xlsx,.XLS,.XLSX";
            string[] AllowedExtensions = FileExt.Split(',');

            bool isExtensionError = false;

            int MaxSizeAllowed = 5 * 1048576;// Size Allow only in mb
            if (uploadControl.PostedFile != null)
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
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    protected void lnkError_Click(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Session["FinalName"].ToString() + ".txt");
        Response.WriteFile(Server.MapPath("~") + "//UploadDocs//" + Session["FinalName"].ToString() + ".txt");
        Response.End();
    }
    
    /// <summary>
    /// Bind company list by company  code and bind project dropdown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {

            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get company list in account
            CompanyName = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            DataTable dataTableAccount = CompanyName.Clone();

            foreach (DataRow dataRowAccount in resultsAccount)
            {
                dataTableAccount.ImportRow(dataRowAccount);
            }
            //set company value.
            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();

            Project_BAO projectBusinessAccessObject = new Project_BAO();

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));
            //get project by company code and bind project dropdown.
            ddlProject.DataSource = projectBusinessAccessObject.GetAccountProject(companycode);
            ddlProject.DataTextField = "Title";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";
        }
    }

    public void RegisterPostbackTrigger(Control triggerOn)
    {
        ScriptManager1.RegisterPostBackControl(triggerOn);
    }

    /// <summary>
    /// Generate unique name for uploaded questionnaire logo.
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
    /// Get Candidate list 
    /// </summary>
    /// <returns></returns>
    private List<AccountUser_BE> GetCandidateList()
    {
        List<AccountUser_BE> assignmentDetailsBusinesEntityList = new List<AccountUser_BE>();

        bool flag = true;

        foreach (RepeaterItem item in rptrCandidateList.Items)
        {
            TextBox textBoxFirstName = (TextBox)item.FindControl("txtFirstName");
            TextBox textBoxLastName = (TextBox)item.FindControl("txtLastName");
            TextBox textBoxCandidateEmail = (TextBox)item.FindControl("txtEmailID");

            if (textBoxFirstName.Text == "" || textBoxLastName.Text == "" || textBoxCandidateEmail.Text == "")
            {
                flag = false;
            }
        }

        if (flag != false)
        {
            //Initilize property from candidate list grid.
            foreach (RepeaterItem item in rptrCandidateList.Items)
            {
                AccountUser_BE assignmentDetailsBusinessEntity = new AccountUser_BE();

                TextBox textBoxFirstName = (TextBox)item.FindControl("txtFirstName");
                TextBox textBoxLastName = (TextBox)item.FindControl("txtLastName");
                TextBox textBoxCandidateEmail = (TextBox)item.FindControl("txtEmailID");
                //AccountUser_BAO maxid = new AccountUser_BAO();
                //int max =  maxid.MaxUser();
                //max = max + 1;
                //string username = txtFirstName.Text.Trim();
                //username = username + max;
                //string password = txtFirstName.Text.Trim();
                //password = password + max;

                assignmentDetailsBusinessEntity.FirstName = textBoxFirstName.Text.Trim();
                assignmentDetailsBusinessEntity.LastName = textBoxLastName.Text.Trim();
                assignmentDetailsBusinessEntity.EmailID = textBoxCandidateEmail.Text.Trim();
                assignmentDetailsBusinessEntity.IsActive = 1;
                assignmentDetailsBusinessEntity.LoginID = textBoxFirstName.Text.Trim();
                assignmentDetailsBusinessEntity.Password = textBoxFirstName.Text.Trim();
                assignmentDetailsBusinessEntity.Salutation = "Mr.";
                assignmentDetailsBusinessEntity.GroupID = Convert.ToInt32(ConfigurationManager.AppSettings["ParticipantRoleID"]);
                assignmentDetailsBusinessEntity.Notification = true;
                assignmentDetailsBusinessEntity.ModifyDate = DateTime.Now;
                assignmentDetailsBusinessEntity.ModifyBy = 1;
                assignmentDetailsBusinessEntity.StatusID = 1;

                identity = this.Page.User.Identity as WADIdentity;

                //If user Is super admin then user account Id else user account id.
                if (identity.User.GroupID == 1)
                {
                    assignmentDetailsBusinessEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                }
                else
                {
                    assignmentDetailsBusinessEntity.AccountID = identity.User.AccountID;
                }

                email += textBoxCandidateEmail.Text.Trim() + ";";

                finalemail = email.TrimEnd(';');

                assignmentDetailsBusinesEntityList.Add(assignmentDetailsBusinessEntity);
            }
        }
        //return list.
        return assignmentDetailsBusinesEntityList;
    }

    /// <summary>
    /// Validae upload control with error massage.
    /// </summary>
    /// <param name="filename"></param>
    private void errorMessage(string filename)
    {

        lblMessage.Text = "Upload Failed.Please fill the Correct Field Value";
    }

    /// <summary>
    /// Convert excel to datatable
    /// </summary>
    /// <param name="FullFileNamePath"></param>
    /// <returns></returns>
    private DataTable ReturnExcelDataTable(string FullFileNamePath)
    {
        //DataTable dtExcel;
        DateTime dt3 = new DateTime();

        string SheetName = string.Empty;
        try
        {
            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;

            int index = 0;
            object rowIndex = 2;

            DataTable dataTableExcel = new DataTable();

            dataTableExcel.Columns.Add("Relationship", typeof(string));
            dataTableExcel.Columns.Add("Name", typeof(string));
            dataTableExcel.Columns.Add("EmailID", typeof(string));

            DataRow row;

            try
            {
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    //rowIndex = 2 + index;
                    row = dataTableExcel.NewRow();

                    //Add rows witj excel values
                    row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                    row[2] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);

                    index++;
                    rowIndex = 2 + index;
                    dataTableExcel.Rows.Add(row);
                }
            }
            catch
            {
                lblMessage.Text = "Please check your file data.";
                dataTableExcel = null;
            }

            app.Workbooks.Close();//Close excel

            return dataTableExcel;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
