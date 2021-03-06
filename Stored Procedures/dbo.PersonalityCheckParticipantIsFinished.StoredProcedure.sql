USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityCheckParticipantIsFinished]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityCheckParticipantIsFinished]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Raj>
-- Create date: <07March2012>
-- Description:	<To ge the copyright text from account>
-- =============================================
Create PROCEDURE [dbo].[PersonalityCheckParticipantIsFinished] 
	@QuestionnaireID uniqueidentifier,
	@ParticiapntDetailsID uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;
	
    select PD.IsFinished as IsFinished ,PPA.EndDate as EndDate from PersonalityParticiapntDetails PD
    inner join PersonalityParticipantAssignments PPA on PPA.UniqueID=PD.ParticipantAssignmentID
    where 1=1
    and PD.UniqueID=@ParticiapntDetailsID
    

END
GO
