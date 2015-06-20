using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using feedbackFramework_BE;
using Admin_BE;

namespace Questionnaire_BE
{
    [Serializable]
    public class AssignQuestionnaire_BE//:BE_Base
    {
        public int? AssignmentID { get; set; }
        public int? ProjecctID { get; set; }
        public int? ProgrammeID { get; set; }
        public int? QuestionnaireID { get; set; }
        public int? TargetPersonID { get; set; }
        public string FeedbackURL { get; set; }
        public String Description { get; set; }
        public int? AccountID { get; set; }
        public int? CandidateNo { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IsActive { get; set; }
        public List<AssignmentDetails_BE> AssignmentDetails { get; set; }
        public List< AccountUser_BE> AssignmentUserDetails { get; set; }
        public int? NewProgrammeID { get; set; }

        public AssignQuestionnaire_BE()
        {
            this.AssignmentID = null;
            this.ProjecctID = null;
            this.ProgrammeID = null;
            this.QuestionnaireID = null;
            this.TargetPersonID = null;
            this.AccountID = null;
            this.FeedbackURL = string.Empty;
            this.Description = String.Empty;
            this.CandidateNo = null;
            this.ModifiedBy = null;
            this.ModifiedDate = null;
            this.IsActive = null;
            this.AssignmentDetails = null;
            this.AssignmentUserDetails = null;
            this.NewProgrammeID = null;
        } 
    }
}
