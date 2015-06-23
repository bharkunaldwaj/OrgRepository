USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Show_FreeTextResponse]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Show_FreeTextResponse]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Report_Show_FreeTextResponse](@accountid int,
	@projectid int)
as
 begin
 Select FreeTextResponses from Survey_ProjectReportSetting where AccountID=@accountid and ProjectID=@projectid
 End
GO
