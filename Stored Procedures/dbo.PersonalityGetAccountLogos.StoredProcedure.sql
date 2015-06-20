USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetAccountLogos]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetAccountLogos]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityGetAccountLogos]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetAccountLogos]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[PersonalityGetAccountLogos] 
	@QuestionnaireID uniqueidentifier
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
    select acc.AccountID,acc.CompanyLogo,acc.RightLogoName  from Account acc where AccountID=(Select AccountID from PersonalityQuestionnaires where UniqueID=@QuestionnaireID)
END


' 
END
GO
