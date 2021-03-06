USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_RspFeedbackByCateogry_cf_or_cg]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_RspFeedbackByCateogry_cf_or_cg]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- [RspFeedbackByCateogry] 38,174,47,'cg'
 --[Survey_RspFeedbackByCateogry_cf_or_cg] 71,206,241,'cf'
CREATE PROCEDURE [dbo].[Survey_RspFeedbackByCateogry_cf_or_cg] 
	@accountid varchar(50),
	@projectid varchar(50),
	@programmeid varchar(50),
	@type varchar(2),
	@companyId int=null
AS
BEGIN

if(@type = 'cg')
	Begin
		
		
		select *  into #temp1 from (
		select c.CategoryID,c.CategoryName,'Full Project Group' as ParticipantName,pg.ProgrammeName,p.Title,
		REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer		
		from dbo.Survey_AssignQuestionnaire aq
		inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Survey_Question q on q.QuestionID = qa.QuestionID
		inner join Survey_Category c on c.CategoryID = q.CateogryID
		inner join [User] u on u.AccountID = aq.AccountID
		inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Survey_Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid --and pg.CompanyID=@companyId
		) t1
		 
		select CategoryName,
		cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as "Full Project Group"
		--cast( sum(Average)/count(Average)as decimal(12,1)) as Answer 
		from (
		select CategoryName,
		cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average 
		from
		(
			select * from #temp1
		) as t1
		Group By CategoryName	
		) as t2
		Group By CategoryName
		Order By CategoryName
		
		drop table #temp1
	end

if(@type = 'cf')
	Begin
		
		
		select *  into #temp2 from (
		select c.CategoryID,c.CategoryName,'Full Project Group' as ParticipantName,pg.ProgrammeName,p.Title,
		REPLACE(SUBSTRING ( Answer ,0 , 3 ),'&','') as Answer		
		from dbo.Survey_AssignQuestionnaire aq
		inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Survey_Question q on q.QuestionID = qa.QuestionID
		inner join Survey_Category c on c.CategoryID = q.CateogryID
		inner join [User] u on u.AccountID = aq.AccountID
		inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Survey_Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid --and pg.CompanyID=@companyId
		) t1
		 
		select CategoryName,
		cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as "Full Project Group"
		--cast( sum(Average)/count(Average)as decimal(12,1)) as Answer 
		from (
		select CategoryName,
		cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average 
		from
		(
			select * from #temp2
		) as t1
		Group By CategoryName	
		) as t2
		Group By CategoryName
		Order By CategoryName
		
		drop table #temp2
	end
	
	IF (@type='cp')
		BEGIN
			DECLARE @AnaType varchar
			--DECLARE @accountid INT
			--DECLARE @programmeid INT
			--DECLARE @projectid INT
			--SET @accountid = 66
			--SET @programmeid = 41
			--SET @projectid = 14
			select 
					c.CategoryID,
					c.CategoryName,
					u.FirstName + ' ' + u.LastName as ParticipantName,
					CandidateName,
					pg.ProgrammeName,
					p.Title,
					c.Sequence,  
					REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
					into #tmp_Programme
					from Account a
					left join Survey_Questionnaire qn on qn.AccountId = a.accountId
					left join Survey_Category c on c.AccountID = a.AccountID and qn.QuestionnaireID = c.QuestionnaireID
					left join Survey_Question q on q.CateogryId = c.CategoryId
					left join Survey_QuestionAnswer qa on qa.QuestionId = q.QuestionId
					left join Survey_AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
					left join Survey_AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID	
					left join [User] u on u.AccountID = aq.AccountID
					left join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
					left join Survey_Project p on p.ProjectID = aq.ProjecctID
					where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
					and aq.accountID =@accountid and aq.ProgrammeID = @programmeid  and 
					CASE	WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I 
							WHEN @AnaType ='ANALYSIS- II ' Then Analysis_II 
							WHEN @AnaType='ANALYSIS- III' Then Analysis_III 
							ELSE Analysis_I 
					END  IN(Select distinct 		
					CASE	WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I 
							WHEN @AnaType ='ANALYSIS- II '  Then Analysis_II 
							WHEN @AnaType='ANALYSIS- III' Then Analysis_III 
							ELSE Analysis_I 
					END  
					from Survey_AssignmentDetails
					inner join Survey_AssignQuestionnaire on Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID 
					WHERE Survey_AssignQuestionnaire.AccountID=@accountid 
							AND Survey_AssignQuestionnaire.ProjecctID=@projectid 
							AND Survey_AssignQuestionnaire.ProgrammeID=@programmeid)
							and qa.answer !='N/A' and ad.SubmitFlag = 'True' order by c.Sequence
				
				
			Select 
					--9999 as 'SR_No',
					--'Programme Average' as 'Analysis_type',
					categoryname, 
					CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer 
			--into	#TEMP9 
			from 
					#tmp_Programme
			group by 
					Categoryname
			
			DROP TABLE #tmp_Programme --, #TEMP9
		END
END
GO
