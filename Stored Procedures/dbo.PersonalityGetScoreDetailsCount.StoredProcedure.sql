USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetScoreDetailsCount]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetScoreDetailsCount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[PersonalityGetScoreDetailsCount]
	@QuestionnaireID uniqueidentifier,
	@RedScore int,
	@YellowScore int,
	@GreenScore int,
	@BlueScore int,
	@RedCondition Varchar(10),
	@YellowCondition Varchar(10),
	@GreenCondition Varchar(10),
	@BlueCondition Varchar(10)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;

declare @SetRedCondition varchar(10)
declare @SetYelloCondition varchar(10)
declare @SetGreenCondition varchar(10)
declare @SetBlueCondition varchar(10)

--Drop temp table here
--DROP TABLE [dbo].[#PersonalityAnserDetailReport_TEMP]
--Create temp table here

create table #PersonalityAnserDetailReport_TEMP
(
	[QuestionnaireID] [uniqueidentifier] NULL,
	[QuestionID] [uniqueidentifier] NULL,
	[ScoreValue] [int] NULL,
	[QuestionChoiceID] [uniqueidentifier] NULL,
	[Text] [varchar](max) NULL,
	[ColorCode] [varchar](10) NULL,
	[Sequence] [varchar](50) NULL,
	[ParticipantDetailsID] [uniqueidentifier] NULL
	
)

--drop PersonalityAnserDetailReport_TEMP
IF(UPPER(@RedCondition) = 'ABOVE')
	BEGIN
	insert into #PersonalityAnserDetailReport_TEMP 
		Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue > @RedScore and ColorCode='Red' and QuestionnaireID=@QuestionnaireID
	END
	ELSE
	BEGIN
	insert into #PersonalityAnserDetailReport_TEMP 
	Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue < @RedScore and ColorCode='Red' and QuestionnaireID=@QuestionnaireID
	END
	
	
IF(UPPER(@YellowCondition) = 'ABOVE')
	BEGIN
	
	insert into #PersonalityAnserDetailReport_TEMP
		Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue>@YellowScore and ColorCode='Yellow' and QuestionnaireID=@QuestionnaireID
	END
	ELSE
	BEGIN
		
	insert into #PersonalityAnserDetailReport_TEMP
		Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue<@YellowScore and ColorCode='Yellow' and QuestionnaireID=@QuestionnaireID
	END
	

IF(UPPER(@GreenCondition) = 'ABOVE')
	BEGIN
	insert into #PersonalityAnserDetailReport_TEMP
		Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue>@GreenScore and ColorCode='Green' and QuestionnaireID=@QuestionnaireID
	END
	ELSE
	BEGIN
	insert into #PersonalityAnserDetailReport_TEMP
		Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue<@GreenScore and ColorCode='Green' and QuestionnaireID=@QuestionnaireID
	END
 	
IF(UPPER(@BlueCondition) = 'ABOVE')
	BEGIN
	insert into #PersonalityAnserDetailReport_TEMP
		Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue>@BlueScore and ColorCode='Blue' and QuestionnaireID=@QuestionnaireID
	END
	ELSE
	BEGIN
	insert into #PersonalityAnserDetailReport_TEMP
		Select QuestionnaireID,QuestionID,ScoreValue,QuestionChoiceID,Text,ColorCode, Sequence,ParticipantDetailsID from
		(Select pqa.QuestionnaireID,pqa.QuestionID,pqa.ScoreValue,pqa.QuestionChoiceID,
		(Select pqc.Text from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as Text,
		(Select pqc.ColorCode from PersonalityQuestionChoices pqc where pqc.UniqueID=pqa.QuestionChoiceID) as ColorCode,
		'Q'+CONVERT(varchar(10),PQ.Sequence) as Sequence,pqa.ParticiapntDetailsID as 'ParticipantDetailsID'
		 from PersonalityQuestionsAnswers pqa
		 inner join PersonalityQuestions PQ on PQ.UniqueID=pqa.QuestionID ) temp
		 where ScoreValue<@BlueScore and ColorCode='Blue' and QuestionnaireID=@QuestionnaireID
	END

			SELECT count(ParticipantDetailsID),ParticipantDetailsID
			FROM #PersonalityAnserDetailReport_TEMP
			group by ParticipantDetailsID
			
	--SELECT scorevalue,Text,colorcode,sequence,ParticipantDetailsID
	--		FROM #PersonalityAnserDetailReport_TEMP
	--		order by Sequence
		
	--SELECT AVG(AverageScore) as 'Total Average Score',COUNT(*) as 'Number of Questions',colorcode
	--FROM (SELECT AVG(scorevalue) as 'AverageScore',COUNT(*) as 'Number of Questions',Sequence,colorcode 
	--FROM #PersonalityAnserDetailReport_TEMP group by Sequence,colorcode) AS X
	--GROUP BY colorcode

--Drop temp table here
DROP TABLE [dbo].[#PersonalityAnserDetailReport_TEMP]

--   exec PersonalityGetScoreDetails 'C89CE89F-46F3-48B1-B3C3-540031F8135F',5,5,5,5,'ABOVE','ABOVE','ABOVE','ABOVE'


END
GO
