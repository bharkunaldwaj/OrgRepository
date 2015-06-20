USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetColleaguesList]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetColleaguesList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspGetColleaguesList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetColleaguesList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[UspGetColleaguesList]

@AssignmentID int
,@SelectFlag char(1)

as

if (@SelectFlag = ''A'')

Begin

SELECT		AsgnDetailID, 
			RelationShip, 
			CandidateName, 
			CandidateEmail, 
			SubmitFlag, 
			EmailSendFlag
			
FROM        dbo.AssignmentDetails

WHERE		AssignmentID = @AssignmentID --and AssignmentDetails.EmailSendFlag = 0

End

else if (@SelectFlag = ''B'')

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
			dbo.AssignmentDetails.SubmitFlag,
			dbo.AssignmentDetails.EmailSendFlag
           
FROM        dbo.AssignQuestionnaire INNER JOIN
            dbo.AssignmentDetails ON dbo.AssignQuestionnaire.AssignmentID = dbo.AssignmentDetails.AssignmentID INNER JOIN
            dbo.Account ON dbo.AssignQuestionnaire.AccountID = dbo.Account.AccountID INNER JOIN
            dbo.Programme ON dbo.AssignQuestionnaire.ProgrammeID = dbo.Programme.ProgrammeID

WHERE		AssignmentDetails.AssignmentID=@AssignmentID and AssignmentDetails.EmailSendFlag = 0

order by	dbo.AssignmentDetails.AsgnDetailID desc

End
' 
END
GO
