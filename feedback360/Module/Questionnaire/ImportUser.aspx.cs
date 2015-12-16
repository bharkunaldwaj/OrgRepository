using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using DatabaseAccessUtilities;

public partial class Module_Questionnaire_ImportUser : CodeBehindBase
{
    //Global variables
    //int i;
    //string SqlType = string.Empty;
    //string filePath = string.Empty;
    //string strName = string.Empty;
    //bool flag = true;
    //int j;
    //string file1;
    //string filename1;
    string expression1;
    string expression2;
    string Finalexpression;
    string Finalexpression2;
    string expression6;
    string Finalexpression6;
    WADIdentity identity;
    DataTable CompanyName;
    //DataTable dtAllAccount;
    //string expression11;
    //string Finalexpression11;

    StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lableCurrentLocation = (Label)this.Master.FindControl("Current_location");
        lableCurrentLocation.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        if (!IsPostBack)
        {
            lblMessage.Text = "";
        }
    }

    /// <summary>
    /// Import user details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgUpload_click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection connection = new SqlConnection(ConnectionString);
            //if control has file
            if (FileUpload1.HasFile)
            {
                //check if uploaded file is valid or not
                if (this.IsFileValid(this.FileUpload1))
                {
                    string filename = "";
                    string file = "";

                    //filename = FileUpload1.FileName;
                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);//Get File name.
                    file = GetUniqueFilename(filename);
                    //save file name in session.
                    Session["FinalName"] = file;
                    //set file path.
                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename);//save file to location.


                    DataTable dataTableProspective = new DataTable();
                    //Read excel and convert it to datatable
                    dataTableProspective = ReturnExcelDataTable(filename);

                    if (dataTableProspective != null && dataTableProspective.Rows.Count > 0)
                    {
                        //Bulk insert value to database.
                        SqlBulkCopyOptions options = SqlBulkCopyOptions.KeepIdentity;
                        SqlBulkCopy bulkcopy = new SqlBulkCopy(connection, options, null);

                        bulkcopy.DestinationTableName = "[User]";

                        bulkcopy.ColumnMappings.Add("LoginID", "LoginID");
                        bulkcopy.ColumnMappings.Add("Password", "Password");
                        bulkcopy.ColumnMappings.Add("GroupID", "GroupID");
                        bulkcopy.ColumnMappings.Add("AccountID", "AccountID");
                        bulkcopy.ColumnMappings.Add("StatusID", "StatusID");
                        bulkcopy.ColumnMappings.Add("Salutation", "Salutation");
                        bulkcopy.ColumnMappings.Add("FirstName", "FirstName");
                        bulkcopy.ColumnMappings.Add("LastName", "LastName");
                        bulkcopy.ColumnMappings.Add("EmailID", "EmailID");
                        bulkcopy.ColumnMappings.Add("Notification", "Notification");
                        bulkcopy.ColumnMappings.Add("ModifyBy", "ModifyBy");
                        bulkcopy.ColumnMappings.Add("ModifyDate", "ModifyDate");
                        bulkcopy.ColumnMappings.Add("IsActive", "IsActive");

                        connection.Open();

                        // write the data in the "dataTable"
                        bulkcopy.WriteToServer(dataTableProspective);
                        connection.Close();
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
                    //Page.RegisterStartupScript("FileTyp", "<script language='JavaScript'>alert('Invalid file type');</script>");
                    lblMessage.Text = "Invalid file type";
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
    public DataTable ReturnExcelDataTable(string FullFileNamePath)
    {
        //DataTable dtExcel;
        //DateTime dt3 = new DateTime();
        DataTable dataTableExcel = new DataTable();
        string SheetName = string.Empty;
        try
        {
            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;

            int index = 0;
            object rowIndex = 2;

            //define columns and its type
            dataTableExcel.Columns.Add("LoginID", typeof(string));
            dataTableExcel.Columns.Add("Password", typeof(string));
            dataTableExcel.Columns.Add("GroupID", typeof(Int32));
            dataTableExcel.Columns.Add("AccountID", typeof(Int32));
            dataTableExcel.Columns.Add("StatusID", typeof(Int32));
            dataTableExcel.Columns.Add("Salutation", typeof(string));
            dataTableExcel.Columns.Add("FirstName", typeof(string));
            dataTableExcel.Columns.Add("LastName", typeof(string));
            dataTableExcel.Columns.Add("EmailID", typeof(string));
            dataTableExcel.Columns.Add("Notification", typeof(bool));
            dataTableExcel.Columns.Add("ModifyBy", typeof(Int32));
            dataTableExcel.Columns.Add("ModifyDate", typeof(DateTime));
            dataTableExcel.Columns.Add("IsActive", typeof(Int32));

            DataRow row;

            try
            {
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    rowIndex = 2 + index;
                    row = dataTableExcel.NewRow();
                    //string temp3 = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 11]).Value2);
                    //if (temp3 != "")
                    //{ dt3 = DateTime.FromOADate(Convert.ToDouble(temp3)); }

                    row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);

                    DataTable dataTableAllGroup = new DataTable();
                    object[] param = new object[3] { null, null, 'A' };

                    DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

                    dataTableAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", param, null).Tables[0];

                    expression1 = "GroupName='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2) + "'";

                    Finalexpression = expression1;

                    DataRow[] results = dataTableAllGroup.Select(Finalexpression);

                    DataTable dataTableGroup = dataTableAllGroup.Clone();

                    foreach (DataRow dr in results)
                    {
                        dataTableGroup.ImportRow(dr);
                    }

                    int GroupId = Convert.ToInt32(dataTableGroup.Rows[0]["GroupID"]);

                    row[2] = GroupId;

                    DataTable dataTableAllAccount = new DataTable();
                    object[] paramAccount = new object[2] { null, 'A' };

                    dataTableAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", paramAccount, null).Tables[0];

                    expression6 = "Code='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2) + "'";

                    Finalexpression6 = expression6;

                    DataRow[] resultsAccount = dataTableAllAccount.Select(Finalexpression6);

                    DataTable dtAccount = dataTableAllAccount.Clone();

                    foreach (DataRow dataRowAccount in resultsAccount)
                    {
                        dtAccount.ImportRow(dataRowAccount);
                    }

                    int accountId = Convert.ToInt32(dtAccount.Rows[0]["AccountID"]);

                    row[3] = accountId;

                    DataTable dataTableAllStatus = new DataTable();
                    object[] param1 = new object[3] { null, null, 'A' };

                    dataTableAllStatus = cDataSrc.ExecuteDataSet("UspGetStatus", param1, null).Tables[0];

                    expression2 = "Name='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2) + "'";

                    Finalexpression2 = expression2;

                    DataRow[] results1 = dataTableAllStatus.Select(Finalexpression2);

                    DataTable dtStatus = dataTableAllStatus.Clone();
                    //Create rows
                    foreach (DataRow dataRow in results1)
                    {
                        dtStatus.ImportRow(dataRow);
                    }

                    int StatusId = Convert.ToInt32(dtStatus.Rows[0]["StatusID"]);

                    row[4] = StatusId;

                    //set value to table rows
                    row[5] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 6]).Value2);
                    row[6] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 7]).Value2);
                    row[7] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 8]).Value2);
                    row[8] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 9]).Value2);
                    row[9] = Convert.ToBoolean(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 10]).Value2);
                    row[10] = 1;
                    row[11] = DateTime.Now;
                    row[12] = 1;

                    index++;
                    rowIndex = 2 + index;
                    dataTableExcel.Rows.Add(row);//add row to datatable.
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
    /// No use
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
    /// Give unique id to uploaded image or file
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
