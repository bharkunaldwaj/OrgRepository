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

using Admin_BE;
using DatabaseAccessUtilities;

namespace Administration_BE {
    
    public class AssignedCategories_DAO:DAO_Base {

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

          public List<AssignedCategories_BE> assignedcategories_BEList { get; set; }

          #endregion


          # region CRUD Operation

          public int AddAssignCategory(AssignedCategories_BE assignedcategories_BE, String Categories) {
              try {
                  //HandleWriteLog("Start", new StackTrace(true));

                     object[] param = new object[6] {
                      assignedcategories_BE.AccountID,
                      assignedcategories_BE.ProjectID,
                      assignedcategories_BE.Name,
                      assignedcategories_BE.QuestionnaireID,
                      Categories, "I" };

                  returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspAssignCategory", param, null));

                  cDataSrc = null;

                  //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                  //HandleWriteLog("End", new StackTrace(true));
              }
              catch (Exception ex) { HandleException(ex); }
              return returnValue;
          }


          public List<AssignedCategories_BE> GetAssignCategory(AssignedCategories_BE assignedcategories_BE) {
              DataTable dtAssignedCategories = new DataTable();
              try {
                  //HandleWriteLog("Start", new StackTrace(true));

                  object[] param = new object[6] {
                      assignedcategories_BE.AccountID,
                      assignedcategories_BE.ProjectID,
                      assignedcategories_BE.Name,
                      assignedcategories_BE.QuestionnaireID,
                      "", "S" };

                  dtAssignedCategories = cDataSrc.ExecuteDataSet("UspAssignCategory", param, null).Tables[0]; ;
                  ShiftDataTableToBEList(dtAssignedCategories);
                  cDataSrc = null;

                  //HandleWriteLogDAU("UspCategoryManagement", param, new StackTrace(true));
                  //HandleWriteLog("End", new StackTrace(true));
              }
              catch (Exception ex) { HandleException(ex); }
              return assignedcategories_BEList;
          }

          private void ShiftDataTableToBEList(DataTable dtCategory) {
              //HandleWriteLog("Start", new StackTrace(true));
              assignedcategories_BEList = new List<AssignedCategories_BE>();

              for (int recordCounter = 0; recordCounter < dtCategory.Rows.Count; recordCounter++) {
                  AssignedCategories_BE category_BE = new AssignedCategories_BE();

                  category_BE.CategoryID = Convert.ToInt32(dtCategory.Rows[recordCounter]["CategoryID"].ToString());
                  category_BE.AccountID = Convert.ToInt32(dtCategory.Rows[recordCounter]["AccountID"].ToString());
                  category_BE.Name = dtCategory.Rows[recordCounter]["RelationshipName"].ToString();
                  category_BE.ProjectID = Convert.ToInt32(dtCategory.Rows[recordCounter]["ProjectID"].ToString());
                  category_BE.QuestionnaireID = Convert.ToInt32(dtCategory.Rows[recordCounter]["QuestionnaireID"].ToString());

                               
                  assignedcategories_BEList.Add(category_BE);
              }

              //HandleWriteLog("End", new StackTrace(true));
          }

          #endregion

    }
}