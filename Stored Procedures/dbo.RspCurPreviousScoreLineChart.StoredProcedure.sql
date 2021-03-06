USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurPreviousScoreLineChart]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurPreviousScoreLineChart]
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
-- [RspCurPreviousScoreLineChart] 1569, 1,1
-- [RspPreviousScoreLineChart] 617, 'Line Manager',1
CREATE PROCEDURE [dbo].[RspCurPreviousScoreLineChart] 
	@targetpersonid int,  	
	@selfvisibility Varchar(5),
	@PreScoreVisibility Varchar(2)
AS
BEGIN	
	IF(@PreScoreVisibility = '1')
	BEGIN	
	--select * into #temp1 from dbo.fn_CSVToTable(@grp)
	
		declare @UpperBound int
		SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 	FROM dbo.AssignQuestionnaire INNER JOIN
				dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
		WHERE   (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
		
		--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend	
			select t1.RelationShip,Sequence,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , '   ' as GrpOrder,@UpperBound as UpperBound
			into #tempdetail from
			(	
			select '                                             ' as Relationship,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer ,
			count(*) as "No Of Candidate",UpperBound
			from AssignQuestionnaire aq 
			left join Category c on c.AccountID = aq.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join Account a on a.AccountID = aq.AccountID
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
			left join [User] u on u.UserID = aq.TargetPersonID
			where  1=0 --and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
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
		
		
		-- This Block: use to Fetch Previous Score 1 Only in this Graph
		insert into #tempdetail	
			select 	case
			when psd.ScoreMonth = 1 then  'Previous Score 1 - January/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 2 then  'Previous Score 1 - February/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 3 then  'Previous Score 1 - March/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 4 then  'Previous Score 1 - April/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 5 then  'Previous Score 1 - May/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 6 then  'Previous Score 1 - June/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 7 then  'Previous Score 1 - July/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 8 then  'Previous Score 1 - August/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 9 then  'Previous Score 1 - September/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 10 then  'Previous Score 1- October/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 11 then  'Previous Score 1 - November/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 12 then  'Previous Score 1 - December/' + cast(psd.ScoreYear as varchar) 		
		end	RelationShip,
		--c.Sequence,CategoryName,'96' as GrpOrder, Score as "Sum",'1' as "No Of Candidate",
		--Score as "Average",1 as UpperBound
		c.Sequence,CategoryName, psd.Score as "Sum",'1' as "No Of Candidate",
		psd.Score as "Average",'96' as GrpOrder,@UpperBound as UpperBound
		from ParticipantScore ps
		left join ParticipantScoreDetails psd on psd.ScoreID = ps.scoreID
		left join Category c on c.CategoryID = psd.CategoryID
		where ps.TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 and psd.ScoreType=1
		order by c.Sequence
		
	
	-- This Block: use to Fetch Previous Score 2 Only in this Graph
		insert into #tempdetail	
			select 	case
			when psd.ScoreMonth = 1 then  'Previous Score 2 - January/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 2 then  'Previous Score 2 - February/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 3 then  'Previous Score 2 - March/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 4 then  'Previous Score 2 - April/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 5 then  'Previous Score 2 - May/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 6 then  'Previous Score 2 - June/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 7 then  'Previous Score 2 - July/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 8 then  'Previous Score 2 - August/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 9 then  'Previous Score 2 - September/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 10 then  'Previous Score 2- October/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 11 then  'Previous Score 2 - November/' + cast(psd.ScoreYear as varchar) 
			when psd.ScoreMonth = 12 then  'Previous Score 2 - December/' + cast(psd.ScoreYear as varchar) 		
		end	RelationShip,
		--c.Sequence,CategoryName,'97' as GrpOrder, Score as "Sum",'1' as "No Of Candidate",
		--Score as "Average",1 as UpperBound
		c.Sequence,CategoryName, psd.Score as "Sum",'1' as "No Of Candidate",
		psd.Score as "Average",'97' as GrpOrder,@UpperBound as UpperBound
		from ParticipantScore ps
		left join ParticipantScoreDetails psd on psd.ScoreID = ps.scoreID
		left join Category c on c.CategoryID = psd.CategoryID
		where ps.TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 and psd.ScoreType=2
		order by c.Sequence
		
		-- Showing reselt set to Report
		select * from #tempdetail
		
		-- then droping Table
		drop table #tempdetail	
		--drop table #temp1
	END	
	Else IF(@PreScoreVisibility = '0')
	BEGIN
		select '' as RelationShip,'' as Sequence, '' as CategoryName, '' as "Sum", '' as  "No Of Candidate",
		'' as Average, '' as GrpOrder,'' as UpperBound
		from [User] where UserID = @targetpersonid
	END		
END
GO
