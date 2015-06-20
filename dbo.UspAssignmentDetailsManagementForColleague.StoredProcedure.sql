USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignmentDetailsManagementForColleague]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignmentDetailsManagementForColleague]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAssignmentDetailsManagementForColleague]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignmentDetailsManagementForColleague]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE proc [dbo].[UspAssignmentDetailsManagementForColleague]

@AssignmentID int
,@RelationShip varchar(50)
,@CandidateName varchar(50)
,@CandidateEmail varchar(100)
,@SubmitFlag bit
,@EmailSendFlag int

as

-- Insert


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
				   
				   update [AssignQuestionnaire] SET [CandidateNo]=CASE WHEN ISNULL([CandidateNo],0) <=0 THEN 1 WHEN [CandidateNo]>0 THEN [CandidateNo]+1  END 
				   WHERE AssignmentID=@AssignmentID
				   
				   SELECT IDENT_CURRENT(''AssignmentDetails'')
			END
			ELSE 
			BEGIN
				select	AsgnDetailID
				from	dbo.AssignmentDetails 
				where	AssignmentID=@AssignmentID and [RelationShip]=''Self''
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
				   
				   
				   update [AssignQuestionnaire] SET [CandidateNo]=CASE WHEN ISNULL([CandidateNo],0) <=0 THEN 1 WHEN [CandidateNo]>0 THEN [CandidateNo]+1 END 
				   WHERE AssignmentID=@AssignmentID
				   
				   
				   SELECT IDENT_CURRENT(''AssignmentDetails'')
	END


' 
END
GO
