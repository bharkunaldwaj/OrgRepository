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
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Administration_BE;
using Administration_DAO;
using System.Data;


namespace Administration_BAO
{
    public class Group_BAO : Base_BAO
    {
        #region "Business Logic for Group BAO"

        /// <summary>
        /// This Method returns the Group from Group_DAO on passing Group_BE as a parameter
        /// </summary>
        /// <param name="p_Group_BE"></param>
        /// <returns></returns>
        public List<Group_BE> GetGroup(Group_BE groupBusinessEntity)
        {
            List<Group_BE> GroupBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Group_DAO GroupDataAccessObject = new Group_DAO();

                GroupDataAccessObject.GetGroup(groupBusinessEntity);

                GroupBusinessEntityList = GroupDataAccessObject._groupBusinessEntityList;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return GroupBusinessEntityList;
        }

        /// <summary>
        /// To check if Group Exists
        /// </summary>
        /// <param name="groupBusinessEntity"></param>
        /// <returns></returns>
        public bool IsGroupExist(Group_BE groupBusinessEntity)
        {
            Group_DAO GroupDataAccessObject = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                GroupDataAccessObject = new Group_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return GroupDataAccessObject.IsGroupExist(groupBusinessEntity);

        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an add operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public int AddGroup(Group_BE groupBusinessEntity)
        {
            Group_DAO GroupDataAccessObject = null;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                GroupDataAccessObject = new Group_DAO();

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
            return GroupDataAccessObject.AddGroup(groupBusinessEntity);
        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an update operation
        /// </summary>
        /// <param name="p_group_BE"></param>
        public void UpdateGroup(Group_BE groupBusinessEntity)
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

                Group_DAO GroupDataAccessObject = new Group_DAO();

                GroupDataAccessObject.UpdateGroup(groupBusinessEntity);

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
        /// <param name="groupBusinessEntity"></param>
        public void DeleteGroup(Group_BE groupBusinessEntity)
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

                Group_DAO GroupDataAccessObject = new Group_DAO();

                GroupDataAccessObject.DeleteGroup(groupBusinessEntity);

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

        /// <summary>
        /// Get Group List by account id
        /// </summary>
        /// <param name="accountID"> account id</param>
        /// <returns></returns>
        public DataTable GetdtGroupList(string accountID)
        {
            DataTable dtGroup = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Group_DAO groupDataAccessObject = new Group_DAO();
                dtGroup = groupDataAccessObject.GetdtGroupList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtGroup;
        }

        /// <summary>
        /// Get Group List count by account id
        /// </summary>
        /// <param name="accountID"> account id</param>
        /// <returns></returns>
        public int GetGroupListCount(string accountID)
        {
            int groupCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Group_DAO groupDataAccessObject = new Group_DAO();
                groupCount = groupDataAccessObject.GetGroupListCount(accountID);

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
        public List<Survey_Group_BE> GetGroup(Survey_Group_BE groupBusinessEntity)
        {
            List<Survey_Group_BE> GroupBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Group_DAO GroupDataAccessObject = new Survey_Group_DAO();

                GroupDataAccessObject.GetGroup(groupBusinessEntity);

                GroupBusinessEntityList = GroupDataAccessObject.GroupBusinessEntityList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return GroupBusinessEntityList;
        }

        /// <summary>
        /// To check if Group Exists
        /// </summary>
        /// <param name="groupBusinessEntity"></param>
        /// <returns></returns>
        public bool IsGroupExist(Survey_Group_BE groupBusinessEntity)
        {
            Survey_Group_DAO GroupDataAccessObject = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                GroupDataAccessObject = new Survey_Group_DAO();
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return GroupDataAccessObject.IsGroupExist(groupBusinessEntity);

        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an add operation
        /// </summary>
        /// <param name="groupBusinessEntity"></param>
        public int AddGroup(Survey_Group_BE groupBusinessEntity)
        {
            Survey_Group_DAO GroupDataAccessObject = null;
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                GroupDataAccessObject = new Survey_Group_DAO();

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
            return GroupDataAccessObject.AddGroup(groupBusinessEntity);
        }

        /// <summary>
        /// This method passes the Group_BE entity to Group_DAO and performs an update operation
        /// </summary>
        /// <param name="groupBusinessEntity"></param>
        public void UpdateGroup(Survey_Group_BE groupBusinessEntity)
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

                Survey_Group_DAO GroupDataAccessObject = new Survey_Group_DAO();

                GroupDataAccessObject.UpdateGroup(groupBusinessEntity);

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
        /// <param name="groupBusinessEntity"></param>
        public void DeleteGroup(Survey_Group_BE groupBusinessEntity)
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

                Survey_Group_DAO GroupDataAccessObject = new Survey_Group_DAO();

                GroupDataAccessObject.DeleteGroup(groupBusinessEntity);

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
            DataTable dataTableGroup = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Group_DAO groupDataAccessObject = new Survey_Group_DAO();
                dataTableGroup = groupDataAccessObject.GetdtGroupList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableGroup;
        }

        public int GetGroupListCount(string accountID)
        {
            int groupCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Group_DAO groupDataAccessObject = new Survey_Group_DAO();
                groupCount = groupDataAccessObject.GetGroupListCount(accountID);

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
