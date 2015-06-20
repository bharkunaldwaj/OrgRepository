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
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using feedbackFramework_BE;
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
        public List<Contact_BE> Contact_BEList { get; set; }
        #endregion

        #region Private Member Variables
        private int returnValue;
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to BEArray object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAllContact)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Contact_BEList = new List<Contact_BE>();
                for (int recordCounter = 0; recordCounter < p_dtAllContact.Rows.Count; recordCounter++)
                {
                    Contact_BE contact_BE = new Contact_BE();


                    contact_BE.ContactID = GetInt(p_dtAllContact.Rows[recordCounter]["ContactID"]);
                    contact_BE.UserID = GetInt(p_dtAllContact.Rows[recordCounter]["UserID"]);
                    contact_BE.BPNumber = Convert.ToString(p_dtAllContact.Rows[recordCounter]["BPNumber"]);
                    contact_BE.SAPBPShipTo = Convert.ToString(p_dtAllContact.Rows[recordCounter]["SAPBPShipTo"]);
                    contact_BE.Name = Convert.ToString(p_dtAllContact.Rows[recordCounter]["Name"]);
                    contact_BE.Email = Convert.ToString(p_dtAllContact.Rows[recordCounter]["Email"]);
                    contact_BE.Address1 = Convert.ToString(p_dtAllContact.Rows[recordCounter]["Address1"]);
                    contact_BE.Address2 = Convert.ToString(p_dtAllContact.Rows[recordCounter]["Address2"]);
                    contact_BE.City = Convert.ToString(p_dtAllContact.Rows[recordCounter]["City"]);
                    contact_BE.State = Convert.ToString(p_dtAllContact.Rows[recordCounter]["State"]);
                    contact_BE.CountryID = GetInt(p_dtAllContact.Rows[recordCounter]["CountryID"]);
                    contact_BE.Zip = Convert.ToString(p_dtAllContact.Rows[recordCounter]["Zip"]);
                    contact_BE.TelNumber = Convert.ToString(p_dtAllContact.Rows[recordCounter]["TelNumber"]);
                    contact_BE.FaxNumber = Convert.ToString(p_dtAllContact.Rows[recordCounter]["FaxNumber"]);
                    contact_BE.IsActive = GetBool(p_dtAllContact.Rows[recordCounter]["IsActive"]);
                    contact_BE.ContactTypeID = GetInt(p_dtAllContact.Rows[recordCounter]["ContactTypeID"]);
                    contact_BE.IsDefault = GetBool(p_dtAllContact.Rows[recordCounter]["IsDefault"]);

                    contact_BE.PKCountry_BE.CountryID = GetInt(p_dtAllContact.Rows[recordCounter]["CountryID"]);
                    contact_BE.PKCountry_BE.Code = Convert.ToString(p_dtAllContact.Rows[recordCounter]["Code"]);
                    contact_BE.PKCountry_BE.Name = Convert.ToString(p_dtAllContact.Rows[recordCounter]["CountryName"]);

                    Contact_BEList.Add(contact_BE);
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
        public int GetContact(Contact_BE p_contactBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                string sqlSelectCommand = string.Empty;
                DataTable dtAllContact = new DataTable();

                object[] param = new object[3] { p_contactBE.UserID,
                                                GetString(p_contactBE.Name),
                                                GetString(p_contactBE.Email)
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
        public void AddContact(Contact_BE p_contactBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[18] {null,
                    p_contactBE.UserID,
                    GetString(p_contactBE.BPNumber),
                    GetString(p_contactBE.SAPBPShipTo),
                    GetString(p_contactBE.Name),
                    GetString(p_contactBE.Email),
                    GetString(p_contactBE.Address1),
                    GetString(p_contactBE.Address2),
                    GetString(p_contactBE.City),
                    GetString(p_contactBE.State),
                     p_contactBE.CountryID,
                    GetString(p_contactBE.Zip),
                    GetString(p_contactBE.TelNumber),
                    GetString(p_contactBE.FaxNumber),
                    GetBool(p_contactBE.IsActive),                    
                   p_contactBE.ContactTypeID,
                    GetBool(p_contactBE.IsDefault),
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
        /// 
        /// </summary>
        /// <param name="p_contactBEList"></param>
        /// <param name="p_userID"></param>
        public void AddContact(List<Contact_BE> p_contactBEList, int p_userID) {
            try {
                HandleWriteLog("Start", new StackTrace(true));

                string flag = string.Empty;
                int userID,contactID;

                foreach (Contact_BE contact_BE in p_contactBEList) {
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
        public void UpdateContact(Contact_BE p_contactBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[18] {  p_contactBE.ContactID.ToString(),
                    p_contactBE.UserID,
                    GetString(p_contactBE.BPNumber),
                    GetString(p_contactBE.SAPBPShipTo),
                    GetString(p_contactBE.Name),
                    GetString(p_contactBE.Email),
                    GetString(p_contactBE.Address1),
                    GetString(p_contactBE.Address2),
                    GetString(p_contactBE.City),
                    GetString(p_contactBE.State),
                    p_contactBE.CountryID,
                    GetString(p_contactBE.Zip),
                    GetString(p_contactBE.TelNumber),
                    GetString(p_contactBE.FaxNumber),
                    true,
                    p_contactBE.ContactTypeID,
                    GetBool(p_contactBE.IsDefault),
                    'U'
                };

                cDataSrc.ExecuteNonQuery("UspContactMaintenance", param, null);
                cDataSrc = null;
                HandleWriteLogDAU("UspContactMaintenance ",param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to Delete records in Contact Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>        
        public void DeleteContact(Contact_BE p_contactBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                
                object[] param = new object[18] { p_contactBE.ContactID.ToString(),
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
                cNameValueList.Add("@intContactID", p_contactBE.ContactID);
                cNameValueList.Add("@chvFlag",'D');
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
