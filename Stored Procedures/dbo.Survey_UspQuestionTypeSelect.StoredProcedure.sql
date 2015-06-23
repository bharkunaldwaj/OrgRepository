USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionTypeSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspQuestionTypeSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:  <Tamanash Chowdhary>
-- Create date: <Oct/14/2010>
-- Description: <Question Type>
-- =============================================
create PROCEDURE [dbo].[Survey_UspQuestionTypeSelect]
 @QuestionnaireID int,
 @SelectFlag char(1)
AS
IF (@SelectFlag = 'A') 

 Begin

 SELECT [QuestionTypeID]
     ,[Name]
     ,[Description]
     ,[IsActive]
     
 FROM [Survey_MSTQuestionType]
 

 End
GO
