using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
using feedbackFramework_DAO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Common_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        public static int ExecutScalarNew(string storedProcName, CNameValueList cname)
        {
            string connString = Convert.ToString(ConfigurationSettings.AppSettings["ConnectionString"]);
            //Int32 newProdID = 0;

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
                {
                    //objConnection.cnn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cname.Any())
                    {
                        foreach (var obj in cname)
                            cmd.Parameters.AddWithValue(obj.Name, obj.Value);
                    }
                    return Convert.ToInt32(Convert.ToDecimal(cmd.ExecuteScalar()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                conn.Close();
            }


        }

        public static DataSet ExecuteFill(string storedProcName, CNameValueList cname)
        {
            string connString = Convert.ToString(ConfigurationSettings.AppSettings["ConnectionString"]);
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
                {
                    //objConnection.cnn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cname.Any())
                    {
                        foreach (var obj in cname)
                            cmd.Parameters.AddWithValue(obj.Name, obj.Value);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();
            return ds;

        }



        public DataTable GetDataTable(string procedureName, CNameValueList objCnameList)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = ExecuteFill(procedureName, objCnameList).Tables[0];

                HandleWriteLogDAU(procedureName, objCnameList, new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dt;
        }


        /// <summary>
        /// Add Company
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int InsertAndUpdate(string procedureName, CNameValueList objCnameList)
        {
            int returnValue = 0;
            //try
            //{
            returnValue = ExecutScalarNew(procedureName, objCnameList);

            cDataSrc = null;
            //}
            //catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
    }
}
