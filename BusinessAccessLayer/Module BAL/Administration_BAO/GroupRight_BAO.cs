/*  
* PURPOSE: This is the Business Access Object for GroupRight Entity
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

using Administration_BE;
using Administration_DAO;
using DatabaseAccessUtilities;
using System.Data.SqlClient;
using System.Data;

namespace Administration_BAO {
    public class GroupRight_BAO : Base_BAO {
        #region "Business Logic for GroupRight BAO"

        /// <summary>
        /// This Method returns the GroupRight from GroupRight_DAO by passing GroupRight_BE as a parameter
        /// </summary>
        /// <param name="p_groupRight_BE"></param>
        /// <returns></returns>
        public List<GroupRight_BE> GetGroupAcessRights(GroupRight_BE p_groupRight_BE)
        {
            List<GroupRight_BE> GroupRight_BEList = null;
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            GroupRight_DAO GroupRight_DAO = new GroupRight_DAO();
            GroupRight_DAO.GetGroupAcessRights(p_groupRight_BE);
            GroupRight_BEList = GroupRight_DAO.GroupRight_BEList;
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) {
            //    HandleException(ex);
            //}
            return GroupRight_BEList;
        }

        /// <summary>
        /// This Method returns the GroupRight from GroupRight_DAO by passing GroupRight_BE as a parameter
        /// </summary>
        /// <param name="p_groupRight_BE"></param>
        /// <returns></returns>
        public List<GroupRight_BE> GetGroupRight(GroupRight_BE p_groupRight_BE)
        {
            List<GroupRight_BE> GroupRight_BEList = null;
            //try {
                HandleWriteLog("Start", new StackTrace(true));
                GroupRight_DAO GroupRight_DAO = new GroupRight_DAO();
                GroupRight_DAO.GetGroupRight(p_groupRight_BE);
                GroupRight_BEList = GroupRight_DAO.GroupRight_BEList;
                HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) {
            //    HandleException(ex);
            //}
            return GroupRight_BEList;
        }
        public List<GroupRight_BE> GetGroupRightFeedback(GroupRight_BE p_groupRight_BE)
        {
            List<GroupRight_BE> GroupRight_BEList = null;
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            GroupRight_DAO GroupRight_DAO = new GroupRight_DAO();
            GroupRight_DAO.GetGroupRightFeedback(p_groupRight_BE);
            GroupRight_BEList = GroupRight_DAO.GroupRight_BEList;
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) {
            //    HandleException(ex);
            //}
            return GroupRight_BEList;
        }

        public DataTable get_parentid()
        {
            GroupRight_DAO GroupRight_DAO = new GroupRight_DAO();
          return  GroupRight_DAO.get_parentid();
        
        }


        /// <summary>
        /// This Method passes the GroupRight_BE entity to GroupRight_DAO and performs an add operation
        /// </summary>
        /// <param name="p_groupRight_BEList"></param>
        public void AddGroupRight(List<GroupRight_BE> p_groupRight_BEList) {

            CSqlClient sqlCient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlCient = CDataSrc.Default as CSqlClient;
                conn = sqlCient.Connection();
                dbTransaction = conn.BeginTransaction();

                    HandleWriteLog("Start", new StackTrace(true));
                    GroupRight_DAO GroupRight_DAO = new GroupRight_DAO();

                    GroupRight_DAO.AddGroupRight(p_groupRight_BEList,dbTransaction);
                    HandleWriteLog("End", new StackTrace(true));

                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex) {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        #endregion
    }






    public class Survey_GroupRight_BAO : Base_BAO
    {
        #region "Business Logic for GroupRight BAO"

        /// <summary>
        /// This Method returns the GroupRight from GroupRight_DAO by passing GroupRight_BE as a parameter
        /// </summary>
        /// <param name="p_groupRight_BE"></param>
        /// <returns></returns>
        public List<Survey_GroupRight_BE> GetGroupRight(Survey_GroupRight_BE p_groupRight_BE)
        {
            List<Survey_GroupRight_BE> GroupRight_BEList = null;
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            Survey_GroupRight_DAO GroupRight_DAO = new Survey_GroupRight_DAO();
            GroupRight_DAO.GetGroupRight(p_groupRight_BE);
            GroupRight_BEList = GroupRight_DAO.GroupRight_BEList;
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) {
            //    HandleException(ex);
            //}
            return GroupRight_BEList;
        }



      
        /// <summary>
        /// This Method passes the GroupRight_BE entity to GroupRight_DAO and performs an add operation
        /// </summary>
        /// <param name="p_groupRight_BEList"></param>
        public void AddGroupRight(List<Survey_GroupRight_BE> p_groupRight_BEList)
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
                Survey_GroupRight_DAO GroupRight_DAO = new Survey_GroupRight_DAO();

                GroupRight_DAO.AddGroupRight(p_groupRight_BEList, dbTransaction);
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
