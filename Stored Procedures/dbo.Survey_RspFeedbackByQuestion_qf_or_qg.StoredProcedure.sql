USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_RspFeedbackByQuestion_qf_or_qg]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_RspFeedbackByQuestion_qf_or_qg]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC [dbo].[Survey_RspFeedbackByQuestion_qf_or_qg] 54,6,36,'qf',14
CREATE PROCEDURE [dbo].[Survey_RspFeedbackByQuestion_qf_or_qg] 
 @accountid varchar(50),
 @projectid varchar(50),
 @programmeid varchar(50),
 @type varchar(2),
 @companyId int=null
AS
BEGIN
 if(@type = 'qg')
 Begin
  select * from Survey_AssignmentDetails
 end
 --Union
 if(@type = 'qf')
 Begin
  
  
  select QuestionID,
  cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1) )as Answer,MAX(t1.C_Sequence) C,MAX(t1.Q_Sequence) Q--into #temp2
  From
  (select q.QuestionID,
  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer,c.Sequence C_Sequence,q.Sequence Q_Sequence  
  from dbo.Survey_AssignQuestionnaire aq
  inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
  inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
  inner join Survey_Question q on q.QuestionID = qa.QuestionID
  inner join Survey_Category c on c.CategoryID = q.CateogryID
  left join [User] u on u.AccountID = aq.AccountID
  inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
  inner join Survey_Project p on p.ProjectID = aq.ProjecctID
  where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
  aq.accountID = @accountid and aq.ProjecctID = @projectid --and pg.CompanyID=@companyId
   ) as t1
  Group By QuestionID
  Order by C,Q
  
 -- select * from #temp2
  
  --select questionid, 
  --cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1) )as Answer
  -- from #temp2 group by Questionid
  --drop table #temp2
 end
 
	IF (@type = 'qp')
	BEGIN
		select QuestionID,
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1) )as Answer,MAX(t1.C_Sequence) C,MAX(t1.Q_Sequence) Q
		into #tm_ProgrammeByQuestion
		From
		(select q.QuestionID,
		REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer,c.Sequence C_Sequence,q.Sequence Q_Sequence  
		from dbo.Survey_AssignQuestionnaire aq
		inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Survey_Question q on q.QuestionID = qa.QuestionID
		inner join Survey_Category c on c.CategoryID = q.CateogryID
		left join [User] u on u.AccountID = aq.AccountID
		inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Survey_Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid and pg.CompanyID=@companyId
		and pg.ProgrammeID = @programmeid
		) as t1
		Group By QuestionID
		Order by C,Q

		Select 
				--9999 as 'SR_No',
				--'Programme Average' as 'Analysis_type',
				Q, 
				CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer 
		--into	#TEMP9 
		from 
				#tm_ProgrammeByQuestion
		group by 
				C,Q

		drop table #tm_ProgrammeByQuestion
	END 
END
GO
