USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAssignmentDetailsManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAssignmentDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspAssignmentDetailsManagement]

@AssignmentID int
,@Analysis_I varchar(50)
,@Analysis_II varchar(50)
,@Analysis_III varchar(50)
,@CandidateName varchar(50)
,@CandidateEmail varchar(100)
,@SubmitFlag bit
,@EmailSendFlag int
,@Operation char(1)

as

-- Insert

IF (@Operation = 'I')

BEGIN

		DECLARE @AccountID INT
		DECLARE @ProjecctID INT
		DECLARE @QuestionnaireID INT
		
		DECLARE @ParticipantCount INT

		--GET VALUES FROM ASSIGN QUESTIONNAIRE TABLE FOR TARGET PERSON
		SELECT	@AccountID=AccountID,
				@ProjecctID=ProjecctID,
				@QuestionnaireID=QuestionnaireID
		
		FROM dbo.Survey_AssignQuestionnaire
		WHERE AssignmentID=@AssignmentID

		--CHECK PREVIOUS ENTRY FOR ABOVE TARGET PERSON, PROJECT AND QUESTIONNAIRE
--		select	@ParticipantCount=count(*) 
--		from	dbo.AssignQuestionnaire 
--		where	AccountID=@AccountID AND
--				ProjecctID=@ProjecctID AND
--				QuestionnaireID=@QuestionnaireID AND
--				TargetPersonID=@TargetPersonID 

		select	@ParticipantCount=count(*) 
		from	dbo.Survey_AssignmentDetails 
		where	AssignmentID=@AssignmentID

		--print @ParticipantCount
		
		IF (@ParticipantCount < 1)
			BEGIN
				INSERT INTO [Survey_AssignmentDetails]
				   ([AssignmentID]
				   ,[Analysis_I]
				   ,[Analysis_II]
				   ,[Analysis_III]
				   ,[CandidateName]
				   ,[CandidateEmail]
				   ,[SubmitFlag]
				   ,[EmailSendFlag])
				VALUES
				   (@AssignmentID 
				   ,@Analysis_I
				   ,@Analysis_II
				   ,@Analysis_III 
				   ,@CandidateName 
				   ,@CandidateEmail
				   ,@SubmitFlag
				   ,@EmailSendFlag) 
			END

ELSE
	BEGIN
		INSERT INTO [Survey_AssignmentDetails]
				   ([AssignmentID]
				   ,[Analysis_I]
				   ,[Analysis_II]
				   ,[Analysis_III]
				   ,[CandidateName]
				   ,[CandidateEmail]
				   ,[SubmitFlag]
				   ,[EmailSendFlag])
			 VALUES
				   (@AssignmentID 
				   ,@Analysis_I
				   ,@Analysis_II
				   ,@Analysis_III  
				   ,@CandidateName 
				   ,@CandidateEmail
				   ,@SubmitFlag
				   ,@EmailSendFlag) 
	END

END

-- Update

ELSE IF (@Operation = 'U')

Begin

DELETE FROM [Survey_AssignQuestionnaire] WHERE [AssignmentID] = @AssignmentID

INSERT INTO [Survey_AssignmentDetails]
           ([AssignmentID]
           ,[Analysis_I]
				   ,[Analysis_II]
				   ,[Analysis_III]
           ,[CandidateName]
		   ,[CandidateEmail]
		   ,[SubmitFlag]
		   ,[EmailSendFlag])
     VALUES
           (@AssignmentID 
           ,@Analysis_I
				   ,@Analysis_II
				   ,@Analysis_III 
           ,@CandidateName 
		   ,@CandidateEmail
		   ,@SubmitFlag
		   ,@EmailSendFlag) 

End

-- Delete

ELSE IF (@Operation = 'D')

Begin

DELETE FROM [Survey_AssignQuestionnaire]
 WHERE [AssignmentID] = @AssignmentID

End
GO
