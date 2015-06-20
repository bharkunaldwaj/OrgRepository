using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
    public class Survey_Programme_BE
    {
        public int? ProgrammeID { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public string ClientName { get; set; }
        public string Logo { get; set; }
        public int? ProjectID { get; set; }
        public int? CompanyID { get; set; }
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

        public string Analysis_I_Name { get; set; }
        public  int? Analysis_I_Category { get; set; }
        public string Analysis_II_Name { get; set; }
        public int? Analysis_II_Category { get; set; }
        public string Analysis_III_Name { get; set; }
        public int? Analysis_III_Category { get; set; }
        public string prog_id { get; set; }


        





        public Survey_Programme_BE()
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

        this.Analysis_I_Name= null;
        this.Analysis_I_Category = null;
        this.Analysis_II_Name = null;
        this.Analysis_II_Category = null;
        this.Analysis_III_Name = null; 
        this.Analysis_III_Category = null;
        this.prog_id = null;
        }
    }



    public class Survey_Programme_BE2
    {
        public int? Analysis_Category_Id { get; set; }
        public string Category_Detail { get; set; }
        public int? ProgrammeID { get; set; }
        public string Analysis_Type { get; set; }
        public string Category_Name { get; set; }



        public Survey_Programme_BE2()
        {
            this.Analysis_Category_Id = null;
            this.Category_Detail = null;
            this.ProgrammeID = null;
            this.Analysis_Type = null;
            this.Category_Name = null;
        }



    }
}
