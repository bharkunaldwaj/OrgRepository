USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_GetProjectFAQ]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_GetProjectFAQ]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_GetProjectFAQ]

@ProjectID int

as

select faqtext from Survey_project where projectid=@ProjectID
GO
