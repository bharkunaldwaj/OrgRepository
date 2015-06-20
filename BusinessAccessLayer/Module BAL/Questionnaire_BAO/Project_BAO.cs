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
namespace Questionnaire_BAO
{
    public class Project_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addProject;
        private int searchProject;


        #endregion

        #region CRUD Operations

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

        public DataTable SearchProject(Project_Search project_Search)
        {
            DataTable dtProjectSearch = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Project_DAO project_DAOSearch = new Project_DAO();
                dtProjectSearch = project_DAOSearch.SearchProject(project_Search);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProjectSearch;
        }

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

        public DataTable GetManagerProject(string candidateEmail, int accountID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                Project_DAO project_DAO = new Project_DAO();
                dtResult = project_DAO.GetManagerProject(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public DataTable GetManagerProgramme(string candidateEmail, int accountID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                Project_DAO project_DAO = new Project_DAO();
                dtResult = project_DAO.GetManagerProgramme(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

    }






















    public class Survey_Project_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addProject;
        private int searchProject;


        #endregion

        #region CRUD Operations


        public DataTable GetProjectRelationship(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                project = project_DAO.GetProjectRelationship(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }




        public int AddProject(Survey_Project_BE project_BE)
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
                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                addProject = project_DAO.AddProject(project_BE);
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

        public int UpdateProject(Survey_Project_BE project_BE)
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
                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                addProject = project_DAO.UpdateProject(project_BE);
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

        public int DeleteProject(Survey_Project_BE project_BE)
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
                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                addProject = project_DAO.DeleteProject(project_BE);
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

        public List<Survey_Project_BE> GetProjectByID(int accountID, int projectID)
        {
            List<Survey_Project_BE> project_BEList = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                project_DAO.GetProjectByID(accountID, projectID);

                project_BEList = project_DAO.project_BEList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project_BEList;
        }

        public List<Survey_Project_BE> GetProjectList()
        {
            List<Survey_Project_BE> project_BEList = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                project_DAO.GetProjectList();

                project_BEList = project_DAO.project_BEList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project_BEList;
        }

        public DataTable GetdtProjectList(string accountID)
        {
            DataTable dtProject = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                dtProject = project_DAO.GetdtProjectList(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProject;
        }

        public DataTable GetdtProjectListNew(string accountID)
        {
            DataTable dtProject = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                dtProject = project_DAO.GetdtProjectListNew(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProject;
        }

        public DataTable GetAdminProjectList(string accountID)
        {
            DataTable dtProject = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                dtProject = project_DAO.GetAdminProjectList(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProject;
        }

        public int GetProjectListCount(string accountID)
        {
            int projectCount = 0;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                projectCount = project_DAO.GetProjectListCount(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return projectCount;
        }

        public DataTable SearchProject(Survey_Project_Search project_Search)
        {
            DataTable dtProjectSearch = null;

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAOSearch = new Survey_Project_DAO();
                dtProjectSearch = project_DAOSearch.SearchProject(project_Search);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProjectSearch;
        }

        public DataTable GetAccountProject(int accountID)
        {
            DataTable dtAccountProject = new DataTable();

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                dtAccountProject = project_DAO.GetAccountProject(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccountProject;
        }

        public void InsertProjID(string Id, int accountid)
        {


            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_insert = new Survey_Project_DAO();
                project_insert.InsertprojID(Id, accountid);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }

        public DataTable GetdataProjectByID(int projectID)
        {
            DataTable project = new DataTable();

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                project = project_DAO.GetdataProjectByID(projectID);



                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        #endregion

      

        public string GetProjectFaqText(int projectId)
        {
            string project = "";

            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                project = project_DAO.GetProjectFaqText(projectId);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return project;
        }

        public DataTable GetAccProject(int accountID)
        {
            DataTable dtAccProject = new DataTable();

            try
            {
               HandleWriteLog("Start", new StackTrace(true));

                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                dtAccProject = project_DAO.GetAccProject(accountID);

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtAccProject;
        }

        public DataTable GetManagerProject(string candidateEmail, int accountID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                dtResult = project_DAO.GetManagerProject(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

        public DataTable GetManagerProgramme(string candidateEmail, int accountID)
        {
            DataTable dtResult = new DataTable();

            try
            {
                Survey_Project_DAO project_DAO = new Survey_Project_DAO();
                dtResult = project_DAO.GetManagerProgramme(candidateEmail, accountID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtResult;
        }

    }












}
