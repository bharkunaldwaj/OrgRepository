USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetEmailTemplateDetails]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetEmailTemplateDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityGetEmailTemplateDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetEmailTemplateDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PersonalityGetEmailTemplateDetails] 
	-- Add the parameters for the stored procedure here
	@AccountID UniqueIdentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT pet.Description,pet.Subject,pet.EmailText from PersonalityEmailTemplates pet where pet.UniqueID = @AccountID-- and pet.AccountID = @AccountID
END
' 
END
GO
