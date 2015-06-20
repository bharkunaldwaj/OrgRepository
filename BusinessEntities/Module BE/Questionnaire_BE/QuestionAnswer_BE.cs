using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using feedbackFramework_BE;

namespace Questionnaire_BE
{
    [Serializable]
    public class QuestionAnswer_BE:BE_Base
    {
        public int? QAID { get; set; }
        public int? AssignDetId { get; set; }
        public int? QuestionID { get; set; }
        public String Answer { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }

        public QuestionAnswer_BE()
        {
            this.QAID = null ;
            this.AssignDetId = null ;
            this.QuestionID = null ;
            this.Answer = String.Empty ;
            this.ModifyBy = null ;
            this.ModifyDate = null ;
            this.IsActive = null ;
        }
    }
















    [Serializable]
    public class Survey_QuestionAnswer_BE : Survey_BE_Base
    {
        public int? QAID { get; set; }
        public int? AssignDetId { get; set; }
        public int? QuestionID { get; set; }
        public String Answer { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }

        public Survey_QuestionAnswer_BE()
        {
            this.QAID = null;
            this.AssignDetId = null;
            this.QuestionID = null;
            this.Answer = String.Empty;
            this.ModifyBy = null;
            this.ModifyDate = null;
            this.IsActive = null;
        }
    }












}
