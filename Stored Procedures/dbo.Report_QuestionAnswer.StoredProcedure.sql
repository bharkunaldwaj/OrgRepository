USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_QuestionAnswer]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_QuestionAnswer] --2,7,28  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[Report_QuestionAnswer]  47,1,2
--exec Report_QuestionAnswer @accountid=66,@projectid=14,@programmeid=41
 --SELECT  ReportManagementID FROM  Survey_ReportManagement WHERE AccountID = 63  and ProgramID = 5 and 
 --   ProjectID = 	3
----Select * from Survey_ProjectReportSetting WHERE AccountID = 63  and
--  ProjectID = 	3
--[dbo].[Report_QuestionAnswer] 47,11,25
CREATE PROCEDURE [dbo].[Report_QuestionAnswer]  --54,6,36 
	@accountid int,
	@projectid int,
	@programmeid int
	as
    BEGIN
    
    declare @path varchar(100)	
    --set @path='file://D:\360_Degree_Feedback\feedback360\UploadDocs\'
    --set @path='file://E:\WorkingProjects\FeedBack\Feedback_360_Prod\feedback360\UploadDocs\'
    set @path=(SELECT dbo.GetUploadDocsPath())
        
	DECLARE @cat_detail varchar(25), @loop int,
	@AnaI int ,@AnaII int ,@AnaIII int, @AI varchar(100),@AII varchar(100),@AIII varchar(100),@Chk_FullProjectGrp varchar(5), @Chk_Programme_Average varchar(5),  @Chk_Ana int
	
	set @loop=0
	--DROP TABLE #Chk_analysis
select AnalysisI,AnalysisII,AnalysisIII INTO #Chk_analysis  from  Survey_ProjectReportSetting 
where ProjectID=@projectid

SELECT @AnaI =AnalysisI,@AnaII =AnalysisII,@AnaIII =AnalysisIII FROM #Chk_analysis
if(@AnaI='0' and @AnaII='0' and @AnaIII='0')
BEGIN
SELECT 'NO ANALYSIS QUESTION AVAILABLE'
return -1 
END

DROP TABLE #Chk_analysis

CREATE TABLE #PICK_ANALYSIS(Picked_Analysis varchar(100))
--PRINT @AnaI
--PRINT @AnaII
--PRINT @AnaIII
if @AnaI='1'
insert into  #PICK_ANALYSIS values('ANALYSIS- I')
if @AnaII='1'
insert into  #PICK_ANALYSIS values('ANALYSIS- II')
if @AnaIII='1'
insert into  #PICK_ANALYSIS values('ANALYSIS- III')

--SELECT * FROM #PICK_ANALYSIS

 
	
	 Select Analysis_I,Analysis_II,Analysis_III into #tempx  from Survey_AssignmentDetails
 inner join Survey_AssignQuestionnaire on Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID 
  WHERE Survey_AssignQuestionnaire.AccountID=@accountid AND Survey_AssignQuestionnaire.ProjecctID=@projectid AND Survey_AssignQuestionnaire.ProgrammeID=@programmeid
--select * from #tempx
create table  #tempy(category_detail varchar(100))
INSERT INTO #tempy (category_detail) SELECT distinct Analysis_I FROM #tempx
INSERT INTO #tempy (category_detail) SELECT distinct Analysis_II FROM #tempx
INSERT INTO #tempy (category_detail) SELECT distinct Analysis_III FROM #tempx

SELECT @Chk_FullProjectGrp=FullProjectGrp, @Chk_Programme_Average=Programme_Average FROM Survey_ProjectReportSetting WHERE ProjectID=@projectid --AND AnalysisI='1' AND AnalysisII='1' AND AnalysisIII='1'
IF(@Chk_FullProjectGrp='1')
BEGIN
INSERT INTO #tempy (category_detail)VALUES('Full Project Group')
END


IF(@Chk_Programme_Average='1')
BEGIN
INSERT INTO #tempy (category_detail)VALUES('Programme Average')
END



delete  from #tempy where category_detail=null or category_detail='0'
--select * from #tempy
drop table #tempx

	
	
	DECLARE c2 CURSOR READ_ONLY
    FOR
   
   select category_detail from #tempy
    
    OPEN c2
    FETCH NEXT FROM c2 INTO @cat_detail

WHILE @@FETCH_STATUS = 0
BEGIN
if(@loop=0)
    begin
    set @loop= @loop + 1 


Select q.QuestionID, q.Description,u.FirstName+' '+u.LastName as ParticipantName, CandidateName,
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
	and qa.answer !='N/A' and ad.SubmitFlag = 'True'
	and (ad.Analysis_I = @cat_detail OR ad.Analysis_II = @cat_detail OR ad.Analysis_III = @cat_detail)	
		
		create table #temp6(Analysis_type varchar(100),QuestionID int,Answer  decimal(12,1))          ---------
		
	insert into #temp6 
	select @cat_detail as 'Sub_Category_Type',QuestionID,
	
	
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
	
	
Select  q.QuestionID, q.Description,u.FirstName+' '+u.LastName 
as ParticipantName, CandidateName,pg.ProgrammeName,p.Title,
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
	where a.AccountID = aq.AccountID and 
	q.QuestionTypeID = 2 and qa.Answer != ' ' 
	and aq.accountID = @accountid and aq.ProgrammeID =@programmeid 		
	and qa.answer !='N/A' and ad.SubmitFlag = 'True' 
	and (ad.Analysis_I = @cat_detail OR ad.Analysis_II = @cat_detail OR ad.Analysis_III = @cat_detail)
	
	insert INTO #temp6 
	select @cat_detail as 'Sub_Category_Type',QuestionID,
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


 drop table #tempy

select  #temp6.Analysis_type,
#temp6.Answer,
#temp6.QuestionID,
Survey_Category.CategoryTitle CategoryName,
Survey_Question.[Description],
Survey_AnalysisSheet_Category_Details.Analysis_Type as 'Ana_I_II_III' INTO #TEMP7
FROM 
#temp6 INNER JOIN Survey_Question  ON #temp6.QuestionID=Survey_Question.QuestionID    
       
       INNER JOIN Survey_Category ON Survey_Category.CategoryID=Survey_Question.CateogryID --order by Survey_Category.CategoryName
       INNER JOIN Survey_AnalysisSheet_Category_Details ON Survey_AnalysisSheet_Category_Details.Category_Detail=#temp6.Analysis_type
       WHERE Survey_AnalysisSheet_Category_Details.Programme_Id=@programmeid 
       AND Survey_AnalysisSheet_Category_Details.Analysis_Type IN (SELECT * FROM #PICK_ANALYSIS)
       order by Survey_Category.Sequence,CategoryName,Ana_I_II_III ,Analysis_type
       drop table #temp6
       select @AI=Analysis_I_Name,@AII=Analysis_II_Name,@AIII=Analysis_III_Name from Survey_Analysis_Sheet where ProgrammeID=@programmeid
       -- select Analysis_I_Name,Analysis_II_Name,Analysis_III_Name from Survey_Analysis_Sheet where ProgrammeID=277
       
       
       --  This is to add a new column in the above result(i.e Analysis Name ) to each category)
 create table #TEMP8(Ana_I_II_III varchar(100),Ana_Name varchar(100))
 INSERT INTO #TEMP8 values('ANALYSIS- I',LTRIM(RTRIM(@AI)))
 INSERT INTO #TEMP8 values('ANALYSIS- II',LTRIM(RTRIM(@AII)))
 INSERT INTO #TEMP8 values('ANALYSIS- III',LTRIM(RTRIM(@AIII)))
 
 select #TEMP7.*,#TEMP8.Ana_Name into #TEMP9
  FROM #TEMP7 INNER JOIN #TEMP8 on #TEMP7.Ana_I_II_III = #TEMP8.Ana_I_II_III
       drop table #temp7
       drop table #temp8
       
       select * into #tta from #temp9 order by categoryname,questionID, ana_I_II_III, Analysis_type desc
       delete from #temp9
       
       insert into #temp9
       select * from #tta 
       
        alter table #temp9
	add Analysis_Type_ID int 
       Update #temp9 set Analysis_Type_ID=0 where Analysis_type!='Full Project Group' and Analysis_type!='Programme Average'
       
       --------------------------------   --------------------------------   --------------------------------   --------------------------------
       --------------------------------   --------------------------------   --------------------------------   --------------------------------
       
       -- For fullProjectGroup --
       
  IF(@Chk_FullProjectGrp='1')
BEGIN
		select *  into #temp11 from (
		select c.CategoryID, c.CategoryTitle CategoryName,q.QuestionID,  'Full Project Group' as ParticipantName,pg.ProgrammeName,
		REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer		----------
		from dbo.Survey_AssignQuestionnaire aq
		inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Survey_Question q on q.QuestionID = qa.QuestionID
		inner join Survey_Category c on c.CategoryID = q.CateogryID
		inner join [User] u on u.AccountID = aq.AccountID
		inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Survey_Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid 
		and qa.answer !='N/A' and ad.SubmitFlag = 'True'   
		) t1
		
	
		
		
    	select CategoryName,'Full Project Group' as 'Analysis_type',QuestionID,
		cast(SUBSTRING(cast(sum(Average)/count(Average)  as Varchar(50)),1,3)as decimal(12,1)) AS Answer
		into #temp12 from (
		select CategoryName,'Full Project Group' as 'Analysis_type',QuestionID,
		cast(sum(cast(Answer as decimal(12,1))) / count(Answer) as decimal(12,1))  As Average 
		from
		(
			select * from #temp11
		) as t1
		Group By CategoryName,QuestionID
		) as t2
		Group By CategoryName,QuestionID
		Order By QuestionID
		
		

--SELECT * FROM #TEMP12		
	
	alter table #temp12
	add [Description] varchar(1500),Ana_I_II_III varchar(100),Ana_Name varchar(100)

 
 
----------------------------------------------------
      select @AI=LTRIM(RTRIM(Analysis_I_Name)),@AII=LTRIM(RTRIM(Analysis_II_Name)),@AIII=LTRIM(RTRIM(Analysis_III_Name)) from Survey_Analysis_Sheet where ProgrammeID=@programmeid
      
     --SET  @AI= LTRIM(RTRIM(@AI) 
     --SET  @AII=LTRIM(RTRIM(@AII)
     --SET  @AIII=LTRIM(RTRIM(@AIII)
     
     SELECT @Chk_Ana=COUNT(*) FROM Survey_ProjectReportSetting WHERE ProjectID=@projectid AND AnalysisI='1'
     IF(@Chk_Ana=1)
     BEGIN
       insert into #TEMP9(CategoryName, Analysis_type, QuestionID, Answer, [Description], Ana_I_II_III,Ana_Name)
       select * from  #temp12
     
       Update #temp9 set Ana_I_II_III ='ANALYSIS- I', Ana_Name=@AI WHERE Ana_I_II_III  IS NULL AND Ana_Name IS NULL
       END
    

     
    SELECT @Chk_Ana=COUNT(*) FROM Survey_ProjectReportSetting WHERE ProjectID=@projectid AND AnalysisII='1'
    
     IF(@Chk_Ana=1)
     BEGIN
      insert into #TEMP9(CategoryName, Analysis_type, QuestionID, Answer, [Description], Ana_I_II_III,Ana_Name)
       select * from  #temp12
       
    Update #temp9 set Ana_I_II_III ='ANALYSIS- II', Ana_Name=@AII WHERE Ana_I_II_III  IS NULL AND Ana_Name IS NULL
END


    SELECT @Chk_Ana=COUNT(*) FROM Survey_ProjectReportSetting WHERE ProjectID=@projectid AND AnalysisIII='1'
     IF(@Chk_Ana=1)
     BEGIN
     insert into #TEMP9(CategoryName, Analysis_type, QuestionID, Answer, [Description], Ana_I_II_III,Ana_Name)
       select * from  #temp12
       
    Update #temp9 set Ana_I_II_III ='ANALYSIS- III', Ana_Name=@AIII WHERE Ana_I_II_III  IS NULL AND Ana_Name IS NULL
    END
                 
         SELECT QuestionID,  [Description] INTO #TEMP13 FROM Survey_Question where AccountID=@accountid AND QuestionTypeID=2
         
         
         SELECT Analysis_type,Answer,#temp9.QuestionID,CategoryName,#temp13.[Description],Ana_I_II_III,Ana_Name,Analysis_Type_ID into  #temp15
        FROM #temp9
         INNER JOIN 
         #temp13
        ON #temp9.QuestionID= #temp13.QuestionID --ORDER BY  #temp9.QuestionID,Ana_I_II_III  ,Analysis_type, Answer , Analysis_Type_ID
        
        
        
       
	
	
    
    Update #temp15 set Analysis_Type_ID=2 where Analysis_type='Full Project Group'
    
     --   select * from #temp15 ORDER BY  #temp15.QuestionID,Ana_I_II_III  ,Analysis_type, Answer
        drop table #temp11
		drop table #temp12
        drop table #temp13
        
        delete from #temp9
        
       insert into #temp9
         select *  from #temp15
	--	drop table #temp15
        END
        
      --  ELSE
     --   BEGIN
      -- SELECT * FROM #TEMP15
      --  END
   -----------------------------------------------------------------------------------------------------------------------------------------------
   -----------------------------------------------------------------------------------------------------------------------------------------------
 -- For Programme Average : -
 
   IF(@Chk_Programme_Average=1)
BEGIN

select @AnaI=AnalysisI,@AnaII=AnalysisII,@AnaIII=AnalysisIII from Survey_ProjectReportSetting where ProjectID=@projectid
--select AnalysisI,AnalysisII,AnalysisIII from Survey_ProjectReportSetting where ProjectID=282

IF(@AnaI='1')
 BEGIN
select Answer,QuestionID,CategoryName,[Description] into #tempJ                                                
		From
		(select q.QuestionID,c.CategoryTitle as CategoryName ,q.[Description],
		REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer		
		from dbo.Survey_AssignQuestionnaire aq
		inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Survey_Question q on q.QuestionID = qa.QuestionID
		inner join Survey_Category c on c.CategoryID = q.CateogryID
		left join [User] u on u.AccountID = aq.AccountID
		inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Survey_Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid AND aq.ProgrammeID=@programmeid 
		and ad.Analysis_I  IN(SELECT DISTINCT Analysis_I FROM Survey_AssignmentDetails WHERE 
		 AssignmentID IN(SELECT AssignmentID FROM Survey_AssignQuestionnaire WHERE AccountID=@accountid AND  ProjecctID=@projectid AND ProgrammeID=@programmeid))	
		) as t1
		
			
		select @AI=Analysis_I_Name from Survey_Analysis_Sheet where ProgrammeID=@programmeid
		  
		select 'Programme Average' as 'Analysis_type',questionid as 'QuestionID' ,LTRIM(RTRIM(categoryname)) AS 'CategoryName' ,[Description],'ANALYSIS- I' AS 'Ana_I_II_III',LTRIM(RTRIM(@AI)) as 'Ana_Name',CAST(1 as int) AS 'Analysis_Type_ID',
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1) )as Answer
	    INTO #TEMP16
		 
		 from #tempJ group by Questionid,categoryname,[Description]
		
		drop table #tempJ
		
		  
		  insert into #temp9(Analysis_type,QuestionID,CategoryName ,[Description],Ana_I_II_III,Ana_Name,Analysis_Type_ID,Answer)
		  select *  from #temp16
		  
		  
		  drop table #TEMP16
		
	END
		-----------------------------------------------------------------------------------------------------------------------------------------------

IF(@AnaII='1')
 BEGIN
select Answer,QuestionID,CategoryName,[Description] into #tempK
		From
		(select q.QuestionID, c.CategoryTitle CategoryName,q.[Description],
		REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer		
		from dbo.Survey_AssignQuestionnaire aq
		inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Survey_Question q on q.QuestionID = qa.QuestionID
		inner join Survey_Category c on c.CategoryID = q.CateogryID
		left join [User] u on u.AccountID = aq.AccountID
		inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Survey_Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid AND aq.ProgrammeID=@programmeid 
		and ad.Analysis_II  IN(SELECT DISTINCT Analysis_II FROM Survey_AssignmentDetails WHERE 
		 AssignmentID IN(SELECT AssignmentID FROM Survey_AssignQuestionnaire WHERE AccountID=@accountid AND  ProjecctID=@projectid AND ProgrammeID=@programmeid))	
		) as t1
		
			
		select @AII=Analysis_II_Name from Survey_Analysis_Sheet where ProgrammeID=@programmeid
		  
		select 'Programme Average' as 'Analysis_type',questionid as 'QuestionID' ,LTRIM(RTRIM(categoryname)) AS 'CategoryName' ,[Description],'ANALYSIS- II' AS 'Ana_I_II_III',LTRIM(RTRIM(@AII)) as 'Ana_Name',CAST(1 as int) AS 'Analysis_Type_ID',
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1) )as Answer
	    INTO #TEMP17
	    from #tempK group by Questionid,categoryname,[Description]

		drop table #tempK		
		
		insert into #temp9(Analysis_type,QuestionID,CategoryName ,[Description],Ana_I_II_III,Ana_Name,Analysis_Type_ID,Answer)
		  select *  from #temp17
		  
		  drop table #TEMP17
END		

   
   --------------------------------------------------------------------------------------------------------------------------------------------------
   
IF(@AnaIII='1')
 BEGIN
select Answer,QuestionID,CategoryName,[Description] into #tempL
		From
		(select q.QuestionID,c.CategoryTitle CategoryName,q.[Description],
		REPLACE(SUBSTRING ( Answer ,0 , 2 ),'&','') as Answer		
		from dbo.Survey_AssignQuestionnaire aq
		inner join Survey_AssignmentDetails ad on ad.AssignmentID = aq.AssignmentID
		inner join Survey_QuestionAnswer qa on qa.AssignDetId = ad.AsgnDetailID
		inner join Survey_Question q on q.QuestionID = qa.QuestionID
		inner join Survey_Category c on c.CategoryID = q.CateogryID
		left join [User] u on u.AccountID = aq.AccountID
		inner join Survey_Analysis_Sheet pg on pg.ProgrammeID = aq.ProgrammeID
		inner join Survey_Project p on p.ProjectID = aq.ProjecctID
		where q.QuestionTypeID = 2 and qa.Answer != ' ' and qa.answer !='N/A' and
		aq.accountID = @accountid and aq.ProjecctID = @projectid AND aq.ProgrammeID=@programmeid 
		and ad.Analysis_III  IN(SELECT DISTINCT Analysis_III FROM Survey_AssignmentDetails WHERE 
		 AssignmentID IN(SELECT AssignmentID FROM Survey_AssignQuestionnaire WHERE AccountID=@accountid AND  ProjecctID=@projectid AND ProgrammeID=@programmeid))	
		) as t1
		
			
		select @AIII=Analysis_III_Name from Survey_Analysis_Sheet where ProgrammeID=@programmeid
		  
		select 'Programme Average' as 'Analysis_type',questionid as 'QuestionID' ,LTRIM(RTRIM(categoryname)) AS 'CategoryName' ,[Description],'ANALYSIS- III' AS 'Ana_I_II_III',LTRIM(RTRIM(@AIII)) as 'Ana_Name',CAST(1 as int) AS 'Analysis_Type_ID',
		cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1) )as Answer
	    INTO #TEMP18
		 
		 from #tempL group by Questionid,categoryname,[Description]
		
		drop table #tempL		
		
		insert into #temp9(Analysis_type,QuestionID,CategoryName ,[Description],Ana_I_II_III,Ana_Name,Analysis_Type_ID,Answer)
		  select *  from #temp18
		
		
		drop table #TEMP18
		
		END
	END


     SELECT Analysis_type,Answer,QuestionID, Survey_Category.CategoryTitle CategoryName,
     #TEMP9.[Description],
     Ana_I_II_III,Ana_Name, 
     Analysis_Type_ID 
     INTO #TEMP20 FROM #TEMP9
     inner join Survey_Category on #TEMP9.CategoryName=Survey_Category.CategoryTitle
     where AccountID=@accountid order by Survey_Category.Sequence    
      , categoryname,QuestionID,Ana_I_II_III,Analysis_Type_ID 
    
    
    ----------------------------------------------------------
    -- Here a new column 'SerialNo' is added to the #temp table in order to show the numbering in the report.
    select Questionid,CateogryID,[Description] into #temp21 from Survey_Question where accountid=@accountid and questiontypeid=2 and QuestionnaireID IN(
	select QuestionnaireID from Survey_Project where AccountID=@accountid and ProjectID=@projectid)
		
	SELECT 	Identity(int,1,1) as 'SerialNo',#temp21.Questionid, Survey_Category.CategoryID,#temp21.[Description]
	INTO #temp22
	FROM #temp21
	INNER JOIN 
	Survey_Category
	ON
	Survey_Category.CategoryID=#temp21.CateogryID
	ORDER BY 
	Survey_Category.CategoryID 
	
    /*SELECT #TEMP20.*, #TEMP22.SerialNo FROM  #TEMP20 inner join #TEMP22 
    on #TEMP20.questionID=#TEMP22.questionID */
    
    
    SELECT #TEMP20.*, #TEMP22.SerialNo, Survey_AnalysisSheet_Category_Details.Analysis_Category_Id into #temp77 FROM  #TEMP20 inner join #TEMP22 
    on #TEMP20.questionID=#TEMP22.questionID 
    inner join Survey_AnalysisSheet_Category_Details on Survey_AnalysisSheet_Category_Details.Category_Detail = #TEMP20.Analysis_type
    where Survey_AnalysisSheet_Category_Details.Programme_Id = @programmeid 
    order by Survey_AnalysisSheet_Category_Details.Analysis_Category_Id
    
    insert into #TEMP77
	SELECT #TEMP20.*, #TEMP22.SerialNo, 9999 FROM #TEMP20 inner join #TEMP22 
    on #TEMP20.questionID=#TEMP22.questionID 
    where #temp20.Analysis_type = 'Programme Average'
    
    Declare @AssignmentId int
    
    Select @AssignmentId = AssignmentID from Survey_AssignQuestionnaire where AccountID = @accountid and ProjecctID = @projectid and ProgrammeID = @programmeid
    
     --Declare @ReportManagementID int
    Declare @ShowScoreRespondents Bit
    --SELECT @ReportManagementID = ReportManagementID FROM  Survey_ReportManagement WHERE AccountID = @accountid  and ProgramID = @programmeid and 
    --ProjectID = 	@projectid
    
    Select @ShowScoreRespondents = ShowScoreRespondents from Survey_ProjectReportSetting WHERE AccountID = @accountid and ProjectID = @projectid
    
    
     Declare @QuestionnaireId int
    Select @QuestionnaireId = SAQ.QuestionnaireID from Survey_AssignQuestionnaire  SAQ WHERE SAQ.AccountID = @accountid AND SAQ.ProjecctID = @projectid AND SAQ.ProgrammeID = @programmeid
    
    --Select * from #temp77 order by Analysis_Category_Id
     select * from (Select distinct
      t77.*,SC.Description CategoryDesc, @AssignmentID as AssignmentID,
      @ShowScoreRespondents as ShowScoreRespondents 
       ,SC.Sequence CategorySequence,SQ.Sequence QuestionSequence ,
       CASE WHEN  @path + sc.IntroImgFileName  = @path 
       then '' ELSE @path + sc.IntroImgFileName END IntroImgFileName
      from #temp77 t77 
      LEFT OUTER JOIN Survey_Category SC ON SC.CategoryTitle = t77.CategoryName AND SC.QuestionnaireID = @QuestionnaireId
      LEFT OUTER JOIN Survey_Question SQ ON SQ.QuestionID = t77.QuestionID
  --    right join Survey_Question sq
		--on t77.QuestionID=sq.QuestionID
      --order by Analysis_Category_Id
      union 
      
		select Max(Score1Title) Score1Title,CAST(AVG(CAST(Score1 as decimal(12,1))) as decimal(12,1)) as Answer,
	   spqd.QuestionID,Max(CategoryTitle) CategoryName,
	   Max(sq.Description) Description,
	   (sacd.Analysis_Type) as Ana_I_II_III,
	   Max(sacd.Category_Name) Ana_Name,'' Analysis_Type_ID,
	   '' SerialNo,'' Analysis_Category_Id,
	   Max(sc.Description) CategoryDesc,@AssignmentID AssignmentID,
	   1 ShowScoreRespondents,Max(SC.Sequence) CategorySequence,
	   Max(SQ.Sequence) QuestionSequence ,
	   CASE WHEN  @path + Max(sc.IntroImgFileName)  = @path 
	   then '' ELSE @path + Max(sc.IntroImgFileName) END  IntroImgFileName
		from Survey_PrvScore_QstDetails spqd
		left join Survey_Question sq
		on spqd.QuestionID=sq.QuestionID
		left join Survey_Category sc
		on CateogryID=sc.CategoryID
		left join Survey_PreviousScores  sps
		on sps.PreviousScoreID=spqd.PreviousScoreID
		left join Survey_AnalysisSheet_Category_Details sacd
        on spqd.CategoryID=sacd.Analysis_Category_Id
		where sps.AccountID=@accountid and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
		and( (@AnaI='1' and sacd.Analysis_Type='ANALYSIS- I ')
		or (@AnaII='1' and sacd.Analysis_Type='ANALYSIS- II ')
		or (@AnaIII='1' and sacd.Analysis_Type='ANALYSIS- III '))
		and QuestionTypeID=2
		group by spqd.QuestionID,sacd.Analysis_Type
		
		 union
		  
		select Max(Score2Title) Score2Title,CAST(AVG(CAST(Score2 as decimal(12,1))) as decimal(12,1)) as Answer,
	   spqd.QuestionID,Max(CategoryTitle) CategoryName,
	   Max(sq.Description) Description,
	   (sacd.Analysis_Type) as Ana_I_II_III,
	   Max(sacd.Category_Name) Ana_Name,'' Analysis_Type_ID,
	   '' SerialNo,'' Analysis_Category_Id,
	   Max(sc.Description) CategoryDesc,@AssignmentID AssignmentID,
	   1 ShowScoreRespondents,Max(SC.Sequence) CategorySequence,
	   Max(SQ.Sequence) QuestionSequence,
	   CASE WHEN  @path + Max(sc.IntroImgFileName)  = @path 
	   then '' ELSE @path + Max(sc.IntroImgFileName) END IntroImgFileName 
		from Survey_PrvScore_QstDetails spqd
		left join Survey_Question sq
		on spqd.QuestionID=sq.QuestionID
		left join Survey_Category sc
		on CateogryID=sc.CategoryID
		left join Survey_PreviousScores  sps
		on sps.PreviousScoreID=spqd.PreviousScoreID
		left join Survey_AnalysisSheet_Category_Details sacd
        on spqd.CategoryID=sacd.Analysis_Category_Id
		where 
		sps.AccountID=@accountid and sps.ProjectID=@projectid and sps.ProgrammeID=@programmeid
		and( (@AnaI='1' and sacd.Analysis_Type='ANALYSIS- I ')
		or (@AnaII='1' and sacd.Analysis_Type='ANALYSIS- II ')
		or (@AnaIII='1' and sacd.Analysis_Type='ANALYSIS- III '))
		and QuestionTypeID=2
		group by spqd.QuestionID,sacd.Analysis_Type)tbl1
    order by tbl1.CategorySequence,tbl1.QuestionSequence 
    
   
    
    
    --Select * from Survey_Category WHERE CategoryName='Trust'
    
    --Select * from #TEMP20 inner join #TEMP22 
    --on #TEMP20.questionID=#TEMP22.questionID -- order by Analysis_Category_Id

    --order by #TEMP20.CategoryName,#TEMP20.QuestionID,
	--  #TEMP20.Ana_Name, #TEMP20.Analysis_Type_ID 
    
 --   select * from survey_analysis_sheet
   ------------------------------------------------------
         
    Drop table #TEMP9
    Drop table #TEMP20
    Drop table #TEMP21
    Drop table #TEMP22
    Drop table #temp77
    
--exec [dbo].[Report_QuestionAnswer]  54,6,9
		
		
		--[Report_QuestionAnswer]   106,282,277
GO
