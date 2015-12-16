using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using System.IO;
using DatabaseAccessUtilities;
using Admin_BE;
using Admin_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Admin_BAO
{
    public class AccountUser_BAO//:Base_BAO
    {

        #region Private Member Variable

        private int addAccountUser;
        int maxid;
        #endregion

        #region CRUD Operations
        /// <summary>
        /// Add account user
        /// </summary>
        /// <returns></returns>
        public int AddAccountUser(AccountUser_BE accountuser_BE)
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
                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                addAccountUser = accountuser_DAO.AddAccountUser(accountuser_BE);
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
            return addAccountUser;
        }
        /// <summary>
        /// update account user
        /// </summary>
        /// <returns></returns>
        public int UpdateAccountUser(AccountUser_BE accountuser_BE)
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
                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                addAccountUser = accountuser_DAO.UpdateAccountUser(accountuser_BE);
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
            return addAccountUser;
        }
        /// <summary>
        /// Delete account user
        /// </summary>
        /// <returns></returns>
        public int DeleteAccountUser(AccountUser_BE accountuser_BE)
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
                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                addAccountUser = accountuser_DAO.DeleteAccountUser(accountuser_BE);
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
            return addAccountUser;
        }
        /// <summary>
        /// Get account user by id.
        /// </summary>
        /// <returns></returns>
        public List<AccountUser_BE> GetAccountUserByID(int accountID, int accountuserID)
        {
            List<AccountUser_BE> accountuser_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                accountuser_DAO.GetAccountUserByID(accountID, accountuserID);

                accountuser_BEList = accountuser_DAO.accountuser_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return accountuser_BEList;
        }

        /// <summary>
        /// Get account user by account id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtAccountUserByID(int accountID, int accountuserID)
        {
            DataTable dtAllAccountUser = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                dtAllAccountUser = accountuser_DAO.GetdtAccountUserByID(accountID, accountuserID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtAllAccountUser;
        }

        //public List<AccountUser_BE> GetAccountUserList()
        //{
        //    List<AccountUser_BE> accountuser_BEList = null;

        //    try
        //    {
        //        //HandleWriteLog("Start", new StackTrace(true));

        //        AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
        //        accountuser_DAO.GetAccountUserList();

        //        accountuser_BEList = accountuser_DAO.accountuser_BEList;

        //        //HandleWriteLog("End", new StackTrace(true));
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //    return accountuser_BEList;
        //}

        /// <summary>
        /// Get account user list by account id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtAccountUserList(string accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetdtAccountUserList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }
        /// <summary>
        /// Get account user list by account id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtAccountUserListNew(string accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetdtAccountUserListNew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }
        /// <summary>
        /// Get Participant list by account id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetParticipantList(string accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetParticipantList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }
        /// <summary>
        /// Get Account User List Count by account id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int GetAccountUserListCount(string accountID)
        {
            int accountuserCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                accountuserCount = accountuser_DAO.GetAccountUserListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return accountuserCount;
        }
        /// <summary>
        /// Count number of user 
        /// </summary>
        /// <returns></returns>
        public int MaxUser()
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
                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                maxid = accountuser_DAO.MaxUser();
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
            return maxid;
        }



        #endregion

        /// <summary>
        /// Get Account Admin details
        /// </summary>
        /// <param name="accountID"> account id.</param>
        /// <returns></returns>
        public DataTable GetdtAccountAdmin(int accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetdtAccountAdmin(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }
        /// <summary>
        /// Use to Handle exceptions
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            //ExceptionLogger.Write(ex.ToString());
            FileStream FS;
            StreamWriter SW;
            string fpath;

            string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                        "Error Application: Feedback 360 - UI" + Environment.NewLine + "," +
                        "Error Function: " + ex.TargetSite + Environment.NewLine + "," +
                        "Error Line: " + ex.StackTrace + Environment.NewLine + "," +
                        "Error Source: " + ex.Source + Environment.NewLine + "," +
                        "Error Message: " + ex.Message + Environment.NewLine;

            fpath = System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"].ToString() + "ErrorLog.txt";

            if (File.Exists(fpath))
            { FS = new FileStream(fpath, FileMode.Append, FileAccess.Write); }
            else
            { FS = new FileStream(fpath, FileMode.Create, FileAccess.Write); }

            string msg = str.Replace(",", "");

            SW = new StreamWriter(FS);
            SW.WriteLine(msg);

            SW.Close();
            FS.Close();

        }

    }
}
