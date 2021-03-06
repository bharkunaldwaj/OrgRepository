USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspSubDetailedQuestionListClient2]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[RspSubDetailedQuestionListClient2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be Used to Display QuestionList by QuestionnareID which is Coming
--				 MainReport From>
-- =============================================
CREATE PROCEDURE [dbo].[RspSubDetailedQuestionListClient2] 
	@questionnaireid int,
	@targetpersonid int
AS
BEGIN
	--select AccountID,QuestionID, CateogryID,QuestionnaireID, Title, [Description] from dbo.Question
	--where QuestionnaireID = @questionnaireid and QuestionTypeID = 1
	declare @FirstName varchar(250)
	declare @LastName varchar(250)
	declare @FullName varchar(250)
		
	select @FirstName=FirstName,@LastName=LastName, @FullName= FirstName + ' ' + LastName   from [user] where userid=@targetpersonid		
	select q.AccountID,QuestionID, CateogryID,CategoryName,q.QuestionnaireID, Title,
	case
		when Token = 1 then REPLACE(q.[Description], '[targetName]', @FirstName)
		when Token = 2 then REPLACE(q.[Description], '[targetName]', @LastName)
		else REPLACE(q.[Description], '[targetName]', @FullName)
	end	[Description]			
	from dbo.Question q
	inner join Category c on c.CategoryID = q.CateogryID
	where q.QuestionnaireID = @questionnaireid and QuestionTypeID = 1
END
GO
