using System;
using System.Collections.Generic;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class Survey_Company_BAO : Base_BAO
    {
        /// <summary>
        /// Get Company By company ID
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns></returns>
        public List<Survey_Company_BE> GetCompanyByID(int companyID)
        {
            List<Survey_Company_BE> companyBusinessEntityList = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Company_DAO companyDataAccessObject = new Survey_Company_DAO();
                companyBusinessEntityList = companyDataAccessObject.GetCompanyByID(companyID);
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return companyBusinessEntityList;
        }

        /// <summary>
        /// No in use
        /// </summary>
        /// <param name="comapnyBusinessEntity"></param>
        public void UpdateProject(Survey_Company_BE comapnyBusinessEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert Company
        /// </summary>
        /// <param name="comapnyBusinessEntity"></param>
        /// <returns></returns>
        public int AddCompany(Survey_Company_BE comapnyBusinessEntity)
        {
            int addcompany = 0;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            Survey_Company_DAO companyDataAccessObject = new Survey_Company_DAO();
            addcompany = companyDataAccessObject.AddCompany(comapnyBusinessEntity);

            dbTransaction.Commit();
            conn.Close();
            return addcompany;

        }

        /// <summary>
        /// Delete Company
        /// </summary>
        /// <param name="comapnyBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteCompany(Survey_Company_BE comapnyBusinessEntity)
        {
            int addcompany = 0;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            Survey_Company_DAO companyDataAccessObject = new Survey_Company_DAO();
            addcompany = companyDataAccessObject.DeleteCompany(comapnyBusinessEntity);

            dbTransaction.Commit();
            conn.Close();
            return addcompany;

        }

        /// <summary>
        /// Get company list by account id.
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtCompanyList(string accountID)
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
