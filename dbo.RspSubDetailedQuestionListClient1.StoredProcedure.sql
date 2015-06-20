USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspSubDetailedQuestionListClient1]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspSubDetailedQuestionListClient1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspSubDetailedQuestionListClient1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspSubDetailedQuestionListClient1]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be Used to Display QuestionList by QuestionnareID which is Coming
--				 MainReport From>
-- =============================================
-- [RspSubDetailedQuestionListClient1] 122, 546
CREATE PROCEDURE [dbo].[RspSubDetailedQuestionListClient1] 
	@questionnaireid int,
	@targetpersonid int
AS
BEGIN
	-- This Procedure will return RangeType Question & We are replacing tokens from question Description
	-- Text Type Question : QuestionTypeID = 1
	declare @FirstName varchar(250)
	declare @LastName varchar(250)
	declare @FullName varchar(250)
		
	select @FirstName=FirstName,@LastName=LastName, @FullName= FirstName + '' '' + LastName   from [user] where userid=@targetpersonid		
	select q.AccountID,QuestionID, CateogryID,CategoryName,q.QuestionnaireID, Title,
	case
		when Token = 1 then REPLACE(q.[Description], ''[targetName]'', @FirstName)
		when Token = 2 then REPLACE(q.[Description], ''[targetName]'', @LastName)
		else REPLACE(q.[Description], ''[targetName]'', @FullName)
	end	[Description]			
	from dbo.Question q
	inner join Category c on c.CategoryID = q.CateogryID
	where q.QuestionnaireID = @questionnaireid and QuestionTypeID = 1
END
' 
END
GO
