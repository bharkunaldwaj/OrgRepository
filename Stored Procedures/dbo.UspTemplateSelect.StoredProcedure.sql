USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspTemplateSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspTemplateSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[UspTemplateSelect]

@ProjectID int,
@SelectFlag char(1)

AS

IF (@SelectFlag = 'A') 

BEGIN

SELECT     [EmailTemplate].EmailText

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLStart = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END
GO
