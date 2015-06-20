/*
* PURPOSE: Base class for all DAO
* AUTHOR: 
* Date Of Creation: <dd/mm/yyyy>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.IO;

using Miscellaneous;
using System.Diagnostics;
using DatabaseAccessUtilities;

namespace feedbackFramework_DAO
{
    public class DAO_Base
    {
        private FileStream FS;
        private StreamWriter SW;
        private string fpath;

        public void HandleWriteLog(string Log, StackTrace stackTrace)
        {
            //TraceLogger.WriteLog(System.Threading.Thread.CurrentPrincipal.Identity.Name.ToString(), Enumerators.LogLevel.DAO, stackTrace, Log);
        }

        public void HandleWriteLogDAU(string p_SP, CNameValueList p_cNameValueList, StackTrace stackTrace)
        {
            for (int parameterCounter = 0; parameterCounter < p_cNameValueList.Count; parameterCounter++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(p_cNameValueList[parameterCounter].Value.ToString()))
                        p_SP +=  p_cNameValueList[parameterCounter].Value.ToString() + ",";
                    else if (string.Empty == p_cNameValueList[parameterCounter].Value.ToString())
                        p_SP += " " + ",";
                    else
                        p_SP += "null" + ",";
                }
                catch (NullReferenceException ex)
                {
                    p_SP += "null" + ",";
                }
            }

            char[] trimCharacter = { ',' };
            p_SP = p_SP.TrimEnd(trimCharacter);
            TraceLogger.WriteLog(System.Threading.Thread.CurrentPrincipal.Identity.Name.ToString(), Enumerators.LogLevel.DAU, stackTrace, p_SP);
        }

        public void HandleWriteLogDAU(string p_SP, Object[] p_paramters, StackTrace stackTrace) {
            for (int parameterCounter = 0; parameterCounter < p_paramters.Length; parameterCounter++) {
                try {
                    if (!string.IsNullOrEmpty(p_paramters[parameterCounter].ToString()))
                        p_SP += p_paramters[parameterCounter].ToString() + ",";
                    else if (string.Empty == p_paramters[parameterCounter].ToString())
                        p_SP += " " + ",";
                    else
                        p_SP += "null" + ",";
                }
                catch (NullReferenceException ex) {
                    p_SP += "null" + ",";
                }
            }

            char[] trimCharacter = { ',' };
            p_SP = p_SP.TrimEnd(trimCharacter);
            TraceLogger.WriteLog(System.Threading.Thread.CurrentPrincipal.Identity.Name.ToString(), Enumerators.LogLevel.DAU, stackTrace, p_SP);
        }






        public void Survey_HandleWriteLogDAU(string p_SP, Object[] p_paramters, StackTrace stackTrace)
        {
            for (int parameterCounter = 0; parameterCounter < p_paramters.Length; parameterCounter++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(p_paramters[parameterCounter].ToString()))
                        p_SP += p_paramters[parameterCounter].ToString() + ",";
                    else if (string.Empty == p_paramters[parameterCounter].ToString())
                        p_SP += " " + ",";
                    else
                        p_SP += "null" + ",";
                }
                catch (NullReferenceException ex)
                {
                    p_SP += "null" + ",";
                }
            }

            char[] trimCharacter = { ',' };
            p_SP = p_SP.TrimEnd(trimCharacter);
            TraceLogger.WriteLog(System.Threading.Thread.CurrentPrincipal.Identity.Name.ToString(), Enumerators.LogLevel.DAU, stackTrace, p_SP);
        }









        public void HandleException(Exception ex)
        {
            //ExceptionLogger.Write(ex.ToString());

            string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                    "Error Application: Feedback 360 - DAO" + Environment.NewLine + "," +
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

        public string GetString(string p_value) {
            string returnValue;

            try {               
                    p_value = p_value.ToString().Replace("'", "''").Trim();
                    returnValue = p_value;                
            }
            catch (NullReferenceException ex) {
                returnValue = null;
            }
            return returnValue;
        }

        public int? GetInt(object p_objValue)
        {
            int? returnValue;
            
            if (p_objValue.Equals(System.DBNull.Value))
                returnValue = null;
            else
                returnValue = (int?)p_objValue;

            return returnValue;
        }


        public bool? GetBool(object objValue)
        {
            bool? returnValue;

            if (objValue.Equals(System.DBNull.Value))
                returnValue = null;
            else
                returnValue = (bool?)objValue;

            return returnValue;
        }

        public DateTime? GetDateTime(object objValue)
        {
            DateTime? returnValue;

            if (objValue.Equals(System.DBNull.Value))
                returnValue = null;
            else
                returnValue = (DateTime?)objValue;

            return returnValue;
        }
    }
}


