using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
    public class Survey_Company_BE
    {

        public int? CompanyID
        { get; set; }

        public int? AccountID
        { get; set; }

        public int? ProjectID
        { get; set; }

        public int? ManagerID
        { get; set; }

        public int? StatusID
        { get; set; }

        public string CompanyName
        { get; set; }

        public string Title
        { get; set; }

        public string Description
        { get; set; }

        public string QuestLogo
        { get; set; }

        public string ReportLogo
        { get; set; }

        public string Finish_EmailID
        { get; set; }

        public bool? Finish_EmailID_Chkbox
        { get; set; }

        public int? EmailTMPLStart
        { get; set; }

        public int? EmailTMPLReminder1
        { get; set; }

        public int? EmailTMPLReminder2
        { get; set; }

        public int? EmailTMPLReminder3
        { get; set; }

        public int? EmailFinishEmailTemplate
        { get; set; }

        public string FaqText
        { get; set; }

        public int? ModifyBy
        { get; set; }

        public DateTime? ModifyDate
        { get; set; }

        public int? IsActive
        { get; set; }


    }
}
