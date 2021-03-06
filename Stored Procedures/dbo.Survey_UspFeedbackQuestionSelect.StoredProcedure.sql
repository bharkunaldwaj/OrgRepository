USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspFeedbackQuestionSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspFeedbackQuestionSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspFeedbackQuestionSelect]

@QuestionnaireID int,
@SelectFlag char(1)

as

IF (@SelectFlag = 'A') -- All

Begin

select row_number() over (order by t1.catSeq,t1.sequence,t1.QuestionnaireID ) as RowNumber,t1.* from 
(
SELECT TOP (100) PERCENT dbo.Survey_Questionnaire.QuestionnaireID,  dbo.Survey_Question.QuestionTypeID
, dbo.Survey_Category.CategoryID, dbo.Survey_Category.CategoryName,dbo.Survey_Category.CategoryTitle, dbo.Survey_Question.Sequence, dbo.Survey_Question.Validation, 
          dbo.Survey_Question.ValidationText, dbo.Survey_Question.QuestionID, dbo.Survey_Question.Title, dbo.Survey_Question.Description, dbo.Survey_Question.Hint, dbo.Survey_Question.Token, dbo.Survey_Question.TokenText, 
          dbo.Survey_Question.LengthMIN, dbo.Survey_Question.LengthMAX, dbo.Survey_Question.Multiline, dbo.Survey_Question.IsActive, dbo.Survey_Category.Sequence AS CatSeq,dbo.Survey_Question.Range_Name
FROM      dbo.Survey_Questionnaire INNER JOIN
                      dbo.Survey_Question ON dbo.Survey_Questionnaire.QuestionnaireID = dbo.Survey_Question.QuestionnaireID INNER JOIN
                      dbo.Survey_Category ON dbo.Survey_Question.CateogryID = dbo.Survey_Category.CategoryID
WHERE     (dbo.Survey_Question.IsActive = 1) AND (dbo.Survey_Questionnaire.QuestionnaireID =@QuestionnaireID)
order by CatSeq , dbo.Survey_Question.Sequence
) as t1


END

ELSE IF (@SelectFlag = 'N') -- Self Questions

Begin

select row_number() over (order by t1.catSeq,t1.QuestionnaireID ) as RowNumber,t1.* from 
(
SELECT TOP (100) PERCENT dbo.Survey_Questionnaire.QuestionnaireID,  dbo.Survey_Question.QuestionTypeID
, dbo.Survey_Category.CategoryID, dbo.Survey_Category.CategoryName,dbo.Survey_Category.CategoryTitle, dbo.Survey_Question.Sequence, dbo.Survey_Question.Validation, 
          dbo.Survey_Question.ValidationText, dbo.Survey_Question.QuestionID, dbo.Survey_Question.Title, dbo.Survey_Question.Hint, dbo.Survey_Question.Token, dbo.Survey_Question.TokenText, 
          dbo.Survey_Question.LengthMIN, dbo.Survey_Question.LengthMAX, dbo.Survey_Question.Multiline, dbo.Survey_Question.IsActive, dbo.Survey_Category.Sequence AS CatSeq
FROM      dbo.Survey_Questionnaire INNER JOIN
                      dbo.Survey_Question ON dbo.Survey_Questionnaire.QuestionnaireID = dbo.Survey_Question.QuestionnaireID INNER JOIN
                      dbo.Survey_Category ON dbo.Survey_Question.CateogryID = dbo.Survey_Category.CategoryID
WHERE     (dbo.Survey_Question.IsActive = 1) AND (dbo.Survey_Questionnaire.QuestionnaireID =@QuestionnaireID)
order by CatSeq , dbo.Survey_Question.Sequence
) as t1


END

ELSE IF (@SelectFlag = 'C') -- Get Record Count

Begin

SELECT count(t1.QuestionnaireID) from 
(
SELECT TOP (100) PERCENT dbo.Survey_Questionnaire.QuestionnaireID,  dbo.Survey_Question.QuestionTypeID
, dbo.Survey_Category.CategoryName, dbo.Survey_Question.Sequence, dbo.Survey_Question.Validation, 
          dbo.Survey_Question.ValidationText, dbo.Survey_Question.Title, dbo.Survey_Question.Description, dbo.Survey_Question.Hint, dbo.Survey_Question.Token, dbo.Survey_Question.TokenText, 
          dbo.Survey_Question.LengthMIN, dbo.Survey_Question.LengthMAX, dbo.Survey_Question.Multiline, dbo.Survey_Question.IsActive, dbo.Survey_Category.Sequence AS CatSeq
FROM      dbo.Survey_Questionnaire INNER JOIN
                      dbo.Survey_Question ON dbo.Survey_Questionnaire.QuestionnaireID = dbo.Survey_Question.QuestionnaireID INNER JOIN
                      dbo.Survey_Category ON dbo.Survey_Question.CateogryID = dbo.Survey_Category.CategoryID
WHERE     (dbo.Survey_Question.IsActive = 1) AND (dbo.Survey_Questionnaire.QuestionnaireID = @QuestionnaireID)
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
	FROM [Survey_Category]
	WHERE [QuestionnaireID] = @QuestionnaireID
	Order by [Sequence]

End
GO
