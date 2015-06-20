using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DAF_BAO;
using DatabaseAccessUtilities;
using Admin_BE;
using Admin_DAO;
using System.Data;
using System.Data.SqlClient;

namespace Admin_BAO
{
    public class ReminderEmailHistory_BAO : Base_BAO
    {
        public List<ReminderEmailHistory_BE> GetReminderEmailHistoryList(string sql)
        {
            List<ReminderEmailHistory_BE> reminderEmailHistory_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReminderEmailHistory_DAO reminderEmailHistory_DAO = new ReminderEmailHistory_DAO();
                reminderEmailHistory_DAO.GetReminderEmailHistoryList(sql);

                reminderEmailHistory_BEList = reminderEmailHistory_DAO.reminderEmailHistory_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return reminderEmailHistory_BEList;
        }

        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dtReminderEmailHistory = new DataTable();
            try
            {
                ReminderEmailHistory_DAO reminderEmailHistory_DAO = new ReminderEmailHistory_DAO();
                dtReminderEmailHistory=reminderEmailHistory_DAO.GetdtReminderEmailHistoryList(sql);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtReminderEmailHistory;
        }

        public int GetQuestionnaireListCount(string sql)
        {
            int reminderEmailHistoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                ReminderEmailHistory_DAO reminderEmailHistory_DAO = new ReminderEmailHistory_DAO();
                reminderEmailHistoryCount = reminderEmailHistory_DAO.GetReminderEmailHistoryListCount(sql);

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
        public List<Survey_ReminderEmailHistory_BE> GetReminderEmailHistoryList(string sql)
        {
            List<Survey_ReminderEmailHistory_BE> reminderEmailHistory_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReminderEmailHistory_DAO reminderEmailHistory_DAO = new Survey_ReminderEmailHistory_DAO();
                reminderEmailHistory_DAO.GetReminderEmailHistoryList(sql);

                reminderEmailHistory_BEList = reminderEmailHistory_DAO.reminderEmailHistory_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return reminderEmailHistory_BEList;
        }

        public DataTable GetdtReminderEmailHistoryList(string sql)
        {
            DataTable dtReminderEmailHistory = new DataTable();
            try
            {
                Survey_ReminderEmailHistory_DAO reminderEmailHistory_DAO = new Survey_ReminderEmailHistory_DAO();
                dtReminderEmailHistory = reminderEmailHistory_DAO.GetdtReminderEmailHistoryList(sql);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return dtReminderEmailHistory;
        }

        public int GetQuestionnaireListCount(string sql)
        {
            int reminderEmailHistoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_ReminderEmailHistory_DAO reminderEmailHistory_DAO = new Survey_ReminderEmailHistory_DAO();
                reminderEmailHistoryCount = reminderEmailHistory_DAO.GetReminderEmailHistoryListCount(sql);

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
