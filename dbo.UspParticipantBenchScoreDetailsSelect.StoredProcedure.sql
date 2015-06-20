USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantBenchScoreDetailsSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantBenchScoreDetailsSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspParticipantBenchScoreDetailsSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantBenchScoreDetailsSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[UspParticipantBenchScoreDetailsSelect]

@ProjectID int
,@ProgrammeID int
,@QuestionnaireID int
,@TargetPersonID int
,@ScoreMonth int
,@ScoreYear int
,@Operation char(1)

as

-- Insert

IF (@Operation = ''A'')

Begin

	SELECT		dbo.ParticipantBenchmarkDetails.CategoryID, 
				dbo.Category.CategoryName, 
				dbo.ParticipantBenchmarkDetails.Score,
				dbo.ParticipantBenchmark.BenchmarkName

	FROM        dbo.ParticipantBenchmark INNER JOIN
				dbo.ParticipantBenchmarkDetails ON dbo.ParticipantBenchmark.BenchmarkID = dbo.ParticipantBenchmarkDetails.BenchmarkID INNER JOIN
				dbo.Category ON dbo.ParticipantBenchmarkDetails.CategoryID = dbo.Category.CategoryID

	WHERE		(dbo.ParticipantBenchmark.ProjectID = @ProjectID) 
				AND (dbo.ParticipantBenchmark.ProgrammeID = @ProgrammeID) 
				AND (dbo.ParticipantBenchmark.QuestionnaireID = @QuestionnaireID) 
				AND (dbo.ParticipantBenchmark.TargetPersonID = @TargetPersonID) 
				AND (dbo.ParticipantBenchmark.ScoreMonth = @ScoreMonth) 
				AND (dbo.ParticipantBenchmark.ScoreYear = @ScoreYear)

END
' 
END
GO
