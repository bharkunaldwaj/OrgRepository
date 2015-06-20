using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;
namespace Questionnaire_BAO {
    public class FeedbackProject_BAO : Base_BAO {
        #region "Private Member Variable"

        private int addProject;
        private int searchProject;


        #endregion

        


        public List<FeedbackProject_BE> GetProjectByID(int accountID, int projectID) {
            List<FeedbackProject_BE> project_BEList = null;

            try {
                //HandleWriteLog("Start", new StackTrace(true));

                FeedbackProject_DAO project_DAO = new FeedbackProject_DAO();
                project_DAO.GetProjectByID(accountID, projectID);

                project_BEList = project_DAO.project_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return project_BEList;
        }



        public DataTable GetdtProjectList(string accountID) {
            DataTable dtProject = null;

            try {
                //HandleWriteLog("Start", new StackTrace(true));

                FeedbackProject_DAO project_DAO = new FeedbackProject_DAO();
                dtProject = project_DAO.GetdtProjectList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }

            return dtProject;
        }



    }



}


 