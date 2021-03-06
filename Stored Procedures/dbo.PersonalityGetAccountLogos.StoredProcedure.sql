USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetAccountLogos]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetAccountLogos]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PersonalityGetAccountLogos] 
	@QuestionnaireID uniqueidentifier
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
    select acc.AccountID,acc.CompanyLogo,acc.RightLogoName  from Account acc where AccountID=(Select AccountID from PersonalityQuestionnaires where UniqueID=@QuestionnaireID)
END
GO
