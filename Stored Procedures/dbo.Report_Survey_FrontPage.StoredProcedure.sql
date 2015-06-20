USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Survey_FrontPage]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Survey_FrontPage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Report_Survey_FrontPage]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Survey_FrontPage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Report_Survey_FrontPage](@accountid int,@projectid int)
as
begin
Select PageHeading1, PageHeading2 ,PageHeading3, PageHeadingColor, PageLogo,FrontPageLogo2,FrontPageLogo3 from Survey_ProjectReportSetting where AccountID=@accountid AND ProjectID=@projectid
end
' 
END
GO
