using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class Survey_Programme_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addProgramme;
        private int searchProgramme;


        #endregion

        #region CRUD Operations
        /// <summary>
        /// Add Programme
        /// </summary>
        /// <param name="programmeBusinessEntity"></param>
        /// <returns></returns>
        public int AddProgramme(Survey_Programme_BE programmeBusinessEntity)
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
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            addProgramme = programmeDataAccessObject.AddProgramme(programmeBusinessEntity);
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
            return addProgramme;
        }

        /// <summary>
        /// update Programme
        /// </summary>
        /// <param name="programmeBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateProgramme(Survey_Programme_BE programmeBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            //    try
            //   {
            sqlClient = CDataSrc.Default as CSqlClient;
            conn = sqlClient.Connection();
            dbTransaction = conn.BeginTransaction();

            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            addProgramme = programmeDataAccessObject.UpdateProgramme(programmeBusinessEntity);
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
            return addProgramme;
        }

        /// <summary>
        /// Get number of Analysis
        /// </summary>
        /// <param name="programme_ID">programme ID</param>
        /// <returns></returns>
        public DataTable No_of_Analysis(int programme_ID)
        {
            DataTable dataTableNoOfAnalysis = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            dataTableNoOfAnalysis = programmeDataAccessObject.Get_No_of_Analysis(programme_ID);
            return dataTableNoOfAnalysis;

        }

        /// <summary>
        /// Get Analysis one by programme ID
        /// </summary>
        /// <param name="programme_ID">programme ID</param>
        /// <returns></returns>
        public DataTable GetAnalysis1(int programme_ID)
        {
            DataTable programme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            programme = programmeDataAccessObject.GetAnalysis1(programme_ID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme;
        }

        /// <summary>
        /// Get Analysis two by programme ID
        /// </summary>
        /// <param name="programme_ID">programme ID</param>
        /// <returns></returns>
        public DataTable GetAnalysis2(int programme_ID)
        {
            DataTable programme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            programme = programmeDataAccessObject.GetAnalysis2(programme_ID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme;
        }

        /// <summary>
        /// Get Analysis two by programme ID
        /// </summary>
        /// <param name="programme_ID">programme ID</param>
        /// <returns></returns>
        public DataTable GetAnalysis3(int programme_ID)
        {
            DataTable programme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            programme = programmeDataAccessObject.GetAnalysis3(programme_ID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme;
        }

        /// <summary>
        /// Delete Programme
        /// </summary>
        /// <param name="programmeBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteProgramme(Survey_Programme_BE programmeBusinessEntity)
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
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            addProgramme = programmeDataAccessObject.DeleteProgramme(programmeBusinessEntity);
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
            return addProgramme;
        }

        /// <summary>
        /// Get Programme By account ID and programme ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <returns></returns>
        public List<Survey_Programme_BE> GetProgrammeByID(int accountID, int programmeID)
        {
            List<Survey_Programme_BE> programmeBusinessEntityList = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            programmeDataAccessObject.GetProgrammeByID(accountID, programmeID);

            programmeBusinessEntityList = programmeDataAccessObject.programme_BEList;

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programmeBusinessEntityList;
        }

        /// <summary>
        /// Get Programme List
        /// </summary>
        /// <returns></returns>
        public List<Survey_Programme_BE> GetProgrammeList()
        {
            List<Survey_Programme_BE> programmeBusinessEntityList = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            programmeDataAccessObject.GetProgrammeList();

            programmeBusinessEntityList = programmeDataAccessObject.programme_BEList;

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programmeBusinessEntityList;
        }

        /// <summary>
        /// Get Programme List by account id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtProgrammeList(string accountID)
        {
            DataTable dataTableProgramme = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            dataTableProgramme = programmeDataAccessObject.GetdtProgrammeList(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableProgramme;
        }

        /// <summary>
        /// Get Programme List by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtProgrammeListNew(string accountID)
        {
            DataTable dataTableProgramme = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            dataTableProgramme = programmeDataAccessObject.GetdtProgrammeListNew(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableProgramme;
        }

        /// <summary>
        /// Get Programme List count by account id
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetProgrammeListCount(string accountID)
        {
            int programmeCount = 0;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            programmeCount = programmeDataAccessObject.GetProgrammeListCount(accountID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return programmeCount;
        }
        #endregion

        /// <summary>
        /// Save category for analysis list
        /// </summary>
        /// <param name="programId">program id</param>
        /// <param name="categoryDetailList">Category Detail list</param>
        /// <param name="analysisTypeList">Analysis Type list</param>
        /// <param name="categoryNameList">Category Name list</param>
        /// <returns></returns>
        public int save_category_for_analysis_list(int programId, string categoryDetailList, string analysisTypeList, string categoryNameList)
        {
            int result = 0;
            ////////   try
            ////////{
            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            result = programmeDataAccessObject.save_category_for_analysis_list(programId, categoryDetailList, analysisTypeList, categoryNameList);

            //HandleWriteLog("End", new StackTrace(true));
            ////////}
            ////////   catch (Exception ex)
            ////////   {
            ////////       HandleException(ex);
            ////////   }

            return result;
        }

        /// <summary>
        /// Get Project Programme
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="companyId">company Id</param>
        /// <param name="programmeId">programme Id</param>
        /// <returns></returns>
        public DataTable GetProjectProgramme(int projectID, int companyId = 0, int programmeId = 0)
        {
            DataTable dataTableProgramme = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            dataTableProgramme = programmeDataAccessObject.GetProjectProgramme(projectID, companyId, programmeId);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableProgramme;
        }

        /// <summary>
        /// Get Programme By programme ID
        /// </summary>
        /// <param name="programmeID">programme ID</param>
        /// <returns></returns>
        public DataTable GetProgrammeByID(int programmeID)
        {
            DataTable dataTableProgramme = null;

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            Survey_Programme_DAO programmeDataAccessObject = new Survey_Programme_DAO();
            dataTableProgramme = programmeDataAccessObject.GetProgrammeByID(programmeID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dataTableProgramme;
        }
    }
}
