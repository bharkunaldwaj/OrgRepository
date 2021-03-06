USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetParticipantAllInfo]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGetParticipantAllInfo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspGetParticipantAllInfo]

@ParticipantID int

as

SELECT     dbo.Survey_AssignQuestionnaireParticipant.ProjecctID, dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID, 
                      dbo.Survey_AssignQuestionnaireParticipant.QuestionnaireID, dbo.Survey_AssignQuestionnaireParticipant.Description, 
                      dbo.Survey_AssignQuestionnaireParticipant.CandidateNo, dbo.Survey_AssignQuestionnaireParticipant.AccountID, dbo.Survey_Project.Logo AS ProjectLogo, 
                      dbo.Survey_Analysis_Sheet.Logo 
FROM         dbo.Survey_AssignQuestionnaireParticipant INNER JOIN
                      dbo.Survey_PaticipantDetails ON dbo.Survey_AssignQuestionnaireParticipant.AssignmentID = dbo.Survey_PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_AssignQuestionnaireParticipant.ProjecctID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaireParticipant.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID
WHERE     (dbo.Survey_PaticipantDetails.UserID = @ParticipantID)
GO
