USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_ShowAnalysis_III]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_ShowAnalysis_III]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Report_ShowAnalysis_III](@projectID int)
as
begin
select AnalysisIII from  Survey_ProjectReportSetting where ProjectID=@projectID
end
GO
