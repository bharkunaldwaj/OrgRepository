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
    public class Account_DAO:DAO_Base
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
        
        public List<Account_BE> account_BEList { get; set; }
        
        #endregion

        # region CRUD Operation

        public int AddAccount(Account_BE account_BE)
        {
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] {null,
                                                account_BE.Code,
                                                account_BE.LoginID,
                                                account_BE.Password,
                                                account_BE.OrganisationName,
                                                account_BE.AccountTypeID,
                                                account_BE.Description,
                                                account_BE.EmailID,
                                                account_BE.Website,
                                                account_BE.StatusID,
                                                account_BE.CompanyLogo,
                                                account_BE.CopyRightLine,
                                                account_BE.HeaderBGColor,
                                                account_BE.MenuBGColor,
                                                account_BE.ModifyBy,
                                                account_BE.ModifyDate,
                                                account_BE.IsActive,
                                                "I" };
                
                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int UpdateAccount(Account_BE account_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] {account_BE.AccountID,
                                                account_BE.Code,
                                                account_BE.LoginID,
                                                account_BE.Password,
                                                account_BE.OrganisationName,
                                                account_BE.AccountTypeID,
                                                account_BE.Description,
                                                account_BE.EmailID,
                                                account_BE.Website,
                                                account_BE.StatusID,
                                                account_BE.CompanyLogo,
                                                account_BE.CopyRightLine,
                                                account_BE.HeaderBGColor,
                                                account_BE.MenuBGColor,
                                                account_BE.ModifyBy,
                                                account_BE.ModifyDate,
                                                account_BE.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAccountManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspAccountManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int DeleteAccount(Account_BE account_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] {account_BE.AccountID,
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
    
        public int GetAccountByID(int accountID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllAccount = new DataTable();
                object[] param = new object[2] { accountID,"I" };

                dtAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllAccount);
                returnValue = 1;

                HandleWriteLogDAU("UspAccountSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetAccountList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllAccount = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAllAccount);
                returnValue = 1;

                //HandleWriteLogDAU("UspAccountSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtAccountList(string accountID)
        {
            DataTable dtAllAccount = new DataTable();
            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[2] { Convert.ToInt32(accountID), "A" };

                dtAllAccount = cDataSrc.ExecuteDataSet("UspAccountSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspAccountSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtAllAccount;
        }

        #endregion 

        private void ShiftDataTableToBEList(DataTable dtAccount)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            account_BEList = new List<Account_BE>();

            for (int recordCounter = 0; recordCounter < dtAccount.Rows.Count; recordCounter++)
            {
                Account_BE account_BE = new Account_BE();

                account_BE.AccountID = Convert.ToInt32(dtAccount.Rows[recordCounter]["AccountID"].ToString());
                account_BE.Code = dtAccount.Rows[recordCounter]["Code"].ToString();
                account_BE.LoginID = dtAccount.Rows[recordCounter]["LoginID"].ToString();
                account_BE.Password = dtAccount.Rows[recordCounter]["Password"].ToString();
                account_BE.OrganisationName = dtAccount.Rows[recordCounter]["OrganisationName"].ToString();
                account_BE.AccountTypeID = Convert.ToInt32(dtAccount.Rows[recordCounter]["AccountTypeID"].ToString());
                account_BE.Description = dtAccount.Rows[recordCounter]["Description"].ToString();
                account_BE.EmailID = dtAccount.Rows[recordCounter]["EmailID"].ToString();
                account_BE.Website = dtAccount.Rows[recordCounter]["Website"].ToString();
                account_BE.StatusID = Convert.ToInt32(dtAccount.Rows[recordCounter]["StatusID"].ToString());
                account_BE.CompanyLogo = dtAccount.Rows[recordCounter]["CompanyLogo"].ToString();
                account_BE.CopyRightLine = dtAccount.Rows[recordCounter]["CopyRightLine"].ToString();
                account_BE.HeaderBGColor = dtAccount.Rows[recordCounter]["HeaderBGColor"].ToString();
                account_BE.MenuBGColor = dtAccount.Rows[recordCounter]["MenuBGColor"].ToString();
               // account_BE.ModifyBy = dtAccount.Rows[recordCounter]["ModifyBy"]==null || dtAccount.Rows[recordCounter]["ModifyBy"].ToString()==""? 0 : Convert.ToInt32(dtAccount.Rows[recordCounter]["ModifyBy"].ToString());
               // account_BE.ModifyDate =Convert.ToDateTime(dtAccount.Rows[recordCounter]["ModifyDate"].ToString());
                account_BE.IsActive = Convert.ToInt32(dtAccount.Rows[recordCounter]["IsActive"].ToString());

                account_BEList.Add(account_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

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
