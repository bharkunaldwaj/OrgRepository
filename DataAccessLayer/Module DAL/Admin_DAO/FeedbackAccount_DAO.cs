using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

using feedbackFramework_DAO;

using Admin_BE;
using DatabaseAccessUtilities;

namespace Admin_DAO
{
    public class FeedbackAccount_DAO : DAO_Base
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

        public List<FeedbackAccount_BE> accountBusinessEntityList { get; set; }

        #endregion

        # region CRUD Operation
        /// <summary>
        /// Get Account List by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
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
        /// Shift account Data Table To BE List
        /// </summary>
        /// <param name="dataTableAccount"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAccount)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            accountBusinessEntityList = new List<FeedbackAccount_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAccount.Rows.Count; recordCounter++)
            {
                FeedbackAccount_BE accountBusinessEntity = new FeedbackAccount_BE();

                accountBusinessEntity.AccountID = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["AccountID"].ToString());
                accountBusinessEntity.Code = dataTableAccount.Rows[recordCounter]["Code"].ToString();
                accountBusinessEntity.LoginID = dataTableAccount.Rows[recordCounter]["LoginID"].ToString();
                accountBusinessEntity.Password = dataTableAccount.Rows[recordCounter]["Password"].ToString();
                accountBusinessEntity.OrganisationName = dataTableAccount.Rows[recordCounter]["OrganisationName"].ToString();
                accountBusinessEntity.AccountTypeID = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["AccountTypeID"].ToString());
                accountBusinessEntity.Description = dataTableAccount.Rows[recordCounter]["Description"].ToString();
                accountBusinessEntity.EmailID = dataTableAccount.Rows[recordCounter]["EmailID"].ToString();
                accountBusinessEntity.Website = dataTableAccount.Rows[recordCounter]["Website"].ToString();
                accountBusinessEntity.StatusID = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["StatusID"].ToString());
                accountBusinessEntity.CompanyLogo = dataTableAccount.Rows[recordCounter]["CompanyLogo"].ToString();
                accountBusinessEntity.CopyRightLine = dataTableAccount.Rows[recordCounter]["CopyRightLine"].ToString();
                accountBusinessEntity.HeaderBGColor = dataTableAccount.Rows[recordCounter]["HeaderBGColor"].ToString();
                accountBusinessEntity.MenuBGColor = dataTableAccount.Rows[recordCounter]["MenuBGColor"].ToString();
                accountBusinessEntity.ModifyBy = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["ModifyBy"].ToString());
                accountBusinessEntity.ModifyDate = Convert.ToDateTime(dataTableAccount.Rows[recordCounter]["ModifyDate"].ToString());
                accountBusinessEntity.IsActive = Convert.ToInt32(dataTableAccount.Rows[recordCounter]["IsActive"].ToString());

                accountBusinessEntityList.Add(accountBusinessEntity);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }
    }
}
