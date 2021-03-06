USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantBenchScoreDetailsSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspParticipantBenchScoreDetailsSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UspParticipantBenchScoreDetailsSelect]

@ProjectID int
,@ProgrammeID int
,@QuestionnaireID int
,@TargetPersonID int
,@ScoreMonth int
,@ScoreYear int
,@Operation char(1)

as

-- Insert

IF (@Operation = 'A')

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
GO
