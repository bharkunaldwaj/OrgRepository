USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_FreeTextResponse]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_FreeTextResponse]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_FreeTextResponse]   (@accountid int,
	@projectid int,
	@programmeid int)
	as
    BEGIN
    
     Declare @AssignmentId int
    
    Select @AssignmentId = AssignmentID from Survey_AssignQuestionnaire where AccountID = @accountid and ProjecctID = @projectid and ProgrammeID = @programmeid
    
    Select * INTO #TEMP1 from Survey_QuestionAnswer SQA
    INNER JOIN Survey_AssignmentDetails SAD ON SQA.AssignDetId = SAD.AsgnDetailID 
    where Answer IS NOT NULL AND Answer NOT LIKE '' AND QuestionID IN(select QuestionID from Survey_Question where QuestionTypeID=1 AND  QuestionnaireID IN(SELECT QuestionnaireID FROM Survey_AssignQuestionnaire WHERE AccountID=@accountid and ProjecctID=@projectid and ProgrammeID=@programmeid)) AND SAD.AssignmentID =@AssignmentId
    
    SELECT #TEMP1.QuestionID,Survey_Question.Description,
     CASE WHEN  right(RTRIM(#TEMP1.Answer),1)='.'
     THEN   
      LEFT(#TEMP1.Answer,(DATALENGTH(RTRIM(LTRIM(#TEMP1.Answer)))-1))
     ELSE
      #TEMP1.Answer
     END AS Answer
     --, #TEMP1.CandidateName  
    from #TEMP1 INNER JOIN Survey_Question
    ON #TEMP1.QuestionID=Survey_Question.QuestionID Order by Survey_Question.sequence,#temp1.QuestionID
    
    END


--Select * from Survey_Question where QuestionID = 13
--Select * from Survey_QuestionAnswer Where QuestionID = 256
--[dbo].[Report_FreeTextResponse] 54,6,9
GO
