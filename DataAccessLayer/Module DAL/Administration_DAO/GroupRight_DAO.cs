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
using System.Collections.Generic;
using System.Diagnostics;
using feedbackFramework_DAO;

using Administration_BE;
using DatabaseAccessUtilities;

namespace Administration_DAO
{
    public class GroupRight_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public GroupRight_DAO()
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        public List<GroupRight_BE> GroupRightBusinessEntityList { get; set; }
        #endregion

        #region Private Member Variables
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to GroupRight_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllGroupRight)
        {
            HandleWriteLog("Start", new StackTrace(true));
            GroupRightBusinessEntityList = new List<GroupRight_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAllGroupRight.Rows.Count; recordCounter++)
            {
                GroupRight_BE groupRightBusinessEntity = new GroupRight_BE();

                groupRightBusinessEntity.GroupRightID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["GroupRightID"]);
                groupRightBusinessEntity.GroupID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRightBusinessEntity.MenuID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRightBusinessEntity.AccessRights = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["AccessRights"]);
                groupRightBusinessEntity.ADEVFlag = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["ADEVFlag"]);
                groupRightBusinessEntity.ParentId = GetInt(dataTableAllGroupRight.Rows[recordCounter]["ParentID"]);


                groupRightBusinessEntity.FKGroup_BE.GroupID = Convert.ToInt32(dataTableAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRightBusinessEntity.FKGroup_BE.GroupName = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["GroupName"]);
                groupRightBusinessEntity.FKGroup_BE.Description = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Description"]);
                groupRightBusinessEntity.FKGroup_BE.WelcomeText = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["WelcomeText"]);
                groupRightBusinessEntity.FKGroup_BE.NewsText = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["NewsText"]);
                groupRightBusinessEntity.FKGroup_BE.IsActive = GetBool(dataTableAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRightBusinessEntity.FKGroup_BE.CreatedDate = GetDateTime(dataTableAllGroupRight.Rows[recordCounter]["CreatedDate"]);


                groupRightBusinessEntity.FKMenuMaster_BE.MenuID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRightBusinessEntity.FKMenuMaster_BE.Name = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Name"]);
                groupRightBusinessEntity.FKMenuMaster_BE.Page = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Page"]);
                groupRightBusinessEntity.FKMenuMaster_BE.LinkedPage = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["LinkedPage"]);
                groupRightBusinessEntity.FKMenuMaster_BE.ParentID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["ParentID"]);
                groupRightBusinessEntity.FKMenuMaster_BE.Visibility = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Visibility"]);
                groupRightBusinessEntity.FKMenuMaster_BE.IsActive = GetBool(dataTableAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRightBusinessEntity.FKMenuMaster_BE.SortOrder = GetInt(dataTableAllGroupRight.Rows[recordCounter]["SortOrder"]);
                groupRightBusinessEntity.FKMenuMaster_BE.ADEVFlag = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["ADEVFlag"]);
                GroupRightBusinessEntityList.Add(groupRightBusinessEntity);
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
            DataTable dataTableAllGroupRight = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlDeleteCommand = string.Empty;
                dataTableAllGroupRight = cDataSrc.ExecuteDataSet("Usp_get_ParentID", null).Tables[0];
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dataTableAllGroupRight;


        }

        /// <summary>
        /// Get Group Acess Rights
        /// </summary>
        /// <param name="groupRightBusinessEntity"></param>
        /// <returns></returns>
        public int GetGroupAcessRights(GroupRight_BE groupRightBusinessEntity)
        {
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            DataTable dataTableAllGroupRight = new DataTable();
            object[] param = new object[1] { groupRightBusinessEntity.GroupID };
            dataTableAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupAcessRights", param, null).Tables[0];

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

            //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

            ShiftDataTableToBEList(dataTableAllGroupRight);
            m_returnValue = 1;
            HandleWriteLogDAU("UspGetGroupAcessRights ", param, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }

        /// <summary>
        /// Get Group Right
        /// </summary>
        /// <param name="groupRightBusinessEntity"></param>
        /// <returns></returns>
        public int GetGroupRight(GroupRight_BE groupRightBusinessEntity)
        {
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            DataTable dataTableAllGroupRight = new DataTable();
            object[] param = new object[1] { groupRightBusinessEntity.GroupID };
            dataTableAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", param, null).Tables[0];

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

            //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

            ShiftDataTableToBEList(dataTableAllGroupRight);
            m_returnValue = 1;
            HandleWriteLogDAU("UspGetGroupRights ", param, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }

        /// <summary>
        /// Get Group Right Feedback
        /// </summary>
        /// <param name="groupRightBusinessEntity"></param>
        /// <returns></returns>
        public int GetGroupRightFeedback(GroupRight_BE groupRightBusinessEntity)
        {
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            DataTable dataTableAllGroupRight = new DataTable();
            object[] param = new object[1] { groupRightBusinessEntity.GroupID };
            dataTableAllGroupRight = cDataSrc.ExecuteDataSet("Feedback_UspGetGroupRights", param, null).Tables[0];

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

            //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

            ShiftDataTableToBEList(dataTableAllGroupRight);
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
        public void AddGroupRight(GroupRight_BE groupRightBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlInsertCommand = string.Empty;
                sqlInsertCommand = "INSERT INTO GroupRight (GroupRights.MenuID, GroupRights.AccessRights) VALUES (";
                sqlInsertCommand = sqlInsertCommand + groupRightBusinessEntity.MenuID.ToString() + ", '" + groupRightBusinessEntity.AccessRights + "')";
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
        public void AddGroupRight(List<GroupRight_BE> groupRightBusinessEntityList, IDbTransaction transaction)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                int groupID = 0;
                string menuIDs = string.Empty;
                string accessRights = string.Empty;
                foreach (GroupRight_BE GroupRight_BE in groupRightBusinessEntityList)
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
        public void UpdateGroupRight(GroupRight_BE groupRightBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlUpdateCommand = string.Empty;
                sqlUpdateCommand = "UPDATE GroupRight SET MenuID = " + groupRightBusinessEntity.MenuID.ToString() + " , AccessRights = '" + groupRightBusinessEntity.AccessRights + "'";
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
        public void DeleteGroupRight(GroupRight_BE groupRightBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlDeleteCommand = string.Empty;
                sqlDeleteCommand = "DELETE FROM GroupRight WHERE GroupRightID = " + groupRightBusinessEntity.GroupRightID.ToString();
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
        public List<Survey_GroupRight_BE> GroupRightBusinessEntityList { get; set; }
        #endregion

        #region Private Member Variables
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to GroupRight_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllGroupRight)
        {
            HandleWriteLog("Start", new StackTrace(true));
            GroupRightBusinessEntityList = new List<Survey_GroupRight_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAllGroupRight.Rows.Count; recordCounter++)
            {
                Survey_GroupRight_BE groupRightBusinessEntity = new Survey_GroupRight_BE();

                groupRightBusinessEntity.GroupRightID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["GroupRightID"]);
                groupRightBusinessEntity.GroupID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRightBusinessEntity.MenuID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRightBusinessEntity.AccessRights = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["AccessRights"]);


                groupRightBusinessEntity.FKGroup_BE.GroupID = Convert.ToInt32(dataTableAllGroupRight.Rows[recordCounter]["GroupID"]);
                groupRightBusinessEntity.FKGroup_BE.GroupName = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["GroupName"]);
                groupRightBusinessEntity.FKGroup_BE.Description = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Description"]);
                groupRightBusinessEntity.FKGroup_BE.WelcomeText = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["WelcomeText"]);
                groupRightBusinessEntity.FKGroup_BE.NewsText = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["NewsText"]);
                groupRightBusinessEntity.FKGroup_BE.IsActive = GetBool(dataTableAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRightBusinessEntity.FKGroup_BE.CreatedDate = GetDateTime(dataTableAllGroupRight.Rows[recordCounter]["CreatedDate"]);
                groupRightBusinessEntity.FKGroup_BE.ModifiedDate = GetDateTime(dataTableAllGroupRight.Rows[recordCounter]["ModifiedDate"]);

                groupRightBusinessEntity.FKMenuMaster_BE.MenuID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["MenuID"]);
                groupRightBusinessEntity.FKMenuMaster_BE.Name = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Name"]);
                groupRightBusinessEntity.FKMenuMaster_BE.Page = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Page"]);
                groupRightBusinessEntity.FKMenuMaster_BE.LinkedPage = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["LinkedPage"]);
                groupRightBusinessEntity.FKMenuMaster_BE.ParentID = GetInt(dataTableAllGroupRight.Rows[recordCounter]["ParentID"]);
                groupRightBusinessEntity.FKMenuMaster_BE.Visibility = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["Visibility"]);
                groupRightBusinessEntity.FKMenuMaster_BE.IsActive = GetBool(dataTableAllGroupRight.Rows[recordCounter]["IsActive"]);
                groupRightBusinessEntity.FKMenuMaster_BE.SortOrder = GetInt(dataTableAllGroupRight.Rows[recordCounter]["SortOrder"]);
                groupRightBusinessEntity.FKMenuMaster_BE.ADEVFlag = Convert.ToString(dataTableAllGroupRight.Rows[recordCounter]["ADEVFlag"]);
                GroupRightBusinessEntityList.Add(groupRightBusinessEntity);
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
        public int GetGroupRight(Survey_GroupRight_BE groupRightBusinessEntity)
        {
            //try {
            HandleWriteLog("Start", new StackTrace(true));
            DataTable dataTableAllGroupRight = new DataTable();
            object[] param = new object[1] { groupRightBusinessEntity.GroupID };
            dataTableAllGroupRight = cDataSrc.ExecuteDataSet("Survey_UspGetGroupRights", param, null).Tables[0];

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intGroupID", p_GroupRightBE.GroupID);

            //dtAllGroupRight = cDataSrc.ExecuteDataSet("UspGetGroupRights", cNameValueList, null).Tables[0];

            ShiftDataTableToBEList(dataTableAllGroupRight);
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
        public void AddGroupRight(Survey_GroupRight_BE groupRightBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlInsertCommand = string.Empty;
                sqlInsertCommand = "INSERT INTO Survey_GroupRights (Survey_GroupRights.MenuID, Survey_GroupRights.AccessRights) VALUES (";
                sqlInsertCommand = sqlInsertCommand + groupRightBusinessEntity.MenuID.ToString() + ", '" + groupRightBusinessEntity.AccessRights + "')";
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
        public void AddGroupRight(List<Survey_GroupRight_BE> groupRightBusinessEntityList, IDbTransaction transaction)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                int groupID = 0;
                string menuIDs = string.Empty;
                string accessRights = string.Empty;
                foreach (Survey_GroupRight_BE groupRightBusinessEntity in groupRightBusinessEntityList)
                {
                    groupID = Convert.ToInt32(groupRightBusinessEntity.GroupID);
                    menuIDs += Convert.ToString(groupRightBusinessEntity.MenuID) + ",";
                    accessRights += groupRightBusinessEntity.AccessRights != null && groupRightBusinessEntity.AccessRights != string.Empty ? Convert.ToString(groupRightBusinessEntity.AccessRights) + "," : "-,";
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

                int result = cDataSrc.ExecuteNonQuery("Survey_UspGroupRights", param, transaction);

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
        public void UpdateGroupRight(Survey_GroupRight_BE groupRightBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlUpdateCommand = string.Empty;
                sqlUpdateCommand = "UPDATE Survey_GroupRights SET MenuID = " + groupRightBusinessEntity.MenuID.ToString() + " , AccessRights = '" + groupRightBusinessEntity.AccessRights + "'";
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
        public void DeleteGroupRight(Survey_GroupRight_BE groupRightBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlDeleteCommand = string.Empty;
                sqlDeleteCommand = "DELETE FROM Survey_GroupRights WHERE GroupRightID = " + groupRightBusinessEntity.GroupRightID.ToString();
                cDataSrc.ExecuteNonQuery(sqlDeleteCommand);
                cDataSrc = null;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }
        #endregion
    }
}
