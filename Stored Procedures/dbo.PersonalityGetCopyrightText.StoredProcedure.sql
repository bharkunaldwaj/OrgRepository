USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityGetCopyrightText]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetCopyrightText]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityGetCopyrightText]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityGetCopyrightText]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
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
' 
END
GO
