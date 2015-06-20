USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspParticipantBenchScoreManagement]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantBenchScoreManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspParticipantBenchScoreManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspParticipantBenchScoreManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[UspParticipantBenchScoreManagement]

@ScoreID int
,@BenchmarkName varchar(250)
,@AccountID int
,@ProjectID int
,@ProgrammeID int
,@QuestionnaireID int
,@TargetPersonID int
,@ScoreMonth int
,@ScoreYear int
,@Description varchar(1000)
,@ModifiedBy int
,@ModifiedDate datetime
,@IsActive int
,@Operation char(1)

as

-- Insert

IF (@Operation = ''I'')

Begin

	delete [ParticipantBenchmark]
	WHERE 
	[ProjectID]=@ProjectID
	and [ProgrammeID]=@ProgrammeID
	and [AccountID]=@AccountID
	and [QuestionnaireID]=@QuestionnaireID
	and [TargetPersonID]=@TargetPersonID
	
	INSERT INTO [ParticipantBenchmark]
			   ([BenchmarkName]
			    ,[ProjectID]
				,[ProgrammeID]
				,[AccountID]
				,[QuestionnaireID]
				,[TargetPersonID]
				,[ScoreMonth]
				,[ScoreYear]
				,[Description]
				,[ModifiedBy]
				,[ModifiedDate]
				,[IsActive])
		 VALUES
				(@BenchmarkName
				,@ProjectID
				,@ProgrammeID
				,@AccountID
				,@QuestionnaireID
				,@TargetPersonID
				,@ScoreMonth
				,@ScoreYear
				,@Description
				,@ModifiedBy 
				,@ModifiedDate 
				,@IsActive)

	SELECT ISNULL(MAX([BenchmarkID]),1)
	  FROM [ParticipantBenchmark]
	
	--declare @RecCount int

	--SELECT @RecCount=COUNT(*) 
	--FROM [ParticipantScore]
	--WHERE 
	--[ProjectID]=@ProjectID
	--and [ProgrammeID]=@ProgrammeID
	--and [AccountID]=@AccountID
	--and [QuestionnaireID]=@QuestionnaireID
	--and [TargetPersonID]=@TargetPersonID
	--and [ScoreMonth]=@ScoreMonth
	--and [ScoreYear]=@ScoreYear

	--IF (@RecCount = 0)
	--	BEGIN

	--		INSERT INTO [ParticipantScore]
	--				   ([ProjectID]
	--					,[ProgrammeID]
	--					,[AccountID]
	--					,[QuestionnaireID]
	--					,[TargetPersonID]
	--					,[ScoreMonth]
	--					,[ScoreYear]
	--					,[Description]
	--					,[ModifiedBy]
	--					,[ModifiedDate]
	--					,[IsActive])
	--			 VALUES
	--					(@ProjectID
	--					,@ProgrammeID
	--					,@AccountID
	--					,@QuestionnaireID
	--					,@TargetPersonID
	--					,@ScoreMonth
	--					,@ScoreYear
	--					,@Description
	--					,@ModifiedBy 
	--					,@ModifiedDate 
	--					,@IsActive)

	--		SELECT ISNULL(MAX([ScoreID]),1)
	--		  FROM [ParticipantScore]

	--	End
	--ELSE
	--	BEGIN

	--		SELECT [ScoreID] 
	--		FROM [ParticipantScore]
	--		WHERE 
	--		[ProjectID]=@ProjectID
	--		and [ProgrammeID]=@ProgrammeID
	--		and [AccountID]=@AccountID
	--		and [QuestionnaireID]=@QuestionnaireID
	--		and [TargetPersonID]=@TargetPersonID
	--		and [ScoreMonth]=@ScoreMonth
	--		and [ScoreYear]=@ScoreYear

	--	End

END
' 
END
GO
