USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspFetchManagerData]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspFetchManagerData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspFetchManagerData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspFetchManagerData]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[UspFetchManagerData]

@CandidateEmail varchar(100),
@AccountID int,
@SelectFlag char(1)

as

IF (@SelectFlag = ''A'') -- Select Manager Programme data

	BEGIN

		SELECT		dbo.Programme.ProgrammeID, dbo.Programme.ProgrammeName

		FROM        dbo.AssignQuestionnaire INNER JOIN
					dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
					dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID
		                
		WHERE		(dbo.AssignmentDetails.CandidateEmail = @CandidateEmail) AND 
					(dbo.AssignmentDetails.RelationShip = ''Manager'') AND 
					(dbo.AssignQuestionnaire.AccountID = @AccountID)

		GROUP BY	dbo.Programme.ProgrammeID, dbo.Programme.ProgrammeName

	END

ELSE IF (@SelectFlag = ''B'')-- Select Manager Project data

	BEGIN

		SELECT		dbo.Project.ProjectID, dbo.Project.Title

		FROM		dbo.AssignQuestionnaire INNER JOIN
					dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
					dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID

		WHERE		(dbo.AssignmentDetails.CandidateEmail = @CandidateEmail) AND 
					(dbo.AssignmentDetails.RelationShip = ''Manager'') AND 
					(dbo.AssignQuestionnaire.AccountID = @AccountID)

		GROUP BY	dbo.Project.ProjectID, dbo.Project.Title

	END
' 
END
GO
