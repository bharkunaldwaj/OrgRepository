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
    public class Account_BAO:Base_BAO
    {
        #region Private Member Variable

        private int addAccount;

        #endregion

        #region CRUD Operations

        public int AddAccount(Account_BE account_BE)
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
                Account_DAO account_DAO = new Account_DAO();
                addAccount = account_DAO.AddAccount(account_BE);
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

        public int UpdateAccount(Account_BE account_BE)
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
                Account_DAO account_DAO = new Account_DAO();
                addAccount = account_DAO.UpdateAccount(account_BE);
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

        public int DeleteAccount(Account_BE account_BE)
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
                Account_DAO account_DAO = new Account_DAO();
                addAccount = account_DAO.DeleteAccount(account_BE);
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

        public List<Account_BE> GetAccountByID(int accountID)
        {
            List<Account_BE> account_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Account_DAO account_DAO = new Account_DAO();
                account_DAO.GetAccountByID(accountID);

                account_BEList = account_DAO.account_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return account_BEList;
        }

        public List<Account_BE> GetAccountList()
        {
            List<Account_BE> account_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Account_DAO account_DAO = new Account_DAO();
                account_DAO.GetAccountList();

                account_BEList = account_DAO.account_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return account_BEList;
        }

        public DataTable GetdtAccountList(string accountID)
        {
            DataTable dtAccount = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

               Account_DAO account_DAO = new Account_DAO();
                dtAccount = account_DAO.GetdtAccountList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtAccount;
        }

        public int GetAccountListCount(string accountID)
        {
            int accountCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Account_DAO account_DAO = new Account_DAO();
                accountCount = account_DAO.GetAccountListCount(accountID);

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
