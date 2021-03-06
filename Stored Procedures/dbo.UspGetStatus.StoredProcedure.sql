USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetStatus]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspGetStatus]
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
    
CREATE PROCEDURE [dbo].[UspGetStatus]    
@intStatusID INT = NULL,    
@chvStatusName VARCHAR(50) = NULL,    
@chvFlag VARCHAR(10)    
    
AS    
SET NOCOUNT ON    
    
IF(@chvFlag = 'A')    
 BEGIN    
  SELECT     
   StatusID,     
   Name,    
   [Description],    
   IsActive
   FROM [MSTStatus]     
   
 END
GO
