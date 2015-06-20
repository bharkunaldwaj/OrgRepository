using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Questionnaire_BAO
{
    public class Common_BAO : Base_BAO
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        public DataTable GetDataTable(string procedureName,CNameValueList objCnameList)
        {
            DataTable dt = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Common_DAO objCommon_DAO = new Common_DAO();
                dt = objCommon_DAO.GetDataTable(procedureName,objCnameList);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dt;
        }

        public int InsertAndUpdate(string procedureName, CNameValueList objCnameList)
        {
            int dt = 0;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Common_DAO objCommon_DAO = new Common_DAO();
                dt = objCommon_DAO.InsertAndUpdate(procedureName, objCnameList);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dt;
        }

      
    }
}
