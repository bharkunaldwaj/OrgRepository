USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantScoreDetailsSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantScoreDetailsSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspParticipantScoreDetailsSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantScoreDetailsSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspParticipantScoreDetailsSelect]

@ProjectID int
,@ProgrammeID int
,@QuestionnaireID int
,@TargetPersonID int
,@ScoreMonth int
,@ScoreYear int
,@Operation char(1)

as

-- Insert

IF (@Operation = ''1'')

Begin

	SELECT		dbo.ParticipantScoreDetails.ScoreMonth,
				dbo.ParticipantScoreDetails.ScoreYear,
				dbo.ParticipantScoreDetails.CategoryID, 
				dbo.Category.CategoryName, 
				dbo.ParticipantScoreDetails.Score as Score1

				
	FROM        dbo.ParticipantScore INNER JOIN
				dbo.ParticipantScoreDetails ON dbo.ParticipantScore.ScoreID = dbo.ParticipantScoreDetails.ScoreID INNER JOIN
				dbo.Category ON dbo.ParticipantScoreDetails.CategoryID = dbo.Category.CategoryID

	WHERE		(dbo.ParticipantScore.ProjectID = @ProjectID) 
				AND (dbo.ParticipantScore.ProgrammeID = @ProgrammeID) 
				AND (dbo.ParticipantScore.QuestionnaireID = @QuestionnaireID) 
				AND (dbo.ParticipantScore.TargetPersonID = @TargetPersonID) 
				AND (dbo.ParticipantScoreDetails.ScoreMonth = @ScoreMonth OR @ScoreMonth = 0) 
				AND (dbo.ParticipantScoreDetails.ScoreYear = @ScoreYear OR @ScoreYear = 0)
				AND dbo.ParticipantScoreDetails.ScoreType=1

END

ELSE IF (@Operation = ''2'')

Begin

	SELECT		dbo.ParticipantScoreDetails.ScoreMonth,
				dbo.ParticipantScoreDetails.ScoreYear,
				dbo.ParticipantScoreDetails.CategoryID, 
				dbo.Category.CategoryName, 
				dbo.ParticipantScoreDetails.Score as Score2

				
	FROM        dbo.ParticipantScore INNER JOIN
				dbo.ParticipantScoreDetails ON dbo.ParticipantScore.ScoreID = dbo.ParticipantScoreDetails.ScoreID INNER JOIN
				dbo.Category ON dbo.ParticipantScoreDetails.CategoryID = dbo.Category.CategoryID

	WHERE		(dbo.ParticipantScore.ProjectID = @ProjectID) 
				AND (dbo.ParticipantScore.ProgrammeID = @ProgrammeID) 
				AND (dbo.ParticipantScore.QuestionnaireID = @QuestionnaireID) 
				AND (dbo.ParticipantScore.TargetPersonID = @TargetPersonID) 
				AND (dbo.ParticipantScoreDetails.ScoreMonth = @ScoreMonth OR @ScoreMonth = 0) 
				AND (dbo.ParticipantScoreDetails.ScoreYear = @ScoreYear OR @ScoreMonth = 0)
				AND dbo.ParticipantScoreDetails.ScoreType=2

END
' 
END
GO
