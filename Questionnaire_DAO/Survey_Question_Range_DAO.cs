using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Web;
using feedbackFramework_BE;
using feedbackFramework_DAO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
  public class Survey_Question_Range_DAO : DAO_Base
    {



       DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        public Survey_Question_Range_DAO() 
        {
            
        }




        public int chk_rng_Availability(string str)
        {
            
            string connectionInfo = ConfigurationSettings.AppSettings["ConnectionInfo"];
            SqlConnection connection = new SqlConnection(connectionInfo);
            connection.Open();

            SqlCommand cmd = new SqlCommand("check_range_availabilty", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@entered_name", str);

            cmd.Parameters.Add("@chk_count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            int ret_Value = (int)cmd.Parameters["@chk_count"].Value;
            connection.Dispose();
            return ret_Value;
        }



        public int RangeCount()
        {
            DataTable rangecount = new DataTable();
            rangecount = cDataSrc.ExecuteDataSet("Survey_RangeCount", null).Tables[0];
            cDataSrc = null;
            if (rangecount.Rows.Count > 0)
                return Convert.ToInt32(rangecount.Rows[0][0]);
            else
                return 0;
        
        }




        public int Programme_insert_Range(Question_Range_BE QR_BE,string mode)
        {
            
      object[] param = new object[6]
         {
            QR_BE.RangeID,
            QR_BE.name, 
            QR_BE.title,
            QR_BE.r_upto,
            QR_BE.rating_text,
            mode
          };

            int returnValue = Convert.ToInt32(cDataSrc.ExecuteNonQuery("insert_range", param, null));
            cDataSrc = null;
            return returnValue;
           
     }





       public DataTable Survey_UspGetRangeList()
       {
           DataTable dtRange = null;

           try
           {

               dtRange = cDataSrc.ExecuteDataSet("Survey_UspGetRangeList", null).Tables[0];
             }

           catch (Exception ex)
           {
               HandleException(ex); 
           }
           return dtRange;
   
       }





       public DataTable Survey_Edit_Range()
       {
           DataTable dtRange = null;

           try
           {

               dtRange = cDataSrc.ExecuteDataSet("Survey_Edit_Range", null).Tables[0];
           }

           catch (Exception ex)
           {
               HandleException(ex);
           }
           return dtRange;

       }


       public DataTable get_range_detail(int r_ID)
       {
           DataTable dtRange = null;
           try
           {
               object[] param = new object[1] { r_ID };
               dtRange = cDataSrc.ExecuteDataSet("Survey_Edit_Range", param, null).Tables[0];
           }

           catch (Exception ex)
           {
               HandleException(ex);
           }
           return dtRange;

       }



       public int? Survey_Delete_Range(int Range_Id)
       {
           int?  dtRange=null;

           try
           {
               object[] param = new object[1] { Range_Id };
              dtRange = cDataSrc.ExecuteNonQuery("Survey_Delete_RangeData", param, null);
           }

           catch (Exception ex)
           {
               HandleException(ex);
           }
           return dtRange;

       }
      }





   
}
