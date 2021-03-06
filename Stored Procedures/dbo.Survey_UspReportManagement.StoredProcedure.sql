USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspReportManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspReportManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:  <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
create PROCEDURE [dbo].[Survey_UspReportManagement]
 @ReportManagementID int,
 @AccountID int,
 @ProjectID int,
    @ProgramID int,
--    @TargetPersonID int,
    @ReportName varchar(50),
    @Admin char(2),
    @Operation char(2)
AS

-- Insert 

IF (@Operation = 'I')

BEGIN

--delete [ReportManagement] where [TargetPersonID]=@TargetPersonID

INSERT INTO [Survey_ReportManagement]
           ([AccountID]
           ,[ProjectID]
           ,[ProgramID]
  --,[TargetPersonID]
           ,[ReportName]
           ,[DateCreated])
     VALUES
           (@AccountID
   ,@ProjectID
   ,@ProgramID
 --  ,@TargetPersonID
   ,@ReportName
   ,GETDATE())

End

-- Update

ELSE IF (@Operation = 'U')

BEGIN

UPDATE .[Survey_ReportManagement]

   SET [AccountID] = @AccountID
      ,[ProjectID] = @ProjectID
      ,[ProgramID] = @ProgramID
      --,[TargetPersonID] = @TargetPersonID
      ,[ReportName] = @ReportName

 WHERE ReportManagementID = @ReportManagementID

END

-- Delete

ELSE IF (@Operation = 'D')

BEGIN

DELETE FROM [Survey_ReportManagement] WHERE ReportManagementID = @ReportManagementID

END


ELSE IF (@Operation = 'P')

BEGIN

SELECT [Survey_Analysis_Sheet].[ProgrammeID]
    ,[Survey_Analysis_Sheet].[ProgrammeName]
    ,[Survey_Analysis_Sheet].[ProgrammeDescription]
    ,[Survey_Analysis_Sheet].[ProjectID]
    ,[Survey_Analysis_Sheet].[AccountID]
    ,[Survey_Analysis_Sheet].[StartDate]
    ,[Survey_Analysis_Sheet].[EndDate]
    ,[Survey_Analysis_Sheet].[Reminder1Date]
    ,[Survey_Analysis_Sheet].[Reminder2Date]
    ,[Survey_Analysis_Sheet].[Reminder3Date]
    
    ,[Survey_Analysis_Sheet].[ModifyBy]
    ,[Survey_Analysis_Sheet].[ModifyDate]
    ,[Survey_Analysis_Sheet].[IsActive]
  
 FROM [Survey_Analysis_Sheet]
 WHERE [ProjectID] = @ProjectID 
 
END

ELSE IF (@Operation = 'UL')

BEGIN
 if (@Admin = 'Y')
  Begin
   SELECT dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS CandidateName
   FROM dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
            dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
            dbo.Survey_Project ON dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.Survey_AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Survey_Questionnaire ON dbo.Survey_Project.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
            dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
   WHERE (dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
  
   --select u.UserID, u.FirstName +' '+ u.LastName as CandidateName
   --from Programme p
   --join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
   --join [User] u on u.UserID = aq.TargetPersonID  
   --where p.ProjectID = @ProjectID and p.ProgrammeID = @ProgramID
  end
 else
  Begin
   SELECT dbo.[User].UserID, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS CandidateName
   FROM dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
            dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
            dbo.Survey_Project ON dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.Survey_AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Survey_Questionnaire ON dbo.Survey_Project.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
            dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
   WHERE 
   --dbo.Programme.ReportAvaliableFrom <= getdate() and 
   --dbo.Programme.ReportAvaliableTo >= getdate() and 
   (dbo.Survey_AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
  
   --select u.UserID, u.FirstName +' '+ u.LastName as CandidateName
   --from Programme p
   --join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
   --join [User] u on u.UserID = aq.TargetPersonID  
   --where p.ReportAvaliableFrom <= getdate() and p.ReportAvaliableTo >= getdate() 
   --and p.ProjectID = @ProjectID and p.AccountID = @AccountID and p.ProgrammeID = @ProgramID
  End  
END

ELSE IF (@Operation = 'R')

BEGIN
  select p.ProgrammeName, pj.Title, a.Code, u.FirstName +' '+ u.LastName as CandidateName
  from Survey_ReportManagement rm
  join Survey_Analysis_Sheet p on p.ProgrammeID = rm.ProgramID
  join Survey_Project pj on pj.ProjectID = rm.ProjectID
  join Account a on a.AccountID = rm.AccountID
  join [User] u on a.AccountID =u.UserID
  
END
ELSE IF (@Operation = 'C') -- Get Record Count

Begin
 if (@Admin = 'Y')
  Begin
   SELECT Count(*)
   
   FROM dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
            dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
            dbo.Survey_Project ON dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.Survey_AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Survey_Questionnaire ON dbo.Survey_Project.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
            dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
   WHERE (dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
   --select Count(*)
   --from Programme p
   --join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
   --join [User] u on u.UserID = aq.TargetPersonID  
   --where p.ProjectID = @ProjectID and p.ProgrammeID = @ProgramID
  End
 Else
  Begin
   SELECT Count(*)
   
   FROM dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
            dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
            dbo.Survey_Project ON dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
            dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
            dbo.Account ON dbo.Survey_AssignQuestionnaireParticipant.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Survey_Questionnaire ON dbo.Survey_Project.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
            dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
   WHERE 
   --dbo.Programme.ReportAvaliableFrom <= getdate() and 
   --dbo.Programme.ReportAvaliableTo >= getdate() and 
   (dbo.Survey_AssignQuestionnaireParticipant.AccountID = @AccountID) AND (dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = @ProgramID)
  
   --select Count(*)
   --from Programme p
   --join AssignQuestionnaire aq on aq.ProgrammeID = p.ProgrammeID
   --join [User] u on u.UserID = aq.TargetPersonID  
   --where p.ReportAvaliableFrom <= getdate() and p.ReportAvaliableTo >= getdate() 
   --and p.ProjectID = @ProjectID and p.AccountID = @AccountID and p.ProgrammeID = @ProgramID
  End 
End


ELSE IF (@Operation = 'RI')

BEGIN 
  select rm.*, p.ProgrammeName, pj.Title, a.Code, u.FirstName +' '+ u.LastName as CandidateName
  from Survey_ReportManagement rm
  join Survey_Analysis_Sheet p on p.ProgrammeID = rm.ProgramID
  join Survey_Project pj on pj.ProjectID = rm.ProjectID
  join Account a on a.AccountID = rm.AccountID
  join [User] u on u.UserID = a.AccountID  
  where ReportManagementID = @ReportManagementID
 
END

ELSE IF (@Operation = 'T')

BEGIN 
  select * from Survey_ReportManagement 

END
GO
