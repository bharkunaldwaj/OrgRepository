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
    public class Programme_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionInfo"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Programme_DAO() 
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        
        #endregion

        #region "Public Properties"

        public List<Programme_BE> programme_BEList { get; set; }
        
        #endregion

        # region CRUD Operation

        public int AddProgramme(Programme_BE programme_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[23] {null,
                                                programme_BE.ProgrammeName,
                                                programme_BE.ProgrammeDescription,
                                                programme_BE.ClientName,
                                                programme_BE.Logo,
                                                programme_BE.ProjectID,
                                                programme_BE.AccountID,
                                                programme_BE.StartDate,
                                                programme_BE.EndDate,
                                                programme_BE.Reminder1Date,
                                                programme_BE.Reminder2Date,
                                                programme_BE.Reminder3Date,
                                                programme_BE.ReportAvaliableFrom,
                                                programme_BE.ReportAvaliableTo,
                                                programme_BE.PartReminder1Date,
                                                programme_BE.PartReminder2Date,
                                                programme_BE.ModifyBy,
                                                programme_BE.ModifyDate,
                                                programme_BE.IsActive,
                                                programme_BE.Instructions,
                                                programme_BE.ColleagueNo,
                                                "I",
                                                programme_BE.ReportTopLogo
                                };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProgrammeManagement", param, null));

                cDataSrc = null;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) 
            { 
                HandleException(ex); 
            }
            return returnValue;
        }

        public int UpdateProgramme(Programme_BE programme_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[23] {programme_BE.ProgrammeID,
                                                programme_BE.ProgrammeName,
                                                programme_BE.ProgrammeDescription,
                                                programme_BE.ClientName,
                                                programme_BE.Logo,
                                                programme_BE.ProjectID,
                                                programme_BE.AccountID,
                                                programme_BE.StartDate,
                                                programme_BE.EndDate,
                                                programme_BE.Reminder1Date,
                                                programme_BE.Reminder2Date,
                                                programme_BE.Reminder3Date,
                                                programme_BE.ReportAvaliableFrom,
                                                programme_BE.ReportAvaliableTo,
                                                programme_BE.PartReminder1Date,
                                                programme_BE.PartReminder2Date,
                                                programme_BE.ModifyBy,
                                                programme_BE.ModifyDate,
                                                programme_BE.IsActive,
                                                programme_BE.Instructions,
                                                programme_BE.ColleagueNo,
                                                "U",
                                                programme_BE.ReportTopLogo
                                };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProgrammeManagement", param, null));

                cDataSrc = null;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return returnValue;
        }

        public int DeleteProgramme(Programme_BE programme_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[20] {programme_BE.ProgrammeID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspProgrammeManagement", param, null));

                cDataSrc = null;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return returnValue;
        }

        public void GetProgrammeByID(int accountID, int programmeID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[3] { programmeID, accountID, "I" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspProgrammeSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);

                //HandleWriteLogDAU("UspProgrammeSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            
        }

        private void ShiftDataTableToBEList(DataTable dtProgramme)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            programme_BEList = new List<Programme_BE>();

            for (int recordCounter = 0; recordCounter < dtProgramme.Rows.Count; recordCounter++)
            {
                Programme_BE programme_BE = new Programme_BE();

                programme_BE.ProgrammeID = Convert.ToInt32(dtProgramme.Rows[recordCounter]["ProgrammeID"].ToString());
                programme_BE.AccountID = Convert.ToInt32(dtProgramme.Rows[recordCounter]["AccountID"].ToString());
                programme_BE.ProgrammeName = dtProgramme.Rows[recordCounter]["ProgrammeName"].ToString();
                programme_BE.ProgrammeDescription = dtProgramme.Rows[recordCounter]["ProgrammeDescription"].ToString();
                programme_BE.ClientName = dtProgramme.Rows[recordCounter]["ClientName"].ToString();
                programme_BE.Logo = dtProgramme.Rows[recordCounter]["Logo"].ToString();
                programme_BE.ProjectID = Convert.ToInt32(dtProgramme.Rows[recordCounter]["ProjectID"].ToString());
                programme_BE.StartDate = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["StartDate"].ToString());
                programme_BE.EndDate = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["EndDate"].ToString());
                programme_BE.Reminder1Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["Reminder1Date"].ToString());
                programme_BE.Reminder2Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["Reminder2Date"].ToString());
                programme_BE.Reminder3Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["Reminder3Date"].ToString());
                programme_BE.ReportAvaliableFrom = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["ReportAvaliableFrom"].ToString());
                programme_BE.ReportAvaliableTo = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["ReportAvaliableTo"].ToString());
                programme_BE.PartReminder1Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["PartReminder1Date"].ToString());
                programme_BE.PartReminder2Date = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["PartReminder2Date"].ToString());
                programme_BE.ModifyBy = Convert.ToInt32(dtProgramme.Rows[recordCounter]["ModifyBy"].ToString());
                programme_BE.ModifyDate = Convert.ToDateTime(dtProgramme.Rows[recordCounter]["ModifyDate"].ToString());
                programme_BE.IsActive = Convert.ToInt32(dtProgramme.Rows[recordCounter]["IsActive"].ToString());
                programme_BE.Instructions = dtProgramme.Rows[recordCounter]["Instructions"].ToString();
                programme_BE.ReportTopLogo = dtProgramme.Rows[recordCounter]["ReportLogo"].ToString();
                if (!string.IsNullOrEmpty(dtProgramme.Rows[recordCounter]["ColleagueNo"].ToString().Trim()))
                    programme_BE.ColleagueNo = Convert.ToInt32(dtProgramme.Rows[recordCounter]["ColleagueNo"].ToString());
                else
                    programme_BE.ColleagueNo = 0;
                programme_BEList.Add(programme_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        public void GetProgrammeList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspProgrammeSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                //HandleWriteLogDAU("UspProjectSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            
        }

        public DataTable GetdtProgrammeList(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

                dtAllProject = cDataSrc.ExecuteDataSet("UspProgrammeSelect", param, null).Tables[0];

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }

        public DataTable GetdtProgrammeListNew(string accountID)
        {
            DataTable dtAllProject = new DataTable();
            try
            {
                string sql = "SELECT [ProgrammeID] " +
                            ",[Programme].[ProgrammeName] " +
                            ",[Programme].[ProgrammeDescription] " +
                            ",[Programme].[ClientName] " +
                            ",[Programme].[Logo] " +
                            ",[Programme].[ProjectID] " +
                            ",[Programme].[AccountID] " +
                            ",[Programme].[StartDate] " +
                            ",[Programme].[EndDate] " +
                            ",[Programme].[Reminder1Date] " +
                            ",[Programme].[Reminder2Date] " +
                            ",[Programme].[Reminder3Date] " +
                            ",[Programme].[ReportAvaliableFrom] " +
                            ",[Programme].[ReportAvaliableTo] " +
                            ",[Programme].[PartReminder1Date] " +
                            ",[Programme].[PartReminder2Date] " +
                            ",[Programme].[ModifyBy] " +
                            ",[Programme].[ModifyDate] " +
                            ",[Programme].[IsActive] " +
                            ",[Project].Title " +
                            ",[Account].Code " +
                            "FROM [Programme] INNER JOIN " +
                            "dbo.Account ON [Programme].AccountID = dbo.Account.AccountID " +
                            "INNER JOIN [Project] on [Project].[ProjectID]=[Programme].[ProjectID] " +
                            "WHERE [Programme].[AccountID] = " + accountID +
                            " ORDER BY dbo.Programme.[ProgrammeName] ";

                DataSet ds = cDataSrc.ExecuteDataSet(sql, null);
                dtAllProject = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllProject;
        }

        public int GetProgrammeListCount(string accountID)
        {
            int projectCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //object[] param = new object[3] { null, Convert.ToInt32(accountID), "C" };
                //projectCount = (int)cDataSrc.ExecuteScalar("UspProgrammeSelect", param, null);

                string sql = "SELECT Count([ProgrammeID]) " +
                            "FROM [Programme] INNER JOIN " +
                            "dbo.Account ON [Programme].AccountID = dbo.Account.AccountID " +
                            "INNER JOIN [Project] on [Project].[ProjectID]=[Programme].[ProjectID] " +
                            "WHERE [Programme].[AccountID] = " + accountID ;

                projectCount = (int)cDataSrc.ExecuteScalar(sql, null);
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return projectCount;
        }

        #endregion


       


        public int Programme_save_analysis()
        {
            //Programme_BE pb = (Programme_BE) HttpContext.Current.Session["aa"];

         //   object[] param = new object[4]
         //{
         //   // pb.ProgrammeID,
            
        
         //};

          //  int returnValue = Convert.ToInt32(cDataSrc.ExecuteNonQuery("insert_range", param, null));
            return returnValue;
            cDataSrc = null;

            return 0;
        
        }


        public DataTable GetProjectProgramme(int projectID)
        {
            DataTable dtProjectProgramme = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                dtProjectProgramme = cDataSrc.ExecuteDataSet("SELECT ProgrammeID, ProgrammeName FROM Programme WHERE ProjectID = " + projectID, null).Tables[0];

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtProjectProgramme;
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
            string strprogrammeInstruction = string.Empty;
            try
            {
                object[] param = new object[1] { programID };

                strprogrammeInstruction = cDataSrc.ExecuteScalar("UspProgrammeInstructionSelect", param, null).ToString();                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return strprogrammeInstruction;
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
            string strprogrammeInstruction = string.Empty;
            try
            {
                object[] param = new object[1] { programID };

                strprogrammeInstruction = cDataSrc.ExecuteScalar("UspProgrammeColleagueNumberSelect", param, null).ToString();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return strprogrammeInstruction;
        }

        /// <summary>
        /// Returns report available and end date of a programme        
        /// </summary>
        /// <param name="programID"></param>
        /// <returns></returns>
        public DateTime[] GetProgramReportDate(int programID)
        {
            //string strprogrammeInstruction = string.Empty;
            DateTime[] dtReport = new DateTime[2] { DateTime.MinValue, DateTime.MinValue };
            try
            {
                object[] param = new object[1] { programID };

                DataSet ds = cDataSrc.ExecuteDataSet("UspProgrammeReportDateSelect", param, null);

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dtReport[0] = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReportAvaliableFrom"]);
                        dtReport[1] = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReportAvaliableTo"]);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtReport;
        }


        public DataTable GetProgrammeByID(int programmeID)
        {
            DataTable dtProgramme = new DataTable();

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                dtProgramme = cDataSrc.ExecuteDataSet("SELECT * FROM Programme WHERE ProgrammeID = " + programmeID, null).Tables[0];

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

