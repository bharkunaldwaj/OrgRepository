USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurGetColleaguesList]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCurGetColleaguesList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspCurGetColleaguesList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspCurGetColleaguesList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'--[RspCurGetColleaguesList] 968
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
' 
END
GO
