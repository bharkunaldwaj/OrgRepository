using System;

using DAF_BAO;
using DatabaseAccessUtilities;
using Questionnaire_BE;
using Questionnaire_DAO;

using System.Data;

namespace Questionnaire_BAO
{
    public class ParticipantScore_BAO:Base_BAO
    {
        #region "Private Member Variable"

        private int addParticipantScore;

        #endregion

        #region CRUD Operations
        /// <summary>
        /// Insert Participant Score
        /// </summary>
        /// <param name="participantScoreBusinessEntity"></param>
        /// <returns></returns>
        public int AddParticipantScore(ParticipantScore_BE participantScoreBusinessEntity)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                ParticipantScore_DAO participantScoreDataAccessObject = new ParticipantScore_DAO();
                addParticipantScore = participantScoreDataAccessObject.AddParticipantScore(participantScoreBusinessEntity, dbTransaction);
                
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

            return addParticipantScore;
        }
        #endregion

        /// <summary>
        /// Get Category Score1
        /// </summary>
        /// <param name="participantScoreBusinessEntity"></param>
        /// <returns></returns>
        public DataTable GetCategoryScore1(ParticipantScore_BE participantScoreBusinessEntity)
        {
            DataTable dataTableCategoryScore = null;

            try
            {
                ParticipantScore_DAO participantScoreDataAccessObject = new ParticipantScore_DAO();
                dataTableCategoryScore = participantScoreDataAccessObject.GetCategoryScore1(participantScoreBusinessEntity);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategoryScore;
        }

        /// <summary>
        /// Get Category Score2
        /// </summary>
        /// <param name="participantScoreBusinessEntity"></param>
        /// <returns></returns>
        public DataTable GetCategoryScore2(ParticipantScore_BE participantScoreBusinessEntity)
        {
            DataTable dataTableCategoryScore = null;

            try
            {
                ParticipantScore_DAO participantScoreDataAccessObject = new ParticipantScore_DAO();
                dataTableCategoryScore = participantScoreDataAccessObject.GetCategoryScore2(participantScoreBusinessEntity);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dataTableCategoryScore;
        }
    }
}
