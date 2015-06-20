using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
    public class Programme_BE
    {
        public int? ProgrammeID { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public string ClientName { get; set; }
        public string Logo { get; set; }
        public int? ProjectID { get; set; }
        public int? AccountID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Reminder1Date { get; set; }
        public DateTime? Reminder2Date { get; set; }
        public DateTime? Reminder3Date { get; set; }
        public DateTime? ReportAvaliableFrom { get; set; }
        public DateTime? ReportAvaliableTo { get; set; }
        public DateTime? PartReminder1Date { get; set; }
        public DateTime? PartReminder2Date { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }
        public string Instructions { get; set; }
        public int? ColleagueNo { get; set; }
        public string ReportTopLogo { get; set; }


        public Programme_BE()
        {
            this.ProgrammeID = null;
            this.ProgrammeName = null;
            this.ProgrammeDescription = null;
            this.ClientName = null;
            this.Logo = null;
            this.ProjectID = null;
            this.AccountID = null;
            this.StartDate = null;
            this.EndDate = null;
            this.Reminder1Date = null;
            this.Reminder2Date = null;
            this.Reminder3Date = null;
            this.ReportAvaliableFrom = null;
            this.ReportAvaliableTo = null;
            this.PartReminder1Date = null;
            this.PartReminder2Date = null;
            this.ModifyBy = null;
            this.ModifyDate = null;
            this.IsActive = null;
            this.Instructions = null;
            this.ColleagueNo = null;
            this.ReportTopLogo = null;
        }
    }

}
