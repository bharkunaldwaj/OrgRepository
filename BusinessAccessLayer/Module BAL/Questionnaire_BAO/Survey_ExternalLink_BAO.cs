using System;
using System.Diagnostics;

using DAF_BAO;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class Survey_ExternalLink_BAO : Base_BAO
    {
        /// <summary>
        /// Get External Link List by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetExternalLinkList(string accountID)
        {
            DataTable dataTableCompany = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Company_DAO companyDataAccessObject = new Survey_Company_DAO();
                dataTableCompany = companyDataAccessObject.GetdtCompanyList(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCompany;
        }
    }
}
