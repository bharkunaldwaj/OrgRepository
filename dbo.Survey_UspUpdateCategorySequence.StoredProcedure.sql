USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspUpdateCategorySequence]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspUpdateCategorySequence]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspUpdateCategorySequence]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspUpdateCategorySequence]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_UspUpdateCategorySequence]

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
' 
END
GO
