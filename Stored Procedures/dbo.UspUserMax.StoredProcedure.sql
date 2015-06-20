USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspUserMax]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspUserMax]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspUserMax]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspUserMax]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[UspUserMax]



as
Begin

SELECT ISNULL(MAX([UserID]),1)
  FROM [User]

end
' 
END
GO
