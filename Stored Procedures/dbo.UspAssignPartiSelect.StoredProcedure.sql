USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignPartiSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignPartiSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspAssignPartiSelect]

@AccountID int,
@ProgrammeID int,
@SelectFlag char(1)

as

BEGIN

IF(@SelectFlag = 'A')

BEGIN
	SELECT    dbo.[User].LoginID, dbo.[User].Password , dbo.[User].UserID as AssignmentID,dbo.AssignQuestionnaireParticipant.AccountID, dbo.AssignQuestionnaireParticipant.ProjecctID, dbo.AssignQuestionnaireParticipant.QuestionnaireID, 
                      dbo.Project.Title, dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS UserName, dbo.[User].FirstName, dbo.[User].LastName, 
                      dbo.[User].EmailID, dbo.Account.Code, dbo.AssignQuestionnaireParticipant.AssignmentID, dbo.Questionnaire.QSTNCode, 
                      dbo.Questionnaire.QSTNName, dbo.Programme.ProgrammeName, dbo.Programme.ProgrammeID
FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
	WHERE     (dbo.AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.AssignQuestionnaireParticipant.ProgrammeID = @ProgrammeID)
END

ELSE IF(@SelectFlag = 'C')

BEGIN
	SELECT COUNT(*)
	FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
	WHERE     (dbo.AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.AssignQuestionnaireParticipant.ProgrammeID = @ProgrammeID)
END

END
GO
