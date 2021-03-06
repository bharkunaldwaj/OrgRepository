USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspBenchMarkScoreRadarChart]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspBenchMarkScoreRadarChart]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Description:	<This Procedure will be used in Main Report to Display the Categories>
--For the radar graph, as sample attached, we compare the average of the
--scores given for the participant with the average of all scores given for
--this project.
--So for Unilever the sample Al Wilson, Al Wilson is the average of the 10
--people (his Candidates) compared to the Full Project Group, 600 people that
-- =============================================
-- [RspPreviousScoreRadarChartData] 617, 'Line Manager,Peer Group, Customer', 'S'
-- [RspBenchMarkScoreRadarChart] 639, 'P'
-- [RspBenchMarkScoreRadarChart] 639, 'S'
CREATE PROCEDURE [dbo].[RspBenchMarkScoreRadarChart] 
	@targetpersonid int,	
	@Operation char(1)
AS
BEGIN	
	IF (@Operation = 'S')
	BEGIN
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
				
		select RelationShip,t1.Sequence, CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'A' as GrpOrder, UpperBound
		from
		(
			select * from #tempself
		) as t1
		Group By RelationShip, CategoryName, t1.Sequence, UpperBound
		Order By t1.Sequence
		
		Drop Table #tempSelf
	END
	
	IF (@Operation = 'P')
	BEGIN
			declare @UpperBound int
			SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
			FROM         dbo.AssignQuestionnaire INNER JOIN
						 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
			WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
		-- Previous Score
	
		-- TODO: First Get CategoryID From This Query this query is same as case if Self (from this we will
		-- get same categoryname in both case self & Previous case and error will not come at generation of
		-- Image  of Radar-Chart in ReportViewer.aspx.cs page)
		SELECT c.CategoryID		
		into #tempfull FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
		and c.ExcludeFromAnalysis = 0		
		ORDER BY c.Sequence		
		
		select CategoryID into #TempCategoryID	
		from
		(
			select * from #tempfull
		) as t1
		Group By CategoryID	

	select 	BenchmarkName as RelationShip,c.Sequence,CategoryName,
	Score as "Sum",'1' as "No Of Candidate",Score as "Average",'B' as GrpOrder, @UpperBound as UpperBound
	from ParticipantBenchmark pb
	left join ParticipantBenchmarkDetails pbd on pbd.BenchmarkID = pb.BenchmarkID
	left join Category c on c.CategoryID = pbd.CategoryID
	where pb.TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 
	and c.CategoryID in (select CategoryID from #TempCategoryID)
	order by c.Sequence
	
	 Drop Table #TempCategoryID
	 Drop Table #tempfull
	
	END
	
	--IF (@Operation = 'F')
	--BEGIN
	--	-- full Project	
		
	--	select c.CategoryID, u.FirstName +' '+ u.LastName as RelationShip ,c.Sequence, c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	
	--	into #tempfull from Account a
	--	left join Category c on c.AccountID = a.AccountID
	--	left join Question q on q.CateogryId = c.CategoryId
	--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	--	left join [User] u on u.UserID = aq.TargetPersonID
	--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	--	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A'
	--	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID  
	--	and ad.SubmitFlag = 'True'	
		
	--	select CategoryID into #TempCategoryID
	--	from
	--	(
	--		select * from #tempfull
	--	) as t1
	--	Group By RelationShip, CategoryName, CategoryID, t1.Sequence
		
	--	--
		
	--	select 'Full Project Group' as Relationship,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
	--	count(*) as "No Of Candidate"
	--	into #tempnewfull from AssignQuestionnaire aq 
	--	left join Category c on c.AccountID = aq.AccountID
	--	left join Question q on q.CateogryId = c.CategoryId
	--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	--	left join Account a on a.AccountID = aq.AccountID
	--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
	--	left join [User] u on u.UserID = aq.TargetPersonID
	--	where  q.QuestionTypeID = 2 and qa.answer !='N/A' and qa.Answer != ' ' and ad.RelationShip not in (@grp)
	--	and c.CategoryID in (select CategoryID from #TempCategoryID)
	--	and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
	--					where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
	--	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID  
	--	and ad.SubmitFlag = 'True'
	--	Group By RelationShip,c.Sequence, c.CategoryName,answer		
		
		
	--	select t1.RelationShip,t1.Sequence,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	--	cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'B' as GrpOrder
	--	from
	--	(	
	--		select * from #tempnewfull
	--	) as t1	
	--	group by t1.RelationShip,t1.CategoryName,t1.Sequence
		
	--	Drop Table #tempnewfull
	--	Drop Table #TempCategoryID
	--	Drop Table #tempfull
	--END
	
END


--BEGIN	
--	IF (@Operation = 'S')
--	BEGIN
--		select u.FirstName +' '+ u.LastName as RelationShip ,c.Sequence,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	
--		into #tempself from Account a
--		left join Category c on c.AccountID = a.AccountID
--		left join Question q on q.CateogryId = c.CategoryId
--		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--		left join [User] u on u.UserID = aq.TargetPersonID
--		where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
--		and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A'
--		and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID
--		and ad.SubmitFlag = 'True'
			
--		select RelationShip,t1.Sequence, CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--		cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'A' as GrpOrder
--		from
--		(
--			select * from #tempself
--		) as t1
--		Group By RelationShip, CategoryName, t1.Sequence
--		Order By t1.Sequence
		
--		Drop Table #tempSelf
--	END
	
--	IF (@Operation = 'P')
--	BEGIN
--		-- Previous Score
	
--		-- TODO: First Get CategoryID From This Query this query is same as case if Self (from this we will
--		-- get same categoryname in both case self & Previous case and error will not come at generation of
--		-- Image  of Radar-Chart in ReportViewer.aspx.cs page)
--		select c.CategoryID, u.FirstName +' '+ u.LastName as RelationShip ,c.Sequence, c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	
--		into #tempfull from Account a
--		left join Category c on c.AccountID = a.AccountID
--		left join Question q on q.CateogryId = c.CategoryId
--		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--		left join [User] u on u.UserID = aq.TargetPersonID
--		where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
--		and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A'
--		and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID  
--		and ad.SubmitFlag = 'True'	
		
--		select CategoryID into #TempCategoryID
--		from
--		(
--			select * from #tempfull
--		) as t1
--		Group By RelationShip, CategoryName, CategoryID, t1.Sequence
	
--	select 	case
--		when ScoreMonth = 1 then  'Previous Score - January/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 2 then  'Previous Score - February/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 3 then  'Previous Score - March/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 4 then  'Previous Score - April/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 5 then  'Previous Score - May/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 6 then  'Previous Score - June/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 7 then  'Previous Score - July/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 8 then  'Previous Score - August/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 9 then  'Previous Score - September/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 10 then  'Previous Score - October/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 11 then  'Previous Score - November/' + cast(ScoreYear as varchar) 
--		when ScoreMonth = 12 then  'Previous Score - December/' + cast(ScoreYear as varchar) 		
--	end	RelationShip,
--	c.Sequence,CategoryName, Score as "Sum",'1' as "No Of Candidate",
--	Score as "Average",'B' as GrpOrder
--	from ParticipantScore ps
--	left join ParticipantScoreDetails psd on psd.ScoreID = ps.scoreID
--	left join Category c on c.CategoryID = psd.CategoryID
--	where ps.TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 
--	and c.CategoryID in (select CategoryID from #TempCategoryID)
--	order by c.Sequence

--	 Drop Table #TempCategoryID
--	 Drop Table #tempfull
	
--	END
	
--END
GO
