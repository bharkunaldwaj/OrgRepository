using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

public partial class Download : System.Web.UI.Page
{
    private string path;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Display uploaded document 
            if (!string.IsNullOrEmpty(Request.QueryString["filename"]))
            {
                FileInfo myFile = null;
                var strReportName = Convert.ToString(Request.QueryString["filename"]);
                string root = Server.MapPath("~") + "\\ReportGenerate\\";
                string openpdf = root + strReportName + ".pdf";
                myFile = new FileInfo(openpdf);

                if (myFile != null)
                {
                    if (myFile.Exists)
                    {
                        switch (myFile.Extension)
                        {
                            case ".jpg":
                                Response.AppendHeader("Content-Type", "image/jpeg");
                                break;
                            case ".doc":
                                Response.AppendHeader("Content-Type", "application/vnd.ms-word");
                                break;
                            case ".docx":
                                Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                                break;
                            case ".xls":
                                Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                                break;
                            case ".pdf":
                                Response.AppendHeader("Content-Type", "application/pdf");
                                break;
                            default:
                                //This octet-stream identifies all the uploaded file extension excepts .Jpg
                                Response.AppendHeader("Content-Type", "application/octet-stream");
                                break;
                        }

                        Response.AppendHeader("Content-disposition", "attachment; filename=" + myFile.Name);
                        Response.Clear();
                        Response.WriteFile(myFile.FullName);
                        Response.End();
                    }
                    else
                        Response.Write("File not Found.");
                }
                else
                    Response.Write("File not Found.");
            }
            else
            {
                Response.Write("File Not Found.");
                return;
            }
        }
    }
}
