USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignmentIDSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAssignmentIDSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspAssignmentIDSelect]

@AccountID int
,@ProjecctID int
,@ProgrammeID int
,@QuestionnaireID int 
,@TargetPersonID int

as

SELECT [AssignmentID] 
			FROM [AssignQuestionnaire]
			WHERE 
			[ProjecctID]=@ProjecctID
			and [ProgrammeID]=@ProgrammeID
			and [AccountID]=@AccountID
			and [QuestionnaireID]=@QuestionnaireID
			and [TargetPersonID]=@TargetPersonID
GO
