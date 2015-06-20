USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspConclusionChartMaxClient2]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspConclusionChartMaxClient2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RspConclusionChartMaxClient2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RspConclusionChartMaxClient2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Main Report to Display the LineChart>
-- =============================================
--[RspConclusionChartMaxClient2] 546, 3
CREATE PROCEDURE [dbo].[RspConclusionChartMaxClient2] 
	@targetpersonid int     -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *		
	,@N int	
AS
BEGIN
--declare @N int
--set @N = 2 
	declare @UpperBound int
	SELECT   @UpperBound= MAX(dbo.Question.UpperBound) 
	FROM         dbo.AssignQuestionnaire INNER JOIN
                 dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
	WHERE     (dbo.AssignQuestionnaire.TargetPersonID = @targetpersonid)
	-- apart from self	
	select RelationShip ,c.Sequence,c.CategoryName,  REPLACE(SUBSTRING ( Answer ,0 , len(q.UpperBound)+1 ),''&'','''') as Answer	
	into #tempsubmax from Account a
	left join Category c on c.AccountID = a.AccountID
	left join Question q on q.CateogryId = c.CategoryId
	left join QuestionAnswer qa on qa.QuestionId = q.QuestionId
	left join AssignmentDetails ad on ad.AsgnDetailID = qa.AssignDetId
	left join AssignQuestionnaire aq on aq.AssignmentID = ad.AssignmentID
	left join [User] u on u.UserID = aq.TargetPersonID
	where a.AccountID = aq.AccountID and q.QuestionTypeID = 2 and qa.Answer != '' '' and ad.RelationShip != ''Self''
	and aq.TargetPersonID = @targetpersonid and qa.answer !=''N/A'' 	
	and c.ExcludeFromAnalysis = 0 and c.QuestionnaireID = aq.QuestionnaireID and ad.SubmitFlag = ''True''
	
	select Top (@N) t1.Sequence, CategoryName, sum(cast(t1.Answer as decimal(12,1))) as "Sum", count(*) as "No Of Candidate",
	cast(sum(cast(Answer as decimal(12,1))) / count(*) as decimal(12,1))  As Average
	, ''B'' as GrpOrder,@UpperBound as UpperBound
	into #tempmax from
	(
		select * from #tempsubmax
	) as t1
	Group By CategoryName,t1.Sequence
	Order by Average desc		

	
	select * from #tempmax Order by Sequence asc	
	drop table #tempmax
	drop table #tempsubmax
END
' 
END
GO
