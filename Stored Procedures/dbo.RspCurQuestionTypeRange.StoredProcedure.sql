USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurQuestionTypeRange]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurQuestionTypeRange]
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
-- [RspCurQuestionTypeRange] 135, 639, 250, 1
-- [RspCurQuestionTypeRange] 135, 639, 250, 0
CREATE PROCEDURE [dbo].[RspCurQuestionTypeRange] 
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
			
		select @FirstName=FirstName,@LastName=LastName, @FullName= FirstName + ' ' + LastName   from [user] where userid=@targetpersonid		
		select CateogryID,QuestionnaireID, QuestionID, Title,
		case
			when Token = 1 then REPLACE([Description], '[TargetName]', @FirstName)
			when Token = 2 then REPLACE([Description], '[TargetName]', @LastName)
			else REPLACE([Description], '[TargetName]', @FullName)
		end	[Description]			
		from dbo.Question
		where QuestionnaireID = @questionnaireid and QuestionTypeID = 2 and CateogryID = @categoryid
	END	
	Else IF(@CategoryQstlistVisibility = '0')
	BEGIN
		select  '' as CateogryID, '' as QuestionnaireID, '' as QuestionID,'' as  Title,'' as [Description]
		from [User] where UserID = @targetpersonid
	END			
END
GO
