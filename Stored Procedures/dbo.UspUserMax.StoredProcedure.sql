USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspUserMax]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspUserMax]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UspUserMax]



as
Begin

SELECT ISNULL(MAX([UserID]),1)
  FROM [User]

end
GO
