USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[USPSchGetReportAvailableData]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[USPSchGetReportAvailableData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USPSchGetReportAvailableData]

as

begin

		SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.EmailText, '[TITLE]', Project.Title)
						,'[FIRSTNAME]', substring(AssignmentDetails.CandidateName,1,case when charindex(' ',AssignmentDetails.CandidateName) > 1 then charindex(' ',AssignmentDetails.CandidateName)
										 else len(AssignmentDetails.CandidateName) end ))
						,'[NAME]', AssignmentDetails.CandidateName)
						,'[COMPANY]', Account.OrganisationName)
						,'[IMAGE]', '<img src=cid:companylogo')
						,'[STARTDATE]', convert(varchar(15),dbo.Programme.StartDate, 106))
						,'[CLOSEDATE]', convert(varchar(15),dbo.Programme.EndDate, 106))
						,'[REPORTSTARTDATE]', convert(varchar(15),dbo.Programme.ReportAvaliableFrom, 106))
						,'[REPORTENDDATE]', convert(varchar(15),dbo.Programme.ReportAvaliableTo, 106))						
						,'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName)
						,'[PARTICIPANTEMAIL]', dbo.[User].EmailID) as EmailText
						,EmailTemplate.EmailImage
						--,EmailTemplate.EmailTemplateID
						, dbo.[User].EmailID as CandidateEmail, dbo.[User].EmailID AS ParticipantEmail, 
						  REPLACE(dbo.EmailTemplate.[Subject],'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName) as [Subject], 
						  dbo.Project.Title, dbo.Programme.ProgrammeName, dbo.Questionnaire.QSTNName,
						  --dbo.Programme.EndDate, dbo.Programme.StartDate, 
						  dbo.AssignmentDetails.CandidateName, dbo.Account.OrganisationName, 
						  dbo.[User].FirstName + ' ' + dbo.[User].LastName AS ParticipantName 
						  ,AssignmentDetails.AsgnDetailID
						  ,dbo.Questionnaire.QuestionnaireID	
						  ,dbo.Account.AccountID
						  ,dbo.Account.Code
						  ,dbo.Programme.ProgrammeID
						  ,dbo.Project.ProjectID
						  ,dbo.[User].UserID
						  ,dbo.[User].LoginID
						  ,dbo.[User].[Password]
						  ,dbo.Programme.ReportAvaliableFrom as EmailDate					  
	FROM         dbo.AssignmentDetails INNER JOIN
						  dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID INNER JOIN
						  dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
						  dbo.EmailTemplate ON dbo.Project.EmailTMPLReportAvalibale = dbo.EmailTemplate.EmailTemplateID INNER JOIN
						  dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
						  dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
						  dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
						  dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID
	where convert(varchar(10),dbo.Programme.ReportAvaliableFrom, 101) = convert(varchar(10),GETDATE(), 101)
	and dbo.AssignmentDetails.Relationship = 'Self'
end
GO
