USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetParticipantAllInfo]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetParticipantAllInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspGetParticipantAllInfo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetParticipantAllInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[UspGetParticipantAllInfo]

@ParticipantID int

as

SELECT     dbo.AssignQuestionnaireParticipant.ProjecctID, dbo.AssignQuestionnaireParticipant.ProgrammeID, 
                      dbo.AssignQuestionnaireParticipant.QuestionnaireID, dbo.AssignQuestionnaireParticipant.Description, 
                      dbo.AssignQuestionnaireParticipant.CandidateNo, dbo.AssignQuestionnaireParticipant.AccountID, dbo.Project.Logo AS ProjectLogo, 
                      dbo.Programme.Logo 
FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
                      dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                      dbo.Project ON dbo.AssignQuestionnaireParticipant.ProjecctID = dbo.Project.ProjectID INNER JOIN
                      dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID
WHERE     (dbo.PaticipantDetails.UserID = @ParticipantID)
' 
END
GO
