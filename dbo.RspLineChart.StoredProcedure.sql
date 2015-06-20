USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspLineChart]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspLineChart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspLineChart]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspLineChart]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the LineChart>
-- =============================================
-- [RspLineChart] 546, ''Line Manager'',''0'',''1'',''0'',''1''
-- [RspLineChart] 639, ''Line Manager,Peer'',''0'',''1'',''0'',''1''
CREATE PROCEDURE [dbo].[RspLineChart] 
	@targetpersonid int,      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *	
	@grp Varchar(500),
	@fullprjgrpvisibility Varchar(5),
	@selfvisibility Varchar(5),
	@programmevisibility Varchar(5),
	@benchmarkvisibility Varchar(5)
AS
BEGIN	
	select * into #temp1 from dbo.fn_CSVToTable(@grp)
	
	declare @UpperBound int
	SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 	FROM dbo.AssignQuestionnaire INNER JOIN
            dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
	WHERE   (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend	
		select t1.RelationShip,Sequence,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average , ''   '' as GrpOrder,@UpperBound as UpperBound
		into #tempdetail from
		(	
		select ''                                             '' as Relationship,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),''&'','''') as Answer ,
		count(*) as "No Of Candidate",UpperBound
		from AssignQuestionnaire aq 
		left join Category c on c.AccountID = aq.AccountID
		left join Question q on q.CateogryId = c.CategoryId
		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
		left join Account a on a.AccountID = aq.AccountID
		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
		left join [User] u on u.UserID = aq.TargetPersonID
		where  1=0 and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
		and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
						where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
		Group By RelationShip,c.Sequence,c.CategoryName,answer,UpperBound
		) as t1	
		group by t1.RelationShip,t1.Sequence,t1.CategoryName,UpperBound	
		
	--All Below Blocks will be used for Insertion in same #tempdetail table
	IF(@selfvisibility = ''1'')
	BEGIN
		BEGIN TRAN		--self	
		
			SELECT Relationship,c.CategoryName, c.Sequence,
  			REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), ''&'', '''') AS Answer, UpperBound
			into #tempSelf FROM QuestionAnswer qa
			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
			left join Question q ON qa.QuestionID = q.QuestionID 
			left join Category c ON q.CateogryID = c.CategoryID
			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = ''True'') AND (qa.Answer <> ''N/A'' and qa.Answer != '' '') 
			and ad.RelationShip = ''Self'' and c.ExcludeFromAnalysis = 0
			ORDER BY c.Sequence				
			
			insert into #tempdetail		
			select RelationShip,Sequence, CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average , ''0'' as GrpOrder,@UpperBound as UpperBound
			from
			(
				select * from #tempSelf
			) as t1
			Group By RelationShip,Sequence, CategoryName,UpperBound	

			drop table #tempSelf	
		COMMIT TRAN
	END
	
	 --Apart from Self : This Block Will Always Run
	select #temp1.Id,RelationShip ,c.Sequence,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),''&'','''') as Answer,UpperBound	
	into #tempapartself from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	--left join [User] u on u.UserID = aq.TargetPersonID
	left join #temp1 on #temp1.[Value]=ad.RelationShip
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != '' '' --and ad.RelationShip != ''Self''
	and aq.TargetPersonID = @targetpersonid and qa.answer !=''N/A'' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID  and ad.SubmitFlag = ''True''

	insert into #tempdetail	
	select RelationShip,t1.Sequence,CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average , Id as GrpOrder,@UpperBound as UpperBound
	from
	(
		select * from #tempapartself
	) as t1
	Group By Id,RelationShip,t1.Sequence, CategoryName,UpperBound
	Order By t1.Sequence 
	
	drop table #tempapartself

	IF(@benchmarkvisibility = ''1'')
	BEGIN
		insert into #tempdetail
		select 	BenchmarkName as RelationShip,c.Sequence,CategoryName,Score as "Sum",''1'' as "No Of Candidate",
		Score as "Average",''96'' as GrpOrder,@UpperBound as UpperBound
		from ParticipantBenchmark pb
		left join ParticipantBenchmarkDetails pbd on pbd.BenchmarkID = pb.BenchmarkID
		left join Category c on c.CategoryID = pbd.CategoryID
		where pb.TargetPersonID = @targetpersonid and c.ExcludeFromAnalysis = 0 
		order by c.Sequence
	END	

	
	-- Want AssignmentID by ProgrammeID by TargetPersonID
	IF(@programmevisibility =''1'')
	BEGIN
		BEGIN TRAN	-- same Programme : Participant + Candidates will be included here.
			Declare @prgid int
			set @prgid = null
			select @prgid = ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid
			
			Declare @prgname varchar(500)
			set @prgname = null
			select @prgname = ProgrammeName from Programme where ProgrammeID = @prgid			
			
			SELECT @prgname as Relationship,c.CategoryName, c.Sequence,
		    REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), ''&'', '''') AS Answer, UpperBound
			into #tempprg FROM QuestionAnswer qa
			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
			left join Question q ON qa.QuestionID = q.QuestionID 
			left join Category c ON q.CateogryID = c.CategoryID
			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProgrammeID = @prgid ))) 
			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 1) AND (qa.Answer <> ''N/A'' and qa.Answer != '' '')
			and c.ExcludeFromAnalysis = 0
			ORDER BY c.Sequence
		
			insert into #tempdetail	
			select t1.RelationShip,t1.Sequence,t1.CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average , ''98'' as GrpOrder,@UpperBound as UpperBound
			from
			(	
				select * from #tempprg
			) as t1	
			group by t1.RelationShip,t1.Sequence,t1.CategoryName,UpperBound	
			order by t1.Sequence	
			
			drop table #tempprg	
		COMMIT TRAN
	END
	
	-- Want AssignmentID by ProjectID by TargetPersonID
	IF(@fullprjgrpvisibility =''1'')
	BEGIN
		BEGIN TRAN	-- full Project
		
			Declare @prjid int
			set @prjid = null
			Declare @QuestionnaireID int
			select @prjid = ProjecctID,@QuestionnaireID=QuestionnaireID from AssignQuestionnaire where TargetPersonID = @targetpersonid
		
			SELECT ''Full Project Group'' as Relationship,c.CategoryName, c.Sequence,
            REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), ''&'', '''') AS Answer, UpperBound
			into #tempfull FROM QuestionAnswer qa
			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
			left join Question q ON qa.QuestionID = q.QuestionID 
			left join Category c ON q.CateogryID = c.CategoryID
			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProjecctID = @prjid))) 
			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = ''True'') AND (qa.Answer <> ''N/A'' and qa.Answer != '' '')
			and c.ExcludeFromAnalysis = 0
			and c.QuestionnaireID=@QuestionnaireID
			ORDER BY c.Sequence
		
		
		
			insert into #tempdetail	
			select t1.RelationShip,t1.Sequence,t1.CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average , ''99'' as GrpOrder,@UpperBound as UpperBound
			from
			(	
				select * from #tempfull		
			) as t1	
			group by t1.RelationShip,t1.Sequence,t1.CategoryName,UpperBound	
			order by t1.Sequence
			
			drop table #tempfull	
		COMMIT TRAN
	END
	
	-- Showing reselt set to Report
	select * from #tempdetail
	
	-- then droping Table
	drop table #tempdetail	
	drop table #temp1
END

--
--BEGIN	
--	select * into #temp1 from dbo.fn_CSVToTable(@grp)
	
--	declare @UpperBound int
--	SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 	FROM dbo.AssignQuestionnaire INNER JOIN
--            dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
--	WHERE   (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
--	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend	
--		select t1.RelationShip,Sequence,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--		cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , ''   '' as GrpOrder,@UpperBound as UpperBound
--		into #tempdetail from
--		(	
--		select ''                                             '' as Relationship,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),''&'','''') as Answer ,
--		count(*) as "No Of Candidate",UpperBound
--		from AssignQuestionnaire aq 
--		left join Category c on c.AccountID = aq.AccountID
--		left join Question q on q.CateogryId = c.CategoryId
--		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--		left join Account a on a.AccountID = aq.AccountID
--		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
--		left join [User] u on u.UserID = aq.TargetPersonID
--		where  1=0 and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
--		and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
--						where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
--		Group By RelationShip,c.Sequence,c.CategoryName,answer,UpperBound
--		) as t1	
--		group by t1.RelationShip,t1.Sequence,t1.CategoryName,UpperBound	
		
--	--All Below Blocks will be used for Insertion in same #tempdetail table
--	IF(@selfvisibility = ''1'')
--	BEGIN
--		BEGIN TRAN		--self	
		
--			select RelationShip ,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( Answer ,0 , 3 ),''&'','''') as Answer,UpperBound	
--			into #tempSelf
--			from Account a
--			left join Category c on c.AccountID = a.AccountID
--			left join Question q on q.CateogryId = c.CategoryId
--			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--			left join [User] u on u.UserID = aq.TargetPersonID
--			where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != '' '' and ad.RelationShip = ''Self''
--			and aq.TargetPersonID = @targetpersonid and qa.answer !=''N/A'' and c.ExcludeFromAnalysis = 0 
--			and c.QuestionnaireID = aq.QuestionnaireID  and ad.SubmitFlag = ''True''
--			order by c.Sequence
			
--			insert into #tempdetail		
--			select RelationShip,Sequence,
--			CategoryName,
--			--t1.Answer,
--			sum(cast(t1.Answer as int)) as "Sum", 
--			count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , ''0'' as GrpOrder,@UpperBound as UpperBound
--			from
--			(
--			select * from #tempSelf
--			) as t1
--			Group By RelationShip,Sequence, CategoryName,UpperBound	

--			drop table #tempSelf	
--		COMMIT TRAN
--	END
	
--	-- Apart from Self : This Block Will Always Run
--	select #temp1.Id,RelationShip ,c.Sequence,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),''&'','''') as Answer,UpperBound	
--	into #tempapartself from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	left join [User] u on u.UserID = aq.TargetPersonID
--	inner join #temp1 on #temp1.[Value]=ad.RelationShip
--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != '' '' and ad.RelationShip != ''Self''
--	and aq.TargetPersonID = @targetpersonid and qa.answer !=''N/A'' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
--	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID  and ad.SubmitFlag = ''True''
	
--	insert into #tempdetail	
--	select RelationShip,t1.Sequence,CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , Id as GrpOrder,@UpperBound as UpperBound
--	from
--	(
--		select * from #tempapartself
--	) as t1
--	Group By Id,RelationShip,t1.Sequence, CategoryName,UpperBound
--	Order By t1.Sequence 
	
--	drop table #tempapartself
	
--	-- Want AssignmentID by ProgrammeID by TargetPersonID
--	IF(@programmevisibility =''1'')
--	BEGIN
--		BEGIN TRAN	-- same Programme : Participant + Candidates will be included here.
--			insert into #tempdetail	
--			select t1.RelationShip,t1.Sequence,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , ''98'' as GrpOrder,@UpperBound as UpperBound
--			from
--			(	
--			select p.ProgrammeName as Relationship,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),''&'','''') as Answer ,
--			count(*) as "No Of Candidate",UpperBound
--			from AssignQuestionnaire aq 
--			left join Category c on c.AccountID = aq.AccountID
--			left join Question q on q.CateogryId = c.CategoryId
--			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--			left join Account a on a.AccountID = aq.AccountID
--			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
--			left join [User] u on u.UserID = aq.TargetPersonID
--			left join Programme p on p.ProgrammeID = aq.ProgrammeID
--			where  q.QuestionTypeID = 2 and qa.answer !=''N/A'' and qa.Answer != '' '' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
--			and aq.ProgrammeID IN (select Distinct ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid)
--			and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
--							where ProgrammeID IN (select ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
--							and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID  and ad.SubmitFlag = ''True''
--			Group By p.ProgrammeName,c.Sequence,c.CategoryName,answer,UpperBound
--			) as t1	
--			group by t1.RelationShip,t1.Sequence,t1.CategoryName,UpperBound	
--			order by t1.Sequence							
				
--		COMMIT TRAN
--	END
	
--	-- Want AssignmentID by ProjectID by TargetPersonID
--	IF(@fullprjgrpvisibility =''1'')
--	BEGIN
--		BEGIN TRAN	-- full Project
--			insert into #tempdetail	
--			select t1.RelationShip,t1.Sequence,t1.CategoryName, sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , ''99'' as GrpOrder,@UpperBound as UpperBound
--			from
--			(	
--			select ''Full Project Group'' as Relationship,c.Sequence,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),''&'','''') as Answer ,
--			count(*) as "No Of Candidate",UpperBound
--			from AssignQuestionnaire aq 
--			left join Category c on c.AccountID = aq.AccountID
--			left join Question q on q.CateogryId = c.CategoryId
--			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--			left join Account a on a.AccountID = aq.AccountID
--			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
--			left join [User] u on u.UserID = aq.TargetPersonID
--			where  q.QuestionTypeID = 2 and qa.answer !=''N/A'' and qa.Answer != '' '' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
--			and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
--							where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
--							and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID  and ad.SubmitFlag = ''True''
--			Group By RelationShip,c.Sequence,c.CategoryName,answer,UpperBound
--			) as t1	
--			group by t1.RelationShip,t1.Sequence,t1.CategoryName,UpperBound	
--			order by t1.Sequence	
--		COMMIT TRAN
--	END
	
--	-- Showing reselt set to Report
--	select * from #tempdetail
	
--	-- then droping Table
--	drop table #tempdetail	
--	drop table #temp1
--END
--
' 
END
GO
