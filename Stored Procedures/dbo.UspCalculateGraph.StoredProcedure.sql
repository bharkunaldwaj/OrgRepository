USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspCalculateGraph]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspCalculateGraph]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspCalculateGraph]

@QuestionnaireID int,
@CandidateID int


as

SELECT     Count(*)
FROM         dbo.Question INNER JOIN
                      dbo.QuestionAnswer ON dbo.Question.QuestionID = dbo.QuestionAnswer.QuestionID
WHERE     (dbo.Question.QuestionnaireID = @QuestionnaireID) AND (dbo.QuestionAnswer.AssignDetId = @CandidateID) 
AND (dbo.QuestionAnswer.Answer <> '')
GO
