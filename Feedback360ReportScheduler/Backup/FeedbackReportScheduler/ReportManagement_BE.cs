using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
    public class ReportManagement_BE
    {        
        public int? ParticipantReportID { get; set; }
        public int? AccountID { get; set; }
        public int? ProjectID { get; set; }
        public int? ProgramID { get; set; }
        public int? TargetPersonID { get; set; }
        public int? TotalCount { get; set; }
        public int? SubmitCount { get; set; }
        public int? SelfAssessment { get; set; }
        public String ReportName { get; set; }

        public int? ProjectReportSettingID { get; set; }
        public String CoverPage { get; set; }
        public String ReportIntroduction { get; set; }
        public String RadarChart { get; set; }
        public String CatQstList { get; set; }
        public String CatDataChart { get; set; }
        public String QstTextResponses { get; set; }
        public String Conclusionpage { get; set; }
        public String CandidateSelfStatus { get; set; }
        public String ProjectRelationGrp { get; set; }
        public String FullProjectGrp { get; set; }
        public String ProgrammeGrp { get; set; }
        public String ReportType { get; set; }
        public String PageHeading1 { get; set; }
        public String PageHeading2 { get; set; }
        public String PageHeading3 { get; set; }
        public String PageHeadingColor { get; set; }
        public String PageHeadingCopyright { get; set; }
        public String PageHeadingIntro { get; set; }
        public String PageHeadingConclusion { get; set; }
        public String PageLogo { get; set; }
        public String ConclusionHighLowRange { get; set; }
        public String PreviousScoreVisible { get; set; }
        public String BenchMarkScoreVisible { get; set; }
        public String BenchMarkGrpVisible { get; set; }
        public String BenchConclusionpage { get; set; }

        public ReportManagement_BE()
        {
            this.ParticipantReportID = null;
            this.AccountID = null;
            this.ProjectID = null;
            this.ProgramID = null;
            this.TargetPersonID = null;
            this.TotalCount = null;
            this.SubmitCount = null;
            this.SelfAssessment = null;
            this.ReportName = String.Empty;

            this.ProjectReportSettingID = null;            
            this.CoverPage = String.Empty;
            this.ReportIntroduction = String.Empty;
            this.RadarChart = String.Empty;
            this.CatQstList = String.Empty;
            this.CatDataChart = String.Empty;
            this.QstTextResponses = String.Empty;
            this.Conclusionpage = String.Empty;
            this.CandidateSelfStatus = String.Empty;
            this.ProjectRelationGrp = String.Empty;
            this.FullProjectGrp = String.Empty;
            this.ProgrammeGrp = String.Empty;
            this.ReportType = String.Empty;
            this.PageHeading1 = String.Empty;
            this.PageHeading2 = String.Empty;
            this.PageHeading3 = String.Empty;
            this.PageHeadingColor = String.Empty;
            this.PageHeadingCopyright = String.Empty;
            this.PageHeadingIntro = String.Empty;
            this.PageHeadingConclusion = String.Empty;
            this.PageLogo = String.Empty;
            this.ConclusionHighLowRange = String.Empty;
            this.PreviousScoreVisible = String.Empty;
            this.BenchMarkScoreVisible = String.Empty;
            this.BenchMarkGrpVisible = String.Empty;
            this.BenchConclusionpage = String.Empty;
        }
    }
}
