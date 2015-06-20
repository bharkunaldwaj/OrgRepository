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

namespace Questionnaire_BAO {
    public class FeedbackCategory_BAO : Base_BAO {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        public DataTable SelectCategory(int accountID, int Questionnaireid) {
            DataTable categoryid = null;

            try {
                //HandleWriteLog("Start", new StackTrace(true));

                FeedbackCategory_DAO Category_DAO = new FeedbackCategory_DAO();
                categoryid = Category_DAO.getcategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }

            return categoryid;
        }



    }


}