USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAssignPartQuestionnaireSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAssignPartQuestionnaireSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_UspAssignPartQuestionnaireSelect]

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
GO
