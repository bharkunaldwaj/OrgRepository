USE [Feedback360_Dev2]
GO
/****** Object:  UserDefinedFunction [dbo].[GetUploadDocsPath]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUploadDocsPath]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetUploadDocsPath]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUploadDocsPath]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetUploadDocsPath]
(
    
)
RETURNS varchar(500) -- or whatever length you need
AS
BEGIN
--    Declare @path varchar(500) = ''file://D:\Feedback360_UAT\Feedback360_Web\feedback360\UploadDocs\'';
	  Declare @path varchar(500) = ''file://D:\360_Degree_Feedback\feedback360\UploadDocs\'';
    
    
    RETURN  @path

END' 
END
GO
