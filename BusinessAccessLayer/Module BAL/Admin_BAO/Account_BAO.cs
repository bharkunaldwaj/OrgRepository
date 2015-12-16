using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;
using Admin_BE;
using Admin_DAO;

using System.Data;

namespace Admin_BAO
{
    public class Account_BAO : Base_BAO
    {
        #region Private Member Variable

        private int addAccount;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Add account details
        /// </summary>
        /// <returns></returns>
        public int AddAccount(Account_BE accountBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                Account_DAO accountDataAccessObject = new Account_DAO();
                addAccount = accountDataAccessObject.AddAccount(accountBusinessEntity);
                //HandleWriteLog("End", new StackTrace(true));

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
            return addAccount;
        }

        /// <summary>
        /// update account details
        /// </summary>
        /// <returns></returns>
        public int UpdateAccount(Account_BE accountBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                Account_DAO accountDataAccessObject = new Account_DAO();
                addAccount = accountDataAccessObject.UpdateAccount(accountBusinessEntity);
                //HandleWriteLog("End", new StackTrace(true));

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
            return addAccount;
        }

        /// <summary>
        /// Delete account details
        /// </summary>
        /// <returns></returns>
        public int DeleteAccount(Account_BE accountBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                Account_DAO accountDataAccessObject = new Account_DAO();
                addAccount = accountDataAccessObject.DeleteAccount(accountBusinessEntity);
                //HandleWriteLog("End", new StackTrace(true));

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
            return addAccount;
        }

        /// <summary>
        /// Get Account details By Account ID
        /// </summary>
        /// <returns></returns>
        public List<Account_BE> GetAccountByID(int accountID)
        {
            List<Account_BE> accountBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Account_DAO accountDataAccessObject = new Account_DAO();
                accountDataAccessObject.GetAccountByID(accountID);

                accountBusinessEntityList = accountDataAccessObject.accountBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return accountBusinessEntityList;
        }

        /// <summary>
        /// Get Account list 
        /// </summary>
        /// <returns></returns>
        public List<Account_BE> GetAccountList()
        {
            List<Account_BE> accountBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Account_DAO accountDataAccessObject = new Account_DAO();
                accountDataAccessObject.GetAccountList();

                accountBusinessEntityList = accountDataAccessObject.accountBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return accountBusinessEntityList;
        }

        /// <summary>
        /// Get Account list By Account ID
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtAccountList(string accountID)
        {
            DataTable dataTableAccount = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Account_DAO accountDataAccessObject = new Account_DAO();
            dataTableAccount = accountDataAccessObject.GetdtAccountList(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableAccount;
        }

        /// <summary>
        /// Get Account list count By Account ID
        /// </summary>
        /// <returns></returns>
        public int GetAccountListCount(string accountID)
        {
            int accountCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Account_DAO accountDataAccessObject = new Account_DAO();
                accountCount = accountDataAccessObject.GetAccountListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return accountCount;
        }
        #endregion
    }
}
