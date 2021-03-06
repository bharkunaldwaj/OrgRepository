USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignSelect1]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignSelect1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[UspAssignSelect1]

@UserID int,
@ProjectID int,
@SelectFlag char(1)

as

IF (@SelectFlag = 'A')

BEGIN
--[UspAssignSelect] 289,137,'A'
--PRINT '100' + @UserID + ' ' + @ProjectID

IF ((@UserID != '') and (@ProjectID != ''))

	BEGIN
--PRINT @UserID + ' ' + @ProjectID
		SELECT        dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, 
                      dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignQuestionnaire.Description, dbo.AssignQuestionnaire.CandidateNo, 
                      dbo.AssignmentDetails.AsgnDetailID AS AssignmentID, dbo.AssignmentDetails.CandidateName, dbo.AssignmentDetails.CandidateEmail, dbo.Project.ProjectID, 
                      dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, dbo.Account.Code, dbo.Questionnaire.QSTNName, dbo.[User].UserID, 
                      dbo.[User].FirstName + ' ' + dbo.[User].LastName AS FirstName, dbo.Programme.ProgrammeName
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
					WHERE    ([User].UserID =  @UserID)  AND dbo.Programme.ProgrammeID = @ProjectID	

	END

ELSE IF (@UserID != '')

	BEGIN
--PRINT 'TEST2'
		SELECT     dbo.AssignQuestionnaire.AssignmentID, dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, 
                      dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignQuestionnaire.Description, dbo.AssignQuestionnaire.CandidateNo, 
                      dbo.AssignmentDetails.AsgnDetailID, dbo.AssignmentDetails.CandidateName, dbo.AssignmentDetails.CandidateEmail, dbo.Project.ProjectID, 
                      dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, dbo.Account.Code, dbo.Questionnaire.QSTNName, dbo.[User].UserID, 
                      dbo.[User].FirstName + ' ' + dbo.[User].LastName AS FirstName, dbo.Programme.ProgrammeName
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
		WHERE    ([User].UserID =  @UserID) 

	END

ELSE 

	BEGIN
--PRINT 'TEST3'
		SELECT     dbo.AssignQuestionnaire.AssignmentID, dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, 
                      dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignQuestionnaire.Description, dbo.AssignQuestionnaire.CandidateNo, 
                      dbo.AssignmentDetails.AsgnDetailID, dbo.AssignmentDetails.CandidateName, dbo.AssignmentDetails.CandidateEmail, dbo.Project.ProjectID, 
                      dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, dbo.Account.Code, dbo.Questionnaire.QSTNName, dbo.[User].UserID, 
                      dbo.[User].FirstName + ' ' + dbo.[User].LastName AS FirstName, dbo.Programme.ProgrammeName
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
		

	END

END

IF (@SelectFlag = 'C')

BEGIN

IF ((@UserID != '') and (@ProjectID != ''))

	BEGIN

		SELECT     COUNT(*)
		FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
		    WHERE    ([User].UserID =  @UserID)  AND dbo.Programme.ProgrammeID = @ProjectID	

	END

ELSE IF (@UserID != '')

	BEGIN

		SELECT     COUNT(*)
		FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
		WHERE    ([User].UserID =  @UserID) 

	END

ELSE 

	BEGIN

		SELECT     COUNT(*)
		FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
		
		

	END


END
GO
