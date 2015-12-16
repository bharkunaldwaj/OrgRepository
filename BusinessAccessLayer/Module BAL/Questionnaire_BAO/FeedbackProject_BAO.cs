using System;
using System.Collections.Generic;

using DAF_BAO;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
namespace Questionnaire_BAO
{
    public class FeedbackProject_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addProject;
        private int searchProject;


        #endregion

        /// <summary>
        /// Get Project By ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public List<FeedbackProject_BE> GetProjectByID(int accountID, int projectID)
        {
            List<FeedbackProject_BE> projectBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                FeedbackProject_DAO projectDataAccessObject = new FeedbackProject_DAO();
                projectDataAccessObject.GetProjectByID(accountID, projectID);

                projectBusinessEntityList = projectDataAccessObject.project_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return projectBusinessEntityList;
        }

        /// <summary>
        /// Get Project List by account id.
        /// </summary>
        /// <param name="accountID">account id</param>
        /// <returns></returns>
        public DataTable GetdtProjectList(string accountID)
        {
            DataTable dataTableProject = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                FeedbackProject_DAO projectDataAccessObject = new FeedbackProject_DAO();
                dataTableProject = projectDataAccessObject.GetdtProjectList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProject;
        }
    }
}


 