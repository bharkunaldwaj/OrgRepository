USE [Feedback360_Dev2]
GO
/****** Object:  View [dbo].[vwPersonalityAccountReportCountNew]    Script Date: 06/23/2015 10:42:49 ******/
DROP VIEW [dbo].[vwPersonalityAccountReportCountNew]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vwPersonalityAccountReportCountNew] 
(AccountCode, AccountID,QuestionnaireName,QuestionnaireID, FinishedDate,Company,Name,Department,Associate,ReportName)
as



Select A.Code AccountCode, A.AccountID AccountID, Q.Name QuestionnaireName,Q.UniqueID QuestionnaireID, R.CreatedDate FinishedDate,P.Company, P.FirstName + ' ' +  ISNULL(P.LastName,'') As Name, P.Department, P.Associate, RM.ReportName from PersonalityAccountReport R
INNER JOIN Account A ON A.AccountID = R.AccountID
INNER JOIN PersonalityReportManagement RM ON RM.UniqueID = R.ReportManagementId
INNER JOIN PersonalityQuestionnaires Q ON Q.UniqueID = R.QuestionnaireID
INNER JOIN PersonalityParticiapntDetails P ON P.UniqueID = R.ParticiapntDetailsID

UNION


Select A.Code AccountCode, A.AccountID AccountID, Q.Name QuestionnaireName,Q.UniqueID QuestionnaireID, 
R.CreatedDate FinishedDate,'' Company, P.ParticipantName As Name, '' Department, 
'' Associate, RM.ReportName from PersonalityAssignScoreAccountReports R
INNER JOIN Account A ON A.AccountID = R.AccountID
INNER JOIN PersonalityReportManagement RM ON RM.UniqueID = R.ReportManagementId
INNER JOIN PersonalityQuestionnaires Q ON Q.UniqueID = R.QuestionnaireID
INNER JOIN PersonalityAssignScoreType P ON P.UniqueID = R.ParticiapntDetailsID
	
	--	Select * from vwPersonalityAccountReportCount
GO
