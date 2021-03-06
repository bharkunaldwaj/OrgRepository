USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantScoreDetailsManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspParticipantScoreDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[UspParticipantScoreDetailsManagement]

@ScoreID int
,@ScoreType int
,@ScoreMonth int
,@ScoreYear int
,@CategoryID int
,@Score decimal(18,2)
,@Operation char(1)

as

-- Insert

IF (@Operation = 'I')

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
GO
