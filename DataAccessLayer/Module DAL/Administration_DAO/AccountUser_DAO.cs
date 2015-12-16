using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

using feedbackFramework_DAO;

using Admin_BE;  // Admin_BE;
using DatabaseAccessUtilities;

namespace Admin_DAO
{
    public class AccountUser_DAO : DAO_Base
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

        public List<AccountUser_BE> accountuserBusinessEntityList { get; set; }

        #endregion

        # region CRUD Operation
        /// <summary>
        /// Insert account user details
        /// </summary>
        /// <param name="accountuserBusinessEntity"></param>
        /// <returns></returns>
        public int AddAccountUser(AccountUser_BE accountuserBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {null,
                                                accountuserBusinessEntity.LoginID,
                                                accountuserBusinessEntity.Password,
                                                accountuserBusinessEntity.GroupID ,
                                                accountuserBusinessEntity.AccountID,
                                                accountuserBusinessEntity.StatusID ,
                                                accountuserBusinessEntity.Salutation,
                                                accountuserBusinessEntity.FirstName,
                                                accountuserBusinessEntity.LastName,
                                                accountuserBusinessEntity.EmailID,
                                                accountuserBusinessEntity.Notification,
                                                accountuserBusinessEntity.ModifyBy,
                                                accountuserBusinessEntity.ModifyDate,
                                                accountuserBusinessEntity.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Update  account user details
        /// </summary>
        /// <param name="accountuserBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateAccountUser(AccountUser_BE accountuserBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {accountuserBusinessEntity.UserID,
                                                accountuserBusinessEntity.LoginID,
                                                accountuserBusinessEntity.Password,
                                                accountuserBusinessEntity.GroupID ,
                                                accountuserBusinessEntity.AccountID,
                                                accountuserBusinessEntity.StatusID ,
                                                accountuserBusinessEntity.Salutation,
                                                accountuserBusinessEntity.FirstName,
                                                accountuserBusinessEntity.LastName,
                                                accountuserBusinessEntity.EmailID,
                                                accountuserBusinessEntity.Notification,
                                                accountuserBusinessEntity.ModifyBy,
                                                accountuserBusinessEntity.ModifyDate,
                                                accountuserBusinessEntity.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountUserManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountUserManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Delete  account user details
        /// </summary>
        /// <param name="accountuserBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteAccountUser(AccountUser_BE accountuserBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[15] {accountuserBusinessEntity.UserID,
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

        /// <summary>
        ///  Get account user details
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="accountuserID">account user ID</param>
        /// <returns></returns>
        public int GetAccountUserByID(int accountID, int accountuserID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllAccountUser = new DataTable();
                object[] param = new object[3] { accountuserID, accountID, "I" };

                dataTableAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dataTableAllAccountUser);
                returnValue = 1;

                HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Get Account User List
        /// </summary>
        /// <returns></returns>
        public int GetAccountUserList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllAccountUser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dataTableAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dataTableAllAccountUser);
                returnValue = 1;

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Get Account User By ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="accountuserID">accountuser ID</param>
        /// <returns></returns>
        public DataTable GetdtAccountUserByID(int accountID, int accountuserID)
        {
            DataTable dataTableAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountuserID, accountID, "I" };

                dataTableAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllAccountUser;
        }

        /// <summary>
        /// Get Account User List by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtAccountUserList(string accountID)
        {
            DataTable dataTableAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };
                dataTableAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllAccountUser;
        }

        /// <summary>
        /// Get Account User List
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtAccountUserListNew(string condition)
        {
            DataTable dataTableAllAccountUser = new DataTable();
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

                dataTableAllAccountUser = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllAccountUser;
        }

        /// <summary>
        /// Get Participant List by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetParticipantList(string accountID)
        {
            DataTable dataTableAllAccountUser = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "P" };

                dataTableAllAccountUser = cDataSrc.ExecuteDataSet("UspAccountUserSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountUserSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllAccountUser;
        }

        #endregion

        /// <summary>
        /// Shift account user Data Table To BEList
        /// </summary>
        /// <param name="dataTableAccountUser"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAccountUser)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            accountuserBusinessEntityList = new List<AccountUser_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAccountUser.Rows.Count; recordCounter++)
            {
                AccountUser_BE accountuser_BE = new AccountUser_BE();

                accountuser_BE.UserID = Convert.ToInt32(dataTableAccountUser.Rows[recordCounter]["UserID"].ToString());
                accountuser_BE.AccountID = Convert.ToInt32(dataTableAccountUser.Rows[recordCounter]["AccountID"].ToString());
                accountuser_BE.LoginID = dataTableAccountUser.Rows[recordCounter]["LoginID"].ToString();
                accountuser_BE.Password = dataTableAccountUser.Rows[recordCounter]["Password"].ToString();
                accountuser_BE.GroupID = Convert.ToInt32(dataTableAccountUser.Rows[recordCounter]["GroupID"].ToString());
                accountuser_BE.AccountID = Convert.ToInt32(dataTableAccountUser.Rows[recordCounter]["AccountID"].ToString());
                accountuser_BE.StatusID = Convert.ToInt32(dataTableAccountUser.Rows[recordCounter]["StatusID"].ToString());
                accountuser_BE.Salutation = dataTableAccountUser.Rows[recordCounter]["Salutation"].ToString();
                accountuser_BE.FirstName = dataTableAccountUser.Rows[recordCounter]["FirstName"].ToString();
                accountuser_BE.LastName = dataTableAccountUser.Rows[recordCounter]["LastName"].ToString();
                accountuser_BE.EmailID = dataTableAccountUser.Rows[recordCounter]["EmailID"].ToString();
                accountuser_BE.Notification = Convert.ToBoolean(dataTableAccountUser.Rows[recordCounter]["Notification"].ToString());
                accountuser_BE.ModifyBy = Convert.ToInt32(dataTableAccountUser.Rows[recordCounter]["ModifyBy"].ToString());
                accountuser_BE.ModifyDate = Convert.ToDateTime(dataTableAccountUser.Rows[recordCounter]["ModifyDate"].ToString());
                accountuser_BE.IsActive = Convert.ToInt32(dataTableAccountUser.Rows[recordCounter]["IsActive"].ToString());
                accountuser_BE.Code = dataTableAccountUser.Rows[recordCounter]["Code"].ToString();

                accountuserBusinessEntityList.Add(accountuser_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Get Account User List Count
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get max user count
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get Account Admin
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtAccountAdmin(int accountID)
        {
            DataTable dataTableAllAccountUser = new DataTable();
            try
            {

                string sql = "SELECT FirstName + ' ' + LastName as FullName, EmailID" +
                            " FROM dbo.[User]" +
                            " WHERE (AccountID = " + accountID + ") AND (GroupID = 2)";

                dataTableAllAccountUser = cDataSrc.ExecuteDataSet(sql, null).Tables[0];

            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllAccountUser;
        }

        /// <summary>
        /// Insert Manager User
        /// </summary>
        /// <param name="managerDetailsBusinessEntity"></param>
        /// <returns></returns>
        public int SaveManagerUser(AccountUser_BE managerDetailsBusinessEntity)
        {
            int managerId = 0;
            try
            {
                object[] newparam = new object[15] {null,
                                                managerDetailsBusinessEntity.LoginID,
                                                managerDetailsBusinessEntity.Password,
                                                managerDetailsBusinessEntity.GroupID,
                                                managerDetailsBusinessEntity.AccountID,
                                                managerDetailsBusinessEntity.StatusID,
                                                managerDetailsBusinessEntity.Salutation,
                                                managerDetailsBusinessEntity.FirstName,
                                                managerDetailsBusinessEntity.LastName,
                                                managerDetailsBusinessEntity.EmailID,
                                                managerDetailsBusinessEntity.Notification,
                                                managerDetailsBusinessEntity.ModifyBy,
                                                managerDetailsBusinessEntity.ModifyDate,
                                                managerDetailsBusinessEntity.IsActive,
                                                "I" };

                managerId = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticiantUserManagement", newparam, null));
            }
            catch (Exception ex) { HandleException(ex); }
            return managerId;
        }
    }
}
