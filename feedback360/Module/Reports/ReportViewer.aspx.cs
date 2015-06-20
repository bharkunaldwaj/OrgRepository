using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using System.IO;
//using Root.Reports;
using System.Drawing.Text;
using Microsoft.Reporting.WebForms;

public partial class Module_Reports_ReportViewer : System.Web.UI.Page
{
    string LogFilePath = string.Empty;
    string mimeType;
    string encoding;
    string fileNameExtension;
    string extension, deviceInfo, outputFileName = "";
    string[] streams;
    string defaultFileName = string.Empty;
    Warning[] warnings;

    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";
        if (!IsPostBack)
        {
            btnExport();
        }
    }

    protected void btnExport()
    {
        Microsoft.Reporting.WebForms.ReportViewer rview = new Microsoft.Reporting.WebForms.ReportViewer();
        rview.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
        //string mimeType, encoding, extension, deviceInfo, outputFileName = "";
        string[] streamids;
        Microsoft.Reporting.WebForms.Warning[] warnings;
        string format = "PDF";


        string root = string.Empty;
        root = Server.MapPath("~") + "\\UploadDocs\\";
        rview.ServerReport.ReportPath = "/DamcoTest/Report1";
        string Report = "FeedBackReports";

        rview.Visible = false;
        byte[] bytes = rview.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        string PDF_path = root + Report + ".pdf";
        FileStream objFs = new FileStream(PDF_path, System.IO.FileMode.Create);
        objFs.Write(bytes, 0, bytes.Length);
        objFs.Close();
        objFs.Dispose();


        //System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> paramList = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
        ////paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("LanguageID", Session["LanguageId"].ToString()));
        ////paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("Report_Comments", ""));
        ////paramList.Add(new Microsoft.Reporting.WebForms.ReportParameter("ReportId", "3"));
        //rview.ServerReport.ReportPath = "/DamcoTest/Report1";

        //rview.ServerReport.SetParameters(paramList);
        //outputFileName = "FeedBackReports.pdf";

        //deviceInfo = "<DeviceInfo>" + "<SimplePageHeaders>True</SimplePageHeaders>" + "</DeviceInfo>";
        //byte[] bytes = rview.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
        //Response.Clear();
        //if (format == "PDF")
        //{
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=" + outputFileName);
        //}

        //Response.OutputStream.Write(bytes, 0, bytes.Length);
        //Response.OutputStream.Flush();
        //Response.OutputStream.Close();
        //Response.Flush();
        //Response.Close();


        
    }

}
