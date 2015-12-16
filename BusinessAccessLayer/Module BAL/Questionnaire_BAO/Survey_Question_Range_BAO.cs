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

        /// <summary>
        /// Get range detail by range id
        /// </summary>
        /// <param name="rangeID">range id</param>
        /// <returns></returns>
        public DataTable get_range_detail(int rangeID)
        {
            DataTable dataTableRange = null;
            Survey_Question_Range_DAO rangeDataAccessObject = new Survey_Question_Range_DAO();
            dataTableRange = rangeDataAccessObject.get_range_detail(rangeID);
            return dataTableRange;

        }

        /// <summary>
        /// check range Availability
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int chk_rng_Availability(string str)
        {
            Survey_Question_Range_DAO checkk_rang_available = new Survey_Question_Range_DAO();
            return checkk_rang_available.chk_rng_Availability(str);
        }

        /// <summary>
        /// Inserrt range
        /// </summary>
        /// <param name="questionRangeBusinessEntity"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int insert_range(Question_Range_BE questionRangeBusinessEntity, string mode)
        {
            Survey_Question_Range_DAO rangeDataAccessObject = null;
            //try
            //{
            rangeDataAccessObject = new Survey_Question_Range_DAO();
            return rangeDataAccessObject.Programme_insert_Range(questionRangeBusinessEntity, mode);
            ////////}
            ////////catch
            ////////{ }
            ////////return i_insert.Programme_insert_Range();
        }

        /// <summary>
        /// Range count
        /// </summary>
        /// <returns></returns>
        public int RangeCount()
        {
            Survey_Question_Range_DAO rangeCountDataAccessObject = new Survey_Question_Range_DAO();
            int rangeCount = rangeCountDataAccessObject.RangeCount();
            return rangeCount;

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

        /// <summary>
        /// Get range list
        /// </summary>
        /// <returns></returns>
        public DataTable Survey_UspGetRangeList()
        {
            DataTable dataTableRange = null;
            Survey_Question_Range_DAO rangeDataAccessObject = new Survey_Question_Range_DAO();
            dataTableRange = rangeDataAccessObject.Survey_UspGetRangeList();
            return dataTableRange;
        }

        /// <summary>
        /// Get range details for edit 
        /// </summary>
        /// <returns></returns>
        public DataTable Survey_Edit_Range()
        {
            DataTable dataTableRange = null;
            Survey_Question_Range_DAO rangeDataAccessObject = new Survey_Question_Range_DAO();
            dataTableRange = rangeDataAccessObject.Survey_Edit_Range();
            return dataTableRange;
        }

        /// <summary>
        /// Delete Range by range id.
        /// </summary>
        /// <param name="rangeId"></param>
        /// <returns></returns>
        public int? Survey_Delete_Range(int rangeId)
        {
            int? dtRange;
            //DataTable dtRange = null;
            Survey_Question_Range_DAO rangeDataAccessObject = new Survey_Question_Range_DAO();
            dtRange = rangeDataAccessObject.Survey_Delete_Range(rangeId);
            return dtRange;
        }
        //public int delete_range()

    }
}
