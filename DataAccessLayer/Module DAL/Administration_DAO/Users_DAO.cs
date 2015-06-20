/*
* PURPOSE: Data Access Object for User Entity
* AUTHOR: Manish Mathur
* Date Of Creation: <30/08/2010>
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*               <Key2><Reason 2 >
*/

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

using Administration_BE;
using DatabaseAccessUtilities;

namespace Administration_DAO {
    public class User_DAO : DAO_Base {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables
        private int returnValue;
        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public User_DAO() {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));
        }
        #endregion

        #region "Public Properties"
        public List<User_BE> User_BEList { get; set; }
        #endregion

        #region Private Methods

        /// <summary>
        /// Function to store DataTable data to User_BE object
        /// </summary>
        /// <param name="p_dtAlluser"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAlluser) {
            HandleWriteLog("Start", new StackTrace(true));
            User_BEList = new List<User_BE>();

            for (int recordCounter = 0; recordCounter < p_dtAlluser.Rows.Count; recordCounter++) {
                User_BE user_BE = new User_BE();

                user_BE.UserID = GetInt(p_dtAlluser.Rows[recordCounter]["UserID"]);
                user_BE.GroupID = GetInt(p_dtAlluser.Rows[recordCounter]["GroupID"]);
                user_BE.LoginID = Convert.ToString(p_dtAlluser.Rows[recordCounter]["LoginID"]);
                
                user_BE.Password = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Password"]);
                user_BE.FName = Convert.ToString(p_dtAlluser.Rows[recordCounter]["FirstName"]);

                user_BE.LName = Convert.ToString(p_dtAlluser.Rows[recordCounter]["LastName"]);
                user_BE.Email = Convert.ToString(p_dtAlluser.Rows[recordCounter]["EmailID"]);
                //user_BE.IsActive = GetBool(p_dtAlluser.Rows[recordCounter]["IsActive"]);
                //user_BE.Address1 = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Address1"]);
                //user_BE.Address2 = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Address2"]);
                //user_BE.City = Convert.ToString(p_dtAlluser.Rows[recordCounter]["City"]);
                //user_BE.State = Convert.ToString(p_dtAlluser.Rows[recordCounter]["State"]);
                //user_BE.CountryID = GetInt(p_dtAlluser.Rows[recordCounter]["CountryID"]);
                //user_BE.Zip = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Zip"]);
                //user_BE.TelNumber = Convert.ToString(p_dtAlluser.Rows[recordCounter]["TelNumber"]);
                //user_BE.FaxNumber = Convert.ToString(p_dtAlluser.Rows[recordCounter]["FaxNumber"]);
                //user_BE.Website = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Website"]);
                //user_BE.Note = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Note"]);
                //user_BE.CreatedDate = GetDateTime(p_dtAlluser.Rows[recordCounter]["CreatedDate"]);
                //user_BE.ModifiedDate = GetDateTime(p_dtAlluser.Rows[recordCounter]["ModifiedDate"]);
                //user_BE.IsConfirmed = GetBool(p_dtAlluser.Rows[recordCounter]["IsConfirmed"]);
                //user_BE.Type = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Type"]);
                //user_BE.BPNumber = Convert.ToString(p_dtAlluser.Rows[recordCounter]["BPNumber"]);

                //user_BE.PKGroup_BE.GroupID = GetInt(p_dtAlluser.Rows[recordCounter]["GroupID"]);
                //user_BE.PKGroup_BE.GroupName = Convert.ToString(p_dtAlluser.Rows[recordCounter]["GroupName"]);
                //user_BE.PKGroup_BE.Description = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Description"]);
                //user_BE.PKGroup_BE.WelcomeText = Convert.ToString(p_dtAlluser.Rows[recordCounter]["WelcomeText"]);
                //user_BE.PKGroup_BE.NewsText = Convert.ToString(p_dtAlluser.Rows[recordCounter]["NewsText"]);
                //user_BE.PKGroup_BE.IsActive = GetBool(p_dtAlluser.Rows[recordCounter]["Group_IsActive"]);
                //user_BE.PKGroup_BE.CreatedDate = GetDateTime(p_dtAlluser.Rows[recordCounter]["Group_CreatedDate"]);
                //user_BE.PKGroup_BE.ModifiedDate = GetDateTime(p_dtAlluser.Rows[recordCounter]["Group_ModifiedDate"]);

                //user_BE.PKCountry_BE.CountryID = GetInt(p_dtAlluser.Rows[recordCounter]["CountryID"]);
                //user_BE.PKCountry_BE.Code = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Code"]);
                //user_BE.PKCountry_BE.Name = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Name"]);

                user_BE.AccountID = GetInt(p_dtAlluser.Rows[recordCounter]["AccountID"]);
                user_BE.AccountCode = Convert.ToString(p_dtAlluser.Rows[recordCounter]["Code"]);
                user_BE.AccountName = Convert.ToString(p_dtAlluser.Rows[recordCounter]["OrganisationName"]);
                user_BE.CompanyLogo = Convert.ToString(p_dtAlluser.Rows[recordCounter]["CompanyLogo"]);
                user_BE.HeaderBGColor = Convert.ToString(p_dtAlluser.Rows[recordCounter]["HeaderBGColor"]);
                user_BE.MenuBGColor = Convert.ToString(p_dtAlluser.Rows[recordCounter]["MenuBGColor"]);
                user_BE.CopyRightLine = Convert.ToString(p_dtAlluser.Rows[recordCounter]["CopyRightLine"]);
                
                User_BEList.Add(user_BE);
            }
            HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Query User"

        /// <summary>
        /// Function to Get User details
        /// </summary>
        /// <param name="p_user_BE"></param>
        /// <returns></returns>
        public int GetUser(User_BE user_BE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = new object[3] {user_BE.LoginID,
                                                user_BE.Password,
                                                user_BE.AccountCode};

                dtAlluser = cDataSrc.ExecuteDataSet("UspGetUser", param, null).Tables[0];
                if (dtAlluser.Rows.Count == 0)
                {
                    dtAlluser = cDataSrc.ExecuteDataSet("Survey_UspGetUser", param, null).Tables[0];
                }
                ShiftDataTableToBEList(dtAlluser);
                
                returnValue = 1;

                //HandleWriteLogDAU("UspGetUser ", cNameValueList, new StackTrace(true));
                HandleWriteLogDAU("UspGetUser ", param, new StackTrace(true));
                HandleWriteLogDAU("Survey_UspGetUser ", param, new StackTrace(true));

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public int GetUsers(User_BE p_userBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dtAlluser = new DataTable();
                object[] param = null;
                //CNameValueList cNameValueList = null;
                param = new object[6] { GetString(p_userBE.FName),
                                        GetString(p_userBE.Email),
                                        p_userBE.CountryID,
                                        0, 
                                        p_userBE.PageSize,
                                        "Count" };

                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@chvFirstName", p_userBE.FName.ToString().Replace("'", "''").Trim());
                //cNameValueList.Add("@chvEmail", p_userBE.Email.ToString().Replace("'", "''").Trim());
                //cNameValueList.Add("@intCountryID", p_userBE.CountryID);
                //cNameValueList.Add("@intPage", 0);
                //cNameValueList.Add("@intPageSize", p_userBE.PageSize);
                //cNameValueList.Add("@chvFlag", "Count");

                int count = Convert.ToInt32(cDataSrc.ExecuteScalar("UspGetVendorDetails", param, null));
                //int count = Convert.ToInt32(cDataSrc.ExecuteScalar("UspGetVendorDetails", cNameValueList, null));

                if (count > Convert.ToInt32(ConfigurationSettings.AppSettings["NoOfRecords"])) {
                    param = new object[6] { GetString(p_userBE.FName),
                                            GetString(p_userBE.Email),
                                            p_userBE.CountryID,
                                            p_userBE.PageIndex, 
                                            p_userBE.PageSize,
                                            "" };

                    //cNameValueList = new CNameValueList();
                    //cNameValueList.Add("@chvFirstName", p_userBE.FName.ToString().Replace("'", "''").Trim());
                    //cNameValueList.Add("@chvEmail", p_userBE.Email.ToString().Replace("'", "''").Trim());
                    //cNameValueList.Add("@intCountryID", p_userBE.CountryID);
                    //cNameValueList.Add("@intPage", p_userBE.PageIndex);
                    //cNameValueList.Add("@intPageSize", p_userBE.PageSize);
                    //cNameValueList.Add("@chvFlag", "");

                    dtAlluser = null;
                    dtAlluser = cDataSrc.ExecuteDataSet("UspGetVendorDetails", param, null).Tables[0];
                    //dtAlluser = cDataSrc.ExecuteDataSet("UspGetVendorDetails", cNameValueList, null).Tables[0];
                }
                else {
                    param = new object[6] { GetString(p_userBE.FName),
                                            GetString(p_userBE.Email),
                                            p_userBE.CountryID,
                                            0,
                                            p_userBE.PageSize, 
                                            "" };

                    //cNameValueList = new CNameValueList();
                    //cNameValueList.Add("@chvFirstName", p_userBE.FName.ToString().Replace("'", "''").Trim());
                    //cNameValueList.Add("@chvEmail", p_userBE.Email.ToString().Replace("'", "''").Trim());
                    //cNameValueList.Add("@intCountryID", p_userBE.CountryID);
                    //cNameValueList.Add("@intPage", 0);
                    //cNameValueList.Add("@intPageSize", p_userBE.PageSize);
                    //cNameValueList.Add("@chvFlag", "");

                    dtAlluser = null;
                    dtAlluser = cDataSrc.ExecuteDataSet("UspGetVendorDetails", param, null).Tables[0];
                    //dtAlluser = cDataSrc.ExecuteDataSet("UspGetVendorDetails", cNameValueList, null).Tables[0];
                }

                ShiftDataTableToBEList(dtAlluser);
                returnValue = 1;

                //HandleWriteLogDAU("UspGetVendorDetails", cNameValueList, new StackTrace(true));
                HandleWriteLogDAU("UspGetVendorDetails", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Insert records in User Entity to create new user
        /// </summary>
        /// <param name="p_user_BE"></param>
        /// <returns></returns>
        public int AddUser(User_BE p_userBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[26] { null,
                                                p_userBE.GroupID,
                                                p_userBE.LoginID,
                                                GetString(p_userBE.UserCode),
                                                GetString(p_userBE.Password) ,
                                                GetString(p_userBE.FName),
                                                GetString(p_userBE.MName),
                                                GetString(p_userBE.LName),
                                                GetString(p_userBE.Email),
                                                GetBool(p_userBE.IsActive),
                                                GetString(p_userBE.Address1) ,
                                                GetString(p_userBE.Address2) ,
                                                GetString(p_userBE.City),
                                                GetString(p_userBE.State) ,
                                                p_userBE.CountryID.ToString().Trim(),
                                                GetString(p_userBE.Zip) ,
                                                GetString(p_userBE.TelNumber) ,
                                                GetString(p_userBE.FaxNumber),
                                                GetString(p_userBE.Website),
                                                GetString(p_userBE.Note),
                                                p_userBE.CreatedDate,
                                                p_userBE.ModifiedDate,                                                
                                                GetBool(p_userBE.IsConfirmed) ,
                                                GetString(p_userBE.Type) ,
                                                GetString(p_userBE.BPNumber) ,
                                                "I" };


                //CNameValueList cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intGroupID", p_userBE.GroupID.ToString().Trim());
                //cNameValueList.Add("@chvLoginID", p_userBE.LoginID.ToString().Trim());
                //cNameValueList.Add("@chvUserCode", p_userBE.UserCode.Trim());
                //cNameValueList.Add("@chvPassword", p_userBE.Password.Trim());
                //cNameValueList.Add("@chvFName", p_userBE.FName.Trim());
                //cNameValueList.Add("@chvMName", p_userBE.MName.Trim());
                //cNameValueList.Add("@chvLName", p_userBE.LName.Trim());
                //cNameValueList.Add("@chvEmail", p_userBE.Email.Trim());
                //cNameValueList.Add("@bitIsActive", GetBool(p_userBE.IsActive));
                //cNameValueList.Add("@chvAddress1", p_userBE.Address1.Trim());
                //cNameValueList.Add("@chvAddress2", p_userBE.Address2.Trim());
                //cNameValueList.Add("@chvCity", p_userBE.City.Trim());
                //cNameValueList.Add("@chvState", p_userBE.State.Trim());
                //cNameValueList.Add("@intCountryID", p_userBE.CountryID.ToString().Trim());
                //cNameValueList.Add("@chvZip", p_userBE.Zip.Trim());
                //cNameValueList.Add("@chvTelNumber", p_userBE.TelNumber.Trim());
                //cNameValueList.Add("@chvFaxNumber", p_userBE.FaxNumber.Trim());
                //cNameValueList.Add("@chvWebsite", p_userBE.Website.Trim());
                //cNameValueList.Add("@chvNote", p_userBE.Note.Trim());
                //cNameValueList.Add("@dtmCreatedDate", p_userBE.CreatedDate);
                //cNameValueList.Add("@dtmModifiedDate", p_userBE.ModifiedDate);
                //cNameValueList.Add("@bitIsConfirmed", GetBool(p_userBE.IsConfirmed));
                //cNameValueList.Add("@chvType", p_userBE.Type.Trim());
                //cNameValueList.Add("@chvBPNumber", p_userBE.BPNumber.Trim());
                //cNameValueList.Add("@chvFlag", 'I');

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspUserMaintenance", param, null));

               // returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspUserMaintenance", cNameValueList, null));

                cDataSrc = null;

                HandleWriteLogDAU("UspUserMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        /// <summary>
        /// Function to Update user details for User Entity
        /// </summary>
        /// <param name="p_user_BE"></param>
        public void UpdateUser(User_BE p_userBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[26] { p_userBE.UserID,
                                    p_userBE.GroupID,
                                    p_userBE.LoginID,
                                    GetString(p_userBE.UserCode),
                                    GetString(p_userBE.Password) ,
                                    GetString(p_userBE.FName),
                                    GetString(p_userBE.MName),
                                    GetString(p_userBE.LName),
                                    GetString(p_userBE.Email),
                                    GetBool(p_userBE.IsActive),
                                    GetString(p_userBE.Address1) ,
                                    GetString(p_userBE.Address2) ,
                                    GetString(p_userBE.City),
                                    GetString(p_userBE.State) ,
                                    p_userBE.CountryID,
                                    GetString(p_userBE.Zip) ,
                                    GetString(p_userBE.TelNumber) ,
                                    GetString(p_userBE.FaxNumber),
                                    GetString(p_userBE.Website),
                                    GetString(p_userBE.Note),
                                    Convert.ToDateTime(p_userBE.CreatedDate),
                                    Convert.ToDateTime(p_userBE.ModifiedDate),                                                
                                    GetBool(p_userBE.IsConfirmed) ,
                                    GetString(p_userBE.Type) ,
                                    GetString(p_userBE.BPNumber) ,
                                    "U" };


                //CNameValueList cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intUserID", p_userBE.UserID.ToString().Trim());
                //cNameValueList.Add("@intGroupID", p_userBE.GroupID.ToString().Trim());
                //cNameValueList.Add("@chvLoginID", p_userBE.LoginID.ToString().Trim());
                //cNameValueList.Add("@chvUserCode", p_userBE.UserCode.Trim());
                //cNameValueList.Add("@chvPassword", p_userBE.Password.Trim());
                //cNameValueList.Add("@chvFName", p_userBE.FName.Trim());
                //cNameValueList.Add("@chvMName", p_userBE.MName.Trim());
                //cNameValueList.Add("@chvLName", p_userBE.LName.Trim());
                //cNameValueList.Add("@chvEmail", p_userBE.Email.Trim());
                //cNameValueList.Add("@bitIsActive", GetBool(p_userBE.IsActive));
                //cNameValueList.Add("@chvAddress1", p_userBE.Address1.Trim());
                //cNameValueList.Add("@chvAddress2", p_userBE.Address2.Trim());
                //cNameValueList.Add("@chvCity", p_userBE.City.Trim());
                //cNameValueList.Add("@chvState", p_userBE.State.Trim());
                //cNameValueList.Add("@intCountryID", p_userBE.CountryID.ToString().Trim());
                //cNameValueList.Add("@chvZip", p_userBE.Zip.Trim());
                //cNameValueList.Add("@chvTelNumber", p_userBE.TelNumber.Trim());
                //cNameValueList.Add("@chvFaxNumber", p_userBE.FaxNumber.Trim());
                //cNameValueList.Add("@chvWebsite", p_userBE.Website.Trim());
                //cNameValueList.Add("@chvNote", p_userBE.Note.Trim());
                //cNameValueList.Add("@dtmCreatedDate", p_userBE.CreatedDate);
                //cNameValueList.Add("@dtmModifiedDate", p_userBE.ModifiedDate);
                //cNameValueList.Add("@bitIsConfirmed", GetBool(p_userBE.IsConfirmed));
                //cNameValueList.Add("@chvType", p_userBE.Type.Trim());
                //cNameValueList.Add("@chvBPNumber", p_userBE.BPNumber.Trim());
                //cNameValueList.Add("@chvFlag", "U");

                cDataSrc.ExecuteNonQuery("UspUserMaintenance", param, null);
               
                cDataSrc = null;
                HandleWriteLogDAU("UspUserMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        public void UpdateUserSession(User_BE p_userBE)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[2] { p_userBE.UserID,
                                    GetString(p_userBE.SessionData)
                                    };



                cDataSrc.ExecuteNonQuery("PersonalityUspUserUpdateSession", param, null);

                cDataSrc = null;
                HandleWriteLogDAU("UspUserMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to Delete user recordsin User Entity
        /// </summary>
        /// <param name="p_user_BE"></param>
        public void DeleteUser(User_BE p_userBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[26] { p_userBE.UserID ,
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
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    null,
                                                    "D"};

                //CNameValueList cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intUserID", p_userBE.UserID.ToString());
                //cNameValueList.Add("@chvFlag", "D");

                cDataSrc.ExecuteNonQuery("UspUserMaintenance", param, null);

                //cDataSrc.ExecuteNonQuery("UspUserMaintenance", cNameValueList, null);
                cDataSrc = null;
                HandleWriteLogDAU("UspUserMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        #endregion
    }
}

