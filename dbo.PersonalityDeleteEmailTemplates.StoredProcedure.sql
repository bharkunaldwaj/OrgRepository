USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityDeleteEmailTemplates]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityDeleteEmailTemplates]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityDeleteEmailTemplates]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityDeleteEmailTemplates]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
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
		--IF Exists (Select * from PersonalityEmailTemplates where AccountID=@AccountID and UniqueID in (select Items from TblUfSplit(@UniqueIDs,'','')))
		--Begin
			delete from PersonalityEmailTemplates where AccountID=@AccountID and UniqueID in (select Items from TblUfSplit(@UniqueIDs,'',''))
		--End
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
