using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using feedbackFramework_BE;

namespace Questionnaire_BE
{
    [Serializable]
    public class AssignmentDetails_BE:BE_Base
    {
        public int? AssignmentID { get; set; }
        public String RelationShip { get; set; }
        public String CandidateName { get; set; }
        public String CandidateEmail { get; set; }
        public bool SubmitFlag { get; set; }
        public int? EmailSendFlag { get; set; }

        public AssignmentDetails_BE()
        {
            this.AssignmentID = null;
            this.RelationShip = String.Empty;
            this.CandidateName = String.Empty;
            this.CandidateEmail = String.Empty;
            this.SubmitFlag = false;
            this.EmailSendFlag = null;
        } 
    }





    [Serializable]
    public class Survey_AssignmentDetails_BE : BE_Base
    {
        public int? AssignmentID { get; set; }
        public String Analysis_I { get; set; }
        public String Analysis_II { get; set; }
        public String Analysis_III { get; set; }
        public String CandidateName { get; set; }
        public String CandidateEmail { get; set; }
        public bool SubmitFlag { get; set; }
        public int? EmailSendFlag { get; set; }

        public Survey_AssignmentDetails_BE()
        {
            this.AssignmentID = null;
            this.Analysis_I = String.Empty;
            this.Analysis_II = String.Empty;
            this.Analysis_II = String.Empty;

            this.CandidateName = String.Empty;
            this.CandidateEmail = String.Empty;
            this.SubmitFlag = false;
            this.EmailSendFlag = null;
        }
    }









}
