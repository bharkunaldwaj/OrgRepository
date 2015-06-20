USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignColleagueSelect]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignColleagueSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAssignColleagueSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignColleagueSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

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
and AD.RelationShip != ''Self''



' 
END
GO
