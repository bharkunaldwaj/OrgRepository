USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetEmailTemplateDetails]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetEmailTemplateDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
GO
