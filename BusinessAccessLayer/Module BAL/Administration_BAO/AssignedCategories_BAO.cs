
using System;
using System.Collections.Generic;
using System.Diagnostics;

using DAF_BAO;

using DatabaseAccessUtilities;
using System.Data;

namespace Administration_BE
{

    public class AssignedCategories_BAO : Base_BAO
    {

        #region "Business Logic for Contact BAO"

        /// <summary>
        /// This Method returns the Contacts from Contact_DAO on passing Contact_BE as a parameter
        /// </summary>
        /// <param name="p_Contact_BE"></param>
        /// <returns></returns>
        public void AddAssignCategory(AssignedCategories_BE assignedCategoriesBusinessEntityList, String Categories)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                AssignedCategories_DAO assignedCategoriesDataAccessObject = new AssignedCategories_DAO();

                assignedCategoriesDataAccessObject.AddAssignCategory(assignedCategoriesBusinessEntityList, Categories);

                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        /// <summary>
        /// Get list of Assign Category 
        /// </summary>
        /// <param name="assignedcategories_BE"></param>
        /// <returns></returns>
        public List<AssignedCategories_BE> GetAssignCategory(AssignedCategories_BE assignedcategoriesBusinessEntity)
        {
            List<AssignedCategories_BE> assignedCategoriesBusinessEntity = new List<AssignedCategories_BE>();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                AssignedCategories_DAO assignedCategoriesDataAccessObject = new AssignedCategories_DAO();
                assignedCategoriesBusinessEntity = assignedCategoriesDataAccessObject.GetAssignCategory(assignedcategoriesBusinessEntity);

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return assignedCategoriesBusinessEntity;
        }
        #endregion
    }
}