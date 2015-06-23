USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Survey_ConclusionHeading]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Survey_ConclusionHeading]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_Survey_ConclusionHeading](@accountid int, @projectid int)
as
begin
select ISnull(ConclusionHeading,'Conclusion') AS ConclusionHeading, PageHeadingConclusion,Conclusionpage from  Survey_ProjectReportSetting where AccountID=@accountid and  ProjectID=@projectid
end
GO
