USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionnaireSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspQuestionnaireSelect]
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
CREATE PROCEDURE [dbo].[Survey_UspQuestionnaireSelect]
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
		FROM [Survey_Questionnaire]
		WHERE QuestionnaireID = @QuestionnaireID AND [AccountID] = @AccountID

		End

		ELSE IF (@SelectFlag = 'A') -- All Records

		Begin

		SELECT	Survey_Questionnaire.QuestionnaireID, 
				Survey_Questionnaire.AccountID, 
				Survey_Questionnaire.QSTNCode, 
				Survey_Questionnaire.QSTNName, 
				Survey_Questionnaire.QSTNDescription, 
				Survey_Questionnaire.DisplayCategory,
				Survey_Questionnaire.ProjectID, 
				Survey_Questionnaire.ManagerID, 
				Survey_Questionnaire.QSTNPrologue, 
				Survey_Questionnaire.QSTNEpilogue, 
				Survey_Questionnaire.ModifyBy, 
				Survey_Questionnaire.ModifyDate, 
				Survey_Questionnaire.IsActive, 
				Survey_MSTQuestionnaireType.[Name],
				[Account].[Code]
		FROM    Survey_Questionnaire INNER JOIN
			Survey_MSTQuestionnaireType ON Survey_Questionnaire.QSTNType = Survey_MSTQuestionnaireType.QSTNID
			INNER JOIN dbo.Account ON Survey_Questionnaire.AccountID = dbo.Account.AccountID
		where Survey_Questionnaire.[AccountID] = @AccountID
		order by dbo.Account.[Code], Survey_Questionnaire.QuestionnaireID desc
		
		END

		ELSE IF (@SelectFlag = 'C') -- Get Record Count

		Begin

		SELECT count(*) FROM [Survey_Questionnaire] where IsActive=1 AND [AccountID] = @AccountID

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
	FROM [Survey_Questionnaire]
	WHERE QuestionnaireID = @QuestionnaireID

	End

	ELSE IF (@SelectFlag = 'A') -- All Records

	Begin
			 

	SELECT	
			
			Survey_Questionnaire.QuestionnaireID, 
			Survey_Questionnaire.AccountID, 
			Survey_Questionnaire.QSTNCode, 
			Survey_Questionnaire.QSTNName, 
			Survey_Questionnaire.QSTNDescription, 
			Survey_Questionnaire.DisplayCategory,
			Survey_Questionnaire.ProjectID, 
			Survey_Questionnaire.ManagerID, 
			Survey_Questionnaire.QSTNPrologue, 
			Survey_Questionnaire.QSTNEpilogue, 
			Survey_Questionnaire.ModifyBy, 
			Survey_Questionnaire.ModifyDate, 
			Survey_Questionnaire.IsActive, 
			Survey_MSTQuestionnaireType.[Name],
			[Account].[Code]
	FROM    Survey_Questionnaire INNER JOIN
		Survey_MSTQuestionnaireType ON Survey_Questionnaire.QSTNType = Survey_MSTQuestionnaireType.QSTNID
		INNER JOIN dbo.Account ON Survey_Questionnaire.AccountID = dbo.Account.AccountID
	where Survey_Questionnaire.[AccountID] = @AccountID
            order by dbo.Account.[Code], Survey_Questionnaire.QuestionnaireID desc

	END
ELSE IF (@SelectFlag = 'C') -- Get Record Count

	Begin

	SELECT count(*) FROM [Survey_Questionnaire] where IsActive=1 AND [AccountID] = @AccountID

	End


END
GO
