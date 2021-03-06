USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityTeamReport]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityTeamReport]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec [PersonalityTeamReport] 'F0BDED17-B750-4229-A127-00C3D70745F9,0426E556-C523-49D7-8DC1-9D8B82F0A590,4E31A810-871A-42E5-A057-012FC957A367,20B75DCC-A343-4A8E-B1AF-01847827710E,3F977DB0-9FEF-4843-A591-40D20E2DA725,02DABFF1-45F6-4E30-8687-4FDC688EA4A3'

CREATE proc [dbo].[PersonalityTeamReport]

@ParticipantList varchar(max)
as

create table #TempColorOrder ([Type] INT,ColorOrder varchar(15),ParticipantID varchar(100),Name VARCHAR(200))
create table #TempParticipant (ParticipantID varchar(100))
create table #TempPersonalityParticiapntDetails (ParticipantID varchar(100))
create table #TempPersonalityAssignScoreType (ParticipantID varchar(100))

INSERT INTO #TempParticipant
select Items AS ParticipantID from TblUfSplit(@ParticipantList,',')

INSERT INTO #TempPersonalityParticiapntDetails
SELECT UniqueID FROM PersonalityParticiapntDetails WHERE UniqueID IN ( SELECT ParticipantID FROM #TempParticipant)

INSERT INTO #TempPersonalityAssignScoreType
SELECT UniqueID FROM PersonalityAssignScoreType WHERE UniqueID IN ( SELECT ParticipantID FROM #TempParticipant)

declare @Type int
declare @Count int 
declare @ColorOrder varchar(15)
declare @FirstName VARCHAR(100)
declare @Name VARCHAR(200)

set @ColorOrder = ''

set @Count=( select COUNT(*) from #TempPersonalityParticiapntDetails)
while(@Count >0)
BEGIN

select  @ColorOrder = ISNULL(@ColorOrder,'') + SUBSTRING(dbo.PersonalityQuestionChoices.ColorCode,1,1)
		from PersonalityQuestionsAnswers
		inner join PersonalityQuestionChoices ON 
		dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
		inner join PersonalityQuestionnaires ON
		PersonalityQuestionnaires.UniqueID=PersonalityQuestionsAnswers.QuestionnaireID
		where PersonalityQuestionsAnswers.ParticiapntDetailsID in (select top 1 ParticipantID from #TempPersonalityParticiapntDetails)
		group by  dbo.PersonalityQuestionChoices.ColorCode
        ORDER BY AVG(CASE WHEN PersonalityQuestionnaires.[Type]=2 THEN  
PersonalityQuestionsAnswers.ScoreValue*10 WHEN PersonalityQuestionnaires.[Type] = 1 
THEN PersonalityQuestionsAnswers.ScoreValue END ) DESC

set @Type = (
	 select PersonalityColorOrderType.Type 
	 from PersonalityColorOrderType 
	 where PersonalityColorOrderType.Color_Order = @ColorOrder)
	 
 if(@Type = '' or @Type is null ) 
(
 select @Type =  PersonalityColorOrderType.Type 
 from PersonalityColorOrderType 
 where @ColorOrder like  PersonalityColorOrderType.Color_Order + '%'
)

select  @Name=FirstName + ' ' + LastName from dbo.PersonalityParticiapntDetails where UniqueID in (select top 1 ParticipantID from #TempPersonalityParticiapntDetails)

INSERT INTO #TempColorOrder (ColorOrder,[Type],ParticipantID,Name) VALUES (@ColorOrder,@Type,(select top 1 ParticipantID from #TempPersonalityParticiapntDetails),@Name)
delete top (1) from #TempPersonalityParticiapntDetails

SET @ColorOrder=''
set @Count=@Count - 1

END
SET @Count=''

set @Count=( select COUNT(*) from #TempPersonalityAssignScoreType)
while(@Count >0)
BEGIN

set @Type=(SELECT Name FROM PersonalityTypes where UniqueID=
	(select TypeID from PersonalityAssignScoreType where UniqueID in (select top 1 ParticipantID from #TempPersonalityAssignScoreType)))

select @Name= p.ParticipantName from dbo.PersonalityAssignScoreType p where UniqueID in (select top 1 ParticipantID from #TempPersonalityAssignScoreType)

INSERT INTO #TempColorOrder ([Type],ColorOrder,ParticipantID,Name) VALUES (@Type,'',(select top 1 ParticipantID from #TempPersonalityAssignScoreType),@Name)

delete top (1) from #TempPersonalityAssignScoreType
set @Count=@Count - 1
end

declare @TeamReportHeading varchar(500)

set @TeamReportHeading = (
select top 1 TeamReportHeading from PersonalityQuestionnaires 
where UniqueID=(select QuestionnaireID from PersonalityParticipantAssignments where UniqueID=(select ParticipantAssignmentID from PersonalityParticiapntDetails where UniqueID=@ParticipantList)))


select ROW_NUMBER() OVER(ORDER BY ParticipantID DESC) AS RowOrder, *,@TeamReportHeading as TeamReportHeading FROM #TempColorOrder
where [Type] < 17

drop table #TempColorOrder
drop table #TempParticipant
drop table #TempPersonalityParticiapntDetails
drop table #TempPersonalityAssignScoreType
GO
