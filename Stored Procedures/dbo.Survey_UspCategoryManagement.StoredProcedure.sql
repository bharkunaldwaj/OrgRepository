USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspCategoryManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspCategoryManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspCategoryManagement]
@CategoryID	int,
@AccountID int,
@CategoryName varchar(50),
@CategoryTitle varchar(250),
@Description varchar(1000),
@Sequence int,
@ExcludeFromAnalysis bit,
@Questionnaire int,
@ModifiedBy int,
@ModifiedDate datetime,
@IsActive int,
@Operation char(1),
@IntroImgFileName varchar(100)=null,
@IntroPdfFileName varchar(100)=null

as

--Insert
IF (@Operation = 'I')

Begin

INSERT INTO [Survey_Category]
           ([CategoryName]
           ,[CategoryTitle] 
           ,[AccountID]
           ,[Description]
           ,[Sequence]
           ,[ExcludeFromAnalysis]
           ,[QuestionnaireID]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[IsActive],
           IntroImgFileName,
           IntroPdfFileName)
     VALUES
           (@CategoryName,
            @CategoryTitle, 
            @AccountID,
			@Description,
			@Sequence,
			@ExcludeFromAnalysis,
			@Questionnaire,
			@ModifiedBy,
			@ModifiedDate,
			@IsActive,
			@IntroImgFileName,
			@IntroPdfFileName)

End

--Update
Else IF (@Operation = 'U')

Begin

UPDATE [Survey_Category]
SET 
	[CategoryName] = @CategoryName
	,[CategoryTitle] = @CategoryTitle
	,[AccountID] = @AccountID
	,[Description] = @Description
	,[Sequence] = @Sequence
	,[ExcludeFromAnalysis] = @ExcludeFromAnalysis
	,[QuestionnaireID] = @Questionnaire
	,[ModifiedBy] = @ModifiedBy
	,[ModifiedDate] = @ModifiedDate
	,[IsActive] = @IsActive
	,IntroImgFileName=@IntroImgFileName
	,IntroPdfFileName=@IntroPdfFileName

WHERE CategoryID=@CategoryID

End

--Delete
Else IF (@Operation = 'D')

Begin

DELETE from [Survey_Category] where [CategoryID]=@CategoryID

End
GO
