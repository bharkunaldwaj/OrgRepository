USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspQuestionnaireIDSelect]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspQuestionnaireIDSelect]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
