USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantBenchScoreDetailsManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspParticipantBenchScoreDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[UspParticipantBenchScoreDetailsManagement]

@BenchmarkID int
,@CategoryID int
,@Score decimal(18,2)
,@Operation char(1)

as

-- Insert

IF (@Operation = 'I')

Begin

	INSERT INTO [ParticipantBenchmarkDetails]
			   ([BenchmarkID]
			   ,[CategoryID]
			   ,[Score])
		 VALUES
			   (@BenchmarkID 
			   ,@CategoryID 
			   ,@Score) 


End
GO
