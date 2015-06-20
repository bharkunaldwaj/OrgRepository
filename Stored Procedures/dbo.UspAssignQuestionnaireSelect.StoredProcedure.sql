USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignQuestionnaireSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignQuestionnaireSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAssignQuestionnaireSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignQuestionnaireSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspAssignQuestionnaireSelect] 

@AssignmentID int

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

WHERE		AssignmentDetails.AssignmentID=@AssignmentID

order by	dbo.AssignmentDetails.AsgnDetailID desc

end
' 
END
GO
