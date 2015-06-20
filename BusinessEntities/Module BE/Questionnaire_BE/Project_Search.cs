using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
  public  class Project_Search
    {
        public int? ProjectID { get; set; }
        public int? StatusID { get; set; }
        public String Reference { get; set; }
        public String Title { get; set; }
        public int? ManagerID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public Project_Search()
        {
            this.ProjectID = null;
            this.StatusID = null;
            this.Reference = String.Empty;
            this.Title = String.Empty;
            this.ManagerID = null;
            this.StartDate = null;
            this.EndDate = null;
           
        } 

    }








  public class Survey_Project_Search
  {
      public int? ProjectID { get; set; }
      public int? StatusID { get; set; }
      public String Reference { get; set; }
      public String Title { get; set; }
      public int? ManagerID { get; set; }
      public DateTime? StartDate { get; set; }
      public DateTime? EndDate { get; set; }


      public Survey_Project_Search()
      {
          this.ProjectID = null;
          this.StatusID = null;
          this.Reference = String.Empty;
          this.Title = String.Empty;
          this.ManagerID = null;
          this.StartDate = null;
          this.EndDate = null;

      }

  }







}
