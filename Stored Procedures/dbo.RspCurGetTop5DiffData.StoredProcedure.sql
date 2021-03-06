USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurGetTop5DiffData]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurGetTop5DiffData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- RspCurGetTop5DiffData 698
CREATE Procedure [dbo].[RspCurGetTop5DiffData]

@TargetPersonID int

as

declare @UpperBound int
		SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 	FROM dbo.AssignQuestionnaire INNER JOIN
				dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
		WHERE   (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)
		
SELECT     QuestionID, Title, Description,AVG(Answer) AS Answer into #TempAllRecords
FROM         (SELECT  dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.Description, dbo.AssignmentDetails.RelationShip, 
                                              CAST(REPLACE(SUBSTRING(dbo.QuestionAnswer.Answer, 0, LEN(dbo.Question.UpperBound) + 1), '&', '') AS decimal(18,2)) AS Answer
                       FROM          dbo.AssignmentDetails INNER JOIN
                                              dbo.QuestionAnswer ON dbo.AssignmentDetails.AsgnDetailID = dbo.QuestionAnswer.AssignDetId INNER JOIN
                                              dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID RIGHT OUTER JOIN
                                              dbo.Question ON dbo.QuestionAnswer.QuestionID = dbo.Question.QuestionID
                       WHERE      (dbo.Question.QuestionTypeID = 2) AND (dbo.AssignmentDetails.RelationShip <> 'Self') AND (dbo.AssignmentDetails.SubmitFlag = 1) AND 
                                              (dbo.QuestionAnswer.Answer <> 'N/A') AND (dbo.QuestionAnswer.Answer <> ' ') AND (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)) 
                      AS table1
GROUP BY QuestionID, Title, Description

select tbl1.QuestionID,tbl1.Diff into #TemptblQuestonID from 
(
select t1.*,#TempAllRecords.Answer as OtherAnswer, (t1.Answer - #TempAllRecords.Answer) as Diff from (
SELECT  dbo.AssignmentDetails.RelationShip, dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.Description, 
      CAST(REPLACE(SUBSTRING(dbo.QuestionAnswer.Answer, 0, LEN(dbo.Question.UpperBound) + 1), '&', '') AS decimal(18,2)) AS Answer
FROM          dbo.AssignmentDetails INNER JOIN
      dbo.QuestionAnswer ON dbo.AssignmentDetails.AsgnDetailID = dbo.QuestionAnswer.AssignDetId INNER JOIN
      dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID RIGHT OUTER JOIN
      dbo.Question ON dbo.QuestionAnswer.QuestionID = dbo.Question.QuestionID
WHERE      (dbo.Question.QuestionTypeID = 2) AND (dbo.AssignmentDetails.RelationShip = 'Self') AND (dbo.AssignmentDetails.SubmitFlag = 1) AND 
      (dbo.QuestionAnswer.Answer <> 'N/A') AND (dbo.QuestionAnswer.Answer <> ' ') AND (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)
) t1

inner join #TempAllRecords on t1.QuestionID=#TempAllRecords.QuestionID ) tbl1
order by Diff asc

--select * from #TemptblQuestonID

-- ############### RETURN MAIN OUTPUT TABLE #######################

select 'All Others' as Relationship, @UpperBound as UpperBound, * from #TempAllRecords where questionid in (select top 5 questionid from #TemptblQuestonID)

union

SELECT  dbo.AssignmentDetails.RelationShip, @UpperBound as UpperBound, dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.Description, 
                      CAST(REPLACE(SUBSTRING(dbo.QuestionAnswer.Answer, 0, LEN(dbo.Question.UpperBound) + 1), '&', '') AS decimal(18,2)) AS Answer
FROM          dbo.AssignmentDetails INNER JOIN
                      dbo.QuestionAnswer ON dbo.AssignmentDetails.AsgnDetailID = dbo.QuestionAnswer.AssignDetId INNER JOIN
                      dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID RIGHT OUTER JOIN
                      dbo.Question ON dbo.QuestionAnswer.QuestionID = dbo.Question.QuestionID
WHERE      (dbo.Question.QuestionTypeID = 2) AND (dbo.AssignmentDetails.RelationShip = 'Self') AND (dbo.AssignmentDetails.SubmitFlag = 1) AND 
                      (dbo.QuestionAnswer.Answer <> 'N/A') AND (dbo.QuestionAnswer.Answer <> ' ') AND (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)
                      and dbo.Question.QuestionID in (select top 5 questionid from #TemptblQuestonID)

drop table #TempAllRecords
drop table #TemptblQuestonID
GO
