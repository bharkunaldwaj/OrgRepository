USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[USPSchGetReminderData2]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[USPSchGetReminderData2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USPSchGetReminderData2]

as

begin

		SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.EmailText, '[TITLE]', Project.Title)
						,'[FIRSTNAME]', substring(AssignmentDetails.CandidateName,1,case when charindex(' ',AssignmentDetails.CandidateName) > 1 then charindex(' ',AssignmentDetails.CandidateName)
										 else len(AssignmentDetails.CandidateName) end ))
						,'[NAME]', AssignmentDetails.CandidateName)
						,'[COMPANY]', Account.OrganisationName)
						,'[IMAGE]', '<img src=cid:companylogo')
						,'[STARTDATE]', convert(varchar(15),dbo.Programme.StartDate, 106))
						,'[CLOSEDATE]', convert(varchar(15),dbo.Programme.EndDate, 106))
						,'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName)
						,'[PARTICIPANTEMAIL]', dbo.[User].EmailID) as EmailText
						,EmailTemplate.EmailImage
						--,EmailTemplate.EmailTemplateID
						, dbo.AssignmentDetails.CandidateEmail, dbo.[User].EmailID AS ParticipantEmail, 
						  
						  REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.Subject,'[TITLE]', dbo.Project.Title), 
                      '[FIRSTNAME]', dbo.[User].FirstName), '[NAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), '[COMPANY]', dbo.Account.OrganisationName), 
                      '[STARTDATE]', CONVERT(varchar(15), dbo.Programme.StartDate, 106)), 
                      '[CLOSEDATE]', CONVERT(varchar(15), dbo.Programme.EndDate, 106)), '[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), 
                      '[PARTICIPANTEMAIL]', dbo.[User].EmailID), '[LOGINID]', dbo.[User].LoginID), '[PASSWORD]', dbo.[User].Password), '[EMAILID]', dbo.[User].EmailID), 
                      '[CODE]', dbo.Account.Code) AS [Subject],
                      
						  dbo.Project.Title, dbo.Programme.ProgrammeName, dbo.Questionnaire.QSTNName,
						  --dbo.Programme.EndDate, dbo.Programme.StartDate, 
						  dbo.AssignmentDetails.CandidateName, dbo.Account.OrganisationName, 
						  dbo.[User].FirstName + ' ' + dbo.[User].LastName AS ParticipantName 
						  ,AssignmentDetails.AsgnDetailID
						  ,dbo.Questionnaire.QuestionnaireID
						  ,dbo.Account.AccountID
						  ,dbo.Programme.ProgrammeID
						  ,dbo.Project.ProjectID
						  ,dbo.[User].UserID
						  ,dbo.Programme.Reminder2Date as EmailDate	
	FROM         dbo.AssignmentDetails INNER JOIN
						  dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID INNER JOIN
						  dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
						  
						  dbo.EmailTemplate ON dbo.EmailTemplate.EmailTemplateID = 
						  CASE WHEN AssignmentDetails.RelationShip !='Self' THEN dbo.Project.EmailTMPLReminder2 
							   WHEN AssignmentDetails.RelationShip ='Self' THEN dbo.Project.EmailTMPSelfReminder 
						  END INNER JOIN
						  
						  --dbo.EmailTemplate ON dbo.Project.EmailTMPLReminder2 = dbo.EmailTemplate.EmailTemplateID INNER JOIN
						  
						  dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
						  dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
						  dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
						  dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID
	where convert(varchar(10),dbo.Programme.Reminder2Date, 101) = convert(varchar(10),GETDATE(), 101) and (AssignmentDetails.SubmitFlag != 1 or AssignmentDetails.SubmitFlag is null) and AssignmentDetails.EmailSendFlag = 1
	AND 
	1 = CASE WHEN AssignmentDetails.RelationShip !='Self' THEN 1 
	WHEN AssignmentDetails.RelationShip ='Self' AND dbo.Project.EmailTMPSelfReminder!=0 THEN 1 ELSE 0 END
	
	--AND AssignmentDetails.RelationShip !='Self'	
	----WHERE     (dbo.Programme.Reminder2Date = CONVERT(DATETIME, '2011-03-20', 102)) and (AssignmentDetails.SubmitFlag != 1 or AssignmentDetails.SubmitFlag is null)
	--UNION

	--SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.EmailText, '[TITLE]', Project.Title)
	--					,'[FIRSTNAME]', substring(AssignmentDetails.CandidateName,1,case when charindex(' ',AssignmentDetails.CandidateName) > 1 then charindex(' ',AssignmentDetails.CandidateName)
	--									 else len(AssignmentDetails.CandidateName) end ))
	--					,'[NAME]', AssignmentDetails.CandidateName)
	--					,'[COMPANY]', Account.OrganisationName)
	--					,'[IMAGE]', '<img src=cid:companylogo')
	--					,'[STARTDATE]', convert(varchar(15),dbo.Programme.StartDate, 106))
	--					,'[CLOSEDATE]', convert(varchar(15),dbo.Programme.EndDate, 106))
	--					,'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName)
	--					,'[PARTICIPANTEMAIL]', dbo.[User].EmailID) as EmailText
	--					,EmailTemplate.EmailImage
	--					--,EmailTemplate.EmailTemplateID
	--					, dbo.AssignmentDetails.CandidateEmail, dbo.[User].EmailID AS ParticipantEmail, 

	--					  REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.Subject,'[TITLE]', dbo.Project.Title), 
 --                     '[FIRSTNAME]', dbo.[User].FirstName), '[NAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), '[COMPANY]', dbo.Account.OrganisationName), 
 --                     '[STARTDATE]', CONVERT(varchar(15), dbo.Programme.StartDate, 106)), 
 --                     '[CLOSEDATE]', CONVERT(varchar(15), dbo.Programme.EndDate, 106)), '[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), 
 --                     '[PARTICIPANTEMAIL]', dbo.[User].EmailID), '[LOGINID]', dbo.[User].LoginID), '[PASSWORD]', dbo.[User].Password), '[EMAILID]', dbo.[User].EmailID), 
 --                     '[CODE]', dbo.Account.Code) AS [Subject],

	--					  dbo.Project.Title, dbo.Programme.ProgrammeName, dbo.Questionnaire.QSTNName,
	--					  --dbo.Programme.EndDate, dbo.Programme.StartDate, 
	--					  dbo.AssignmentDetails.CandidateName, dbo.Account.OrganisationName, 
	--					  dbo.[User].FirstName + ' ' + dbo.[User].LastName AS ParticipantName 
	--					  ,AssignmentDetails.AsgnDetailID
	--					  ,dbo.Questionnaire.QuestionnaireID
	--					  ,dbo.Account.AccountID
	--					  ,dbo.Programme.ProgrammeID
	--					  ,dbo.Project.ProjectID
	--					  ,dbo.[User].UserID
	--					  ,dbo.Programme.Reminder2Date as EmailDate	
	--FROM         dbo.AssignmentDetails INNER JOIN
	--					  dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID INNER JOIN
	--					  dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
	--					  dbo.EmailTemplate ON dbo.Project.EmailTMPSelfReminder  = dbo.EmailTemplate.EmailTemplateID INNER JOIN
	--					  dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
	--					  dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
	--					  dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
	--					  dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID
	--where convert(varchar(10),dbo.Programme.Reminder2Date, 101) = convert(varchar(10),GETDATE(), 101) 
	--and AssignmentDetails.SubmitFlag = 1 
	--and AssignmentDetails.EmailSendFlag = 0
	--AND AssignmentDetails.RelationShip ='Self' AND dbo.Project.EmailTMPSelfReminder!=0
	
	
end
GO
