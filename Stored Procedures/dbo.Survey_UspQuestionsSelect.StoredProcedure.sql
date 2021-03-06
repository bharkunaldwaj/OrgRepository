USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionsSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspQuestionsSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspQuestionsSelect]

@QuestionID int,
@AccountID int,
@SelectFlag char(1)

as

IF (@AccountID != 2 )

	BEGIN

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT      [QuestionID]
			   ,[AccountID]
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

		   FROM [Survey_Question]
		  WHERE [QuestionID] = @QuestionID 

		 End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT      [QuestionID]
			   ,Survey_Question.AccountID AS AccountID
			   ,Account.Code as Code
			   ,Survey_Questionnaire.QSTNName as Questionnaire
			   ,Survey_MSTQuestionType.Name as Name
			   ,Survey_Category.CategoryName as CategoryName
			   ,Survey_Question.Sequence  as Sequence
			   ,[Validation]
			   ,[ValidationText]
			   ,[Title]
			   ,Survey_Question.Description as Descriptions
			  
			   ,[Hint]
			   ,[Token]
			   ,[TokenText]
			   ,[LengthMIN]
			   ,[LengthMAX]
			   ,[Multiline]
			   
			   ,Survey_Question.ModifyBy
			   ,Survey_Question.ModifyDate
			   ,Survey_Question.IsActive   
			   ,[Range_Name]

		 FROM   [Survey_Question] Inner Join [Account] on dbo.Survey_Question.CompanyID = Account.AccountID
				Inner Join Survey_Questionnaire on Survey_Question.QuestionnaireID = Survey_Questionnaire.QuestionnaireID
				Inner Join Survey_Category  on Survey_Question.CateogryID = Survey_Category.CategoryID
				Inner Join Survey_MSTQuestionType on Survey_Question.QuestionTypeID = Survey_MSTQuestionType.QuestionTypeID
		Where	Account.[AccountID]=@AccountID
		order by [Code],Sequence, Survey_Question.ModifyDate, Title 
	   
	   END

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Question] where [AccountID]=@AccountID

	End


	END

ELSE

	BEGIN

	IF (@SelectFlag = 'I') -- Id based

	Begin

	SELECT      [QuestionID]
			   ,[AccountID]
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

		   FROM [Survey_Question]
		  WHERE [QuestionID] = @QuestionID

		 End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT      [QuestionID]
			   ,Survey_Question.AccountID AS AccountID
			   ,Account.Code as Code
			   ,Survey_Questionnaire.QSTNName as Questionnaire
			   ,Survey_MSTQuestionType.Name as Name
			   ,Survey_Category.CategoryName as CategoryName
			   ,Survey_Question.Sequence  as Sequence
			   ,[Validation]
			   ,[ValidationText]
			   ,[Title]
			   ,Survey_Question.Description as Descriptions
			 
			   ,[Hint]
			   ,[Token]
			   ,[TokenText]
			   ,[LengthMIN]
			   ,[LengthMAX]
			   ,[Multiline]
			   
			   ,Survey_Question.ModifyBy
			   ,Survey_Question.ModifyDate
			   ,Survey_Question.IsActive   
			   ,[Range_Name]

		 FROM   [Survey_Question] Inner Join [Account] on dbo.Survey_Question.CompanyID = Account.AccountID
				Inner Join Survey_Questionnaire on Survey_Question.QuestionnaireID = Survey_Questionnaire.QuestionnaireID
				Inner Join 
Survey_Category  on Survey_Question.CateogryID = 
Survey_Category.CategoryID
				Inner Join 
Survey_MSTQuestionType on Survey_Question.QuestionTypeID = 
Survey_MSTQuestionType.QuestionTypeID
				order by [Code],Sequence, Survey_Question.ModifyDate, Title 
	   
	   END

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Question] 

	End


	END
GO
