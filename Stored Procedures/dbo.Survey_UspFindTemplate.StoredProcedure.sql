USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspFindTemplate]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspFindTemplate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspFindTemplate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspFindTemplate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create procedure [dbo].[Survey_UspFindTemplate]

@ProjectID int,
@SelectFlag char(1)

AS

IF (@SelectFlag = ''A'') 

BEGIN

SELECT     [Survey_EmailTemplate].EmailText

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLStart = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
IF (@SelectFlag = ''P'') 

BEGIN

SELECT     [Survey_EmailTemplate].EmailText

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLParticipant = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
   
   IF (@SelectFlag = ''S'') 

BEGIN

SELECT     [Survey_EmailTemplate].[Subject]

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLParticipant = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
 



 IF (@SelectFlag = ''Q'') 
 
 BEGIN
 
 SELECT     [Survey_EmailTemplate].[Subject]

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLStart = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END 

IF (@SelectFlag = ''I'')  -- For Participant

BEGIN

SELECT     [Survey_EmailTemplate].EmailImage

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLParticipant = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
 



 IF (@SelectFlag = ''J'')  -- For Candidate
 
 BEGIN
 
 SELECT     [Survey_EmailTemplate].EmailImage

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLStart = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
' 
END
GO
