USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignQuestionnaireDetailSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignQuestionnaireDetailSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAssignQuestionnaireDetailSelect] 

@AssignmentID varchar(500)

as

Begin

SELECT		dbo.Account.OrganisationName, 
			dbo.Programme.StartDate,            
			dbo.Programme.EndDate, 
			dbo.AssignQuestionnaire.AccountID ,
			dbo.AssignQuestionnaire.AssignmentID , 
			dbo.AssignQuestionnaire.ProjecctID ,
			dbo.AssignQuestionnaire.QuestionnaireID ,
			dbo.AssignQuestionnaire.TargetPersonID ,
			dbo.AssignQuestionnaire.Description , 
			dbo.AssignQuestionnaire.CandidateNo ,
			dbo.AssignQuestionnaire.ModifiedBy , 
			dbo.AssignQuestionnaire.ModifiedDate ,
			dbo.AssignQuestionnaire.IsActive ,
			dbo.AssignmentDetails.AsgnDetailID , 
			dbo.AssignmentDetails.AssignmentID ,
			dbo.AssignmentDetails.RelationShip,
			dbo.AssignmentDetails.CandidateName, 
			dbo.AssignmentDetails.CandidateEmail,
			dbo.AssignmentDetails.SubmitFlag
           
FROM        dbo.AssignQuestionnaire INNER JOIN
            dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
            dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID

WHERE		AssignmentDetails.AsgnDetailID in (SELECT Items FROM TblUfSplit(@AssignmentID,',')) 

order by	dbo.AssignmentDetails.AsgnDetailID desc

end
GO
