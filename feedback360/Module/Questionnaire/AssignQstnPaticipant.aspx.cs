using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using DatabaseAccessUtilities;
using System.Diagnostics;
using Miscellaneous;
using DAF_BAO;
using Admin_BE;
using System.Net.Mail;

public partial class Module_Questionnaire_AssignQstnPaticipant : CodeBehindBase
{
    int i;
    string SqlType = string.Empty;
    string filePath = string.Empty;
    string strName = string.Empty;
    bool flag = true;
    int j;
    string file1;
    string filename1;
    string expression1;
    string expression2;
    string Finalexpression;
    string Finalexpression2;
    string expression6;
    string Finalexpression6;
    string email;
    string Template;
    string finalemail;
    string TemplateLink;
    string ProjectTitle;
    string ParticipantEmailID;
    string ParticipantFirstName;
    string Subject;
    string Questionnaire_id;
    string mailid;
    WADIdentity identity;
    DataTable CompanyName;
    DataTable dtAllAccount;
    string expression5;
    string Finalexpression5;

    StringBuilder sb = new StringBuilder();


    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        if (!IsPostBack)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            identity = this.Page.User.Identity as WADIdentity;

            AssignQstnParticipant_BAO assignquestionnaire = new AssignQstnParticipant_BAO();
            Project_BAO project_BAO = new Project_BAO();

            ddlProject.DataSource = project_BAO.GetAccountProject(Convert.ToInt32(identity.User.AccountID));
            ddlProject.DataTextField = "Title";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();

            Account_BAO account_BAO = new Account_BAO();
            ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
            ddlAccountCode.DataValueField = "AccountID";
            ddlAccountCode.DataTextField = "Code";
            ddlAccountCode.DataBind();

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

    public void RegisterPostbackTrigger(Control triggerOn)
    {
        ScriptManager1.RegisterPostBackControl(triggerOn);
    }



    protected void imbAssign_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            lblMessage.Text = "";
            lblvalidation.Text = "";

            AssignQuestionnaire_BE assignquestionnaire_BE = new AssignQuestionnaire_BE();
            AssignQstnParticipant_BAO assignquestionnaire_BAO = new AssignQstnParticipant_BAO();
            AssignQuestionnaire_BAO assignquestionnaireTemplete_BAO = new AssignQuestionnaire_BAO();

            assignquestionnaire_BE.ProjecctID = Convert.ToInt32(ddlProject.SelectedValue);
            assignquestionnaire_BE.ProgrammeID = Convert.ToInt32(ddlProgramme.SelectedValue);
            assignquestionnaire_BE.QuestionnaireID = Convert.ToInt32(ddlQuestionnaire.SelectedValue);

            assignquestionnaire_BE.Description = txtDescription.Text.Trim();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                assignquestionnaire_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                assignquestionnaire_BE.AccountID = identity.User.AccountID;
            }



            if (txtCandidateNo.Text.Trim() != "" || txtCandidateNo.Text.Trim() == "0")
            {
                assignquestionnaire_BE.CandidateNo = Convert.ToInt32(txtCandidateNo.Text.Trim());
            }
            assignquestionnaire_BE.ModifiedBy = 1;
            assignquestionnaire_BE.ModifiedDate = DateTime.Now;
            assignquestionnaire_BE.IsActive = 1;

            assignquestionnaire_BE.AssignmentUserDetails = GetCandidateList();

            if (assignquestionnaire_BE.AssignmentUserDetails.Count == assignquestionnaire_BE.CandidateNo || assignquestionnaire_BE.AssignmentUserDetails.Count != 0)
            {
                //Save Assign questionnaire
                Int32 assignmentID = assignquestionnaire_BAO.AddAssignQuestionnaire(assignquestionnaire_BE);

                DataTable dtResult = new DataTable();
                dtResult = assignquestionnaire_BAO.GetdtAssignQuestionnaireList(assignmentID);

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
                for (int i = 0; i < assignquestionnaire_BE.CandidateNo; i++)
                {
                    Programme_BAO programme_BAO = new Programme_BAO();
                    List<Programme_BE> lstProgramme = new List<Programme_BE>();
                    lstProgramme = programme_BAO.GetProgrammeByID(Convert.ToInt32(assignquestionnaire_BE.AccountID), Convert.ToInt32(assignquestionnaire_BE.ProgrammeID));

                    AccountUser_BAO accountUser_BAO = new AccountUser_BAO();
                    DataTable dtAccountAdmin = new DataTable();
                    dtAccountAdmin = accountUser_BAO.GetdtAccountAdmin(Convert.ToInt32(assignquestionnaire_BE.AccountID));

                    Template = assignquestionnaireTemplete_BAO.FindParticipantTemplate(Convert.ToInt32(ddlProject.SelectedValue));
                    Subject = assignquestionnaireTemplete_BAO.FindParticipantSubjectTemplate(Convert.ToInt32(ddlProject.SelectedValue));

                    // Get Candidate Email Image Name & Will Combined with EmailImagePath
                    DataTable dtCandidateEmailImage = new DataTable();
                    string emailimagepath = "";
                    dtCandidateEmailImage = assignquestionnaireTemplete_BAO.GetParticipantEmailImageInfo(Convert.ToInt32(ddlProject.SelectedValue));

                    if (dtCandidateEmailImage.Rows.Count > 0 && dtCandidateEmailImage.Rows[0]["EmailImage"].ToString() != "")
                        emailimagepath = imagepath + dtCandidateEmailImage.Rows[0]["EmailImage"].ToString();

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

                    urlPath += "Login.aspx?" + Utilities.CreateEncryptedQueryString(Loginid, password, Accountcode);

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

                    if (lstProgramme.Count > 0)
                    {
                        Template = Template.Replace("[STARTDATE]", Convert.ToDateTime(lstProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                        Template = Template.Replace("[CLOSEDATE]", Convert.ToDateTime(lstProgramme[0].EndDate).ToString("dd-MMM-yyyy"));

                        Subject = Subject.Replace("[STARTDATE]", Convert.ToDateTime(lstProgramme[0].StartDate).ToString("dd-MMM-yyyy"));
                        Subject = Subject.Replace("[CLOSEDATE]", Convert.ToDateTime(lstProgramme[0].EndDate).ToString("dd-MMM-yyyy"));
                    }

                    if (dtAccountAdmin.Rows.Count > 0)
                    {
                        Template = Template.Replace("[ACCOUNTADMIN]", dtAccountAdmin.Rows[0]["FullName"].ToString());
                        Template = Template.Replace("[ADMINEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                        Subject = Subject.Replace("[ACCOUNTADMIN]", dtAccountAdmin.Rows[0]["FullName"].ToString());
                        Subject = Subject.Replace("[ADMINEMAIL]", dtAccountAdmin.Rows[0]["EmailID"].ToString());

                        //MailAddress maddr = new MailAddress(dtAccountAdmin.Rows[0]["EmailID"].ToString(), dtAccountAdmin.Rows[0]["FullName"].ToString());
                        MailAddress maddr = new MailAddress("admin@i-comment360.com", "360 feedback");
                        SendEmail.Send(Subject, Template, EmailID, maddr, emailimagepath);
                    }
                    else
                    {
                        Template = Template.Replace("[ACCOUNTADMIN]", "Account Admin");
                        Template = Template.Replace("[ADMINEMAIL]", "");
                        Template = Template.Replace("<img src=cid:companylogo>", "");

                        Subject = Subject.Replace("[ACCOUNTADMIN]", "Account Admin");
                        Subject = Subject.Replace("[ADMINEMAIL]", "");

                        SendEmail.Send(Subject, Template, EmailID, "");
                    }
                }

                lblMessage.Text = "Questionnnaire assigned successfully";

                ddlProject.SelectedIndex = 0;
                ddlProgramme.SelectedIndex = 0;
                ddlQuestionnaire.SelectedIndex = 0;

                txtDescription.Text = "";

                txtCandidateNo.Text = "";

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

    private List<AccountUser_BE> GetCandidateList()
    {
        List<AccountUser_BE> assignmentDetails_BEList = new List<AccountUser_BE>();


        bool flag = true;

        foreach (RepeaterItem item in rptrCandidateList.Items)
        {
            TextBox txtFirstName = (TextBox)item.FindControl("txtFirstName");
            TextBox txtLastName = (TextBox)item.FindControl("txtLastName");
            TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");

            if (txtFirstName.Text == "" || txtLastName.Text == "" || txtCandidateEmail.Text == "")
            {
                flag = false;
            }
        }
        if (flag != false)
        {
            foreach (RepeaterItem item in rptrCandidateList.Items)
            {
                AccountUser_BE assignmentDetails_BE = new AccountUser_BE();

                TextBox txtFirstName = (TextBox)item.FindControl("txtFirstName");
                TextBox txtLastName = (TextBox)item.FindControl("txtLastName");
                TextBox txtCandidateEmail = (TextBox)item.FindControl("txtEmailID");
                //AccountUser_BAO maxid = new AccountUser_BAO();
                //int max =  maxid.MaxUser();
                //max = max + 1;
                //string username = txtFirstName.Text.Trim();
                //username = username + max;
                //string password = txtFirstName.Text.Trim();
                //password = password + max;

                assignmentDetails_BE.FirstName = txtFirstName.Text.Trim();
                assignmentDetails_BE.LastName = txtLastName.Text.Trim();
                assignmentDetails_BE.EmailID = txtCandidateEmail.Text.Trim();
                assignmentDetails_BE.IsActive = 1;
                assignmentDetails_BE.LoginID = txtFirstName.Text.Trim();
                assignmentDetails_BE.Password = txtFirstName.Text.Trim();
                assignmentDetails_BE.Salutation = "Mr.";
                assignmentDetails_BE.GroupID = Convert.ToInt32(ConfigurationManager.AppSettings["ParticipantRoleID"]);
                assignmentDetails_BE.Notification = true;
                assignmentDetails_BE.ModifyDate = DateTime.Now;
                assignmentDetails_BE.ModifyBy = 1;
                assignmentDetails_BE.StatusID = 1;

                identity = this.Page.User.Identity as WADIdentity;

                if (identity.User.GroupID == 1)
                {
                    assignmentDetails_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
                }
                else
                {
                    assignmentDetails_BE.AccountID = identity.User.AccountID;
                }






                email += txtCandidateEmail.Text.Trim() + ";";



                finalemail = email.TrimEnd(';');


                assignmentDetails_BEList.Add(assignmentDetails_BE);
            }
        }

        return assignmentDetails_BEList;
    }

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

    private void BindCandidateList(int candidateCount)
    {
        try
        {
            DataTable dtCandidate = new DataTable();
            dtCandidate.Columns.Add("Relationship");
            dtCandidate.Columns.Add("Name");
            dtCandidate.Columns.Add("EmailID");

            for (int count = 0; count < candidateCount; count++)
                dtCandidate.Rows.Add("", "", "");

            rptrCandidateList.DataSource = dtCandidate;
            rptrCandidateList.DataBind();



        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Questionnaire_BAO.Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Questionnaire_BAO();

        ddlQuestionnaire.Items.Clear();
        DataTable dtQuestionnaire = new DataTable();
        dtQuestionnaire = questionnaire_BAO.GetProjectQuestionnaire(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtQuestionnaire.Rows.Count > 0)
        {
            ddlQuestionnaire.DataSource = dtQuestionnaire;
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataBind();
        }

        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlQuestionnaire.Items.Count > 1)
            ddlQuestionnaire.Items[1].Selected = true;


        Programme_BAO programme_BAO = new Programme_BAO();

        ddlProgramme.Items.Clear();
        DataTable dtProgramme = new DataTable();
        dtProgramme = programme_BAO.GetProjectProgramme(Convert.ToInt32(ddlProject.SelectedValue));

        if (dtProgramme.Rows.Count > 0)
        {
            ddlProgramme.DataSource = dtProgramme;
            ddlProgramme.DataTextField = "ProgrammeName";
            ddlProgramme.DataValueField = "ProgrammeID";
            ddlProgramme.DataBind();
        }

        ddlProgramme.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlProgramme.Items.Count > 1)
            ddlProgramme.Items[1].Selected = true;

    }

    protected void ImgUpload_click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string constr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection scon = new SqlConnection(constr);

            if (FileUpload1.HasFile)
            {
                if (this.IsFileValid(this.FileUpload1))
                {

                    string filename = "";
                    string file = "";

                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);

                    file = GetUniqueFilename(filename);

                    Session["FinalName"] = file;

                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename);



                    //filename = FileUpload1.FileName;



                    //filename = FileUpload1.PostedFile.FileName;



                    DataTable dtProspective = new DataTable();

                    dtProspective = ReturnExcelDataTableMot(filename);


                    if (dtProspective != null && dtProspective.Rows.Count > 0)
                    {


                        txtCandidateNo.Text = Convert.ToString(dtProspective.Rows.Count);

                        //foreach (RepeaterItem item in rptrCandidateList.Items)
                        //{
                        //    TextBox txtRelation = (TextBox)item.FindControl("ddlYearInsured");
                        //    TextBox txtCandidate = (TextBox)item.FindControl("ddlCompanyName");
                        //    TextBox txtCandEmail = (TextBox)item.FindControl("txtCompAddress");


                        //}

                        rptrCandidateList.DataSource = dtProspective;
                        rptrCandidateList.DataBind();

                        lblMessage.Text = "";

                        int candidateCount = Convert.ToInt32(txtCandidateNo.Text.Trim());

                        for (int i = 0; i < candidateCount; i++)
                        {
                            email += dtProspective.Rows[i]["EmailID"].ToString() + ";";

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


    public System.Data.DataTable ReturnExcelDataTableMot(string FullFileNamePath)
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

            DataTable dtExcel = new DataTable();

            dtExcel.Columns.Add("Relationship", typeof(string));
            dtExcel.Columns.Add("Name", typeof(string));
            dtExcel.Columns.Add("EmailID", typeof(string));



            DataRow row;

            try
            {
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    //rowIndex = 2 + index;
                    row = dtExcel.NewRow();



                    row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                    row[2] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);






                    index++;
                    rowIndex = 2 + index;
                    dtExcel.Rows.Add(row);
                }


            }
            catch
            {
                lblMessage.Text = "Please check your file data.";
                dtExcel = null;
            }
            app.Workbooks.Close();

            return dtExcel;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void errorMessage(string filename)
    {

        lblMessage.Text = "Upload Failed.Please fill the Correct Field Value";



    }

    protected void lnkError_Click(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Session["FinalName"].ToString() + ".txt");
        Response.WriteFile(Server.MapPath("~") + "//UploadDocs//" + Session["FinalName"].ToString() + ".txt");
        Response.End();

    }

    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniquefilename;
    }

    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {

            int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);

            Account_BAO account1_BAO = new Account_BAO();

            CompanyName = account1_BAO.GetdtAccountList(Convert.ToString(companycode));

            expression1 = "AccountID='" + companycode + "'";

            Finalexpression = expression1;

            DataRow[] resultsAccount = CompanyName.Select(Finalexpression);

            DataTable dtAccount = CompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
            {
                dtAccount.ImportRow(drAccount);
            }

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            Project_BAO project_BAO = new Project_BAO();

            ddlProject.Items.Clear();
            ddlProject.Items.Insert(0, new ListItem("Select", "0"));

            ddlProject.DataSource = project_BAO.GetAccountProject(companycode);
            ddlProject.DataTextField = "Title";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();

        }
        else
        {
            lblcompanyname.Text = "";
        }
    }
}
