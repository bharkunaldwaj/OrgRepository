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
    public class FeedbackAccount_DAO:DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public FeedbackAccount_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"

        public List<FeedbackAccount_BE> account_BEList { get; set; }
        
        #endregion

        # region CRUD Operation

       

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
            account_BEList = new List<FeedbackAccount_BE>();

            for (int recordCounter = 0; recordCounter < dtAccount.Rows.Count; recordCounter++)
            {
                FeedbackAccount_BE account_BE = new FeedbackAccount_BE();

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
                account_BE.ModifyBy = Convert.ToInt32(dtAccount.Rows[recordCounter]["ModifyBy"].ToString());
                account_BE.ModifyDate = Convert.ToDateTime(dtAccount.Rows[recordCounter]["ModifyDate"].ToString());
                account_BE.IsActive = Convert.ToInt32(dtAccount.Rows[recordCounter]["IsActive"].ToString());

                account_BEList.Add(account_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

       
    }
}
