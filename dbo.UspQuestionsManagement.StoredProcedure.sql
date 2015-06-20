USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspQuestionsManagement]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspQuestionsManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspQuestionsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspQuestionsManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[UspQuestionsManagement]
@QuestionID	int,
@AccountID int,
@CompanyID int,
@QuestionnaireID int,
@QuestionTypeID int,
@CateogryID int,
@Sequence int,
@Validation int,
@ValidationText varchar(50),
@Title varchar(50),
@Description NVARCHAR(3000),
@DescriptionSelf NVARCHAR(3000),
@Hint NVARCHAR(2000),
@Token int,
@TokenText varchar(50),
@LengthMIN int,
@LengthMAX int,
@Multiline bit,
@LowerLabel varchar(50),
@UpperLabel varchar(50),
@LowerBound int,
@UpperBound int,
@Increment int,
@Reverse bit,
@ModifyBy int,
@ModifyDate datetime,
@IsActive int,
@Operation char(1)

as

--Insert
IF (@Operation = ''I'')

Begin

INSERT INTO [Question]
           ([AccountID]
           ,[CompanyID]
           ,[QuestionnaireID]
           ,[QuestionTypeID]
           ,[CateogryID]
           ,[Sequence]
           ,[Validation]
           ,[ValidationText]
           ,[Title]
           ,[Description]
           ,[DescriptionSelf]
           ,[Hint]
           ,[Token]
           ,[TokenText]
           ,[LengthMIN]
           ,[LengthMAX]
           ,[Multiline]
           ,[LowerLabel]
           ,[UpperLabel]
           ,[LowerBound]
           ,[UpperBound]
           ,[Increment]
           ,[Reverse]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
     VALUES
           (@AccountID,
            @CompanyID,
            @QuestionnaireID,
            @QuestionTypeID,
            @CateogryID,
            @Sequence,
            @Validation,
            @ValidationText,
            @Title,
            @Description,
            @DescriptionSelf,
            @Hint,
            @Token,
            @TokenText,
            @LengthMIN,
            @LengthMAX,
            @Multiline,
            @LowerLabel,
            @UpperLabel,
            @LowerBound,
            @UpperBound,
            @Increment,
            @Reverse,
            @ModifyBy,
            @ModifyDate,
            @IsActive
            )

End

--Update
Else IF (@Operation = ''U'')

Begin

UPDATE [Question]
SET
     [AccountID] = @AccountID
	,[CompanyID] = @CompanyID
	,[QuestionnaireID] = @QuestionnaireID
	,[QuestionTypeID] = @QuestionTypeID
	,[CateogryID] = @CateogryID
	,[Sequence] = @Sequence
	,[Validation] = @Validation
	,[ValidationText] = @ValidationText
	,[Title] = @Title
	,[Description] = @Description
	,[DescriptionSelf]=@DescriptionSelf
	,[Hint] = @Hint
	,[Token] = @Token
	,[TokenText] = @TokenText
	,[LengthMIN] = @LengthMIN
	,[LengthMAX] = @LengthMAX
	,[Multiline] = @Multiline
	,[LowerLabel] = @LowerLabel
	,[UpperLabel] = @UpperLabel
	,[LowerBound] = @LowerBound
	,[UpperBound] = @UpperBound
	,[Increment] = @Increment
	,[Reverse] = @Reverse
	,[ModifyBy] = @ModifyBy
	,[ModifyDate] = @ModifyDate
	,[IsActive] = @IsActive

WHERE QuestionID = @QuestionID

End

--Delete
Else IF (@Operation = ''D'')

Begin

DELETE from [Question] where [QuestionID]=@QuestionID

End
' 
END
GO
