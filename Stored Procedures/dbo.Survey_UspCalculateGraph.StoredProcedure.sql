USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspCalculateGraph]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspCalculateGraph]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspCalculateGraph]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspCalculateGraph]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_UspCalculateGraph]

@QuestionnaireID int,
@CandidateID int


as

SELECT     Count(*)
FROM         dbo.Survey_Question INNER JOIN
                      dbo.Survey_QuestionAnswer ON dbo.Survey_Question.QuestionID = dbo.Survey_QuestionAnswer.QuestionID
WHERE     (dbo.Survey_Question.QuestionnaireID = @QuestionnaireID) AND (dbo.Survey_QuestionAnswer.AssignDetId = @CandidateID) 
AND (dbo.Survey_QuestionAnswer.Answer <> '''')
' 
END
GO
