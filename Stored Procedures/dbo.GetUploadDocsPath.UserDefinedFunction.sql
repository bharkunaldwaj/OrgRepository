USE [Feedback360_Dev2]
GO
/****** Object:  UserDefinedFunction [dbo].[GetUploadDocsPath]    Script Date: 06/23/2015 10:42:52 ******/
DROP FUNCTION [dbo].[GetUploadDocsPath]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetUploadDocsPath]
(
    
)
RETURNS varchar(500) -- or whatever length you need
AS
BEGIN
--    Declare @path varchar(500) = 'file://D:\Feedback360_UAT\Feedback360_Web\feedback360\UploadDocs\';
	  Declare @path varchar(500) = 'file://D:\360_Degree_Feedback\feedback360\UploadDocs\';
    
    
    RETURN  @path

END
GO
