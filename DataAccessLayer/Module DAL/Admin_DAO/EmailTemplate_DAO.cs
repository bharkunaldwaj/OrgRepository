#region Creation
// Creator : Adeel
// Date of Creation : Oct-05-2010
#endregion

using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

using System.Diagnostics;

using feedbackFramework_DAO;

//using Questionnaire_BE;
using DatabaseAccessUtilities;
using Admin_BE;


#region Module
namespace Admin_DAO
{
    public class EmailTemplate_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public EmailTemplate_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<EmailTemplate_BE> emailtemplateBusinessEntityList { get; set; }

        #endregion

        # region CRUD Operation
        /// <summary>
        /// Add email address
        /// </summary>
        /// <param name="emailtemplateBusinessEntity"></param>
        /// <returns></returns>
        public int AddEmailTemplate(EmailTemplate_BE emailtemplateBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                /*object[] param = new object[11] {null,
                                                emailtemplate_BE.AccountID,
                                                emailtemplate_BE.Title,
                                                emailtemplate_BE.Description,
                                                emailtemplate_BE.Subject,
                                                emailtemplate_BE.EmailText,
                                                emailtemplate_BE.EmailImage,
                                                emailtemplate_BE.ModifyBy,
                                                emailtemplate_BE.ModifyDate,
                                                emailtemplate_BE.IsActive,
                                                "I" };
                 */

                // To support chiense / japnese language
                CNameValueList param = new CNameValueList();
                param.Add(new CNameValue("@EmailTemplateID", emailtemplateBusinessEntity.EmailTemplateID));
                param.Add(new CNameValue("@AccountID", emailtemplateBusinessEntity.AccountID));
                param.Add(new CNameValue("@Title", emailtemplateBusinessEntity.Title));
                param.Add(new CNameValue("@Description", emailtemplateBusinessEntity.Description));
                param.Add(new CNameValue("@Subject", emailtemplateBusinessEntity.Subject));
                param.Add(new CNameValue("@EmailText", emailtemplateBusinessEntity.EmailText));
                param.Add(new CNameValue("@EmailImage", emailtemplateBusinessEntity.EmailImage));
                param.Add(new CNameValue("@ModifyBy", emailtemplateBusinessEntity.ModifyBy));
                param.Add(new CNameValue("@ModifyDate", emailtemplateBusinessEntity.ModifyDate));
                param.Add(new CNameValue("@IsActive", emailtemplateBusinessEntity.IsActive));
                param.Add(new CNameValue("@Operation", "I"));

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspEmailTemplateManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Update the email address
        /// </summary>
        /// <param name="emailtemplateBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateEmailTemplate(EmailTemplate_BE emailtemplateBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                /*object[] param = new object[11] {
                                                emailtemplate_BE.EmailTemplateID,
                                                emailtemplate_BE.AccountID,
                                                emailtemplate_BE.Title,
                                                emailtemplate_BE.Description,
                                                emailtemplate_BE.Subject,
                                                emailtemplate_BE.EmailText,
                                                emailtemplate_BE.EmailImage,
                                                emailtemplate_BE.ModifyBy,
                                                emailtemplate_BE.ModifyDate,
                                                emailtemplate_BE.IsActive,
                                                "U" };
                */

                // To support chiense / japnese language 
                CNameValueList param = new CNameValueList();
                param.Add(new CNameValue("@EmailTemplateID", emailtemplateBusinessEntity.EmailTemplateID));
                param.Add(new CNameValue("@AccountID", emailtemplateBusinessEntity.AccountID));
                param.Add(new CNameValue("@Title", emailtemplateBusinessEntity.Title));
                param.Add(new CNameValue("@Description", emailtemplateBusinessEntity.Description));
                param.Add(new CNameValue("@Subject", emailtemplateBusinessEntity.Subject));
                param.Add(new CNameValue("@EmailText", emailtemplateBusinessEntity.EmailText));
                param.Add(new CNameValue("@EmailImage", emailtemplateBusinessEntity.EmailImage));
                param.Add(new CNameValue("@ModifyBy", emailtemplateBusinessEntity.ModifyBy));
                param.Add(new CNameValue("@ModifyDate", emailtemplateBusinessEntity.ModifyDate));
                param.Add(new CNameValue("@IsActive", emailtemplateBusinessEntity.IsActive));
                param.Add(new CNameValue("@Operation", "U"));

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspEmailTemplateManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Delete the Email Template
        /// </summary>
        /// <param name="emailtemplateBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteEmailTemplate(EmailTemplate_BE emailtemplateBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {emailtemplateBusinessEntity.EmailTemplateID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspEmailTemplateManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Retrive Email IDs for the records
        /// </summary>
        /// <param name="emailID"></param>
        /// <returns></returns>
        public int GetEmailTemplateByID(int accountID, int emailID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAlluser = new DataTable();
                object[] param = new object[3] { emailID, accountID, "I" };

                dataTableAlluser = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dataTableAlluser);
                returnValue = 1;

                HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Retrive Selected Email Template
        /// </summary>
        /// <returns></returns>
        public int GetEmailTemplateList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //WADPrincipal identity;//= this.Page.User as WADPrincipal;
                //identity = this.Page.User.Identity as WADIdentity;

                DataTable dataTableAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dataTableAlluser = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dataTableAlluser);
                returnValue = 1;

                HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Retrive Data Wise Selected Email Template
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtEmailTemplateList(string accountID)
        {
            DataTable dataTableAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

                dataTableAllEmailTemplate = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllEmailTemplate;
        }

        /// <summary>
        /// Get Admin Email Template by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dataTableAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "F" };

                dataTableAllEmailTemplate = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllEmailTemplate;
        }

        /// <summary>
        /// Insert Mail Template 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountid"></param>
        public void InsertMailTemplateID(string id, int accountid)
        {

            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //foreach (string to in id.Split(';'))
                //{
                //    int i = Convert.ToInt32(to);
                object[] param = new object[3] { id, accountid, "C" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspEmailTemplateCopy", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
                //}
            }
            catch (Exception ex) { HandleException(ex); }
        }
        #endregion

        #region MISC Data Operations
        /// <summary>
        /// Copy Date from the Data Table into the Business Entity of Email
        /// Templates
        /// </summary>
        /// <param name="dataTableEmailTemplate"></param>
        private void ShiftDataTableToBEList(DataTable dataTableEmailTemplate)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            emailtemplateBusinessEntityList = new List<EmailTemplate_BE>();

            for (int recordCounter = 0; recordCounter < dataTableEmailTemplate.Rows.Count; recordCounter++)
            {
                EmailTemplate_BE emailtemplate_BE = new EmailTemplate_BE();

                emailtemplate_BE.EmailTemplateID = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["EmailTemplateID"].ToString());
                emailtemplate_BE.AccountID = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["AccountID"].ToString());
                emailtemplate_BE.Title = dataTableEmailTemplate.Rows[recordCounter]["Title"].ToString();
                emailtemplate_BE.Description = dataTableEmailTemplate.Rows[recordCounter]["Description"].ToString();
                emailtemplate_BE.Subject = dataTableEmailTemplate.Rows[recordCounter]["Subject"].ToString();
                emailtemplate_BE.EmailText = Convert.ToString(dataTableEmailTemplate.Rows[recordCounter]["EmailText"].ToString());
                emailtemplate_BE.EmailImage = dataTableEmailTemplate.Rows[recordCounter]["EmailImage"].ToString();
                emailtemplate_BE.ModifyBy = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["ModifyBy"].ToString());
                emailtemplate_BE.ModifyDate = Convert.ToDateTime(dataTableEmailTemplate.Rows[recordCounter]["ModifyDate"].ToString());
                emailtemplate_BE.IsActive = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["IsActive"].ToString());

                emailtemplateBusinessEntityList.Add(emailtemplate_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Enumerate the list of Email Templates
        /// </summary>
        /// <returns></returns>
        public int GetEmailTemplateListCount(string accountID)
        {
            int emailtemplateCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "C" };

                emailtemplateCount = (int)cDataSrc.ExecuteScalar("UspEmailTemplateSelect", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return emailtemplateCount;
        }
        #endregion
    }

    public class Survey_EmailTemplate_DAO : DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Survey_EmailTemplate_DAO()
        {
            //HandleWriteLog("Start", new StackTrace(true));
            //HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Properties"

        public List<Survey_EmailTemplate_BE> emailtemplateBusinessEntityList { get; set; }

        #endregion

        # region CRUD Operation

        /// <summary>
        /// Add email address
        /// </summary>
        /// <param name="emailtemplateBusinessEntity"></param>
        /// <returns></returns>
        public int AddEmailTemplate(Survey_EmailTemplate_BE emailtemplateBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {null,
                                                emailtemplateBusinessEntity.AccountID,
                                                emailtemplateBusinessEntity.Title,
                                                emailtemplateBusinessEntity.Description,
                                                emailtemplateBusinessEntity.Subject,
                                                emailtemplateBusinessEntity.EmailText,
                                                emailtemplateBusinessEntity.EmailImage,
                                                emailtemplateBusinessEntity.ModifyBy,
                                                emailtemplateBusinessEntity.ModifyDate,
                                                emailtemplateBusinessEntity.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspEmailTemplateManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Update the email address
        /// </summary>
        /// <param name="emailtemplateBusinessEntity"></param>
        /// <returns></returns>
        public int UpdateEmailTemplate(Survey_EmailTemplate_BE emailtemplateBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {
                                                emailtemplateBusinessEntity.EmailTemplateID,
                                                emailtemplateBusinessEntity.AccountID,
                                                emailtemplateBusinessEntity.Title,
                                                emailtemplateBusinessEntity.Description,
                                                emailtemplateBusinessEntity.Subject,
                                                emailtemplateBusinessEntity.EmailText,
                                                emailtemplateBusinessEntity.EmailImage,
                                                emailtemplateBusinessEntity.ModifyBy,
                                                emailtemplateBusinessEntity.ModifyDate,
                                                emailtemplateBusinessEntity.IsActive,
                                                "U" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspEmailTemplateManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Delete the Email Template
        /// </summary>
        /// <param name="emailtemplateBusinessEntity"></param>
        /// <returns></returns>
        public int DeleteEmailTemplate(Survey_EmailTemplate_BE emailtemplateBusinessEntity)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {emailtemplateBusinessEntity.EmailTemplateID,
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

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspEmailTemplateManagement", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateManagement", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Retrive Email IDs for the records
        /// </summary>
        /// <param name="emailID"></param>
        /// <returns></returns>
        public int GetEmailTemplateByID(int accountID, int emailID)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAlluser = new DataTable();
                object[] param = new object[3] { emailID, accountID, "I" };

                dataTableAlluser = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dataTableAlluser);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Retrive Selected Email Template
        /// </summary>
        /// <returns></returns>
        public int GetEmailTemplateList()
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                //WADPrincipal identity;//= this.Page.User as WADPrincipal;
                //identity = this.Page.User.Identity as WADIdentity;

                DataTable dataTableAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dataTableAlluser = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dataTableAlluser);
                returnValue = 1;

                HandleWriteLogDAU("Survey_UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Retrive Data Wise Selected Email Template
        /// </summary>
        /// <returns></returns>
        public DataTable GetdtEmailTemplateList(string accountID)
        {
            DataTable dataTableAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

                dataTableAllEmailTemplate = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllEmailTemplate;
        }

        /// <summary>
        /// Get Admin Email Template by accountID
        /// </summary>
        /// <param name="accountID">account ID</param>
        /// <returns></returns>
        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dataTableAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "F" };

                dataTableAllEmailTemplate = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dataTableAllEmailTemplate;
        }

        /// <summary>
        /// Insert Mail Template 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountid"></param>
        public void InsertMailTemplateID(string id, int accountid)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                //foreach (string to in id.Split(';'))
                //{
                //    int i = Convert.ToInt32(to);

                object[] param = new object[3] { id, accountid, "C" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("Survey_UspEmailTemplateCopy", param, null));

                cDataSrc = null;

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
                //}
            }
            catch (Exception ex) { HandleException(ex); }
        }
        #endregion

        #region MISC Data Operations
        /// <summary>
        /// Copy Date from the Data Table into the Business Entity of Email
        /// Templates
        /// </summary>
        /// <param name="dataTableEmailTemplate"></param>
        private void ShiftDataTableToBEList(DataTable dataTableEmailTemplate)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            emailtemplateBusinessEntityList = new List<Survey_EmailTemplate_BE>();

            for (int recordCounter = 0; recordCounter < dataTableEmailTemplate.Rows.Count; recordCounter++)
            {
                Survey_EmailTemplate_BE emailtemplate_BE = new Survey_EmailTemplate_BE();

                emailtemplate_BE.EmailTemplateID = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["EmailTemplateID"].ToString());
                emailtemplate_BE.AccountID = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["AccountID"].ToString());
                emailtemplate_BE.Title = dataTableEmailTemplate.Rows[recordCounter]["Title"].ToString();
                emailtemplate_BE.Description = dataTableEmailTemplate.Rows[recordCounter]["Description"].ToString();
                emailtemplate_BE.Subject = dataTableEmailTemplate.Rows[recordCounter]["Subject"].ToString();
                emailtemplate_BE.EmailText = Convert.ToString(dataTableEmailTemplate.Rows[recordCounter]["EmailText"].ToString());
                emailtemplate_BE.EmailImage = dataTableEmailTemplate.Rows[recordCounter]["EmailImage"].ToString();
                emailtemplate_BE.ModifyBy = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["ModifyBy"].ToString());
                emailtemplate_BE.ModifyDate = Convert.ToDateTime(dataTableEmailTemplate.Rows[recordCounter]["ModifyDate"].ToString());
                emailtemplate_BE.IsActive = Convert.ToInt32(dataTableEmailTemplate.Rows[recordCounter]["IsActive"].ToString());

                emailtemplateBusinessEntityList.Add(emailtemplate_BE);
            }

            //HandleWriteLog("End", new StackTrace(true));
        }

        /// <summary>
        /// Enumerate the list of Email Templates
        /// </summary>
        /// <returns></returns>
        public int GetEmailTemplateListCount(string accountID)
        {
            int emailtemplateCount = 0;
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "C" };

                emailtemplateCount = (int)cDataSrc.ExecuteScalar("Survey_UspEmailTemplateSelect", param, null);

                //HandleWriteLogDAU("UspCategorySelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return emailtemplateCount;
        }
        #endregion
    }
}
#endregion