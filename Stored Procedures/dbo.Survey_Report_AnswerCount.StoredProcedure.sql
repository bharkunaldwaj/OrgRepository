USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Report_AnswerCount]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_Report_AnswerCount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Survey_Report_AnswerCount]  --  83, 5
	@QuestionId int,
	@AssignmentId int
	as
	
	  BEGIN
	  
	    
	   CREATE TABLE #AnswerCount
		(
		  Sequence int
		);
	   
	   DECLARE @Range int
	   
	   SELECT  @Range = R.Range_upto  from Question_Range R INNER JOIN  Survey_Question Q ON Q.Range_Name = R.Range_Name WHERE Q.QuestionID  = @QuestionId
	   
	   DECLARE @Counter int
	   SET @Counter = 1
	   
	   --SET @Range = 10
	   
	   WHILE @Counter <=  @Range
	   BEGIN
	     INSERT INTO #AnswerCount (Sequence) values (@Counter)
		SET @Counter = @Counter + 1;
	   END
	   
	   --SELECT * FROM #AnswerCount
	   
	   --SELECT * FROM Survey_QuestionAnswer
	   
	   
	   /*SELECT ANC.Sequence,Count(A.Answer) answer FROM #AnswerCount ANC 
	   LEFT OUTER JOIN Survey_QuestionAnswer A  ON ANC.Sequence = CONVERT(int, A.Answer)	   
	   AND  A.QuestionID = @QuestionId
       GROUP BY ANC.Sequence  ,A.Answer
	   ORDER BY ANC.Sequence*/
	   
	   SELECT ANC.Sequence,Count(SQA.Answer) answer FROM Survey_Question SQ INNER JOIN Survey_QuestionAnswer	   SQA ON SQ.QuestionID = SQA.QuestionID --AND SQ.QuestionnaireID = @AssignmentId
	   INNER JOIN Survey_AssignmentDetails SAD ON SAD.AsgnDetailID = SQA.AssignDetId AND SAD.AssignmentID = @AssignmentId
	   RIGHT OUTER JOIN #AnswerCount ANC ON ANC.Sequence = Convert(int, SQA.Answer)
	   AND SQA.QuestionID = @QuestionId
	   --where QuestionTypeID=2
	   GROUP BY ANC.Sequence  ,SQA.Answer
	   ORDER BY ANC.Sequence
	   
	   DROP Table #AnswerCount
	   
	  END
	  
	  
	  
--            [dbo].[Survey_Report_AnswerCount]    1
GO
