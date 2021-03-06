USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspReportManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspReportManagement]
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
CREATE PROCEDURE [dbo].[UspReportManagement]
	@ReportManagementID int,
	@AccountID int,
	@ProjectID int,
    @ProgramID int,
    @TargetPersonID int,
    @ReportName varchar(50),
    @Admin char(2),
    @Operation char(2)
AS

-- Insert 

IF (@Operation = 'I')

BEGIN

delete [ReportManagement] where [TargetPersonID]=@TargetPersonID

INSERT INTO [ReportManagement]
           ([AccountID]
           ,[ProjectID]
           ,[ProgramID]
           ,[TargetPersonID]
           ,[ReportName]
           ,[DateCreated])
     VALUES
           (@AccountID
			,@ProjectID
			,@ProgramID
			,@TargetPersonID
			,@ReportName
			,GETDATE())

End

-- Update

ELSE IF (@Operation = 'U')

BEGIN

UPDATE .[ReportManagement]

   SET [AccountID] = @AccountID
      ,[ProjectID] = @ProjectID
      ,[ProgramID] = @ProgramID
      ,[TargetPersonID] = @TargetPersonID
      ,[ReportName] = @ReportName

 WHERE ReportManagementID = @ReportManagementID

END

-- Delete

ELSE IF (@Operation = 'D')

BEGIN

DELETE FROM [ReportManagement] WHERE ReportManagementID = @ReportManagementID

END


ELSE IF (@Operation = 'P')

BEGIN

SELECT [Programme].[ProgrammeID]
		  ,[Programme].[ProgrammeName]
		  ,[Programme].[ProgrammeDescription]
		  ,[Programme].[ProjectID]
		  ,[Programme].[AccountID]
		  ,[Programme].[StartDate]
		  ,[Programme].[EndDate]
		  ,[Programme].[Reminder1Date]
		  ,[Programme].[Reminder2Date]
		  ,[Programme].[Reminder3Date]
		  ,[Programme].[ReportAvaliableFrom]
		  ,[Programme].[ReportAvaliableTo]
		  ,[Programme].[ModifyBy]
		  ,[Programme].[ModifyDate]
		  ,[Programme].[IsActive]
		
	FROM [Programme]
	WHERE [ProjectID] = @ProjectID 
	
END

ELSE IF (@Operation = 'UL')

BEGIN
	if (@Admin = 'Y')
		Begin
			SELECT dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS CandidateName
			FROM dbo.AssignQuestionnaireParticipant INNER JOIN
            dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
            dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
            dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
			WHERE (dbo.AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
		
			--select u.UserID, u.FirstName +' '+ u.LastName as CandidateName
			--from Programme p
			--join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
			--join [User] u on u.UserID = aq.TargetPersonID		
			--where p.ProjectID = @ProjectID and p.ProgrammeID = @ProgramID
		end
	else
		Begin
			SELECT dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS CandidateName
			FROM dbo.AssignQuestionnaireParticipant INNER JOIN
            dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
            dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
            dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
			WHERE 
			dbo.Programme.ReportAvaliableFrom <= getdate() and 
			dbo.Programme.ReportAvaliableTo >= getdate() and	
			(dbo.AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
		
			--select u.UserID, u.FirstName +' '+ u.LastName as CandidateName
			--from Programme p
			--join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
			--join [User] u on u.UserID = aq.TargetPersonID		
			--where p.ReportAvaliableFrom <= getdate() and p.ReportAvaliableTo >= getdate()	
			--and	p.ProjectID = @ProjectID and p.AccountID = @AccountID and p.ProgrammeID = @ProgramID
		End		
END

ELSE IF (@Operation = 'R')

BEGIN
		select rm.*, p.ProgrammeName, pj.Title, a.Code, u.FirstName +' '+ u.LastName as CandidateName
		from ReportManagement rm
		join Programme p on p.ProgrammeID = rm.ProgramID
		join Project pj on pj.ProjectID = rm.ProjectID
		join Account a on a.AccountID = rm.AccountID
		join [User] u on u.UserID = rm.TargetPersonID		
		where p.ReportAvaliableFrom <= getdate() and p.ReportAvaliableTo >= getdate()			
END
ELSE IF (@Operation = 'C') -- Get Record Count

Begin
	if (@Admin = 'Y')
		Begin
			SELECT Count(*)
			FROM dbo.AssignQuestionnaireParticipant INNER JOIN
            dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
            dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
            dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
			WHERE (dbo.AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
			--select Count(*)
			--from Programme p
			--join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
			--join [User] u on u.UserID = aq.TargetPersonID		
			--where p.ProjectID = @ProjectID and p.ProgrammeID = @ProgramID
		End
	Else
		Begin
			SELECT Count(*)
			FROM dbo.AssignQuestionnaireParticipant INNER JOIN
            dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
            dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Questionnaire ON dbo.Project.QuestionnaireID = dbo.Questionnaire.QuestionnaireID INNER JOIN
            dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
			WHERE 
			dbo.Programme.ReportAvaliableFrom <= getdate() and 
			dbo.Programme.ReportAvaliableTo >= getdate()	and	
			(dbo.AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
		
			--select Count(*)
			--from Programme p
			--join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
			--join [User] u on u.UserID = aq.TargetPersonID		
			--where p.ReportAvaliableFrom <= getdate() and p.ReportAvaliableTo >= getdate()	
			--and	p.ProjectID = @ProjectID and p.AccountID = @AccountID and p.ProgrammeID = @ProgramID
		End	
End


ELSE IF (@Operation = 'RI')

BEGIN	
		select rm.*, p.ProgrammeName, pj.Title, a.Code, u.FirstName +' '+ u.LastName as CandidateName
		from ReportManagement rm
		join Programme p on p.ProgrammeID = rm.ProgramID
		join Project pj on pj.ProjectID = rm.ProjectID
		join Account a on a.AccountID = rm.AccountID
		join [User] u on u.UserID = rm.TargetPersonID		
		where ReportManagementID = @ReportManagementID
		and p.ReportAvaliableFrom <= getdate() and p.ReportAvaliableTo >= getdate()		
END

ELSE IF (@Operation = 'T')

BEGIN	
		select * from ReportManagement 
		where TargetPersonID = @TargetPersonID
END
GO
