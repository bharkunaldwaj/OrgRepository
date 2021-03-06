USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspQuestionsSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspQuestionsSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspQuestionsSelect]

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
			   ,[IsActive]   

		   FROM [Question]
		  WHERE [QuestionID] = @QuestionID 

		 End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT      [QuestionID]
			   ,Question.AccountID AS AccountID
			   ,Account.Code as Code
			   ,Questionnaire.QSTNName as Questionnaire
			   ,MSTQuestionType.Name as Name
			   ,Category.CategoryName as CategoryName
			   ,Question.Sequence  as Sequence
			   ,[Validation]
			   ,[ValidationText]
			   ,[Title]
			   ,Question.Description as Descriptions
			   ,Question.DescriptionSelf as DescriptionSelf
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
			   ,Question.ModifyBy
			   ,Question.ModifyDate
			   ,Question.IsActive   

		 FROM   [Question] Inner Join [Account] on dbo.Question.CompanyID = Account.AccountID
				Inner Join Questionnaire on Question.QuestionnaireID = Questionnaire.QuestionnaireID
				Inner Join Category  on Question.CateogryID = Category.CategoryID
				Inner Join MSTQuestionType on Question.QuestionTypeID = MSTQuestionType.QuestionTypeID
		Where	Account.[AccountID]=@AccountID
		order by [Code],Sequence, Question.ModifyDate, Title 
	   
	   END

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Question] where [AccountID]=@AccountID

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
			   ,[IsActive]   

		   FROM [Question]
		  WHERE [QuestionID] = @QuestionID

		 End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT      [QuestionID]
			   ,Question.AccountID AS AccountID
			   ,Account.Code as Code
			   ,Questionnaire.QSTNName as Questionnaire
			   ,MSTQuestionType.Name as Name
			   ,Category.CategoryName as CategoryName
			   ,Question.Sequence  as Sequence
			   ,[Validation]
			   ,[ValidationText]
			   ,[Title]
			   ,Question.Description as Descriptions
			   ,Question.DescriptionSelf as DescriptionSelf
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
			   ,Question.ModifyBy
			   ,Question.ModifyDate
			   ,Question.IsActive   

		 FROM   [Question] Inner Join [Account] on dbo.Question.CompanyID = Account.AccountID
				Inner Join Questionnaire on Question.QuestionnaireID = Questionnaire.QuestionnaireID
				Inner Join Category  on Question.CateogryID = Category.CategoryID
				Inner Join MSTQuestionType on Question.QuestionTypeID = MSTQuestionType.QuestionTypeID
				order by [Code],Sequence, Question.ModifyDate, Title 
	   
	   END

	ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Question] 

	End


	END
GO
