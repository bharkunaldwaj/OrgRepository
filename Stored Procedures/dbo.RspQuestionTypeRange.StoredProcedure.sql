USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspQuestionTypeRange]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspQuestionTypeRange]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>	
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Sub Report to Display the All The Questions
--				(Type is : Range)>
-- =============================================
-- [RspQuestionTypeRange] 135, 639, 250, 1
-- [RspQuestionTypeRange] 135, 639, 250, 0
CREATE PROCEDURE [dbo].[RspQuestionTypeRange] 
	@questionnaireid int,
	@targetpersonid int,
	@categoryid int,
	@CategoryQstlistVisibility varchar(2)
AS
BEGIN
	IF(@CategoryQstlistVisibility = '1')
	BEGIN
		declare @FirstName varchar(250)
		declare @LastName varchar(250)
		declare @FullName varchar(250)
		declare @ClientName varchar(200)
	
		select @ClientName = isnull(ClientName, '-') from assignquestionnaire aq inner join Programme pg on
		aq.ProgrammeID = pg.ProgrammeID 
		where aq.QuestionnaireID = @questionnaireid and aq.TargetPersonID = @targetpersonid
	
		select @FirstName=FirstName,@LastName=LastName, @FullName= FirstName + ' ' + LastName   from [user] where userid=@targetpersonid		
		select AccountID, CateogryID,QuestionnaireID, Title,
		case
			when Token = 1 then REPLACE(REPLACE([Description], '[TargetName]', @FirstName), '[CLIENTNAME]', @ClientName) 
			when Token = 2 then REPLACE(REPLACE([Description], '[TargetName]', @LastName), '[CLIENTNAME]', @ClientName) 
			else REPLACE(REPLACE([Description], '[TargetName]', @FullName), '[CLIENTNAME]', @ClientName) 
		end	[Description]			
		from dbo.Question
		where QuestionnaireID = @questionnaireid and QuestionTypeID = 2 and CateogryID = @categoryid
	END	
	Else IF(@CategoryQstlistVisibility = '0')
	BEGIN
		select '' as AccountID, '' as CateogryID, '' as QuestionnaireID, '' as  Title,'' as [Description]
		from [User] where UserID = @targetpersonid
	END			
END
GO
