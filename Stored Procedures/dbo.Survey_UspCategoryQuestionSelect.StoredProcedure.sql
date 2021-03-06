USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspCategoryQuestionSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspCategoryQuestionSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspCategoryQuestionSelect]

@CategoryID int

as

SELECT		Title, 
			Description,
			Sequence, 
			Hint, 
            CASE WHEN QuestionTypeID = 2 THEN 'Range' WHEN QuestionTypeID = 1 THEN 'Free Text' END AS [Type]

FROM         dbo.Survey_Question
WHERE     (CateogryID = @CategoryID)
ORDER BY Sequence
GO
