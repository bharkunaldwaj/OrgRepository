USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAssignmentIDSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignmentIDSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAssignmentIDSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAssignmentIDSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

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


' 
END
GO
