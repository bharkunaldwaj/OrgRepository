/*
* PURPOSE: Data Access Object for GroupRight Entity
* AUTHOR: 
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

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

using Administration_BE;
using DatabaseAccessUtilities;

namespace Administration_DAO {
    public class GroupRight_DAO : DAO_Base {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public GroupRight_DAO() {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        public List<GroupRight_BE> GroupRight_BEList { get; set; }
        #endregion

        #region Private Member Variables
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to GroupRight_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAllGroupRight) {
            HandleWriteLog("Start", new StackTrace(true));
            GroupRight_BEList = new List<GroupRight_BE>();
            for (int recordCounter = 0; recordCounter < p_dtAllGroupRight.Rows.Count; recordCounter++) {
                GroupRight_BE groupRight_BE = new GroupRight_BE();

                groupRight_BE.GroupRightID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["GroupRightID"]);
                groupRight_BE.GroupID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRight_BE.MenuID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRight_BE.AccessRights = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["AccessRights"]);
                groupRight_BE.ADEVFlag = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["ADEVFlag"]);
                groupRight_BE.ParentId=GetInt(p_dtAllGroupRight.Rows[recordCounter]["ParentID"]);


                groupRight_BE.FKGroup_BE.GroupID = Convert.ToInt32(p_dtAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRight_BE.FKGroup_BE.GroupName = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["GroupName"]);
                groupRight_BE.FKGroup_BE.Description = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Description"]);
                groupRight_BE.FKGroup_BE.WelcomeText = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["WelcomeText"]);
                groupRight_BE.FKGroup_BE.NewsText = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["NewsText"]);
                groupRight_BE.FKGroup_BE.IsActive = GetBool(p_dtAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRight_BE.FKGroup_BE.CreatedDate = GetDateTime(p_dtAllGroupRight.Rows[recordCounter]["CreatedDate"]);


                groupRight_BE.FKMenuMaster_BE.MenuID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRight_BE.FKMenuMaster_BE.Name = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Name"]);
                groupRight_BE.FKMenuMaster_BE.Page = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Page"]);
                groupRight_BE.FKMenuMaster_BE.LinkedPage = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["LinkedPage"]);
                groupRight_BE.FKMenuMaster_BE.ParentID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["ParentID"]);
                groupRight_BE.FKMenuMaster_BE.Visibility = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Visibility"]);
                groupRight_BE.FKMenuMaster_BE.IsActive = GetBool(p_dtAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRight_BE.FKMenuMaster_BE.SortOrder = GetInt(p_dtAllGroupRight.Rows[recordCounter]["SortOrder"]);
                groupRight_BE.FKMenuMaster_BE.ADEVFlag = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["ADEVFlag"]);
                GroupRight_BEList.Add(groupRight_BE);
            }
            HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Function to generate where clause for searching
        /// </summary>
        /// <param name="p_GroupRight_BE"></param>
        /// <returns></returns>
        //private string GenerateWhereClause(GroupRight_BE p_GroupRight_BE) {
        //HandleWriteLog("Start", new StackTrace(true));
        //    string whereClause = "";
        //    whereClause = whereClause + " WHERE ";

        //    if (p_GroupRight_BE.GroupRightID != null) {
        //        whereClause = whereClause + " GroupRightID = " + p_GroupRight_BE.GroupRightID + " and ";
        //    }
        //    else if (p_GroupRight_BE.UserID != null) {
        //        whereClause = whereClause + " UserID = " + p_GroupRight_BE.UserID + " and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.BPNumber)) {
        //        whereClause = whereClause + " BPNumber = '" + p_GroupRight_BE.BPNumber + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.SAPBPShipTo)) {
        //        whereClause = whereClause + " SAPBPShipTo = '" + p_GroupRight_BE.SAPBPShipTo + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.Name)) {
        //        whereClause = whereClause + " Name = '" + p_GroupRight_BE.Name + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.Email)) {
        //        whereClause = whereClause + " Email = '" + p_GroupRight_BE.Email + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.Address1)) {
        //        whereClause = whereClause + " Address1 = '" + p_GroupRight_BE.Address1 + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.Address2)) {
        //        whereClause = whereClause + " Address2 = '" + p_GroupRight_BE.Address2 + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.City)) {
        //        whereClause = whereClause + " City = '" + p_GroupRight_BE.City + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.State)) {
        //        whereClause = whereClause + " State = '" + p_GroupRight_BE.State + "' and ";
        //    }
        //    else if (p_GroupRight_BE.CountryID != null) {
        //        whereClause = whereClause + " CountryID = " + p_GroupRight_BE.CountryID.ToString() + " and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.Zip)) {
        //        whereClause = whereClause + " Zip = '" + p_GroupRight_BE.Zip + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.TelNumber)) {
        //        whereClause = whereClause + " TelNumber = '" + p_GroupRight_BE.TelNumber + "' and ";
        //    }
        //    else if (!string.IsNullOrEmpty(p_GroupRight_BE.FaxNumber)) {
        //        whereClause = whereClause + " FaxNumber = '" + p_GroupRight_BE.FaxNumber + "' and ";
        //    }
        //    else if (p_GroupRight_BE.IsActive != null) {
        //        whereClause = whereClause + " IsActive = " + Convert.ToInt32(p_GroupRight_BE.IsActive).ToString() + " and ";
        //    }
        //    else if (p_GroupRight_BE.GroupRightTypeID != null) {
        //        whereClause = whereClause + " GroupRightTypeID = " + p_GroupRight_BE.GroupRightTypeID + " and ";
        //    }
        //    else if (p_GroupRight_BE.IsDefault != null) {
        //        whereClause = whereClause + " IsDefault = " + Convert.ToInt32(p_GroupRight_BE.IsDefault).ToString() + " and ";
        //    }

        //    if (whereClause.Length > 6)
        //        whereClause = whereClause.Substring(0, whereClause.Length - 4);
        //    else
        //        whereClause = string.Empty;
        //HandleWriteLogDAU(whereClause, new StackTrace(true));
        //HandleWriteLog("End", new StackTrace(true));
        //    return whereClause.ToString();
        //}

        #endregion

        #region "Query GroupRight"

        /// <summary>
        /// Function to Get GroupRights
        /// </summary>
        /// <param name="p_contact_BE"></param>
        /// <returns></returns>
        /// 


        public DataTable get_parentid()
        {
            DataTable dtAllGroupRight=null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlDeleteCommand = string.Empty;
                dtAllGroupRight = cDataSrc.ExecuteDataSet("Usp_get_ParentID", null).Tables[0];
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtAllGroupRight;


        }


        public int GetGroupAcessRights(GroupRight_BE p_GroupRightBE)
        {
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            DataTable dtAllGroupRight = new DataTable();
            object[] param = new object[1] { p_GroupRightBE.GroupID};
            dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupAcessRights", param, null).Tables[0];

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

            //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

            ShiftDataTableToBEList(dtAllGroupRight);
            m_returnValue = 1;
            HandleWriteLogDAU("UspGetGroupAcessRights ", param, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }


        public int GetGroupRight(GroupRight_BE p_GroupRightBE) {
            //try {
                HandleWriteLog("Start", new StackTrace(true));               
                DataTable dtAllGroupRight = new DataTable();                                
                object[] param = new object[1] { p_GroupRightBE.GroupID};
                dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", param, null).Tables[0];

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

                //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

                ShiftDataTableToBEList(dtAllGroupRight);
                m_returnValue = 1;
                HandleWriteLogDAU("UspGetGroupRights ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }
        public int GetGroupRightFeedback(GroupRight_BE p_GroupRightBE)
        {
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            DataTable dtAllGroupRight = new DataTable();
            object[] param = new object[1] { p_GroupRightBE.GroupID };
            dtAllGroupRight = cDataSrc.ExecuteDataSet("Feedback_UspGetGroupRights", param, null).Tables[0];

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

            //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

            ShiftDataTableToBEList(dtAllGroupRight);
            m_returnValue = 1;
            HandleWriteLogDAU("UspGetGroupRights ", param, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }
        #endregion



       




        #region Public Methods

        /// <summary>
        /// Function to insert records in GroupRights Entity to add group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void AddGroupRight(GroupRight_BE p_GroupRightBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlInsertCommand = string.Empty;
                sqlInsertCommand = "INSERT INTO GroupRight (GroupRights.MenuID, GroupRights.AccessRights) VALUES (";
                sqlInsertCommand = sqlInsertCommand + p_GroupRightBE.MenuID.ToString() + ", '" + p_GroupRightBE.AccessRights + "')";
                cDataSrc.ExecuteNonQuery(sqlInsertCommand);
                cDataSrc = null;              
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to insert records in GroupRights Entity to add group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void AddGroupRight(List<GroupRight_BE> p_GroupRightBEList, IDbTransaction transaction)
        {
            try {
                HandleWriteLog("Start", new StackTrace(true));                
                int groupID = 0;
                string menuIDs = string.Empty;
                string accessRights = string.Empty;
                foreach (GroupRight_BE GroupRight_BE in p_GroupRightBEList) {
                    groupID = Convert.ToInt32(GroupRight_BE.GroupID);
                    menuIDs += Convert.ToString(GroupRight_BE.MenuID) + ",";
                    accessRights += GroupRight_BE.AccessRights != null && GroupRight_BE.AccessRights != string.Empty ? Convert.ToString(GroupRight_BE.AccessRights) + "," : "-,";
                    //accessRights += Convert.ToString(GroupRight_BE.AccessRights)+",";
                }
                //Trim , from the end of the texts
                menuIDs = menuIDs.TrimEnd(',');
               
                accessRights = accessRights.TrimEnd(',');

                //to set parameters
                object[] param = new object[3] { groupID, menuIDs, accessRights };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", groupID);
                //cNameValueList.Add("@chvMenuID", menuIDs);
                //cNameValueList.Add("@chvAccessRights", accessRights);

                int iRes = cDataSrc.ExecuteNonQuery("UspGroupRights", param, transaction);

                //int iRes = cDataSrc.ExecuteNonQuery("UspGroupRights", cNameValueList, transaction);

                cDataSrc = null;
                HandleWriteLogDAU("UspGetGroupRights ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to update records in GroupRights Entity to update group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void UpdateGroupRight(GroupRight_BE p_GroupRightBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlUpdateCommand = string.Empty;
                sqlUpdateCommand = "UPDATE GroupRight SET MenuID = " + p_GroupRightBE.MenuID.ToString() + " , AccessRights = '" + p_GroupRightBE.AccessRights + "'";
                //sqlUpdateCommand = sqlUpdateCommand + GenerateWhereClause(p_GroupRight_BE);
                cDataSrc.ExecuteNonQuery(sqlUpdateCommand);
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to delete records in GroupRights Entity to remove group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void DeleteGroupRight(GroupRight_BE p_GroupRightBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlDeleteCommand = string.Empty;
                sqlDeleteCommand = "DELETE FROM GroupRight WHERE GroupRightID = " + p_GroupRightBE.GroupRightID.ToString();
                cDataSrc.ExecuteNonQuery(sqlDeleteCommand);
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }
        #endregion
    }


























    public class Survey_GroupRight_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_GroupRight_DAO()
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        public List<Survey_GroupRight_BE> GroupRight_BEList { get; set; }
        #endregion

        #region Private Member Variables
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to GroupRight_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAllGroupRight)
        {
            HandleWriteLog("Start", new StackTrace(true));
            GroupRight_BEList = new List<Survey_GroupRight_BE>();
            for (int recordCounter = 0; recordCounter < p_dtAllGroupRight.Rows.Count; recordCounter++)
            {
                Survey_GroupRight_BE groupRight_BE = new Survey_GroupRight_BE();

                groupRight_BE.GroupRightID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["GroupRightID"]);
                groupRight_BE.GroupID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRight_BE.MenuID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRight_BE.AccessRights = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["AccessRights"]);


                groupRight_BE.FKGroup_BE.GroupID = Convert.ToInt32(p_dtAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRight_BE.FKGroup_BE.GroupName = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["GroupName"]);
                groupRight_BE.FKGroup_BE.Description = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Description"]);
                groupRight_BE.FKGroup_BE.WelcomeText = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["WelcomeText"]);
                groupRight_BE.FKGroup_BE.NewsText = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["NewsText"]);
                groupRight_BE.FKGroup_BE.IsActive = GetBool(p_dtAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRight_BE.FKGroup_BE.CreatedDate = GetDateTime(p_dtAllGroupRight.Rows[recordCounter]["CreatedDate"]);
                groupRight_BE.FKGroup_BE.ModifiedDate = GetDateTime(p_dtAllGroupRight.Rows[recordCounter]["ModifiedDate"]);
                
                groupRight_BE.FKMenuMaster_BE.MenuID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRight_BE.FKMenuMaster_BE.Name = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Name"]);
                groupRight_BE.FKMenuMaster_BE.Page = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Page"]);
                groupRight_BE.FKMenuMaster_BE.LinkedPage = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["LinkedPage"]);
                groupRight_BE.FKMenuMaster_BE.ParentID = GetInt(p_dtAllGroupRight.Rows[recordCounter]["ParentID"]);
                groupRight_BE.FKMenuMaster_BE.Visibility = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["Visibility"]);
                groupRight_BE.FKMenuMaster_BE.IsActive = GetBool(p_dtAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRight_BE.FKMenuMaster_BE.SortOrder = GetInt(p_dtAllGroupRight.Rows[recordCounter]["SortOrder"]);
                groupRight_BE.FKMenuMaster_BE.ADEVFlag = Convert.ToString(p_dtAllGroupRight.Rows[recordCounter]["ADEVFlag"]);
                GroupRight_BEList.Add(groupRight_BE);
            }
            HandleWriteLog("End", new StackTrace(true));
        }

       

        #endregion

        #region "Query GroupRight"

        /// <summary>
        /// Function to Get GroupRights
        /// </summary>
        /// <param name="p_contact_BE"></param>
        /// <returns></returns>
        public int GetGroupRight(Survey_GroupRight_BE p_GroupRightBE)
        {
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            DataTable dtAllGroupRight = new DataTable();
            object[] param = new object[1] { p_GroupRightBE.GroupID };
            dtAllGroupRight = cDataSrc.ExecuteDataSet("Survey_UspGetGroupRights", param, null).Tables[0];

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

            //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

            ShiftDataTableToBEList(dtAllGroupRight);
            m_returnValue = 1;
            HandleWriteLogDAU("Survey_UspGetGroupRights ", param, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }
        #endregion



       


        #region Public Methods

        /// <summary>
        /// Function to insert records in GroupRights Entity to add group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void AddGroupRight(Survey_GroupRight_BE p_GroupRightBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlInsertCommand = string.Empty;
                sqlInsertCommand = "INSERT INTO Survey_GroupRights (Survey_GroupRights.MenuID, Survey_GroupRights.AccessRights) VALUES (";
                sqlInsertCommand = sqlInsertCommand + p_GroupRightBE.MenuID.ToString() + ", '" + p_GroupRightBE.AccessRights + "')";
                cDataSrc.ExecuteNonQuery(sqlInsertCommand);
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to insert records in GroupRights Entity to add group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void AddGroupRight(List<Survey_GroupRight_BE> p_GroupRightBEList, IDbTransaction transaction)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                int groupID = 0;
                string menuIDs = string.Empty;
                string accessRights = string.Empty;
                foreach (Survey_GroupRight_BE GroupRight_BE in p_GroupRightBEList)
                {
                    groupID = Convert.ToInt32(GroupRight_BE.GroupID);
                    menuIDs += Convert.ToString(GroupRight_BE.MenuID) + ",";
                    accessRights += GroupRight_BE.AccessRights != null && GroupRight_BE.AccessRights != string.Empty ? Convert.ToString(GroupRight_BE.AccessRights) + "," : "-,";
                    //accessRights += Convert.ToString(GroupRight_BE.AccessRights)+",";
                }
                //Trim , from the end of the texts
                menuIDs = menuIDs.TrimEnd(',');
                accessRights = accessRights.TrimEnd(',');

                //to set parameters
                object[] param = new object[3] { groupID, menuIDs, accessRights };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", groupID);
                //cNameValueList.Add("@chvMenuID", menuIDs);
                //cNameValueList.Add("@chvAccessRights", accessRights);

                int iRes = cDataSrc.ExecuteNonQuery("Survey_UspGroupRights", param, transaction);

                //int iRes = cDataSrc.ExecuteNonQuery("UspGroupRights", cNameValueList, transaction);

                cDataSrc = null;
                HandleWriteLogDAU("Survey_UspGetGroupRights ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to update records in GroupRights Entity to update group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void UpdateGroupRight(Survey_GroupRight_BE p_GroupRightBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlUpdateCommand = string.Empty;
                sqlUpdateCommand = "UPDATE Survey_GroupRights SET MenuID = " + p_GroupRightBE.MenuID.ToString() + " , AccessRights = '" + p_GroupRightBE.AccessRights + "'";
                //sqlUpdateCommand = sqlUpdateCommand + GenerateWhereClause(p_GroupRight_BE);
                cDataSrc.ExecuteNonQuery(sqlUpdateCommand);
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to delete records in GroupRights Entity to remove group rights
        /// </summary>
        /// <param name="p_GroupRight_BEList"></param>
        public void DeleteGroupRight(Survey_GroupRight_BE p_GroupRightBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlDeleteCommand = string.Empty;
                sqlDeleteCommand = "DELETE FROM Survey_GroupRights WHERE GroupRightID = " + p_GroupRightBE.GroupRightID.ToString();
                cDataSrc.ExecuteNonQuery(sqlDeleteCommand);
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }
        #endregion
    }














}
