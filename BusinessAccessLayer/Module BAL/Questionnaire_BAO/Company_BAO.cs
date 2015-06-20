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
    public class Survey_Company_BAO : Base_BAO
    {


        public List<Survey_Company_BE> GetCompanyByID(int companyID)
        {
            List<Survey_Company_BE> company_BEList = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Company_DAO company_DAO = new Survey_Company_DAO();
                company_BEList = company_DAO.GetCompanyByID(companyID);
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return company_BEList;
        }

        public void UpdateProject(Survey_Company_BE comapnyBE)
        {
            throw new NotImplementedException();
        }

        public int AddCompany(Survey_Company_BE comapnyBE)
        {
            int addcompany = 0;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            Survey_Company_DAO company_DAO = new Survey_Company_DAO();
            addcompany = company_DAO.AddCompany(comapnyBE);

            dbTransaction.Commit();
            conn.Close();
            return addcompany;

        }

        public int DeleteCompany(Survey_Company_BE comapnyBE)
        {
            int addcompany = 0;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            Survey_Company_DAO company_DAO = new Survey_Company_DAO();
            addcompany = company_DAO.DeleteCompany(comapnyBE);

            dbTransaction.Commit();
            conn.Close();
            return addcompany;

        }

        public DataTable GetdtCompanyList(string accountID)
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
