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

    public class User_BAO : Base_BAO {
        #region "Business Logic for User BAO"

        /// <summary>
        /// This Method returns the Users from User_DAO on passing User_BE as a parameter
        /// </summary>
        /// <param name="p_user_BE"></param>
        /// <returns></returns>

        public List<User_BE> GetUser(User_BE user_BE) {
            List<User_BE> user_BEList = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));
                User_DAO user_DAO = new User_DAO();
                
                user_DAO.GetUser(user_BE);

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

        public void AddUser(User_BE p_user_BE) {
            CSqlClient sqlCient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlCient = CDataSrc.Default as CSqlClient;
                conn = sqlCient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                User_DAO user_DAO = new User_DAO();
                user_DAO.AddUser(p_user_BE);
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

        public void UpdateUserSession(User_BE p_user_BE)
        {
            CSqlClient sqlCient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlCient = CDataSrc.Default as CSqlClient;
                conn = sqlCient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                User_DAO user_DAO = new User_DAO();
                user_DAO.UpdateUserSession(p_user_BE);
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
        /// This method passes the User_BE entity to User_DAO and performs an update operation
        /// </summary>
        /// <param name="p_user_BE"></param>

        public void UpdateUser(User_BE p_user_BE) {
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

        public void DeleteUser(User_BE p_user_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                User_DAO user_DAO = new User_DAO();
                p_user_BE.IsActive = false;
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

        #endregion
    }
}
