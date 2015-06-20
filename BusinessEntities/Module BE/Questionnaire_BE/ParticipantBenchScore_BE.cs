﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using feedbackFramework_BE;

namespace Questionnaire_BE
{
    [Serializable]
    public class ParticipantBenchScore_BE
    {
        public int? BenchmarkID { get; set; }
        public String BenchmarkName { get; set; }
        public int? AccountID { get; set; }
        public int? ProjectID { get; set; }
        public int? ProgrammeID { get; set; }
        public int? QuestionnaireID { get; set; }
        public int? TargetPersonID { get; set; }
        public int? ScoreMonth { get; set; }
        public int? ScoreYear { get; set; }
        public String Description { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IsActive { get; set; }
        public List<ParticipantBenchScoreDetails_BE> ParticipantBenchScoreDetails { get; set; }

        public ParticipantBenchScore_BE()
        {
            this.BenchmarkID = null;
            this.BenchmarkName = string.Empty;
            this.AccountID = null;
            this.ProjectID = null;
            this.ProgrammeID = null;
            this.QuestionnaireID = null;
            this.TargetPersonID = null;
            this.ScoreMonth = null;
            this.ScoreYear = null;
            this.Description = String.Empty;
            this.ModifiedBy = null;
            this.ModifiedDate = null;
            this.IsActive = null;
            this.ParticipantBenchScoreDetails = null;
        } 
    }
}
