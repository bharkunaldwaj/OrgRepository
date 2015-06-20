using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Admin_BAO;
using System.Diagnostics;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using System.Data;
using System.IO;

public partial class Module_Questionnaire_Category : CodeBehindBase
{
    Survey_Category_BAO category_BAO = new Survey_Category_BAO();
    Survey_Category_BE category_BE = new Survey_Category_BE();
    List<Survey_Category_BE> category_BEList = new List<Survey_Category_BE>();

    DataTable dtCompanyName;
    DataTable dtAllAccount;
    string expression1;
    string Finalexpression;
    WADIdentity identity;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));
            if (!IsPostBack)
            {
                identity = this.Page.User.Identity as WADIdentity;
                int categoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                category_BEList = category_BAO.GetCategoryByID(Convert.ToInt32(identity.User.AccountID), categoryID);

                Account_BAO account_BAO = new Account_BAO();
                ddlAccountCode.DataSource = account_BAO.GetdtAccountList(Convert.ToString(identity.User.AccountID));
                ddlAccountCode.DataValueField = "AccountID";
                ddlAccountCode.DataTextField = "Code";
                ddlAccountCode.DataBind();

                Questionnaire_BAO.Survey_Questionnaire_BAO questionnnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
                ddlQuestionnaire.DataSource = questionnnaire_BAO.GetdtQuestionnaireList(identity.User.AccountID.ToString());
                ddlQuestionnaire.DataTextField = "QSTNName";
                ddlQuestionnaire.DataValueField = "QuestionnaireID";
                ddlQuestionnaire.DataBind();

                if (category_BEList.Count > 0)
                {
                    SetCategoryValue(category_BEList);

                    ddlAccountCode.SelectedValue = ddlAccountCode.SelectedValue;
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


                if (Request.QueryString["Mode"] == "E")
                {
                    ibtnSave.Visible = true;
                    ibtnCancel.Visible = true;
                    imbBack.Visible = false;
                    lblheader.Text = "Edit Category";
                }
                else if (Request.QueryString["Mode"] == "R")
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

    private void SetCategoryValue(List<Survey_Category_BE> category_BEList)
    {
        try
        {
            //HandleWriteLog("Start", new StackTrace(true));

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {
                ddlAccountCode.SelectedValue = category_BEList[0].AccountID.ToString();
                string abc = category_BEList[0].AccountID.ToString();
                ddlAccountCode.SelectedValue = abc;

                if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
                {
                    int companycode = Convert.ToInt32(ddlAccountCode.SelectedValue);
                    Account_BAO account1_BAO = new Account_BAO();
                    dtCompanyName = account1_BAO.GetdtAccountList(Convert.ToString(companycode));
                    //expression1 = "AccountID='" + companycode + "'";
                    //Finalexpression = expression1;

                    DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + companycode + "'");
                    DataTable dtAccount = dtCompanyName.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                    {
                        dtAccount.ImportRow(drAccount);
                    }

                    lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();
                }
                else
                {
                    lblcompanyname.Text = "";
                }


            }
            txtCategoryName.Text = category_BEList[0].Name;
            txtCategoryTitle.Text = category_BEList[0].Title;
            txtDescription.Text = category_BEList[0].Description;
            ddlQuestionnaire.SelectedValue = category_BEList[0].Questionnaire.ToString();
            txtSequence.Text = category_BEList[0].Sequence.ToString();
            chkExcludeFromAnalysis.Checked = Convert.ToBoolean(category_BEList[0].ExcludeFromAnalysis);

            if (!string.IsNullOrEmpty(category_BEList[0].IntroPdfFileName))
            {
                Session["CategoryPdf"] = category_BEList[0].IntroPdfFileName;
            }

            if (!string.IsNullOrEmpty(category_BEList[0].IntroImgFileName))
            {
                hdnQuestImage.Value = category_BEList[0].IntroImgFileName;
                Session["CategoryImage"] = category_BEList[0].IntroImgFileName;
                imgQuestlogo.Src = "../../UploadDocs/" + category_BEList[0].IntroImgFileName;
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

    protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            HandleWriteLog("Start", new StackTrace(true));
            Survey_Category_BE category_BE = new Survey_Category_BE();
            Survey_Category_BAO category_BAO = new Survey_Category_BAO();

            identity = this.Page.User.Identity as WADIdentity;

            if (identity.User.GroupID == 1)
            {

                category_BE.AccountID = Convert.ToInt32(ddlAccountCode.SelectedValue);

            }
            else
            {
                category_BE.AccountID = identity.User.AccountID;
            }



            category_BE.Name = GetString(txtCategoryName.Text);
            category_BE.Title = GetString(txtCategoryTitle.Text);
            category_BE.Description = GetString(txtDescription.Text);
            category_BE.Sequence = Convert.ToInt32(GetString(txtSequence.Text));
            category_BE.ExcludeFromAnalysis = chkExcludeFromAnalysis.Checked;
            category_BE.Questionnaire = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
            category_BE.ModifiedBy = 1;
            category_BE.ModifiedDate = DateTime.Now;
            category_BE.IsActive = 1;


            if (introImageFileUpload.HasFile)
            {
                string filename = System.IO.Path.GetFileName(introImageFileUpload.PostedFile.FileName);
                //filename = FileUpload.FileName;
                string file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                introImageFileUpload.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                category_BE.IntroImgFileName = file;
            }
            else
            {
                //if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryImage"])))
                //    category_BE.IntroImgFileName = Convert.ToString(Session["CategoryImage"]);


                if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryImage"])) && hdnQuestImage.Value != "")
                    category_BE.IntroImgFileName = hdnQuestImage.Value;
                else if (Request.QueryString["Mode"] == "E" && pdfFileUpload.FileName == "" && hdnQuestImage.Value != "")
                    category_BE.IntroImgFileName = Convert.ToString(Session["CategoryImage"]);
                else
                    category_BE.IntroImgFileName = "";


            }

            if (pdfFileUpload.HasFile)
            {
                string filename = System.IO.Path.GetFileName(pdfFileUpload.PostedFile.FileName);
                //filename = FileUpload.FileName;
                string file = GetUniqueFilename(filename);

                string path = MapPath("~\\UploadDocs\\") + file;
                pdfFileUpload.SaveAs(path);
                string name = file;
                FileStream fs1 = new FileStream(Server.MapPath("~\\UploadDocs\\") + file, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);
                Byte[] docbytes = br1.ReadBytes((Int32)fs1.Length);
                br1.Close();
                fs1.Close();
                category_BE.IntroPdfFileName = file;
            }
            else
            {
                //if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryPdf"])))
                //    category_BE.IntroPdfFileName = Convert.ToString(Session["CategoryPdf"]);

                if (!string.IsNullOrEmpty(Convert.ToString(Session["CategoryPdf"])) && hdnRemoveIntroPdf.Value != "")
                    category_BE.IntroPdfFileName = hdnRemoveIntroPdf.Value;
                else if (Request.QueryString["Mode"] == "E" && pdfFileUpload.FileName == "" && hdnRemoveIntroPdf.Value != "")
                    category_BE.IntroPdfFileName = Convert.ToString(Session["CategoryPdf"]);
                else
                    category_BE.IntroPdfFileName = "";

            }


            if (Request.QueryString["Mode"] == "E")
            {
                category_BE.CategoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                category_BAO.UpdateCategory(category_BE);
                lblMessage.Text = "Category updated successfully";
                Response.Redirect("CategoryList.aspx", false);
            }
            else
            {
                category_BAO.AddCategory(category_BE);
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


    protected void ddlAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        Questionnaire_BAO.Survey_Questionnaire_BAO questionnaire_BAO = new Questionnaire_BAO.Survey_Questionnaire_BAO();
        ddlQuestionnaire.Items.Clear();
        ddlQuestionnaire.Items.Insert(0, new ListItem("Select", "0"));
        identity = this.Page.User.Identity as WADIdentity;

        if (Convert.ToInt32(ddlAccountCode.SelectedValue) > 0)
        {
            Account_BAO account_BAO = new Account_BAO();

            dtCompanyName = account_BAO.GetdtAccountList(ddlAccountCode.SelectedValue);
            DataRow[] resultsAccount = dtCompanyName.Select("AccountID='" + ddlAccountCode.SelectedValue + "'");
            DataTable dtAccount = dtCompanyName.Clone();
            foreach (DataRow drAccount in resultsAccount)
                dtAccount.ImportRow(drAccount);

            lblcompanyname.Text = dtAccount.Rows[0]["OrganisationName"].ToString();

            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(ddlAccountCode.SelectedValue));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
        else
        {
            lblcompanyname.Text = "";

            ddlQuestionnaire.DataSource = questionnaire_BAO.GetdtQuestionnaireList(Convert.ToString(identity.User.AccountID));
            ddlQuestionnaire.DataValueField = "QuestionnaireID";
            ddlQuestionnaire.DataTextField = "QSTNName";
            ddlQuestionnaire.DataBind();
        }
    }

    public string GetUniqueFilename(string filename)
    {
        string basename = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        string uniquefilename = string.Format("{0}{1}{2}", basename, DateTime.Now.Ticks, Path.GetExtension(filename));
        // Thread.Sleep(1); // To really prevent collisions, but usually not needed 
        return uniquefilename;
    }
    protected void lnkPdf_Click(object sender, EventArgs e)
    {
        string path = MapPath("~\\UploadDocs\\");
        string filename = Convert.ToString(Session["CategoryPdf"]);
        string FilePath = path + filename;
        if (File.Exists(FilePath) && Session["CategoryPdf"] != null && Session["CategoryPdf"] != "" && hdnRemoveIntroPdf.Value!="")
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
