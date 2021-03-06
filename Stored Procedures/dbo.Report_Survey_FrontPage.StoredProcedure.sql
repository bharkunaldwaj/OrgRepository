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

DECLARE @Company VARCHAR(500)
DECLARE @CompanyID int
DECLARE @CloseDate VARCHAR(500)
SELECT @Company=Title,@CompanyID=CompanyID FROM Survey_Company WHERE AccountID=@accountid and ProjectID=@projectid
SELECT @CloseDate=CONVERT(VARCHAR(24),Convert(Date,EndDate),113) FROM Survey_Analysis_Sheet WHERE AccountID=@accountid and ProjectID=@projectid and CompanyID=@CompanyID

-- [COMPANYNAME], Heading 3 [CLOSEDATE]


Select REPLACE(REPLACE(PageHeading1,'[COMPANYNAME]',@Company),'[CLOSEDATE]',@CloseDate) as PageHeading1, 
 REPLACE(REPLACE(PageHeading2,'[COMPANYNAME]',@Company),'[CLOSEDATE]',@CloseDate) as PageHeading2 ,
REPLACE(REPLACE(PageHeading3,'[COMPANYNAME]',@Company),'[CLOSEDATE]',@CloseDate) as PageHeading3 ,
PageHeadingColor, PageLogo,FrontPageLogo2,FrontPageLogo3 
from Survey_ProjectReportSetting where AccountID=@accountid AND ProjectID=@projectid
end
GO


