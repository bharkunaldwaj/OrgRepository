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
    public class ParticipantBenchScore_DAO:DAO_Base
    {
        DatabaseAccessUtilities.CDataSrc cDataSrc = new CSqlClient(ConfigurationSettings.AppSettings["ConnectionString"].ToString());

        #region Private Variables

        private int returnValue;

        #endregion

        #region "Public Properties"

        public List<AssignQuestionnaire_BE> assignQuestionnaire_BEList { get; set; }

        #endregion

        # region CRUD Operation

        public int AddParticipantBenchScore(ParticipantBenchScore_BE participantBenchScore_BE, IDbTransaction dbTransaction)
        {
            try
            {
                object[] param = new object[14] {null,
                                                participantBenchScore_BE.BenchmarkName,
                                                participantBenchScore_BE.AccountID,
                                                participantBenchScore_BE.ProjectID,
                                                participantBenchScore_BE.ProgrammeID,
                                                participantBenchScore_BE.QuestionnaireID,
                                                participantBenchScore_BE.TargetPersonID,
                                                participantBenchScore_BE.ScoreMonth,
                                                participantBenchScore_BE.ScoreYear,
                                                participantBenchScore_BE.Description,
                                                participantBenchScore_BE.ModifiedBy,
                                                participantBenchScore_BE.ModifiedDate,
                                                participantBenchScore_BE.IsActive,
                                                "I" };

                returnValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticipantBenchScoreManagement", param, dbTransaction));


                //DELETE EXISTING RECORD FROM PARTICIPANTSCOREDETAILS TABLE

                cDataSrc.ExecuteNonQuery("Delete from [ParticipantBenchmarkDetails] where [BenchmarkID]=" + returnValue, dbTransaction);


                // ADD RECORD OF CATEGORY SCORES

                List<ParticipantBenchScoreDetails_BE> lstparticipantBenchScoreDetails = new List<ParticipantBenchScoreDetails_BE>();
                lstparticipantBenchScoreDetails = participantBenchScore_BE.ParticipantBenchScoreDetails;

                int newValue;

                for (int count = 0; count < lstparticipantBenchScoreDetails.Count; count++)
                {
                    object[] newparam = new object[4] {returnValue, //score id
                                                lstparticipantBenchScoreDetails[count].CategoryID,
                                                lstparticipantBenchScoreDetails[count].Score,
                                                "I" };

                    newValue = Convert.ToInt32(cDataSrc.ExecuteScalar("UspParticipantBenchScoreDetailsManagement", newparam, dbTransaction));
                }

                cDataSrc = null;
            }
            catch (Exception ex) { HandleException(ex); }
            return returnValue;
        }

        public DataTable GetCategoryScore(ParticipantBenchScore_BE participantBenchScore_BE)
        {
            DataTable dtCategoryScore = new DataTable();

            try
            {
                object[] param = new object[7] {participantBenchScore_BE.ProjectID,
                                                participantBenchScore_BE.ProgrammeID,
                                                participantBenchScore_BE.QuestionnaireID,
                                                participantBenchScore_BE.TargetPersonID,
                                                participantBenchScore_BE.ScoreMonth,
                                                participantBenchScore_BE.ScoreYear,
                                                "A" };

                dtCategoryScore = cDataSrc.ExecuteDataSet("UspParticipantBenchScoreDetailsSelect",param, null).Tables[0];
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
