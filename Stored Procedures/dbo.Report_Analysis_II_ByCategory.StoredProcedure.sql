USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Analysis_II_ByCategory]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Analysis_II_ByCategory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [Report_Analysis_II_ByCategory] @accountid=54,@projectid=6,@programmeid=36
CREATE PROCEDURE [dbo].[Report_Analysis_II_ByCategory] 
	@accountid int,
	@projectid int,
	@programmeid int
	
	as
	declare 
	@category_detail_count int,
	@enter_flag int,
	@ChkProgrammeAvg int,
	@ChkFullProjectGroup int, @QuestionnaireID int
	
	
	SELECT @QuestionnaireID=q.QuestionnaireID
	FROM Survey_Questionnaire Q
	INNER JOIN Survey_Project P ON Q.QuestionnaireID = P.QuestionnaireID
	WHERE Q.AccountID=@accountid
	AND P.ProjectID=@ProjectId
	
	
	BEGIN
	If object_id('tempdb..#temp6') Is Not Null
    Begin
      Drop Table #temp6
    End   
	create table #temp6(SR_No int identity(1,1),Analysis_type varchar(25),CategoryName varchar(50),Answer varchar(50))
	

	DECLARE @cat_detail varchar(25), @loop int
	set @loop=0
	
	DECLARE c2 CURSOR READ_ONLY
    FOR
    Select category_detail from survey_analysissheet_category_details where analysis_type='ANALYSIS- II' AND PROGRAMME_id=@programmeid
   --  Select category_detail from survey_analysissheet_category_details where analysis_type='ANALYSIS- II' AND PROGRAMME_id=277
    OPEN c2
    FETCH NEXT FROM c2 INTO @cat_detail

WHILE @@FETCH_STATUS = 0
BEGIN
    if(@loop=0)
    begin
    set @loop= @loop + 1 
    select c.CategoryID,c.CategoryName,u.FirstName + ' ' + u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp4 from Account a
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
	and aq.accountID =@accountid and aq.ProgrammeID = @programmeid  and Analysis_II =@cat_detail
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' 
	
insert into #temp6(Analysis_type,CategoryName,Answer)

	select @cat_detail as 'Analysis_type', CategoryName,
	cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	
	
	 from (
	select CategoryName,
	cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average
	from
	(
		select * from #temp4
	) as t1
	Group By CategoryName
	) as t2
	Group By CategoryName
--	Order By CategoryName
	drop table #temp4
	end
	
	else
	
	
	begin
	
	set @loop= @loop + 1 
	select c.CategoryID,c.CategoryName,u.FirstName+' '+ u.LastName as ParticipantName, CandidateName,	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp5
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
	and aq.accountID =@accountid and aq.ProgrammeID = @programmeid  and Analysis_II =@cat_detail
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' 
	
insert INTO #TEMP6 (Analysis_type,CategoryName,Answer) 
	select @cat_detail as 'Analysis_type',CategoryName,
	cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) as Answer
	from (
	select CategoryName,
	cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average
	from
	(
		select * from #temp5
	) as t1
	Group By CategoryName
	) as t2
	Group By CategoryName
--	Order By CategoryName

drop table #temp5
	
end


FETCH NEXT FROM c2 INTO @cat_detail
END
end
CLOSE c2
DEALLOCATE c2
select #temp6.SR_No,#temp6.Analysis_type,Survey_Category.CategoryName, #temp6.Answer into #temp7 
from #temp6
inner join Survey_Category on #temp6.CategoryName=Survey_Category.CategoryName where AccountID=@accountid order by Survey_Category.Sequence

------------------------------------------------------------------------------------------------------------------------
Select @ChkProgrammeAvg=Programme_Average,@ChkFullProjectGroup=FullProjectGrp from Survey_ProjectReportSetting  where AccountID=@accountid and ProjectID=@projectid

-------------------------------------------------------------------------------------------------------------------------
-- For Programme Average -:

if(@ChkProgrammeAvg=1)
BEGIN
   select c.CategoryID,c.CategoryName,u.FirstName + ' ' + u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp8 from Account a
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
	and aq.accountID =@accountid and aq.ProgrammeID = @programmeid  and Analysis_II IN(Select distinct Analysis_II from Survey_AssignmentDetails
  inner join Survey_AssignQuestionnaire on Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID 
  WHERE Survey_AssignQuestionnaire.AccountID=@accountid AND Survey_AssignQuestionnaire.ProjecctID=@projectid AND Survey_AssignQuestionnaire.ProgrammeID=@programmeid )
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' 
	
	
  Select 9999 as 'SR_No',   'Programme Average' as 'Analysis_type',categoryname, CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer into #TEMP9 from #temp8 group by Categoryname
  --Here '0' is taken just like that(to give it any number)


insert into #TEMP7(SR_No,Analysis_type, CategoryName, Answer)
select * from #temp9
  
--Select #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryName, #temp7.Answer from #TEMP7
--inner join Survey_Category on #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid order by Survey_Category.Sequence

drop table #TEMP9
drop table #TEMP8


END

------------------------------------------------------------------------------------------------------------------------------------------
-- For Project Group :

if(@ChkFullProjectGroup=1)
BEGIN
select c.CategoryID,c.CategoryName,u.FirstName + ' ' + u.LastName as ParticipantName, CandidateName,
	pg.ProgrammeName,p.Title,
	c.Sequence,  REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer
	into #temp10 from Account a
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
	and aq.accountID =@accountid and Analysis_II IN(Select distinct Analysis_II from Survey_AssignmentDetails
  inner join Survey_AssignQuestionnaire on Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID 
  WHERE Survey_AssignQuestionnaire.AccountID=@accountid AND Survey_AssignQuestionnaire.ProjecctID=@projectid)
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' 
	
	
  Select 99999 as 'SR_No',   'Full Project Group' as 'Analysis_type',categoryname, CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer into #TEMP11 from #temp10 group by Categoryname
  --Here '1' is taken just like that(to give it any number)


insert into #TEMP7(SR_No,Analysis_type, CategoryName, Answer)
select * from #temp11
  
Select #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryTitle CategoryName, #temp7.Answer,Survey_Category.Sequence from #TEMP7
inner join Survey_Category on #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid 
and Survey_Category.QuestionnaireID=@QuestionnaireID
order by Survey_Category.Sequence,#temp7.SR_No desc

drop table #TEMP10
drop table #TEMP11

END
ELSE
BEGIN
--insert into #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id,Sequence)
SELECT ROW_NUMBER() OVER (ORDER BY Survey_AnalysisSheet_Category_Details.Analysis_Category_Id, Survey_Category.Sequence DESC) AS SR_NO, #temp7.Analysis_type, #temp7.CategoryName,
 #temp7.Answer,Survey_AnalysisSheet_Category_Details.Analysis_Category_Id,Survey_Category.Sequence into #temp77 from  #temp7 
inner join Survey_Category on #temp7.CategoryName=Survey_Category.CategoryName 
inner join Survey_AnalysisSheet_Category_Details on Survey_AnalysisSheet_Category_Details.Category_Detail = #temp7.Analysis_type
where AccountID=@accountid  and Survey_AnalysisSheet_Category_Details.Programme_Id = @programmeid 

insert into #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id,Sequence)
SELECT #temp7.*,9999,Survey_Category.Sequence FROM #temp7 
inner join Survey_Category on #temp7.CategoryName=Survey_Category.CategoryName 
WHERE SR_No =9999 

insert into #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id,Sequence)
SELECT #temp7.*,99999,Survey_Category.Sequence FROM #temp7 
inner join Survey_Category on #temp7.CategoryName=Survey_Category.CategoryName
WHERE SR_No =99999 

SELECT SR_No,Analysis_type, Survey_Category.CategoryTitle CategoryName, Answer,Survey_Category.Sequence FROM #temp77 
INNER JOIN Survey_Category ON #temp77.CategoryName=Survey_Category.CategoryName where AccountID=@accountid and 
Survey_Category.QuestionnaireID=@QuestionnaireID
order by Survey_Category.Sequence, SR_No DESC

/*
SELECT SR_No,Analysis_type, #temp77.CategoryName, Answer,Survey_Category.Sequence FROM #temp77 
INNER JOIN Survey_Category ON #temp77.CategoryName=Survey_Category.CategoryName where AccountID=@accountid and 
Survey_Category.QuestionnaireID=@QuestionnaireID
order by Survey_Category.Sequence, SR_No DESC
*/

--UNION 
--SELECT * FROM #temp7 WHERE SR_No in (9999, 99999) order by Survey_AnalysisSheet_Category_Details.Analysis_Category_Id, Survey_Category.Sequence --#temp7.SR_No, Survey_Category.Sequence
--order by #temp7.SR_No 
If object_id('tempdb..#TEMP77') Is Not Null
   Begin
      Drop Table #TEMP77
   End

END
------------------------------------------------------------------------------------------------------------------------------------------

If object_id('tempdb..#TEMP7') Is Not Null
   Begin
      Drop Table #TEMP7
   End

--exec [dbo].[Report_Analysis_I_ByCategory] 47, 1, 2
GO
