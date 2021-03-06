USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspSubHorizontalBarChartMinMax]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[RspSubHorizontalBarChartMinMax]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Create Date,,>
-- Description:	<This Procedure will be used in Sub Report to Display the Horizontal BarGraph
-- =============================================
CREATE PROCEDURE [dbo].[RspSubHorizontalBarChartMinMax] 	
	@categoryid int,
	@targetpersonid int,    --   *PLease Pass this in like this : aq.TargetPersonID = @targetpersonid *
	@grp Varchar(500)
AS
BEGIN	
			select * into #temp1 from dbo.fn_CSVToTable(@grp)	
	
			SELECT     Id as GrpOrder ,RelationShip, CategoryName, MIN(cast(Answer as decimal(12,1))) AS Minimum, MAX(cast(Answer as decimal(12,1))) AS Maximum
			FROM         (SELECT      #temp1.Id,ad.RelationShip, c.CategoryName, REPLACE(SUBSTRING(qa.Answer, 0, len(q.UpperBound)+1), '&', '') AS Answer, q.UpperBound
                       FROM          dbo.Account AS a LEFT OUTER JOIN
                                              dbo.Category AS c ON c.AccountID = a.AccountID LEFT OUTER JOIN
                                              dbo.Question AS q ON q.CateogryID = c.CategoryID LEFT OUTER JOIN
                                              dbo.QuestionAnswer AS qa ON qa.QuestionID = q.QuestionID LEFT OUTER JOIN
                                              dbo.AssignmentDetails AS ad ON ad.AsgnDetailID = qa.AssignDetId LEFT OUTER JOIN
                                              dbo.AssignQuestionnaire AS aq ON aq.AssignmentID = ad.AssignmentID AND a.AccountID = aq.AccountID LEFT OUTER JOIN
                                              dbo.[User] AS u ON u.UserID = aq.TargetPersonID
                                              inner join #temp1 on #temp1.[Value]=ad.RelationShip
                       WHERE ad.SubmitFlag = 'True' and (q.QuestionTypeID = 2) AND (ad.RelationShip <> 'Self') AND (aq.TargetPersonID = @targetpersonid) AND (c.CategoryID = @categoryid) AND (qa.Answer <> ' ') AND 
                                              (qa.Answer <> 'N/A')) AS t1
			GROUP BY Id,RelationShip, CategoryName

END
GO
