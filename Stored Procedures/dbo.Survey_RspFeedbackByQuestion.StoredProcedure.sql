USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_RspFeedbackByQuestion]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_RspFeedbackByQuestion]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Survey_RspFeedbackByQuestion] 
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
	Begin
		
  Select Identity(int,1,1) as "QuestionNum", q.QuestionID,q.Title as "QuestionTitle", q.Description,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence CS,q.Sequence QS,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
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
	and aq.accountID = @accountid and aq.ProgrammeID = @programmeid 	and pg.CompanyID=@companyId	
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' and Analysis_I=@AnalysisValue
	
		
	
	--select QuestionID,
	--cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	--from (
	select QuestionID,Average Answer from (
	select QuestionID,
	CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,4)) As Average
	,MAX(CS) CS1,MAX(QS) QS1
	from
	(
		select * from #temp1
	) as t1
	Group By QuestionID	
	--) as t2
	--Group By QuestionID
	)tb
	Order By CS1,QS1
	
	drop table #temp1
end		
	Else IF (UPPER(@AnalysisType) = 'ANALYSIS-II' or UPPER(@AnalysisType) = 'ANALYSIS- II' or UPPER(@AnalysisType) = 'Analysis_II' or UPPER(@AnalysisType) = 'ANALYSIS_ II')
	begin
	 Select Identity(int,1,1) as "QuestionNum", q.QuestionID,q.Title as "QuestionTitle", q.Description,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence CS,q.Sequence QS,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	
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
	and aq.accountID = @accountid and aq.ProgrammeID = @programmeid 	and pg.CompanyID=@companyId	
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' and Analysis_II=@AnalysisValue
		
	
	--select QuestionID,
	--cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	--from (
	select QuestionID,Average Answer from (
	select QuestionID,
	CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,4)) As Average
	,MAX(CS) CS1,MAX(QS) QS1
	from
	(
		select * from #temp2
	) as t1
	Group By QuestionID	
	--) as t2
	--Group By QuestionID
	)tb
	Order By CS1,QS1
	drop table #temp2
	
	end





Else IF (UPPER(@AnalysisType) = 'ANALYSIS-III' or UPPER(@AnalysisType) = 'ANALYSIS- III' or UPPER(@AnalysisType) = 'Analysis_III' or UPPER(@AnalysisType) = 'ANALYSIS_ III')
	begin
	 Select Identity(int,1,1) as "QuestionNum", q.QuestionID,q.Title as "QuestionTitle", q.Description,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence CS,q.Sequence QS,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
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
		
	
	--select QuestionID,
	--cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	--from (
	select QuestionID,Average Answer from (
	select QuestionID,
	CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,4)) As Average
	,MAX(CS) CS1,MAX(QS) QS1
	from
	(
		select * from #temp3
	) as t1
	Group By QuestionID	
	--) as t2
	--Group By QuestionID
	)tb
	Order By CS1,QS1
	
	drop table #temp3
	
	end




END
GO
