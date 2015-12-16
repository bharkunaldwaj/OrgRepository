/*
* PURPOSE: Data Access Object for MenuMaster Entity
* AUTHOR: 
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using feedbackFramework_DAO;

using Administration_BE;
using DatabaseAccessUtilities;

namespace Administration_DAO
{
    public class MenuMaster_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        //List<MenuMaster_BE> MenuMaster_BEList = new List<MenuMaster_BE>();

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public MenuMaster_DAO()
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region Public Properties
        public List<MenuMaster_BE> MenuMasterBusinessEntityList { get; set; }
        #endregion

        #region "Private Variable"
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to MenuMaster_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllMenus)
        {
            HandleWriteLog("Start", new StackTrace(true));
            MenuMasterBusinessEntityList = new List<MenuMaster_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAllMenus.Rows.Count; recordCounter++)
            {
                MenuMaster_BE menuMaster_BE = new MenuMaster_BE();

                menuMaster_BE.MenuID = GetInt(dataTableAllMenus.Rows[recordCounter]["MenuID"]);
                menuMaster_BE.Name = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Name"]);
                menuMaster_BE.Page = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Page"]);
                menuMaster_BE.ParentID = GetInt(dataTableAllMenus.Rows[recordCounter]["ParentID"]);
                menuMaster_BE.Visibility = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Visibility"]);
                menuMaster_BE.SortOrder = GetInt(dataTableAllMenus.Rows[recordCounter]["SortOrder"]);
                menuMaster_BE.ADEVFlag = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["ADEVFlag"]);
                MenuMasterBusinessEntityList.Add(menuMaster_BE);
            }
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Get Menus as per User Rights
        /// </summary>
        /// <param name="p_MenuMaster_BE"></param>
        /// <returns></returns>
        public int GetMenuMaster(MenuMaster_BE menuMasterBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllMenus = new DataTable();
                object[] param1 = new object[1] { menuMasterBusinessEntity.ADEVFlag };
                dataTableAllMenus = cDataSrc.ExecuteDataSet("UspGetMenuMaster", param1, null).Tables[0];

                ShiftDataTableToBEList(dataTableAllMenus);
                m_returnValue = 1;
                object[] param = new object[1];

                // CNameValueList cNameValueList = new CNameValueList();
                // cNameValueList.Add("@id", "");
                HandleWriteLogDAU("UspGetMenuMaster ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }

        /// <summary>
        /// Function to get the MenuParent and add it to MenuMaster_BE
        /// </summary>
        /// <param name="p_MenuMaster_BE"></param>
        /// <returns></returns>
        public List<MenuMaster_BE> GetMenuParent(List<MenuMaster_BE> menuMasterBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                for (int counter = 0; counter < menuMasterBusinessEntity.Count; counter++)
                {
                    if (menuMasterBusinessEntity[counter].ParentID == null || menuMasterBusinessEntity[counter].ParentID == 0)
                    {
                        MenuMasterBusinessEntityList.Add(menuMasterBusinessEntity[counter]);
                    }
                }
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return MenuMasterBusinessEntityList;
        }

        #endregion
    }

    public class Survey_MenuMaster_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        //List<MenuMaster_BE> MenuMaster_BEList = new List<MenuMaster_BE>();

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_MenuMaster_DAO()
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region Public Properties
        public List<Survey_MenuMaster_BE> MenuMasterBusinessEntityList { get; set; }
        #endregion

        #region "Private Variable"
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to MenuMaster_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllMenus)
        {
            HandleWriteLog("Start", new StackTrace(true));
            MenuMasterBusinessEntityList = new List<Survey_MenuMaster_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAllMenus.Rows.Count; recordCounter++)
            {
                Survey_MenuMaster_BE menuMasterBusinessEntity = new Survey_MenuMaster_BE();

                menuMasterBusinessEntity.MenuID = GetInt(dataTableAllMenus.Rows[recordCounter]["MenuID"]);
                menuMasterBusinessEntity.Name = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Name"]);
                menuMasterBusinessEntity.Page = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Page"]);
                menuMasterBusinessEntity.ParentID = GetInt(dataTableAllMenus.Rows[recordCounter]["ParentID"]);
                menuMasterBusinessEntity.Visibility = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Visibility"]);
                menuMasterBusinessEntity.SortOrder = GetInt(dataTableAllMenus.Rows[recordCounter]["SortOrder"]);
                menuMasterBusinessEntity.ADEVFlag = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["ADEVFlag"]);
                MenuMasterBusinessEntityList.Add(menuMasterBusinessEntity);
            }
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Get Menus as per User Rights
        /// </summary>
        /// <param name="p_MenuMaster_BE"></param>
        /// <returns></returns>
        public int GetMenuMaster(Survey_MenuMaster_BE menuMasterBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllMenus = new DataTable();
                dataTableAllMenus = cDataSrc.ExecuteDataSet("Survey_UspGetMenuMaster", null).Tables[0];

                ShiftDataTableToBEList(dataTableAllMenus);
                m_returnValue = 1;
                object[] param = new object[1];

                // CNameValueList cNameValueList = new CNameValueList();
                // cNameValueList.Add("@id", "");
                HandleWriteLogDAU("Survey_UspGetMenuMaster ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }

        /// <summary>
        /// Function to get the MenuParent and add it to MenuMaster_BE
        /// </summary>
        /// <param name="p_MenuMaster_BE"></param>
        /// <returns></returns>
        public List<Survey_MenuMaster_BE> GetMenuParent(List<Survey_MenuMaster_BE> menuMasterBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                for (int counter = 0; counter < menuMasterBusinessEntity.Count; counter++)
                {
                    if (menuMasterBusinessEntity[counter].ParentID == null || menuMasterBusinessEntity[counter].ParentID == 0)
                    {
                        MenuMasterBusinessEntityList.Add(menuMasterBusinessEntity[counter]);
                    }
                }
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return MenuMasterBusinessEntityList;
        }
        #endregion
    }
}
