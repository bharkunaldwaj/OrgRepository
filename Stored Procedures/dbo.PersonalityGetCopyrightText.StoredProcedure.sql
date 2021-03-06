USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetCopyrightText]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityGetCopyrightText]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Raj>
-- Create date: <07March2012>
-- Description:	<To ge the copyright text from account>
-- =============================================
CREATE PROCEDURE [dbo].[PersonalityGetCopyrightText] 
	@QuestionnaireID uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;

    select ac.CopyRightLine from Account ac where ac.AccountID in(select pq.AccountID from PersonalityQuestionnaires pq where pq.UniqueID=@QuestionnaireID)

END
GO
