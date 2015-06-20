#region Creation
// Creator : Adeel
// Date of Creation : Oct-05-2010
#endregion


using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security;

using System.Reflection;
using System.Diagnostics;

using feedbackFramework_BE;
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

        public List<EmailTemplate_BE> emailtemplate_BEList { get; set; }

        #endregion

        # region CRUD Operation

        /// <summary>
        /// Add email address
        /// </summary>
        /// <param name="emailtemplate_BE"></param>
        /// <returns></returns>
        public int AddEmailTemplate(EmailTemplate_BE emailtemplate_BE)
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
                param.Add(new CNameValue("@AccountID", emailtemplate_BE.AccountID));
                param.Add(new CNameValue("@Title", emailtemplate_BE.Title));
                param.Add(new CNameValue("@Description", emailtemplate_BE.Description));
                param.Add(new CNameValue("@Subject", emailtemplate_BE.Subject));
                param.Add(new CNameValue("@EmailText", emailtemplate_BE.EmailText));
                param.Add(new CNameValue("@EmailImage", emailtemplate_BE.EmailImage));
                param.Add(new CNameValue("@ModifyBy", emailtemplate_BE.ModifyBy));
                param.Add(new CNameValue("@ModifyDate", emailtemplate_BE.ModifyDate));
                param.Add(new CNameValue("@IsActive", emailtemplate_BE.IsActive));
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
        /// <param name="emailtemplate_BE"></param>
        /// <returns></returns>
        public int UpdateEmailTemplate(EmailTemplate_BE emailtemplate_BE)
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
                param.Add(new CNameValue("@EmailTemplateID", emailtemplate_BE.EmailTemplateID));
                param.Add(new CNameValue("@AccountID", emailtemplate_BE.AccountID));
                param.Add(new CNameValue("@Title", emailtemplate_BE.Title));
                param.Add(new CNameValue("@Description", emailtemplate_BE.Description));
                param.Add(new CNameValue("@Subject", emailtemplate_BE.Subject));
                param.Add(new CNameValue("@EmailText", emailtemplate_BE.EmailText));
                param.Add(new CNameValue("@EmailImage", emailtemplate_BE.EmailImage));
                param.Add(new CNameValue("@ModifyBy", emailtemplate_BE.ModifyBy));
                param.Add(new CNameValue("@ModifyDate", emailtemplate_BE.ModifyDate));
                param.Add(new CNameValue("@IsActive", emailtemplate_BE.IsActive));
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
        /// <param name="emailtemplate_BE"></param>
        /// <returns></returns>
        public int DeleteEmailTemplate(EmailTemplate_BE emailtemplate_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {emailtemplate_BE.EmailTemplateID,
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
                DataTable dtAlluser = new DataTable();
                object[] param = new object[3] { emailID, accountID, "I" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
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
                
                DataTable dtAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAlluser = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
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
            DataTable dtAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[3] { null,Convert.ToInt32(accountID), "A" };
                
                dtAllEmailTemplate = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllEmailTemplate;
        }

        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dtAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "F" };

                dtAllEmailTemplate = cDataSrc.ExecuteDataSet("UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllEmailTemplate;
        }

        public void InsertMailTemplateID(string id,int accountid)
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
        /// <param name="dtEmailTemplate"></param>
        private void ShiftDataTableToBEList(DataTable dtEmailTemplate)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            emailtemplate_BEList = new List<EmailTemplate_BE>();

            for (int recordCounter = 0; recordCounter < dtEmailTemplate.Rows.Count; recordCounter++)
            {
                EmailTemplate_BE emailtemplate_BE = new EmailTemplate_BE();

                emailtemplate_BE.EmailTemplateID    = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["EmailTemplateID"].ToString());
                emailtemplate_BE.AccountID          = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["AccountID"].ToString());
                emailtemplate_BE.Title              = dtEmailTemplate.Rows[recordCounter]["Title"].ToString();
                emailtemplate_BE.Description        = dtEmailTemplate.Rows[recordCounter]["Description"].ToString();
                emailtemplate_BE.Subject            = dtEmailTemplate.Rows[recordCounter]["Subject"].ToString();
                emailtemplate_BE.EmailText          = Convert.ToString(dtEmailTemplate.Rows[recordCounter]["EmailText"].ToString());
                emailtemplate_BE.EmailImage         = dtEmailTemplate.Rows[recordCounter]["EmailImage"].ToString();
                emailtemplate_BE.ModifyBy           = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["ModifyBy"].ToString());
                emailtemplate_BE.ModifyDate         = Convert.ToDateTime(dtEmailTemplate.Rows[recordCounter]["ModifyDate"].ToString());
                emailtemplate_BE.IsActive           = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["IsActive"].ToString());

                emailtemplate_BEList.Add(emailtemplate_BE);
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

        public List<Survey_EmailTemplate_BE> emailtemplate_BEList { get; set; }

        #endregion

        # region CRUD Operation

        /// <summary>
        /// Add email address
        /// </summary>
        /// <param name="emailtemplate_BE"></param>
        /// <returns></returns>
        public int AddEmailTemplate(Survey_EmailTemplate_BE emailtemplate_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {null,
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
        /// <param name="emailtemplate_BE"></param>
        /// <returns></returns>
        public int UpdateEmailTemplate(Survey_EmailTemplate_BE emailtemplate_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {
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
        /// <param name="emailtemplate_BE"></param>
        /// <returns></returns>
        public int DeleteEmailTemplate(Survey_EmailTemplate_BE emailtemplate_BE)
        {
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[11] {emailtemplate_BE.EmailTemplateID,
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
                DataTable dtAlluser = new DataTable();
                object[] param = new object[3] { emailID, accountID, "I" };

                dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
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

                DataTable dtAlluser = new DataTable();
                object[] param = new object[2] { null, "A" };

                dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                ShiftDataTableToBEList(dtAlluser);
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
            DataTable dtAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "A" };

                dtAllEmailTemplate = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllEmailTemplate;
        }

        public DataTable GetAdminEmailTemplate(string accountID)
        {
            DataTable dtAllEmailTemplate = new DataTable();
            try
            {
                //HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[3] { null, Convert.ToInt32(accountID), "F" };

                dtAllEmailTemplate = cDataSrc.ExecuteDataSet("Survey_UspEmailTemplateSelect", param, null).Tables[0];

                //HandleWriteLogDAU("UspEmailTemplateSelect", param, new StackTrace(true));
                //HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return dtAllEmailTemplate;
        }

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
        /// <param name="dtEmailTemplate"></param>
        private void ShiftDataTableToBEList(DataTable dtEmailTemplate)
        {
            //HandleWriteLog("Start", new StackTrace(true));
            emailtemplate_BEList = new List<Survey_EmailTemplate_BE>();

            for (int recordCounter = 0; recordCounter < dtEmailTemplate.Rows.Count; recordCounter++)
            {
                Survey_EmailTemplate_BE emailtemplate_BE = new Survey_EmailTemplate_BE();

                emailtemplate_BE.EmailTemplateID = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["EmailTemplateID"].ToString());
                emailtemplate_BE.AccountID = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["AccountID"].ToString());
                emailtemplate_BE.Title = dtEmailTemplate.Rows[recordCounter]["Title"].ToString();
                emailtemplate_BE.Description = dtEmailTemplate.Rows[recordCounter]["Description"].ToString();
                emailtemplate_BE.Subject = dtEmailTemplate.Rows[recordCounter]["Subject"].ToString();
                emailtemplate_BE.EmailText = Convert.ToString(dtEmailTemplate.Rows[recordCounter]["EmailText"].ToString());
                emailtemplate_BE.EmailImage = dtEmailTemplate.Rows[recordCounter]["EmailImage"].ToString();
                emailtemplate_BE.ModifyBy = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["ModifyBy"].ToString());
                emailtemplate_BE.ModifyDate = Convert.ToDateTime(dtEmailTemplate.Rows[recordCounter]["ModifyDate"].ToString());
                emailtemplate_BE.IsActive = Convert.ToInt32(dtEmailTemplate.Rows[recordCounter]["IsActive"].ToString());

                emailtemplate_BEList.Add(emailtemplate_BE);
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