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

namespace Questionnaire_BAO
{
    public class Survey_ExternalLink_BAO : Base_BAO
    {
        public DataTable GetExternalLinkList(string accountID)
        {
            DataTable dtCompany = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Company_DAO company_DAO = new Survey_Company_DAO();
                dtCompany = company_DAO.GetdtCompanyList(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCompany;
        }
    }
}
