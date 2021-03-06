USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspUpdateCategorySequence]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspUpdateCategorySequence]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspUpdateCategorySequence]

@AccountID int,
@QuestionnaireID int,
@Increment int

as

DECLARE @CategoryID int
Declare @Counter int
DECLARE @CUR_CategoryID CURSOR

SET @CUR_CategoryID = CURSOR FOR
SELECT     CategoryID
FROM         Survey_Category
WHERE     (AccountID = @AccountID) AND (QuestionnaireID = @QuestionnaireID)
ORDER BY Sequence, ModifiedDate, CategoryName

OPEN @CUR_CategoryID

FETCH NEXT
FROM @CUR_CategoryID INTO @CategoryID

set @Counter=@Increment

WHILE @@FETCH_STATUS = 0
BEGIN
	
	--PRINT @CategoryID

	update [Survey_Category] set [Sequence]=@Counter where [CategoryID]=@CategoryID

	set @Counter=@Counter + @Increment

	FETCH NEXT
	FROM @CUR_CategoryID INTO @CategoryID

END

CLOSE @CUR_CategoryID
DEALLOCATE @CUR_CategoryID
GO
