USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurImage]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurImage]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- [RspCurImage] 968
CREATE PROCEDURE [dbo].[RspCurImage] 
	@targetpersonid int    --   *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
AS
BEGIN
	declare @path varchar(100)	
	
	--set @path='file://D:\360_Degree_Feedback\feedback360\UploadDocs\'
	--set @path='file://D:\Feedback360\feedback360\UploadDocs\'
	
	--set @path='file://D:\360_Degree_Feedback\feedback360\UploadDocs\'
	set @path=(SELECT dbo.GetUploadDocsPath())
	
	select	@path + p.Logo as imagePath1,
			p.Logo as Vlogo,
			@path + prs.PageLogo as pagelogo,
			prs.PageLogo as VPageLogo,
			@path + prs.FrontPageLogo2 as FrontPageLogo2, 
			prs.FrontPageLogo2 as VFrontPageLogo2,
			@path + prs.FrontPageLogo3 as FrontPageLogo3, 
			prs.FrontPageLogo3 as VFrontPageLogo3,
			@path + prs.FrontPageLogo4 as FrontPageLogo4
			
	from AssignQuestionnaire aq 
	
			join Account a on a.AccountID = aq.AccountID
			join [User] u on u.UserID = aq.TargetPersonID
			--join Project p on p.ProjectID = aq.ProjecctID
			join Programme p on p.ProgrammeID = aq.ProgrammeID
			join ProjectReportSetting prs on prs.ProjectID = aq.ProjecctID
			
	where aq.TargetPersonID = @targetpersonid
END
GO
