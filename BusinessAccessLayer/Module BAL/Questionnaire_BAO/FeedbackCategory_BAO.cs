using System;

using DAF_BAO;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class FeedbackCategory_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        /// <summary>
        /// Get Category
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="Questionnaireid">Questionnaire id</param>
        /// <returns></returns>
        public DataTable SelectCategory(int accountID, int Questionnaireid)
        {
            DataTable categoryid = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                FeedbackCategory_DAO CategoryDataAccessObject = new FeedbackCategory_DAO();
                categoryid = CategoryDataAccessObject.getcategory(accountID, Questionnaireid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryid;
        }
    }
}