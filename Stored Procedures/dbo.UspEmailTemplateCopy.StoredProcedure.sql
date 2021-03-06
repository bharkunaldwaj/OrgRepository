USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspEmailTemplateCopy]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspEmailTemplateCopy]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspEmailTemplateCopy]
	
	@EmailID		varchar(5000),
	@AccountID      int,
	@Operation	    char(1)

AS

	--Insert Operation
	IF (@Operation = 'C')
	
	BEGIN
	
	INSERT [EmailTemplate]  
           ([Title]
           ,[AccountID]
           ,[Description]
           ,[Subject]
           ,[EmailText]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
	
			( SELECT   [Title] , @AccountID , [Description],[Subject], [EmailText], [ModifyBy], [ModifyDate], [IsActive]
						FROM dbo.EmailTemplate 
			WHERE EmailTemplateID IN (select [value] from fn_csvtotable(@EmailID)) )
					
					
	END
	
	SELECT 1
GO
