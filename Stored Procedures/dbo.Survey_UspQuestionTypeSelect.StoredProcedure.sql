USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspQuestionTypeSelect]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspQuestionTypeSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspQuestionTypeSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspQuestionTypeSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:  <Tamanash Chowdhary>
-- Create date: <Oct/14/2010>
-- Description: <Question Type>
-- =============================================
create PROCEDURE [dbo].[Survey_UspQuestionTypeSelect]
 @QuestionnaireID int,
 @SelectFlag char(1)
AS
IF (@SelectFlag = ''A'') 

 Begin

 SELECT [QuestionTypeID]
     ,[Name]
     ,[Description]
     ,[IsActive]
     
 FROM [Survey_MSTQuestionType]
 

 End

' 
END
GO
