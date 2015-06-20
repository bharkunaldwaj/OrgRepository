using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Admin_BE;
using Admin_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Admin_BAO
{
    public class FeedbackAccount_BAO : Base_BAO
    {
        #region Private Member Variable

        private int addAccount;

        #endregion

        #region CRUD Operations

        public DataTable GetdtAccountList(string accountID)
        {
            DataTable dtAccount = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

            FeedbackAccount_DAO account_DAO = new FeedbackAccount_DAO();
            dtAccount = account_DAO.GetdtAccountList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtAccount;
        }

        #endregion

    }
}
