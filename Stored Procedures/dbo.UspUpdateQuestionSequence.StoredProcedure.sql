USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspUpdateQuestionSequence]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspUpdateQuestionSequence]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspUpdateQuestionSequence]

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
GO
