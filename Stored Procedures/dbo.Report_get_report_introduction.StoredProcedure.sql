USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_get_report_introduction]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_get_report_introduction]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_get_report_introduction](@accountid int ,@projectid int)
as
begin
select PageHeadingIntro,ReportIntroduction from Survey_ProjectReportSetting where AccountID=@accountid and ProjectID=@projectid
end
GO
