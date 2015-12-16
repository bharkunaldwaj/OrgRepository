/*  
* PURPOSE: This is the Business Access Object for Contact Entity
* AUTHOR: Manish Mathur
* Date Of Creation: 30/08/2010
* Modification Details
*      Date: <dd/mm/yyyy> Author :: < Name of the author >
*      Reasons: <Key1><Reason 1 >
*                    <Key2><Reason 2 >
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

using DAF_BAO;

using Administration_BE;
using Administration_DAO;
using DatabaseAccessUtilities;
using System.Data;


namespace Administration_BAO
{
    public class Contact_BAO : Base_BAO
    {

        #region "Business Logic for Contact BAO"

        /// <summary>
        /// This Method returns the Contacts from Contact_DAO on passing Contact_BE as a parameter
        /// </summary>
        /// <param name="p_Contact_BE"></param>
        /// <returns></returns>
        public List<Contact_BE> GetContact(Contact_BE contactBusinessEntity)
        {
            List<Contact_BE> contactBusinessEntityList = null;
            try
            {
                HandleWriteLog("Start", new StackTrace(true));
                Contact_DAO contactDataAccessObject = new Contact_DAO();

                contactDataAccessObject.GetContact(contactBusinessEntity);
                contactBusinessEntityList = contactDataAccessObject.ContactBusinessEntityList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return contactBusinessEntityList;

        }

        /// <summary>
        /// This method passes the Contact_BE entity to Contact_DAO and performs an add operation
        /// </summary>
        /// <param name="p_Contact_BE"></param>
        public void AddContact(List<Contact_BE> contactBusinessEntityList, int userID)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                Contact_DAO contactDataAccessObject = new Contact_DAO();

                contactDataAccessObject.AddContact(contactBusinessEntityList, userID);

                HandleWriteLog("End", new StackTrace(true));
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
        }

        /// <summary>
        /// This method passes the Contact_BE entity to Contact_DAO and performs an update operation
        /// </summary>
        /// <param name="p_Contact_BE"></param>
        public void UpdateContact(Contact_BE contactBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                Contact_DAO contactDataAccessObject = new Contact_DAO();

                contactDataAccessObject.UpdateContact(contactBusinessEntity);

                HandleWriteLog("End", new StackTrace(true));
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
        }

        /// <summary>
        /// This method passes the Contact_BE entity to Contact_DAO and performs a delete operation
        /// </summary>
        /// <param name="p_Contact_BE"></param>
        public void DeleteContact(Contact_BE contactBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));

                Contact_DAO contactDataAccessObject = new Contact_DAO();
                contactBusinessEntity.IsActive = false;

                contactDataAccessObject.UpdateContact(contactBusinessEntity);

                HandleWriteLog("End", new StackTrace(true));
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
        }

        #endregion
    }
}
