USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGroupMaintenance]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGroupMaintenance]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================    
-- Author:  Rajesh Kumar    
-- Create date: 20Aug2010    
-- Description: This will Add/Edit Group with Rights in GroupRights table    
-- =========================================================================    
    
create PROCEDURE [dbo].[Survey_UspGroupMaintenance] --0,'TestSubu','This is a Test',0,2,'A'    
-- Add the parameters for the stored procedure here    
@intID INT= NULL,      
@chvGroupName VARCHAR(50) = NULL,    
@chvnDescription VARCHAR(100) = NULL,    
@chvnWelcomeText VARCHAR(MAX) = NULL,    
@chvnNewsText VARCHAR(MAX) = NULL,    
@bitIsActive BIT=0,    
@chvMode VARCHAR(50) = NULL    
--@intGroupID AS INT OUTPUT    
    
AS    
BEGIN TRAN    
 SET NOCOUNT OFF;    
IF(@chvMode='I')    
 BEGIN    
  INSERT INTO [Survey_Group]     
   (    
    GroupName,    
    [Description],    
    WelcomeText,    
    NewsText,    
    IsActive,    
    CreatedDate,    
    ModifiedDate    
   )    
  VALUES     
   (    
    @chvGroupName,    
    @chvnDescription,    
    @chvnWelcomeText,     
    @chvnNewsText,     
    @bitIsActive,    
    GETDATE(),    
    GETDATE()    
   )  

Select max(isnull(GroupID,0)) from [Survey_Group]
  
--/SET @intGroupID=@@IDENTITY    
    
 END    
    
ELSE IF(@chvMode='U')    
    
 BEGIN    
  UPDATE  [Survey_Group]    
  SET      
  GroupName = @chvGroupName,    
  [Description] = @chvnDescription,    
  WelcomeText = @chvnWelcomeText,    
  NewsText = @chvnWelcomeText,    
  IsActive = @bitIsActive,    
  ModifiedDate = GETDATE()    
  WHERE GroupID=@intID    
    
  --SET @intGroupID = @intID    
  select @intID  

 END    
    
 ELSE IF (@chvMode='D')    
   BEGIN    
   DELETE FROM [Survey_GROUP]    
   WHERE GroupID=@intID    
  END    
    
IF(@@ERROR <> 0)    
 ROLLBACK TRAN    
ELSE    
 COMMIT TRAN
GO
