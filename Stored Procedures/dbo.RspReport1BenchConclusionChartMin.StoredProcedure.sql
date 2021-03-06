USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspReport1BenchConclusionChartMin]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspReport1BenchConclusionChartMin]
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
-- [RspReport1BenchConclusionChartMin] 639, 2,1
CREATE PROCEDURE [dbo].[RspReport1BenchConclusionChartMin] 
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
		into #tempsubmin from Account a
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
		into #tempmin from
		(
			select * from #tempsubmin
		) as t1
		Group By CategoryName,t1.Sequence
		Order by Average asc
		
		select * from #tempmin Order by Sequence asc	
		drop table #tempmin
		drop table #tempsubmin
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
	
----TODO : In this Procedure we wil show Only Self Score in Table of Report & BenchScore will be in 
--	--       HorizontalBarChart, Average is coming here to Get the Lowest Difference on the basis of 
--	--       these difference (need to do this: Order by Average asc) report will generate
		
--		declare @UpperBound int
--		SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
--		FROM         dbo.AssignQuestionnaire INNER JOIN
--					 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
--		WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
		
--		-- apart from self	
--		SELECT Relationship, c.CategoryID ,c.CategoryName, c.Sequence,
--		REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, @targetpersonid as TargetPersonID
--		into #tempsubmin 
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
--		into #tempmin from
--		(
--			select ts.*,pbd.Score from #tempsubmin ts
--			left join ParticipantBenchmark pb on pb.TargetPersonID = ts.TargetPersonID
--			left join ParticipantBenchmarkDetails pbd on pbd.BenchmarkID = pb.BenchmarkID
--			where ts.CategoryID = pbd.CategoryID
--		) as t1
--		Group By CategoryName,t1.Sequence,Score
--		Order by Average asc
		
		
--		select * from #tempmin Order by Sequence asc	
--		drop table #tempmin
--		drop table #tempsubmin
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
