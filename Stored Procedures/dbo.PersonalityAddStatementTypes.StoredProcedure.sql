USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityAddStatementTypes]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityAddStatementTypes]
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
CREATE PROCEDURE [dbo].[PersonalityAddStatementTypes]
	-- Add the parameters for the stored procedure here
	@StatementID UniqueIdentifier,
	@TypeIds Varchar(Max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    Insert into PersonalityStatementTypesAssociation (StatementID,TypeID)
    (SELECT @StatementID,Items FROM dbo.TblUfSplit(@TypeIds,','))
    
	 
END
GO
