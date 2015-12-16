using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

using feedbackFramework_DAO;

using DatabaseAccessUtilities;

namespace Administration_BE
{

    public class AssignedCategories_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public AssignedCategories_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<AssignedCategories_BE> assignedcategoriesBusinessEntityList { get; set; }

        #endregion


        # region CRUD Operation
        /// <summary>
        /// Insert Assign Category
        /// </summary>
        /// <param name="assignedcategoriesBusinessEntity"></param>
        /// <param name="Categories"></param>
        /// <returns></returns>
        public int AddAssignCategory(AssignedCategories_BE assignedcategoriesBusinessEntity, String Categories)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[6] {
                      assignedcategoriesBusinessEntity.AccountID,
                      assignedcategoriesBusinessEntity.ProjectID,
                      assignedcategoriesBusinessEntity.Name,
                      assignedcategoriesBusinessEntity.QuestionnaireID,
                      Categories, "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignCategory", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Get Assign Category
        /// </summary>
        /// <param name="assignedCategoriesBusinessEntity"></param>
        /// <returns></returns>
        public List<AssignedCategories_BE> GetAssignCategory(AssignedCategories_BE assignedCategoriesBusinessEntity)
        {
            DataTable dataTableAssignedCategories = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[6] {
                      assignedCategoriesBusinessEntity.AccountID,
                      assignedCategoriesBusinessEntity.ProjectID,
                      assignedCategoriesBusinessEntity.Name,
                      assignedCategoriesBusinessEntity.QuestionnaireID,
                      "", "S" };

                dataTableAssignedCategories = cDataSrc.ExecuteDataSet("UspAssignCategory", param, null).Tables[0]; ;
                ShiftDataTableToBEList(dataTableAssignedCategories);
                cDataSrc = null;

                //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return assignedcategoriesBusinessEntityList;
        }

        /// <summary>
        /// Shift Assign Category Data Table To BEList 
        /// </summary>
        /// <param name="dataTableCategory"></param>
        private void ShiftDataTableToBEList(DataTable dataTableCategory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            assignedcategoriesBusinessEntityList = new List<AssignedCategories_BE>();

            for (int recordCounter = 0; recordCounter < dataTableCategory.Rows.Count; recordCounter++)
            {
                AssignedCategories_BE category_BE = new AssignedCategories_BE();

                category_BE.CategoryID = Convert.ToInt32(dataTableCategory.Rows[recordCounter]["CategoryID"].ToString());
                category_BE.AccountID = Convert.ToInt32(dataTableCategory.Rows[recordCounter]["AccountID"].ToString());
                category_BE.Name = dataTableCategory.Rows[recordCounter]["RelationshipName"].ToString();
                category_BE.ProjectID = Convert.ToInt32(dataTableCategory.Rows[recordCounter]["ProjectID"].ToString());
                category_BE.QuestionnaireID = Convert.ToInt32(dataTableCategory.Rows[recordCounter]["QuestionnaireID"].ToString());


                assignedcategoriesBusinessEntityList.Add(category_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }
        #endregion
    }
}