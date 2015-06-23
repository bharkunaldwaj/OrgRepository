USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspQuestionTypeRangeClient1]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspQuestionTypeRangeClient1]
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
-- [RspQuestionTypeRangeClient1] 122, 546
CREATE PROCEDURE [dbo].[RspQuestionTypeRangeClient1] 
	@questionnaireid int,
	@targetpersonid int
AS
BEGIN
	-- This Procedure will return RangeType Question & We are replacing tokens from question Description
	-- Range Type Question : QuestionTypeID = 2
	declare @FirstName varchar(250)
	declare @LastName varchar(250)
	declare @FullName varchar(250)
		
	select @FirstName=FirstName,@LastName=LastName, @FullName= FirstName + ' ' + LastName   from [user] where userid=@targetpersonid		
	select AccountID, CateogryID,QuestionnaireID, Title,
	case
		when Token = 1 then REPLACE([Description], '[TargetName]', @FirstName)
		when Token = 2 then REPLACE([Description], '[TargetName]', @LastName)
		else REPLACE([Description], '[TargetName]', @FullName)
	end	[Description]			
	from dbo.Question
	where QuestionnaireID = @questionnaireid and QuestionTypeID = 2
END
GO
