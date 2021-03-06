USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspFeedbackQuestionSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspFeedbackQuestionSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspFeedbackQuestionSelect]

@QuestionnaireID int,
@SelectFlag char(1),
@ProjectID int = null,
@AccountID int = null,
@RelationShip Varchar(100) = null

as

DECLARE @Count AS INT
SELECT @Count = COUNT(*) FROM AssignedCategories 
								WHERE (AccountID = @AccountID) 
								AND (ProjectID = @ProjectID )
								AND (QuestionnaireID = @QuestionnaireID )
								AND (RelationshipName = @RelationShip )

IF (@SelectFlag = 'A') -- All

Begin

select row_number() over (order by t1.catSeq,t1.QuestionnaireID ) as RowNumber,t1.* from 
(
SELECT TOP (100) PERCENT dbo.Questionnaire.QuestionnaireID,  dbo.Question.QuestionTypeID
, dbo.Category.CategoryID, dbo.Category.CategoryName,dbo.Category.CategoryTitle, dbo.Question.Sequence, dbo.Question.Validation, 
          dbo.Question.ValidationText, dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.Description, dbo.Question.Hint, dbo.Question.Token, dbo.Question.TokenText, 
          dbo.Question.LengthMIN, dbo.Question.LengthMAX, dbo.Question.Multiline, dbo.Question.LowerLabel, dbo.Question.UpperLabel, 
          dbo.Question.LowerBound, dbo.Question.UpperBound, dbo.Question.Increment,
           dbo.Question.IsActive, 
           dbo.Category.Sequence AS CatSeq,
           Category.QuestionnaireCategoryDescription 
FROM      dbo.Questionnaire INNER JOIN
                      dbo.Question ON dbo.Questionnaire.QuestionnaireID = dbo.Question.QuestionnaireID INNER JOIN
                      dbo.Category ON dbo.Question.CateogryID = dbo.Category.CategoryID
WHERE     (dbo.Question.IsActive = 1) AND (dbo.Questionnaire.QuestionnaireID =@QuestionnaireID)
AND 

				(
				@Count = 0
				OR 
					Question.CateogryID IN
					(

								SELECT CategoryID FROM AssignedCategories 
								WHERE (AccountID = @AccountID) 
								AND (ProjectID = @ProjectID )
								AND (QuestionnaireID = @QuestionnaireID )
								AND (RelationshipName = @RelationShip )
								
						
					)
				
				)
order by CatSeq , dbo.Question.Sequence
) as t1


END

ELSE IF (@SelectFlag = 'N') -- Self Questions

Begin

select row_number() over (order by t1.catSeq,t1.QuestionnaireID ) as RowNumber,t1.* from 
(
SELECT TOP (100) PERCENT dbo.Questionnaire.QuestionnaireID,  dbo.Question.QuestionTypeID
, dbo.Category.CategoryID, dbo.Category.CategoryName,dbo.Category.CategoryTitle, dbo.Question.Sequence, dbo.Question.Validation, 
          dbo.Question.ValidationText, dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.DescriptionSelf as [Description], dbo.Question.Hint, dbo.Question.Token, dbo.Question.TokenText, 
          dbo.Question.LengthMIN, dbo.Question.LengthMAX, dbo.Question.Multiline, dbo.Question.LowerLabel, dbo.Question.UpperLabel, 
          dbo.Question.LowerBound, dbo.Question.UpperBound,
           dbo.Question.Increment, 
           dbo.Question.IsActive,
            dbo.Category.Sequence AS CatSeq,
            Category.QuestionnaireCategoryDescription
FROM      dbo.Questionnaire INNER JOIN
                      dbo.Question ON dbo.Questionnaire.QuestionnaireID = dbo.Question.QuestionnaireID INNER JOIN
                      dbo.Category ON dbo.Question.CateogryID = dbo.Category.CategoryID
WHERE     (dbo.Question.IsActive = 1) AND (dbo.Questionnaire.QuestionnaireID =@QuestionnaireID)
order by CatSeq , dbo.Question.Sequence
) as t1


END

ELSE IF (@SelectFlag = 'C') -- Get Record Count

Begin

SELECT count(t1.QuestionnaireID) from 
(
SELECT TOP (100) PERCENT dbo.Questionnaire.QuestionnaireID,  dbo.Question.QuestionTypeID
, dbo.Category.CategoryName, dbo.Question.Sequence, dbo.Question.Validation, 
          dbo.Question.ValidationText, dbo.Question.Title, dbo.Question.Description, dbo.Question.Hint, dbo.Question.Token, dbo.Question.TokenText, 
          dbo.Question.LengthMIN, dbo.Question.LengthMAX, dbo.Question.Multiline, dbo.Question.LowerLabel, dbo.Question.UpperLabel, 
          dbo.Question.LowerBound, dbo.Question.UpperBound, dbo.Question.Increment, dbo.Question.IsActive, dbo.Category.Sequence AS CatSeq
FROM      dbo.Questionnaire INNER JOIN
                      dbo.Question ON dbo.Questionnaire.QuestionnaireID = dbo.Question.QuestionnaireID INNER JOIN
                      dbo.Category ON dbo.Question.CateogryID = dbo.Category.CategoryID
WHERE     (dbo.Question.IsActive = 1) AND (dbo.Questionnaire.QuestionnaireID = @QuestionnaireID)
) as t1

End

ELSE IF (@SelectFlag = 'S') -- Get Category Details of a questionnaire

Begin

	SELECT row_number() over (order by [Sequence],[CategoryID] ) as RowNumber
			,[CategoryID]
		  ,[AccountID]
		  ,[CategoryName]
		  ,[CategoryTitle]
		  ,[Description]
		  ,[Sequence]
		  ,[ExcludeFromAnalysis]
		  ,[QuestionnaireID]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[IsActive]
		  ,QuestionnaireCategoryDescription
	FROM [Category]
	WHERE [QuestionnaireID] = @QuestionnaireID
	AND (
	(
	@Count = 0
				OR  	
	CategoryID IN
		(	
			SELECT CategoryID FROM AssignedCategories 
			WHERE (AccountID = @AccountID) 
			AND (ProjectID = @ProjectID )
			AND (QuestionnaireID = @QuestionnaireID )
			AND (RelationshipName = @RelationShip )
			
		)
	 )
	)
	Order by [Sequence]

End
GO
