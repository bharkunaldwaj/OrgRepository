USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspPageHeaderFooter]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspPageHeaderFooter]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be Use in PageHeader & Footer>
-- =============================================
CREATE PROCEDURE [dbo].[RspPageHeaderFooter] 
	@targetpersonid int    --   *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
AS
BEGIN
	select aq.AccountID as "a.AccountID", a.OrganisationName, u.*,
	p.Title, p.Description, p.Logo,prs.*
	from AssignQuestionnaire aq 
	join Account a on a.AccountID = aq.AccountID
	join [User] u on u.UserID = aq.TargetPersonID
	join Project p on p.ProjectID = aq.ProjecctID
	join ProjectReportSetting prs on prs.ProjectID = aq.ProjecctID
	where aq.TargetPersonID = @targetpersonid	
	
	--select aq.AccountID as "a.AccountID", a.OrganisationName, u.*,
	--p.Title, p.Description, p.Logo
	--from AssignQuestionnaire aq 
	--left join Account a on a.AccountID = aq.AccountID
	--left join [User] u on u.UserID = aq.TargetPersonID
	--left join Project p on p.ProjectID = aq.ProjecctID
	--where aq.TargetPersonID = @targetpersonid	
END
GO
