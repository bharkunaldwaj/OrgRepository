USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Analysis_I_ByCategory]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Analysis_I_ByCategory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=16,@AnaType='ANALYSIS- I',@Filter='TABLE'
exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=16,@AnaType='ANALYSIS- I',@Filter='TOP'

--exec [Report_Analysis_I_ByCategory] @accountid=54,@projectid=6,@programmeid=36,@AnaType='ANALYSIS- I',@Filter='TABLE'
--exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=16,@AnaType='ANALYSIS- I',@Filter='TABLE'
--exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=16,@AnaType='ANALYSIS- II        '
CREATE PROCEDURE [dbo].[Report_Analysis_I_ByCategory] 
	@accountid int,
	@projectid int,
	@programmeid int,
	@Filter varchar(25)=null,
	@AnaType varchar(25)=null
	
	as
	declare 
	@category_detail_count int,
	@enter_flag int,
	@ChkProgrammeAvg int,
	@ChkFullProjectGroup int,
	@ChkPrvScore1 bit,
	@ChkPrvScore2 bit,	
	@QuestionnaireID INT=0,
	@ShowPreViousScore1 Bit =1,
	@ShowPreViousScore2 Bit =1
	
	BEGIN

 

Select @QuestionnaireID=q.QuestionnaireID from Survey_Questionnaire Q INNER JOIN Survey_Project P On Q.QuestionnaireID = P.QuestionnaireID
WHERE Q.AccountID=@accountid and P.ProjectID=@ProjectId

Select  @ShowPreViousScore1=ShowPreViousScore1,@ShowPreViousScore2=ShowPreViousScore1 from Survey_ProjectReportSetting WHERE AccountID=@accountid and ProjectID=@ProjectId

	select @ChkPrvScore1=ISNULL(ShowPreviousScore1,0),@ChkPrvScore2=ISNULL(ShowPreviousScore2,0) from Survey_ProjectReportSetting where AccountID=@accountid and ProjectID=@projectid and ReportType=5
	DECLARE @cat_detail varchar(25), @loop int, @cat_name varchar(25),@last_ID int
	set @loop=0
	
	DECLARE c2 CURSOR READ_ONLY
    FOR
    Select category_detail from survey_analysissheet_category_details where analysis_type=@AnaType AND PROGRAMME_id=@programmeid 
    OPEN c2
    FETCH NEXT FROM c2 INTO @cat_detail
create  table #temp6(SR_No int identity(1,1),Analysis_type varchar(25),CategoryName varchar(50),Answer varchar(50))
WHILE @@FETCH_STATUS = 0
BEGIN
	IF(@loop=0)
		BEGIN 
	   
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
		and aq.accountID =@accountid and aq.ProgrammeID = @programmeid  and CASE WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I WHEN @AnaType ='ANALYSIS- II '  Then Analysis_II WHEN @AnaType='ANALYSIS- III' Then Analysis_III ELSE Analysis_I END =@cat_detail
		and qa.answer !='N/A' and ad.SubmitFlag = 'True' order by c.Sequence
		--DEBUG
		--SELECT * FROM #temp4
		--CREATE INDEX i_temp6 ON #temp6(CategoryName)
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
		--Order By CategoryName
		
		END
		
	ELSE 
	BEGIN
		
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
		and aq.accountID =@accountid and aq.ProgrammeID = @programmeid  and CASE WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I WHEN @AnaType ='ANALYSIS- II '  Then Analysis_II WHEN @AnaType='ANALYSIS- III' Then Analysis_III ELSE Analysis_I END =@cat_detail
		and qa.answer !='N/A' and ad.SubmitFlag = 'True' order by c.Sequence
	--DEBUG
	--select * from #temp5
		
		insert INTO #TEMP6(Analysis_type,CategoryName,Answer) 

		select @cat_detail as 'Analysis_type', CategoryName,
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
		--Order By CategoryName
		
		
	drop table #temp5
		
END

FETCH NEXT FROM c2 INTO @cat_detail
END

	CLOSE c2
	DEALLOCATE c2

END

select Survey_Category.Sequence as SR_No,#temp6.Analysis_type,Survey_Category.CategoryName, #temp6.Answer into #temp7
from #temp6
inner join Survey_Category on #temp6.CategoryName=Survey_Category.CategoryName where AccountID=@accountid and Survey_Category.QuestionnaireID=@QuestionnaireID order by Survey_Category.Sequence

------------------------------------------------------------------------------------------------------------------------
Select @ChkProgrammeAvg=Programme_Average,@ChkFullProjectGroup=FullProjectGrp from Survey_ProjectReportSetting  where AccountID=@accountid and ProjectID=@projectid

-------------------------------------------------------------------------------------------------------------------------
-- For Programme Average -:

IF(@ChkProgrammeAvg=1)
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
		and aq.accountID =@accountid and aq.ProgrammeID = @programmeid  and 
		CASE WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I WHEN @AnaType ='ANALYSIS- II '  Then Analysis_II WHEN @AnaType='ANALYSIS- III' Then Analysis_III ELSE Analysis_I END  IN(Select distinct 		
		CASE WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I WHEN @AnaType ='ANALYSIS- II '  Then Analysis_II WHEN @AnaType='ANALYSIS- III' Then Analysis_III ELSE Analysis_I END  from Survey_AssignmentDetails
		inner join Survey_AssignQuestionnaire on Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID 
		WHERE Survey_AssignQuestionnaire.AccountID=@accountid AND Survey_AssignQuestionnaire.ProjecctID=@projectid AND Survey_AssignQuestionnaire.ProgrammeID=@programmeid)
		and qa.answer !='N/A' and ad.SubmitFlag = 'True' order by c.Sequence

	
	Select 9999 as 'SR_No',   'Programme Average' as 'Analysis_type',categoryname, CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer into 
	#TEMP9 from #temp8 group by Categoryname
	--Here '0' is taken just like that(to give it any number)


	insert into #TEMP7(SR_No,Analysis_type, CategoryName, Answer)
	select * from #temp9
  
--Select #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryName, #temp7.Answer from #TEMP7
--inner join Survey_Category on #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid order by Survey_Category.Sequence

drop table #TEMP9
drop table #TEMP8

END
------------------------------------------------------------------------------------------------------------------------------------------
 --For Project Group :
SELECT @ChkFullProjectGroup
IF(@ChkFullProjectGroup=1)
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
	and aq.accountID =@accountid and 
	CASE WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I WHEN @AnaType ='ANALYSIS- II '  Then Analysis_II WHEN @AnaType='ANALYSIS- III' Then Analysis_III ELSE Analysis_I END 
	 IN(Select distinct CASE WHEN @AnaType = 'ANALYSIS- I' THEN  Analysis_I WHEN @AnaType ='ANALYSIS- II '  Then Analysis_II WHEN @AnaType='ANALYSIS- III' Then Analysis_III ELSE Analysis_I END from Survey_AssignmentDetails
  inner join Survey_AssignQuestionnaire on Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID 
  WHERE Survey_AssignQuestionnaire.AccountID=@accountid AND Survey_AssignQuestionnaire.ProjecctID=@projectid)
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' order by c.Sequence
	
	
  Select 99999 as 'SR_No',   'Full Project Group' as 'Analysis_type',categoryname, CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer into #TEMP11 from #temp10 group by Categoryname
  --Here '1' is taken just like that(to give it any number)


insert into #TEMP7(SR_No,Analysis_type, CategoryName, Answer)
select * from #temp11

--update #temp7 set Analysis_type='Current' where Analysis_type not in ('Programme Average','Full Project Group')


declare @AnalysisType11 varchar(50)=(select Score1Title from Survey_PreviousScores where AccountID=@accountid and ProjectID=@projectid and ProgrammeID=@programmeid)
declare @AnalysisType22 varchar(50)=(select Score2Title from Survey_PreviousScores where AccountID=@accountid and ProjectID=@projectid and ProgrammeID=@programmeid)

IF(@Filter is null or @Filter='')
BEGIN
		SELECT * FROM (Select #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryName, #temp7.Answer from #TEMP7
		inner join Survey_Category on #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid 
		--order by Survey_Category.Sequence,#temp7.SR_No desc

		union

		select sc.Sequence as SR_No,@AnalysisType11 as Analysis_Type,sc.CategoryName,SUM(Score1)/COUNT(spqd.CategoryID) Answer 
		from 
		Survey_PrvScore_QstDetails spqd
		left join Survey_Question sq
		on spqd.QuestionID=sq.QuestionID
		left join Survey_Category sc
		on CateogryID=sc.CategoryID
		left join Survey_PreviousScores  sps
		on sps.PreviousScoreID=spqd.PreviousScoreID
		where  

		sps.AccountID=CASE WHEN @ChkPrvScore1 =1
		THEN @accountid
		ELSE 0
		END 
		and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
		and QuestionTypeID=2
		group by spqd.CategoryID,sc.CategoryName,sc.Sequence
HAVING SUM(spqd.Score1)/COUNT(spqd.CategoryID)>0
		union 
		select sc.Sequence as SR_No,@AnalysisType22 as Analysis_Type,sc.CategoryName,SUM(Score2)/COUNT(spqd.CategoryID) Answer 
		from 
		Survey_PrvScore_QstDetails spqd
		left join Survey_Question sq
		on spqd.QuestionID=sq.QuestionID
		left join Survey_Category sc
		on CateogryID=sc.CategoryID
		left join Survey_PreviousScores  sps
		on sps.PreviousScoreID=spqd.PreviousScoreID
		where  sps.AccountID=CASE WHEN @ChkPrvScore2 =1
		THEN @accountid
		ELSE 0
		END 
		 and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
		 and QuestionTypeID=2
		group by spqd.CategoryID,sc.CategoryName,sc.Sequence
		HAVING SUM(spqd.Score2)/COUNT(spqd.CategoryID)>0
		) a Order by SR_No
		

END

ELSE
		BEGIN
		IF(Upper(@Filter)='TOP')
		BEGIN
		 
			select top (SELECT  top 1 RadarGraphCategoryCount FROM Survey_ProjectReportSetting Where  AccountID=@accountid and ProjectID=@ProjectId) CategoryName,CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer from 
			(
			Select #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryName, #temp7.Answer from #TEMP7
			inner join Survey_Category on #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid 
			and Analysis_type not in ('Programme Average','Full Project Group')
			--order by Survey_Category.Sequence,#temp7.SR_No desc


			) tbtop
			--where tbtop.Analysis_type='Current'
			group by tbtop.CategoryName
			order by CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) desc,  tbtop.CategoryName  asc
		END
		ELSE If(Upper(@Filter)='BOTTOM')
		BEGIN
			select top (SELECT  top 1 RadarGraphCategoryCount FROM Survey_ProjectReportSetting Where  AccountID=@accountid and ProjectID=@ProjectId)  CategoryName,CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer from 
			(
			Select #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryName, #temp7.Answer from #TEMP7
			inner join Survey_Category on #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid 
			and Analysis_type not in ('Programme Average','Full Project Group')
			--order by Survey_Category.Sequence,#temp7.SR_No desc
			) tbtop
			--where tbtop.Analysis_type='Current'
			group by tbtop.CategoryName
			order by CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) asc,tbtop.CategoryName desc--CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) asc

		END
		ELSE IF(Upper(@Filter)='TABLE')
		BEGIN
		
		
				select * into #tempPrev1 from
				(select 1 as SR_No,@AnalysisType11 as Analysis_Type,sc.CategoryName,SUM(Score1)/COUNT(spqd.CategoryID) Answer 
				from 
				Survey_PrvScore_QstDetails spqd
				left join Survey_Question sq
				on spqd.QuestionID=sq.QuestionID
				left join Survey_Category sc
				on CateogryID=sc.CategoryID
				left join Survey_PreviousScores  sps
				on sps.PreviousScoreID=spqd.PreviousScoreID
				where  sps.AccountID=CASE WHEN @ChkPrvScore1 =1
				THEN @accountid
				ELSE 0
				END 
				 and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
				and AnalysisType=1
				and QuestionTypeID=2
				group by spqd.CategoryID,sc.CategoryName
				HAVING SUM(spqd.Score1)/COUNT(spqd.CategoryID)>0)tbPrev1
 
				select * into #tempPrev2 from 
										(select 1 as SR_No,@AnalysisType22 as Analysis_Type,sc.CategoryName,SUM(Score2)/COUNT(spqd.CategoryID) Answer 
										from 
										Survey_PrvScore_QstDetails spqd
										left join Survey_Question sq
										on spqd.QuestionID=sq.QuestionID
										left join Survey_Category sc
										on CateogryID=sc.CategoryID
										left join Survey_PreviousScores  sps
										on sps.PreviousScoreID=spqd.PreviousScoreID
										where  sps.AccountID=CASE WHEN @ChkPrvScore2 =1
															THEN @accountid
															ELSE 0
															END 
										and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
										and AnalysisType=1
										and QuestionTypeID=2
										group by spqd.CategoryID,sc.CategoryName
										HAVING SUM(spqd.Score2)/COUNT(spqd.CategoryID)>0)tbPrev2
				--update #temp7 set Analysis_type='Current' where Analysis_type not in ('Programme Average','Full Project Group')

 
								
				select * into #tempmain1 from (select Analysis_Type,CategoryName,CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer 
								from #temp7 
								where Analysis_type not in ('Programme Average','Full Project Group')
								group by CategoryName,Analysis_Type)tbMain1
						
				--Main query	
				
				If @ShowPreViousScore1=1 and @ShowPreViousScore2 =1 
					Begin			
							Select * FROM (
							select	Isnull(#tempmain1.CategoryName,#tempPrev1.CategoryName) CategoryName
									,Isnull(#tempmain1.Answer,0) CurrentScore,
									CAST(#tempPrev1.Answer as decimal(12,1)) Score1,
									CAST(#tempPrev2.Answer as decimal(12,1)) Score2,@AnalysisType11 Socre1Title,@AnalysisType22 Socre2Title
									
							from #tempPrev1
							left join #tempmain1
							on #tempmain1.CategoryName=#tempPrev1.CategoryName
							left join #tempPrev2
							on #tempPrev1.CategoryName=#tempPrev2.CategoryName
							) as T 
							order by T.CurrentScore desc,T.CategoryName asc
						End
						
					If @ShowPreViousScore1=1 and @ShowPreViousScore2 =0
					Begin			
							Select * FROM (
							
							select	Isnull(#tempmain1.CategoryName,#tempPrev1.CategoryName) CategoryName
									,Isnull(#tempmain1.Answer,0) CurrentScore,
									CAST(#tempPrev1.Answer as decimal(12,1)) Score1,
									CAST(0 as decimal(12,1)) Score2,@AnalysisType11 Socre1Title,@AnalysisType22 Socre2Title
									
							from #tempPrev1
							left join #tempmain1
							on #tempmain1.CategoryName=#tempPrev1.CategoryName
							 ) AS T 
							order by T.CurrentScore desc,T.CategoryName  asc
						End
					If @ShowPreViousScore1=0 and @ShowPreViousScore2 =0
					Begin	
							
							select	Isnull(#tempmain1.CategoryName,'') CategoryName
									,Isnull(#tempmain1.Answer,0) CurrentScore,
									CAST(0 as decimal(12,1)) Score1,
									CAST(0 as decimal(12,1)) Score2,@AnalysisType11 Socre1Title,@AnalysisType22 Socre2Title
									
							from  #tempmain1
							
							 
							order by CurrentScore desc,CategoryName  asc
						End

		END


END


drop table #TEMP10
drop table #TEMP11

END
ELSE
BEGIN


		--insert into #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id,Sequence)
		--ROW_NUMBER() OVER (ORDER BY Survey_AnalysisSheet_Category_Details.Analysis_Category_Id, Survey_Category.Sequence DESC)
		SELECT  #temp7.SR_No AS SR_No, #temp7.Analysis_type, #temp7.CategoryName,
		 #temp7.Answer,Survey_AnalysisSheet_Category_Details.Analysis_Category_Id,Survey_Category.Sequence into #temp77 from  #temp7 
		inner join Survey_Category on #temp7.CategoryName=Survey_Category.CategoryName 
		inner join Survey_AnalysisSheet_Category_Details on Survey_AnalysisSheet_Category_Details.Category_Detail = #temp7.Analysis_type
		where AccountID=@accountid  and Survey_AnalysisSheet_Category_Details.Programme_Id = @programmeid 
		AND Survey_Category.AccountID=@accountid and Survey_Category.QuestionnaireID=@QuestionnaireID

		insert into #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id,Sequence)
		SELECT #temp7.*,9999,Survey_Category.Sequence FROM #temp7 
		inner join Survey_Category on #temp7.CategoryName=Survey_Category.CategoryName 
		WHERE SR_No =9999 
		AND AccountID=@accountid and Survey_Category.QuestionnaireID=@QuestionnaireID

		insert into #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id,Sequence)
		SELECT #temp7.*,99999,Survey_Category.Sequence FROM #temp7 
		inner join Survey_Category on #temp7.CategoryName=Survey_Category.CategoryName
		WHERE SR_No =99999  AND Survey_Category.AccountID=@accountid and Survey_Category.QuestionnaireID=@QuestionnaireID
--PRINT 'SUCCESS'
--update #temp77 set Analysis_type='Current' where Analysis_type not in ('Programme Average','Full Project Group')
--SELECT * FROM #TEMP77 ORDER BY SR_No

--RETURN;
		declare @AnalysisType1 varchar(50)=(select Top 1 Score1Title from Survey_PreviousScores where AccountID=@accountid and ProjectID=@projectid and ProgrammeID=@programmeid)
		declare @AnalysisType2 varchar(50)=(select Top 1 Score2Title from Survey_PreviousScores where AccountID=@accountid and ProjectID=@projectid and ProgrammeID=@programmeid)
--PRINT 'SUCCESS'
IF(@Filter is null or @Filter='')
	BEGIN
	--SELECT @QuestionnaireID	
	
				Select Survey_Category.Sequence SR_No,Result.Analysis_Type,Result.CategoryName,Result.Answer from (SELECT SR_No,Analysis_type, CategoryName, Answer FROM #temp77 
				--order by Analysis_Category_Id, Sequence DESC

				union

				select 1 as SR_No,@AnalysisType1 as Analysis_Type,sc.CategoryName,SUM(Score1)/COUNT(spqd.CategoryID) Answer 
				from 
				Survey_PrvScore_QstDetails spqd
				left join Survey_Question sq
				on spqd.QuestionID=sq.QuestionID
				left join Survey_Category sc
				on CateogryID=sc.CategoryID
				left join Survey_PreviousScores  sps
				on sps.PreviousScoreID=spqd.PreviousScoreID
				where  sps.AccountID=CASE WHEN @ChkPrvScore1 =1
				THEN @accountid
				ELSE 0
				END 
				and QuestionTypeID=2
				 and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
				group by spqd.CategoryID,sc.CategoryName
				HAVING SUM(spqd.Score1)/COUNT(spqd.CategoryID)>0

				union 
				select 1 as SR_No,@AnalysisType2 as Analysis_Type,sc.CategoryName,SUM(Score2)/COUNT(spqd.CategoryID) Answer 
				from 
				Survey_PrvScore_QstDetails spqd
				left join Survey_Question sq
				on spqd.QuestionID=sq.QuestionID
				left join Survey_Category sc
				on CateogryID=sc.CategoryID
				left join Survey_PreviousScores  sps
				on sps.PreviousScoreID=spqd.PreviousScoreID
				where  sps.AccountID=CASE WHEN @ChkPrvScore2 =1
				THEN @accountid
				ELSE 0
				END and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid and QuestionTypeID=2
				group by spqd.CategoryID,sc.CategoryName
				HAVING SUM(spqd.Score1)/COUNT(spqd.CategoryID)>0
				) as Result
				inner join Survey_Category on Result.CategoryName=Survey_Category.CategoryName WHERE Survey_Category.QuestionnaireID=@QuestionnaireID order by Survey_Category.Sequence
	--SELECT @QuestionnaireID			
	END

ELSE
BEGIN

if(Upper(@Filter)='TOP')
	Begin
		select top (SELECT  top 1 RadarGraphCategoryCount FROM Survey_ProjectReportSetting Where  AccountID=@accountid and ProjectID=@ProjectId)  CategoryName,CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer
				from(
				SELECT SR_No,Analysis_type, CategoryName, Answer FROM #temp77 
				where Analysis_type not in ('Programme Average','Full Project Group')
				--order by Analysis_Category_Id, Sequence DESC

				)tbtop1
		--where tbtop1.Analysis_type='Current'
		group by tbtop1.CategoryName
		order by CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) desc ,tbtop1.CategoryName asc
	End
Else If(Upper(@Filter)='BOTTOM')
	Begin
 
		select Top (SELECT  top 1 RadarGraphCategoryCount FROM Survey_ProjectReportSetting Where  AccountID=@accountid and ProjectID=@ProjectId)  CategoryName,CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer
				from(
				SELECT SR_No,Analysis_type, CategoryName, Answer FROM #temp77 
				where Analysis_type not in ('Programme Average','Full Project Group')
				--order by Analysis_Category_Id, Sequence DESC
		)tbtop1
		--where tbtop1.Analysis_type='Current'
		group by tbtop1.CategoryName
		order by CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) asc ,tbtop1.CategoryName desc --CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) asc
	END
Else if(Upper(@Filter)='TABLE')
BEGIN

		select * into #tempPrev11 from
		(select 1 as SR_No,@AnalysisType1 as Analysis_Type,sc.CategoryName,CAST(AVG(CAST(Score1 as decimal(12,1))) as decimal(12,1)) Answer 
		from 
		Survey_PrvScore_QstDetails spqd
		left join Survey_Question sq
		on spqd.QuestionID=sq.QuestionID
		left join Survey_Category sc
		on CateogryID=sc.CategoryID
		left join Survey_PreviousScores  sps
		on sps.PreviousScoreID=spqd.PreviousScoreID
		where  sps.AccountID=CASE WHEN @ChkPrvScore1 =1
		THEN @accountid
		ELSE 0
		END  and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
		and AnalysisType=1
		and QuestionTypeID=2
		group by spqd.CategoryID,sc.CategoryName
		HAVING CAST(AVG(CAST(spqd.Score1 as decimal(12,1))) as decimal(12,1))> 0--SUM(spqd.Score1)/COUNT(spqd.CategoryID)>0
		
		)tbPrev1

		select * into #tempPrev22 from 
								(select 1 as SR_No,@AnalysisType2 as Analysis_Type,sc.CategoryName,CAST(AVG(CAST(Score2 as decimal(12,1))) as decimal(12,1)) Answer-- SUM(Score2)/COUNT(spqd.CategoryID)  
								from 
								Survey_PrvScore_QstDetails spqd
								left join Survey_Question sq
								on spqd.QuestionID=sq.QuestionID
								left join Survey_Category sc
								on CateogryID=sc.CategoryID
								left join Survey_PreviousScores  sps
								on sps.PreviousScoreID=spqd.PreviousScoreID
								where  sps.AccountID=CASE WHEN @ChkPrvScore2 =1
								THEN @accountid
								ELSE 0
								END  and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
								and AnalysisType=1
								and QuestionTypeID=2
								group by spqd.CategoryID,sc.CategoryName
								HAVING CAST(AVG(CAST(spqd.Score2 as decimal(12,1))) as decimal(12,1))>0--SUM(spqd.Score1)/COUNT(spqd.CategoryID)>0
								)tbPrev2
		--update #temp77 set Analysis_type='Current' where Analysis_type not in ('Programme Average','Full Project Group')
		select * into #tempmain11 from (select CategoryName,CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer 
						from #temp77 
						where Analysis_type not in ('Programme Average','Full Project Group')
						group by CategoryName)tbMain1
		--exec [Report_Analysis_I_ByCategory] 47,11,28,'TABLE','ANALYSIS- I'
		--select * from #tempPrev11
		--Main query
		 
		If @ShowPreViousScore1=1 and @ShowPreViousScore2 =1 
		Begin				
			Select * FROM (
			
			select	DISTINCT Isnull(#tempmain11.CategoryName,#tempPrev11.CategoryName) CategoryName
					,Isnull(#tempmain11.Answer,0) CurrentScore,
					CAST(#tempPrev11.Answer as decimal(12,1)) Score1,
					CAST(#tempPrev22.Answer as decimal(12,1)) Score2,@AnalysisType1 Socre1Title,@AnalysisType2 Socre2Title--,#tempmain11.Answer
					
			from #tempPrev11
			left join #tempmain11
			on #tempmain11.CategoryName=#tempPrev11.CategoryName
			left join #tempPrev22
			on #tempPrev11.CategoryName=#tempPrev22.CategoryName
			
			) AS T 
			order by T.CurrentScore desc,T.CategoryName  asc
		End
		else If @ShowPreViousScore1=1 and @ShowPreViousScore2 = 0 
		Begin
			Select * FROM (
			
			select	DISTINCT Isnull(#tempmain11.CategoryName,#tempPrev11.CategoryName) CategoryName
						,Isnull(#tempmain11.Answer,0) CurrentScore,
						CAST(#tempPrev11.Answer as decimal(12,1)) Score1,
						CAST(0 as decimal(12,1)) Score2,@AnalysisType1 Socre1Title,@AnalysisType2 Socre2Title--,#tempmain11.Answer
						
				from #tempPrev11
				left join #tempmain11
				on #tempmain11.CategoryName=#tempPrev11.CategoryName
				) AS T 
				
				order by T.CurrentScore desc,T.CategoryName asc
		End
		else If @ShowPreViousScore1=0 and @ShowPreViousScore2 = 0 
		Begin
			
			select	DISTINCT Isnull(#tempmain11.CategoryName,'') CategoryName
						,Isnull(#tempmain11.Answer,0) CurrentScore,
						CAST(0 as decimal(12,1)) Score1,
						CAST(0 as decimal(12,1)) Score2,@AnalysisType1 Socre1Title,@AnalysisType2 Socre2Title--,#tempmain11.Answer
						
				from  #tempmain11
				order by CurrentScore desc ,CategoryName asc
		End
			
			 
			--Select * from #tempPrev11
			--Select * from #tempmain11
			 
			--Select * from #tempPrev22
			
			
			 
		-- select 1 as SR_No,@AnalysisType1 as Analysis_Type,sc.CategoryName,SUM(spqd.Score1)/COUNT(spqd.CategoryID) Answer 
		--from 
		--Survey_PrvScore_QstDetails spqd
		--left join Survey_Question sq
		--on spqd.QuestionID=sq.QuestionID
		--left join Survey_Category sc
		--on CateogryID=sc.CategoryID
		--left join Survey_PreviousScores  sps
		--on sps.PreviousScoreID=spqd.PreviousScoreID
		--where  sps.AccountID=CASE WHEN @ChkPrvScore1 =1
		--THEN @accountid
		--ELSE 0
		--END  and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
		--and AnalysisType=1
		--and QuestionTypeID=2
		--group by spqd.CategoryID,sc.CategoryName 
		--Having SUM(spqd.Score1)/COUNT(spqd.CategoryID)<>0

			 

END



END


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
