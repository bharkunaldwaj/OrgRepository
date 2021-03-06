USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspQuestionListClient2]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspQuestionListClient2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<>
-- =============================================
-- [RspQuestionListClient2] 725
CREATE PROCEDURE [dbo].[RspQuestionListClient2] 	
	@targetpersonid int
AS
BEGIN
	--Query Will Return List of Question which have IsActive Field is 1 with category by sequence
	-- and Title & Description will Display in Report
	-- Need to Dispay q.Sequence and c.Sequence will be use in Group By Clause coz need c.Sequence in Order by clause
	
	declare @FirstName varchar(250)
	declare @LastName varchar(250)
	declare @FullName varchar(250)
		
	select @FirstName=FirstName,@LastName=LastName, @FullName= FirstName + ' ' + LastName   from [user] where userid=@targetpersonid	
	select c.CategoryID, c.CategoryName, q.Sequence,q.Title,
	case
		when Token = 1 then REPLACE(q.[Description], '[targetName]', @FirstName)
		when Token = 2 then REPLACE(q.[Description], '[targetName]', @LastName)
		else REPLACE(q.[Description], '[targetName]', @FullName)
	end	[Description]
	--c.QuestionnaireID
	from AssignQuestionnaire aq 
	left join Category c on c.AccountID =  aq.AccountID
	left join Question q on q.CateogryID = c.CategoryID
	where TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 and q.IsActive = 1
	and c.QuestionnaireID = aq.QuestionnaireID 
	Group By c.CategoryID, c.CategoryName, c.Sequence,q.Sequence, q.Description,
	q.Token,q.Title
	Order by c.Sequence
END
GO
