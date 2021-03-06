USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspReport1BenchConclusionSelfMin]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[RspReport1BenchConclusionSelfMin]
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
-- [RspConclusionChartSelfMin] 546, 3
-- [RspReport1BenchConclusionSelfMin] 639, 2 , 1
CREATE PROCEDURE [dbo].[RspReport1BenchConclusionSelfMin] 
	@targetpersonid int
	,@N int
	,@BenchConclusionVisibility varchar(2)
AS
BEGIN
	IF(@BenchConclusionVisibility = '1')
	BEGIN
		DECLARE @catname varchar(50)
		DECLARE @sum varchar(50)
		DECLARE @NoOfCan varchar(50)
		DECLARE @Avg varchar(50)
		DECLARE @GrpOrd varchar(50)

			-- Query For Minimum Average of Category & CategoryName will Pass To Below Query in SELF Relation
			select RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	
			into #tempsubselfmin from Account a
			left join Category c on c.AccountID = a.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
			left join [User] u on u.UserID = aq.TargetPersonID
			where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
			and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' 	
			and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = 'True'

		DECLARE cat_name CURSOR READ_ONLY FOR 
				
			select Top (@N) CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average
			, 'B' as GrpOrder
			from
			(	
				select * from #tempsubselfmin
			) as t1
			Group By CategoryName
			Order by Average asc
			
			
		BEGIN 
		   OPEN cat_name; 
		   IF EXISTS (SELECT 1 FROM sysobjects WHERE xtype='U' AND name='Test1') 
	DROP TABLE Test1 
			Create table Test1(catname varchar(50),summ varchar(50),NoOfCan VARCHAR(50),Avg varchar(50),GrpOrd varchar(50))   
		   
		   FETCH NEXT FROM cat_name INTO @catname,@sum,@NoOfCan,@Avg,@GrpOrd ; 
		   WHILE @@FETCH_STATUS = 0
		   BEGIN		
				INSERT INTO Test1 Values(@catname,@sum,@NoOfCan,@Avg,@GrpOrd);
				FETCH NEXT FROM cat_name INTO @catname,@sum,@NoOfCan,@Avg,@GrpOrd ;
		   END

			select RelationShip ,c.CategoryID ,c.Sequence,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	
			,@targetpersonid as TargetPersonID
			into #tempsubsubselfmin from Account a
			left join Category c on c.AccountID = a.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
			left join [User] u on u.UserID = aq.TargetPersonID
			where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip = 'Self'
			and c.CategoryName IN (Select catname from Test1)
			and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' 	
			and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = 'True'

			select t1.Sequence, CategoryName,cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As SelfScore
			,Score as BenchScore, 'A' as GrpOrder
			into #tempselfmin from
			(					
				select ts.*,pbd.Score from #tempsubsubselfmin ts
				left join ParticipantBenchmark pb on pb.TargetPersonID = ts.TargetPersonID
				left join ParticipantBenchmarkDetails pbd on pbd.BenchmarkID = pb.BenchmarkID
				where ts.CategoryID = pbd.CategoryID
			) as t1
			Group By CategoryName,t1.Sequence ,Score
			Order by SelfScore asc;
						

			select * from #tempselfmin Order by Sequence asc	
			drop table #tempselfmin			
						
						
			--Select * from Test1
			DROP table Test1
		    
			drop table #tempsubselfmin
			drop table #tempsubsubselfmin
		   CLOSE cat_name;    			
		   DEALLOCATE cat_name
		END;
	END	
	Else IF(@BenchConclusionVisibility = '0')
	BEGIN
		select '' as Sequence, '' as CategoryName, '' as SelfScore,'' as BenchScore, '' as GrpOrder
		from [User] where UserID = @targetpersonid
	END	
END





--
--BEGIN


--DECLARE @catname varchar(50)
--DECLARE @sum varchar(50)
--DECLARE @NoOfCan varchar(50)
--DECLARE @Avg varchar(50)
--DECLARE @GrpOrd varchar(50)

----declare @UpperBound int
----SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 
----FROM    dbo.AssignQuestionnaire INNER JOIN dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
----WHERE  (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)

--DECLARE cat_name CURSOR READ_ONLY FOR 
--	-- Query For Minimum Average of Category & CategoryName will Pass To Below Query in SELF Relation
--	select Top (@N) CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average
--	, 'B' as GrpOrder
--	from
--	(
--	select RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	
--	from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	left join [User] u on u.UserID = aq.TargetPersonID
--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
--	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' 	
--	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = 'True'
--	) as t1
--	Group By CategoryName
--	Order by Average asc
--BEGIN 
--   OPEN cat_name; 
--	Create table Test1(catname varchar(50),summ varchar(50),NoOfCan VARCHAR(50),Avg varchar(50),GrpOrd varchar(50))   
   
--   FETCH NEXT FROM cat_name INTO @catname,@sum,@NoOfCan,@Avg,@GrpOrd ; 
--   WHILE @@FETCH_STATUS = 0
--   BEGIN		
--		INSERT INTO Test1 Values(@catname,@sum,@NoOfCan,@Avg,@GrpOrd);
--		FETCH NEXT FROM cat_name INTO @catname,@sum,@NoOfCan,@Avg,@GrpOrd ;
--   END

--	select CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average
--	, 'A' as GrpOrder
--	from
--	(
--	select RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	
--	from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	left join [User] u on u.UserID = aq.TargetPersonID
--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip = 'Self'
--	and c.CategoryName IN (Select catname from Test1)
--	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' 	
--	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = 'True'
--	) as t1
--	Group By CategoryName
--	Order by Average asc;
				
--	--Select * from Test1
--    DROP table Test1
    
--   CLOSE cat_name;    			
--   DEALLOCATE cat_name
--END;

--END
--
GO
