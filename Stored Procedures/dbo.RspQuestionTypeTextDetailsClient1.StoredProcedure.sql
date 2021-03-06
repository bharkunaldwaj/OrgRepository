USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspQuestionTypeTextDetailsClient1]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspQuestionTypeTextDetailsClient1]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta#tempdetail
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Sub Report to Display the
-- ============================================= 451, 452
-- [RspQuestionTypeTextDetailsClient1] 446 ,546,'Peer,Line Manager,Customer',1
CREATE PROCEDURE [dbo].[RspQuestionTypeTextDetailsClient1] 	
	@questionid int,
	@targetpersonid int,      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500),
	@selfvisibility Varchar(5)
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
	
	--Here Creating Structure of #tempdetail table and if below condition is true then insertion will happend	
	select '                     ' as DisplayText,'                                               ' as RelationShip,c.CategoryName,
	Answer, count(*) as "No Of Candidate", ad.CandidateName, q.QuestionID,0 as GrpOrder 
	into #tempdetail from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	where 1 = 0 and a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
	and aq.TargetPersonID = @targetpersonid and ad.RelationShip = 'Self' and Answer != 'N/A'
	and ad.SubmitFlag = 'True'
	Group By u.FirstName +' '+ u.LastName ,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
	
	
	IF(@selfvisibility = '1')
		BEGIN		
			--self
			-- This Two Line Block will be use in apart from Self query to increase count by 1:
			-- We Need -  'Self : Feedback 1' & 'Line Manager : Feedback 2' and so on.
			-- Here we are inserting DummyRow with unusable record, to increase the Count (if self block run
			-- then it is compulsory to increase count for apart from self or below block.)		
			insert into tblTempRelations values (0,null,null)			
			--insert into tblTempRelations
			--select *,'Feedback 1' from #temp1 where value='Self'
			
			insert into #tempdetail
			select 'Self' as DisplayText,u.FirstName +' '+ u.LastName as RelationShip,c.CategoryName,
			Answer, count(*) as "No Of Candidate", ad.CandidateName, q.QuestionID,0 as GrpOrder 
			from Account a
			left join Category c on c.AccountID = a.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
			left join [User] u on u.UserID = aq.TargetPersonID
			where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
			and aq.TargetPersonID = @targetpersonid and ad.RelationShip = 'Self' and Answer != 'N/A'
			and ad.SubmitFlag = 'True'
			Group By u.FirstName +' '+ u.LastName ,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
		END
		
		-- Apart from Self : This Block Will Always Run
		--@count : to generate Feedback 1 for Line Manager 
		select @count=COUNT(Id) from tblTempRelations
		insert into tblTempRelations 
		select *,'Feedback ' + cast((@count + 1) as varchar(10)) from #temp1 where value='Line Manager'
	
		insert into #tempdetail
		select tblTempRelations.DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
		q.QuestionID, tblTempRelations.Id as GrpOrder 
		from Account a
		left join Category c on c.AccountID = a.AccountID
		left join Question q on q.CateogryId = c.CategoryId
		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
		left join [User] u on u.UserID = aq.TargetPersonID
		inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
		where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
		and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
		and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable('Line Manager'))
		and ad.SubmitFlag = 'True'
		Group By Id,tblTempRelations.DisplayText,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
	
		-- Apart from Self2 : This Block Will Always Run But will never including Self,Line Manager Relation in this Block
		select @count=COUNT(Id) from tblTempRelations
		insert into tblTempRelations 
		-- Below: we are adding [Id] + (rn + @count) to increase count
		Select [Id] + cast((rn + @count) as varchar(10)),[Value],Disp + cast((rn + @count) as varchar(10)) from 
		(select 'Feedback ' as Disp ,row_number() over (order by id) as rn, * from #temp1 where value not in('Line Manager','Self')
		) as t1
		
		insert into #tempdetail
		select tblTempRelations.DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
		q.QuestionID, tblTempRelations.Id as GrpOrder 
		from Account a
		left join Category c on c.AccountID = a.AccountID
		left join Question q on q.CateogryId = c.CategoryId
		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
		left join [User] u on u.UserID = aq.TargetPersonID
		inner join tblTempRelations on tblTempRelations.[Value]=ad.RelationShip
		where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
		and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
		and ad.RelationShip != 'Line Manager' and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
		and ad.SubmitFlag = 'True'
		Group By Id,tblTempRelations.DisplayText,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
	
	
	
	
	
	-- Showing reselt set to Report
	select * from #tempdetail Order by GrpOrder asc, newid()
	
	-- then droping Table
	drop table tblTempRelations
	drop table #tempdetail
	drop table #temp1
END

















--
--BEGIN
--	select * into #temp1 from dbo.fn_CSVToTable(@grp)
--	IF(@selfvisibility = '1')
--		BEGIN
--			BEGIN TRAN
--				--self
--				select 'Feedback 1' as DisplayText,u.FirstName +' '+ u.LastName as RelationShip,c.CategoryName,
--				Answer, count(*) as "No Of Candidate", ad.CandidateName, q.QuestionID,'0' as GrpOrder 
--				from Account a
--				left join Category c on c.AccountID = a.AccountID
--				left join Question q on q.CateogryId = c.CategoryId
--				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--				left join [User] u on u.UserID = aq.TargetPersonID
--				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--				and aq.TargetPersonID = @targetpersonid and ad.RelationShip = 'Self' and Answer != 'N/A'
--				and ad.SubmitFlag = 'True'
--				Group By u.FirstName +' '+ u.LastName ,c.CategoryName, Answer, ad.CandidateName, q.QuestionID

--				--Union 
--				----apart from self
--				--select ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
--				--q.QuestionID, #temp1.Id as GrpOrder 
--				--from Account a
--				--left join Category c on c.AccountID = a.AccountID
--				--left join Question q on q.CateogryId = c.CategoryId
--				--left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--				--left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--				--left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--				--left join [User] u on u.UserID = aq.TargetPersonID
--				--inner join #temp1 on #temp1.[Value]=ad.RelationShip
--				--where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--				--and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
--				--and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
--				--and ad.SubmitFlag = 'True'
--				--Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
				
--			COMMIT TRAN
--		END
--	ELSE     
--		BEGIN
--			BEGIN TRAN	
--				--apart from self
--				select ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
--				q.QuestionID, #temp1.Id as GrpOrder 
--				from Account a
--				left join Category c on c.AccountID = a.AccountID
--				left join Question q on q.CateogryId = c.CategoryId
--				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--				left join [User] u on u.UserID = aq.TargetPersonID
--				inner join #temp1 on #temp1.[Value]=ad.RelationShip
--				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--				and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
--				and ad.RelationShip in (select [Value] from dbo.fn_CSVToTable(@grp))
--				and ad.SubmitFlag = 'True'
--				Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID		
--			COMMIT TRAN
--		END
--END
--
GO
