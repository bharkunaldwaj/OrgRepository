USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_QuestionairCount]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_QuestionairCount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_QuestionairCount]


@ProjectID int,
@SelectFlag char(1)

as

BEGIN
	SELECT COUNT(*)
	FROM  dbo.Survey_AssignQuestionnaire INNER JOIN
          dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID INNER JOIN
          dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
          dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
          dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaire.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
          dbo.Survey_Analysis_Sheet ON dbo.Survey_Project.ProjectID = dbo.Survey_Analysis_Sheet.ProjectID
	WHERE dbo.Survey_Analysis_Sheet.ProgrammeID = @ProjectID	


END
GO
