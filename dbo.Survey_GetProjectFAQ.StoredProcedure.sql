USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_GetProjectFAQ]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_GetProjectFAQ]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_GetProjectFAQ]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_GetProjectFAQ]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[Survey_GetProjectFAQ]

@ProjectID int

as

select faqtext from Survey_project where projectid=@ProjectID
' 
END
GO
