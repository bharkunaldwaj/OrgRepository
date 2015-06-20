using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using DatabaseAccessUtilities;
using System.Diagnostics;
using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_BAO;
using Admin_BAO;

public partial class Survey_Module_Questionnaire_ImportQuestions : CodeBehindBase
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
    string Finalexpression1;
    string Finalexpression2;
    string expression3;
    string expression4;
    string Finalexpression3;
    WADIdentity identity;
    DataTable CompanyName;
    DataTable dtAllAccount;
    string expression11;
    string Finalexpression11;
    StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label llx = (Label)this.Master.FindControl("Current_location");
        llx.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";
        if (!IsPostBack)
        {

            lblMessage.Text = "";
           }
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

                    //filename = FileUpload1.FileName;
                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                    file = GetUniqueFilename(filename);
                    Session["FinalName"] = file;

                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename);

                    //filename = FileUpload1.PostedFile.FileName;  
                    
                    DataTable dtProspective = new DataTable();
                    dtProspective = ReturnExcelDataTableMot(filename);

                    if (dtProspective != null && dtProspective.Rows.Count > 0)
                    {

                        SqlBulkCopyOptions options = SqlBulkCopyOptions.KeepIdentity;
                        SqlBulkCopy bcp = new SqlBulkCopy(scon, options, null);

                        bcp.DestinationTableName = "[Survey_Question]";
                        
                        bcp.ColumnMappings.Add("AccountID", "AccountID");
                        bcp.ColumnMappings.Add("CompanyID", "CompanyID");
                        bcp.ColumnMappings.Add("QuestionnaireName", "QuestionnaireID");
                        bcp.ColumnMappings.Add("QuestionType", "QuestionTypeID");
                        bcp.ColumnMappings.Add("CategoryName", "CateogryID");
                        bcp.ColumnMappings.Add("Sequence", "Sequence");
                        bcp.ColumnMappings.Add("ValidationText", "ValidationText");
                        bcp.ColumnMappings.Add("Validation", "Validation");
                        bcp.ColumnMappings.Add("Title", "Title");
                        bcp.ColumnMappings.Add("QuestionText", "Description");
                       // bcp.ColumnMappings.Add("QuestionTextSelf", "DescriptionSelf");
                        bcp.ColumnMappings.Add("QuestionHint", "Hint");
                        bcp.ColumnMappings.Add("TokenText", "TokenText");
                        bcp.ColumnMappings.Add("Token", "Token");
                        bcp.ColumnMappings.Add("LengthMIN", "LengthMIN");
                        bcp.ColumnMappings.Add("LengthMAX", "LengthMAX");
                        bcp.ColumnMappings.Add("Multiline", "Multiline");
                       // bcp.ColumnMappings.Add("LowerLabel", "LowerLabel");
                        //bcp.ColumnMappings.Add("UpperLabel", "UpperLabel");
                       // bcp.ColumnMappings.Add("LowerBound", "LowerBound");
                      //  bcp.ColumnMappings.Add("UpperBound", "UpperBound");
                     //   bcp.ColumnMappings.Add("Increment", "Increment");
                      //  bcp.ColumnMappings.Add("Reverse", "Reverse");
                        bcp.ColumnMappings.Add("ModifyBy", "ModifyBy");
                        bcp.ColumnMappings.Add("ModifyDate", "ModifyDate");
                        bcp.ColumnMappings.Add("IsActive", "IsActive");
                       bcp.ColumnMappings.Add("Range_Name", "Range_Name");    
                        scon.Open();

                        // write the data in the "dataTable"
                        bcp.WriteToServer(dtProspective);
                        scon.Close();
                        File.Delete(filename);
                        lblMessage.Text = "File Uploaded Successfully";
                       
                    }
                    else
                    {
                        errorMessage(filename);
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid file type";
                   // Page.RegisterStartupScript("FileTyp", "<script language='JavaScript'>alert('Invalid file type');</script>");
                }
            }
            else
            {
                lblMessage.Text = "Please upload a file";
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
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
            
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

       return isFileOk;
    }

    public DataTable ReturnExcelDataTableMot(string FullFileNamePath)
    {
        DataTable dtExcel = new DataTable();
        string SheetName = string.Empty;
     
        try
        {
            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;

            dtExcel.Columns.Add("AccountID", typeof(Int32));
            dtExcel.Columns.Add("CompanyID", typeof(Int32));
            dtExcel.Columns.Add("QuestionnaireName", typeof(Int32));
            dtExcel.Columns.Add("QuestionType", typeof(Int32));
            dtExcel.Columns.Add("CategoryName", typeof(Int32));
            dtExcel.Columns.Add("Sequence", typeof(Int32));
            dtExcel.Columns.Add("Validation", typeof(Int32));
            dtExcel.Columns.Add("ValidationText", typeof(string));
            dtExcel.Columns.Add("Title", typeof(string));
            dtExcel.Columns.Add("QuestionText", typeof(string));
           // dtExcel.Columns.Add("QuestionTextSelf", typeof(string));
            dtExcel.Columns.Add("QuestionHint", typeof(string));
            dtExcel.Columns.Add("Token", typeof(Int32));
            dtExcel.Columns.Add("TokenText", typeof(string));
            dtExcel.Columns.Add("LengthMIN", typeof(Int32));
            dtExcel.Columns.Add("LengthMAX", typeof(Int32));
            dtExcel.Columns.Add("Multiline", typeof(bool));
           // dtExcel.Columns.Add("LowerLabel", typeof(string));
           // dtExcel.Columns.Add("UpperLabel", typeof(string));
           // dtExcel.Columns.Add("LowerBound", typeof(Int32));
           // dtExcel.Columns.Add("UpperBound", typeof(Int32));
          //  dtExcel.Columns.Add("Increment", typeof(Int32));
            //dtExcel.Columns.Add("Reverse", typeof(bool));
            dtExcel.Columns.Add("ModifyBy", typeof(Int32));
            dtExcel.Columns.Add("ModifyDate", typeof(DateTime));
            dtExcel.Columns.Add("IsActive", typeof(Int32));
            dtExcel.Columns.Add("Range_Name", typeof(string)); 
            DataRow row;

            try
            {
                int accountId = 0;
                int questionnaireId = 0;
                int QuestionTypeId = 0;
                int CategoryId = 0;
                string validationType="";
                string qstToken = "";

                DataTable dtAllAccount = new DataTable();
                DataTable dtAllQuestionnaire = new DataTable();
                DataTable dtAllQuestionType = new DataTable();
                DataTable dtAllCategory = new DataTable();

                //int index = 0;
                int rowIndex = 2;
                int flag;
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    flag = 0;
                    row = dtExcel.NewRow();
                    
                    DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

                    //SETTING ACCOUNT 

                    object[] param = new object[2] { null, 'A' };
                    dtAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

                    DataRow[] resultsAccount = dtAllAccount.Select("Code='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2).Trim() + "'");
                    DataTable dtAccount = dtAllAccount.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                        dtAccount.ImportRow(drAccount);
                    if (dtAccount.Rows[0]["AccountID"] != null)
                    {
                        accountId = Convert.ToInt32(dtAccount.Rows[0]["AccountID"]);
                        row["CompanyID"] = accountId;
                        row["AccountID"] = accountId;
                    }
                    else
                        flag = 1;
                    //SETTING QUESTIONNAIRE                 
                    
                    object[] paramQuestionnaire = new object[3] { null, accountId, "A" };
                    dtAllQuestionnaire = cDataSrc.ExecuteDataSet("Survey_UspQuestionnaireSelect", paramQuestionnaire, null).Tables[0];

                    DataRow[] resultsQuestionnaire = dtAllQuestionnaire.Select("QSTNName='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2).Trim() + "'");
                    DataTable dtQuestionnaire = dtAllQuestionnaire.Clone();

                    foreach (DataRow drQuestionnaire in resultsQuestionnaire)
                        dtQuestionnaire.ImportRow(drQuestionnaire);

                    if (dtQuestionnaire.Rows[0]["QuestionnaireID"] != null)
                    {
                        questionnaireId = Convert.ToInt32(dtQuestionnaire.Rows[0]["QuestionnaireID"]);
                        row["QuestionnaireName"] = questionnaireId;
                    }
                    else
                        flag = 1;

                    //SETTING QUESTIONTYPE 

                    object[] paramQuestionType = new object[2] { null, 'A' };
                    dtAllQuestionType = cDataSrc.ExecuteDataSet("Survey_UspQuestionTypeSelect", paramQuestionType, null).Tables[0];

                    DataRow[] resultsQuestionType = dtAllQuestionType.Select("Name='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2).Trim() + "'");
                    DataTable dtQuestionType = dtAllQuestionType.Clone();

                    foreach (DataRow drQuestionType in resultsQuestionType)
                        dtQuestionType.ImportRow(drQuestionType);

                    if(dtQuestionType.Rows[0]["QuestionTypeID"]!=null)
                    {
                    QuestionTypeId = Convert.ToInt32(dtQuestionType.Rows[0]["QuestionTypeID"]);
                    row["QuestionType"] = QuestionTypeId;
                    }
                    else
                        flag = 1;


                    //SETTING QUESTION CATEGORY 
                    
                    object[] paramCategory = new object[2] { questionnaireId, 'S' };
                    dtAllCategory = cDataSrc.ExecuteDataSet("Survey_UspFeedbackQuestionSelect", paramCategory, null).Tables[0];

                    DataRow[] resultsCategory = dtAllCategory.Select("CategoryName='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2).Trim() + "'");
                    DataTable dtCategory = dtAllCategory.Clone();

                    foreach (DataRow drCategory in resultsCategory)
                        dtCategory.ImportRow(drCategory);
                    if (dtCategory.Rows[0]["CategoryID"] != null)
                    {
                        CategoryId = Convert.ToInt32(dtCategory.Rows[0]["CategoryID"]);
                        row["CategoryName"] = CategoryId;
                    }
                    else
                        flag = 1;

                    //SETTING QUESTION SEQUENCE 
                    if(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2 !=null && ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2!="")
                    row["Sequence"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2);


                    //SETTING QUESTION VALIDATION TYPE 
                    if (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 6]).Value2 != null && ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 6]).Value2 != "")
                    {
                        validationType = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 6]).Value2).Trim();

                        if (validationType == "None")
                            row["Validation"] = 1;
                        else if (validationType == "Light")
                            row["Validation"] = 2;
                        else if (validationType == "Strong")
                            row["Validation"] = 3;
                    }
                    else
                        flag = 1;
                    
                    //SETTING QUESTION VALIDATION TEXT 
                    row["ValidationText"] = validationType;

                    //SETTING QUESTION TITLE
                    row["Title"] = "";

                    //SETTING QUESTION TEXT 
                    if (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 7]).Value2 != null && ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 7]).Value2!="")
                    row["QuestionText"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 7]).Value2);
                    else
                        flag = 1;


                    //SETTING QUESTION TEXT SELF 
            //        row["QuestionTextSelf"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 8]).Value2);


                    //SETTING QUESTION TEXT HINT 
                    row["QuestionHint"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 8]).Value2);

                    //SETTING QUESTION TOKEN 
                    qstToken = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 9]).Value2).Trim();

                    if (qstToken == "First Name")
                        row["Token"] = 1;
                    else if (qstToken == "Last Name")
                        row["Token"] = 2;
                    else if (qstToken == "First Name & Last Name")
                        row["Token"] = 3;
                    else if(qstToken == "" || qstToken == null)
                        row["Token"] = 4;

                    //SETTING QUESTION TOKEN TEXT 
                    row["TokenText"] = qstToken;

                    //SETTING LENGTH MINIMUM 
                    row["LengthMIN"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 10]).Value2);

                    //SETTING LENGTH MAXIMUM 
                    row["LengthMAX"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 11]).Value2);

                    //SETTING MULTILINE 
                    row["Multiline"] = Convert.ToBoolean(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 12]).Value2);
                    if (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 13]).Value2 != null && ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 13]).Value2!="")
                    row["Range_Name"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 13]).Value2);
                    else
                        flag = 1;
                    //SETTING LOWER LABEL 
      //              row["LowerLabel"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 14]).Value2);

                    //SETTING UPPER LABEL 
      //              row["UpperLabel"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 15]).Value2);

                    //SETTING LOWER BOUND 
     //               row["LowerBound"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 16]).Value2);

                    //SETTING UPPER BOUND 
      //              row["UpperBound"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 17]).Value2);

                    //SETTING INCREMENT 
      //              row["Increment"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 18]).Value2);

      //              row["Reverse"] = false;
                    row["ModifyBy"] = 1;
                    row["ModifyDate"] = DateTime.Now;
                    row["IsActive"] = 1;


                    if (flag == 1)
                    {
                        errorMessage(FullFileNamePath);
                        break;
                    }
                    //index++;
                    //rowIndex = 2 + index;
                    rowIndex++;

                    dtExcel.Rows.Add(row);
                }
            }
            catch
            {
                lblMessage.Text = "Please check your file data.";
                dtExcel = null;
            }
            app.Workbooks.Close();
           
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        return dtExcel;
    }

    private void errorMessage(string filename)
    {

        lblMessage.Text = "File cannot be uploaded. Please fill the Correct Field Value";



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




}
