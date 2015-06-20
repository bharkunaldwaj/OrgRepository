USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityAssignQuestionnaireFromAccount]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityAssignQuestionnaireFromAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityAssignQuestionnaireFromAccount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityAssignQuestionnaireFromAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<>
-- Create date: <01/16/2012 10:59:20>
-- Description:	<>
-- =============================================
Create PROCEDURE [dbo].[PersonalityAssignQuestionnaireFromAccount] 
	@UniqueIDs nvarchar(max),
	@AccountID int
	
AS
BEGIN
	BEGIN TRAN
		Declare @NewUniqueQuestionnairesID as uniqueidentifier
		SET NOCOUNT ON;

		IF not exists (Select * from PersonalityQuestionnaires where AccountID=@AccountID and 
						UniqueID in (select Items from TblUfSplit(@UniqueIDs,'','')))
		Begin
			
			set @NewUniqueQuestionnairesID=NEWID()
			INSERT INTO  PersonalityQuestionnaires
					(           
					   UniqueID,
					   CreatedDate,
					   CreatedBy,
					   AccountID,
					   Type,
					   Code,
					   Name,
					   Description,
					   NoOfQuestions,
					   Prologue,
					   Epilogue,
					   IsActive

					)
					(Select 
						@NewUniqueQuestionnairesID, 
						GETDATE(), 
						NEWID(),
						@AccountID,
						Type,
						Code,
						Name,
						Description,
						NoOfQuestions,
						Prologue,
						Epilogue,
						IsActive
					from PersonalityQuestionnaires where UniqueID in (select Items from TblUfSplit(@UniqueIDs,'','')) )
					
					
							
		End
IF @@ERROR <> 0
   BEGIN
	ROLLBACK TRAN
   END
ELSE
	COMMIT TRAN

END
' 
END
GO
