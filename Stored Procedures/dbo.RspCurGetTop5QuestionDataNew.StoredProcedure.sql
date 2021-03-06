USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[RspCurGetTop5QuestionDataNew]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[RspCurGetTop5QuestionDataNew]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- RspCurGetTop5QuestionData 968, 1
-- RspCurGetTop5QuestionData 613, 2
Create procedure [dbo].[RspCurGetTop5QuestionDataNew]

@TargetPersonID int,
@SelectFlag int

as

declare @UpperBound int
		SELECT  @UpperBound= MAX(dbo.Question.UpperBound) 	FROM dbo.AssignQuestionnaire INNER JOIN
				dbo.Question ON dbo.AssignQuestionnaire.QuestionnaireID = dbo.Question.QuestionnaireID
		WHERE   (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)

--######################################### TOP 5 QUESTION DATA  ###################################
if (@SelectFlag = 1) 

begin

SELECT     QuestionID, Title, Description,AVG(Answer) AS Answer into #TempAllRecords1
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
Order by Answer desc

select top 5 'All Others' as Relationship, @UpperBound as UpperBound,* from #tempallrecords1 

union

SELECT  dbo.AssignmentDetails.RelationShip, @UpperBound as UpperBound, dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.Description, 
          CAST(REPLACE(SUBSTRING(dbo.QuestionAnswer.Answer, 0, LEN(dbo.Question.UpperBound) + 1), '&', '') AS decimal(18,2)) AS Answer
FROM          dbo.AssignmentDetails INNER JOIN
          dbo.QuestionAnswer ON dbo.AssignmentDetails.AsgnDetailID = dbo.QuestionAnswer.AssignDetId INNER JOIN
          dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID RIGHT OUTER JOIN
          dbo.Question ON dbo.QuestionAnswer.QuestionID = dbo.Question.QuestionID
WHERE      (dbo.Question.QuestionTypeID = 2) AND (dbo.AssignmentDetails.RelationShip = 'Self') AND (dbo.AssignmentDetails.SubmitFlag = 1) AND 
          (dbo.QuestionAnswer.Answer <> 'N/A') AND (dbo.QuestionAnswer.Answer <> ' ') AND (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)
          and dbo.Question.QuestionID in (select top 5.questionid from #tempallrecords1 order by answer desc)

drop table #TempAllRecords1     

end

--######################################### BELOW 5 QUESTION DATA  ###################################
else if (@SelectFlag = 2) 

begin

SELECT     QuestionID, Title, Description,AVG(Answer) AS Answer into #TempAllRecords2
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

select top 5 'All Others' as Relationship, @UpperBound as UpperBound,* from #tempallrecords2 

union

SELECT  dbo.AssignmentDetails.RelationShip, @UpperBound as UpperBound, dbo.Question.QuestionID, dbo.Question.Title, dbo.Question.Description, 
          CAST(REPLACE(SUBSTRING(dbo.QuestionAnswer.Answer, 0, LEN(dbo.Question.UpperBound) + 1), '&', '') AS decimal(18,2)) AS Answer
FROM          dbo.AssignmentDetails INNER JOIN
          dbo.QuestionAnswer ON dbo.AssignmentDetails.AsgnDetailID = dbo.QuestionAnswer.AssignDetId INNER JOIN
          dbo.AssignQuestionnaire ON dbo.AssignmentDetails.AssignmentID = dbo.AssignQuestionnaire.AssignmentID RIGHT OUTER JOIN
          dbo.Question ON dbo.QuestionAnswer.QuestionID = dbo.Question.QuestionID
WHERE      (dbo.Question.QuestionTypeID = 2) AND (dbo.AssignmentDetails.RelationShip = 'Self') AND (dbo.AssignmentDetails.SubmitFlag = 1) AND 
          (dbo.QuestionAnswer.Answer <> 'N/A') AND (dbo.QuestionAnswer.Answer <> ' ') AND (dbo.AssignQuestionnaire.TargetPersonID = @TargetPersonID)
          and dbo.Question.QuestionID in (select top 5.questionid from #tempallrecords2 order by answer)

drop table #TempAllRecords2     

end
GO
