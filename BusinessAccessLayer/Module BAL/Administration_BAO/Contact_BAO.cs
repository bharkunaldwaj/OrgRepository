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
using System.Linq;
using System.Text;
using System.Diagnostics;

using DAF_BAO;

using Administration_BE;
using Administration_DAO;
using DatabaseAccessUtilities;
using System.Data;


namespace Administration_BAO {
    public class Contact_BAO : Base_BAO {

        #region "Business Logic for Contact BAO"

        /// <summary>
        /// This Method returns the Contacts from Contact_DAO on passing Contact_BE as a parameter
        /// </summary>
        /// <param name="p_Contact_BE"></param>
        /// <returns></returns>

        public List<Contact_BE> GetContact(Contact_BE p_contact_BE) {
            List<Contact_BE> contact_BEList = null;
            try {
                HandleWriteLog("Start", new StackTrace(true));
                Contact_DAO contact_DAO = new Contact_DAO();

                contact_DAO.GetContact(p_contact_BE);
                contact_BEList = contact_DAO.Contact_BEList;

                HandleWriteLog("End", new StackTrace(true));
            }
            catch (Exception ex) {
                HandleException(ex);
            }
            return contact_BEList;

        }

        /// <summary>
        /// This method passes the Contact_BE entity to Contact_DAO and performs an add operation
        /// </summary>
        /// <param name="p_Contact_BE"></param>

        public void AddContact(List<Contact_BE> p_contact_BEList, int p_userID)
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
                Contact_DAO contact_DAO = new Contact_DAO();
                contact_DAO.AddContact(p_contact_BEList, p_userID);
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

        public void UpdateContact(Contact_BE p_contact_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                Contact_DAO contact_DAO = new Contact_DAO();
                contact_DAO.UpdateContact(p_contact_BE);
                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex) {
                if (dbTransaction != null) {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        /// <summary>
        /// This method passes the Contact_BE entity to Contact_DAO and performs a delete operation
        /// </summary>
        /// <param name="p_Contact_BE"></param>

        public void DeleteContact(Contact_BE p_contact_BE) {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();
                HandleWriteLog("Start", new StackTrace(true));
                Contact_DAO contact_DAO = new Contact_DAO();
                p_contact_BE.IsActive = false;
                contact_DAO.UpdateContact(p_contact_BE);
                HandleWriteLog("End", new StackTrace(true));
                dbTransaction.Commit();
                conn.Close();
            }
            catch (Exception ex) {
                if (dbTransaction != null) {
                    dbTransaction.Rollback();
                }

                HandleException(ex);
            }
        }

        #endregion
    }
}
