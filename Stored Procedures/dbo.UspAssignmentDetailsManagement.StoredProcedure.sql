USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignmentDetailsManagement]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignmentDetailsManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAssignmentDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignmentDetailsManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspAssignmentDetailsManagement]

@AssignmentID int
,@RelationShip varchar(50)
,@CandidateName varchar(50)
,@CandidateEmail varchar(100)
,@SubmitFlag bit
,@EmailSendFlag int
,@Operation char(1)

as

-- Insert

IF (@Operation = ''I'')

Begin

IF (@RelationShip = ''Self'')
	BEGIN
		DECLARE @AccountID INT
		DECLARE @ProjecctID INT
		DECLARE @QuestionnaireID INT
		DECLARE @TargetPersonID INT
		DECLARE @ParticipantCount INT

		--GET VALUES FROM ASSIGN QUESTIONNAIRE TABLE FOR TARGET PERSON
		SELECT	@AccountID=AccountID,
				@ProjecctID=ProjecctID,
				@QuestionnaireID=QuestionnaireID,
				@TargetPersonID=TargetPersonID 
		FROM dbo.AssignQuestionnaire
		WHERE AssignmentID=@AssignmentID

		--CHECK PREVIOUS ENTRY FOR ABOVE TARGET PERSON, PROJECT AND QUESTIONNAIRE
--		select	@ParticipantCount=count(*) 
--		from	dbo.AssignQuestionnaire 
--		where	AccountID=@AccountID AND
--				ProjecctID=@ProjecctID AND
--				QuestionnaireID=@QuestionnaireID AND
--				TargetPersonID=@TargetPersonID 

		select	@ParticipantCount=count(*) 
		from	dbo.AssignmentDetails 
		where	AssignmentID=@AssignmentID and [RelationShip]=''Self''

		--print @ParticipantCount
		
		IF (@ParticipantCount < 1)
			BEGIN
				INSERT INTO [AssignmentDetails]
				   ([AssignmentID]
				   ,[RelationShip]
				   ,[CandidateName]
				   ,[CandidateEmail]
				   ,[SubmitFlag]
				   ,[EmailSendFlag])
				VALUES
				   (@AssignmentID 
				   ,@RelationShip 
				   ,@CandidateName 
				   ,@CandidateEmail
				   ,@SubmitFlag
				   ,@EmailSendFlag) 
			END
	END
ELSE
	BEGIN
		INSERT INTO [AssignmentDetails]
				   ([AssignmentID]
				   ,[RelationShip]
				   ,[CandidateName]
				   ,[CandidateEmail]
				   ,[SubmitFlag]
				   ,[EmailSendFlag])
			 VALUES
				   (@AssignmentID 
				   ,@RelationShip 
				   ,@CandidateName 
				   ,@CandidateEmail
				   ,@SubmitFlag
				   ,@EmailSendFlag) 
	END

End

-- Update

ELSE IF (@Operation = ''U'')

Begin

DELETE FROM [AssignQuestionnaire] WHERE [AssignmentID] = @AssignmentID

INSERT INTO [AssignmentDetails]
           ([AssignmentID]
           ,[RelationShip]
           ,[CandidateName]
		   ,[CandidateEmail]
		   ,[SubmitFlag]
		   ,[EmailSendFlag])
     VALUES
           (@AssignmentID 
           ,@RelationShip 
           ,@CandidateName 
		   ,@CandidateEmail
		   ,@SubmitFlag
		   ,@EmailSendFlag) 

End

-- Delete

ELSE IF (@Operation = ''D'')

Begin

DELETE FROM [AssignQuestionnaire]
 WHERE [AssignmentID] = @AssignmentID

End
' 
END
GO
