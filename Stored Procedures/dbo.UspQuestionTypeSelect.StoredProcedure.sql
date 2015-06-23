USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspQuestionTypeSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspQuestionTypeSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Tamanash Chowdhary>
-- Create date: <Oct/14/2010>
-- Description:	<Question Type>
-- =============================================
CREATE PROCEDURE [dbo].[UspQuestionTypeSelect]
	@QuestionnaireID int,
	@SelectFlag char(1)
AS
IF (@SelectFlag = 'A') 

	Begin

	SELECT [QuestionTypeID]
		   ,[Name]
		   ,[Description]
		   ,[IsActive]
		   
	FROM [MSTQuestionType]
	

	End
GO
