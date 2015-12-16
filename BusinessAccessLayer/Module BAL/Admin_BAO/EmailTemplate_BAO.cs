using System;
using System.Collections.Generic;

using DAF_BAO;
using DatabaseAccessUtilities;

//using Questionnaire_BE;
//using Questionnaire_DAO;
using Admin_BE;
using Admin_DAO;

using System.Data;

namespace Admin_BAO
{
    public class EmailTemplate_BAO : Base_BAO
    {
        #region "Private Member Variable"

        private int addCategory;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Add Email Template
        /// </summary>
        /// <param name="emailTemplateBusinessEntity"></param>
        /// <returns></returns>
        public int AddEmailTemplate(EmailTemplate_BE emailTemplateBusinessEntity)
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
                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                addCategory = emailTemplateDataAccessObject.AddEmailTemplate(emailTemplateBusinessEntity);
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

        /// <summary>
        /// update Email Template
        /// </summary>
        public int UpdateEmailTemplate(EmailTemplate_BE emailTemplateBusinessEntity)
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
                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                addCategory = emailTemplateDataAccessObject.UpdateEmailTemplate(emailTemplateBusinessEntity);
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

        /// <summary>
        /// Delete Email Template
        /// </summary>
        public int DeleteEmailTemplate(EmailTemplate_BE emailTemplateBusinessEntity)
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
                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                addCategory = emailTemplateDataAccessObject.DeleteEmailTemplate(emailTemplateBusinessEntity);
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

        /// <summary>
        /// Get Email Template by tempalte id
        /// </summary>
        public List<EmailTemplate_BE> GetEmailTemplateByID(int accountID, int EmailTemplateID)
        {
            List<EmailTemplate_BE> EmailTemplateBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailTemplateBusinessAccessObject = new EmailTemplate_DAO();
                emailTemplateBusinessAccessObject.GetEmailTemplateByID(accountID, EmailTemplateID);

                EmailTemplateBusinessEntityList = emailTemplateBusinessAccessObject.emailtemplateBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return EmailTemplateBusinessEntityList;
        }

        /// <summary>
        /// Get Email Tempalte List.
        /// </summary>
        /// <returns></returns>
        public List<EmailTemplate_BE> GetEmailTemplateList()
        {
            List<EmailTemplate_BE> categoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                //emailtemplate_DAO.GetdtEmailTemplateList();

                categoryBusinessEntityList = emailTemplateDataAccessObject.emailtemplateBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return categoryBusinessEntityList;
        }

        /// <summary>
        /// Get Email Tempalte List.
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtEmailTemplateList(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                dataTableCategory = emailTemplateDataAccessObject.GetdtEmailTemplateList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
        }

        /// <summary>
        /// Get Admin Email Tempalte List.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                dataTableCategory = emailTemplateDataAccessObject.GetAdminEmailTemplate(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
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

        /// <summary>
        /// Get Email Tempalte List count by account id.
        /// </summary>
        /// <returns></returns>
        public int GetEmailTemplateListCount(string accountID)
        {
            int categoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                categoryCount = emailTemplateDataAccessObject.GetEmailTemplateListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryCount;
        }

        /// <summary>
        /// Insert Email Tempalte .
        /// </summary>
        /// <returns></returns>
        public void InsertMailTemplateID(string Id, int accountid)
        {


            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                EmailTemplate_DAO emailTemplateDataAccessObject = new EmailTemplate_DAO();
                emailTemplateDataAccessObject.InsertMailTemplateID(Id, accountid);

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
        /// <summary>
        /// Add Email Template
        /// </summary>
        /// <param name="emailTemplateBusinessEntity"></param>
        /// <returns></returns>
        public int AddEmailTemplate(Survey_EmailTemplate_BE emailTemplateBusinessEntity)
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
                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                addCategory = emailTemplateDataAccessObject.AddEmailTemplate(emailTemplateBusinessEntity);
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
        /// <summary>
        /// update Email Template
        /// </summary>
        public int UpdateEmailTemplate(Survey_EmailTemplate_BE emailTemplateBusinessEntity)
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
                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                addCategory = emailTemplateDataAccessObject.UpdateEmailTemplate(emailTemplateBusinessEntity);
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
        /// <summary>
        /// Delete Email Template
        /// </summary>
        public int DeleteEmailTemplate(Survey_EmailTemplate_BE emailTemplateBusinessEntity)
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
                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                addCategory = emailTemplateDataAccessObject.DeleteEmailTemplate(emailTemplateBusinessEntity);
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
        /// <summary>
        /// Get Email Template by tempalte id
        /// </summary>
        public List<Survey_EmailTemplate_BE> GetEmailTemplateByID(int accountID, int EmailTemplateID)
        {
            List<Survey_EmailTemplate_BE> EmailTemplateBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                emailTemplateDataAccessObject.GetEmailTemplateByID(accountID, EmailTemplateID);

                EmailTemplateBusinessEntityList = emailTemplateDataAccessObject.emailtemplateBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return EmailTemplateBusinessEntityList;
        }
        /// <summary>
        /// Get Email Tempalte List.
        /// </summary>
        /// <returns></returns>
        public List<Survey_EmailTemplate_BE> GetEmailTemplateList()
        {
            List<Survey_EmailTemplate_BE> categoryBusinessEntityList = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                //emailtemplate_DAO.GetdtEmailTemplateList();

                categoryBusinessEntityList = emailTemplateDataAccessObject.emailtemplateBusinessEntityList;

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return categoryBusinessEntityList;
        }
        /// <summary>
        /// Get Email Tempalte List by account id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtEmailTemplateList(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                dataTableCategory = emailTemplateDataAccessObject.GetdtEmailTemplateList(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
        }
        /// <summary>
        /// Get Admin Email Tempalte List.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dataTableCategory = null;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                dataTableCategory = emailTemplateDataAccessObject.GetAdminEmailTemplate(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategory;
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

        /// <summary>
        /// Get Email Tempalte List count by account id.
        /// </summary>
        /// <returns></returns>
        public int GetEmailTemplateListCount(string accountID)
        {
            int categoryCount = 0;

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                categoryCount = emailTemplateDataAccessObject.GetEmailTemplateListCount(accountID);

                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryCount;
        }
        /// <summary>
        /// Insert Email Tempalte .
        /// </summary>
        /// <returns></returns>
        public void InsertMailTemplateID(string Id, int accountid)
        {


            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                Survey_EmailTemplate_DAO emailTemplateDataAccessObject = new Survey_EmailTemplate_DAO();
                emailTemplateDataAccessObject.InsertMailTemplateID(Id, accountid);

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