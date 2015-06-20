USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspQuestionnaireIDSelect]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspQuestionnaireIDSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspQuestionnaireIDSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspQuestionnaireIDSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Rudra Prakash Mishra>
-- Create date: <Jun/04/2014>
-- Description:	<Get questionnaire ID based on project ID>
-- =============================================
CREATE PROCEDURE [dbo].[UspQuestionnaireIDSelect]
	@ProjectID int
AS

SELECT QuestionnaireID FROM dbo.Project 
WHERE ProjectID=@ProjectID


' 
END
GO
