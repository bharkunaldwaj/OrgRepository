using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class Programme_BAO //:Base_BAO
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

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Programme_DAO programme_DAO = new Programme_DAO();
                dtProgramme = programme_DAO.GetProjectProgramme(projectID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProgramme;
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
