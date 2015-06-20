/*
* PURPOSE: Exception Logger class to log exception at different layers
* AUTHOR:  Manish Mathur
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnterpriseLibaray = Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System.Configuration;

namespace Miscellaneous
{
    /// <summary>
    /// Enumerator for Exception logger
    /// </summary>
    public enum CategoryType
    {
        Presentation,
        ExcelImport,
        DataAccess
    }

    public static class ExceptionLogger
    {
        #region public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string p_message)
        {
            Write(p_message, CategoryType.Presentation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void Write(string p_message, CategoryType p_categoryType)
        {
            Write(p_message, p_categoryType, System.Diagnostics.TraceEventType.Information, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void Write(Exception ex)
        {
            Write(ex, CategoryType.Presentation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="type"></param>
        public static void Write(Exception ex, CategoryType p_categoryType)
        {
            Write(ex.ToString(), p_categoryType, System.Diagnostics.TraceEventType.Error, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="severity"></param>
        /// <param name="dictionary"></param>
        public static void Write(string p_message, CategoryType p_categoryType, System.Diagnostics.TraceEventType p_severity, IDictionary<string, object> p_dictionary)
        {

            string category = Enum.GetName(typeof(CategoryType), p_categoryType);

            EnterpriseLibaray.LogEntry entry = new EnterpriseLibaray.LogEntry();
            entry.Categories.Add(category);
            entry.Message = p_message;
            entry.Severity = p_severity;
            entry.Priority = 1;
            entry.ExtendedProperties = p_dictionary;
            EnterpriseLibaray.Logger.Write(entry);

            //SendEmail.Send("Error", p_message, ConfigurationSettings.AppSettings["errorMailTo"].ToString(), null, null, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="dictionary"></param>
        public static void WriteLogForSendEmailError(Exception ex, IDictionary<string, object> p_dictionary)
        {
            string category = Enum.GetName(typeof(CategoryType), 0);

            EnterpriseLibaray.LogEntry entry = new EnterpriseLibaray.LogEntry();
            entry.Categories.Add(category);
            entry.Message = ex.Message;
            entry.Severity = System.Diagnostics.TraceEventType.Error;
            entry.Priority = 1;
            entry.ExtendedProperties = p_dictionary;
            EnterpriseLibaray.Logger.Write(entry);
        }
        #endregion

    }
}
