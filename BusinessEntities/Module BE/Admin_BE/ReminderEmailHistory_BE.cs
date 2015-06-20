using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Admin_BE
{
    [Serializable]
    public class ReminderEmailHistory_BE : feedbackFramework_BE.BE_Base
    {
        public int? RemId { get; set; }
        public int? Type { get; set; }
        public int? AccountId { get; set; }
        public String AccountName { get; set; }
        public int? ParticipantId { get; set; }
        public String ParticipantName { get; set; }
        public int? CandidateId { get; set; }
        public String CandidateName { get; set; }
        public int? ProjectId { get; set; }
        public String ProjectName { get; set; }
        public int? ProgrammeId { get; set; }
        public String ProgrammeName { get; set; }
        public DateTime? EmailDate { get; set; }
        public bool? EmailStatus { get; set; }

        public ReminderEmailHistory_BE()
        {
            this.RemId = null;
            this.Type = null;
            this.AccountId = null;
            this.AccountName = String.Empty;
            this.ParticipantId = null;
            this.ParticipantName = String.Empty;
            this.CandidateId = null;
            this.CandidateName = String.Empty;
            this.ProjectId = null;
            this.ProjectName = String.Empty;
            this.ProgrammeId = null;
            this.ProgrammeName = String.Empty;
            this.EmailDate = null;
            this.EmailStatus = null;
        }
    }








    [Serializable]
    public class Survey_ReminderEmailHistory_BE : feedbackFramework_BE.BE_Base
    {
        public int? RemId { get; set; }
        public int? Type { get; set; }
        public int? AccountId { get; set; }
        public String AccountName { get; set; }
        public int? ParticipantId { get; set; }
        public String ParticipantName { get; set; }
        public int? CandidateId { get; set; }
        public String CandidateName { get; set; }
        public int? ProjectId { get; set; }
        public String ProjectName { get; set; }
        public int? ProgrammeId { get; set; }
        public String ProgrammeName { get; set; }
        public DateTime? EmailDate { get; set; }
        public bool? EmailStatus { get; set; }

        public Survey_ReminderEmailHistory_BE()
        {
            this.RemId = null;
            this.Type = null;
            this.AccountId = null;
            this.AccountName = String.Empty;
            this.ParticipantId = null;
            this.ParticipantName = String.Empty;
            this.CandidateId = null;
            this.CandidateName = String.Empty;
            this.ProjectId = null;
            this.ProjectName = String.Empty;
            this.ProgrammeId = null;
            this.ProgrammeName = String.Empty;
            this.EmailDate = null;
            this.EmailStatus = null;
        }
    }






}
