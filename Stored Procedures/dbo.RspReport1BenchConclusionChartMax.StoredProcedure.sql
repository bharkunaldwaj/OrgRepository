USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspReport1BenchConclusionChartMax]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspReport1BenchConclusionChartMax]
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
-- [RspReport1BenchConclusionChartMax] 639, 2,1
CREATE PROCEDURE [dbo].[RspReport1BenchConclusionChartMax] 
	@targetpersonid int,      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *		
	@N int,
	@BenchConclusionVisibility varchar(2)
AS
BEGIN 
	IF(@BenchConclusionVisibility = '1')
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
	Else IF(@BenchConclusionVisibility = '0')
	BEGIN
		select '' as Sequence, '' as CategoryName, '' as "Sum", '' as  "No Of Candidate",
		'' as Average, '' as GrpOrder,'' as UpperBound
		from [User] where UserID = @targetpersonid
	END	
END

--
--BEGIN 
--	IF(@BenchConclusionVisibility = '1')
--	BEGIN
--	--TODO : In this Procedure we wil show Only Self Score in Table of Report & BenchScore will be in 
--	--       HorizontalBarChart, Average is coming here to Get the highest Difference on the basis of 
--	--       these difference (need to do this: Order by Average desc) report will generate
	
--		declare @UpperBound int
--		SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
--		FROM         dbo.AssignQuestionnaire INNER JOIN
--					 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
--		WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
		
--		-- apart from self	
--		SELECT Relationship, c.CategoryID ,c.CategoryName, c.Sequence,
--		REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, @targetpersonid as TargetPersonID
--		into #tempsubmax 
--		FROM QuestionAnswer qa
--		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--		left join Question q ON qa.QuestionID = q.QuestionID 
--		left join Category c ON q.CateogryID = c.CategoryID
--		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
--		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
--		and ad.RelationShip = 'Self' and c.ExcludeFromAnalysis = 0
--		ORDER BY c.Sequence	
		
--		select Top (@N) t1.Sequence,CategoryName, cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As SelfScore
--		,Score as BenchScore, (cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1)) - Score) as Average
--		,@UpperBound as UpperBound
--		into #tempmax from
--		(
--			select ts.*,pbd.Score from #tempsubmax ts
--			left join ParticipantBenchmark pb on pb.TargetPersonID = ts.TargetPersonID
--			left join ParticipantBenchmarkDetails pbd on pbd.BenchmarkID = pb.BenchmarkID
--			where ts.CategoryID = pbd.CategoryID
--		) as t1
--		Group By CategoryName,t1.Sequence,Score
--		Order by Average desc
		
		
--		select * from #tempmax Order by Sequence asc	
--		drop table #tempmax
--		drop table #tempsubmax
--	END	
--	Else IF(@BenchConclusionVisibility = '0')
--	BEGIN
--		select '' as Sequence, '' as CategoryName, '' as SelfScore, '' as  BenchScore,
--		'' as Average,'' as UpperBound
--		from [User] where UserID = @targetpersonid
--	END	
--END
--
GO
