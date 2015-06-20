USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAssignPartiSelect]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAssignPartiSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspAssignPartiSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAssignPartiSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[Survey_UspAssignPartiSelect]

@AccountID int,
@ProgrammeID int,
@SelectFlag char(1)

as

BEGIN

IF(@SelectFlag = ''A'')

BEGIN
	SELECT    dbo.[User].LoginID, dbo.[User].Password , dbo.[User].UserID as AssignmentID,dbo.Survey_AssignQuestionnaire.AccountID, dbo.Survey_AssignQuestionnaire.ProjecctID, dbo.Survey_AssignQuestionnaire.QuestionnaireID, 
                      dbo.Survey_Project.Title, dbo.[User].UserID, dbo.[User].FirstName + '' '' + dbo.[User].LastName AS UserName, dbo.[User].FirstName, dbo.[User].LastName, 
                      dbo.[User].EmailID, dbo.Account.Code, dbo.Survey_AssignQuestionnaire.AssignmentID, dbo.Survey_Questionnaire.QSTNCode, 
                      dbo.Survey_Questionnaire.QSTNName, dbo.Survey_Analysis_Sheet.ProgrammeName, dbo.Survey_Analysis_Sheet.ProgrammeID
FROM         dbo.Survey_AssignQuestionnaire INNER JOIN
                      dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Survey_Questionnaire ON dbo.Survey_Project.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
                      dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaire.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
	WHERE     (dbo.Survey_AssignQuestionnaire.AccountID = @AccountID) AND (dbo.Survey_AssignQuestionnaire.ProgrammeID = @ProgrammeID)
END

ELSE IF(@SelectFlag = ''C'')

BEGIN
	SELECT COUNT(*)
	FROM         dbo.Survey_AssignQuestionnaire INNER JOIN
                      dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Survey_Questionnaire ON dbo.Survey_Project.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
                      dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaire.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
	WHERE     (dbo.Survey_AssignQuestionnaire.AccountID = @AccountID) AND (dbo.Survey_AssignQuestionnaire.ProgrammeID = @ProgrammeID)
END

END




' 
END
GO
