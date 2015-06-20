using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Administration_BE {
    [Serializable]
    public class AssignedCategories_BE : feedbackFramework_BE.BE_Base {
        public int? CategoryID { get; set; }
        public int? AccountID { get; set; }
        public int? ProjectID { get; set; }
        public String Name { get; set; }
        public int? QuestionnaireID { get; set; }
        
        public AssignedCategories_BE()
        {
            this.CategoryID = null;
            this.AccountID = null;
            this.ProjectID = null;
            this.Name = String.Empty;
            this.QuestionnaireID = null;
        }
    }
}