USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionnaireManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspQuestionnaireManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
IF (@Operation = 'I')

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
Else IF (@Operation = 'U')

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
Else IF (@Operation = 'D')

	Begin

	DELETE from [Survey_Questionnaire] where QuestionnaireID=@QuestionnaireID

	End
GO
