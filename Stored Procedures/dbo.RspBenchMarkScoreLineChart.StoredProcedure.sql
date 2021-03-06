USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspBenchMarkScoreLineChart]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspBenchMarkScoreLineChart]
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
-- [RspBenchMarkScoreLineChart] 546, 'Line Manager',1
-- [RspBenchMarkScoreLineChart] 639, 1
CREATE PROCEDURE [dbo].[RspBenchMarkScoreLineChart] 
	@targetpersonid int,		
	@selfvisibility Varchar(5),
	@BenchMarkVisibility Varchar(2)
AS
BEGIN		
	IF(@BenchMarkVisibility = '1')
	BEGIN	
		declare @UpperBound int
		SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 	FROM dbo.AssignQuestionnaire INNER JOIN
				dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
		WHERE   (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
		
		--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend	
			select t1.RelationShip,Sequence,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , '   ' as GrpOrder,@UpperBound as UpperBound
			into #tempdetail from
			(	
			select '                                             ' as Relationship,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
			count(*) as "No Of Candidate",UpperBound
			from AssignQuestionnaire aq 
			left join Category c on c.AccountID = aq.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join Account a on a.AccountID = aq.AccountID
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
			left join [User] u on u.UserID = aq.TargetPersonID
			where  1=0 
			and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
							where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
			Group By RelationShip,c.Sequence,c.CategoryName,answer,UpperBound
			) as t1	
			group by t1.RelationShip,t1.Sequence,t1.CategoryName,UpperBound	
			
		--All Below Blocks will be used for Insertion in same #tempdetail table
		IF(@selfvisibility = '1')
		BEGIN
			BEGIN TRAN		-- self	+ apart From Self in Same Query		
				-- Here We are removing a condition "and ad.RelationShip = 'Self'" if we will do this then
				-- Result will come for SELF+ApartFromSelf - RelationShip. in Same Query
				Declare @usrame varchar(500)
				set @usrame = null
				select @usrame = FirstName +' '+ LastName from [User] where UserID = @targetpersonid
				
				SELECT @usrame as Relationship,c.CategoryName, c.Sequence,
  				REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
				into #tempSelf FROM QuestionAnswer qa
				left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
				left join Question q ON qa.QuestionID = q.QuestionID 
				left join Category c ON q.CateogryID = c.CategoryID
				WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
				AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
				and c.ExcludeFromAnalysis = 0
				ORDER BY c.Sequence	
				
				insert into #tempdetail		
				select RelationShip,Sequence, CategoryName,	sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
				cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , '0' as GrpOrder,@UpperBound as UpperBound
				from
				(
					select * from #tempSelf
				) as t1
				Group By RelationShip,Sequence, CategoryName,UpperBound	

				drop table #tempSelf	
			COMMIT TRAN
		END
		
		
		-- This Block: use to Fetch Previous Score Only in this Graph
		
		insert into #tempdetail
		select 	BenchmarkName as RelationShip,c.Sequence,CategoryName,
		Score as "Sum",'1' as "No Of Candidate",Score as "Average",'96' as GrpOrder,@UpperBound as UpperBound
		from ParticipantBenchmark pb
		left join ParticipantBenchmarkDetails pbd on pbd.BenchmarkID = pb.BenchmarkID
		left join Category c on c.CategoryID = pbd.CategoryID
		where pb.TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 
		order by c.Sequence
		
		
		-- Showing reselt set to Report
		select * from #tempdetail
		
		-- then droping Table
		drop table #tempdetail		
	END	
	Else IF(@BenchMarkVisibility = '0')
	BEGIN
		select '' as RelationShip,'' as Sequence, '' as CategoryName, '' as "Sum", '' as  "No Of Candidate",
		'' as Average, '' as GrpOrder,'' as UpperBound
		from [User] where UserID = @targetpersonid
	END			
END
GO
