USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAssignQuestionnaireManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAssignQuestionnaireManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_UspAssignQuestionnaireManagement]

@AssignmentID int
,@AccountID int
,@ProjecctID int
,@ProgrammeID int
,@QuestionnaireID int

,@FeedbackURL varchar(250)
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

	declare @RecCount int

	SELECT @RecCount=COUNT(*) 
	FROM [Survey_AssignQuestionnaire]
	WHERE 
	[ProjecctID]=@ProjecctID
	and [ProgrammeID]=@ProgrammeID
	and [AccountID]=@AccountID
	and [QuestionnaireID]=@QuestionnaireID
	

	IF (@RecCount = 0)
		BEGIN

			INSERT INTO [Survey_AssignQuestionnaire]
					   ([ProjecctID]
						,[ProgrammeID]
					   ,[AccountID]
					   ,[QuestionnaireID]
					   
						--,[FeedbackURL]
					   ,[Description]
					   ,[CandidateNo]
					   ,[ModifiedBy]
					   ,[ModifiedDate]
					   ,[IsActive])
				 VALUES
						(@ProjecctID
						,@ProgrammeID
						,@AccountID
						,@QuestionnaireID
					
						--,@FeedbackURL
						,@Description
						,@CandidateNo 
						,@ModifiedBy 
						,@ModifiedDate 
						,@IsActive)

			SELECT ISNULL(MAX([AssignmentID]),1)
			  FROM [Survey_AssignQuestionnaire]

		End
	ELSE
		BEGIN

			SELECT [AssignmentID] 
			FROM [Survey_AssignQuestionnaire]
			WHERE 
			[ProjecctID]=@ProjecctID
			and [ProgrammeID]=@ProgrammeID
			and [AccountID]=@AccountID
			and [QuestionnaireID]=@QuestionnaireID
			

		End

END

-- Update

ELSE IF (@Operation = 'U')

Begin

UPDATE [Survey_AssignQuestionnaire]
   SET [ProjecctID] = @ProjecctID
      ,[QuestionnaireID] = @QuestionnaireID
      
      ,[Description] = @Description
      ,[IsActive] = @IsActive
 WHERE [AssignmentID] = @AssignmentID

End

-- Delete

ELSE IF (@Operation = 'D')

Begin

DELETE FROM [Survey_AssignmentDetails]
WHERE [AsgnDetailID] = @AssignmentID

DELETE from Survey_QuestionAnswer  
where AssignDetId = @AssignmentID

UPDATE [Survey_AssignQuestionnaire]
Set CandidateNo=CandidateNo-1
WHERE [AssignmentID] = @AssignmentID

End
GO
