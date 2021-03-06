USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityDeleteEmailTemplates]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityDeleteEmailTemplates]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Raj>
-- Create date: <01/17/2012 10:59:20>
-- Description:	<Delete the records on basis of Accout id and Emailtemplate ID>
-- =============================================
CREATE PROCEDURE [dbo].[PersonalityDeleteEmailTemplates]
	@UniqueIDs nvarchar(max),
	@AccountID int
	
AS
BEGIN
	BEGIN TRAN
		SET NOCOUNT ON;
		--IF Exists (Select * from PersonalityEmailTemplates where AccountID=@AccountID and UniqueID in (select Items from TblUfSplit(@UniqueIDs,',')))
		--Begin
			delete from PersonalityEmailTemplates where AccountID=@AccountID and UniqueID in (select Items from TblUfSplit(@UniqueIDs,','))
		--End
IF @@ERROR <> 0
   BEGIN
	ROLLBACK TRAN
   END
ELSE
	COMMIT TRAN

END
GO
