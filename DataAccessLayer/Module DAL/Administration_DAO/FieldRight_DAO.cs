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

namespace Administration_DAO
{
    public class FieldRight_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Properties"
        //public FieldRight_BE[] FieldRight_BEArray { get; set; }
        public List<FieldRight_BE> FieldRightBusinessEntityList { get; set; }
        #endregion

        #region "Private Variable"
        private int m_returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to BEArray object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllMenus)
        {
            HandleWriteLog("Start", new StackTrace(true));
            FieldRightBusinessEntityList = new List<FieldRight_BE>();

            for (int recordCounter = 0; recordCounter < dataTableAllMenus.Rows.Count; recordCounter++)
            {
                FieldRight_BE fieldRightBusinessEntity = new FieldRight_BE();

                fieldRightBusinessEntity.FRID = GetInt(dataTableAllMenus.Rows[recordCounter]["FRID"]);
                fieldRightBusinessEntity.Name = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Name"]);
                fieldRightBusinessEntity.Value = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["Value"]);
                fieldRightBusinessEntity.SortOrder = GetInt(dataTableAllMenus.Rows[recordCounter]["SortOrder"]);
                fieldRightBusinessEntity.IsActive = GetInt(dataTableAllMenus.Rows[recordCounter]["IsActive"]);
                fieldRightBusinessEntity.SortOrder = GetInt(dataTableAllMenus.Rows[recordCounter]["SortOrder"]);
                fieldRightBusinessEntity.MenuID = Convert.ToString(dataTableAllMenus.Rows[recordCounter]["MenuID"]);
                FieldRightBusinessEntityList.Add(fieldRightBusinessEntity);
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
        public int GetFieldRight()
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlSelectCommand = string.Empty;
                DataTable dataTableAllMenus = new DataTable();

                dataTableAllMenus = cDataSrc.ExecuteDataSet("UspGetFieldRights", null).Tables[0];
                object[] param = new object[1];

                ShiftDataTableToBEList(dataTableAllMenus);

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
