USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetAccountInformation]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetAccountInformation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityGetAccountInformation]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetAccountInformation]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PersonalityGetAccountInformation]
	@UniqueID uniqueidentifier
	-- Add the parameters for the stored procedure here
	--@QuestionnaireID uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @AccountId int;
	declare @QuestionnaireID uniqueidentifier;
	declare @ReportMgmtID uniqueidentifier;
	declare @EmailTo varchar(150);
	declare @EmailID varchar(250);
	declare @CustomEmailID varchar(250);
	declare @EmailTemplateID varchar(150);
	declare @IsActive bit;

	select @EmailTemplateID=perExLink.EmailTemplateId,@AccountId = perExLink.AccountID,@QuestionnaireID = perExLink.QuestionnaireID,@ReportMgmtID = perExLink.ReportManagementID,@EmailTo = perExLink.EmailTo,
	@IsActive = perExLink.IsActive,@CustomEmailID = CustomEmail
	from PersonallityExternalLink perExLink 
	where perExLink.UniqueID = @UniqueID
	
	--select PRM.BarGraph,PRM.WheelGraph,PRM.ShowGraphOnPageNumber from PersonalityReportManagement PRM where PRM.UniqueID = @ReportMgmtID
	
 
	
	Select @EmailID = u.EmailID from [User] u INNER JOIN 
	[Group] g on u.GroupID = g.GroupID
	
	 WHERE GroupName=''Account Admin''  and u.AccountID =   @AccountId
	
	
	
	
    -- Insert statements for procedure here
	select acc.AccountID,acc.Code,acc.OrganisationName ,personQues.Name,@QuestionnaireID as QuestionnaireID,@ReportMgmtID as ReportMgmtID,@EmailTo as EmailTo,PRM.BarGraph,PRM.WheelGraph,PRM.ShowGraphOnPageNumber,
	@EmailTemplateID EmailTemplateID,@EmailID EmailID,@IsActive IsActive,@CustomEmailID CustomEmailID
	from Account acc ,PersonalityQuestionnaires personQues,PersonalityReportManagement PRM
	where acc.AccountID= @AccountId and personQues.UniqueID = @QuestionnaireID and PRM.UniqueID = @ReportMgmtID
END
' 
END
GO
