USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspUpdateAssignProgramme]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspUpdateAssignProgramme]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspUpdateAssignProgramme]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspUpdateAssignProgramme]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE Proc [dbo].[UspUpdateAssignProgramme]

@AccountID int
,@ProgrammeID int
,@NewProgrammeID int
,@TargetPersonId int

as

-- Update programme with new programme id

declare @AssignmentID int

select @AssignmentID=isnull(max(AssignmentID),0) from AssignQuestionnaireParticipant
where AccountID=@AccountID and ProgrammeID=@NewProgrammeID

IF (@AssignmentID != 0)

	BEGIN

		UPDATE [PaticipantDetails]
		   SET [AssignmentID] = @AssignmentID     
		 WHERE [UserID]=@TargetPersonId
	 
	END

ELSE

	BEGIN

		declare @MaxAsgnId int

		Insert into AssignQuestionnaireParticipant
		SELECT    dbo.AssignQuestionnaireParticipant.AccountID, dbo.AssignQuestionnaireParticipant.ProjecctID, @NewProgrammeID, 
				  dbo.AssignQuestionnaireParticipant.QuestionnaireID,'''',0, dbo.AssignQuestionnaireParticipant.ModifiedBy,GETDATE(), dbo.AssignQuestionnaireParticipant.IsActive
		FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
							  dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID
		WHERE     (dbo.PaticipantDetails.UserID = @TargetPersonId)

		select @MaxAsgnId=max(AssignmentID) from AssignQuestionnaireParticipant 

		UPDATE [PaticipantDetails]
		   SET [AssignmentID] = @MaxAsgnId     
		 WHERE [UserID]=@TargetPersonId

	END
 
 -- Update Participant existing score for new programme

UPDATE [ParticipantScore]
SET [ProgrammeID] = @NewProgrammeID
WHERE [AccountID] = @AccountID
and [ProgrammeID] = @ProgrammeID
and [TargetPersonID] = @TargetPersonId

 
-- Update data in AssignQuestionnaire Table(s)

UPDATE [assignquestionnaire]
SET [ProgrammeID] = @NewProgrammeID      
WHERE [TargetPersonID]=@TargetPersonId
' 
END
GO
