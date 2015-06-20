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



public partial class Survey_Module_Questionnaire_ImportUser : CodeBehindBase
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
            string constr=System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection scon=new SqlConnection(constr);
            
            if (FileUpload1.HasFile)
            {
                if (this.IsFileValid(this.FileUpload1))
                {
                  
                    string filename="";
                    string file = "";
                    
                    //filename = FileUpload1.FileName;
                    filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                    file = GetUniqueFilename(filename); 
                   
                    Session["FinalName"] = file;

                    filename = Server.MapPath("~") + "\\UploadDocs\\" + file;
                    FileUpload1.SaveAs(filename);


                    DataTable dtProspective = new DataTable();
                   
                    dtProspective = ReturnExcelDataTableMot(filename);


                    if (dtProspective != null && dtProspective.Rows.Count > 0)
                    {

                        SqlBulkCopyOptions options = SqlBulkCopyOptions.KeepIdentity;
                        SqlBulkCopy bcp = new SqlBulkCopy(scon, options, null);

                        bcp.DestinationTableName = "[User]";

                        bcp.ColumnMappings.Add("LoginID", "LoginID");
                        bcp.ColumnMappings.Add("Password", "Password");
                        bcp.ColumnMappings.Add("GroupID", "GroupID");
                        bcp.ColumnMappings.Add("AccountID", "AccountID");
                        bcp.ColumnMappings.Add("StatusID", "StatusID");
                        bcp.ColumnMappings.Add("Salutation", "Salutation");
                        bcp.ColumnMappings.Add("FirstName", "FirstName");
                        bcp.ColumnMappings.Add("LastName", "LastName");
                        bcp.ColumnMappings.Add("EmailID", "EmailID");
                        bcp.ColumnMappings.Add("Notification", "Notification");
                        bcp.ColumnMappings.Add("ModifyBy", "ModifyBy");
                        bcp.ColumnMappings.Add("ModifyDate", "ModifyDate");
                        bcp.ColumnMappings.Add("IsActive", "IsActive");


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


    public System.Data.DataTable ReturnExcelDataTableMot(string FullFileNamePath)
    {
        //DataTable dtExcel;
        DateTime dt3 = new DateTime();
        DataTable dtExcel = new DataTable();
        string SheetName = string.Empty;
        try
        {

            Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(FullFileNamePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;

            int index = 0;
            object rowIndex = 2;

            
           
            dtExcel.Columns.Add("LoginID", typeof(string));
            dtExcel.Columns.Add("Password", typeof(string));
            dtExcel.Columns.Add("GroupID", typeof(Int32));
            dtExcel.Columns.Add("AccountID", typeof(Int32));
            dtExcel.Columns.Add("StatusID", typeof(Int32));
            dtExcel.Columns.Add("Salutation", typeof(string));
            dtExcel.Columns.Add("FirstName", typeof(string));
            dtExcel.Columns.Add("LastName", typeof(string));
            dtExcel.Columns.Add("EmailID", typeof(string));
            dtExcel.Columns.Add("Notification", typeof(bool));
            dtExcel.Columns.Add("ModifyBy", typeof(Int32));
            dtExcel.Columns.Add("ModifyDate", typeof(DateTime));
            dtExcel.Columns.Add("IsActive", typeof(Int32));


            DataRow row;

            try
            {
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    rowIndex = 2 + index;
                    row = dtExcel.NewRow();
                    //string temp3 = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 11]).Value2);
                    //if (temp3 != "")
                    //{ dt3 = DateTime.FromOADate(Convert.ToDouble(temp3)); }


                    row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);

                    DataTable dtAllGroup = new DataTable();
                    object[] param = new object[3] {null, null, 'A' };

                    DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

                    dtAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", param, null).Tables[0];

                    expression1 = "GroupName='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2) + "'";

                    Finalexpression = expression1;

                    DataRow[] results = dtAllGroup.Select(Finalexpression);

                    DataTable dtGroup = dtAllGroup.Clone();

                    foreach (DataRow dr in results)
                    {
                        dtGroup.ImportRow(dr);
                    }

                    int GroupId = Convert.ToInt32(dtGroup.Rows[0]["GroupID"]);

                    row[2] = GroupId;


                    DataTable dtAllAccount = new DataTable();
                    object[] paramAccount = new object[2] { null, 'A' };



                    dtAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", paramAccount, null).Tables[0];

                    expression6 = "Code='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2) + "'";

                    Finalexpression6 = expression6;

                    DataRow[] resultsAccount = dtAllAccount.Select(Finalexpression6);

                    DataTable dtAccount = dtAllAccount.Clone();

                    foreach (DataRow drAccount in resultsAccount)
                    {
                        dtAccount.ImportRow(drAccount);
                    }

                    int AccId = Convert.ToInt32(dtAccount.Rows[0]["AccountID"]);


                    row[3] = AccId;


                    DataTable dtAllStatus = new DataTable();
                    object[] param1 = new object[3] { null, null, 'A' };



                    dtAllStatus = cDataSrc.ExecuteDataSet("UspGetStatus", param1, null).Tables[0];

                    expression2 = "Name='" + Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2) + "'";

                    Finalexpression2 = expression2;

                    DataRow[] results1 = dtAllStatus.Select(Finalexpression2);

                    DataTable dtStatus = dtAllStatus.Clone();

                    foreach (DataRow dr1 in results1)
                    {
                        dtStatus.ImportRow(dr1);
                    }

                    int StatusId = Convert.ToInt32(dtStatus.Rows[0]["StatusID"]);

                    row[4] = StatusId;


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
