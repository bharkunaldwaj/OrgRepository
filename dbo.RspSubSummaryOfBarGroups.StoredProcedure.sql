USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspSubSummaryOfBarGroups]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspSubSummaryOfBarGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspSubSummaryOfBarGroups]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspSubSummaryOfBarGroups]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be Used to Display QuestionList by QuestionnareID which is Coming
--				 MainReport From>
-- =============================================
CREATE PROCEDURE [dbo].[RspSubSummaryOfBarGroups] 
	@categoryid int,
	@Code varchar(5)
AS
BEGIN
	select ad.RelationShip,c.CategoryName, sum(cast(Answer as decimal(12,2))) As "Sum" , count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2)) As Average
	from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	where a.Code = @Code and q.QuestionTypeID = 2 and qa.Answer != '' '' 
	and c.CategoryID = @categoryid
	Group By ad.RelationShip,c.CategoryName
END
' 
END
GO
