USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityParticipantAverageScoreChart]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityParticipantAverageScoreChart]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--PersonalityRightGraph '4E31A810-871A-42E5-A057-012FC957A367'
CREATE proc [dbo].[PersonalityParticipantAverageScoreChart] ---'4E31A810-871A-42E5-A057-012FC957A367'
@ParticipantID varchar(50)=null,
@ReportManagement varchar(50) = null

as

--select ROUND(AVG(Convert(float,CASE WHEN PersonalityQuestionnaires.[Type]=2 THEN  PersonalityQuestionsAnswers.ScoreValue*10 WHEN PersonalityQuestionnaires.[Type] = 1 
--THEN PersonalityQuestionsAnswers.ScoreValue END )),0) as ScoreValue ,PersonalityQuestionChoices.ColorCode,
--	(case when ColorCode ='Red' then 1 
--		  when ColorCode ='Yellow' then 2
--		  when ColorCode ='Green' then 3
--		  when ColorCode ='Blue' then 4 end ) as Sequence ,PersonalityQuestionnaires.[Type]
--	from PersonalityQuestionsAnswers
--	inner join PersonalityQuestionChoices ON 
--	dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
--	inner join PersonalityQuestionnaires ON
--	PersonalityQuestionnaires.UniqueID=PersonalityQuestionsAnswers.QuestionnaireID
--	where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID 
--	group by PersonalityQuestionChoices.ColorCode   ,PersonalityQuestionnaires.[Type]
--	order by Sequence


create table #tempcolor (seq int, ColorCode varchar(50))
create table #tempold (ScoreValue int, color varchar(10), Sequence int, [Type] int)
insert into #tempcolor (seq,ColorCode) values (1, (select Color1 from PersonalityReportManagement where UniqueID = @ReportManagement))
insert into #tempcolor (seq,ColorCode) values (2, (select Color2 from PersonalityReportManagement where UniqueID = @ReportManagement))
insert into #tempcolor (seq,ColorCode) values (3, (select Color3 from PersonalityReportManagement where UniqueID = @ReportManagement))
insert into #tempcolor (seq,ColorCode) values (4, (select Color4 from PersonalityReportManagement where UniqueID = @ReportManagement))

insert into #tempold
select ROUND(AVG(Convert(float,CASE WHEN PersonalityQuestionnaires.[Type]=2 THEN  
PersonalityQuestionsAnswers.ScoreValue*10 WHEN PersonalityQuestionnaires.[Type] = 1 
THEN PersonalityQuestionsAnswers.ScoreValue END )),0) as ScoreValue ,PersonalityQuestionChoices.ColorCode as color,
	(case when ColorCode ='Red' then 1 
		  when ColorCode ='Yellow' then 2
		  when ColorCode ='Green' then 3
		  when ColorCode ='Blue' then 4 end ) as Sequence ,PersonalityQuestionnaires.[Type]
	from PersonalityQuestionsAnswers
	inner join PersonalityQuestionChoices ON 
	dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
	inner join PersonalityQuestionnaires ON
	PersonalityQuestionnaires.UniqueID=PersonalityQuestionsAnswers.QuestionnaireID
	where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID 
	group by PersonalityQuestionChoices.ColorCode   ,PersonalityQuestionnaires.[Type]
	order by Sequence

select #tempold.ScoreValue,#tempcolor.ColorCode, #tempold.Sequence,#tempold.Type 
from #tempold
inner join #tempcolor on 
#tempcolor.seq = #tempold.Sequence

Drop table #tempcolor
Drop table #tempold
GO
