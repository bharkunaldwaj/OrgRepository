USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Survey_Image]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Survey_Image]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_Survey_Image] 
@accountid int,	@projectid int  
AS
BEGIN
	declare @path varchar(100)	
	
	set @path=(SELECT dbo.GetUploadDocsPath())
	--'file://D:\360_Degree_Feedback\feedback360\UploadDocs\'
	--set @path='file://D:\Feedback360_UAT\Feedback360_Web\feedback360\UploadDocs\'
	
	select	@path + p.Logo as imagePath1,
			p.Logo as Vlogo,
			CASE WHEN prs.PageLogo IS NULL OR LEN(prs.PageLogo )=0 THEN '' ELSE  @path + prs.PageLogo END as pagelogo,
			prs.PageLogo as VPageLogo,
			CASE WHEN prs.FrontPageLogo2 IS NULL OR LEN(prs.FrontPageLogo2 )=0 THEN '' ELSE  @path + prs.FrontPageLogo2 END  as  FrontPageLogo2, 
			prs.FrontPageLogo2 as VFrontPageLogo2,
			CASE WHEN prs.FrontPageLogo3 IS NULL OR LEN(prs.FrontPageLogo3 )=0 THEN '' ELSE  @path + prs.FrontPageLogo3 END  as FrontPageLogo3, 
			prs.FrontPageLogo3 as VFrontPageLogo3
			
	from Survey_AssignQuestionnaire aq 
	
			join Account a on a.AccountID = aq.AccountID
			--join [User] u on u.UserID = aq.AccountID
			--join Project p on p.ProjectID = aq.ProjecctID
			join Survey_Analysis_Sheet p on p.ProgrammeID = aq.ProgrammeID
			join Survey_ProjectReportSetting prs on prs.ProjectID = aq.ProjecctID
			
	where (prs.ProjectID=@projectid) and (prs.AccountID=@accountid)
END
GO
