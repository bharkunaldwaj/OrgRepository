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
        public String FrontPageLogo2 { get; set; }
        public String FrontPageLogo3 { get; set; }
        public String ConclusionHighLowRange { get; set; }
        public String PreviousScoreVisible { get; set; }
        public String BenchMarkScoreVisible { get; set; }
        public String BenchMarkGrpVisible { get; set; }
        public String BenchConclusionpage { get; set; }
        public String ConclusionHeading { get; set; }
        public String FrontPageLogo4 { get; set; }

        

        public ReportManagement_BE()
        {
            this.ParticipantReportID = null;
            this.AccountID = null;
            this.ProjectID = null;
            this.ProgramID = null;
            this.TargetPersonID = null;
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
            this.FrontPageLogo2 = String.Empty;
            this.FrontPageLogo3 = String.Empty;
            this.FrontPageLogo4 = String.Empty;
            this.ConclusionHighLowRange = String.Empty;
            this.PreviousScoreVisible = String.Empty;
            this.BenchMarkScoreVisible = String.Empty;
            this.BenchMarkGrpVisible = String.Empty;
            this.BenchConclusionpage = String.Empty;
            this.ConclusionHeading = String.Empty;
        }
    }




    public class Survey_ReportManagement_BE
    {
      //  public int? ParticipantReportID { get; set; }
        public int? AccountID { get; set; }
        public int? ProjectID { get; set; }
        public int? ProgramID { get; set; }
      //  public int? TargetPersonID { get; set; }
        public String ReportName { get; set; }
        public int? ProjectReportSettingID { get; set; }
        public String CoverPage { get; set; }
        public String ReportIntroduction { get; set; }
//        public String RadarChart { get; set; }
        public String CatQstList { get; set; }
        public String CatDataChart { get; set; }
        public String QstTextResponses { get; set; }
        public String Conclusionpage { get; set; }
  //      public String CandidateSelfStatus { get; set; }
        public String ProjectRelationGrp { get; set; }
  //      public String FullProjectGrp { get; set; }
    //    public String ProgrammeGrp { get; set; }
        public String ReportType { get; set; }
        public String PageHeading1 { get; set; }
        public String PageHeading2 { get; set; }
        public String PageHeading3 { get; set; }
        public String PageHeadingColor { get; set; }
        public String PageHeadingCopyright { get; set; }
        public String PageHeadingIntro { get; set; }
        public String PageHeadingConclusion { get; set; }
        public String PageLogo { get; set; }
        public String FrontPageLogo2 { get; set; }
        public String FrontPageLogo3 { get; set; }
        public String ConclusionHighLowRange { get; set; }
        public String PreviousScoreVisible { get; set; }
   //     public String BenchMarkScoreVisible { get; set; }
   //     public String BenchMarkGrpVisible { get; set; }
   //     public String BenchConclusionpage { get; set; }
        public String ConclusionHeading { get; set; }


        public String FullProjectGrp { get; set; }
        public String AnalysisI { get; set; }
        public String AnalysisII { get; set; }
        public String AnalysisIII { get; set; }
        public String Programme_Average { get; set; }
        public String FreeTextResponse { get; set; }


        public string ddlAccountCode{ get; set; }
        public string ddlProgramme { get; set; }
        public string DDList_analysis { get; set; }
        public string SelectFlag { get; set; }

        public bool ShowScoreRespondents { get; set; }

        public int RadarGraphCategoryCount { get; set; }





        public Survey_ReportManagement_BE()
        {
         //   this.ParticipantReportID = null;
            this.AccountID = null;
            this.ProjectID = null;
            this.ProgramID = null;
         //   this.TargetPersonID = null;
            this.ReportName = String.Empty;
            this.ProjectReportSettingID = null;
            this.CoverPage = String.Empty;
            this.ReportIntroduction = String.Empty;
         //   this.RadarChart = String.Empty;
            this.CatQstList = String.Empty;
            this.CatDataChart = String.Empty;
            this.QstTextResponses = String.Empty;
            this.Conclusionpage = String.Empty;
        //    this.CandidateSelfStatus = String.Empty;
            this.ProjectRelationGrp = String.Empty;
       //     this.FullProjectGrp = String.Empty;
        //    this.ProgrammeGrp = String.Empty;
            this.ReportType = String.Empty;
            this.PageHeading1 = String.Empty;
            this.PageHeading2 = String.Empty;
            this.PageHeading3 = String.Empty;
            this.PageHeadingColor = String.Empty;
            this.PageHeadingCopyright = String.Empty;
            this.PageHeadingIntro = String.Empty;
            this.PageHeadingConclusion = String.Empty;
            this.PageLogo = String.Empty;
            this.FrontPageLogo2 = String.Empty;
            this.FrontPageLogo3 = String.Empty;
            this.ConclusionHighLowRange = String.Empty;
            this.PreviousScoreVisible = String.Empty;
       //     this.BenchMarkScoreVisible = String.Empty;
       //     this.BenchMarkGrpVisible = String.Empty;
      //      this.BenchConclusionpage = String.Empty;
            this.ConclusionHeading = String.Empty;


                this.FullProjectGrp= String.Empty;
                this.AnalysisI= String.Empty;
                this.AnalysisII = String.Empty;
                this.AnalysisIII = String.Empty;
                this.Programme_Average = String.Empty;
                this.FreeTextResponse = String.Empty;
            
            this.ddlAccountCode = String.Empty;
            this.ddlProgramme = String.Empty;
            this.DDList_analysis = String.Empty;
            this.SelectFlag = String.Empty;



        }

        public bool ShowRadar { get; set; }

        public bool ShowTable { get; set; }

        public bool ShowPreviousScore1 { get; set; }

        public bool ShowPreviousScore2 { get; set; }

        public bool ShowBarGraph { get; set; }

        public bool ShowLineChart { get; set; }


        public string FrontPdfFileName { get; set; }

        public string ScoreTableImage { get; set; }

        public string FooterImage { get; set; }
    }
}
