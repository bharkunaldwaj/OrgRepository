USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetScoreDetailsIndividual]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetScoreDetailsIndividual]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Select * from Personality_UAT_Server.dbo.PersonalityParticiapntDetails p WHERE 
--p.ParticipantAssignmentID='0FC6DFF8-2593-4502-82F1-70DE56834F60'
--and p.IsFinished=1
--Select * from Personality_UAT_Server.dbo.PersonalityParticipantAssignments WHERE QuestionnaireID='F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06'

--  [dbo].PersonalityGetScoreDetailsIndividual  'ccaf9404-cde5-4fec-9b21-00bf7af1977d',0,0,0,0,'Above','Above','Above','Above',null,null

CREATE procedure [dbo].[PersonalityGetScoreDetailsIndividual]  
	@QuestionnaireID uniqueidentifier,
	@RedScore int,
	@YellowScore int,
	@GreenScore int,
	@BlueScore int,
	@RedCondition Varchar(10),
	@YellowCondition Varchar(10),
	@GreenCondition Varchar(10),
	@BlueCondition Varchar(10),
	@FromDate Varchar(50)=null,
	@ToDate varchar(50)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;


declare @SetRedCondition varchar(10)
declare @SetYelloCondition varchar(10)
declare @SetGreenCondition varchar(10)
declare @SetBlueCondition varchar(10)

declare @QuestionnaireName varchar(10)
declare @AccountCode varchar(10)


--Create temp table for insert records indiviually
create table #PersonalityAnserDetailReport_TEMP
(
	[QuestionnaireID] [uniqueidentifier] NULL,
	[QuestionID] [uniqueidentifier] NULL,
	[ScoreValue] [int] NULL,
	[QuestionChoiceID] [uniqueidentifier] NULL,
	[Text] [varchar](max) NULL,
	[ColorCode] [varchar](10) NULL,
	[Sequence] [varchar](50) NULL,
	[ParticipantDetailsID] [uniqueidentifier] NULL,
	FinishedDate datetime
)

create table #PersonalityAnserDetailReport_TEMP1
(
[ParticipantDetailsID] [uniqueidentifier] NULL,
[ColorCode] [varchar](10) NULL,
[ScoreValue] [int] NULL
)
create table #PersonalityAnserDetailReport_TEMP2
(
[ParticipantDetailsID] [uniqueidentifier] NULL,
[ColorCode] [varchar](10) NULL,
[ScoreValue] [int] NULL
)
create table #PersonalityAnserDetailReport_TEMP3
(
[ParticipantDetailsID] [uniqueidentifier] NULL,
[ColorCode] [varchar](10) NULL,
[ScoreValue] [int] NULL
)
create table #PersonalityAnserDetailReport_TEMP4
(
[ParticipantDetailsID] [uniqueidentifier] NULL,
[ColorCode] [varchar](10) NULL,
[ScoreValue] [int] NULL
)

--drop PersonalityAnserDetailReport_TEMP
IF(UPPER(@RedCondition) = 'ABOVE')
	BEGIN
		 --================================
	insert into #PersonalityAnserDetailReport_TEMP1
		 Select distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select distinct pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	
	where ColorCode='Red' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ColorCode, ParticipantDetailsID
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END )  > @RedScore 
		 --================================
	END
	ELSE
	IF UPPER(@RedCondition) = 'BELOW'
		BEGIN 
		 --================================
	insert into #PersonalityAnserDetailReport_TEMP1
		Select  distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	where ColorCode='Red' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ColorCode, ParticipantDetailsID
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END )  < @RedScore 
		 --================================
	END
	
print 'Hello'
	
IF UPPER(@YellowCondition) = 'ABOVE'
	BEGIN
			 --================================
	insert into #PersonalityAnserDetailReport_TEMP2
		 Select  distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	where  ColorCode='Yellow' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ColorCode, ParticipantDetailsID
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) > @YellowScore
			 --================================
	END
	ELSE IF UPPER(@YellowCondition) = 'BELOW'
	BEGIN
	
		 --================================
	insert into #PersonalityAnserDetailReport_TEMP2
		Select  distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	where  ColorCode='Yellow' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ParticipantDetailsID, ColorCode
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) < @YellowScore
		 --================================
	END


	
IF UPPER(@GreenCondition) = 'ABOVE'
	BEGIN 
				 --================================
	insert into #PersonalityAnserDetailReport_TEMP3
		 Select distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	where ColorCode='Green' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ParticipantDetailsID, ColorCode
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) > @GreenScore 
			 --================================
	END
	ELSE 
	IF UPPER(@GreenCondition) = 'BELOW'
	BEGIN
		  --================================
	insert into #PersonalityAnserDetailReport_TEMP3
		 Select distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	where ColorCode='Green' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ParticipantDetailsID, ColorCode
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END )< @GreenScore
		 --================================
	END


IF UPPER(@BlueCondition) = 'ABOVE'
	BEGIN
					 --================================
	insert into #PersonalityAnserDetailReport_TEMP4
		 Select distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	where ColorCode='Blue' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ParticipantDetailsID, ColorCode
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END )> @BlueScore
			 --================================
	END
	ELSE IF UPPER(@BlueCondition) = 'BELOW'
	BEGIN
		  --================================
	insert into #PersonalityAnserDetailReport_TEMP4
		 Select  distinct ParticipantDetailsID, ColorCode, avg(ScoreValue) from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
	pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,PQD.UniqueID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,PQD.FinishedDate as FinishedDate,PQQ.Type
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	where ColorCode='Blue' and pq.QuestionnaireID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
	) temp
		group by ParticipantDetailsID, ColorCode
		having avg(CASE WHEN [Type]=2 THEN  ScoreValue*10 WHEN [Type] = 1 THEN ScoreValue END ) < @BlueScore
		 --================================
	END

insert into #PersonalityAnserDetailReport_TEMP ([ParticipantDetailsID])
	SELECT ParticipantDetailsID FROM #PersonalityAnserDetailReport_TEMP1
	intersect
	SELECT ParticipantDetailsID FROM #PersonalityAnserDetailReport_TEMP2
	intersect
	SELECT ParticipantDetailsID FROM #PersonalityAnserDetailReport_TEMP3
	intersect
	SELECT ParticipantDetailsID FROM #PersonalityAnserDetailReport_TEMP4



Select distinct  ScoreValue as ScoreValue ,QuestionChoiceID,ColorCode, 
case	
	when UPPER(ColorCode)='RED' then 1
	when UPPER(ColorCode)='YELLOW' then 2
	when UPPER(ColorCode)='GREEN' then 3
	when UPPER(ColorCode)='BLUE' then 4
	End AS ColorNumber,
	Sequence,OrderBy,(Select COUNT(*) from #PersonalityAnserDetailReport_TEMP) as CountOfParticipant,ParticiapntDetailsID,CandidateName,ISNULL(Country,'') as Country, PrizeDrawOption from
	(Select pqa.QuestionnaireID,pqa.QuestionID
	,(CASE WHEN PQQ.[Type]=2 THEN  pqa.ScoreValue*10 WHEN PQQ.[Type] = 1 THEN pqa.ScoreValue END ) as ScoreValue
	,pqa.QuestionChoiceID,pqc.Text  as Text,pqc.ColorCode as ColorCode,
	'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,PQ.Sequence as OrderBy ,pqa.ParticiapntDetailsID as ParticipantDetailsID
	, PQQ.Name as Questionnaire, ACC.Code as AccountCode,pqa.ParticiapntDetailsID,PQD.FirstName + ' ' + ISNULL(PQD.LastName,'') as CandidateName,PC.Name Country,CASE  WHEN  PQD.PrizeDraw = 1 Then 'Yes' Else 'No' End PrizeDrawOption
	from PersonalityQuestionsAnswers pqa
	inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID
	inner join PersonalityParticipantAssignments PQAA on pqaa.QuestionnaireID=  pqa.QuestionnaireID
	inner join PersonalityParticiapntDetails PQD on PQD.ParticipantAssignmentID=PQAA.UniqueID and PQD.UniqueID=pqa.ParticiapntDetailsID
	inner join PersonalityQuestionnaires PQQ on PQQ.UniqueID=pqa.QuestionnaireID
	inner join Account ACC on ACC.AccountID=PQ.AccountID
	inner join PersonalityQuestionChoices pqc on pqc.UniqueID=pqa.QuestionChoiceID
	LEFT OUTER JOIN PersonalityCountries PC ON Pc.UniqueID = PQD.CountryID
	
	WHERE pqa.ParticiapntDetailsID IN (SELECT ParticipantDetailsID FROM #PersonalityAnserDetailReport_TEMP)    
	and PQQ.UniqueID=@QuestionnaireID
	and IsFinished=1
	and (CONVERT(datetime,PQD.FinishedDate) >= CONVERT(datetime,@FromDate) or @FromDate is null )
	and (CONVERT(datetime,PQD.FinishedDate) <= CONVERT(datetime,@ToDate) or @ToDate is null )
) temp
--Group BY QuestionChoiceID,Colorcode,Sequence,OrderBy
order by ParticiapntDetailsID

	 
	

--Drop temp table here
DROP TABLE [dbo].[#PersonalityAnserDetailReport_TEMP]
DROP TABLE [dbo].[#PersonalityAnserDetailReport_TEMP1]
DROP TABLE [dbo].[#PersonalityAnserDetailReport_TEMP2]
DROP TABLE [dbo].[#PersonalityAnserDetailReport_TEMP3]
DROP TABLE [dbo].[#PersonalityAnserDetailReport_TEMP4]




END


/*
  exec PersonalityGetScoreDetails 'F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06',25,0,20,0,'ABOVE','ABOVE','BELOW','ABOVE',NULL,NULL
'2012-03-01','2012-03-27'
   exec PersonalityGetScoreDetails 'f830b9ae-5ba9-4ecd-9a0f-ca53a57f5e06',50,50,40,20,'BELOW','BELOW','BELOW','BELOW',null,null
*/
--[dbo].PersonalityGetScoreDetailsIndividual  'ccaf9404-cde5-4fec-9b21-00bf7af1977d',0,0,0,0,'Above','Above','Above','Above',null,null
GO
