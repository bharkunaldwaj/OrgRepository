USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspparticipantSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspparticipantSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspparticipantSelect]

@AccountID int,
@CategoryID int,
@SelectFlag char(1)

as


BEGIN

	
	IF (@SelectFlag = 'Q') -- Get Record Count

	Begin

	SELECT count(*) FROM  dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID AND 
                      dbo.Project.ProjectID = dbo.Questionnaire.ProjectID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID where AssignQuestionnaire.IsActive=1 AND ([User].UserID =  @AccountID) 
                      AND (dbo.AssignQuestionnaire.ProjecctID = @CategoryID)
	End
	
	
	ELSE IF (@SelectFlag = 'P') -- Get Record Count

	Begin

	
	
	SELECT  count(*)   
         	
	FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
						  dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
						  dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
						  dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
						  dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
						  dbo.Questionnaire ON dbo.AssignQuestionnaireParticipant.QuestionnaireID = dbo.Questionnaire.QuestionnaireID
	WHERE     (dbo.AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.AssignQuestionnaireParticipant.ProjecctID = @CategoryID)
	                 AND AssignQuestionnaireParticipant.IsActive = 1
	
	
	End

	ELSE IF (@SelectFlag = 'Z') -- Get Record Count

	Begin

	
	
	SELECT  count(*)   
         	
	FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID AND 
                      dbo.Project.ProjectID = dbo.Questionnaire.ProjectID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
            WHERE     [AssignQuestionnaire].[ProgrammeID] = @CategoryID and dbo.Programme.ProgrammeID=@CategoryID
	
	
	End

END
GO
