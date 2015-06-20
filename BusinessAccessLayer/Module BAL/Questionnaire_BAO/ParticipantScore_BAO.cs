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
    public class ParticipantScore_BAO:Base_BAO
    {
        #region "Private Member Variable"

        private int addParticipantScore;

        #endregion

        #region CRUD Operations

        public int AddParticipantScore(ParticipantScore_BE participantScore_BE)
        {
            CSqlClient sqlClient = null;
            IDbConnection conn = null;
            IDbTransaction dbTransaction = null;

            try
            {
                sqlClient = CDataSrc.Default as CSqlClient;
                conn = sqlClient.Connection();
                dbTransaction = conn.BeginTransaction();

                ParticipantScore_DAO participantScore_DAO = new ParticipantScore_DAO();
                addParticipantScore = participantScore_DAO.AddParticipantScore(participantScore_BE, dbTransaction);
                
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

        public DataTable GetCategoryScore1(ParticipantScore_BE participantScore_BE)
        {
            DataTable dtCategoryScore = null;

            try
            {
                ParticipantScore_DAO participantScore_DAO = new ParticipantScore_DAO();
                dtCategoryScore = participantScore_DAO.GetCategoryScore1(participantScore_BE);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategoryScore;
        }

        public DataTable GetCategoryScore2(ParticipantScore_BE participantScore_BE)
        {
            DataTable dtCategoryScore = null;

            try
            {
                ParticipantScore_DAO participantScore_DAO = new ParticipantScore_DAO();
                dtCategoryScore = participantScore_DAO.GetCategoryScore2(participantScore_BE);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategoryScore;
        }

    }
}
