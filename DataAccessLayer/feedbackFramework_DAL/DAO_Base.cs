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
using System.IO;

using Miscellaneous;
using System.Diagnostics;
using DatabaseAccessUtilities;

namespace feedbackFramework_DAO
{
    public class DAO_Base
    {
        /// <summary>
        /// Global variables
        /// </summary>
        private FileStream fileStream;
        private StreamWriter streamWriter;
        private string filePath;

        public void HandleWriteLog(string Log, StackTrace stackTrace)
        {
            //TraceLogger.WriteLog(System.Threading.Thread.CurrentPrincipal.Identity.Name.ToString(), Enumerators.LogLevel.DAO, stackTrace, Log);
        }

        /// <summary>
        /// Error log for dataAccess layer
        /// </summary>
        /// <param name="p_SP"></param>
        /// <param name="p_paramters"></param>
        /// <param name="stackTrace"></param>
        public void HandleWriteLogDAU(string p_SP, CNameValueList p_cNameValueList, StackTrace stackTrace)
        {
            for (int parameterCounter = 0; parameterCounter < p_cNameValueList.Count; parameterCounter++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(p_cNameValueList[parameterCounter].Value.ToString()))
                        p_SP += p_cNameValueList[parameterCounter].Value.ToString() + ",";
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

        /// <summary>
        /// Error log for dataAccess layer
        /// </summary>
        /// <param name="p_SP"></param>
        /// <param name="p_paramters"></param>
        /// <param name="stackTrace"></param>
        public void HandleWriteLogDAU(string p_SP, Object[] p_paramters, StackTrace stackTrace)
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

        /// <summary>
        /// Error log for dataAccess layer
        /// </summary>
        /// <param name="p_SP"></param>
        /// <param name="p_paramters"></param>
        /// <param name="stackTrace"></param>
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

            filePath = HttpContext.Current.Server.MapPath("~") + "//Error.log";

            if (File.Exists(filePath))
            { fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write); }
            else
            { fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write); }

            string msg = str.Replace(",", "");

            streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(msg);

            streamWriter.Close();
            fileStream.Close();

            string errorPage = "";
            errorPage = HttpContext.Current.Request.ApplicationPath + "\\Error.aspx";
            HttpContext.Current.Response.Redirect(errorPage);
        }

        /// <summary>
        /// Remove quotes from string
        /// </summary>
        /// <param name="p_value"></param>
        /// <returns></returns>
        public string GetString(string p_value)
        {
            string returnValue;

            try
            {
                p_value = p_value.ToString().Replace("'", "''").Trim();
                returnValue = p_value;
            }
            catch (NullReferenceException ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        /// <summary>
        /// convert object to int
        /// </summary>
        /// <param name="p_objValue"></param>
        /// <returns></returns>
        public int? GetInt(object p_objValue)
        {
            int? returnValue;

            if (p_objValue.Equals(System.DBNull.Value))
                returnValue = null;
            else
                returnValue = (int?)p_objValue;

            return returnValue;
        }

        /// <summary>
        /// Convert object ot Boolian
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public bool? GetBool(object objValue)
        {
            bool? returnValue;

            if (objValue.Equals(System.DBNull.Value))
                returnValue = null;
            else
                returnValue = (bool?)objValue;

            return returnValue;
        }

        /// <summary>
        /// convert object to time
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
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


