using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class Programme_BAO : Base_BAO
    {
        #region "Private Member Variable"
        private int addProgramme;
        private int searchProgramme;
        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Programme 
        /// </summary>
        /// <param name="programmeBusinessEntity"></param>
        /// <returns></returns>
        public int AddProgramme(Programme_BE programmeBusinessEntity)
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
                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                addProgramme = programmeDataAccessObject.AddProgramme(programmeBusinessEntity);
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
            return addProgramme;
        }

        /// <summary>
        /// Update Programme
        /// </summary>
        /// <param name="programmeBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateProgramme(Programme_BE programmeBusinessEntity)
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
                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                addProgramme = programmeDataAccessObject.UpdateProgramme(programmeBusinessEntity);
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
            return addProgramme;
        }

        /// <summary>
        /// Delete Programme
        /// </summary>
        /// <param name="programmeBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteProgramme(Programme_BE programmeBusinessEntity)
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
                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                addProgramme = programmeDataAccessObject.DeleteProgramme(programmeBusinessEntity);
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
            return addProgramme;
        }

        /// <summary>
        /// Get Programme By ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <param name="programmeID">programme ID</param>
        /// <returns></returns>
        public List<Programme_BE> GetProgrammeByID(int accountID, int programmeID)
        {
            List<Programme_BE> programmeBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                programmeDataAccessObject.GetProgrammeByID(accountID, programmeID);

                programmeBusinessEntityList = programmeDataAccessObject.programme_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return programmeBusinessEntityList;
        }

        /// <summary>
        /// Get Programme List
        /// </summary>
        /// <returns></returns>
        public List<Programme_BE> GetProgrammeList()
        {
            List<Programme_BE> programmeBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                programmeDataAccessObject.GetProgrammeList();

                programmeBusinessEntityList = programmeDataAccessObject.programme_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return programmeBusinessEntityList;
        }

        /// <summary>
        /// Get Programme List
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable GetdtProgrammeList(string accountID)
        {
            DataTable dataTableProgramme = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                dataTableProgramme = programmeDataAccessObject.GetdtProgrammeList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProgramme;
        }

        /// <summary>
        /// Get Programme List by account id.
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetdtProgrammeListNew(string accountID)
        {
            DataTable dataTableProgramme = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                dataTableProgramme = programmeDataAccessObject.GetdtProgrammeListNew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProgramme;
        }

        /// <summary>
        /// Get Programme List Count by account ID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public int GetProgrammeListCount(string accountID)
        {
            int programmeCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                programmeCount = programmeDataAccessObject.GetProgrammeListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return programmeCount;
        }

        #endregion

        /// <summary>
        /// Get Project Programme by projectID
        /// </summary>
        /// <param name="projectID">projectID</param>
        /// <returns></returns>
        public DataTable GetProjectProgramme(int projectID)
        {
            DataTable dataTableProgramme = null;

            //   try
            //   {
            //HandleWriteLog("Start", new StackTrace(true));

            Programme_DAO programmeDataAccessObject = new Programme_DAO();
            dataTableProgramme = programmeDataAccessObject.GetProjectProgramme(projectID);

            //HandleWriteLog("End", new StackTrace(true));
            //  }
            //  catch (Exception ex)
            //  {
            //      HandleException(ex);
            //  }

            return dataTableProgramme;
        }

        /// <summary>
        /// Returns programme Instruction as per the programmeID
        /// <Author>Rudra Prakash Mishra</Author>
        /// <Date>03/06/2014</Date>
        /// </summary>
        /// <param name="programID"></param>
        /// <returns></returns>
        public string GetProgramInstructions(int programID)
        {
            try
            {
                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                return programmeDataAccessObject.GetProgramInstructions(programID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// Returns colleague Number as per the programmeID
        /// <Author>Rudra Prakash Mishra</Author>
        /// <Date>03/06/2014</Date>
        /// </summary>
        /// <param name="programID"></param>
        /// <returns></returns>
        public string GetProgramColleagueNumber(int programID)
        {
            try
            {
                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                return programmeDataAccessObject.GetProgramColleagueNumber(programID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// Returns report available and end date of a programme
        /// </summary>
        /// <param name="programID"></param>
        /// <returns></returns>
        public DateTime[] GetProgramReportDate(int programID)
        {
            try
            {
                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                return programmeDataAccessObject.GetProgramReportDate(programID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }

        }

        /// <summary>
        /// Get Programme By programme ID
        /// </summary>
        /// <param name="programmeID">programme ID</param>
        /// <returns></returns>
        public DataTable GetProgrammeByID(int programmeID)
        {
            DataTable dataTableProgramme = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programmeDataAccessObject = new Programme_DAO();
                dataTableProgramme = programmeDataAccessObject.GetProgrammeByID(programmeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableProgramme;
        }
    }
}
