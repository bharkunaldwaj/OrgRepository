/*
* PURPOSE:  Data Access Object for Group Entity
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
    public class Group_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Constructor"
        /// <summary>
        /// Public Constructor
        /// </summary>
        public Group_DAO()
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        public List<Group_BE> _groupBusinessEntityList { get; set; }
        #endregion

        #region Private Member Variables
        private int returnValue;
        private int result;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to GroupBE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllGroup)
        {
            HandleWriteLog("Start", new StackTrace(true));
            _groupBusinessEntityList = new List<Group_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAllGroup.Rows.Count; recordCounter++)
            {
                Group_BE groupBusinessEntity = new Group_BE();

                groupBusinessEntity.GroupID = GetInt(dataTableAllGroup.Rows[recordCounter]["GroupID"]);
                groupBusinessEntity.GroupName = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["GroupName"]);
                groupBusinessEntity.Description = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["Description"]);
                groupBusinessEntity.WelcomeText = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["WelcomeText"]);
                groupBusinessEntity.NewsText = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["NewsText"]);
                groupBusinessEntity.IsActive = GetBool(dataTableAllGroup.Rows[recordCounter]["IsActive"]);
                groupBusinessEntity.CreatedDate = GetDateTime(dataTableAllGroup.Rows[recordCounter]["CreatedDate"]);
                groupBusinessEntity.ModifiedDate = GetDateTime(dataTableAllGroup.Rows[recordCounter]["ModifiedDate"]);
                groupBusinessEntity.Description = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["Description"]);
                _groupBusinessEntityList.Add(groupBusinessEntity);
            }
            HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Query Group"
        /// <summary>
        /// Function to Get Group Details details
        /// </summary>
        /// <param name="p_contact_BE"></param>
        /// <returns></returns>
        public int GetGroup(Group_BE groupBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllGroup = new DataTable();

                object[] param = new object[3] {groupBusinessEntity.GroupID,
                                                GetString(groupBusinessEntity.GroupName),
                                                "ALL" };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_GroupBE.GroupID);
                //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName.ToString());
                //cNameValueList.Add("@chvFlag", "ALL");


                dataTableAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", param, null).Tables[0];

                //dtAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", cNameValueList, null).Tables[0];                              

                ShiftDataTableToBEList(dataTableAllGroup);
                returnValue = 1;

                HandleWriteLog("End", new StackTrace(true));
                HandleWriteLogDAU("UspGetGroup ", param, new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Function to check existence of Group
        /// </summary>
        /// <param name="groupBusinessEntity"></param>
        /// <returns></returns>
        public bool IsGroupExist(Group_BE groupBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllGroup = new DataTable();

                object[] param = new object[3] {groupBusinessEntity.GroupID,
                                                GetString(groupBusinessEntity.GroupName) ,
                                                "COUNT" };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_Group_BE.GroupID);
                //cNameValueList.Add("@chvGroupName", p_Group_BE.GroupName.ToString());
                //cNameValueList.Add("@chvFlag", "COUNT");


                result = Convert.ToInt32(cDataSrc.ExecuteScalar("UspGetGroup", param, null));

                //result = Convert.ToInt32(cDataSrc.ExecuteScalar("UspGetGroup", cNameValueList, null));

                HandleWriteLog("End", new StackTrace(true));
                HandleWriteLogDAU("UspGetGroup ", param, new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return result == 0 ? false : true;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Insert records in Group Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public int AddGroup(Group_BE groupBusinessEntity)
        {
            //int IsActive=0;
            //IsActive = p_Group_BE.IsActive == true ? 1 : 0;
            string GroupID = string.Empty;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[7] {null,
                    GetString(groupBusinessEntity.GroupName) ,
                    GetString(groupBusinessEntity.Description),
                    GetString(groupBusinessEntity.WelcomeText),
                    GetString(groupBusinessEntity.NewsText),
                    groupBusinessEntity.IsActive,
                    "I"
                };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName);
                //cNameValueList.Add("@chvnDescription", p_GroupBE.Description);
                //cNameValueList.Add("@chvnWelcomeText", p_GroupBE.WelcomeText);
                //cNameValueList.Add("@chvnNewsText", p_GroupBE.NewsText);
                //cNameValueList.Add("@bitIsActive", p_GroupBE.IsActive);
                //cNameValueList.Add("@chvMode",'I');

                GroupID = Convert.ToString(cDataSrc.ExecuteScalar("UspGroupMaintenance", param, null));

                //GroupID = Convert.ToString(cDataSrc.ExecuteScalar("UspGroupMaintenance", cNameValueList, null));                   

                cDataSrc = null;

                HandleWriteLogDAU("UspGroupMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return (Convert.ToInt32(GroupID));
        }

        /// <summary>
        /// Function to Update records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void UpdateGroup(Group_BE groupBusinessEntity)
        {
            HandleWriteLog("Start", new StackTrace(true));
            object[] param = new object[7] { groupBusinessEntity.GroupID,
                                GetString(groupBusinessEntity.GroupName),
                                GetString(groupBusinessEntity.Description),
                                GetString(groupBusinessEntity.WelcomeText),
                                GetString(groupBusinessEntity.NewsText),
                                groupBusinessEntity.IsActive,
                                'U'
                };

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName);
            //cNameValueList.Add("@chvnDescription", p_GroupBE.Description);
            //cNameValueList.Add("@chvnWelcomeText", p_GroupBE.WelcomeText);
            //cNameValueList.Add("@chvnNewsText", p_GroupBE.NewsText);
            //cNameValueList.Add("@bitIsActive", p_GroupBE.IsActive);
            //cNameValueList.Add("@chvMode", 'U');

            cDataSrc.ExecuteNonQuery("UspGroupMaintenance", param, null);

            //cDataSrc.ExecuteNonQuery("UspGroupMaintenance", cNameValueList, null);    

            cDataSrc = null;
            HandleWriteLogDAU("UspGroupMaintenance ", param, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Function to Delete records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void DeleteGroup(Group_BE groupBusinessEntity)
        {
            HandleWriteLog("Start", new StackTrace(true));
            object[] param = new object[7] { groupBusinessEntity.GroupID,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                            "D" };

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intID", p_GroupBE.GroupID.ToString());
            //cNameValueList.Add("@chvMode", 'D');

            cDataSrc.ExecuteNonQuery("UspGroupMaintenance", param, null);

            //cDataSrc.ExecuteNonQuery("UspGroupMaintenance", cNameValueList, null);            

            cDataSrc = null;
            //HandleWriteLogDAU("UspGroupMaintenance ", cNameValueList, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        /// <summary>
        /// Get Group List Count by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetGroupListCount(string accountID)
        {
            int groupCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, null, "C" };

                groupCount = (int)cDataSrc.ExecuteScalar("UspGroupSelect", param, null);

                //HandleWriteLogDAU("UspGroupSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return groupCount;
        }

        /// <summary>
        /// Get datatable Group List by account ID
        /// </summary>
        /// <param name="accountID"> account ID</param>
        /// <returns></returns>
        public DataTable GetdtGroupList(string accountID)
        {
            DataTable dataTableAllGroup = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, null, "A" };

                dataTableAllGroup = cDataSrc.ExecuteDataSet("UspGroupSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspGroupSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllGroup;
        }
    }

    public class Survey_Group_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Constructor"
        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_Group_DAO()
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        public List<Survey_Group_BE> GroupBusinessEntityList { get; set; }
        #endregion

        #region Private Member Variables
        private int returnValue;
        private int result;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to GroupBE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllGroup)
        {
            HandleWriteLog("Start", new StackTrace(true));
            GroupBusinessEntityList = new List<Survey_Group_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAllGroup.Rows.Count; recordCounter++)
            {
                Survey_Group_BE groupBusinessEntity = new Survey_Group_BE();

                groupBusinessEntity.GroupID = GetInt(dataTableAllGroup.Rows[recordCounter]["GroupID"]);
                groupBusinessEntity.GroupName = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["GroupName"]);
                groupBusinessEntity.Description = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["Description"]);
                groupBusinessEntity.WelcomeText = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["WelcomeText"]);
                groupBusinessEntity.NewsText = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["NewsText"]);
                groupBusinessEntity.IsActive = GetBool(dataTableAllGroup.Rows[recordCounter]["IsActive"]);
                groupBusinessEntity.CreatedDate = GetDateTime(dataTableAllGroup.Rows[recordCounter]["CreatedDate"]);
                groupBusinessEntity.ModifiedDate = GetDateTime(dataTableAllGroup.Rows[recordCounter]["ModifiedDate"]);
                groupBusinessEntity.Description = Convert.ToString(dataTableAllGroup.Rows[recordCounter]["Description"]);
                GroupBusinessEntityList.Add(groupBusinessEntity);
            }
            HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Query Group"
        /// <summary>
        /// Function to Get Group Details details
        /// </summary>
        /// <param name="p_contact_BE"></param>
        /// <returns></returns>
        public int GetGroup(Survey_Group_BE groupBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllGroup = new DataTable();

                object[] param = new object[3] {groupBusinessEntity.GroupID,
                                                GetString(groupBusinessEntity.GroupName),
                                                "ALL" };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_GroupBE.GroupID);
                //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName.ToString());
                //cNameValueList.Add("@chvFlag", "ALL");


                dataTableAllGroup = cDataSrc.ExecuteDataSet("Survey_UspGetGroup", param, null).Tables[0];

                //dtAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", cNameValueList, null).Tables[0];                              

                ShiftDataTableToBEList(dataTableAllGroup);
                returnValue = 1;

                HandleWriteLog("End", new StackTrace(true));
                HandleWriteLogDAU("Survey_UspGetGroup ", param, new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Function to check existence of Group
        /// </summary>
        /// <param name="groupBusinessEntity"></param>
        /// <returns></returns>
        public bool IsGroupExist(Survey_Group_BE groupBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllGroup = new DataTable();

                object[] param = new object[3] {groupBusinessEntity.GroupID,
                                                GetString(groupBusinessEntity.GroupName) ,
                                                "COUNT" };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_Group_BE.GroupID);
                //cNameValueList.Add("@chvGroupName", p_Group_BE.GroupName.ToString());
                //cNameValueList.Add("@chvFlag", "COUNT");


                result = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspGetGroup", param, null));

                //result = Convert.ToInt32(cDataSrc.ExecuteScalar("UspGetGroup", cNameValueList, null));

                HandleWriteLog("End", new StackTrace(true));
                HandleWriteLogDAU("Survey_UspGetGroup ", param, new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return result == 0 ? false : true;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Insert records in Group Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public int AddGroup(Survey_Group_BE groupBusinessEntity)
        {
            //int IsActive=0;
            //IsActive = p_Group_BE.IsActive == true ? 1 : 0;
            string GroupID = string.Empty;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[7] {null,
                    GetString(groupBusinessEntity.GroupName) ,
                    GetString(groupBusinessEntity.Description),
                    GetString(groupBusinessEntity.WelcomeText),
                    GetString(groupBusinessEntity.NewsText),
                    groupBusinessEntity.IsActive,
                    "I"
                };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName);
                //cNameValueList.Add("@chvnDescription", p_GroupBE.Description);
                //cNameValueList.Add("@chvnWelcomeText", p_GroupBE.WelcomeText);
                //cNameValueList.Add("@chvnNewsText", p_GroupBE.NewsText);
                //cNameValueList.Add("@bitIsActive", p_GroupBE.IsActive);
                //cNameValueList.Add("@chvMode",'I');

                GroupID = Convert.ToString(cDataSrc.ExecuteScalar("Survey_UspGroupMaintenance", param, null));

                //GroupID = Convert.ToString(cDataSrc.ExecuteScalar("UspGroupMaintenance", cNameValueList, null));                   

                cDataSrc = null;

                HandleWriteLogDAU("Survey_UspGroupMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return (Convert.ToInt32(GroupID));
        }

        /// <summary>
        /// Function to Update records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void UpdateGroup(Survey_Group_BE groupBusinessEntity)
        {
            HandleWriteLog("Start", new StackTrace(true));
            object[] param = new object[7] { groupBusinessEntity.GroupID,
                                GetString(groupBusinessEntity.GroupName),
                                GetString(groupBusinessEntity.Description),
                                GetString(groupBusinessEntity.WelcomeText),
                                GetString(groupBusinessEntity.NewsText),
                                groupBusinessEntity.IsActive,
                                'U'
                };

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName);
            //cNameValueList.Add("@chvnDescription", p_GroupBE.Description);
            //cNameValueList.Add("@chvnWelcomeText", p_GroupBE.WelcomeText);
            //cNameValueList.Add("@chvnNewsText", p_GroupBE.NewsText);
            //cNameValueList.Add("@bitIsActive", p_GroupBE.IsActive);
            //cNameValueList.Add("@chvMode", 'U');

            cDataSrc.ExecuteNonQuery("Survey_UspGroupMaintenance", param, null);

            //cDataSrc.ExecuteNonQuery("UspGroupMaintenance", cNameValueList, null);    

            cDataSrc = null;
            HandleWriteLogDAU("Survey_UspGroupMaintenance ", param, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Function to Delete records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void DeleteGroup(Survey_Group_BE groupBusinessEntity)
        {
            HandleWriteLog("Start", new StackTrace(true));
            object[] param = new object[7] { groupBusinessEntity.GroupID,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                            "D" };

            //CNameValueList cNameValueList = null;
            //cNameValueList = new CNameValueList();
            //cNameValueList.Add("@intID", p_GroupBE.GroupID.ToString());
            //cNameValueList.Add("@chvMode", 'D');

            cDataSrc.ExecuteNonQuery("Survey_UspGroupMaintenance", param, null);

            //cDataSrc.ExecuteNonQuery("UspGroupMaintenance", cNameValueList, null);            

            cDataSrc = null;
            //HandleWriteLogDAU("UspGroupMaintenance ", cNameValueList, new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        /// <summary>
        /// Get Group List Count by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetGroupListCount(string accountID)
        {
            int groupCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, null, "C" };

                groupCount = (int)cDataSrc.ExecuteScalar("Survey_UspGroupSelect", param, null);

                //HandleWriteLogDAU("UspGroupSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return groupCount;
        }

        /// <summary>
        /// Get datatable Group List by account ID
        /// </summary>
        /// <param name="accountID"> account ID</param>
        /// <returns></returns>
        public DataTable GetdtGroupList(string accountID)
        {
            DataTable dataTableAllGroup = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, null, "A" };

                dataTableAllGroup = cDataSrc.ExecuteDataSet("Survey_UspGroupSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspGroupSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllGroup;
        }
    }
}
