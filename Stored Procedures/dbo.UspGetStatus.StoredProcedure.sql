USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetStatus]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspGetStatus]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetStatus]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================    
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
    
IF(@chvFlag = ''A'')    
 BEGIN    
  SELECT     
   StatusID,     
   Name,    
   [Description],    
   IsActive
   FROM [MSTStatus]     
   
 END
' 
END
GO
