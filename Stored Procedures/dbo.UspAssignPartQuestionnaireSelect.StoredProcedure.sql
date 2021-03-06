USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignPartQuestionnaireSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignPartQuestionnaireSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAssignPartQuestionnaireSelect]

@AssignmentID int

as
Begin

SELECT                dbo.PaticipantDetails.PaticipantID, 
                      dbo.PaticipantDetails.AssignmentID , 
                      dbo.PaticipantDetails.ProjectID, 
                      dbo.PaticipantDetails.UserID, 
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
                      dbo.Project.Title,
                      dbo.Account.Code

FROM        dbo.PaticipantDetails INNER JOIN
                      dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID INNER JOIN
                      dbo.Project ON dbo.PaticipantDetails.ProjectID = dbo.Project.ProjectID INNER JOIN
                      dbo.Account ON dbo.[User].AccountID = dbo.Account.AccountID

WHERE	PaticipantDetails.AssignmentID = @AssignmentID

order by dbo.[User].UserID desc

end
GO
