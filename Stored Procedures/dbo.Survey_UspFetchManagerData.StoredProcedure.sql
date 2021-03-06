USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspFetchManagerData]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspFetchManagerData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Survey_UspFetchManagerData]

@CandidateEmail varchar(100),
@AccountID int,
@SelectFlag char(1)

as

IF (@SelectFlag = 'A') -- Select Manager Programme data

	BEGIN

		SELECT		dbo.Survey_Analysis_Sheet.ProgrammeID, dbo.Survey_Analysis_Sheet.ProgrammeName

		FROM        dbo.AssignQuestionnaire INNER JOIN
					dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
					dbo.Survey_Analysis_Sheet ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
		                
		WHERE		(dbo.AssignmentDetails.CandidateEmail = @CandidateEmail) AND 
					
					(dbo.AssignQuestionnaire.AccountID = @AccountID)

		GROUP BY	dbo.Survey_Analysis_Sheet.ProgrammeID, dbo.Survey_Analysis_Sheet.ProgrammeName

	END

ELSE IF (@SelectFlag = 'B')-- Select Manager Project data

	BEGIN

		SELECT		dbo.Project.ProjectID, dbo.Project.Title

		FROM		dbo.AssignQuestionnaire INNER JOIN
					dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
					dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID

		WHERE		(dbo.AssignmentDetails.CandidateEmail = @CandidateEmail) AND 
					
					(dbo.AssignQuestionnaire.AccountID = @AccountID)

		GROUP BY	dbo.Project.ProjectID, dbo.Project.Title

	END
GO
