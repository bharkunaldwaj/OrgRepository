USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspEmailTemplateSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspEmailTemplateSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspEmailTemplateSelect]
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
		FROM	[EmailTemplate]
		WHERE	[EmailTemplateID]=@EmailTemplateID
	END

	ELSE IF (@SelectFlag = 'A') -- All Records
	BEGIN
		SELECT   dbo.EmailTemplate.EmailTemplateID, dbo.EmailTemplate.AccountID, [Title] , CASE WHEN LEN(EmailTemplate.[Description]) > 40 THEN SUBSTRING(EmailTemplate.[Description], 1, 40) 
							  + '...' ELSE EmailTemplate.[Description] END AS Description, CASE WHEN LEN([EmailText]) > 70 THEN SUBSTRING([EmailText], 1, 70) 
							  + '...' ELSE [EmailText] END AS EmailText, dbo.EmailTemplate.ModifyBy, dbo.EmailTemplate.ModifyDate, dbo.EmailTemplate.IsActive, 
							  dbo.Account.[Code],dbo.EmailTemplate.[Subject] 
		FROM         dbo.EmailTemplate INNER JOIN
							  dbo.Account ON dbo.EmailTemplate.AccountID = dbo.Account.AccountID
		Where	dbo.EmailTemplate.AccountID=@AccountID
		ORDER BY dbo.Account.Code, dbo.EmailTemplate.EmailTemplateID DESC

	END

	ELSE IF (@SelectFlag = 'C') -- Get Total Number of Records Count
	BEGIN
		SELECT COUNT(*) FROM [EmailTemplate] WHERE IsActive=1 and AccountID=@AccountID
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
		FROM	[EmailTemplate] 
		Where	dbo.EmailTemplate.AccountID=@AccountID
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
		FROM	[EmailTemplate]
		WHERE	[EmailTemplateID]=@EmailTemplateID
	END

	ELSE IF (@SelectFlag = 'A') -- All Records
	BEGIN
		SELECT   dbo.EmailTemplate.EmailTemplateID, dbo.EmailTemplate.AccountID, [Title], CASE WHEN LEN(EmailTemplate.[Description]) > 40 THEN SUBSTRING(EmailTemplate.[Description], 1, 40) 
							  + '...' ELSE EmailTemplate.[Description] END AS Description, CASE WHEN LEN([EmailText]) > 70 THEN SUBSTRING([EmailText], 1, 70) 
							  + '...' ELSE [EmailText] END AS EmailText, dbo.EmailTemplate.ModifyBy, dbo.EmailTemplate.ModifyDate, dbo.EmailTemplate.IsActive, 
							  dbo.Account.[Code] ,dbo.EmailTemplate.[Subject]
		FROM         dbo.EmailTemplate INNER JOIN
							  dbo.Account ON dbo.EmailTemplate.AccountID = dbo.Account.AccountID
		where	dbo.EmailTemplate.AccountID=@AccountID
		ORDER BY dbo.Account.Code, dbo.EmailTemplate.EmailTemplateID DESC

	END

	ELSE IF (@SelectFlag = 'C') -- Get Total Number of Records Count
	BEGIN
		SELECT COUNT(*) FROM [EmailTemplate] WHERE IsActive=1 and dbo.EmailTemplate.AccountID=@AccountID
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
		FROM	[EmailTemplate] 
		Where	dbo.EmailTemplate.AccountID=@AccountID
	END
END

END
GO
