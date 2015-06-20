using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
    public class Survey_PrvScore_QstDetails_BE
    {
        public int QuestionDetailID
        { get; set; }

        public int? PreviousScoreID
        { get; set; }

        public int? CategoryID
        { get; set; }

        public int? QuestionID
        { get; set; }

        public string AnalysisType
        { get; set; }

        public decimal Score1
        { get; set; }

        public decimal Score2
        { get; set; }



    }
}
