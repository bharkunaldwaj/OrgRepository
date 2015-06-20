using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using feedbackFramework_BE;

namespace Questionnaire_BE
{
    [Serializable]
    public class AssignmentDetails_BE//:BE_Base
    {
        public int? AssignmentID { get; set; }
        public String RelationShip { get; set; }
        public String CandidateName { get; set; }
        public String CandidateEmail { get; set; }

        public AssignmentDetails_BE()
        {
            this.AssignmentID = null;
            this.RelationShip = String.Empty;
            this.CandidateName = String.Empty;
            this.CandidateEmail = String.Empty;
        } 
    }
}
