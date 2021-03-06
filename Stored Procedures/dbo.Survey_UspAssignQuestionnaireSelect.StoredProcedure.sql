USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAssignQuestionnaireSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAssignQuestionnaireSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_UspAssignQuestionnaireSelect] 

@AssignmentID int

as

Begin

SELECT	DISTINCT	--dbo.Account.OrganisationName, 
SC.Title OrganisationName,
			sas.StartDate,            
			sas.EndDate, 
			saq.AccountID ,
			saq.AssignmentID , 
			saq.ProjecctID ,
			saq.QuestionnaireID ,
			
			saq.Description , 
			saq.CandidateNo ,
			saq.ModifiedBy , 
			saq.ModifiedDate ,
			saq.IsActive ,
			sad.AsgnDetailID , 
			sad.AssignmentID ,
			
			sad.CandidateName, 
			sad.CandidateEmail,
			sad.SubmitFlag
           
FROM        



 dbo.Account  AC INNER JOIN  dbo.Survey_Project SP ON AC.AccountID = SP.AccountID
 INNER JOIN  dbo.Survey_Company SC ON Sp.ProjectID = SC.ProjectID
 INNER JOIN  dbo.Survey_Analysis_Sheet SAS On SAS.CompanyID = SC.CompanyID
 INNER JOIN  dbo.Survey_Questionnaire  SQ ON SQ.AccountID = AC.AccountID
 INNER JOIN dbo.Survey_AssignQuestionnaire  SAQ ON SQ.QuestionnaireID = SQ.QuestionnaireID and sas.ProgrammeID = saq.ProgrammeID
 INNER JOIN dbo.Survey_AssignmentDetails SAD ON SAD.AssignmentID = SAQ.AssignmentID
         
WHERE		sad.AssignmentID=@AssignmentID

order by	sad.AsgnDetailID desc

end
GO
