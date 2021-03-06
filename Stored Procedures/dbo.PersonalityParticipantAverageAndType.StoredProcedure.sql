USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityParticipantAverageAndType]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityParticipantAverageAndType]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec [PersonalityParticipantAverageAndType]'f830b9ae-5ba9-4ecd-9a0f-ca53a57f5e06',null,null,null

CREATE proc [dbo].[PersonalityParticipantAverageAndType]
@QuestionannaireID uniqueidentifier,
@FromDate varchar(20)=null,
@ToDate varchar(20)=null,
@Company varchar(max)=null
as
Begin

if Upper(@Company)='ALL COMPANY'
begin
set @Company=null
end

 --create table #TempRed (Average int,ColorCode varchar(10),ParticipantDetailsID uniqueidentifier,Questionnaire varchar(max),AccountCode varchar(100)) 
 --  For Red Color
 Select avg(ScoreValue) as Average,ColorCode,ParticipantDetailsID,'1' as sequence,
 					(Select avg(ScoreValue) as Average from 
				    (Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
						,FinishedDate from
						(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
						pqc.Text  as Text,pqc.ColorCode as ColorCode,
						'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
						, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
						from PersonalityQuestionsAnswers pqa
						inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
						inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
						inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
						inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
						inner join Account ACC on ACC.AccountID=PQ.AccountID
						inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
						where (PQD.Company=@Company or @Company is null )
						) temp3
						where QuestionnaireID=@QuestionannaireID and ColorCode='Red'
						and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
					    and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
						) temp4 ) as AverageSum,
				 
 
		Questionnaire,AccountCode into #TempRed from 
(Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
,FinishedDate from
(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
pqc.Text  as Text,pqc.ColorCode as ColorCode,
'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
from PersonalityQuestionsAnswers pqa
inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
inner join Account ACC on ACC.AccountID=PQ.AccountID
inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
where (PQD.Company=@Company or @Company is null )
) temp1
where QuestionnaireID=@QuestionannaireID and ColorCode='Red'
and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
) temp2 group by ParticipantDetailsID,ColorCode,Questionnaire,AccountCode


 --  For Yellow Color
 Select avg(ScoreValue) as Average,ColorCode,ParticipantDetailsID,'2' as sequence,
 					(Select avg(ScoreValue) as Average from 
				    (Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
						,FinishedDate from
						(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
						pqc.Text  as Text,pqc.ColorCode as ColorCode,
						'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
						, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
						from PersonalityQuestionsAnswers pqa
						inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
						inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
						inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
						inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
						inner join Account ACC on ACC.AccountID=PQ.AccountID
						inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
						where (PQD.Company=@Company or @Company is null )
						) temp3
						where QuestionnaireID=@QuestionannaireID and ColorCode='Yellow'
						and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
					    and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
						) temp4 ) as AverageSum,
				 
 
		Questionnaire,AccountCode into #TempYellow from 
(Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
,FinishedDate from
(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
pqc.Text  as Text,pqc.ColorCode as ColorCode,
'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
from PersonalityQuestionsAnswers pqa
inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
inner join Account ACC on ACC.AccountID=PQ.AccountID
inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
where (PQD.Company=@Company or @Company is null )
) temp1
where QuestionnaireID=@QuestionannaireID and ColorCode='Yellow'
and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
) temp2 group by ParticipantDetailsID,ColorCode,Questionnaire,AccountCode



 --  For Green Color
 Select avg(ScoreValue) as Average,ColorCode,ParticipantDetailsID,'3' as sequence,
 					(Select avg(ScoreValue) as Average from 
				    (Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
						,FinishedDate from
						(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
						pqc.Text  as Text,pqc.ColorCode as ColorCode,
						'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
						, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
						from PersonalityQuestionsAnswers pqa
						inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
						inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
						inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
						inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
						inner join Account ACC on ACC.AccountID=PQ.AccountID
						inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
						where (PQD.Company=@Company or @Company is null )
						) temp3
						where QuestionnaireID=@QuestionannaireID and ColorCode='Green'
						and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
						and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
						) temp4 ) as AverageSum,
				 
 
		Questionnaire,AccountCode into #TempGreen from 
(Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
,FinishedDate from
(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
pqc.Text  as Text,pqc.ColorCode as ColorCode,
'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
from PersonalityQuestionsAnswers pqa
inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
inner join Account ACC on ACC.AccountID=PQ.AccountID
inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
where (PQD.Company=@Company or @Company is null )
) temp1
where QuestionnaireID=@QuestionannaireID and ColorCode='Green'
and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )						
) temp2 group by ParticipantDetailsID,ColorCode,Questionnaire,AccountCode



 --  For Blue Color
 Select avg(ScoreValue) as Average,ColorCode,ParticipantDetailsID,'4' as sequence,
 					(Select avg(ScoreValue) as Average from 
				    (Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
						,FinishedDate from
						(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
						pqc.Text  as Text,pqc.ColorCode as ColorCode,
						'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
						, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
						from PersonalityQuestionsAnswers pqa
						inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
						inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
						inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
						inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
						inner join Account ACC on ACC.AccountID=PQ.AccountID
						inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
						where (PQD.Company=@Company or @Company is null )
						) temp3
						where QuestionnaireID=@QuestionannaireID and ColorCode='Blue'
						and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
                        and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
						) temp4 ) as AverageSum,
				 
 
		Questionnaire,AccountCode into #TempBlue from 
(Select  QuestionnaireID,QuestionID, ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID,Questionnaire,AccountCode
,FinishedDate from
(Select distinct pqa.QuestionnaireID,pqa.QuestionID,(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) as ScoreValue,pqa.QuestionChoiceID,
pqc.Text  as Text,pqc.ColorCode as ColorCode,
'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
from PersonalityQuestionsAnswers pqa
inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID 
inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
inner join Account ACC on ACC.AccountID=PQ.AccountID
inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
where (PQD.Company=@Company or @Company is null )
) temp1
where QuestionnaireID=@QuestionannaireID and ColorCode='Blue'
and (CONVERT(datetime,FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
and (CONVERT(datetime,FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
) temp2 group by ParticipantDetailsID,ColorCode,Questionnaire,AccountCode

select avg(Average) as Average,ColorCode,sequence from (
select * from #TempRed
union
select * from #TempGreen
union
select * from #TempYellow
union
select * from #TempBlue

) as tt group by ColorCode,sequence
order by sequence

drop table #TempRed
drop table #TempGreen
drop table #TempYellow
drop table #TempBlue
			
End
GO
