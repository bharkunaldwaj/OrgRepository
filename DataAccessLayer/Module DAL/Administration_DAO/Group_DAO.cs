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
    public class Group_DAO : DAO_Base {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Constructor"
        /// <summary>
        /// Public Constructor
        /// </summary>
        public Group_DAO() {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        public List<Group_BE> Group_BEList { get; set; }
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
        private void ShiftDataTableToBEList(DataTable p_dtAllGroup) {
            HandleWriteLog("Start", new StackTrace(true));
            Group_BEList = new List<Group_BE>();
            for (int recordCounter = 0; recordCounter < p_dtAllGroup.Rows.Count; recordCounter++) {
                Group_BE group_BE = new Group_BE();

                group_BE.GroupID = GetInt(p_dtAllGroup.Rows[recordCounter]["GroupID"]);
                group_BE.GroupName = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["GroupName"]);
                group_BE.Description = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["Description"]);
                group_BE.WelcomeText = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["WelcomeText"]);
                group_BE.NewsText = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["NewsText"]);
                group_BE.IsActive = GetBool(p_dtAllGroup.Rows[recordCounter]["IsActive"]);
                group_BE.CreatedDate = GetDateTime(p_dtAllGroup.Rows[recordCounter]["CreatedDate"]);
                group_BE.ModifiedDate = GetDateTime(p_dtAllGroup.Rows[recordCounter]["ModifiedDate"]);
                group_BE.Description = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["Description"]);
                Group_BEList.Add(group_BE);
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
        public int GetGroup(Group_BE p_GroupBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));              
                DataTable dtAllGroup = new DataTable();

                object[] param = new object[3] {p_GroupBE.GroupID,
                                                GetString(p_GroupBE.GroupName),
                                                "ALL" };
  
                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_GroupBE.GroupID);
                //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName.ToString());
                //cNameValueList.Add("@chvFlag", "ALL");


                dtAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", param, null).Tables[0];

                //dtAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", cNameValueList, null).Tables[0];                              

                ShiftDataTableToBEList(dtAllGroup);
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
        /// <param name="p_Group_BE"></param>
        /// <returns></returns>
        public bool IsGroupExist(Group_BE p_Group_BE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));                
                DataTable dtAllGroup = new DataTable();

                object[] param = new object[3] {p_Group_BE.GroupID,
                                                GetString(p_Group_BE.GroupName) ,
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
        public int AddGroup(Group_BE p_GroupBE) {
            //int IsActive=0;
            //IsActive = p_Group_BE.IsActive == true ? 1 : 0;
            string GroupID = string.Empty;
            try {
                HandleWriteLog("Start", new StackTrace(true));                             
                object[] param = new object[7] {null,
                    GetString(p_GroupBE.GroupName) ,
                    GetString(p_GroupBE.Description),
                    GetString(p_GroupBE.WelcomeText),
                    GetString(p_GroupBE.NewsText),
                    p_GroupBE.IsActive,
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
            catch (Exception ex) {
                HandleException(ex);
            }
            return (Convert.ToInt32(GroupID));
        }

        /// <summary>
        /// Function to Update records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void UpdateGroup(Group_BE p_GroupBE) {
            HandleWriteLog("Start", new StackTrace(true));           
            object[] param = new object[7] { p_GroupBE.GroupID,
                                GetString(p_GroupBE.GroupName),
                                GetString(p_GroupBE.Description),
                                GetString(p_GroupBE.WelcomeText),
                                GetString(p_GroupBE.NewsText),
                                p_GroupBE.IsActive,
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
        public void DeleteGroup(Group_BE p_GroupBE) {
            HandleWriteLog("Start", new StackTrace(true));               
            object[] param = new object[7] { p_GroupBE.GroupID,
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

        public int GetGroupListCount(string accountID)
        {
            int groupCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] {accountID, null, "C" };

                groupCount = (int)cDataSrc.ExecuteScalar("UspGroupSelect", param, null);

                //HandleWriteLogDAU("UspGroupSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return groupCount;
        }

        public DataTable GetdtGroupList(string accountID)
        {
            DataTable dtAllGroup = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, null, "A" };

                dtAllGroup = cDataSrc.ExecuteDataSet("UspGroupSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspGroupSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllGroup;
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
        public List<Survey_Group_BE> Group_BEList { get; set; }
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
        private void ShiftDataTableToBEList(DataTable p_dtAllGroup)
        {
            HandleWriteLog("Start", new StackTrace(true));
            Group_BEList = new List<Survey_Group_BE>();
            for (int recordCounter = 0; recordCounter < p_dtAllGroup.Rows.Count; recordCounter++)
            {
                Survey_Group_BE group_BE = new Survey_Group_BE();

                group_BE.GroupID = GetInt(p_dtAllGroup.Rows[recordCounter]["GroupID"]);
                group_BE.GroupName = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["GroupName"]);
                group_BE.Description = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["Description"]);
                group_BE.WelcomeText = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["WelcomeText"]);
                group_BE.NewsText = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["NewsText"]);
                group_BE.IsActive = GetBool(p_dtAllGroup.Rows[recordCounter]["IsActive"]);
                group_BE.CreatedDate = GetDateTime(p_dtAllGroup.Rows[recordCounter]["CreatedDate"]);
                group_BE.ModifiedDate = GetDateTime(p_dtAllGroup.Rows[recordCounter]["ModifiedDate"]);
                group_BE.Description = Convert.ToString(p_dtAllGroup.Rows[recordCounter]["Description"]);
                Group_BEList.Add(group_BE);
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
        public int GetGroup(Survey_Group_BE p_GroupBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllGroup = new DataTable();

                object[] param = new object[3] {p_GroupBE.GroupID,
                                                GetString(p_GroupBE.GroupName),
                                                "ALL" };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_GroupBE.GroupID);
                //cNameValueList.Add("@chvGroupName", p_GroupBE.GroupName.ToString());
                //cNameValueList.Add("@chvFlag", "ALL");


                dtAllGroup = cDataSrc.ExecuteDataSet("Survey_UspGetGroup", param, null).Tables[0];

                //dtAllGroup = cDataSrc.ExecuteDataSet("UspGetGroup", cNameValueList, null).Tables[0];                              

                ShiftDataTableToBEList(dtAllGroup);
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
        /// <param name="p_Group_BE"></param>
        /// <returns></returns>
        public bool IsGroupExist(Survey_Group_BE p_Group_BE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllGroup = new DataTable();

                object[] param = new object[3] {p_Group_BE.GroupID,
                                                GetString(p_Group_BE.GroupName) ,
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
        public int AddGroup(Survey_Group_BE p_GroupBE)
        {
            //int IsActive=0;
            //IsActive = p_Group_BE.IsActive == true ? 1 : 0;
            string GroupID = string.Empty;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[7] {null,
                    GetString(p_GroupBE.GroupName) ,
                    GetString(p_GroupBE.Description),
                    GetString(p_GroupBE.WelcomeText),
                    GetString(p_GroupBE.NewsText),
                    p_GroupBE.IsActive,
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
        public void UpdateGroup(Survey_Group_BE p_GroupBE)
        {
            HandleWriteLog("Start", new StackTrace(true));
            object[] param = new object[7] { p_GroupBE.GroupID,
                                GetString(p_GroupBE.GroupName),
                                GetString(p_GroupBE.Description),
                                GetString(p_GroupBE.WelcomeText),
                                GetString(p_GroupBE.NewsText),
                                p_GroupBE.IsActive,
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
        public void DeleteGroup(Survey_Group_BE p_GroupBE)
        {
            HandleWriteLog("Start", new StackTrace(true));
            object[] param = new object[7] { p_GroupBE.GroupID,
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

        public DataTable GetdtGroupList(string accountID)
        {
            DataTable dtAllGroup = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { accountID, null, "A" };

                dtAllGroup = cDataSrc.ExecuteDataSet("Survey_UspGroupSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspGroupSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllGroup;
        }
    }









}
