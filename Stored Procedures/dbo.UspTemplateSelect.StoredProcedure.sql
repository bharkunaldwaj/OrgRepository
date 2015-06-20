USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspTemplateSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspTemplateSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspTemplateSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspTemplateSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'Create procedure [dbo].[UspTemplateSelect]

@ProjectID int,
@SelectFlag char(1)

AS

IF (@SelectFlag = ''A'') 

BEGIN

SELECT     [EmailTemplate].EmailText

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLStart = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END
' 
END
GO
