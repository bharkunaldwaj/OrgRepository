using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
using feedbackFramework_DAO;

using Admin_BE;
using DatabaseAccessUtilities;

namespace Admin_DAO
{
    public class ReminderEmailHistory_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        private int returnValue;
        public List<ReminderEmailHistory_BE> reminderEmailHistory_BEList { get; set; }

        public int GetReminderEmailHistoryList(string sql)
        {
            try
            {
                DataTable dtReminderEmailHistory = new DataTable();
                dtReminderEmailHistory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
                ShiftDataTableToBEList(dtReminderEmailHistory);
                returnValue = 1;
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dtReminderEmailHistory = new DataTable();
            try
            {
                dtReminderEmailHistory= cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }
            return dtReminderEmailHistory;
        }

        private void ShiftDataTableToBEList(DataTable dtReminderEmailHistory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            reminderEmailHistory_BEList = new List<ReminderEmailHistory_BE>();

            for (int recordCounter = 0; recordCounter < dtReminderEmailHistory.Rows.Count; recordCounter++)
            {
                ReminderEmailHistory_BE reminderEmailHistory_BE = new ReminderEmailHistory_BE();

                reminderEmailHistory_BE.RemId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["RemId"].ToString());
                reminderEmailHistory_BE.Type = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["Type"].ToString());
                reminderEmailHistory_BE.AccountId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["AccountId"].ToString());
                reminderEmailHistory_BE.AccountName = dtReminderEmailHistory.Rows[recordCounter]["AccountName"].ToString();
                reminderEmailHistory_BE.ParticipantId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["ParticipantId"].ToString());
                reminderEmailHistory_BE.ParticipantName = dtReminderEmailHistory.Rows[recordCounter]["ParticipantName"].ToString();
                reminderEmailHistory_BE.CandidateId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["CandidateId"].ToString());
                reminderEmailHistory_BE.CandidateName = dtReminderEmailHistory.Rows[recordCounter]["CandidateName"].ToString();
                reminderEmailHistory_BE.ProjectId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["ProjectId"].ToString());
                reminderEmailHistory_BE.ProjectName = dtReminderEmailHistory.Rows[recordCounter]["ProjectName"].ToString();
                reminderEmailHistory_BE.ProgrammeId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["ProgrammeId"].ToString());
                reminderEmailHistory_BE.ProgrammeName = dtReminderEmailHistory.Rows[recordCounter]["ProgrammeName"].ToString();
                reminderEmailHistory_BE.EmailDate = Convert.ToDateTime(dtReminderEmailHistory.Rows[recordCounter]["EmailDate"].ToString());
                reminderEmailHistory_BE.EmailStatus = Convert.ToBoolean(dtReminderEmailHistory.Rows[recordCounter]["EmailStatus"].ToString());
                
                reminderEmailHistory_BEList.Add(reminderEmailHistory_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

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
        public List<Survey_ReminderEmailHistory_BE> reminderEmailHistory_BEList { get; set; }

        public int GetReminderEmailHistoryList(string sql)
        {
            try
            {
                DataTable dtReminderEmailHistory = new DataTable();
                dtReminderEmailHistory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
                ShiftDataTableToBEList(dtReminderEmailHistory);
                returnValue = 1;
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dtReminderEmailHistory = new DataTable();
            try
            {
                dtReminderEmailHistory = cDataSrc.ExecuteDataSet(sql, null).Tables[0];
            }
            catch (Exception ex) { HandleException(ex); }
            return dtReminderEmailHistory;
        }

        private void ShiftDataTableToBEList(DataTable dtReminderEmailHistory)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            reminderEmailHistory_BEList = new List<Survey_ReminderEmailHistory_BE>();

            for (int recordCounter = 0; recordCounter < dtReminderEmailHistory.Rows.Count; recordCounter++)
            {
                Survey_ReminderEmailHistory_BE reminderEmailHistory_BE = new Survey_ReminderEmailHistory_BE();

                reminderEmailHistory_BE.RemId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["RemId"].ToString());
                reminderEmailHistory_BE.Type = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["Type"].ToString());
                reminderEmailHistory_BE.AccountId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["AccountId"].ToString());
                reminderEmailHistory_BE.AccountName = dtReminderEmailHistory.Rows[recordCounter]["AccountName"].ToString();
                reminderEmailHistory_BE.ParticipantId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["ParticipantId"].ToString());
                reminderEmailHistory_BE.ParticipantName = dtReminderEmailHistory.Rows[recordCounter]["ParticipantName"].ToString();
                reminderEmailHistory_BE.CandidateId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["CandidateId"].ToString());
                reminderEmailHistory_BE.CandidateName = dtReminderEmailHistory.Rows[recordCounter]["CandidateName"].ToString();
                reminderEmailHistory_BE.ProjectId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["ProjectId"].ToString());
                reminderEmailHistory_BE.ProjectName = dtReminderEmailHistory.Rows[recordCounter]["ProjectName"].ToString();
                reminderEmailHistory_BE.ProgrammeId = Convert.ToInt32(dtReminderEmailHistory.Rows[recordCounter]["ProgrammeId"].ToString());
                reminderEmailHistory_BE.ProgrammeName = dtReminderEmailHistory.Rows[recordCounter]["ProgrammeName"].ToString();
                reminderEmailHistory_BE.EmailDate = Convert.ToDateTime(dtReminderEmailHistory.Rows[recordCounter]["EmailDate"].ToString());
                reminderEmailHistory_BE.EmailStatus = Convert.ToBoolean(dtReminderEmailHistory.Rows[recordCounter]["EmailStatus"].ToString());

                reminderEmailHistory_BEList.Add(reminderEmailHistory_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

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
