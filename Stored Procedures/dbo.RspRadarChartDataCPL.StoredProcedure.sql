USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspRadarChartDataCPL]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspRadarChartDataCPL]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Description:	<This Procedure will be used in Main Report to Display the Categories>
--For the radar graph, as sample attached, we compare the average of the
--scores given for the participant with the average of all scores given for
--this project.
--So for Unilever the sample Al Wilson, Al Wilson is the average of the 10
--people (his Candidates) compared to the Full Project Group, 600 people that
-- =============================================
-- [RspRadarChartData] 639, 'Line Manager,Peer,Customer,Direct Report,Other', 'S'
--[RspRadarChartData] 470, 'Line Manager,Peer Group, Customer', 'S'
--[RspRadarChartData] 470, 'Line Manager,Peer Group, Customer', 'F'
-- exec [RspRadarChartDataCPL] 1640, 'Line Manager,Peer,', 'S'
-- exec [RspRadarChartDataCPL] 1640, 'Line Manager,Peer,', 'F'
CREATE PROCEDURE [dbo].[RspRadarChartDataCPL] 
	@targetpersonid int,      -- *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500),
	@Operation char(1)
AS
BEGIN	
	IF (@Operation = 'S')
	BEGIN
		Declare @usrame varchar(500)
		set @usrame = null
		select @usrame = FirstName +' '+ LastName from [User] where UserID = @targetpersonid	
	
		SELECT @usrame as Relationship,c.CategoryName, c.Sequence,
		REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
		into #tempSelf FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') 
		--AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
		and c.ExcludeFromAnalysis = 0 and (ad.RelationShip = 'Self')
		ORDER BY c.Sequence	
		
		select RelationShip,t1.Sequence, CategoryName, sum(cast(Case When t1.Answer = 'N/' or ltrim(rtrim(t1.Answer)) = '' then convert(varchar,'0') else t1.answer end as int)) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Case When t1.Answer = 'N/' or ltrim(rtrim(t1.Answer)) = '' then convert(varchar,'0') else t1.answer end as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'A' as GrpOrder, UpperBound
		from
		(
			select * from #tempself
		) as t1
		Group By RelationShip, CategoryName, t1.Sequence, UpperBound
		Order By t1.Sequence
		
		Drop Table #tempSelf
	END

	IF (@Operation = 'F')
	BEGIN
		-- this is not full project, this is sum of scores of the colleagues of the participant
		
		SELECT 'Colleagues' as Relationship,c.CategoryName, c.Sequence,
		REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
		into #tempNotSelf FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
		and c.ExcludeFromAnalysis = 0 and (ad.RelationShip <> 'Self')
		ORDER BY c.Sequence	
		
		select RelationShip,t2.Sequence, CategoryName, sum(cast(t2.Answer as int)) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'A' as GrpOrder, UpperBound
		from
		(
			select * from #tempNotSelf
		) as t2
		Group By RelationShip, CategoryName, t2.Sequence, UpperBound
		Order By t2.Sequence
		
		Drop Table #tempNotSelf
	END
END	

/*		
		--Full Project Group
		SELECT c.CategoryID		
		into #tempfull FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (TargetPersonID = @targetpersonid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ') 
		and c.ExcludeFromAnalysis = 0		
		ORDER BY c.Sequence		
		
		select CategoryID into #TempCategoryID	
		from
		(
			select * from #tempfull
		) as t1
		Group By CategoryID
		
		Declare @prjid int
		set @prjid = null
		select @prjid = ProjecctID from AssignQuestionnaire where TargetPersonID = @targetpersonid
	
		SELECT 'Full Project Group' as Relationship,c.CategoryName, c.Sequence,
        REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, UpperBound
		into #tempnewfull FROM QuestionAnswer qa
		left join AssignmentDetails ad ON qa.AssignDetId = ad.AsgnDetailID 
		left join Question q ON qa.QuestionID = q.QuestionID 
		left join Category c ON q.CateogryID = c.CategoryID
		WHERE (ad.AssignmentID IN (SELECT AssignmentID FROM dbo.AssignQuestionnaire WHERE (ProjecctID = @prjid))) 
		AND (q.QuestionTypeID = 2) AND (ad.SubmitFlag = 'True') AND (qa.Answer <> 'N/A' and qa.Answer != ' ')
		and c.ExcludeFromAnalysis = 0 and c.CategoryID in (select CategoryID from #TempCategoryID)
		--and ad.RelationShip NOT IN (@grp)
		ORDER BY c.Sequence
		
		select t1.RelationShip,t1.Sequence,t1.CategoryName, sum(cast(t1.Answer as decimal(12,2))) as "Sum", count(*) as "No Of Candidate",
		cast(sum(cast(t1.Answer as decimal(12,2))) / count(*) as decimal(12,2))  As Average , 'B' as GrpOrder, UpperBound
		from
		(	
			select * from #tempnewfull
		) as t1	
		group by t1.RelationShip,t1.CategoryName,t1.Sequence, UpperBound
		
		Drop Table #tempnewfull
		Drop Table #TempCategoryID
		Drop Table #tempfull
	END
	
END
*/
GO
