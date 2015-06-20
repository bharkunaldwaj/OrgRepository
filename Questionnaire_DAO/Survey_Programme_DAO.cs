using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Web;
using feedbackFramework_BE;
using feedbackFramework_DAO;
using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Survey_Programme_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_Programme_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Survey_Programme_BE> programme_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public SqlConnection conn;


        public int save_category_for_analysis_list(int prog_id, string Category_Detail_list, string Analysis_Type_list, string Category_Name_list)
        {
            //int rr=0;
            //try
            //{

            object[] param2 = new object[5] 
                 {
                    prog_id,
                    Category_Detail_list,
                    Analysis_Type_list,
                    Category_Name_list,
                    ','
                 };

            returnValue = Convert.ToInt32(cDataSrc.ExecuteNonQuery("Survey_UspProgrammeManagement_foreign", param2, null));

            cDataSrc = null;

            //}

            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            //finally
            //{
            //    //conn.Close();
            //}
            return returnValue;



        }
        //return 0;
        //}


        public DataTable Get_No_of_Analysis(int programme_ID)
        {
            DataTable dtResult = new DataTable();
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[1] { programme_ID };

            dtResult = cDataSrc.ExecuteDataSet("Survey_Get_no_of_Analysis", param, null).Tables[0];

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtResult;
        }


        public DataTable GetAnalysis1(int programme_ID)
        {
            DataTable dtResult = new DataTable();
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[1] { programme_ID };

            dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetProgramme_Analysis1", param, null).Tables[0];

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtResult;
        }



        public DataTable GetAnalysis2(int programme_ID)
        {
            DataTable dtResult = new DataTable();
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[1] { programme_ID };

            dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetProgramme_Analysis2", param, null).Tables[0];

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtResult;
        }




        public DataTable GetAnalysis3(int programme_ID)
        {
            DataTable dtResult = new DataTable();
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[1] { programme_ID };

            dtResult = cDataSrc.ExecuteDataSet("Survey_UspGetProgramme_Analysis3", param, null).Tables[0];

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtResult;
        }












        public int AddProgramme(Survey_Programme_BE programme_BE)
        {
            //////////try
            //////////{
            string connectionInfo = ConfigurationSettings.AppSettings["ConnectionInfo"];
            SqlConnection connection = new SqlConnection(connectionInfo);
            connection.Open();

            SqlCommand cmd = new SqlCommand("Survey_UspProgrammeManagement", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProgrammeID", programme_BE.ProgrammeID);
            cmd.Parameters.AddWithValue("@ProgrammeName", programme_BE.ProgrammeName);
            cmd.Parameters.AddWithValue("@ProgrammeDescription", programme_BE.ProgrammeDescription);
            cmd.Parameters.AddWithValue("@ClientName", programme_BE.ClientName);
            cmd.Parameters.AddWithValue("@Logo", programme_BE.Logo);
            cmd.Parameters.AddWithValue("@ProjectID", programme_BE.ProjectID);
            cmd.Parameters.AddWithValue("@CompanyID", programme_BE.CompanyID);
            cmd.Parameters.AddWithValue("@AccountID", programme_BE.AccountID);
            cmd.Parameters.AddWithValue("@StartDate", programme_BE.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", programme_BE.EndDate);
            cmd.Parameters.AddWithValue("@Reminder1Date", programme_BE.Reminder1Date);
            cmd.Parameters.AddWithValue("@Reminder2Date", programme_BE.Reminder2Date);
            cmd.Parameters.AddWithValue("@Reminder3Date", programme_BE.Reminder3Date);
            cmd.Parameters.AddWithValue("@ModifyBy", programme_BE.ModifyBy);
            cmd.Parameters.AddWithValue("@ModifyDate", programme_BE.ModifyDate);
            cmd.Parameters.AddWithValue("@IsActive", programme_BE.IsActive);

            cmd.Parameters.AddWithValue("@Analysis_I_Name", programme_BE.Analysis_I_Name);
            cmd.Parameters.AddWithValue("@Analysis_I_Category", Convert.ToInt32((programme_BE.Analysis_I_Category)));
            cmd.Parameters.AddWithValue("@Analysis_II_Name", programme_BE.Analysis_II_Name);
            cmd.Parameters.AddWithValue("@Analysis_II_Category", Convert.ToInt32(programme_BE.Analysis_II_Category));
            cmd.Parameters.AddWithValue("@Analysis_III_Name", programme_BE.Analysis_III_Name);
            cmd.Parameters.AddWithValue("@Analysis_III_Category", Convert.ToInt32(programme_BE.Analysis_III_Category));
            cmd.Parameters.AddWithValue("@Operation", "I");
            cmd.Parameters.Add("@prog_id", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            returnValue = (int)cmd.Parameters["@prog_id"].Value;



            ////////////////object[] param = new object[23] {null,
            ////////////////                                 programme_BE.ProgrammeName,
            ////////////////                                 programme_BE.ProgrammeDescription,
            ////////////////                                 programme_BE.ClientName,
            ////////////////                                 programme_BE.Logo,
            ////////////////                                 programme_BE.ProjectID,
            ////////////////                                 programme_BE.AccountID,
            ////////////////                                 programme_BE.StartDate,
            ////////////////                                 programme_BE.EndDate,
            ////////////////                                 programme_BE.Reminder1Date,
            ////////////////                                 programme_BE.Reminder2Date,
            ////////////////                                 programme_BE.Reminder3Date,
            ////////////////                                 programme_BE.ModifyBy,
            ////////////////                                 programme_BE.ModifyDate,
            ////////////////                                 programme_BE.IsActive,


            ////////////////                                 programme_BE.Analysis_I_Name,
            ////////////////                                 programme_BE.Analysis_I_Category,
            ////////////////                                 programme_BE.Analysis_II_Name,
            ////////////////                                 programme_BE.Analysis_II_Category,
            ////////////////                                 programme_BE.Analysis_III_Name,
            ////////////////                                 programme_BE.Analysis_III_Category,
            ////////////////                                 "I",
            ////////////////                                 programme_BE.prog_id
            ////////////////                                 };

            //////////////// returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspProgrammeManagement", param, null));

            //////////////// cDataSrc = null;

            //HandleWriteLog("End", new StackTrace(true));
            ////////}
            ////////catch (Exception ex) 
            ////////{ 
            ////////    HandleException(ex); 
            ////////}
            return returnValue;
        }

        public int UpdateProgramme(Survey_Programme_BE programme_BE)
        {
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[23] {programme_BE.ProgrammeID,
                                                programme_BE.ProgrammeName,
                                                programme_BE.ProgrammeDescription,
                                                programme_BE.ClientName,
                                                programme_BE.Logo,
                                                programme_BE.ProjectID,
                                                programme_BE.CompanyID,
                                                programme_BE.AccountID,
                                                programme_BE.StartDate,
                                                programme_BE.EndDate,
                                                programme_BE.Reminder1Date,
                                                programme_BE.Reminder2Date,
                                                programme_BE.Reminder3Date,
                                                programme_BE.ModifyBy,
                                                programme_BE.ModifyDate,
                                                programme_BE.IsActive,
                                                programme_BE.Analysis_I_Name,
                                                programme_BE.Analysis_I_Category,
                                                programme_BE.Analysis_II_Name,
                                                programme_BE.Analysis_II_Category,
                                                programme_BE.Analysis_III_Name,
                                                programme_BE.Analysis_III_Category,
                                                "U" };

            returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspProgrammeManagement", param, null));

            cDataSrc = null;

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return returnValue;
        }

        public int DeleteProgramme(Survey_Programme_BE programme_BE)
        {
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[23] {programme_BE.ProgrammeID,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,

                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null,
                                                "D" };

            returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspProgrammeManagement", param, null));

            cDataSrc = null;

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            return returnValue;
        }

        public void GetProgrammeByID(int accountID, int programmeID)
        {
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            DataTable dtAlluser = new DataTable();
            object[] param = new object[3] { programmeID, accountID, "I" };

            dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspProgrammeSelect", param, null).Tables[0];

            ShiftDataTableToBEList(dtAlluser);

            //HandleWriteLogDAU("UspProgrammeSelect", param, new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }

        }

        private void ShiftDataTableToBEList(DataTable dtProgramme)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            programme_BEList = new List<Survey_Programme_BE>();

            for (int recordCounter = 0; recordCounter < dtProgramme.Rows.Count; recordCounter++)
            {
                Survey_Programme_BE programme_BE = new Survey_Programme_BE();

                programme_BE.ProgrammeID = Convert.ToInt32(dtProgramme.Rows[recordCounter]["ProgrammeID"].ToString());
                programme_BE.AccountID = Convert.ToInt32(dtProgramme.Rows[recordCounter]["AccountID"].ToString());
                programme_BE.ProgrammeName = dtProgramme.Rows[recordCounter]["ProgrammeName"].ToString();
                programme_BE.ProgrammeDescription = dtProgramme.Rows[recordCounter]["ProgrammeDescription"].ToString();
                programme_BE.ClientName = dtProgramme.Rows[recordCounter]["ClientName"].ToString();
                programme_BE.Logo = dtProgramme.Rows[recordCounter]["Logo"].ToString();
                if (!string.IsNullOrEmpty(Convert.ToString(dtProgramme.Rows[recordCounter]["ProjectID"])))
                    programme_BE.ProjectID = Convert.ToInt32(Convert.ToString(dtProgramme.Rows[recordCounter]["ProjectID"]));
                if (!string.IsNullOrEmpty(Convert.ToString(dtProgramme.Rows[recordCounter]["CompanyID"])))
                    programme_BE.CompanyID = Convert.ToInt32(dtProgramme.Rows[recordCounter]["CompanyID"].ToString());
                programme_BE.StartDate = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["StartDate"].ToString());
                programme_BE.EndDate = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["EndDate"].ToString());
                programme_BE.Reminder1Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["Reminder1Date"].ToString());
                programme_BE.Reminder2Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["Reminder2Date"].ToString());
                programme_BE.Reminder3Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["Reminder3Date"].ToString());




                programme_BE.ModifyBy = Convert.ToInt32(dtProgramme.Rows[recordCounter]["ModifyBy"].ToString());
                programme_BE.ModifyDate = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["ModifyDate"].ToString());
                programme_BE.IsActive = Convert.ToInt32(dtProgramme.Rows[recordCounter]["IsActive"].ToString());



                //   if (dtProgramme.Rows[recordCounter]["Analysis_I_Name"].ToString() DBNull)
                programme_BE.Analysis_I_Name = dtProgramme.Rows[recordCounter]["Analysis_I_Name"].ToString();
                if (dtProgramme.Rows[recordCounter]["Analysis_I_Category"] == DBNull.Value)
                { }
                else
                    programme_BE.Analysis_I_Category = Convert.ToInt32(dtProgramme.Rows[recordCounter]["Analysis_I_Category"]);

                programme_BE.Analysis_II_Name = dtProgramme.Rows[recordCounter]["Analysis_II_Name"].ToString();
                if (dtProgramme.Rows[recordCounter]["Analysis_II_Category"] == DBNull.Value)
                { }
                else
                    programme_BE.Analysis_II_Category = Convert.ToInt32(dtProgramme.Rows[recordCounter]["Analysis_II_Category"]);

                programme_BE.Analysis_III_Name = dtProgramme.Rows[recordCounter]["Analysis_III_Name"].ToString();
                if (dtProgramme.Rows[recordCounter]["Analysis_III_Category"] == DBNull.Value)
                { }
                else
                    programme_BE.Analysis_III_Category = Convert.ToInt32(dtProgramme.Rows[recordCounter]["Analysis_III_Category"]);


                programme_BEList.Add(programme_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public void GetProgrammeList()
        {
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            DataTable dtAlluser = new DataTable();
            object[] param = new object[2] { null, "A" };

            dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspProgrammeSelect", param, null).Tables[0];

            ShiftDataTableToBEList(dtAlluser);
            returnValue = 1;

            //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }

        }

        public DataTable GetdtProgrammeList(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

            dtAllProject = cDataSrc.ExecuteDataSet("Survey_UspProgrammeSelect", param, null).Tables[0];

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }

        public DataTable GetdtProgrammeListNew(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            //try
            //{
            string sql = "SELECT [ProgrammeID] " +
                        ",[Survey_Analysis_Sheet].[ProgrammeName] " +
                        ",[Survey_Analysis_Sheet].[ProgrammeDescription] " +
                        ",[Survey_Analysis_Sheet].[ClientName] " +
                        ",[Survey_Analysis_Sheet].[Logo] " +
                        ",[Survey_Analysis_Sheet].[ProjectID] " +
                        ",[Survey_Analysis_Sheet].[CompanyID] " +
                        ",[Survey_Company].[Title] as CompanyTitle " +
                        ",[Survey_Analysis_Sheet].[AccountID] " +
                        ",[Survey_Analysis_Sheet].[StartDate] " +
                        ",[Survey_Analysis_Sheet].[EndDate] " +
                        ",[Survey_Analysis_Sheet].[Reminder1Date] " +
                        ",[Survey_Analysis_Sheet].[Reminder2Date] " +
                        ",[Survey_Analysis_Sheet].[Reminder3Date] " +



      ",[Survey_Analysis_Sheet].[Analysis_I_Name] " +
      ",[Survey_Analysis_Sheet].[Analysis_I_Category] " +
      ",[Survey_Analysis_Sheet].[Analysis_II_Name] " +
      ",[Survey_Analysis_Sheet].[Analysis_II_Category] " +
      ",[Survey_Analysis_Sheet].[Analysis_III_Name] " +
      ",[Survey_Analysis_Sheet].[Analysis_III_Category] " +



                        ",[Survey_Analysis_Sheet].[ModifyBy] " +
                        ",[Survey_Analysis_Sheet].[ModifyDate] " +
                        ",[Survey_Analysis_Sheet].[IsActive] " +
                        ",[Survey_Project].Title " +
                        ",[Account].Code " +
                        "FROM [Survey_Analysis_Sheet] INNER JOIN " +
                        "dbo.Account ON [Survey_Analysis_Sheet].AccountID = dbo.Account.AccountID " +
                        "INNER JOIN [Survey_Project] on [Survey_Project].[ProjectID]=[Survey_Analysis_Sheet].[ProjectID] " +
                        "left JOIN [Survey_Company] on [Survey_Analysis_Sheet].[CompanyID]=[Survey_Company].[CompanyID] " +
                        "WHERE [Survey_Analysis_Sheet].[AccountID] = " + accountID +
                        " ORDER BY dbo.Survey_Analysis_Sheet.[ProgrammeName] ";

            dtAllProject = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            //}
            //catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }

        public int GetProgrammeListCount(string accountID)
        {
            int projectCount = 0;
            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));
            //object[] param = new object[3] { null, Convert.ToInt32(accountID), "C" };
            //projectCount = (int)cDataSrc.ExecuteScalar("UspProgrammeSelect", param, null);

            string sql = "SELECT Count([ProgrammeID]) " +
                        "FROM [Survey_Analysis_Sheet] INNER JOIN " +
                        "dbo.Account ON [Survey_Analysis_Sheet].AccountID = dbo.Account.AccountID " +
                        "INNER JOIN [Survey_Project] on [Survey_Project].[ProjectID]=[Survey_Analysis_Sheet].[ProjectID] " +
                        "WHERE [Survey_Analysis_Sheet].[AccountID] = " + accountID;

            projectCount = (int)cDataSrc.ExecuteScalar(sql, null);
            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex) { HandleException(ex); }
            return projectCount;
        }

        #endregion



        public DataTable GetProjectProgramme(int projectID, int companyId, int programmeId = 0)
        {
            DataTable dtProjectProgramme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            dtProjectProgramme = cDataSrc.ExecuteDataSet("SELECT ProgrammeID, ProgrammeName,Analysis_I_Name,Analysis_II_Name,Analysis_III_Name FROM Survey_Analysis_Sheet WHERE ProjectID = " + projectID + "and (" + companyId + "= 0 or CompanyId = " + companyId + ")" + "and (" + programmeId + "= 0 or ProgrammeID = " + programmeId + ")", null).Tables[0];

            //HandleWriteLog("End", new StackTrace(true));
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            return dtProjectProgramme;
        }

        public DataTable GetProgrammeByID(int programmeID)
        {
            DataTable dtProgramme = new DataTable();

            //try
            //{
            //HandleWriteLog("Start", new StackTrace(true));

            dtProgramme = cDataSrc.ExecuteDataSet("SELECT * FROM Survey_Analysis_Sheet WHERE ProgrammeID = " + programmeID, null).Tables[0];

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

