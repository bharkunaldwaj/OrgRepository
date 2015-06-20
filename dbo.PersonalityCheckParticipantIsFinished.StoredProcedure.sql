USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityCheckParticipantIsFinished]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityCheckParticipantIsFinished]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityCheckParticipantIsFinished]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityCheckParticipantIsFinished]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
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


' 
END
GO
