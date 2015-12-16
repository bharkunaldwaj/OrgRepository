using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;
using Admin_BE;
using Admin_DAO;

using System.Data;

namespace Admin_BAO
{
    public class AccountUser_BAO : Base_BAO
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
        public int AddAccountUser(AccountUser_BE accountuserBusinessEntity)
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
                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                addAccountUser = accountUserDataAccessObject.AddAccountUser(accountuserBusinessEntity);
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
        public int UpdateAccountUser(AccountUser_BE accountuserBusinessEntity)
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
                AccountUser_DAO accountuserDataAccessObject = new AccountUser_DAO();
                addAccountUser = accountuserDataAccessObject.UpdateAccountUser(accountuserBusinessEntity);
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
        public int DeleteAccountUser(AccountUser_BE accountuserBusinessEntity)
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
                AccountUser_DAO accountuserDataAccessObject = new AccountUser_DAO();
                addAccountUser = accountuserDataAccessObject.DeleteAccountUser(accountuserBusinessEntity);
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
            List<AccountUser_BE> accountuserBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                accountUserDataAccessObject.GetAccountUserByID(accountID, accountuserID);

                accountuserBusinessEntityList = accountUserDataAccessObject.accountuserBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return accountuserBusinessEntityList;
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

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                dtAllAccountUser = accountUserDataAccessObject.GetdtAccountUserByID(accountID, accountuserID);

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

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetdtAccountUserList(accountID);

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

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetdtAccountUserListNew(accountID);

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

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetParticipantList(accountID);

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

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                accountuserCount = accountUserDataAccessObject.GetAccountUserListCount(accountID);

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
                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                maxid = accountUserDataAccessObject.MaxUser();
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

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetdtAccountAdmin(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }

        /// <summary>
        /// Insert Manager details
        /// </summary>
        public int SaveManagerUser(AccountUser_BE managerDetailsBusinessEntity)
        {
            int managerId = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountUserDataAccessObject = new AccountUser_DAO();
                managerId = accountUserDataAccessObject.SaveManagerUser(managerDetailsBusinessEntity);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return managerId;
        }
    }

    public class Survey_AccountUser_BAO : Base_BAO
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
        public int AddAccountUser(Survey_AccountUser_BE accountuserBusinessEntity)
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
                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                addAccountUser = accountUserDataAccessObject.AddAccountUser(accountuserBusinessEntity);
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
        public int UpdateAccountUser(Survey_AccountUser_BE accountuserBusinessEntity)
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
                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                addAccountUser = accountUserDataAccessObject.UpdateAccountUser(accountuserBusinessEntity);
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
        public int DeleteAccountUser(Survey_AccountUser_BE accountuserBusinessEntity)
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
                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                addAccountUser = accountUserDataAccessObject.DeleteAccountUser(accountuserBusinessEntity);
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
        public List<Survey_AccountUser_BE> GetAccountUserByID(int accountID, int accountuserID)
        {
            List<Survey_AccountUser_BE> accountuserBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                accountUserDataAccessObject.GetAccountUserByID(accountID, accountuserID);

                accountuserBusinessEntityList = accountUserDataAccessObject.accountuserBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return accountuserBusinessEntityList;
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

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                dtAllAccountUser = accountUserDataAccessObject.GetdtAccountUserByID(accountID, accountuserID);

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

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetdtAccountUserList(accountID);

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

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetdtAccountUserListNew(accountID);

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

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetParticipantList(accountID);

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

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                accountuserCount = accountUserDataAccessObject.GetAccountUserListCount(accountID);

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
                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                maxid = accountUserDataAccessObject.MaxUser();
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

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                dtAccountUser = accountUserDataAccessObject.GetdtAccountAdmin(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }

        /// <summary>
        /// Insert Manager details
        /// </summary>
        public int SaveManagerUser(Survey_AccountUser_BE managerDetailsBusinessEntity)
        {
            int managerId = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountUserDataAccessObject = new Survey_AccountUser_DAO();
                managerId = accountUserDataAccessObject.SaveManagerUser(managerDetailsBusinessEntity);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return managerId;
        }
    }
}