/*  
* PURPOSE: This is the Business Access Object for FieldRight Entity
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

namespace Administration_BAO {
    public class FieldRight_BAO : Base_BAO {
        #region "Public Constructor"

        public FieldRight_BAO() {

        }

        #endregion

        #region "Business Logic for FieldRight BAO"

        /// <summary>
        /// This Method returns the array of FieldRight
        /// </summary>
        /// <returns></returns>
        public List<FieldRight_BE> GetFieldRight() {
            List<FieldRight_BE> FieldRight_BEList = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));
                FieldRight_DAO FieldRight_DAO = new FieldRight_DAO();

                FieldRight_DAO.GetFieldRight();

                FieldRight_BEList = FieldRight_DAO.FieldRight_BEList;
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return FieldRight_BEList;
        }

        /// <summary>
        /// This method passes the FieldRight_BE entity to FieldRight_DAO and performs an add operation
        /// </summary>
        /// <param name="p_fieldRight_BE"></param>
        //public int AddFieldRight(FieldRight_BE p_fieldRight_BE)
        //{

        //HandleWriteLog("Start", new StackTrace(true));
        //FieldRight_DAO FieldRight_DAO = new FieldRight_DAO();
        //HandleWriteLog("End", new StackTrace(true));     

        //    return FieldRight_DAO.AddFieldRight(p_fieldRight_BE);
        //    
        //}

        /// <summary>
        /// This method passes the FieldRight_BE entity to FieldRight_DAO and performs an add operation
        /// </summary>
        /// <param name="p_fieldRight_BE"></param>
        //public void UpdateFieldRight(FieldRight_BE p_fieldRight_BE)
        //{
        //    HandleWriteLog("Start", new StackTrace(true));
        //    FieldRight_DAO FieldRight_DAO = new FieldRight_DAO();
        //    FieldRight_DAO.UpdateFieldRight(p_fieldRight_BE);
        //    HandleWriteLog("End", new StackTrace(true));
        //}

        /// <summary>
        /// This method passes the FieldRight_BE entity to FieldRight_DAO and performs an add operation
        /// </summary>
        /// <param name="p_fieldRight_BE"></param>
        //public void DeleteFieldRight(FieldRight_BE p_fieldRight_BE)
        //{
        //    HandleWriteLog("Start", new StackTrace(true));
        //    FieldRight_DAO FieldRight_DAO = new FieldRight_DAO();
        //    FieldRight_DAO.UpdateFieldRight(p_fieldRight_BE);
        //    HandleWriteLog("End", new StackTrace(true));
        //}

        #endregion
    }
}
