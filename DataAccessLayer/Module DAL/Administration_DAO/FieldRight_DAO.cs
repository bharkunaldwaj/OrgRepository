/*
* PURPOSE: Data Access Object for FieldRight Entity
* AUTHOR:  Manish Mathur
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
    public class FieldRight_DAO : DAO_Base {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Properties"
        //public FieldRight_BE[] FieldRight_BEArray { get; set; }
        public List<FieldRight_BE> FieldRight_BEList { get; set; }
        #endregion

        #region "Private Variable"
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to BEArray object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAllMenus) {
            HandleWriteLog("Start", new StackTrace(true));
            FieldRight_BEList = new List<FieldRight_BE>();
            for (int recordCounter = 0; recordCounter < p_dtAllMenus.Rows.Count; recordCounter++) {
                FieldRight_BE fieldRight_BE = new FieldRight_BE();

                fieldRight_BE.FRID = GetInt(p_dtAllMenus.Rows[recordCounter]["FRID"]);
                fieldRight_BE.Name = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Name"]);
                fieldRight_BE.Value = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["Value"]);
                fieldRight_BE.SortOrder = GetInt(p_dtAllMenus.Rows[recordCounter]["SortOrder"]);
                fieldRight_BE.IsActive = GetInt(p_dtAllMenus.Rows[recordCounter]["IsActive"]);
                fieldRight_BE.SortOrder = GetInt(p_dtAllMenus.Rows[recordCounter]["SortOrder"]);
                fieldRight_BE.MenuID = Convert.ToString(p_dtAllMenus.Rows[recordCounter]["MenuID"]);
                FieldRight_BEList.Add(fieldRight_BE);
            }
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Get FieldRight details
        /// </summary>
        /// <param name="p_contact_BE"></param>
        /// <returns></returns>
        public int GetFieldRight() {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlSelectCommand = string.Empty;
                DataTable dtAllMenus = new DataTable();

                dtAllMenus = cDataSrc.ExecuteDataSet("UspGetFieldRights",null).Tables[0];
                object[] param = new object[1];
                ShiftDataTableToBEList(dtAllMenus);
                m_returnValue = 1;
                HandleWriteLogDAU("UspGetFieldRights ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));

            }
            catch (Exception ex) { HandleException(ex); }
            return m_returnValue;
        }
        #endregion
    }
}
