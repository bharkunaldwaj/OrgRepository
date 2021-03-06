USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAssignQstnParticipantManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAssignQstnParticipantManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspAssignQstnParticipantManagement]

@AssignmentID int
,@AccountID int
,@ProjecctID int
,@ProgrammeID int
,@QuestionnaireID int
,@Description varchar(1000)
,@CandidateNo int
,@ModifiedBy int
,@ModifiedDate datetime
,@IsActive int
,@Operation char(1)

as

-- Insert

IF (@Operation = 'I')

Begin

INSERT INTO [Survey_AssignQuestionnaireParticipant]
           ([ProjecctID]
           ,[AccountID]
		   ,[ProgrammeID]
           ,[QuestionnaireID]
           ,[Description]
           ,[CandidateNo]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[IsActive])
     VALUES
			(@ProjecctID
			,@AccountID
			,@ProgrammeID
			,@QuestionnaireID
			,@Description
			,@CandidateNo 
			,@ModifiedBy 
			,@ModifiedDate 
			,@IsActive)

SELECT ISNULL(MAX([AssignmentID]),1)
  FROM [Survey_AssignQuestionnaireParticipant]

End

-- Update

ELSE IF (@Operation = 'U')

Begin

UPDATE [Survey_AssignQuestionnaireParticipant]
   SET [ProjecctID] = @ProjecctID
      ,[QuestionnaireID] = @QuestionnaireID
      ,[Description] = @Description
      ,[IsActive] = @IsActive
 WHERE [AssignmentID] = @AssignmentID

End

-- Delete

ELSE IF (@Operation = 'D')

Begin

DELETE FROM [Survey_AssignQuestionnaireParticipant]
WHERE [AssignmentID] = @AssignmentID

End
GO
