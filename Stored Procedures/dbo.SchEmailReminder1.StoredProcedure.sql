USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[SchEmailReminder1]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[SchEmailReminder1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SchEmailReminder1] 
	
AS
BEGIN
	DECLARE
	@out_desc VARCHAR(1000),
	@out_mesg VARCHAR(10)

	DECLARE @emailtext VARCHAR(2000),
	@candidateemail NVARCHAR(50),
	@participantemail NVARCHAR(50),
	@subject NVARCHAR(50),
	@title NVARCHAR(50),
	@programmename NVARCHAR(50),
	@qstname NVARCHAR(50),
	@candidatename NVARCHAR(50),
	@organisationname NVARCHAR(50),
	@participantname NVARCHAR(50)
	

	DECLARE @body NVARCHAR(1000)

	DECLARE C1 CURSOR READ_ONLY
	FOR
	SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.EmailTemplate.EmailText, '[TITLE]', Project.Title)
						,'[FIRSTNAME]', substring(AssignmentDetails.CandidateName,1,case when charindex(' ',AssignmentDetails.CandidateName) > 1 then charindex(' ',AssignmentDetails.CandidateName)
										 else len(AssignmentDetails.CandidateName) end ))
						,'[NAME]', AssignmentDetails.CandidateName)
						,'[COMPANY]', Account.OrganisationName)
						,'[STARTDATE]', convert(varchar(10),dbo.Programme.StartDate, 101))
						,'[CLOSEDATE]', convert(varchar(10),dbo.Programme.EndDate, 101))
						,'[PARTICIPANTNAME]', dbo.[User].FirstName + ' ' + dbo.[User].LastName)
						,'[PARTICIPANTEMAIL]', dbo.[User].EmailID) as EmailText
						, dbo.AssignmentDetails.CandidateEmail, dbo.[User].EmailID AS ParticipantEmail, 
						  dbo.EmailTemplate.Subject, dbo.Project.Title, dbo.Programme.ProgrammeName, dbo.Questionnaire.QSTNName,
						  --dbo.Programme.EndDate, dbo.Programme.StartDate, 
						  dbo.AssignmentDetails.CandidateName, dbo.Account.OrganisationName, 
						  dbo.[User].FirstName + ' ' + dbo.[User].LastName AS ParticipantName
	FROM         dbo.AssignmentDetails INNER JOIN
						  dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID INNER JOIN
						  dbo.Project ON dbo.AssignQuestionnaire.ProjecctID = dbo.Project.ProjectID INNER JOIN
						  dbo.EmailTemplate ON dbo.Project.EmailTMPLReminder1 = dbo.EmailTemplate.EmailTemplateID INNER JOIN
						  dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
						  dbo.[User] ON dbo.AssignQuestionnaire.TargetPersonID = dbo.[User].UserID INNER JOIN
						  dbo.Questionnaire ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
						  dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID
	where convert(varchar(10),dbo.Programme.Reminder1Date, 101) = convert(varchar(10),getdate(), 101)
	--and AssignmentDetails.AssignmentID = 228

	OPEN C1
	FETCH NEXT FROM C1 INTO @emailtext,	@candidateemail,@participantemail ,	@subject,@title ,@programmename , @qstname, @candidatename , @organisationname ,	@participantname 

	WHILE @@FETCH_STATUS = 0
	BEGIN

		SET @body =  @EmailText
		EXEC sp_send_mail

		@participantemail,   -- participant email      
		@candidateemail, -- Candidate email
		@subject, -- Subject
		@emailtext, -- Email text
		'htmlbody', 
		@output_mesg = @out_mesg output,
		@output_desc = @out_desc output

		PRINT @out_mesg
		PRINT @out_desc

		FETCH NEXT FROM C1 INTO @emailtext,	@candidateemail,@participantemail ,	@subject,@title ,@programmename , @qstname, @candidatename , @organisationname ,	@participantname 

	END

	CLOSE C1
	DEALLOCATE C1
	

END

--exec [SchEmailReminder1]
GO
