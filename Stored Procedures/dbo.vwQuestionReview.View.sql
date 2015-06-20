USE [Feedback360_Dev2]
GO
/****** Object:  View [dbo].[vwQuestionReview]    Script Date: 06/19/2015 13:26:27 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwQuestionReview]'))
DROP VIEW [dbo].[vwQuestionReview]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwQuestionReview]'))
EXEC dbo.sp_executesql @statement = N'
create view [dbo].[vwQuestionReview] 
as

SELECT Q.MainText,C.Text,C.ColorCode,
case	
	when UPPER(C.ColorCode)=''RED'' then 1
	when UPPER(C.ColorCode)=''YELLOW'' then 2
	when UPPER(C.ColorCode)=''GREEN'' then 3
	when UPPER(C.ColorCode)=''BLUE'' then 4
	End AS ColorNumber

,PQ.Name,Q.UniqueID,PQ.UniqueID as QuestionnareID
  FROM PersonalityQuestionChoices C
  inner join PersonalityQuestions Q on Q.UniqueID=C.QuestionID
  inner join PersonalityQuestionnaires PQ on PQ.UniqueID=Q.QuestionnaireID

  --where Q.QuestionnaireID= ''94BD7B93-A017-4D5C-9D11-44BB25D56BF3'' 
--  order by Q.UniqueID,ColorNumber
'
GO
