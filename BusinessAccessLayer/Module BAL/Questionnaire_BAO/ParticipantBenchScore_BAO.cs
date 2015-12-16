using System;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class ParticipantBenchScore_BAO:Base_BAO
    {
        #region "Private Member Variable"
        private int addParticipantBenchScore;
        #endregion

        #region CRUD Operations
        /// <summary>
        /// Add Participant Bench Score
        /// </summary>
        /// <param name="participantBenchScoreBusinessEntity"></param>
        /// <returns></returns>
        public int AddParticipantBenchScore(ParticipantBenchScore_BE participantBenchScoreBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                ParticipantBenchScore_DAO participantBenchScoreDataAccessObject = new ParticipantBenchScore_DAO();
                addParticipantBenchScore = participantBenchScoreDataAccessObject.AddParticipantBenchScore(participantBenchScoreBusinessEntity, dbTransaction);
                
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

        /// <summary>
        /// Get Category Score
        /// </summary>
        /// <param name="participantBenchScoreBusinessEntity"></param>
        /// <returns></returns>
        public DataTable GetCategoryScore(ParticipantBenchScore_BE participantBenchScoreBusinessEntity)
        {
            DataTable dataTableCategoryScore = null;

            try
            {
                ParticipantBenchScore_DAO participantBenchScoreDataAccessObject = new ParticipantBenchScore_DAO();
                dataTableCategoryScore = participantBenchScoreDataAccessObject.GetCategoryScore(participantBenchScoreBusinessEntity);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategoryScore;
        }
    }
}
