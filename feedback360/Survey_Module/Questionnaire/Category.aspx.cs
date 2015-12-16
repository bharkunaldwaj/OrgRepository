using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin_BAO;
using System.Diagnostics;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.Data;
using System.IO;

public partial class Module_Questionnaire_Category : CodeBehindBase
{
    //Global variables.
    Survey_Category_BAO categoryBusinessAccessObject = new Survey_Category_BAO();
    //Survey_Category_BE category_BE = new Survey_Category_BE();
    List<Survey_Category_BE> categoryBusinesEntityList = new List<Survey_Category_BE>();

    DataTable dataTableCompanyName;
    //DataTable dtAllAccount;
    //string expression1;
    //string Finalexpression;
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;
                int categoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                //Get all category List by user account id and category id.
                categoryBusinesEntityList = categoryBusinessAccessObject.GetCategoryByID(Convert.ToInt32(identity.User.AccountID), categoryID);

                Account_BAO accountBusinessAccessObject = new Account_BAO();
                //Get Account list by user account id  and bind account dropdown.
                ddlAccountCode.DataSource = accountBusinessAccessObject.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                Survey_Questionnaire_BAO questionnnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
                //Get Questionnaire list and bind Questionnaire dropdown by user account Id.
                ddlQuestionnaire.DataSource = questionnnaireBusinessAccessObject.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                if (categoryBusinesEntityList.Count > 0)
                {
                    //Bind controls by value.
                    SetCategoryValue(categoryBusinesEntityList);

                    ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
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

                //If query string contains Mode="E" then Edit mode else view mode and hide show controls accordingly.
                if (Request.QueryString["Mode"] == "E")//Edit Mode.
                {
                    ibtnSave.Visible = true;
                    ibtnCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Category";
                }
                else if (Request.QueryString["Mode"] == "R")//View Mode.
                {
                    ibtnSave.Visible = false;
                    ibtnCancel.Visible = false;
                    imbBack.Visible = true;
                    lblheader.Text = "View Category";
                }
            }
            HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Set value to controls
    /// </summary>
    /// <param name="categoryBusinesEntityList"></param>
    private void SetCategoryValue(List<Survey_Category_BE> categoryBusinesEntityList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            identity = this.Page.User.Identity as WADIdentity;
            //If user is a Super Admin then use account dropdown value else user account to get records.
            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = categoryBusinesEntityList[0].AccountID.ToString();
                string accountID = categoryBusinesEntityList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = accountID;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    Account_BAO account1BusinessAccessObject = new Account_BAO();
                    dataTableCompanyName = account1BusinessAccessObject.GetdtAccountList(Convert.ToString(companycode));
                    //expression1 = "AccountID='" + companycode + "'";
                    //Finalexpression = expression1;

                    DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + companycode + "'");
                    DataTable dataTableAccount = dataTableCompanyName.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                    {
                        dataTableAccount.ImportRow(dataRowAccount);
                    }
                    //bind comapny name.
                    lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }
            }
            //Set values to control.
            txtCategoryName.Text = categoryBusinesEntityList[0].Name;
            txtCategoryTitle.Text = categoryBusinesEntityList[0].Title;
            txtDescription.Text = categoryBusinesEntityList[0].Description;
            ddlQuestionnaire.SelectedValue = categoryBusinesEntityList[0].Questionnaire.ToString();
            txtSequence.Text = categoryBusinesEntityList[0].Sequence.ToString();
            chkExcludeFromAnalysis.Checked = Convert.ToBoolean(categoryBusinesEntityList[0].ExcludeFromAnalysis);

            if (!string.IsNullOrEmpty(categoryBusinesEntityList[0].IntroPdfFileName))
            {
                Session["CategoryPdf"] = categoryBusinesEntityList[0].IntroPdfFileName;
            }

            if (!string.IsNullOrEmpty(categoryBusinesEntityList[0].IntroImgFileName))
            {
                hdnQuestImage.Value = categoryBusinesEntityList[0].IntroImgFileName;
                Session["CategoryImage"] = categoryBusinesEntityList[0].IntroImgFileName;
                imgQuestlogo.Src = "../../UploadDocs/" + categoryBusinesEntityList[0].IntroImgFileName;
            }

            else
                imgQuestlogo.Src = "../../UploadDocs/noImage.jpg";


            HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Initilize properties and save to the data base. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HandleWriteLog("Start", new StackTrace(true));
            Survey_Category_BE categoryBusinesEntity = new Survey_Category_BE();
            Survey_Category_BAO categoryBusinessAccessObject = new Survey_Category_BAO();

            identity = this.Page.User.Identity as WADIdentity;
            //If user is a Super Admin then use account drop down value else user account .
            if (identity.User.GroupID == 1)
            {
                categoryBusinesEntity.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);
            }
            else
            {
                categoryBusinesEntity.AccountID = identity.User.AccountID;
            }

            //Initilize properties.
            categoryBusinesEntity.Name = GetString(txtCategoryName.Text);
            categoryBusinesEntity.Title = GetString(txtCategoryTitle.Text);
            categoryBusinesEntity.Description = GetString(txtDescription.Text);
            categoryBusinesEntity.Sequence = Convert.ToInt32(GetString(txtSequence.Text));
            categoryBusinesEntity.ExcludeFromAnalysis = chkExcludeFromAnalysis.Checked;
            categoryBusinesEntity.Questionnaire = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            categoryBusinesEntity.ModifiedBy = 1;
            categoryBusinesEntity.ModifiedDate = DateTime.Now;
            categoryBusinesEntity.IsActive = 1;

            //Upload category Image.
            if (introImageFileUpload.HasFile)
            {
                string filename = System.IO.Path.GetFileName(introImageFileUpload.PostedFile.FileName);
                //filename = FileUpload.FileName;
                //Get File unique name.
                string file = GetUniqueFilename(filename);
                //Set file path.
                string path = MapPath("~\\UploadDocs\\") + file;
                introImageFileUpload.SaveAs(path);
                string name = file;
                //Read file from stream.
                FileStream categoryFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader categoryBinaryReader = new BinaryReader(categoryFileStream);
                Byte[] docbytes = categoryBinaryReader.ReadBytes((Int32)categoryFileStream.Length);
                categoryBinaryReader.Close();
                categoryFileStream.Close();
                categoryBusinesEntity.IntroImgFileName = file;
            }
            else
            {
                //if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryImage"])))
                //    category_BE.IntroImgFileName = Convert.ToString(Session["CategoryImage"]);


                if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryImage"])) && hdnQuestImage.Value != "")
                    categoryBusinesEntity.IntroImgFileName = hdnQuestImage.Value;
                else if (Request.QueryString["Mode"] == "E" && pdfFileUpload.FileName == "" && hdnQuestImage.Value != "")
                    categoryBusinesEntity.IntroImgFileName = Convert.ToString(Session["CategoryImage"]);
                else
                    categoryBusinesEntity.IntroImgFileName = "";
            }
            //Upload category Pdf file.
            if (pdfFileUpload.HasFile)
            {
                string filename = System.IO.Path.GetFileName(pdfFileUpload.PostedFile.FileName);
                //filename = FileUpload.FileName;
                //Get File unique name.
                string file = GetUniqueFilename(filename);
                //Set file path.
                string path = MapPath("~\\UploadDocs\\") + file;
                pdfFileUpload.SaveAs(path);
                string name = file;
                //Read file from stream.
                FileStream categoryFileStream = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader categoryBinaryReader = new BinaryReader(categoryFileStream);
                Byte[] docbytes = categoryBinaryReader.ReadBytes((Int32)categoryFileStream.Length);
                categoryBinaryReader.Close();
                categoryFileStream.Close();
                categoryBusinesEntity.IntroPdfFileName = file;
            }
            else
            {
                //if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryPdf"])))
                //    category_BE.IntroPdfFileName = Convert.ToString(Session["CategoryPdf"]);
                if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryPdf"])) && hdnRemoveIntroPdf.Value != "")
                    categoryBusinesEntity.IntroPdfFileName = hdnRemoveIntroPdf.Value;
                else if (Request.QueryString["Mode"] == "E" && pdfFileUpload.FileName == "" && hdnRemoveIntroPdf.Value != "")
                    categoryBusinesEntity.IntroPdfFileName = Convert.ToString(Session["CategoryPdf"]);
                else
                    categoryBusinesEntity.IntroPdfFileName = "";
            }

            //If querey string contains mode="E" then update else Insert.
            if (Request.QueryString["Mode"] == "E")
            {
                categoryBusinesEntity.CategoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                categoryBusinessAccessObject.UpdateCategory(categoryBusinesEntity);
                lblMessage.Text = "Category updated successfully";
                Response.Redirect("CategoryList.aspx", false);
            }
            else
            {
                categoryBusinessAccessObject.AddCategory(categoryBusinesEntity);
                lblMessage.Text = "Category saved successfully";
            }

            Session["CategoryPdf"] = null;
            Session["CategoryImage"] = null;
            txtCategoryName.Text = "";
            txtDescription.Text = "";
            txtSequence.Text = "";
            chkExcludeFromAnalysis.Checked = false;

            //  Response.Redirect("CategoryList.aspx", false);
            HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Redirect to category list page when click on calcel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HandleWriteLog("Start", new StackTrace(true));

            Response.Redirect("CategoryList.aspx", false);

            //HandleWriteLog("Start", new StackTrace(true));
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    /// <summary>
    /// Bind questionnaire dropdown by account.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Survey_Questionnaire_BAO questionnaireBusinessAccessObject = new Survey_Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        identity = this.Page.User.Identity as WADIdentity;
        //If account dropdown is selected value >0
        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO accountBusinessAccessObject = new Account_BAO();
            //Get account details
            dataTableCompanyName = accountBusinessAccessObject.GetdtAccountList(ddlAccountCode.SelectedValue);

            DataRow[] resultsAccount = dataTableCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");

            DataTable dataTableAccount = dataTableCompanyName.Clone();

            foreach (DataRow drAccount in resultsAccount)
                dataTableAccount.ImportRow(drAccount);

            lblcompanyname.Text = dataTableAccount.Rows[0]["OrganisationName"].ToString();
            //Get QuestionnaireList by account Id and bind Questionnaire dropdown.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";
            //Get QuestionnaireList by user account Id and bind Questionnaire dropdown.
            ddlQuestionnaire.DataSource = questionnaireBusinessAccessObject.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
    }

    /// <summary>
    /// Generate unique name for uploaded file.
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
    /// Download category PDf.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPdf_Click(object sender, EventArgs e)
    {
        //Set Folder path.
        string path = MapPath("~\\UploadDocs\\");
        //Set file name.
        string filename = Convert.ToString(Session["CategoryPdf"]);
        //Create full path.
        string FilePath = path + filename;
        //If file exist then download.
        if (File.Exists(FilePath) && Session["CategoryPdf"] != null && Session["CategoryPdf"] != "" && hdnRemoveIntroPdf.Value != "")
        {
            Response.ClearContent();
            Response.ClearHeaders();

            Response.AddHeader("Content-Disposition", "attachment; filename=" + path + filename);
            Response.ContentType = "application/pdf";
            Response.TransmitFile(path + filename);

            Response.Flush();
            Response.Clear();
            Response.Close();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "noPreview", "alert('No pdf file to preview.');", true);
        }
    }
}
