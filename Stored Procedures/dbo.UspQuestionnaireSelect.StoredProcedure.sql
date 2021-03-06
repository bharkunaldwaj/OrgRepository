USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspQuestionnaireSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspQuestionnaireSelect]
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
CREATE PROCEDURE [dbo].[UspQuestionnaireSelect]
	@QuestionnaireID int,
	@AccountID int,
	@SelectFlag char(1)
AS

IF (@AccountID != 2 )

	BEGIN

		IF (@SelectFlag = 'I') 

		Begin

		SELECT [QuestionnaireID]
			   ,[AccountID]
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
			   ,[IsActive]
		FROM [Questionnaire]
		WHERE QuestionnaireID = @QuestionnaireID AND [AccountID] = @AccountID

		End

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT	Questionnaire.QuestionnaireID, 
				Questionnaire.AccountID, 
				Questionnaire.QSTNCode, 
				Questionnaire.QSTNName, 
				Questionnaire.QSTNDescription, 
				Questionnaire.DisplayCategory,
				Questionnaire.ProjectID, 
				Questionnaire.ManagerID, 
				Questionnaire.QSTNPrologue, 
				Questionnaire.QSTNEpilogue, 
				Questionnaire.ModifyBy, 
				Questionnaire.ModifyDate, 
				Questionnaire.IsActive, 
				MSTQuestionnaireType.[Name],
				[Account].[Code]
		FROM    Questionnaire INNER JOIN
			MSTQuestionnaireType ON Questionnaire.QSTNType = MSTQuestionnaireType.QSTNID
			INNER JOIN dbo.Account ON Questionnaire.AccountID = dbo.Account.AccountID
		where Questionnaire.[AccountID] = @AccountID
		order by dbo.Account.[Code], Questionnaire.QuestionnaireID desc
		
		END

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [Questionnaire] where IsActive=1 AND [AccountID] = @AccountID

		End
		

	END

ELSE

BEGIN

IF (@SelectFlag = 'I') 

	Begin

	SELECT [QuestionnaireID]
		   ,[AccountID]
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
		   ,[IsActive]
	FROM [Questionnaire]
	WHERE QuestionnaireID = @QuestionnaireID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin

	SELECT	Questionnaire.QuestionnaireID, 
			Questionnaire.AccountID, 
			Questionnaire.QSTNCode, 
			Questionnaire.QSTNName, 
			Questionnaire.QSTNDescription, 
			Questionnaire.DisplayCategory,
			Questionnaire.ProjectID, 
			Questionnaire.ManagerID, 
			Questionnaire.QSTNPrologue, 
			Questionnaire.QSTNEpilogue, 
			Questionnaire.ModifyBy, 
			Questionnaire.ModifyDate, 
			Questionnaire.IsActive, 
			MSTQuestionnaireType.[Name],
			[Account].[Code]
	FROM    Questionnaire INNER JOIN
		MSTQuestionnaireType ON Questionnaire.QSTNType = MSTQuestionnaireType.QSTNID
		INNER JOIN dbo.Account ON Questionnaire.AccountID = dbo.Account.AccountID
	where Questionnaire.[AccountID] = @AccountID
            order by dbo.Account.[Code], Questionnaire.QuestionnaireID desc

	END
ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Questionnaire] where IsActive=1 AND [AccountID] = @AccountID

	End


END
GO
