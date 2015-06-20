using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using feedbackFramework_BE;

namespace Questionnaire_BE
{
    [Serializable]
    public class ParticipantBenchScoreDetails_BE:BE_Base
    {
        public int? BenchmarkDetailID { get; set; }
        public int? BenchmarkID { get; set; }
        public int? CategoryID { get; set; }
        public decimal? Score { get; set; }

        public ParticipantBenchScoreDetails_BE()
        {
            this.BenchmarkDetailID = null;
            this.BenchmarkID = null;
            this.CategoryID = null;
            this.Score = null;
        } 
    }
}
