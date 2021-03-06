USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAddPartictDetailsManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAddPartictDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAddPartictDetailsManagement]

 @PaticipantID int
,@AssignmentID int
,@ProjectID int
,@UserID int
,@Operation char(1)

as

-- Insert

IF (@Operation = 'I')

Begin

INSERT INTO [PaticipantDetails]
           (
            [AssignmentID]
           ,[ProjectID]
           ,[UserID]
            )
     VALUES
			(
			 @AssignmentID
			,@ProjectID
			,@UserID
			 )

SELECT ISNULL(MAX([PaticipantID]),1)
  FROM [PaticipantDetails]

End


IF (@Operation = 'S') -- Id based

Begin

	SELECT  [AssignmentID]
           ,[ProjectID]
           ,[UserID]
           
	FROM [PaticipantDetails]
	WHERE [UserID] = @UserID

End

IF (@Operation = 'R') -- Only For Report Scheduler Only

Begin

SELECT     dbo.AssignQuestionnaireParticipant.AssignmentID, dbo.AssignQuestionnaireParticipant.AccountID, dbo.AssignQuestionnaireParticipant.ProjecctID, 
                      dbo.AssignQuestionnaireParticipant.ProgrammeID, dbo.PaticipantDetails.UserID, dbo.[User].FirstName, dbo.[User].LastName, dbo.Account.Code,

(Select TotalCount from dbo.ReportManagement where TargetPersonID=@UserID) as TotalCount,

(Select SubmitCount from dbo.ReportManagement where TargetPersonID=@UserID) as SubmitCount,

(Select SelfAssessment from dbo.ReportManagement where TargetPersonID=@UserID) as SelfAssessment,

(select ReportName from  dbo.ReportManagement WHERE TargetPersonID = @UserID) as ReportName

--(SELECT     COUNT(dbo.AssignQuestionnaire.AssignmentID) 
--FROM         dbo.AssignQuestionnaire INNER JOIN
--                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID
--WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @UserID) ) as TotalCount,

--(SELECT     COUNT(dbo.AssignQuestionnaire.AssignmentID)
--FROM         dbo.AssignQuestionnaire INNER JOIN
--                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID
--WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @UserID) AND (dbo.AssignmentDetails.SubmitFlag = 1) ) as SubmitCount,

--(SELECT     COUNT(dbo.AssignQuestionnaire.AssignmentID) 
--FROM         dbo.AssignQuestionnaire INNER JOIN
--                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID
--WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @UserID) AND (dbo.AssignmentDetails.SubmitFlag = 1) AND (dbo.AssignmentDetails.RelationShip = 'Self')) as SelfAssessment,

--(select ReportName from  dbo.ReportManagement
--WHERE  dbo.ReportManagement.TargetPersonID = @UserID) as ReportName
                      
FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID
WHERE     (dbo.PaticipantDetails.UserID = @UserID)


End



-- Update
GO
