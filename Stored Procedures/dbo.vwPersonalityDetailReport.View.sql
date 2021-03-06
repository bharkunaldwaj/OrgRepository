USE [Feedback360_Dev2]
GO
/****** Object:  View [dbo].[vwPersonalityDetailReport]    Script Date: 06/23/2015 10:42:49 ******/
DROP VIEW [dbo].[vwPersonalityDetailReport]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[vwPersonalityDetailReport](QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode)
as
Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode from
(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode
 from PersonalityQuestionsAnswers pqa ) temp
GO
