USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityDailyFeedbackMailScheduler]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityDailyFeedbackMailScheduler]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[PersonalityDailyFeedbackMailScheduler]
As
BEGIN
create table #tempuser
(Id int identity(1,1),userName varchar(500), email varchar(300),Emailtext varchar(max),subject varchar(400))

select PD.FirstName+SPACE(1)+PD.LastName,PD.Email,
 
REPLACE( 
Replace(Replace(PE.EmailText,'[FIRSTNAME]',PD.FirstName),'[CLOSEDATE]',convert(varchar(10),PA.EndDate,103))
,'[LINK]','http://84.22.182.63:8090/PersonalityFeedback.aspx?QID='+ convert(varchar(100),PA.QuestionnaireID) +'&UID='+ convert(varchar(100),PD.UniqueID) +''), 
PE.Subject 

 from PersonalityParticiapntDetails PD
inner join PersonalityParticipantAssignments PA on PD.ParticipantAssignmentID=PA.UniqueID
inner join PersonalityEmailTemplates PE on PE.UniqueID=PA.StartDateEmailTemplateID
--inner Join [User] U on U.AccountID=PA.AccountID
where convert(varchar(10),PA.StartDate,103)=convert(varchar(10),GETDATE(),103)

UNION
--===================================================================================================================
select PD.FirstName+SPACE(1)+PD.LastName,PD.Email,
 
REPLACE( 
Replace(Replace(PE.EmailText,'[FIRSTNAME]',PD.FirstName),'[CLOSEDATE]',convert(varchar(10),PA.EndDate,103))
,'[LINK]','http://84.22.182.63:8090/PersonalityFeedback.aspx?QID='+ convert(varchar(100),PA.QuestionnaireID) +'&UID='+ convert(varchar(100),PD.UniqueID) +''),
PE.Subject

 from PersonalityParticiapntDetails PD
inner join PersonalityParticipantAssignments PA on PD.ParticipantAssignmentID=PA.UniqueID
inner join PersonalityEmailTemplates PE on PE.UniqueID=PA.ReminderDateEmailTemplateID
--inner Join [User] U on U.AccountID=PA.AccountID
where convert(varchar(10),PA.ReminderDate,103)=convert(varchar(10),GETDATE(),103)

UNION
select PD.FirstName+SPACE(1)+PD.LastName,PD.Email,
REPLACE( 
Replace(Replace(PE.EmailText,'[FIRSTNAME]',PD.FirstName),'[CLOSEDATE]',convert(varchar(10),PA.EndDate,103))
,'[LINK]','http://84.22.182.63:8090/PersonalityFeedback.aspx?QID='+ convert(varchar(100),PA.QuestionnaireID) +'&UID='+ convert(varchar(100),PD.UniqueID) +''),
PE.Subject

  from PersonalityParticiapntDetails PD
inner join PersonalityParticipantAssignments PA on PD.ParticipantAssignmentID=PA.UniqueID
inner join PersonalityEmailTemplates PE on PE.UniqueID=PA.EndDateEmailTemplateID
--inner Join [User] U on U.AccountID=PA.AccountID
where convert(varchar(10),PA.EndDate,103)=convert(varchar(10),GETDATE(),103)


Declare @totalCount int,@iCount int
set @iCount=1
set @totalCount=(select COUNT(*) from #tempuser)

WHILE @iCount <= @totalCount 
BEGIN
DECLARE @Subject varchar(300),@EmailText varchar(max),@Email varchar(200)

select @Email=#tempuser.email,@EmailText=#tempuser.Emailtext,@Subject=#tempuser.subject from #tempuser

EXEC msdb.dbo.Sp_send_dbmail 
@profile_name='feedback',
@recipients = 'anshumanc@damcogroup.com' , 
@subject = @Subject, 
@body = @EmailText , 
@body_format = 'HTML',
@query_result_header = 0,
@exclude_query_output = 0,
@append_query_error = 0,
@query_result_no_padding = 1 

set @iCount=@iCount+1
END 

END
GO
