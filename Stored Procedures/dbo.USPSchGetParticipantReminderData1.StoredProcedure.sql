USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[USPSchGetParticipantReminderData1]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[USPSchGetParticipantReminderData1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USPSchGetParticipantReminderData1]

as

begin

SELECT     REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.EmailText,
                      '[TITLE]', dbo.Project.Title), '[FIRSTNAME]', dbo.[User].FirstName), '[NAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), '[COMPANY]', 
                      dbo.Account.OrganisationName), '[IMAGE]', '<img src=cid:companylogo'), '[STARTDATE]', CONVERT(varchar(15), dbo.Programme.StartDate, 106)), 
                      '[CLOSEDATE]', CONVERT(varchar(15), dbo.Programme.EndDate, 106)), '[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), 
                      '[PARTICIPANTEMAIL]', dbo.[User].EmailID), '[LOGINID]', dbo.[User].LoginID), '[PASSWORD]', dbo.[User].Password), '[EMAILID]', dbo.[User].EmailID), 
                      '[CODE]', dbo.Account.Code) AS EmailText, 
                      
                      dbo.EmailTemplate.EmailImage, 'admin@orgref.com' AS AccountAdminEmail, 'Admin' AS AdminName, 
                      dbo.[User].EmailID AS ParticipantEmail, 
                      
                      REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.Subject,'[TITLE]', dbo.Project.Title), 
                      '[FIRSTNAME]', dbo.[User].FirstName), '[NAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), '[COMPANY]', dbo.Account.OrganisationName), 
                      '[STARTDATE]', CONVERT(varchar(15), dbo.Programme.StartDate, 106)), 
                      '[CLOSEDATE]', CONVERT(varchar(15), dbo.Programme.EndDate, 106)), '[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), 
                      '[PARTICIPANTEMAIL]', dbo.[User].EmailID), '[LOGINID]', dbo.[User].LoginID), '[PASSWORD]', dbo.[User].Password), '[EMAILID]', dbo.[User].EmailID), 
                      '[CODE]', dbo.Account.Code) AS [Subject],

                      dbo.Project.Title, dbo.Programme.ProgrammeName, dbo.Questionnaire.QSTNName, dbo.Account.OrganisationName, 
                      dbo.[User].FirstName + ' ' + dbo.[User].LastName AS ParticipantName, dbo.Questionnaire.QuestionnaireID, dbo.Account.AccountID, 
                      dbo.Programme.ProgrammeID, dbo.Project.ProjectID, dbo.[User].UserID, dbo.Programme.PartReminder1Date AS EmailDate
                      ,dbo.Account.Code
					  ,dbo.[User].LoginID
					  ,dbo.[User].[Password]
FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.EmailTemplate ON dbo.Project.EmailTMPPartReminder1 = dbo.EmailTemplate.EmailTemplateID INNER JOIN
                      dbo.Questionnaire ON dbo.AssignQuestionnaireParticipant.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
                      dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
                      dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
WHERE convert(varchar(10),dbo.Programme.PartReminder1Date, 101) = convert(varchar(10),getdate(), 101) 
	and dbo.[User].UserID not in (select TargetPersonID from AssignQuestionnaire)
ORDER BY dbo.[User].UserID

	--SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.EmailText, '[TITLE]', Project.Title)
	--					,'[FIRSTNAME]', [User].FirstName)
	--					,'[NAME]', [User].FirstName + ' ' + [User].LastName)
	--					,'[COMPANY]', Account.OrganisationName)
	--					,'[IMAGE]', '<img src=cid:companylogo')
	--					,'[STARTDATE]', convert(varchar(15),dbo.Programme.StartDate, 106))
	--					,'[CLOSEDATE]', convert(varchar(15),dbo.Programme.EndDate, 106))
	--					,'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName)
	--					,'[PARTICIPANTEMAIL]', dbo.[User].EmailID) 
	--					,'[LOGINID]', dbo.[User].LoginID) 
	--					,'[PASSWORD]', dbo.[User].[Password]) 
	--					,'[EMAILID]', dbo.[User].EmailID)
	--					,'[CODE]', dbo.[Account].Code) as EmailText
	--					,EmailTemplate.EmailImage
	--					,'admin@orgref.com' as AccountAdminEmail
	--					,'Admin' as AdminName
	--					--,EmailTemplate.EmailTemplateID
	--					--, dbo.AssignmentDetails.CandidateEmail
	--					, dbo.[User].EmailID AS ParticipantEmail, 
	--					  REPLACE(dbo.EmailTemplate.[Subject],'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName) as [Subject], 
	--					  dbo.Project.Title, dbo.Programme.ProgrammeName, dbo.Questionnaire.QSTNName,
	--					  --dbo.Programme.EndDate, dbo.Programme.StartDate, 
	--					  --dbo.AssignmentDetails.CandidateName
	--					  dbo.Account.OrganisationName, 
	--					  dbo.[User].FirstName + ' ' + dbo.[User].LastName AS ParticipantName 
	--					  --,AssignmentDetails.AsgnDetailID
	--					  ,dbo.Questionnaire.QuestionnaireID
	--					  ,dbo.Account.AccountID
	--					  ,dbo.Programme.ProgrammeID
	--					  ,dbo.Project.ProjectID
	--					  ,dbo.[User].UserID
	--					  ,dbo.Programme.PartReminder1Date  as EmailDate	
	--FROM         dbo.AssignQuestionnaire INNER JOIN
	--					  --dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID INNER JOIN
	--					  dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
	--					  dbo.EmailTemplate ON dbo.Project.EmailTMPPartReminder1 = dbo.EmailTemplate.EmailTemplateID INNER JOIN
	--					  dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
	--					  dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
	--					  dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
	--					  dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID
	--where convert(varchar(10),dbo.Programme.PartReminder1Date, 101) = convert(varchar(10),getdate(), 101) 
	----where dbo.AssignmentDetails.AsgnDetailID = 2105
	--order by [User].UserID 
	
end
GO
