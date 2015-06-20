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
    public class Survey_Programme_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addProgramme;
        private int searchProgramme;


        #endregion

        #region CRUD Operations

        public int AddProgramme(Survey_Programme_BE programme_BE)
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
                    Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                addProgramme = programme_DAO.AddProgramme(programme_BE);
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

        public int UpdateProgramme(Survey_Programme_BE programme_BE)
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
                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                addProgramme = programme_DAO.UpdateProgramme(programme_BE);
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



        public DataTable No_of_Analysis(int programme_ID)
        {
            DataTable No_of_Analysis = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            No_of_Analysis = programme_DAO.Get_No_of_Analysis(programme_ID);
            return No_of_Analysis;

        }


        public DataTable GetAnalysis1(int programme_ID)
        {
            DataTable programme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            programme = programme_DAO.GetAnalysis1(programme_ID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme;
        }




        public DataTable GetAnalysis2(int programme_ID)
        {
            DataTable programme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            programme = programme_DAO.GetAnalysis2(programme_ID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme;
        }






        public DataTable GetAnalysis3(int programme_ID)
        {
            DataTable programme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
            //     Survey_Project_DAO project_DAO = new Survey_Project_DAO();
            programme = programme_DAO.GetAnalysis3(programme_ID);

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme;
        }








        public int DeleteProgramme(Survey_Programme_BE programme_BE)
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
                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                addProgramme = programme_DAO.DeleteProgramme(programme_BE);
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

        public List<Survey_Programme_BE> GetProgrammeByID(int accountID, int programmeID)
        {
            List<Survey_Programme_BE> programme_BEList = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                programme_DAO.GetProgrammeByID(accountID, programmeID);

                programme_BEList = programme_DAO.programme_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme_BEList;
        }

        public List<Survey_Programme_BE> GetProgrammeList()
        {
            List<Survey_Programme_BE> programme_BEList = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                programme_DAO.GetProgrammeList();

                programme_BEList = programme_DAO.programme_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return programme_BEList;
        }

        public DataTable GetdtProgrammeList(string accountID)
        {
            DataTable dtProgramme = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                dtProgramme = programme_DAO.GetdtProgrammeList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtProgramme;
        }

        public DataTable GetdtProgrammeListNew(string accountID)
        {
            DataTable dtProgramme = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                dtProgramme = programme_DAO.GetdtProgrammeListNew(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtProgramme;
        }


        public int GetProgrammeListCount(string accountID)
        {
            int programmeCount = 0;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                programmeCount = programme_DAO.GetProgrammeListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return programmeCount;
        }

        #endregion







        public int save_category_for_analysis_list(int prog_id, string Category_Detail_list, string Analysis_Type_list, string Category_Name_list)
        {
            int rr=0;
            ////////   try
            ////////{
                  Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
             rr= programme_DAO.save_category_for_analysis_list(prog_id,Category_Detail_list, Analysis_Type_list,Category_Name_list);

                //HandleWriteLog("End", new StackTrace(true));
            ////////}
            ////////   catch (Exception ex)
            ////////   {
            ////////       HandleException(ex);
            ////////   }

              return rr;
        }


















        public DataTable GetProjectProgramme(int projectID,int companyId=0,int programmeId=0)
        {
            DataTable dtProgramme = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                dtProgramme = programme_DAO.GetProjectProgramme(projectID,companyId,programmeId);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtProgramme;
        }

        public DataTable GetProgrammeByID(int programmeID)
        {
            DataTable dtProgramme = null;

            //try
            //{
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_Programme_DAO programme_DAO = new Survey_Programme_DAO();
                dtProgramme = programme_DAO.GetProgrammeByID(programmeID);

                //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtProgramme;
        }

    }
}
