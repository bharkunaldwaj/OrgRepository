USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_ShowAnalysis_II]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_ShowAnalysis_II]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Report_ShowAnalysis_II](@projectID int)
as
begin
select AnalysisII from  Survey_ProjectReportSetting where ProjectID=@projectID
end
GO
