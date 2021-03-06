USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspCalculateGraph]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspCalculateGraph]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspCalculateGraph]

@QuestionnaireID int,
@CandidateID int


as

SELECT     Count(*)
FROM         dbo.Survey_Question INNER JOIN
                      dbo.Survey_QuestionAnswer ON dbo.Survey_Question.QuestionID = dbo.Survey_QuestionAnswer.QuestionID
WHERE     (dbo.Survey_Question.QuestionnaireID = @QuestionnaireID) AND (dbo.Survey_QuestionAnswer.AssignDetId = @CandidateID) 
AND (dbo.Survey_QuestionAnswer.Answer <> '')
GO
