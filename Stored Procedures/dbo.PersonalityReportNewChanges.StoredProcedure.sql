USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityReportNewChanges]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityReportNewChanges]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[PersonalityReportNewChanges] 

@Action varchar(2),
@AccountID varchar(50) = null,
@ParticipantID varchar(50)=null,
@ReportManagement varchar(50) = null

as

If(@Action = 'E')
Begin

select ShowFreeTextResponse,Color1,Color2,Color3,Color4,WheelGraph as ShowWheelGraphExplanation 
from PersonalityReportManagement
where UniqueID = @ReportManagement

End

If(@Action = 'D')
Begin

select PersonalityQuestions.MainText,PersonalityQuestionsAnswers.FreeTextAnswer
from PersonalityQuestions 
inner join PersonalityQuestionsAnswers on
PersonalityQuestionsAnswers.QuestionID = PersonalityQuestions.UniqueID
where (PersonalityQuestionsAnswers.FreeTextAnswer <> '' or PersonalityQuestionsAnswers.FreeTextAnswer is not null)
and PersonalityQuestions.AccountID = @AccountID
and PersonalityQuestionsAnswers.ParticiapntDetailsID = @ParticipantID
and PersonalityQuestions.QuestionType = 'T'
Order By PersonalityQuestions.Sequence 

End
GO
