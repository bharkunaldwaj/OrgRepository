USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspConclusionChartSelf]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspConclusionChartSelf]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in >
-- =============================================
-- [RspConclusionChartSelf] 546
CREATE PROCEDURE [dbo].[RspConclusionChartSelf] 
	@targetpersonid int      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *		
AS
BEGIN	
	select u.FirstName +' '+ u.LastName as RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	
	into #tempself from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip = 'Self'
	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A'
	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = 'True'

	select RelationShip, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average , 'A' as GrpOrder
	from
	(
		select * from #tempself
	) as t1
	Group By RelationShip
	
	drop table #tempself
END
GO
