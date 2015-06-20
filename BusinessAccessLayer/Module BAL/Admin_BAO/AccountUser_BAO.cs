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
    public class AccountUser_BAO:Base_BAO
    {

        #region Private Member Variable

        private int addAccountUser;
        int maxid;
        #endregion

        #region CRUD Operations

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

        public int SaveManagerUser(AccountUser_BE managerDetails_BE)
        {
            int managerId = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AccountUser_DAO accountuser_DAO = new AccountUser_DAO();
                managerId = accountuser_DAO.SaveManagerUser(managerDetails_BE);

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

        public int AddAccountUser(Survey_AccountUser_BE accountuser_BE)
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
                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
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

        public int UpdateAccountUser(Survey_AccountUser_BE accountuser_BE)
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
                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
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

        public int DeleteAccountUser(Survey_AccountUser_BE accountuser_BE)
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
                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
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

        public List<Survey_AccountUser_BE> GetAccountUserByID(int accountID, int accountuserID)
        {
            List<Survey_AccountUser_BE> accountuser_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
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


        public DataTable GetdtAccountUserByID(int accountID, int accountuserID)
        {
            DataTable dtAllAccountUser = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
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

        public DataTable GetdtAccountUserList(string accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetdtAccountUserList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }

        public DataTable GetdtAccountUserListNew(string accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetdtAccountUserListNew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }

        public DataTable GetParticipantList(string accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetParticipantList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }

        public int GetAccountUserListCount(string accountID)
        {
            int accountuserCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
                accountuserCount = accountuser_DAO.GetAccountUserListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return accountuserCount;
        }

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
                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
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


        public DataTable GetdtAccountAdmin(int accountID)
        {
            DataTable dtAccountUser = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
                dtAccountUser = accountuser_DAO.GetdtAccountAdmin(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountUser;
        }

        public int SaveManagerUser(Survey_AccountUser_BE managerDetails_BE)
        {
            int managerId = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_AccountUser_DAO accountuser_DAO = new Survey_AccountUser_DAO();
                managerId = accountuser_DAO.SaveManagerUser(managerDetails_BE);

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
