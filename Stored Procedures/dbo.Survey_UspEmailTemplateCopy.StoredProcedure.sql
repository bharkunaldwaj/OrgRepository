USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspEmailTemplateCopy]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspEmailTemplateCopy]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Survey_UspEmailTemplateCopy]
	
	@EmailID		varchar(5000),
	@AccountID      int,
	@Operation	    char(1)

AS

	--Insert Operation
	IF (@Operation = 'C')
	
	BEGIN
	
	INSERT [Survey_EmailTemplate]  
           ([Title]
           ,[AccountID]
           ,[Description]
           ,[Subject]
           ,[EmailText]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
	
			( SELECT   [Title] , @AccountID , [Description],[Subject], [EmailText], [ModifyBy], [ModifyDate], [IsActive]
						FROM dbo.Survey_EmailTemplate 
			WHERE EmailTemplateID IN (select [value] from fn_csvtotable(@EmailID)) )
					
					
	END
	
	SELECT 1
GO
