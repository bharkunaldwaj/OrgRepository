USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetParticipantData]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspGetParticipantData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspGetParticipantData]
@ProgramID int = NULL
AS

declare @StartDate datetime
declare @EndDate datetime

set @StartDate=CONVERT(DATETIME, CONVERT(Varchar(50), DATEADD(day, - 4, GETDATE()), 103), 103)
set @EndDate=CONVERT(DATETIME, CONVERT(Varchar(50), DATEADD(day, - 1, GETDATE()), 103), 103)
--print @StartDate
IF (@ProgramID IS NULL)
BEGIN
	SELECT     dbo.PaticipantDetails.UserID,dbo.AssignQuestionnaireParticipant.ProgrammeID 

	FROM         dbo.AssignQuestionnaireParticipant INNER JOIN
						  dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
						  dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
						  dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID

	WHERE (dbo.Programme.EndDate >= @StartDate) AND (dbo.Programme.EndDate <= @EndDate)
END	
	ELSE
	BEGIN
		SELECT  dbo.PaticipantDetails.UserID,dbo.AssignQuestionnaireParticipant.ProgrammeID 

		FROM	dbo.AssignQuestionnaireParticipant INNER JOIN
                dbo.PaticipantDetails ON dbo.AssignQuestionnaireParticipant.AssignmentID = dbo.PaticipantDetails.AssignmentID INNER JOIN
                dbo.Programme ON dbo.AssignQuestionnaireParticipant.ProgrammeID = dbo.Programme.ProgrammeID INNER JOIN
                dbo.[User] ON dbo.PaticipantDetails.UserID = dbo.[User].UserID

		WHERE Programme.ProgrammeID=@ProgramID
	END
--Programme.ProgrammeID IN (591,595)
--WHERE     (dbo.Programme.EndDate = CONVERT(DATETIME, CONVERT(Varchar(50), DATEADD(day, - 1, GETDATE()), 103), 103))
--WHERE   (dbo.Programme.EndDate >= CONVERT(DATETIME, GETDATE(), 102)) AND (dbo.Programme.ReportAvaliableTo <= CONVERT(DATETIME, GETDATE(), 102))
GO
