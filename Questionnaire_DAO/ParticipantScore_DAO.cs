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

using Questionnaire_BE;
using DatabaseAccessUtilities;

namespace Questionnaire_DAO
{
    public class ParticipantScore_DAO:DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Properties"

        public List<AssignQuestionnaire_BE> assignQuestionnaire_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public int AddParticipantScore(ParticipantScore_BE participantScore_BE, IDbTransaction dbTransaction)
        {
            try
            {
                object[] param = new object[13] {null,
                                                participantScore_BE.AccountID,
                                                participantScore_BE.ProjectID,
                                                participantScore_BE.ProgrammeID,
                                                participantScore_BE.QuestionnaireID,
                                                participantScore_BE.TargetPersonID,
                                                participantScore_BE.ScoreMonth,
                                                participantScore_BE.ScoreYear,
                                                participantScore_BE.Description,
                                                participantScore_BE.ModifiedBy,
                                                participantScore_BE.ModifiedDate,
                                                participantScore_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticipantScoreManagement", param, dbTransaction));


                //DELETE EXISTING RECORD FROM PARTICIPANTSCOREDETAILS TABLE
                cDataSrc.ExecuteNonQuery("Delete from [ParticipantScoreDetails] where [ScoreID]=" + returnValue, dbTransaction);

                // ADD RECORD OF CATEGORY SCORES 1
                List<ParticipantScoreDetails_BE> lstparticipantScore1Details = new List<ParticipantScoreDetails_BE>();
                lstparticipantScore1Details = participantScore_BE.ParticipantScore1Details;

                int newValue;

                for (int count = 0; count < lstparticipantScore1Details.Count; count++)
                {
                    object[] newparam = new object[7] {returnValue, //score id
                                                lstparticipantScore1Details[count].ScoreType,
                                                lstparticipantScore1Details[count].Month,
                                                lstparticipantScore1Details[count].Year,
                                                lstparticipantScore1Details[count].CategoryID,
                                                lstparticipantScore1Details[count].Score,
                                                "I" };

                    newValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticipantScoreDetailsManagement", newparam, dbTransaction));
                }

                // ADD RECORD OF CATEGORY SCORES 2
                List<ParticipantScoreDetails_BE> lstparticipantScore2Details = new List<ParticipantScoreDetails_BE>();
                lstparticipantScore2Details = participantScore_BE.ParticipantScore2Details;

                for (int count = 0; count < lstparticipantScore2Details.Count; count++)
                {
                    object[] newparam = new object[7] {returnValue, //score id
                                                lstparticipantScore2Details[count].ScoreType,
                                                lstparticipantScore2Details[count].Month,
                                                lstparticipantScore2Details[count].Year,
                                                lstparticipantScore2Details[count].CategoryID,
                                                lstparticipantScore2Details[count].Score,
                                                "I" };

                    newValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticipantScoreDetailsManagement", newparam, dbTransaction));
                }

                cDataSrc = null;
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetCategoryScore1(ParticipantScore_BE participantScore_BE)
        {
            DataTable dtCategoryScore = new DataTable();

            try
            {
                object[] param = new object[7] {participantScore_BE.ProjectID,
                                                participantScore_BE.ProgrammeID,
                                                participantScore_BE.QuestionnaireID,
                                                participantScore_BE.TargetPersonID,
                                                participantScore_BE.ScoreMonth,
                                                participantScore_BE.ScoreYear,
                                                "1" };

                dtCategoryScore = cDataSrc.ExecuteDataSet("UspParticipantScoreDetailsSelect",param, null).Tables[0];
            }
            catch (Exception ex) 
            { 
                HandleException(ex); 
            }

            return dtCategoryScore;
        }

        public DataTable GetCategoryScore2(ParticipantScore_BE participantScore_BE)
        {
            DataTable dtCategoryScore = new DataTable();

            try
            {
                object[] param = new object[7] {participantScore_BE.ProjectID,
                                                participantScore_BE.ProgrammeID,
                                                participantScore_BE.QuestionnaireID,
                                                participantScore_BE.TargetPersonID,
                                                participantScore_BE.ScoreMonth,
                                                participantScore_BE.ScoreYear,
                                                "2" };

                dtCategoryScore = cDataSrc.ExecuteDataSet("UspParticipantScoreDetailsSelect", param, null).Tables[0];
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return dtCategoryScore;
        }

        #endregion

    }
}
