USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspEmailTemplateManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspEmailTemplateManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Survey_UspEmailTemplateManagement]
(
	@EmailTemplateID	INT,
	@AccountID          INT,
	@Title				VARCHAR(50),
	@Description		VARCHAR(1000),
	@Subject			varchar(200),
	@EmailText			VARCHAR(8000),
	@EmailImage			varchar(250),
	@ModifyBy			INT,
	@ModifyDate			DATETIME, 
	@IsActive			INT,
	@Operation			char(1)
)
AS
BEGIN
	--Insert Operation
	IF (@Operation = 'I')
	BEGIN
		INSERT INTO [Survey_EmailTemplate]
				   (
				    [AccountID] 
				   ,[Title]
				   ,[Description]
				   ,[Subject]
				   ,[EmailText]
				   ,[EmailImage]
				   ,[ModifyBy]
				   ,[ModifyDate]
				   ,[IsActive])
			 VALUES
				   (
				    @AccountID,
				    @Title,
					@Description,
					@Subject,
					@EmailText,
					@EmailImage,
					@ModifyBy,
					@ModifyDate,
					@IsActive)
	END

	--Update Operation
	Else IF (@Operation = 'U')
	BEGIN
		UPDATE [Survey_EmailTemplate]
		SET 
		     [AccountID]        = @AccountID
			,[Title]			= @Title
			,[Description]		= @Description
			,[Subject]			= @Subject
			,[EmailText]		= @EmailText
			,[EmailImage]		= @EmailImage
			,[ModifyBy]			= @ModifyBy
			,[ModifyDate]		= @ModifyDate
			,[IsActive]		    = @IsActive

		WHERE [EmailTemplateID]=@EmailTemplateID
	END

	--Delete Operation
	ELSE IF (@Operation = 'D')
	BEGIN
	
		DELETE FROM [Survey_EmailTemplate] WHERE [EmailTemplateID]=@EmailTemplateID
	
		-- Delete Entries from Project for deleted email template		
		update dbo.Survey_Project set EmailTMPLStart=0 where EmailTMPLStart=@EmailTemplateID
		update dbo.Survey_Project set EmailTMPLReminder1=0 where EmailTMPLReminder1=@EmailTemplateID
		update dbo.Survey_Project set EmailTMPLReminder2=0 where EmailTMPLReminder2=@EmailTemplateID
		update dbo.Survey_Project set EmailTMPLReminder3=0 where EmailTMPLReminder3=@EmailTemplateID
		

		
		
	END
END
GO
