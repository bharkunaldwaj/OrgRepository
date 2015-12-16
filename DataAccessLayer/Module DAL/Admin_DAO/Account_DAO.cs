using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

using feedbackFramework_DAO;

using Admin_BE;
using DatabaseAccessUtilities;

namespace Admin_DAO
{
    public class Account_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Account_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Account_BE> accountBusinessEntityList { get; set; }

        #endregion

        # region CRUD Operation
        /// <summary>
        /// Insert account details
        /// </summary>
        /// <param name="accountBusinessEntity"></param>
        /// <returns></returns>
        public int AddAccount(Account_BE accountBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[19] {null,
                                                accountBusinessEntity.Code,
                                                accountBusinessEntity.LoginID,
                                                accountBusinessEntity.Password,
                                                accountBusinessEntity.OrganisationName,
                                                accountBusinessEntity.AccountTypeID,
                                                accountBusinessEntity.Description,
                                                accountBusinessEntity.EmailID,
                                                accountBusinessEntity.Website,
                                                accountBusinessEntity.StatusID,
                                                accountBusinessEntity.CompanyLogo,
                                                accountBusinessEntity.CopyRightLine,
                                                accountBusinessEntity.HeaderBGColor,
                                                accountBusinessEntity.MenuBGColor,
                                                accountBusinessEntity.ModifyBy,
                                                accountBusinessEntity.ModifyDate,
                                                accountBusinessEntity.IsActive,
                                                accountBusinessEntity.EmailPseudonym,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// update account details
        /// </summary>
        /// <param name="accountBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateAccount(Account_BE accountBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[19] {accountBusinessEntity.AccountID,
                                                accountBusinessEntity.Code,
                                                accountBusinessEntity.LoginID,
                                                accountBusinessEntity.Password,
                                                accountBusinessEntity.OrganisationName,
                                                accountBusinessEntity.AccountTypeID,
                                                accountBusinessEntity.Description,
                                                accountBusinessEntity.EmailID,
                                                accountBusinessEntity.Website,
                                                accountBusinessEntity.StatusID,
                                                accountBusinessEntity.CompanyLogo,
                                                accountBusinessEntity.CopyRightLine,
                                                accountBusinessEntity.HeaderBGColor,
                                                accountBusinessEntity.MenuBGColor,
                                                accountBusinessEntity.ModifyBy,
                                                accountBusinessEntity.ModifyDate,
                                                accountBusinessEntity.IsActive,
                                                accountBusinessEntity.EmailPseudonym,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Delete account details
        /// </summary>
        /// <param name="accountBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteAccount(Account_BE accountBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] {accountBusinessEntity.AccountID,
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
                                                null,
                                                null,
                                                null,
                                                "D" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Get Account details By ID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int GetAccountByID(int accountID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllAccount = new DataTable();
                object[] param = new object[2] { accountID, "I" };

                dtAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllAccount);
                returnValue = 1;

                HandleWriteLogDAU("UspAccountSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Get Account List
        /// </summary>
        /// <returns></returns>
        public int GetAccountList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllAccount = new DataTable();
                object[] param = new object[2] { null, "A" };

                dataTableAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dataTableAllAccount);
                returnValue = 1;

                //HandleWriteLogDAU("UspAccountSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Get Account List
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtAccountList(string accountID)
        {
            DataTable dataTableAllAccount = new DataTable();
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[2] { Convert.ToInt32(accountID), "A" };

            dataTableAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

            //HandleWriteLogDAU("UspAccountSelect", param, new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dataTableAllAccount;
        }

        #endregion
        /// <summary>
        /// Add datatable to Business entity list
        /// </summary>
        /// <param name="dataTableAccount"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAccount)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            accountBusinessEntityList = new List<Account_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAccount.Rows.Count; recordCounter++)
            {
                Account_BE account_BE = new Account_BE();

                account_BE.AccountID = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["AccountID"].ToString());
                account_BE.Code = dataTableAccount.Rows[recordCounter]["Code"].ToString();
                account_BE.LoginID = dataTableAccount.Rows[recordCounter]["LoginID"].ToString();
                account_BE.Password = dataTableAccount.Rows[recordCounter]["Password"].ToString();
                account_BE.OrganisationName = dataTableAccount.Rows[recordCounter]["OrganisationName"].ToString();
                account_BE.AccountTypeID = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["AccountTypeID"].ToString());
                account_BE.Description = dataTableAccount.Rows[recordCounter]["Description"].ToString();
                account_BE.EmailID = dataTableAccount.Rows[recordCounter]["EmailID"].ToString();
                account_BE.Website = dataTableAccount.Rows[recordCounter]["Website"].ToString();
                account_BE.StatusID = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["StatusID"].ToString());
                account_BE.CompanyLogo = dataTableAccount.Rows[recordCounter]["CompanyLogo"].ToString();
                account_BE.CopyRightLine = dataTableAccount.Rows[recordCounter]["CopyRightLine"].ToString();
                account_BE.HeaderBGColor = dataTableAccount.Rows[recordCounter]["HeaderBGColor"].ToString();
                account_BE.MenuBGColor = dataTableAccount.Rows[recordCounter]["MenuBGColor"].ToString();
                account_BE.EmailPseudonym = dataTableAccount.Rows[recordCounter].Field<string>("Pseudonym");
                // account_BE.ModifyBy = dtAccount.Rows[recordCounter]["ModifyBy"]==null || dtAccount.Rows[recordCounter]["ModifyBy"].ToString()==""? 0 : Convert.ToInt32(dtAccount.Rows[recordCounter]["ModifyBy"].ToString());
                // account_BE.ModifyDate =Convert.ToDateTime(dtAccount.Rows[recordCounter]["ModifyDate"].ToString());
                account_BE.IsActive = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["IsActive"].ToString());

                accountBusinessEntityList.Add(account_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Get Account List Count by account id.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int GetAccountListCount(string accountID)
        {
            int accountCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[2] { Convert.ToInt32(accountID), "C" };

                accountCount = (int)cDataSrc.ExecuteScalar("UspAccountSelect", param, null);

                //HandleWriteLogDAU("UspAccountSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return accountCount;
        }
    }
}
