USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_ShowAnalysis_III]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_ShowAnalysis_III]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Report_ShowAnalysis_III]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_ShowAnalysis_III]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[Report_ShowAnalysis_III](@projectID int)
as
begin
select AnalysisIII from  Survey_ProjectReportSetting where ProjectID=@projectID
end
' 
END
GO
