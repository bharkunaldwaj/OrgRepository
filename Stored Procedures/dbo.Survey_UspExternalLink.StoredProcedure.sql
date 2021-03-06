USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspExternalLink]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspExternalLink]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspExternalLink]  
@CompanyID	int=null,
@ProjectID	int=null,
@AccountID int=null,
@QuestionnaireID int=null,
@ProgrammeID int=null,
@ModifyBy int=null,
@ModifyDate datetime=null,
@IsActive bit=null,
@Operation varchar(15)=null,
@CreatedBy varchar(50)=null,
@CustomEmail varchar(500)=null,
@EmailTemplateId int=null,
@ExternalLink varchar(max)=null,
@EmailTo varchar(25)=null,
@UniqueID uniqueidentifier=null,
@Status bit=null,
@AnalysisType varchar(50)=null,
@Analysis1 varchar(100)=null,
@Analysis2 varchar(100)=null,
@Analysis3 varchar(100)=null,
@CandidateEmail varchar(100)=null,
@CandidateName varchar(100)=null,
@EmailSendFlag int =null,
@SubmitFlag bit=null,
@SendEmailOnCompletion bit = null,
@Instructions varchar(max)= null,
@SendReportToParticipant bit = null
as


IF (@Operation = 'ADDEXLINK')
Begin

insert into 
Survey_ExternalLink(AccountID,CompanyId,CreatedBy,CreatedDate,CustomEmail,EmailTemplateId,EmailTo,ExternalLink,IsActive,ProgrammeId,ProjectId,UniqueID,SendEmailOnCompletion,Instructions,SendReportToParticipant)
			values(@AccountID,@CompanyId,@CreatedBy,GETDATE(),@CustomEmail,@EmailTemplateId,@EmailTo,@ExternalLink,@IsActive,@ProgrammeId,@ProjectId,@UniqueID,@SendEmailOnCompletion,@Instructions,@SendReportToParticipant)

End


IF (@Operation = 'UPDATEEXLINK')
Begin


UPDATE [Survey_ExternalLink]
   SET 
		--[AccountID] = @AccountID
  --    ,[ProjectId] = @ProjectId
  --    ,[CompanyId] = @CompanyId
  --    ,[ProgrammeId] = @ProgrammeId,
       [EmailTo] = @EmailTo
      ,[CustomEmail] = @CustomEmail
      ,[EmailTemplateId] = @EmailTemplateId
      --,[ExternalLink] = <ExternalLink, varchar,>
      --,[IsActive] = <IsActive, bit,>
      ,[SendEmailOnCompletion] = @SendEmailOnCompletion
      ,[Instructions] = @Instructions
      ,[SendReportToParticipant]=@SendReportToParticipant
      
		WHERE [UniqueID] = @UniqueID
 

End



IF (@Operation = 'GETEXLINK')
Begin

--select sel.*,sp.Title as ProgrammeName,acc.Code as AccountCode,sete.Title EmailTitle,sq.QSTNName QuestionName,
--Case sel.IsActive when 1 then 'Active' else 'InActive' end as Status,
--sel.IsActive Active,
--Case sel.IsActive when 1 then 0 else 1 end as Inactive

--from Survey_ExternalLink sel
--left join Survey_EmailTemplate sete
--on sel.EmailTemplateId=sete.EmailTemplateID
--left join Survey_Project sp
--on sel.ProjectId=sp.ProjectID
--left join Survey_Company sc
--on sc.ProjectId=sp.ProjectID
--left join Survey_Questionnaire sq
--on sp.QuestionnaireID=sq.QuestionnaireID
--left join Account acc
--on sel.AccountID=acc.AccountID
--where (@AccountID is null or sel.AccountID=@AccountID)
--and (@CompanyID is null or sel.CompanyId=@CompanyID)
--and (@ProjectID is null or sel.ProjectID=@ProjectID)
--and (@Status is null or sel.IsActive=@Status)

select sel.*,sete.Title EmailTitle,sq.QSTNName QuestionName,
Case sel.IsActive when 1 then 'Active' else 'InActive' end as Status,
sel.IsActive Active,
Case sel.IsActive when 1 then 0 else 1 end as Inactive,sc.Title as CompanyName,sas.ProgrammeName

from Survey_ExternalLink sel
left join Survey_EmailTemplate sete
on sel.EmailTemplateId=sete.EmailTemplateID
left join Survey_Project sp
on sel.ProjectId=sp.ProjectID
left join Survey_Questionnaire sq
on sp.QuestionnaireID=sq.QuestionnaireID

left join Survey_Company sc
on sc.CompanyID=sel.CompanyId

left join Survey_Analysis_Sheet sas 
on sas.ProgrammeID=sel.ProgrammeId
where (@AccountID is null or sel.AccountID=@AccountID)
and (@CompanyID is null or sel.CompanyId=@CompanyID)
and (@ProjectID is null or sel.ProjectID=@ProjectID)
and (@Status is null or sel.IsActive=@Status)

End


IF (@Operation = 'UPDEXLINK')
Begin

update Survey_ExternalLink set IsActive=@Status where UniqueID=@UniqueID

End

IF (@Operation = 'GETLINK')
Begin



select sel.*,sp.StartDate,sp.EndDate, sp.Title as ProjectName,acc.Code as AccountCode,sas.ProgrammeName,sc.Title CompanyName,sq.QSTNName QuestionName,sq.QuestionnaireID,sel.Instructions,sc.Finish_EmailID
from Survey_ExternalLink sel
left join Account acc
on sel.AccountID=acc.AccountID
left join Survey_Company sc
on sel.CompanyId=sc.CompanyID
left join Survey_Project sp
on sel.ProjectId=sp.ProjectID
left join Survey_Questionnaire sq
on sp.QuestionnaireID=sq.QuestionnaireID


left join Survey_Analysis_Sheet sas 
on sas.ProgrammeID=sel.ProgrammeId

where UniqueID=@UniqueID

End

IF (@Operation = 'GETANA')
Begin

select * 
from 
Survey_AnalysisSheet_Category_Details where Programme_Id=@ProgrammeID --and Analysis_Type=@AnalysisType

End

IF (@Operation = 'UPPARTIC')
Begin
declare @assigncount int
declare @assignId int

set @assigncount=(
select COUNT(*) from Survey_AssignQuestionnaire 
where AccountID=@AccountID and ProjecctID=@ProjectID
and ProgrammeID=@ProgrammeID and QuestionnaireID=@QuestionnaireID)

		if(@assigncount=0)
		Begin 
			insert Survey_AssignQuestionnaire(AccountID,ProgrammeID,ProjecctID,QuestionnaireID,CandidateNo,IsActive)
			values(@AccountID,@ProgrammeID,@ProjectID,@QuestionnaireID,1,1)
			set @assignId=(select SCOPE_IDENTITY())
		End
		else
		Begin
		    set @assignId=(
							select AssignmentID from Survey_AssignQuestionnaire 
							where AccountID=@AccountID and ProjecctID=@ProjectID
							and ProgrammeID=@ProgrammeID and QuestionnaireID=@QuestionnaireID)
		End
		
		insert into Survey_AssignmentDetails(Analysis_I,Analysis_II,Analysis_III,AssignmentID,CandidateEmail,CandidateName,EmailSendFlag,SubmitFlag)
		values (@Analysis1,@Analysis2,@Analysis3,@assignId,@CandidateEmail,@CandidateName,@EmailSendFlag,@SubmitFlag)
		select SCOPE_IDENTITY()
End
GO
