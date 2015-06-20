USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspCalculateGraph]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspCalculateGraph]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspCalculateGraph]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspCalculateGraph]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspCalculateGraph]

@QuestionnaireID int,
@CandidateID int


as

SELECT     Count(*)
FROM         dbo.Question INNER JOIN
                      dbo.QuestionAnswer ON dbo.Question.QuestionID = dbo.QuestionAnswer.QuestionID
WHERE     (dbo.Question.QuestionnaireID = @QuestionnaireID) AND (dbo.QuestionAnswer.AssignDetId = @CandidateID) 
AND (dbo.QuestionAnswer.Answer <> '''')
' 
END
GO
