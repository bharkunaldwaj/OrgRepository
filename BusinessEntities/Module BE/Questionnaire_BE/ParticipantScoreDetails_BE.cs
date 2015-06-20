using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using feedbackFramework_BE;

namespace Questionnaire_BE
{
    [Serializable]
    public class ParticipantScoreDetails_BE:BE_Base
    {
        public int? ScoreDetailID { get; set; }
        public int? ScoreID { get; set; }
        public int? ScoreType { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? CategoryID { get; set; }
        public decimal? Score { get; set; }

        public ParticipantScoreDetails_BE()
        {
            this.ScoreDetailID = null;
            this.ScoreID = null;
            this.ScoreType = null;
            this.Month = null;
            this.Year = null;
            this.CategoryID = null;
            this.Score = null;
        } 
    }
}
