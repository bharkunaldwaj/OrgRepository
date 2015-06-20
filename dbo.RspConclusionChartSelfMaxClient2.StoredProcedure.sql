USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspConclusionChartSelfMaxClient2]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspConclusionChartSelfMaxClient2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspConclusionChartSelfMaxClient2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspConclusionChartSelfMaxClient2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the LineChart>
-- =============================================
--[RspConclusionChartSelfMaxClient2] 546, 4
CREATE PROCEDURE [dbo].[RspConclusionChartSelfMaxClient2] 
	@targetpersonid int
	,@N int
AS
BEGIN	

DECLARE @sequence int
DECLARE @catname varchar(50)
DECLARE @sum varchar(50)
DECLARE @NoOfCan varchar(50)
DECLARE @Avg varchar(50)
DECLARE @GrpOrd varchar(50)

	select RelationShip ,c.Sequence ,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),''&'','''') as Answer	
	into #tempsubselfmax from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != '' '' and ad.RelationShip != ''Self''
	and aq.TargetPersonID = @targetpersonid and qa.answer !=''N/A'' 	
	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = ''True''

	DECLARE cat_name1 CURSOR READ_ONLY FOR 	
	-- Query For Maximum Average of Category & CategoryName will Pass To Below Query in SELF Relation
	-- ad.RelationShip != ''Self'' is Required here 
	select Top (@N) t1.Sequence, CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average
	, ''B'' as GrpOrder
	from
	(	
		select * from #tempsubselfmax
	) as t1
	Group By CategoryName,t1.Sequence
	Order by Average desc
BEGIN 
   OPEN cat_name1; 
	IF EXISTS (SELECT 1 FROM sysobjects WHERE xtype=''U'' AND name=''Test2'') 
	DROP TABLE Test2 
	Create table Test2(sequence int,catname varchar(50),summ varchar(50),NoOfCan VARCHAR(50),Avg varchar(50),GrpOrd varchar(50))   
   
   FETCH NEXT FROM cat_name1 INTO @sequence,@catname,@sum,@NoOfCan,@Avg,@GrpOrd ; 
   WHILE @@FETCH_STATUS = 0
   BEGIN		
		INSERT INTO Test2 Values(@sequence,@catname,@sum,@NoOfCan,@Avg,@GrpOrd);
		FETCH NEXT FROM cat_name1 INTO @sequence,@catname,@sum,@NoOfCan,@Avg,@GrpOrd ;
   END

	select RelationShip ,c.Sequence,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),''&'','''') as Answer	
	into #tempsubsubselfmax from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != '' '' and ad.RelationShip = ''Self''
	and c.CategoryName IN (Select catname from Test2)
	and aq.TargetPersonID = @targetpersonid and qa.answer !=''N/A'' 	
	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = ''True''

	select t1.Sequence ,CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average
	, ''A'' as GrpOrder
	into #tempselfmax from
	(	
		select * from #tempsubsubselfmax
	) as t1
	Group By CategoryName,t1.Sequence 
	Order by Average desc;


	select * from #tempselfmax Order by Sequence asc	
	drop table #tempselfmax	
				
	--Select * from Test2
    DROP table Test2
    
    drop table #tempsubselfmax
    drop table #tempsubsubselfmax
   CLOSE cat_name1;    			
   DEALLOCATE cat_name1
END

END
' 
END
GO
