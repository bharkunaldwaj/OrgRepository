/*  
* PURPOSE: This is the Business Access Object for Country Entity
* AUTHOR: Manish Mathur
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

using DAF_BAO;

using Administration_BE;
using Administration_DAO;
using DatabaseAccessUtilities;
using System.Data;

namespace Administration_BAO
{
    public class Country_BAO : Base_BAO
    {

        #region "Business Logic for Contact BAO"
        /// <summary>
        /// This Method returns the Countries from Country_DAO on passing Country_BE as a parameter
        /// </summary>
        /// <param name="p_Country_BE"></param>
        /// <returns></returns>
        /// 
        public List<Country_BE> GetCountry(Country_BE countryBusinessEntity)
        {
            List<Country_BE> countryBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Country_DAO countryDataAccessObject = new Country_DAO();

                countryDataAccessObject.GetCountry(countryBusinessEntity);

                countryBusinessEntityList = countryDataAccessObject.CountryBusinessEntityList;
                HandleWriteLog("End", new StackTrace(true));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return countryBusinessEntityList;
        }

        /// <summary>
        /// This method passes the Country_BE entity to Country_DAO and performs an add operation
        /// </summary>
        /// <param name="p_Country_BE"></param>
        public void AddCountry(Country_BE countryBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                Country_DAO countryDataAccessObject = new Country_DAO();

                countryDataAccessObject.AddCountry(countryBusinessEntity);

                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        /// <summary>
        /// This method passes the Country_BE entity to Country_DAO and performs an update operation
        /// </summary>
        /// <param name="p_Country_BE"></param>
        public void UpdateCountry(Country_BE countryBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                Country_DAO countryDataAccessObject = new Country_DAO();

                countryDataAccessObject.UpdateCountry(countryBusinessEntity);

                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        /// <summary>
        /// This method passes the Country_BE entity to Country_DAO and performs an delete operation
        /// </summary>
        /// <param name="p_Country_BE"></param>
        public void DeleteCountry(Country_BE countryBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                Country_DAO countryDataAccessObject = new Country_DAO();

                countryDataAccessObject.UpdateCountry(countryBusinessEntity);

                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }
        #endregion
    }
}
