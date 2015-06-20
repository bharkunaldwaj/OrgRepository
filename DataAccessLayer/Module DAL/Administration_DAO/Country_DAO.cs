/*
* PURPOSE: Data Access Object for Country Entity
* AUTHOR:  Manish Mathur
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
    public class Country_DAO : DAO_Base {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Properties"
        //public Country_BE[] Country_BEArray { get; set; }
        public List<Country_BE> Country_BEList { get; set; }
        #endregion

        #region Private Member Variables
        private int returnValue;
        #endregion

        #region Private Methods
        /// <summary>
        /// Function to store DataTable data to BEArray object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable p_dtAllCountry) {
            HandleWriteLog("Start", new StackTrace(true));
            Country_BEList = new List<Country_BE>();
            for (int recordCounter = 0; recordCounter < p_dtAllCountry.Rows.Count; recordCounter++) {
                Country_BE country_BE = new Country_BE();

                country_BE.CountryID = GetInt(p_dtAllCountry.Rows[recordCounter]["CountryID"]);
                country_BE.Code = Convert.ToString(p_dtAllCountry.Rows[recordCounter]["Code"]);
                country_BE.Name = Convert.ToString(p_dtAllCountry.Rows[recordCounter]["Name"]);
                Country_BEList.Add(country_BE);
            }
            HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Country_DAO() {
            HandleWriteLog("Start", new StackTrace(true));
            HandleWriteLog("End", new StackTrace(true));

        }
        #endregion

        #region "Query Country"

        /// <summary>
        /// Function to Get Country details
        /// </summary>
        /// <param name="p_contact_BE"></param>
        /// <returns></returns>
        public int GetCountry(Country_BE p_countryBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));                
                DataTable dtAllCountry = new DataTable();                
                object[] param = new object[3] { p_countryBE.CountryID,
                                            GetString(p_countryBE.Code),
                                            GetString(p_countryBE.Name),
                                            };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intCountryID", p_countryBE.CountryID);
                //cNameValueList.Add("@chvCode", p_countryBE.Code.Trim());
                //cNameValueList.Add("@chvName", p_countryBE.Name.Trim());

                dtAllCountry = cDataSrc.ExecuteDataSet("UspGetCountry", param, null).Tables[0];

                //dtAllCountry = cDataSrc.ExecuteDataSet("UspGetCountry", cNameValueList, null).Tables[0];

                ShiftDataTableToBEList(dtAllCountry);
                returnValue = 1;
                HandleWriteLogDAU("UspGetCountry ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Function to Insert records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void AddCountry(Country_BE p_countryBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));             
                object[] param = new object[4] {p_countryBE.CountryID,
                      GetString(p_countryBE.Code),
                      GetString(p_countryBE.Name),
                      'I'
                };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intCountryID", p_countryBE.CountryID);
                //cNameValueList.Add("@chvCode", p_countryBE.Code.Trim());
                //cNameValueList.Add("@chvName", p_countryBE.Name.Trim());
                //cNameValueList.Add("@chvFlag", 'I');
             
                
                
                cDataSrc.ExecuteNonQuery("UspCountryMaintenance", param, null);

                //cDataSrc.ExecuteNonQuery("UspCountryMaintenance", cNameValueList, null);

                cDataSrc = null;
                HandleWriteLogDAU("UspCountryMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to Update records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void UpdateCountry(Country_BE p_countryBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[4] {p_countryBE.CountryID,
                      GetString(p_countryBE.Code),
                      GetString(p_countryBE.Name),
                      'U'
                };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intCountryID", p_countryBE.CountryID);
                //cNameValueList.Add("@chvCode", p_countryBE.Code.Trim());
                //cNameValueList.Add("@chvName", p_countryBE.Name.Trim());
                //cNameValueList.Add("@chvFlag", 'U');

                cDataSrc.ExecuteNonQuery("UspCountryMaintenance", param, null);

                //cDataSrc.ExecuteNonQuery("UspCountryMaintenance", cNameValueList, null);
                cDataSrc = null;
                HandleWriteLogDAU("UspCountryMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Function to Delete records in Country Entity
        /// </summary>
        /// <param name="p_contact_BE"></param>
        public void DeleteCountry(Country_BE p_countryBE) {
            try {
                HandleWriteLog("Start", new StackTrace(true));                             

                object[] param = new object[4] {p_countryBE.CountryID.ToString().Trim(),
                                                null,
                                                null,
                                                'D' };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intCountryID", p_countryBE.CountryID);
                //cNameValueList.Add("@chvFlag", 'D');

                cDataSrc.ExecuteNonQuery("UspCountryMaintenance", param, null);

                //cDataSrc.ExecuteNonQuery("UspCountryMaintenance", cNameValueList, null);
                cDataSrc = null;
                HandleWriteLogDAU("UspCountryMaintenance ", param, new StackTrace(true));
                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) { HandleException(ex); }
        }

        #endregion
    }
}
