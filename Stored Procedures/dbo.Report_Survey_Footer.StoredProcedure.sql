USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Survey_Footer]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Survey_Footer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_Survey_Footer](@accountid int,@projectid int )
as
begin
select PageHeadingCopyright from Survey_ProjectReportSetting where accountid=@accountid and ProjectID=@projectid
end
GO
