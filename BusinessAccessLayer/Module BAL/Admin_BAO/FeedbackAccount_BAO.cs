
using DAF_BAO;
using Admin_DAO;

using System.Data;

namespace Admin_BAO
{
    public class FeedbackAccount_BAO : Base_BAO
    {
        #region Private Member Variable

        private int addAccount;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Get Account List by account id
        /// </summary>
        /// <param name="accountID">Account id.</param>
        /// <returns></returns>
        public DataTable GetdtAccountList(string accountID)
        {
            DataTable dataTableAccount = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

            FeedbackAccount_DAO accountDataAccessObject = new FeedbackAccount_DAO();
            dataTableAccount = accountDataAccessObject.GetdtAccountList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableAccount;
        }
        #endregion

    }
}
