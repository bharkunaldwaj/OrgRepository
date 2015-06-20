USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantScoreDetailsManagement]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantScoreDetailsManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspParticipantScoreDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantScoreDetailsManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UspParticipantScoreDetailsManagement]

@ScoreID int
,@ScoreType int
,@ScoreMonth int
,@ScoreYear int
,@CategoryID int
,@Score decimal(18,2)
,@Operation char(1)

as

-- Insert

IF (@Operation = ''I'')

Begin

	INSERT INTO [ParticipantScoreDetails]
			   ([ScoreID]
			   ,[ScoreType]
			   ,[ScoreMonth]
			   ,[ScoreYear]
			   ,[CategoryID]
			   ,[Score])
		 VALUES
			   (@ScoreID 
			   ,@ScoreType
			   ,@ScoreMonth
			   ,@ScoreYear
			   ,@CategoryID 
			   ,@Score) 


End
' 
END
GO
