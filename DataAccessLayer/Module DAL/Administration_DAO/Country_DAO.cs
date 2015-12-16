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

namespace Administration_DAO
{
    public class Country_DAO : DAO_Base
    {

        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region "Public Properties"
        //public Country_BE[] Country_BEArray { get; set; }
        public List<Country_BE> CountryBusinessEntityList { get; set; }
        #endregion

        #region Private Member Variables
        private int returnValue;
        #endregion

        #region Private Methods
        /// <summary>
        /// Function to store DataTable data to BEArray object
        /// </summary>
        /// <param name="p_contact_BE"></param>
        private void ShiftDataTableToBEList(DataTable dataTableAllCountry)
        {
            HandleWriteLog("Start", new StackTrace(true));
            CountryBusinessEntityList = new List<Country_BE>();
            for (int recordCounter = 0; recordCounter < dataTableAllCountry.Rows.Count; recordCounter++)
            {
                Country_BE countryBusinessEntity = new Country_BE();

                countryBusinessEntity.CountryID = GetInt(dataTableAllCountry.Rows[recordCounter]["CountryID"]);
                countryBusinessEntity.Code = Convert.ToString(dataTableAllCountry.Rows[recordCounter]["Code"]);
                countryBusinessEntity.Name = Convert.ToString(dataTableAllCountry.Rows[recordCounter]["Name"]);
                CountryBusinessEntityList.Add(countryBusinessEntity);
            }
            HandleWriteLog("End", new StackTrace(true));
        }

        #endregion

        #region "Public Constructor"

        /// <summary>
        /// Public Constructor
        /// </summary>
        public Country_DAO()
        {
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
        public int GetCountry(Country_BE countryBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                DataTable dataTableAllCountry = new DataTable();
                object[] param = new object[3] { countryBusinessEntity.CountryID,
                                            GetString(countryBusinessEntity.Code),
                                            GetString(countryBusinessEntity.Name),
                                            };

                //CNameValueList cNameValueList = null;
                //cNameValueList = new CNameValueList();
                //cNameValueList.Add("@intCountryID", p_countryBE.CountryID);
                //cNameValueList.Add("@chvCode", p_countryBE.Code.Trim());
                //cNameValueList.Add("@chvName", p_countryBE.Name.Trim());

                dataTableAllCountry = cDataSrc.ExecuteDataSet("UspGetCountry", param, null).Tables[0];

                //dtAllCountry = cDataSrc.ExecuteDataSet("UspGetCountry", cNameValueList, null).Tables[0];

                ShiftDataTableToBEList(dataTableAllCountry);
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
        public void AddCountry(Country_BE countryBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[4] {countryBusinessEntity.CountryID,
                      GetString(countryBusinessEntity.Code),
                      GetString(countryBusinessEntity.Name),
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
        public void UpdateCountry(Country_BE countryBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                object[] param = new object[4] {countryBusinessEntity.CountryID,
                      GetString(countryBusinessEntity.Code),
                      GetString(countryBusinessEntity.Name),
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
        public void DeleteCountry(Country_BE countryBusinessEntity)
        {
            try
            {
                HandleWriteLog("Start", new StackTrace(true));

                object[] param = new object[4] {countryBusinessEntity.CountryID.ToString().Trim(),
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
