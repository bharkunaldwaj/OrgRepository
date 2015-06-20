
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using DAF_BAO;

using Administration_BE;
using Administration_DAO;
using DatabaseAccessUtilities;
using System.Data;

namespace Administration_BE {
    
    public class AssignedCategories_BAO:Base_BAO {

        #region "Business Logic for Contact BAO"

        /// <summary>
        /// This Method returns the Contacts from Contact_DAO on passing Contact_BE as a parameter
        /// </summary>
        /// <param name="p_Contact_BE"></param>
        /// <returns></returns>
        public void AddAssignCategory(AssignedCategories_BE p_AssignedCategories_BEList, String Categories) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                AssignedCategories_DAO assignedCategories_DAO = new AssignedCategories_DAO();
                assignedCategories_DAO.AddAssignCategory(p_AssignedCategories_BEList, Categories);
                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex) {
                if (dbTransaction != null) {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        public List<AssignedCategories_BE> GetAssignCategory(AssignedCategories_BE assignedcategories_BE) {
            List<AssignedCategories_BE> assignedCategories_BE = new List<AssignedCategories_BE>();
            try {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignedCategories_DAO assignedCategories_DAO = new AssignedCategories_DAO();
               assignedCategories_BE =  assignedCategories_DAO.GetAssignCategory(assignedcategories_BE);

                

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return assignedCategories_BE;
        }
        #endregion

    }
}