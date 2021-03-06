USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_USPSchGetReminderData2]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_USPSchGetReminderData2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_USPSchGetReminderData2]

as

begin

		SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.Survey_EmailTemplate.EmailText, '[TITLE]', Survey_Project.Title)
						,'[FIRSTNAME]', substring(Survey_AssignmentDetails.CandidateName,1,case when charindex(' ',Survey_AssignmentDetails.CandidateName) > 1 then charindex(' ',Survey_AssignmentDetails.CandidateName)
										 else len(Survey_AssignmentDetails.CandidateName) end ))
						,'[NAME]', Survey_AssignmentDetails.CandidateName)
						,'[COMPANY]', Account.OrganisationName)
						,'[IMAGE]', '<img src=cid:companylogo')
						,'[STARTDATE]', convert(varchar(15),dbo.Survey_Analysis_Sheet.StartDate, 106))
						,'[CLOSEDATE]', convert(varchar(15),dbo.Survey_Analysis_Sheet.EndDate, 106))
						--,'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName)
						--,'[PARTICIPANTEMAIL]', dbo.[User].EmailID) 
						as EmailText
						,Survey_EmailTemplate.EmailImage
						--,EmailTemplate.EmailTemplateID
						, dbo.Survey_AssignmentDetails.CandidateEmail, 
						--dbo.[User].EmailID AS ParticipantEmail, 
						  
						  REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.Survey_EmailTemplate.Subject,'[TITLE]', dbo.Survey_Project.Title), 
                      --'[FIRSTNAME]', dbo.[User].FirstName), '[NAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), 
                      '[COMPANY]', dbo.Account.OrganisationName), 
                      '[STARTDATE]', CONVERT(varchar(15), dbo.Survey_Analysis_Sheet.StartDate, 106)), 
                      '[CLOSEDATE]', CONVERT(varchar(15), dbo.Survey_Analysis_Sheet.EndDate, 106)), 
                      --'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName), 
                      --'[PARTICIPANTEMAIL]', dbo.[User].EmailID), '[LOGINID]', dbo.[User].LoginID), '[PASSWORD]', dbo.[User].Password), '[EMAILID]', dbo.[User].EmailID), 
                      '[CODE]', dbo.Account.Code) AS [Subject],
                      
						  dbo.Survey_Project.Title, dbo.Survey_Analysis_Sheet.ProgrammeName, dbo.Survey_Questionnaire.QSTNName,
						  --dbo.Programme.EndDate, dbo.Programme.StartDate, 
						  dbo.Survey_AssignmentDetails.CandidateName, dbo.Account.OrganisationName, 
						  --dbo.[User].FirstName + ' ' + dbo.[User].LastName AS ParticipantName,
						  Survey_AssignmentDetails.AsgnDetailID
						  ,dbo.Survey_Questionnaire.QuestionnaireID
						  ,dbo.Account.AccountID
						  ,dbo.Survey_Analysis_Sheet.ProgrammeID
						  ,dbo.Survey_Project.ProjectID
						  --,dbo.[User].UserID
						  ,dbo.Survey_Analysis_Sheet.Reminder2Date as EmailDate	
	FROM         dbo.Survey_AssignmentDetails INNER JOIN
						  dbo.Survey_AssignQuestionnaire ON dbo.Survey_AssignmentDetails.AssignmentID = dbo.Survey_AssignQuestionnaire.AssignmentID INNER JOIN
						  dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
						  dbo.Survey_Company ON dbo.Survey_Company.ProjectId= dbo.Survey_Project.ProjectId INNER JOIN
						  dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaire.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID 
						  and dbo.Survey_Analysis_Sheet.CompanyID = dbo.Survey_Company.CompanyID INNER JOIN						  
						  dbo.Survey_EmailTemplate ON dbo.Survey_Project.EmailTMPLReminder2 = dbo.Survey_EmailTemplate.EmailTemplateID INNER JOIN
						  
						  --dbo.[User] ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.[User].AccountID INNER JOIN
						  dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaire.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
						  dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID
	where convert(varchar(10),dbo.Survey_Analysis_Sheet.Reminder2Date, 101) = convert(varchar(10),GETDATE(), 101) and (Survey_AssignmentDetails.SubmitFlag != 1 or Survey_AssignmentDetails.SubmitFlag is null)
	--WHERE     (dbo.Programme.Reminder2Date = CONVERT(DATETIME, '2011-03-20', 102)) and (AssignmentDetails.SubmitFlag != 1 or AssignmentDetails.SubmitFlag is null)
	
end
GO
