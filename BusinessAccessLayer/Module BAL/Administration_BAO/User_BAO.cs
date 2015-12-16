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

    public class User_BAO : Base_BAO
    {
        #region "Business Logic for User BAO"
        /// <summary>
        /// This Method returns the Users from User_DAO on passing User_BE as a parameter
        /// </summary>
        /// <param name="p_user_BE"></param>
        /// <returns></returns>
        public List<User_BE> GetUser(User_BE userBusinessEntity)
        {
            List<User_BE> userBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                User_DAO userDataAccessObject = new User_DAO();

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
        /// This method passes the User_BE entity to User_DAO and performs an add operation
        /// </summary>
        /// <param name="userBusinessEntity"></param>
        public void AddUser(User_BE userBusinessEntity)
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

                User_DAO userDataAccessObject = new User_DAO();

                userDataAccessObject.AddUser(userBusinessEntity);

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
        /// Update session value.
        /// </summary>
        /// <param name="userBusinessEntity"></param>
        public void UpdateUserSession(User_BE userBusinessEntity)
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

                User_DAO userDataAccessObject = new User_DAO();

                userDataAccessObject.UpdateUserSession(userBusinessEntity);

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
        /// <param name="userBusinessEntity"></param>
        public void UpdateUser(User_BE userBusinessEntity)
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
        public void DeleteUser(User_BE userBusinessEntity)
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

                userBusinessEntity.IsActive = false;

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
        #endregion
    }
}
