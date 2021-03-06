USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityParticipantCountByType]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityParticipantCountByType]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[PersonalityParticipantCountByType] @QuestionannaireID uniqueidentifier
as 

Begin



Select  ParticipantType,Count(ParticipantType)as NumberOfTypes  from (
Select
   (select [Type]  from PersonalityColorOrderType where Color_Order like ColorOrder) as ParticipantType
from 
(Select  QuestionnaireID,ColorCode, ParticipantDetailsID,
(SELECT     SubString(dbo.PersonalityQuestionChoices.ColorCode,1,1)
FROM       dbo.PersonalityQuestionnaires 
			INNER JOIN
                      dbo.PersonalityQuestions ON 
                      dbo.PersonalityQuestionnaires.UniqueID = dbo.PersonalityQuestions.QuestionnaireID 
            INNER JOIN
                      dbo.PersonalityQuestionChoices ON 
                      dbo.PersonalityQuestions.UniqueID = dbo.PersonalityQuestionChoices.QuestionID 
            INNER JOIN
                      dbo.PersonalityParticipantAssignments ON 
                      dbo.PersonalityQuestionnaires.UniqueID = dbo.PersonalityParticipantAssignments.QuestionnaireID 
            INNER JOIN 
					  PersonalityParticiapntDetails on 
					  PersonalityParticiapntDetails.ParticipantAssignmentID = PersonalityParticipantAssignments.UniqueID
            INNER JOIN   
                      dbo.PersonalityQuestionsAnswers ON 
                      dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
            
           where PersonalityQuestionnaires.AccountID=48
           and PersonalityQuestionnaires.UniqueID=@QuestionannaireID	--'f830b9ae-5ba9-4ecd-9a0f-ca53a57f5e06'		--QuestionannaireID
           and PersonalityParticiapntDetails.UniqueID=ParticipantDetailsID --'4E31A810-871A-42E5-A057-012FC957A367'	--ParticipantID
           group by  dbo.PersonalityQuestionChoices.ColorCode
           ORDER BY SUM(ScoreValue) DESC
           FOR XML PATH('') )as ColorOrder
from
(Select distinct pqa.QuestionnaireID,pqc.ColorCode as ColorCode,pqa.ParticiapntDetailsID as ParticipantDetailsID
from PersonalityQuestionsAnswers pqa
inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID) temp1
where QuestionnaireID=@QuestionannaireID	 --'f830b9ae-5ba9-4ecd-9a0f-ca53a57f5e06'
) temp2 group by ParticipantDetailsID,ColorOrder 

 ) temp3 group by ParticipantType
End
GO
