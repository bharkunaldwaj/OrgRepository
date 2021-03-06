USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_GetQuestionnaireSubmitStatus]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_GetQuestionnaireSubmitStatus]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_GetQuestionnaireSubmitStatus] 

@accountid int,
@projectid int,
@programmeid int,
@SelectFlag char(1)

as


IF(@SelectFlag = 'C')

BEGIN

SELECT     COUNT(dbo.Survey_AssignQuestionnaire.AssignmentID) AS SubmitCount
FROM         dbo.Survey_AssignQuestionnaire INNER JOIN
                      dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID
WHERE     (dbo.Survey_AssignQuestionnaire.AccountID = @accountid  and dbo.Survey_AssignQuestionnaire.ProjecctID=@projectid
and dbo.Survey_AssignQuestionnaire.ProgrammeID=@programmeid ) 

END

ELSE IF(@SelectFlag = 'T')

BEGIN

SELECT     COUNT(dbo.Survey_AssignQuestionnaire.AssignmentID) AS SubmitCount
FROM         dbo.Survey_AssignQuestionnaire INNER JOIN
                      dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID
WHERE     (dbo.Survey_AssignQuestionnaire.AccountID = @accountid  and dbo.Survey_AssignQuestionnaire.ProjecctID=@projectid
and dbo.Survey_AssignQuestionnaire.ProgrammeID=@programmeid ) AND (dbo.Survey_AssignmentDetails.SubmitFlag = 1) 


END

ELSE IF(@SelectFlag = 'S')

BEGIN

SELECT     COUNT(dbo.Survey_AssignQuestionnaire.AssignmentID) AS SubmitCount
FROM         dbo.Survey_AssignQuestionnaire INNER JOIN
                      dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID
WHERE     (dbo.Survey_AssignQuestionnaire.AccountID = @accountid  and dbo.Survey_AssignQuestionnaire.ProjecctID=@projectid
and dbo.Survey_AssignQuestionnaire.ProgrammeID=@programmeid ) AND (dbo.Survey_AssignmentDetails.SubmitFlag = 1) 

END

ELSE IF(@SelectFlag = 'R')

BEGIN

select ReportName from  dbo.Survey_ReportManagement
WHERE dbo.Survey_ReportManagement.AccountID = @accountid  and dbo.Survey_ReportManagement.ProjectID=@projectid
and dbo.Survey_ReportManagement.ProgramID=@programmeid

END

ELSE IF(@SelectFlag = 'P') -- Participant Report Information

BEGIN

SELECT     dbo.Survey_AssignQuestionnaireParticipant.AssignmentID, dbo.Survey_AssignQuestionnaireParticipant.AccountID, dbo.Survey_AssignQuestionnaireParticipant.ProjecctID, 
                      dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID, dbo.Survey_PaticipantDetails.UserID, dbo.[User].FirstName, dbo.[User].LastName, dbo.Account.Code,

(Select TotalCount from dbo.Survey_ReportManagement where dbo.Survey_ReportManagement.AccountID = @accountid  and dbo.Survey_ReportManagement.ProjectID=@projectid
and dbo.Survey_ReportManagement.ProgramID=@programmeid) as TotalCount,

(Select SubmitCount from dbo.Survey_ReportManagement where dbo.Survey_ReportManagement.AccountID = @accountid  and dbo.Survey_ReportManagement.ProjectID=@projectid
and dbo.Survey_ReportManagement.ProgramID=@programmeid) as SubmitCount,

(Select SelfAssessment from dbo.Survey_ReportManagement where dbo.Survey_ReportManagement.AccountID = @accountid  and dbo.Survey_ReportManagement.ProjectID=@projectid
and dbo.Survey_ReportManagement.ProgramID=@programmeid) as SelfAssessment,

(select ReportName from  dbo.Survey_ReportManagement WHERE dbo.Survey_ReportManagement.AccountID = @accountid  and dbo.Survey_ReportManagement.ProjectID=@projectid
and dbo.Survey_ReportManagement.ProgramID=@programmeid) as ReportName
                      
FROM         dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
                      dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
                      dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID
WHERE     (dbo.Survey_PaticipantDetails.UserID = [User].UserID)

END
GO
