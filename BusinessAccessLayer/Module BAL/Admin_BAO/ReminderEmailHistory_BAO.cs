using System;
using System.Collections.Generic;
using DAF_BAO;
using Admin_BE;
using Admin_DAO;
using System.Data;

namespace Admin_BAO
{
    public class ReminderEmailHistory_BAO : Base_BAO
    {
        /// <summary>
        /// Get Reminder Email History List
        /// </summary>
        public List<ReminderEmailHistory_BE> GetReminderEmailHistoryList(string sql)
        {
            List<ReminderEmailHistory_BE> reminderEmailHistoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReminderEmailHistory_DAO reminderEmailHistoryDataAccessObject = new ReminderEmailHistory_DAO();
                reminderEmailHistoryDataAccessObject.GetReminderEmailHistoryList(sql);

                reminderEmailHistoryBusinessEntityList = reminderEmailHistoryDataAccessObject.reminderEmailHistoryBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return reminderEmailHistoryBusinessEntityList;
        }

        /// <summary>
        /// Get Reminder Email History List
        /// </summary>
        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dataTableReminderEmailHistory = new DataTable();
            try
            {
                ReminderEmailHistory_DAO reminderEmailHistoryDataAccessObject = new ReminderEmailHistory_DAO();
                dataTableReminderEmailHistory = reminderEmailHistoryDataAccessObject.GetdtReminderEmailHistoryList(sql);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dataTableReminderEmailHistory;
        }

        /// <summary>
        /// Get Questionnaire List count
        /// </summary>
        public int GetQuestionnaireListCount(string sql)
        {
            int reminderEmailHistoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReminderEmailHistory_DAO reminderEmailHistoryDataAccessObject = new ReminderEmailHistory_DAO();
                reminderEmailHistoryCount = reminderEmailHistoryDataAccessObject.GetReminderEmailHistoryListCount(sql);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return reminderEmailHistoryCount;
        }

    }

    public class Survey_ReminderEmailHistory_BAO : Base_BAO
    {
        /// <summary>
        /// Get Reminder Email History List
        /// </summary>
        public List<Survey_ReminderEmailHistory_BE> GetReminderEmailHistoryList(string sql)
        {
            List<Survey_ReminderEmailHistory_BE> reminderEmailHistoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReminderEmailHistory_DAO reminderEmailHistoryDataAccessObject = new Survey_ReminderEmailHistory_DAO();
                reminderEmailHistoryDataAccessObject.GetReminderEmailHistoryList(sql);

                reminderEmailHistoryBusinessEntityList = reminderEmailHistoryDataAccessObject.reminderEmailHistoryBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return reminderEmailHistoryBusinessEntityList;
        }

        /// <summary>
        /// Get Reminder Email History List
        /// </summary>
        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dtReminderEmailHistory = new DataTable();
            try
            {
                Survey_ReminderEmailHistory_DAO reminderEmailHistoryDataAccessObject = new Survey_ReminderEmailHistory_DAO();
                dtReminderEmailHistory = reminderEmailHistoryDataAccessObject.GetdtReminderEmailHistoryList(sql);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtReminderEmailHistory;
        }

        /// <summary>
        /// Get Questionnaire List Count
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetQuestionnaireListCount(string sql)
        {
            int reminderEmailHistoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReminderEmailHistory_DAO reminderEmailHistoryDataAccessObject = new Survey_ReminderEmailHistory_DAO();
                reminderEmailHistoryCount = reminderEmailHistoryDataAccessObject.GetReminderEmailHistoryListCount(sql);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return reminderEmailHistoryCount;
        }
    }
}
