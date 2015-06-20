/*  
* PURPOSE: This is the Base Class for all Business Access Objects classes
* AUTHOR: Manish Mathur
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*                    <Key2><Reason 2 >
*/

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Miscellaneous;

namespace DAF_BAO {
    
    public class Base_BAO {

        private FileStream FS;
        private StreamWriter SW;
        private string fpath;

        

        public void HandleWriteLog(string Log, StackTrace stackTrace)
        {            
            //TraceLogger.WriteLog( System.Threading.Thread.CurrentPrincipal.Identity.Name.ToString(),Enumerators.LogLevel.BAO, stackTrace, Log);            
        }
        public void HandleException(Exception ex)
        {


            
            //ExceptionLogger.Write(ex.ToString());
            string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                    "Error Application: Feedback 360 - BAO" + Environment.NewLine + "," +
                    "Error Function: " + ex.TargetSite + Environment.NewLine + "," +
                    "Error Line: " + ex.StackTrace + Environment.NewLine + "," +
                    "Error Source: " + ex.Source + Environment.NewLine + "," +
                    "Error Message: " + ex.Message + Environment.NewLine;

            fpath = HttpContext.Current.Server.MapPath("~") + "//Error.log";

            if (File.Exists(fpath))
            { FS = new FileStream(fpath, FileMode.Append, FileAccess.Write); }
            else
            { FS = new FileStream(fpath, FileMode.Create, FileAccess.Write); }

            string msg = str.Replace(",", "");

            SW = new StreamWriter(FS);
            SW.WriteLine(msg);

            SW.Close();
            FS.Close();

            string errorPage = "";
            errorPage = HttpContext.Current.Request.ApplicationPath + "\\Error.aspx";
            HttpContext.Current.Response.Redirect(errorPage);
        }
    }
}
