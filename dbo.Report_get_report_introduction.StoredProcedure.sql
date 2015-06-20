USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_get_report_introduction]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_get_report_introduction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Report_get_report_introduction]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_get_report_introduction]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Report_get_report_introduction](@accountid int ,@projectid int)
as
begin
select PageHeadingIntro,ReportIntroduction from Survey_ProjectReportSetting where AccountID=@accountid and ProjectID=@projectid
end
' 
END
GO
