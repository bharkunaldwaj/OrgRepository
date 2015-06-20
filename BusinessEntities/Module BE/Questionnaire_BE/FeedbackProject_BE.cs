using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
   public class FeedbackProject_BE
    {

        public int? ProjectID { get; set; }
        public int? StatusID { get; set; }
        public String Reference { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public int? AccountID { get; set; }
        public int? ManagerID { get; set; }
        public int? MaxCandidate { get; set; }
        public String Logo { get; set; }
        public String Password { get; set; }
        public int? QuestionnaireID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Reminder1Date { get; set; }
        public DateTime? Reminder2Date { get; set; }
        public DateTime? Reminder3Date { get; set; }
        public DateTime? ReportAvaliableFrom { get; set; }
        public DateTime? ReportAvaliableTo { get; set; }
        public int? EmailTMPLStart { get; set; }
        public int? EmailTMPLReminder1 { get; set; }
        public int? EmailTMPLReminder2 { get; set; }
        public int? EmailTMPLReminder3 { get; set; }
        public int? EmailTMPLReportAvalibale { get; set; }
        public int? EmailTMPLParticipant { get; set; }
        public int? EmailTMPPartReminder1 { get; set; }
        public int? EmailTMPPartReminder2 { get; set; }
        public int? EmailTMPManager { get; set; }
        public String Relationship1 { get; set; }
        public String Relationship2 { get; set; }
        public String Relationship3 { get; set; }
        public String Relationship4 { get; set; }
        public String Relationship5 { get; set; }
        public String FaqText { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }

        public FeedbackProject_BE()
        {
            this.ProjectID = null;
            this.StatusID = null;
            this.Reference = String.Empty;
            this.Title = String.Empty;
            this.Description = null;
            this.AccountID = null;
            this.ManagerID = null;
            this.MaxCandidate = null;
            this.Logo = String.Empty;
            this.QuestionnaireID = null;
            this.StartDate = null;
            this.EndDate = null;
            this.Reminder1Date = null;
            this.Reminder2Date = null;
            this.Reminder3Date = null;
            this.ReportAvaliableFrom = null;
            this.ReportAvaliableTo = null;
            this.EmailTMPLStart = null;
            this.EmailTMPLReminder1 = null;
            this.EmailTMPLReminder2 = null;
            this.EmailTMPLReminder3 = null;
            this.EmailTMPLReportAvalibale = null;
            this.EmailTMPLParticipant = null;
            this.EmailTMPPartReminder1 = null;
            this.EmailTMPPartReminder2 = null;
            this.EmailTMPManager = null;
            this.Relationship1 = null;
            this.Relationship2 = null;
            this.Relationship3 = null;
            this.Relationship4 = null;
            this.Relationship5 = null;
            this.FaqText = null;
            this.ModifyBy = null;
            this.ModifyDate = null;
            this.IsActive = null;
        } 
    }















  





}
