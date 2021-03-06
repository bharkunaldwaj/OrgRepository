USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspSubHorizontalBarChartClient1]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[RspSubHorizontalBarChartClient1]
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
-- [RspSubHorizontalBarChartClient1]  174, 546,'Line Manager,Peer,Customer',1,1,1
-- [RspSubHorizontalBarChartClient1]  174, 528,'Peer,Line Manager',1,1,1
CREATE PROCEDURE [dbo].[RspSubHorizontalBarChartClient1] 	
	@categoryid int,
	@targetpersonid int,    --   *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500),
	@fullprjgrpvisibility Varchar(5),
	@selfvisibility Varchar(5),
	@programmevisibility Varchar(5)
AS
BEGIN	
	-- Table: tblTempRelations will be use in apart from self because we need to display LineManger in second
	-- and then other relation for Client1 report		
	create table tblTempRelations
	(
	Id int,
	Value varchar(100),
	DisplayText varchar(100)
	)
	
	declare @count int

	select * into #temp1 from dbo.fn_CSVToTable(@grp)	
	declare @UpperBound int
	SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
	FROM         dbo.AssignQuestionnaire INNER JOIN
                 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
	WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend
	select  t1.RelationShip,t1.CategoryName,'   ' as GrpOrder ,t1.DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
	@UpperBound as UpperBound
	into #tempdetail from
	(	
	select '                     ' as DisplayText, '                           ' as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer ,
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
	group by t1.DisplayText,t1.RelationShip,t1.CategoryName,UpperBound
	
	--All Below Blocks will be used for Insertion in same #tempdetail table
	IF(@selfvisibility = '1')
	BEGIN
		BEGIN TRAN		
			-------------------------------------- |START| -----------------------------------------
			--self
			select '0' as Id,'Self' as DisplayText, RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	,UpperBound
			into #tempApartSelf from Account a
			left join Category c on c.AccountID = a.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
			left join [User] u on u.UserID = aq.TargetPersonID
			--inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
			where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip = 'Self'
			and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
			and ad.SubmitFlag = 'True'	

			insert into #tempdetail
			select RelationShip,CategoryName,Id as GrpOrder,DisplayText ,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,@UpperBound as UpperBound
			from
			(
				select * from #tempApartSelf
			) as t1
			Group By Id,DisplayText,RelationShip, CategoryName,UpperBound

			drop table #tempApartSelf
			-------------------------------------- |END| -----------------------------------------
			
			-------------------------------------- |START| -----------------------------------------
			--if Table: #tempdetail having atleast 1 count then we will Insert New dummy row			
			--declare @tempselfcount int
			--select @tempselfcount = COUNT(*) from #tempdetail
			-- This Two Line Block will be use in apart from Self query to increase count by 1:
			-- We Need -  'Self : Feedback 1' & 'Line Manager : Feedback 2' and so on.
			-- Here we are inserting DummyRow with unusable record, to increase the Count (if self block run
			-- then it is compulsory to increase count for apart from self or below block.)		
			--IF(@tempselfcount = '1')
			--BEGIN
					--insert into tblTempRelations values (0,'slf','slf')		
			--END
			-------------------------------------- |END| -----------------------------------------
		
		COMMIT TRAN
	END	
	
		
	-------------------------------------- |START| -----------------------------------------
	-- Apart from Self + 'Line Manager' : This Block Will Always Run
	--@count : to generate Feedback 1 for Line Manager 
	--If @grp does not have 'Line Manager' the #temp1 will not contain Line Manager then Below Query 
	-- will not execute Means Line Manager Block will never ececute
	select @count=COUNT(Id) from tblTempRelations
	insert into tblTempRelations 
	select *,'Feedback ' + cast((@count + 1) as varchar(10)) from #temp1 where value='Line Manager'
	
	--select tblTempRelations.Id,ad.CandidateName as DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	,UpperBound
	
	select tblTempRelations.Id,tblTempRelations.DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	,UpperBound	
	into #tempApartSelf1 from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	left join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'	
	and ad.RelationShip = 'Line Manager'
	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
	and ad.SubmitFlag = 'True'	
	group by tblTempRelations.Id,tblTempRelations.DisplayText ,RelationShip ,c.CategoryName, Answer,UpperBound
	
	--group by tblTempRelations.Id,ad.AsgnDetailID,ad.CandidateName ,RelationShip ,c.CategoryName, Answer,UpperBound
	
	insert into #tempdetail
	select RelationShip,CategoryName,Id as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
	@UpperBound as UpperBound
	from
	(
		select * from #tempApartSelf1
	) as t1
	Group By Id,DisplayText,RelationShip, CategoryName,UpperBound
	drop table #tempApartSelf1
	
	
	
	-------------------------------------- |END| -----------------------------------------
	
	
	
	
	-------------------------------------- |START| -----------------------------------------
	-- Apart from Self2 + 'Not Inlcude Line Manager' : This Block Will Always Run But not including Self,Line Manager Relation in this Block
	-- TODO: ad.AsgnDetailID is neccessary to put in group by, this will resolve same name candidate problem because Display text is based on CandidateName 
	select @count=COUNT(Id) from tblTempRelations
	insert into tblTempRelations 
	-- Below: we are adding [Id] + (rn + @count) to increase count
	Select [Id] + cast((rn + @count) as varchar(10)),[Value],Disp + cast((rn + @count) as varchar(10)) from 
	(select 'Feedback ' as Disp ,row_number() over (order by id) as rn, * from #temp1 where value not in('Line Manager','Self')
	) as t1
	
	--select tblTempRelations.Id, ad.CandidateName as DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	,UpperBound
	
	select tblTempRelations.Id, 'Feedback ' +  cast(1 + row_number() over (order by ad.AsgnDetailID)as varchar(10)) as DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	,UpperBound	
	into #tempApartSelf2 from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
	and ad.RelationShip != 'Line Manager' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp)) 
	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
	and ad.SubmitFlag = 'True'		
	group by ad.AsgnDetailID,ad.CandidateName,tblTempRelations.Id,RelationShip ,c.CategoryName, Answer,UpperBound
	
	--group by ad.AsgnDetailID,tblTempRelations.Id,RelationShip ,c.CategoryName, Answer,UpperBound
	
	insert into #tempdetail
	select RelationShip,CategoryName,Id as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
	@UpperBound as UpperBound
	from
	(
		select * from #tempApartSelf2
	) as t1
	Group By Id,DisplayText,RelationShip, CategoryName,UpperBound
	drop table #tempApartSelf2
	
	
	
	-------------------------------------- |END| -------------------------------------------
	
	-------------------------------------- |START| -----------------------------------------
	-- it is 'Average' block, here if we are getting two or more rows then
	-- In report we will make 'DisplayText' is group (in previous case we are using 'Relationship' is a group)
	-- in this case report will make 1 row of Two or more rows.
		
	select 'Average' as DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer	,UpperBound
	into #tempAverage from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID	
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp)) 
	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
	and ad.SubmitFlag = 'True'	
	
	
	insert into #tempdetail
	select RelationShip,CategoryName,'97' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
	@UpperBound as UpperBound
	from
	(
		select * from #tempAverage
	) as t1
	Group By DisplayText,RelationShip, CategoryName,UpperBound
	drop table #tempAverage
	
	
	
	-------------------------------------- |END| -----------------------------------------

	
	-- Want AssignmentID by ProjectID by TargetPersonID
	IF(@fullprjgrpvisibility ='1')
	BEGIN
		BEGIN TRAN	-- full Project
			-- Here Count will be use 
			--select @count=COUNT(*) from tblTempRelations		

			select 'Full Project Group' as DisplayText,'Full Project Group' as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , len(q.UpperBound)+1 ),'&','') as Answer ,
			count(*) as "No Of Candidate",UpperBound
			into #tempproject from AssignQuestionnaire aq 
			left join Category c on c.AccountID = aq.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join Account a on a.AccountID = aq.AccountID
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
			left join [User] u on u.UserID = aq.TargetPersonID
			left join Programme p on p.ProgrammeID = aq.ProgrammeID
			where  q.QuestionTypeID = 2 and qa.answer !='N/A' and qa.Answer != ' ' and CategoryID = @categoryid  
			and ad.RelationShip not in (@grp) 
			and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
							where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid))	
			and ad.SubmitFlag = 'True'
			Group By Relationship,c.CategoryName,answer,UpperBound

			insert into #tempdetail
			select t1.RelationShip,t1.CategoryName,'99' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
			@UpperBound as UpperBound
			from
			(	
				select * from #tempproject
			) as t1	
			group by t1.DisplayText,t1.RelationShip,t1.CategoryName,UpperBound	

			drop table #tempproject
			
		COMMIT TRAN
	END
	
		
	-- Showing reselt set to Report
	select * from #tempdetail
	
	-- then droping Table
	drop table tblTempRelations
	drop table #tempdetail
	drop table #temp1
END




--
--BEGIN	
--	-- Table: tblTempRelations will be use in apart from self because we need to display LineManger in second
--	-- and then other relation for Client1 report		
--	create table tblTempRelations
--	(
--	Id int,
--	Value varchar(100),
--	DisplayText varchar(100)
--	)
	
--	declare @count int

--	select * into #temp1 from dbo.fn_CSVToTable(@grp)	
--	declare @UpperBound int
--	SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
--	FROM         dbo.AssignQuestionnaire INNER JOIN
--                 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
--	WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	
--	--Here Creating Structure of #tempdetail table and if below condition is true then insertion happend
--	select  t1.RelationShip,t1.CategoryName,'   ' as GrpOrder ,t1.DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--	@UpperBound as UpperBound
--	into #tempdetail from
--	(	
--	select '                     ' as DisplayText, '                           ' as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
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
--	group by t1.DisplayText,t1.RelationShip,t1.CategoryName,UpperBound
	
--	--All Below Blocks will be used for Insertion in same #tempdetail table
--	IF(@selfvisibility = '1')
--	BEGIN
--		BEGIN TRAN		
--			-------------------------------------- |START| -----------------------------------------
--			--self
--			select '0' as Id,'Self' as DisplayText, RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	,UpperBound
--			into #tempApartSelf from Account a
--			left join Category c on c.AccountID = a.AccountID
--			left join Question q on q.CateogryId = c.CategoryId
--			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--			left join [User] u on u.UserID = aq.TargetPersonID
--			--inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
--			where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip = 'Self'
--			and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
--			and ad.SubmitFlag = 'True'	

--			insert into #tempdetail
--			select RelationShip,CategoryName,Id as GrpOrder,DisplayText ,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,@UpperBound as UpperBound
--			from
--			(
--				select * from #tempApartSelf
--			) as t1
--			Group By Id,DisplayText,RelationShip, CategoryName,UpperBound

--			drop table #tempApartSelf
--			-------------------------------------- |END| -----------------------------------------
			
--			-------------------------------------- |START| -----------------------------------------
--			--if Table: #tempdetail having atleast 1 count then we will Insert New dummy row			
--			--declare @tempselfcount int
--			--select @tempselfcount = COUNT(*) from #tempdetail
--			-- This Two Line Block will be use in apart from Self query to increase count by 1:
--			-- We Need -  'Self : Feedback 1' & 'Line Manager : Feedback 2' and so on.
--			-- Here we are inserting DummyRow with unusable record, to increase the Count (if self block run
--			-- then it is compulsory to increase count for apart from self or below block.)		
--			--IF(@tempselfcount = '1')
--			--BEGIN
--					insert into tblTempRelations values (0,'slf','slf')		
--			--END
--			-------------------------------------- |END| -----------------------------------------
		
--		COMMIT TRAN
--	END	
	
		
--	-------------------------------------- |START| -----------------------------------------
--	-- Apart from Self + 'Line Manager' : This Block Will Always Run
--	--@count : to generate Feedback 1 for Line Manager 
--	--If @grp does not have 'Line Manager' the #temp1 will not contain Line Manager then Below Query 
--	-- will not execute Means Line Manager Block will never ececute
--	select @count=COUNT(Id) from tblTempRelations
--	insert into tblTempRelations 
--	select *,'Feedback ' + cast((@count + 1) as varchar(10)) from #temp1 where value='Line Manager'
	
--	select tblTempRelations.Id,tblTempRelations.DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	,UpperBound
--	into #tempApartSelf1 from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	left join [User] u on u.UserID = aq.TargetPersonID
--	inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
--	and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable('Line Manager')) 
--	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
--	and ad.SubmitFlag = 'True'	
--	insert into #tempdetail
--	select RelationShip,CategoryName,Id as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
--	@UpperBound as UpperBound
--	from
--	(
--		select * from #tempApartSelf1
--	) as t1
--	Group By Id,DisplayText,RelationShip, CategoryName,UpperBound
--	drop table #tempApartSelf1
--	-------------------------------------- |END| -----------------------------------------
	
	
	
	
--	-------------------------------------- |START| -----------------------------------------
--	-- Apart from Self2 + 'Not Inlcude Line Manager' : This Block Will Always Run But not including Self,Line Manager Relation in this Block
--	select @count=COUNT(Id) from tblTempRelations
--	insert into tblTempRelations 
--	-- Below: we are adding [Id] + (rn + @count) to increase count
--	Select [Id] + cast((rn + @count) as varchar(10)),[Value],Disp + cast((rn + @count) as varchar(10)) from 
--	(select 'Feedback ' as Disp ,row_number() over (order by id) as rn, * from #temp1 where value not in('Line Manager','Self')
--	) as t1
	
--	select tblTempRelations.Id,tblTempRelations.DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	,UpperBound
--	into #tempApartSelf2 from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	left join [User] u on u.UserID = aq.TargetPersonID
--	inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' and ad.RelationShip != 'Self'
--	and ad.RelationShip != 'Line Manager' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp)) 
--	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
--	and ad.SubmitFlag = 'True'	
	
	
--	insert into #tempdetail
--	select RelationShip,CategoryName,Id as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
--	@UpperBound as UpperBound
--	from
--	(
--		select * from #tempApartSelf2
--	) as t1
--	Group By Id,DisplayText,RelationShip, CategoryName,UpperBound
--	drop table #tempApartSelf2
	
	
	
--	-------------------------------------- |END| -------------------------------------------
	
--	-------------------------------------- |START| -----------------------------------------
--	-- it is 'Average' block, here if we are getting two or more rows then
--	-- In report we will make 'DisplayText' is group (in previous case we are using 'Relationship' is a group)
--	-- in this case report will make 1 row of Two or more rows.
		
--	select 'Average' as DisplayText,RelationShip ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer	,UpperBound
--	into #tempAverage from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	left join [User] u on u.UserID = aq.TargetPersonID	
--	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
--	and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp)) 
--	and aq.TargetPersonID = @targetpersonid and qa.answer !='N/A' and CategoryID = @categoryid
--	and ad.SubmitFlag = 'True'	
	
	
--	insert into #tempdetail
--	select RelationShip,CategoryName,'97' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average ,
--	@UpperBound as UpperBound
--	from
--	(
--		select * from #tempAverage
--	) as t1
--	Group By DisplayText,RelationShip, CategoryName,UpperBound
--	drop table #tempAverage
	
	
	
--	-------------------------------------- |END| -----------------------------------------
	
	
	
--	--ToDo: Here We are using the same structure of programme-query for Average.
--	-- So no need to use 'IF(@programmevisibility ='1')' this condition because its not progamme block
--	-- it is 'Average' block. so we are putting comments here for avearge (in later case you can 
--	--  Uncomments this block for only programme block)
--	-- Want AssignmentID by ProgrammeID by TargetPersonID
	
--	----IF(@programmevisibility ='1')
--	----BEGIN
--	----	BEGIN TRAN
	
--	--	-- same Programme : Participant + Candidates will be included here.
--	--	-- Here Count will be use 
--	--	--select @count=COUNT(*) from tblTempRelations		
--	--	-- Here we are inserting DummyRow with unusable record, to increase the Count (if progaramme block run
--	--	-- then it is compulsory to increase count for Projectblock.)		
--	--	--insert into tblTempRelations values (98,'prg','prg')		
	
--	--							-- (@count + 1) is required then You will get Result like this : Feedback 1, 2, 3, 5, 6
--	--							-- Means 1 extra count will come in between 'Dynamic and Programme' Relation [3 - 5 : here 4 miss] 
	
	
--		--select 'Average' as DisplayText,p.ProgrammeName as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
--		--count(*) as "No Of Candidate",UpperBound
--		--into #tempprogramme from AssignQuestionnaire aq 
--		--left join Category c on c.AccountID = aq.AccountID
--		--left join Question q on q.CateogryId = c.CategoryId
--		--left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--		--left join Account a on a.AccountID = aq.AccountID
--		--left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
--		--left join [User] u on u.UserID = aq.TargetPersonID
--		--left join Programme p on p.ProgrammeID = aq.ProgrammeID
--		----inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
--		--where  q.QuestionTypeID = 2 and qa.answer !='N/A' and qa.Answer != ' '  and CategoryID = @categoryid  
--		--and ad.RelationShip not in (@grp) and aq.ProgrammeID IN (select Distinct ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid)
--		--and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
--		--				where ProgrammeID IN (select ProgrammeID from AssignQuestionnaire where TargetPersonID = @targetpersonid ))	
--		--and ad.SubmitFlag = 'True'
--		--Group By p.ProgrammeName,c.CategoryName,answer,UpperBound

--		--insert into #tempdetail
--		--select t1.RelationShip,t1.CategoryName,'98' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--		--cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--		--@UpperBound as UpperBound
--		--from
--		--(	
--		--	select * from #tempprogramme
--		--) as t1	
--		--group by t1.DisplayText,t1.RelationShip,t1.CategoryName,UpperBound	

--		--drop table #tempprogramme
		
		
		
--	----	COMMIT TRAN
--	----END
	
	
--	-- Want AssignmentID by ProjectID by TargetPersonID
--	IF(@fullprjgrpvisibility ='1')
--	BEGIN
--		BEGIN TRAN	-- full Project
--			-- Here Count will be use 
--			--select @count=COUNT(*) from tblTempRelations		

--			select 'Full Project Group' as DisplayText,'Full Project Group' as Relationship,c.CategoryName, REPLACE(SUBSTRING ( qa.Answer ,0 , 3 ),'&','') as Answer ,
--			count(*) as "No Of Candidate",UpperBound
--			into #tempproject from AssignQuestionnaire aq 
--			left join Category c on c.AccountID = aq.AccountID
--			left join Question q on q.CateogryId = c.CategoryId
--			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--			left join Account a on a.AccountID = aq.AccountID
--			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId	
--			left join [User] u on u.UserID = aq.TargetPersonID
--			left join Programme p on p.ProgrammeID = aq.ProgrammeID
--			where  q.QuestionTypeID = 2 and qa.answer !='N/A' and qa.Answer != ' ' and CategoryID = @categoryid  
--			and ad.RelationShip not in (@grp) 
--			and ad.AssignmentID In (select AssignmentID from AssignQuestionnaire
--							where ProjecctID IN (select ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid))	
--			and ad.SubmitFlag = 'True'
--			Group By Relationship,c.CategoryName,answer,UpperBound

--			insert into #tempdetail
--			select t1.RelationShip,t1.CategoryName,'99' as GrpOrder ,DisplayText,  sum(cast(t1.Answer as int)) as "Sum", count(*) as "No Of Candidate",
--			cast(sum(cast(t1.Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average,
--			@UpperBound as UpperBound
--			from
--			(	
--				select * from #tempproject
--			) as t1	
--			group by t1.DisplayText,t1.RelationShip,t1.CategoryName,UpperBound	

--			drop table #tempproject
			
--		COMMIT TRAN
--	END
	
		
--	-- Showing reselt set to Report
--	select * from #tempdetail
	
--	-- then droping Table
--	drop table tblTempRelations
--	drop table #tempdetail
--	drop table #temp1
--END
--
GO
