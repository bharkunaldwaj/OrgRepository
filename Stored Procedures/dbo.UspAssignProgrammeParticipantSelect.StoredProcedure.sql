USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignProgrammeParticipantSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignProgrammeParticipantSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspAssignProgrammeParticipantSelect]

@AccountID int,
@ProgrammeID int,
@SelectFlag char(1)

as

BEGIN

       SELECT     dbo.AssignQuestionnaire.AssignmentID, dbo.AssignQuestionnaire.AccountID, dbo.AssignQuestionnaire.QuestionnaireID, 
                      dbo.AssignQuestionnaire.TargetPersonID, dbo.AssignQuestionnaire.Description, dbo.AssignQuestionnaire.CandidateNo, 
                      dbo.AssignmentDetails.AsgnDetailID, dbo.AssignmentDetails.CandidateName, dbo.AssignmentDetails.CandidateEmail, dbo.Project.ProjectID, 
                      dbo.Project.StatusID, dbo.Project.Title, dbo.AssignmentDetails.RelationShip, dbo.Account.Code, dbo.Questionnaire.QSTNName, dbo.[User].UserID, 
                      dbo.[User].FirstName + ' ' + dbo.[User].LastName as FirstName, dbo.Programme.ProgrammeName, dbo.Programme.ProgrammeID,[AssignQuestionnaire].[ProgrammeID]
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID AND 
                      dbo.Project.ProjectID = dbo.Questionnaire.ProjectID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.Programme ON dbo.Project.ProjectID = dbo.Programme.ProjectID
            WHERE     [AssignQuestionnaire].[ProgrammeID] =@ProgrammeID and dbo.Programme.ProgrammeID=@ProgrammeID


END
GO
