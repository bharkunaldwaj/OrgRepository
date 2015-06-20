using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseAccessUtilities {
    public class DAU
    {
        private string m_connectionString;
        private SqlConnection m_conOject;

        #region "Public Constructor"
        public DAU() {
            //m_connectionString = 
            OpenConnection();
        }
        #endregion

        #region "Manage Connection"
        private void OpenConnection() {
            m_conOject = new SqlConnection(m_connectionString);
        }

        private void CloseConnection() {
            m_conOject.Close();
        }
        #endregion

        #region Read Data
        public int GetSQLData(string p_SQLStatement, out DataTable p_dtReturn) {
            int returnValue;
            SqlDataAdapter sqlDataAdapter;

            if (m_conOject.State == ConnectionState.Closed) {
                m_conOject.Open();
            }
            p_dtReturn = new DataTable();
            sqlDataAdapter = new SqlDataAdapter(p_SQLStatement, m_conOject);
            returnValue = sqlDataAdapter.Fill(p_dtReturn);
            return returnValue;
        }

        public int GetSQLData(string p_SQLStatement, out DataSet p_dsReturn) {
            int returnValue;
            SqlDataAdapter sqlDataAdapter;

            if (m_conOject.State == ConnectionState.Closed) {
                m_conOject.Open();
            }
            p_dsReturn = new DataSet();
            sqlDataAdapter = new SqlDataAdapter(p_SQLStatement, m_conOject);
            returnValue = sqlDataAdapter.Fill(p_dsReturn);
            //m_conOject = null;
            return returnValue;
        }

        public void GetSQLData(string p_SQLStatement, out SqlDataReader p_drReturn) {
            SqlCommand cmd;

            if (m_conOject.State == ConnectionState.Closed) {
                m_conOject.Open();
            }
            cmd = new SqlCommand(p_SQLStatement, m_conOject);
            p_drReturn = cmd.ExecuteReader();
            //m_conOject = null;
        }
        #endregion

        #region "Update Database"
        public int ExecuteNonQuery(string p_DMLStatement) {
            int returnValue;
            if (m_conOject.State == ConnectionState.Closed) {
                m_conOject.Open();
            }
            SqlCommand cmdSQLCommand = new SqlCommand(p_DMLStatement, m_conOject);
            cmdSQLCommand.Connection = m_conOject;
            cmdSQLCommand.CommandType = CommandType.Text;
            cmdSQLCommand.CommandText = p_DMLStatement;
            cmdSQLCommand.CommandTimeout = 10000;
            returnValue = cmdSQLCommand.ExecuteNonQuery();
            return returnValue;
        }
        #endregion

    }
}
