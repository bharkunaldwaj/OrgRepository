USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurGetColleaguesList]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurGetColleaguesList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[RspCurGetColleaguesList] 968
CREATE proc [dbo].[RspCurGetColleaguesList]

@TargetPersonID int

as

SELECT  dbo.AssignQuestionnaire.TargetPersonID, 
		dbo.AssignmentDetails.CandidateName, 
		dbo.AssignmentDetails.RelationShip
		
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID

WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID) and dbo.AssignmentDetails.SubmitFlag = 1

ORDER BY dbo.AssignmentDetails.RelationShip
GO
