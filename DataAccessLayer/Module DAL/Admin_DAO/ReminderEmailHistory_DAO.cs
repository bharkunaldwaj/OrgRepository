using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

using feedbackFramework_DAO;

using Admin_BE;
using DatabaseAccessUtilities;

namespace Admin_DAO
{
    public class ReminderEmailHistory_DAO : DAO_Base
    {
        //Global variables
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        private int returnValue;
        public List<ReminderEmailHistory_BE> reminderEmailHistoryBusinessEntityList { get; set; }

        /// <summary>
        /// Get Reminder Email History List
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetReminderEmailHistoryList(string sql)
        {
            try
            {
                DataTable dataTableReminderEmailHistory = new DataTable();
                dataTableReminderEmailHistory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
                ShiftDataTableToBEList(dataTableReminderEmailHistory);
                returnValue = 1;
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Get Reminder Email History List
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dataTableReminderEmailHistory = new DataTable();
            try
            {
                dataTableReminderEmailHistory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableReminderEmailHistory;
        }

        /// <summary>
        /// Shift Reminder Email History Data Table To BEList
        /// </summary>
        /// <param name="dataTableReminderEmailHistory"></param>
        private void ShiftDataTableToBEList(DataTable dataTableReminderEmailHistory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            reminderEmailHistoryBusinessEntityList = new List<ReminderEmailHistory_BE>();

            for (int recordCounter = 0; recordCounter < dataTableReminderEmailHistory.Rows.Count; recordCounter++)
            {
                ReminderEmailHistory_BE reminderEmailHistory_BE = new ReminderEmailHistory_BE();

                reminderEmailHistory_BE.RemId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["RemId"].ToString());
                reminderEmailHistory_BE.Type = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["Type"].ToString());
                reminderEmailHistory_BE.AccountId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["AccountId"].ToString());
                reminderEmailHistory_BE.AccountName = dataTableReminderEmailHistory.Rows[recordCounter]["AccountName"].ToString();
                reminderEmailHistory_BE.ParticipantId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["ParticipantId"].ToString());
                reminderEmailHistory_BE.ParticipantName = dataTableReminderEmailHistory.Rows[recordCounter]["ParticipantName"].ToString();
                reminderEmailHistory_BE.CandidateId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["CandidateId"].ToString());
                reminderEmailHistory_BE.CandidateName = dataTableReminderEmailHistory.Rows[recordCounter]["CandidateName"].ToString();
                reminderEmailHistory_BE.ProjectId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["ProjectId"].ToString());
                reminderEmailHistory_BE.ProjectName = dataTableReminderEmailHistory.Rows[recordCounter]["ProjectName"].ToString();
                reminderEmailHistory_BE.ProgrammeId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["ProgrammeId"].ToString());
                reminderEmailHistory_BE.ProgrammeName = dataTableReminderEmailHistory.Rows[recordCounter]["ProgrammeName"].ToString();
                reminderEmailHistory_BE.EmailDate = Convert.ToDateTime(dataTableReminderEmailHistory.Rows[recordCounter]["EmailDate"].ToString());
                reminderEmailHistory_BE.EmailStatus = Convert.ToBoolean(dataTableReminderEmailHistory.Rows[recordCounter]["EmailStatus"].ToString());

                reminderEmailHistoryBusinessEntityList.Add(reminderEmailHistory_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Get Reminder Email History List Count
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetReminderEmailHistoryListCount(string sql)
        {
            int reminderEmailHistoryListCount = 0;
            try
            {
                reminderEmailHistoryListCount = (int)cDataSrc.ExecuteScalar(sql, null);
            }
            catch (Exception ex) { HandleException(ex); }
            return reminderEmailHistoryListCount;
        }
    }


    public class Survey_ReminderEmailHistory_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        private int returnValue;
        public List<Survey_ReminderEmailHistory_BE> reminderEmailHistoryBusinessEntityList { get; set; }

        /// <summary>
        /// Get reminder email history details
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetReminderEmailHistoryList(string sql)
        {
            try
            {
                DataTable dataTableReminderEmailHistory = new DataTable();
                dataTableReminderEmailHistory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
                ShiftDataTableToBEList(dataTableReminderEmailHistory);
                returnValue = 1;
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// get reminder email list
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dataTableReminderEmailHistory = new DataTable();
            try
            {
                dataTableReminderEmailHistory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableReminderEmailHistory;
        }

        /// <summary>
        /// Move dataTable to Reminder business entity object
        /// </summary>
        /// <param name="dataTableReminderEmailHistory"></param>
        private void ShiftDataTableToBEList(DataTable dataTableReminderEmailHistory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            reminderEmailHistoryBusinessEntityList = new List<Survey_ReminderEmailHistory_BE>();

            for (int recordCounter = 0; recordCounter < dataTableReminderEmailHistory.Rows.Count; recordCounter++)
            {
                Survey_ReminderEmailHistory_BE reminderEmailHistory_BE = new Survey_ReminderEmailHistory_BE();

                reminderEmailHistory_BE.RemId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["RemId"].ToString());
                reminderEmailHistory_BE.Type = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["Type"].ToString());
                reminderEmailHistory_BE.AccountId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["AccountId"].ToString());
                reminderEmailHistory_BE.AccountName = dataTableReminderEmailHistory.Rows[recordCounter]["AccountName"].ToString();
                reminderEmailHistory_BE.ParticipantId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["ParticipantId"].ToString());
                reminderEmailHistory_BE.ParticipantName = dataTableReminderEmailHistory.Rows[recordCounter]["ParticipantName"].ToString();
                reminderEmailHistory_BE.CandidateId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["CandidateId"].ToString());
                reminderEmailHistory_BE.CandidateName = dataTableReminderEmailHistory.Rows[recordCounter]["CandidateName"].ToString();
                reminderEmailHistory_BE.ProjectId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["ProjectId"].ToString());
                reminderEmailHistory_BE.ProjectName = dataTableReminderEmailHistory.Rows[recordCounter]["ProjectName"].ToString();
                reminderEmailHistory_BE.ProgrammeId = Convert.ToInt32(dataTableReminderEmailHistory.Rows[recordCounter]["ProgrammeId"].ToString());
                reminderEmailHistory_BE.ProgrammeName = dataTableReminderEmailHistory.Rows[recordCounter]["ProgrammeName"].ToString();
                reminderEmailHistory_BE.EmailDate = Convert.ToDateTime(dataTableReminderEmailHistory.Rows[recordCounter]["EmailDate"].ToString());
                reminderEmailHistory_BE.EmailStatus = Convert.ToBoolean(dataTableReminderEmailHistory.Rows[recordCounter]["EmailStatus"].ToString());

                reminderEmailHistoryBusinessEntityList.Add(reminderEmailHistory_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Get reminder history count
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetReminderEmailHistoryListCount(string sql)
        {
            int reminderEmailHistoryListCount = 0;
            try
            {
                reminderEmailHistoryListCount = (int)cDataSrc.ExecuteScalar(sql, null);
            }
            catch (Exception ex) { HandleException(ex); }
            return reminderEmailHistoryListCount;
        }
    }
}
