/*
* PURPOSE: TraceLogger to log at which layer exception occurs
* AUTHOR:  
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace Miscellaneous
{
    public static class TraceLogger {
        public static FileStream traceLogFilePointer;
        public static TextWriter textWriter;

        #region Constructor
        static TraceLogger() {
            return;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Writes Log
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="p_loggedInformationLevel"></param>
        /// <param name="stkInfo"></param>
        /// <param name="Log"></param>
        public static void WriteLog(string p_userName,Enumerators.LogLevel p_loggedInformationLevel, StackTrace p_stkInfo,string p_log) {            
            if (Convert.ToInt16(p_loggedInformationLevel) > Convert.ToInt16(ConfigurationSettings.AppSettings["traceLogLevel"])) {
                return;
            }

            string writeString = "- Method Name : " + p_stkInfo.GetFrame(0).GetMethod().Name + " - File Name : " + p_stkInfo.GetFrame(0).GetFileName().ToString() + " - Layer : " + p_loggedInformationLevel + " - Extra Log Description: " + p_log;

            string fileName =  AppDomain.CurrentDomain.BaseDirectory;
            

            fileName = fileName + @"TraceLog\\TraceLog_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".log";
            if (!File.Exists(fileName)) {
                traceLogFilePointer = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                //string introductoryStatement = "User ID : " + UserName + " - Start Logging: " + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                textWriter = new StreamWriter(traceLogFilePointer);
                //textWriter.WriteLine(introductoryStatement);  
            }
            else {
                traceLogFilePointer = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                textWriter = new StreamWriter(traceLogFilePointer);
            }            
            
            writeString = "User ID : " + p_userName + " - Log Date : " + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + " " + writeString;
            textWriter.WriteLine(writeString);                        
            textWriter.Close();
            traceLogFilePointer.Close();
            traceLogFilePointer = null;
            textWriter = null;
            return;
        }
        #endregion
    }
}
