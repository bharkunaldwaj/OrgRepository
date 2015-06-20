USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetAccountLogosByAccountID]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetAccountLogosByAccountID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityGetAccountLogosByAccountID]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetAccountLogosByAccountID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[PersonalityGetAccountLogosByAccountID] 
	@AccountID INT
	
AS
BEGIN
	
	SET NOCOUNT ON;
    select acc.AccountID,acc.CompanyLogo,acc.RightLogoName  from Account acc where AccountID=@AccountID
END


' 
END
GO
