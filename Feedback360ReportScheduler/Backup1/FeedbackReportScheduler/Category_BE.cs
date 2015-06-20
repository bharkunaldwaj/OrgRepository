using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
    [Serializable]
    public class Category_BE 
    {
        public int? CategoryID { get; set; }
        public int? AccountID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int? Questionnaire { get; set; }
        public int? Sequence { get; set; }
        public bool? ExcludeFromAnalysis { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IsActive { get; set; }

        public Category_BE()
        {
            this.CategoryID = null;
            this.AccountID = null;
            this.Name = String.Empty;
            this.Description = String.Empty;
            this.Sequence = null;
            this.Questionnaire = null;
            this.ExcludeFromAnalysis = null;
            this.ModifiedBy = null;
            this.ModifiedDate = null;
            this.IsActive = null;
        }
    }
}
