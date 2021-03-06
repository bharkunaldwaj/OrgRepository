USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[GetQuestionnaireSubmitStatus]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[GetQuestionnaireSubmitStatus]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetQuestionnaireSubmitStatus] 

@TargetPerssonID int,
@SelectFlag char(1)

as


IF(@SelectFlag = 'C')

BEGIN

SELECT     COUNT(dbo.AssignQuestionnaire.AssignmentID) AS SubmitCount
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID
WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @TargetPerssonID) 

END

ELSE IF(@SelectFlag = 'T')

BEGIN

SELECT     COUNT(dbo.AssignQuestionnaire.AssignmentID) AS SubmitCount
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID
WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @TargetPerssonID) AND (dbo.AssignmentDetails.SubmitFlag = 1) 


END

ELSE IF(@SelectFlag = 'S')

BEGIN

SELECT     COUNT(dbo.AssignQuestionnaire.AssignmentID) AS SubmitCount
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID
WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @TargetPerssonID) AND (dbo.AssignmentDetails.SubmitFlag = 1) AND (dbo.AssignmentDetails.RelationShip = 'Self')

END

ELSE IF(@SelectFlag = 'R')

BEGIN

select ReportName from  dbo.ReportManagement
WHERE  dbo.ReportManagement.TargetPersonID = @TargetPerssonID --931

END

ELSE IF(@SelectFlag = 'P') -- Participant Report Information

BEGIN

SELECT     dbo.AssignQuestionnaireParticipant.AssignmentID, dbo.AssignQuestionnaireParticipant.AccountID, dbo.AssignQuestionnaireParticipant.ProjecctID, 
                      dbo.AssignQuestionnaireParticipant.ProgrammeID, dbo.PaticipantDetails.UserID, dbo.[User].FirstName, dbo.[User].LastName, dbo.Account.Code,

(Select TotalCount from dbo.ReportManagement where TargetPersonID=@TargetPerssonID) as TotalCount,

(Select SubmitCount from dbo.ReportManagement where TargetPersonID=@TargetPerssonID) as SubmitCount,

(Select SelfAssessment from dbo.ReportManagement where TargetPersonID=@TargetPerssonID) as SelfAssessment,

(select ReportName from  dbo.ReportManagement WHERE TargetPersonID = @TargetPerssonID) as ReportName
                      
FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID
WHERE     (dbo.PaticipantDetails.UserID = @TargetPerssonID)

END
GO
