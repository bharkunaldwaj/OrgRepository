USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProjectReportSetting]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProjectReportSetting]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Survey_UspProjectReportSetting]
	@ProjectReportSettingID int,
	@AccountID int,
	@ProjectID int,
    @CoverPage Varchar(5),
    @ReportIntroduction Varchar(5),
  --  @RadarChart Varchar(5),
    @CatQstList Varchar(5),
    @CatDataChart Varchar(5),
  --  @QstTextResponses Varchar(5),
    @Conclusionpage Varchar(5),
  --  @CandidateSelfStatus Varchar(5),
    @ProjectRelationGrp Varchar(500),
   
  --  @ProgrammeGrp Varchar(5),    
    @ReportType Varchar(5), 
    @PageHeading1 Varchar(500),
    @PageHeading2 Varchar(500),
    @PageHeading3 Varchar(500),
    @PageHeadingColor Varchar(500),
    @PageHeadingCopyright Varchar(500),
    @PageHeadingIntro Varchar(5000),
    @PageHeadingConclusion Varchar(5000),   
    @PageLogo Varchar(50),
    @FrontPageLogo2 Varchar(50),
    @FrontPageLogo3 Varchar(50),
 -- @ConclusionHighLowRange Varchar(100),
 -- @PreviousScoreVisible Varchar(5),
  --  @BenchMarkScoreVisible Varchar(5),
  --  @BenchMarkGrpVisible Varchar(5),
  --  @BenchConclusionpage Varchar(5),
    @ConclusionHeading varchar(50),
    @FullProjectGrp Varchar(5), 
    
    @AnalysisI Varchar(100),
    @AnalysisII Varchar(100),
    @AnalysisIII Varchar(100),
    @Programme_Average Varchar(5),
    @FreeTextResponses Varchar(10),    
    @Operation char(2),
    @ShowScoreRespondents bit,
    @ShowRadar bit=0,
    @ShowTable bit=0,
    @ShowPreviousScore1 bit=0,
    @ShowPreviousScore2 bit=0,
    @ShowBarGraph bit=0,
    @ShowLineChart bit=0,
    @FrontPdfFileName varchar(100)=null,
    @ScoreTable varchar(100)=null,
    @FooterImage varchar(100)=null,
    @RadarGraphCategoryCount int = 4
as
-- Insert 


IF (@Operation = 'I')

BEGIN

INSERT INTO [Survey_ProjectReportSetting]
           (AccountID,			ProjectID,			CoverPage,			ReportIntroduction,--			RadarChart,			CatQstList,			CatDataChart,--			QstTextResponses,			Conclusionpage,--			CandidateSelfStatus,			ProjectRelationGrp,
--			ProgrammeGrp,
			CatDataChart,
			CatQstList,
			Conclusionpage,
			ReportType,
			PageHeading1,
			PageHeading2,
			PageHeading3,
			PageHeadingColor,
			PageHeadingCopyright,
			PageHeadingIntro,
			PageHeadingConclusion,
			PageLogo,
			FrontPageLogo2,
			FrontPageLogo3,
--			ConclusionHighLowRange,
--			PreviousScoreVisible,

--			BenchMarkScoreVisible,
--			BenchMarkGrpVisible,
--			BenchConclusionpage,
			ConclusionHeading,
			FullProjectGrp,
			AnalysisI,
			AnalysisII,
			AnalysisIII,
			Programme_Average,
			FreeTextResponses,
			ShowScoreRespondents,
			ShowRadar,
			ShowTable,
			ShowPreviousScore1,
			ShowPreviousScore2,
			ShowBarGraph,
			ShowLineChart,
			FrontPdfFileName,
			ScoreTableImage,
			FooterImage,
			RadarGraphCategoryCount
			)
     VALUES
           (@AccountID,			@ProjectID,			@CoverPage,			@ReportIntroduction,--			@RadarChart,			@CatQstList,			@CatDataChart,--			@QstTextResponses,			@Conclusionpage,--			@CandidateSelfStatus,			@ProjectRelationGrp,--			@FullProjectGrp,
--			@ProgrammeGrp,
			@CatDataChart,
			@CatQstList,
			@Conclusionpage,
			@ReportType,
			@PageHeading1,
			@PageHeading2 ,
			@PageHeading3 ,
			@PageHeadingColor ,
			@PageHeadingCopyright,
			@PageHeadingIntro,
			@PageHeadingConclusion,
			@PageLogo,
			@FrontPageLogo2,
			@FrontPageLogo3,
--			@ConclusionHighLowRange,
--			@PreviousScoreVisible,
--			@BenchMarkScoreVisible,
--			@BenchMarkGrpVisible,
--			@BenchConclusionpage,
			@ConclusionHeading,
			@FullProjectGrp,
			@AnalysisI,
			@AnalysisII ,
			@AnalysisIII,
			@Programme_Average,
			@FreeTextResponses,			
			@ShowScoreRespondents,
			@ShowRadar,
			@ShowTable,
			@ShowPreviousScore1,
			@ShowPreviousScore2,
			@ShowBarGraph,
			@ShowLineChart
			,@FrontPdfFileName
			,@ScoreTable
			,@FooterImage,
			@RadarGraphCategoryCount
			)

End

IF (@Operation = 'D')

BEGIN

			Delete [Survey_ProjectReportSetting]
			where ProjectID = @ProjectID
End

IF (@Operation = 'S')

BEGIN

			SELECT * FROM [Survey_ProjectReportSetting]
			where ProjectID = @ProjectID
End
GO
