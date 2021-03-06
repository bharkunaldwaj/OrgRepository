USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionsManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspQuestionsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspQuestionsManagement]
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
@Description varchar(1500),

@Hint varchar(1000),
@Token int,
@TokenText varchar(50),
@LengthMIN int,
@LengthMAX int,
@Multiline bit,



@ModifyBy int,
@ModifyDate datetime,
@IsActive int,
@Range_Name varchar(200),
@Operation char(1)

as

--Insert
IF (@Operation = 'I')

Begin

INSERT INTO [Survey_Question]
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

           ,[Hint]
           ,[Token]
           ,[TokenText]
           ,[LengthMIN]
           ,[LengthMAX]
           ,[Multiline]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive]
           ,[Range_Name]
           )
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

            @Hint,
            @Token,
            @TokenText,
            @LengthMIN,
            @LengthMAX,
            @Multiline,
            @ModifyBy,
            @ModifyDate,
            @IsActive,
            @Range_Name
            )

End

--Update
Else IF (@Operation = 'U')

Begin

UPDATE [Survey_Question]
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

	,[Hint] = @Hint
	,[Token] = @Token
	,[TokenText] = @TokenText
	,[LengthMIN] = @LengthMIN
	,[LengthMAX] = @LengthMAX
	,[Multiline] = @Multiline
	
	
	
	,[ModifyBy] = @ModifyBy
	,[ModifyDate] = @ModifyDate
	,[IsActive] = @IsActive
	,[Range_Name]=@Range_Name

WHERE QuestionID = @QuestionID

End

--Delete
Else IF (@Operation = 'D')

Begin

DELETE from [Survey_Question] where [QuestionID]=@QuestionID

End
GO
