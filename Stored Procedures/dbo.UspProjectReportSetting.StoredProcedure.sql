USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProjectReportSetting]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProjectReportSetting]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UspProjectReportSetting]
	@ProjectReportSettingID int,
	@AccountID int,
	@ProjectID int,
    @CoverPage Varchar(5),
    @ReportIntroduction Varchar(5),
    @RadarChart Varchar(5),
    @CatQstList Varchar(5),
    @CatDataChart Varchar(5),
    @QstTextResponses Varchar(5),
    @Conclusionpage Varchar(5),
    @CandidateSelfStatus Varchar(5),
    @ProjectRelationGrp Varchar(500),
    @FullProjectGrp Varchar(5),    
    @ProgrammeGrp Varchar(5),    
    @ReportType Varchar(5), 
    @PageHeading1 Varchar(500),
    @PageHeading2 Varchar(500),
    @PageHeading3 Varchar(500),
    @PageHeadingColor Varchar(500),
    @PageHeadingCopyright Varchar(500),
    @PageHeadingIntro Varchar(5000),
    @PageHeadingConclusion Varchar(5000),   
    @PageLogo Varchar(250),
    @FrontPageLogo2 Varchar(250),
    @FrontPageLogo3 Varchar(250),
    @ConclusionHighLowRange Varchar(100),
    @PreviousScoreVisible Varchar(5),
    @BenchMarkScoreVisible Varchar(5),
    @BenchMarkGrpVisible Varchar(5),
    @BenchConclusionpage Varchar(5),
    @ConclusionHeading varchar(50),
    @Operation char(2),
    @FrontPageLogo4 Varchar(250)
as
-- Insert 


IF (@Operation = 'I')

BEGIN

INSERT INTO [ProjectReportSetting]
           (AccountID,			ProjectID,			CoverPage,			ReportIntroduction,			RadarChart,			CatQstList,			CatDataChart,			QstTextResponses,			Conclusionpage,			CandidateSelfStatus,			ProjectRelationGrp,			FullProjectGrp,
			ProgrammeGrp,
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
			ConclusionHighLowRange,
			PreviousScoreVisible,
			BenchMarkScoreVisible,
			BenchMarkGrpVisible,
			BenchConclusionpage,
			ConclusionHeading,
			FrontPageLogo4
			)
     VALUES
           (@AccountID,			@ProjectID,			@CoverPage,			@ReportIntroduction,			@RadarChart,			@CatQstList,			@CatDataChart,			@QstTextResponses,			@Conclusionpage,			@CandidateSelfStatus,			@ProjectRelationGrp,			@FullProjectGrp,
			@ProgrammeGrp,
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
			@ConclusionHighLowRange,
			@PreviousScoreVisible,
			@BenchMarkScoreVisible,
			@BenchMarkGrpVisible,
			@BenchConclusionpage,
			@ConclusionHeading,
			@FrontPageLogo4)

End

IF (@Operation = 'D')

BEGIN

			Delete [ProjectReportSetting]
			where ProjectID = @ProjectID
End

IF (@Operation = 'S')

BEGIN

			SELECT * FROM [ProjectReportSetting]
			where ProjectID = @ProjectID
End
GO
