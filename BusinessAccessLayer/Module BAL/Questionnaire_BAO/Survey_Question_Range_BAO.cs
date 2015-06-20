using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;
using System.Data;

namespace Questionnaire_BAO
{
    public class Survey_Question_Range_BAO
    {
        public Survey_Question_Range_BAO()
        {
           
        }


        public DataTable get_range_detail(int r_ID)
        { 
        DataTable dtRange=null;
        Survey_Question_Range_DAO sur_edit_range = new Survey_Question_Range_DAO();
        dtRange = sur_edit_range.get_range_detail(r_ID);
        return dtRange;
         
        }




        public int chk_rng_Availability(string str)
        {
            Survey_Question_Range_DAO chk_rng_avail = new Survey_Question_Range_DAO();
           return  chk_rng_avail.chk_rng_Availability(str);
        }



        public int insert_range(Question_Range_BE QR_BE, string mode)
        {
            Survey_Question_Range_DAO i_insert=null;
            //try
            //{
                i_insert = new Survey_Question_Range_DAO();
                return i_insert.Programme_insert_Range(QR_BE,mode);
            ////////}
            ////////catch
            ////////{ }
            ////////return i_insert.Programme_insert_Range();
        }


        public int RangeCount()
        {
            Survey_Question_Range_DAO r_count = new Survey_Question_Range_DAO();
            int rr =r_count.RangeCount();
            return rr;

        }





//public int Range_DeleteProgramme( )
//{


//    Survey_Question_Range_DAO range_delete_DAO = null;
//    //try
//    //{
//    range_delete_DAO = new Survey_Question_Range_DAO();
//    int get_delete_result;
//    CSqlClient sqlClient = null;
//    IDbConnection conn = null;
//    IDbTransaction dbTransaction = null;

//    try
//    {
//        sqlClient = CDataSrc.Default as CSqlClient;
//        conn = sqlClient.Connection();
//        dbTransaction = conn.BeginTransaction();

//        //HandleWriteLog("Start", new StackTrace(true));
        
//     get_delete_result = range_delete_DAO.DeleteProgramme();
//        //HandleWriteLog("End", new StackTrace(true));

//        dbTransaction.Commit();
//        conn.Close();
//    }
//    catch (Exception ex)
//    {
//        if (dbTransaction != null)
//        {
//            dbTransaction.Rollback();
//        }

//       // HandleException(ex);
//    }
//    return 0; // get_delete_result;
//}


       




        public DataTable Survey_UspGetRangeList()
        {
            DataTable dtRange = null;
            Survey_Question_Range_DAO q_range_DAO = new Survey_Question_Range_DAO();
            dtRange = q_range_DAO.Survey_UspGetRangeList();
            return dtRange;
        }



        public DataTable Survey_Edit_Range()
        {
            DataTable dtRange = null;
            Survey_Question_Range_DAO q_range_DAO = new Survey_Question_Range_DAO();
            dtRange = q_range_DAO.Survey_Edit_Range();
            return dtRange;
        }



        public int? Survey_Delete_Range(int Range_Id)
        {
            int? dtRange;
            //DataTable dtRange = null;
            Survey_Question_Range_DAO q_range_DAO = new Survey_Question_Range_DAO();
            dtRange = q_range_DAO.Survey_Delete_Range(Range_Id);
            return dtRange;
        }






        //public int delete_range()
           
    }
}
