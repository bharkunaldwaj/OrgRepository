USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProjectInfoSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProjectInfoSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspProjectInfoSelect]

@QuestionnaireID int,
@CandidateID int


as

SELECT  DISTINCT   

sq.QSTNCode, sq.QSTNName, sq.QSTNDescription,sq.DisplayCategory, dbo.Survey_Project.ProjectID, dbo.Survey_Project.Reference, dbo.Survey_Project.Title, 
dbo.Survey_Project.Description, sc.Finish_EmailID,sad.CandidateEmail as Email,sad.CandidateName as FullName, CASE WHEN CHARINDEX(' ', sad.CandidateName)= 0 THEN sad.CandidateName ELSE SUBSTRING(sad.CandidateName,1,CHARINDEX(' ', sad.CandidateName)) END as FirstName, 
 CASE WHEN CHARINDEX(' ', sad.CandidateName)= 0 THEN '' ELSE SUBSTRING(sad.CandidateName,CHARINDEX(' ', sad.CandidateName),LEN(sad.CandidateName)) END as LastName
FROM         
--dbo.Survey_AssignQuestionnaire INNER JOIN 
--dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID = dbo.Survey_Project.ProjectID  INNER JOIN
--dbo.Survey_Company ON dbo.Survey_Company.ProjectID = dbo.Survey_Project.ProjectID INNER JOIN
--dbo.Survey_Questionnaire ON dbo.Survey_AssignQuestionnaire.QuestionnaireID = dbo.Survey_Questionnaire.QuestionnaireID INNER JOIN
--dbo.[User] ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.[User].AccountID INNER JOIN
--dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID

dbo.Account INNER JOIN
dbo.Survey_Project  ON dbo.Survey_Project.AccountID = dbo.Account.AccountID INNER JOIN
dbo.Survey_Company SC ON SC.ProjectID = dbo.Survey_Project.ProjectID  INNER JOIN
Survey_Analysis_Sheet sp on sp.CompanyID = SC.CompanyId   inner join
dbo.Survey_Questionnaire SQ on  sq.AccountID=dbo.Account.AccountID INNER JOIN 
dbo.Survey_AssignQuestionnaire saq ON sq.QuestionnaireID = saq.QuestionnaireID and sp.ProgrammeID = saq.ProgrammeID
INNER JOIN Survey_AssignmentDetails sad ON sad.AssignmentID = saq.AssignmentID

WHERE     (saq.QuestionnaireID = @QuestionnaireID) AND (sad.AsgnDetailID = @CandidateID)
GO
