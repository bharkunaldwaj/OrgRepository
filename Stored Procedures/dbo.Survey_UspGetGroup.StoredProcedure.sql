USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetGroup]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGetGroup]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  Rajesh Kumar    
-- Create date: 20Aug2010    
-- Description: This Stored Procedure will return all groups    
-- =============================================    
    
create PROCEDURE [dbo].[Survey_UspGetGroup]    
@intGroupID INT = NULL,    
@chvGroupName VARCHAR(50) = NULL,    
@chvFlag VARCHAR(10)    
    
AS    
SET NOCOUNT ON    
    
IF(@chvFlag = 'ALL')    
 BEGIN    
  SELECT     
   GroupID,     
   GroupName,    
   [Description],    
   WelcomeText,    
   NewsText, 
   IsActive,   
   --CASE WHEN IsActive=1 THEN 'true' ELSE 'false' END AS IsActive,    
   --CONVERT(VARCHAR(10),CreatedDate,101) AS CreatedDate,    
   CreatedDate,  
   ModifiedDate    
  FROM [Survey_Group]     
  WHERE (GroupID = ISNULL(@intGroupID,GroupID))      
  ORDER BY 1    
 END    

ELSE IF(@chvFlag = 'COUNT')    
 BEGIN    
  SELECT COUNT(*) FROM [Survey_Group]    
  WHERE GroupName = @chvGroupName  
  END
  
 ELSE IF(@chvFlag = 'A')    
 BEGIN    
  SELECT     
   GroupID,     
   GroupName,    
   [Description],    
   WelcomeText,    
   NewsText, 
   IsActive,   
   --CASE WHEN IsActive=1 THEN 'true' ELSE 'false' END AS IsActive,    
   --CONVERT(VARCHAR(10),CreatedDate,101) AS CreatedDate,    
   CreatedDate,  
   ModifiedDate    
  FROM [Survey_Group]     
    
 END
GO
