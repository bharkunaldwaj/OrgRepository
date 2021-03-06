USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspFindTemplate]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspFindTemplate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Survey_UspFindTemplate]

@ProjectID int,
@SelectFlag char(1)

AS

IF (@SelectFlag = 'A') 

BEGIN

SELECT     [Survey_EmailTemplate].EmailText

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLStart = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
IF (@SelectFlag = 'P') 

BEGIN

SELECT     [Survey_EmailTemplate].EmailText

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLParticipant = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
   
   IF (@SelectFlag = 'S') 

BEGIN

SELECT     [Survey_EmailTemplate].[Subject]

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLParticipant = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
 



 IF (@SelectFlag = 'Q') 
 
 BEGIN
 
 SELECT     [Survey_EmailTemplate].[Subject]

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLStart = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END 

IF (@SelectFlag = 'I')  -- For Participant

BEGIN

SELECT     [Survey_EmailTemplate].EmailImage

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLParticipant = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
 



 IF (@SelectFlag = 'J')  -- For Candidate
 
 BEGIN
 
 SELECT     [Survey_EmailTemplate].EmailImage

     FROM   [Survey_Project] Inner Join [Survey_EmailTemplate] on dbo.Survey_Project.EmailTMPLStart = [Survey_EmailTemplate].EmailTemplateID
            where Survey_Project.ProjectID = @ProjectID
   END
GO
