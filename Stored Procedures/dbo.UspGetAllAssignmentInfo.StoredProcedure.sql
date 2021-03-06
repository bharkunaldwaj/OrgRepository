USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetAllAssignmentInfo]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspGetAllAssignmentInfo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspGetAllAssignmentInfo] 

@CandidateId int

as

SELECT     dbo.AssignmentDetails.CandidateName, dbo.AssignmentDetails.RelationShip, dbo.AssignmentDetails.CandidateEmail, dbo.Questionnaire.QSTNCode, dbo.Questionnaire.QSTNName, 
                      dbo.Questionnaire.QSTNDescription, dbo.Project.Title, dbo.Project.Description, dbo.Project.Logo, dbo.Account.Code, dbo.Account.AccountID,dbo.Account.OrganisationName, 
                      dbo.Account.CompanyLogo, dbo.Account.HeaderBGColor, dbo.Account.MenuBGColor, dbo.Account.CopyRightLine,dbo.AssignQuestionnaire.ProgrammeID
FROM         dbo.Project INNER JOIN
                      dbo.Account ON dbo.Project.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.AssignmentDetails INNER JOIN
                      dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID ON 
                      dbo.Project.ProjectID = dbo.AssignQuestionnaire.ProjecctID INNER JOIN
                      dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID
WHERE     (dbo.AssignmentDetails.AsgnDetailID = @CandidateId) and dbo.project.StatusID = 1
GO
