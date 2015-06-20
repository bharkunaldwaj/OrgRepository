using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
using feedbackFramework_DAO;

using Admin_BE;
using DatabaseAccessUtilities;

namespace Admin_DAO
{
    public class AccountUser_DAO:DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public AccountUser_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"
        
        public List<AccountUser_BE> accountuser_BEList { get; set; }
        
        #endregion

        # region CRUD Operation

        public int AddAccountUser(AccountUser_BE accountuser_BE)
        {
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {null,
                                                accountuser_BE.LoginID,
                                                accountuser_BE.Password,
                                                accountuser_BE.GroupID ,
                                                accountuser_BE.AccountID,
                                                accountuser_BE.StatusID ,
                                                accountuser_BE.Salutation,
                                                accountuser_BE.FirstName,
                                                accountuser_BE.LastName,
                                                accountuser_BE.EmailID,
                                                accountuser_BE.Notification,
                                                accountuser_BE.ModifyBy,
                                                accountuser_BE.ModifyDate,
                                                accountuser_BE.IsActive,
                                                "I" };
                
                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateAccountUser(AccountUser_BE accountuser_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {accountuser_BE.UserID,
                                                accountuser_BE.LoginID,
                                                accountuser_BE.Password,
                                                accountuser_BE.GroupID ,
                                                accountuser_BE.AccountID,
                                                accountuser_BE.StatusID ,
                                                accountuser_BE.Salutation,
                                                accountuser_BE.FirstName,
                                                accountuser_BE.LastName,
                                                accountuser_BE.EmailID,
                                                accountuser_BE.Notification,
                                                accountuser_BE.ModifyBy,
                                                accountuser_BE.ModifyDate,
                                                accountuser_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteAccountUser(AccountUser_BE accountuser_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {accountuser_BE.UserID,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
    
        public int GetAccountUserByID(int accountID, int accountuserID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllAccountUser = new DataTable();
                object[] param = new object[3] { accountuserID, accountID, "I" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllAccountUser);
                returnValue = 1;

                HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetAccountUserList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllAccountUser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllAccountUser);
                returnValue = 1;

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtAccountUserByID(int accountID, int accountuserID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[3] { accountuserID, accountID, "I" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public DataTable GetdtAccountUserList(string accountID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };
                dtAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public DataTable GetdtAccountUserListNew(string condition)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {

                string sql = "SELECT Account.Code, " +
                            "[Group].GroupName, " +
                            "[User].UserID, " +
                            "[User].LoginID, " +
                            "[User].Password," +
                            "[User].GroupID, " +
                            "[User].AccountID, " +
                            "[User].StatusID, " +
                            "[User].Salutation, " +
                            "[User].FirstName + ' ' + [User].LastName as UserName, " +
                            "[User].EmailID, " +
                            "[User].[Notification]," +
                            "[User].ModifyBy, " +
                            "[User].ModifyDate, " +
                            "[User].IsActive" +
                            " FROM   Account INNER JOIN" +
                            " [User] ON  Account.AccountID =  [User].AccountID INNER JOIN " +
                            " [Group] ON  [User].GroupID = [Group].GroupID" +
                            " WHERE [User].IsActive=1 AND [User].AccountID=" + condition +
                            " order by [User].AccountID Desc";

                dtAllAccountUser = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public DataTable GetParticipantList(string accountID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "P" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        #endregion 

        private void ShiftDataTableToBEList(DataTable dtAccountUser)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            accountuser_BEList = new List<AccountUser_BE>();

            for (int recordCounter = 0; recordCounter < dtAccountUser.Rows.Count; recordCounter++)
            {
                AccountUser_BE accountuser_BE = new AccountUser_BE();

                accountuser_BE.UserID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["UserID"].ToString());
                accountuser_BE.AccountID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["AccountID"].ToString());
                accountuser_BE.LoginID = dtAccountUser.Rows[recordCounter]["LoginID"].ToString();
                accountuser_BE.Password = dtAccountUser.Rows[recordCounter]["Password"].ToString();
                accountuser_BE.GroupID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["GroupID"].ToString());
                accountuser_BE.AccountID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["AccountID"].ToString());
                accountuser_BE.StatusID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["StatusID"].ToString());
                accountuser_BE.Salutation = dtAccountUser.Rows[recordCounter]["Salutation"].ToString();
                accountuser_BE.FirstName = dtAccountUser.Rows[recordCounter]["FirstName"].ToString();
                accountuser_BE.LastName = dtAccountUser.Rows[recordCounter]["LastName"].ToString();
                accountuser_BE.EmailID = dtAccountUser.Rows[recordCounter]["EmailID"].ToString();
                accountuser_BE.Notification = Convert.ToBoolean(dtAccountUser.Rows[recordCounter]["Notification"].ToString());
                accountuser_BE.ModifyBy = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["ModifyBy"].ToString());
                accountuser_BE.ModifyDate = Convert.ToDateTime(dtAccountUser.Rows[recordCounter]["ModifyDate"].ToString());
                accountuser_BE.IsActive = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["IsActive"].ToString());
                accountuser_BE.Code = dtAccountUser.Rows[recordCounter]["Code"].ToString();

                accountuser_BEList.Add(accountuser_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetAccountUserListCount(string condition)
        {
            int accountuserCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[3] { null, Convert.ToInt32(accountID), "C" };
                //accountuserCount = (int)cDataSrc.ExecuteScalar("UspAccountUserSelect", param, null);

                string sql ="SELECT Count(Account.Code) " +
                            " FROM   Account INNER JOIN" +
                            " [User] ON  Account.AccountID =  [User].AccountID INNER JOIN " +
                            " [Group] ON  [User].GroupID = [Group].GroupID" +
                            " WHERE [User].IsActive=1 AND [User].AccountID=" + condition;

                //object[] param = new object[2] { condition, "A" };
                accountuserCount = (int)cDataSrc.ExecuteScalar(sql, null);

                //object[] param = new object[2] { condition, "C" };
                //accountuserCount = (int)cDataSrc.ExecuteScalar("UspAccountUserSearch", param, null);

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return accountuserCount;
        }

        public int MaxUser()
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[0] { };




                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspUserMax", param, null));

                // returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspUserMaintenance", cNameValueList, null));

                cDataSrc = null;

                HandleWriteLogDAU("UspUserMax ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }


        public DataTable GetdtAccountAdmin(int accountID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {
                
                string sql = "SELECT FirstName + ' ' + LastName as FullName, EmailID" +
                            " FROM dbo.[User]" +
                            " WHERE (AccountID = " + accountID + ") AND (GroupID = 2)";

                dtAllAccountUser = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public int SaveManagerUser(AccountUser_BE managerDetails_BE)
        {
            int managerId = 0;
            try
            {
                object[] newparam = new object[15] {null,
                                                managerDetails_BE.LoginID,
                                                managerDetails_BE.Password,
                                                managerDetails_BE.GroupID,
                                                managerDetails_BE.AccountID,
                                                managerDetails_BE.StatusID,
                                                managerDetails_BE.Salutation,
                                                managerDetails_BE.FirstName,
                                                managerDetails_BE.LastName,
                                                managerDetails_BE.EmailID,
                                                managerDetails_BE.Notification,
                                                managerDetails_BE.ModifyBy,
                                                managerDetails_BE.ModifyDate,
                                                managerDetails_BE.IsActive,
                                                "I" };

                managerId = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticiantUserManagement", newparam, null));
            }
            catch (Exception ex) { HandleException(ex); }
            return managerId;
        }
    }

































    public class Survey_AccountUser_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_AccountUser_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Survey_AccountUser_BE> accountuser_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public int AddAccountUser(Survey_AccountUser_BE accountuser_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {null,
                                                accountuser_BE.LoginID,
                                                accountuser_BE.Password,
                                                accountuser_BE.GroupID ,
                                                accountuser_BE.AccountID,
                                                accountuser_BE.StatusID ,
                                                accountuser_BE.Salutation,
                                                accountuser_BE.FirstName,
                                                accountuser_BE.LastName,
                                                accountuser_BE.EmailID,
                                                accountuser_BE.Notification,
                                                accountuser_BE.ModifyBy,
                                                accountuser_BE.ModifyDate,
                                                accountuser_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateAccountUser(Survey_AccountUser_BE accountuser_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {accountuser_BE.UserID,
                                                accountuser_BE.LoginID,
                                                accountuser_BE.Password,
                                                accountuser_BE.GroupID ,
                                                accountuser_BE.AccountID,
                                                accountuser_BE.StatusID ,
                                                accountuser_BE.Salutation,
                                                accountuser_BE.FirstName,
                                                accountuser_BE.LastName,
                                                accountuser_BE.EmailID,
                                                accountuser_BE.Notification,
                                                accountuser_BE.ModifyBy,
                                                accountuser_BE.ModifyDate,
                                                accountuser_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteAccountUser(Survey_AccountUser_BE accountuser_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {accountuser_BE.UserID,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetAccountUserByID(int accountID, int accountuserID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllAccountUser = new DataTable();
                object[] param = new object[3] { accountuserID, accountID, "I" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("Survey_UspAccountUserSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllAccountUser);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetAccountUserList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllAccountUser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("Survey_UspAccountUserSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllAccountUser);
                returnValue = 1;

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtAccountUserByID(int accountID, int accountuserID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountuserID, accountID, "I" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("Survey_UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public DataTable GetdtAccountUserList(string accountID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };
                dtAllAccountUser = cDataSrc.ExecuteDataSet("Survey_UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public DataTable GetdtAccountUserListNew(string condition)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {

                string sql = "SELECT Account.Code, " +
                            "[Survey_Group].GroupName, " +
                            "[User].UserID, " +
                            "[User].LoginID, " +
                            "[User].Password," +
                            "[User].GroupID, " +
                            "[User].AccountID, " +
                            "[User].StatusID, " +
                            "[User].Salutation, " +
                            "[User].FirstName + ' ' + [User].LastName as UserName, " +
                            "[User].EmailID, " +
                            "[User].[Notification]," +
                            "[User].ModifyBy, " +
                            "[User].ModifyDate, " +
                            "[User].IsActive" +
                            " FROM   Account INNER JOIN" +
                            " [User] ON  Account.AccountID =  [User].AccountID INNER JOIN " +
                            " [Survey_Group] ON  [User].GroupID = [Survey_Group].GroupID" +
                            " WHERE [User].IsActive=1 AND [User].AccountID=" + condition +
                            " order by [User].AccountID Desc";

                dtAllAccountUser = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public DataTable GetParticipantList(string accountID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "P" };

                dtAllAccountUser = cDataSrc.ExecuteDataSet("Survey_UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        #endregion

        private void ShiftDataTableToBEList(DataTable dtAccountUser)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            accountuser_BEList = new List<Survey_AccountUser_BE>();

            for (int recordCounter = 0; recordCounter < dtAccountUser.Rows.Count; recordCounter++)
            {
                Survey_AccountUser_BE accountuser_BE = new Survey_AccountUser_BE();

                accountuser_BE.UserID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["UserID"].ToString());
                accountuser_BE.AccountID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["AccountID"].ToString());
                accountuser_BE.LoginID = dtAccountUser.Rows[recordCounter]["LoginID"].ToString();
                accountuser_BE.Password = dtAccountUser.Rows[recordCounter]["Password"].ToString();
                accountuser_BE.GroupID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["GroupID"].ToString());
                accountuser_BE.AccountID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["AccountID"].ToString());
                accountuser_BE.StatusID = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["StatusID"].ToString());
                accountuser_BE.Salutation = dtAccountUser.Rows[recordCounter]["Salutation"].ToString();
                accountuser_BE.FirstName = dtAccountUser.Rows[recordCounter]["FirstName"].ToString();
                accountuser_BE.LastName = dtAccountUser.Rows[recordCounter]["LastName"].ToString();
                accountuser_BE.EmailID = dtAccountUser.Rows[recordCounter]["EmailID"].ToString();
                accountuser_BE.Notification = Convert.ToBoolean(dtAccountUser.Rows[recordCounter]["Notification"].ToString());
                accountuser_BE.ModifyBy = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["ModifyBy"].ToString());
                accountuser_BE.ModifyDate = Convert.ToDateTime(dtAccountUser.Rows[recordCounter]["ModifyDate"].ToString());
                accountuser_BE.IsActive = Convert.ToInt32(dtAccountUser.Rows[recordCounter]["IsActive"].ToString());
                accountuser_BE.Code = dtAccountUser.Rows[recordCounter]["Code"].ToString();

                accountuser_BEList.Add(accountuser_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public int GetAccountUserListCount(string condition)
        {
            int accountuserCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //object[] param = new object[3] { null, Convert.ToInt32(accountID), "C" };
                //accountuserCount = (int)cDataSrc.ExecuteScalar("UspAccountUserSelect", param, null);

                string sql = "SELECT Count(Account.Code) " +
                            " FROM   Account INNER JOIN" +
                            " [User] ON  Account.AccountID =  [User].AccountID INNER JOIN " +
                            " [Survey_Group] ON  [User].GroupID = [Survey_Group].GroupID" +
                            " WHERE [User].IsActive=1 AND [User].AccountID=" + condition;

                //object[] param = new object[2] { condition, "A" };
                accountuserCount = (int)cDataSrc.ExecuteScalar(sql, null);

                //object[] param = new object[2] { condition, "C" };
                //accountuserCount = (int)cDataSrc.ExecuteScalar("UspAccountUserSearch", param, null);

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return accountuserCount;
        }

        public int MaxUser()
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[0] { };




                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspUserMax", param, null));

                // returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspUserMaintenance", cNameValueList, null));

                cDataSrc = null;

                HandleWriteLogDAU("UspUserMax ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }


        public DataTable GetdtAccountAdmin(int accountID)
        {
            DataTable dtAllAccountUser = new DataTable();
            try
            {

                string sql = "SELECT FirstName + ' ' + LastName as FullName, EmailID" +
                            " FROM dbo.[User]" +
                            " WHERE (AccountID = " + accountID + ") AND (GroupID = 2)";

                dtAllAccountUser = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllAccountUser;
        }

        public int SaveManagerUser(Survey_AccountUser_BE managerDetails_BE)
        {
            int managerId = 0;
            try
            {
                object[] newparam = new object[15] {null,
                                                managerDetails_BE.LoginID,
                                                managerDetails_BE.Password,
                                                managerDetails_BE.GroupID,
                                                managerDetails_BE.AccountID,
                                                managerDetails_BE.StatusID,
                                                managerDetails_BE.Salutation,
                                                managerDetails_BE.FirstName,
                                                managerDetails_BE.LastName,
                                                managerDetails_BE.EmailID,
                                                managerDetails_BE.Notification,
                                                managerDetails_BE.ModifyBy,
                                                managerDetails_BE.ModifyDate,
                                                managerDetails_BE.IsActive,
                                                "I" };

                managerId = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticiantUserManagement", newparam, null));
            }
            catch (Exception ex) { HandleException(ex); }
            return managerId;
        }
    }
}
