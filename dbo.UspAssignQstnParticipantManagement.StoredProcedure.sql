USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignQstnParticipantManagement]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignQstnParticipantManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAssignQstnParticipantManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignQstnParticipantManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspAssignQstnParticipantManagement]

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

IF (@Operation = ''I'')

Begin

INSERT INTO [AssignQuestionnaireParticipant]
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
  FROM [AssignQuestionnaireParticipant]

End

-- Update

ELSE IF (@Operation = ''U'')

Begin

UPDATE [AssignQuestionnaireParticipant]
   SET [ProjecctID] = @ProjecctID
      ,[QuestionnaireID] = @QuestionnaireID
      ,[Description] = @Description
      ,[IsActive] = @IsActive
 WHERE [AssignmentID] = @AssignmentID

End

-- Delete

ELSE IF (@Operation = ''D'')

Begin

DELETE FROM [AssignQuestionnaireParticipant]
WHERE [AssignmentID] = @AssignmentID

End
' 
END
GO
