/*
* PURPOSE: Data Access Object for Contact Entity
* AUTHOR:  Manish Mathur
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using feedbackFramework_DAO;

using Administration_BE;
using DatabaseAccessUtilities;

namespace Administration_DAO
{
    public class Contact_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Contact_DAO()
        {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        //public Contact_BE[] Contact_BEArray { get; set; }
        public List<Contact_BE> ContactBusinessEntityList { get; set; }
        #endregion

        #region Private Member Variables
        private int returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to BEArray object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllContact)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                ContactBusinessEntityList = new List<Contact_BE>();
                for (int recordCounter = 0; recordCounter < dataTableAllContact.Rows.Count; recordCounter++)
                {
                    Contact_BE contactBusinessEntity = new Contact_BE();


                    contactBusinessEntity.ContactID = GetInt(dataTableAllContact.Rows[recordCounter]["ContactID"]);
                    contactBusinessEntity.UserID = GetInt(dataTableAllContact.Rows[recordCounter]["UserID"]);
                    contactBusinessEntity.BPNumber = Convert.ToString(dataTableAllContact.Rows[recordCounter]["BPNumber"]);
                    contactBusinessEntity.SAPBPShipTo = Convert.ToString(dataTableAllContact.Rows[recordCounter]["SAPBPShipTo"]);
                    contactBusinessEntity.Name = Convert.ToString(dataTableAllContact.Rows[recordCounter]["Name"]);
                    contactBusinessEntity.Email = Convert.ToString(dataTableAllContact.Rows[recordCounter]["Email"]);
                    contactBusinessEntity.Address1 = Convert.ToString(dataTableAllContact.Rows[recordCounter]["Address1"]);
                    contactBusinessEntity.Address2 = Convert.ToString(dataTableAllContact.Rows[recordCounter]["Address2"]);
                    contactBusinessEntity.City = Convert.ToString(dataTableAllContact.Rows[recordCounter]["City"]);
                    contactBusinessEntity.State = Convert.ToString(dataTableAllContact.Rows[recordCounter]["State"]);
                    contactBusinessEntity.CountryID = GetInt(dataTableAllContact.Rows[recordCounter]["CountryID"]);
                    contactBusinessEntity.Zip = Convert.ToString(dataTableAllContact.Rows[recordCounter]["Zip"]);
                    contactBusinessEntity.TelNumber = Convert.ToString(dataTableAllContact.Rows[recordCounter]["TelNumber"]);
                    contactBusinessEntity.FaxNumber = Convert.ToString(dataTableAllContact.Rows[recordCounter]["FaxNumber"]);
                    contactBusinessEntity.IsActive = GetBool(dataTableAllContact.Rows[recordCounter]["IsActive"]);
                    contactBusinessEntity.ContactTypeID = GetInt(dataTableAllContact.Rows[recordCounter]["ContactTypeID"]);
                    contactBusinessEntity.IsDefault = GetBool(dataTableAllContact.Rows[recordCounter]["IsDefault"]);

                    contactBusinessEntity.PKCountry_BE.CountryID = GetInt(dataTableAllContact.Rows[recordCounter]["CountryID"]);
                    contactBusinessEntity.PKCountry_BE.Code = Convert.ToString(dataTableAllContact.Rows[recordCounter]["Code"]);
                    contactBusinessEntity.PKCountry_BE.Name = Convert.ToString(dataTableAllContact.Rows[recordCounter]["CountryName"]);

                    ContactBusinessEntityList.Add(contactBusinessEntity);
                }
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion

        #region "Query Contact"
        /// <summary>
        /// Function to Get Contact details
        /// </summary>
        /// <param name="p_contact_BE"></param>
        /// <returns></returns>
        public int GetContact(Contact_BE contactBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlSelectCommand = string.Empty;
                DataTable dtAllContact = new DataTable();

                object[] param = new object[3] { contactBusinessEntity.UserID,
                                                GetString(contactBusinessEntity.Name),
                                                GetString(contactBusinessEntity.Email)
                                            };

                //CNameValueList cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intUserID", p_contactBE.UserID);
                //cNameValueList.Add("@chvName", p_contactBE.Name);
                //cNameValueList.Add("@chvEmail", p_contactBE.Email);

                dtAllContact = cDataSrc.ExecuteDataSet("UspGetContact", param, null).Tables[0];
                ShiftDataTableToBEList(dtAllContact);
                returnValue = 1;

                HandleWriteLogDAU("UspGetContact ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Insert records in Contact Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void AddContact(Contact_BE contactBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] {null,
                    contactBusinessEntity.UserID,
                    GetString(contactBusinessEntity.BPNumber),
                    GetString(contactBusinessEntity.SAPBPShipTo),
                    GetString(contactBusinessEntity.Name),
                    GetString(contactBusinessEntity.Email),
                    GetString(contactBusinessEntity.Address1),
                    GetString(contactBusinessEntity.Address2),
                    GetString(contactBusinessEntity.City),
                    GetString(contactBusinessEntity.State),
                     contactBusinessEntity.CountryID,
                    GetString(contactBusinessEntity.Zip),
                    GetString(contactBusinessEntity.TelNumber),
                    GetString(contactBusinessEntity.FaxNumber),
                    GetBool(contactBusinessEntity.IsActive),                    
                   contactBusinessEntity.ContactTypeID,
                    GetBool(contactBusinessEntity.IsDefault),
                    'I'                };

                //CNameValueList cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intUserID", p_contactBE.UserID.ToString().Trim());
                //cNameValueList.Add("@chvBPNumber", p_contactBE.BPNumber.ToString().Trim());
                //cNameValueList.Add("@chvSAPBPShipTo", p_contactBE.SAPBPShipTo.Trim());
                //cNameValueList.Add("@chvName", p_contactBE.Name.Trim());
                //cNameValueList.Add("@chvEmail", p_contactBE.Email.Trim());
                //cNameValueList.Add("@chvAddress1", p_contactBE.Address1.Trim());
                //cNameValueList.Add("@chvAddress2", p_contactBE.Address2.Trim());
                //cNameValueList.Add("@chvCity", p_contactBE.City.Trim());
                //cNameValueList.Add("@chvState", p_contactBE.State.Trim());
                //cNameValueList.Add("@intCountryID", p_contactBE.CountryID.ToString().Trim());
                //cNameValueList.Add("@chvZip", p_contactBE.Zip.Trim());
                //cNameValueList.Add("@chvTelNumber", p_contactBE.TelNumber.Trim());
                //cNameValueList.Add("@chvFaxNumber", p_contactBE.FaxNumber.Trim());
                //cNameValueList.Add("@bitIsActive", GetBool(p_contactBE.IsActive));
                //cNameValueList.Add("@intContactTypeID", p_contactBE.ContactTypeID.ToString().Trim());
                //cNameValueList.Add("@bitIsDefault", GetBool(p_contactBE.IsDefault));
                //cNameValueList.Add("@chvFlag", 'I');

                cDataSrc.ExecuteNonQuery("UspContactMaintenance", param, null);
                cDataSrc = null;
                HandleWriteLogDAU("UspContactMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// insert user contact
        /// </summary>
        /// <param name="contactBusinessEntityList"></param>
        /// <param name="p_userID"></param>
        public void AddContact(List<Contact_BE> contactBusinessEntityList, int p_userID)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                string flag = string.Empty;
                int userID, contactID;

                foreach (Contact_BE contact_BE in contactBusinessEntityList)
                {
                    flag = contact_BE.Status == Contact_BE.statusType.New ? "I" : contact_BE.Status == Contact_BE.statusType.Delete ? "D" : "U";
                    userID = contact_BE.UserID.HasValue && contact_BE.UserID.Value != 0 ? contact_BE.UserID.Value : p_userID;
                    contactID = contact_BE.ContactID.HasValue ? contact_BE.ContactID.Value : 0;

                    object[] param = new object[18] 
                    { 
                        contactID,
                        userID,
                        GetString(contact_BE.BPNumber),
                        GetString(contact_BE.SAPBPShipTo),
                        GetString(contact_BE.Name),
                        GetString(contact_BE.Email),
                        GetString(contact_BE.Address1),
                        GetString(contact_BE.Address2),
                        GetString(contact_BE.City),
                        GetString(contact_BE.State),
                         contact_BE.CountryID,
                        GetString(contact_BE.Zip),
                        GetString(contact_BE.TelNumber),
                        GetString(contact_BE.FaxNumber),
                        contact_BE.IsActive,
                        contact_BE.ContactTypeID,
                        contact_BE.IsDefault,
                        flag
                    };



                    //CNameValueList cNameValueList = new CNameValueList();
                    //cNameValueList.Add("@intContactID", contactID);
                    //cNameValueList.Add("@intUserID", userID);
                    //cNameValueList.Add("@chvBPNumber", contact_BE.BPNumber.ToString().Trim());
                    //cNameValueList.Add("@chvSAPBPShipTo", contact_BE.SAPBPShipTo.Trim());
                    //cNameValueList.Add("@chvName", contact_BE.Name.Trim());
                    //cNameValueList.Add("@chvEmail", contact_BE.Email.Trim());
                    //cNameValueList.Add("@chvAddress1", contact_BE.Address1.Trim());
                    //cNameValueList.Add("@chvAddress2", contact_BE.Address2.Trim());
                    //cNameValueList.Add("@chvCity", contact_BE.City.Trim());
                    //cNameValueList.Add("@chvState", contact_BE.State.Trim());
                    //cNameValueList.Add("@intCountryID", contact_BE.CountryID.ToString().Trim());
                    //cNameValueList.Add("@chvZip", contact_BE.Zip.Trim());
                    //cNameValueList.Add("@chvTelNumber", contact_BE.TelNumber.Trim());
                    //cNameValueList.Add("@chvFaxNumber", contact_BE.FaxNumber.Trim());
                    //cNameValueList.Add("@bitIsActive", GetBool(contact_BE.IsActive));
                    //cNameValueList.Add("@intContactTypeID", contact_BE.ContactTypeID.ToString().Trim());
                    //cNameValueList.Add("@bitIsDefault", GetBool(contact_BE.IsDefault));
                    // cNameValueList.Add("@chvFlag", flag);


                    int iRes = cDataSrc.ExecuteNonQuery("UspContactMaintenance", param, null);
                    HandleWriteLogDAU("UspContactMaintenance ", param, new StackTrace(true));
                }

                cDataSrc = null;


                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to Update records in Contact Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>        
        public void UpdateContact(Contact_BE contactBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] {  contactBusinessEntity.ContactID.ToString(),
                    contactBusinessEntity.UserID,
                    GetString(contactBusinessEntity.BPNumber),
                    GetString(contactBusinessEntity.SAPBPShipTo),
                    GetString(contactBusinessEntity.Name),
                    GetString(contactBusinessEntity.Email),
                    GetString(contactBusinessEntity.Address1),
                    GetString(contactBusinessEntity.Address2),
                    GetString(contactBusinessEntity.City),
                    GetString(contactBusinessEntity.State),
                    contactBusinessEntity.CountryID,
                    GetString(contactBusinessEntity.Zip),
                    GetString(contactBusinessEntity.TelNumber),
                    GetString(contactBusinessEntity.FaxNumber),
                    true,
                    contactBusinessEntity.ContactTypeID,
                    GetBool(contactBusinessEntity.IsDefault),
                    'U'
                };

                cDataSrc.ExecuteNonQuery("UspContactMaintenance", param, null);
                cDataSrc = null;
                HandleWriteLogDAU("UspContactMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to Delete records in Contact Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>        
        public void DeleteContact(Contact_BE contactBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] { contactBusinessEntity.ContactID.ToString(),
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
                                                    'D' };
                CNameValueList cNameValueList = new CNameValueList();
                cNameValueList.Add("@intContactID", contactBusinessEntity.ContactID);
                cNameValueList.Add("@chvFlag", 'D');
                cDataSrc.ExecuteNonQuery("UspContactMaintenance", cNameValueList, null);
                cDataSrc = null;
                HandleWriteLogDAU("UspContactMaintenance ", cNameValueList, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        #endregion
    }
}
