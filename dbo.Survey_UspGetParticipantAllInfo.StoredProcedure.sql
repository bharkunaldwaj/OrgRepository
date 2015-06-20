USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetParticipantAllInfo]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetParticipantAllInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspGetParticipantAllInfo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetParticipantAllInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[Survey_UspGetParticipantAllInfo]

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
' 
END
GO
