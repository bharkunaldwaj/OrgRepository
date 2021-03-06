USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspQuestionTypeTextDetailsClient2]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspQuestionTypeTextDetailsClient2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Sub Report to Display the
-- ============================================= 451, 452
-- [RspQuestionTypeTextDetailsClient2] 944,703 
-- [RspQuestionTypeTextDetailsClient2] 446 ,546
CREATE PROCEDURE [dbo].[RspQuestionTypeTextDetailsClient2] 	
	@questionid int,
	@targetpersonid int	
	--@selfvisibility Varchar(5)
AS
BEGIN

	declare @count int
	
	declare @tablecount int  --will use in all blocks(All Query)
	
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
	
	
			--self
			select 'Self' as DisplayText,u.FirstName +' '+ u.LastName as RelationShip,c.CategoryName,
			Answer, count(*) as "No Of Candidate", ad.CandidateName, q.QuestionID,0 as GrpOrder 
			into #tempcheckcount from Account a
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
			
			set @tablecount = null
			select @tablecount=COUNT(*) from #tempcheckcount			
			IF(@tablecount = 0)
			begin
				insert into #tempdetail
				select 'Self' as DisplayText, 'Self' as RelationShip, CategoryName, '        ' as Answer,
				'' as "No Of Candidate",'' as CandidateName,@questionid as QuestionID, 0 as GrpOrder
				from AssignQuestionnaire aq
				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
				inner join Question q on q.CateogryId = c.CategoryId
				where TargetPersonID = @targetpersonid  and  q.QuestionID = @questionid
			end
			else IF(@tablecount > 0)
			begin
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
			end
			drop table #tempcheckcount

		
		
		
		-- Apart from Self : This Block Will Always Run	for only Line Manager relation
			SELECT AsgnDetailID
			into #tempLineAsgndetailID FROM QuestionAnswer qa
			left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
			left join Question q ON qa.QuestionID = q.QuestionID 
			left join Category c ON q.CateogryID = c.CategoryID
			WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
			AND (q.QuestionTypeID = 1) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A')
			And ad.RelationShip = 'Line Manager'
			group by ad.AsgnDetailID
			ORDER BY ad.AsgnDetailID
		
			select 'Feedback 1' as DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
			q.QuestionID, 2 as GrpOrder 
			into #tempApartSelf1  from Account a
			left join Category c on c.AccountID = a.AccountID
			left join Question q on q.CateogryId = c.CategoryId
			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
			left join [User] u on u.UserID = aq.TargetPersonID		
			where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
			and aq.TargetPersonID = @targetpersonid and Answer != 'N/A'
			and ad.RelationShip = 'Line Manager' and ad.SubmitFlag = 'True'
			Group By ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID

			--We are using this @linecount in Next Block of Query 
			declare @linecount int
			select @linecount=COUNT(*) from #tempApartSelf1			
			IF(@linecount = 0)
			begin
				insert into #tempdetail
				select 'Feedback 1' as DisplayText, 'Line Manager' as RelationShip, CategoryName, '        ' as Answer,
				'' as "No Of Candidate",'' as CandidateName,@questionid as QuestionID, 2 as GrpOrder
				from AssignQuestionnaire aq
				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
				inner join Question q on q.CateogryId = c.CategoryId
				where TargetPersonID = @targetpersonid  and  q.QuestionID = @questionid
			end
			else IF(@linecount > 0)
			begin							
				insert into #tempdetail
				select 'Feedback 1' as DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
				q.QuestionID, 2 as GrpOrder 
				from Account a
				left join Category c on c.AccountID = a.AccountID
				left join Question q on q.CateogryId = c.CategoryId
				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
				left join [User] u on u.UserID = aq.TargetPersonID		
				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
				and ad.AsgnDetailID IN (select top 1 * from #tempLineAsgndetailID)and Answer != 'N/A'
				and ad.RelationShip = 'Line Manager' and ad.SubmitFlag = 'True'
				Group By ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID								
			end
			drop table #tempApartSelf1

	
	
		-- Apart from Self2 : This Block Will Always Run But will never including Self,Line Manager Relation in this Block			
		declare @tempc int
		select @tempc = count(*) from #tempLineAsgndetailID
		IF(@tempc = 0)
		begin
			insert into #tempLineAsgndetailID values(0)
		end
		
		declare @CandidateNameGrp Varchar(5000)
		IF(@linecount = 0)
		begin			
			select @CandidateNameGrp =  COALESCE(@CandidateNameGrp + ',','') + ad.CandidateName 
			from AssignQuestionnaire aq
			inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
			where TargetPersonID = @targetpersonid and ad.SubmitFlag = 'True' and ad.RelationShip != 'Self' 
			and ad.RelationShip != 'Line Manager'			
		end
		Else IF(@linecount > 0)
		begin			
			select @CandidateNameGrp =  COALESCE(@CandidateNameGrp + ',','') + ad.CandidateName 
			from AssignQuestionnaire aq
			inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
			where TargetPersonID = @targetpersonid and ad.SubmitFlag = 'True' and ad.RelationShip != 'Self' 
			and ad.AsgnDetailID != (select top 1 * from #tempLineAsgndetailID)			
		end
		
		
		select * into #temp1 from dbo.fn_CSVToTable(@CandidateNameGrp)
			
		insert into #tempdetail
		select 'Feedback ' +  cast((#temp1.Id + 1) as varchar(10)) as DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
		q.QuestionID, (#temp1.Id + 2) as GrpOrder 
		from Account a
		left join Category c on c.AccountID = a.AccountID
		left join Question q on q.CateogryId = c.CategoryId
		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
		left join [User] u on u.UserID = aq.TargetPersonID		
		left join #temp1 on #temp1.[Value] = ad.CandidateName
		where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
		and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
		--and ad.RelationShip != 'Line Manager' 
		and ad.AsgnDetailID != (select top 1 * from #tempLineAsgndetailID)
		and ad.SubmitFlag = 'True'
		Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
	
		drop table #tempLineAsgndetailID
	
	-- Showing reselt set to Report
	select * from #tempdetail  Order by GrpOrder asc,newid()
	
	-- then droping Table	
	drop table #tempdetail
	drop table #temp1
END


--
--BEGIN

--	declare @count int
	
--	declare @tablecount int  --will use in all blocks(All Query)
	
--	--Here Creating Structure of #tempdetail table and if below condition is true then insertion will happend	
--	select '                     ' as DisplayText,'                                               ' as RelationShip,c.CategoryName,
--	Answer, count(*) as "No Of Candidate", ad.CandidateName, q.QuestionID,'0' as GrpOrder 
--	into #tempdetail from Account a
--	left join Category c on c.AccountID = a.AccountID
--	left join Question q on q.CateogryId = c.CategoryId
--	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--	left join [User] u on u.UserID = aq.TargetPersonID
--	where 1 = 0 and a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--	and aq.TargetPersonID = @targetpersonid and ad.RelationShip = 'Self' and Answer != 'N/A'
--	and ad.SubmitFlag = 'True'
--	Group By u.FirstName +' '+ u.LastName ,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
	
	
--			--self
--			select 'Self' as DisplayText,u.FirstName +' '+ u.LastName as RelationShip,c.CategoryName,
--			Answer, count(*) as "No Of Candidate", ad.CandidateName, q.QuestionID,'0' as GrpOrder 
--			into #tempcheckcount from Account a
--			left join Category c on c.AccountID = a.AccountID
--			left join Question q on q.CateogryId = c.CategoryId
--			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--			left join [User] u on u.UserID = aq.TargetPersonID
--			where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--			and aq.TargetPersonID = @targetpersonid and ad.RelationShip = 'Self' and Answer != 'N/A'
--			and ad.SubmitFlag = 'True'
--			Group By u.FirstName +' '+ u.LastName ,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
			
--			set @tablecount = null
--			select @tablecount=COUNT(*) from #tempcheckcount			
--			IF(@tablecount = 0)
--			begin
--				insert into #tempdetail
--				select 'Self' as DisplayText, 'Self' as RelationShip, CategoryName, '        ' as Answer,
--				'' as "No Of Candidate",'' as CandidateName,@questionid as QuestionID, '0' as GrpOrder
--				from AssignQuestionnaire aq
--				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
--				inner join Question q on q.CateogryId = c.CategoryId
--				where TargetPersonID = @targetpersonid  and  q.QuestionID = @questionid
--			end
--			else IF(@tablecount > 0)
--			begin
--				insert into #tempdetail
--				select 'Self' as DisplayText,u.FirstName +' '+ u.LastName as RelationShip,c.CategoryName,
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
--			end
--			drop table #tempcheckcount

		
		
		
--		-- Apart from Self : This Block Will Always Run	for only Line Manager relation
--			select 'Feedback 1' as DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
--			q.QuestionID, '2' as GrpOrder 
--			into #tempApartSelf1  from Account a
--			left join Category c on c.AccountID = a.AccountID
--			left join Question q on q.CateogryId = c.CategoryId
--			left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--			left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--			left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--			left join [User] u on u.UserID = aq.TargetPersonID		
--			where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = 944  
--			and aq.TargetPersonID = @targetpersonid and Answer != 'N/A'
--			and ad.RelationShip = 'Line Manager' and ad.SubmitFlag = 'True'
--			Group By ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID

--			declare @linecount int
--			select @linecount=COUNT(*) from #tempApartSelf1			
--			IF(@linecount = 0)
--			begin
--				insert into #tempdetail
--				select 'Feedback 1' as DisplayText, 'Line Manager' as RelationShip, CategoryName, '        ' as Answer,
--				'' as "No Of Candidate",'' as CandidateName,@questionid as QuestionID, '2' as GrpOrder
--				from AssignQuestionnaire aq
--				inner join Category c on c.QuestionnaireID = aq.QuestionnaireID
--				inner join Question q on q.CateogryId = c.CategoryId
--				where TargetPersonID = @targetpersonid  and  q.QuestionID = @questionid
--			end
--			else IF(@linecount > 0)
--			begin
--				insert into #tempdetail
--				select 'Feedback 1' as DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
--				q.QuestionID, '2' as GrpOrder 
--				from Account a
--				left join Category c on c.AccountID = a.AccountID
--				left join Question q on q.CateogryId = c.CategoryId
--				left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--				left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--				left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--				left join [User] u on u.UserID = aq.TargetPersonID		
--				where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--				and aq.TargetPersonID = @targetpersonid and Answer != 'N/A'
--				and ad.RelationShip = 'Line Manager' and ad.SubmitFlag = 'True'
--				Group By ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
--			end
--			drop table #tempApartSelf1
		
		
--		--insert into #tempdetail
--		--select 'Feedback1' as DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
--		--q.QuestionID, '2' as GrpOrder 
--		--from Account a
--		--left join Category c on c.AccountID = a.AccountID
--		--left join Question q on q.CateogryId = c.CategoryId
--		--left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--		--left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--		--left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--		--left join [User] u on u.UserID = aq.TargetPersonID		
--		--where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--		--and aq.TargetPersonID = @targetpersonid and Answer != 'N/A'
--		--and ad.RelationShip = 'Line Manager' and ad.SubmitFlag = 'True'
--		--Group By ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
	
	
	
	
	
--		-- Apart from Self2 : This Block Will Always Run But will never including Self,Line Manager Relation in this Block			
--		declare @CandidateNameGrp Varchar(5000)
--		select @CandidateNameGrp =  COALESCE(@CandidateNameGrp + ',','') + ad.CandidateName 
--		from AssignQuestionnaire aq
--		inner join AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
--		where TargetPersonID = @targetpersonid and ad.SubmitFlag = 'True' and ad.RelationShip != 'Self' and ad.RelationShip != 'Line Manager'
		
--		select * into #temp1 from dbo.fn_CSVToTable(@CandidateNameGrp)
			
--		insert into #tempdetail
--		select 'Feedback ' +  cast((#temp1.Id + 1) as varchar(10)) as DisplayText,ad.RelationShip,c.CategoryName, Answer, count(*) as "No Of Candidate", ad.CandidateName,
--		q.QuestionID, (#temp1.Id + 2) as GrpOrder 
--		from Account a
--		left join Category c on c.AccountID = a.AccountID
--		left join Question q on q.CateogryId = c.CategoryId
--		left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
--		left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
--		left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
--		left join [User] u on u.UserID = aq.TargetPersonID		
--		left join #temp1 on #temp1.[Value] = ad.CandidateName
--		where a.AccountID = aq.AccountID and q.QuestionTypeID = 1 and  q.QuestionID = @questionid  
--		and aq.TargetPersonID = @targetpersonid and ad.RelationShip != 'Self' and Answer != 'N/A'
--		and ad.RelationShip != 'Line Manager'
--		and ad.SubmitFlag = 'True'
--		Group By Id,ad.RelationShip,c.CategoryName, Answer, ad.CandidateName, q.QuestionID
	
	
--	-- Showing reselt set to Report
--	select * from #tempdetail
	
--	-- then droping Table	
--	drop table #tempdetail
--	drop table #temp1
--END
--
GO
