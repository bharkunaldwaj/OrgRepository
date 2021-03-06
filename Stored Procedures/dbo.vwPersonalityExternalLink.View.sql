USE [Feedback360_Dev2]
GO
/****** Object:  View [dbo].[vwPersonalityExternalLink]    Script Date: 06/23/2015 10:42:49 ******/
DROP VIEW [dbo].[vwPersonalityExternalLink]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 select * from [vwPersonalityParticiapntDetails]
*/
CREATE view [dbo].[vwPersonalityExternalLink] 
(UniqueID,CreatedDate,EmailTo,AccountID, AccountCode,accountDesc,ReportName,
QuestionnairesDesc,EmailDescription,IsActive)
as


Select l.UniqueID,l.CreatedDate,l.EmailTo,a.AccountID,a.Code AccountCode, a.Description accountDesc,r.ReportName,
q.Description QuestionnairesDesc,e.Description EmailDescription,l.IsActive  from PersonallityExternalLink l
INNER JOIN Account a on a.AccountID = l.AccountID 
INNER JOIN PersonalityReportManagement r on r.UniqueID = l.ReportManagementID
INNER JOIN PersonalityQuestionnaires q on q.UniqueID = l.QuestionnaireID
INNER JOIN PersonalityEmailTemplates e on e.UniqueID = l.EmailTemplateId
GO
