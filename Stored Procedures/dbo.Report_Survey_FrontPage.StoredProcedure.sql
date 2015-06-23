USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Survey_FrontPage]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Survey_FrontPage]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_Survey_FrontPage](@accountid int,@projectid int)
as
begin
Select PageHeading1, PageHeading2 ,PageHeading3, PageHeadingColor, PageLogo,FrontPageLogo2,FrontPageLogo3 from Survey_ProjectReportSetting where AccountID=@accountid AND ProjectID=@projectid
end
GO
