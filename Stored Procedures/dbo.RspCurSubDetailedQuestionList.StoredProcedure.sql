USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurSubDetailedQuestionList]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurSubDetailedQuestionList]
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
-- [RspCurSubDetailedQuestionList] 135, 639, 250, 1
-- [RspCurSubDetailedQuestionList] 135, 639, 250, 0
Create PROCEDURE [dbo].[RspCurSubDetailedQuestionList] 
	@questionnaireid int,
	@targetpersonid int,
	@categoryid int,
	@DetailedQstVisibility varchar(2)
AS
BEGIN
	IF(@DetailedQstVisibility = '1')
	BEGIN	
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
		where q.QuestionnaireID = @questionnaireid and QuestionTypeID = 1 and CateogryID = @categoryid
	END	
	Else IF(@DetailedQstVisibility = '0')
	BEGIN
		select '' as AccountID, '' as QuestionID, '' as CateogryID, '' as  CategoryName,'' as QuestionnaireID,
		'' as Title, '' as [Description]
		from [User] where UserID = @targetpersonid
	END					
END
GO
