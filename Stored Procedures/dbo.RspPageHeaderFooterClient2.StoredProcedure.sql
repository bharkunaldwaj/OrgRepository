USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspPageHeaderFooterClient2]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspPageHeaderFooterClient2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspPageHeaderFooterClient2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspPageHeaderFooterClient2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be Use in PageHeader & Footer>
-- =============================================
CREATE PROCEDURE [dbo].[RspPageHeaderFooterClient2] 
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
END
' 
END
GO
