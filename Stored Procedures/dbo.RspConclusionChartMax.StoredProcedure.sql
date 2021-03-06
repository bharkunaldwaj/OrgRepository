USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspConclusionChartMax]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspConclusionChartMax]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the LineChart>
-- =============================================
--[RspConclusionChartMax] 1299, 2, 1
CREATE PROCEDURE [dbo].[RspConclusionChartMax] 
	@targetpersonid int,
	@N int,
	@ConclusionVisibility varchar(2)
AS
BEGIN
	IF(@ConclusionVisibility = '1')
	BEGIN
		declare @UpperBound int
		SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
		FROM         dbo.AssignQuestionnaire INNER JOIN
					 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
		WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
		-- apart from self	
		select RelationShip ,c.Sequence,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	
		into #tempsubmax from Account a
		left join Category c on c.AccountID = a.AccountID
		left join Question q on q.CateogryId = c.CategoryId
		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
		left join [User] u on u.UserID = aq.TargetPersonID
		where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
		and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' 	
		and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = 'True'
		
		select Top (@N) t1.Sequence, CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average
		, 'B' as GrpOrder,@UpperBound as UpperBound
		into #tempmax from
		(
			select * from #tempsubmax
		) as t1
		Group By CategoryName,t1.Sequence
		Order by Average desc		

		
		select * from #tempmax Order by Sequence asc	
		drop table #tempmax
		drop table #tempsubmax
	END	
	Else IF(@ConclusionVisibility = '0')
	BEGIN
		select '' as Sequence, '' as CategoryName, '' as "Sum", '' as  "No Of Candidate",
		'' as Average, '' as GrpOrder,'' as UpperBound
		from [User] where UserID = @targetpersonid
	END
END
GO
