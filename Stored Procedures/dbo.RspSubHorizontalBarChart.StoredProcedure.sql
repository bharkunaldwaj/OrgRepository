USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspSubHorizontalBarChart]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[RspSubHorizontalBarChart]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Sub Report to Display the Horizontal BarGraph
-- =============================================
-- [RspSubHorizontalBarChart]  174, 546,'Line Manger',1,1,1
-- [RspSubHorizontalBarChart]  175, 526,'Line Manager,Peer',1,1,1
-- [RspSubHorizontalBarChart]  250, 639,'Line Manager,Peer',0,1,0,1,0
-- [RspSubHorizontalBarChart]  250, 639,'Line Manager,Peer',0,1,0,1,1
CREATE PROCEDURE [dbo].[RspSubHorizontalBarChart] 	
	@categoryid int,
	@targetpersonid int,    --   *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500),
	@fullprjgrpvisibility Varchar(5),
	@selfvisibility Varchar(5),
	@programmevisibility Varchar(5),
	@benchmarkvisibility Varchar(5),
	@CategoryBarChartVisibility Varchar(2)
AS
BEGIN	
	IF(@CategoryBarChartVisibility = '1')
	BEGIN		
		declare @UpperBound int
		SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
		FROM         dbo.AssignQuestionnaire INNER JOIN
					 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
		WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
		
		--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend	
		select t1.RelationShip,t1.CategoryName,'   ' as GrpOrder ,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
		Min(cast(Answer as decimal(12,1)))  As Minimum,
		Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
		into #tempdetail from
		(	
		select '                           ' as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
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
					where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid))	
		Group By RelationShip,c.CategoryName,answer,UpperBound
		) as t1	
		group by t1.RelationShip,t1.CategoryName,UpperBound
		
		--All Below Blocks will be used for Insertion in same #tempdetail table
		IF(@selfvisibility = '1')
		BEGIN 	--self	
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
			and ad.RelationShip = 'Self' and c.ExcludeFromAnalysis = 0 and CategoryID = @categoryid
			ORDER BY c.Sequence	
			
			
				--For that First we have to get Count from temp table- #tempSelf and if count is gretter
				-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
				declare @selfcount int
				set @selfcount = null
				select @selfcount=COUNT(*) from #tempSelf			
				IF(@selfcount = 0)
				begin
				
					SELECT Relationship,c.CategoryName, c.Sequence,
					REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
					into #tempcheck FROM QuestionAnswer qa
					left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
					left join Question q ON qa.QuestionID = q.QuestionID 
					left join Category c ON q.CateogryID = c.CategoryID
					WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
					AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
					and ad.RelationShip = 'Self' and c.ExcludeFromAnalysis = 0
					ORDER BY c.Sequence	
				
					set @selfcount = null
					select @selfcount=COUNT(*) from #tempcheck			
					IF(@selfcount > 0)
					begin				
						-- INSERT BLANK RECORD
						insert into #tempSelf (RelationShip,CategoryName,Sequence, Answer, UpperBound)
						select @usrame as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
						--select '0' as Id,'Self' as DisplayText, 'Self' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
						from AssignQuestionnaire aq
						inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
						inner join Question q on q.CateogryId = c.CategoryId
						where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2 
						and c.ExcludeFromAnalysis = 0 and CategoryID = @categoryid
					end	
					drop table #tempcheck
				end
					
			insert into #tempdetail
			select RelationShip,CategoryName,'0' as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
			Min(cast(Answer as decimal(12,1)))  As Minimum,
			Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
			from
			(
				select * from #tempSelf
			) as t1
			Group By RelationShip, CategoryName,UpperBound			

		
			Drop Table #tempSelf
		END	
		
		
		-- Apart from Self : This Block Will Always Run
		select * into #temp1 from dbo.fn_CSVToTable(@grp)
		
		select #temp1.Id,RelationShip ,c.CategoryName,c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer,UpperBound
		into #tempApartSelf from Account a
		left join Category c on c.AccountID = a.AccountID
		left join Question q on q.CateogryId = c.CategoryId
		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID	
		left join #temp1 on #temp1.[Value] = ad.RelationShip
		where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
		and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp)) 
		and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
		and ad.SubmitFlag = 'True'	
			
		DECLARE @rel_name varchar(100)
		declare @apartselfcount int
		DECLARE relation_name CURSOR READ_ONLY FOR
			select Value from #temp1  --where Value != 'Line Manager'	
		BEGIN 
		   OPEN relation_name; 
		   FETCH NEXT FROM relation_name INTO @rel_name ; 
		   WHILE @@FETCH_STATUS = 0
		   BEGIN	
		      
				set @apartselfcount = null;
				select @apartselfcount=COUNT(*) from #tempApartSelf	where RelationShip = @rel_name	
				IF(@apartselfcount = 0)
				begin
				
					select #temp1.Id,RelationShip ,c.CategoryName,c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	,UpperBound
					into #tempcheck1 from Account a
					left join Category c on c.AccountID = a.AccountID
					left join Question q on q.CateogryId = c.CategoryId
					left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
					left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
					left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID	
					inner join #temp1 on #temp1.[Value]=ad.RelationShip
					where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' '
					and ad.RelationShip = @rel_name and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' 
					--and ad.SubmitFlag = 'True'
				
					set @apartselfcount = null;
					select @apartselfcount=COUNT(*) from #tempcheck1	where RelationShip = @rel_name	
					IF(@apartselfcount > 0)
					begin
						insert into #tempApartSelf (Id,RelationShip,CategoryName,Sequence, Answer, UpperBound)
						select #temp1.Id as Id,@rel_name as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound				
						from AssignQuestionnaire aq
						left join Category c on c.QuestionnaireID = aq.QuestionnaireID
						left join Question q on q.CateogryId = c.CategoryId
						left join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
						left join #temp1 on #temp1.[Value]=ad.RelationShip
						where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2 
						and c.ExcludeFromAnalysis = 0 and Value = @rel_name and CategoryID = @categoryid
					end				
					drop table #tempcheck1
				end
				
				
			FETCH NEXT FROM relation_name INTO @rel_name; 
			END				   
		   CLOSE relation_name;    			
		   DEALLOCATE relation_name
		END;	
			
			
		insert into #tempdetail	
		select RelationShip,CategoryName,Id as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
		Min(cast(Answer as decimal(12,1)))  As Minimum,
		Max(cast(Answer as decimal(12,1)))  As Maximum ,@UpperBound as UpperBound
		from
		(
			select * from #tempApartSelf
		) as t1
		Group By Id,RelationShip, CategoryName,UpperBound
		
		drop table #tempApartSelf

		IF(@benchmarkvisibility = '1')
		BEGIN
			insert into #tempdetail
			select 	BenchmarkName as RelationShip,CategoryName,'96' as GrpOrder,
			Score as "Sum",'1' as "No Of Candidate",Score as "Average",
			'0'  As Minimum, '0' As Maximum ,@UpperBound as UpperBound
			from ParticipantBenchmark pb
			left join ParticipantBenchmarkDetails pbd on pbd.BenchmarkID = pb.BenchmarkID
			left join Category c on c.CategoryID = pbd.CategoryID
			where pb.TargetPersonID = @targetpersonid and c.CategoryID = @categoryid and c.ExcludeFromAnalysis = 0 		
		END			
		
		-- Want AssignmentID by ProgrammeID by TargetPersonID
		IF(@programmevisibility ='1')
		BEGIN
			BEGIN TRAN	-- same Programme : Participant + Candidates will be included here.
				Declare @prgid int
				set @prgid = null
				select @prgid = ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid
				
				Declare @prgname varchar(500)
				set @prgname = null
				select @prgname = ProgrammeName from Programme where ProgrammeID = @prgid			
				
				SELECT @prgname as Relationship,c.CategoryName, c.Sequence,
				REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
				into #tempprg FROM QuestionAnswer qa
				left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
				left join Question q ON qa.QuestionID = q.QuestionID 
				left join Category c ON q.CateogryID = c.CategoryID
				WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProgrammeID = @prgid ))) 
				AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 1) AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
				and c.ExcludeFromAnalysis = 0 and CategoryID = @categoryid
				ORDER BY c.Sequence
					
				insert into #tempdetail	
				select t1.RelationShip,t1.CategoryName,'98' as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
				cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
				Min(cast(Answer as decimal(12,1)))  As Minimum,
				Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
				from
				(	
					select * from #tempprg
				) as t1	
				group by t1.RelationShip,t1.CategoryName,UpperBound			
				
				drop table #tempprg
			COMMIT TRAN
		END
		
		-- Want AssignmentID by ProjectID by TargetPersonID
		IF(@fullprjgrpvisibility ='1')
		BEGIN
			BEGIN TRAN	-- full Project
				Declare @prjid int
				set @prjid = null
				select @prjid = ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid
			
				SELECT 'Full Project Group' as Relationship,c.CategoryName, c.Sequence,
				REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
				into #tempfull FROM QuestionAnswer qa
				left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
				left join Question q ON qa.QuestionID = q.QuestionID 
				left join Category c ON q.CateogryID = c.CategoryID
				WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProjecctID = @prjid))) 
				AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
				and c.ExcludeFromAnalysis = 0 and CategoryID = @categoryid
				ORDER BY c.Sequence
			
				insert into #tempdetail
				select t1.RelationShip,t1.CategoryName,'99' as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
				cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
				Min(cast(Answer as decimal(12,1)))  As Minimum,
				Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
				from
				(	
					select * from #tempfull
				) as t1	
				group by t1.RelationShip,t1.CategoryName,UpperBound
				
				drop table #tempfull
			COMMIT TRAN
		END
		
			
		-- Showing reselt set to Report
		select * from #tempdetail
		
		-- then droping Table
		drop table #tempdetail
		drop table #temp1
	END	
	Else IF(@CategoryBarChartVisibility = '0')
	BEGIN
		select '' as RelationShip, '' as CategoryName, '' as GrpOrder, '' as  "Sum",'' as "No Of Candidate",
		'' as Average, '' as Minimum, '' as Maximum, '' as  UpperBound
		from [User] where UserID = @targetpersonid
	END			
END

--
--BEGIN	
--	select * into #temp1 from dbo.fn_CSVToTable(@grp)	
--	declare @UpperBound int
--	SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
--	FROM         dbo.AssignQuestionnaire INNER JOIN
--                 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
--	WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
--	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend	
--	select t1.RelationShip,t1.CategoryName,'   ' as GrpOrder ,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--	Min(cast(Answer as decimal(12,1)))  As Minimum,
--	Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
--	into #tempdetail from
--	(	
--	select '                           ' as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
--	count(*) as "No Of Candidate",UpperBound
--	from AssignQuestionnaire aq 
--	left join Category c on c.AccountID = aq.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join Account a on a.AccountID = aq.AccountID
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
--	left join [User] u on u.UserID = aq.TargetPersonID
--	where  1=0
--	and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
--				where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid))	
--	Group By RelationShip,c.CategoryName,answer,UpperBound
--	) as t1	
--	group by t1.RelationShip,t1.CategoryName,UpperBound
	
--	--All Below Blocks will be used for Insertion in same #tempdetail table
--	IF(@selfvisibility = '1')
--	BEGIN
--		BEGIN TRAN		--self	
--			Declare @usrame varchar(500)
--			set @usrame = null
--			select @usrame = FirstName +' '+ LastName from [User] where UserID = @targetpersonid			
		
--			SELECT @usrame as Relationship,c.CategoryName, c.Sequence,
--  			REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--			into #tempSelf FROM QuestionAnswer qa
--			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--			left join Question q ON qa.QuestionID = q.QuestionID 
--			left join Category c ON q.CateogryID = c.CategoryID
--			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
--			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
--			and ad.RelationShip = 'Self' and c.ExcludeFromAnalysis = 0 and CategoryID = @categoryid
--			ORDER BY c.Sequence	
					
--			insert into #tempdetail
--			select RelationShip,CategoryName,'0' as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--			Min(cast(Answer as decimal(12,1)))  As Minimum,
--			Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
--			from
--			(
--				select * from #tempSelf
--			) as t1
--			Group By RelationShip, CategoryName,UpperBound			
--		COMMIT TRAN
		
--		Drop Table #tempSelf
--	END	
	
	
--	-- Apart from Self : This Block Will Always Run
--	select #temp1.Id,RelationShip ,c.CategoryName,c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	,UpperBound
--	into #tempApartSelf from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	--left join [User] u on u.UserID = aq.TargetPersonID
--	inner join #temp1 on #temp1.[Value]=ad.RelationShip
--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
--	and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp)) 
--	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
--	and ad.SubmitFlag = 'True'	
		
--	insert into #tempdetail	
--	select RelationShip,CategoryName,Id as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
--	Min(cast(Answer as decimal(12,1)))  As Minimum,
--	Max(cast(Answer as decimal(12,1)))  As Maximum ,@UpperBound as UpperBound
--	from
--	(
--		select * from #tempApartSelf
--	) as t1
--	Group By Id,RelationShip, CategoryName,UpperBound
	
--	drop table #tempApartSelf
	
	
	
--	-- Want AssignmentID by ProgrammeID by TargetPersonID
--	IF(@programmevisibility ='1')
--	BEGIN
--		BEGIN TRAN	-- same Programme : Participant + Candidates will be included here.
--			Declare @prgid int
--			set @prgid = null
--			select @prgid = ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid
			
--			Declare @prgname varchar(500)
--			set @prgname = null
--			select @prgname = ProgrammeName from Programme where ProgrammeID = @prgid			
			
--			SELECT @prgname as Relationship,c.CategoryName, c.Sequence,
--		    REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--			into #tempprg FROM QuestionAnswer qa
--			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--			left join Question q ON qa.QuestionID = q.QuestionID 
--			left join Category c ON q.CateogryID = c.CategoryID
--			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProgrammeID = @prgid ))) 
--			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 1) AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
--			and c.ExcludeFromAnalysis = 0 and CategoryID = @categoryid
--			ORDER BY c.Sequence
				
--			insert into #tempdetail	
--			select t1.RelationShip,t1.CategoryName,'98' as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--			Min(cast(Answer as decimal(12,1)))  As Minimum,
--			Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
--			from
--			(	
--				select * from #tempprg
--			) as t1	
--			group by t1.RelationShip,t1.CategoryName,UpperBound			
			
--			drop table #tempprg
--		COMMIT TRAN
--	END
	
--	-- Want AssignmentID by ProjectID by TargetPersonID
--	IF(@fullprjgrpvisibility ='1')
--	BEGIN
--		BEGIN TRAN	-- full Project
--			Declare @prjid int
--			set @prjid = null
--			select @prjid = ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid
		
--			SELECT 'Full Project Group' as Relationship,c.CategoryName, c.Sequence,
--            REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--			into #tempfull FROM QuestionAnswer qa
--			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--			left join Question q ON qa.QuestionID = q.QuestionID 
--			left join Category c ON q.CateogryID = c.CategoryID
--			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProjecctID = @prjid))) 
--			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
--			and c.ExcludeFromAnalysis = 0 and CategoryID = @categoryid
--			ORDER BY c.Sequence
		
--			insert into #tempdetail
--			select t1.RelationShip,t1.CategoryName,'99' as GrpOrder ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--			Min(cast(Answer as decimal(12,1)))  As Minimum,
--			Max(cast(Answer as decimal(12,1)))  As Maximum  ,@UpperBound as UpperBound
--			from
--			(	
--				select * from #tempfull
--			) as t1	
--			group by t1.RelationShip,t1.CategoryName,UpperBound
			
--			drop table #tempfull
--		COMMIT TRAN
--	END
	
		
--	-- Showing reselt set to Report
--	select * from #tempdetail
	
--	-- then droping Table
--	drop table #tempdetail
--	drop table #temp1
--END
--
GO
