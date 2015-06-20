using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class Programme_BAO:Base_BAO
    {
        #region "Private Member Variable"

        private int addProgramme;
        private int searchProgramme;


        #endregion

        #region CRUD Operations

        public int AddProgramme(Programme_BE programme_BE)
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
                Programme_DAO programme_DAO = new Programme_DAO();
                addProgramme = programme_DAO.AddProgramme(programme_BE);
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

        public int UpdateProgramme(Programme_BE programme_BE)
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
                Programme_DAO programme_DAO = new Programme_DAO();
                addProgramme = programme_DAO.UpdateProgramme(programme_BE);
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

        public int DeleteProgramme(Programme_BE programme_BE)
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
                Programme_DAO programme_DAO = new Programme_DAO();
                addProgramme = programme_DAO.DeleteProgramme(programme_BE);
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

        public List<Programme_BE> GetProgrammeByID(int accountID, int programmeID)
        {
            List<Programme_BE> programme_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                programme_DAO.GetProgrammeByID(accountID, programmeID);

                programme_BEList = programme_DAO.programme_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return programme_BEList;
        }

        public List<Programme_BE> GetProgrammeList()
        {
            List<Programme_BE> programme_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                programme_DAO.GetProgrammeList();

                programme_BEList = programme_DAO.programme_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return programme_BEList;
        }

        public DataTable GetdtProgrammeList(string accountID)
        {
            DataTable dtProgramme = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                dtProgramme = programme_DAO.GetdtProgrammeList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProgramme;
        }

        public DataTable GetdtProgrammeListNew(string accountID)
        {
            DataTable dtProgramme = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                dtProgramme = programme_DAO.GetdtProgrammeListNew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProgramme;
        }


        public int GetProgrammeListCount(string accountID)
        {
            int programmeCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                programmeCount = programme_DAO.GetProgrammeListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return programmeCount;
        }

        #endregion


        public DataTable GetProjectProgramme(int projectID)
        {
            DataTable dtProgramme = null;

         //   try
         //   {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                dtProgramme = programme_DAO.GetProjectProgramme(projectID);

                //HandleWriteLog("End", new StackTrace(true));
          //  }
          //  catch (Exception ex)
          //  {
          //      HandleException(ex);
          //  }

            return dtProgramme;
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
                Programme_DAO programme_DAO = new Programme_DAO();
                return programme_DAO.GetProgramInstructions(programID);
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
                Programme_DAO programme_DAO = new Programme_DAO();
                return programme_DAO.GetProgramColleagueNumber(programID);
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
                Programme_DAO programme_DAO = new Programme_DAO();
                return programme_DAO.GetProgramReportDate(programID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }

        }

        public DataTable GetProgrammeByID(int programmeID)
        {
            DataTable dtProgramme = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                dtProgramme = programme_DAO.GetProgrammeByID(programmeID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProgramme;
        }

    }
}
