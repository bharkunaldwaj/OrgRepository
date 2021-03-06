USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspSubDetailedQuestionList]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[RspSubDetailedQuestionList]
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
-- [RspSubDetailedQuestionList] 135, 639, 250, 1
-- [RspSubDetailedQuestionList] 135, 639, 250, 0

CREATE PROCEDURE [dbo].[RspSubDetailedQuestionList] 
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
		declare @ClientName varchar(200)
			
		SELECT @ClientName = ISNULL(ClientName, '-') 
		FROM AssignQuestionnaire assignQuestionnaire INNER JOIN Programme programme ON assignQuestionnaire.ProgrammeID = programme.ProgrammeID 
		WHERE assignQuestionnaire.QuestionnaireID = @questionnaireid AND assignQuestionnaire.TargetPersonID = @targetpersonid		
			
		select @FirstName=FirstName,@LastName=LastName, @FullName= FirstName + ' ' + LastName 
		from [user] where userid=@targetpersonid
				
		SELECT 
		q.AccountID,
		QuestionID, 
		CateogryID,
		CategoryName,
		q.QuestionnaireID, 
		Title,
		[Description] = CASE
							WHEN Token = 1 THEN REPLACE(REPLACE(q.[Description], '[targetName]', @FirstName), '[CLIENTNAME]', @ClientName)
							WHEN Token = 2 THEN REPLACE(REPLACE(q.[Description], '[targetName]', @LastName), '[CLIENTNAME]', @ClientName)
							ELSE REPLACE(REPLACE(q.[Description], '[targetName]', @FullName), '[CLIENTNAME]', @ClientName)
						END				
		FROM dbo.Question q
		INNER JOIN Category c on c.CategoryID = q.CateogryID
		WHERE q.QuestionnaireID = @questionnaireid AND QuestionTypeID = 1 AND CateogryID = @categoryid
	END	
	Else IF(@DetailedQstVisibility = '0')
	BEGIN
		select '' as AccountID, '' as QuestionID, '' as CateogryID, '' as  CategoryName,'' as QuestionnaireID,
		'' as Title, '' as [Description]
		from [User] where UserID = @targetpersonid
	END					
END
GO
