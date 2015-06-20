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
using System.Linq;
using System.Text;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Administration_BE;
using Administration_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Administration_BAO {
    public class Vendor_BAO : Base_BAO {

        #region "Private Member Variable"

        private int addVendor;

        #endregion


        #region "Business Logic for Vendor BAO"

        /// <summary>
        /// This Method returns the Users from User_DAO on passing User_BE as a parameter
        /// </summary>
        /// <param name="p_user_BE"></param>
        /// <returns></returns>

        public List<User_BE> GetVendorById(User_BE p_user_BE) {
            List<User_BE> user_BEList = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));

                User_DAO user_DAO = new User_DAO();
                p_user_BE.IsActive = true;
                user_DAO.GetUser(p_user_BE);

                user_BEList = user_DAO.User_BEList;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return user_BEList;
        }

        /// <summary>
        /// This Method returns the Users from User_DAO on passing User_BE, PageIndex
        /// </summary>
        /// <param name="p_user_BE"></param>
        /// <param name="p_pageIndex"></param>
        /// <returns></returns>

        public List<User_BE> GetVendor(User_BE p_user_BE) {
            List<User_BE> user_BEList = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));

                User_DAO user_DAO = new User_DAO();
                p_user_BE.IsActive = true;
                user_DAO.GetUsers(p_user_BE);

                user_BEList = user_DAO.User_BEList;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return user_BEList;
        }

        /// <summary>
        /// This method passes the User_BE entity to User_DAO and performs an add operation
        /// </summary>
        /// <param name="p_user_BE"></param>
        public int AddVendor(User_BE p_user_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                User_DAO user_DAO = new User_DAO();
                addVendor = user_DAO.AddUser(p_user_BE);
                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex) {
                if (dbTransaction != null) {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
            return addVendor;
        }

        /// <summary>
        /// This method passes the User_BE entity to User_DAO and performs an update operation
        /// </summary>
        /// <param name="p_user_BE"></param>
        public void UpdateVendor(User_BE p_user_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                User_DAO user_DAO = new User_DAO();

                user_DAO.UpdateUser(p_user_BE);

                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex) {
                if (dbTransaction != null) {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        /// <summary>
        /// This method passes the User_BE entity to User_DAO and performs a delete operation
        /// </summary>
        /// <param name="p_user_BE"></param>
        public void DeleteVendor(User_BE p_user_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                
                User_DAO user_DAO = new User_DAO();                
                User_BE user_BE = new User_BE();
                List<User_BE> user_BEList;

                user_BE.UserID = p_user_BE.UserID;

                user_DAO.GetUser(user_BE);
                user_BEList = user_DAO.User_BEList;

                user_BE = user_BEList[0];
                user_BE.IsActive = false;
                UpdateVendor(user_BE);

                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex) {
                if (dbTransaction != null) {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        public List<User_BE> GetData() {
            User_BE user_BE = new User_BE();
            List<User_BE> userBEList = GetVendorById(user_BE);
            return userBEList;
        }

        public List<User_BE> GetVendorDetails() {
            User_BE user_BE = (User_BE)System.Web.HttpContext.Current.Session["userBE"];
            List<User_BE> user_BEList = GetVendor(user_BE);
            return user_BEList;
        }
        public void DeleteVendorDetails() {
            User_BE p_user_BE =(User_BE)System.Web.HttpContext.Current.Session["user_BE_Delete"];
            DeleteVendor(p_user_BE);
        }

        private User_BE UpdateBE(User_BE p_user_BE,string p_type ) {
            User_BE user_BEPrimary = new User_BE();
            User_DAO user_DAO = new User_DAO();
            List<User_BE> user_BEList = new List<User_BE>();
            
            user_BEPrimary.UserID = p_user_BE.UserID;

            user_DAO.GetUser(user_BEPrimary);

            user_BEList = user_DAO.User_BEList;

            User_BE p_user_BELatest = new User_BE();

            p_user_BELatest = user_BEList[0];

            if (p_type == "U") {
                p_user_BELatest.FName = p_user_BE.FName;
                p_user_BELatest.Email = p_user_BE.Email;
                p_user_BELatest.IsActive = p_user_BE.IsActive;
                p_user_BELatest.Address1 = p_user_BE.Address1;
                p_user_BELatest.Address2 = p_user_BE.Address2;
                p_user_BELatest.City = p_user_BE.City;
                p_user_BELatest.State = p_user_BE.State;
                p_user_BELatest.CountryID = p_user_BE.CountryID;
                p_user_BELatest.Zip = p_user_BE.Zip;
                p_user_BELatest.TelNumber = p_user_BE.TelNumber;
                p_user_BELatest.FaxNumber = p_user_BE.FaxNumber;
                p_user_BELatest.Note = p_user_BE.Note;
                p_user_BELatest.IsConfirmed = p_user_BE.IsConfirmed;
                p_user_BELatest.Type = p_user_BE.Type;
                p_user_BELatest.BPNumber = p_user_BE.BPNumber;
            }
            else {
                p_user_BELatest.IsActive = false;}

            return p_user_BELatest;
        }
        #endregion
    }
}
