USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspCategoryQuestionSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspCategoryQuestionSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[UspCategoryQuestionSelect]

@CategoryID int

as

SELECT		Title, 
			Description, 
			Hint, 
            CASE WHEN QuestionTypeID = 2 THEN 'Range' WHEN QuestionTypeID = 1 THEN 'Free Text' END AS [Type]

FROM         dbo.Question
WHERE     (CateogryID = @CategoryID)
ORDER BY Sequence
GO
