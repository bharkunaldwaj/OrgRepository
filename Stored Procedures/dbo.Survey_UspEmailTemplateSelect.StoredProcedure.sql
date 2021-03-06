USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspEmailTemplateSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspEmailTemplateSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Survey_UspEmailTemplateSelect]
(
	@EmailTemplateID	INT,
	@AccountID			INT,
	@SelectFlag			CHAR(1)
)
AS
BEGIN

IF (@AccountID != 2)

	BEGIN

	IF (@SelectFlag = 'I') -- Id based
	BEGIN
		SELECT	[EmailTemplateID]
		        ,[AccountID]
				,[Title]
				,[Description]
				,[Subject]
				,[EmailText]
				,[EmailImage]
				,[ModifyBy]
				,[ModifyDate]
				,[IsActive]
		FROM	[Survey_EmailTemplate]
		WHERE	[EmailTemplateID]=@EmailTemplateID
	END

	ELSE IF (@SelectFlag = 'A') -- All Records
	BEGIN
		SELECT   dbo.Survey_EmailTemplate.EmailTemplateID, dbo.Survey_EmailTemplate.AccountID, [Title] , CASE WHEN LEN(Survey_EmailTemplate.[Description]) > 40 THEN SUBSTRING(Survey_EmailTemplate.[Description], 1, 40) 
							  + '...' ELSE Survey_EmailTemplate.[Description] END AS Description, CASE WHEN LEN([EmailText]) > 70 THEN SUBSTRING([EmailText], 1, 70) 
							  + '...' ELSE [EmailText] END AS EmailText, dbo.Survey_EmailTemplate.ModifyBy, dbo.Survey_EmailTemplate.ModifyDate, dbo.Survey_EmailTemplate.IsActive, 
							  dbo.Account.[Code],dbo.Survey_EmailTemplate.[Subject] 
		FROM         dbo.Survey_EmailTemplate INNER JOIN
							  dbo.Account ON dbo.Survey_EmailTemplate.AccountID = dbo.Account.AccountID
		Where	dbo.Survey_EmailTemplate.AccountID=@AccountID
		ORDER BY dbo.Account.Code, dbo.Survey_EmailTemplate.EmailTemplateID DESC

	END

	ELSE IF (@SelectFlag = 'C') -- Get Total Number of Records Count
	BEGIN
		SELECT COUNT(*) FROM [Survey_EmailTemplate] WHERE IsActive=1 and AccountID=@AccountID
	END
	
	ELSE IF (@SelectFlag = 'F') -- All Records
	BEGIN
		SELECT	[EmailTemplateID]
		        ,[AccountID]
				/*******Word Wrapping of Title, Description and Email Text in case its length exceeds*******/
				,[Title]
				,CASE WHEN LEN([Description])>40	THEN SUBSTRING([Description],1,40)+'...'	ELSE [Description]	END [Description]
				,CASE WHEN LEN([EmailText])>70		THEN SUBSTRING([EmailText],1,70)+'...'		ELSE [EmailText]	END [EmailText]
				,[Subject]				
				,[ModifyBy]
				,[ModifyDate]
				,[IsActive]
		FROM	[Survey_EmailTemplate] 
		Where	dbo.Survey_EmailTemplate.AccountID=@AccountID
	END
END

ELSE

BEGIN

IF (@SelectFlag = 'I') -- Id based
	BEGIN
		SELECT	[EmailTemplateID]
		        ,[AccountID]
				,[Title]
				,[Description]
				,[Subject]
				,[EmailText]
				,[EmailImage]
				,[ModifyBy]
				,[ModifyDate]
				,[IsActive]
		FROM	[Survey_EmailTemplate]
		WHERE	[EmailTemplateID]=@EmailTemplateID
	END

	ELSE IF (@SelectFlag = 'A') -- All Records
	BEGIN
		SELECT   dbo.Survey_EmailTemplate.EmailTemplateID, dbo.Survey_EmailTemplate.AccountID, [Title], CASE WHEN LEN(Survey_EmailTemplate.[Description]) > 40 THEN SUBSTRING(Survey_EmailTemplate.[Description], 1, 40) 
							  + '...' ELSE Survey_EmailTemplate.[Description] END AS Description, CASE WHEN LEN([EmailText]) > 70 THEN SUBSTRING([EmailText], 1, 70) 
							  + '...' ELSE [EmailText] END AS EmailText, dbo.Survey_EmailTemplate.ModifyBy, dbo.Survey_EmailTemplate.ModifyDate, dbo.Survey_EmailTemplate.IsActive, 
							  dbo.Account.[Code] ,dbo.Survey_EmailTemplate.[Subject]
		FROM         dbo.Survey_EmailTemplate INNER JOIN
							  dbo.Account ON dbo.Survey_EmailTemplate.AccountID = dbo.Account.AccountID
		where	dbo.Survey_EmailTemplate.AccountID=@AccountID
		ORDER BY dbo.Account.Code, dbo.Survey_EmailTemplate.EmailTemplateID DESC

	END

	ELSE IF (@SelectFlag = 'C') -- Get Total Number of Records Count
	BEGIN
		SELECT COUNT(*) FROM [Survey_EmailTemplate] WHERE IsActive=1 and dbo.Survey_EmailTemplate.AccountID=@AccountID
	END
	
	ELSE IF (@SelectFlag = 'F') -- All Records
	BEGIN
		SELECT	[EmailTemplateID]
		        ,[AccountID]
				/*******Word Wrapping of Title, Description and Email Text in case its length exceeds*******/
				,[Title]
				,CASE WHEN LEN([Description])>40	THEN SUBSTRING([Description],1,40)+'...'	ELSE [Description]	END [Description]
				,CASE WHEN LEN([EmailText])>70		THEN SUBSTRING([EmailText],1,70)+'...'		ELSE [EmailText]	END [EmailText]
				,[Subject]				
				,[ModifyBy]
				,[ModifyDate]
				,[IsActive]
		FROM	[Survey_EmailTemplate] 
		Where	dbo.Survey_EmailTemplate.AccountID=@AccountID
	END
END

END
GO
