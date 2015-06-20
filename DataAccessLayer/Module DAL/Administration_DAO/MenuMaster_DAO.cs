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
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using feedbackFramework_BE;
using feedbackFramework_DAO;

using Administration_BE;
using DatabaseAccessUtilities;

namespace Administration_DAO {
    public class MenuMaster_DAO : DAO_Base {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        //List<MenuMaster_BE> MenuMaster_BEList = new List<MenuMaster_BE>();

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public MenuMaster_DAO() {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region Public Properties
        public List<MenuMaster_BE> MenuMaster_BEList { get; set; }
        #endregion

        #region "Private Variable"
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to MenuMaster_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAllMenus) {
            HandleWriteLog("Start", new StackTrace(true));
            MenuMaster_BEList = new List<MenuMaster_BE>();

            for (int recordCounter = 0; recordCounter < p_dtAllMenus.Rows.Count; recordCounter++) {
                MenuMaster_BE menuMaster_BE = new MenuMaster_BE();

                menuMaster_BE.MenuID = GetInt(p_dtAllMenus.Rows[recordCounter]["MenuID"]);
                menuMaster_BE.Name = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Name"]);
                menuMaster_BE.Page = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Page"]);
                menuMaster_BE.ParentID = GetInt(p_dtAllMenus.Rows[recordCounter]["ParentID"]);
                menuMaster_BE.Visibility = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Visibility"]);
                menuMaster_BE.SortOrder = GetInt(p_dtAllMenus.Rows[recordCounter]["SortOrder"]);
                menuMaster_BE.ADEVFlag = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["ADEVFlag"]);
                MenuMaster_BEList.Add(menuMaster_BE);
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
        public int GetMenuMaster(MenuMaster_BE p_MenuMasterBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));             
                DataTable dtAllMenus = new DataTable();
                object[] param1 = new object[1] { p_MenuMasterBE.ADEVFlag };
                dtAllMenus = cDataSrc.ExecuteDataSet("UspGetMenuMaster", param1,null).Tables[0];

                ShiftDataTableToBEList(dtAllMenus);
                m_returnValue = 1;
                object[] param=new object[1];

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
        public List<MenuMaster_BE> GetMenuParent(List<MenuMaster_BE> p_MenuMasterBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));

                for (int counter = 0; counter < p_MenuMasterBE.Count; counter++) {
                    if (p_MenuMasterBE[counter].ParentID == null || p_MenuMasterBE[counter].ParentID == 0) {
                        MenuMaster_BEList.Add(p_MenuMasterBE[counter]);
                    }
                }
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return MenuMaster_BEList;
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
        public List<Survey_MenuMaster_BE> MenuMaster_BEList { get; set; }
        #endregion

        #region "Private Variable"
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to MenuMaster_BE object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAllMenus)
        {
            HandleWriteLog("Start", new StackTrace(true));
            MenuMaster_BEList = new List<Survey_MenuMaster_BE>();

            for (int recordCounter = 0; recordCounter < p_dtAllMenus.Rows.Count; recordCounter++)
            {
                Survey_MenuMaster_BE menuMaster_BE = new Survey_MenuMaster_BE();

                menuMaster_BE.MenuID = GetInt(p_dtAllMenus.Rows[recordCounter]["MenuID"]);
                menuMaster_BE.Name = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Name"]);
                menuMaster_BE.Page = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Page"]);
                menuMaster_BE.ParentID = GetInt(p_dtAllMenus.Rows[recordCounter]["ParentID"]);
                menuMaster_BE.Visibility = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Visibility"]);
                menuMaster_BE.SortOrder = GetInt(p_dtAllMenus.Rows[recordCounter]["SortOrder"]);
                menuMaster_BE.ADEVFlag = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["ADEVFlag"]);
                MenuMaster_BEList.Add(menuMaster_BE);
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
        public int GetMenuMaster(Survey_MenuMaster_BE p_MenuMasterBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAllMenus = new DataTable();
                dtAllMenus = cDataSrc.ExecuteDataSet("Survey_UspGetMenuMaster", null).Tables[0];

                ShiftDataTableToBEList(dtAllMenus);
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
        public List<Survey_MenuMaster_BE> GetMenuParent(List<Survey_MenuMaster_BE> p_MenuMasterBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                for (int counter = 0; counter < p_MenuMasterBE.Count; counter++)
                {
                    if (p_MenuMasterBE[counter].ParentID == null || p_MenuMasterBE[counter].ParentID == 0)
                    {
                        MenuMaster_BEList.Add(p_MenuMasterBE[counter]);
                    }
                }
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return MenuMaster_BEList;
        }

        #endregion
    }

















}
