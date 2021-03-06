USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspFindTemplate]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspFindTemplate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspFindTemplate]

@ProjectID int,
@SelectFlag char(1)

AS

IF (@SelectFlag = 'A') 

BEGIN

SELECT     [EmailTemplate].EmailText

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLStart = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END
IF (@SelectFlag = 'P') 

BEGIN

SELECT     [EmailTemplate].EmailText

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLParticipant = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END
   
   IF (@SelectFlag = 'S') 

BEGIN

SELECT     [EmailTemplate].[Subject]

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLParticipant = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END
 



 IF (@SelectFlag = 'Q') 
 
 BEGIN
 
 SELECT     [EmailTemplate].[Subject]

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLStart = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END 

IF (@SelectFlag = 'I')  -- For Participant

BEGIN

SELECT     [EmailTemplate].EmailImage

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLParticipant = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END
 



 IF (@SelectFlag = 'J')  -- For Candidate
 
 BEGIN
 
 SELECT     [EmailTemplate].EmailImage

     FROM   [Project] Inner Join [EmailTemplate] on dbo.Project.EmailTMPLStart = [EmailTemplate].EmailTemplateID
            where Project.ProjectID = @ProjectID
   END
GO
