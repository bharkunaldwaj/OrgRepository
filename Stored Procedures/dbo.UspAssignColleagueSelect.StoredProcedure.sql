USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignColleagueSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignColleagueSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspAssignColleagueSelect]

@UserID int,
@ProgrammeID int

as

SELECT AQ.AccountID,AQ.TargetPersonID,AQ.ProjecctID as ProjectID,AQ.AssignmentID as AssignID,AD.RelationShip As Relationship,
	   Ad.CandidateName As Name,AD.CandidateEmail As EmailID,AD.AsgnDetailID as AssignmentID,AD.SubmitFlag,AD.EmailSendFlag
FROM 
dbo.AssignQuestionnaire AQ INNER JOIN 
dbo.AssignmentDetails AD ON AQ.AssignmentID = AD.AssignmentID 
INNER JOIN dbo.[User] U ON AQ.TargetPersonID =U.UserID
WHERE AQ.ProgrammeID=@ProgrammeID AND U.UserID=@UserID
and AD.RelationShip != 'Self'
GO
