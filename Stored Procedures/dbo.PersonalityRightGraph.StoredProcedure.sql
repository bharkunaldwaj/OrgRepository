USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityRightGraph]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityRightGraph]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec [PersonalityRightGraph]  '17365bf0-9d79-4eb2-ae10-02a7b441191b', '3dd00653-b14b-40e9-835a-cbf151b5b2e4'

--  exec [PersonalityRightGraph] 'f17727ee-95e1-431e-ab96-24ef7f29bd4b'


CREATE proc [dbo].[PersonalityRightGraph] --'4E31A810-871A-42E5-A057-012FC957A367'
@ParticipantID varchar(50)=null,
@ReportManagement varchar(50) = null
as


----------------------------------- Change Due to Client Demanded -------------------------------------
/*
Declare @minValue int
select @minValue=min(PersonalityQuestionsAnswers.ScoreValue) 
	from PersonalityQuestionsAnswers
	inner join PersonalityQuestionChoices ON 
	dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
	where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID --and ColorCode ='Green'

create table #Temp (MinCount int ,ColorCode varchar(10),Sequence char(1),NewColorCode varchar(10))

insert into  #Temp
select COUNT(*) as MinCount,ColorCode,'1' AS Sequence,'Red' as NewColorCode
from PersonalityQuestionsAnswers
inner join PersonalityQuestionChoices ON 
dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID and ColorCode ='Green'
and PersonalityQuestionsAnswers.ScoreValue =@minValue
 
group by PersonalityQuestionChoices.ColorCode 

UNION

select COUNT(*) as MinCount,ColorCode,'2' AS Sequence,'Yellow' as NewColorCode
from PersonalityQuestionsAnswers
inner join PersonalityQuestionChoices ON 
dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID and ColorCode ='Blue'
and PersonalityQuestionsAnswers.ScoreValue =@minValue
group by PersonalityQuestionChoices.ColorCode 

UNION

select COUNT(*) as MinCount,ColorCode,'3' AS Sequence,'Green' as NewColorCode
from PersonalityQuestionsAnswers
inner join PersonalityQuestionChoices ON 
dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID and ColorCode ='Red'
and PersonalityQuestionsAnswers.ScoreValue =@minValue
group by PersonalityQuestionChoices.ColorCode  

UNION

select COUNT(*) as MinCount,ColorCode,'4' AS Sequence,'Blue' as NewColorCode
from PersonalityQuestionsAnswers
inner join PersonalityQuestionChoices ON 
dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID and ColorCode ='Yellow'
and PersonalityQuestionsAnswers.ScoreValue =@minValue
group by PersonalityQuestionChoices.ColorCode 


IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Red'))
	INSERT INTO #Temp SELECT 0,'Red',3,'Green'
	
IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Green'))
	INSERT INTO #Temp SELECT 0,'Green',1,'Red'
	
IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Yellow'))
	INSERT INTO #Temp SELECT 0,'Yellow',4,'Blue'
	
IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Blue'))
	INSERT INTO #Temp SELECT 0,'Blue',2,'Yellow'


declare @SumMinCount int
set @SumMinCount = (select sum(MinCount) from #Temp)

select MinCount,ColorCode, @SumMinCount as SumMinCount,
( CASE WHEN @SumMinCount = 0 THEN '0'
       WHEN @SumMinCount > 0 THEN ROUND(Convert(float,(Convert(float,MinCount)*Convert(float,100)/Convert(float,@SumMinCount))),0) END ) as Percentage,NewColorCode from #Temp
ORDER BY Sequence
    
drop table #Temp
*/

----------------------------------- Change Due to Client Demand -------------------------------------
 
 
 
declare @Least1 int
declare @Least2 int



create table #TempWhole (ScoreValue int,QuestionID varchar(50),Text varchar(500) ,ColorCode varchar(10),AnswerID Varchar(50))
create table #TempDesc (ScoreValue int,QuestionID varchar(50),Text varchar(500) ,ColorCode varchar(10),AnswerID Varchar(50))
create table #TempASC (ScoreValue int,QuestionID varchar(50),Text varchar(500) ,ColorCode varchar(10),AnswerID Varchar(50))
 
create table #TempInterchange (ScoreValueAsc int,ScoreValueDesc int,QuestionID varchar(50))
/*
insert into #TempWhole
select PersonalityQuestionsAnswers.ScoreValue,PersonalityQuestionsAnswers.QuestionID,Text,ColorCode
	from PersonalityQuestionsAnswers
	inner join PersonalityQuestionChoices ON 
	dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
	where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID
	order by 2,4
	
insert into #TempLeastTwo
select top 2 PersonalityQuestionsAnswers.ScoreValue
	from PersonalityQuestionsAnswers
	inner join PersonalityQuestionChoices ON 
	dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
	where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID
	group by PersonalityQuestionsAnswers.ScoreValue
	order by 1

set @Least1= (select top 1 ScoreValue from #TempLeastTwo)	
delete top (1) from #TempLeastTwo
set @Least2= (select top 1 ScoreValue from #TempLeastTwo)

Print @Least1
Print @Least2

insert into #TempNewWhole
select  CASE WHEN ScoreValue =@Least1 then @Least2
			 WHEN ScoreValue =@Least2 then @Least1 
			 ELSE 0 END as ScoreValue,
		QuestionID,[Text],ColorCode from #TempWhole 
order by 4
*/
--SELECT * FROM #TempNewWhole

insert into #TempWhole
SELECT m.ScoreValue,m.QuestionID,Text,ColorCode,m.UniqueID
	from PersonalityQuestionsAnswers m
	inner join PersonalityQuestionChoices ON 
	dbo.PersonalityQuestionChoices.UniqueID = m.QuestionChoiceID
	
	WHERE m.UniqueID NOT IN 
	(
			select  s.UniqueID 
			from PersonalityQuestionsAnswers s
			inner join PersonalityQuestionChoices c ON 
			c.UniqueID = s.QuestionChoiceID
			WHERE s.UniqueID IN
			(
			select top 2 p.UniqueID
			from PersonalityQuestionsAnswers p

			WHERE p.QuestionID = s.QuestionID 
			AND
			p.ParticiapntDetailsID=@ParticipantID
			Order by p.ScoreValue  DESC,c.ColorCode
			)
			AND s.ParticiapntDetailsID=@ParticipantID
			
	)
	AND m.ParticiapntDetailsID=@ParticipantID
Order by m.QuestionID

INSERT INTO #TempDESC
SELECT m.ScoreValue,m.QuestionID,Text,ColorCode,m.AnswerID FROM #TempWhole m

WHERE m.AnswerID IN (

SELECT top 1  s.AnswerID 
			from #TempWhole s 
			WHERE s.QuestionID = m.QuestionID
			Order by s.ScoreValue DESC
)

INSERT INTO #TempASC
SELECT m.ScoreValue,m.QuestionID,Text,ColorCode,m.AnswerID FROM #TempWhole m

WHERE m.AnswerID IN (

SELECT top 1  s.AnswerID 
			from #TempWhole s 
			WHERE s.QuestionID = m.QuestionID
			Order by s.ScoreValue ASC
)
 

INSERT INTO #TempInterchange(ScoreValueDesc,QuestionID,ScoreValueAsc)
SELECT #TempDESC.Scorevalue,#TempASC.QuestionID,#TempASC.ScoreValue from #TempDESC 
INNER JOIN #TempASC on #TempASC.QuestionID = #TempDesc.QuestionID 


--Select * from #TempASC
-- UNION 
-- Select * from #TempDesc 
--Order by QuestionID


--SELECT #TempDESC.Scorevalue,#TempASC.QuestionID,#TempASC.ScoreValue from #TempDESC 
--INNER JOIN #TempASC on #TempASC.QuestionID = #TempDesc.QuestionID 

--SELECT * FROM #TempInterchange

 Update d
 SET d.ScoreValue = i.ScoreValueAsc
From #TempDesc d INNER JOIN #TempInterchange i
ON d.QuestionID = i.QuestionID

 
Update d
 SET d.ScoreValue = i.ScoreValueDesc
From #TempASC d INNER JOIN #TempInterchange i
ON d.QuestionID = i.QuestionID
 
 
 
 
--  SELECT #TempDESC.Scorevalue,#TempASC.AnswerID,#TempASC.ScoreValue from #TempDESC 
--INNER JOIN #TempASC on #TempASC.QuestionID = #TempDesc.QuestionID 
 
 
 
--  Select * from #TempASC
 
-- Select * from #TempDesc
/*
DECLARE @ScoreValue Decimal
DECLARE @ColorCode Varchar(10)
DECLARE @QuestionId Varchar(50)


DECLARE @ScoreValue2 Decimal
DECLARE @ColorCode2 Varchar(10)
DECLARE @QuestionId2 Varchar(50)

 DECLARE _cursor CURSOR FOR 
    SELECT Scorevalue,ColorCode,QuestionID
    FROM #TempDESC  -- Variable value from the outer cursor

    OPEN _cursor
    FETCH NEXT FROM _cursor INTO @ScoreValue,@ColorCode,@QuestionId

    --IF @@FETCH_STATUS <> 0 
    --    PRINT '         <<None>>'     

		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			SELECT @ColorCode2 = ColorCode,@ScoreValue2=ScoreValue FROM #TempASC WHERE QuestionID = @QuestionId
		
			UPDATE #TempASC SET ScoreValue = @ScoreValue WHERE QuestionID = @QuestionId
			
			UPDATE #TempDESC SET ScoreValue = @ScoreValue2  WHERE QuestionID = @QuestionId
         
        FETCH NEXT FROM _cursor INTO @ScoreValue,@ColorCode,@QuestionId
        END

    CLOSE  _cursor
    DEALLOCATE  _cursor


*/
Truncate Table #TempWhole

INSERT INTO #TempWhole
 Select * from #TempASC
 UNION 
 Select * from #TempDesc
 

 ------------------ New Change Discussed by Madhur Anand Sir 20 June 2014 ------------------------------
 
-----------------------------------------------------------------------------------------------------
--create table #Temp (TotalSumPerColor int ,ColorCode varchar(10),Sequence char(1),NewColorCode varchar(10))

--insert into  #Temp
--select SUM(ScoreValue),ColorCode,'1' AS Sequence,'Red' as NewColorCode from #TempWhole
--where ColorCode ='Green' 
--group by ColorCode 

--insert into  #Temp
--select SUM(ScoreValue),ColorCode,'2' AS Sequence,'Yellow' as NewColorCode from #TempWhole
--where ColorCode ='Blue' 
--group by ColorCode 

--insert into  #Temp
--select SUM(ScoreValue),ColorCode,'3' AS Sequence,'Green' as NewColorCode from #TempWhole
--where ColorCode ='Red' 
--group by ColorCode 

--insert into  #Temp
--select SUM(ScoreValue),ColorCode,'4' AS Sequence,'Blue' as NewColorCode from #TempWhole
--where ColorCode ='Yellow' 
--group by ColorCode 


----select * from #Temp

--IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Red'))
--	INSERT INTO #Temp SELECT 0,'Red',3,'Green'
	
--IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Green'))
--	INSERT INTO #Temp SELECT 0,'Green',1,'Red'
	
--IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Yellow'))
--	INSERT INTO #Temp SELECT 0,'Yellow',4,'Blue'
	
--IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Blue'))
--	INSERT INTO #Temp SELECT 0,'Blue',2,'Yellow'



---------------------------------------------------------------------------------------------------
create table #Temp (TotalSumPerColor int ,ColorCode varchar(50),Sequence char(1),NewColorCode varchar(50))

insert into  #Temp
select SUM(ScoreValue),ColorCode,'1' AS Sequence,(select Color1 from PersonalityReportManagement where UniqueID = @ReportManagement) as NewColorCode from #TempWhole
where ColorCode ='Green' 
group by ColorCode 

insert into  #Temp
select SUM(ScoreValue),ColorCode,'2' AS Sequence,(select Color2 from PersonalityReportManagement where UniqueID = @ReportManagement) as NewColorCode from #TempWhole
where ColorCode ='Blue' 
group by ColorCode 

insert into  #Temp
select SUM(ScoreValue),ColorCode,'3' AS Sequence,(select Color3 from PersonalityReportManagement where UniqueID = @ReportManagement) as NewColorCode from #TempWhole
where ColorCode ='Red' 
group by ColorCode 

insert into  #Temp
select SUM(ScoreValue),ColorCode,'4' AS Sequence,(select Color4 from PersonalityReportManagement where UniqueID = @ReportManagement) as NewColorCode from #TempWhole
where ColorCode ='Yellow' 
group by ColorCode 


--select * from #Temp


IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Red'))
	INSERT INTO #Temp SELECT 0,'Red',3, (select Color3 from PersonalityReportManagement where UniqueID = @ReportManagement)
	
IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Green'))
	INSERT INTO #Temp SELECT 0,'Green',1, (select Color1 from PersonalityReportManagement where UniqueID = @ReportManagement)
	
IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Yellow'))
	INSERT INTO #Temp SELECT 0,'Yellow',4, (select Color4 from PersonalityReportManagement where UniqueID = @ReportManagement)
	
IF(NOT EXISTS(SELECT * FROM #Temp WHERE ColorCode='Blue'))
	INSERT INTO #Temp SELECT 0,'Blue',2, (select Color2 from PersonalityReportManagement where UniqueID = @ReportManagement)








 ------------------ New Change Discussed by Madhur Anand Sir 20 June 2014 ------------------------------

declare @SumMinCount int
set @SumMinCount = (select sum(TotalSumPerColor) from #Temp)

DECLARE @Type INT
select @Type = PQ.Type from dbo.PersonalityParticiapntDetails pd INNER JOIN 
dbo.PersonalityParticipantAssignments PPA on pd.ParticipantAssignmentID = PPA.UniqueID
INNER JOIN 
PersonalityQuestionnaires  PQ on PQ.UniqueID = PPA.QuestionnaireID
WHERE
	 pd.UniqueID =@ParticipantID

select TotalSumPerColor as MinCount,ColorCode, @SumMinCount as SumMinCount,
( CASE WHEN @SumMinCount = 0 THEN '0'
       WHEN @SumMinCount > 0 THEN ROUND(Convert(float,(Convert(float,TotalSumPerColor)*Convert(float,100)/Convert(float,@SumMinCount))),0) END ) as Percentage,NewColorCode,@Type as Type from #Temp
ORDER BY Sequence

drop table #Temp
 
drop table #TempWhole
drop table #TempInterchange
drop table #TempDesc
 drop table #TempASC
GO
