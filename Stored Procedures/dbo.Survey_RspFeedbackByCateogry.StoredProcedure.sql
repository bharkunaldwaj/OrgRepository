USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_RspFeedbackByCateogry]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_RspFeedbackByCateogry]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Survey_RspFeedbackByCateogry] 
	@accountid int,
	@projectid int,
	@programmeid int,
	@AnalysisType varchar(100),
	@AnalysisValue varchar(100),
	@companyId int=null
	
	
AS
BEGIN

DECLARE @QuestionnaireID as INT

Select @QuestionnaireID = QuestionnaireID from survey_project where AccountID = @accountid and ProjectID = @projectID

	IF (UPPER(@AnalysisType) = 'ANALYSIS-I' or UPPER(@AnalysisType) = 'ANALYSIS- I' or UPPER(@AnalysisType) = 'Analysis_I' or UPPER(@AnalysisType) = 'ANALYSIS_ I')
	begin
	select c.CategoryID,c.CategoryName,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp1 from Account a
	left join Survey_Category c on c.AccountID = a.AccountID
	left join Survey_Question q on q.CateogryId = c.CategoryId
	left join Survey_QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join Survey_AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join Survey_AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID	
	left join Survey_Questionnaire sq on sq.QuestionnaireID = aq.QuestionnaireID
	left join [User] u on u.AccountID = aq.AccountID
	left join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
	left join Survey_Project p on p.ProjectID = aq.ProjecctID and p.QuestionnaireID = sq.QuestionnaireID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and aq.accountID = @accountid and aq.ProgrammeID = @programmeid and pg.CompanyID=@companyId	
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' and Analysis_I=@AnalysisValue
	
	select CategoryName,
	cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	--cast( sum(Average)/count(Average)as decimal(12,1)) as Answer 
	into #xx1
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
	
	select Survey_Category.CategoryName,#xx1.Answer from #xx1
	inner join Survey_Category on #xx1.CategoryName=Survey_Category.CategoryName 
    where AccountID=@accountid and Survey_Category.QuestionnaireID = @QuestionnaireID
    order by Survey_Category.Sequence    
	
	drop table #xx1
	drop table #temp1
	
	end
	
	Else IF (UPPER(@AnalysisType) = 'ANALYSIS-II' or UPPER(@AnalysisType) = 'ANALYSIS- II' or UPPER(@AnalysisType) = 'Analysis_II' or UPPER(@AnalysisType) = 'ANALYSIS_ II')
	begin
	select c.CategoryID,c.CategoryName,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp2 from Account a
	left join Survey_Category c on c.AccountID = a.AccountID
	left join Survey_Question q on q.CateogryId = c.CategoryId
	left join Survey_QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join Survey_AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join Survey_AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join Survey_Questionnaire sq on sq.QuestionnaireID = aq.QuestionnaireID	
	left join [User] u on u.AccountID = aq.AccountID
	left join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
	left join Survey_Project p on p.ProjectID = aq.ProjecctID and p.QuestionnaireID = sq.QuestionnaireID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and aq.accountID = @accountid and aq.ProgrammeID = @programmeid and pg.CompanyID=@companyId	
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' and Analysis_II=@AnalysisValue
	
	select CategoryName,
	cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	--cast( sum(Average)/count(Average)as decimal(12,1)) as Answer 
	into #xx2
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
	
	
	select Survey_Category.CategoryName,#xx2.Answer from #xx2
	inner join Survey_Category on #xx2.CategoryName=Survey_Category.CategoryName 
    where AccountID=@accountid and Survey_Category.QuestionnaireID = @QuestionnaireID
    order by Survey_Category.Sequence    
     
	drop table #xx2
	
	drop table #temp2
	
	end
	Else IF (UPPER(@AnalysisType) = 'ANALYSIS-III' or UPPER(@AnalysisType) = 'ANALYSIS- III' or UPPER(@AnalysisType) = 'Analysis_III' or UPPER(@AnalysisType) = 'ANALYSIS_ III')
	begin
	select c.CategoryID,c.CategoryName,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp3 from Account a
	left join Survey_Category c on c.AccountID = a.AccountID
	left join Survey_Question q on q.CateogryId = c.CategoryId
	left join Survey_QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join Survey_AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join Survey_AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join Survey_Questionnaire sq on sq.QuestionnaireID = aq.QuestionnaireID		
	left join [User] u on u.AccountID = aq.AccountID
	left join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
	left join Survey_Project p on p.ProjectID = aq.ProjecctID and p.QuestionnaireID = sq.QuestionnaireID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and aq.accountID = @accountid and aq.ProgrammeID = @programmeid 	and pg.CompanyID=@companyId	
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' and Analysis_III=@AnalysisValue
	
	select CategoryName,
	cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	--cast( sum(Average)/count(Average)as decimal(12,1)) as Answer 
	into #xx3
	 from (
	select CategoryName,
	cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average
	from
	(
		select * from #temp3
	) as t1
	Group By CategoryName
	) as t2
	Group By CategoryName
	Order By CategoryName
	
	select Survey_Category.CategoryName,#xx3.Answer from #xx3
	inner join Survey_Category on #xx3.CategoryName=Survey_Category.CategoryName 
    where AccountID=@accountid and Survey_Category.QuestionnaireID = @QuestionnaireID
    order by Survey_Category.Sequence    
	
	
	drop table #xx3
	drop table #temp3
	end


	
end
GO
