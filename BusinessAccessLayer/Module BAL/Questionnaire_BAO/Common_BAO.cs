using System;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_DAO;

using System.Data;
using System.Configuration;
namespace Questionnaire_BAO
{
    public class Common_BAO : Base_BAO
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        /// <summary>
        /// Use to get DataTable by passing procedure name
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="objCnameList"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string procedureName,CNameValueList objCnameList)
        {
            DataTable dataTableResult = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Common_DAO CommonDataAccessObject = new Common_DAO();
                dataTableResult = CommonDataAccessObject.GetDataTable(procedureName,objCnameList);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Use to insert record by passing procedure name and list of details
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="objCnameList"></param>
        /// <returns></returns>
        public int InsertAndUpdate(string procedureName, CNameValueList objCnameList)
        {
            int dataTableResult = 0;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Common_DAO CommonDataAccessObject = new Common_DAO();
                dataTableResult = CommonDataAccessObject.InsertAndUpdate(procedureName, objCnameList);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }
    }
}
