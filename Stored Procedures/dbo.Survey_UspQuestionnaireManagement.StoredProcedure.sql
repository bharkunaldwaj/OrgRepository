USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionnaireManagement]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspQuestionnaireManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspQuestionnaireManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspQuestionnaireManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Ashish Gupta>
-- Create date: <Oct/06/2010>
-- Description:	<QuestionnaireManagement>
-- =============================================
CREATE PROCEDURE [dbo].[Survey_UspQuestionnaireManagement]
	@QuestionnaireID int,
	@AccountID int,
	@QSTNType varchar(1000),
	@QSTNCode varchar(5),
	@QSTNName varchar(50),
	@QSTNDescription varchar(1000),
	@DisplayCategory int,
	@ProjectID int,
	@ManagerID int,
	@QSTNPrologue varchar(8000),
	@QSTNEpilogue varchar(8000),
	@ModifiedBy int,
	@ModifiedDate datetime,
	@IsActive int,
	@Operation char(1)
as

--Insert
IF (@Operation = ''I'')

	Begin

	INSERT INTO [Survey_Questionnaire]
			   ([AccountID]
			   ,[QSTNType]
			   ,[QSTNCode]
			   ,[QSTNName]
			   ,[QSTNDescription]
			   ,[DisplayCategory]
			   ,[ProjectID]
			   ,[ManagerID]			   
			   ,[QSTNPrologue]
			   ,[QSTNEpilogue]
			   ,[ModifyBy]			   
			   ,[ModifyDate]
			   ,[IsActive])
		 VALUES
			   (@AccountID,
				@QSTNType,
				@QSTNCode,
				@QSTNName,
				@QSTNDescription,
				@DisplayCategory,
				@ProjectID,
				@ManagerID,
				@QSTNPrologue,
				@QSTNEpilogue,
				@ModifiedBy,
				@ModifiedDate,
				@IsActive)

	End
	
--Update
Else IF (@Operation = ''U'')

	Begin

	UPDATE [Survey_Questionnaire]
	SET 
		[AccountID] = @AccountID
		,[QSTNType] = @QSTNType
		,[QSTNCode] = @QSTNCode
		,[QSTNName] = @QSTNName
		,[QSTNDescription] = @QSTNDescription
		,[DisplayCategory]=@DisplayCategory
		,[ProjectID] = @ProjectID
		,[ManagerID] = @ManagerID			   
		,[QSTNPrologue] = @QSTNPrologue
		,[QSTNEpilogue] = @QSTNEpilogue
		,[ModifyBy] = @ModifiedBy			   
		,[ModifyDate] = @ModifiedDate
		,[IsActive] = @IsActive
		

	WHERE QuestionnaireID=@QuestionnaireID

	End

--Delete
Else IF (@Operation = ''D'')

	Begin

	DELETE from [Survey_Questionnaire] where QuestionnaireID=@QuestionnaireID

	End
' 
END
GO
