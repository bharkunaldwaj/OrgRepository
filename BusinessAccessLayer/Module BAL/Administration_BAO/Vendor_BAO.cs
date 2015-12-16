/*  
* PURPOSE: This is the Business Access Object for User Entity
* AUTHOR: 
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
 *                    <Key2><Reason 2 >
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Administration_BE;
using Administration_DAO;

using System.Data;

namespace Administration_BAO
{
    public class Vendor_BAO : Base_BAO
    {

        #region "Private Member Variable"

        private int addVendor;

        #endregion

        #region "Business Logic for Vendor BAO"

        /// <summary>
        /// This Method returns the Users from User_DAO on passing User_BE as a parameter
        /// </summary>
        /// <param name="userBusinessEntity"></param>
        /// <returns></returns>
        public List<User_BE> GetVendorById(User_BE userBusinessEntity)
        {
            List<User_BE> userBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                User_DAO userDataAccessObject = new User_DAO();
                userBusinessEntity.IsActive = true;

                userDataAccessObject.GetUser(userBusinessEntity);

                userBusinessEntityList = userDataAccessObject.UserBusinessEntityList;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return userBusinessEntityList;
        }

        /// <summary>
        /// This Method returns the Users from User_DAO on passing User_BE, PageIndex
        /// </summary>
        /// <param name="userBusinessEntity"></param>
        /// <param name="p_pageIndex"></param>
        /// <returns></returns>
        public List<User_BE> GetVendor(User_BE userBusinessEntity)
        {
            List<User_BE> userBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                User_DAO userDataAccessObject = new User_DAO();
                userBusinessEntity.IsActive = true;

                userDataAccessObject.GetUsers(userBusinessEntity);

                userBusinessEntityList = userDataAccessObject.UserBusinessEntityList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return userBusinessEntityList;
        }

        /// <summary>
        /// This method passes the User_BE entity to User_DAO and performs an add operation
        /// </summary>
        /// <param name="userBusinessEntity"></param>
        public int AddVendor(User_BE userBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                User_DAO userDataAccessObject = new User_DAO();

                addVendor = userDataAccessObject.AddUser(userBusinessEntity);

                HandleWriteLog("End", new StackTrace(true));
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
            return addVendor;
        }

        /// <summary>
        /// This method passes the User_BE entity to User_DAO and performs an update operation
        /// </summary>
        /// <param name="userBusinessEntity"></param>
        public void UpdateVendor(User_BE userBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                User_DAO userDataAccessObject = new User_DAO();

                userDataAccessObject.UpdateUser(userBusinessEntity);

                HandleWriteLog("End", new StackTrace(true));
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
        }

        /// <summary>
        /// This method passes the User_BE entity to User_DAO and performs a delete operation
        /// </summary>
        /// <param name="userBusinessEntity"></param>
        public void DeleteVendor(User_BE userBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                User_DAO userDataAccessObject = new User_DAO();
                User_BE BusinessEntityUser = new User_BE();
                List<User_BE> userBusinessEntityList;

                BusinessEntityUser.UserID = userBusinessEntity.UserID;

                userDataAccessObject.GetUser(BusinessEntityUser);
                userBusinessEntityList = userDataAccessObject.UserBusinessEntityList;

                BusinessEntityUser = userBusinessEntityList[0];
                BusinessEntityUser.IsActive = false;
                UpdateVendor(BusinessEntityUser);

                HandleWriteLog("End", new StackTrace(true));
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
        }

        /// <summary>
        /// Get vender details llist by userid
        /// </summary>
        /// <returns></returns>
        public List<User_BE> GetData()
        {
            User_BE userBusinessEntity = new User_BE();
            List<User_BE> userBEList = GetVendorById(userBusinessEntity);
            return userBEList;
        }

        /// <summary>
        /// Get vender details 
        /// </summary>
        /// <returns></returns>
        public List<User_BE> GetVendorDetails()
        {
            User_BE userBusinessEntity = (User_BE)System.Web.HttpContext.Current.Session["userBE"];
            List<User_BE> userBusinessEntityList = GetVendor(userBusinessEntity);
            return userBusinessEntityList;
        }

        /// <summary>
        /// Delete vender details by userid
        /// </summary>
        /// <returns></returns>
        public void DeleteVendorDetails()
        {
            User_BE userBusinessEntity = (User_BE)System.Web.HttpContext.Current.Session["user_BE_Delete"];
            DeleteVendor(userBusinessEntity);
        }

        /// <summary>
        /// update User_BE values
        /// </summary>
        /// <returns></returns>
        private User_BE UpdateBE(User_BE userBusinessEntity, string type)
        {
            User_BE userBusinessEntityPrimary = new User_BE();
            User_DAO userDataAccessObject = new User_DAO();
            List<User_BE> userBusinessEntityList = new List<User_BE>();

            userBusinessEntityPrimary.UserID = userBusinessEntity.UserID;

            userDataAccessObject.GetUser(userBusinessEntityPrimary);

            userBusinessEntityList = userDataAccessObject.UserBusinessEntityList;

            User_BE userBusinessEntityLatest = new User_BE();

            userBusinessEntityLatest = userBusinessEntityList[0];

            if (type == "U")
            {
                userBusinessEntityLatest.FName = userBusinessEntity.FName;
                userBusinessEntityLatest.Email = userBusinessEntity.Email;
                userBusinessEntityLatest.IsActive = userBusinessEntity.IsActive;
                userBusinessEntityLatest.Address1 = userBusinessEntity.Address1;
                userBusinessEntityLatest.Address2 = userBusinessEntity.Address2;
                userBusinessEntityLatest.City = userBusinessEntity.City;
                userBusinessEntityLatest.State = userBusinessEntity.State;
                userBusinessEntityLatest.CountryID = userBusinessEntity.CountryID;
                userBusinessEntityLatest.Zip = userBusinessEntity.Zip;
                userBusinessEntityLatest.TelNumber = userBusinessEntity.TelNumber;
                userBusinessEntityLatest.FaxNumber = userBusinessEntity.FaxNumber;
                userBusinessEntityLatest.Note = userBusinessEntity.Note;
                userBusinessEntityLatest.IsConfirmed = userBusinessEntity.IsConfirmed;
                userBusinessEntityLatest.Type = userBusinessEntity.Type;
                userBusinessEntityLatest.BPNumber = userBusinessEntity.BPNumber;
            }
            else
            {
                userBusinessEntityLatest.IsActive = false;
            }

            return userBusinessEntityLatest;
        }
        #endregion
    }
}
