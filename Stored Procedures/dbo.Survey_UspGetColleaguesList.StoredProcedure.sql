USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspGetColleaguesList]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetColleaguesList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspGetColleaguesList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspGetColleaguesList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[Survey_UspGetColleaguesList]

@AssignmentID int
,@SelectFlag char(1)

as

if (@SelectFlag = ''A'')

Begin

SELECT		AsgnDetailID, 
			Analysis_I, 
			Analysis_II,
			Analysis_III,
			CandidateName, 
			CandidateEmail, 
			SubmitFlag, 
			EmailSendFlag
			
FROM        dbo.Survey_AssignmentDetails

WHERE		AssignmentID = @AssignmentID --and AssignmentDetails.EmailSendFlag = 0

End

else if (@SelectFlag = ''B'')

Begin

SELECT		dbo.Account.OrganisationName, 
			dbo.Survey_Analysis_Sheet.StartDate,            
			dbo.Survey_Analysis_Sheet.EndDate, 
			dbo.Survey_AssignQuestionnaire.AccountID ,
			dbo.Survey_AssignQuestionnaire.AssignmentID , 
			dbo.Survey_AssignQuestionnaire.ProjecctID ,
			dbo.Survey_AssignQuestionnaire.QuestionnaireID ,
			
			dbo.Survey_AssignQuestionnaire.Description , 
			dbo.Survey_AssignQuestionnaire.CandidateNo ,
			dbo.Survey_AssignQuestionnaire.ModifiedBy , 
			dbo.Survey_AssignQuestionnaire.ModifiedDate ,
			dbo.Survey_AssignQuestionnaire.IsActive ,
			dbo.Survey_AssignmentDetails.AsgnDetailID , 
			dbo.Survey_AssignmentDetails.AssignmentID ,
			
			dbo.Survey_AssignmentDetails.CandidateName, 
			dbo.Survey_AssignmentDetails.CandidateEmail,
			dbo.Survey_AssignmentDetails.SubmitFlag,
			dbo.Survey_AssignmentDetails.EmailSendFlag,
			
			dbo.Survey_AssignmentDetails.Analysis_I,
			dbo.Survey_AssignmentDetails.Analysis_II,
			dbo.Survey_AssignmentDetails.Analysis_III
           
FROM        dbo.Survey_AssignQuestionnaire INNER JOIN
            dbo.Survey_AssignmentDetails ON dbo.Survey_AssignQuestionnaire.AssignmentID = dbo.Survey_AssignmentDetails.AssignmentID INNER JOIN
            dbo.Account ON dbo.Survey_AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Survey_Analysis_Sheet ON dbo.Survey_AssignQuestionnaire.ProgrammeID = dbo.Survey_Analysis_Sheet.ProgrammeID

WHERE		Survey_AssignmentDetails.AssignmentID=@AssignmentID and Survey_AssignmentDetails.EmailSendFlag = 0

order by	dbo.Survey_AssignmentDetails.AsgnDetailID desc

End
' 
END
GO
