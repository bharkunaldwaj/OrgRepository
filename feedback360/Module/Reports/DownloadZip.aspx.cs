using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Zip;
using System.Net;
using System.IO;


public partial class Module_Reports_DownloadZip : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Label ll = (Label)this.Master.FindControl("Current_location");
        ll.Text = "<marquee> You are in <strong>Feedback 360</strong> </marquee>";

        string sPath = Server.MapPath("~") + "\\ReportGenerate\\" +Request.QueryString["dirName"].ToString() + "\\";
        //string zipName = "EN-20110419-180513.zip";
        string zipName = Request.QueryString["filename"].ToString();

        string openZip = sPath + zipName;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment; filename=Feedback.zip");
        Response.ContentType = "application/zip";
        //Response.WriteFile(openZip);        
        Response.TransmitFile(openZip);
        Response.Flush();
        Response.Close();

        //This Code Will Delete Zip
        File.Delete(openZip);

        DirectoryInfo drInfo = new DirectoryInfo(sPath);
        drInfo.Delete(true);
    }
}
