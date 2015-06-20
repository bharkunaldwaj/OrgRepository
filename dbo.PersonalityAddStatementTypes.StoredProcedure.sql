USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityAddStatementTypes]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityAddStatementTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityAddStatementTypes]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityAddStatementTypes]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
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
    (SELECT @StatementID,Items FROM dbo.TblUfSplit(@TypeIds,'',''))
    
	 
END
' 
END
GO
