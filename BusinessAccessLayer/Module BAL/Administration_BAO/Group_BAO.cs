/*  
* PURPOSE: This is the Business Access Object for Group Entity
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
    public class Group_BAO : Base_BAO {
        #region "Business Logic for Group BAO"

        /// <summary>
        /// This Method returns the Group from Group_DAO on passing Group_BE as a parameter
        /// </summary>
        /// <param name="p_Group_BE"></param>
        /// <returns></returns>
        public List<Group_BE> GetGroup(Group_BE p_group_BE) {
            List<Group_BE> Group_BEList = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO Group_DAO = new Group_DAO();

                Group_DAO.GetGroup(p_group_BE);

                Group_BEList = Group_DAO.Group_BEList;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return Group_BEList;
        }

        /// <summary>
        /// To check if Group Exists
        /// </summary>
        /// <param name="p_group_BE"></param>
        /// <returns></returns>
        public bool IsGroupExist(Group_BE p_group_BE) {
            Group_DAO Group_DAO = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO = new Group_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return Group_DAO.IsGroupExist(p_group_BE);

        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an add operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public int AddGroup(Group_BE p_group_BE) {
            Group_DAO Group_DAO = null;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO = new Group_DAO();
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
            return Group_DAO.AddGroup(p_group_BE);
        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an update operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public void UpdateGroup(Group_BE p_group_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO Group_DAO = new Group_DAO();
                Group_DAO.UpdateGroup(p_group_BE);
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
        /// This method passes the Group_BE entity to Group_DAO and performs an delete operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public void DeleteGroup(Group_BE p_group_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO Group_DAO = new Group_DAO();
                Group_DAO.DeleteGroup(p_group_BE);
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

        public DataTable GetdtGroupList(string accountID)
        {
            DataTable dtGroup = null;
            
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Group_DAO group_DAO = new Group_DAO();
                dtGroup = group_DAO.GetdtGroupList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtGroup;
        }

        public int GetGroupListCount(string accountID)
        {
            int groupCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Group_DAO group_DAO = new Group_DAO();
                groupCount = group_DAO.GetGroupListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return groupCount;
        }
    }




    

    public class Survey_Group_BAO : Base_BAO
    {
        #region "Business Logic for Group BAO"

        /// <summary>
        /// This Method returns the Group from Group_DAO on passing Group_BE as a parameter
        /// </summary>
        /// <param name="p_Group_BE"></param>
        /// <returns></returns>
        public List<Survey_Group_BE> GetGroup(Survey_Group_BE p_group_BE)
        {
            List<Survey_Group_BE> Group_BEList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Survey_Group_DAO Group_DAO = new Survey_Group_DAO();

                Group_DAO.GetGroup(p_group_BE);

                Group_BEList = Group_DAO.Group_BEList;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return Group_BEList;
        }

        /// <summary>
        /// To check if Group Exists
        /// </summary>
        /// <param name="p_group_BE"></param>
        /// <returns></returns>
        public bool IsGroupExist(Survey_Group_BE p_group_BE)
        {
            Survey_Group_DAO Group_DAO = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO = new Survey_Group_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return Group_DAO.IsGroupExist(p_group_BE);

        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an add operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public int AddGroup(Survey_Group_BE p_group_BE)
        {
            Survey_Group_DAO Group_DAO = null;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO = new Survey_Group_DAO();
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
            return Group_DAO.AddGroup(p_group_BE);
        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an update operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public void UpdateGroup(Survey_Group_BE p_group_BE)
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
                Survey_Group_DAO Group_DAO = new Survey_Group_DAO();
                Group_DAO.UpdateGroup(p_group_BE);
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
        /// This method passes the Group_BE entity to Group_DAO and performs an delete operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public void DeleteGroup(Survey_Group_BE p_group_BE)
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
                Survey_Group_DAO Group_DAO = new Survey_Group_DAO();
                Group_DAO.DeleteGroup(p_group_BE);
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

        public DataTable GetdtGroupList(string accountID)
        {
            DataTable dtGroup = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Group_DAO group_DAO = new Survey_Group_DAO();
                dtGroup = group_DAO.GetdtGroupList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtGroup;
        }

        public int GetGroupListCount(string accountID)
        {
            int groupCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Group_DAO group_DAO = new Survey_Group_DAO();
                groupCount = group_DAO.GetGroupListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return groupCount;
        }
    }





}
