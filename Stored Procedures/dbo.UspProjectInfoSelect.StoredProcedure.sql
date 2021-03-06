USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProjectInfoSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProjectInfoSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspProjectInfoSelect]

@QuestionnaireID int,
@CandidateID int


as

SELECT     dbo.Questionnaire.QSTNCode, dbo.Questionnaire.QSTNName, dbo.Questionnaire.QSTNDescription,dbo.Questionnaire.DisplayCategory, dbo.Project.ProjectID, dbo.Project.Reference, dbo.Project.Title, 
			dbo.[User].FirstName, dbo.[User].LastName, dbo.Project.Description, dbo.[User].FirstName + ' ' +  dbo.[User].LastName as FullName, dbo.[User].EmailID
FROM         dbo.AssignQuestionnaire INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
                      dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID
WHERE     (dbo.AssignQuestionnaire.QuestionnaireID = @QuestionnaireID) AND (dbo.AssignmentDetails.AsgnDetailID = @CandidateID) --and dbo.Project.StatusID = 1
GO
