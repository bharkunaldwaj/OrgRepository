USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantBenchScoreDetailsManagement]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantBenchScoreDetailsManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspParticipantBenchScoreDetailsManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantBenchScoreDetailsManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[UspParticipantBenchScoreDetailsManagement]

@BenchmarkID int
,@CategoryID int
,@Score decimal(18,2)
,@Operation char(1)

as

-- Insert

IF (@Operation = ''I'')

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
' 
END
GO
