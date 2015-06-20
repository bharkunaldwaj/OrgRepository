USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_QuestionairCount]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_QuestionairCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_QuestionairCount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_QuestionairCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[Survey_QuestionairCount]


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
' 
END
GO
