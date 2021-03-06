USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAddPartictDetailsManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAddPartictDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_UspAddPartictDetailsManagement]

 @PaticipantID int
,@AssignmentID int
,@ProjectID int
,@UserID int
,@Operation char(1)

as

-- Insert

IF (@Operation = 'I')

Begin

INSERT INTO [Survey_PaticipantDetails]
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
  FROM [Survey_PaticipantDetails]

End


IF (@Operation = 'S') -- Id based

Begin

	SELECT  [AssignmentID]
           ,[ProjectID]
           ,[UserID]
           
	FROM [Survey_PaticipantDetails]
	WHERE [UserID] = @UserID

End

IF (@Operation = 'R') -- Only For Report Scheduler Only

Begin

SELECT     dbo.Survey_AssignQuestionnaireParticipant.AssignmentID, dbo.Survey_AssignQuestionnaireParticipant.AccountID, dbo.Survey_AssignQuestionnaireParticipant.ProjecctID, 
                      dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID, dbo.Survey_PaticipantDetails.UserID, dbo.[User].FirstName, dbo.[User].LastName, dbo.Account.Code,

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
                      
FROM         dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
                      dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
                      dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID
WHERE     (dbo.Survey_PaticipantDetails.UserID = @UserID)


End



-- Update
GO
