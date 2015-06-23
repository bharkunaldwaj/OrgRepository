USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_ShowCoverPage]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_ShowCoverPage]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Report_ShowCoverPage](@accountid int ,@projectid int)
as
begin
select CoverPage from Survey_ProjectReportSetting where AccountID=@accountid and ProjectID=@projectid
end
GO
