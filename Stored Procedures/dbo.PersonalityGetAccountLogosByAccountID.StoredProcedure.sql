USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetAccountLogosByAccountID]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetAccountLogosByAccountID]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PersonalityGetAccountLogosByAccountID] 
	@AccountID INT
	
AS
BEGIN
	
	SET NOCOUNT ON;
    select acc.AccountID,acc.CompanyLogo,acc.RightLogoName  from Account acc where AccountID=@AccountID
END
GO
