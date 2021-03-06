USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityQuestionReview]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityQuestionReview]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[PersonalityQuestionReview]
@QuestionnaireID uniqueidentifier
as

BEGIN

--SELECT Q.MainText,C.Text,C.ColorCode,PQ.Name,Q.UniqueID
--  FROM PersonalityQuestionChoices C
--  inner join PersonalityQuestions Q on Q.UniqueID=C.QuestionID
--  inner join PersonalityQuestionnaires PQ on PQ.UniqueID=Q.QuestionnaireID
--  where Q.QuestionnaireID= @QuestionnaireID 
--  order by Q.MainText

SELECT Q.MainText,C.Text,C.ColorCode,
--case	
--	when UPPER(C.ColorCode)='RED' then 1
--	when UPPER(C.ColorCode)='YELLOW' then 2
--	when UPPER(C.ColorCode)='GREEN' then 3
--	when UPPER(C.ColorCode)='BLUE' then 4
--	End AS ColorNumber,

PQ.Name,Q.UniqueID,C.Sequence as ChoicesSeq,Q.Sequence as QuestionsSeq
  FROM PersonalityQuestionChoices C
  inner join PersonalityQuestions Q on Q.UniqueID=C.QuestionID
  inner join PersonalityQuestionnaires PQ on PQ.UniqueID=Q.QuestionnaireID

  where Q.QuestionnaireID= @QuestionnaireID 
  order by Q.UniqueID,C.Sequence,Q.Sequence

--   exec  [dbo].[PersonalityQuestionReview] '94BD7B93-A017-4D5C-9D11-44BB25D56BF3'

END
GO
