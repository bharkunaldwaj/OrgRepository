USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAssignPartQuestionnaireSelect]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAssignPartQuestionnaireSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspAssignPartQuestionnaireSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAssignPartQuestionnaireSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[Survey_UspAssignPartQuestionnaireSelect]

@AssignmentID int

as
Begin

SELECT                dbo.Survey_PaticipantDetails.PaticipantID, 
                      dbo.Survey_PaticipantDetails.AssignmentID , 
                      dbo.Survey_PaticipantDetails.ProjectID, 
                      dbo.Survey_PaticipantDetails.UserID, 
                      dbo.[User].UserID , 
                      dbo.[User].LoginID, 
                      dbo.[User].Password, 
                      dbo.[User].GroupID, 
                      dbo.[User].AccountID , 
                      dbo.[User].StatusID, 
                      dbo.[User].Salutation, 
                      dbo.[User].FirstName, 
                      dbo.[User].LastName, 
                      dbo.[User].EmailID, 
                      dbo.[User].Notification, 
                      dbo.[User].ModifyBy, 
                      dbo.[User].ModifyDate,
                      dbo.Survey_Project.Title,
                      dbo.Account.Code

FROM        dbo.Survey_PaticipantDetails INNER JOIN
                      dbo.[User] ON dbo.Survey_PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Survey_Project ON dbo.Survey_PaticipantDetails.ProjectID = dbo.Survey_Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID

WHERE	Survey_PaticipantDetails.AssignmentID = @AssignmentID

order by dbo.[User].UserID desc

end
' 
END
GO
