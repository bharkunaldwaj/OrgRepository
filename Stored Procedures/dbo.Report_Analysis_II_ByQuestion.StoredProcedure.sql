USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Analysis_II_ByQuestion]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Analysis_II_ByQuestion]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_Analysis_II_ByQuestion] 
	@accountid int,
	@projectid int,
	@programmeid int
	as
    BEGIN
	DECLARE @cat_detail varchar(25), @loop int
	set @loop=0
	
	DECLARE c2 CURSOR READ_ONLY
    FOR
    
    select distinct  Analysis_II from Survey_Assignmentdetails where AssignmentID in(
   select AssignmentID from Survey_AssignQuestionnaire where ProgrammeID=@programmeid ) and Analysis_I!='0'
    
    -- Select category_detail from survey_analysissheet_category_details where analysis_type='ANALYSIS- II' AND PROGRAMME_id=@programmeid
    OPEN c2
    FETCH NEXT FROM c2 INTO @cat_detail

WHILE @@FETCH_STATUS = 0
BEGIN
if(@loop=0)
    begin
    set @loop= @loop + 1 

Select Identity(int,1,1) as "QuestionNum", q.QuestionID,q.Title as "QuestionTitle", q.Description,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp1 from Account a
	left join Survey_Category c on c.AccountID = a.AccountID
	left join Survey_Question q on q.CateogryId = c.CategoryId
	left join Survey_QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join Survey_AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join Survey_AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID	
	left join [User] u on u.AccountID = aq.AccountID
	left join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
	left join Survey_Project p on p.ProjectID = aq.ProjecctID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and aq.accountID =@accountid and aq.ProgrammeID =@programmeid 		
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' and Analysis_II=@cat_detail
		
		create table #temp6(Analysis_type varchar(25),Question_No int,Answer varchar(50))
		
	insert into #temp6 
	select @cat_detail as 'Analysis_type',QuestionID,
	cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	from (
	select QuestionID,
	cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average 
	from
	(
		select * from #temp1
	) as t1
	Group By QuestionID	
	) as t2
	Group By QuestionID
	Order By QuestionID
	
	drop table #temp1
end
	
	else
	
	
	begin
	set @loop= @loop + 1 
	
	
Select Identity(int,1,1) as "QuestionNum", q.QuestionID,q.Title as "QuestionTitle", q.Description,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp2 from Account a
	left join Survey_Category c on c.AccountID = a.AccountID
	left join Survey_Question q on q.CateogryId = c.CategoryId
	left join Survey_QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join Survey_AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join Survey_AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID	
	left join [User] u on u.AccountID = aq.AccountID
	left join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
	left join Survey_Project p on p.ProjectID = aq.ProjecctID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and aq.accountID = @accountid and aq.ProgrammeID =@programmeid 		
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' and Analysis_II=@cat_detail
	
	insert INTO #temp6 
	
	select @cat_detail as 'Analysis_type',QuestionID,
	cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	from (
	select QuestionID,
	cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average 
	from
	(
		select * from #temp2
	) as t1
	Group By QuestionID	
	) as t2
	Group By QuestionID
	Order By QuestionID
	
	drop table #temp2
	end


FETCH NEXT FROM c2 INTO @cat_detail
END
end
CLOSE c2
DEALLOCATE c2
select  #temp6.Analysis_type,
		#temp6.Answer,
		#temp6.Question_No,
		CategoryName = Survey_Category.CategoryTitle,
		Survey_Question.[Description]
FROM 
#temp6 INNER JOIN Survey_Question  ON #temp6.Question_No=Survey_Question.QuestionID    
        
       INNER JOIN Survey_Category ON Survey_Category.CategoryID=Survey_Question.CateogryID 
       order by Survey_Category.Sequence
      drop table #temp6
GO
