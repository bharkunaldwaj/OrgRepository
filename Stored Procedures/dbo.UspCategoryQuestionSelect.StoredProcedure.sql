USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspCategoryQuestionSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspCategoryQuestionSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspCategoryQuestionSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspCategoryQuestionSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create procedure [dbo].[UspCategoryQuestionSelect]

@CategoryID int

as

SELECT		Title, 
			Description, 
			Hint, 
            CASE WHEN QuestionTypeID = 2 THEN ''Range'' WHEN QuestionTypeID = 1 THEN ''Free Text'' END AS [Type]

FROM         dbo.Question
WHERE     (CateogryID = @CategoryID)
ORDER BY Sequence
' 
END
GO
