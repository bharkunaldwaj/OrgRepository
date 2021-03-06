USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurGetBelow5QuestionData]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurGetBelow5QuestionData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- RspCurGetBelow5QuestionData 1569, 1
-- RspCurGetTop5QuestionData 613, 2
CREATE procedure [dbo].[RspCurGetBelow5QuestionData]

@TargetPersonID int

as

declare @UpperBound int
		SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 	FROM dbo.AssignQuestionnaire INNER JOIN
				dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
		WHERE   (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)


--######################################### BELOW 5 QUESTION DATA  ###################################
SELECT     QuestionID, Title, Description,AVG(Answer) AS Answer into #TempAllRecords3
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
Order by Answer 

select top 5 ROW_NUMBER() 
        OVER (ORDER BY answer) AS Row, 'All Others' as Relationship, @UpperBound as UpperBound,* into #tempallrecords4 from #tempallrecords3 

select * from #tempallrecords4

union

SELECT  '', dbo.AssignmentDetails.RelationShip, @UpperBound as UpperBound, dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.Description, 
          CAST(REPLACE(SUBSTRING(dbo.QuestionAnswer.Answer, 0, LEN(dbo.Question.UpperBound) + 1), '&', '') AS decimal(18,2)) AS Answer
FROM          dbo.AssignmentDetails INNER JOIN
          dbo.QuestionAnswer ON dbo.AssignmentDetails.AsgnDetailID = dbo.QuestionAnswer.AssignDetId INNER JOIN
          dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID RIGHT OUTER JOIN
          dbo.Question ON dbo.QuestionAnswer.QuestionID = dbo.Question.QuestionID
WHERE      (dbo.Question.QuestionTypeID = 2) AND (dbo.AssignmentDetails.RelationShip = 'Self') AND (dbo.AssignmentDetails.SubmitFlag = 1) AND 
          (dbo.QuestionAnswer.Answer <> 'N/A') AND (dbo.QuestionAnswer.Answer <> ' ') AND (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)
          and dbo.Question.QuestionID in (select top 5.questionid from #tempallrecords4 order by answer)

drop table #TempAllRecords3
drop table #TempAllRecords4
GO
