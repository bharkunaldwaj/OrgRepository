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
    //Global variable
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
        Label labelCurrentLocation = (Label)this.Master.FindControl("Current_location");
        labelCurrentLocation.Text = "<marquee> You are in <strong>Survey</strong> </marquee>";

        if (!IsPostBack)
        {
            lblMessage.Text = "";
        }
    }

    /// <summary>
    /// Import question details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgUpload_click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string constr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection scon = new SqlConnection(constr);
            //if control has file
            if (FileUpload1.HasFile)
            {
                //check if uploaded file is valid or not
                if (this.IsFileValid(this.FileUpload1))
                {
                    string filename = "";
                    string file = "";

                    //filename = FileUpload1.FileName;
                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);//Get the file name.
                    file = GetUniqueFilename(filename);//Get file unique name.
                    Session["FinalName"] = file;//save file name in session.
                    //set file path
                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename);//save file to location.

                    //filename = FileUpload1.PostedFile.FileName;  
                    DataTable dataTableProspective = new DataTable();
                    dataTableProspective = ReturnExcelDataTableMot(filename);//Read excel and convert it to datatable

                    if (dataTableProspective != null && dataTableProspective.Rows.Count > 0)
                    {
                        //Bulk insert value to database.
                        SqlBulkCopyOptions options = SqlBulkCopyOptions.KeepIdentity;
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(scon, options, null);

                        bulkCopy.DestinationTableName = "[Survey_Question]";

                        bulkCopy.ColumnMappings.Add("AccountID", "AccountID");
                        bulkCopy.ColumnMappings.Add("CompanyID", "CompanyID");
                        bulkCopy.ColumnMappings.Add("QuestionnaireName", "QuestionnaireID");
                        bulkCopy.ColumnMappings.Add("QuestionType", "QuestionTypeID");
                        bulkCopy.ColumnMappings.Add("CategoryName", "CateogryID");
                        bulkCopy.ColumnMappings.Add("Sequence", "Sequence");
                        bulkCopy.ColumnMappings.Add("ValidationText", "ValidationText");
                        bulkCopy.ColumnMappings.Add("Validation", "Validation");
                        bulkCopy.ColumnMappings.Add("Title", "Title");
                        bulkCopy.ColumnMappings.Add("QuestionText", "Description");
                        // bcp.ColumnMappings.Add("QuestionTextSelf", "DescriptionSelf");
                        bulkCopy.ColumnMappings.Add("QuestionHint", "Hint");
                        bulkCopy.ColumnMappings.Add("TokenText", "TokenText");
                        bulkCopy.ColumnMappings.Add("Token", "Token");
                        bulkCopy.ColumnMappings.Add("LengthMIN", "LengthMIN");
                        bulkCopy.ColumnMappings.Add("LengthMAX", "LengthMAX");
                        bulkCopy.ColumnMappings.Add("Multiline", "Multiline");
                        // bcp.ColumnMappings.Add("LowerLabel", "LowerLabel");
                        //bcp.ColumnMappings.Add("UpperLabel", "UpperLabel");
                        // bcp.ColumnMappings.Add("LowerBound", "LowerBound");
                        //  bcp.ColumnMappings.Add("UpperBound", "UpperBound");
                        //   bcp.ColumnMappings.Add("Increment", "Increment");
                        //  bcp.ColumnMappings.Add("Reverse", "Reverse");
                        bulkCopy.ColumnMappings.Add("ModifyBy", "ModifyBy");
                        bulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate");
                        bulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                        bulkCopy.ColumnMappings.Add("Range_Name", "Range_Name");
                        scon.Open();

                        // write the data in the "dataTable"
                        bulkCopy.WriteToServer(dataTableProspective);
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

    /// <summary>
    /// Check if uploaeded excel is valied in extension and size
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
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return isFileOk;
    }

    /// <summary>
    /// Read excel and Convert excel to datatable
    /// </summary>
    /// <param name="FullFileNamePath"></param>
    /// <returns></returns>
    public DataTable ReturnExcelDataTableMot(string FullFileNamePath)
    {
        DataTable dataTableExcel = new DataTable();
        string SheetName = string.Empty;

        try
        {
            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
            //define columns and its type
            dataTableExcel.Columns.Add("AccountID", typeof(Int32));
            dataTableExcel.Columns.Add("CompanyID", typeof(Int32));
            dataTableExcel.Columns.Add("QuestionnaireName", typeof(Int32));
            dataTableExcel.Columns.Add("QuestionType", typeof(Int32));
            dataTableExcel.Columns.Add("CategoryName", typeof(Int32));
            dataTableExcel.Columns.Add("Sequence", typeof(Int32));
            dataTableExcel.Columns.Add("Validation", typeof(Int32));
            dataTableExcel.Columns.Add("ValidationText", typeof(string));
            dataTableExcel.Columns.Add("Title", typeof(string));
            dataTableExcel.Columns.Add("QuestionText", typeof(string));
            // dtExcel.Columns.Add("QuestionTextSelf", typeof(string));
            dataTableExcel.Columns.Add("QuestionHint", typeof(string));
            dataTableExcel.Columns.Add("Token", typeof(Int32));
            dataTableExcel.Columns.Add("TokenText", typeof(string));
            dataTableExcel.Columns.Add("LengthMIN", typeof(Int32));
            dataTableExcel.Columns.Add("LengthMAX", typeof(Int32));
            dataTableExcel.Columns.Add("Multiline", typeof(bool));
            // dtExcel.Columns.Add("LowerLabel", typeof(string));
            // dtExcel.Columns.Add("UpperLabel", typeof(string));
            // dtExcel.Columns.Add("LowerBound", typeof(Int32));
            // dtExcel.Columns.Add("UpperBound", typeof(Int32));
            //  dtExcel.Columns.Add("Increment", typeof(Int32));
            //dtExcel.Columns.Add("Reverse", typeof(bool));
            dataTableExcel.Columns.Add("ModifyBy", typeof(Int32));
            dataTableExcel.Columns.Add("ModifyDate", typeof(DateTime));
            dataTableExcel.Columns.Add("IsActive", typeof(Int32));
            dataTableExcel.Columns.Add("Range_Name", typeof(string));
            DataRow row;

            try
            {
                int accountId = 0;
                int questionnaireId = 0;
                int QuestionTypeId = 0;
                int CategoryId = 0;
                string validationType = "";
                string qstToken = "";

                DataTable dataTableAllAccount = new DataTable();
                DataTable dataTableAllQuestionnaire = new DataTable();
                DataTable dataTableAllQuestionType = new DataTable();
                DataTable dataTableAllCategory = new DataTable();

                //int index = 0;
                int rowIndex = 2;
                int flag;
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    flag = 0;
                    row = dataTableExcel.NewRow();

                    DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

                    //SETTING ACCOUNT 

                    object[] param = new object[2] { null, 'A' };
                    dataTableAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

                    DataRow[] resultsAccount = dataTableAllAccount.Select("Code='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2).Trim() + "'");
                    DataTable dataTableAccount = dataTableAllAccount.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                        dataTableAccount.ImportRow(dataRowAccount);

                    if (dataTableAccount.Rows[0]["AccountID"] != null)
                    {
                        accountId = Convert.ToInt32(dataTableAccount.Rows[0]["AccountID"]);
                        row["CompanyID"] = accountId;
                        row["AccountID"] = accountId;
                    }
                    else
                        flag = 1;
                    //SETTING QUESTIONNAIRE                 

                    object[] paramQuestionnaire = new object[3] { null, accountId, "A" };
                    dataTableAllQuestionnaire = cDataSrc.ExecuteDataSet("Survey_UspQuestionnaireSelect", paramQuestionnaire, null).Tables[0];

                    DataRow[] resultsQuestionnaire = dataTableAllQuestionnaire.Select("QSTNName='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2).Trim() + "'");
                    DataTable dataTableQuestionnaire = dataTableAllQuestionnaire.Clone();

                    foreach (DataRow dataRowQuestionnaire in resultsQuestionnaire)
                        dataTableQuestionnaire.ImportRow(dataRowQuestionnaire);

                    if (dataTableQuestionnaire.Rows[0]["QuestionnaireID"] != null)
                    {
                        questionnaireId = Convert.ToInt32(dataTableQuestionnaire.Rows[0]["QuestionnaireID"]);
                        row["QuestionnaireName"] = questionnaireId;
                    }
                    else
                        flag = 1;

                    //SETTING QUESTIONTYPE 

                    object[] paramQuestionType = new object[2] { null, 'A' };
                    dataTableAllQuestionType = cDataSrc.ExecuteDataSet("Survey_UspQuestionTypeSelect", paramQuestionType, null).Tables[0];

                    DataRow[] resultsQuestionType = dataTableAllQuestionType.Select("Name='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2).Trim() + "'");
                    DataTable dataTableQuestionType = dataTableAllQuestionType.Clone();

                    foreach (DataRow drQuestionType in resultsQuestionType)
                        dataTableQuestionType.ImportRow(drQuestionType);

                    if (dataTableQuestionType.Rows[0]["QuestionTypeID"] != null)
                    {
                        QuestionTypeId = Convert.ToInt32(dataTableQuestionType.Rows[0]["QuestionTypeID"]);
                        row["QuestionType"] = QuestionTypeId;
                    }
                    else
                        flag = 1;


                    //SETTING QUESTION CATEGORY 

                    object[] paramCategory = new object[2] { questionnaireId, 'S' };
                    dataTableAllCategory = cDataSrc.ExecuteDataSet("Survey_UspFeedbackQuestionSelect", paramCategory, null).Tables[0];

                    DataRow[] resultsCategory = dataTableAllCategory.Select("CategoryName='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2).Trim() + "'");
                    DataTable dataTableCategory = dataTableAllCategory.Clone();

                    foreach (DataRow dataRowCategory in resultsCategory)
                        dataTableCategory.ImportRow(dataRowCategory);

                    if (dataTableCategory.Rows[0]["CategoryID"] != null)
                    {
                        CategoryId = Convert.ToInt32(dataTableCategory.Rows[0]["CategoryID"]);
                        row["CategoryName"] = CategoryId;
                    }
                    else
                        flag = 1;

                    //SETTING QUESTION SEQUENCE 
                    if (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2 != null && ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2 != "")
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
                    if (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 7]).Value2 != null && ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 7]).Value2 != "")
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
                    else if (qstToken == "" || qstToken == null)
                        row["Token"] = 4;

                    //SETTING QUESTION TOKEN TEXT 
                    row["TokenText"] = qstToken;

                    //SETTING LENGTH MINIMUM 
                    row["LengthMIN"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 10]).Value2);

                    //SETTING LENGTH MAXIMUM 
                    row["LengthMAX"] = Convert.ToInt32(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 11]).Value2);

                    //SETTING MULTILINE 
                    row["Multiline"] = Convert.ToBoolean(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 12]).Value2);
                    if (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 13]).Value2 != null && ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 13]).Value2 != "")
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

                    dataTableExcel.Rows.Add(row);
                }
            }
            catch
            {
                lblMessage.Text = "Please check your file data.";
                dataTableExcel = null;
            }

            app.Workbooks.Close();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return dataTableExcel;
    }

    /// <summary>
    /// Validate user if incorrect file format.
    /// </summary>
    /// <param name="filename"></param>
    private void errorMessage(string filename)
    {
        lblMessage.Text = "File cannot be uploaded. Please fill the Correct Field Value";
    }

    /// <summary>
    /// no use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkError_Click(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Session["FinalName"].ToString() + ".txt");
        Response.WriteFile(Server.MapPath("~") + "//UploadDocs//" + Session["FinalName"].ToString() + ".txt");
        Response.End();
    }

    /// <summary>
    /// Give unique name to image or file
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
