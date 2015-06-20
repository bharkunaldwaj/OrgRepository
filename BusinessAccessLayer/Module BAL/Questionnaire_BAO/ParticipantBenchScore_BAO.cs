using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;
using System.Data.SqlClient;

namespace Questionnaire_BAO
{
    public class ParticipantBenchScore_BAO:Base_BAO
    {
        #region "Private Member Variable"

        private int addParticipantBenchScore;

        #endregion

        #region CRUD Operations

        public int AddParticipantBenchScore(ParticipantBenchScore_BE participantBenchScore_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                ParticipantBenchScore_DAO participantBenchScore_DAO = new ParticipantBenchScore_DAO();
                addParticipantBenchScore = participantBenchScore_DAO.AddParticipantBenchScore(participantBenchScore_BE, dbTransaction);
                
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

            return addParticipantBenchScore;
        }

        #endregion

        public DataTable GetCategoryScore(ParticipantBenchScore_BE participantBenchScore_BE)
        {
            DataTable dtCategoryScore = null;

            try
            {
                ParticipantBenchScore_DAO participantBenchScore_DAO = new ParticipantBenchScore_DAO();
                dtCategoryScore = participantBenchScore_DAO.GetCategoryScore(participantBenchScore_BE);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategoryScore;
        }
    }
}
