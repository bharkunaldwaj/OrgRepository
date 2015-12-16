using System;
using System.Collections.Generic;
using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
namespace Questionnaire_BAO
{
    public class Project_BAO : Base_BAO
    {
        #region "Private Member Variable"
        private int addProject;
        private int searchProject;
        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Project details
        /// </summary>
        /// <param name="projectBusinessEntity"></param>
        /// <returns></returns>
        public int AddProject(Project_BE projectBusinessEntity)
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
                Project_DAO projectDataAccessObject = new Project_DAO();
                addProject = projectDataAccessObject.AddProject(projectBusinessEntity);
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
        /// <param name="projectBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateProject(Project_BE projectBusinessEntity)
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
                Project_DAO projectDataAccessObject = new Project_DAO();
                addProject = projectDataAccessObject.UpdateProject(projectBusinessEntity);
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
        /// <param name="projectBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteProject(Project_BE projectBusinessEntity)
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
                Project_DAO projectDataAccessObject = new Project_DAO();
                addProject = projectDataAccessObject.DeleteProject(projectBusinessEntity);
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
            List<Project_BE> projectBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
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
        /// Get Project List
        /// </summary>
        /// <returns></returns>
        public List<Project_BE> GetProjectList()
        {
            List<Project_BE> projectBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
                projectDataAccessObject.GetProjectList();

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
        /// Get Project List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtProjectList(string accountID)
        {
            DataTable dataTableProject = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
                dataTableProject = projectDataAccessObject.GetdtProjectList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProject;
        }

        /// <summary>
        /// Get Project List by accountID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtProjectListNew(string accountID)
        {
            DataTable dataTableProject = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
                dataTableProject = projectDataAccessObject.GetdtProjectListNew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProject;
        }

        /// <summary>
        /// Get Admin Project List by account ID
        /// </summary>
        /// <param name="accountID"> accountID</param>
        /// <returns></returns>
        public DataTable GetAdminProjectList(string accountID)
        {
            DataTable dataTableProject = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
                dataTableProject = projectDataAccessObject.GetAdminProjectList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProject;
        }

        /// <summary>
        /// Get Project List Count by account ID 
        /// </summary>
        /// <param name="accountID">accountID</param>
        /// <returns></returns>
        public int GetProjectListCount(string accountID)
        {
            int projectCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
                projectCount = projectDataAccessObject.GetProjectListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return projectCount;
        }

        /// <summary>
        /// Search Project
        /// </summary>
        /// <param name="project_Search"></param>
        /// <returns></returns>
        public DataTable SearchProject(Project_Search project_Search)
        {
            DataTable dataTableProjectSearch = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObjectSearch = new Project_DAO();
                dataTableProjectSearch = projectDataAccessObjectSearch.SearchProject(project_Search);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProjectSearch;
        }

        /// <summary>
        /// Get Account Project by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetAccountProject(int accountID)
        {
            DataTable dataTableAccountProject = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
                dataTableAccountProject = projectDataAccessObject.GetAccountProject(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAccountProject;
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

                Project_DAO projectinsert = new Project_DAO();
                projectinsert.InsertprojID(Id, accountid);

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

                Project_DAO projectDataAccessObject = new Project_DAO();
                project = projectDataAccessObject.GetdataProjectByID(projectID);



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

                Project_DAO projectDataAccessObject = new Project_DAO();
                project = projectDataAccessObject.GetProjectRelationship(projectID);

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

                Project_DAO projectDataAccessObject = new Project_DAO();
                project = projectDataAccessObject.GetProjectFaqText(projectId);

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
            DataTable dataTableAccountProject = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO projectDataAccessObject = new Project_DAO();
                dataTableAccountProject = projectDataAccessObject.GetAccProject(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAccountProject;
        }

        /// <summary>
        /// Get Manager in Project
        /// </summary>
        /// <param name="candidateEmail"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetManagerProject(string candidateEmail, int accountID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                Project_DAO projectDataAccessObject = new Project_DAO();
                dataTableResult = projectDataAccessObject.GetManagerProject(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Manager Programme
        /// </summary>
        /// <param name="candidateEmail"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetManagerProgramme(string candidateEmail, int accountID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                Project_DAO projectDataAccessObject = new Project_DAO();
                dataTableResult = projectDataAccessObject.GetManagerProgramme(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

    }


    public class Survey_Project_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addProject;
        private int searchProject;


        #endregion

        #region CRUD Operations

        /// <summary>
        /// Get Project Relationship by project id
        /// </summary>
        /// <param name="projectID">project id</param>
        /// <returns></returns>
        public DataTable GetProjectRelationship(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                project = projectDataAccessObject.GetProjectRelationship(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        /// <summary>
        /// Insert project
        /// </summary>
        /// <param name="projectBusinessEntity"></param>
        /// <returns></returns>
        public int AddProject(Survey_Project_BE projectBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            //try
            //{
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
            addProject = projectDataAccessObject.AddProject(projectBusinessEntity);
            //HandleWriteLog("End", new StackTrace(true));

            dbTransaction.Commit();
            conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    if (dbTransaction != null)
            //    {
            //        dbTransaction.Rollback();
            //    }

            //    HandleException(ex);
            //}
            return addProject;
        }

        /// <summary>
        /// update project
        /// </summary>
        /// <param name="projectBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateProject(Survey_Project_BE projectBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            //try
            //{
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
            addProject = projectDataAccessObject.UpdateProject(projectBusinessEntity);
            //HandleWriteLog("End", new StackTrace(true));

            dbTransaction.Commit();
            conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    if (dbTransaction != null)
            //    {
            //        dbTransaction.Rollback();
            //    }

            //    HandleException(ex);
            //}
            return addProject;
        }

        /// <summary>
        /// Delete project
        /// </summary>
        /// <param name="projectBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteProject(Survey_Project_BE projectBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            //try
            //{
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
            addProject = projectDataAccessObject.DeleteProject(projectBusinessEntity);
            //HandleWriteLog("End", new StackTrace(true));

            dbTransaction.Commit();
            conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    if (dbTransaction != null)
            //    {
            //        dbTransaction.Rollback();
            //    }

            //    HandleException(ex);
            //}
            return addProject;
        }

        /// <summary>
        /// Get Project By ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public List<Survey_Project_BE> GetProjectByID(int accountID, int projectID)
        {
            List<Survey_Project_BE> projectBusinessEntityList = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                projectDataAccessObject.GetProjectByID(accountID, projectID);

                projectBusinessEntityList = projectDataAccessObject.project_BEList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return projectBusinessEntityList;
        }

        /// <summary>
        /// Get Project List
        /// </summary>
        /// <returns></returns>
        public List<Survey_Project_BE> GetProjectList()
        {
            List<Survey_Project_BE> projectBusinessEntityList = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                projectDataAccessObject.GetProjectList();

                projectBusinessEntityList = projectDataAccessObject.project_BEList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return projectBusinessEntityList;
        }

        /// <summary>
        /// Get Project List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtProjectList(string accountID)
        {
            DataTable dataTableProject = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                dataTableProject = projectDataAccessObject.GetdtProjectList(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProject;
        }

        /// <summary>
        /// Get Project List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtProjectListNew(string accountID)
        {
            DataTable dataTableProject = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                dataTableProject = projectDataAccessObject.GetdtProjectListNew(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProject;
        }

        /// <summary>
        /// Get Admin  Project List by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetAdminProjectList(string accountID)
        {
            DataTable dataTableProject = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                dataTableProject = projectDataAccessObject.GetAdminProjectList(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProject;
        }

        /// <summary>
        /// Get Project List  count by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetProjectListCount(string accountID)
        {
            int projectCount = 0;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                projectCount = projectDataAccessObject.GetProjectListCount(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return projectCount;
        }

        /// <summary>
        /// Search project
        /// </summary>
        /// <param name="projectSearch"></param>
        /// <returns></returns>
        public DataTable SearchProject(Survey_Project_Search projectSearch)
        {
            DataTable dataTableProjectSearch = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObjectSearch = new Survey_Project_DAO();
                dataTableProjectSearch = projectDataAccessObjectSearch.SearchProject(projectSearch);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProjectSearch;
        }

        /// <summary>
        /// Get Account Project by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetAccountProject(int accountID)
        {
            DataTable dataTableAccountProject = new DataTable();

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                dataTableAccountProject = projectDataAccessObject.GetAccountProject(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAccountProject;
        }

        /// <summary>
        /// Insert project .
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountid"></param>
        public void InsertProjID(string Id, int accountid)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectInsert = new Survey_Project_DAO();
                projectInsert.InsertprojID(Id, accountid);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }

        /// <summary>
        /// Get datatable Project By ID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                project = projectDataAccessObject.GetdataProjectByID(projectID);



                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }
        #endregion


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
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                project = projectDataAccessObject.GetProjectFaqText(projectId);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        /// <summary>
        /// Get company Faq Text
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string GetCompanyFaqText(int projectId)
        {
            string project = "";

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                project = projectDataAccessObject.GetCompanyFaqText(projectId);

                HandleWriteLog("End", new StackTrace(true));
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
            DataTable dataTableAccountProject = new DataTable();

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                dataTableAccountProject = projectDataAccessObject.GetAccProject(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableAccountProject;
        }

        /// <summary>
        /// Get Manager in Project
        /// </summary>
        /// <param name="candidateEmail"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetManagerProject(string candidateEmail, int accountID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                dataTableResult = projectDataAccessObject.GetManagerProject(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }

        /// <summary>
        /// Get Manager Programme
        /// </summary>
        /// <param name="candidateEmail"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetManagerProgramme(string candidateEmail, int accountID)
        {
            DataTable dataTableResult = new DataTable();

            try
            {
                Survey_Project_DAO projectDataAccessObject = new Survey_Project_DAO();
                dataTableResult = projectDataAccessObject.GetManagerProgramme(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableResult;
        }
    }
}
