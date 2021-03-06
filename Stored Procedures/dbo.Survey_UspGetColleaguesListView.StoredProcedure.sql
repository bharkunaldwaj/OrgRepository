USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetColleaguesListView]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspGetColleaguesListView]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspGetColleaguesListView]

@projectID int,
@progID int

as

Begin

SELECT		[Survey_AssignmentDetails].AsgnDetailID, 
			[Survey_AssignmentDetails].Analysis_I, 
			[Survey_AssignmentDetails].Analysis_II,
			[Survey_AssignmentDetails].Analysis_III,
			[Survey_AssignmentDetails].CandidateName, 
			[Survey_AssignmentDetails].CandidateEmail, 
			[Survey_AssignmentDetails].SubmitFlag, 
			[Survey_AssignmentDetails].EmailSendFlag
			
 FROM 
[Survey_AssignmentDetails] 
INNER JOIN survey_assignquestionnaire ON
Survey_AssignmentDetails.AssignmentID=survey_assignquestionnaire.AssignmentID
INNER JOIN survey_Project ON survey_assignquestionnaire.ProjecctID=survey_Project.ProjectID 
INNER JOIN Survey_Analysis_Sheet on survey_assignquestionnaire.ProgrammeID = Survey_Analysis_Sheet.ProgrammeID 
WHERE Survey_Project.ProjectID=@projectID and Survey_Analysis_Sheet.ProgrammeID = @progID

End
GO
