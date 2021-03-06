USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspCategoryManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspCategoryManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspCategoryManagement]
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
@ReportCategoryDescription nvarchar(500),  
@QuestionnaireCategoryDescription nvarchar(500),  
@Operation char(1)

as

--Insert
IF (@Operation = 'I')

Begin

INSERT INTO [Category]
           ([CategoryName]
           ,[CategoryTitle] 
           ,[AccountID]
           ,[Description]
           ,[Sequence]
           ,[ExcludeFromAnalysis]
           ,[QuestionnaireID]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[IsActive]
		   ,[ReportCategoryDescription]  
           ,[QuestionnaireCategoryDescription]) 
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
			@ReportCategoryDescription,  
			@QuestionnaireCategoryDescription)

End

--Update
Else IF (@Operation = 'U')

Begin

UPDATE [Category]
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
	,[ReportCategoryDescription] = @ReportCategoryDescription  
	,[QuestionnaireCategoryDescription] = @QuestionnaireCategoryDescription  

WHERE CategoryID=@CategoryID

End

--Delete
Else IF (@Operation = 'D')

Begin

DELETE from [Category] where [CategoryID]=@CategoryID

End
GO
