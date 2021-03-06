USE [Feedback360_Dev2]
GO
/****** Object:  View [dbo].[vwPersonalityParticiapntDetails]    Script Date: 06/23/2015 10:42:49 ******/
DROP VIEW [dbo].[vwPersonalityParticiapntDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 select * from [vwPersonalityParticiapntDetails]
*/
CREATE view [dbo].[vwPersonalityParticiapntDetails] 
(ParticipantID,Name,Email,EndDate,CreatedDate,Company,Department,Honorific,Percentage,IsFinished,FinishedDate,QuestionnaireID,AccountID,FirstReportManagementID,SecondReportManagementID,IsParticipate,Associate)
as

Select distinct PD.UniqueID as ParticipantID, PD.FirstName+' '+PD.LastName as 'Name',PD.Email,PA.EndDate,PD.CreatedDate,PD.Company,PD.Department,PD.Honorific,
Convert(int,((Convert(Float,(select COUNT(*) from PersonalityQuestionsAnswers where ParticiapntDetailsID=PD.UniqueID))/
CASE WHEN (
SELECT SUM(TotalQ) FROM (
select  COUNT(*)*  CASE WHEN QuestionType='T' THEN 1 ELSE 4 END AS TotalQ  from PersonalityQuestions where QuestionnaireID=PA.QuestionnaireID
group by QuestionType) as tbl1
)=0 THEN 1
ELSE (
SELECT SUM(TotalQ) FROM (
select  COUNT(*)*  CASE WHEN QuestionType='T' THEN 1 ELSE 4 END AS TotalQ  from PersonalityQuestions where QuestionnaireID=PA.QuestionnaireID
group by QuestionType) as tbl1) END)*100)) as 'Percentage',
Isnull(PD.IsFinished,0),PD.FinishedDate,PA.QuestionnaireID as QuestionnaireID,PA.AccountID as 'AccountID',
(Select Top 1 RM.UniqueID from PersonalityReportManagement RM where RM.QuestionnareID=PA.QuestionnaireID and RM.AccountID=PA.AccountID order by CreatedDate) as FirstReportManagementID,
(SELECT TOP 1 a.UniqueID FROM (SELECT  TOP 2 aa.UniqueID FROM PersonalityReportManagement aa where aa.QuestionnareID=PA.QuestionnaireID and aa.AccountID=PA.AccountID ORDER BY CreatedDate DESC) a  )  as SecondReportManagementID,
CONVERT(bit,1) as IsParticipate,Associate
from PersonalityParticiapntDetails PD
inner join PersonalityParticipantAssignments PA on PA.UniqueID=PD.ParticipantAssignmentID
where 1=1
--and (CONVERT(datetime,PD.CreatedDate,103) >=  CONVERT(datetime,DATEADD(day,-30,GETDATE()),103) )
--and (CONVERT(datetime,PD.CreatedDate,103) <=  CONVERT(datetime,GETDATE(),103) )

UNION

Select sc.UniqueID,ParticipantName,'',GETDATE(),CreateDate,null,null,'',0,CONVERT(bit,1),sc.CreateDate,sc.QuestionnaireID,sc.AccountID,(Select distinct top 1 (Select Top 1 RM.UniqueID from PersonalityReportManagement RM 
		where RM.QuestionnareID=PA.QuestionnaireID and RM.AccountID=PA.AccountID order by CreatedDate) as FirstReportManagementID
						from PersonalityParticiapntDetails PD
						inner join PersonalityParticipantAssignments PA on PA.UniqueID=PD.ParticipantAssignmentID),
						(Select distinct top 1 (Select Top 1 RM.UniqueID from PersonalityReportManagement RM 
		where RM.QuestionnareID=PA.QuestionnaireID and RM.AccountID=PA.AccountID order by CreatedDate desc) as SecondReportManagementID
						from PersonalityParticiapntDetails PD
						inner join PersonalityParticipantAssignments PA on PA.UniqueID=PD.ParticipantAssignmentID),Convert(bit,0),'' AS Associate
from PersonalityAssignScoreType sc
inner join PersonalityParticipantAssignments PAA on PAA.QuestionnaireID=sc.QuestionnaireID
where 1=1
--and (CONVERT(datetime,sc.CreateDate,103) >=  CONVERT(datetime,DATEADD(day,-30,GETDATE()),103) )
--and (CONVERT(datetime,sc.CreateDate,103) <=  CONVERT(datetime,GETDATE(),103) )
GO
