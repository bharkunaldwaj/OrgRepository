using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using DAF_BAO;
using DatabaseAccessUtilities;

//using Questionnaire_BE;
//using Questionnaire_DAO;
using Admin_BE;
using Admin_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Admin_BAO
{
    public class EmailTemplate_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        #region CRUD Operations

        public int AddEmailTemplate(EmailTemplate_BE emailtemplate_BE)
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
                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                addCategory = emailtemplate_DAO.AddEmailTemplate(emailtemplate_BE);
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
            return addCategory;
        }

        public int UpdateEmailTemplate(EmailTemplate_BE emailtemplate_BE)
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
                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                addCategory = emailtemplate_DAO.UpdateEmailTemplate(emailtemplate_BE);
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
            return addCategory;
        }

        public int DeleteEmailTemplate(EmailTemplate_BE emailtemplate_BE)
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
                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                addCategory = emailtemplate_DAO.DeleteEmailTemplate(emailtemplate_BE);
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
            return addCategory;
        }

        public List<EmailTemplate_BE> GetEmailTemplateByID(int accountID, int EmailTemplateID)
        {
            List<EmailTemplate_BE> EmailTemplate_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                emailtemplate_DAO.GetEmailTemplateByID(accountID,  EmailTemplateID);

                EmailTemplate_BEList = emailtemplate_DAO.emailtemplate_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return EmailTemplate_BEList;
        }



     



        public List<EmailTemplate_BE> GetEmailTemplateList()
        {
            List<EmailTemplate_BE> category_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                //emailtemplate_DAO.GetdtEmailTemplateList();

                category_BEList = emailtemplate_DAO.emailtemplate_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return category_BEList;
        }

        public DataTable GetdtEmailTemplateList(string accountID)
        {
            DataTable dtCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                dtCategory = emailtemplate_DAO.GetdtEmailTemplateList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategory;
        }

        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dtCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                dtCategory = emailtemplate_DAO.GetAdminEmailTemplate(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategory;
        }

        //public DataTable GetdtEmailTemplateList()
        //{
        //    DataTable dtCategory = null;

        //    try
        //    {
        //        //HandleWriteLog("Start", new StackTrace(true));

        //        EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
        //        //dtCategory = emailtemplate_DAO.GetdtEmailTemplateList(accountID);

        //        //HandleWriteLog("End", new StackTrace(true));
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    return dtCategory;
        //}

        public int GetEmailTemplateListCount(string accountID)
        {
            int categoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                categoryCount = emailtemplate_DAO.GetEmailTemplateListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryCount;
        }


        public void InsertMailTemplateID(string Id, int accountid)
        {
           

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
                emailtemplate_DAO.InsertMailTemplateID(Id,accountid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

           
        }




        #endregion
    }






















    public class Survey_EmailTemplate_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        #region CRUD Operations

        public int AddEmailTemplate(Survey_EmailTemplate_BE emailtemplate_BE)
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
                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                addCategory = emailtemplate_DAO.AddEmailTemplate(emailtemplate_BE);
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
            return addCategory;
        }

        public int UpdateEmailTemplate(Survey_EmailTemplate_BE emailtemplate_BE)
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
                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                addCategory = emailtemplate_DAO.UpdateEmailTemplate(emailtemplate_BE);
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
            return addCategory;
        }

        public int DeleteEmailTemplate(Survey_EmailTemplate_BE emailtemplate_BE)
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
                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                addCategory = emailtemplate_DAO.DeleteEmailTemplate(emailtemplate_BE);
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
            return addCategory;
        }

        public List<Survey_EmailTemplate_BE> GetEmailTemplateByID(int accountID, int EmailTemplateID)
        {
            List<Survey_EmailTemplate_BE> EmailTemplate_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                emailtemplate_DAO.GetEmailTemplateByID(accountID, EmailTemplateID);

                EmailTemplate_BEList = emailtemplate_DAO.emailtemplate_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return EmailTemplate_BEList;
        }







        public List<Survey_EmailTemplate_BE> GetEmailTemplateList()
        {
            List<Survey_EmailTemplate_BE> category_BEList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                //emailtemplate_DAO.GetdtEmailTemplateList();

                category_BEList = emailtemplate_DAO.emailtemplate_BEList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return category_BEList;
        }

        public DataTable GetdtEmailTemplateList(string accountID)
        {
            DataTable dtCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                dtCategory = emailtemplate_DAO.GetdtEmailTemplateList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategory;
        }

        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dtCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                dtCategory = emailtemplate_DAO.GetAdminEmailTemplate(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategory;
        }

        //public DataTable GetdtEmailTemplateList()
        //{
        //    DataTable dtCategory = null;

        //    try
        //    {
        //        //HandleWriteLog("Start", new StackTrace(true));

        //        EmailTemplate_DAO emailtemplate_DAO = new EmailTemplate_DAO();
        //        //dtCategory = emailtemplate_DAO.GetdtEmailTemplateList(accountID);

        //        //HandleWriteLog("End", new StackTrace(true));
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    return dtCategory;
        //}

        public int GetEmailTemplateListCount(string accountID)
        {
            int categoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                categoryCount = emailtemplate_DAO.GetEmailTemplateListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryCount;
        }


        public void InsertMailTemplateID(string Id, int accountid)
        {


            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailtemplate_DAO = new Survey_EmailTemplate_DAO();
                emailtemplate_DAO.InsertMailTemplateID(Id, accountid);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }




        #endregion
    }








}