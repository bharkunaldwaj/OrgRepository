using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using System.IO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class Project_BAO ///: Base_BAO
    {
        #region "Private Member Variable"

        private int addProject;
        private int searchProject;


        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Project details
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int AddProject(Project_BE project_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                Project_DAO project_DAO = new Project_DAO();
                addProject = project_DAO.AddProject(project_BE);
                //HandleWriteLog("End", new StackTrace(true));

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
            return addProject;
        }
        /// <summary>
        /// Update Project
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int UpdateProject(Project_BE project_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                Project_DAO project_DAO = new Project_DAO();
                addProject = project_DAO.UpdateProject(project_BE);
                //HandleWriteLog("End", new StackTrace(true));

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
            return addProject;
        }
        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="project_BE"></param>
        /// <returns></returns>
        public int DeleteProject(Project_BE project_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                //HandleWriteLog("Start", new StackTrace(true));
                Project_DAO project_DAO = new Project_DAO();
                addProject = project_DAO.DeleteProject(project_BE);
                //HandleWriteLog("End", new StackTrace(true));

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
            return addProject;
        }
        /// <summary>
        /// Get Project By ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public List<Project_BE> GetProjectByID(int accountID, int projectID)
        {
            List<Project_BE> project_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                project_DAO.GetProjectByID(accountID, projectID);

                project_BEList = project_DAO.project_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project_BEList;
        }
        /// <summary>
        /// Get Project List
        /// </summary>
        /// <returns></returns>
        public List<Project_BE> GetProjectList()
        {
            List<Project_BE> project_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                project_DAO.GetProjectList();

                project_BEList = project_DAO.project_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project_BEList;
        }
        /// <summary>
        /// Get Project List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtProjectList(string accountID)
        {
            DataTable dtProject = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                dtProject = project_DAO.GetdtProjectList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProject;
        }
        /// <summary>
        /// Get Project List by accountID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtProjectListNew(string accountID)
        {
            DataTable dtProject = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                dtProject = project_DAO.GetdtProjectListNew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProject;
        }
        /// <summary>
        /// Get Admin Project List by account ID
        /// </summary>
        /// <param name="accountID"> accountID</param>
        /// <returns></returns>
        public DataTable GetAdminProjectList(string accountID)
        {
            DataTable dtProject = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                dtProject = project_DAO.GetAdminProjectList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProject;
        }
        /// <summary>
        /// Search Project
        /// </summary>
        /// <param name="project_Search"></param>
        /// <returns></returns>
        public int GetProjectListCount(string accountID)
        {
            int projectCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                projectCount = project_DAO.GetProjectListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return projectCount;
        }

        //public DataTable SearchProject(Project_Search project_Search)
        //{
        //    DataTable dtProjectSearch = null;

        //    try
        //    {
        //        //HandleWriteLog("Start", new StackTrace(true));

        //        Project_DAO project_DAOSearch = new Project_DAO();
        //        dtProjectSearch = project_DAOSearch.SearchProject(project_Search);

        //        //HandleWriteLog("End", new StackTrace(true));
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    return dtProjectSearch;
        //}

        /// <summary>
        /// Get Account Project by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetAccountProject(int accountID)
        {
            DataTable dtAccountProject = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                dtAccountProject = project_DAO.GetAccountProject(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountProject;
        }
        /// <summary>
        /// Insert Project by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="accountid"></param>
        public void InsertProjID(string Id, int accountid)
        {


            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_insert = new Project_DAO();
                project_insert.InsertprojID(Id, accountid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }
        /// <summary>
        /// Get project details By ID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                project = project_DAO.GetdataProjectByID(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        #endregion
        /// <summary>
        /// Get Project Relation ship
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetProjectRelationship(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                project = project_DAO.GetProjectRelationship(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        /// <summary>
        /// Get Project Faq Text
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string GetProjectFaqText(int projectId)
        {
            string project = "";

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                project = project_DAO.GetProjectFaqText(projectId);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        /// <summary>
        /// Get Project by account id.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetAccProject(int accountID)
        {
            DataTable dtAccProject = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAO = new Project_DAO();
                dtAccProject = project_DAO.GetAccProject(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccProject;
        }
        /// <summary>
        /// Use to Handlexceprion
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            //ExceptionLogger.Write(ex.ToString());
            FileStream FS;
            StreamWriter SW;
            string fpath;

            string str = "Error Occured on: " + DateTime.Now + Environment.NewLine + "," +
                        "Error Application: Feedback 360 - UI" + Environment.NewLine + "," +
                        "Error Function: " + ex.TargetSite + Environment.NewLine + "," +
                        "Error Line: " + ex.StackTrace + Environment.NewLine + "," +
                        "Error Source: " + ex.Source + Environment.NewLine + "," +
                        "Error Message: " + ex.Message + Environment.NewLine;

            fpath = System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"].ToString() + "ErrorLog.txt";

            if (File.Exists(fpath))
            { FS = new FileStream(fpath, FileMode.Append, FileAccess.Write); }
            else
            { FS = new FileStream(fpath, FileMode.Create, FileAccess.Write); }

            string msg = str.Replace(",", "");

            SW = new StreamWriter(FS);
            SW.WriteLine(msg);

            SW.Close();
            FS.Close();

        }

    }
}
