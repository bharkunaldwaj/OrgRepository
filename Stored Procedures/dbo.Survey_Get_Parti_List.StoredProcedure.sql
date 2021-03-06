USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_Get_Parti_List]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_Get_Parti_List]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Survey_Get_Parti_List](@accountID INT, @ProgrammeID INT)
as
begin
SELECT distinct Survey_AssignmentDetails.CandidateName, Survey_AssignmentDetails.CandidateEmail, Survey_AssignmentDetails.AsgnDetailID, 
Survey_AssignQuestionnaire.QuestionnaireID,
Survey_AssignQuestionnaire.AccountID, dbo.Survey_Analysis_Sheet.ProgrammeName, 
dbo.Survey_AssignQuestionnaire.QuestionnaireID, 
dbo.Survey_AssignmentDetails.SubmitFlag,  dbo.Survey_Project.StatusID,dbo.Survey_Analysis_Sheet.ProgrammeID 
FROM  
dbo.Survey_AssignQuestionnaire  
INNER JOIN dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID =  dbo.Survey_AssignmentDetails.AssignmentID  
INNER JOIN dbo.Survey_Project ON dbo.Survey_AssignQuestionnaire.ProjecctID= dbo.Survey_Project.ProjectID  
INNER JOIN dbo.Survey_Analysis_Sheet ON dbo.Survey_Project.ProjectID = dbo.Survey_Analysis_Sheet.ProjectID  
AND Survey_Analysis_Sheet.AccountID= Survey_AssignQuestionnaire.AccountID AND Survey_Analysis_Sheet.ProgrammeID = Survey_AssignQuestionnaire.ProgrammeID 
WHERE Survey_AssignQuestionnaire.AccountID=@accountID AND Survey_AssignQuestionnaire.ProgrammeID=@ProgrammeID
end
GO
