USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignQuestionnaireManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignQuestionnaireManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAssignQuestionnaireManagement]

@AssignmentID int
,@AccountID int
,@ProjecctID int
,@ProgrammeID int
,@QuestionnaireID int
,@TargetPersonID int
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
	FROM [AssignQuestionnaire]
	WHERE 
	[ProjecctID]=@ProjecctID
	and [ProgrammeID]=@ProgrammeID
	and [AccountID]=@AccountID
	and [QuestionnaireID]=@QuestionnaireID
	and [TargetPersonID]=@TargetPersonID

	IF (@RecCount = 0)
		BEGIN

			INSERT INTO [AssignQuestionnaire]
					   ([ProjecctID]
						,[ProgrammeID]
					   ,[AccountID]
					   ,[QuestionnaireID]
					   ,[TargetPersonID]
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
						,@TargetPersonID
						--,@FeedbackURL
						,@Description
						,@CandidateNo 
						,@ModifiedBy 
						,@ModifiedDate 
						,@IsActive)

			SELECT ISNULL(MAX([AssignmentID]),1)
			  FROM [AssignQuestionnaire]

		End
	ELSE
		BEGIN

			SELECT [AssignmentID] 
			FROM [AssignQuestionnaire]
			WHERE 
			[ProjecctID]=@ProjecctID
			and [ProgrammeID]=@ProgrammeID
			and [AccountID]=@AccountID
			and [QuestionnaireID]=@QuestionnaireID
			and [TargetPersonID]=@TargetPersonID

		End

END

-- Update

ELSE IF (@Operation = 'U')

Begin

UPDATE [AssignQuestionnaire]
   SET [ProjecctID] = @ProjecctID
      ,[QuestionnaireID] = @QuestionnaireID
      ,[TargetPersonID] = @TargetPersonID
      ,[Description] = @Description
      ,[IsActive] = @IsActive
 WHERE [AssignmentID] = @AssignmentID

End

-- Delete

ELSE IF (@Operation = 'D')

Begin

DELETE FROM [AssignmentDetails]
WHERE [AsgnDetailID] = @AssignmentID

DELETE from QuestionAnswer  
where AssignDetId = @AssignmentID

UPDATE [AssignQuestionnaire]
Set CandidateNo=CandidateNo-1
WHERE [AssignmentID] = @AssignmentID

End
GO
