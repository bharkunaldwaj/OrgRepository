﻿using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class Programme_DAO
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        
        private int returnValue;
        
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Programme_DAO() 
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
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

                object[] param = new object[20] {null,
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
                                                "I" };

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

                object[] param = new object[20] {programme_BE.ProgrammeID,
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
                                                "U" };

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

