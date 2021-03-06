USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityAssignTemplatesFromAccount]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityAssignTemplatesFromAccount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Vandana Sukhija>
-- Create date: <01/16/2012 10:59:20>
-- Description:	<List PersonalityAssignTemplates for selected Account ID>
-- =============================================
CREATE PROCEDURE [dbo].[PersonalityAssignTemplatesFromAccount] --'7429862d-0e40-e111-9fdb-d48654e0030h', 2
	@UniqueIDs nvarchar(max),
	@AccountID int
	
AS
BEGIN
	BEGIN TRAN
		
		SET NOCOUNT ON;

		IF not exists (Select * from PersonalityEmailTemplates where AccountID=@AccountID and 
						UniqueID in (select Items from TblUfSplit(@UniqueIDs,',')))
		Begin
			
			INSERT INTO  PersonalityEmailTemplates
					(UniqueID,CreatedDate,CreatedBy,Title,Description,AccountID,Subject,EmailText,EmailImage,IsActive)
					(Select NEWID(), GETDATE(), NEWID(),Title,Description,@AccountID,Subject,EmailText,EmailImage,IsActive 
							from PersonalityEmailTemplates where UniqueID in (select Items from TblUfSplit(@UniqueIDs,',')) )
							
		End
		----Else
		----Begin
		--	--If (LEN(@RemoveUniqueIDs)>0)
		--	--Begin
		--	IF Exists (Select * from PersonalityEmailTemplates where AccountID=@AccountID and UniqueID in (select Items from TblUfSplit(@RemoveUniqueIDs,',')))
		--	Begin
		--			delete from PersonalityEmailTemplates where AccountID=@AccountID and UniqueID in (select Items from TblUfSplit(@RemoveUniqueIDs,','))
		--		--End
		--	End

		----End
		
		
				
		
IF @@ERROR <> 0
   BEGIN
	ROLLBACK TRAN
   END
ELSE
	COMMIT TRAN

END
GO
