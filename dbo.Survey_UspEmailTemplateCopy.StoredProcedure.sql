USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspEmailTemplateCopy]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspEmailTemplateCopy]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspEmailTemplateCopy]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspEmailTemplateCopy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Survey_UspEmailTemplateCopy]
	
	@EmailID		varchar(5000),
	@AccountID      int,
	@Operation	    char(1)

AS

	--Insert Operation
	IF (@Operation = ''C'')
	
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
' 
END
GO
