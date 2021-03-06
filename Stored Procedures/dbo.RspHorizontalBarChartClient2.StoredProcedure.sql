USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspHorizontalBarChartClient2]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspHorizontalBarChartClient2]
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
-- [RspHorizontalBarChartClient2]   703,1,1
-- [RspHorizontalBarChartClient2]   682,1,1
-- [RspHorizontalBarChartClient2]   725,1,1
CREATE PROCEDURE [dbo].[RspHorizontalBarChartClient2] 	
	@targetpersonid int,	
	@fullprjgrpvisibility Varchar(5),	
	@programmevisibility Varchar(5)
AS
BEGIN	

	declare @count int
	--select * into #temp1 from dbo.fn_CSVToTable(@grp)	
	declare @UpperBound int
	SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
	FROM         dbo.AssignQuestionnaire INNER JOIN
                 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
	WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend
	select  t1.RelationShip,t1.CategoryName,Sequence,'   ' as GrpOrder ,t1.DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
	@UpperBound as UpperBound
	into #tempdetail from
	(	
	select '                     ' as DisplayText, '                           ' as Relationship,c.CategoryName,c.Sequence, REPLACE(SUBSTRING ( qa.Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer ,
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
	Group By RelationShip,c.CategoryName,c.Sequence,answer,UpperBound
	) as t1	
	group by t1.DisplayText,t1.RelationShip,t1.CategoryName,Sequence,UpperBound
	
	--All Below Blocks will be used for Insertion in same #tempdetail table
	
	-------------------------------------- |START| -----------------------------------------
	--self
		SELECT '0' as Id,'Self' as DisplayText, Relationship,c.CategoryName, c.Sequence,
  	    REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
		into #tempApartSelf FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 1) AND (qa.Answer <> 'N/A' and qa.Answer != ' ') and ad.RelationShip = 'Self'
		ORDER BY ad.AsgnDetailID
	
			--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
			-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
			declare @selfcount int
			select @selfcount=COUNT(*) from #tempApartSelf			
			IF(@selfcount = 0)
			begin
				insert into #tempApartSelf (Id,DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
				select '0' as Id,'Self' as DisplayText, 'Self' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
				from AssignQuestionnaire aq
				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
				inner join Question q on q.CateogryId = c.CategoryId
				where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
			end

		insert into #tempdetail
		select RelationShip,CategoryName,Sequence,Id as GrpOrder,DisplayText ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,@UpperBound as UpperBound
		from
		(
			select * from #tempApartSelf
		) as t1
		Group By Id,DisplayText,RelationShip, CategoryName,Sequence,UpperBound

		drop table #tempApartSelf
	-------------------------------------- |END| -----------------------------------------

	
	
			
	---------------------------------------- |START| -----------------------------------------
		-- Apart from Self + 'Line Manager' : This Block Will Always Run	
		/*
		*TODO:If there is more than 1 'Line Manager' then in this Query will take First-Line Manager as Feedback 1
			  and 2nd 'Line Manager' as 'Feedback 2' or 'Feedback N' (will come after Feedback 1)
			  So here in First Block will take AsgnDetailID of First Line Manager
		*/
		
		
		SELECT AsgnDetailID
		into #tempLineAsgndetailID FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
		And ad.RelationShip = 'Line Manager'
		group by ad.AsgnDetailID
		ORDER BY ad.AsgnDetailID
		
		SELECT 'Feedback 1' as DisplayText, Relationship,c.CategoryName, c.Sequence,
  	    REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
		into #tempApartSelf1 FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE ad.AsgnDetailID IN (select top 1 * from #tempLineAsgndetailID)
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') and ad.RelationShip = 'Line Manager'
		ORDER BY ad.AsgnDetailID

			
			--TODO: IF There is no Record for LineManager Then Will Insert a 'Blank-Record' for LineManager.
			--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
			-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
			declare @linecount int
			select @linecount=COUNT(*) from #tempApartSelf1			
			IF(@linecount = 0)
			begin
				insert into #tempApartSelf1 (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
				select 'Feedback 1' as DisplayText, 'Line Manager' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
				from AssignQuestionnaire aq
				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
				inner join Question q on q.CateogryId = c.CategoryId
				where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
			end
			
		
		insert into #tempdetail
		select RelationShip,CategoryName,Sequence,'2' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
		@UpperBound as UpperBound
		from
		(
			select * from #tempApartSelf1
		) as t1
		Group By DisplayText,RelationShip, CategoryName,Sequence,UpperBound
		drop table #tempApartSelf1	
	---------------------------------------- |END| -----------------------------------------
	
	---------------------------------------- |START| -----------------------------------------
		-- Apart from Self2 + 'Not Inlcude Line Manager' : This Block Will Always Run But not including Self,Line Manager Relation in this Block
		-- TODO: ad.AsgnDetailID is neccessary to put in group by, this will resolve same name candidate problem because Display text is based on CandidateName 
		-- NotIncludeing'LineManger'+ Apartfrom'LineManger'Group will show
		
		declare @tempc int
		select @tempc = count(*) from #tempLineAsgndetailID
		IF(@tempc = 0)
		begin
			insert into #tempLineAsgndetailID values(0)
		end
		
		--TODO: Creating #temp1 table by CandidateName and then will Give Feedback2,Feedback3,..etc for each candidate
		-- which have ad.SubmitFlag = 'True'
		declare @CandidateNameGrp Varchar(5000)
		select @CandidateNameGrp =  COALESCE(@CandidateNameGrp + ',','') + ad.CandidateName 
		from AssignQuestionnaire aq
		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		where TargetPersonID = @targetpersonid and ad.SubmitFlag = 'True'		
		and ad.RelationShip != 'Self' --and ad.RelationShip != 'Line Manager'
		and ad.AsgnDetailID != (select top 1 * from #tempLineAsgndetailID)
		
		
		
		select * into #temp1 from dbo.fn_CSVToTable(@CandidateNameGrp)		
		
		SELECT (#temp1.Id + 2) as Id, 'Feedback ' +  cast((#temp1.Id + 1 ) as varchar(10)) as DisplayText, Relationship,c.CategoryName, c.Sequence,
  	    REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
		into #tempApartSelf2 FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		left join #temp1 on #temp1.[Value] = ad.CandidateName
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
		--and ad.RelationShip != 'Line Manager' 		
		and ad.RelationShip != 'Self' and ad.AsgnDetailID != (select top 1 * from #tempLineAsgndetailID)
		ORDER BY ad.AsgnDetailID
		
		drop table #tempLineAsgndetailID
				
		insert into #tempdetail
		select RelationShip,CategoryName,Sequence,Id as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
		@UpperBound as UpperBound
		from
		(
			select * from #tempApartSelf2
		) as t1
		Group By Id,DisplayText,RelationShip, CategoryName,Sequence,UpperBound
		drop table #tempApartSelf2	
	---------------------------------------- |END| -------------------------------------------
	
	
	------------------------------------------ |START| -----------------------------------------
		---- it is 'Average' block, here if we are getting two or more rows then
		---- In report we will make 'DisplayText' is group (in previous case we are using 'Relationship' is a group)
		---- in this case report will make 1 row of Two or more rows.	
		SELECT 'Average' as DisplayText,'Average' as Relationship,c.CategoryName, c.Sequence,
  	    REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
		into #tempAverage FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
		and ad.RelationShip != 'Self'
		ORDER BY ad.AsgnDetailID
				
			--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
			-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
			declare @avgcount int
			select @avgcount=COUNT(*) from #tempAverage			
			IF(@avgcount = 0)
			begin
				insert into #tempAverage (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
				select 'Average' as DisplayText, 'Average' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
				from AssignQuestionnaire aq
				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
				inner join Question q on q.CateogryId = c.CategoryId
				where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
			end
				
		insert into #tempdetail
		select RelationShip,CategoryName,Sequence,'1' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,	@UpperBound as UpperBound
		from
		(
			select * from #tempAverage
		) as t1
		Group By DisplayText,RelationShip, CategoryName,Sequence,UpperBound
		drop table #tempAverage
	
	------------------------------------------ |END| -----------------------------------------




	----ToDo: Here We are using the same structure of programme-query for Average.
	---- So no need to use 'IF(@programmevisibility ='1')' this condition because its not progamme block
	---- it is 'Average' block. so we are putting comments here for avearge (in later case you can 
	----  Uncomments this block for only programme block)
	---- Want AssignmentID by ProgrammeID by TargetPersonID
	
	IF(@programmevisibility ='1')
	BEGIN
		BEGIN TRAN	
			-- same Programme : Participant + Candidates will be included here.	
			Declare @prgid int
			select @prgid = ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid
			
			SELECT 'Programme' as DisplayText,'Programme' as Relationship,c.CategoryName, c.Sequence,
		    REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
			into #tempprogramme FROM QuestionAnswer qa
			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
			left join Question q ON qa.QuestionID = q.QuestionID 
			left join Category c ON q.CateogryID = c.CategoryID
			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProgrammeID = @prgid ))) 
			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 1) AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
			ORDER BY ad.AsgnDetailID
			
				--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
				-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
				declare @prgcount int
				set @prgcount = null
				select @prgcount=COUNT(*) from #tempprogramme			
				IF(@prgcount = 0)
				begin
					insert into #tempprogramme (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
					select 'Programme' as DisplayText, 'Programme' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
					from AssignQuestionnaire aq
					inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
					inner join Question q on q.CateogryId = c.CategoryId					
					where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
				end

			insert into #tempdetail
			select t1.RelationShip,t1.CategoryName,Sequence,'98' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,@UpperBound as UpperBound
			from
			(	
				select * from #tempprogramme
			) as t1	
			group by t1.DisplayText,t1.RelationShip,t1.CategoryName,Sequence,UpperBound	

			drop table #tempprogramme
		COMMIT TRAN
	END
	
	-- Want AssignmentID by ProjectID by TargetPersonID
		IF(@fullprjgrpvisibility ='1')
	BEGIN
		BEGIN TRAN	-- full Project	
		
			Declare @prjid int
			select @prjid = ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid
		
			SELECT 'Full Project Group' as DisplayText,'Full Project Group' as Relationship,c.CategoryName, c.Sequence,
            REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
			into #tempproject FROM QuestionAnswer qa
			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
			left join Question q ON qa.QuestionID = q.QuestionID 
			left join Category c ON q.CateogryID = c.CategoryID
			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProjecctID = @prjid))) 
			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
			ORDER BY ad.AsgnDetailID		
	
				--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
				-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
				declare @prjcount int
				select @prjcount=COUNT(*) from #tempproject			
				IF(@prjcount = 0)
				begin
					insert into #tempproject (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
					select 'Full Project Group' as DisplayText, 'Full Project Group' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
					from AssignQuestionnaire aq
					inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
					inner join Question q on q.CateogryId = c.CategoryId
					where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
				end

			insert into #tempdetail
			select t1.RelationShip,t1.CategoryName,Sequence,'99' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
			@UpperBound as UpperBound
			from
			(	
				select * from #tempproject
			) as t1	
			group by t1.DisplayText,t1.RelationShip,t1.CategoryName,Sequence,UpperBound	

			drop table #tempproject			
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

--	declare @count int
--	--select * into #temp1 from dbo.fn_CSVToTable(@grp)	
--	declare @UpperBound int
--	SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
--	FROM         dbo.AssignQuestionnaire INNER JOIN
--                 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
--	WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
--	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend
--	select  t1.RelationShip,t1.CategoryName,Sequence,'   ' as GrpOrder ,t1.DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--	@UpperBound as UpperBound
--	into #tempdetail from
--	(	
--	select '                     ' as DisplayText, '                           ' as Relationship,c.CategoryName,c.Sequence, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
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
--	Group By RelationShip,c.CategoryName,c.Sequence,answer,UpperBound
--	) as t1	
--	group by t1.DisplayText,t1.RelationShip,t1.CategoryName,Sequence,UpperBound
	
--	--All Below Blocks will be used for Insertion in same #tempdetail table
	
--	-------------------------------------- |START| -----------------------------------------
--	--self
--		SELECT '0' as Id,'Self' as DisplayText, Relationship,c.CategoryName, c.Sequence,
--  	    REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--		into #tempApartSelf FROM QuestionAnswer qa
--		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--		left join Question q ON qa.QuestionID = q.QuestionID 
--		left join Category c ON q.CateogryID = c.CategoryID
--		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
--		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 1) AND (qa.Answer <> 'N/A' and qa.Answer != ' ') and ad.RelationShip = 'Self'
--		ORDER BY ad.AsgnDetailID
	
--			--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
--			-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
--			declare @selfcount int
--			select @selfcount=COUNT(*) from #tempApartSelf			
--			IF(@selfcount = 0)
--			begin
--				insert into #tempApartSelf (Id,DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
--				select '0' as Id,'Self' as DisplayText, 'Self' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
--				from AssignQuestionnaire aq
--				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
--				inner join Question q on q.CateogryId = c.CategoryId
--				where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
--			end

--		insert into #tempdetail
--		select RelationShip,CategoryName,Sequence,Id as GrpOrder,DisplayText ,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,@UpperBound as UpperBound
--		from
--		(
--			select * from #tempApartSelf
--		) as t1
--		Group By Id,DisplayText,RelationShip, CategoryName,Sequence,UpperBound

--		drop table #tempApartSelf
--	-------------------------------------- |END| -----------------------------------------

	
	
			
--	---------------------------------------- |START| -----------------------------------------
--		-- Apart from Self + 'Line Manager' : This Block Will Always Run	
--		SELECT 'Feedback 1' as DisplayText, Relationship,c.CategoryName, c.Sequence,
--  	    REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--		into #tempApartSelf1 FROM QuestionAnswer qa
--		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--		left join Question q ON qa.QuestionID = q.QuestionID 
--		left join Category c ON q.CateogryID = c.CategoryID
--		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
--		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') and ad.RelationShip = 'Line Manager'
--		ORDER BY ad.AsgnDetailID
			
--			--TODO: IF There is no Record for LineManager Then Will Insert a 'Blank-Record' for LineManager.
--			--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
--			-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
--			declare @linecount int
--			select @linecount=COUNT(*) from #tempApartSelf1			
--			IF(@linecount = 0)
--			begin
--				insert into #tempApartSelf1 (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
--				select 'Feedback 1' as DisplayText, 'Line Manager' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
--				from AssignQuestionnaire aq
--				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
--				inner join Question q on q.CateogryId = c.CategoryId
--				where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
--			end
			
		
--		insert into #tempdetail
--		select RelationShip,CategoryName,Sequence,'2' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
--		@UpperBound as UpperBound
--		from
--		(
--			select * from #tempApartSelf1
--		) as t1
--		Group By DisplayText,RelationShip, CategoryName,Sequence,UpperBound
--		drop table #tempApartSelf1	
--	---------------------------------------- |END| -----------------------------------------
	
	
--	---------------------------------------- |START| -----------------------------------------
--		-- Apart from Self2 + 'Not Inlcude Line Manager' : This Block Will Always Run But not including Self,Line Manager Relation in this Block
--		-- TODO: ad.AsgnDetailID is neccessary to put in group by, this will resolve same name candidate problem because Display text is based on CandidateName 
--		-- NotIncludeing'LineManger'+ Apartfrom'LineManger'Group will show
		
--		--TODO: Creating #temp1 table by CandidateName and then will Give Feedback2,Feedback3,..etc for each candidate
--		-- which have ad.SubmitFlag = 'True'
--		declare @CandidateNameGrp Varchar(5000)
--		select @CandidateNameGrp =  COALESCE(@CandidateNameGrp + ',','') + ad.CandidateName 
--		from AssignQuestionnaire aq
--		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
--		where TargetPersonID = @targetpersonid and ad.SubmitFlag = 'True'		
--		and ad.RelationShip != 'Self' and ad.RelationShip != 'Line Manager'

--		select * into #temp1 from dbo.fn_CSVToTable(@CandidateNameGrp)		
		
--		SELECT (#temp1.Id + 2) as Id, 'Feedback ' +  cast((#temp1.Id + 1 ) as varchar(10)) as DisplayText, Relationship,c.CategoryName, c.Sequence,
--  	    REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--		into #tempApartSelf2 FROM QuestionAnswer qa
--		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--		left join Question q ON qa.QuestionID = q.QuestionID 
--		left join Category c ON q.CateogryID = c.CategoryID
--		left join #temp1 on #temp1.[Value] = ad.CandidateName
--		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
--		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
--		and ad.RelationShip != 'Line Manager' and ad.RelationShip != 'Self' 
--		ORDER BY ad.AsgnDetailID
		
				
--		insert into #tempdetail
--		select RelationShip,CategoryName,Sequence,Id as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
--		@UpperBound as UpperBound
--		from
--		(
--			select * from #tempApartSelf2
--		) as t1
--		Group By Id,DisplayText,RelationShip, CategoryName,Sequence,UpperBound
--		drop table #tempApartSelf2	
--	---------------------------------------- |END| -------------------------------------------
	
	
--	------------------------------------------ |START| -----------------------------------------
--		---- it is 'Average' block, here if we are getting two or more rows then
--		---- In report we will make 'DisplayText' is group (in previous case we are using 'Relationship' is a group)
--		---- in this case report will make 1 row of Two or more rows.	
--		SELECT 'Average' as DisplayText,'Average' as Relationship,c.CategoryName, c.Sequence,
--  	    REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--		into #tempAverage FROM QuestionAnswer qa
--		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--		left join Question q ON qa.QuestionID = q.QuestionID 
--		left join Category c ON q.CateogryID = c.CategoryID
--		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
--		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
--		and ad.RelationShip != 'Self'
--		ORDER BY ad.AsgnDetailID
				
--			--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
--			-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
--			declare @avgcount int
--			select @avgcount=COUNT(*) from #tempAverage			
--			IF(@avgcount = 0)
--			begin
--				insert into #tempAverage (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
--				select 'Average' as DisplayText, 'Average' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
--				from AssignQuestionnaire aq
--				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
--				inner join Question q on q.CateogryId = c.CategoryId
--				where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
--			end
				
--		insert into #tempdetail
--		select RelationShip,CategoryName,Sequence,'1' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,	@UpperBound as UpperBound
--		from
--		(
--			select * from #tempAverage
--		) as t1
--		Group By DisplayText,RelationShip, CategoryName,Sequence,UpperBound
--		drop table #tempAverage
	
--	------------------------------------------ |END| -----------------------------------------




--	----ToDo: Here We are using the same structure of programme-query for Average.
--	---- So no need to use 'IF(@programmevisibility ='1')' this condition because its not progamme block
--	---- it is 'Average' block. so we are putting comments here for avearge (in later case you can 
--	----  Uncomments this block for only programme block)
--	---- Want AssignmentID by ProgrammeID by TargetPersonID
	
--	IF(@programmevisibility ='1')
--	BEGIN
--		BEGIN TRAN	
--			-- same Programme : Participant + Candidates will be included here.	
--			Declare @prgid int
--			select @prgid = ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid
			
--			SELECT 'Programme' as DisplayText,'Programme' as Relationship,c.CategoryName, c.Sequence,
--		    REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--			into #tempprogramme FROM QuestionAnswer qa
--			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--			left join Question q ON qa.QuestionID = q.QuestionID 
--			left join Category c ON q.CateogryID = c.CategoryID
--			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProgrammeID = @prgid ))) 
--			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 1) AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
--			ORDER BY ad.AsgnDetailID
			
--				--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
--				-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
--				declare @prgcount int
--				set @prgcount = null
--				select @prgcount=COUNT(*) from #tempprogramme			
--				IF(@prgcount = 0)
--				begin
--					insert into #tempprogramme (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
--					select 'Programme' as DisplayText, 'Programme' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
--					from AssignQuestionnaire aq
--					inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
--					inner join Question q on q.CateogryId = c.CategoryId					
--					where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
--				end

--			insert into #tempdetail
--			select t1.RelationShip,t1.CategoryName,Sequence,'98' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,@UpperBound as UpperBound
--			from
--			(	
--				select * from #tempprogramme
--			) as t1	
--			group by t1.DisplayText,t1.RelationShip,t1.CategoryName,Sequence,UpperBound	

--			drop table #tempprogramme
--		COMMIT TRAN
--	END
	
--	-- Want AssignmentID by ProjectID by TargetPersonID
--		IF(@fullprjgrpvisibility ='1')
--	BEGIN
--		BEGIN TRAN	-- full Project	
		
--			Declare @prjid int
--			select @prjid = ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid
		
--			SELECT 'Full Project Group' as DisplayText,'Full Project Group' as Relationship,c.CategoryName, c.Sequence,
--            REPLACE(SUBSTRING(qa.Answer, 0, 3), '&', '') AS Answer, UpperBound
--			into #tempproject FROM QuestionAnswer qa
--			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
--			left join Question q ON qa.QuestionID = q.QuestionID 
--			left join Category c ON q.CateogryID = c.CategoryID
--			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProjecctID = @prjid))) 
--			AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
--			ORDER BY ad.AsgnDetailID		
	
--				--For that First we have to get Count from temp table- #tempApartSelf1 and if count is gretter
--				-- Then Zero Then 'No Insertion' and if Count is Equal to Zero then 'Blank Record Insert.'
--				declare @prjcount int
--				select @prjcount=COUNT(*) from #tempproject			
--				IF(@prjcount = 0)
--				begin
--					insert into #tempproject (DisplayText,RelationShip,CategoryName,Sequence, Answer, UpperBound)
--					select 'Full Project Group' as DisplayText, 'Full Project Group' as RelationShip, CategoryName, c.Sequence , '0' as Answer, '' as UpperBound
--					from AssignQuestionnaire aq
--					inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
--					inner join Question q on q.CateogryId = c.CategoryId
--					where TargetPersonID = @targetpersonid and q.QuestionTypeID = 2
--				end

--			insert into #tempdetail
--			select t1.RelationShip,t1.CategoryName,Sequence,'99' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--			@UpperBound as UpperBound
--			from
--			(	
--				select * from #tempproject
--			) as t1	
--			group by t1.DisplayText,t1.RelationShip,t1.CategoryName,Sequence,UpperBound	

--			drop table #tempproject			
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
