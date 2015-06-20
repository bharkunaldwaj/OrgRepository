using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{

    public class Question_Range_BE
    {

        public int RangeID { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public int? r_upto { get; set; }
        public string rating_text { get; set; }

        public Question_Range_BE()
        {
            this.name = null;
            this.title = null;
            this.r_upto = null;
            this.rating_text = null;
        }
    }
}
