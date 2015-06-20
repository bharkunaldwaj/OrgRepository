USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspUpdateQuestionSequence]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspUpdateQuestionSequence]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspUpdateQuestionSequence]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspUpdateQuestionSequence]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspUpdateQuestionSequence]

@AccountID int,
@QuestionnaireID int,
@Increment int

as

DECLARE @QuestionID int
Declare @Counter int
DECLARE @CUR_QuestionID CURSOR

SET @CUR_QuestionID = CURSOR FOR
SELECT     QuestionID
FROM         Question
WHERE     (AccountID = @AccountID) AND (QuestionnaireID = @QuestionnaireID)
ORDER BY Sequence, ModifyDate, Title

OPEN @CUR_QuestionID

FETCH NEXT
FROM @CUR_QuestionID INTO @QuestionID

set @Counter=@Increment

WHILE @@FETCH_STATUS = 0
BEGIN
	
	--PRINT @Counter

	update [Question] set [Sequence]=@Counter where [QuestionID]=@QuestionID

	set @Counter=@Counter + @Increment

	FETCH NEXT
	FROM @CUR_QuestionID INTO @QuestionID

END

CLOSE @CUR_QuestionID
DEALLOCATE @CUR_QuestionID
' 
END
GO
